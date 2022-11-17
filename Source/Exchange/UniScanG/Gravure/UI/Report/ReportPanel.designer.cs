namespace UniScanG.Gravure.UI.Report
{
    partial class ReportPanel
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.layoutTop = new System.Windows.Forms.TableLayoutPanel();
            this.layoutAdvance = new System.Windows.Forms.TableLayoutPanel();
            this.labelExport = new System.Windows.Forms.Label();
            this.labelWindowCapture = new System.Windows.Forms.Label();
            this.buttonExportSheet = new Infragistics.Win.Misc.UltraButton();
            this.buttonWindowCapture = new Infragistics.Win.Misc.UltraButton();
            this.labelExportDefect = new System.Windows.Forms.Label();
            this.labelExportLength = new System.Windows.Forms.Label();
            this.buttonExportLength = new Infragistics.Win.Misc.UltraButton();
            this.layoutFilter = new System.Windows.Forms.TableLayoutPanel();
            this.layoutSelectFilter = new System.Windows.Forms.TableLayoutPanel();
            this.panelSelectCam = new System.Windows.Forms.Panel();
            this.checkBoxCam = new System.Windows.Forms.CheckBox();
            this.labelCam = new System.Windows.Forms.Label();
            this.layoutType = new System.Windows.Forms.TableLayoutPanel();
            this.noprint = new System.Windows.Forms.RadioButton();
            this.pinhole = new System.Windows.Forms.RadioButton();
            this.sheetAttack = new System.Windows.Forms.RadioButton();
            this.total = new System.Windows.Forms.RadioButton();
            this.dielectric = new System.Windows.Forms.RadioButton();
            this.layoutSize = new System.Windows.Forms.TableLayoutPanel();
            this.labelMaxUnit = new System.Windows.Forms.Label();
            this.sizeMax = new System.Windows.Forms.NumericUpDown();
            this.labelMinUnit = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.sizeMin = new System.Windows.Forms.NumericUpDown();
            this.labelMin = new System.Windows.Forms.Label();
            this.useSize = new System.Windows.Forms.CheckBox();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.labelFilterTitle = new System.Windows.Forms.Label();
            this.layoutbottom = new System.Windows.Forms.TableLayoutPanel();
            this.layoutLeft = new System.Windows.Forms.TableLayoutPanel();
            this.pinHoleNum = new System.Windows.Forms.Label();
            this.dielectricNum = new System.Windows.Forms.Label();
            this.noPrintNum = new System.Windows.Forms.Label();
            this.sheetList = new System.Windows.Forms.DataGridView();
            this.columnPattern = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelSheetList = new System.Windows.Forms.Label();
            this.labelSheetAttack = new System.Windows.Forms.Label();
            this.sheetAttackNum = new System.Windows.Forms.Label();
            this.layoutResultFilter = new System.Windows.Forms.TableLayoutPanel();
            this.ngFilter = new System.Windows.Forms.CheckBox();
            this.okFilter = new System.Windows.Forms.CheckBox();
            this.labelDielectric = new System.Windows.Forms.Label();
            this.labelPinHole = new System.Windows.Forms.Label();
            this.productionTargetSpd = new System.Windows.Forms.Label();
            this.labelNoPrint = new System.Windows.Forms.Label();
            this.sheetTotal = new System.Windows.Forms.Label();
            this.productionEndTime = new System.Windows.Forms.TextBox();
            this.productionStartTime = new System.Windows.Forms.TextBox();
            this.sheetRatio = new System.Windows.Forms.Label();
            this.sheetNG = new System.Windows.Forms.Label();
            this.labelProductionStartTime = new System.Windows.Forms.Label();
            this.productionLotName = new System.Windows.Forms.TextBox();
            this.labelSheetNG = new System.Windows.Forms.Label();
            this.labelSheetTotal = new System.Windows.Forms.Label();
            this.labelProductionLotName = new System.Windows.Forms.Label();
            this.labelProductionEndTime = new System.Windows.Forms.Label();
            this.productionModelName = new System.Windows.Forms.TextBox();
            this.labelProductionTargetSpd = new System.Windows.Forms.Label();
            this.labelProductionModelName = new System.Windows.Forms.Label();
            this.defectInfoPanel = new System.Windows.Forms.Panel();
            this.labelDefectInfo = new System.Windows.Forms.Label();
            this.sheetRadioButton = new System.Windows.Forms.RadioButton();
            this.patternRadioButton = new System.Windows.Forms.RadioButton();
            this.labelSheetRatio = new System.Windows.Forms.Label();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelInfo = new System.Windows.Forms.Label();
            this.buttonInfoDetail = new System.Windows.Forms.Button();
            this.labelUnit = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.infoHeight1 = new System.Windows.Forms.Label();
            this.infoHeight2 = new System.Windows.Forms.Label();
            this.infoHeight3 = new System.Windows.Forms.Label();
            this.labelSheetLength = new System.Windows.Forms.Label();
            this.panelResult = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ultraFlowLayoutManager1 = new Infragistics.Win.Misc.UltraFlowLayoutManager(this.components);
            this.layoutMain.SuspendLayout();
            this.layoutTop.SuspendLayout();
            this.layoutAdvance.SuspendLayout();
            this.layoutFilter.SuspendLayout();
            this.layoutSelectFilter.SuspendLayout();
            this.panelSelectCam.SuspendLayout();
            this.layoutType.SuspendLayout();
            this.layoutSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).BeginInit();
            this.layoutbottom.SuspendLayout();
            this.layoutLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetList)).BeginInit();
            this.layoutResultFilter.SuspendLayout();
            this.defectInfoPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.layoutTop, 0, 0);
            this.layoutMain.Controls.Add(this.layoutbottom, 0, 1);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 2;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(1206, 734);
            this.layoutMain.TabIndex = 0;
            // 
            // layoutTop
            // 
            this.layoutTop.ColumnCount = 2;
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutTop.Controls.Add(this.layoutAdvance, 1, 0);
            this.layoutTop.Controls.Add(this.layoutFilter, 0, 0);
            this.layoutTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTop.Location = new System.Drawing.Point(0, 0);
            this.layoutTop.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTop.Name = "layoutTop";
            this.layoutTop.RowCount = 1;
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTop.Size = new System.Drawing.Size(1206, 150);
            this.layoutTop.TabIndex = 143;
            // 
            // layoutAdvance
            // 
            this.layoutAdvance.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutAdvance.ColumnCount = 3;
            this.layoutAdvance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.layoutAdvance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.layoutAdvance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32F));
            this.layoutAdvance.Controls.Add(this.labelExport, 1, 0);
            this.layoutAdvance.Controls.Add(this.labelWindowCapture, 0, 0);
            this.layoutAdvance.Controls.Add(this.buttonExportSheet, 1, 2);
            this.layoutAdvance.Controls.Add(this.buttonWindowCapture, 0, 2);
            this.layoutAdvance.Controls.Add(this.labelExportDefect, 1, 1);
            this.layoutAdvance.Controls.Add(this.labelExportLength, 2, 1);
            this.layoutAdvance.Controls.Add(this.buttonExportLength, 2, 2);
            this.layoutAdvance.Dock = System.Windows.Forms.DockStyle.Right;
            this.layoutAdvance.Location = new System.Drawing.Point(969, 0);
            this.layoutAdvance.Margin = new System.Windows.Forms.Padding(0);
            this.layoutAdvance.Name = "layoutAdvance";
            this.layoutAdvance.RowCount = 4;
            this.layoutAdvance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutAdvance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutAdvance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutAdvance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutAdvance.Size = new System.Drawing.Size(237, 150);
            this.layoutAdvance.TabIndex = 112;
            // 
            // labelExport
            // 
            this.labelExport.AutoSize = true;
            this.labelExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutAdvance.SetColumnSpan(this.labelExport, 2);
            this.labelExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExport.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelExport.Location = new System.Drawing.Point(86, 2);
            this.labelExport.Margin = new System.Windows.Forms.Padding(0);
            this.labelExport.Name = "labelExport";
            this.labelExport.Size = new System.Drawing.Size(149, 35);
            this.labelExport.TabIndex = 2;
            this.labelExport.Text = "Export";
            this.labelExport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelWindowCapture
            // 
            this.labelWindowCapture.AutoSize = true;
            this.labelWindowCapture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelWindowCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWindowCapture.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelWindowCapture.Location = new System.Drawing.Point(2, 2);
            this.labelWindowCapture.Margin = new System.Windows.Forms.Padding(0);
            this.labelWindowCapture.Name = "labelWindowCapture";
            this.layoutAdvance.SetRowSpan(this.labelWindowCapture, 2);
            this.labelWindowCapture.Size = new System.Drawing.Size(82, 72);
            this.labelWindowCapture.TabIndex = 147;
            this.labelWindowCapture.Text = "Window Capture";
            this.labelWindowCapture.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonExportSheet
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.Image = global::UniScanG.Properties.Resources.export;
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.buttonExportSheet.Appearance = appearance4;
            this.buttonExportSheet.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonExportSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonExportSheet.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonExportSheet.ImageSize = new System.Drawing.Size(40, 40);
            this.buttonExportSheet.Location = new System.Drawing.Point(86, 76);
            this.buttonExportSheet.Margin = new System.Windows.Forms.Padding(0);
            this.buttonExportSheet.Name = "buttonExportSheet";
            this.layoutAdvance.SetRowSpan(this.buttonExportSheet, 2);
            this.buttonExportSheet.Size = new System.Drawing.Size(73, 72);
            this.buttonExportSheet.TabIndex = 146;
            this.buttonExportSheet.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonExportSheet.Click += new System.EventHandler(this.buttonExportSheet_Click);
            // 
            // buttonWindowCapture
            // 
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.Image = global::UniScanG.Properties.Resources.Cam;
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.buttonWindowCapture.Appearance = appearance5;
            this.buttonWindowCapture.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonWindowCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonWindowCapture.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonWindowCapture.ImageSize = new System.Drawing.Size(40, 40);
            this.buttonWindowCapture.Location = new System.Drawing.Point(2, 76);
            this.buttonWindowCapture.Margin = new System.Windows.Forms.Padding(0);
            this.buttonWindowCapture.Name = "buttonWindowCapture";
            this.layoutAdvance.SetRowSpan(this.buttonWindowCapture, 2);
            this.buttonWindowCapture.Size = new System.Drawing.Size(82, 72);
            this.buttonWindowCapture.TabIndex = 148;
            this.buttonWindowCapture.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonWindowCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // labelExportDefect
            // 
            this.labelExportDefect.AutoSize = true;
            this.labelExportDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelExportDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExportDefect.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelExportDefect.Location = new System.Drawing.Point(86, 39);
            this.labelExportDefect.Margin = new System.Windows.Forms.Padding(0);
            this.labelExportDefect.Name = "labelExportDefect";
            this.labelExportDefect.Size = new System.Drawing.Size(73, 35);
            this.labelExportDefect.TabIndex = 2;
            this.labelExportDefect.Text = "Defect";
            this.labelExportDefect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelExportLength
            // 
            this.labelExportLength.AutoSize = true;
            this.labelExportLength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelExportLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExportLength.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelExportLength.Location = new System.Drawing.Point(161, 39);
            this.labelExportLength.Margin = new System.Windows.Forms.Padding(0);
            this.labelExportLength.Name = "labelExportLength";
            this.labelExportLength.Size = new System.Drawing.Size(74, 35);
            this.labelExportLength.TabIndex = 2;
            this.labelExportLength.Text = "Length";
            this.labelExportLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonExportLength
            // 
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.Image = global::UniScanG.Properties.Resources.chartBar512;
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.buttonExportLength.Appearance = appearance6;
            this.buttonExportLength.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonExportLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonExportLength.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonExportLength.ImageSize = new System.Drawing.Size(40, 40);
            this.buttonExportLength.Location = new System.Drawing.Point(161, 76);
            this.buttonExportLength.Margin = new System.Windows.Forms.Padding(0);
            this.buttonExportLength.Name = "buttonExportLength";
            this.layoutAdvance.SetRowSpan(this.buttonExportLength, 2);
            this.buttonExportLength.Size = new System.Drawing.Size(74, 72);
            this.buttonExportLength.TabIndex = 146;
            this.buttonExportLength.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonExportLength.Click += new System.EventHandler(this.buttonExportLength_Click);
            // 
            // layoutFilter
            // 
            this.layoutFilter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutFilter.ColumnCount = 1;
            this.layoutFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFilter.Controls.Add(this.layoutSelectFilter, 0, 1);
            this.layoutFilter.Controls.Add(this.labelFilterTitle, 0, 0);
            this.layoutFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutFilter.Location = new System.Drawing.Point(0, 0);
            this.layoutFilter.Margin = new System.Windows.Forms.Padding(0);
            this.layoutFilter.Name = "layoutFilter";
            this.layoutFilter.RowCount = 2;
            this.layoutFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFilter.Size = new System.Drawing.Size(969, 150);
            this.layoutFilter.TabIndex = 108;
            // 
            // layoutSelectFilter
            // 
            this.layoutSelectFilter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutSelectFilter.ColumnCount = 2;
            this.layoutSelectFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutSelectFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSelectFilter.Controls.Add(this.panelSelectCam, 1, 0);
            this.layoutSelectFilter.Controls.Add(this.labelCam, 0, 0);
            this.layoutSelectFilter.Controls.Add(this.layoutType, 1, 1);
            this.layoutSelectFilter.Controls.Add(this.layoutSize, 1, 2);
            this.layoutSelectFilter.Controls.Add(this.labelSize, 0, 2);
            this.layoutSelectFilter.Controls.Add(this.labelType, 0, 1);
            this.layoutSelectFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSelectFilter.Location = new System.Drawing.Point(2, 39);
            this.layoutSelectFilter.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSelectFilter.Name = "layoutSelectFilter";
            this.layoutSelectFilter.RowCount = 3;
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutSelectFilter.Size = new System.Drawing.Size(965, 109);
            this.layoutSelectFilter.TabIndex = 2;
            // 
            // panelSelectCam
            // 
            this.panelSelectCam.Controls.Add(this.checkBoxCam);
            this.panelSelectCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelectCam.Location = new System.Drawing.Point(104, 2);
            this.panelSelectCam.Margin = new System.Windows.Forms.Padding(0);
            this.panelSelectCam.Name = "panelSelectCam";
            this.panelSelectCam.Size = new System.Drawing.Size(859, 33);
            this.panelSelectCam.TabIndex = 88;
            // 
            // checkBoxCam
            // 
            this.checkBoxCam.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCam.AutoSize = true;
            this.checkBoxCam.Checked = true;
            this.checkBoxCam.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCam.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxCam.FlatAppearance.BorderSize = 0;
            this.checkBoxCam.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCam.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.checkBoxCam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCam.Location = new System.Drawing.Point(0, 0);
            this.checkBoxCam.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxCam.Name = "checkBoxCam";
            this.checkBoxCam.Size = new System.Drawing.Size(76, 33);
            this.checkBoxCam.TabIndex = 0;
            this.checkBoxCam.Text = "Defualt";
            this.checkBoxCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCam.UseVisualStyleBackColor = true;
            this.checkBoxCam.Visible = false;
            // 
            // labelCam
            // 
            this.labelCam.AutoSize = true;
            this.labelCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCam.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelCam.Location = new System.Drawing.Point(2, 2);
            this.labelCam.Margin = new System.Windows.Forms.Padding(0);
            this.labelCam.Name = "labelCam";
            this.labelCam.Size = new System.Drawing.Size(100, 33);
            this.labelCam.TabIndex = 89;
            this.labelCam.Text = "Cam";
            this.labelCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutType
            // 
            this.layoutType.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutType.ColumnCount = 6;
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutType.Controls.Add(this.noprint, 1, 0);
            this.layoutType.Controls.Add(this.pinhole, 2, 0);
            this.layoutType.Controls.Add(this.sheetAttack, 3, 0);
            this.layoutType.Controls.Add(this.total, 0, 0);
            this.layoutType.Controls.Add(this.dielectric, 4, 0);
            this.layoutType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutType.Location = new System.Drawing.Point(104, 37);
            this.layoutType.Margin = new System.Windows.Forms.Padding(0);
            this.layoutType.Name = "layoutType";
            this.layoutType.RowCount = 1;
            this.layoutType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutType.Size = new System.Drawing.Size(859, 33);
            this.layoutType.TabIndex = 86;
            // 
            // noprint
            // 
            this.noprint.Appearance = System.Windows.Forms.Appearance.Button;
            this.noprint.AutoSize = true;
            this.noprint.BackColor = System.Drawing.Color.Transparent;
            this.noprint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noprint.FlatAppearance.BorderSize = 0;
            this.noprint.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.noprint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.noprint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noprint.ForeColor = System.Drawing.Color.Red;
            this.noprint.Location = new System.Drawing.Point(127, 1);
            this.noprint.Margin = new System.Windows.Forms.Padding(0);
            this.noprint.Name = "noprint";
            this.noprint.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.noprint.Size = new System.Drawing.Size(150, 31);
            this.noprint.TabIndex = 1;
            this.noprint.Text = "Noprint";
            this.noprint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.noprint.UseVisualStyleBackColor = false;
            this.noprint.CheckedChanged += new System.EventHandler(this.noPrint_CheckedChanged);
            // 
            // pinhole
            // 
            this.pinhole.Appearance = System.Windows.Forms.Appearance.Button;
            this.pinhole.AutoSize = true;
            this.pinhole.BackColor = System.Drawing.Color.Transparent;
            this.pinhole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pinhole.FlatAppearance.BorderSize = 0;
            this.pinhole.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.pinhole.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.pinhole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pinhole.ForeColor = System.Drawing.Color.DarkMagenta;
            this.pinhole.Location = new System.Drawing.Point(278, 1);
            this.pinhole.Margin = new System.Windows.Forms.Padding(0);
            this.pinhole.Name = "pinhole";
            this.pinhole.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.pinhole.Size = new System.Drawing.Size(125, 31);
            this.pinhole.TabIndex = 83;
            this.pinhole.Text = "Pinhole";
            this.pinhole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pinhole.UseVisualStyleBackColor = false;
            this.pinhole.CheckedChanged += new System.EventHandler(this.pinHole_CheckedChanged);
            // 
            // sheetAttack
            // 
            this.sheetAttack.Appearance = System.Windows.Forms.Appearance.Button;
            this.sheetAttack.AutoSize = true;
            this.sheetAttack.BackColor = System.Drawing.Color.Transparent;
            this.sheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetAttack.FlatAppearance.BorderSize = 0;
            this.sheetAttack.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.sheetAttack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.sheetAttack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sheetAttack.ForeColor = System.Drawing.Color.Maroon;
            this.sheetAttack.Location = new System.Drawing.Point(404, 1);
            this.sheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.sheetAttack.Name = "sheetAttack";
            this.sheetAttack.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.sheetAttack.Size = new System.Drawing.Size(125, 31);
            this.sheetAttack.TabIndex = 86;
            this.sheetAttack.Text = "SheetAttack";
            this.sheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sheetAttack.UseVisualStyleBackColor = false;
            this.sheetAttack.CheckedChanged += new System.EventHandler(this.sheetAttack_CheckedChanged);
            // 
            // total
            // 
            this.total.Appearance = System.Windows.Forms.Appearance.Button;
            this.total.BackColor = System.Drawing.Color.Transparent;
            this.total.Checked = true;
            this.total.Dock = System.Windows.Forms.DockStyle.Fill;
            this.total.FlatAppearance.BorderSize = 0;
            this.total.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.total.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.total.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.total.Location = new System.Drawing.Point(1, 1);
            this.total.Margin = new System.Windows.Forms.Padding(0);
            this.total.Name = "total";
            this.total.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.total.Size = new System.Drawing.Size(125, 31);
            this.total.TabIndex = 81;
            this.total.TabStop = true;
            this.total.Text = "Total";
            this.total.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.total.UseVisualStyleBackColor = false;
            this.total.CheckedChanged += new System.EventHandler(this.total_CheckedChanged);
            // 
            // dielectric
            // 
            this.dielectric.Appearance = System.Windows.Forms.Appearance.Button;
            this.dielectric.AutoSize = true;
            this.dielectric.BackColor = System.Drawing.Color.Transparent;
            this.dielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dielectric.FlatAppearance.BorderSize = 0;
            this.dielectric.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.dielectric.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.dielectric.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dielectric.ForeColor = System.Drawing.Color.Blue;
            this.dielectric.Location = new System.Drawing.Point(530, 1);
            this.dielectric.Margin = new System.Windows.Forms.Padding(0);
            this.dielectric.Name = "dielectric";
            this.dielectric.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.dielectric.Size = new System.Drawing.Size(125, 31);
            this.dielectric.TabIndex = 0;
            this.dielectric.Text = "Dielectric";
            this.dielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dielectric.UseVisualStyleBackColor = false;
            this.dielectric.CheckedChanged += new System.EventHandler(this.dielectric_CheckedChanged);
            // 
            // layoutSize
            // 
            this.layoutSize.ColumnCount = 8;
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSize.Controls.Add(this.labelMaxUnit, 6, 0);
            this.layoutSize.Controls.Add(this.sizeMax, 5, 0);
            this.layoutSize.Controls.Add(this.labelMinUnit, 3, 0);
            this.layoutSize.Controls.Add(this.labelMax, 4, 0);
            this.layoutSize.Controls.Add(this.sizeMin, 2, 0);
            this.layoutSize.Controls.Add(this.labelMin, 1, 0);
            this.layoutSize.Controls.Add(this.useSize, 0, 0);
            this.layoutSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSize.Location = new System.Drawing.Point(104, 72);
            this.layoutSize.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSize.Name = "layoutSize";
            this.layoutSize.RowCount = 1;
            this.layoutSize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSize.Size = new System.Drawing.Size(859, 35);
            this.layoutSize.TabIndex = 67;
            // 
            // labelMaxUnit
            // 
            this.labelMaxUnit.AutoSize = true;
            this.labelMaxUnit.BackColor = System.Drawing.Color.Transparent;
            this.labelMaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxUnit.Location = new System.Drawing.Point(420, 0);
            this.labelMaxUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMaxUnit.Name = "labelMaxUnit";
            this.labelMaxUnit.Size = new System.Drawing.Size(60, 35);
            this.labelMaxUnit.TabIndex = 0;
            this.labelMaxUnit.Text = "(um)";
            this.labelMaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sizeMax
            // 
            this.sizeMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sizeMax.Enabled = false;
            this.sizeMax.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.sizeMax.Location = new System.Drawing.Point(320, 5);
            this.sizeMax.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sizeMax.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.sizeMax.Name = "sizeMax";
            this.sizeMax.Size = new System.Drawing.Size(100, 29);
            this.sizeMax.TabIndex = 0;
            this.sizeMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sizeMax.ValueChanged += new System.EventHandler(this.sizeMax_ValueChanged);
            // 
            // labelMinUnit
            // 
            this.labelMinUnit.AutoSize = true;
            this.labelMinUnit.BackColor = System.Drawing.Color.Transparent;
            this.labelMinUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMinUnit.Location = new System.Drawing.Point(210, 0);
            this.labelMinUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMinUnit.Name = "labelMinUnit";
            this.labelMinUnit.Size = new System.Drawing.Size(50, 35);
            this.labelMinUnit.TabIndex = 85;
            this.labelMinUnit.Text = "~";
            this.labelMinUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.BackColor = System.Drawing.Color.Transparent;
            this.labelMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMax.Location = new System.Drawing.Point(260, 0);
            this.labelMax.Margin = new System.Windows.Forms.Padding(0);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(60, 35);
            this.labelMax.TabIndex = 84;
            this.labelMax.Text = "Max";
            this.labelMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sizeMin
            // 
            this.sizeMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sizeMin.Enabled = false;
            this.sizeMin.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.sizeMin.Location = new System.Drawing.Point(110, 5);
            this.sizeMin.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sizeMin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.sizeMin.Name = "sizeMin";
            this.sizeMin.Size = new System.Drawing.Size(100, 29);
            this.sizeMin.TabIndex = 69;
            this.sizeMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sizeMin.ValueChanged += new System.EventHandler(this.sizeMin_ValueChanged);
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.BackColor = System.Drawing.Color.Transparent;
            this.labelMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMin.Location = new System.Drawing.Point(50, 0);
            this.labelMin.Margin = new System.Windows.Forms.Padding(0);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(60, 35);
            this.labelMin.TabIndex = 73;
            this.labelMin.Text = "Min";
            this.labelMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // useSize
            // 
            this.useSize.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.useSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.useSize.Location = new System.Drawing.Point(0, 0);
            this.useSize.Margin = new System.Windows.Forms.Padding(0);
            this.useSize.Name = "useSize";
            this.useSize.Size = new System.Drawing.Size(50, 35);
            this.useSize.TabIndex = 99;
            this.useSize.UseVisualStyleBackColor = true;
            this.useSize.CheckedChanged += new System.EventHandler(this.useSize_CheckedChanged);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSize.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelSize.Location = new System.Drawing.Point(2, 72);
            this.labelSize.Margin = new System.Windows.Forms.Padding(0);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(100, 35);
            this.labelSize.TabIndex = 0;
            this.labelSize.Text = "Size";
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelType.Location = new System.Drawing.Point(2, 37);
            this.labelType.Margin = new System.Windows.Forms.Padding(0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(100, 33);
            this.labelType.TabIndex = 0;
            this.labelType.Text = "Type";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFilterTitle
            // 
            this.labelFilterTitle.AutoSize = true;
            this.labelFilterTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelFilterTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFilterTitle.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelFilterTitle.Location = new System.Drawing.Point(2, 2);
            this.labelFilterTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelFilterTitle.Name = "labelFilterTitle";
            this.labelFilterTitle.Size = new System.Drawing.Size(965, 35);
            this.labelFilterTitle.TabIndex = 1;
            this.labelFilterTitle.Text = "Filter";
            this.labelFilterTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutbottom
            // 
            this.layoutbottom.ColumnCount = 2;
            this.layoutbottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.layoutbottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutbottom.Controls.Add(this.layoutLeft, 0, 0);
            this.layoutbottom.Controls.Add(this.panelResult, 1, 0);
            this.layoutbottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutbottom.Location = new System.Drawing.Point(0, 150);
            this.layoutbottom.Margin = new System.Windows.Forms.Padding(0);
            this.layoutbottom.Name = "layoutbottom";
            this.layoutbottom.RowCount = 1;
            this.layoutbottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutbottom.Size = new System.Drawing.Size(1206, 584);
            this.layoutbottom.TabIndex = 144;
            // 
            // layoutLeft
            // 
            this.layoutLeft.AutoSize = true;
            this.layoutLeft.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutLeft.ColumnCount = 2;
            this.layoutLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.layoutLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLeft.Controls.Add(this.pinHoleNum, 1, 11);
            this.layoutLeft.Controls.Add(this.dielectricNum, 1, 10);
            this.layoutLeft.Controls.Add(this.noPrintNum, 1, 9);
            this.layoutLeft.Controls.Add(this.sheetList, 0, 17);
            this.layoutLeft.Controls.Add(this.labelSheetList, 0, 12);
            this.layoutLeft.Controls.Add(this.labelSheetAttack, 0, 8);
            this.layoutLeft.Controls.Add(this.sheetAttackNum, 1, 8);
            this.layoutLeft.Controls.Add(this.layoutResultFilter, 0, 19);
            this.layoutLeft.Controls.Add(this.labelDielectric, 0, 10);
            this.layoutLeft.Controls.Add(this.labelPinHole, 0, 11);
            this.layoutLeft.Controls.Add(this.productionTargetSpd, 1, 5);
            this.layoutLeft.Controls.Add(this.labelNoPrint, 0, 9);
            this.layoutLeft.Controls.Add(this.sheetTotal, 1, 14);
            this.layoutLeft.Controls.Add(this.productionEndTime, 1, 4);
            this.layoutLeft.Controls.Add(this.productionStartTime, 1, 3);
            this.layoutLeft.Controls.Add(this.sheetRatio, 1, 16);
            this.layoutLeft.Controls.Add(this.sheetNG, 1, 15);
            this.layoutLeft.Controls.Add(this.labelProductionStartTime, 0, 3);
            this.layoutLeft.Controls.Add(this.productionLotName, 1, 2);
            this.layoutLeft.Controls.Add(this.labelSheetNG, 0, 15);
            this.layoutLeft.Controls.Add(this.labelSheetTotal, 0, 14);
            this.layoutLeft.Controls.Add(this.labelProductionLotName, 0, 2);
            this.layoutLeft.Controls.Add(this.labelProductionEndTime, 0, 4);
            this.layoutLeft.Controls.Add(this.productionModelName, 1, 1);
            this.layoutLeft.Controls.Add(this.labelProductionTargetSpd, 0, 5);
            this.layoutLeft.Controls.Add(this.labelProductionModelName, 0, 1);
            this.layoutLeft.Controls.Add(this.defectInfoPanel, 0, 7);
            this.layoutLeft.Controls.Add(this.labelSheetRatio, 0, 16);
            this.layoutLeft.Controls.Add(this.buttonSelectAll, 0, 18);
            this.layoutLeft.Controls.Add(this.panel1, 0, 0);
            this.layoutLeft.Controls.Add(this.labelUnit, 0, 13);
            this.layoutLeft.Controls.Add(this.tableLayoutPanel1, 1, 6);
            this.layoutLeft.Controls.Add(this.labelSheetLength, 0, 6);
            this.layoutLeft.Cursor = System.Windows.Forms.Cursors.Default;
            this.layoutLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutLeft.Location = new System.Drawing.Point(0, 0);
            this.layoutLeft.Margin = new System.Windows.Forms.Padding(0);
            this.layoutLeft.Name = "layoutLeft";
            this.layoutLeft.RowCount = 20;
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLeft.Size = new System.Drawing.Size(300, 584);
            this.layoutLeft.TabIndex = 92;
            // 
            // pinHoleNum
            // 
            this.pinHoleNum.AutoSize = true;
            this.pinHoleNum.BackColor = System.Drawing.Color.White;
            this.pinHoleNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pinHoleNum.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.pinHoleNum.Location = new System.Drawing.Point(104, 262);
            this.pinHoleNum.Margin = new System.Windows.Forms.Padding(0);
            this.pinHoleNum.Name = "pinHoleNum";
            this.pinHoleNum.Size = new System.Drawing.Size(195, 20);
            this.pinHoleNum.TabIndex = 184;
            this.pinHoleNum.Text = "0";
            this.pinHoleNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dielectricNum
            // 
            this.dielectricNum.AutoSize = true;
            this.dielectricNum.BackColor = System.Drawing.Color.White;
            this.dielectricNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dielectricNum.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.dielectricNum.Location = new System.Drawing.Point(104, 241);
            this.dielectricNum.Margin = new System.Windows.Forms.Padding(0);
            this.dielectricNum.Name = "dielectricNum";
            this.dielectricNum.Size = new System.Drawing.Size(195, 20);
            this.dielectricNum.TabIndex = 183;
            this.dielectricNum.Text = "0";
            this.dielectricNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // noPrintNum
            // 
            this.noPrintNum.AutoSize = true;
            this.noPrintNum.BackColor = System.Drawing.Color.White;
            this.noPrintNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noPrintNum.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.noPrintNum.Location = new System.Drawing.Point(104, 220);
            this.noPrintNum.Margin = new System.Windows.Forms.Padding(0);
            this.noPrintNum.Name = "noPrintNum";
            this.noPrintNum.Size = new System.Drawing.Size(195, 20);
            this.noPrintNum.TabIndex = 182;
            this.noPrintNum.Text = "0";
            this.noPrintNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetList
            // 
            this.sheetList.AllowUserToAddRows = false;
            this.sheetList.AllowUserToDeleteRows = false;
            this.sheetList.AllowUserToResizeColumns = false;
            this.sheetList.AllowUserToResizeRows = false;
            this.sheetList.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sheetList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.sheetList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sheetList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnPattern,
            this.columnQty});
            this.layoutLeft.SetColumnSpan(this.sheetList, 2);
            this.sheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.sheetList.Location = new System.Drawing.Point(1, 403);
            this.sheetList.Margin = new System.Windows.Forms.Padding(0);
            this.sheetList.Name = "sheetList";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.sheetList.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.sheetList.RowHeadersVisible = false;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.sheetList.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.sheetList.RowTemplate.Height = 23;
            this.sheetList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sheetList.Size = new System.Drawing.Size(298, 113);
            this.sheetList.TabIndex = 1;
            this.sheetList.SelectionChanged += new System.EventHandler(this.sheetList_SelectionChanged);
            this.sheetList.Click += new System.EventHandler(this.sheetList_Click);
            // 
            // columnPattern
            // 
            this.columnPattern.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnPattern.FillWeight = 65.9137F;
            this.columnPattern.HeaderText = "Pattern";
            this.columnPattern.Name = "columnPattern";
            this.columnPattern.Width = 91;
            // 
            // columnQty
            // 
            this.columnQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnQty.FillWeight = 139.0863F;
            this.columnQty.HeaderText = "Qty.";
            this.columnQty.Name = "columnQty";
            // 
            // labelSheetList
            // 
            this.labelSheetList.AutoSize = true;
            this.labelSheetList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutLeft.SetColumnSpan(this.labelSheetList, 2);
            this.labelSheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetList.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelSheetList.Location = new System.Drawing.Point(1, 283);
            this.labelSheetList.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetList.Name = "labelSheetList";
            this.labelSheetList.Size = new System.Drawing.Size(298, 35);
            this.labelSheetList.TabIndex = 0;
            this.labelSheetList.Text = "Sheet List";
            this.labelSheetList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetAttack
            // 
            this.labelSheetAttack.AutoSize = true;
            this.labelSheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetAttack.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSheetAttack.ForeColor = System.Drawing.Color.Maroon;
            this.labelSheetAttack.Location = new System.Drawing.Point(1, 199);
            this.labelSheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetAttack.Name = "labelSheetAttack";
            this.labelSheetAttack.Size = new System.Drawing.Size(102, 20);
            this.labelSheetAttack.TabIndex = 182;
            this.labelSheetAttack.Text = "SheetAttack";
            this.labelSheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetAttackNum
            // 
            this.sheetAttackNum.AutoSize = true;
            this.sheetAttackNum.BackColor = System.Drawing.Color.White;
            this.sheetAttackNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetAttackNum.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sheetAttackNum.Location = new System.Drawing.Point(104, 199);
            this.sheetAttackNum.Margin = new System.Windows.Forms.Padding(0);
            this.sheetAttackNum.Name = "sheetAttackNum";
            this.sheetAttackNum.Size = new System.Drawing.Size(195, 20);
            this.sheetAttackNum.TabIndex = 182;
            this.sheetAttackNum.Text = "0";
            this.sheetAttackNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutResultFilter
            // 
            this.layoutResultFilter.ColumnCount = 2;
            this.layoutLeft.SetColumnSpan(this.layoutResultFilter, 2);
            this.layoutResultFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutResultFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutResultFilter.Controls.Add(this.ngFilter, 1, 0);
            this.layoutResultFilter.Controls.Add(this.okFilter, 0, 0);
            this.layoutResultFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutResultFilter.Location = new System.Drawing.Point(1, 548);
            this.layoutResultFilter.Margin = new System.Windows.Forms.Padding(0);
            this.layoutResultFilter.Name = "layoutResultFilter";
            this.layoutResultFilter.RowCount = 1;
            this.layoutResultFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutResultFilter.Size = new System.Drawing.Size(298, 35);
            this.layoutResultFilter.TabIndex = 63;
            // 
            // ngFilter
            // 
            this.ngFilter.AutoSize = true;
            this.ngFilter.Checked = true;
            this.ngFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ngFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ngFilter.Location = new System.Drawing.Point(152, 3);
            this.ngFilter.Name = "ngFilter";
            this.ngFilter.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ngFilter.Size = new System.Drawing.Size(143, 29);
            this.ngFilter.TabIndex = 1;
            this.ngFilter.Text = "NG";
            this.ngFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ngFilter.UseVisualStyleBackColor = true;
            this.ngFilter.CheckedChanged += new System.EventHandler(this.ngFilter_CheckedChanged);
            // 
            // okFilter
            // 
            this.okFilter.AutoSize = true;
            this.okFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okFilter.Location = new System.Drawing.Point(3, 3);
            this.okFilter.Name = "okFilter";
            this.okFilter.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.okFilter.Size = new System.Drawing.Size(143, 29);
            this.okFilter.TabIndex = 0;
            this.okFilter.Text = "OK";
            this.okFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.okFilter.UseVisualStyleBackColor = true;
            this.okFilter.CheckedChanged += new System.EventHandler(this.okFilter_CheckedChanged);
            // 
            // labelDielectric
            // 
            this.labelDielectric.AutoSize = true;
            this.labelDielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDielectric.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelDielectric.ForeColor = System.Drawing.Color.Blue;
            this.labelDielectric.Location = new System.Drawing.Point(1, 241);
            this.labelDielectric.Margin = new System.Windows.Forms.Padding(0);
            this.labelDielectric.Name = "labelDielectric";
            this.labelDielectric.Size = new System.Drawing.Size(102, 20);
            this.labelDielectric.TabIndex = 182;
            this.labelDielectric.Text = "Dielectric";
            this.labelDielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPinHole
            // 
            this.labelPinHole.AutoSize = true;
            this.labelPinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPinHole.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelPinHole.ForeColor = System.Drawing.Color.DarkMagenta;
            this.labelPinHole.Location = new System.Drawing.Point(1, 262);
            this.labelPinHole.Margin = new System.Windows.Forms.Padding(0);
            this.labelPinHole.Name = "labelPinHole";
            this.labelPinHole.Size = new System.Drawing.Size(102, 20);
            this.labelPinHole.TabIndex = 182;
            this.labelPinHole.Text = "PinHole";
            this.labelPinHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // productionTargetSpd
            // 
            this.productionTargetSpd.BackColor = System.Drawing.Color.White;
            this.productionTargetSpd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionTargetSpd.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.productionTargetSpd.Location = new System.Drawing.Point(104, 121);
            this.productionTargetSpd.Margin = new System.Windows.Forms.Padding(0);
            this.productionTargetSpd.Name = "productionTargetSpd";
            this.productionTargetSpd.Size = new System.Drawing.Size(195, 20);
            this.productionTargetSpd.TabIndex = 2;
            this.productionTargetSpd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelNoPrint
            // 
            this.labelNoPrint.AutoSize = true;
            this.labelNoPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNoPrint.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelNoPrint.ForeColor = System.Drawing.Color.Red;
            this.labelNoPrint.Location = new System.Drawing.Point(1, 220);
            this.labelNoPrint.Margin = new System.Windows.Forms.Padding(0);
            this.labelNoPrint.Name = "labelNoPrint";
            this.labelNoPrint.Size = new System.Drawing.Size(102, 20);
            this.labelNoPrint.TabIndex = 182;
            this.labelNoPrint.Text = "Noprint";
            this.labelNoPrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetTotal
            // 
            this.sheetTotal.AutoSize = true;
            this.sheetTotal.BackColor = System.Drawing.Color.White;
            this.sheetTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetTotal.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sheetTotal.Location = new System.Drawing.Point(104, 340);
            this.sheetTotal.Margin = new System.Windows.Forms.Padding(0);
            this.sheetTotal.Name = "sheetTotal";
            this.sheetTotal.Size = new System.Drawing.Size(195, 20);
            this.sheetTotal.TabIndex = 6;
            this.sheetTotal.Text = "0";
            this.sheetTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // productionEndTime
            // 
            this.productionEndTime.BackColor = System.Drawing.Color.White;
            this.productionEndTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productionEndTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionEndTime.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.productionEndTime.Location = new System.Drawing.Point(104, 100);
            this.productionEndTime.Margin = new System.Windows.Forms.Padding(0);
            this.productionEndTime.Multiline = true;
            this.productionEndTime.Name = "productionEndTime";
            this.productionEndTime.ReadOnly = true;
            this.productionEndTime.Size = new System.Drawing.Size(195, 20);
            this.productionEndTime.TabIndex = 2;
            this.productionEndTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // productionStartTime
            // 
            this.productionStartTime.BackColor = System.Drawing.Color.White;
            this.productionStartTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productionStartTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionStartTime.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.productionStartTime.Location = new System.Drawing.Point(104, 79);
            this.productionStartTime.Margin = new System.Windows.Forms.Padding(0);
            this.productionStartTime.Multiline = true;
            this.productionStartTime.Name = "productionStartTime";
            this.productionStartTime.ReadOnly = true;
            this.productionStartTime.Size = new System.Drawing.Size(195, 20);
            this.productionStartTime.TabIndex = 4;
            this.productionStartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // sheetRatio
            // 
            this.sheetRatio.AutoSize = true;
            this.sheetRatio.BackColor = System.Drawing.Color.White;
            this.sheetRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetRatio.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sheetRatio.Location = new System.Drawing.Point(104, 382);
            this.sheetRatio.Margin = new System.Windows.Forms.Padding(0);
            this.sheetRatio.Name = "sheetRatio";
            this.sheetRatio.Size = new System.Drawing.Size(195, 20);
            this.sheetRatio.TabIndex = 2;
            this.sheetRatio.Text = "0";
            this.sheetRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetNG
            // 
            this.sheetNG.AutoSize = true;
            this.sheetNG.BackColor = System.Drawing.Color.White;
            this.sheetNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetNG.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.sheetNG.Location = new System.Drawing.Point(104, 361);
            this.sheetNG.Margin = new System.Windows.Forms.Padding(0);
            this.sheetNG.Name = "sheetNG";
            this.sheetNG.Size = new System.Drawing.Size(195, 20);
            this.sheetNG.TabIndex = 4;
            this.sheetNG.Text = "0";
            this.sheetNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProductionStartTime
            // 
            this.labelProductionStartTime.AutoSize = true;
            this.labelProductionStartTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductionStartTime.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelProductionStartTime.Location = new System.Drawing.Point(1, 79);
            this.labelProductionStartTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionStartTime.Name = "labelProductionStartTime";
            this.labelProductionStartTime.Size = new System.Drawing.Size(102, 20);
            this.labelProductionStartTime.TabIndex = 3;
            this.labelProductionStartTime.Text = "Start";
            this.labelProductionStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // productionLotName
            // 
            this.productionLotName.BackColor = System.Drawing.Color.White;
            this.productionLotName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productionLotName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionLotName.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.productionLotName.Location = new System.Drawing.Point(104, 58);
            this.productionLotName.Margin = new System.Windows.Forms.Padding(0);
            this.productionLotName.Multiline = true;
            this.productionLotName.Name = "productionLotName";
            this.productionLotName.ReadOnly = true;
            this.productionLotName.Size = new System.Drawing.Size(195, 20);
            this.productionLotName.TabIndex = 6;
            this.productionLotName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelSheetNG
            // 
            this.labelSheetNG.AutoSize = true;
            this.labelSheetNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetNG.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSheetNG.Location = new System.Drawing.Point(1, 361);
            this.labelSheetNG.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetNG.Name = "labelSheetNG";
            this.labelSheetNG.Size = new System.Drawing.Size(102, 20);
            this.labelSheetNG.TabIndex = 3;
            this.labelSheetNG.Text = "NG";
            this.labelSheetNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetTotal
            // 
            this.labelSheetTotal.AutoSize = true;
            this.labelSheetTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetTotal.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSheetTotal.Location = new System.Drawing.Point(1, 340);
            this.labelSheetTotal.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetTotal.Name = "labelSheetTotal";
            this.labelSheetTotal.Size = new System.Drawing.Size(102, 20);
            this.labelSheetTotal.TabIndex = 1;
            this.labelSheetTotal.Text = "Total";
            this.labelSheetTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProductionLotName
            // 
            this.labelProductionLotName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductionLotName.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelProductionLotName.Location = new System.Drawing.Point(1, 58);
            this.labelProductionLotName.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionLotName.Name = "labelProductionLotName";
            this.labelProductionLotName.Size = new System.Drawing.Size(102, 20);
            this.labelProductionLotName.TabIndex = 1;
            this.labelProductionLotName.Text = "Lot";
            this.labelProductionLotName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProductionEndTime
            // 
            this.labelProductionEndTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductionEndTime.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelProductionEndTime.Location = new System.Drawing.Point(1, 100);
            this.labelProductionEndTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionEndTime.Name = "labelProductionEndTime";
            this.labelProductionEndTime.Size = new System.Drawing.Size(102, 20);
            this.labelProductionEndTime.TabIndex = 0;
            this.labelProductionEndTime.Text = "End";
            this.labelProductionEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // productionModelName
            // 
            this.productionModelName.BackColor = System.Drawing.Color.White;
            this.productionModelName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productionModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionModelName.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.productionModelName.Location = new System.Drawing.Point(104, 37);
            this.productionModelName.Margin = new System.Windows.Forms.Padding(0);
            this.productionModelName.Multiline = true;
            this.productionModelName.Name = "productionModelName";
            this.productionModelName.ReadOnly = true;
            this.productionModelName.Size = new System.Drawing.Size(195, 20);
            this.productionModelName.TabIndex = 6;
            this.productionModelName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelProductionTargetSpd
            // 
            this.labelProductionTargetSpd.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelProductionTargetSpd.Location = new System.Drawing.Point(1, 121);
            this.labelProductionTargetSpd.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionTargetSpd.Name = "labelProductionTargetSpd";
            this.labelProductionTargetSpd.Size = new System.Drawing.Size(102, 20);
            this.labelProductionTargetSpd.TabIndex = 0;
            this.labelProductionTargetSpd.Text = "P.G. Speed";
            this.labelProductionTargetSpd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProductionModelName
            // 
            this.labelProductionModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductionModelName.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelProductionModelName.Location = new System.Drawing.Point(1, 37);
            this.labelProductionModelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionModelName.Name = "labelProductionModelName";
            this.labelProductionModelName.Size = new System.Drawing.Size(102, 20);
            this.labelProductionModelName.TabIndex = 1;
            this.labelProductionModelName.Text = "Model";
            this.labelProductionModelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defectInfoPanel
            // 
            this.layoutLeft.SetColumnSpan(this.defectInfoPanel, 2);
            this.defectInfoPanel.Controls.Add(this.labelDefectInfo);
            this.defectInfoPanel.Controls.Add(this.sheetRadioButton);
            this.defectInfoPanel.Controls.Add(this.patternRadioButton);
            this.defectInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectInfoPanel.Location = new System.Drawing.Point(1, 163);
            this.defectInfoPanel.Margin = new System.Windows.Forms.Padding(0);
            this.defectInfoPanel.Name = "defectInfoPanel";
            this.defectInfoPanel.Size = new System.Drawing.Size(298, 35);
            this.defectInfoPanel.TabIndex = 180;
            // 
            // labelDefectInfo
            // 
            this.labelDefectInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelDefectInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefectInfo.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelDefectInfo.Location = new System.Drawing.Point(0, 0);
            this.labelDefectInfo.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefectInfo.Name = "labelDefectInfo";
            this.labelDefectInfo.Size = new System.Drawing.Size(197, 35);
            this.labelDefectInfo.TabIndex = 174;
            this.labelDefectInfo.Text = "Defect Info";
            this.labelDefectInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetRadioButton
            // 
            this.sheetRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.sheetRadioButton.AutoSize = true;
            this.sheetRadioButton.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sheetRadioButton.Checked = true;
            this.sheetRadioButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sheetRadioButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.sheetRadioButton.FlatAppearance.BorderSize = 0;
            this.sheetRadioButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.sheetRadioButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.sheetRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sheetRadioButton.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.sheetRadioButton.Location = new System.Drawing.Point(197, 0);
            this.sheetRadioButton.Margin = new System.Windows.Forms.Padding(0);
            this.sheetRadioButton.Name = "sheetRadioButton";
            this.sheetRadioButton.Size = new System.Drawing.Size(46, 35);
            this.sheetRadioButton.TabIndex = 176;
            this.sheetRadioButton.TabStop = true;
            this.sheetRadioButton.Text = "Sheet";
            this.sheetRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sheetRadioButton.UseVisualStyleBackColor = false;
            this.sheetRadioButton.CheckedChanged += new System.EventHandler(this.patternRadioButton_CheckedChanged);
            // 
            // patternRadioButton
            // 
            this.patternRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.patternRadioButton.AutoSize = true;
            this.patternRadioButton.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.patternRadioButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.patternRadioButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.patternRadioButton.FlatAppearance.BorderSize = 0;
            this.patternRadioButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.patternRadioButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.patternRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.patternRadioButton.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.patternRadioButton.Location = new System.Drawing.Point(243, 0);
            this.patternRadioButton.Margin = new System.Windows.Forms.Padding(0);
            this.patternRadioButton.Name = "patternRadioButton";
            this.patternRadioButton.Size = new System.Drawing.Size(55, 35);
            this.patternRadioButton.TabIndex = 175;
            this.patternRadioButton.Text = "Pattern";
            this.patternRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.patternRadioButton.UseVisualStyleBackColor = false;
            this.patternRadioButton.CheckedChanged += new System.EventHandler(this.totalRadioButton_CheckedChanged);
            // 
            // labelSheetRatio
            // 
            this.labelSheetRatio.AutoSize = true;
            this.labelSheetRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetRatio.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSheetRatio.Location = new System.Drawing.Point(1, 382);
            this.labelSheetRatio.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetRatio.Name = "labelSheetRatio";
            this.labelSheetRatio.Size = new System.Drawing.Size(102, 20);
            this.labelSheetRatio.TabIndex = 0;
            this.labelSheetRatio.Text = "Ratio";
            this.labelSheetRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSelectAll
            // 
            this.layoutLeft.SetColumnSpan(this.buttonSelectAll, 2);
            this.buttonSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSelectAll.Location = new System.Drawing.Point(1, 517);
            this.buttonSelectAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(298, 30);
            this.buttonSelectAll.TabIndex = 0;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // panel1
            // 
            this.layoutLeft.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.labelInfo);
            this.panel1.Controls.Add(this.buttonInfoDetail);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(1, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(298, 35);
            this.panel1.TabIndex = 185;
            // 
            // labelInfo
            // 
            this.labelInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelInfo.Location = new System.Drawing.Point(0, 0);
            this.labelInfo.Margin = new System.Windows.Forms.Padding(0);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(223, 35);
            this.labelInfo.TabIndex = 69;
            this.labelInfo.Text = "Info";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonInfoDetail
            // 
            this.buttonInfoDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonInfoDetail.Location = new System.Drawing.Point(223, 0);
            this.buttonInfoDetail.Name = "buttonInfoDetail";
            this.buttonInfoDetail.Size = new System.Drawing.Size(75, 35);
            this.buttonInfoDetail.TabIndex = 70;
            this.buttonInfoDetail.Text = "Detail";
            this.buttonInfoDetail.UseVisualStyleBackColor = true;
            this.buttonInfoDetail.Visible = false;
            this.buttonInfoDetail.Click += new System.EventHandler(this.buttonInfoDetail_Click);
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUnit.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelUnit.ForeColor = System.Drawing.Color.Red;
            this.labelUnit.Location = new System.Drawing.Point(4, 319);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(96, 20);
            this.labelUnit.TabIndex = 68;
            this.labelUnit.Text = "Unit : Print";
            this.labelUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.infoHeight1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.infoHeight2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.infoHeight3, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(104, 142);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(195, 20);
            this.tableLayoutPanel1.TabIndex = 186;
            // 
            // infoHeight1
            // 
            this.infoHeight1.BackColor = System.Drawing.Color.White;
            this.infoHeight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoHeight1.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.infoHeight1.Location = new System.Drawing.Point(0, 0);
            this.infoHeight1.Margin = new System.Windows.Forms.Padding(0);
            this.infoHeight1.Name = "infoHeight1";
            this.infoHeight1.Size = new System.Drawing.Size(65, 20);
            this.infoHeight1.TabIndex = 0;
            this.infoHeight1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoHeight2
            // 
            this.infoHeight2.BackColor = System.Drawing.Color.White;
            this.infoHeight2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoHeight2.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.infoHeight2.Location = new System.Drawing.Point(65, 0);
            this.infoHeight2.Margin = new System.Windows.Forms.Padding(0);
            this.infoHeight2.Name = "infoHeight2";
            this.infoHeight2.Size = new System.Drawing.Size(65, 20);
            this.infoHeight2.TabIndex = 0;
            this.infoHeight2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoHeight3
            // 
            this.infoHeight3.BackColor = System.Drawing.Color.White;
            this.infoHeight3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoHeight3.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.infoHeight3.Location = new System.Drawing.Point(130, 0);
            this.infoHeight3.Margin = new System.Windows.Forms.Padding(0);
            this.infoHeight3.Name = "infoHeight3";
            this.infoHeight3.Size = new System.Drawing.Size(65, 20);
            this.infoHeight3.TabIndex = 0;
            this.infoHeight3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetLength
            // 
            this.labelSheetLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLength.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSheetLength.Location = new System.Drawing.Point(1, 142);
            this.labelSheetLength.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLength.Name = "labelSheetLength";
            this.labelSheetLength.Size = new System.Drawing.Size(102, 20);
            this.labelSheetLength.TabIndex = 0;
            this.labelSheetLength.Text = "Sheet Length";
            this.labelSheetLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelResult
            // 
            this.panelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResult.Location = new System.Drawing.Point(300, 0);
            this.panelResult.Margin = new System.Windows.Forms.Padding(0);
            this.panelResult.Name = "panelResult";
            this.panelResult.Size = new System.Drawing.Size(906, 584);
            this.panelResult.TabIndex = 91;
            // 
            // ReportPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Name = "ReportPanel";
            this.Size = new System.Drawing.Size(1206, 734);
            this.Load += new System.EventHandler(this.ReportPanel_Load);
            this.layoutMain.ResumeLayout(false);
            this.layoutTop.ResumeLayout(false);
            this.layoutAdvance.ResumeLayout(false);
            this.layoutAdvance.PerformLayout();
            this.layoutFilter.ResumeLayout(false);
            this.layoutFilter.PerformLayout();
            this.layoutSelectFilter.ResumeLayout(false);
            this.layoutSelectFilter.PerformLayout();
            this.panelSelectCam.ResumeLayout(false);
            this.panelSelectCam.PerformLayout();
            this.layoutType.ResumeLayout(false);
            this.layoutType.PerformLayout();
            this.layoutSize.ResumeLayout(false);
            this.layoutSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).EndInit();
            this.layoutbottom.ResumeLayout(false);
            this.layoutbottom.PerformLayout();
            this.layoutLeft.ResumeLayout(false);
            this.layoutLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetList)).EndInit();
            this.layoutResultFilter.ResumeLayout(false);
            this.layoutResultFilter.PerformLayout();
            this.defectInfoPanel.ResumeLayout(false);
            this.defectInfoPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.TableLayoutPanel layoutbottom;
        private System.Windows.Forms.TableLayoutPanel layoutResultFilter;
        private System.Windows.Forms.CheckBox ngFilter;
        private System.Windows.Forms.CheckBox okFilter;
        private System.Windows.Forms.Label labelSheetList;
        private System.Windows.Forms.Label labelSheetNG;
        private System.Windows.Forms.Label sheetRatio;
        private System.Windows.Forms.Label labelSheetTotal;
        private System.Windows.Forms.Label labelSheetRatio;
        private System.Windows.Forms.Label sheetNG;
        private System.Windows.Forms.Label sheetTotal;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.TableLayoutPanel layoutTop;
        private System.Windows.Forms.TableLayoutPanel layoutAdvance;
        private Infragistics.Win.Misc.UltraButton buttonExportSheet;
        private System.Windows.Forms.Label labelExport;
        private System.Windows.Forms.TableLayoutPanel layoutFilter;
        private System.Windows.Forms.TableLayoutPanel layoutSelectFilter;
        private System.Windows.Forms.TableLayoutPanel layoutType;
        private System.Windows.Forms.RadioButton pinhole;
        private System.Windows.Forms.RadioButton sheetAttack;
        private System.Windows.Forms.RadioButton total;
        private System.Windows.Forms.TableLayoutPanel layoutSize;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label labelMaxUnit;
        private System.Windows.Forms.NumericUpDown sizeMax;
        private System.Windows.Forms.Label labelMinUnit;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.NumericUpDown sizeMin;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelFilterTitle;
        private System.Windows.Forms.DataGridView sheetList;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPattern;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnQty;
        private System.Windows.Forms.CheckBox useSize;
        private System.Windows.Forms.RadioButton dielectric;
        private System.Windows.Forms.Label labelProductionLotName;
        private System.Windows.Forms.Label labelProductionStartTime;
        private System.Windows.Forms.TextBox productionStartTime;
        private System.Windows.Forms.TextBox productionLotName;
        private System.Windows.Forms.TextBox productionEndTime;
        private System.Windows.Forms.Label labelProductionEndTime;
        private System.Windows.Forms.Label labelCam;
        private System.Windows.Forms.Panel panelSelectCam;
        private System.Windows.Forms.CheckBox checkBoxCam;
        private System.Windows.Forms.RadioButton noprint;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label productionTargetSpd;
        private System.Windows.Forms.Label labelProductionTargetSpd;
        private System.Windows.Forms.Panel panelResult;
        private System.Windows.Forms.Label labelProductionModelName;
        private System.Windows.Forms.TextBox productionModelName;
        private Infragistics.Win.Misc.UltraButton buttonWindowCapture;
        private System.Windows.Forms.Label labelWindowCapture;
        private System.Windows.Forms.TableLayoutPanel layoutLeft;
        private System.Windows.Forms.Label noPrintNum;
        private System.Windows.Forms.Label dielectricNum;
        private System.Windows.Forms.Label pinHoleNum;
        private System.Windows.Forms.Label sheetAttackNum;
        private System.Windows.Forms.Label labelPinHole;
        private System.Windows.Forms.Label labelDielectric;
        private System.Windows.Forms.Label labelNoPrint;
        private System.Windows.Forms.Panel defectInfoPanel;
        public System.Windows.Forms.Label labelDefectInfo;
        private System.Windows.Forms.RadioButton sheetRadioButton;
        private System.Windows.Forms.RadioButton patternRadioButton;
        private System.Windows.Forms.Label labelSheetAttack;
        private Infragistics.Win.Misc.UltraFlowLayoutManager ultraFlowLayoutManager1;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonInfoDetail;
        private System.Windows.Forms.Label labelSheetLength;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label infoHeight1;
        private System.Windows.Forms.Label infoHeight2;
        private System.Windows.Forms.Label infoHeight3;
        private System.Windows.Forms.Label labelExportDefect;
        private System.Windows.Forms.Label labelExportLength;
        private Infragistics.Win.Misc.UltraButton buttonExportLength;
    }
}
