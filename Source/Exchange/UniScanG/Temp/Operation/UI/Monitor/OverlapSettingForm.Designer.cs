namespace UniScanG.Temp
{
    partial class OverlapSettingForm
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
            this.panelControl = new System.Windows.Forms.Panel();
            this.panelImageLeft = new System.Windows.Forms.Panel();
            this.panelImageRight = new System.Windows.Forms.Panel();
            this.panelImageOverlap = new System.Windows.Forms.Panel();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.buttonSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panelImageOverlap, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelImageRight, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelImageLeft, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(616, 522);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelControl
            // 
            this.panelControl.Controls.Add(this.buttonSave);
            this.panelControl.Controls.Add(this.numericUpDown1);
            this.panelControl.Controls.Add(this.label1);
            this.panelControl.Controls.Add(this.hScrollBar1);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl.Location = new System.Drawing.Point(616, 0);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(240, 522);
            this.panelControl.TabIndex = 1;
            // 
            // panelImageLeft
            // 
            this.panelImageLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageLeft.Location = new System.Drawing.Point(3, 3);
            this.panelImageLeft.Name = "panelImageLeft";
            this.panelImageLeft.Size = new System.Drawing.Size(302, 255);
            this.panelImageLeft.TabIndex = 2;
            // 
            // panelImageRight
            // 
            this.panelImageRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageRight.Location = new System.Drawing.Point(311, 3);
            this.panelImageRight.Name = "panelImageRight";
            this.panelImageRight.Size = new System.Drawing.Size(302, 255);
            this.panelImageRight.TabIndex = 2;
            // 
            // panelImageOverlap
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panelImageOverlap, 2);
            this.panelImageOverlap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageOverlap.Location = new System.Drawing.Point(3, 264);
            this.panelImageOverlap.Name = "panelImageOverlap";
            this.panelImageOverlap.Size = new System.Drawing.Size(610, 255);
            this.panelImageOverlap.TabIndex = 2;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(11, 80);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(216, 25);
            this.hScrollBar1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Overlap Area";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(143, 56);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(84, 21);
            this.numericUpDown1.TabIndex = 2;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(108, 467);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(120, 43);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // OverlapSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 522);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panelControl);
            this.Name = "OverlapSettingForm";
            this.Text = "Overlap Setting";
            this.Load += new System.EventHandler(this.OverlapSettingForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelImageOverlap;
        private System.Windows.Forms.Panel panelImageRight;
        private System.Windows.Forms.Panel panelImageLeft;
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Button buttonSave;
    }
}