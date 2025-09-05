namespace MainForm.Forms
{
    public partial class Frm_Main : DevExpress.XtraEditors.XtraForm
    {
        public Frm_Main()
        {
            InitializeComponent();
        }

        private void btn_HardwareCamera_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var frm = new Frm_Camera2D();
            frm.Show();
        }
    }
}