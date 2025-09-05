using DevExpress.XtraEditors;
using System;

namespace MainForm.Forms
{
  public partial class Frm_Main : XtraForm
  {
    public Frm_Main()
    {
      InitializeComponent();
    }
    private void Frm_Main_Load(object sender, EventArgs e)
    {

    }
    private void btn_HardwareCamera_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
      var frm = new Frm_Camera2D();
      frm.Show();
    }
  }
}