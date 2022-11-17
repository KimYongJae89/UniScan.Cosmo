//namespace UniScanG.Temp
//{
//    partial class SettingPage
//    {
//        /// <summary> 
//        /// 필수 디자이너 변수입니다.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// 사용 중인 모든 리소스를 정리합니다.
//        /// </summary>
//        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region 구성 요소 디자이너에서 생성한 코드

//        /// <summary> 
//        /// 디자이너 지원에 필요한 메서드입니다. 
//        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
//            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
//            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
//            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
//            this.groupBox5 = new System.Windows.Forms.GroupBox();
//            this.buttonMonCommTest = new System.Windows.Forms.Button();
//            this.groupBox4 = new System.Windows.Forms.GroupBox();
//            this.buttonMonGrabTest = new System.Windows.Forms.Button();
//            this.groupMonitorTransfer = new System.Windows.Forms.GroupBox();
//            this.buttonNew = new System.Windows.Forms.Button();
//            this.buttonDelete = new System.Windows.Forms.Button();
//            this.buttonApply = new System.Windows.Forms.Button();
//            this.inspectorDataGridView = new System.Windows.Forms.DataGridView();
//            this.columnCamID = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.columnPCID = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.columnIPAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.columnResultPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.vncViewerPath = new System.Windows.Forms.TextBox();
//            this.labelVncViewerPath = new System.Windows.Forms.Label();
//            this.groupBox2 = new System.Windows.Forms.GroupBox();
//            this.label23 = new System.Windows.Forms.Label();
//            this.label24 = new System.Windows.Forms.Label();
//            this.storingDays = new System.Windows.Forms.NumericUpDown();
//            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
//            this.groupBox1 = new System.Windows.Forms.GroupBox();
//            this.buttonInspGrabTest = new System.Windows.Forms.Button();
//            this.groupBox3 = new System.Windows.Forms.GroupBox();
//            this.standAlone = new System.Windows.Forms.CheckBox();
//            this.labelStandAlone = new System.Windows.Forms.Label();
//            this.monitorIpAddress = new System.Windows.Forms.TextBox();
//            this.labelMonitorIpAddress = new System.Windows.Forms.Label();
//            this.clientIndex = new System.Windows.Forms.NumericUpDown();
//            this.labelClientIndex = new System.Windows.Forms.Label();
//            this.camIndex = new System.Windows.Forms.NumericUpDown();
//            this.labelCamIndex = new System.Windows.Forms.Label();
//            this.inspectorIpAddress = new System.Windows.Forms.TextBox();
//            this.labelInspectorIpAddress = new System.Windows.Forms.Label();
//            this.groupCalibration = new System.Windows.Forms.GroupBox();
//            this.cameraResolution = new System.Windows.Forms.Label();
//            this.label16 = new System.Windows.Forms.Label();
//            this.buttonCalibration = new System.Windows.Forms.Button();
//            this.startYPosition = new System.Windows.Forms.NumericUpDown();
//            this.labelCalibration = new System.Windows.Forms.Label();
//            this.label17 = new System.Windows.Forms.Label();
//            this.label6 = new System.Windows.Forms.Label();
//            this.sheetHeight = new System.Windows.Forms.NumericUpDown();
//            this.label1 = new System.Windows.Forms.Label();
//            this.label5 = new System.Windows.Forms.Label();
//            this.startXPosition = new System.Windows.Forms.NumericUpDown();
//            this.fovX = new System.Windows.Forms.NumericUpDown();
//            this.label7 = new System.Windows.Forms.Label();
//            this.label4 = new System.Windows.Forms.Label();
//            this.label3 = new System.Windows.Forms.Label();
//            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
//            this.groupBox7 = new System.Windows.Forms.GroupBox();
//            this.label19 = new System.Windows.Forms.Label();
//            this.label18 = new System.Windows.Forms.Label();
//            this.saveInspectImage = new System.Windows.Forms.CheckBox();
//            this.saveFiducialImage = new System.Windows.Forms.CheckBox();
//            this.saveImageImage = new System.Windows.Forms.CheckBox();
//            this.label15 = new System.Windows.Forms.Label();
//            this.saveInspectText = new System.Windows.Forms.CheckBox();
//            this.label12 = new System.Windows.Forms.Label();
//            this.saveFiducialText = new System.Windows.Forms.CheckBox();
//            this.label2 = new System.Windows.Forms.Label();
//            this.saveImageText = new System.Windows.Forms.CheckBox();
//            this.groupBox6 = new System.Windows.Forms.GroupBox();
//            this.label11 = new System.Windows.Forms.Label();
//            this.logLevel = new System.Windows.Forms.ComboBox();
//            this.asyncMode = new System.Windows.Forms.CheckBox();
//            this.labelAsyncMode = new System.Windows.Forms.Label();
//            this.groupBoxDefect = new System.Windows.Forms.GroupBox();
//            this.maxPattern = new System.Windows.Forms.NumericUpDown();
//            this.label21 = new System.Windows.Forms.Label();
//            this.circleRadius = new System.Windows.Forms.NumericUpDown();
//            this.label10 = new System.Windows.Forms.Label();
//            this.label9 = new System.Windows.Forms.Label();
//            this.groupSystemSettings = new System.Windows.Forms.GroupBox();
//            this.labelIpSetting = new System.Windows.Forms.Label();
//            this.buttonIPSet = new System.Windows.Forms.Button();
//            this.systemType = new System.Windows.Forms.ComboBox();
//            this.labelSystemType = new System.Windows.Forms.Label();
//            this.language = new System.Windows.Forms.ComboBox();
//            this.labelLanguage = new System.Windows.Forms.Label();
//            this.groupImage = new System.Windows.Forms.GroupBox();
//            this.label8 = new System.Windows.Forms.Label();
//            this.bufferSize = new System.Windows.Forms.NumericUpDown();
//            this.labelMaskThV = new System.Windows.Forms.Label();
//            this.maskThV = new System.Windows.Forms.NumericUpDown();
//            this.labelMaskThH = new System.Windows.Forms.Label();
//            this.label13 = new System.Windows.Forms.Label();
//            this.saturationRange = new System.Windows.Forms.NumericUpDown();
//            this.labelMaskTh = new System.Windows.Forms.Label();
//            this.maskThH = new System.Windows.Forms.NumericUpDown();
//            this.imageCheckInterval = new System.Windows.Forms.NumericUpDown();
//            this.labelImageCheckInterval = new System.Windows.Forms.Label();
//            this.labelSec = new System.Windows.Forms.Label();
//            this.labelMs = new System.Windows.Forms.Label();
//            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
//            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
//            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
//            this.tabControlParam = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
//            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
//            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
//            this.buttonTransfer = new System.Windows.Forms.Button();
//            this.ultraTabPageControl3.SuspendLayout();
//            this.groupBox5.SuspendLayout();
//            this.groupBox4.SuspendLayout();
//            this.groupMonitorTransfer.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.inspectorDataGridView)).BeginInit();
//            this.groupBox2.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.storingDays)).BeginInit();
//            this.ultraTabPageControl2.SuspendLayout();
//            this.groupBox1.SuspendLayout();
//            this.groupBox3.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.clientIndex)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.camIndex)).BeginInit();
//            this.groupCalibration.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.startYPosition)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.sheetHeight)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.startXPosition)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.fovX)).BeginInit();
//            this.ultraTabPageControl4.SuspendLayout();
//            this.groupBox7.SuspendLayout();
//            this.groupBox6.SuspendLayout();
//            this.groupBoxDefect.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.maxPattern)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.circleRadius)).BeginInit();
//            this.groupSystemSettings.SuspendLayout();
//            this.groupImage.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.bufferSize)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.maskThV)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.saturationRange)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.maskThH)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.tabControlParam)).BeginInit();
//            this.tabControlParam.SuspendLayout();
//            this.buttonPanel.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // ultraTabPageControl3
//            // 
//            this.ultraTabPageControl3.Controls.Add(this.groupBox5);
//            this.ultraTabPageControl3.Controls.Add(this.groupBox4);
//            this.ultraTabPageControl3.Controls.Add(this.groupMonitorTransfer);
//            this.ultraTabPageControl3.Controls.Add(this.groupBox2);
//            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
//            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
//            this.ultraTabPageControl3.Size = new System.Drawing.Size(1561, 1035);
//            // 
//            // groupBox5
//            // 
//            this.groupBox5.BackColor = System.Drawing.SystemColors.Control;
//            this.groupBox5.Controls.Add(this.buttonMonCommTest);
//            this.groupBox5.Location = new System.Drawing.Point(536, 78);
//            this.groupBox5.Name = "groupBox5";
//            this.groupBox5.Size = new System.Drawing.Size(251, 63);
//            this.groupBox5.TabIndex = 237;
//            this.groupBox5.TabStop = false;
//            this.groupBox5.Text = "Comm Test";
//            // 
//            // buttonMonCommTest
//            // 
//            this.buttonMonCommTest.Location = new System.Drawing.Point(112, 20);
//            this.buttonMonCommTest.Margin = new System.Windows.Forms.Padding(4);
//            this.buttonMonCommTest.Name = "buttonMonCommTest";
//            this.buttonMonCommTest.Size = new System.Drawing.Size(113, 30);
//            this.buttonMonCommTest.TabIndex = 244;
//            this.buttonMonCommTest.Text = "Start";
//            this.buttonMonCommTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
//            this.buttonMonCommTest.UseVisualStyleBackColor = true;
//            this.buttonMonCommTest.Click += new System.EventHandler(this.buttonMonCommTest_Click);
//            // 
//            // groupBox4
//            // 
//            this.groupBox4.BackColor = System.Drawing.SystemColors.Control;
//            this.groupBox4.Controls.Add(this.buttonMonGrabTest);
//            this.groupBox4.Location = new System.Drawing.Point(536, 9);
//            this.groupBox4.Name = "groupBox4";
//            this.groupBox4.Size = new System.Drawing.Size(251, 63);
//            this.groupBox4.TabIndex = 236;
//            this.groupBox4.TabStop = false;
//            this.groupBox4.Text = "Grab Test";
//            // 
//            // buttonMonGrabTest
//            // 
//            this.buttonMonGrabTest.Location = new System.Drawing.Point(112, 20);
//            this.buttonMonGrabTest.Margin = new System.Windows.Forms.Padding(4);
//            this.buttonMonGrabTest.Name = "buttonMonGrabTest";
//            this.buttonMonGrabTest.Size = new System.Drawing.Size(113, 30);
//            this.buttonMonGrabTest.TabIndex = 244;
//            this.buttonMonGrabTest.Text = "Start";
//            this.buttonMonGrabTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
//            this.buttonMonGrabTest.UseVisualStyleBackColor = true;
//            this.buttonMonGrabTest.Click += new System.EventHandler(this.buttonMonGrabTest_Click);
//            // 
//            // groupMonitorTransfer
//            // 
//            this.groupMonitorTransfer.BackColor = System.Drawing.SystemColors.Control;
//            this.groupMonitorTransfer.Controls.Add(this.buttonNew);
//            this.groupMonitorTransfer.Controls.Add(this.buttonDelete);
//            this.groupMonitorTransfer.Controls.Add(this.buttonApply);
//            this.groupMonitorTransfer.Controls.Add(this.inspectorDataGridView);
//            this.groupMonitorTransfer.Controls.Add(this.vncViewerPath);
//            this.groupMonitorTransfer.Controls.Add(this.labelVncViewerPath);
//            this.groupMonitorTransfer.Location = new System.Drawing.Point(13, 78);
//            this.groupMonitorTransfer.Name = "groupMonitorTransfer";
//            this.groupMonitorTransfer.Size = new System.Drawing.Size(507, 242);
//            this.groupMonitorTransfer.TabIndex = 235;
//            this.groupMonitorTransfer.TabStop = false;
//            this.groupMonitorTransfer.Text = "Transfer";
//            // 
//            // buttonNew
//            // 
//            this.buttonNew.Location = new System.Drawing.Point(135, 200);
//            this.buttonNew.Margin = new System.Windows.Forms.Padding(4);
//            this.buttonNew.Name = "buttonNew";
//            this.buttonNew.Size = new System.Drawing.Size(113, 30);
//            this.buttonNew.TabIndex = 245;
//            this.buttonNew.Text = "New";
//            this.buttonNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
//            this.buttonNew.UseVisualStyleBackColor = true;
//            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
//            // 
//            // buttonDelete
//            // 
//            this.buttonDelete.Location = new System.Drawing.Point(256, 200);
//            this.buttonDelete.Margin = new System.Windows.Forms.Padding(4);
//            this.buttonDelete.Name = "buttonDelete";
//            this.buttonDelete.Size = new System.Drawing.Size(113, 30);
//            this.buttonDelete.TabIndex = 244;
//            this.buttonDelete.Text = "Delete";
//            this.buttonDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
//            this.buttonDelete.UseVisualStyleBackColor = true;
//            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
//            // 
//            // buttonApply
//            // 
//            this.buttonApply.Location = new System.Drawing.Point(377, 200);
//            this.buttonApply.Margin = new System.Windows.Forms.Padding(4);
//            this.buttonApply.Name = "buttonApply";
//            this.buttonApply.Size = new System.Drawing.Size(113, 30);
//            this.buttonApply.TabIndex = 243;
//            this.buttonApply.Text = "Apply ";
//            this.buttonApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
//            this.buttonApply.UseVisualStyleBackColor = true;
//            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
//            // 
//            // inspectorDataGridView
//            // 
//            this.inspectorDataGridView.AllowUserToAddRows = false;
//            this.inspectorDataGridView.AllowUserToDeleteRows = false;
//            this.inspectorDataGridView.AllowUserToResizeColumns = false;
//            this.inspectorDataGridView.AllowUserToResizeRows = false;
//            this.inspectorDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            this.inspectorDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
//            this.columnCamID,
//            this.columnPCID,
//            this.columnIPAddress,
//            this.columnResultPath});
//            this.inspectorDataGridView.Location = new System.Drawing.Point(24, 66);
//            this.inspectorDataGridView.Name = "inspectorDataGridView";
//            this.inspectorDataGridView.RowHeadersVisible = false;
//            this.inspectorDataGridView.RowTemplate.Height = 23;
//            this.inspectorDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
//            this.inspectorDataGridView.Size = new System.Drawing.Size(466, 127);
//            this.inspectorDataGridView.TabIndex = 206;
//            // 
//            // columnCamID
//            // 
//            this.columnCamID.HeaderText = "Cam ID";
//            this.columnCamID.Name = "columnCamID";
//            this.columnCamID.Width = 50;
//            // 
//            // columnPCID
//            // 
//            this.columnPCID.HeaderText = "PC ID";
//            this.columnPCID.Name = "columnPCID";
//            this.columnPCID.Width = 50;
//            // 
//            // columnIPAddress
//            // 
//            this.columnIPAddress.HeaderText = "IP Address";
//            this.columnIPAddress.Name = "columnIPAddress";
//            // 
//            // columnResultPath
//            // 
//            this.columnResultPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
//            this.columnResultPath.HeaderText = "Shared Path";
//            this.columnResultPath.Name = "columnResultPath";
//            // 
//            // vncViewerPath
//            // 
//            this.vncViewerPath.Location = new System.Drawing.Point(168, 25);
//            this.vncViewerPath.Name = "vncViewerPath";
//            this.vncViewerPath.Size = new System.Drawing.Size(322, 24);
//            this.vncViewerPath.TabIndex = 205;
//            this.vncViewerPath.TextChanged += new System.EventHandler(this.vncViewerPath_TextChanged);
//            // 
//            // labelVncViewerPath
//            // 
//            this.labelVncViewerPath.AutoSize = true;
//            this.labelVncViewerPath.Location = new System.Drawing.Point(21, 28);
//            this.labelVncViewerPath.Name = "labelVncViewerPath";
//            this.labelVncViewerPath.Size = new System.Drawing.Size(121, 18);
//            this.labelVncViewerPath.TabIndex = 204;
//            this.labelVncViewerPath.Text = "VNC Viewer Path";
//            // 
//            // groupBox2
//            // 
//            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
//            this.groupBox2.Controls.Add(this.label23);
//            this.groupBox2.Controls.Add(this.label24);
//            this.groupBox2.Controls.Add(this.storingDays);
//            this.groupBox2.Location = new System.Drawing.Point(13, 9);
//            this.groupBox2.Name = "groupBox2";
//            this.groupBox2.Size = new System.Drawing.Size(507, 63);
//            this.groupBox2.TabIndex = 234;
//            this.groupBox2.TabStop = false;
//            this.groupBox2.Text = "Data Remove";
//            // 
//            // label23
//            // 
//            this.label23.AutoSize = true;
//            this.label23.Location = new System.Drawing.Point(456, 26);
//            this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label23.Name = "label23";
//            this.label23.Size = new System.Drawing.Size(34, 18);
//            this.label23.TabIndex = 232;
//            this.label23.Text = "Day";
//            // 
//            // label24
//            // 
//            this.label24.AutoSize = true;
//            this.label24.Location = new System.Drawing.Point(13, 26);
//            this.label24.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
//            this.label24.Name = "label24";
//            this.label24.Size = new System.Drawing.Size(93, 18);
//            this.label24.TabIndex = 230;
//            this.label24.Text = "Storing Days";
//            // 
//            // storingDays
//            // 
//            this.storingDays.Location = new System.Drawing.Point(375, 24);
//            this.storingDays.Margin = new System.Windows.Forms.Padding(7);
//            this.storingDays.Maximum = new decimal(new int[] {
//            50000,
//            0,
//            0,
//            0});
//            this.storingDays.Name = "storingDays";
//            this.storingDays.Size = new System.Drawing.Size(69, 24);
//            this.storingDays.TabIndex = 231;
//            this.storingDays.Value = new decimal(new int[] {
//            21,
//            0,
//            0,
//            0});
//            this.storingDays.ValueChanged += new System.EventHandler(this.storingDays_ValueChanged);
//            // 
//            // ultraTabPageControl2
//            // 
//            this.ultraTabPageControl2.Controls.Add(this.groupBox1);
//            this.ultraTabPageControl2.Controls.Add(this.groupBox3);
//            this.ultraTabPageControl2.Controls.Add(this.groupCalibration);
//            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
//            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
//            this.ultraTabPageControl2.Size = new System.Drawing.Size(1561, 1035);
//            // 
//            // groupBox1
//            // 
//            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
//            this.groupBox1.Controls.Add(this.buttonInspGrabTest);
//            this.groupBox1.Location = new System.Drawing.Point(424, 12);
//            this.groupBox1.Name = "groupBox1";
//            this.groupBox1.Size = new System.Drawing.Size(251, 63);
//            this.groupBox1.TabIndex = 241;
//            this.groupBox1.TabStop = false;
//            this.groupBox1.Text = "Grab Test";
//            // 
//            // buttonInspGrabTest
//            // 
//            this.buttonInspGrabTest.Location = new System.Drawing.Point(112, 20);
//            this.buttonInspGrabTest.Margin = new System.Windows.Forms.Padding(4);
//            this.buttonInspGrabTest.Name = "buttonInspGrabTest";
//            this.buttonInspGrabTest.Size = new System.Drawing.Size(113, 30);
//            this.buttonInspGrabTest.TabIndex = 244;
//            this.buttonInspGrabTest.Text = "Start";
//            this.buttonInspGrabTest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
//            this.buttonInspGrabTest.UseVisualStyleBackColor = true;
//            this.buttonInspGrabTest.Click += new System.EventHandler(this.buttonInspGrabTest_Click);
//            // 
//            // groupBox3
//            // 
//            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
//            this.groupBox3.Controls.Add(this.standAlone);
//            this.groupBox3.Controls.Add(this.labelStandAlone);
//            this.groupBox3.Controls.Add(this.monitorIpAddress);
//            this.groupBox3.Controls.Add(this.labelMonitorIpAddress);
//            this.groupBox3.Controls.Add(this.clientIndex);
//            this.groupBox3.Controls.Add(this.labelClientIndex);
//            this.groupBox3.Controls.Add(this.camIndex);
//            this.groupBox3.Controls.Add(this.labelCamIndex);
//            this.groupBox3.Controls.Add(this.inspectorIpAddress);
//            this.groupBox3.Controls.Add(this.labelInspectorIpAddress);
//            this.groupBox3.Location = new System.Drawing.Point(12, 198);
//            this.groupBox3.Name = "groupBox3";
//            this.groupBox3.Size = new System.Drawing.Size(406, 173);
//            this.groupBox3.TabIndex = 240;
//            this.groupBox3.TabStop = false;
//            this.groupBox3.Text = "Communicate";
//            // 
//            // standAlone
//            // 
//            this.standAlone.AutoSize = true;
//            this.standAlone.Location = new System.Drawing.Point(343, 144);
//            this.standAlone.Name = "standAlone";
//            this.standAlone.Size = new System.Drawing.Size(15, 14);
//            this.standAlone.TabIndex = 232;
//            this.standAlone.UseVisualStyleBackColor = true;
//            this.standAlone.CheckedChanged += new System.EventHandler(this.standAlone_CheckedChanged);
//            // 
//            // labelStandAlone
//            // 
//            this.labelStandAlone.AutoSize = true;
//            this.labelStandAlone.Location = new System.Drawing.Point(9, 142);
//            this.labelStandAlone.Name = "labelStandAlone";
//            this.labelStandAlone.Size = new System.Drawing.Size(125, 18);
//            this.labelStandAlone.TabIndex = 231;
//            this.labelStandAlone.Text = "StandAlone Mode";
//            // 
//            // monitorIpAddress
//            // 
//            this.monitorIpAddress.Location = new System.Drawing.Point(165, 23);
//            this.monitorIpAddress.Name = "monitorIpAddress";
//            this.monitorIpAddress.Size = new System.Drawing.Size(220, 24);
//            this.monitorIpAddress.TabIndex = 230;
//            this.monitorIpAddress.TextChanged += new System.EventHandler(this.monitorIpAddress_TextChanged);
//            // 
//            // labelMonitorIpAddress
//            // 
//            this.labelMonitorIpAddress.AutoSize = true;
//            this.labelMonitorIpAddress.Location = new System.Drawing.Point(9, 26);
//            this.labelMonitorIpAddress.Name = "labelMonitorIpAddress";
//            this.labelMonitorIpAddress.Size = new System.Drawing.Size(134, 18);
//            this.labelMonitorIpAddress.TabIndex = 229;
//            this.labelMonitorIpAddress.Text = "Monitor IP Address";
//            // 
//            // clientIndex
//            // 
//            this.clientIndex.Location = new System.Drawing.Point(316, 113);
//            this.clientIndex.Margin = new System.Windows.Forms.Padding(7);
//            this.clientIndex.Maximum = new decimal(new int[] {
//            100000,
//            0,
//            0,
//            0});
//            this.clientIndex.Minimum = new decimal(new int[] {
//            1,
//            0,
//            0,
//            -2147483648});
//            this.clientIndex.Name = "clientIndex";
//            this.clientIndex.Size = new System.Drawing.Size(69, 24);
//            this.clientIndex.TabIndex = 228;
//            this.clientIndex.Value = new decimal(new int[] {
//            100,
//            0,
//            0,
//            0});
//            this.clientIndex.ValueChanged += new System.EventHandler(this.clientIndex_ValueChanged);
//            // 
//            // labelClientIndex
//            // 
//            this.labelClientIndex.AutoSize = true;
//            this.labelClientIndex.Location = new System.Drawing.Point(9, 113);
//            this.labelClientIndex.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.labelClientIndex.Name = "labelClientIndex";
//            this.labelClientIndex.Size = new System.Drawing.Size(83, 18);
//            this.labelClientIndex.TabIndex = 227;
//            this.labelClientIndex.Text = "Client Index";
//            // 
//            // camIndex
//            // 
//            this.camIndex.Location = new System.Drawing.Point(316, 82);
//            this.camIndex.Margin = new System.Windows.Forms.Padding(7);
//            this.camIndex.Maximum = new decimal(new int[] {
//            100000,
//            0,
//            0,
//            0});
//            this.camIndex.Name = "camIndex";
//            this.camIndex.Size = new System.Drawing.Size(69, 24);
//            this.camIndex.TabIndex = 226;
//            this.camIndex.Value = new decimal(new int[] {
//            50,
//            0,
//            0,
//            0});
//            this.camIndex.ValueChanged += new System.EventHandler(this.camIndex_ValueChanged);
//            // 
//            // labelCamIndex
//            // 
//            this.labelCamIndex.AutoSize = true;
//            this.labelCamIndex.Location = new System.Drawing.Point(9, 84);
//            this.labelCamIndex.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.labelCamIndex.Name = "labelCamIndex";
//            this.labelCamIndex.Size = new System.Drawing.Size(78, 18);
//            this.labelCamIndex.TabIndex = 225;
//            this.labelCamIndex.Text = "Cam Index";
//            // 
//            // inspectorIpAddress
//            // 
//            this.inspectorIpAddress.Location = new System.Drawing.Point(165, 51);
//            this.inspectorIpAddress.Name = "inspectorIpAddress";
//            this.inspectorIpAddress.Size = new System.Drawing.Size(220, 24);
//            this.inspectorIpAddress.TabIndex = 205;
//            this.inspectorIpAddress.TextChanged += new System.EventHandler(this.inspectorIpAddress_TextChanged);
//            // 
//            // labelInspectorIpAddress
//            // 
//            this.labelInspectorIpAddress.AutoSize = true;
//            this.labelInspectorIpAddress.Location = new System.Drawing.Point(9, 54);
//            this.labelInspectorIpAddress.Name = "labelInspectorIpAddress";
//            this.labelInspectorIpAddress.Size = new System.Drawing.Size(144, 18);
//            this.labelInspectorIpAddress.TabIndex = 204;
//            this.labelInspectorIpAddress.Text = "Inspector IP Address";
//            // 
//            // groupCalibration
//            // 
//            this.groupCalibration.BackColor = System.Drawing.SystemColors.Control;
//            this.groupCalibration.Controls.Add(this.cameraResolution);
//            this.groupCalibration.Controls.Add(this.label16);
//            this.groupCalibration.Controls.Add(this.buttonCalibration);
//            this.groupCalibration.Controls.Add(this.startYPosition);
//            this.groupCalibration.Controls.Add(this.labelCalibration);
//            this.groupCalibration.Controls.Add(this.label17);
//            this.groupCalibration.Controls.Add(this.label6);
//            this.groupCalibration.Controls.Add(this.sheetHeight);
//            this.groupCalibration.Controls.Add(this.label1);
//            this.groupCalibration.Controls.Add(this.label5);
//            this.groupCalibration.Controls.Add(this.startXPosition);
//            this.groupCalibration.Controls.Add(this.fovX);
//            this.groupCalibration.Controls.Add(this.label7);
//            this.groupCalibration.Controls.Add(this.label4);
//            this.groupCalibration.Controls.Add(this.label3);
//            this.groupCalibration.Location = new System.Drawing.Point(12, 12);
//            this.groupCalibration.Name = "groupCalibration";
//            this.groupCalibration.Size = new System.Drawing.Size(406, 180);
//            this.groupCalibration.TabIndex = 239;
//            this.groupCalibration.TabStop = false;
//            this.groupCalibration.Text = "Calibration";
//            // 
//            // cameraResolution
//            // 
//            this.cameraResolution.AutoSize = true;
//            this.cameraResolution.Location = new System.Drawing.Point(166, 149);
//            this.cameraResolution.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
//            this.cameraResolution.Name = "cameraResolution";
//            this.cameraResolution.Size = new System.Drawing.Size(96, 18);
//            this.cameraResolution.TabIndex = 243;
//            this.cameraResolution.Text = "00.00 [um/px]";
//            // 
//            // label16
//            // 
//            this.label16.AutoSize = true;
//            this.label16.Location = new System.Drawing.Point(9, 58);
//            this.label16.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
//            this.label16.Name = "label16";
//            this.label16.Size = new System.Drawing.Size(110, 18);
//            this.label16.TabIndex = 239;
//            this.label16.Text = "Start Y Position";
//            // 
//            // buttonCalibration
//            // 
//            this.buttonCalibration.Location = new System.Drawing.Point(272, 143);
//            this.buttonCalibration.Margin = new System.Windows.Forms.Padding(4);
//            this.buttonCalibration.Name = "buttonCalibration";
//            this.buttonCalibration.Size = new System.Drawing.Size(113, 30);
//            this.buttonCalibration.TabIndex = 242;
//            this.buttonCalibration.Text = "Start";
//            this.buttonCalibration.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
//            this.buttonCalibration.UseVisualStyleBackColor = true;
//            this.buttonCalibration.Click += new System.EventHandler(this.buttonCalibration_Click);
//            // 
//            // startYPosition
//            // 
//            this.startYPosition.Location = new System.Drawing.Point(272, 56);
//            this.startYPosition.Margin = new System.Windows.Forms.Padding(7);
//            this.startYPosition.Maximum = new decimal(new int[] {
//            50000,
//            0,
//            0,
//            0});
//            this.startYPosition.Name = "startYPosition";
//            this.startYPosition.Size = new System.Drawing.Size(69, 24);
//            this.startYPosition.TabIndex = 240;
//            this.startYPosition.Value = new decimal(new int[] {
//            50,
//            0,
//            0,
//            0});
//            this.startYPosition.ValueChanged += new System.EventHandler(this.startYPosition_ValueChanged);
//            // 
//            // labelCalibration
//            // 
//            this.labelCalibration.AutoSize = true;
//            this.labelCalibration.Location = new System.Drawing.Point(9, 149);
//            this.labelCalibration.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
//            this.labelCalibration.Name = "labelCalibration";
//            this.labelCalibration.Size = new System.Drawing.Size(114, 18);
//            this.labelCalibration.TabIndex = 241;
//            this.labelCalibration.Text = "Cam Calibration";
//            // 
//            // label17
//            // 
//            this.label17.AutoSize = true;
//            this.label17.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.label17.Location = new System.Drawing.Point(352, 60);
//            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label17.Name = "label17";
//            this.label17.Size = new System.Drawing.Size(33, 19);
//            this.label17.TabIndex = 241;
//            this.label17.Text = "mm";
//            // 
//            // label6
//            // 
//            this.label6.AutoSize = true;
//            this.label6.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.label6.Location = new System.Drawing.Point(352, 117);
//            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label6.Name = "label6";
//            this.label6.Size = new System.Drawing.Size(33, 19);
//            this.label6.TabIndex = 238;
//            this.label6.Text = "mm";
//            // 
//            // sheetHeight
//            // 
//            this.sheetHeight.Location = new System.Drawing.Point(272, 113);
//            this.sheetHeight.Margin = new System.Windows.Forms.Padding(7);
//            this.sheetHeight.Maximum = new decimal(new int[] {
//            50000,
//            0,
//            0,
//            0});
//            this.sheetHeight.Minimum = new decimal(new int[] {
//            5,
//            0,
//            0,
//            0});
//            this.sheetHeight.Name = "sheetHeight";
//            this.sheetHeight.Size = new System.Drawing.Size(69, 24);
//            this.sheetHeight.TabIndex = 237;
//            this.sheetHeight.Value = new decimal(new int[] {
//            50,
//            0,
//            0,
//            0});
//            this.sheetHeight.ValueChanged += new System.EventHandler(this.sheetHeight_ValueChanged);
//            // 
//            // label1
//            // 
//            this.label1.AutoSize = true;
//            this.label1.Location = new System.Drawing.Point(9, 31);
//            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
//            this.label1.Name = "label1";
//            this.label1.Size = new System.Drawing.Size(111, 18);
//            this.label1.TabIndex = 230;
//            this.label1.Text = "Start X Position";
//            // 
//            // label5
//            // 
//            this.label5.AutoSize = true;
//            this.label5.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.label5.Location = new System.Drawing.Point(352, 90);
//            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label5.Name = "label5";
//            this.label5.Size = new System.Drawing.Size(33, 19);
//            this.label5.TabIndex = 236;
//            this.label5.Text = "mm";
//            // 
//            // startXPosition
//            // 
//            this.startXPosition.Location = new System.Drawing.Point(272, 29);
//            this.startXPosition.Margin = new System.Windows.Forms.Padding(7);
//            this.startXPosition.Maximum = new decimal(new int[] {
//            50000,
//            0,
//            0,
//            0});
//            this.startXPosition.Name = "startXPosition";
//            this.startXPosition.Size = new System.Drawing.Size(69, 24);
//            this.startXPosition.TabIndex = 231;
//            this.startXPosition.Value = new decimal(new int[] {
//            50,
//            0,
//            0,
//            0});
//            this.startXPosition.ValueChanged += new System.EventHandler(this.startXPosition_ValueChanged);
//            // 
//            // fovX
//            // 
//            this.fovX.Location = new System.Drawing.Point(272, 85);
//            this.fovX.Margin = new System.Windows.Forms.Padding(7);
//            this.fovX.Maximum = new decimal(new int[] {
//            50000,
//            0,
//            0,
//            0});
//            this.fovX.Minimum = new decimal(new int[] {
//            5,
//            0,
//            0,
//            0});
//            this.fovX.Name = "fovX";
//            this.fovX.Size = new System.Drawing.Size(69, 24);
//            this.fovX.TabIndex = 235;
//            this.fovX.Value = new decimal(new int[] {
//            50,
//            0,
//            0,
//            0});
//            this.fovX.ValueChanged += new System.EventHandler(this.fovX_ValueChanged);
//            // 
//            // label7
//            // 
//            this.label7.AutoSize = true;
//            this.label7.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.label7.Location = new System.Drawing.Point(352, 33);
//            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label7.Name = "label7";
//            this.label7.Size = new System.Drawing.Size(33, 19);
//            this.label7.TabIndex = 232;
//            this.label7.Text = "mm";
//            // 
//            // label4
//            // 
//            this.label4.AutoSize = true;
//            this.label4.Location = new System.Drawing.Point(9, 88);
//            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
//            this.label4.Name = "label4";
//            this.label4.Size = new System.Drawing.Size(52, 18);
//            this.label4.TabIndex = 234;
//            this.label4.Text = "FOV X";
//            // 
//            // label3
//            // 
//            this.label3.AutoSize = true;
//            this.label3.Location = new System.Drawing.Point(9, 115);
//            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
//            this.label3.Name = "label3";
//            this.label3.Size = new System.Drawing.Size(92, 18);
//            this.label3.TabIndex = 233;
//            this.label3.Text = "Sheet Height";
//            // 
//            // ultraTabPageControl4
//            // 
//            this.ultraTabPageControl4.Controls.Add(this.groupBox7);
//            this.ultraTabPageControl4.Controls.Add(this.groupBox6);
//            this.ultraTabPageControl4.Controls.Add(this.groupBoxDefect);
//            this.ultraTabPageControl4.Controls.Add(this.groupSystemSettings);
//            this.ultraTabPageControl4.Controls.Add(this.groupImage);
//            this.ultraTabPageControl4.Location = new System.Drawing.Point(1, 26);
//            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
//            this.ultraTabPageControl4.Size = new System.Drawing.Size(1561, 1035);
//            // 
//            // groupBox7
//            // 
//            this.groupBox7.Controls.Add(this.label19);
//            this.groupBox7.Controls.Add(this.label18);
//            this.groupBox7.Controls.Add(this.saveInspectImage);
//            this.groupBox7.Controls.Add(this.saveFiducialImage);
//            this.groupBox7.Controls.Add(this.saveImageImage);
//            this.groupBox7.Controls.Add(this.label15);
//            this.groupBox7.Controls.Add(this.saveInspectText);
//            this.groupBox7.Controls.Add(this.label12);
//            this.groupBox7.Controls.Add(this.saveFiducialText);
//            this.groupBox7.Controls.Add(this.label2);
//            this.groupBox7.Controls.Add(this.saveImageText);
//            this.groupBox7.Location = new System.Drawing.Point(407, 122);
//            this.groupBox7.Name = "groupBox7";
//            this.groupBox7.Size = new System.Drawing.Size(285, 186);
//            this.groupBox7.TabIndex = 227;
//            this.groupBox7.TabStop = false;
//            this.groupBox7.Text = "Save Debug Data";
//            // 
//            // label19
//            // 
//            this.label19.AutoSize = true;
//            this.label19.Location = new System.Drawing.Point(212, 20);
//            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label19.Name = "label19";
//            this.label19.Size = new System.Drawing.Size(48, 18);
//            this.label19.TabIndex = 247;
//            this.label19.Text = "Image";
//            // 
//            // label18
//            // 
//            this.label18.AutoSize = true;
//            this.label18.Location = new System.Drawing.Point(161, 20);
//            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label18.Name = "label18";
//            this.label18.Size = new System.Drawing.Size(36, 18);
//            this.label18.TabIndex = 246;
//            this.label18.Text = "Text";
//            // 
//            // saveInspectImage
//            // 
//            this.saveInspectImage.AutoSize = true;
//            this.saveInspectImage.Location = new System.Drawing.Point(229, 116);
//            this.saveInspectImage.Name = "saveInspectImage";
//            this.saveInspectImage.Size = new System.Drawing.Size(15, 14);
//            this.saveInspectImage.TabIndex = 245;
//            this.saveInspectImage.UseVisualStyleBackColor = true;
//            this.saveInspectImage.CheckedChanged += new System.EventHandler(this.saveInspect_CheckedChanged);
//            // 
//            // saveFiducialImage
//            // 
//            this.saveFiducialImage.AutoSize = true;
//            this.saveFiducialImage.Location = new System.Drawing.Point(229, 83);
//            this.saveFiducialImage.Name = "saveFiducialImage";
//            this.saveFiducialImage.Size = new System.Drawing.Size(15, 14);
//            this.saveFiducialImage.TabIndex = 243;
//            this.saveFiducialImage.UseVisualStyleBackColor = true;
//            this.saveFiducialImage.CheckedChanged += new System.EventHandler(this.saveFiducial_CheckedChanged);
//            // 
//            // saveImageImage
//            // 
//            this.saveImageImage.AutoSize = true;
//            this.saveImageImage.Location = new System.Drawing.Point(229, 49);
//            this.saveImageImage.Name = "saveImageImage";
//            this.saveImageImage.Size = new System.Drawing.Size(15, 14);
//            this.saveImageImage.TabIndex = 242;
//            this.saveImageImage.UseVisualStyleBackColor = true;
//            this.saveImageImage.CheckedChanged += new System.EventHandler(this.saveImageImage_CheckedChanged);
//            // 
//            // label15
//            // 
//            this.label15.AutoSize = true;
//            this.label15.Location = new System.Drawing.Point(20, 111);
//            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label15.Name = "label15";
//            this.label15.Size = new System.Drawing.Size(75, 18);
//            this.label15.TabIndex = 241;
//            this.label15.Text = "Inspection";
//            // 
//            // saveInspectText
//            // 
//            this.saveInspectText.AutoSize = true;
//            this.saveInspectText.Location = new System.Drawing.Point(172, 116);
//            this.saveInspectText.Name = "saveInspectText";
//            this.saveInspectText.Size = new System.Drawing.Size(15, 14);
//            this.saveInspectText.TabIndex = 240;
//            this.saveInspectText.UseVisualStyleBackColor = true;
//            this.saveInspectText.CheckedChanged += new System.EventHandler(this.saveInspect_CheckedChanged);
//            // 
//            // label12
//            // 
//            this.label12.AutoSize = true;
//            this.label12.Location = new System.Drawing.Point(20, 78);
//            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label12.Name = "label12";
//            this.label12.Size = new System.Drawing.Size(87, 18);
//            this.label12.TabIndex = 237;
//            this.label12.Text = "Mark Finder";
//            // 
//            // saveFiducialText
//            // 
//            this.saveFiducialText.AutoSize = true;
//            this.saveFiducialText.Location = new System.Drawing.Point(172, 83);
//            this.saveFiducialText.Name = "saveFiducialText";
//            this.saveFiducialText.Size = new System.Drawing.Size(15, 14);
//            this.saveFiducialText.TabIndex = 236;
//            this.saveFiducialText.UseVisualStyleBackColor = true;
//            this.saveFiducialText.CheckedChanged += new System.EventHandler(this.saveFiducial_CheckedChanged);
//            // 
//            // label2
//            // 
//            this.label2.AutoSize = true;
//            this.label2.Location = new System.Drawing.Point(20, 44);
//            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label2.Name = "label2";
//            this.label2.Size = new System.Drawing.Size(101, 18);
//            this.label2.TabIndex = 235;
//            this.label2.Text = "Frame / Sheet";
//            // 
//            // saveImageText
//            // 
//            this.saveImageText.AutoSize = true;
//            this.saveImageText.Location = new System.Drawing.Point(172, 49);
//            this.saveImageText.Name = "saveImageText";
//            this.saveImageText.Size = new System.Drawing.Size(15, 14);
//            this.saveImageText.TabIndex = 224;
//            this.saveImageText.UseVisualStyleBackColor = true;
//            this.saveImageText.CheckedChanged += new System.EventHandler(this.saveImageText_CheckedChanged);
//            // 
//            // groupBox6
//            // 
//            this.groupBox6.Controls.Add(this.label11);
//            this.groupBox6.Controls.Add(this.logLevel);
//            this.groupBox6.Controls.Add(this.asyncMode);
//            this.groupBox6.Controls.Add(this.labelAsyncMode);
//            this.groupBox6.Location = new System.Drawing.Point(407, 12);
//            this.groupBox6.Name = "groupBox6";
//            this.groupBox6.Size = new System.Drawing.Size(285, 104);
//            this.groupBox6.TabIndex = 227;
//            this.groupBox6.TabStop = false;
//            this.groupBox6.Text = "Operation";
//            // 
//            // label11
//            // 
//            this.label11.AutoSize = true;
//            this.label11.Location = new System.Drawing.Point(20, 31);
//            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label11.Name = "label11";
//            this.label11.Size = new System.Drawing.Size(71, 18);
//            this.label11.TabIndex = 237;
//            this.label11.Text = "Log Level";
//            // 
//            // logLevel
//            // 
//            this.logLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.logLevel.FormattingEnabled = true;
//            this.logLevel.Location = new System.Drawing.Point(152, 28);
//            this.logLevel.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
//            this.logLevel.Name = "logLevel";
//            this.logLevel.Size = new System.Drawing.Size(91, 26);
//            this.logLevel.TabIndex = 236;
//            this.logLevel.SelectedIndexChanged += new System.EventHandler(this.logLevel_SelectedIndexChanged);
//            // 
//            // asyncMode
//            // 
//            this.asyncMode.AutoSize = true;
//            this.asyncMode.Location = new System.Drawing.Point(197, 68);
//            this.asyncMode.Name = "asyncMode";
//            this.asyncMode.Size = new System.Drawing.Size(15, 14);
//            this.asyncMode.TabIndex = 234;
//            this.asyncMode.UseVisualStyleBackColor = true;
//            this.asyncMode.CheckedChanged += new System.EventHandler(this.asyncMode_CheckedChanged);
//            // 
//            // labelAsyncMode
//            // 
//            this.labelAsyncMode.AutoSize = true;
//            this.labelAsyncMode.Location = new System.Drawing.Point(20, 65);
//            this.labelAsyncMode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.labelAsyncMode.Name = "labelAsyncMode";
//            this.labelAsyncMode.Size = new System.Drawing.Size(118, 18);
//            this.labelAsyncMode.TabIndex = 233;
//            this.labelAsyncMode.Text = "Use S/W Trigger";
//            // 
//            // groupBoxDefect
//            // 
//            this.groupBoxDefect.BackColor = System.Drawing.SystemColors.Control;
//            this.groupBoxDefect.Controls.Add(this.maxPattern);
//            this.groupBoxDefect.Controls.Add(this.label21);
//            this.groupBoxDefect.Controls.Add(this.circleRadius);
//            this.groupBoxDefect.Controls.Add(this.label10);
//            this.groupBoxDefect.Controls.Add(this.label9);
//            this.groupBoxDefect.Location = new System.Drawing.Point(18, 294);
//            this.groupBoxDefect.Name = "groupBoxDefect";
//            this.groupBoxDefect.Size = new System.Drawing.Size(383, 103);
//            this.groupBoxDefect.TabIndex = 226;
//            this.groupBoxDefect.TabStop = false;
//            this.groupBoxDefect.Text = "Defect";
//            // 
//            // maxPattern
//            // 
//            this.maxPattern.Location = new System.Drawing.Point(273, 66);
//            this.maxPattern.Margin = new System.Windows.Forms.Padding(7);
//            this.maxPattern.Maximum = new decimal(new int[] {
//            100000,
//            0,
//            0,
//            0});
//            this.maxPattern.Minimum = new decimal(new int[] {
//            10,
//            0,
//            0,
//            0});
//            this.maxPattern.Name = "maxPattern";
//            this.maxPattern.Size = new System.Drawing.Size(63, 24);
//            this.maxPattern.TabIndex = 226;
//            this.maxPattern.Value = new decimal(new int[] {
//            50,
//            0,
//            0,
//            0});
//            this.maxPattern.ValueChanged += new System.EventHandler(this.maxPattern_ValueChanged);
//            // 
//            // label21
//            // 
//            this.label21.AutoSize = true;
//            this.label21.Location = new System.Drawing.Point(13, 68);
//            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label21.Name = "label21";
//            this.label21.Size = new System.Drawing.Size(87, 18);
//            this.label21.TabIndex = 225;
//            this.label21.Text = "Max Pattern";
//            // 
//            // circleRadius
//            // 
//            this.circleRadius.Location = new System.Drawing.Point(273, 30);
//            this.circleRadius.Margin = new System.Windows.Forms.Padding(7);
//            this.circleRadius.Maximum = new decimal(new int[] {
//            100000,
//            0,
//            0,
//            0});
//            this.circleRadius.Minimum = new decimal(new int[] {
//            10,
//            0,
//            0,
//            0});
//            this.circleRadius.Name = "circleRadius";
//            this.circleRadius.Size = new System.Drawing.Size(63, 24);
//            this.circleRadius.TabIndex = 224;
//            this.circleRadius.Value = new decimal(new int[] {
//            50,
//            0,
//            0,
//            0});
//            this.circleRadius.ValueChanged += new System.EventHandler(this.circleRadius_ValueChanged);
//            // 
//            // label10
//            // 
//            this.label10.AutoSize = true;
//            this.label10.Location = new System.Drawing.Point(337, 33);
//            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label10.Name = "label10";
//            this.label10.Size = new System.Drawing.Size(39, 18);
//            this.label10.TabIndex = 223;
//            this.label10.Text = "Pixel";
//            // 
//            // label9
//            // 
//            this.label9.AutoSize = true;
//            this.label9.Location = new System.Drawing.Point(13, 32);
//            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label9.Name = "label9";
//            this.label9.Size = new System.Drawing.Size(54, 18);
//            this.label9.TabIndex = 222;
//            this.label9.Text = "Radius";
//            // 
//            // groupSystemSettings
//            // 
//            this.groupSystemSettings.Controls.Add(this.labelIpSetting);
//            this.groupSystemSettings.Controls.Add(this.buttonIPSet);
//            this.groupSystemSettings.Controls.Add(this.systemType);
//            this.groupSystemSettings.Controls.Add(this.labelSystemType);
//            this.groupSystemSettings.Controls.Add(this.language);
//            this.groupSystemSettings.Controls.Add(this.labelLanguage);
//            this.groupSystemSettings.Location = new System.Drawing.Point(18, 12);
//            this.groupSystemSettings.Name = "groupSystemSettings";
//            this.groupSystemSettings.Size = new System.Drawing.Size(383, 134);
//            this.groupSystemSettings.TabIndex = 196;
//            this.groupSystemSettings.TabStop = false;
//            this.groupSystemSettings.Text = "System Settings";
//            // 
//            // labelIpSetting
//            // 
//            this.labelIpSetting.AutoSize = true;
//            this.labelIpSetting.Location = new System.Drawing.Point(21, 101);
//            this.labelIpSetting.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
//            this.labelIpSetting.Name = "labelIpSetting";
//            this.labelIpSetting.Size = new System.Drawing.Size(68, 18);
//            this.labelIpSetting.TabIndex = 210;
//            this.labelIpSetting.Text = "IP setting";
//            // 
//            // buttonIPSet
//            // 
//            this.buttonIPSet.Location = new System.Drawing.Point(263, 95);
//            this.buttonIPSet.Margin = new System.Windows.Forms.Padding(4);
//            this.buttonIPSet.Name = "buttonIPSet";
//            this.buttonIPSet.Size = new System.Drawing.Size(113, 30);
//            this.buttonIPSet.TabIndex = 209;
//            this.buttonIPSet.Text = "Set";
//            this.buttonIPSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
//            this.buttonIPSet.UseVisualStyleBackColor = true;
//            this.buttonIPSet.Click += new System.EventHandler(this.buttonIPSet_Click);
//            // 
//            // systemType
//            // 
//            this.systemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.systemType.Enabled = false;
//            this.systemType.FormattingEnabled = true;
//            this.systemType.Items.AddRange(new object[] {
//            "Inspector",
//            "Monitor"});
//            this.systemType.Location = new System.Drawing.Point(130, 28);
//            this.systemType.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
//            this.systemType.Name = "systemType";
//            this.systemType.Size = new System.Drawing.Size(246, 26);
//            this.systemType.TabIndex = 202;
//            this.systemType.SelectedIndexChanged += new System.EventHandler(this.systemType_SelectedIndexChanged);
//            // 
//            // labelSystemType
//            // 
//            this.labelSystemType.AutoSize = true;
//            this.labelSystemType.Location = new System.Drawing.Point(21, 31);
//            this.labelSystemType.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
//            this.labelSystemType.Name = "labelSystemType";
//            this.labelSystemType.Size = new System.Drawing.Size(94, 18);
//            this.labelSystemType.TabIndex = 201;
//            this.labelSystemType.Text = "System Type";
//            // 
//            // language
//            // 
//            this.language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.language.FormattingEnabled = true;
//            this.language.Items.AddRange(new object[] {
//            "English",
//            "Korean[ko-kr]",
//            "Chinese(Simplified)[zh-cn]"});
//            this.language.Location = new System.Drawing.Point(130, 62);
//            this.language.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
//            this.language.Name = "language";
//            this.language.Size = new System.Drawing.Size(246, 26);
//            this.language.TabIndex = 200;
//            // 
//            // labelLanguage
//            // 
//            this.labelLanguage.AutoSize = true;
//            this.labelLanguage.Location = new System.Drawing.Point(21, 65);
//            this.labelLanguage.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
//            this.labelLanguage.Name = "labelLanguage";
//            this.labelLanguage.Size = new System.Drawing.Size(72, 18);
//            this.labelLanguage.TabIndex = 199;
//            this.labelLanguage.Text = "Language";
//            // 
//            // groupImage
//            // 
//            this.groupImage.Controls.Add(this.label8);
//            this.groupImage.Controls.Add(this.bufferSize);
//            this.groupImage.Controls.Add(this.labelMaskThV);
//            this.groupImage.Controls.Add(this.maskThV);
//            this.groupImage.Controls.Add(this.labelMaskThH);
//            this.groupImage.Controls.Add(this.label13);
//            this.groupImage.Controls.Add(this.saturationRange);
//            this.groupImage.Controls.Add(this.labelMaskTh);
//            this.groupImage.Controls.Add(this.maskThH);
//            this.groupImage.Location = new System.Drawing.Point(18, 152);
//            this.groupImage.Name = "groupImage";
//            this.groupImage.Size = new System.Drawing.Size(383, 136);
//            this.groupImage.TabIndex = 223;
//            this.groupImage.TabStop = false;
//            this.groupImage.Text = "Image Processing";
//            // 
//            // label8
//            // 
//            this.label8.AutoSize = true;
//            this.label8.Location = new System.Drawing.Point(21, 100);
//            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label8.Name = "label8";
//            this.label8.Size = new System.Drawing.Size(80, 18);
//            this.label8.TabIndex = 232;
//            this.label8.Text = "Buffer Size";
//            // 
//            // bufferSize
//            // 
//            this.bufferSize.Location = new System.Drawing.Point(273, 98);
//            this.bufferSize.Margin = new System.Windows.Forms.Padding(7);
//            this.bufferSize.Maximum = new decimal(new int[] {
//            250,
//            0,
//            0,
//            0});
//            this.bufferSize.Name = "bufferSize";
//            this.bufferSize.Size = new System.Drawing.Size(63, 24);
//            this.bufferSize.TabIndex = 233;
//            this.bufferSize.Value = new decimal(new int[] {
//            100,
//            0,
//            0,
//            0});
//            this.bufferSize.ValueChanged += new System.EventHandler(this.bufferSize_ValueChanged);
//            // 
//            // labelMaskThV
//            // 
//            this.labelMaskThV.AutoSize = true;
//            this.labelMaskThV.Location = new System.Drawing.Point(254, 27);
//            this.labelMaskThV.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.labelMaskThV.Name = "labelMaskThV";
//            this.labelMaskThV.Size = new System.Drawing.Size(17, 18);
//            this.labelMaskThV.TabIndex = 231;
//            this.labelMaskThV.Text = "V";
//            // 
//            // maskThV
//            // 
//            this.maskThV.Location = new System.Drawing.Point(273, 25);
//            this.maskThV.Margin = new System.Windows.Forms.Padding(7);
//            this.maskThV.Maximum = new decimal(new int[] {
//            250,
//            0,
//            0,
//            0});
//            this.maskThV.Minimum = new decimal(new int[] {
//            1,
//            0,
//            0,
//            0});
//            this.maskThV.Name = "maskThV";
//            this.maskThV.Size = new System.Drawing.Size(63, 24);
//            this.maskThV.TabIndex = 230;
//            this.maskThV.Value = new decimal(new int[] {
//            100,
//            0,
//            0,
//            0});
//            this.maskThV.ValueChanged += new System.EventHandler(this.maskThV_ValueChanged);
//            // 
//            // labelMaskThH
//            // 
//            this.labelMaskThH.AutoSize = true;
//            this.labelMaskThH.Location = new System.Drawing.Point(160, 27);
//            this.labelMaskThH.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.labelMaskThH.Name = "labelMaskThH";
//            this.labelMaskThH.Size = new System.Drawing.Size(19, 18);
//            this.labelMaskThH.TabIndex = 229;
//            this.labelMaskThH.Text = "H";
//            // 
//            // label13
//            // 
//            this.label13.AutoSize = true;
//            this.label13.Location = new System.Drawing.Point(21, 62);
//            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.label13.Name = "label13";
//            this.label13.Size = new System.Drawing.Size(122, 18);
//            this.label13.TabIndex = 227;
//            this.label13.Text = "Saturation Range";
//            // 
//            // saturationRange
//            // 
//            this.saturationRange.Location = new System.Drawing.Point(273, 60);
//            this.saturationRange.Margin = new System.Windows.Forms.Padding(7);
//            this.saturationRange.Maximum = new decimal(new int[] {
//            250,
//            0,
//            0,
//            0});
//            this.saturationRange.Name = "saturationRange";
//            this.saturationRange.Size = new System.Drawing.Size(63, 24);
//            this.saturationRange.TabIndex = 228;
//            this.saturationRange.Value = new decimal(new int[] {
//            100,
//            0,
//            0,
//            0});
//            this.saturationRange.ValueChanged += new System.EventHandler(this.saturationRange_ValueChanged);
//            // 
//            // labelMaskTh
//            // 
//            this.labelMaskTh.AutoSize = true;
//            this.labelMaskTh.Location = new System.Drawing.Point(21, 27);
//            this.labelMaskTh.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
//            this.labelMaskTh.Name = "labelMaskTh";
//            this.labelMaskTh.Size = new System.Drawing.Size(66, 18);
//            this.labelMaskTh.TabIndex = 222;
//            this.labelMaskTh.Text = "Mask Th";
//            // 
//            // maskThH
//            // 
//            this.maskThH.Location = new System.Drawing.Point(180, 25);
//            this.maskThH.Margin = new System.Windows.Forms.Padding(7);
//            this.maskThH.Maximum = new decimal(new int[] {
//            250,
//            0,
//            0,
//            0});
//            this.maskThH.Minimum = new decimal(new int[] {
//            1,
//            0,
//            0,
//            0});
//            this.maskThH.Name = "maskThH";
//            this.maskThH.Size = new System.Drawing.Size(63, 24);
//            this.maskThH.TabIndex = 223;
//            this.maskThH.Value = new decimal(new int[] {
//            100,
//            0,
//            0,
//            0});
//            this.maskThH.ValueChanged += new System.EventHandler(this.maskThH_ValueChanged);
//            // 
//            // imageCheckInterval
//            // 
//            this.imageCheckInterval.Location = new System.Drawing.Point(211, 119);
//            this.imageCheckInterval.Name = "imageCheckInterval";
//            this.imageCheckInterval.Size = new System.Drawing.Size(68, 21);
//            this.imageCheckInterval.TabIndex = 13;
//            // 
//            // labelImageCheckInterval
//            // 
//            this.labelImageCheckInterval.AutoSize = true;
//            this.labelImageCheckInterval.Location = new System.Drawing.Point(15, 123);
//            this.labelImageCheckInterval.Name = "labelImageCheckInterval";
//            this.labelImageCheckInterval.Size = new System.Drawing.Size(151, 17);
//            this.labelImageCheckInterval.TabIndex = 6;
//            this.labelImageCheckInterval.Text = "Image Check Interval";
//            // 
//            // labelSec
//            // 
//            this.labelSec.AutoSize = true;
//            this.labelSec.Location = new System.Drawing.Point(286, 126);
//            this.labelSec.Name = "labelSec";
//            this.labelSec.Size = new System.Drawing.Size(29, 17);
//            this.labelSec.TabIndex = 10;
//            this.labelSec.Text = "sec";
//            // 
//            // labelMs
//            // 
//            this.labelMs.Location = new System.Drawing.Point(0, 0);
//            this.labelMs.Name = "labelMs";
//            this.labelMs.Size = new System.Drawing.Size(100, 23);
//            this.labelMs.TabIndex = 0;
//            // 
//            // contextMenuStrip1
//            // 
//            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
//            this.contextMenuStrip1.Name = "contextMenuStrip1";
//            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
//            // 
//            // tabControlParam
//            // 
//            this.tabControlParam.Controls.Add(this.ultraTabSharedControlsPage1);
//            this.tabControlParam.Controls.Add(this.ultraTabPageControl2);
//            this.tabControlParam.Controls.Add(this.ultraTabPageControl3);
//            this.tabControlParam.Controls.Add(this.ultraTabPageControl4);
//            this.tabControlParam.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tabControlParam.Location = new System.Drawing.Point(0, 0);
//            this.tabControlParam.Name = "tabControlParam";
//            this.tabControlParam.SharedControlsPage = this.ultraTabSharedControlsPage1;
//            this.tabControlParam.Size = new System.Drawing.Size(1565, 1064);
//            this.tabControlParam.TabIndex = 227;
//            appearance1.BackColor = System.Drawing.Color.Transparent;
//            ultraTab3.Appearance = appearance1;
//            ultraTab3.Key = "Monitoring";
//            ultraTab3.TabPage = this.ultraTabPageControl3;
//            ultraTab3.Text = "Monitoring";
//            appearance2.BackColor = System.Drawing.Color.Transparent;
//            ultraTab2.Appearance = appearance2;
//            ultraTab2.Key = "Inspector";
//            ultraTab2.TabPage = this.ultraTabPageControl2;
//            ultraTab2.Text = "Inspector";
//            ultraTab4.Key = "Developer";
//            ultraTab4.TabPage = this.ultraTabPageControl4;
//            ultraTab4.Text = "Developer";
//            ultraTab4.Visible = false;
//            this.tabControlParam.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
//            ultraTab3,
//            ultraTab2,
//            ultraTab4});
//            // 
//            // ultraTabSharedControlsPage1
//            // 
//            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
//            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
//            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1561, 1035);
//            // 
//            // buttonPanel
//            // 
//            this.buttonPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.buttonPanel.Controls.Add(this.buttonTransfer);
//            this.buttonPanel.Cursor = System.Windows.Forms.Cursors.Default;
//            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Right;
//            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
//            this.buttonPanel.Location = new System.Drawing.Point(1457, 0);
//            this.buttonPanel.Name = "buttonPanel";
//            this.buttonPanel.Size = new System.Drawing.Size(108, 1064);
//            this.buttonPanel.TabIndex = 228;
//            // 
//            // buttonTransfer
//            // 
//            this.buttonTransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.buttonTransfer.Image = global::UniScanG.Properties.Resources.Reset;
//            this.buttonTransfer.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
//            this.buttonTransfer.Location = new System.Drawing.Point(3, 3);
//            this.buttonTransfer.Name = "buttonTransfer";
//            this.buttonTransfer.Size = new System.Drawing.Size(100, 94);
//            this.buttonTransfer.TabIndex = 193;
//            this.buttonTransfer.Text = "동기화";
//            this.buttonTransfer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
//            this.buttonTransfer.UseVisualStyleBackColor = true;
//            this.buttonTransfer.Click += new System.EventHandler(this.buttonSaveSetting_Click);
//            // 
//            // SettingPage
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.buttonPanel);
//            this.Controls.Add(this.tabControlParam);
//            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
//            this.Name = "SettingPage";
//            this.Size = new System.Drawing.Size(1565, 1064);
//            this.ultraTabPageControl3.ResumeLayout(false);
//            this.groupBox5.ResumeLayout(false);
//            this.groupBox4.ResumeLayout(false);
//            this.groupMonitorTransfer.ResumeLayout(false);
//            this.groupMonitorTransfer.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.inspectorDataGridView)).EndInit();
//            this.groupBox2.ResumeLayout(false);
//            this.groupBox2.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.storingDays)).EndInit();
//            this.ultraTabPageControl2.ResumeLayout(false);
//            this.groupBox1.ResumeLayout(false);
//            this.groupBox3.ResumeLayout(false);
//            this.groupBox3.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.clientIndex)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.camIndex)).EndInit();
//            this.groupCalibration.ResumeLayout(false);
//            this.groupCalibration.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.startYPosition)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.sheetHeight)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.startXPosition)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.fovX)).EndInit();
//            this.ultraTabPageControl4.ResumeLayout(false);
//            this.groupBox7.ResumeLayout(false);
//            this.groupBox7.PerformLayout();
//            this.groupBox6.ResumeLayout(false);
//            this.groupBox6.PerformLayout();
//            this.groupBoxDefect.ResumeLayout(false);
//            this.groupBoxDefect.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.maxPattern)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.circleRadius)).EndInit();
//            this.groupSystemSettings.ResumeLayout(false);
//            this.groupSystemSettings.PerformLayout();
//            this.groupImage.ResumeLayout(false);
//            this.groupImage.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.bufferSize)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.maskThV)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.saturationRange)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.maskThH)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.tabControlParam)).EndInit();
//            this.tabControlParam.ResumeLayout(false);
//            this.buttonPanel.ResumeLayout(false);
//            this.ResumeLayout(false);

//        }

//        #endregion
//        private System.Windows.Forms.NumericUpDown imageCheckInterval;
//        private System.Windows.Forms.Label labelImageCheckInterval;
//        private System.Windows.Forms.Label labelSec;
//        private System.Windows.Forms.Label labelMs;
//        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
//        private System.ComponentModel.BackgroundWorker backgroundWorker1;
//        private System.ComponentModel.BackgroundWorker backgroundWorker2;
//        private System.Windows.Forms.Button buttonTransfer;
//        private System.Windows.Forms.GroupBox groupSystemSettings;
//        private System.Windows.Forms.ComboBox systemType;
//        private System.Windows.Forms.Label labelSystemType;
//        private System.Windows.Forms.ComboBox language;
//        private System.Windows.Forms.Label labelLanguage;
//        private System.Windows.Forms.Label labelIpSetting;
//        private System.Windows.Forms.Button buttonIPSet;
//        private System.Windows.Forms.CheckBox saveImageText;
//        private System.Windows.Forms.GroupBox groupImage;
//        private System.Windows.Forms.Label labelMaskTh;
//        private System.Windows.Forms.NumericUpDown maskThH;
//        private System.Windows.Forms.Label label13;
//        private System.Windows.Forms.NumericUpDown saturationRange;
//        private Infragistics.Win.UltraWinTabControl.UltraTabControl tabControlParam;
//        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
//        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
//        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
//        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
//        private System.Windows.Forms.GroupBox groupCalibration;
//        private System.Windows.Forms.Label label6;
//        private System.Windows.Forms.NumericUpDown sheetHeight;
//        private System.Windows.Forms.Label label1;
//        private System.Windows.Forms.Label label5;
//        private System.Windows.Forms.NumericUpDown startXPosition;
//        private System.Windows.Forms.NumericUpDown fovX;
//        private System.Windows.Forms.Label label7;
//        private System.Windows.Forms.Label label4;
//        private System.Windows.Forms.Label label3;
//        private System.Windows.Forms.GroupBox groupBoxDefect;
//        private System.Windows.Forms.NumericUpDown maxPattern;
//        private System.Windows.Forms.Label label21;
//        private System.Windows.Forms.NumericUpDown circleRadius;
//        private System.Windows.Forms.Label label10;
//        private System.Windows.Forms.Label label9;
//        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
//        private System.Windows.Forms.GroupBox groupBox2;
//        private System.Windows.Forms.Label label23;
//        private System.Windows.Forms.Label label24;
//        private System.Windows.Forms.NumericUpDown storingDays;
//        private System.Windows.Forms.Label label16;
//        private System.Windows.Forms.NumericUpDown startYPosition;
//        private System.Windows.Forms.Label label17;
//        private System.Windows.Forms.GroupBox groupMonitorTransfer;
//        private System.Windows.Forms.DataGridView inspectorDataGridView;
//        private System.Windows.Forms.TextBox vncViewerPath;
//        private System.Windows.Forms.Label labelVncViewerPath;
//        private System.Windows.Forms.GroupBox groupBox3;
//        private System.Windows.Forms.TextBox inspectorIpAddress;
//        private System.Windows.Forms.Label labelInspectorIpAddress;
//        private System.Windows.Forms.Label cameraResolution;
//        private System.Windows.Forms.Button buttonCalibration;
//        private System.Windows.Forms.Label labelCalibration;
//        private System.Windows.Forms.NumericUpDown clientIndex;
//        private System.Windows.Forms.Label labelClientIndex;
//        private System.Windows.Forms.NumericUpDown camIndex;
//        private System.Windows.Forms.Label labelCamIndex;
//        private System.Windows.Forms.TextBox monitorIpAddress;
//        private System.Windows.Forms.Label labelMonitorIpAddress;
//        private System.Windows.Forms.Button buttonApply;
//        private System.Windows.Forms.Button buttonDelete;
//        private System.Windows.Forms.Button buttonNew;
//        private System.Windows.Forms.Label labelMaskThV;
//        private System.Windows.Forms.NumericUpDown maskThV;
//        private System.Windows.Forms.Label labelMaskThH;
//        private System.Windows.Forms.CheckBox asyncMode;
//        private System.Windows.Forms.Label labelAsyncMode;
//        private System.Windows.Forms.DataGridViewTextBoxColumn columnCamID;
//        private System.Windows.Forms.DataGridViewTextBoxColumn columnPCID;
//        private System.Windows.Forms.DataGridViewTextBoxColumn columnIPAddress;
//        private System.Windows.Forms.DataGridViewTextBoxColumn columnResultPath;
//        private System.Windows.Forms.GroupBox groupBox4;
//        private System.Windows.Forms.Button buttonMonGrabTest;
//        private System.Windows.Forms.GroupBox groupBox1;
//        private System.Windows.Forms.Button buttonInspGrabTest;
//        private System.Windows.Forms.GroupBox groupBox5;
//        private System.Windows.Forms.Button buttonMonCommTest;
//        private System.Windows.Forms.CheckBox standAlone;
//        private System.Windows.Forms.Label labelStandAlone;
//        private System.Windows.Forms.GroupBox groupBox6;
//        private System.Windows.Forms.Label label2;
//        private System.Windows.Forms.Label label8;
//        private System.Windows.Forms.NumericUpDown bufferSize;
//        private System.Windows.Forms.Label label11;
//        private System.Windows.Forms.ComboBox logLevel;
//        private System.Windows.Forms.GroupBox groupBox7;
//        private System.Windows.Forms.Label label15;
//        private System.Windows.Forms.CheckBox saveInspectText;
//        private System.Windows.Forms.Label label12;
//        private System.Windows.Forms.CheckBox saveFiducialText;
//        private System.Windows.Forms.Label label19;
//        private System.Windows.Forms.Label label18;
//        private System.Windows.Forms.CheckBox saveInspectImage;
//        private System.Windows.Forms.CheckBox saveFiducialImage;
//        private System.Windows.Forms.CheckBox saveImageImage;
//    }
//}
