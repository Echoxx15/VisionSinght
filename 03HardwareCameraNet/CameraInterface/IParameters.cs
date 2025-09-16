using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareCameraNet;

/// <summary>
/// 相机参数操作接口，统一参数获取/设置的方法
/// </summary>
public interface IParameters
{
    /// <summary>
    /// 曝光
    /// </summary>
    double ExposureTime { get; set; }
    double MaxExposureTime { get; }
    /// <summary>
    /// 增益
    /// </summary>
    double Gain { get; set; }
    double MaxGain { get; }
    /// <summary>
    /// 当前触发源
    /// </summary>
    string TirggerSoure { get;set; }
    /// <summary>
    /// 相机触发源枚举项
    /// </summary>
    List<string> TirggerSoures { get; }
}