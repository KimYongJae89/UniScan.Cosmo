namespace UniScanG.UI.Teach.Inspector
{
    partial class ImageController
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.layoutImage = new System.Windows.Forms.TableLayoutPanel();
            this.panelDefectLegend = new System.Windows.Forms.Panel();
            this.labelImage = new System.Windows.Forms.Label();
            this.layoutImageInfo = new System.Windows.Forms.TableLayoutPanel();
            this.buttonDeleteFigure = new Infragistics.Win.Misc.UltraButton();
            this.buttonZoomFit = new Infragistics.Win.Misc.UltraButton();
            this.v = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.buttonZoomOut = new Infragistics.Win.Misc.UltraButton();
            this.buttonZoomIn = new Infragistics.Win.Misc.UltraButton();
            this.y = new System.Windows.Forms.Label();
            this.labelV = new System.Windows.Forms.Label();
            this.x = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.imageContanier = new System.Windows.Forms.Panel();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.layoutInfo = new System.Windows.Forms.TableLayoutPanel();
            this.defectList = new System.Windows.Forms.DataGridView();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.labelDefect = new System.Windows.Forms.Label();
            this.layoutFilter = new System.Windows.Forms.TableLayoutPanel();
            this.layoutSelectFilter = new System.Windows.Forms.TableLayoutPanel();
            this.panelDefectType = new System.Windows.Forms.Panel();
            this.layoutSize = new System.Windows.Forms.TableLayoutPanel();
            this.useSize = new System.Windows.Forms.CheckBox();
            this.labelMin = new System.Windows.Forms.Label();
            this.labelMaxUnit = new System.Windows.Forms.Label();
            this.sizeMax = new System.Windows.Forms.NumericUpDown();
            this.labelMinUnit = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.sizeMin = new System.Windows.Forms.NumericUpDown();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.layoutDefectInfo = new System.Windows.Forms.TableLayoutPanel();
            this.layoutInspectTime = new System.Windows.Forms.TableLayoutPanel();
            this.labelInspectTime = new System.Windows.Forms.Label();
            this.panelInspectTime = new System.Windows.Forms.Panel();
            this.inspectTime = new System.Windows.Forms.Label();
            this.labelinspectTimeUnit = new System.Windows.Forms.Label();
            this.totalDefectNum = new System.Windows.Forms.Label();
            this.labelTotalDefectNum = new System.Windows.Forms.Label();
            this.ultraFlowLayoutManager1 = new Infragistics.Win.Misc.UltraFlowLayoutManager(this.components);
            this.layoutImage.SuspendLayout();
            this.layoutImageInfo.SuspendLayout();
            this.layoutMain.SuspendLayout();
            this.layoutInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).BeginInit();
            this.layoutFilter.SuspendLayout();
            this.layoutSelectFilter.SuspendLayout();
            this.layoutSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).BeginInit();
            this.layoutDefectInfo.SuspendLayout();
            this.layoutInspectTime.SuspendLayout();
            this.panelInspectTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutImage
            // 
            this.layoutImage.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutImage.ColumnCount = 1;
            this.layoutImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutImage.Controls.Add(this.panelDefectLegend, 0, 3);
            this.layoutImage.Controls.Add(this.labelImage, 0, 0);
            this.layoutImage.Controls.Add(this.layoutImageInfo, 0, 1);
            this.layoutImage.Controls.Add(this.imageContanier, 0, 2);
            this.layoutImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutImage.Location = new System.Drawing.Point(0, 0);
            this.layoutImage.Margin = new System.Windows.Forms.Padding(0);
            this.layoutImage.Name = "layoutImage";
            this.layoutImage.Padding = new System.Windows.Forms.Padding(5);
            this.layoutImage.RowCount = 4;
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutImage.Size = new System.Drawing.Size(592, 543);
            this.layoutImage.TabIndex = 5;
            // 
            // panelDefectLegend
            // 
            this.panelDefectLegend.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDefectLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDefectLegend.Location = new System.Drawing.Point(6, 497);
            this.panelDefectLegend.Margin = new System.Windows.Forms.Padding(0);
            this.panelDefectLegend.Name = "panelDefectLegend";
            this.panelDefectLegend.Size = new System.Drawing.Size(580, 40);
            this.panelDefectLegend.TabIndex = 55;
            // 
            // labelImage
            // 
            this.labelImage.AutoSize = true;
            this.labelImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImage.Font = new System.Drawing.Font("Malgun Gothic", 20F, System.Drawing.FontStyle.Bold);
            this.labelImage.Location = new System.Drawing.Point(6, 6);
            this.labelImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelImage.Name = "labelImage";
            this.labelImage.Size = new System.Drawing.Size(580, 40);
            this.labelImage.TabIndex = 54;
            this.labelImage.Text = "Image";
            this.labelImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutImageInfo
            // 
            this.layoutImageInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutImageInfo.ColumnCount = 10;
            this.layoutImageInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutImageInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutImageInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutImageInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutImageInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutImageInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutImageInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutImageInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutImageInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutImageInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutImageInfo.Controls.Add(this.buttonDeleteFigure, 9, 0);
            this.layoutImageInfo.Controls.Add(this.buttonZoomFit, 8, 0);
            this.layoutImageInfo.Controls.Add(this.v, 5, 0);
            this.layoutImageInfo.Controls.Add(this.labelX, 0, 0);
            this.layoutImageInfo.Controls.Add(this.buttonZoomOut, 7, 0);
            this.layoutImageInfo.Controls.Add(this.buttonZoomIn, 6, 0);
            this.layoutImageInfo.Controls.Add(this.y, 3, 0);
            this.layoutImageInfo.Controls.Add(this.labelV, 4, 0);
            this.layoutImageInfo.Controls.Add(this.x, 1, 0);
            this.layoutImageInfo.Controls.Add(this.labelY, 2, 0);
            this.layoutImageInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutImageInfo.Location = new System.Drawing.Point(6, 47);
            this.layoutImageInfo.Margin = new System.Windows.Forms.Padding(0);
            this.layoutImageInfo.Name = "layoutImageInfo";
            this.layoutImageInfo.RowCount = 1;
            this.layoutImageInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutImageInfo.Size = new System.Drawing.Size(580, 50);
            this.layoutImageInfo.TabIndex = 6;
            // 
            // buttonDeleteFigure
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.Image = global::UniScanG.Properties.Resources.delete_32;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonDeleteFigure.Appearance = appearance1;
            this.buttonDeleteFigure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDeleteFigure.ImageSize = new System.Drawing.Size(30, 30);
            this.buttonDeleteFigure.Location = new System.Drawing.Point(529, 1);
            this.buttonDeleteFigure.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeleteFigure.Name = "buttonDeleteFigure";
            this.buttonDeleteFigure.Size = new System.Drawing.Size(50, 48);
            this.buttonDeleteFigure.TabIndex = 2;
            this.buttonDeleteFigure.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonDeleteFigure.Click += new System.EventHandler(this.buttonDeleteFigure_Click);
            // 
            // buttonZoomFit
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.Image = global::UniScanG.Properties.Resources.zoom_fit_32;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonZoomFit.Appearance = appearance2;
            this.buttonZoomFit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonZoomFit.ImageSize = new System.Drawing.Size(30, 30);
            this.buttonZoomFit.Location = new System.Drawing.Point(478, 1);
            this.buttonZoomFit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonZoomFit.Name = "buttonZoomFit";
            this.buttonZoomFit.Size = new System.Drawing.Size(50, 48);
            this.buttonZoomFit.TabIndex = 1;
            this.buttonZoomFit.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonZoomFit.Click += new System.EventHandler(this.buttonZoomFit_Click);
            // 
            // v
            // 
            this.v.AutoSize = true;
            this.v.Dock = System.Windows.Forms.DockStyle.Fill;
            this.v.Location = new System.Drawing.Point(282, 1);
            this.v.Margin = new System.Windows.Forms.Padding(0);
            this.v.Name = "v";
            this.v.Size = new System.Drawing.Size(93, 48);
            this.v.TabIndex = 9;
            this.v.Text = "255";
            this.v.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelX.Location = new System.Drawing.Point(1, 1);
            this.labelX.Margin = new System.Windows.Forms.Padding(0);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(30, 48);
            this.labelX.TabIndex = 7;
            this.labelX.Text = "X";
            this.labelX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonZoomOut
            // 
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.Image = global::UniScanG.Properties.Resources.zoom_out_32;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonZoomOut.Appearance = appearance3;
            this.buttonZoomOut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonZoomOut.ImageSize = new System.Drawing.Size(30, 30);
            this.buttonZoomOut.Location = new System.Drawing.Point(427, 1);
            this.buttonZoomOut.Margin = new System.Windows.Forms.Padding(0);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(50, 48);
            this.buttonZoomOut.TabIndex = 0;
            this.buttonZoomOut.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonZoomOut.Click += new System.EventHandler(this.buttonZoomOut_Click);
            // 
            // buttonZoomIn
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.Image = global::UniScanG.Properties.Resources.zoom_in_32;
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance4.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonZoomIn.Appearance = appearance4;
            this.buttonZoomIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonZoomIn.ImageSize = new System.Drawing.Size(30, 30);
            this.buttonZoomIn.Location = new System.Drawing.Point(376, 1);
            this.buttonZoomIn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(50, 48);
            this.buttonZoomIn.TabIndex = 0;
            this.buttonZoomIn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
            // 
            // y
            // 
            this.y.AutoSize = true;
            this.y.Dock = System.Windows.Forms.DockStyle.Fill;
            this.y.Location = new System.Drawing.Point(157, 1);
            this.y.Margin = new System.Windows.Forms.Padding(0);
            this.y.Name = "y";
            this.y.Size = new System.Drawing.Size(93, 48);
            this.y.TabIndex = 10;
            this.y.Text = "255";
            this.y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelV
            // 
            this.labelV.AutoSize = true;
            this.labelV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelV.Location = new System.Drawing.Point(251, 1);
            this.labelV.Margin = new System.Windows.Forms.Padding(0);
            this.labelV.Name = "labelV";
            this.labelV.Size = new System.Drawing.Size(30, 48);
            this.labelV.TabIndex = 12;
            this.labelV.Text = "V";
            this.labelV.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // x
            // 
            this.x.AutoSize = true;
            this.x.Dock = System.Windows.Forms.DockStyle.Fill;
            this.x.Location = new System.Drawing.Point(32, 1);
            this.x.Margin = new System.Windows.Forms.Padding(0);
            this.x.Name = "x";
            this.x.Size = new System.Drawing.Size(93, 48);
            this.x.TabIndex = 8;
            this.x.Text = "255";
            this.x.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelY.Location = new System.Drawing.Point(126, 1);
            this.labelY.Margin = new System.Windows.Forms.Padding(0);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(30, 48);
            this.labelY.TabIndex = 11;
            this.labelY.Text = "Y";
            this.labelY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageContanier
            // 
            this.imageContanier.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageContanier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageContanier.Location = new System.Drawing.Point(6, 98);
            this.imageContanier.Margin = new System.Windows.Forms.Padding(0);
            this.imageContanier.Name = "imageContanier";
            this.imageContanier.Padding = new System.Windows.Forms.Padding(5);
            this.imageContanier.Size = new System.Drawing.Size(580, 398);
            this.imageContanier.TabIndex = 7;
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 2;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.layoutMain.Controls.Add(this.layoutInfo, 1, 0);
            this.layoutMain.Controls.Add(this.layoutImage, 0, 0);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 1;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(1316, 543);
            this.layoutMain.TabIndex = 0;
            // 
            // layoutInfo
            // 
            this.layoutInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutInfo.ColumnCount = 1;
            this.layoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInfo.Controls.Add(this.defectList, 0, 3);
            this.layoutInfo.Controls.Add(this.labelDefect, 0, 0);
            this.layoutInfo.Controls.Add(this.layoutFilter, 0, 2);
            this.layoutInfo.Controls.Add(this.layoutDefectInfo, 0, 1);
            this.layoutInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutInfo.Location = new System.Drawing.Point(592, 0);
            this.layoutInfo.Margin = new System.Windows.Forms.Padding(0);
            this.layoutInfo.Name = "layoutInfo";
            this.layoutInfo.Padding = new System.Windows.Forms.Padding(5);
            this.layoutInfo.RowCount = 4;
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutInfo.Size = new System.Drawing.Size(724, 543);
            this.layoutInfo.TabIndex = 6;
            // 
            // defectList
            // 
            this.defectList.AllowUserToAddRows = false;
            this.defectList.AllowUserToDeleteRows = false;
            this.defectList.AllowUserToResizeRows = false;
            this.defectList.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Bold);
            this.defectList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.defectList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.defectList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnName,
            this.columnInfo,
            this.columnImage});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.defectList.DefaultCellStyle = dataGridViewCellStyle4;
            this.defectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectList.EnableHeadersVisualStyles = false;
            this.defectList.Location = new System.Drawing.Point(6, 189);
            this.defectList.Margin = new System.Windows.Forms.Padding(0);
            this.defectList.MultiSelect = false;
            this.defectList.Name = "defectList";
            this.defectList.ReadOnly = true;
            this.defectList.RowHeadersVisible = false;
            this.defectList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.defectList.Size = new System.Drawing.Size(712, 348);
            this.defectList.TabIndex = 111;
            this.defectList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.defectList_CellContentClick);
            this.defectList.SelectionChanged += new System.EventHandler(this.defectList_SelectionChanged);
            // 
            // columnName
            // 
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.columnName.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnName.HeaderText = "Type";
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
            // 
            // columnInfo
            // 
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.columnInfo.DefaultCellStyle = dataGridViewCellStyle3;
            this.columnInfo.HeaderText = "Info";
            this.columnInfo.Name = "columnInfo";
            this.columnInfo.ReadOnly = true;
            this.columnInfo.Width = 300;
            // 
            // columnImage
            // 
            this.columnImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnImage.HeaderText = "Image";
            this.columnImage.Name = "columnImage";
            this.columnImage.ReadOnly = true;
            this.columnImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // labelDefect
            // 
            this.labelDefect.AutoSize = true;
            this.labelDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefect.Font = new System.Drawing.Font("Malgun Gothic", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelDefect.Location = new System.Drawing.Point(6, 6);
            this.labelDefect.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefect.Name = "labelDefect";
            this.labelDefect.Size = new System.Drawing.Size(712, 40);
            this.labelDefect.TabIndex = 57;
            this.labelDefect.Text = "Defect";
            this.labelDefect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutFilter
            // 
            this.layoutFilter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutFilter.ColumnCount = 1;
            this.layoutFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFilter.Controls.Add(this.layoutSelectFilter, 0, 0);
            this.layoutFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutFilter.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Bold);
            this.layoutFilter.Location = new System.Drawing.Point(6, 98);
            this.layoutFilter.Margin = new System.Windows.Forms.Padding(0);
            this.layoutFilter.Name = "layoutFilter";
            this.layoutFilter.RowCount = 1;
            this.layoutFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutFilter.Size = new System.Drawing.Size(712, 90);
            this.layoutFilter.TabIndex = 108;
            // 
            // layoutSelectFilter
            // 
            this.layoutSelectFilter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutSelectFilter.ColumnCount = 2;
            this.layoutSelectFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutSelectFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSelectFilter.Controls.Add(this.panelDefectType, 1, 0);
            this.layoutSelectFilter.Controls.Add(this.layoutSize, 1, 1);
            this.layoutSelectFilter.Controls.Add(this.labelSize, 0, 1);
            this.layoutSelectFilter.Controls.Add(this.labelType, 0, 0);
            this.layoutSelectFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSelectFilter.Location = new System.Drawing.Point(1, 1);
            this.layoutSelectFilter.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSelectFilter.Name = "layoutSelectFilter";
            this.layoutSelectFilter.RowCount = 2;
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutSelectFilter.Size = new System.Drawing.Size(710, 88);
            this.layoutSelectFilter.TabIndex = 0;
            // 
            // panelDefectType
            // 
            this.panelDefectType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDefectType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDefectType.Location = new System.Drawing.Point(62, 1);
            this.panelDefectType.Margin = new System.Windows.Forms.Padding(0);
            this.panelDefectType.Name = "panelDefectType";
            this.panelDefectType.Size = new System.Drawing.Size(647, 56);
            this.panelDefectType.TabIndex = 91;
            // 
            // layoutSize
            // 
            this.layoutSize.ColumnCount = 8;
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSize.Controls.Add(this.useSize, 0, 0);
            this.layoutSize.Controls.Add(this.labelMin, 1, 0);
            this.layoutSize.Controls.Add(this.labelMaxUnit, 6, 0);
            this.layoutSize.Controls.Add(this.sizeMax, 5, 0);
            this.layoutSize.Controls.Add(this.labelMinUnit, 3, 0);
            this.layoutSize.Controls.Add(this.labelMax, 4, 0);
            this.layoutSize.Controls.Add(this.sizeMin, 2, 0);
            this.layoutSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSize.Location = new System.Drawing.Point(62, 58);
            this.layoutSize.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSize.Name = "layoutSize";
            this.layoutSize.RowCount = 1;
            this.layoutSize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSize.Size = new System.Drawing.Size(647, 29);
            this.layoutSize.TabIndex = 90;
            // 
            // useSize
            // 
            this.useSize.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.useSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.useSize.Location = new System.Drawing.Point(0, 0);
            this.useSize.Margin = new System.Windows.Forms.Padding(0);
            this.useSize.Name = "useSize";
            this.useSize.Size = new System.Drawing.Size(27, 29);
            this.useSize.TabIndex = 97;
            this.useSize.UseVisualStyleBackColor = true;
            this.useSize.CheckedChanged += new System.EventHandler(this.useSize_CheckedChanged);
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.BackColor = System.Drawing.Color.Transparent;
            this.labelMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMin.Location = new System.Drawing.Point(27, 0);
            this.labelMin.Margin = new System.Windows.Forms.Padding(0);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(50, 29);
            this.labelMin.TabIndex = 73;
            this.labelMin.Text = "Min";
            this.labelMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMaxUnit
            // 
            this.labelMaxUnit.AutoSize = true;
            this.labelMaxUnit.BackColor = System.Drawing.Color.Transparent;
            this.labelMaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxUnit.Location = new System.Drawing.Point(287, 0);
            this.labelMaxUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMaxUnit.Name = "labelMaxUnit";
            this.labelMaxUnit.Size = new System.Drawing.Size(50, 29);
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
            this.sizeMax.Location = new System.Drawing.Point(217, 0);
            this.sizeMax.Margin = new System.Windows.Forms.Padding(0);
            this.sizeMax.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.sizeMax.Name = "sizeMax";
            this.sizeMax.Size = new System.Drawing.Size(70, 25);
            this.sizeMax.TabIndex = 0;
            this.sizeMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sizeMax.ValueChanged += new System.EventHandler(this.sizeMax_ValueChanged);
            // 
            // labelMinUnit
            // 
            this.labelMinUnit.AutoSize = true;
            this.labelMinUnit.BackColor = System.Drawing.Color.Transparent;
            this.labelMinUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMinUnit.Location = new System.Drawing.Point(147, 0);
            this.labelMinUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMinUnit.Name = "labelMinUnit";
            this.labelMinUnit.Size = new System.Drawing.Size(20, 29);
            this.labelMinUnit.TabIndex = 85;
            this.labelMinUnit.Text = "~";
            this.labelMinUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.BackColor = System.Drawing.Color.Transparent;
            this.labelMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMax.Location = new System.Drawing.Point(167, 0);
            this.labelMax.Margin = new System.Windows.Forms.Padding(0);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(50, 29);
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
            this.sizeMin.Location = new System.Drawing.Point(77, 0);
            this.sizeMin.Margin = new System.Windows.Forms.Padding(0);
            this.sizeMin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.sizeMin.Name = "sizeMin";
            this.sizeMin.Size = new System.Drawing.Size(70, 25);
            this.sizeMin.TabIndex = 69;
            this.sizeMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sizeMin.ValueChanged += new System.EventHandler(this.sizeMin_ValueChanged);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSize.Location = new System.Drawing.Point(1, 58);
            this.labelSize.Margin = new System.Windows.Forms.Padding(0);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(60, 29);
            this.labelSize.TabIndex = 0;
            this.labelSize.Text = "Size";
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Location = new System.Drawing.Point(1, 1);
            this.labelType.Margin = new System.Windows.Forms.Padding(0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(60, 56);
            this.labelType.TabIndex = 0;
            this.labelType.Text = "Type";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutDefectInfo
            // 
            this.layoutDefectInfo.ColumnCount = 4;
            this.layoutDefectInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 270F));
            this.layoutDefectInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutDefectInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutDefectInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutDefectInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutDefectInfo.Controls.Add(this.layoutInspectTime, 0, 0);
            this.layoutDefectInfo.Controls.Add(this.totalDefectNum, 3, 0);
            this.layoutDefectInfo.Controls.Add(this.labelTotalDefectNum, 2, 0);
            this.layoutDefectInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutDefectInfo.Location = new System.Drawing.Point(6, 47);
            this.layoutDefectInfo.Margin = new System.Windows.Forms.Padding(0);
            this.layoutDefectInfo.Name = "layoutDefectInfo";
            this.layoutDefectInfo.Padding = new System.Windows.Forms.Padding(5);
            this.layoutDefectInfo.RowCount = 1;
            this.layoutDefectInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutDefectInfo.Size = new System.Drawing.Size(712, 50);
            this.layoutDefectInfo.TabIndex = 109;
            // 
            // layoutInspectTime
            // 
            this.layoutInspectTime.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutInspectTime.ColumnCount = 2;
            this.layoutInspectTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutInspectTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutInspectTime.Controls.Add(this.labelInspectTime, 0, 0);
            this.layoutInspectTime.Controls.Add(this.panelInspectTime, 1, 0);
            this.layoutInspectTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutInspectTime.Location = new System.Drawing.Point(5, 5);
            this.layoutInspectTime.Margin = new System.Windows.Forms.Padding(0);
            this.layoutInspectTime.Name = "layoutInspectTime";
            this.layoutInspectTime.RowCount = 1;
            this.layoutInspectTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutInspectTime.Size = new System.Drawing.Size(270, 40);
            this.layoutInspectTime.TabIndex = 0;
            // 
            // labelInspectTime
            // 
            this.labelInspectTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelInspectTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInspectTime.Location = new System.Drawing.Point(1, 1);
            this.labelInspectTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelInspectTime.Name = "labelInspectTime";
            this.labelInspectTime.Size = new System.Drawing.Size(133, 38);
            this.labelInspectTime.TabIndex = 50;
            this.labelInspectTime.Text = "Inspect Time";
            this.labelInspectTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelInspectTime
            // 
            this.panelInspectTime.Controls.Add(this.inspectTime);
            this.panelInspectTime.Controls.Add(this.labelinspectTimeUnit);
            this.panelInspectTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInspectTime.Location = new System.Drawing.Point(135, 1);
            this.panelInspectTime.Margin = new System.Windows.Forms.Padding(0);
            this.panelInspectTime.Name = "panelInspectTime";
            this.panelInspectTime.Size = new System.Drawing.Size(134, 38);
            this.panelInspectTime.TabIndex = 52;
            // 
            // inspectTime
            // 
            this.inspectTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inspectTime.Location = new System.Drawing.Point(0, 0);
            this.inspectTime.Margin = new System.Windows.Forms.Padding(0);
            this.inspectTime.Name = "inspectTime";
            this.inspectTime.Size = new System.Drawing.Size(95, 38);
            this.inspectTime.TabIndex = 52;
            this.inspectTime.Text = "0";
            this.inspectTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelinspectTimeUnit
            // 
            this.labelinspectTimeUnit.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelinspectTimeUnit.Location = new System.Drawing.Point(95, 0);
            this.labelinspectTimeUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelinspectTimeUnit.Name = "labelinspectTimeUnit";
            this.labelinspectTimeUnit.Size = new System.Drawing.Size(39, 38);
            this.labelinspectTimeUnit.TabIndex = 0;
            this.labelinspectTimeUnit.Text = "s";
            this.labelinspectTimeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // totalDefectNum
            // 
            this.totalDefectNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.totalDefectNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalDefectNum.Location = new System.Drawing.Point(607, 5);
            this.totalDefectNum.Margin = new System.Windows.Forms.Padding(0);
            this.totalDefectNum.Name = "totalDefectNum";
            this.totalDefectNum.Size = new System.Drawing.Size(100, 40);
            this.totalDefectNum.TabIndex = 49;
            this.totalDefectNum.Text = "0";
            this.totalDefectNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTotalDefectNum
            // 
            this.labelTotalDefectNum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelTotalDefectNum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTotalDefectNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalDefectNum.Location = new System.Drawing.Point(507, 5);
            this.labelTotalDefectNum.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalDefectNum.Name = "labelTotalDefectNum";
            this.labelTotalDefectNum.Size = new System.Drawing.Size(100, 40);
            this.labelTotalDefectNum.TabIndex = 0;
            this.labelTotalDefectNum.Text = "Total";
            this.labelTotalDefectNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ImageController
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ImageController";
            this.Size = new System.Drawing.Size(1316, 543);
            this.layoutImage.ResumeLayout(false);
            this.layoutImage.PerformLayout();
            this.layoutImageInfo.ResumeLayout(false);
            this.layoutImageInfo.PerformLayout();
            this.layoutMain.ResumeLayout(false);
            this.layoutInfo.ResumeLayout(false);
            this.layoutInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).EndInit();
            this.layoutFilter.ResumeLayout(false);
            this.layoutSelectFilter.ResumeLayout(false);
            this.layoutSelectFilter.PerformLayout();
            this.layoutSize.ResumeLayout(false);
            this.layoutSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).EndInit();
            this.layoutDefectInfo.ResumeLayout(false);
            this.layoutInspectTime.ResumeLayout(false);
            this.panelInspectTime.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutImage;
        private Infragistics.Win.Misc.UltraButton buttonZoomIn;
        private System.Windows.Forms.TableLayoutPanel layoutImageInfo;
        private Infragistics.Win.Misc.UltraButton buttonDeleteFigure;
        private Infragistics.Win.Misc.UltraButton buttonZoomFit;
        private System.Windows.Forms.Label v;
        private System.Windows.Forms.Label labelX;
        private Infragistics.Win.Misc.UltraButton buttonZoomOut;
        private System.Windows.Forms.Label y;
        private System.Windows.Forms.Label labelV;
        private System.Windows.Forms.Label x;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Panel imageContanier;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.TableLayoutPanel layoutInfo;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.Label labelDefect;
        private Infragistics.Win.Misc.UltraFlowLayoutManager ultraFlowLayoutManager1;
        private System.Windows.Forms.DataGridView defectList;
        private System.Windows.Forms.TableLayoutPanel layoutDefectInfo;
        private System.Windows.Forms.TableLayoutPanel layoutInspectTime;
        private System.Windows.Forms.Label labelInspectTime;
        private System.Windows.Forms.Panel panelInspectTime;
        private System.Windows.Forms.Label inspectTime;
        private System.Windows.Forms.Label labelinspectTimeUnit;
        private System.Windows.Forms.Label totalDefectNum;
        private System.Windows.Forms.Label labelTotalDefectNum;
        private System.Windows.Forms.TableLayoutPanel layoutFilter;
        private System.Windows.Forms.TableLayoutPanel layoutSelectFilter;
        private System.Windows.Forms.TableLayoutPanel layoutSize;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label labelMaxUnit;
        private System.Windows.Forms.NumericUpDown sizeMax;
        private System.Windows.Forms.Label labelMinUnit;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.NumericUpDown sizeMin;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.CheckBox useSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnInfo;
        private System.Windows.Forms.DataGridViewImageColumn columnImage;
        private System.Windows.Forms.Panel panelDefectLegend;
        private System.Windows.Forms.Panel panelDefectType;
    }
}
