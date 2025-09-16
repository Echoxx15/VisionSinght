using System;

namespace HardwareCameraNet;

/// <summary>
/// 相机接口，定义所有相机插件必须实现的方法和属性
/// </summary>
public interface ICamera
{

    /// <summary>
    /// 图片回调事件
    /// </summary>
    event EventHandler<object> FrameGrabedEvent;

    /// <summary>
    /// 掉线事件,掉线时触发，true 表示掉线，false表示恢复
    /// </summary>
    event EventHandler<object> DisConnetEvent;

    #region 属性
    /// <summary>
    /// 品牌标识用于显示和枚举对应品牌相机，如：Basler、Daheng、Hikvision等
    /// </summary>
    string Manufacturer { get; }
    /// <summary>
    /// 设备序列号
    /// </summary>
    string SN { get; }

    /// <summary>
    /// 是否已连接
    /// </summary>
    bool IsConnected { get; }

    IParameters Parameters { get; }
    #endregion

    #region 方法
    /// <summary>
    /// 打开相机
    /// </summary>
    /// <returns>成功返回true，失败返回false</returns>
    int Open();
    /// <summary>
    /// 关闭相机
    /// </summary>
    void Close();
    #endregion
}