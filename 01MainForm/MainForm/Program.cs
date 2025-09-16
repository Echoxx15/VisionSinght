using System;
using System.Windows.Forms;
using MainForm.Forms;

namespace MainForm;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Frm_Main());
    }
}
