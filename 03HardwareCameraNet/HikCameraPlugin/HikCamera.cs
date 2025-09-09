using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using HardwareCameraNet;
using HardwareCameraNet.IValue;
using MvCameraControl;

namespace HikCameraPlugin;

/// <summary>
/// 海康相机实现类
/// </summary>
// 标记支持的品牌名称
[CameraManufacturer("海康面阵相机")]
public class HikCamera : ICamera
{
    public string PluginId => "Hikrobot";
    public Version Version => new Version("1.0.0");
    public string Description => "海康面阵相机系列";

    /// <summary>
    /// 海康支持的设备类型
    /// </summary>
    const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice;// | DeviceTLayerType.MvGenTLCameraLinkDevice
                                                                                                       //| DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLXoFDevice;


    #region ICamera接口属性
    // 图像回调事件（需显式实现事件添加/移除逻辑，确保线程安全）
    private event EventHandler<object> frameGrabedEvent;
    public event EventHandler<object> FrameGrabedEvent
    {
        add => frameGrabedEvent += value;
        remove => frameGrabedEvent -= value;
    }
    public event EventHandler<object> DisConnetEvent;
    public string SN { get; }

    public bool IsConnected => device is { IsConnected: true };

    #endregion

    #region 相机属性
    // 相机特有属性
    private IDevice device;
    private static readonly Dictionary<string, IDeviceInfo> gDeviceInfos = new();
    /// <summary>
    /// ch: 断线重连线程 | en: Reconnect thread
    /// </summary>
    private Thread ReconnectThread;
    /// <summary>
    /// ch: 帧缓存队列 | en: frame queue for process
    /// </summary>
    private readonly ConcurrentQueue<IFrameOut> frameQueue = new();

    /// <summary>
    /// ch: 异步处理线程 | asynchronous processing thread
    /// </summary>
    private Thread _asyncProcessThread;
    /// <summary>
    /// ch: 队列图像数量上限 | en: maximum number of frames in the queue
    /// </summary>
    private const uint _maxQueueSize = 200;
    /// <summary>
    /// ch: 异步处理线程退出标志 false 不退出 | en: Flag to notify the  processing thread to exit
    /// </summary>
    private volatile bool _processThreadExit;

    private readonly Stopwatch _stopwatch = new();
    #endregion

    #region 构造函数
    public HikCamera(string sn)
    {
        SN = sn ?? throw new ArgumentNullException(nameof(sn));
    }
    #endregion

    #region 静态枚举方法
    public static List<string> EnumerateDevices()
    {
        // ch:枚举设备 | en:Enum device
        int ret = DeviceEnumerator.EnumDevicesEx(devLayerType, "Hikrobot", out var devInfoList);
        if (ret != MvError.MV_OK)
        {
            Console.WriteLine("Enum device failed:{0:x8}", ret);
            return [];
        }
        foreach (var devInfo in devInfoList.Where(devInfo => !gDeviceInfos.ContainsKey(devInfo.SerialNumber)))
        {
            gDeviceInfos.Add(devInfo.SerialNumber, devInfo);
        }
        return gDeviceInfos.Keys.ToList();
    }
    #endregion

    #region 实现ICamera接口的方法


    public int Open()
    {
        try
        {
            // ch:枚举设备 | en:Enum device
            var ret = DeviceEnumerator.EnumDevices(devLayerType, out var devInfoList);
            if (ret != MvError.MV_OK)
            {
                Console.WriteLine("Enum device failed:{0:x8}", ret);
                return ret;
            }

            if (0 == devInfoList.Count)
            {
                return -1;
            }

            foreach (var devInfo in devInfoList)
            {
                if (SN != devInfo.SerialNumber) continue;
                device = DeviceFactory.CreateDevice(devInfo);
                ret = device.Open();
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Open device failed:{0:x8}", ret);
                    return ret;
                }
                break;
            }

            //if (!gDeviceInfos.TryGetValue(SN, out var info)) return -1;

            //device = DeviceFactory.CreateDevice(info);

            //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
            if (device is IGigEDevice)
            {
                //ch: 转换为gigE设备 | en: Convert to Gige device
                IGigEDevice gigEDevice = device as IGigEDevice;

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                ret = gigEDevice.GetOptimalPacketSize(out var optionPacketSize);
                if (ret != MvError.MV_OK)
                {
                    Console.WriteLine("Warning: Get Packet Size failed!");
                }
                else
                {
                    ret = device.Parameters.SetIntValue("GevSCPSPacketSize", optionPacketSize);
                    if (ret != MvError.MV_OK)
                    {
                        Console.WriteLine("Warning: Set Packet Size failed!");
                    }
                }
            }
            // ch:设置触发模式为on | en:set trigger mode as on
            ret = device.Parameters.SetEnumValueByString("TriggerMode", "On");
            if (ret != MvError.MV_OK)
            {
                //Console.WriteLine("Set TriggerMode failed:{0:x8}", ret);
                return ret;
            }

            // ch:注册回调函数 | en:Register image callback
            device.StreamGrabber.FrameGrabedEvent += FrameGrabedEventHandler;
            device.DeviceExceptionEvent += ExceptionEventHandler;

            //ch：创建异步处理线程 | en: Create an asynchronous processing thread
            _processThreadExit = false;
            _asyncProcessThread = new Thread(AsyncProcessThread) { IsBackground = true };
            _asyncProcessThread.Start();

            // ch:开启抓图 || en: start grab image
            ret = StartGrabbing();
            if (ret != MvError.MV_OK)
            {
                return ret;
            }
            Console.WriteLine("Open device Success:{0:x8}", ret);
            return MvError.MV_OK;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }
    }

    public IFloatVal GetExposureTime()
    {
        if (device.Parameters.GetFloatValue("ExposureTime", out var val) != MvError.MV_OK) return null;

        var floatVal = new FloatValImpl(
            curValue: Convert.ToDouble(val.CurValue),
            max: Convert.ToDouble(val.Max),
            min: Convert.ToDouble(val.Min)
        );

        return floatVal;
    }

    public void SetExposureTime(double val)
    {
        try
        {
            device.Parameters.SetEnumValue("ExposureAuto", 0u);
            device.Parameters.SetFloatValue("ExposureTime", (float)val);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public IFloatVal GetGain()
    {
        if (device.Parameters.GetFloatValue("Gain", out var val) != MvError.MV_OK) return null;

        var floatVal = new FloatValImpl(
            curValue: Convert.ToDouble(val.CurValue),
            max: Convert.ToDouble(val.Max),
            min: Convert.ToDouble(val.Min)
        );

        return floatVal;
    }
    public void SetGain(double val)
    {
        try
        {
            device.Parameters.SetEnumValue("GainAuto", 0u);
            device.Parameters.SetFloatValue("Gain", (float)val);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    public void SetSoftwareTrigger()
    {
        device.Parameters.SetEnumValueByString("TriggerMode", "On");
        device.Parameters.SetEnumValueByString("TriggerSource", "Software");
    }
    public void SetTriggerSource(string triggerSource)
    {
        device.Parameters.SetEnumValueByString("TriggerMode", "On");
        device.Parameters.SetEnumValueByString("TriggerSource", triggerSource);
    }

    public IStringVal GetTriggerSource()
    {
        if (device.Parameters.GetEnumValue("TriggerSource", out var val) != MvError.MV_OK) return null;

        List<string> se = [];
        foreach (var item in val.SupportEnumEntries)
        {
            se.Add(item.Symbolic);
        }
        var stringval = new StringValImpl(
            curEnumEntry: val.CurEnumEntry.Symbolic,
            supportEnumEntries: se
        );

        return stringval;
    }

    public void SoftwareTriggerOnce()
    {
        SetSoftwareTrigger();
        device.Parameters.SetCommandValue("TriggerSoftware");
    }

    public void ContinuousGrab()
    {
        device.Parameters.SetEnumValueByString("TriggerSource", "Software");
        device.Parameters.SetEnumValueByString("TriggerMode", "Off");
    }

    public void StopContinuousGrab()
    {
        device.Parameters.SetEnumValueByString("TriggerMode", "On");
        lock (this)
        {
            while (frameQueue.TryDequeue(out var _)) { }
        }
    }

    public int StartGrabbing()
    {

        // ch:开启抓图 | en: start grab image
        int ret = device.StreamGrabber.StartGrabbing();
        if (ret != MvError.MV_OK)
        {
            Console.WriteLine("Start grabbing failed:{0:x8}", ret);
        }
        return ret;
    }

    public int StopGrabbing()
    {
        // ch:停止抓图 | en:Stop grabbing
        var ret = device.StreamGrabber.StopGrabbing();
        if (ret != MvError.MV_OK)
        {
            Console.WriteLine("Stop grabbing failed:{0:x8}", ret);
            return ret;
        }
        return 0;
    }

    public void DisConnet()
    {

        // ch:关闭设备 | en:Close device
        if (device == null) return;
        StopThread();
        StopGrabbing();
        int ret = device.Close();
        if (ret != MvError.MV_OK)
        {
            Console.WriteLine("Close device failed:{0:x8}", ret);
        }
    }

    public void Close()
    {
        // ch:关闭设备 | en:Close device
        if (device == null) return;
        StopThread();
        if (device.IsConnected)
            device.Close();
        device.Dispose();
        device = null;
    }

    //public CameraConfig GetConfig()
    //{
    //    var config = new CameraConfig(SN,
    //        typeof(HikCamera).FullName,
    //        typeof(HikCamera).Assembly.GetName().Name);
    //    return config;
    //}

    //public void SetConfig(CameraConfig config)
    //{
    //    if (config == null)
    //        throw new ArgumentNullException(nameof(config));

    //    if (config.SerialNumber != this.SN)
    //        throw new ArgumentException("配置的序列号与相机不匹配");
    //}
    #endregion

    #region 内部方法

    #region 图像回调
    private void FrameGrabedEventHandler(object sender, FrameGrabbedEventArgs e)
    {
        if (e.FrameOut.Image.PixelDataPtr == IntPtr.Zero)
            return;

        try
        {
            lock (this)
            {
                if (frameQueue.Count <= _maxQueueSize)
                {
                    //ch: 添加到队列
                    frameQueue.Enqueue(e.FrameOut);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine("FrameGrabedEventHandler exception: " + exception);
        }
    }

    void AsyncProcessThread()
    {
        while (!_processThreadExit)
        {
            try
            {
                lock (this)
                {
                    _stopwatch.Restart();
                    if (!frameQueue.TryDequeue(out var frame)) continue;

                    var image = frame.Image;

                    //var dstPixelType = IsMonoPixelFormat(image.PixelType) ? MvGvspPixelType.PixelType_Gvsp_Mono8 : MvGvspPixelType.PixelType_Gvsp_BGR8_Packed;

                    //if (image.PixelType == MvGvspPixelType.PixelType_Gvsp_Undefined) continue;
                    //// ch:像素格式转换 | en:Pixel type convert 
                    //int result = device.PixelTypeConverter.ConvertPixelType(image, out var outImage, dstPixelType);
                    //if (result != MvError.MV_OK)
                    //{
                    //    Console.WriteLine("Image Convert failed:{0:x8}", result);
                    //    continue;
                    //}
                    //if (outImage.PixelDataPtr == IntPtr.Zero)
                    //{
                    //    continue;
                    //}

                    using var bitmap = image.ToBitmap();
                    frameGrabedEvent?.Invoke(this, bitmap.Clone());
                    _stopwatch.Stop();

                    image.Dispose();
                    device.StreamGrabber.FreeImageBuffer(frame);
                    GC.Collect();
                    Console.WriteLine("转换耗时:{0}", _stopwatch.ElapsedMilliseconds);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("AsyncProcessThread exception: " + e.Message);
            }
            Thread.Sleep(2);
        }
    }

    private bool IsMonoPixelFormat(MvGvspPixelType enType)
    {
        switch (enType)
        {
            case MvGvspPixelType.PixelType_Gvsp_Mono8:
            case MvGvspPixelType.PixelType_Gvsp_Mono10_Packed:
            case MvGvspPixelType.PixelType_Gvsp_Mono12_Packed:
            case MvGvspPixelType.PixelType_Gvsp_Mono10:
            case MvGvspPixelType.PixelType_Gvsp_Mono12:
                return true;
            default:
                return false;
        }
    }

    #endregion


    #region 断线重连
    private void ExceptionEventHandler(object sender, DeviceExceptionArgs e)
    {
        if (e.MsgType == DeviceExceptionType.DisConnect)
        {
            DisConnetEvent?.Invoke(this,true);
            Console.WriteLine($"[{SN}]Device disconnect!");
            StopThread();
            if (device != null)
            {
                device.Close();
                device.Dispose();
                device = null;
            }
            ReconnectThread = new Thread(ReconnectProcess) { IsBackground = true };
            ReconnectThread.Start();
        }
    }

    private void ReconnectProcess()
    {
        while (!IsConnected)
        {
            if (Open() == MvError.MV_OK)
            {
                DisConnetEvent?.Invoke(this, false);
                Console.WriteLine($"[{SN}]相机重连成功");
                break;
            }
            if (device != null)
            {
                device.Close();
                device.Dispose();
                device = null;
            }
            Thread.Sleep(3000);
        }
    }
    #endregion

    private void StopThread()
    {
        try
        {
            device.StreamGrabber.FrameGrabedEvent -= FrameGrabedEventHandler;
            device.DeviceExceptionEvent -= ExceptionEventHandler;
            //ch: 通知异步处理线程退出 | en: Notify the thread to exit
            _processThreadExit = true;
            _asyncProcessThread.Join();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    #endregion
}