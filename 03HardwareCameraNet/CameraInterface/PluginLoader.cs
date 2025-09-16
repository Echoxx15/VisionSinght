using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VisionCore.PluginBase;

namespace HardwareCameraNet;
/// <summary>
/// 泛型插件加载器（支持加载任意IPlugin子接口类型的插件）
/// </summary>
/// <typeparam name="T">插件接口类型（需继承IPlugin）</typeparam>
internal class PluginLoader<T> where T : ICamera
{
    /// <summary>
    /// 插件目录（相对程序根目录）
    /// </summary>
    public string PluginDirectory { get; }

    /// <summary>
    /// 已加载的插件类型（键：类型完全限定名，值：类型）
    /// </summary>
    private Dictionary<string, Type> _loadedPluginTypes = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// 构造函数（指定插件类型和目录）
    /// </summary>
    /// <param name="relativeDir">插件相对目录（如Plugins/Camera、Plugins/Communication）</param>
    public PluginLoader(string relativeDir)
    {
        // 拼接绝对路径（程序根目录 + 相对目录）
        PluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeDir);
        EnsureDirectoryExists();
    }

    /// <summary>
    /// 确保插件目录存在
    /// </summary>
    private void EnsureDirectoryExists()
    {
        if (!Directory.Exists(PluginDirectory))
        {
            Directory.CreateDirectory(PluginDirectory);
            Console.WriteLine($"创建插件目录：{PluginDirectory}");
        }
    }

    /// <summary>
    /// 加载目录下所有插件类型
    /// </summary>
    public void LoadPluginTypes()
    {
        _loadedPluginTypes.Clear();
        var dllFiles = Directory.GetFiles(PluginDirectory, "*.dll", SearchOption.TopDirectoryOnly);

        foreach (var dllPath in dllFiles)
        {
            try
            {
                // 加载程序集（避免锁定DLL文件，用LoadFrom而非LoadFile）
                Assembly assembly = Assembly.LoadFrom(dllPath);

                // 遍历程序集中所有类型，筛选实现T接口的非抽象类
                foreach (Type type in assembly.GetTypes())
                {
                    if (typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        string typeFullName = type.FullName;
                        if (!_loadedPluginTypes.ContainsKey(typeFullName))
                        {
                            _loadedPluginTypes.Add(typeFullName, type);
                            Console.WriteLine($"加载{typeof(T).Name}插件：{typeFullName}（来自{Path.GetFileName(dllPath)}）");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载插件{Path.GetFileName(dllPath)}失败：{ex.InnerException?.Message ?? ex.Message}");
            }
        }
    }

    /// <summary>
    /// 获取已加载的插件类型（返回副本，避免外部修改）
    /// </summary>
    public Dictionary<string, Type> GetLoadedPluginTypes()
    {
        return new Dictionary<string, Type>(_loadedPluginTypes, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 创建插件实例（支持带参数的构造函数）
    /// </summary>
    /// <param name="pluginTypeName">插件类型完全限定名</param>
    /// <param name="constructorArgs">构造函数参数（如相机序列号）</param>
    public T CreatePluginInstance(string pluginTypeName, params object[] constructorArgs)
    {
        if (string.IsNullOrEmpty(pluginTypeName) || !_loadedPluginTypes.TryGetValue(pluginTypeName, out Type type))
        {
            Console.WriteLine($"未找到{typeof(T).Name}插件类型：{pluginTypeName}");
            return default;
        }

        try
        {
            // 反射创建实例并初始化
            T instance = (T)Activator.CreateInstance(type, constructorArgs);
            Console.WriteLine($"创建{typeof(T).Name}实例：{type.FullName}（参数：{string.Join(",", constructorArgs)}）");
            return instance;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"创建{typeof(T).Name}实例失败：{ex.InnerException?.Message ?? ex.Message}");
            return default;
        }
    }
}
