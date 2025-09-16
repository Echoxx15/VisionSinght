namespace VisionCore.Frm_Solution
{
    partial class TreeToolBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.coltreeListColumn0 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.acc_ToolBar = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.accordionControlElement1 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement2 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement3 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement4 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement5 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            ((System.ComponentModel.ISupportInitialize)(this.acc_ToolBar)).BeginInit();
            this.SuspendLayout();
            // 
            // coltreeListColumn0
            // 
            this.coltreeListColumn0.Caption = "coltreeListColumn0";
            this.coltreeListColumn0.FieldName = "treeListColumn0";
            this.coltreeListColumn0.Name = "coltreeListColumn0";
            this.coltreeListColumn0.Visible = true;
            this.coltreeListColumn0.VisibleIndex = 0;
            // 
            // acc_ToolBar
            // 
            this.acc_ToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.acc_ToolBar.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement1,
            this.accordionControlElement2,
            this.accordionControlElement3,
            this.accordionControlElement4,
            this.accordionControlElement5});
            this.acc_ToolBar.Location = new System.Drawing.Point(0, 0);
            this.acc_ToolBar.Name = "acc_ToolBar";
            this.acc_ToolBar.Size = new System.Drawing.Size(247, 795);
            this.acc_ToolBar.TabIndex = 0;
            // 
            // accordionControlElement1
            // 
            this.accordionControlElement1.Name = "accordionControlElement1";
            this.accordionControlElement1.Text = "图像采集";
            // 
            // accordionControlElement2
            // 
            this.accordionControlElement2.Name = "accordionControlElement2";
            this.accordionControlElement2.Text = "视觉模块";
            // 
            // accordionControlElement3
            // 
            this.accordionControlElement3.Name = "accordionControlElement3";
            this.accordionControlElement3.Text = "逻辑工具";
            // 
            // accordionControlElement4
            // 
            this.accordionControlElement4.Name = "accordionControlElement4";
            this.accordionControlElement4.Text = "变量工具";
            // 
            // accordionControlElement5
            // 
            this.accordionControlElement5.Name = "accordionControlElement5";
            this.accordionControlElement5.Text = "通讯工具";
            // 
            // TreeToolBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.acc_ToolBar);
            this.Name = "TreeToolBar";
            this.Size = new System.Drawing.Size(247, 795);
            ((System.ComponentModel.ISupportInitialize)(this.acc_ToolBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraTreeList.Columns.TreeListColumn coltreeListColumn0;
        private DevExpress.XtraBars.Navigation.AccordionControl acc_ToolBar;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement2;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement3;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement4;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement5;
    }
}
