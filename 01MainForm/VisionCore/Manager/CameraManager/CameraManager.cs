// VisionCore/Manager/CameraManager/CameraManager.cs
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;
using HardwareCameraNet;
using VisionCore.PluginBase;

namespace VisionCore.Manager.CameraManager;

public sealed class CameraManager
{
    // 单例实例（线程安全）
    private static readonly Lazy<CameraManager> _instance = new(() => new CameraManager(),
        LazyThreadSafetyMode.ExecutionAndPublication);
    public static CameraManager Instance => _instance.Value;

    // 插件目录（相对程序根目录）
    private readonly string _pluginDirectory;

    // 配置文件路径
    private readonly string _configFilePath;

    // 品牌-插件映射（品牌名 -> 插件信息）
    private readonly Dictionary<string, PluginInfo> _manufacturerPluginMap = new(StringComparer.OrdinalIgnoreCase);

    // 已加载的插件类型（用于创建实例）
    private readonly Dictionary<string, Type> _pluginTypes = new(StringComparer.OrdinalIgnoreCase);

    // 已添加的相机配置（用户配置）
    private readonly ConcurrentDictionary<string, CameraConfig> _cameraConfigs = new(StringComparer.OrdinalIgnoreCase);

    // 相机实例缓存（序列号 -> 相机实例）
    private readonly ConcurrentDictionary<string, ICamera> _cameraInstances = new(StringComparer.OrdinalIgnoreCase);

    private CameraManager()
    {
        // 初始化路径
        _pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", "Camera");
        _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs", "CameraConfigs.xml");

        // 确保目录存在
        if (!Directory.Exists(_pluginDirectory))
            Directory.CreateDirectory(_pluginDirectory);
        if (!Directory.Exists(Path.GetDirectoryName(_configFilePath)!))
            Directory.CreateDirectory(Path.GetDirectoryName(_configFilePath)!);

        // 初始化：加载插件 -> 加载配置
        LoadPlugins();
        LoadCameraConfigs();
    }

    #region 插件管理

    /// <summary>
    /// 加载所有相机插件
    /// </summary>
    private void LoadPlugins()
    {
        _manufacturerPluginMap.Clear();
        _pluginTypes.Clear();

        var dllFiles = Directory.GetFiles(_pluginDirectory, "*.dll", SearchOption.TopDirectoryOnly);
        foreach (var dllPath in dllFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                foreach (var type in assembly.GetTypes())
                {
                    // 筛选实现ICamera的非抽象类
                    if (typeof(ICamera).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
                    {
                        // 通过反射获取厂商名称（从实例属性获取）
                        var instance = (ICamera)Activator.CreateInstance(type, string.Empty); // 临时实例用于获取属性
                        var manufacturer = instance.Manufacturer;
                        if (string.IsNullOrEmpty(manufacturer))
                            continue;

                        // 存储插件信息
                        var pluginInfo = new PluginInfo
                        {
                            TypeName = type.FullName!,
                            AssemblyName = assembly.GetName().Name!
                        };

                        _manufacturerPluginMap[manufacturer] = pluginInfo;
                        _pluginTypes[type.FullName!] = type;

                        Console.WriteLine($"加载相机插件：{manufacturer}（{type.FullName}）");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载插件失败（{dllPath}）：{ex.Message}");
            }
        }
    }

    /// <summary>
    /// 获取所有支持的相机品牌
    /// </summary>
    public List<string> GetAllManufacturers()
    {
        return _manufacturerPluginMap.Keys.OrderBy(k => k).ToList();
    }

    #endregion

    #region 配置管理

    /// <summary>
    /// 加载本地相机配置
    /// </summary>
    private void LoadCameraConfigs()
    {
        if (!File.Exists(_configFilePath))
            return;

        try
        {
            var serializer = new XmlSerializer(typeof(List<CameraConfig>));
            using var stream = File.OpenRead(_configFilePath);
            var configs = (List<CameraConfig>)serializer.Deserialize(stream)!;

            foreach (var config in configs)
            {
                _cameraConfigs[config.SerialNumber] = config;
            }
            Console.WriteLine($"加载相机配置：共{_cameraConfigs.Count}台");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"加载配置失败：{ex.Message}");
        }
    }

    /// <summary>
    /// 保存相机配置到本地
    /// </summary>
    public void SaveCameraConfigs()
    {
        try
        {
            var serializer = new XmlSerializer(typeof(List<CameraConfig>));
            using var stream = File.Create(_configFilePath);
            serializer.Serialize(stream, _cameraConfigs.Values.ToList());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"保存配置失败：{ex.Message}");
        }
    }

    /// <summary>
    /// 添加/更新相机配置
    /// </summary>
    public void AddOrUpdateCameraConfig(CameraConfig config)
    {
        if (string.IsNullOrEmpty(config.SerialNumber) || string.IsNullOrEmpty(config.Manufacturer))
            throw new ArgumentException("序列号和品牌不能为空");

        if (!_manufacturerPluginMap.ContainsKey(config.Manufacturer))
            throw new ArgumentException($"不支持的相机品牌：{config.Manufacturer}");

        _cameraConfigs[config.SerialNumber] = config;
        SaveCameraConfigs();
    }

    /// <summary>
    /// 删除相机配置
    /// </summary>
    public bool RemoveCameraConfig(string serialNumber)
    {
        if (_cameraConfigs.TryRemove(serialNumber, out _))
        {
            // 同时移除实例
            _cameraInstances.TryRemove(serialNumber, out _);
            SaveCameraConfigs();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取所有相机配置
    /// </summary>
    public List<CameraConfig> GetAllCameraConfigs()
    {
        return _cameraConfigs.Values.ToList();
    }

    #endregion

    #region 相机实例管理

    /// <summary>
    /// 枚举指定品牌的设备（返回序列号列表）
    /// </summary>
    public List<string> EnumerateDevices(string manufacturer)
    {
        if (!_manufacturerPluginMap.TryGetValue(manufacturer, out var pluginInfo))
            return new List<string>();

        // 调用插件的静态枚举方法
        var type = _pluginTypes[pluginInfo.TypeName];
        var method = type.GetMethod("EnumerateDevices", BindingFlags.Public | BindingFlags.Static);
        if (method == null)
            return new List<string>();

        return (List<string>)method.Invoke(null, null)! ?? new List<string>();
    }

    /// <summary>
    /// 获取相机实例（不存在则创建）
    /// </summary>
    public ICamera GetCamera(string serialNumber)
    {
        // 从缓存获取
        if (_cameraInstances.TryGetValue(serialNumber, out var camera))
            return camera;

        // 检查配置
        if (!_cameraConfigs.TryGetValue(serialNumber, out var config))
            throw new KeyNotFoundException($"未找到序列号为{serialNumber}的相机配置");

        // 创建实例
        camera = CreateCameraInstance(config);
        if (camera != null)
        {
            _cameraInstances[serialNumber] = camera;
        }
        return camera;
    }

    /// <summary>
    /// 创建相机实例
    /// </summary>
    private ICamera CreateCameraInstance(CameraConfig config)
    {
        if (!_manufacturerPluginMap.TryGetValue(config.Manufacturer, out var pluginInfo))
            return null;

        var type = _pluginTypes[pluginInfo.TypeName];
        try
        {
            // 调用带序列号的构造函数
            var camera = (ICamera)Activator.CreateInstance(type, config.SerialNumber)!;
            return camera;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"创建{config.Manufacturer}相机实例失败：{ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// 获取所有相机实例
    /// </summary>
    public List<ICamera> GetAllCameras()
    {
        return _cameraInstances.Values.ToList();
    }

    #endregion
}