namespace UniScanG.Temp
{
    partial class StateManualForm
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
            this.components = new System.ComponentModel.Container();
            this.appStylistRuntime1 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
            this.pictureBoxStateManual = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStateManual)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxStateManual
            // 
            this.pictureBoxStateManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxStateManual.Image = global::UniScanG.Properties.Resources.State_Manual;
            this.pictureBoxStateManual.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxStateManual.Name = "pictureBoxStateManual";
            this.pictureBoxStateManual.Size = new System.Drawing.Size(784, 566);
            this.pictureBoxStateManual.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxStateManual.TabIndex = 0;
            this.pictureBoxStateManual.TabStop = false;
            // 
            // StateManualForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 566);
            this.Controls.Add(this.pictureBoxStateManual);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StateManualForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "State Guide";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStateManual)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime1;
        private System.Windows.Forms.PictureBox pictureBoxStateManual;
    }
}