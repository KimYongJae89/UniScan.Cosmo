namespace UniScanG.Gravure.UI.Teach.Inspector
{
    partial class DefectLegendPanel
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.layoutDefectColor = new System.Windows.Forms.TableLayoutPanel();
            this.labelSheetAttack = new Infragistics.Win.Misc.UltraLabel();
            this.labelDielectric = new Infragistics.Win.Misc.UltraLabel();
            this.labelPinHole = new Infragistics.Win.Misc.UltraLabel();
            this.labelNotprint = new Infragistics.Win.Misc.UltraLabel();
            this.layoutDefectColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutDefectColor
            // 
            this.layoutDefectColor.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutDefectColor.ColumnCount = 4;
            this.layoutDefectColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutDefectColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutDefectColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutDefectColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutDefectColor.Controls.Add(this.labelSheetAttack, 0, 0);
            this.layoutDefectColor.Controls.Add(this.labelDielectric, 2, 0);
            this.layoutDefectColor.Controls.Add(this.labelPinHole, 3, 0);
            this.layoutDefectColor.Controls.Add(this.labelNotprint, 1, 0);
            this.layoutDefectColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutDefectColor.Location = new System.Drawing.Point(0, 0);
            this.layoutDefectColor.Margin = new System.Windows.Forms.Padding(0);
            this.layoutDefectColor.Name = "layoutDefectColor";
            this.layoutDefectColor.RowCount = 1;
            this.layoutDefectColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutDefectColor.Size = new System.Drawing.Size(537, 39);
            this.layoutDefectColor.TabIndex = 2;
            // 
            // labelSheetAttack
            // 
            appearance1.FontData.Name = "Malgun Gothic";
            appearance1.FontData.SizeInPoints = 12F;
            appearance1.ForeColor = System.Drawing.Color.Maroon;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.labelSheetAttack.Appearance = appearance1;
            this.labelSheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetAttack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelSheetAttack.Location = new System.Drawing.Point(2, 2);
            this.labelSheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetAttack.Name = "labelSheetAttack";
            this.labelSheetAttack.Size = new System.Drawing.Size(131, 35);
            this.labelSheetAttack.TabIndex = 66;
            this.labelSheetAttack.Text = "SheetAttack";
            // 
            // labelDielectric
            // 
            appearance2.FontData.Name = "Malgun Gothic";
            appearance2.FontData.SizeInPoints = 12F;
            appearance2.ForeColor = System.Drawing.Color.Blue;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.labelDielectric.Appearance = appearance2;
            this.labelDielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDielectric.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelDielectric.Location = new System.Drawing.Point(268, 2);
            this.labelDielectric.Margin = new System.Windows.Forms.Padding(0);
            this.labelDielectric.Name = "labelDielectric";
            this.labelDielectric.Size = new System.Drawing.Size(131, 35);
            this.labelDielectric.TabIndex = 62;
            this.labelDielectric.Text = "Dielectric";
            // 
            // labelPinHole
            // 
            appearance3.FontData.Name = "Malgun Gothic";
            appearance3.FontData.SizeInPoints = 12F;
            appearance3.ForeColor = System.Drawing.Color.DarkMagenta;
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.labelPinHole.Appearance = appearance3;
            this.labelPinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPinHole.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelPinHole.Location = new System.Drawing.Point(401, 2);
            this.labelPinHole.Margin = new System.Windows.Forms.Padding(0);
            this.labelPinHole.Name = "labelPinHole";
            this.labelPinHole.Size = new System.Drawing.Size(134, 35);
            this.labelPinHole.TabIndex = 65;
            this.labelPinHole.Text = "Pinhole";
            // 
            // labelNotprint
            // 
            appearance4.FontData.Name = "Malgun Gothic";
            appearance4.FontData.SizeInPoints = 12F;
            appearance4.ForeColor = System.Drawing.Color.Red;
            appearance4.TextHAlignAsString = "Center";
            appearance4.TextVAlignAsString = "Middle";
            this.labelNotprint.Appearance = appearance4;
            this.labelNotprint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNotprint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelNotprint.Location = new System.Drawing.Point(135, 2);
            this.labelNotprint.Margin = new System.Windows.Forms.Padding(0);
            this.labelNotprint.Name = "labelNotprint";
            this.labelNotprint.Size = new System.Drawing.Size(131, 35);
            this.labelNotprint.TabIndex = 67;
            this.labelNotprint.Text = "NoPrint";
            // 
            // DefectLegentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutDefectColor);
            this.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "DefectLegentPanel";
            this.Size = new System.Drawing.Size(537, 39);
            this.layoutDefectColor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutDefectColor;
        private Infragistics.Win.Misc.UltraLabel labelSheetAttack;
        private Infragistics.Win.Misc.UltraLabel labelDielectric;
        private Infragistics.Win.Misc.UltraLabel labelPinHole;
        private Infragistics.Win.Misc.UltraLabel labelNotprint;
    }
}
