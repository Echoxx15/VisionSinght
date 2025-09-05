using System;

namespace VisionCore.PluginBase;

/// <summary>
/// 所有插件的基接口
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// 插件唯一标识
    /// </summary>
    string PluginId { get; }
    /// <summary>
    /// 插件版本（如"1.0.0"）
    /// </summary>
    Version Version { get; }

    /// <summary>
    /// 插件描述
    /// </summary>
    string Description { get; }
}
