using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace HardwareCameraNet;

/// <summary>
/// 通用配置工具（XML序列化/反序列化）
/// </summary>
public static class ConfigHelper
{
    /// <summary>
    /// 保存配置到本地XML文件
    /// </summary>
    public static void SaveConfig<T>(T config, string filePath) where T : class
    {
        if (config == null) throw new ArgumentNullException(nameof(config));
        if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath));

        // 确保目录存在
        string dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        // 序列化
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        using StreamWriter writer = new StreamWriter(filePath);
        serializer.Serialize(writer, config);

        Console.WriteLine($"配置已保存到：{filePath}");
    }

    /// <summary>
    /// 从本地XML文件加载配置
    /// </summary>
    public static T LoadConfig<T>(string filePath) where T : class, new()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"配置文件不存在：{filePath}，返回默认实例");
            return new T();
        }

        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using StreamReader reader = new StreamReader(filePath);
            T config = (T)serializer.Deserialize(reader);
            Console.WriteLine($"从{filePath}加载配置成功");
            return config;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"加载配置失败：{ex.InnerException?.Message ?? ex.Message}，返回默认实例");
            return new T();
        }
    }

    /// <summary>
    /// 相机配置集合（解决Dictionary无法直接序列化的问题）
    /// </summary>
    [XmlRoot("CameraConfigCollection")]
    public class CameraConfigCollection
    {
        [XmlElement("CameraConfig")]
        public List<CameraConfig> Configs { get; set; } = new();

        /// <summary>
        /// 转换为Dictionary（键：序列号）
        /// </summary>
        public Dictionary<string, CameraConfig> ToDictionary()
        {
            return Configs.ToDictionary(c => c.SerialNumber, c => c, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 从Dictionary转换为Collection
        /// </summary>
        public static CameraConfigCollection FromDictionary(Dictionary<string, CameraConfig> dict)
        {
            return new CameraConfigCollection
            {
                Configs = dict.Values.ToList()
            };
        }
    }
}

