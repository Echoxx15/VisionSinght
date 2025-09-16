namespace VisionCore.PluginBase
{
    partial class Frm_ToolBase
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
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btn_Run = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Confirm = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Cancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Appearance.BackColor = System.Drawing.Color.SlateBlue;
            this.tablePanel1.Appearance.Options.UseBackColor = true;
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F)});
            this.tablePanel1.Controls.Add(this.panelControl1);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 668.0001F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(1050, 764);
            this.tablePanel1.TabIndex = 0;
            this.tablePanel1.UseSkinIndents = true;
            // 
            // panelControl1
            // 
            this.panelControl1.Appearance.BackColor = System.Drawing.Color.SlateBlue;
            this.panelControl1.Appearance.Options.UseBackColor = true;
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.tablePanel1.SetColumn(this.panelControl1, 0);
            this.panelControl1.Controls.Add(this.btn_Cancel);
            this.panelControl1.Controls.Add(this.btn_Confirm);
            this.panelControl1.Controls.Add(this.btn_Run);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(19, 686);
            this.panelControl1.Name = "panelControl1";
            this.tablePanel1.SetRow(this.panelControl1, 1);
            this.panelControl1.Size = new System.Drawing.Size(1012, 59);
            this.panelControl1.TabIndex = 0;
            // 
            // btn_Run
            // 
            this.btn_Run.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_Run.Appearance.BackColor = System.Drawing.Color.Blue;
            this.btn_Run.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.btn_Run.Appearance.Options.UseBackColor = true;
            this.btn_Run.Appearance.Options.UseFont = true;
            this.btn_Run.Location = new System.Drawing.Point(621, 7);
            this.btn_Run.Name = "btn_Run";
            this.btn_Run.Size = new System.Drawing.Size(109, 44);
            this.btn_Run.TabIndex = 0;
            this.btn_Run.Text = "执行";
            // 
            // btn_Confirm
            // 
            this.btn_Confirm.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_Confirm.Appearance.BackColor = System.Drawing.Color.Lime;
            this.btn_Confirm.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.btn_Confirm.Appearance.Options.UseBackColor = true;
            this.btn_Confirm.Appearance.Options.UseFont = true;
            this.btn_Confirm.Location = new System.Drawing.Point(760, 7);
            this.btn_Confirm.Name = "btn_Confirm";
            this.btn_Confirm.Size = new System.Drawing.Size(109, 44);
            this.btn_Confirm.TabIndex = 1;
            this.btn_Confirm.Text = "确定";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btn_Cancel.Appearance.BackColor = System.Drawing.Color.Red;
            this.btn_Cancel.Appearance.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.btn_Cancel.Appearance.Options.UseBackColor = true;
            this.btn_Cancel.Appearance.Options.UseFont = true;
            this.btn_Cancel.Location = new System.Drawing.Point(891, 7);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(109, 44);
            this.btn_Cancel.TabIndex = 2;
            this.btn_Cancel.Text = "取消";
            // 
            // Frm_ToolBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 764);
            this.Controls.Add(this.tablePanel1);
            this.Name = "Frm_ToolBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_ToolBase";
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btn_Cancel;
        private DevExpress.XtraEditors.SimpleButton btn_Confirm;
        private DevExpress.XtraEditors.SimpleButton btn_Run;
    }
}