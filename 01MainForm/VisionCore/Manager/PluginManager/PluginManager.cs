using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;


namespace VisionCore.Manager.PluginManager;

public sealed class PluginManager
{
    // 单例模式
    private static readonly Lazy<PluginManager> _instance = new(() => new PluginManager(),
        LazyThreadSafetyMode.ExecutionAndPublication);
    public static PluginManager Instance => _instance.Value;

    // 插件根目录
    private readonly string _pluginRootDirectory;

    // 插件存储（按接口类型分类）
    private readonly Dictionary<Type, List<IPlugin>> _pluginsByType = new();

    // 相机插件映射（厂商名称 -> 相机插件实例）
    private readonly Dictionary<string, ICameraPlugin> _cameraPluginsByManufacturer = new(
        StringComparer.OrdinalIgnoreCase);

    private PluginManager()
    {
        // 初始化插件根目录
        _pluginRootDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        EnsureDirectoryExists(_pluginRootDirectory);

        // 初始化支持的插件类型
        RegisterPluginType<ICameraPlugin>();
        RegisterPluginType<IToolPlugin>();
        RegisterPluginType<ICommunicationPlugin>();

        // 加载所有插件
        LoadAllPlugins();
    }

    #region 初始化与加载

    /// <summary>
    /// 注册支持的插件类型
    /// </summary>
    private void RegisterPluginType<T>() where T : IPlugin
    {
        var type = typeof(T);
        if (!_pluginsByType.ContainsKey(type))
        {
            _pluginsByType[type] = new List<IPlugin>();
        }
    }

    /// <summary>
    /// 加载所有子目录的插件（按类型分目录存放）
    /// </summary>
    private void LoadAllPlugins()
    {
        // 清空现有插件
        _pluginsByType.ToList().ForEach(kv => kv.Value.Clear());
        _cameraPluginsByManufacturer.Clear();

        // 加载相机插件（Plugins/Camera目录）
        LoadPluginsFromDirectory<ICameraPlugin>("Camera");
        // 加载工具插件（Plugins/Tool目录）
        LoadPluginsFromDirectory<IToolPlugin>("Tool");
        // 加载通讯插件（Plugins/Communication目录）
        LoadPluginsFromDirectory<ICommunicationPlugin>("Communication");

        // 构建相机插件厂商映射
        BuildCameraPluginMap();
    }

    /// <summary>
    /// 从指定子目录加载特定类型插件
    /// </summary>
    private void LoadPluginsFromDirectory<T>(string subDirectory) where T : IPlugin
    {
        var pluginType = typeof(T);
        var directoryPath = Path.Combine(_pluginRootDirectory, subDirectory);
        EnsureDirectoryExists(directoryPath);

        foreach (var dllPath in Directory.GetFiles(directoryPath, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsClass || type.IsAbstract) continue;
                    if (pluginType.IsAssignableFrom(type))
                    {
                        var pluginInstance = (T)Activator.CreateInstance(type);
                        _pluginsByType[pluginType].Add(pluginInstance);
                        Console.WriteLine($"加载{pluginType.Name}插件：{type.FullName}（来自{Path.GetFileName(dllPath)}）");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载{pluginType.Name}插件失败（{dllPath}）：{ex.Message}");
            }
        }
    }

    /// <summary>
    /// 构建相机插件厂商映射（厂商名称 -> 插件实例）
    /// </summary>
    private void BuildCameraPluginMap()
    {
        var cameraPlugins = GetPlugins<ICameraPlugin>();
        foreach (var plugin in cameraPlugins)
        {
            if (!_cameraPluginsByManufacturer.ContainsKey(plugin.Manufacturer))
            {
                _cameraPluginsByManufacturer[plugin.Manufacturer] = plugin;
            }
            else
            {
                Console.WriteLine($"警告：厂商{plugin.Manufacturer}存在多个插件，仅保留第一个");
            }
        }
    }

    #endregion

    #region 公共接口

    /// <summary>
    /// 获取指定类型的所有插件
    /// </summary>
    public List<T> GetPlugins<T>() where T : IPlugin
    {
        var type = typeof(T);
        if (_pluginsByType.TryGetValue(type, out var plugins))
        {
            return plugins.Cast<T>().ToList();
        }
        return new List<T>();
    }

    /// <summary>
    /// 获取指定厂商的相机插件
    /// </summary>
    public ICameraPlugin GetCameraPluginByManufacturer(string manufacturer)
    {
        _cameraPluginsByManufacturer.TryGetValue(manufacturer, out var plugin);
        return plugin;
    }

    /// <summary>
    /// 获取所有支持的相机厂商
    /// </summary>
    public List<string> GetAllCameraManufacturers()
    {
        return _cameraPluginsByManufacturer.Keys.OrderBy(k => k).ToList();
    }

    /// <summary>
    /// 重新加载所有插件
    /// </summary>
    public void ReloadPlugins()
    {
        LoadAllPlugins();
    }

    #endregion

    #region 辅助方法

    private void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Console.WriteLine($"创建插件目录：{path}");
        }
    }

    #endregion
}