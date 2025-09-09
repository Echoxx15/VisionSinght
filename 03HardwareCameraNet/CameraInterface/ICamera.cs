using System;
using HardwareCameraNet.IValue;
using VisionCore.PluginBase;

namespace HardwareCameraNet;

/// <summary>
/// 相机接口，定义所有相机插件必须实现的方法和属性
/// </summary>
public interface ICamera : IPlugin
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
    /// 设备序列号
    /// </summary>
    string SN { get; }
    /// <summary>
    /// 是否已连接
    /// </summary>
    bool IsConnected { get;}
    #endregion

    #region 方法

    /// <summary>
    /// 打开相机
    /// </summary>
    /// <returns>成功返回true，失败返回false</returns>
    int Open();
    /// <summary>
    /// 获取曝光时间
    /// </summary>
     IFloatVal GetExposureTime();
    /// <summary>
    /// 设置曝光时间
    /// </summary>
    /// <returns></returns>
    void SetExposureTime(double val);
    /// <summary>
    /// 获取增益
    /// </summary>
    /// <returns></returns>
    IFloatVal GetGain();
    /// <summary>
    /// 设置增益
    /// </summary>
    /// <returns></returns>
    void SetGain(double val);
    /// <summary>
    /// 设置软件触发模式
    /// </summary>
    /// <returns></returns>
    void SetSoftwareTrigger();
    /// <summary>
    /// 设置硬件触发模式
    /// </summary>
    /// <returns></returns>
    void SetTriggerSource(string triggerSource);
    /// <summary>
    /// 获取触发源
    /// </summary>
    /// <returns></returns>
    IStringVal GetTriggerSource();
    /// <summary>
    /// 软触发一次
    /// </summary>
    void SoftwareTriggerOnce();
    /// <summary>
    /// 持续抓图
    /// </summary>
    /// <returns></returns>
    void ContinuousGrab();
    /// <summary>
    /// 停止持续抓图
    /// </summary>
    void StopContinuousGrab();
    /// <summary>
    /// 开始抓图
    /// </summary>
    /// <returns></returns>
    int StartGrabbing();
    /// <summary>
    /// 停止抓图
    /// </summary>
    /// <returns></returns>
    int StopGrabbing();
    /// <summary>
    /// 断开相机
    /// </summary>
    void DisConnet();
    /// <summary>
    /// 关闭相机
    /// </summary>
    void Close();
    #endregion
}