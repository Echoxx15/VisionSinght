using System.Xml.Serialization;

namespace HardwareCameraNet;

/// <summary>
/// 相机配置类，用于序列化和反序列化相机配置信息
/// </summary>
[XmlRoot("CameraConfig")]
public class CameraConfig
{
    /// <summary>
    /// 相机序列号
    /// </summary>
    [XmlElement("SerialNumber")]
    public string SerialNumber { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [XmlElement("Expain")]
    public string Expain { get; set; } = "";
    /// <summary>
    /// 插件类型全名（命名空间+类名）
    /// </summary>
    [XmlElement("PluginTypeName")]
    public string PluginTypeName { get; set; }

    /// <summary>
    /// 插件程序集名称
    /// </summary>
    [XmlElement("PluginAssemblyName")]
    public string PluginAssemblyName { get; set; }

    /// <summary>
    /// 无参构造函数，用于XML序列化
    /// </summary>
    public CameraConfig() { }

    /// <summary>
    /// 带参数的构造函数
    /// </summary>
    /// <param name="serialNumber">序列号</param>
    /// <param name="pluginTypeName">插件类型全名</param>
    /// <param name="pluginAssemblyName">插件程序集名称</param>
    public CameraConfig(string serialNumber, string pluginTypeName, string pluginAssemblyName)
    {
        SerialNumber = serialNumber;
        PluginTypeName = pluginTypeName;
        PluginAssemblyName = pluginAssemblyName;
    }
}