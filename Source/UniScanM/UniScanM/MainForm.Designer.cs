namespace UniScanM
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            this.panelBody = new System.Windows.Forms.Panel();
            this.imageListBottom = new System.Windows.Forms.ImageList(this.components);
            this.imageListSide = new System.Windows.Forms.ImageList(this.components);
            this.panelHeader = new System.Windows.Forms.Panel();
            this.tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelLHeader = new System.Windows.Forms.TableLayoutPanel();
            this.panelClock = new System.Windows.Forms.Panel();
            this.panelCompanyLogo = new System.Windows.Forms.Panel();
            this.panelMHeader = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelInfoHeader = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonReport = new Infragistics.Win.Misc.UltraButton();
            this.buttonSetting = new Infragistics.Win.Misc.UltraButton();
            this.buttonTeach = new Infragistics.Win.Misc.UltraButton();
            this.buttonModelManager = new Infragistics.Win.Misc.UltraButton();
            this.buttonInspection = new Infragistics.Win.Misc.UltraButton();
            this.buttonExit = new Infragistics.Win.Misc.UltraButton();
            this.panelPLCStatus = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.tableLayoutPanelHeader.SuspendLayout();
            this.tableLayoutPanelLHeader.SuspendLayout();
            this.panelMHeader.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBody
            // 
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 76);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(1575, 490);
            this.panelBody.TabIndex = 2;
            // 
            // imageListBottom
            // 
            this.imageListBottom.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListBottom.ImageStream")));
            this.imageListBottom.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListBottom.Images.SetKeyName(0, "Model.png");
            this.imageListBottom.Images.SetKeyName(1, "Monitoring.png");
            this.imageListBottom.Images.SetKeyName(2, "Teach.png");
            this.imageListBottom.Images.SetKeyName(3, "Report2.png");
            this.imageListBottom.Images.SetKeyName(4, "Setting.png");
            this.imageListBottom.Images.SetKeyName(5, "Exit2.png");
            // 
            // imageListSide
            // 
            this.imageListSide.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSide.ImageStream")));
            this.imageListSide.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSide.Images.SetKeyName(0, "Auto2.png");
            this.imageListSide.Images.SetKeyName(1, "st3.png");
            this.imageListSide.Images.SetKeyName(2, "Stop2.png");
            this.imageListSide.Images.SetKeyName(3, "Round.png");
            // 
            // panelHeader
            // 
            this.panelHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelHeader.BackgroundImage")));
            this.panelHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelHeader.Controls.Add(this.tableLayoutPanelHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1575, 76);
            this.panelHeader.TabIndex = 0;
            // 
            // tableLayoutPanelHeader
            // 
            this.tableLayoutPanelHeader.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelHeader.ColumnCount = 3;
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelHeader.Controls.Add(this.tableLayoutPanelLHeader, 0, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.panelMHeader, 1, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.panelInfoHeader, 2, 0);
            this.tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            this.tableLayoutPanelHeader.RowCount = 1;
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.Size = new System.Drawing.Size(1575, 76);
            this.tableLayoutPanelHeader.TabIndex = 2;
            // 
            // tableLayoutPanelLHeader
            // 
            this.tableLayoutPanelLHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tableLayoutPanelLHeader.ColumnCount = 2;
            this.tableLayoutPanelLHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelLHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelLHeader.Controls.Add(this.panelClock, 0, 0);
            this.tableLayoutPanelLHeader.Controls.Add(this.panelCompanyLogo, 0, 0);
            this.tableLayoutPanelLHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLHeader.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLHeader.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelLHeader.Name = "tableLayoutPanelLHeader";
            this.tableLayoutPanelLHeader.RowCount = 1;
            this.tableLayoutPanelLHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLHeader.Size = new System.Drawing.Size(525, 76);
            this.tableLayoutPanelLHeader.TabIndex = 0;
            // 
            // panelClock
            // 
            this.panelClock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelClock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelClock.Location = new System.Drawing.Point(315, 0);
            this.panelClock.Margin = new System.Windows.Forms.Padding(0);
            this.panelClock.Name = "panelClock";
            this.panelClock.Size = new System.Drawing.Size(210, 76);
            this.panelClock.TabIndex = 2;
            // 
            // panelCompanyLogo
            // 
            this.panelCompanyLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelCompanyLogo.BackgroundImage")));
            this.panelCompanyLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelCompanyLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCompanyLogo.Location = new System.Drawing.Point(0, 0);
            this.panelCompanyLogo.Margin = new System.Windows.Forms.Padding(0);
            this.panelCompanyLogo.Name = "panelCompanyLogo";
            this.panelCompanyLogo.Size = new System.Drawing.Size(315, 76);
            this.panelCompanyLogo.TabIndex = 1;
            // 
            // panelMHeader
            // 
            this.panelMHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMHeader.Controls.Add(this.labelTitle);
            this.panelMHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMHeader.Location = new System.Drawing.Point(525, 0);
            this.panelMHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelMHeader.Name = "panelMHeader";
            this.panelMHeader.Size = new System.Drawing.Size(525, 76);
            this.panelMHeader.TabIndex = 3;
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("맑은 고딕", 33F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelTitle.Image = ((System.Drawing.Image)(resources.GetObject("labelTitle.Image")));
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(525, 76);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelInfoHeader
            // 
            this.panelInfoHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfoHeader.Location = new System.Drawing.Point(1050, 0);
            this.panelInfoHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelInfoHeader.Name = "panelInfoHeader";
            this.panelInfoHeader.Size = new System.Drawing.Size(525, 76);
            this.panelInfoHeader.TabIndex = 4;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.Transparent;
            this.panelBottom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelBottom.BackgroundImage")));
            this.panelBottom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBottom.Controls.Add(this.buttonReport);
            this.panelBottom.Controls.Add(this.buttonSetting);
            this.panelBottom.Controls.Add(this.buttonTeach);
            this.panelBottom.Controls.Add(this.buttonModelManager);
            this.panelBottom.Controls.Add(this.buttonInspection);
            this.panelBottom.Controls.Add(this.buttonExit);
            this.panelBottom.Controls.Add(this.panelPLCStatus);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 566);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1575, 114);
            this.panelBottom.TabIndex = 13;
            // 
            // buttonReport
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "맑은 고딕";
            appearance1.FontData.SizeInPoints = 12F;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Bottom";
            appearance1.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonReport.Appearance = appearance1;
            this.buttonReport.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonReport.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonReport.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonReport.Location = new System.Drawing.Point(330, 0);
            this.buttonReport.Margin = new System.Windows.Forms.Padding(0);
            this.buttonReport.Name = "buttonReport";
            this.buttonReport.Size = new System.Drawing.Size(110, 80);
            this.buttonReport.TabIndex = 24;
            this.buttonReport.Text = "Report";
            this.buttonReport.Click += new System.EventHandler(this.PageButton_Click);
            // 
            // buttonSetting
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "맑은 고딕";
            appearance2.FontData.SizeInPoints = 12F;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Bottom";
            appearance2.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonSetting.Appearance = appearance2;
            this.buttonSetting.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSetting.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonSetting.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonSetting.Location = new System.Drawing.Point(1351, 0);
            this.buttonSetting.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(110, 80);
            this.buttonSetting.TabIndex = 22;
            this.buttonSetting.Text = "Setting";
            this.buttonSetting.Click += new System.EventHandler(this.PageButton_Click);
            // 
            // buttonTeach
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "맑은 고딕";
            appearance3.FontData.SizeInPoints = 12F;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Bottom";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonTeach.Appearance = appearance3;
            this.buttonTeach.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonTeach.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonTeach.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonTeach.Location = new System.Drawing.Point(220, 0);
            this.buttonTeach.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTeach.Name = "buttonTeach";
            this.buttonTeach.Size = new System.Drawing.Size(110, 80);
            this.buttonTeach.TabIndex = 23;
            this.buttonTeach.Text = "Teach";
            this.buttonTeach.Click += new System.EventHandler(this.PageButton_Click);
            // 
            // buttonModelManager
            // 
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "맑은 고딕";
            appearance4.FontData.SizeInPoints = 12F;
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance4.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance4.TextHAlignAsString = "Center";
            appearance4.TextVAlignAsString = "Bottom";
            appearance4.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonModelManager.Appearance = appearance4;
            this.buttonModelManager.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonModelManager.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonModelManager.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonModelManager.Location = new System.Drawing.Point(110, 0);
            this.buttonModelManager.Margin = new System.Windows.Forms.Padding(0);
            this.buttonModelManager.Name = "buttonModelManager";
            this.buttonModelManager.Size = new System.Drawing.Size(110, 80);
            this.buttonModelManager.TabIndex = 19;
            this.buttonModelManager.Text = "Model";
            this.buttonModelManager.Visible = false;
            this.buttonModelManager.Click += new System.EventHandler(this.PageButton_Click);
            // 
            // buttonInspection
            // 
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.Name = "맑은 고딕";
            appearance5.FontData.SizeInPoints = 12F;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance5.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Bottom";
            appearance5.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonInspection.Appearance = appearance5;
            this.buttonInspection.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonInspection.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonInspection.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonInspection.Location = new System.Drawing.Point(0, 0);
            this.buttonInspection.Margin = new System.Windows.Forms.Padding(0);
            this.buttonInspection.Name = "buttonInspection";
            this.buttonInspection.Size = new System.Drawing.Size(110, 80);
            this.buttonInspection.TabIndex = 20;
            this.buttonInspection.Text = "Inspection";
            this.buttonInspection.Click += new System.EventHandler(this.PageButton_Click);
            // 
            // buttonExit
            // 
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance6.FontData.BoldAsString = "True";
            appearance6.FontData.Name = "맑은 고딕";
            appearance6.FontData.SizeInPoints = 12F;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance6.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance6.TextHAlignAsString = "Center";
            appearance6.TextVAlignAsString = "Bottom";
            appearance6.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonExit.Appearance = appearance6;
            this.buttonExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExit.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonExit.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonExit.Location = new System.Drawing.Point(1461, 0);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(110, 80);
            this.buttonExit.TabIndex = 21;
            this.buttonExit.Text = "Exit";
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // panelPLCStatus
            // 
            this.panelPLCStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPLCStatus.Location = new System.Drawing.Point(0, 80);
            this.panelPLCStatus.Margin = new System.Windows.Forms.Padding(0);
            this.panelPLCStatus.Name = "panelPLCStatus";
            this.panelPLCStatus.Size = new System.Drawing.Size(1571, 30);
            this.panelPLCStatus.TabIndex = 18;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1575, 680);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.panelHeader.ResumeLayout(false);
            this.tableLayoutPanelHeader.ResumeLayout(false);
            this.tableLayoutPanelLHeader.ResumeLayout(false);
            this.panelMHeader.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLHeader;
        private System.Windows.Forms.Panel panelCompanyLogo;
        private System.Windows.Forms.Panel panelMHeader;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.ImageList imageListBottom;
        private System.Windows.Forms.ImageList imageListSide;
        private System.Windows.Forms.Panel panelInfoHeader;
        private System.Windows.Forms.Panel panelPLCStatus;
        private Infragistics.Win.Misc.UltraButton buttonSetting;
        private Infragistics.Win.Misc.UltraButton buttonExit;
        private Infragistics.Win.Misc.UltraButton buttonTeach;
        private Infragistics.Win.Misc.UltraButton buttonInspection;
        private Infragistics.Win.Misc.UltraButton buttonModelManager;
        private Infragistics.Win.Misc.UltraButton buttonReport;
        private System.Windows.Forms.Panel panelClock;
    }
}