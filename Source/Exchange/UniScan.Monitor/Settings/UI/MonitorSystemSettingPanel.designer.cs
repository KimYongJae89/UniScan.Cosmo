namespace UniScan.Monitor.Settings.Monitor.UI
{
    partial class MonitorSystemSettingPanel
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonDeleteInspectorInfo = new System.Windows.Forms.Button();
            this.buttonAddInspectorInfo = new System.Windows.Forms.Button();
            this.InspectorInfoGridView = new System.Windows.Forms.DataGridView();
            this.columnCam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vncViewerPath = new System.Windows.Forms.TextBox();
            this.labelVncViewerPath = new System.Windows.Forms.Label();
            this.buttonConfig = new System.Windows.Forms.Button();
            this.buttonFov = new System.Windows.Forms.Button();
            this.useLocalMonitorMode = new System.Windows.Forms.CheckBox();
            this.useTestbedStage = new System.Windows.Forms.CheckBox();
            this.useLaserBurner = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.InspectorInfoGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonDeleteInspectorInfo
            // 
            this.buttonDeleteInspectorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteInspectorInfo.Location = new System.Drawing.Point(478, 219);
            this.buttonDeleteInspectorInfo.Name = "buttonDeleteInspectorInfo";
            this.buttonDeleteInspectorInfo.Size = new System.Drawing.Size(56, 26);
            this.buttonDeleteInspectorInfo.TabIndex = 0;
            this.buttonDeleteInspectorInfo.Text = "Delete";
            this.buttonDeleteInspectorInfo.UseVisualStyleBackColor = true;
            this.buttonDeleteInspectorInfo.Click += new System.EventHandler(this.buttonDeleteInspectorInfo_Click);
            // 
            // buttonAddInspectorInfo
            // 
            this.buttonAddInspectorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddInspectorInfo.Location = new System.Drawing.Point(415, 219);
            this.buttonAddInspectorInfo.Name = "buttonAddInspectorInfo";
            this.buttonAddInspectorInfo.Size = new System.Drawing.Size(56, 26);
            this.buttonAddInspectorInfo.TabIndex = 0;
            this.buttonAddInspectorInfo.Text = "Add";
            this.buttonAddInspectorInfo.UseVisualStyleBackColor = true;
            this.buttonAddInspectorInfo.Click += new System.EventHandler(this.buttonAddInspectorInfo_Click);
            // 
            // InspectorInfoGridView
            // 
            this.InspectorInfoGridView.AllowUserToAddRows = false;
            this.InspectorInfoGridView.AllowUserToDeleteRows = false;
            this.InspectorInfoGridView.AllowUserToResizeColumns = false;
            this.InspectorInfoGridView.AllowUserToResizeRows = false;
            this.InspectorInfoGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InspectorInfoGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InspectorInfoGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnCam,
            this.columnClient,
            this.columnPath});
            this.InspectorInfoGridView.Location = new System.Drawing.Point(18, 110);
            this.InspectorInfoGridView.Name = "InspectorInfoGridView";
            this.InspectorInfoGridView.RowHeadersVisible = false;
            this.InspectorInfoGridView.RowTemplate.Height = 23;
            this.InspectorInfoGridView.Size = new System.Drawing.Size(516, 103);
            this.InspectorInfoGridView.TabIndex = 0;
            // 
            // columnCam
            // 
            this.columnCam.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnCam.HeaderText = "Cam";
            this.columnCam.Name = "columnCam";
            this.columnCam.Width = 57;
            // 
            // columnClient
            // 
            this.columnClient.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnClient.HeaderText = "Client";
            this.columnClient.Name = "columnClient";
            this.columnClient.Width = 62;
            // 
            // columnPath
            // 
            this.columnPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnPath.HeaderText = "Path";
            this.columnPath.Name = "columnPath";
            // 
            // vncViewerPath
            // 
            this.vncViewerPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.vncViewerPath.Location = new System.Drawing.Point(381, 17);
            this.vncViewerPath.Name = "vncViewerPath";
            this.vncViewerPath.Size = new System.Drawing.Size(153, 21);
            this.vncViewerPath.TabIndex = 0;
            // 
            // labelVncViewerPath
            // 
            this.labelVncViewerPath.AutoSize = true;
            this.labelVncViewerPath.Location = new System.Drawing.Point(16, 20);
            this.labelVncViewerPath.Name = "labelVncViewerPath";
            this.labelVncViewerPath.Size = new System.Drawing.Size(103, 12);
            this.labelVncViewerPath.TabIndex = 0;
            this.labelVncViewerPath.Text = "VNC Viewer Path";
            // 
            // buttonConfig
            // 
            this.buttonConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfig.Location = new System.Drawing.Point(478, 79);
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.Size = new System.Drawing.Size(56, 26);
            this.buttonConfig.TabIndex = 1;
            this.buttonConfig.Text = "Config";
            this.buttonConfig.UseVisualStyleBackColor = true;
            this.buttonConfig.Click += new System.EventHandler(this.buttonConfig_Click);
            // 
            // buttonFov
            // 
            this.buttonFov.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFov.Location = new System.Drawing.Point(416, 79);
            this.buttonFov.Name = "buttonFov";
            this.buttonFov.Size = new System.Drawing.Size(56, 26);
            this.buttonFov.TabIndex = 2;
            this.buttonFov.Text = "FOV";
            this.buttonFov.UseVisualStyleBackColor = true;
            this.buttonFov.Click += new System.EventHandler(this.buttonFov_Click);
            // 
            // useLocalMonitorMode
            // 
            this.useLocalMonitorMode.AutoSize = true;
            this.useLocalMonitorMode.Location = new System.Drawing.Point(18, 44);
            this.useLocalMonitorMode.Name = "useLocalMonitorMode";
            this.useLocalMonitorMode.Size = new System.Drawing.Size(163, 16);
            this.useLocalMonitorMode.TabIndex = 11;
            this.useLocalMonitorMode.Text = "Use Loacl Monitor Mode";
            this.useLocalMonitorMode.UseVisualStyleBackColor = true;
            // 
            // useTestbedStage
            // 
            this.useTestbedStage.AutoSize = true;
            this.useTestbedStage.Location = new System.Drawing.Point(18, 64);
            this.useTestbedStage.Name = "useTestbedStage";
            this.useTestbedStage.Size = new System.Drawing.Size(132, 16);
            this.useTestbedStage.TabIndex = 11;
            this.useTestbedStage.Text = "Use Testbed Stage";
            this.useTestbedStage.UseVisualStyleBackColor = true;
            // 
            // useLaserBurner
            // 
            this.useLaserBurner.AutoSize = true;
            this.useLaserBurner.Location = new System.Drawing.Point(18, 83);
            this.useLaserBurner.Name = "useLaserBurner";
            this.useLaserBurner.Size = new System.Drawing.Size(123, 16);
            this.useLaserBurner.TabIndex = 11;
            this.useLaserBurner.Text = "Use Laser Burner";
            this.useLaserBurner.UseVisualStyleBackColor = true;
            // 
            // MonitorSystemSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.useLaserBurner);
            this.Controls.Add(this.useTestbedStage);
            this.Controls.Add(this.useLocalMonitorMode);
            this.Controls.Add(this.buttonFov);
            this.Controls.Add(this.buttonConfig);
            this.Controls.Add(this.vncViewerPath);
            this.Controls.Add(this.labelVncViewerPath);
            this.Controls.Add(this.buttonDeleteInspectorInfo);
            this.Controls.Add(this.buttonAddInspectorInfo);
            this.Controls.Add(this.InspectorInfoGridView);
            this.Name = "MonitorSystemSettingPanel";
            this.Size = new System.Drawing.Size(541, 251);
            ((System.ComponentModel.ISupportInitialize)(this.InspectorInfoGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button buttonDeleteInspectorInfo;
        private System.Windows.Forms.Button buttonAddInspectorInfo;
        private System.Windows.Forms.DataGridView InspectorInfoGridView;
        private System.Windows.Forms.TextBox vncViewerPath;
        private System.Windows.Forms.Label labelVncViewerPath;
        private System.Windows.Forms.Button buttonConfig;
        private System.Windows.Forms.Button buttonFov;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCam;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPath;
        private System.Windows.Forms.CheckBox useLocalMonitorMode;
        private System.Windows.Forms.CheckBox useTestbedStage;
        private System.Windows.Forms.CheckBox useLaserBurner;
    }
}
