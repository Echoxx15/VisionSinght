using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using VisionCore.PluginBase;

namespace HardwareCameraNet;

/// <summary>
/// 相机管理器
/// </summary>
public sealed class CameraManager
{
    // 单例实例（线程安全）
    private static readonly Lazy<CameraManager> _instance = new(() => new CameraManager(), LazyThreadSafetyMode.ExecutionAndPublication);

    // 2. 线程安全锁（保护配置字典操作）
    private readonly object _lockObj = new();
    // 3. 依赖组件
    private readonly PluginLoader<ICamera> _cameraPluginLoader; // 相机插件加载器
    private readonly string _configFilePath;                   // 相机配置文件路径（本地）
    /// <summary>
    /// 已添加的相机列表
    /// </summary>
    private readonly ConcurrentDictionary<string,ICamera> gCameraList = new();
    /// <summary>
    /// 管理已连接的相机列表
    /// </summary>
    private readonly ConcurrentDictionary<string, ICamera> temCameraList = new();
    /// <summary>
    /// 已添加的相机配置
    /// </summary>
    private Dictionary<string, CameraConfig> _userCameraConfigs; // 用户添加的相机配置（键：序列号）

    // 存储：厂商 -> (枚举方法委托, 插件类型)
    private readonly Dictionary<string, (Func<List<string>> EnumerateFunc, Type PluginType)> ManufacturerMap;

    /// <summary>
    /// 私有构造函数（禁止外部实例化）
    /// </summary>
    private CameraManager()
    {
        // 初始化插件加载器（相机插件目录：Plugins/Camera）
        _cameraPluginLoader = new PluginLoader<ICamera>("Plugins/Camera");

        // 初始化配置文件路径（程序目录/Configs/CameraConfigs.xml）
        _configFilePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Configs",
            "CameraConfigs.xml");

        // 初始化状态字典
        ManufacturerMap = new(StringComparer.OrdinalIgnoreCase);
        _userCameraConfigs = new(StringComparer.OrdinalIgnoreCase);

        // 启动初始化（加载插件+加载本地配置）
        Initialize();
    }

    /// <summary>
    /// 全局唯一实例
    /// </summary>
    public static CameraManager Instance => _instance.Value;

    /// <summary>
    /// 初始化：加载插件、构建厂商映射、加载本地配置
    /// </summary>
    private void Initialize()
    {
        // 步骤1：加载相机插件
        _cameraPluginLoader.LoadPluginTypes();
        var loadedCameraTypes = _cameraPluginLoader.GetLoadedPluginTypes();

        // 步骤2：构建厂商映射（从插件类型提取厂商特性和枚举方法）
        BuildManufacturerMap(loadedCameraTypes.Values);

        // 步骤3：加载本地相机配置（程序启动时恢复用户配置）
        LoadUserConfigsFromLocal();
    }

    /// <summary>
    /// 构建厂商映射：从插件类型中提取厂商信息和枚举方法
    /// </summary>
    private void BuildManufacturerMap(IEnumerable<Type> cameraTypes)
    {
        ManufacturerMap.Clear();

        foreach (Type type in cameraTypes)
        {
            // 提取厂商特性（标记插件所属厂商）
            var manufacturerAttr = type.GetCustomAttribute<CameraManufacturerAttribute>();
            if (manufacturerAttr == null)
            {
                Console.WriteLine($"插件{type.FullName}未标记CameraManufacturerAttribute，跳过");
                continue;
            }

            string manufacturerName = manufacturerAttr.ManufacturerName;
            if (ManufacturerMap.ContainsKey(manufacturerName))
            {
                Console.WriteLine($"厂商{manufacturerName}已存在，跳过重复插件{type.FullName}");
                continue;
            }

            // 查找静态枚举方法（无参数，返回List<string>序列号列表）
            MethodInfo enumerateMethod = type.GetMethod(
                name: "EnumerateDevices",
                bindingAttr: BindingFlags.Public | BindingFlags.Static,
                binder: null,  // 补充binder参数（可设为null）
                types: Type.EmptyTypes,
                modifiers: null  // 补充modifiers参数（可设为null）
            );

            if (enumerateMethod == null || enumerateMethod.ReturnType != typeof(List<string>))
            {
                Console.WriteLine($"插件{type.FullName}缺少有效的EnumerateDevices静态方法，跳过");
                continue;
            }

            // 封装枚举委托（统一调用方式）
            List<string> EnumerateFunc()
            {
                try
                {
                    return (List<string>)enumerateMethod.Invoke(null, null) ?? [];
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"枚举{manufacturerName}设备失败：{ex.InnerException?.Message}");
                    return [];
                }
            }

            // 添加到厂商映射
            ManufacturerMap.Add(manufacturerName, (EnumerateFunc, type));
            Console.WriteLine($"注册厂商：{manufacturerName}（插件类型：{type.FullName}）");
        }
    }

    /// <summary>
    /// 从本地加载用户相机配置
    /// </summary>
    private void LoadUserConfigsFromLocal()
    {
        lock (_lockObj)
        {
            var configCollection = ConfigHelper.LoadConfig<ConfigHelper.CameraConfigCollection>(_configFilePath);
            _userCameraConfigs = configCollection.ToDictionary();
            Console.WriteLine($"加载本地配置：共{_userCameraConfigs.Count}台相机");
        }
    }

    /// <summary>
    /// 保存用户相机配置到本地（实时同步）
    /// </summary>
    private void SaveUserConfigsToLocal()
    {
        lock (_lockObj)
        {
            var configCollection = ConfigHelper.CameraConfigCollection.FromDictionary(_userCameraConfigs);
            ConfigHelper.SaveConfig(configCollection, _configFilePath);
        }
    }

    // ------------------------------ 对外API ------------------------------
    /// <summary>
    /// 获取所有支持的厂商名称
    /// </summary>
    public List<string> GetAllManufacturers()
    {
        lock (_lockObj)
        {
            return ManufacturerMap.Keys.OrderBy(k => k).ToList();
        }
    }

    /// <summary>
    /// 枚举指定厂商的设备（返回序列号列表）
    /// </summary>
    public List<string> EnumerateDevices(string manufacturerName)
    {
        if (string.IsNullOrEmpty(manufacturerName))
        {
            Console.WriteLine("厂商名称不能为空");
            return [];
        }

        lock (_lockObj)
        {
            if (ManufacturerMap.TryGetValue(manufacturerName, out var item))
            {
                var devices = item.EnumerateFunc.Invoke();
                Console.WriteLine($"枚举{manufacturerName}设备：{devices.Count}台");
                return devices;
            }

            Console.WriteLine($"未支持的厂商：{manufacturerName}");
            return [];
        }
    }

    /// <summary>
    /// 根据厂商和序列号创建相机实例
    /// </summary>
    public ICamera CreateCamera(string manufacturerName, string serialNumber)
    {
        if (string.IsNullOrEmpty(manufacturerName) || string.IsNullOrEmpty(serialNumber))
        {
            Console.WriteLine("厂商名称和序列号不能为空");
            return null;
        }

        // 先检查缓存：存在则直接返回（避免重复创建）
        if (temCameraList.TryGetValue(serialNumber, out var cachedCamera))
        {
            Console.WriteLine($"复用缓存的相机实例：{serialNumber}");
            return cachedCamera;
        }

        lock (_lockObj)
        {
            // 双重检查锁定（DCL）：防止多线程下同时创建同一实例
            if (temCameraList.TryGetValue(serialNumber, out cachedCamera))
            {
                return cachedCamera;
            }

            // 1. 检查厂商是否支持
            if (!ManufacturerMap.TryGetValue(manufacturerName, out var item))
            {
                Console.WriteLine($"未支持的厂商：{manufacturerName}");
                return null;
            }

            // 2. 检查设备是否已枚举（可选：避免创建不存在的设备）
            var enumeratedDevices = item.EnumerateFunc.Invoke();
            if (!enumeratedDevices.Contains(serialNumber, StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine($"厂商{manufacturerName}下无序列号{serialNumber}的设备");
                return null;
            }
            if(temCameraList.TryGetValue(serialNumber, out var cam))
            {
                return cam;
            }

            // 3. 创建实例（用插件类型完全限定名）
            var pluginTypeName = item.PluginType.FullName;
            ICamera newCamera = _cameraPluginLoader.CreatePluginInstance(pluginTypeName, serialNumber);
            if (newCamera != null)
            {
                // 添加到线程安全缓存
                temCameraList.TryAdd(serialNumber, newCamera);
                Console.WriteLine($"创建新相机实例并缓存：{serialNumber}");
            }
            return newCamera;
        }
    }

    /// <summary>
    /// 添加/更新相机配置（自动同步本地）
    /// </summary>
    public bool AddOrUpdateCameraConfig(CameraConfig config)
    {
        if (config == null || string.IsNullOrEmpty(config.SerialNumber))
        {
            Console.WriteLine("配置无效（序列号不能为空）");
            return false;
        }

        lock (_lockObj)
        {
            if (!_userCameraConfigs.ContainsKey(config.SerialNumber))
            {
                _userCameraConfigs.Add(config.SerialNumber, config);
                // 实时保存到本地
                SaveUserConfigsToLocal();
                Console.WriteLine($"添加相机配置：{config.SerialNumber}");
                return true;
            }

            Console.WriteLine($"序列号{config.SerialNumber}已添加");
            return false;
        }
    }

    /// <summary>
    /// 通过厂商名称和序列号添加/更新相机配置（自动关联插件信息）
    /// </summary>
    public bool AddOrUpdateCameraConfig(string manufacturerName, string serialNumber)
    {
        if (string.IsNullOrEmpty(manufacturerName) || string.IsNullOrEmpty(serialNumber))
        {
            Console.WriteLine("厂商名称和序列号不能为空");
            return false;
        }

        lock (_lockObj)
        {
            // 检查是否已存在配置
            if (_userCameraConfigs.ContainsKey(serialNumber))
            {
                Console.WriteLine($"序列号{serialNumber}已添加");
                return false;
            }

            // 从厂商映射中获取插件类型信息
            if (!ManufacturerMap.TryGetValue(manufacturerName, out var manufacturerInfo))
            {
                Console.WriteLine($"未找到厂商{manufacturerName}的插件信息");
                return false;
            }

            // 自动构造CameraConfig（插件类型全名和程序集名从插件类型中获取）
            var pluginType = manufacturerInfo.PluginType;
            var config = new CameraConfig(
                serialNumber,
                pluginType.FullName,       // 插件类型全名（命名空间+类名）
                pluginType.Assembly.GetName().Name  // 插件程序集名称
            );

            // 添加配置并保存
            _userCameraConfigs.Add(serialNumber, config);
            SaveUserConfigsToLocal();
            Console.WriteLine($"添加相机配置：{serialNumber}（厂商：{manufacturerName}）");
            return true;
        }
    }

    /// <summary>
    /// 删除相机配置（自动同步本地）
    /// </summary>
    public bool RemoveCameraConfig(string serialNumber)
    {
        if (string.IsNullOrEmpty(serialNumber))
        {
            Console.WriteLine("序列号不能为空");
            return false;
        }

        lock (_lockObj)
        {
            if (_userCameraConfigs.Remove(serialNumber))
            {
                Console.WriteLine($"删除相机配置：{serialNumber}");
                SaveUserConfigsToLocal(); // 实时保存
                return true;
            }

            Console.WriteLine($"未找到序列号{serialNumber}的配置");
            return false;
        }
    }

    /// <summary>
    /// 获取所有用户相机配置（返回副本，避免外部修改）
    /// </summary>
    public Dictionary<string, CameraConfig> GetAllUserCameraConfigs()
    {
        lock (_lockObj)
        {
            return new Dictionary<string, CameraConfig>(_userCameraConfigs, StringComparer.OrdinalIgnoreCase);
        }
    }

    /// <summary>
    /// 根据序列号获取相机配置
    /// </summary>
    public CameraConfig GetCameraConfig(string serialNumber)
    {
        if (string.IsNullOrEmpty(serialNumber)) return null;

        lock (_lockObj)
        {
            _userCameraConfigs.TryGetValue(serialNumber, out var config);
            return config;
        }
    }

    public void UnInitialize()
    {
        foreach (var item in temCameraList)
        {
            item.Value.Close();
        }
    }
}

