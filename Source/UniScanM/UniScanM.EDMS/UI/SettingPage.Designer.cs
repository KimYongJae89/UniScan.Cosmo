namespace UniScanM.EDMS.UI
{
    partial class SettingPage
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
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonOK = new Infragistics.Win.Misc.UltraButton();
            this.layoutButton = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCamera = new System.Windows.Forms.Button();
            this.layoutButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(945, 434);
            this.propertyGrid.TabIndex = 10;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            this.propertyGrid.Click += new System.EventHandler(this.propertyGrid_Click);
            // 
            // buttonOK
            // 
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "맑은 고딕";
            appearance1.FontData.SizeInPoints = 12F;
            this.buttonOK.Appearance = appearance1;
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Location = new System.Drawing.Point(400, 3);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(144, 36);
            this.buttonOK.TabIndex = 12;
            this.buttonOK.Text = "Apply";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // layoutButton
            // 
            this.layoutButton.ColumnCount = 3;
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutButton.Controls.Add(this.buttonCamera, 0, 0);
            this.layoutButton.Controls.Add(this.buttonOK, 1, 0);
            this.layoutButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.layoutButton.Location = new System.Drawing.Point(0, 434);
            this.layoutButton.Name = "layoutButton";
            this.layoutButton.RowCount = 1;
            this.layoutButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutButton.Size = new System.Drawing.Size(945, 42);
            this.layoutButton.TabIndex = 13;
            // 
            // buttonCamera
            // 
            this.buttonCamera.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonCamera.Location = new System.Drawing.Point(3, 2);
            this.buttonCamera.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCamera.Name = "buttonCamera";
            this.buttonCamera.Size = new System.Drawing.Size(85, 38);
            this.buttonCamera.TabIndex = 13;
            this.buttonCamera.Text = "Camera";
            this.buttonCamera.UseVisualStyleBackColor = true;
            this.buttonCamera.Click += new System.EventHandler(this.buttonCamera_Click);
            // 
            // SettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.layoutButton);
            this.Name = "SettingPage";
            this.Size = new System.Drawing.Size(945, 476);
            this.layoutButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private Infragistics.Win.Misc.UltraButton buttonOK;
        private System.Windows.Forms.TableLayoutPanel layoutButton;
        private System.Windows.Forms.Button buttonCamera;
    }
}
