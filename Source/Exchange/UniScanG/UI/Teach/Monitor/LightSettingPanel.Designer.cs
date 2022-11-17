namespace UniScanG.UI.Teach.Monitor
{
    partial class LightSettingPanel
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
            this.numericLightTopLeft = new System.Windows.Forms.NumericUpDown();
            this.trackBarLightTopLeft = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelLightTop = new System.Windows.Forms.Label();
            this.labelLightBottom = new System.Windows.Forms.Label();
            this.trackBarLightTopMiddle = new System.Windows.Forms.TrackBar();
            this.trackBarLightTopRight = new System.Windows.Forms.TrackBar();
            this.trackBarLightTopBottom = new System.Windows.Forms.TrackBar();
            this.labelLightTopLeft = new System.Windows.Forms.Label();
            this.labelLightTopMiddle = new System.Windows.Forms.Label();
            this.labelLightTopRight = new System.Windows.Forms.Label();
            this.lightTopLeft = new System.Windows.Forms.Label();
            this.lightTopMiddle = new System.Windows.Forms.Label();
            this.lightTopRight = new System.Windows.Forms.Label();
            this.lightBottom = new System.Windows.Forms.Label();
            this.buttonOff = new System.Windows.Forms.Button();
            this.buttonOn = new System.Windows.Forms.Button();
            this.numericLightTopMiddle = new System.Windows.Forms.NumericUpDown();
            this.numericLightTopRight = new System.Windows.Forms.NumericUpDown();
            this.numericLightTopBottom = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopLeft)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopMiddle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopMiddle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopBottom)).BeginInit();
            this.SuspendLayout();
            // 
            // numericLightTopLeft
            // 
            this.numericLightTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericLightTopLeft.Location = new System.Drawing.Point(461, 6);
            this.numericLightTopLeft.Margin = new System.Windows.Forms.Padding(5);
            this.numericLightTopLeft.Name = "numericLightTopLeft";
            this.numericLightTopLeft.Size = new System.Drawing.Size(50, 29);
            this.numericLightTopLeft.TabIndex = 0;
            this.numericLightTopLeft.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // trackBarLightTopLeft
            // 
            this.trackBarLightTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarLightTopLeft.Location = new System.Drawing.Point(176, 6);
            this.trackBarLightTopLeft.Margin = new System.Windows.Forms.Padding(5);
            this.trackBarLightTopLeft.Name = "trackBarLightTopLeft";
            this.trackBarLightTopLeft.Size = new System.Drawing.Size(274, 30);
            this.trackBarLightTopLeft.TabIndex = 1;
            this.trackBarLightTopLeft.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel1.Controls.Add(this.trackBarLightTopLeft, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelLightTop, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelLightBottom, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.trackBarLightTopMiddle, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.trackBarLightTopRight, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.trackBarLightTopBottom, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelLightTopLeft, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelLightTopMiddle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelLightTopRight, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lightTopLeft, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lightTopMiddle, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lightTopRight, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lightBottom, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.numericLightTopLeft, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericLightTopMiddle, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericLightTopRight, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.numericLightTopBottom, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonOn, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonOff, 5, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(579, 160);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // labelLightTop
            // 
            this.labelLightTop.AutoSize = true;
            this.labelLightTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightTop.Location = new System.Drawing.Point(1, 1);
            this.labelLightTop.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightTop.Name = "labelLightTop";
            this.tableLayoutPanel1.SetRowSpan(this.labelLightTop, 3);
            this.labelLightTop.Size = new System.Drawing.Size(70, 122);
            this.labelLightTop.TabIndex = 4;
            this.labelLightTop.Text = "Top";
            this.labelLightTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLightBottom
            // 
            this.labelLightBottom.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelLightBottom, 2);
            this.labelLightBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightBottom.Location = new System.Drawing.Point(1, 124);
            this.labelLightBottom.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightBottom.Name = "labelLightBottom";
            this.labelLightBottom.Size = new System.Drawing.Size(121, 40);
            this.labelLightBottom.TabIndex = 4;
            this.labelLightBottom.Text = "Bottom";
            this.labelLightBottom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarLightTopMiddle
            // 
            this.trackBarLightTopMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarLightTopMiddle.Location = new System.Drawing.Point(176, 47);
            this.trackBarLightTopMiddle.Margin = new System.Windows.Forms.Padding(5);
            this.trackBarLightTopMiddle.Name = "trackBarLightTopMiddle";
            this.trackBarLightTopMiddle.Size = new System.Drawing.Size(274, 30);
            this.trackBarLightTopMiddle.TabIndex = 1;
            this.trackBarLightTopMiddle.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // trackBarLightTopRight
            // 
            this.trackBarLightTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarLightTopRight.Location = new System.Drawing.Point(176, 88);
            this.trackBarLightTopRight.Margin = new System.Windows.Forms.Padding(5);
            this.trackBarLightTopRight.Name = "trackBarLightTopRight";
            this.trackBarLightTopRight.Size = new System.Drawing.Size(274, 30);
            this.trackBarLightTopRight.TabIndex = 1;
            this.trackBarLightTopRight.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // trackBarLightTopBottom
            // 
            this.trackBarLightTopBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarLightTopBottom.Location = new System.Drawing.Point(176, 129);
            this.trackBarLightTopBottom.Margin = new System.Windows.Forms.Padding(5);
            this.trackBarLightTopBottom.Name = "trackBarLightTopBottom";
            this.trackBarLightTopBottom.Size = new System.Drawing.Size(274, 30);
            this.trackBarLightTopBottom.TabIndex = 1;
            this.trackBarLightTopBottom.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // labelLightTopLeft
            // 
            this.labelLightTopLeft.AutoSize = true;
            this.labelLightTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightTopLeft.Location = new System.Drawing.Point(72, 1);
            this.labelLightTopLeft.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightTopLeft.Name = "labelLightTopLeft";
            this.labelLightTopLeft.Size = new System.Drawing.Size(50, 40);
            this.labelLightTopLeft.TabIndex = 4;
            this.labelLightTopLeft.Text = "L";
            this.labelLightTopLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLightTopMiddle
            // 
            this.labelLightTopMiddle.AutoSize = true;
            this.labelLightTopMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightTopMiddle.Location = new System.Drawing.Point(72, 42);
            this.labelLightTopMiddle.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightTopMiddle.Name = "labelLightTopMiddle";
            this.labelLightTopMiddle.Size = new System.Drawing.Size(50, 40);
            this.labelLightTopMiddle.TabIndex = 4;
            this.labelLightTopMiddle.Text = "M";
            this.labelLightTopMiddle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLightTopRight
            // 
            this.labelLightTopRight.AutoSize = true;
            this.labelLightTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightTopRight.Location = new System.Drawing.Point(72, 83);
            this.labelLightTopRight.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightTopRight.Name = "labelLightTopRight";
            this.labelLightTopRight.Size = new System.Drawing.Size(50, 40);
            this.labelLightTopRight.TabIndex = 4;
            this.labelLightTopRight.Text = "R";
            this.labelLightTopRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lightTopLeft
            // 
            this.lightTopLeft.AutoSize = true;
            this.lightTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightTopLeft.Location = new System.Drawing.Point(128, 6);
            this.lightTopLeft.Margin = new System.Windows.Forms.Padding(5);
            this.lightTopLeft.Name = "lightTopLeft";
            this.lightTopLeft.Size = new System.Drawing.Size(37, 30);
            this.lightTopLeft.TabIndex = 4;
            this.lightTopLeft.Text = "000";
            this.lightTopLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lightTopMiddle
            // 
            this.lightTopMiddle.AutoSize = true;
            this.lightTopMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightTopMiddle.Location = new System.Drawing.Point(128, 47);
            this.lightTopMiddle.Margin = new System.Windows.Forms.Padding(5);
            this.lightTopMiddle.Name = "lightTopMiddle";
            this.lightTopMiddle.Size = new System.Drawing.Size(37, 30);
            this.lightTopMiddle.TabIndex = 4;
            this.lightTopMiddle.Text = "000";
            this.lightTopMiddle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lightTopRight
            // 
            this.lightTopRight.AutoSize = true;
            this.lightTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightTopRight.Location = new System.Drawing.Point(128, 88);
            this.lightTopRight.Margin = new System.Windows.Forms.Padding(5);
            this.lightTopRight.Name = "lightTopRight";
            this.lightTopRight.Size = new System.Drawing.Size(37, 30);
            this.lightTopRight.TabIndex = 4;
            this.lightTopRight.Text = "000";
            this.lightTopRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lightBottom
            // 
            this.lightBottom.AutoSize = true;
            this.lightBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightBottom.Location = new System.Drawing.Point(128, 129);
            this.lightBottom.Margin = new System.Windows.Forms.Padding(5);
            this.lightBottom.Name = "lightBottom";
            this.lightBottom.Size = new System.Drawing.Size(37, 30);
            this.lightBottom.TabIndex = 4;
            this.lightBottom.Text = "000";
            this.lightBottom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonOff
            // 
            this.buttonOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOff.Location = new System.Drawing.Point(520, 127);
            this.buttonOff.Name = "buttonOff";
            this.buttonOff.Size = new System.Drawing.Size(55, 34);
            this.buttonOff.TabIndex = 3;
            this.buttonOff.Text = "OFF";
            this.buttonOff.UseVisualStyleBackColor = true;
            this.buttonOff.Click += new System.EventHandler(this.buttonOff_Click);
            // 
            // buttonOn
            // 
            this.buttonOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOn.Location = new System.Drawing.Point(520, 86);
            this.buttonOn.Name = "buttonOn";
            this.buttonOn.Size = new System.Drawing.Size(55, 34);
            this.buttonOn.TabIndex = 2;
            this.buttonOn.Text = "ON";
            this.buttonOn.UseVisualStyleBackColor = true;
            this.buttonOn.Click += new System.EventHandler(this.buttonOn_Click);
            // 
            // numericLightTopMiddle
            // 
            this.numericLightTopMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericLightTopMiddle.Location = new System.Drawing.Point(461, 47);
            this.numericLightTopMiddle.Margin = new System.Windows.Forms.Padding(5);
            this.numericLightTopMiddle.Name = "numericLightTopMiddle";
            this.numericLightTopMiddle.Size = new System.Drawing.Size(50, 29);
            this.numericLightTopMiddle.TabIndex = 0;
            this.numericLightTopMiddle.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // numericLightTopRight
            // 
            this.numericLightTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericLightTopRight.Location = new System.Drawing.Point(461, 88);
            this.numericLightTopRight.Margin = new System.Windows.Forms.Padding(5);
            this.numericLightTopRight.Name = "numericLightTopRight";
            this.numericLightTopRight.Size = new System.Drawing.Size(50, 29);
            this.numericLightTopRight.TabIndex = 0;
            this.numericLightTopRight.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // numericLightTopBottom
            // 
            this.numericLightTopBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericLightTopBottom.Location = new System.Drawing.Point(461, 129);
            this.numericLightTopBottom.Margin = new System.Windows.Forms.Padding(5);
            this.numericLightTopBottom.Name = "numericLightTopBottom";
            this.numericLightTopBottom.Size = new System.Drawing.Size(50, 29);
            this.numericLightTopBottom.TabIndex = 0;
            this.numericLightTopBottom.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // LightSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LightSettingPanel";
            this.Size = new System.Drawing.Size(579, 160);
            this.Load += new System.EventHandler(this.LightSettingPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopLeft)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopMiddle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopMiddle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopBottom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericLightTopLeft;
        private System.Windows.Forms.TrackBar trackBarLightTopLeft;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelLightTop;
        private System.Windows.Forms.Label labelLightBottom;
        private System.Windows.Forms.TrackBar trackBarLightTopMiddle;
        private System.Windows.Forms.TrackBar trackBarLightTopRight;
        private System.Windows.Forms.TrackBar trackBarLightTopBottom;
        private System.Windows.Forms.NumericUpDown numericLightTopMiddle;
        private System.Windows.Forms.NumericUpDown numericLightTopRight;
        private System.Windows.Forms.NumericUpDown numericLightTopBottom;
        private System.Windows.Forms.Button buttonOn;
        private System.Windows.Forms.Button buttonOff;
        private System.Windows.Forms.Label labelLightTopLeft;
        private System.Windows.Forms.Label labelLightTopMiddle;
        private System.Windows.Forms.Label labelLightTopRight;
        private System.Windows.Forms.Label lightTopLeft;
        private System.Windows.Forms.Label lightTopMiddle;
        private System.Windows.Forms.Label lightTopRight;
        private System.Windows.Forms.Label lightBottom;
    }
}
