namespace UniScanG.Gravure.UI.Inspect
{
    partial class DefectPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.defectList = new System.Windows.Forms.DataGridView();
            this.columnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.labelCurrentDefect = new System.Windows.Forms.Label();
            this.labelFilterTitle = new System.Windows.Forms.Label();
            this.totalDefect = new System.Windows.Forms.Label();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelCam = new System.Windows.Forms.Label();
            this.layoutSize = new System.Windows.Forms.TableLayoutPanel();
            this.useSize = new System.Windows.Forms.CheckBox();
            this.labelMin = new System.Windows.Forms.Label();
            this.labelMaxUnit = new System.Windows.Forms.Label();
            this.sizeMax = new System.Windows.Forms.NumericUpDown();
            this.labelMinUnit = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.sizeMin = new System.Windows.Forms.NumericUpDown();
            this.layoutType = new System.Windows.Forms.TableLayoutPanel();
            this.total = new System.Windows.Forms.RadioButton();
            this.sheetAttack = new System.Windows.Forms.RadioButton();
            this.pinhole = new System.Windows.Forms.RadioButton();
            this.dielectric = new System.Windows.Forms.RadioButton();
            this.noprint = new System.Windows.Forms.RadioButton();
            this.maxViewCount = new System.Windows.Forms.NumericUpDown();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.panelSelectCam = new System.Windows.Forms.Panel();
            this.checkBoxCam = new System.Windows.Forms.CheckBox();
            this.labelMaxViewCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).BeginInit();
            this.layoutMain.SuspendLayout();
            this.layoutSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).BeginInit();
            this.layoutType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxViewCount)).BeginInit();
            this.panelSelectCam.SuspendLayout();
            this.SuspendLayout();
            // 
            // defectList
            // 
            this.defectList.AllowUserToAddRows = false;
            this.defectList.AllowUserToDeleteRows = false;
            this.defectList.AllowUserToResizeRows = false;
            this.defectList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.defectList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.defectList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.defectList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.defectList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIndex,
            this.columnCam,
            this.columnType,
            this.columnPosition,
            this.columnSize,
            this.columnInfo,
            this.columnImage});
            this.layoutMain.SetColumnSpan(this.defectList, 5);
            this.defectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectList.Location = new System.Drawing.Point(1, 132);
            this.defectList.Margin = new System.Windows.Forms.Padding(0);
            this.defectList.MultiSelect = false;
            this.defectList.Name = "defectList";
            this.defectList.ReadOnly = true;
            this.defectList.RowHeadersVisible = false;
            this.defectList.RowTemplate.Height = 23;
            this.defectList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.defectList.Size = new System.Drawing.Size(846, 418);
            this.defectList.TabIndex = 0;
            this.defectList.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.defectList_CellValueNeeded);
            this.defectList.SelectionChanged += new System.EventHandler(this.defectList_SelectionChanged);
            // 
            // columnIndex
            // 
            this.columnIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnIndex.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnIndex.HeaderText = "Pattern";
            this.columnIndex.Name = "columnIndex";
            this.columnIndex.ReadOnly = true;
            this.columnIndex.Width = 91;
            // 
            // columnCam
            // 
            this.columnCam.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnCam.DefaultCellStyle = dataGridViewCellStyle3;
            this.columnCam.HeaderText = "Cam";
            this.columnCam.Name = "columnCam";
            this.columnCam.ReadOnly = true;
            this.columnCam.Width = 69;
            // 
            // columnType
            // 
            this.columnType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnType.DefaultCellStyle = dataGridViewCellStyle4;
            this.columnType.HeaderText = "Type";
            this.columnType.Name = "columnType";
            this.columnType.ReadOnly = true;
            this.columnType.Width = 72;
            // 
            // columnPosition
            // 
            this.columnPosition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnPosition.HeaderText = "Position";
            this.columnPosition.Name = "columnPosition";
            this.columnPosition.ReadOnly = true;
            this.columnPosition.Width = 96;
            // 
            // columnSize
            // 
            this.columnSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnSize.HeaderText = "Size";
            this.columnSize.Name = "columnSize";
            this.columnSize.ReadOnly = true;
            this.columnSize.Width = 65;
            // 
            // columnInfo
            // 
            this.columnInfo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.columnInfo.DefaultCellStyle = dataGridViewCellStyle5;
            this.columnInfo.HeaderText = "Info";
            this.columnInfo.Name = "columnInfo";
            this.columnInfo.ReadOnly = true;
            this.columnInfo.Width = 66;
            // 
            // columnImage
            // 
            this.columnImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnImage.HeaderText = "Image";
            this.columnImage.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.columnImage.Name = "columnImage";
            this.columnImage.ReadOnly = true;
            // 
            // labelCurrentDefect
            // 
            this.labelCurrentDefect.AutoSize = true;
            this.labelCurrentDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelCurrentDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrentDefect.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelCurrentDefect.Location = new System.Drawing.Point(1, 1);
            this.labelCurrentDefect.Margin = new System.Windows.Forms.Padding(0);
            this.labelCurrentDefect.Name = "labelCurrentDefect";
            this.labelCurrentDefect.Size = new System.Drawing.Size(120, 40);
            this.labelCurrentDefect.TabIndex = 100;
            this.labelCurrentDefect.Text = "Current";
            this.labelCurrentDefect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFilterTitle
            // 
            this.labelFilterTitle.AutoSize = true;
            this.labelFilterTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutMain.SetColumnSpan(this.labelFilterTitle, 4);
            this.labelFilterTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFilterTitle.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelFilterTitle.Location = new System.Drawing.Point(122, 1);
            this.labelFilterTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelFilterTitle.Name = "labelFilterTitle";
            this.labelFilterTitle.Size = new System.Drawing.Size(725, 40);
            this.labelFilterTitle.TabIndex = 1;
            this.labelFilterTitle.Text = "Filter";
            this.labelFilterTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // totalDefect
            // 
            this.totalDefect.AutoSize = true;
            this.totalDefect.BackColor = System.Drawing.Color.White;
            this.totalDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalDefect.Font = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Bold);
            this.totalDefect.Location = new System.Drawing.Point(1, 42);
            this.totalDefect.Margin = new System.Windows.Forms.Padding(0);
            this.totalDefect.Name = "totalDefect";
            this.layoutMain.SetRowSpan(this.totalDefect, 3);
            this.totalDefect.Size = new System.Drawing.Size(120, 89);
            this.totalDefect.TabIndex = 102;
            this.totalDefect.Text = "0";
            this.totalDefect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutMain
            // 
            this.layoutMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutMain.ColumnCount = 5;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 83.33333F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutMain.Controls.Add(this.labelCam, 1, 1);
            this.layoutMain.Controls.Add(this.layoutSize, 2, 3);
            this.layoutMain.Controls.Add(this.layoutType, 2, 2);
            this.layoutMain.Controls.Add(this.maxViewCount, 4, 1);
            this.layoutMain.Controls.Add(this.labelSize, 1, 3);
            this.layoutMain.Controls.Add(this.labelCurrentDefect, 0, 0);
            this.layoutMain.Controls.Add(this.labelType, 1, 2);
            this.layoutMain.Controls.Add(this.panelSelectCam, 2, 1);
            this.layoutMain.Controls.Add(this.totalDefect, 0, 1);
            this.layoutMain.Controls.Add(this.labelFilterTitle, 1, 0);
            this.layoutMain.Controls.Add(this.defectList, 0, 4);
            this.layoutMain.Controls.Add(this.labelMaxViewCount, 3, 1);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 5;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(848, 551);
            this.layoutMain.TabIndex = 106;
            // 
            // labelCam
            // 
            this.labelCam.AutoSize = true;
            this.labelCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCam.Location = new System.Drawing.Point(122, 42);
            this.labelCam.Margin = new System.Windows.Forms.Padding(0);
            this.labelCam.Name = "labelCam";
            this.labelCam.Size = new System.Drawing.Size(100, 29);
            this.labelCam.TabIndex = 1;
            this.labelCam.Text = "Cam";
            this.labelCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutSize
            // 
            this.layoutSize.ColumnCount = 8;
            this.layoutMain.SetColumnSpan(this.layoutSize, 3);
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSize.Controls.Add(this.useSize, 0, 0);
            this.layoutSize.Controls.Add(this.labelMin, 1, 0);
            this.layoutSize.Controls.Add(this.labelMaxUnit, 6, 0);
            this.layoutSize.Controls.Add(this.sizeMax, 5, 0);
            this.layoutSize.Controls.Add(this.labelMinUnit, 3, 0);
            this.layoutSize.Controls.Add(this.labelMax, 4, 0);
            this.layoutSize.Controls.Add(this.sizeMin, 2, 0);
            this.layoutSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSize.Location = new System.Drawing.Point(223, 102);
            this.layoutSize.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSize.Name = "layoutSize";
            this.layoutSize.RowCount = 1;
            this.layoutSize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSize.Size = new System.Drawing.Size(624, 29);
            this.layoutSize.TabIndex = 90;
            // 
            // useSize
            // 
            this.useSize.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.useSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.useSize.Location = new System.Drawing.Point(0, 0);
            this.useSize.Margin = new System.Windows.Forms.Padding(0);
            this.useSize.Name = "useSize";
            this.useSize.Size = new System.Drawing.Size(35, 29);
            this.useSize.TabIndex = 98;
            this.useSize.UseVisualStyleBackColor = true;
            this.useSize.CheckedChanged += new System.EventHandler(this.useSize_CheckedChanged);
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.BackColor = System.Drawing.Color.Transparent;
            this.labelMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMin.Location = new System.Drawing.Point(35, 0);
            this.labelMin.Margin = new System.Windows.Forms.Padding(0);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(70, 29);
            this.labelMin.TabIndex = 73;
            this.labelMin.Text = "Min";
            this.labelMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMaxUnit
            // 
            this.labelMaxUnit.AutoSize = true;
            this.labelMaxUnit.BackColor = System.Drawing.Color.Transparent;
            this.labelMaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxUnit.Location = new System.Drawing.Point(410, 0);
            this.labelMaxUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMaxUnit.Name = "labelMaxUnit";
            this.labelMaxUnit.Size = new System.Drawing.Size(70, 29);
            this.labelMaxUnit.TabIndex = 0;
            this.labelMaxUnit.Text = "[um]";
            this.labelMaxUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
            this.sizeMax.Location = new System.Drawing.Point(310, 0);
            this.sizeMax.Margin = new System.Windows.Forms.Padding(0);
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
            this.labelMinUnit.Location = new System.Drawing.Point(205, 0);
            this.labelMinUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMinUnit.Name = "labelMinUnit";
            this.labelMinUnit.Size = new System.Drawing.Size(35, 29);
            this.labelMinUnit.TabIndex = 85;
            this.labelMinUnit.Text = "~";
            this.labelMinUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.BackColor = System.Drawing.Color.Transparent;
            this.labelMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMax.Location = new System.Drawing.Point(240, 0);
            this.labelMax.Margin = new System.Windows.Forms.Padding(0);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(70, 29);
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
            this.sizeMin.Location = new System.Drawing.Point(105, 0);
            this.sizeMin.Margin = new System.Windows.Forms.Padding(0);
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
            // layoutType
            // 
            this.layoutType.ColumnCount = 6;
            this.layoutMain.SetColumnSpan(this.layoutType, 3);
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutType.Controls.Add(this.total, 0, 0);
            this.layoutType.Controls.Add(this.sheetAttack, 3, 0);
            this.layoutType.Controls.Add(this.pinhole, 2, 0);
            this.layoutType.Controls.Add(this.dielectric, 4, 0);
            this.layoutType.Controls.Add(this.noprint, 1, 0);
            this.layoutType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutType.Location = new System.Drawing.Point(223, 72);
            this.layoutType.Margin = new System.Windows.Forms.Padding(0);
            this.layoutType.Name = "layoutType";
            this.layoutType.RowCount = 1;
            this.layoutType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutType.Size = new System.Drawing.Size(624, 29);
            this.layoutType.TabIndex = 86;
            // 
            // total
            // 
            this.total.Appearance = System.Windows.Forms.Appearance.Button;
            this.total.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.total.Checked = true;
            this.total.Cursor = System.Windows.Forms.Cursors.Hand;
            this.total.Dock = System.Windows.Forms.DockStyle.Fill;
            this.total.FlatAppearance.BorderSize = 0;
            this.total.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.total.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.total.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.total.Location = new System.Drawing.Point(0, 0);
            this.total.Margin = new System.Windows.Forms.Padding(0);
            this.total.Name = "total";
            this.total.Size = new System.Drawing.Size(100, 30);
            this.total.TabIndex = 86;
            this.total.TabStop = true;
            this.total.Text = "Total";
            this.total.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.total.UseVisualStyleBackColor = false;
            this.total.CheckedChanged += new System.EventHandler(this.total_CheckedChanged);
            // 
            // sheetAttack
            // 
            this.sheetAttack.Appearance = System.Windows.Forms.Appearance.Button;
            this.sheetAttack.AutoSize = true;
            this.sheetAttack.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sheetAttack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.sheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetAttack.FlatAppearance.BorderSize = 0;
            this.sheetAttack.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.sheetAttack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.sheetAttack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sheetAttack.ForeColor = System.Drawing.Color.Maroon;
            this.sheetAttack.Location = new System.Drawing.Point(360, 0);
            this.sheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.sheetAttack.Name = "sheetAttack";
            this.sheetAttack.Size = new System.Drawing.Size(130, 30);
            this.sheetAttack.TabIndex = 86;
            this.sheetAttack.Text = "SheetAttack";
            this.sheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sheetAttack.UseVisualStyleBackColor = false;
            this.sheetAttack.CheckedChanged += new System.EventHandler(this.sheetAttack_CheckedChanged);
            // 
            // pinhole
            // 
            this.pinhole.Appearance = System.Windows.Forms.Appearance.Button;
            this.pinhole.AutoSize = true;
            this.pinhole.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pinhole.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pinhole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pinhole.FlatAppearance.BorderSize = 0;
            this.pinhole.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.pinhole.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.pinhole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pinhole.ForeColor = System.Drawing.Color.DarkMagenta;
            this.pinhole.Location = new System.Drawing.Point(230, 0);
            this.pinhole.Margin = new System.Windows.Forms.Padding(0);
            this.pinhole.Name = "pinhole";
            this.pinhole.Size = new System.Drawing.Size(130, 30);
            this.pinhole.TabIndex = 83;
            this.pinhole.Text = "Pinhole";
            this.pinhole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pinhole.UseVisualStyleBackColor = false;
            this.pinhole.CheckedChanged += new System.EventHandler(this.pinHole_CheckedChanged);
            // 
            // dielectric
            // 
            this.dielectric.Appearance = System.Windows.Forms.Appearance.Button;
            this.dielectric.AutoSize = true;
            this.dielectric.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dielectric.Cursor = System.Windows.Forms.Cursors.Hand;
            this.dielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dielectric.FlatAppearance.BorderSize = 0;
            this.dielectric.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.dielectric.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.dielectric.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dielectric.ForeColor = System.Drawing.Color.Blue;
            this.dielectric.Location = new System.Drawing.Point(490, 0);
            this.dielectric.Margin = new System.Windows.Forms.Padding(0);
            this.dielectric.Name = "dielectric";
            this.dielectric.Size = new System.Drawing.Size(130, 30);
            this.dielectric.TabIndex = 0;
            this.dielectric.Text = "Dielectric";
            this.dielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dielectric.UseVisualStyleBackColor = false;
            this.dielectric.CheckedChanged += new System.EventHandler(this.dielectric_CheckedChanged);
            // 
            // noprint
            // 
            this.noprint.Appearance = System.Windows.Forms.Appearance.Button;
            this.noprint.AutoSize = true;
            this.noprint.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.noprint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.noprint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noprint.FlatAppearance.BorderSize = 0;
            this.noprint.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.noprint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.noprint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noprint.ForeColor = System.Drawing.Color.OrangeRed;
            this.noprint.Location = new System.Drawing.Point(100, 0);
            this.noprint.Margin = new System.Windows.Forms.Padding(0);
            this.noprint.Name = "noprint";
            this.noprint.Size = new System.Drawing.Size(130, 30);
            this.noprint.TabIndex = 87;
            this.noprint.Text = "Noprint";
            this.noprint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.noprint.UseVisualStyleBackColor = false;
            this.noprint.CheckedChanged += new System.EventHandler(this.noprint_CheckedChanged);
            // 
            // maxViewCount
            // 
            this.maxViewCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.maxViewCount.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.maxViewCount.Location = new System.Drawing.Point(759, 42);
            this.maxViewCount.Margin = new System.Windows.Forms.Padding(0);
            this.maxViewCount.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.maxViewCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.maxViewCount.Name = "maxViewCount";
            this.maxViewCount.Size = new System.Drawing.Size(88, 29);
            this.maxViewCount.TabIndex = 0;
            this.maxViewCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.maxViewCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.maxViewCount.ValueChanged += new System.EventHandler(this.sizeMax_ValueChanged);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSize.Location = new System.Drawing.Point(122, 102);
            this.labelSize.Margin = new System.Windows.Forms.Padding(0);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(100, 29);
            this.labelSize.TabIndex = 0;
            this.labelSize.Text = "Size";
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Location = new System.Drawing.Point(122, 72);
            this.labelType.Margin = new System.Windows.Forms.Padding(0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(100, 29);
            this.labelType.TabIndex = 0;
            this.labelType.Text = "Type";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSelectCam
            // 
            this.panelSelectCam.Controls.Add(this.checkBoxCam);
            this.panelSelectCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelectCam.Location = new System.Drawing.Point(223, 42);
            this.panelSelectCam.Margin = new System.Windows.Forms.Padding(0);
            this.panelSelectCam.Name = "panelSelectCam";
            this.panelSelectCam.Size = new System.Drawing.Size(434, 29);
            this.panelSelectCam.TabIndex = 86;
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
            this.checkBoxCam.Size = new System.Drawing.Size(76, 29);
            this.checkBoxCam.TabIndex = 0;
            this.checkBoxCam.Text = "Defualt";
            this.checkBoxCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCam.UseVisualStyleBackColor = true;
            this.checkBoxCam.Visible = false;
            // 
            // labelMaxViewCount
            // 
            this.labelMaxViewCount.AutoSize = true;
            this.labelMaxViewCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxViewCount.Location = new System.Drawing.Point(658, 42);
            this.labelMaxViewCount.Margin = new System.Windows.Forms.Padding(0);
            this.labelMaxViewCount.Name = "labelMaxViewCount";
            this.labelMaxViewCount.Size = new System.Drawing.Size(100, 29);
            this.labelMaxViewCount.TabIndex = 1;
            this.labelMaxViewCount.Text = "Max View";
            this.labelMaxViewCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DefectPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "DefectPanel";
            this.Size = new System.Drawing.Size(848, 551);
            this.Load += new System.EventHandler(this.DefectPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).EndInit();
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.layoutSize.ResumeLayout(false);
            this.layoutSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).EndInit();
            this.layoutType.ResumeLayout(false);
            this.layoutType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxViewCount)).EndInit();
            this.panelSelectCam.ResumeLayout(false);
            this.panelSelectCam.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView defectList;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        public System.Windows.Forms.Label totalDefect;
        private System.Windows.Forms.Label labelFilterTitle;
        public System.Windows.Forms.Label labelCurrentDefect;
        private System.Windows.Forms.TableLayoutPanel layoutSize;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label labelMaxUnit;
        private System.Windows.Forms.NumericUpDown sizeMax;
        private System.Windows.Forms.Label labelMinUnit;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.NumericUpDown sizeMin;
        private System.Windows.Forms.TableLayoutPanel layoutType;
        private System.Windows.Forms.RadioButton total;
        private System.Windows.Forms.RadioButton sheetAttack;
        private System.Windows.Forms.RadioButton pinhole;
        private System.Windows.Forms.RadioButton dielectric;
        //private System.Windows.Forms.Label labelCam;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Panel panelSelectCam;
        private System.Windows.Forms.CheckBox checkBoxCam;
        private System.Windows.Forms.Label labelCam;
        private System.Windows.Forms.CheckBox useSize;
        private System.Windows.Forms.RadioButton noprint;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCam;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnInfo;
        private System.Windows.Forms.DataGridViewImageColumn columnImage;
        private System.Windows.Forms.NumericUpDown maxViewCount;
        private System.Windows.Forms.Label labelMaxViewCount;
    }
}
