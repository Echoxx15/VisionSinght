using System.Xml.Serialization;

namespace VisionCore.Manager.CameraManager;

/// <summary>
/// 相机配置类（仅包含用户可见配置）
/// </summary>
[XmlRoot("CameraConfig")]
public class CameraConfig
{
    [XmlElement("SerialNumber")]
    public string SerialNumber { get; set; }   // 相机序列号（唯一标识）

    [XmlElement("Expain")]
    public string Expain { get; set; } = "";   // 相机备注

    [XmlElement("Manufacturer")]
    public string Manufacturer { get; set; }   // 相机品牌（用于关联插件）
}