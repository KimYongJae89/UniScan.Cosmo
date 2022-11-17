namespace UniScanG.Screen.UI.Teach.Inspector
{
    partial class DefectTypeFilterPanel
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
            this.layoutTotal = new System.Windows.Forms.TableLayoutPanel();
            this.typePoleCircle = new System.Windows.Forms.RadioButton();
            this.typeTotal = new System.Windows.Forms.RadioButton();
            this.typePoleLine = new System.Windows.Forms.RadioButton();
            this.typeShape = new System.Windows.Forms.RadioButton();
            this.typeSheetAttack = new System.Windows.Forms.RadioButton();
            this.typeDielectric = new System.Windows.Forms.RadioButton();
            this.typePinHole = new System.Windows.Forms.RadioButton();
            this.layoutTotal.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutTotal
            // 
            this.layoutTotal.ColumnCount = 5;
            this.layoutTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 172F));
            this.layoutTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 172F));
            this.layoutTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 172F));
            this.layoutTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 172F));
            this.layoutTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTotal.Controls.Add(this.typePoleCircle, 3, 0);
            this.layoutTotal.Controls.Add(this.typeTotal, 0, 0);
            this.layoutTotal.Controls.Add(this.typePoleLine, 2, 0);
            this.layoutTotal.Controls.Add(this.typeShape, 3, 1);
            this.layoutTotal.Controls.Add(this.typeSheetAttack, 1, 0);
            this.layoutTotal.Controls.Add(this.typeDielectric, 1, 1);
            this.layoutTotal.Controls.Add(this.typePinHole, 2, 1);
            this.layoutTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTotal.Location = new System.Drawing.Point(0, 0);
            this.layoutTotal.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTotal.Name = "layoutTotal";
            this.layoutTotal.RowCount = 2;
            this.layoutTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutTotal.Size = new System.Drawing.Size(689, 60);
            this.layoutTotal.TabIndex = 57;
            // 
            // typePoleCircle
            // 
            this.typePoleCircle.Appearance = System.Windows.Forms.Appearance.Button;
            this.typePoleCircle.AutoSize = true;
            this.typePoleCircle.BackColor = System.Drawing.Color.Transparent;
            this.typePoleCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typePoleCircle.FlatAppearance.BorderSize = 0;
            this.typePoleCircle.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.typePoleCircle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.typePoleCircle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.typePoleCircle.ForeColor = System.Drawing.Color.OrangeRed;
            this.typePoleCircle.Location = new System.Drawing.Point(516, 0);
            this.typePoleCircle.Margin = new System.Windows.Forms.Padding(0);
            this.typePoleCircle.Name = "typePoleCircle";
            this.typePoleCircle.Size = new System.Drawing.Size(172, 30);
            this.typePoleCircle.TabIndex = 85;
            this.typePoleCircle.Text = "Pole (Circle)";
            this.typePoleCircle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typePoleCircle.UseVisualStyleBackColor = false;
            this.typePoleCircle.CheckedChanged += new System.EventHandler(this.type_CheckedChanged);
            // 
            // typeTotal
            // 
            this.typeTotal.Appearance = System.Windows.Forms.Appearance.Button;
            this.typeTotal.BackColor = System.Drawing.Color.Transparent;
            this.typeTotal.Checked = true;
            this.typeTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeTotal.FlatAppearance.BorderSize = 0;
            this.typeTotal.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.typeTotal.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.typeTotal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.typeTotal.Location = new System.Drawing.Point(0, 0);
            this.typeTotal.Margin = new System.Windows.Forms.Padding(0);
            this.typeTotal.Name = "typeTotal";
            this.layoutTotal.SetRowSpan(this.typeTotal, 2);
            this.typeTotal.Size = new System.Drawing.Size(172, 60);
            this.typeTotal.TabIndex = 82;
            this.typeTotal.TabStop = true;
            this.typeTotal.Text = "Total";
            this.typeTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typeTotal.UseVisualStyleBackColor = false;
            this.typeTotal.CheckedChanged += new System.EventHandler(this.type_CheckedChanged);
            // 
            // typePoleLine
            // 
            this.typePoleLine.Appearance = System.Windows.Forms.Appearance.Button;
            this.typePoleLine.AutoSize = true;
            this.typePoleLine.BackColor = System.Drawing.Color.Transparent;
            this.typePoleLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typePoleLine.FlatAppearance.BorderSize = 0;
            this.typePoleLine.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.typePoleLine.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.typePoleLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.typePoleLine.ForeColor = System.Drawing.Color.Red;
            this.typePoleLine.Location = new System.Drawing.Point(344, 0);
            this.typePoleLine.Margin = new System.Windows.Forms.Padding(0);
            this.typePoleLine.Name = "typePoleLine";
            this.typePoleLine.Size = new System.Drawing.Size(172, 30);
            this.typePoleLine.TabIndex = 81;
            this.typePoleLine.Text = "Pole (Line)";
            this.typePoleLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typePoleLine.UseVisualStyleBackColor = false;
            this.typePoleLine.CheckedChanged += new System.EventHandler(this.type_CheckedChanged);
            // 
            // typeShape
            // 
            this.typeShape.Appearance = System.Windows.Forms.Appearance.Button;
            this.typeShape.AutoSize = true;
            this.typeShape.BackColor = System.Drawing.Color.Transparent;
            this.typeShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeShape.FlatAppearance.BorderSize = 0;
            this.typeShape.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.typeShape.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.typeShape.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.typeShape.ForeColor = System.Drawing.Color.DarkGreen;
            this.typeShape.Location = new System.Drawing.Point(516, 30);
            this.typeShape.Margin = new System.Windows.Forms.Padding(0);
            this.typeShape.Name = "typeShape";
            this.typeShape.Size = new System.Drawing.Size(172, 30);
            this.typeShape.TabIndex = 84;
            this.typeShape.Text = "Shape";
            this.typeShape.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typeShape.UseVisualStyleBackColor = false;
            this.typeShape.CheckedChanged += new System.EventHandler(this.type_CheckedChanged);
            // 
            // typeSheetAttack
            // 
            this.typeSheetAttack.Appearance = System.Windows.Forms.Appearance.Button;
            this.typeSheetAttack.AutoSize = true;
            this.typeSheetAttack.BackColor = System.Drawing.Color.Transparent;
            this.typeSheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeSheetAttack.FlatAppearance.BorderSize = 0;
            this.typeSheetAttack.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.typeSheetAttack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.typeSheetAttack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.typeSheetAttack.ForeColor = System.Drawing.Color.Maroon;
            this.typeSheetAttack.Location = new System.Drawing.Point(172, 0);
            this.typeSheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.typeSheetAttack.Name = "typeSheetAttack";
            this.typeSheetAttack.Size = new System.Drawing.Size(172, 30);
            this.typeSheetAttack.TabIndex = 86;
            this.typeSheetAttack.Text = "SheetAttack";
            this.typeSheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typeSheetAttack.UseVisualStyleBackColor = false;
            this.typeSheetAttack.CheckedChanged += new System.EventHandler(this.type_CheckedChanged);
            // 
            // typeDielectric
            // 
            this.typeDielectric.Appearance = System.Windows.Forms.Appearance.Button;
            this.typeDielectric.AutoSize = true;
            this.typeDielectric.BackColor = System.Drawing.Color.Transparent;
            this.typeDielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeDielectric.FlatAppearance.BorderSize = 0;
            this.typeDielectric.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.typeDielectric.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.typeDielectric.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.typeDielectric.ForeColor = System.Drawing.Color.Blue;
            this.typeDielectric.Location = new System.Drawing.Point(172, 30);
            this.typeDielectric.Margin = new System.Windows.Forms.Padding(0);
            this.typeDielectric.Name = "typeDielectric";
            this.typeDielectric.Size = new System.Drawing.Size(172, 30);
            this.typeDielectric.TabIndex = 0;
            this.typeDielectric.Text = "Dielectric";
            this.typeDielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typeDielectric.UseVisualStyleBackColor = false;
            this.typeDielectric.CheckedChanged += new System.EventHandler(this.type_CheckedChanged);
            // 
            // typePinHole
            // 
            this.typePinHole.Appearance = System.Windows.Forms.Appearance.Button;
            this.typePinHole.AutoSize = true;
            this.typePinHole.BackColor = System.Drawing.Color.Transparent;
            this.typePinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typePinHole.FlatAppearance.BorderSize = 0;
            this.typePinHole.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.typePinHole.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.typePinHole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.typePinHole.ForeColor = System.Drawing.Color.DarkMagenta;
            this.typePinHole.Location = new System.Drawing.Point(344, 30);
            this.typePinHole.Margin = new System.Windows.Forms.Padding(0);
            this.typePinHole.Name = "typePinHole";
            this.typePinHole.Size = new System.Drawing.Size(172, 30);
            this.typePinHole.TabIndex = 83;
            this.typePinHole.Text = "Pinhole";
            this.typePinHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typePinHole.UseVisualStyleBackColor = false;
            this.typePinHole.CheckedChanged += new System.EventHandler(this.type_CheckedChanged);
            // 
            // DefectTypeFilterPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutTotal);
            this.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "DefectTypeFilterPanel";
            this.Size = new System.Drawing.Size(689, 60);
            this.layoutTotal.ResumeLayout(false);
            this.layoutTotal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutTotal;
        private System.Windows.Forms.RadioButton typePoleCircle;
        private System.Windows.Forms.RadioButton typeTotal;
        private System.Windows.Forms.RadioButton typePoleLine;
        private System.Windows.Forms.RadioButton typeShape;
        private System.Windows.Forms.RadioButton typeSheetAttack;
        private System.Windows.Forms.RadioButton typeDielectric;
        private System.Windows.Forms.RadioButton typePinHole;
    }
}
