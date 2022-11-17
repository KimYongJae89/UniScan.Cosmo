namespace SheetFinderTest
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.columnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnParents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelFrameHeight = new System.Windows.Forms.Label();
            this.labelBoundarySize = new System.Windows.Forms.Label();
            this.frameHeight = new System.Windows.Forms.NumericUpDown();
            this.boundarySize = new System.Windows.Forms.NumericUpDown();
            this.labelLengthMul = new System.Windows.Forms.Label();
            this.lengthMul = new System.Windows.Forms.NumericUpDown();
            this.labelThresMul = new System.Windows.Forms.Label();
            this.thresMul = new System.Windows.Forms.NumericUpDown();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boundarySize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lengthMul)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresMul)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1057, 548);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(425, 3);
            this.panel3.Name = "panel3";
            this.tableLayoutPanel1.SetRowSpan(this.panel3, 2);
            this.panel3.Size = new System.Drawing.Size(416, 542);
            this.panel3.TabIndex = 7;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIndex,
            this.columnHeight,
            this.columnParents});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(847, 194);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(207, 351);
            this.dataGridView.TabIndex = 6;
            // 
            // columnIndex
            // 
            this.columnIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnIndex.HeaderText = "Index";
            this.columnIndex.Name = "columnIndex";
            this.columnIndex.ReadOnly = true;
            this.columnIndex.Width = 61;
            // 
            // columnHeight
            // 
            this.columnHeight.HeaderText = "Height";
            this.columnHeight.Name = "columnHeight";
            this.columnHeight.ReadOnly = true;
            this.columnHeight.Width = 143;
            // 
            // columnParents
            // 
            this.columnParents.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnParents.HeaderText = "Parents";
            this.columnParents.Name = "columnParents";
            this.columnParents.ReadOnly = true;
            this.columnParents.Width = 73;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelFrameHeight);
            this.panel1.Controls.Add(this.labelBoundarySize);
            this.panel1.Controls.Add(this.frameHeight);
            this.panel1.Controls.Add(this.boundarySize);
            this.panel1.Controls.Add(this.labelLengthMul);
            this.panel1.Controls.Add(this.lengthMul);
            this.panel1.Controls.Add(this.labelThresMul);
            this.panel1.Controls.Add(this.thresMul);
            this.panel1.Controls.Add(this.buttonLoad);
            this.panel1.Controls.Add(this.buttonStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(847, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(207, 185);
            this.panel1.TabIndex = 0;
            // 
            // labelFrameHeight
            // 
            this.labelFrameHeight.AutoSize = true;
            this.labelFrameHeight.Location = new System.Drawing.Point(21, 67);
            this.labelFrameHeight.Name = "labelFrameHeight";
            this.labelFrameHeight.Size = new System.Drawing.Size(80, 12);
            this.labelFrameHeight.TabIndex = 5;
            this.labelFrameHeight.Text = "Frame Height";
            // 
            // labelBoundarySize
            // 
            this.labelBoundarySize.AutoSize = true;
            this.labelBoundarySize.Location = new System.Drawing.Point(21, 93);
            this.labelBoundarySize.Name = "labelBoundarySize";
            this.labelBoundarySize.Size = new System.Drawing.Size(82, 12);
            this.labelBoundarySize.TabIndex = 5;
            this.labelBoundarySize.Text = "Gap Size Half";
            // 
            // frameHeight
            // 
            this.frameHeight.Location = new System.Drawing.Point(117, 63);
            this.frameHeight.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.frameHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.frameHeight.Name = "frameHeight";
            this.frameHeight.Size = new System.Drawing.Size(75, 21);
            this.frameHeight.TabIndex = 4;
            this.frameHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.frameHeight.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // boundarySize
            // 
            this.boundarySize.Location = new System.Drawing.Point(117, 89);
            this.boundarySize.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.boundarySize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.boundarySize.Name = "boundarySize";
            this.boundarySize.Size = new System.Drawing.Size(75, 21);
            this.boundarySize.TabIndex = 4;
            this.boundarySize.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.boundarySize.ValueChanged += new System.EventHandler(this.numericUpDown4_ValueChanged);
            // 
            // labelLengthMul
            // 
            this.labelLengthMul.AutoSize = true;
            this.labelLengthMul.Location = new System.Drawing.Point(9, 161);
            this.labelLengthMul.Name = "labelLengthMul";
            this.labelLengthMul.Size = new System.Drawing.Size(95, 12);
            this.labelLengthMul.TabIndex = 3;
            this.labelLengthMul.Text = "Gap Length Mul";
            // 
            // lengthMul
            // 
            this.lengthMul.DecimalPlaces = 1;
            this.lengthMul.Location = new System.Drawing.Point(117, 157);
            this.lengthMul.Name = "lengthMul";
            this.lengthMul.Size = new System.Drawing.Size(75, 21);
            this.lengthMul.TabIndex = 2;
            this.lengthMul.Value = new decimal(new int[] {
            60,
            0,
            0,
            65536});
            // 
            // labelThresMul
            // 
            this.labelThresMul.AutoSize = true;
            this.labelThresMul.Location = new System.Drawing.Point(44, 134);
            this.labelThresMul.Name = "labelThresMul";
            this.labelThresMul.Size = new System.Drawing.Size(63, 12);
            this.labelThresMul.TabIndex = 3;
            this.labelThresMul.Text = "Thres Mul";
            // 
            // thresMul
            // 
            this.thresMul.DecimalPlaces = 1;
            this.thresMul.Location = new System.Drawing.Point(117, 130);
            this.thresMul.Name = "thresMul";
            this.thresMul.Size = new System.Drawing.Size(75, 21);
            this.thresMul.TabIndex = 2;
            this.thresMul.Value = new decimal(new int[] {
            13,
            0,
            0,
            65536});
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(19, 9);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 48);
            this.buttonLoad.TabIndex = 0;
            this.buttonLoad.Text = "LoadImg";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(117, 9);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 48);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start/Stop";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.tableLayoutPanel1.SetRowSpan(this.panel2, 2);
            this.panel2.Size = new System.Drawing.Size(416, 542);
            this.panel2.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 548);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boundarySize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lengthMul)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresMul)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelLengthMul;
        private System.Windows.Forms.NumericUpDown lengthMul;
        private System.Windows.Forms.Label labelThresMul;
        private System.Windows.Forms.NumericUpDown thresMul;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Label labelBoundarySize;
        private System.Windows.Forms.NumericUpDown boundarySize;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnParents;
        private System.Windows.Forms.Label labelFrameHeight;
        private System.Windows.Forms.NumericUpDown frameHeight;
    }
}

