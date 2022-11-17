namespace UniScanG.Gravure.UI.Setting
{
    partial class SettingCommPage
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
            this.checkBoxAutoOperation = new System.Windows.Forms.CheckBox();
            this.groupBoxPlcCommStatus = new System.Windows.Forms.GroupBox();
            this.openIoViewer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBoxAutoOperation
            // 
            this.checkBoxAutoOperation.AutoSize = true;
            this.checkBoxAutoOperation.Location = new System.Drawing.Point(3, 3);
            this.checkBoxAutoOperation.Name = "checkBoxAutoOperation";
            this.checkBoxAutoOperation.Size = new System.Drawing.Size(145, 25);
            this.checkBoxAutoOperation.TabIndex = 0;
            this.checkBoxAutoOperation.Text = "Auto Operation";
            this.checkBoxAutoOperation.UseVisualStyleBackColor = true;
            this.checkBoxAutoOperation.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // groupBoxPlcCommStatus
            // 
            this.groupBoxPlcCommStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBoxPlcCommStatus.Location = new System.Drawing.Point(0, 185);
            this.groupBoxPlcCommStatus.Name = "groupBoxPlcCommStatus";
            this.groupBoxPlcCommStatus.Size = new System.Drawing.Size(949, 308);
            this.groupBoxPlcCommStatus.TabIndex = 2;
            this.groupBoxPlcCommStatus.TabStop = false;
            this.groupBoxPlcCommStatus.Text = "groupBox1";
            // 
            // openIoViewer
            // 
            this.openIoViewer.Location = new System.Drawing.Point(3, 34);
            this.openIoViewer.Name = "openIoViewer";
            this.openIoViewer.Size = new System.Drawing.Size(173, 47);
            this.openIoViewer.TabIndex = 3;
            this.openIoViewer.Text = "Open I/O Viewer";
            this.openIoViewer.UseVisualStyleBackColor = true;
            this.openIoViewer.Click += new System.EventHandler(this.openIoViewer_Click);
            // 
            // SettingCommPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.openIoViewer);
            this.Controls.Add(this.groupBoxPlcCommStatus);
            this.Controls.Add(this.checkBoxAutoOperation);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "SettingCommPage";
            this.Size = new System.Drawing.Size(949, 493);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxAutoOperation;
        private System.Windows.Forms.GroupBox groupBoxPlcCommStatus;
        private System.Windows.Forms.Button openIoViewer;
    }
}
