using System;
using System.Collections.Generic;

namespace VisionCore.PluginBase;

/// <summary>
/// 所有插件的基接口
/// </summary>
public interface IPlugin
{
    string PluginId { get; }
    Version Version { get; }
    string Description { get; }
}

/// <summary>
/// 相机插件接口
/// </summary>
public interface ICameraPlugin : IPlugin
{
    string Manufacturer { get; }
    /// <summary>
    /// 枚举设备（返回序列号列表）
    /// </summary>
    List<string> EnumerateDevices();
    /// <summary>
    /// 创建相机实例
    /// </summary>
    ICameraDevice CreateDevice(string serialNumber);
}

/// <summary>
/// 相机设备实例接口（具体设备操作）
/// </summary>
public interface ICameraDevice
{
    string SerialNumber { get; }
    bool IsConnected { get; }
    int Open();
    void Close();
    // 其他相机操作接口
}

/// <summary>
/// 工具插件接口
/// </summary>
public interface IToolPlugin : IPlugin
{
    void AddToToolbar(object toolbar);
}

/// <summary>
/// 通讯插件接口
/// </summary>
public interface ICommunicationPlugin : IPlugin
{
    void Connect();
    void Disconnect();
}