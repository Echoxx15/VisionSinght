namespace HardwareCameraNet;
/// <summary>
/// 面阵相机设备接口
/// </summary>
public interface IDevice2D:ICamera
{
    #region 方法
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

    #endregion
}