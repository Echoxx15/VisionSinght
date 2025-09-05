namespace MainForm.Forms
{
      sealed partial class Frm_Camera2D
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            split_Display = new DevExpress.XtraEditors.SplitContainerControl();
            tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            panelControl1 = new DevExpress.XtraEditors.PanelControl();
            btn_Continuous = new DevExpress.XtraEditors.SimpleButton();
            btn_TriggerOnce = new DevExpress.XtraEditors.SimpleButton();
            btn_Close = new DevExpress.XtraEditors.SimpleButton();
            btn_Connect = new DevExpress.XtraEditors.SimpleButton();
            panelControl3 = new DevExpress.XtraEditors.PanelControl();
            panelControl2 = new DevExpress.XtraEditors.PanelControl();
            cmb_TriggerSource = new DevExpress.XtraEditors.ComboBoxEdit();
            txt_MaxExposure = new DevExpress.XtraEditors.LabelControl();
            labelControl9 = new DevExpress.XtraEditors.LabelControl();
            labelControl8 = new DevExpress.XtraEditors.LabelControl();
            labelControl7 = new DevExpress.XtraEditors.LabelControl();
            chk_Hard = new DevExpress.XtraEditors.CheckEdit();
            txt_MaxGain = new DevExpress.XtraEditors.LabelControl();
            labelControl5 = new DevExpress.XtraEditors.LabelControl();
            txt_Exposure = new DevExpress.XtraEditors.SpinEdit();
            labelControl4 = new DevExpress.XtraEditors.LabelControl();
            txt_Gain = new DevExpress.XtraEditors.SpinEdit();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            labelControl3 = new DevExpress.XtraEditors.LabelControl();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            txt_Sn = new DevExpress.XtraEditors.TextEdit();
            pnl_DevInfo = new DevExpress.XtraEditors.PanelControl();
            labelControl10 = new DevExpress.XtraEditors.LabelControl();
            cmb_SnList = new DevExpress.XtraEditors.ComboBoxEdit();
            labelControl6 = new DevExpress.XtraEditors.LabelControl();
            cmb_Manufacturers = new DevExpress.XtraEditors.ComboBoxEdit();
            tabPanel_Main = new DevExpress.Utils.Layout.TablePanel();
            ((System.ComponentModel.ISupportInitialize)split_Display).BeginInit();
            ((System.ComponentModel.ISupportInitialize)split_Display.Panel1).BeginInit();
            split_Display.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)split_Display.Panel2).BeginInit();
            split_Display.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tablePanel1).BeginInit();
            tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelControl1).BeginInit();
            panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)panelControl3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)panelControl2).BeginInit();
            panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmb_TriggerSource.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chk_Hard.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txt_Exposure.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txt_Gain.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txt_Sn.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pnl_DevInfo).BeginInit();
            pnl_DevInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cmb_SnList.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cmb_Manufacturers.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tabPanel_Main).BeginInit();
            tabPanel_Main.SuspendLayout();
            SuspendLayout();
            // 
            // split_Display
            // 
            split_Display.Appearance.BackColor = System.Drawing.Color.SlateBlue;
            split_Display.Appearance.BorderColor = System.Drawing.Color.SlateBlue;
            split_Display.Appearance.Options.UseBackColor = true;
            split_Display.Appearance.Options.UseBorderColor = true;
            split_Display.Dock = System.Windows.Forms.DockStyle.Fill;
            split_Display.Location = new System.Drawing.Point(23, 24);
            split_Display.Margin = new System.Windows.Forms.Padding(4);
            split_Display.Name = "split_Display";
            // 
            // split_Display.Panel1
            // 
            split_Display.Panel1.Appearance.BackColor = System.Drawing.Color.SlateBlue;
            split_Display.Panel1.Appearance.Options.UseBackColor = true;
            split_Display.Panel1.Controls.Add(tablePanel1);
            split_Display.Panel1.Text = "Panel1";
            // 
            // split_Display.Panel2
            // 
            split_Display.Panel2.Appearance.BackColor = System.Drawing.Color.SlateBlue;
            split_Display.Panel2.Appearance.Options.UseBackColor = true;
            split_Display.Panel2.Text = "Panel2";
            split_Display.Size = new System.Drawing.Size(1678, 911);
            split_Display.SplitterPosition = 759;
            split_Display.TabIndex = 2;
            // 
            // tablePanel1
            // 
            tablePanel1.AutoSize = true;
            tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] { new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F) });
            tablePanel1.Controls.Add(panelControl1);
            tablePanel1.Controls.Add(panelControl3);
            tablePanel1.Controls.Add(panelControl2);
            tablePanel1.Controls.Add(pnl_DevInfo);
            tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tablePanel1.Location = new System.Drawing.Point(0, 0);
            tablePanel1.Margin = new System.Windows.Forms.Padding(4);
            tablePanel1.Name = "tablePanel1";
            tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] { new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 130.6667F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 206.6669F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 224.666F), new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F) });
            tablePanel1.Size = new System.Drawing.Size(759, 911);
            tablePanel1.TabIndex = 0;
            tablePanel1.UseSkinIndents = true;
            // 
            // panelControl1
            // 
            panelControl1.Appearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            panelControl1.Appearance.Options.UseBackColor = true;
            panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            tablePanel1.SetColumn(panelControl1, 0);
            panelControl1.Controls.Add(btn_Continuous);
            panelControl1.Controls.Add(btn_TriggerOnce);
            panelControl1.Controls.Add(btn_Close);
            panelControl1.Controls.Add(btn_Connect);
            panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControl1.Location = new System.Drawing.Point(20, 582);
            panelControl1.Margin = new System.Windows.Forms.Padding(4);
            panelControl1.Name = "panelControl1";
            tablePanel1.SetRow(panelControl1, 3);
            panelControl1.Size = new System.Drawing.Size(719, 309);
            panelControl1.TabIndex = 14;
            // 
            // btn_Continuous
            // 
            btn_Continuous.Appearance.BackColor = System.Drawing.Color.SlateBlue;
            btn_Continuous.Appearance.Options.UseBackColor = true;
            btn_Continuous.Cursor = System.Windows.Forms.Cursors.Hand;
            btn_Continuous.Location = new System.Drawing.Point(538, 28);
            btn_Continuous.Margin = new System.Windows.Forms.Padding(4);
            btn_Continuous.Name = "btn_Continuous";
            btn_Continuous.Size = new System.Drawing.Size(137, 45);
            btn_Continuous.TabIndex = 3;
            btn_Continuous.Text = "连续采集";
            btn_Continuous.Click += btn_Continue_Click;
            // 
            // btn_TriggerOnce
            // 
            btn_TriggerOnce.Appearance.BackColor = System.Drawing.Color.SlateBlue;
            btn_TriggerOnce.Appearance.Options.UseBackColor = true;
            btn_TriggerOnce.Cursor = System.Windows.Forms.Cursors.Hand;
            btn_TriggerOnce.Location = new System.Drawing.Point(374, 28);
            btn_TriggerOnce.Margin = new System.Windows.Forms.Padding(4);
            btn_TriggerOnce.Name = "btn_TriggerOnce";
            btn_TriggerOnce.Size = new System.Drawing.Size(137, 45);
            btn_TriggerOnce.TabIndex = 2;
            btn_TriggerOnce.Text = "软触发一次";
            btn_TriggerOnce.Click += btn_TriggerOnce_Click;
            // 
            // btn_Close
            // 
            btn_Close.Appearance.BackColor = System.Drawing.Color.SlateBlue;
            btn_Close.Appearance.Options.UseBackColor = true;
            btn_Close.Cursor = System.Windows.Forms.Cursors.Hand;
            btn_Close.Location = new System.Drawing.Point(192, 28);
            btn_Close.Margin = new System.Windows.Forms.Padding(4);
            btn_Close.Name = "btn_Close";
            btn_Close.Size = new System.Drawing.Size(137, 45);
            btn_Close.TabIndex = 1;
            btn_Close.Text = "断开";
            btn_Close.Click += btn_Close_Click;
            // 
            // btn_Connect
            // 
            btn_Connect.Appearance.BackColor = System.Drawing.Color.SlateBlue;
            btn_Connect.Appearance.Options.UseBackColor = true;
            btn_Connect.Cursor = System.Windows.Forms.Cursors.Hand;
            btn_Connect.Location = new System.Drawing.Point(15, 28);
            btn_Connect.Margin = new System.Windows.Forms.Padding(4);
            btn_Connect.Name = "btn_Connect";
            btn_Connect.Size = new System.Drawing.Size(137, 45);
            btn_Connect.TabIndex = 0;
            btn_Connect.Text = "连接";
            btn_Connect.Click += btn_Connect_Click;
            // 
            // panelControl3
            // 
            panelControl3.Appearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            panelControl3.Appearance.Options.UseBackColor = true;
            panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            tablePanel1.SetColumn(panelControl3, 0);
            panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControl3.Location = new System.Drawing.Point(20, 150);
            panelControl3.Margin = new System.Windows.Forms.Padding(4);
            panelControl3.Name = "panelControl3";
            tablePanel1.SetRow(panelControl3, 1);
            panelControl3.Size = new System.Drawing.Size(719, 199);
            panelControl3.TabIndex = 25;
            // 
            // panelControl2
            // 
            panelControl2.Appearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            panelControl2.Appearance.Options.UseBackColor = true;
            panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            tablePanel1.SetColumn(panelControl2, 0);
            panelControl2.Controls.Add(cmb_TriggerSource);
            panelControl2.Controls.Add(txt_MaxExposure);
            panelControl2.Controls.Add(labelControl9);
            panelControl2.Controls.Add(labelControl8);
            panelControl2.Controls.Add(labelControl7);
            panelControl2.Controls.Add(chk_Hard);
            panelControl2.Controls.Add(txt_MaxGain);
            panelControl2.Controls.Add(labelControl5);
            panelControl2.Controls.Add(txt_Exposure);
            panelControl2.Controls.Add(labelControl4);
            panelControl2.Controls.Add(txt_Gain);
            panelControl2.Controls.Add(labelControl1);
            panelControl2.Controls.Add(labelControl3);
            panelControl2.Controls.Add(labelControl2);
            panelControl2.Controls.Add(txt_Sn);
            panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControl2.Location = new System.Drawing.Point(20, 357);
            panelControl2.Margin = new System.Windows.Forms.Padding(4);
            panelControl2.Name = "panelControl2";
            tablePanel1.SetRow(panelControl2, 2);
            panelControl2.Size = new System.Drawing.Size(719, 217);
            panelControl2.TabIndex = 13;
            // 
            // cmb_TriggerSource
            // 
            cmb_TriggerSource.Cursor = System.Windows.Forms.Cursors.Hand;
            cmb_TriggerSource.Location = new System.Drawing.Point(245, 79);
            cmb_TriggerSource.Margin = new System.Windows.Forms.Padding(4);
            cmb_TriggerSource.Name = "cmb_TriggerSource";
            cmb_TriggerSource.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            cmb_TriggerSource.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            cmb_TriggerSource.Properties.Appearance.Options.UseBackColor = true;
            cmb_TriggerSource.Properties.Appearance.Options.UseForeColor = true;
            cmb_TriggerSource.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cmb_TriggerSource.Properties.DropDownRows = 3;
            cmb_TriggerSource.Properties.PopupSizeable = true;
            cmb_TriggerSource.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cmb_TriggerSource.Size = new System.Drawing.Size(171, 28);
            cmb_TriggerSource.TabIndex = 19;
            cmb_TriggerSource.SelectedIndexChanged += cmb_HardSource_SelectedIndexChanged;
            // 
            // txt_MaxExposure
            // 
            txt_MaxExposure.Appearance.ForeColor = System.Drawing.Color.White;
            txt_MaxExposure.Appearance.Options.UseForeColor = true;
            txt_MaxExposure.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            txt_MaxExposure.Location = new System.Drawing.Point(541, 157);
            txt_MaxExposure.Margin = new System.Windows.Forms.Padding(4);
            txt_MaxExposure.Name = "txt_MaxExposure";
            txt_MaxExposure.Size = new System.Drawing.Size(10, 22);
            txt_MaxExposure.TabIndex = 18;
            txt_MaxExposure.Text = "0";
            // 
            // labelControl9
            // 
            labelControl9.Appearance.ForeColor = System.Drawing.Color.White;
            labelControl9.Appearance.Options.UseForeColor = true;
            labelControl9.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            labelControl9.Location = new System.Drawing.Point(425, 157);
            labelControl9.Margin = new System.Windows.Forms.Padding(4);
            labelControl9.Name = "labelControl9";
            labelControl9.Size = new System.Drawing.Size(78, 22);
            labelControl9.TabIndex = 17;
            labelControl9.Text = "最大曝光:";
            // 
            // labelControl8
            // 
            labelControl8.Appearance.ForeColor = System.Drawing.Color.White;
            labelControl8.Appearance.Options.UseForeColor = true;
            labelControl8.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            labelControl8.Location = new System.Drawing.Point(286, 159);
            labelControl8.Margin = new System.Windows.Forms.Padding(4);
            labelControl8.Name = "labelControl8";
            labelControl8.Size = new System.Drawing.Size(18, 22);
            labelControl8.TabIndex = 16;
            labelControl8.Text = "us";
            // 
            // labelControl7
            // 
            labelControl7.Appearance.ForeColor = System.Drawing.Color.White;
            labelControl7.Appearance.Options.UseForeColor = true;
            labelControl7.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            labelControl7.Location = new System.Drawing.Point(286, 217);
            labelControl7.Margin = new System.Windows.Forms.Padding(4);
            labelControl7.Name = "labelControl7";
            labelControl7.Size = new System.Drawing.Size(20, 22);
            labelControl7.TabIndex = 15;
            labelControl7.Text = "db";
            // 
            // chk_Hard
            // 
            chk_Hard.Location = new System.Drawing.Point(125, 80);
            chk_Hard.Margin = new System.Windows.Forms.Padding(4);
            chk_Hard.Name = "chk_Hard";
            chk_Hard.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            chk_Hard.Properties.Appearance.Options.UseForeColor = true;
            chk_Hard.Properties.Caption = "硬触发";
            chk_Hard.Size = new System.Drawing.Size(137, 27);
            chk_Hard.TabIndex = 14;
            chk_Hard.CheckedChanged += chk_Hard_CheckedChanged;
            // 
            // txt_MaxGain
            // 
            txt_MaxGain.Appearance.ForeColor = System.Drawing.Color.White;
            txt_MaxGain.Appearance.Options.UseForeColor = true;
            txt_MaxGain.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            txt_MaxGain.Location = new System.Drawing.Point(541, 216);
            txt_MaxGain.Margin = new System.Windows.Forms.Padding(4);
            txt_MaxGain.Name = "txt_MaxGain";
            txt_MaxGain.Size = new System.Drawing.Size(10, 22);
            txt_MaxGain.TabIndex = 12;
            txt_MaxGain.Text = "0";
            // 
            // labelControl5
            // 
            labelControl5.Appearance.ForeColor = System.Drawing.Color.White;
            labelControl5.Appearance.Options.UseForeColor = true;
            labelControl5.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            labelControl5.Location = new System.Drawing.Point(425, 216);
            labelControl5.Margin = new System.Windows.Forms.Padding(4);
            labelControl5.Name = "labelControl5";
            labelControl5.Size = new System.Drawing.Size(78, 22);
            labelControl5.TabIndex = 11;
            labelControl5.Text = "最大增益:";
            // 
            // txt_Exposure
            // 
            txt_Exposure.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
            txt_Exposure.Location = new System.Drawing.Point(125, 156);
            txt_Exposure.Margin = new System.Windows.Forms.Padding(4);
            txt_Exposure.Name = "txt_Exposure";
            txt_Exposure.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            txt_Exposure.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            txt_Exposure.Properties.Appearance.Options.UseBackColor = true;
            txt_Exposure.Properties.Appearance.Options.UseForeColor = true;
            txt_Exposure.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            txt_Exposure.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            txt_Exposure.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            txt_Exposure.Properties.MaskSettings.Set("mask", "f3");
            txt_Exposure.Properties.MaxValue = new decimal(new int[] { 10000000, 0, 0, 0 });
            txt_Exposure.Size = new System.Drawing.Size(150, 28);
            txt_Exposure.TabIndex = 7;
            txt_Exposure.EditValueChanged += txt_Exposure_EditValueChanged;
            // 
            // labelControl4
            // 
            labelControl4.Appearance.ForeColor = System.Drawing.Color.White;
            labelControl4.Appearance.Options.UseForeColor = true;
            labelControl4.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            labelControl4.Location = new System.Drawing.Point(48, 216);
            labelControl4.Margin = new System.Windows.Forms.Padding(4);
            labelControl4.Name = "labelControl4";
            labelControl4.Size = new System.Drawing.Size(36, 22);
            labelControl4.TabIndex = 10;
            labelControl4.Text = "增益";
            // 
            // txt_Gain
            // 
            txt_Gain.EditValue = new decimal(new int[] { 0, 0, 0, 0 });
            txt_Gain.Location = new System.Drawing.Point(125, 215);
            txt_Gain.Margin = new System.Windows.Forms.Padding(4);
            txt_Gain.Name = "txt_Gain";
            txt_Gain.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            txt_Gain.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            txt_Gain.Properties.Appearance.Options.UseBackColor = true;
            txt_Gain.Properties.Appearance.Options.UseForeColor = true;
            txt_Gain.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            txt_Gain.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            txt_Gain.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            txt_Gain.Properties.MaskSettings.Set("mask", "f3");
            txt_Gain.Properties.MaxValue = new decimal(new int[] { 100, 0, 0, 0 });
            txt_Gain.Size = new System.Drawing.Size(150, 28);
            txt_Gain.TabIndex = 9;
            txt_Gain.EditValueChanged += txt_Gain_EditValueChanged;
            // 
            // labelControl1
            // 
            labelControl1.Appearance.ForeColor = System.Drawing.Color.White;
            labelControl1.Appearance.Options.UseForeColor = true;
            labelControl1.Location = new System.Drawing.Point(10, 21);
            labelControl1.Margin = new System.Windows.Forms.Padding(4);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new System.Drawing.Size(54, 22);
            labelControl1.TabIndex = 1;
            labelControl1.Text = "序列号";
            // 
            // labelControl3
            // 
            labelControl3.Appearance.ForeColor = System.Drawing.Color.White;
            labelControl3.Appearance.Options.UseForeColor = true;
            labelControl3.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            labelControl3.Location = new System.Drawing.Point(48, 157);
            labelControl3.Margin = new System.Windows.Forms.Padding(4);
            labelControl3.Name = "labelControl3";
            labelControl3.Size = new System.Drawing.Size(36, 22);
            labelControl3.TabIndex = 8;
            labelControl3.Text = "曝光";
            // 
            // labelControl2
            // 
            labelControl2.Appearance.ForeColor = System.Drawing.Color.White;
            labelControl2.Appearance.Options.UseForeColor = true;
            labelControl2.LineStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            labelControl2.Location = new System.Drawing.Point(10, 85);
            labelControl2.Margin = new System.Windows.Forms.Padding(4);
            labelControl2.Name = "labelControl2";
            labelControl2.Size = new System.Drawing.Size(72, 22);
            labelControl2.TabIndex = 3;
            labelControl2.Text = "触发模式";
            // 
            // txt_Sn
            // 
            txt_Sn.Cursor = System.Windows.Forms.Cursors.Hand;
            txt_Sn.Location = new System.Drawing.Point(125, 16);
            txt_Sn.Margin = new System.Windows.Forms.Padding(4);
            txt_Sn.Name = "txt_Sn";
            txt_Sn.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            txt_Sn.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            txt_Sn.Properties.Appearance.Options.UseBackColor = true;
            txt_Sn.Properties.Appearance.Options.UseForeColor = true;
            txt_Sn.Size = new System.Drawing.Size(291, 28);
            txt_Sn.TabIndex = 4;
            // 
            // pnl_DevInfo
            // 
            pnl_DevInfo.Appearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            pnl_DevInfo.Appearance.Options.UseBackColor = true;
            pnl_DevInfo.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            tablePanel1.SetColumn(pnl_DevInfo, 0);
            pnl_DevInfo.Controls.Add(labelControl10);
            pnl_DevInfo.Controls.Add(cmb_SnList);
            pnl_DevInfo.Controls.Add(labelControl6);
            pnl_DevInfo.Controls.Add(cmb_Manufacturers);
            pnl_DevInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            pnl_DevInfo.Location = new System.Drawing.Point(20, 19);
            pnl_DevInfo.Margin = new System.Windows.Forms.Padding(4);
            pnl_DevInfo.Name = "pnl_DevInfo";
            tablePanel1.SetRow(pnl_DevInfo, 0);
            pnl_DevInfo.Size = new System.Drawing.Size(719, 123);
            pnl_DevInfo.TabIndex = 3;
            // 
            // labelControl10
            // 
            labelControl10.Appearance.ForeColor = System.Drawing.Color.White;
            labelControl10.Appearance.Options.UseForeColor = true;
            labelControl10.Location = new System.Drawing.Point(15, 78);
            labelControl10.Margin = new System.Windows.Forms.Padding(4);
            labelControl10.Name = "labelControl10";
            labelControl10.Size = new System.Drawing.Size(72, 22);
            labelControl10.TabIndex = 7;
            labelControl10.Text = "相机列表";
            // 
            // cmb_SnList
            // 
            cmb_SnList.Cursor = System.Windows.Forms.Cursors.Hand;
            cmb_SnList.Location = new System.Drawing.Point(125, 74);
            cmb_SnList.Margin = new System.Windows.Forms.Padding(4);
            cmb_SnList.Name = "cmb_SnList";
            cmb_SnList.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            cmb_SnList.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            cmb_SnList.Properties.Appearance.Options.UseBackColor = true;
            cmb_SnList.Properties.Appearance.Options.UseForeColor = true;
            cmb_SnList.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cmb_SnList.Properties.DropDownRows = 20;
            cmb_SnList.Properties.PopupSizeable = true;
            cmb_SnList.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cmb_SnList.Size = new System.Drawing.Size(291, 28);
            cmb_SnList.TabIndex = 8;
            cmb_SnList.SelectedIndexChanged += cmb_SnList_SelectedIndexChanged;
            // 
            // labelControl6
            // 
            labelControl6.Appearance.ForeColor = System.Drawing.Color.White;
            labelControl6.Appearance.Options.UseForeColor = true;
            labelControl6.Location = new System.Drawing.Point(15, 24);
            labelControl6.Margin = new System.Windows.Forms.Padding(4);
            labelControl6.Name = "labelControl6";
            labelControl6.Size = new System.Drawing.Size(72, 22);
            labelControl6.TabIndex = 5;
            labelControl6.Text = "相机品牌";
            // 
            // cmb_Manufacturers
            // 
            cmb_Manufacturers.Cursor = System.Windows.Forms.Cursors.Hand;
            cmb_Manufacturers.Location = new System.Drawing.Point(125, 20);
            cmb_Manufacturers.Margin = new System.Windows.Forms.Padding(4);
            cmb_Manufacturers.Name = "cmb_Manufacturers";
            cmb_Manufacturers.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            cmb_Manufacturers.Properties.Appearance.ForeColor = System.Drawing.Color.White;
            cmb_Manufacturers.Properties.Appearance.Options.UseBackColor = true;
            cmb_Manufacturers.Properties.Appearance.Options.UseForeColor = true;
            cmb_Manufacturers.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            cmb_Manufacturers.Properties.DropDownRows = 20;
            cmb_Manufacturers.Properties.PopupSizeable = true;
            cmb_Manufacturers.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cmb_Manufacturers.Size = new System.Drawing.Size(291, 28);
            cmb_Manufacturers.TabIndex = 6;
            cmb_Manufacturers.SelectedIndexChanged += cmb_Manufacturers_SelectedIndexChanged;
            // 
            // tabPanel_Main
            // 
            tabPanel_Main.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] { new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F) });
            tabPanel_Main.Controls.Add(split_Display);
            tabPanel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            tabPanel_Main.Location = new System.Drawing.Point(0, 0);
            tabPanel_Main.Margin = new System.Windows.Forms.Padding(4);
            tabPanel_Main.Name = "tabPanel_Main";
            tabPanel_Main.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] { new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 602.0003F) });
            tabPanel_Main.Size = new System.Drawing.Size(1725, 960);
            tabPanel_Main.TabIndex = 2;
            tabPanel_Main.UseSkinIndents = true;
            // 
            // Frm_Camera2D
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.SlateBlue;
            ClientSize = new System.Drawing.Size(1725, 960);
            Controls.Add(tabPanel_Main);
            DoubleBuffered = true;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4);
            Name = "Frm_Camera2D";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "面阵相机测试界面";
            Load += Frm_Camera2D_Load;
            ((System.ComponentModel.ISupportInitialize)split_Display.Panel1).EndInit();
            split_Display.Panel1.ResumeLayout(false);
            split_Display.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)split_Display.Panel2).EndInit();
            ((System.ComponentModel.ISupportInitialize)split_Display).EndInit();
            split_Display.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)tablePanel1).EndInit();
            tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)panelControl1).EndInit();
            panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)panelControl3).EndInit();
            ((System.ComponentModel.ISupportInitialize)panelControl2).EndInit();
            panelControl2.ResumeLayout(false);
            panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cmb_TriggerSource.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)chk_Hard.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txt_Exposure.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txt_Gain.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txt_Sn.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)pnl_DevInfo).EndInit();
            pnl_DevInfo.ResumeLayout(false);
            pnl_DevInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cmb_SnList.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)cmb_Manufacturers.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)tabPanel_Main).EndInit();
            tabPanel_Main.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SplitContainerControl split_Display;
    private DevExpress.Utils.Layout.TablePanel tablePanel1;
    private DevExpress.XtraEditors.PanelControl panelControl1;
    private DevExpress.XtraEditors.SimpleButton btn_Continuous;
    private DevExpress.XtraEditors.SimpleButton btn_TriggerOnce;
    private DevExpress.XtraEditors.SimpleButton btn_Close;
    private DevExpress.XtraEditors.SimpleButton btn_Connect;
    private DevExpress.XtraEditors.PanelControl panelControl2;
    private DevExpress.XtraEditors.LabelControl txt_MaxExposure;
    private DevExpress.XtraEditors.LabelControl labelControl9;
    private DevExpress.XtraEditors.LabelControl labelControl8;
    private DevExpress.XtraEditors.LabelControl labelControl7;
    private DevExpress.XtraEditors.CheckEdit chk_Hard;
    private DevExpress.XtraEditors.LabelControl txt_MaxGain;
    private DevExpress.XtraEditors.LabelControl labelControl5;
    private DevExpress.XtraEditors.SpinEdit txt_Exposure;
    private DevExpress.XtraEditors.LabelControl labelControl4;
    private DevExpress.XtraEditors.SpinEdit txt_Gain;
    private DevExpress.XtraEditors.LabelControl labelControl1;
    private DevExpress.XtraEditors.LabelControl labelControl3;
    private DevExpress.XtraEditors.LabelControl labelControl2;
    private DevExpress.XtraEditors.PanelControl pnl_DevInfo;
    private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.Utils.Layout.TablePanel tabPanel_Main;
        private DevExpress.XtraEditors.ComboBoxEdit cmb_TriggerSource;
    private DevExpress.XtraEditors.TextEdit txt_Sn;
    private DevExpress.XtraEditors.LabelControl labelControl10;
    private DevExpress.XtraEditors.ComboBoxEdit cmb_SnList;
    private DevExpress.XtraEditors.LabelControl labelControl6;
    private DevExpress.XtraEditors.ComboBoxEdit cmb_Manufacturers;
  }
}