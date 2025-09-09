﻿using System;
using System.Collections.Generic;
using HardwareCameraNet;
using HardwareCameraNet.IValue;
using MVSDK_Net;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;

namespace iRAYPLECameraPlugin;

[CameraManufacturer("华睿面阵相机")]
public class IRAYPLECamera : ICamera
{

    #region 相机属性
    private readonly MyCamera device = new();
    private IMVDefine.IMV_ConnectCallBack pConnectCallBack;
    private IMVDefine.IMV_FrameCallBack frameCallBack;
    private readonly ConcurrentQueue<IMVDefine.IMV_Frame> frameQueue = new();
    private Thread _asyncProcessThread;
    private static readonly Dictionary<string, string> gDevInfoCameraKeys = new();

    private readonly Stopwatch _stopwatch = new();
    /// <summary>
    /// ch: 队列图像数量上限 | en: maximum number of frames in the queue
    /// </summary>
    private const uint _maxQueueSize = 200;
    /// <summary>
    /// ch: 异步处理线程退出标志 false 不退出 | en: Flag to notify the  processing thread to exit
    /// </summary>
    private volatile bool _processThreadExit;
    #endregion

    #region 属性
    public string PluginId => "iRAYPLE";
    public Version Version => new Version("1.0.0");
    public string Description => "华睿面阵相机系列";

    // 图像回调事件（需显式实现事件添加/移除逻辑，确保线程安全）
    private event EventHandler<object> frameGrabedEvent;
    public event EventHandler<object> FrameGrabedEvent
    {
        add => frameGrabedEvent += value;
        remove => frameGrabedEvent -= value;
    }
    public event EventHandler<object> DisConnetEvent;
    public string SN { get; }
    public bool IsConnected => device.IMV_IsOpen();

    #endregion

    #region 构造函数
    public IRAYPLECamera(string sn)
    {
        SN = sn ?? throw new ArgumentNullException(nameof(sn));
    }
    #endregion

    #region 静态枚举方法
    public static List<string> EnumerateDevices()
    {
        //枚举设备
        //Discover device
        IMVDefine.IMV_DeviceList deviceList = new IMVDefine.IMV_DeviceList();
        IMVDefine.IMV_EInterfaceType interfaceTp = IMVDefine.IMV_EInterfaceType.interfaceTypeGige | IMVDefine.IMV_EInterfaceType.interfaceTypeUsb3;
        var res = MyCamera.IMV_EnumDevices(ref deviceList, (uint)interfaceTp);
        if (res == IMVDefine.IMV_OK)
        {
            if (deviceList.nDevNum > 0)
            {
                for (int i = 0; i < deviceList.nDevNum; i++)
                {
                    IMVDefine.IMV_DeviceInfo devInfo =
                        (IMVDefine.IMV_DeviceInfo)
                        Marshal.PtrToStructure(
                            deviceList.pDevInfo + Marshal.SizeOf(typeof(IMVDefine.IMV_DeviceInfo)) * i,
                            typeof(IMVDefine.IMV_DeviceInfo));

                    if (devInfo.manufactureInfo != "Huaray Technology") continue;

                    if (!gDevInfoCameraKeys.ContainsKey(devInfo.serialNumber))
                        gDevInfoCameraKeys.Add(devInfo.serialNumber, devInfo.cameraKey);
                }
                return gDevInfoCameraKeys.Keys.ToList();
            }
        }
        Console.WriteLine("Enumeration devices failed! ErrorCode:[{0}]", res);
        return [];
    }
    #endregion

    #region 实现ICamera接口的方法



    public int Open()
    {
        try
        {
            // 创建设备句柄
            // Create Device Handle
            var res = device.IMV_CreateHandle(IMVDefine.IMV_ECreateHandleMode.modeByCameraKey, 0, gDevInfoCameraKeys[SN]);
            if (res != IMVDefine.IMV_OK)
            {
                Console.WriteLine("Create devHandle failed! ErrorCode[{0}]", res);
                return res;
            }
            // 打开相机设备 
            // Connect to camera 
            res = device.IMV_Open();
            if (res != IMVDefine.IMV_OK)
            {
                Console.WriteLine("Open camera failed! ErrorCode:[{0}]", res);
                return res;
            }
            SetSoftwareTrigger();
            // 注册数据帧回调函数
            // Register data frame callback function
            frameCallBack = onGetFrame;
            res = device.IMV_AttachGrabbing(frameCallBack, IntPtr.Zero);
            if (res != IMVDefine.IMV_OK)
            {
                Console.WriteLine("Attach grabbing failed! ErrorCode:[{0}]", res);
                return res;
            }
            // 设备连接状态事件回调函数
            // Device connection status event callback function
            pConnectCallBack = onDeviceLinkNotify;
            res = device.IMV_SubscribeConnectArg(pConnectCallBack, IntPtr.Zero);
            if (res != IMVDefine.IMV_OK)
            {
                Console.WriteLine("SubscribeConnectArg failed! ErrorCode:[{0}]", res);
                return res;
            }
            //ch：创建异步处理线程 | en: Create an asynchronous processing thread
            _processThreadExit = false;
            _asyncProcessThread = new Thread(AsyncProcessThread) { IsBackground = true };
            _asyncProcessThread.Start();

            res = StartGrabbing();
            return IMVDefine.IMV_OK != res ? res : IMVDefine.IMV_OK;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }
    }

    public IFloatVal GetExposureTime()
    {
        double val = 0;
        double max = 0;
        // 获取属性值
        if (IMVDefine.IMV_OK != device.IMV_GetDoubleFeatureValue("ExposureTime", ref val)) return null;
        if (IMVDefine.IMV_OK != device.IMV_GetDoubleFeatureMax("ExposureTime", ref max)) return null;
        var floatVal = new FloatValImpl(
            curValue: Convert.ToDouble(val),
            max: Convert.ToDouble(max),
            min: Convert.ToDouble(0)
        );

        return floatVal;
    }

    public void SetExposureTime(double val)
    {
        // 设置属性值
        device.IMV_SetEnumFeatureSymbol("ExposureAuto", "Off");
        var ret = device.IMV_SetDoubleFeatureValue("ExposureTime", val);
        if (IMVDefine.IMV_OK != ret)
        {
            Console.WriteLine($"Get feature value failed! ErrorCode[{ret}]");
        }
    }

    public IFloatVal GetGain()
    {
        double val = 0;
        double max = 0;
        // 获取属性值
        if (IMVDefine.IMV_OK != device.IMV_GetDoubleFeatureValue("GainRaw", ref val)) return null;
        if (IMVDefine.IMV_OK != device.IMV_GetDoubleFeatureMax("GainRaw", ref max)) return null;
        var floatVal = new FloatValImpl(
            curValue: Convert.ToDouble(val),
            max: Convert.ToDouble(max),
            min: Convert.ToDouble(0)
        );

        return floatVal;
    }

    public void SetGain(double val)
    {
        var ret = device.IMV_SetDoubleFeatureValue("GainRaw", val);
        if (IMVDefine.IMV_OK != ret)
        {
            Console.WriteLine($"Get feature value failed! ErrorCode[{ret}]");
        }
    }

    public void SetSoftwareTrigger()
    {
        // 设置触发源为软触发 
        // Set trigger source to Software 
        int res = device.IMV_SetEnumFeatureSymbol("TriggerSource", "Software");
        if (res != IMVDefine.IMV_OK)
        {
            Console.WriteLine("Set triggerSource value failed! ErrorCode[{0}]", res);
            return;
        }

        // 设置触发器 
        // Set trigger selector to FrameStart 
        res = device.IMV_SetEnumFeatureSymbol("TriggerSelector", "FrameStart");
        if (res != IMVDefine.IMV_OK)
        {
            Console.WriteLine("Set triggerSelector value failed! ErrorCode[{0}]", res);
            return;
        }

        // 设置触发模式 
        // Set trigger mode to On 
        res = device.IMV_SetEnumFeatureSymbol("TriggerMode", "On");
        if (res != IMVDefine.IMV_OK)
        {
            Console.WriteLine("Set triggerMode value failed! ErrorCode[{0}]", res);
        }
    }

    public void SetTriggerSource(string triggerSource)
    {
        // set TriggerMode On
        device.IMV_SetEnumFeatureSymbol("TriggerMode", "On");
        device.IMV_SetEnumFeatureSymbol("TriggerSource", triggerSource);
    }

    public IStringVal GetTriggerSource()
    {
        IMVDefine.IMV_EnumEntryList enumEntryList = new IMVDefine.IMV_EnumEntryList();
        uint nEntryNum = 0;

        // 获取枚举属性的可设枚举值的个数
        //  Get the number of enumeration property settable enumeration
        var res = device.IMV_GetEnumFeatureEntryNum("TriggerSource", ref nEntryNum);
        if (res != IMVDefine.IMV_OK)
        {
            Console.WriteLine("Get settable enumeration number failed! ErrorCode[{0}]", res);
            return null;
        }

        enumEntryList.nEnumEntryBufferSize = (uint)Marshal.SizeOf(typeof(IMVDefine.IMV_EnumEntryInfo)) * nEntryNum;
        enumEntryList.pEnumEntryInfo = Marshal.AllocHGlobal((int)enumEntryList.nEnumEntryBufferSize);

        if (enumEntryList.pEnumEntryInfo == IntPtr.Zero)
        {
            Console.WriteLine("pEnumEntryInfo is NULL");
            return null;
        }

        // 获取枚举属性的可设枚举值列表
        // Get enumeration property's settable enumeration value list
        res = device.IMV_GetEnumFeatureEntrys("TriggerSource", ref enumEntryList);
        if (res != IMVDefine.IMV_OK)
        {
            Console.WriteLine("Get settable enumeration value failed! ErrorCode[{0}]", res);
            return null;
        }
        
        var se = new List<string>();
        for (int i = 0; i < nEntryNum; i++)
        {
            var t =
                (IMVDefine.IMV_EnumEntryInfo)
                Marshal.PtrToStructure(
                    enumEntryList.pEnumEntryInfo + Marshal.SizeOf(typeof(IMVDefine.IMV_EnumEntryInfo)) * i,
                    typeof(IMVDefine.IMV_EnumEntryInfo));

            se.Add(t.name);
        }
        var cur = new IMVDefine.IMV_String();
        device.IMV_GetEnumFeatureSymbol("TriggerSource", ref cur);
        var stringval = new StringValImpl(
            curEnumEntry: cur.str,
            supportEnumEntries: se
        );

        return stringval;
    }

    public void SoftwareTriggerOnce()
    {
        SetSoftwareTrigger();
        // 开始采集 
        // ExecuteCommand TriggerSoftware
        var res = device.IMV_ExecuteCommandFeature("TriggerSoftware");
        if (res != IMVDefine.IMV_OK)
        {
            Console.WriteLine("Set triggerMode value failed! ErrorCode[{0}]", res);
        }
    }

    public void ContinuousGrab()
    {
        // set TriggerMode Off
        device.IMV_SetEnumFeatureSymbol("TriggerMode", "Off");
    }

    public void StopContinuousGrab()
    {
        // set TriggerMode Off
        device.IMV_SetEnumFeatureSymbol("TriggerMode", "On");
        lock (this)
        {
            while (frameQueue.TryDequeue(out var _)) { }
        }
    }

    public int StartGrabbing()
    {
        if (device.IMV_IsGrabbing()) return IMVDefine.IMV_OK;
        // 开始拉流 
        // Start grabbing 
        var res = device.IMV_StartGrabbing();
        if (res != IMVDefine.IMV_OK)
        {
            Console.WriteLine("Start grabbing failed! ErrorCode:[{0}]", res);
            return res;
        }

        return IMVDefine.IMV_OK;
    }

    public int StopGrabbing()
    {
        if (!device.IMV_IsGrabbing()) return IMVDefine.IMV_OK;
        // 停止拉流 
        // Stop grabbing 
        var res = device.IMV_StopGrabbing();
        if (res != IMVDefine.IMV_OK)
        {
            Console.WriteLine("Stop grabbing failed! ErrorCode:[{0}]", res);
            return res;
        }
        return IMVDefine.IMV_OK;
    }

    public void DisConnet()
    {
        //关闭相机
        //Close camera 
        StopGrabbing();
        StopThread();
        var res = device.IMV_Close();
        if (res != IMVDefine.IMV_OK)
        {
            Console.WriteLine("Close camera failed! ErrorCode:[{0}]", res);
        }
    }

    public void Close()
    {
        //关闭相机
        //Close camera 
        StopThread();
        var res = device.IMV_Close();
        if (res != IMVDefine.IMV_OK)
        {
            Console.WriteLine("Close camera failed! ErrorCode:[{0}]", res);
        }

        // 销毁设备句柄
        // Destroy Device Handle
        res = device.IMV_DestroyHandle();
        if (res != IMVDefine.IMV_OK)
        {
            Console.WriteLine("Destroy camera failed! ErrorCode[{0}]", res);
        }
    }
    #endregion

    #region 内部方法
    #region 断线重连
    // 连接事件通知回调函数
    // Connect event notify callback function
    private void onDeviceLinkNotify(ref IMVDefine.IMV_SConnectArg pConnectArg, IntPtr pUser)
    {
        switch (pConnectArg.EvType)
        {
            // 断线通知
            // offLine notify 
            case IMVDefine.IMV_EVType.offLine:
                DisConnetEvent?.Invoke(this, true);
                StopThread();
                Console.WriteLine($"[{SN}]Device disconnect!");
                break;
            // 上线通知
            // onLine notify 
            case IMVDefine.IMV_EVType.onLine:
                {
                    device.IMV_Close();
                    if(IMVDefine.IMV_OK != Open())
                        Console.WriteLine($"[{SN}]Device Reconnect Fail!");
                    break;
                }
        }
    }
    #endregion

    #region 图像回调
    // 数据帧回调函数
    // Data frame callback function
    private void onGetFrame(ref IMVDefine.IMV_Frame frame, IntPtr pUser)
    {
        try
        {
            if (frame.frameHandle == IntPtr.Zero)
            {
                Console.WriteLine("frame is NULL");
                return;
            }


            if (frame.pData == IntPtr.Zero || frame.frameInfo.size == 0)
            {
                Console.WriteLine("Invalid frame data");
                // 即使数据无效，也需要释放帧
                device.IMV_ReleaseFrame(ref frame);
                return;
            }

            IMVDefine.IMV_Frame frameclone = new IMVDefine.IMV_Frame();
            int res = device.IMV_CloneFrame(ref frame, ref frameclone);

            if (res != IMVDefine.IMV_OK)
            {
                Console.WriteLine($"Clone frame failed: {res}");
                device.IMV_ReleaseFrame(ref frame);
                return;
            }
            lock (this)
            {
                if (frameQueue.Count <= _maxQueueSize)
                    frameQueue.Enqueue(frameclone);
            }

            // 释放图像缓存
            // Free image buffer
            device.IMV_ReleaseFrame(ref frame);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private void AsyncProcessThread()
    {
        while (!_processThreadExit)
        {
            lock (this)
            {
                if (frameQueue.TryDequeue(out var frame))
                {
                    try
                    {
                        _stopwatch.Restart();
                        Bitmap bmp = null;
                        ConvertToBitmap(ref frame, ref bmp);

                        if (bmp != null)
                        {
                            frameGrabedEvent?.Invoke(this, bmp.Clone());
                            _stopwatch.Stop();
                            bmp.Dispose();
                            GC.Collect();
                            Console.WriteLine("回调转换耗时:{0}", _stopwatch.ElapsedMilliseconds);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    finally
                    {
                        //释放克隆帧
                        var res = device.IMV_ReleaseFrame(ref frame);
                        if (res != IMVDefine.IMV_OK)
                        {
                            Console.WriteLine($"Release cloned frame failed: {res}");
                        }
                    }
                }
            }

            Thread.Sleep(1);
        }
    }

    private bool isColor(IMVDefine.IMV_EPixelType format)
    {
        if (format == IMVDefine.IMV_EPixelType.gvspPixelMono8 || format == IMVDefine.IMV_EPixelType.gvspPixelMono10 ||
            format == IMVDefine.IMV_EPixelType.gvspPixelMono12 || format == IMVDefine.IMV_EPixelType.gvspPixelMono10Packed ||
            format == IMVDefine.IMV_EPixelType.gvspPixelMono12Packed)
        {
            return false;
        }

        return true;
    }

    private void ConvertToBitmap(ref IMVDefine.IMV_Frame frame, ref Bitmap bitmap)
    {
        try
        {
            BitmapData bmpData;
            Rectangle bitmapRect = new Rectangle();
            IMVDefine.IMV_PixelConvertParam stPixelConvertParam = new IMVDefine.IMV_PixelConvertParam();

            // mono8和BGR8裸数据不需要转码
            // mono8 and BGR8 raw data is not need to convert
            IntPtr pDstRGB;
            if (frame.frameInfo.pixelFormat != IMVDefine.IMV_EPixelType.gvspPixelMono8
                && frame.frameInfo.pixelFormat != IMVDefine.IMV_EPixelType.gvspPixelBGR8)
            {
                //转目标内存 彩色
                var ImgSize = (int)frame.frameInfo.width * (int)frame.frameInfo.height * 3;
                //当内存申请失败，返回false
                try
                {
                    pDstRGB = Marshal.AllocHGlobal(ImgSize);
                }
                catch
                {
                    return;
                }

                if (pDstRGB == IntPtr.Zero)
                {
                    return;
                }

                // 图像转换成BGR8
                // convert image to BGR8
                stPixelConvertParam.nWidth = frame.frameInfo.width;
                stPixelConvertParam.nHeight = frame.frameInfo.height;
                stPixelConvertParam.ePixelFormat = frame.frameInfo.pixelFormat;
                stPixelConvertParam.pSrcData = frame.pData;
                stPixelConvertParam.nSrcDataLen = frame.frameInfo.size;
                stPixelConvertParam.nPaddingX = frame.frameInfo.paddingX;
                stPixelConvertParam.nPaddingY = frame.frameInfo.paddingY;
                stPixelConvertParam.eBayerDemosaic = IMVDefine.IMV_EBayerDemosaic.demosaicNearestNeighbor;
                stPixelConvertParam.eDstPixelFormat = IMVDefine.IMV_EPixelType.gvspPixelBGR8;
                stPixelConvertParam.pDstBuf = pDstRGB;
                stPixelConvertParam.nDstBufSize = (uint)ImgSize;
                var res = device.IMV_PixelConvert(ref stPixelConvertParam);
                if (res != IMVDefine.IMV_OK)
                {
                    Console.WriteLine("image convert to BGR failed!");
                    return;
                }
            }
            else
            {
                pDstRGB = frame.pData;
            }

            if (frame.frameInfo.pixelFormat == IMVDefine.IMV_EPixelType.gvspPixelMono8)
            {
                // 用Mono8数据生成Bitmap
                bitmap = new Bitmap((int)frame.frameInfo.width, (int)frame.frameInfo.height, PixelFormat.Format8bppIndexed);
                ColorPalette colorPalette = bitmap.Palette;
                for (int i = 0; i != 256; ++i)
                {
                    colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                }

                bitmap.Palette = colorPalette;

                bitmapRect.Height = bitmap.Height;
                bitmapRect.Width = bitmap.Width;
                bmpData = bitmap.LockBits(bitmapRect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                CopyMemory(bmpData.Scan0, pDstRGB, bmpData.Stride * bitmap.Height);
                bitmap.UnlockBits(bmpData);
            }
            else
            {
                // 用BGR24数据生成Bitmap
                bitmap = new Bitmap((int)frame.frameInfo.width, (int)frame.frameInfo.height, PixelFormat.Format24bppRgb);
                bitmapRect.Height = bitmap.Height;
                bitmapRect.Width = bitmap.Width;
                bmpData = bitmap.LockBits(bitmapRect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                CopyMemory(bmpData.Scan0, pDstRGB, bmpData.Stride * bitmap.Height);
                bitmap.UnlockBits(bmpData);
                if (frame.frameInfo.pixelFormat != IMVDefine.IMV_EPixelType.gvspPixelBGR8)
                {
                    Marshal.FreeHGlobal(pDstRGB);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    #endregion

    private void StopThread()
    {
        try
        {
            //ch: 通知异步处理线程退出 | en: Notify the thread to exit
            _processThreadExit = true;
            _asyncProcessThread.Join();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    /// <summary>
    /// 指针之间进行数据拷贝
    /// </summary>
    /// <param name="pDst">目标地址</param>
    /// <param name="pSrc">源地址</param>
    /// <param name="len">拷贝数据长度</param>
    [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", CharSet = CharSet.Ansi)]
    private static extern void CopyMemory(IntPtr pDst, IntPtr pSrc, int len);
    #endregion
}

