namespace VisionCore.Frm_Solution;

/// <summary>
/// 工具栏
/// </summary>
public partial class TreeToolBar : DevExpress.XtraEditors.XtraUserControl
{
    public TreeToolBar()
    {
        InitializeComponent();
        acc_ToolBar.AllowDrop = true;
    }

}