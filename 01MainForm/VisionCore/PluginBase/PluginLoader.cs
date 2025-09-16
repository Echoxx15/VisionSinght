using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace VisionCore.PluginBase;
/// <summary>
/// 泛型插件加载器（支持加载任意IPlugin子接口类型的插件）
/// </summary>
/// <typeparam name="T">插件接口类型（需继承IPlugin）</typeparam>
public class PluginLoader<T> where T : IPlugin
{
    private readonly string PluginDirectory;
    private readonly Dictionary<string, Type> _loadedPluginTypes = new(StringComparer.OrdinalIgnoreCase);

    public PluginLoader(string relativeDir)
    {
        PluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativeDir);
        EnsureDirectoryExists();
    }

    private void EnsureDirectoryExists()
    {
        if (!Directory.Exists(PluginDirectory))
            Directory.CreateDirectory(PluginDirectory);
    }

    public void LoadPluginTypes()
    {
        _loadedPluginTypes.Clear();
        var dllFiles = Directory.GetFiles(PluginDirectory, "*.dll", SearchOption.TopDirectoryOnly);
        foreach (var dllPath in dllFiles)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        _loadedPluginTypes[type.FullName] = type;
                    }
                }
            }
            catch { /* 可加日志 */ }
        }
    }

    public IEnumerable<T> CreateAllPluginInstances(params object[] constructorArgs)
    {
        foreach (var type in _loadedPluginTypes.Values)
        {
            yield return (T)Activator.CreateInstance(type, constructorArgs);
        }
    }
}

