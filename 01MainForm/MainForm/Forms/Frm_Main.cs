using HardwareCameraNet;

namespace MainForm.Forms
{
    public partial class Frm_Main : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Main()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            WindowState = System.Windows.Forms.FormWindowState.Minimized;
        }
        private void Frm_Main_Load(object sender, System.EventArgs e)
        {





            ShowInTaskbar = true;
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }
        private void btn_HardwareCamera_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm = new Frm_Camera2D();
            frm.Show();
        }

        private void Frm_Main_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            CameraManager.Instance.UnInitialize();
        }


    }
}