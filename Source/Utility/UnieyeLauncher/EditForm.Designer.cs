namespace UnieyeLauncher
{
    partial class EditForm
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.autoPatchCheckBox = new System.Windows.Forms.CheckBox();
            this.patchFilePathTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.patchFilePathLabel = new Infragistics.Win.Misc.UltraLabel();
            this.patchFilePathDirButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // autoPatchCheckBox
            // 
            this.autoPatchCheckBox.AutoSize = true;
            this.autoPatchCheckBox.Location = new System.Drawing.Point(160, 65);
            this.autoPatchCheckBox.Name = "autoPatchCheckBox";
            this.autoPatchCheckBox.Size = new System.Drawing.Size(121, 16);
            this.autoPatchCheckBox.TabIndex = 0;
            this.autoPatchCheckBox.Text = "Auto Patch Mode";
            this.autoPatchCheckBox.UseVisualStyleBackColor = true;
            // 
            // patchFilePathTextBox
            // 
            this.patchFilePathTextBox.Location = new System.Drawing.Point(113, 24);
            this.patchFilePathTextBox.Name = "patchFilePathTextBox";
            this.patchFilePathTextBox.Size = new System.Drawing.Size(249, 21);
            this.patchFilePathTextBox.TabIndex = 2;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(122, 103);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(83, 31);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(241, 103);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(83, 31);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // patchFilePathLabel
            // 
            appearance1.BorderColor2 = System.Drawing.Color.Black;
            this.patchFilePathLabel.Appearance = appearance1;
            this.patchFilePathLabel.Location = new System.Drawing.Point(14, 27);
            this.patchFilePathLabel.Name = "patchFilePathLabel";
            this.patchFilePathLabel.Size = new System.Drawing.Size(100, 23);
            this.patchFilePathLabel.TabIndex = 6;
            this.patchFilePathLabel.Text = "Patch File Path";
            // 
            // patchFilePathDirButton
            // 
            this.patchFilePathDirButton.Location = new System.Drawing.Point(368, 24);
            this.patchFilePathDirButton.Name = "patchFilePathDirButton";
            this.patchFilePathDirButton.Size = new System.Drawing.Size(33, 23);
            this.patchFilePathDirButton.TabIndex = 8;
            this.patchFilePathDirButton.Text = "...";
            this.patchFilePathDirButton.UseVisualStyleBackColor = true;
            this.patchFilePathDirButton.Click += new System.EventHandler(this.patchFilePathDirButton_Click);
            // 
            // EditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 143);
            this.Controls.Add(this.patchFilePathDirButton);
            this.Controls.Add(this.patchFilePathLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.patchFilePathTextBox);
            this.Controls.Add(this.autoPatchCheckBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditForm";
            this.Text = "EditForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox autoPatchCheckBox;
        private System.Windows.Forms.TextBox patchFilePathTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private Infragistics.Win.Misc.UltraLabel patchFilePathLabel;
        private System.Windows.Forms.Button patchFilePathDirButton;
    }
}