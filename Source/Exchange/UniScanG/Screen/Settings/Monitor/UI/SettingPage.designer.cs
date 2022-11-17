namespace UniScanG.Screen.Settings.Monitor.UI
{
    partial class SettingPage
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.tabPageMonitoring = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.layoutMonitoring = new System.Windows.Forms.FlowLayoutPanel();
            this.groupClassification = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.labelWhiteDefectClassificationWhite = new System.Windows.Forms.Label();
            this.whiteDefectClassification = new System.Windows.Forms.TrackBar();
            this.labelWhiteDefectClassificationPinhole = new System.Windows.Forms.Label();
            this.whiteDefectClassificationText = new System.Windows.Forms.TextBox();
            this.groupSize = new System.Windows.Forms.GroupBox();
            this.labelBlackCircleSizeFilter = new System.Windows.Forms.Label();
            this.blackCircleSizeFilter = new System.Windows.Forms.NumericUpDown();
            this.labelPinHoleSizeFilterUnit = new System.Windows.Forms.Label();
            this.labelBlackCircleSizeFilterUnit = new System.Windows.Forms.Label();
            this.pinHoleSizeFilter = new System.Windows.Forms.NumericUpDown();
            this.labelPinHoleSizeFilter = new System.Windows.Forms.Label();
            this.groupElogation = new System.Windows.Forms.GroupBox();
            this.blackDefectElogationText = new System.Windows.Forms.TextBox();
            this.labelElogationLine = new System.Windows.Forms.Label();
            this.labelElogationCircle = new System.Windows.Forms.Label();
            this.blackDefectElogation = new System.Windows.Forms.TrackBar();
            this.groupCompectness = new System.Windows.Forms.GroupBox();
            this.blackDefectClassification = new System.Windows.Forms.TrackBar();
            this.labelBlackDefectClassificationLine = new System.Windows.Forms.Label();
            this.labelBlackDefectClassificationCircle = new System.Windows.Forms.Label();
            this.blackDefectClassificationText = new System.Windows.Forms.TextBox();
            this.groupDataRemove = new System.Windows.Forms.GroupBox();
            this.labelStoringDaysUnit = new System.Windows.Forms.Label();
            this.labelStoringDays = new System.Windows.Forms.Label();
            this.storingDays = new System.Windows.Forms.NumericUpDown();
            this.groupIO = new System.Windows.Forms.GroupBox();
            this.useWholeDefectIO = new System.Windows.Forms.CheckBox();
            this.groupBoxCheckPoint = new System.Windows.Forms.GroupBox();
            this.checkPointGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonPortView = new System.Windows.Forms.Button();
            this.groupWholePattern = new System.Windows.Forms.GroupBox();
            this.labelWholeResultNum = new System.Windows.Forms.Label();
            this.wholeDefectRatio = new System.Windows.Forms.NumericUpDown();
            this.labelDefectRatio = new System.Windows.Forms.Label();
            this.labelWholeDefectRatioUnit = new System.Windows.Forms.Label();
            this.wholeDefectNum = new System.Windows.Forms.NumericUpDown();
            this.groupSamePoint = new System.Windows.Forms.GroupBox();
            this.samePointGridView = new System.Windows.Forms.DataGridView();
            this.columnSamePointNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSamePointRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSheetAttack = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnBlackCircle = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnBlackLine = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnPinHole = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnWhite = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnShape = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.labelCircleRadius = new System.Windows.Forms.Label();
            this.showSamePointRadius = new System.Windows.Forms.CheckBox();
            this.samePointRadius = new System.Windows.Forms.NumericUpDown();
            this.labelCircleRadiusUnit = new System.Windows.Forms.Label();
            this.useAlarmForm = new System.Windows.Forms.CheckBox();
            this.useIOOutput = new System.Windows.Forms.CheckBox();
            this.monitoringSettingList = new System.Windows.Forms.ListBox();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonTransfer = new System.Windows.Forms.Button();
            this.tabPageDeveloper = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.imageCheckInterval = new System.Windows.Forms.NumericUpDown();
            this.labelImageCheckInterval = new System.Windows.Forms.Label();
            this.labelSec = new System.Windows.Forms.Label();
            this.labelMs = new System.Windows.Forms.Label();
            this.settingTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tabPageMonitoring.SuspendLayout();
            this.layoutMonitoring.SuspendLayout();
            this.groupClassification.SuspendLayout();
            this.groupBox10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.whiteDefectClassification)).BeginInit();
            this.groupSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackCircleSizeFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pinHoleSizeFilter)).BeginInit();
            this.groupElogation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackDefectElogation)).BeginInit();
            this.groupCompectness.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackDefectClassification)).BeginInit();
            this.groupDataRemove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.storingDays)).BeginInit();
            this.groupIO.SuspendLayout();
            this.groupBoxCheckPoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkPointGridView)).BeginInit();
            this.groupWholePattern.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wholeDefectRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wholeDefectNum)).BeginInit();
            this.groupSamePoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samePointGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.samePointRadius)).BeginInit();
            this.buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingTabControl)).BeginInit();
            this.settingTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageMonitoring
            // 
            this.tabPageMonitoring.Controls.Add(this.layoutMonitoring);
            this.tabPageMonitoring.Controls.Add(this.monitoringSettingList);
            this.tabPageMonitoring.Controls.Add(this.buttonPanel);
            this.tabPageMonitoring.Location = new System.Drawing.Point(130, 0);
            this.tabPageMonitoring.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageMonitoring.Name = "tabPageMonitoring";
            this.tabPageMonitoring.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageMonitoring.Size = new System.Drawing.Size(1087, 1637);
            // 
            // layoutMonitoring
            // 
            this.layoutMonitoring.Controls.Add(this.groupClassification);
            this.layoutMonitoring.Controls.Add(this.groupDataRemove);
            this.layoutMonitoring.Controls.Add(this.groupIO);
            this.layoutMonitoring.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMonitoring.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.layoutMonitoring.Location = new System.Drawing.Point(162, 10);
            this.layoutMonitoring.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMonitoring.Name = "layoutMonitoring";
            this.layoutMonitoring.Padding = new System.Windows.Forms.Padding(10);
            this.layoutMonitoring.Size = new System.Drawing.Size(827, 1617);
            this.layoutMonitoring.TabIndex = 2;
            // 
            // groupClassification
            // 
            this.groupClassification.BackColor = System.Drawing.Color.Transparent;
            this.groupClassification.Controls.Add(this.groupBox10);
            this.groupClassification.Controls.Add(this.groupSize);
            this.groupClassification.Controls.Add(this.groupElogation);
            this.groupClassification.Controls.Add(this.groupCompectness);
            this.groupClassification.Location = new System.Drawing.Point(10, 10);
            this.groupClassification.Margin = new System.Windows.Forms.Padding(0);
            this.groupClassification.Name = "groupClassification";
            this.groupClassification.Padding = new System.Windows.Forms.Padding(0);
            this.groupClassification.Size = new System.Drawing.Size(433, 418);
            this.groupClassification.TabIndex = 0;
            this.groupClassification.TabStop = false;
            this.groupClassification.Text = "Classification";
            // 
            // groupBox10
            // 
            this.groupBox10.BackColor = System.Drawing.Color.Transparent;
            this.groupBox10.Controls.Add(this.labelWhiteDefectClassificationWhite);
            this.groupBox10.Controls.Add(this.whiteDefectClassification);
            this.groupBox10.Controls.Add(this.labelWhiteDefectClassificationPinhole);
            this.groupBox10.Controls.Add(this.whiteDefectClassificationText);
            this.groupBox10.Location = new System.Drawing.Point(15, 210);
            this.groupBox10.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox10.Size = new System.Drawing.Size(403, 83);
            this.groupBox10.TabIndex = 0;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Circularity";
            // 
            // labelWhiteDefectClassificationWhite
            // 
            this.labelWhiteDefectClassificationWhite.AutoSize = true;
            this.labelWhiteDefectClassificationWhite.Location = new System.Drawing.Point(8, 32);
            this.labelWhiteDefectClassificationWhite.Name = "labelWhiteDefectClassificationWhite";
            this.labelWhiteDefectClassificationWhite.Size = new System.Drawing.Size(63, 25);
            this.labelWhiteDefectClassificationWhite.TabIndex = 0;
            this.labelWhiteDefectClassificationWhite.Text = "White";
            // 
            // whiteDefectClassification
            // 
            this.whiteDefectClassification.BackColor = System.Drawing.Color.White;
            this.whiteDefectClassification.Location = new System.Drawing.Point(78, 34);
            this.whiteDefectClassification.Margin = new System.Windows.Forms.Padding(0);
            this.whiteDefectClassification.Maximum = 20;
            this.whiteDefectClassification.Name = "whiteDefectClassification";
            this.whiteDefectClassification.Size = new System.Drawing.Size(177, 45);
            this.whiteDefectClassification.TabIndex = 0;
            // 
            // labelWhiteDefectClassificationPinhole
            // 
            this.labelWhiteDefectClassificationPinhole.AutoSize = true;
            this.labelWhiteDefectClassificationPinhole.Location = new System.Drawing.Point(258, 32);
            this.labelWhiteDefectClassificationPinhole.Name = "labelWhiteDefectClassificationPinhole";
            this.labelWhiteDefectClassificationPinhole.Size = new System.Drawing.Size(86, 25);
            this.labelWhiteDefectClassificationPinhole.TabIndex = 0;
            this.labelWhiteDefectClassificationPinhole.Text = "Pinhole";
            // 
            // whiteDefectClassificationText
            // 
            this.whiteDefectClassificationText.BackColor = System.Drawing.Color.White;
            this.whiteDefectClassificationText.Location = new System.Drawing.Point(351, 31);
            this.whiteDefectClassificationText.Margin = new System.Windows.Forms.Padding(0);
            this.whiteDefectClassificationText.Name = "whiteDefectClassificationText";
            this.whiteDefectClassificationText.ReadOnly = true;
            this.whiteDefectClassificationText.Size = new System.Drawing.Size(44, 32);
            this.whiteDefectClassificationText.TabIndex = 0;
            // 
            // groupSize
            // 
            this.groupSize.BackColor = System.Drawing.Color.Transparent;
            this.groupSize.Controls.Add(this.labelBlackCircleSizeFilter);
            this.groupSize.Controls.Add(this.blackCircleSizeFilter);
            this.groupSize.Controls.Add(this.labelPinHoleSizeFilterUnit);
            this.groupSize.Controls.Add(this.labelBlackCircleSizeFilterUnit);
            this.groupSize.Controls.Add(this.pinHoleSizeFilter);
            this.groupSize.Controls.Add(this.labelPinHoleSizeFilter);
            this.groupSize.Location = new System.Drawing.Point(15, 297);
            this.groupSize.Margin = new System.Windows.Forms.Padding(0);
            this.groupSize.Name = "groupSize";
            this.groupSize.Padding = new System.Windows.Forms.Padding(0);
            this.groupSize.Size = new System.Drawing.Size(403, 109);
            this.groupSize.TabIndex = 0;
            this.groupSize.TabStop = false;
            this.groupSize.Text = "Size";
            // 
            // labelBlackCircleSizeFilter
            // 
            this.labelBlackCircleSizeFilter.AutoSize = true;
            this.labelBlackCircleSizeFilter.Location = new System.Drawing.Point(8, 30);
            this.labelBlackCircleSizeFilter.Name = "labelBlackCircleSizeFilter";
            this.labelBlackCircleSizeFilter.Size = new System.Drawing.Size(60, 25);
            this.labelBlackCircleSizeFilter.TabIndex = 0;
            this.labelBlackCircleSizeFilter.Text = "Circle";
            // 
            // blackCircleSizeFilter
            // 
            this.blackCircleSizeFilter.Location = new System.Drawing.Point(284, 28);
            this.blackCircleSizeFilter.Margin = new System.Windows.Forms.Padding(0);
            this.blackCircleSizeFilter.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.blackCircleSizeFilter.Name = "blackCircleSizeFilter";
            this.blackCircleSizeFilter.Size = new System.Drawing.Size(54, 32);
            this.blackCircleSizeFilter.TabIndex = 0;
            // 
            // labelPinHoleSizeFilterUnit
            // 
            this.labelPinHoleSizeFilterUnit.AutoSize = true;
            this.labelPinHoleSizeFilterUnit.Location = new System.Drawing.Point(344, 65);
            this.labelPinHoleSizeFilterUnit.Name = "labelPinHoleSizeFilterUnit";
            this.labelPinHoleSizeFilterUnit.Size = new System.Drawing.Size(40, 25);
            this.labelPinHoleSizeFilterUnit.TabIndex = 0;
            this.labelPinHoleSizeFilterUnit.Text = "um";
            // 
            // labelBlackCircleSizeFilterUnit
            // 
            this.labelBlackCircleSizeFilterUnit.AutoSize = true;
            this.labelBlackCircleSizeFilterUnit.Location = new System.Drawing.Point(344, 30);
            this.labelBlackCircleSizeFilterUnit.Name = "labelBlackCircleSizeFilterUnit";
            this.labelBlackCircleSizeFilterUnit.Size = new System.Drawing.Size(40, 25);
            this.labelBlackCircleSizeFilterUnit.TabIndex = 0;
            this.labelBlackCircleSizeFilterUnit.Text = "um";
            // 
            // pinHoleSizeFilter
            // 
            this.pinHoleSizeFilter.Location = new System.Drawing.Point(284, 65);
            this.pinHoleSizeFilter.Margin = new System.Windows.Forms.Padding(0);
            this.pinHoleSizeFilter.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.pinHoleSizeFilter.Name = "pinHoleSizeFilter";
            this.pinHoleSizeFilter.Size = new System.Drawing.Size(54, 32);
            this.pinHoleSizeFilter.TabIndex = 0;
            // 
            // labelPinHoleSizeFilter
            // 
            this.labelPinHoleSizeFilter.AutoSize = true;
            this.labelPinHoleSizeFilter.Location = new System.Drawing.Point(8, 65);
            this.labelPinHoleSizeFilter.Name = "labelPinHoleSizeFilter";
            this.labelPinHoleSizeFilter.Size = new System.Drawing.Size(86, 25);
            this.labelPinHoleSizeFilter.TabIndex = 0;
            this.labelPinHoleSizeFilter.Text = "Pinhole";
            // 
            // groupElogation
            // 
            this.groupElogation.BackColor = System.Drawing.Color.Transparent;
            this.groupElogation.Controls.Add(this.blackDefectElogationText);
            this.groupElogation.Controls.Add(this.labelElogationLine);
            this.groupElogation.Controls.Add(this.labelElogationCircle);
            this.groupElogation.Controls.Add(this.blackDefectElogation);
            this.groupElogation.Location = new System.Drawing.Point(15, 36);
            this.groupElogation.Margin = new System.Windows.Forms.Padding(0);
            this.groupElogation.Name = "groupElogation";
            this.groupElogation.Padding = new System.Windows.Forms.Padding(0);
            this.groupElogation.Size = new System.Drawing.Size(403, 83);
            this.groupElogation.TabIndex = 0;
            this.groupElogation.TabStop = false;
            this.groupElogation.Text = "Elogation";
            // 
            // blackDefectElogationText
            // 
            this.blackDefectElogationText.BackColor = System.Drawing.Color.White;
            this.blackDefectElogationText.Location = new System.Drawing.Point(351, 33);
            this.blackDefectElogationText.Margin = new System.Windows.Forms.Padding(0);
            this.blackDefectElogationText.Name = "blackDefectElogationText";
            this.blackDefectElogationText.ReadOnly = true;
            this.blackDefectElogationText.Size = new System.Drawing.Size(44, 32);
            this.blackDefectElogationText.TabIndex = 0;
            // 
            // labelElogationLine
            // 
            this.labelElogationLine.AutoSize = true;
            this.labelElogationLine.Location = new System.Drawing.Point(10, 31);
            this.labelElogationLine.Name = "labelElogationLine";
            this.labelElogationLine.Size = new System.Drawing.Size(47, 25);
            this.labelElogationLine.TabIndex = 0;
            this.labelElogationLine.Text = "Line";
            // 
            // labelElogationCircle
            // 
            this.labelElogationCircle.AutoSize = true;
            this.labelElogationCircle.Location = new System.Drawing.Point(258, 31);
            this.labelElogationCircle.Name = "labelElogationCircle";
            this.labelElogationCircle.Size = new System.Drawing.Size(60, 25);
            this.labelElogationCircle.TabIndex = 0;
            this.labelElogationCircle.Text = "Circle";
            // 
            // blackDefectElogation
            // 
            this.blackDefectElogation.BackColor = System.Drawing.Color.White;
            this.blackDefectElogation.LargeChange = 10;
            this.blackDefectElogation.Location = new System.Drawing.Point(78, 33);
            this.blackDefectElogation.Margin = new System.Windows.Forms.Padding(0);
            this.blackDefectElogation.Maximum = 50;
            this.blackDefectElogation.Name = "blackDefectElogation";
            this.blackDefectElogation.Size = new System.Drawing.Size(177, 45);
            this.blackDefectElogation.TabIndex = 0;
            this.blackDefectElogation.Value = 10;
            // 
            // groupCompectness
            // 
            this.groupCompectness.BackColor = System.Drawing.Color.Transparent;
            this.groupCompectness.Controls.Add(this.blackDefectClassification);
            this.groupCompectness.Controls.Add(this.labelBlackDefectClassificationLine);
            this.groupCompectness.Controls.Add(this.labelBlackDefectClassificationCircle);
            this.groupCompectness.Controls.Add(this.blackDefectClassificationText);
            this.groupCompectness.Location = new System.Drawing.Point(15, 123);
            this.groupCompectness.Margin = new System.Windows.Forms.Padding(0);
            this.groupCompectness.Name = "groupCompectness";
            this.groupCompectness.Padding = new System.Windows.Forms.Padding(0);
            this.groupCompectness.Size = new System.Drawing.Size(403, 83);
            this.groupCompectness.TabIndex = 0;
            this.groupCompectness.TabStop = false;
            this.groupCompectness.Text = "Circularity";
            // 
            // blackDefectClassification
            // 
            this.blackDefectClassification.BackColor = System.Drawing.Color.White;
            this.blackDefectClassification.LargeChange = 10;
            this.blackDefectClassification.Location = new System.Drawing.Point(78, 34);
            this.blackDefectClassification.Margin = new System.Windows.Forms.Padding(0);
            this.blackDefectClassification.Maximum = 50;
            this.blackDefectClassification.Name = "blackDefectClassification";
            this.blackDefectClassification.Size = new System.Drawing.Size(177, 45);
            this.blackDefectClassification.TabIndex = 0;
            // 
            // labelBlackDefectClassificationLine
            // 
            this.labelBlackDefectClassificationLine.AutoSize = true;
            this.labelBlackDefectClassificationLine.Location = new System.Drawing.Point(10, 32);
            this.labelBlackDefectClassificationLine.Name = "labelBlackDefectClassificationLine";
            this.labelBlackDefectClassificationLine.Size = new System.Drawing.Size(47, 25);
            this.labelBlackDefectClassificationLine.TabIndex = 0;
            this.labelBlackDefectClassificationLine.Text = "Line";
            // 
            // labelBlackDefectClassificationCircle
            // 
            this.labelBlackDefectClassificationCircle.AutoSize = true;
            this.labelBlackDefectClassificationCircle.Location = new System.Drawing.Point(258, 32);
            this.labelBlackDefectClassificationCircle.Name = "labelBlackDefectClassificationCircle";
            this.labelBlackDefectClassificationCircle.Size = new System.Drawing.Size(60, 25);
            this.labelBlackDefectClassificationCircle.TabIndex = 0;
            this.labelBlackDefectClassificationCircle.Text = "Circle";
            // 
            // blackDefectClassificationText
            // 
            this.blackDefectClassificationText.BackColor = System.Drawing.Color.White;
            this.blackDefectClassificationText.Location = new System.Drawing.Point(351, 31);
            this.blackDefectClassificationText.Margin = new System.Windows.Forms.Padding(0);
            this.blackDefectClassificationText.Name = "blackDefectClassificationText";
            this.blackDefectClassificationText.ReadOnly = true;
            this.blackDefectClassificationText.Size = new System.Drawing.Size(44, 32);
            this.blackDefectClassificationText.TabIndex = 0;
            // 
            // groupDataRemove
            // 
            this.groupDataRemove.BackColor = System.Drawing.Color.Transparent;
            this.groupDataRemove.Controls.Add(this.labelStoringDaysUnit);
            this.groupDataRemove.Controls.Add(this.labelStoringDays);
            this.groupDataRemove.Controls.Add(this.storingDays);
            this.groupDataRemove.Location = new System.Drawing.Point(10, 428);
            this.groupDataRemove.Margin = new System.Windows.Forms.Padding(0);
            this.groupDataRemove.Name = "groupDataRemove";
            this.groupDataRemove.Padding = new System.Windows.Forms.Padding(0);
            this.groupDataRemove.Size = new System.Drawing.Size(403, 83);
            this.groupDataRemove.TabIndex = 0;
            this.groupDataRemove.TabStop = false;
            this.groupDataRemove.Text = "Data Remove";
            // 
            // labelStoringDaysUnit
            // 
            this.labelStoringDaysUnit.AutoSize = true;
            this.labelStoringDaysUnit.Location = new System.Drawing.Point(348, 38);
            this.labelStoringDaysUnit.Name = "labelStoringDaysUnit";
            this.labelStoringDaysUnit.Size = new System.Drawing.Size(45, 25);
            this.labelStoringDaysUnit.TabIndex = 0;
            this.labelStoringDaysUnit.Text = "Day";
            // 
            // labelStoringDays
            // 
            this.labelStoringDays.AutoSize = true;
            this.labelStoringDays.Location = new System.Drawing.Point(10, 38);
            this.labelStoringDays.Margin = new System.Windows.Forms.Padding(0);
            this.labelStoringDays.Name = "labelStoringDays";
            this.labelStoringDays.Size = new System.Drawing.Size(122, 25);
            this.labelStoringDays.TabIndex = 0;
            this.labelStoringDays.Text = "Storing Days";
            // 
            // storingDays
            // 
            this.storingDays.Location = new System.Drawing.Point(288, 38);
            this.storingDays.Margin = new System.Windows.Forms.Padding(0);
            this.storingDays.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.storingDays.Name = "storingDays";
            this.storingDays.Size = new System.Drawing.Size(54, 32);
            this.storingDays.TabIndex = 0;
            this.storingDays.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // groupIO
            // 
            this.groupIO.BackColor = System.Drawing.Color.Transparent;
            this.groupIO.Controls.Add(this.useWholeDefectIO);
            this.groupIO.Controls.Add(this.groupBoxCheckPoint);
            this.groupIO.Controls.Add(this.buttonPortView);
            this.groupIO.Controls.Add(this.groupWholePattern);
            this.groupIO.Controls.Add(this.groupSamePoint);
            this.groupIO.Controls.Add(this.useAlarmForm);
            this.groupIO.Controls.Add(this.useIOOutput);
            this.groupIO.Location = new System.Drawing.Point(10, 511);
            this.groupIO.Margin = new System.Windows.Forms.Padding(0);
            this.groupIO.Name = "groupIO";
            this.groupIO.Padding = new System.Windows.Forms.Padding(0);
            this.groupIO.Size = new System.Drawing.Size(665, 517);
            this.groupIO.TabIndex = 0;
            this.groupIO.TabStop = false;
            this.groupIO.Text = "IO";
            // 
            // useWholeDefectIO
            // 
            this.useWholeDefectIO.AutoSize = true;
            this.useWholeDefectIO.Location = new System.Drawing.Point(156, 117);
            this.useWholeDefectIO.Margin = new System.Windows.Forms.Padding(0);
            this.useWholeDefectIO.Name = "useWholeDefectIO";
            this.useWholeDefectIO.Size = new System.Drawing.Size(15, 14);
            this.useWholeDefectIO.TabIndex = 0;
            this.useWholeDefectIO.UseVisualStyleBackColor = true;
            // 
            // groupBoxCheckPoint
            // 
            this.groupBoxCheckPoint.Controls.Add(this.checkPointGridView);
            this.groupBoxCheckPoint.Location = new System.Drawing.Point(431, 25);
            this.groupBoxCheckPoint.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxCheckPoint.Name = "groupBoxCheckPoint";
            this.groupBoxCheckPoint.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxCheckPoint.Size = new System.Drawing.Size(225, 198);
            this.groupBoxCheckPoint.TabIndex = 0;
            this.groupBoxCheckPoint.TabStop = false;
            this.groupBoxCheckPoint.Text = "Check Point";
            // 
            // checkPointGridView
            // 
            this.checkPointGridView.AllowUserToAddRows = false;
            this.checkPointGridView.AllowUserToDeleteRows = false;
            this.checkPointGridView.AllowUserToResizeColumns = false;
            this.checkPointGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.checkPointGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.checkPointGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.checkPointGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.checkPointGridView.Location = new System.Drawing.Point(15, 33);
            this.checkPointGridView.Name = "checkPointGridView";
            this.checkPointGridView.RowHeadersVisible = false;
            this.checkPointGridView.RowTemplate.Height = 23;
            this.checkPointGridView.Size = new System.Drawing.Size(200, 147);
            this.checkPointGridView.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Num";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 60;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 119.7605F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Ratio (%)";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // buttonPortView
            // 
            this.buttonPortView.Location = new System.Drawing.Point(288, 34);
            this.buttonPortView.Name = "buttonPortView";
            this.buttonPortView.Size = new System.Drawing.Size(130, 44);
            this.buttonPortView.TabIndex = 0;
            this.buttonPortView.Text = "IO port view";
            this.buttonPortView.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonPortView.UseVisualStyleBackColor = true;
            // 
            // groupWholePattern
            // 
            this.groupWholePattern.Controls.Add(this.labelWholeResultNum);
            this.groupWholePattern.Controls.Add(this.wholeDefectRatio);
            this.groupWholePattern.Controls.Add(this.labelDefectRatio);
            this.groupWholePattern.Controls.Add(this.labelWholeDefectRatioUnit);
            this.groupWholePattern.Controls.Add(this.wholeDefectNum);
            this.groupWholePattern.Location = new System.Drawing.Point(15, 110);
            this.groupWholePattern.Margin = new System.Windows.Forms.Padding(0);
            this.groupWholePattern.Name = "groupWholePattern";
            this.groupWholePattern.Padding = new System.Windows.Forms.Padding(0);
            this.groupWholePattern.Size = new System.Drawing.Size(403, 113);
            this.groupWholePattern.TabIndex = 0;
            this.groupWholePattern.TabStop = false;
            this.groupWholePattern.Text = "Whole Pattern";
            // 
            // labelWholeResultNum
            // 
            this.labelWholeResultNum.AutoSize = true;
            this.labelWholeResultNum.Location = new System.Drawing.Point(10, 32);
            this.labelWholeResultNum.Margin = new System.Windows.Forms.Padding(0);
            this.labelWholeResultNum.Name = "labelWholeResultNum";
            this.labelWholeResultNum.Size = new System.Drawing.Size(55, 25);
            this.labelWholeResultNum.TabIndex = 0;
            this.labelWholeResultNum.Text = "Num";
            // 
            // wholeDefectRatio
            // 
            this.wholeDefectRatio.Location = new System.Drawing.Point(288, 69);
            this.wholeDefectRatio.Margin = new System.Windows.Forms.Padding(0);
            this.wholeDefectRatio.Name = "wholeDefectRatio";
            this.wholeDefectRatio.Size = new System.Drawing.Size(54, 32);
            this.wholeDefectRatio.TabIndex = 0;
            this.wholeDefectRatio.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // labelDefectRatio
            // 
            this.labelDefectRatio.AutoSize = true;
            this.labelDefectRatio.Location = new System.Drawing.Point(10, 69);
            this.labelDefectRatio.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefectRatio.Name = "labelDefectRatio";
            this.labelDefectRatio.Size = new System.Drawing.Size(57, 25);
            this.labelDefectRatio.TabIndex = 0;
            this.labelDefectRatio.Text = "Ratio";
            // 
            // labelWholeDefectRatioUnit
            // 
            this.labelWholeDefectRatioUnit.AutoSize = true;
            this.labelWholeDefectRatioUnit.Location = new System.Drawing.Point(348, 69);
            this.labelWholeDefectRatioUnit.Name = "labelWholeDefectRatioUnit";
            this.labelWholeDefectRatioUnit.Size = new System.Drawing.Size(28, 25);
            this.labelWholeDefectRatioUnit.TabIndex = 0;
            this.labelWholeDefectRatioUnit.Text = "%";
            // 
            // wholeDefectNum
            // 
            this.wholeDefectNum.Location = new System.Drawing.Point(288, 32);
            this.wholeDefectNum.Margin = new System.Windows.Forms.Padding(0);
            this.wholeDefectNum.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.wholeDefectNum.Name = "wholeDefectNum";
            this.wholeDefectNum.Size = new System.Drawing.Size(54, 32);
            this.wholeDefectNum.TabIndex = 0;
            this.wholeDefectNum.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // groupSamePoint
            // 
            this.groupSamePoint.Controls.Add(this.samePointGridView);
            this.groupSamePoint.Controls.Add(this.labelCircleRadius);
            this.groupSamePoint.Controls.Add(this.showSamePointRadius);
            this.groupSamePoint.Controls.Add(this.samePointRadius);
            this.groupSamePoint.Controls.Add(this.labelCircleRadiusUnit);
            this.groupSamePoint.Location = new System.Drawing.Point(15, 232);
            this.groupSamePoint.Margin = new System.Windows.Forms.Padding(0);
            this.groupSamePoint.Name = "groupSamePoint";
            this.groupSamePoint.Padding = new System.Windows.Forms.Padding(0);
            this.groupSamePoint.Size = new System.Drawing.Size(641, 277);
            this.groupSamePoint.TabIndex = 0;
            this.groupSamePoint.TabStop = false;
            this.groupSamePoint.Text = "Same Point";
            // 
            // samePointGridView
            // 
            this.samePointGridView.AllowUserToAddRows = false;
            this.samePointGridView.AllowUserToDeleteRows = false;
            this.samePointGridView.AllowUserToResizeColumns = false;
            this.samePointGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.samePointGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.samePointGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.samePointGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnSamePointNum,
            this.columnSamePointRatio,
            this.columnSheetAttack,
            this.columnBlackCircle,
            this.columnBlackLine,
            this.columnPinHole,
            this.columnWhite,
            this.columnShape});
            this.samePointGridView.Location = new System.Drawing.Point(15, 75);
            this.samePointGridView.Name = "samePointGridView";
            this.samePointGridView.RowHeadersVisible = false;
            this.samePointGridView.RowTemplate.Height = 23;
            this.samePointGridView.Size = new System.Drawing.Size(616, 190);
            this.samePointGridView.TabIndex = 0;
            // 
            // columnSamePointNum
            // 
            this.columnSamePointNum.HeaderText = "Num";
            this.columnSamePointNum.Name = "columnSamePointNum";
            this.columnSamePointNum.Width = 60;
            // 
            // columnSamePointRatio
            // 
            this.columnSamePointRatio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnSamePointRatio.FillWeight = 119.7605F;
            this.columnSamePointRatio.HeaderText = "Ratio (%)";
            this.columnSamePointRatio.Name = "columnSamePointRatio";
            // 
            // columnSheetAttack
            // 
            this.columnSheetAttack.FillWeight = 80.23952F;
            this.columnSheetAttack.HeaderText = "SheetAttack";
            this.columnSheetAttack.Name = "columnSheetAttack";
            this.columnSheetAttack.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnSheetAttack.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnSheetAttack.Width = 70;
            // 
            // columnBlackCircle
            // 
            this.columnBlackCircle.HeaderText = "Black Circle";
            this.columnBlackCircle.Name = "columnBlackCircle";
            this.columnBlackCircle.Width = 70;
            // 
            // columnBlackLine
            // 
            this.columnBlackLine.HeaderText = "Black Line";
            this.columnBlackLine.Name = "columnBlackLine";
            this.columnBlackLine.Width = 70;
            // 
            // columnPinHole
            // 
            this.columnPinHole.HeaderText = "Pinhole";
            this.columnPinHole.Name = "columnPinHole";
            this.columnPinHole.Width = 70;
            // 
            // columnWhite
            // 
            this.columnWhite.HeaderText = "White";
            this.columnWhite.Name = "columnWhite";
            this.columnWhite.Width = 70;
            // 
            // columnShape
            // 
            this.columnShape.HeaderText = "Shape";
            this.columnShape.Name = "columnShape";
            this.columnShape.Width = 70;
            // 
            // labelCircleRadius
            // 
            this.labelCircleRadius.AutoSize = true;
            this.labelCircleRadius.Location = new System.Drawing.Point(13, 37);
            this.labelCircleRadius.Margin = new System.Windows.Forms.Padding(0);
            this.labelCircleRadius.Name = "labelCircleRadius";
            this.labelCircleRadius.Size = new System.Drawing.Size(69, 25);
            this.labelCircleRadius.TabIndex = 0;
            this.labelCircleRadius.Text = "Radius";
            // 
            // showSamePointRadius
            // 
            this.showSamePointRadius.AutoSize = true;
            this.showSamePointRadius.Location = new System.Drawing.Point(431, 36);
            this.showSamePointRadius.Margin = new System.Windows.Forms.Padding(0);
            this.showSamePointRadius.Name = "showSamePointRadius";
            this.showSamePointRadius.Size = new System.Drawing.Size(77, 29);
            this.showSamePointRadius.TabIndex = 0;
            this.showSamePointRadius.Text = "Show";
            this.showSamePointRadius.UseVisualStyleBackColor = true;
            // 
            // samePointRadius
            // 
            this.samePointRadius.Location = new System.Drawing.Point(522, 35);
            this.samePointRadius.Margin = new System.Windows.Forms.Padding(0);
            this.samePointRadius.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.samePointRadius.Name = "samePointRadius";
            this.samePointRadius.Size = new System.Drawing.Size(54, 32);
            this.samePointRadius.TabIndex = 0;
            this.samePointRadius.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // labelCircleRadiusUnit
            // 
            this.labelCircleRadiusUnit.AutoSize = true;
            this.labelCircleRadiusUnit.Location = new System.Drawing.Point(579, 37);
            this.labelCircleRadiusUnit.Name = "labelCircleRadiusUnit";
            this.labelCircleRadiusUnit.Size = new System.Drawing.Size(52, 25);
            this.labelCircleRadiusUnit.TabIndex = 0;
            this.labelCircleRadiusUnit.Text = "Pixel";
            // 
            // useAlarmForm
            // 
            this.useAlarmForm.AutoSize = true;
            this.useAlarmForm.Location = new System.Drawing.Point(30, 68);
            this.useAlarmForm.Margin = new System.Windows.Forms.Padding(0);
            this.useAlarmForm.Name = "useAlarmForm";
            this.useAlarmForm.Size = new System.Drawing.Size(202, 29);
            this.useAlarmForm.TabIndex = 0;
            this.useAlarmForm.Text = "Use Alram Message";
            this.useAlarmForm.UseVisualStyleBackColor = true;
            // 
            // useIOOutput
            // 
            this.useIOOutput.AutoSize = true;
            this.useIOOutput.Location = new System.Drawing.Point(30, 34);
            this.useIOOutput.Margin = new System.Windows.Forms.Padding(0);
            this.useIOOutput.Name = "useIOOutput";
            this.useIOOutput.Size = new System.Drawing.Size(158, 29);
            this.useIOOutput.TabIndex = 0;
            this.useIOOutput.Text = "Use IO Output";
            this.useIOOutput.UseVisualStyleBackColor = true;
            // 
            // monitoringSettingList
            // 
            this.monitoringSettingList.Dock = System.Windows.Forms.DockStyle.Left;
            this.monitoringSettingList.FormattingEnabled = true;
            this.monitoringSettingList.ItemHeight = 25;
            this.monitoringSettingList.Items.AddRange(new object[] {
            "Classification",
            "Data Remove",
            "IO"});
            this.monitoringSettingList.Location = new System.Drawing.Point(10, 10);
            this.monitoringSettingList.Margin = new System.Windows.Forms.Padding(0);
            this.monitoringSettingList.Name = "monitoringSettingList";
            this.monitoringSettingList.Size = new System.Drawing.Size(152, 1617);
            this.monitoringSettingList.Sorted = true;
            this.monitoringSettingList.TabIndex = 3;
            this.monitoringSettingList.SelectedIndexChanged += new System.EventHandler(this.monitoringSettingList_SelectedIndexChanged);
            // 
            // buttonPanel
            // 
            this.buttonPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.buttonPanel.Controls.Add(this.buttonTransfer);
            this.buttonPanel.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.buttonPanel.Location = new System.Drawing.Point(989, 10);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(88, 1617);
            this.buttonPanel.TabIndex = 1;
            // 
            // buttonTransfer
            // 
            this.buttonTransfer.Image = global::UniScanG.Properties.Resources.Reset;
            this.buttonTransfer.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonTransfer.Location = new System.Drawing.Point(0, 0);
            this.buttonTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransfer.Name = "buttonTransfer";
            this.buttonTransfer.Size = new System.Drawing.Size(87, 88);
            this.buttonTransfer.TabIndex = 0;
            this.buttonTransfer.Text = "Sync";
            this.buttonTransfer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonTransfer.UseVisualStyleBackColor = true;
            // 
            // tabPageDeveloper
            // 
            this.tabPageDeveloper.Location = new System.Drawing.Point(-10000, -10000);
            this.tabPageDeveloper.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageDeveloper.Name = "tabPageDeveloper";
            this.tabPageDeveloper.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageDeveloper.Size = new System.Drawing.Size(1087, 1637);
            // 
            // imageCheckInterval
            // 
            this.imageCheckInterval.Location = new System.Drawing.Point(211, 119);
            this.imageCheckInterval.Name = "imageCheckInterval";
            this.imageCheckInterval.Size = new System.Drawing.Size(68, 21);
            this.imageCheckInterval.TabIndex = 0;
            // 
            // labelImageCheckInterval
            // 
            this.labelImageCheckInterval.AutoSize = true;
            this.labelImageCheckInterval.Location = new System.Drawing.Point(15, 123);
            this.labelImageCheckInterval.Name = "labelImageCheckInterval";
            this.labelImageCheckInterval.Size = new System.Drawing.Size(151, 17);
            this.labelImageCheckInterval.TabIndex = 0;
            this.labelImageCheckInterval.Text = "Image Check Interval";
            // 
            // labelSec
            // 
            this.labelSec.AutoSize = true;
            this.labelSec.Location = new System.Drawing.Point(286, 126);
            this.labelSec.Name = "labelSec";
            this.labelSec.Size = new System.Drawing.Size(29, 17);
            this.labelSec.TabIndex = 0;
            this.labelSec.Text = "sec";
            // 
            // labelMs
            // 
            this.labelMs.Location = new System.Drawing.Point(0, 0);
            this.labelMs.Name = "labelMs";
            this.labelMs.Size = new System.Drawing.Size(100, 23);
            this.labelMs.TabIndex = 0;
            // 
            // settingTabControl
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            this.settingTabControl.ClientAreaAppearance = appearance1;
            this.settingTabControl.Controls.Add(this.ultraTabSharedControlsPage1);
            this.settingTabControl.Controls.Add(this.tabPageMonitoring);
            this.settingTabControl.Controls.Add(this.tabPageDeveloper);
            this.settingTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingTabControl.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.settingTabControl.InterRowSpacing = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.InterTabSpacing = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.Location = new System.Drawing.Point(0, 0);
            this.settingTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.settingTabControl.Name = "settingTabControl";
            this.settingTabControl.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.settingTabControl.Size = new System.Drawing.Size(1217, 1637);
            this.settingTabControl.SpaceAfterTabs = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.SpaceBeforeTabs = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.StateButtons;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(226)))), ((int)(((byte)(241)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "맑은 고딕";
            this.settingTabControl.TabHeaderAreaAppearance = appearance2;
            this.settingTabControl.TabIndex = 0;
            this.settingTabControl.TabOrientation = Infragistics.Win.UltraWinTabs.TabOrientation.LeftTop;
            this.settingTabControl.TabPadding = new System.Drawing.Size(10, 10);
            appearance3.BackColor = System.Drawing.Color.Transparent;
            ultraTab3.Appearance = appearance3;
            ultraTab3.Key = "Monitoring";
            ultraTab3.TabPage = this.tabPageMonitoring;
            ultraTab3.Text = "Monitoring";
            ultraTab4.Key = "Developer";
            ultraTab4.TabPage = this.tabPageDeveloper;
            ultraTab4.Text = "Developer";
            ultraTab4.Visible = false;
            this.settingTabControl.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab3,
            ultraTab4});
            this.settingTabControl.TextOrientation = Infragistics.Win.UltraWinTabs.TextOrientation.Horizontal;
            this.settingTabControl.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabControlParam_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Margin = new System.Windows.Forms.Padding(0);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1087, 1637);
            // 
            // SettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.settingTabControl);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SettingPage";
            this.Size = new System.Drawing.Size(1217, 1637);
            this.tabPageMonitoring.ResumeLayout(false);
            this.layoutMonitoring.ResumeLayout(false);
            this.groupClassification.ResumeLayout(false);
            this.groupBox10.ResumeLayout(false);
            this.groupBox10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.whiteDefectClassification)).EndInit();
            this.groupSize.ResumeLayout(false);
            this.groupSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackCircleSizeFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pinHoleSizeFilter)).EndInit();
            this.groupElogation.ResumeLayout(false);
            this.groupElogation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackDefectElogation)).EndInit();
            this.groupCompectness.ResumeLayout(false);
            this.groupCompectness.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blackDefectClassification)).EndInit();
            this.groupDataRemove.ResumeLayout(false);
            this.groupDataRemove.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.storingDays)).EndInit();
            this.groupIO.ResumeLayout(false);
            this.groupIO.PerformLayout();
            this.groupBoxCheckPoint.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkPointGridView)).EndInit();
            this.groupWholePattern.ResumeLayout(false);
            this.groupWholePattern.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wholeDefectRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wholeDefectNum)).EndInit();
            this.groupSamePoint.ResumeLayout(false);
            this.groupSamePoint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samePointGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.samePointRadius)).EndInit();
            this.buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingTabControl)).EndInit();
            this.settingTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NumericUpDown imageCheckInterval;
        private System.Windows.Forms.Label labelImageCheckInterval;
        private System.Windows.Forms.Label labelSec;
        private System.Windows.Forms.Label labelMs;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl settingTabControl;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageMonitoring;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageDeveloper;
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private System.Windows.Forms.Button buttonTransfer;
        private System.Windows.Forms.FlowLayoutPanel layoutMonitoring;
        private System.Windows.Forms.GroupBox groupClassification;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.Label labelWhiteDefectClassificationWhite;
        private System.Windows.Forms.TrackBar whiteDefectClassification;
        private System.Windows.Forms.Label labelWhiteDefectClassificationPinhole;
        private System.Windows.Forms.TextBox whiteDefectClassificationText;
        private System.Windows.Forms.GroupBox groupSize;
        private System.Windows.Forms.Label labelBlackCircleSizeFilter;
        private System.Windows.Forms.NumericUpDown blackCircleSizeFilter;
        private System.Windows.Forms.Label labelPinHoleSizeFilterUnit;
        private System.Windows.Forms.Label labelBlackCircleSizeFilterUnit;
        private System.Windows.Forms.NumericUpDown pinHoleSizeFilter;
        private System.Windows.Forms.Label labelPinHoleSizeFilter;
        private System.Windows.Forms.GroupBox groupElogation;
        private System.Windows.Forms.TextBox blackDefectElogationText;
        private System.Windows.Forms.Label labelElogationLine;
        private System.Windows.Forms.Label labelElogationCircle;
        private System.Windows.Forms.TrackBar blackDefectElogation;
        private System.Windows.Forms.GroupBox groupCompectness;
        private System.Windows.Forms.TrackBar blackDefectClassification;
        private System.Windows.Forms.Label labelBlackDefectClassificationLine;
        private System.Windows.Forms.Label labelBlackDefectClassificationCircle;
        private System.Windows.Forms.TextBox blackDefectClassificationText;
        private System.Windows.Forms.GroupBox groupDataRemove;
        private System.Windows.Forms.Label labelStoringDaysUnit;
        private System.Windows.Forms.Label labelStoringDays;
        private System.Windows.Forms.NumericUpDown storingDays;
        private System.Windows.Forms.GroupBox groupIO;
        private System.Windows.Forms.CheckBox useWholeDefectIO;
        private System.Windows.Forms.GroupBox groupBoxCheckPoint;
        private System.Windows.Forms.DataGridView checkPointGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.Button buttonPortView;
        private System.Windows.Forms.GroupBox groupWholePattern;
        private System.Windows.Forms.Label labelWholeResultNum;
        private System.Windows.Forms.NumericUpDown wholeDefectRatio;
        private System.Windows.Forms.Label labelDefectRatio;
        private System.Windows.Forms.Label labelWholeDefectRatioUnit;
        private System.Windows.Forms.NumericUpDown wholeDefectNum;
        private System.Windows.Forms.GroupBox groupSamePoint;
        private System.Windows.Forms.DataGridView samePointGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSamePointNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSamePointRatio;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnSheetAttack;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnBlackCircle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnBlackLine;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnPinHole;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnWhite;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnShape;
        private System.Windows.Forms.Label labelCircleRadius;
        private System.Windows.Forms.CheckBox showSamePointRadius;
        private System.Windows.Forms.NumericUpDown samePointRadius;
        private System.Windows.Forms.Label labelCircleRadiusUnit;
        private System.Windows.Forms.CheckBox useAlarmForm;
        private System.Windows.Forms.CheckBox useIOOutput;
        private System.Windows.Forms.ListBox monitoringSettingList;
    }
}
