using System;
using System.Drawing;
using System.Windows.Forms;
using Cognex.VisionPro;
using HardwareCameraNet;
using HardwareCameraNet.IValue;

namespace MainForm.Forms;

public partial class Frm_Camera2D : DevExpress.XtraEditors.XtraForm
{
    //private CogDisplay user_ShowDisplay;

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

        // 清空并加载本地相机配置到表格
        dgv_CameraConfig.Rows.Clear();
        foreach (var config in CameraManager.Instance.GetAllUserConfigs())
        {
            dgv_CameraConfig.Rows.Add(config.SerialNumber, config.Expain);
        }

        SetControlState();
    }


    private void dgv_CameraConfig_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {

        var expainCol = dgv_CameraConfig.Columns["col_Expain"];
        if (expainCol == null) return; // 列未初始化，直接返回

        if (e.ColumnIndex != expainCol.Index) return;

        var sn = dgv_CameraConfig.Rows[e.RowIndex].Cells["col_Sn"].Value?.ToString();
        var expain = dgv_CameraConfig.Rows[e.RowIndex].Cells["col_Expain"].Value?.ToString();

        if (!string.IsNullOrEmpty(sn))
        {
            var config = CameraManager.Instance.GetUserConfig(sn);
            if (config != null)
            {
                config.Expain = expain;
                CameraManager.Instance.ModifyCameraConfig(); // 自动保存
            }
        }
    }

    private void SetControlState(bool connect = false)
    {
        SetControlText();
        if (connect)
        {
            txt_Exposure.Enabled = true;
            txt_Gain.Enabled = true;
            chk_HardTrigger.Enabled = true;

            btn_Connect.Enabled = false;
            btn_TriggerOnce.Enabled = true;
            btn_DisConnect.Enabled = true;
            btn_Continuous.Enabled = true;
            chk_HardTrigger.Checked = TriggerSource.CurEnumEntry != "Software";
        }
        else
        {
            txt_Exposure.Enabled = false;
            txt_Gain.Enabled = false;
            chk_HardTrigger.Enabled = false;

            btn_Connect.Enabled = true;
            btn_TriggerOnce.Enabled = false;
            btn_DisConnect.Enabled = false;
            btn_Continuous.Enabled = false;
            chk_HardTrigger.Checked = false;
        }
        chk_HardTrigger_CheckedChanged(null, null);
        
    }
    private void SetControlText()
    {
        try
        {
            if(currentSelectedCamera == null)return;

            if (currentSelectedCamera.IsConnected)
            {
                Exposure = currentSelectedCamera.GetExposureTime();
                Gain = currentSelectedCamera.GetGain();
                TriggerSource = currentSelectedCamera.GetTriggerSource();
            }

            txt_Exposure.EditValue = Exposure == null? "": Exposure.CurValue;
            txt_Gain.EditValue = Gain == null ? "" : Gain.ToString();
            txt_MaxExposure.Text = Exposure == null ? "" : Exposure.Max.ToString("F5");
            txt_MaxGain.Text = Gain == null ? "" : Gain.Max.ToString("F5");
            cmb_TriggerSource.Properties.Items.AddRange(TriggerSource == null ? [] : TriggerSource.SupportEnumEntries);
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
                SetControlState(connect: false); // 更新UI为未连接状态
                return;
            }

            // 订阅新相机的事件
            SubscribeNewCameraEvent(newCamera);

            //更新当前相机状态，并同步UI
            currentSelectedCamera = newCamera;
            SetControlState(currentSelectedCamera.IsConnected);
            
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void cmb_Manufacturers_SelectedIndexChanged(object sender, EventArgs e)
    {
        cmb_SnList.SelectedText = "";
        cmb_SnList.Properties.Items.Clear();
        var list = CameraManager.Instance.EnumerateDevices(cmb_Manufacturers.Text);
        cmb_SnList.Properties.Items.AddRange(list);
    }
    /// <summary>
    /// 订阅新相机的FrameGrabedEvent（确保只订阅一次）
    /// </summary>
    private void SubscribeNewCameraEvent(ICamera newCamera)
    {
        if (newCamera == null) return;

        //订阅前先取消一次（防止相机实例被其他地方订阅过）
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
            // 取消订阅
            currentSelectedCamera.FrameGrabedEvent -= UpdateUIImage;
            Console.WriteLine($"取消订阅相机{currentSelectedCamera.SN}的图像事件");
        }
    }

    private void UpdateUIImage(object sender, object img)
    {
        lock (this)
        {
            if (user_ShowDisplay.InvokeRequired)
            {
                user_ShowDisplay.BeginInvoke(
                    new Action<object, object>(UpdateUIImage),
                    sender,  // 传递事件源
                    img      // 传递图像参数
                );
            }
            else
            {
                var bmp = (Bitmap)img;
                user_ShowDisplay.Image = bmp.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb ? new CogImage24PlanarColor(bmp) : new CogImage8Grey(bmp);
            }
        }
    }

    private void DisConnectEvent(object sender, object disconnect)
    {
        Invoke(() =>
        {
            if (cmb_SnList.SelectedText == ((ICamera)sender).SN)
            {
                SetControlState(!(bool)disconnect);
            }
        });

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
    private void cmb_TriggerSource_SelectedIndexChanged(object sender, EventArgs e)
    {
        currentSelectedCamera.SetTriggerSource(cmb_TriggerSource.Text);
    }

    private void btn_Add_Click(object sender, EventArgs e)
    {
        var selectedManufacturer = cmb_Manufacturers.Text;
        var selectedSerial = cmb_SnList.SelectedText.Trim();
        if (string.IsNullOrEmpty(selectedManufacturer) || string.IsNullOrEmpty(selectedSerial))
        {
            MessageBox.Show("请选择厂商和序列号");
            return;
        }

        if (CameraManager.Instance.AddOrUpdateCameraConfig(selectedManufacturer, selectedSerial))
            dgv_CameraConfig.Rows.Add(selectedSerial, "");
    }

    private void btn_Remove_Click(object sender, EventArgs e)
    {
        // 判断是否有选中行
        if (dgv_CameraConfig.SelectedRows.Count == 0)
        {
            //MessageBox.Show("请先选择要移除的相机配置行！");
            return;
        }

        // 获取选中行
        var row = dgv_CameraConfig.SelectedRows[0];
        var sn = row.Cells["col_Sn"].Value?.ToString();

        if (string.IsNullOrEmpty(sn))
        {
            MessageBox.Show("选中的行序列号无效！");
            return;
        }

        // 调用CameraManager移除配置
        if (CameraManager.Instance.RemoveCameraConfig(sn))
        {
            dgv_CameraConfig.Rows.Remove(row);
            //MessageBox.Show($"已移除相机配置：{sn}");
        }
        else
        {
            MessageBox.Show("移除失败，未找到该序列号的配置！");
        }
    }
    private void btn_Connect_Click(object sender, EventArgs e)
    {
        try
        {
            currentSelectedCamera.Open();
            SetControlState(currentSelectedCamera.IsConnected);
            currentSelectedCamera.DisConnetEvent += DisConnectEvent;
        }
        catch (Exception)
        {
            //MessageBox.Show($"打开相机失败,错误码{errCode}," + exception, "", MessageBoxButtons.OK);
        }
    }
    private void btn_DisConnect_Click(object sender, EventArgs e)
    {
        try
        {
            currentSelectedCamera.DisConnet();
            SetControlState(currentSelectedCamera.IsConnected);
            currentSelectedCamera.DisConnetEvent -= DisConnectEvent;
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
    private void btn_Continuous_Click(object sender, EventArgs e)
    {
        if (btn_Continuous.Text == "连续采集")
        {
            currentSelectedCamera.ContinuousGrab();
            btn_Continuous.Text = "停止采集";
            btn_DisConnect.Enabled = false;
        }
        else
        {
            currentSelectedCamera.StopContinuousGrab();
            btn_Continuous.Text = "连续采集";
            btn_DisConnect.Enabled = true;
        }
    }
    private void chk_HardTrigger_CheckedChanged(object sender, EventArgs e)
    {
        cmb_TriggerSource.Visible = chk_HardTrigger.Checked;
        if (chk_HardTrigger.Checked)
            cmb_TriggerSource.SelectedItem = TriggerSource.CurEnumEntry;
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            // 2. 取消相机事件订阅，避免残留引用
            UnsubscribeCurrentCameraEvent();

            // 3. 释放设计器组件（默认逻辑）
            if (components != null)
                components.Dispose();
        }
        base.Dispose(disposing);
    }
}