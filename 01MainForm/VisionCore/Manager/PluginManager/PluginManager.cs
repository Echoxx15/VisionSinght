using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using VisionCore.PluginBase;

namespace VisionCore.Manager.PluginManager;

public class PluginManager
{
    public List<ICameraPlugin> CameraPlugins { get; } = new();
    public List<IToolPlugin> ToolPlugins { get; } = new();
    public List<ICommunicationPlugin> CommunicationPlugins { get; } = new();

    public void LoadPlugins(string pluginDir)
    {
        if (!Directory.Exists(pluginDir)) return;
        foreach (var dll in Directory.GetFiles(pluginDir, "*.dll"))
        {
            try
            {
                var asm = Assembly.LoadFrom(dll);
                foreach (var type in asm.GetTypes())
                {
                    if (!type.IsClass || type.IsAbstract) continue;
                    if (typeof(IPlugin).IsAssignableFrom(type))
                    {
                        var instance = Activator.CreateInstance(type) as IPlugin;
                        if (instance != null)
                        {
                            if (instance is ICameraPlugin cam) CameraPlugins.Add(cam);
                            if (instance is IToolPlugin tool) ToolPlugins.Add(tool);
                            if (instance is ICommunicationPlugin comm) CommunicationPlugins.Add(comm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"插件加载失败: {dll}, {ex.Message}");
            }
        }
    }
}