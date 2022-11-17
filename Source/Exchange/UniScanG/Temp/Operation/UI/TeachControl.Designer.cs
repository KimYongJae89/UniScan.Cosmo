//namespace UniScanG.Temp
//{
//    partial class TeachControl
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
//        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
//            this.teachTabControl = new System.Windows.Forms.TabControl();
//            this.tabPageImage = new System.Windows.Forms.TabPage();
//            this.tabPagePattern = new System.Windows.Forms.TabPage();
//            this.patternImageSelector = new System.Windows.Forms.DataGridView();
//            this.ColumnPatternImage = new System.Windows.Forms.DataGridViewImageColumn();
//            this.columnDontCareType = new System.Windows.Forms.DataGridViewCheckBoxColumn();
//            this.columnCnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.columnCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.regionImage = new System.Windows.Forms.PictureBox();
//            this.teachTabControl.SuspendLayout();
//            this.tabPageImage.SuspendLayout();
//            this.tabPagePattern.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.patternImageSelector)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.regionImage)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // teachTabControl
//            // 
//            this.teachTabControl.Controls.Add(this.tabPageImage);
//            this.teachTabControl.Controls.Add(this.tabPagePattern);
//            this.teachTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.teachTabControl.Location = new System.Drawing.Point(0, 0);
//            this.teachTabControl.Name = "teachTabControl";
//            this.teachTabControl.SelectedIndex = 0;
//            this.teachTabControl.Size = new System.Drawing.Size(467, 665);
//            this.teachTabControl.TabIndex = 2;
//            // 
//            // tabPageImage
//            // 
//            this.tabPageImage.Controls.Add(this.regionImage);
//            this.tabPageImage.Location = new System.Drawing.Point(4, 22);
//            this.tabPageImage.Name = "tabPageImage";
//            this.tabPageImage.Padding = new System.Windows.Forms.Padding(3);
//            this.tabPageImage.Size = new System.Drawing.Size(459, 639);
//            this.tabPageImage.TabIndex = 0;
//            this.tabPageImage.Text = "Reagion";
//            this.tabPageImage.UseVisualStyleBackColor = true;
//            // 
//            // tabPagePattern
//            // 
//            this.tabPagePattern.Controls.Add(this.patternImageSelector);
//            this.tabPagePattern.Location = new System.Drawing.Point(4, 22);
//            this.tabPagePattern.Name = "tabPagePattern";
//            this.tabPagePattern.Size = new System.Drawing.Size(459, 639);
//            this.tabPagePattern.TabIndex = 2;
//            this.tabPagePattern.Text = "Pattern";
//            this.tabPagePattern.UseVisualStyleBackColor = true;
//            // 
//            // patternImageSelector
//            // 
//            this.patternImageSelector.AllowUserToAddRows = false;
//            this.patternImageSelector.AllowUserToDeleteRows = false;
//            this.patternImageSelector.AllowUserToResizeColumns = false;
//            this.patternImageSelector.AllowUserToResizeRows = false;
//            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
//            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
//            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
//            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
//            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
//            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
//            this.patternImageSelector.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
//            this.patternImageSelector.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            this.patternImageSelector.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
//            this.ColumnPatternImage,
//            this.columnDontCareType,
//            this.columnCnt,
//            this.columnCount});
//            this.patternImageSelector.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.patternImageSelector.Location = new System.Drawing.Point(0, 0);
//            this.patternImageSelector.Margin = new System.Windows.Forms.Padding(2);
//            this.patternImageSelector.Name = "patternImageSelector";
//            this.patternImageSelector.RowHeadersVisible = false;
//            this.patternImageSelector.RowTemplate.Height = 23;
//            this.patternImageSelector.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
//            this.patternImageSelector.Size = new System.Drawing.Size(459, 639);
//            this.patternImageSelector.TabIndex = 38;
//            this.patternImageSelector.SelectionChanged += new System.EventHandler(this.patternImageSelector_SelectionChanged);
//            // 
//            // ColumnPatternImage
//            // 
//            this.ColumnPatternImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
//            this.ColumnPatternImage.HeaderText = "Pattern";
//            this.ColumnPatternImage.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
//            this.ColumnPatternImage.Name = "ColumnPatternImage";
//            this.ColumnPatternImage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
//            // 
//            // columnDontCareType
//            // 
//            this.columnDontCareType.FillWeight = 70F;
//            this.columnDontCareType.HeaderText = "Use";
//            this.columnDontCareType.Name = "columnDontCareType";
//            this.columnDontCareType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
//            this.columnDontCareType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
//            this.columnDontCareType.Width = 70;
//            // 
//            // columnCnt
//            // 
//            this.columnCnt.HeaderText = "Cnt";
//            this.columnCnt.Name = "columnCnt";
//            this.columnCnt.Width = 75;
//            // 
//            // columnCount
//            // 
//            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
//            this.columnCount.DefaultCellStyle = dataGridViewCellStyle4;
//            this.columnCount.FillWeight = 150F;
//            this.columnCount.HeaderText = "Info";
//            this.columnCount.Name = "columnCount";
//            this.columnCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
//            this.columnCount.Width = 125;
//            // 
//            // regionImage
//            // 
//            this.regionImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.regionImage.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.regionImage.Location = new System.Drawing.Point(3, 3);
//            this.regionImage.Name = "regionImage";
//            this.regionImage.Size = new System.Drawing.Size(453, 633);
//            this.regionImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
//            this.regionImage.TabIndex = 2;
//            this.regionImage.TabStop = false;
//            // 
//            // TeachControl
//            // 
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
//            this.Controls.Add(this.teachTabControl);
//            this.Name = "TeachControl";
//            this.Size = new System.Drawing.Size(467, 665);
//            this.teachTabControl.ResumeLayout(false);
//            this.tabPageImage.ResumeLayout(false);
//            this.tabPagePattern.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.patternImageSelector)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.regionImage)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion
//        private System.Windows.Forms.TabControl teachTabControl;
//        private System.Windows.Forms.TabPage tabPageImage;
//        private System.Windows.Forms.TabPage tabPagePattern;
//        public System.Windows.Forms.DataGridView patternImageSelector;
//        private System.Windows.Forms.DataGridViewImageColumn ColumnPatternImage;
//        private System.Windows.Forms.DataGridViewCheckBoxColumn columnDontCareType;
//        private System.Windows.Forms.DataGridViewTextBoxColumn columnCnt;
//        private System.Windows.Forms.DataGridViewTextBoxColumn columnCount;
//        private System.Windows.Forms.PictureBox regionImage;
//    }
//}
