namespace UniScanG.Gravure.UI.Teach.Inspector
{
    partial class RegionEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveImage = new System.Windows.Forms.Button();
            this.showPatternImage = new System.Windows.Forms.RadioButton();
            this.showTrainImage = new System.Windows.Forms.RadioButton();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonApplyAll = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.labelLinePair = new System.Windows.Forms.Label();
            this.panelImage = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.loadImage = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.oddEvenPair = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveImage
            // 
            this.saveImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveImage.Location = new System.Drawing.Point(477, 0);
            this.saveImage.Margin = new System.Windows.Forms.Padding(0);
            this.saveImage.Name = "saveImage";
            this.saveImage.Size = new System.Drawing.Size(100, 32);
            this.saveImage.TabIndex = 6;
            this.saveImage.Text = "Save";
            this.saveImage.UseVisualStyleBackColor = true;
            this.saveImage.Click += new System.EventHandler(this.saveImage_Click);
            // 
            // showPatternImage
            // 
            this.showPatternImage.AutoSize = true;
            this.showPatternImage.Location = new System.Drawing.Point(127, 2);
            this.showPatternImage.Margin = new System.Windows.Forms.Padding(4);
            this.showPatternImage.Name = "showPatternImage";
            this.showPatternImage.Size = new System.Drawing.Size(93, 29);
            this.showPatternImage.TabIndex = 4;
            this.showPatternImage.Text = "Pattern";
            this.showPatternImage.UseVisualStyleBackColor = true;
            this.showPatternImage.CheckedChanged += new System.EventHandler(this.Image_CheckedChanged);
            // 
            // showTrainImage
            // 
            this.showTrainImage.AutoSize = true;
            this.showTrainImage.Checked = true;
            this.showTrainImage.Location = new System.Drawing.Point(26, 2);
            this.showTrainImage.Margin = new System.Windows.Forms.Padding(4);
            this.showTrainImage.Name = "showTrainImage";
            this.showTrainImage.Size = new System.Drawing.Size(73, 29);
            this.showTrainImage.TabIndex = 3;
            this.showTrainImage.TabStop = true;
            this.showTrainImage.Text = "Train";
            this.showTrainImage.UseVisualStyleBackColor = true;
            this.showTrainImage.CheckedChanged += new System.EventHandler(this.Image_CheckedChanged);
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.Location = new System.Drawing.Point(7, 13);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(4);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(89, 43);
            this.buttonReset.TabIndex = 2;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(104, 13);
            this.buttonApply.Margin = new System.Windows.Forms.Padding(4);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(89, 43);
            this.buttonApply.TabIndex = 2;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonApplyAll
            // 
            this.buttonApplyAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApplyAll.Location = new System.Drawing.Point(200, 13);
            this.buttonApplyAll.Margin = new System.Windows.Forms.Padding(4);
            this.buttonApplyAll.Name = "buttonApplyAll";
            this.buttonApplyAll.Size = new System.Drawing.Size(111, 43);
            this.buttonApplyAll.TabIndex = 2;
            this.buttonApplyAll.Text = "Apply All";
            this.buttonApplyAll.UseVisualStyleBackColor = true;
            this.buttonApplyAll.Click += new System.EventHandler(this.buttonApplyAll_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonClose.Location = new System.Drawing.Point(319, 13);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(89, 43);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDown1.Location = new System.Drawing.Point(148, 0);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(0);
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(71, 32);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // labelLinePair
            // 
            this.labelLinePair.AutoSize = true;
            this.labelLinePair.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLinePair.Location = new System.Drawing.Point(0, 0);
            this.labelLinePair.Margin = new System.Windows.Forms.Padding(0);
            this.labelLinePair.Name = "labelLinePair";
            this.labelLinePair.Size = new System.Drawing.Size(148, 32);
            this.labelLinePair.TabIndex = 0;
            this.labelLinePair.Text = "Line Pair";
            this.labelLinePair.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelImage
            // 
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(0, 64);
            this.panelImage.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(1406, 572);
            this.panelImage.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(0, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 32);
            this.label1.TabIndex = 7;
            this.label1.Text = "Odd/Even Pair";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 148F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 677F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 412F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelLinePair, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDown1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.oddEvenPair, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1406, 64);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.loadImage, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.saveImage, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(219, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(677, 32);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 32);
            this.label2.TabIndex = 9;
            this.label2.Text = "Image";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // loadImage
            // 
            this.loadImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadImage.Location = new System.Drawing.Point(577, 0);
            this.loadImage.Margin = new System.Windows.Forms.Padding(0);
            this.loadImage.Name = "loadImage";
            this.loadImage.Size = new System.Drawing.Size(100, 32);
            this.loadImage.TabIndex = 6;
            this.loadImage.Text = "Load";
            this.loadImage.UseVisualStyleBackColor = true;
            this.loadImage.Click += new System.EventHandler(this.loadImage_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.showTrainImage);
            this.panel1.Controls.Add(this.showPatternImage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(77, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 32);
            this.panel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonReset);
            this.panel2.Controls.Add(this.buttonClose);
            this.panel2.Controls.Add(this.buttonApply);
            this.panel2.Controls.Add(this.buttonApplyAll);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(994, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.tableLayoutPanel1.SetRowSpan(this.panel2, 2);
            this.panel2.Size = new System.Drawing.Size(412, 64);
            this.panel2.TabIndex = 9;
            // 
            // oddEvenPair
            // 
            this.oddEvenPair.AutoSize = true;
            this.oddEvenPair.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.oddEvenPair.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oddEvenPair.Location = new System.Drawing.Point(148, 32);
            this.oddEvenPair.Margin = new System.Windows.Forms.Padding(0);
            this.oddEvenPair.Name = "oddEvenPair";
            this.oddEvenPair.Size = new System.Drawing.Size(71, 32);
            this.oddEvenPair.TabIndex = 10;
            this.oddEvenPair.UseVisualStyleBackColor = true;
            this.oddEvenPair.CheckedChanged += new System.EventHandler(this.oddEvenPair_CheckedChanged);
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(219, 32);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(677, 32);
            this.panel3.TabIndex = 11;
            // 
            // RegionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelImage);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Name = "RegionEditor";
            this.Size = new System.Drawing.Size(1406, 636);
            this.Load += new System.EventHandler(this.RegionEditor_Load);
            this.SizeChanged += new System.EventHandler(this.RegionEditor_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label labelLinePair;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonApplyAll;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button saveImage;
        private System.Windows.Forms.RadioButton showPatternImage;
        private System.Windows.Forms.RadioButton showTrainImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox oddEvenPair;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button loadImage;
    }
}
