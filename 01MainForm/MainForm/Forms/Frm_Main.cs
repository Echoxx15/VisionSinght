using System;
using System.Windows.Forms;
using HardwareCameraNet;

namespace MainForm.Forms;

public partial class Frm_Main : DevExpress.XtraEditors.XtraForm
{
    public Frm_Main()
    {
        InitializeComponent();
    }

    private Frm_Splash splash;

    private void Frm_Main_Load(object sender, System.EventArgs e)
    {
        // 显示加载界面
        splash = new Frm_Splash();
        splash.Show();
        splash.Refresh();

        // 同步加载资源
        splash.SetProgress(10, "正在加载相机插件...");
        var _ = HardwareCameraNet.DeviceFactory.Instance;

        splash.SetProgress(40, "正在加载硬件配置...");
        // TODO: 加载其他硬件相关配置
        // 例如：HardwareConfigManager.Instance.Load();

        splash.SetProgress(60, "正在打开相机硬件...");
        foreach (var config in HardwareCameraNet.DeviceFactory.Instance.GetAllUserConfigs())
        {
            splash.SetProgress(70, $"正在打开相机：{config.SerialNumber}");
            // TODO: 打开相机硬件
            // CameraManager.Instance.CreateCamera(config.PluginTypeName, config.SerialNumber)?.Open();
        }

        splash.SetProgress(80, "正在加载解决方案...");
        // TODO: 加载解决方案相关资源
        // SolutionManager.Instance.Load();

        splash.SetProgress(100, "加载完成");
        splash.Close();
        WindowState = FormWindowState.Maximized;
        DevExpress.XtraEditors.WindowsFormsSettings.ForceDirectXPaint();
        DevExpress.XtraEditors.WindowsFormsSettings.SetDPIAware();
        ShowInTaskbar = true;
    }

    private void btn_HardwareCamera_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        var frm = new Frm_Camera2D();
        frm.Show();
    }

    private void Frm_Main_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
    {
        DeviceFactory.Instance.UnInitialize();
    }
}