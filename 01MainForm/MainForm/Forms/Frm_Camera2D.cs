using System;
using System.Drawing;
using System.Windows.Forms;
using Cognex.VisionPro.Display;
using HardwareCameraNet;
using HardwareCameraNet.IValue;

namespace MainForm.Forms;

public sealed partial class Frm_Camera2D : Form
{
    private CogDisplay user_ShowDisplay;

    // 维护当前选中的相机实例，用于切换时取消订阅以防重复订阅
    private ICamera currentSelectedCamera;

    IFloatVal Exposure;
    IFloatVal Gain;
    IStringVal TriggerSource;
    public Frm_Camera2D()
    {
        InitializeComponent();
    }

    private void Frm_Camera2D_Load(object sender, EventArgs e)
    {
        cmb_Manufacturers.Properties.Items.AddRange(CameraManager.Instance.GetAllManufacturers());
        user_ShowDisplay = new CogDisplay();
        user_ShowDisplay.Dock = DockStyle.Fill;
        split_Display.Panel2.Controls.Add(user_ShowDisplay);
        Update();
    }

    private void SetControlState(bool flag = false)
    {
        chk_Hard.Checked = currentSelectedCamera.GetTriggerSource().CurEnumEntry != "Software";
        chk_Hard_CheckedChanged(null, null);
        if (flag)
        {
            txt_Exposure.Enabled = true;
            txt_Gain.Enabled = true;
            chk_Hard.Enabled = true;

            btn_Connect.Enabled = false;
            btn_TriggerOnce.Enabled = true;
            btn_Close.Enabled = true;
            btn_Continuous.Enabled = true;
        }
        else
        {
            txt_Exposure.Enabled = false;
            txt_Gain.Enabled = false;
            chk_Hard.Enabled = false;

            btn_Connect.Enabled = true;
            btn_TriggerOnce.Enabled = false;
            btn_Close.Enabled = false;
            btn_Continuous.Enabled = false;
        }
    }
    private void SetControlText()
    {
        try
        {
            //if (currentSelectedCamera.IsConnected) return;

            Exposure = currentSelectedCamera.GetExposureTime();
            Gain = currentSelectedCamera.GetGain();
            TriggerSource = currentSelectedCamera.GetTriggerSource();
            txt_Exposure.Text = Exposure.CurValue.ToString();
            txt_Gain.Text = Gain.ToString();
            txt_MaxExposure.Text = Exposure.Max.ToString("F5");
            txt_MaxGain.Text = Gain.Max.ToString("F5");
            btn_Connect.Enabled = !currentSelectedCamera.IsConnected;
            cmb_TriggerSource.Properties.Items.AddRange(TriggerSource.SupportEnumEntries);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void cmb_SnList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // 防止下拉框初始化时触发（无实际选择）
            if (cmb_SnList.SelectedItem == null || string.IsNullOrEmpty(cmb_SnList.SelectedText))
                return;

            // 取消当前相机的事件订阅（避免切换时残留订阅）
            UnsubscribeCurrentCameraEvent();

            // 获取厂商名称和选中的序列号
            var selectedManufacturer = cmb_Manufacturers.Text;
            var selectedSerial = cmb_SnList.SelectedText.Trim(); // 确保去除空格

            // 通过CameraManager获取相机实例（缓存中存在则直接返回）
            var newCamera = CameraManager.Instance.CreateCamera(selectedManufacturer, selectedSerial);
            if (newCamera == null)
            {
                MessageBox.Show($"获取相机{selectedSerial}失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                currentSelectedCamera = null;
                SetControlState(flag: false); // 更新UI为未连接状态
                return;
            }

            // 订阅新相机的事件（此时同一相机实例不会重复订阅，因步骤1已清空）
            SubscribeNewCameraEvent(newCamera);

            //更新当前相机状态，并同步UI
            currentSelectedCamera = newCamera;
            SetControlText();
        }
        catch (Exception)
        {
            // ignored
        }
    }
    /// <summary>
    /// 订阅新相机的FrameGrabedEvent（确保只订阅一次）
    /// </summary>
    private void SubscribeNewCameraEvent(ICamera newCamera)
    {
        if (newCamera == null) return;

        // （可选）双重保险：订阅前先取消一次（防止相机实例被其他地方订阅过）
        newCamera.FrameGrabedEvent -= UpdateUIImage;
        // 正式订阅
        newCamera.FrameGrabedEvent += UpdateUIImage;
        Console.WriteLine($"订阅相机{newCamera.SN}的图像事件");
    }
    /// <summary>
    /// 取消当前相机的FrameGrabedEvent订阅
    /// </summary>
    private void UnsubscribeCurrentCameraEvent()
    {
        if (currentSelectedCamera != null)
        {
            // 取消订阅（必须与订阅的方法完全一致，此处为UpdateUIImage）
            currentSelectedCamera.FrameGrabedEvent -= UpdateUIImage;
            Console.WriteLine($"取消订阅相机{currentSelectedCamera.SN}的图像事件");
        }
    }

    private void UpdateUIImage(object sender,Bitmap bmp)
    {
        if (user_ShowDisplay.InvokeRequired)
        {
            user_ShowDisplay.BeginInvoke(new Action<object,Bitmap>(UpdateUIImage), bmp);
        }
        else
        {
            //user_ShowDisplay.Image = imageData;
        }
    }

    private void txt_Exposure_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            var val = double.Parse(txt_Exposure.Text);
            if (val > Exposure.Max)
            {
                val = Exposure.Max;
                txt_Exposure.EditValue = val;
            }
            currentSelectedCamera.SetExposureTime(val);
        }
        catch (Exception exception)
        {
            MessageBox.Show("曝光设置失败" + exception, "", MessageBoxButtons.OK);
        }
    }

    private void txt_Gain_EditValueChanged(object sender, EventArgs e)
    {
        try
        {
            double val = double.Parse(txt_Gain.Text);
            if (val > Gain.Max)
            {
                val = Gain.Max;
                txt_Gain.EditValue = val;
            }
            currentSelectedCamera.SetGain(val);
        }
        catch (Exception exception)
        {
            MessageBox.Show("增益设置失败" + exception, "", MessageBoxButtons.OK);
        }
    }

    private void chk_Hard_CheckedChanged(object sender, EventArgs e)
    {
        cmb_TriggerSource.SelectedIndex = 0;
        cmb_TriggerSource.Visible = chk_Hard.Checked;
    }
    private void cmb_HardSource_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetHardTrigger();
    }
    private void SetHardTrigger()
    {
        currentSelectedCamera.SetTriggerSource(cmb_TriggerSource.Text);
    }
    private void btn_Connect_Click(object sender, EventArgs e)
    {
        int errCode = 0;
        try
        {
            errCode = currentSelectedCamera.Open();
            SetControlText();
            SetControlState(currentSelectedCamera.IsConnected);
        }
        catch (Exception exception)
        {
            MessageBox.Show($"关闭相机失败,错误码{errCode}," + exception, "", MessageBoxButtons.OK);
        }
    }
    private void btn_Close_Click(object sender, EventArgs e)
    {
        try
        {
            currentSelectedCamera.DisConnet();
            SetControlState(currentSelectedCamera.IsConnected);
        }
        catch (Exception exception)
        {
            MessageBox.Show($"关闭相机失败," + exception, "", MessageBoxButtons.OK);
        }
    }
    private void btn_TriggerOnce_Click(object sender, EventArgs e)
    {
        currentSelectedCamera.SoftwareTriggerOnce();
    }

    private void btn_Continue_Click(object sender, EventArgs e)
    {
        if (btn_Continuous.Text == "连续采集")
        {
            currentSelectedCamera.ContinuousGrab();
            btn_Continuous.Text = "停止采集";
            btn_Close.Enabled = false;
        }
        else
        {
            currentSelectedCamera.SetSoftwareTrigger();
            btn_Continuous.Text = "连续采集";
            btn_Close.Enabled = true;
        }
    }

    private void cmb_Manufacturers_SelectedIndexChanged(object sender, EventArgs e)
    {
        var list = CameraManager.Instance.EnumerateDevices(cmb_Manufacturers.Text);
        cmb_SnList.Properties.Items.AddRange(list);
    }
}

