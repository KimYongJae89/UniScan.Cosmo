//namespace UniScanG.Temp
//{
//    partial class ImageViewer
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
//            this.panelTop = new System.Windows.Forms.Panel();
//            this.buttonZoomFit = new System.Windows.Forms.Button();
//            this.buttonZoomIn = new System.Windows.Forms.Button();
//            this.buttonZoomOut = new System.Windows.Forms.Button();
//            this.labelCamView = new Infragistics.Win.Misc.UltraLabel();
//            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
//            this.camViewPanel = new Infragistics.Win.Misc.UltraPanel();
//            this.lastDefectView = new System.Windows.Forms.DataGridView();
//            this.columnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.columnInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
//            this.columnImage = new System.Windows.Forms.DataGridViewImageColumn();
//            this.total = new System.Windows.Forms.Label();
//            this.index = new System.Windows.Forms.Label();
//            this.panelTop.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
//            this.splitContainer1.Panel1.SuspendLayout();
//            this.splitContainer1.Panel2.SuspendLayout();
//            this.splitContainer1.SuspendLayout();
//            this.camViewPanel.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.lastDefectView)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // panelTop
//            // 
//            this.panelTop.Controls.Add(this.total);
//            this.panelTop.Controls.Add(this.index);
//            this.panelTop.Controls.Add(this.buttonZoomFit);
//            this.panelTop.Controls.Add(this.buttonZoomIn);
//            this.panelTop.Controls.Add(this.buttonZoomOut);
//            this.panelTop.Controls.Add(this.labelCamView);
//            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.panelTop.Location = new System.Drawing.Point(0, 0);
//            this.panelTop.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
//            this.panelTop.Name = "panelTop";
//            this.panelTop.Size = new System.Drawing.Size(1030, 49);
//            this.panelTop.TabIndex = 52;
//            // 
//            // buttonZoomFit
//            // 
//            this.buttonZoomFit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.buttonZoomFit.Image = global::UniScanG.Properties.Resources.zoom_fit_32;
//            this.buttonZoomFit.Location = new System.Drawing.Point(976, 0);
//            this.buttonZoomFit.Name = "buttonZoomFit";
//            this.buttonZoomFit.Size = new System.Drawing.Size(51, 45);
//            this.buttonZoomFit.TabIndex = 61;
//            this.buttonZoomFit.UseVisualStyleBackColor = true;
//            this.buttonZoomFit.Click += new System.EventHandler(this.buttonZoomFit_Click);
//            // 
//            // buttonZoomIn
//            // 
//            this.buttonZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.buttonZoomIn.BackColor = System.Drawing.SystemColors.Control;
//            this.buttonZoomIn.Image = global::UniScanG.Properties.Resources.zoom_in_32;
//            this.buttonZoomIn.Location = new System.Drawing.Point(862, 1);
//            this.buttonZoomIn.Name = "buttonZoomIn";
//            this.buttonZoomIn.Size = new System.Drawing.Size(51, 45);
//            this.buttonZoomIn.TabIndex = 59;
//            this.buttonZoomIn.UseVisualStyleBackColor = false;
//            this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
//            // 
//            // buttonZoomOut
//            // 
//            this.buttonZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.buttonZoomOut.Image = global::UniScanG.Properties.Resources.zoom_out_32;
//            this.buttonZoomOut.Location = new System.Drawing.Point(919, 1);
//            this.buttonZoomOut.Name = "buttonZoomOut";
//            this.buttonZoomOut.Size = new System.Drawing.Size(51, 45);
//            this.buttonZoomOut.TabIndex = 60;
//            this.buttonZoomOut.UseVisualStyleBackColor = true;
//            this.buttonZoomOut.Click += new System.EventHandler(this.buttonZoomOut_Click);
//            // 
//            // labelCamView
//            // 
//            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(240)))));
//            appearance1.FontData.BoldAsString = "True";
//            appearance1.FontData.Name = "Malgun Gothic";
//            appearance1.FontData.SizeInPoints = 24F;
//            appearance1.ForeColor = System.Drawing.Color.Black;
//            appearance1.TextHAlignAsString = "Left";
//            appearance1.TextVAlignAsString = "Middle";
//            this.labelCamView.Appearance = appearance1;
//            this.labelCamView.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
//            this.labelCamView.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.labelCamView.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.labelCamView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
//            this.labelCamView.Location = new System.Drawing.Point(0, 0);
//            this.labelCamView.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
//            this.labelCamView.Name = "labelCamView";
//            this.labelCamView.Size = new System.Drawing.Size(1030, 49);
//            this.labelCamView.TabIndex = 34;
//            this.labelCamView.Text = "  Image View";
//            // 
//            // splitContainer1
//            // 
//            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
//            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
//            this.splitContainer1.Name = "splitContainer1";
//            // 
//            // splitContainer1.Panel1
//            // 
//            this.splitContainer1.Panel1.Controls.Add(this.camViewPanel);
//            // 
//            // splitContainer1.Panel2
//            // 
//            this.splitContainer1.Panel2.Controls.Add(this.lastDefectView);
//            this.splitContainer1.Size = new System.Drawing.Size(1030, 632);
//            this.splitContainer1.SplitterDistance = 471;
//            this.splitContainer1.TabIndex = 53;
//            // 
//            // camViewPanel
//            // 
//            this.camViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.camViewPanel.Location = new System.Drawing.Point(0, 0);
//            this.camViewPanel.Name = "camViewPanel";
//            this.camViewPanel.Size = new System.Drawing.Size(471, 632);
//            this.camViewPanel.TabIndex = 54;
//            // 
//            // lastDefectView
//            // 
//            this.lastDefectView.AllowUserToAddRows = false;
//            this.lastDefectView.AllowUserToDeleteRows = false;
//            this.lastDefectView.AllowUserToResizeColumns = false;
//            this.lastDefectView.AllowUserToResizeRows = false;
//            this.lastDefectView.BackgroundColor = System.Drawing.SystemColors.Control;
//            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
//            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
//            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
//            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
//            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
//            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
//            this.lastDefectView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
//            this.lastDefectView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            this.lastDefectView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
//            this.columnType,
//            this.columnInfo,
//            this.columnImage});
//            this.lastDefectView.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.lastDefectView.Location = new System.Drawing.Point(0, 0);
//            this.lastDefectView.Margin = new System.Windows.Forms.Padding(2);
//            this.lastDefectView.MultiSelect = false;
//            this.lastDefectView.Name = "lastDefectView";
//            this.lastDefectView.ReadOnly = true;
//            this.lastDefectView.RowHeadersVisible = false;
//            this.lastDefectView.RowTemplate.Height = 23;
//            this.lastDefectView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
//            this.lastDefectView.Size = new System.Drawing.Size(555, 632);
//            this.lastDefectView.TabIndex = 48;
//            this.lastDefectView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lastDefectView_CellClick);
//            this.lastDefectView.Paint += new System.Windows.Forms.PaintEventHandler(this.lastDefectView_Paint);
//            // 
//            // columnType
//            // 
//            this.columnType.HeaderText = "Type";
//            this.columnType.Name = "columnType";
//            this.columnType.ReadOnly = true;
//            this.columnType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
//            // 
//            // columnInfo
//            // 
//            this.columnInfo.HeaderText = "Info";
//            this.columnInfo.Name = "columnInfo";
//            this.columnInfo.ReadOnly = true;
//            this.columnInfo.Width = 220;
//            // 
//            // columnImage
//            // 
//            this.columnImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
//            this.columnImage.HeaderText = "Image";
//            this.columnImage.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
//            this.columnImage.Name = "columnImage";
//            this.columnImage.ReadOnly = true;
//            // 
//            // total
//            // 
//            this.total.AutoSize = true;
//            this.total.Font = new System.Drawing.Font("굴림", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.total.Location = new System.Drawing.Point(562, 9);
//            this.total.Name = "total";
//            this.total.Size = new System.Drawing.Size(162, 30);
//            this.total.TabIndex = 65;
//            this.total.Text = "Total: 100";
//            // 
//            // index
//            // 
//            this.index.AutoSize = true;
//            this.index.Font = new System.Drawing.Font("굴림", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.index.Location = new System.Drawing.Point(307, 9);
//            this.index.Name = "index";
//            this.index.Size = new System.Drawing.Size(216, 30);
//            this.index.TabIndex = 64;
//            this.index.Text = "Index : 20000";
//            // 
//            // ImageViewer
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(1030, 681);
//            this.Controls.Add(this.splitContainer1);
//            this.Controls.Add(this.panelTop);
//            this.Name = "ImageViewer";
//            this.Text = "ImageViewer";
//            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageViewer_FormClosing);
//            this.Load += new System.EventHandler(this.ImageViewer_Load);
//            this.panelTop.ResumeLayout(false);
//            this.panelTop.PerformLayout();
//            this.splitContainer1.Panel1.ResumeLayout(false);
//            this.splitContainer1.Panel2.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
//            this.splitContainer1.ResumeLayout(false);
//            this.camViewPanel.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.lastDefectView)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private System.Windows.Forms.Panel panelTop;
//        private System.Windows.Forms.Button buttonZoomFit;
//        private System.Windows.Forms.Button buttonZoomIn;
//        private System.Windows.Forms.Button buttonZoomOut;
//        private Infragistics.Win.Misc.UltraLabel labelCamView;
//        private System.Windows.Forms.SplitContainer splitContainer1;
//        private Infragistics.Win.Misc.UltraPanel camViewPanel;
//        private System.Windows.Forms.DataGridView lastDefectView;
//        private System.Windows.Forms.DataGridViewTextBoxColumn columnType;
//        private System.Windows.Forms.DataGridViewTextBoxColumn columnInfo;
//        private System.Windows.Forms.DataGridViewImageColumn columnImage;
//        private System.Windows.Forms.Label total;
//        private System.Windows.Forms.Label index;
//    }
//}