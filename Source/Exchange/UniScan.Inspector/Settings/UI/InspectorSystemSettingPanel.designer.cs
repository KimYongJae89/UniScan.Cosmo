namespace UniScan.Inspector.Settings.Inspector.UI
{
    partial class InspectorSystemSettingPanel
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
            this.labelClientIndex = new System.Windows.Forms.Label();
            this.clientIndex = new System.Windows.Forms.NumericUpDown();
            this.labelCamIndex = new System.Windows.Forms.Label();
            this.camIndex = new System.Windows.Forms.NumericUpDown();
            this.inspectorInfoGridView = new System.Windows.Forms.DataGridView();
            this.columnClient = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonDeleteInspectorInfo = new System.Windows.Forms.Button();
            this.buttonAddInspectorInfo = new System.Windows.Forms.Button();
            this.groupMaster = new System.Windows.Forms.GroupBox();
            this.buttonConfig = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.useLocalMonitorMode = new System.Windows.Forms.CheckBox();
            this.calculatorVersion = new System.Windows.Forms.ComboBox();
            this.labelCalculatorVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.clientIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.inspectorInfoGridView)).BeginInit();
            this.groupMaster.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelClientIndex
            // 
            this.labelClientIndex.AutoSize = true;
            this.labelClientIndex.Location = new System.Drawing.Point(14, 40);
            this.labelClientIndex.Margin = new System.Windows.Forms.Padding(0);
            this.labelClientIndex.Name = "labelClientIndex";
            this.labelClientIndex.Size = new System.Drawing.Size(72, 12);
            this.labelClientIndex.TabIndex = 0;
            this.labelClientIndex.Text = "Client Index";
            // 
            // clientIndex
            // 
            this.clientIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clientIndex.Location = new System.Drawing.Point(477, 38);
            this.clientIndex.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.clientIndex.Name = "clientIndex";
            this.clientIndex.Size = new System.Drawing.Size(78, 21);
            this.clientIndex.TabIndex = 3;
            this.clientIndex.ValueChanged += new System.EventHandler(this.clientIndex_ValueChanged);
            // 
            // labelCamIndex
            // 
            this.labelCamIndex.AutoSize = true;
            this.labelCamIndex.Location = new System.Drawing.Point(14, 13);
            this.labelCamIndex.Margin = new System.Windows.Forms.Padding(0);
            this.labelCamIndex.Name = "labelCamIndex";
            this.labelCamIndex.Size = new System.Drawing.Size(67, 12);
            this.labelCamIndex.TabIndex = 0;
            this.labelCamIndex.Text = "Cam Index";
            // 
            // camIndex
            // 
            this.camIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.camIndex.Location = new System.Drawing.Point(477, 11);
            this.camIndex.Name = "camIndex";
            this.camIndex.Size = new System.Drawing.Size(78, 21);
            this.camIndex.TabIndex = 0;
            // 
            // inspectorInfoGridView
            // 
            this.inspectorInfoGridView.AllowUserToAddRows = false;
            this.inspectorInfoGridView.AllowUserToDeleteRows = false;
            this.inspectorInfoGridView.AllowUserToResizeColumns = false;
            this.inspectorInfoGridView.AllowUserToResizeRows = false;
            this.inspectorInfoGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inspectorInfoGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inspectorInfoGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnClient,
            this.columnPath});
            this.inspectorInfoGridView.Location = new System.Drawing.Point(15, 20);
            this.inspectorInfoGridView.Name = "inspectorInfoGridView";
            this.inspectorInfoGridView.RowHeadersVisible = false;
            this.inspectorInfoGridView.RowTemplate.Height = 23;
            this.inspectorInfoGridView.Size = new System.Drawing.Size(518, 115);
            this.inspectorInfoGridView.TabIndex = 4;
            this.inspectorInfoGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.inspectorInfoGridView_CellEndEdit);
            // 
            // columnClient
            // 
            this.columnClient.HeaderText = "Client";
            this.columnClient.Name = "columnClient";
            this.columnClient.Width = 50;
            // 
            // columnPath
            // 
            this.columnPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnPath.FillWeight = 80.23952F;
            this.columnPath.HeaderText = "Path";
            this.columnPath.Name = "columnPath";
            // 
            // buttonDeleteInspectorInfo
            // 
            this.buttonDeleteInspectorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDeleteInspectorInfo.Location = new System.Drawing.Point(477, 141);
            this.buttonDeleteInspectorInfo.Name = "buttonDeleteInspectorInfo";
            this.buttonDeleteInspectorInfo.Size = new System.Drawing.Size(56, 26);
            this.buttonDeleteInspectorInfo.TabIndex = 5;
            this.buttonDeleteInspectorInfo.Text = "Delete";
            this.buttonDeleteInspectorInfo.UseVisualStyleBackColor = true;
            this.buttonDeleteInspectorInfo.Click += new System.EventHandler(this.buttonDeleteInspectorInfo_Click);
            // 
            // buttonAddInspectorInfo
            // 
            this.buttonAddInspectorInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddInspectorInfo.Location = new System.Drawing.Point(415, 141);
            this.buttonAddInspectorInfo.Name = "buttonAddInspectorInfo";
            this.buttonAddInspectorInfo.Size = new System.Drawing.Size(56, 26);
            this.buttonAddInspectorInfo.TabIndex = 6;
            this.buttonAddInspectorInfo.Text = "Add";
            this.buttonAddInspectorInfo.UseVisualStyleBackColor = true;
            this.buttonAddInspectorInfo.Click += new System.EventHandler(this.buttonAddInspectorInfo_Click);
            // 
            // groupMaster
            // 
            this.groupMaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupMaster.Controls.Add(this.buttonConfig);
            this.groupMaster.Controls.Add(this.inspectorInfoGridView);
            this.groupMaster.Controls.Add(this.buttonDeleteInspectorInfo);
            this.groupMaster.Controls.Add(this.buttonAddInspectorInfo);
            this.groupMaster.Location = new System.Drawing.Point(16, 124);
            this.groupMaster.Name = "groupMaster";
            this.groupMaster.Size = new System.Drawing.Size(539, 173);
            this.groupMaster.TabIndex = 7;
            this.groupMaster.TabStop = false;
            this.groupMaster.Text = "Master";
            // 
            // buttonConfig
            // 
            this.buttonConfig.Location = new System.Drawing.Point(15, 141);
            this.buttonConfig.Name = "buttonConfig";
            this.buttonConfig.Size = new System.Drawing.Size(56, 26);
            this.buttonConfig.TabIndex = 8;
            this.buttonConfig.Text = "Config";
            this.buttonConfig.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(499, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 26);
            this.button1.TabIndex = 9;
            this.button1.Text = "Config";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // useLocalMonitorMode
            // 
            this.useLocalMonitorMode.AutoSize = true;
            this.useLocalMonitorMode.Location = new System.Drawing.Point(16, 65);
            this.useLocalMonitorMode.Name = "useLocalMonitorMode";
            this.useLocalMonitorMode.Size = new System.Drawing.Size(163, 16);
            this.useLocalMonitorMode.TabIndex = 10;
            this.useLocalMonitorMode.Text = "Use Loacl Monitor Mode";
            this.useLocalMonitorMode.UseVisualStyleBackColor = true;
            // 
            // calculatorVersion
            // 
            this.calculatorVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.calculatorVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.calculatorVersion.FormattingEnabled = true;
            this.calculatorVersion.Location = new System.Drawing.Point(428, 98);
            this.calculatorVersion.Name = "calculatorVersion";
            this.calculatorVersion.Size = new System.Drawing.Size(121, 20);
            this.calculatorVersion.TabIndex = 11;
            // 
            // labelCalculatorVersion
            // 
            this.labelCalculatorVersion.AutoSize = true;
            this.labelCalculatorVersion.Location = new System.Drawing.Point(15, 98);
            this.labelCalculatorVersion.Margin = new System.Windows.Forms.Padding(0);
            this.labelCalculatorVersion.Name = "labelCalculatorVersion";
            this.labelCalculatorVersion.Size = new System.Drawing.Size(109, 12);
            this.labelCalculatorVersion.TabIndex = 0;
            this.labelCalculatorVersion.Text = "Calculator Version";
            // 
            // InspectorSystemSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.calculatorVersion);
            this.Controls.Add(this.useLocalMonitorMode);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupMaster);
            this.Controls.Add(this.camIndex);
            this.Controls.Add(this.labelCamIndex);
            this.Controls.Add(this.clientIndex);
            this.Controls.Add(this.labelCalculatorVersion);
            this.Controls.Add(this.labelClientIndex);
            this.Name = "InspectorSystemSettingPanel";
            this.Size = new System.Drawing.Size(569, 314);
            ((System.ComponentModel.ISupportInitialize)(this.clientIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.inspectorInfoGridView)).EndInit();
            this.groupMaster.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label labelClientIndex;
        private System.Windows.Forms.NumericUpDown clientIndex;
        private System.Windows.Forms.Label labelCamIndex;
        private System.Windows.Forms.NumericUpDown camIndex;
        private System.Windows.Forms.DataGridView inspectorInfoGridView;
        private System.Windows.Forms.Button buttonDeleteInspectorInfo;
        private System.Windows.Forms.Button buttonAddInspectorInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnClient;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPath;
        private System.Windows.Forms.GroupBox groupMaster;
        private System.Windows.Forms.Button buttonConfig;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox useLocalMonitorMode;
        private System.Windows.Forms.ComboBox calculatorVersion;
        private System.Windows.Forms.Label labelCalculatorVersion;
    }
}
