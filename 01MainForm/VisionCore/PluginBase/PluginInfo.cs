namespace VisionCore.PluginBase;

/// <summary>
/// 插件信息结构体，存储插件类型信息
/// </summary>
public struct PluginInfo
{
    public string TypeName { get; set; }       // 插件类型全名（命名空间+类名）
    public string AssemblyName { get; set; }   // 插件程序集名称
}