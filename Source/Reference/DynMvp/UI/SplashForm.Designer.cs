namespace DynMvp.UI
{
    partial class SplashForm
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
            this.title = new System.Windows.Forms.Label();
            this.copyrightText = new System.Windows.Forms.Label();
            this.buildText = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressMessage = new System.Windows.Forms.Label();
            this.splashActionTimer = new System.Windows.Forms.Timer(this.components);
            this.versionText = new System.Windows.Forms.Label();
            this.productLogo = new System.Windows.Forms.PictureBox();
            this.companyLogo = new System.Windows.Forms.PictureBox();
            this.backgroundImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.productLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundImage)).BeginInit();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.title.Location = new System.Drawing.Point(14, 101);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(537, 89);
            this.title.TabIndex = 0;
            this.title.Text = "UniEye";
            // 
            // copyrightText
            // 
            this.copyrightText.AutoSize = true;
            this.copyrightText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.copyrightText.Location = new System.Drawing.Point(25, 315);
            this.copyrightText.Name = "copyrightText";
            this.copyrightText.Size = new System.Drawing.Size(239, 12);
            this.copyrightText.TabIndex = 1;
            this.copyrightText.Text = "©2015 PlanB Solutions. All right reserved.";
            // 
            // buildText
            // 
            this.buildText.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buildText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buildText.Location = new System.Drawing.Point(377, 258);
            this.buildText.Name = "buildText";
            this.buildText.Size = new System.Drawing.Size(178, 16);
            this.buildText.TabIndex = 1;
            this.buildText.Text = "Build yyyyMMdd.HHmm";
            this.buildText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(27, 279);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(344, 22);
            this.progressBar.TabIndex = 4;
            // 
            // progressMessage
            // 
            this.progressMessage.AutoSize = true;
            this.progressMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressMessage.Location = new System.Drawing.Point(25, 264);
            this.progressMessage.Name = "progressMessage";
            this.progressMessage.Size = new System.Drawing.Size(62, 12);
            this.progressMessage.TabIndex = 1;
            this.progressMessage.Text = "Loading...";
            // 
            // splashActionTimer
            // 
            this.splashActionTimer.Interval = 5000;
            this.splashActionTimer.Tick += new System.EventHandler(this.splashActionTimer_Tick);
            // 
            // versionText
            // 
            this.versionText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.versionText.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.versionText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.versionText.Location = new System.Drawing.Point(377, 241);
            this.versionText.Name = "versionText";
            this.versionText.Size = new System.Drawing.Size(178, 16);
            this.versionText.TabIndex = 1;
            this.versionText.Text = "Version x.y";
            this.versionText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // productLogo
            // 
            this.productLogo.Location = new System.Drawing.Point(484, 0);
            this.productLogo.Name = "productLogo";
            this.productLogo.Size = new System.Drawing.Size(67, 56);
            this.productLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.productLogo.TabIndex = 2;
            this.productLogo.TabStop = false;
            // 
            // companyLogo
            // 
            this.companyLogo.Location = new System.Drawing.Point(377, 279);
            this.companyLogo.Name = "companyLogo";
            this.companyLogo.Size = new System.Drawing.Size(178, 48);
            this.companyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.companyLogo.TabIndex = 2;
            this.companyLogo.TabStop = false;
            // 
            // backgroundImage
            // 
            this.backgroundImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backgroundImage.Image = global::DynMvp.Properties.Resources.sumnale;
            this.backgroundImage.Location = new System.Drawing.Point(0, 0);
            this.backgroundImage.Name = "backgroundImage";
            this.backgroundImage.Size = new System.Drawing.Size(567, 339);
            this.backgroundImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backgroundImage.TabIndex = 3;
            this.backgroundImage.TabStop = false;
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(567, 339);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.productLogo);
            this.Controls.Add(this.companyLogo);
            this.Controls.Add(this.versionText);
            this.Controls.Add(this.buildText);
            this.Controls.Add(this.progressMessage);
            this.Controls.Add(this.copyrightText);
            this.Controls.Add(this.title);
            this.Controls.Add(this.backgroundImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashForm";
            this.Load += new System.EventHandler(this.SplashForm_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SplashForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.productLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backgroundImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox backgroundImage;
        private System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.Label title;
        public System.Windows.Forms.Label buildText;
        public System.Windows.Forms.Label copyrightText;
        public System.Windows.Forms.PictureBox companyLogo;
        public System.Windows.Forms.Label progressMessage;
        private System.Windows.Forms.Timer splashActionTimer;
        public System.Windows.Forms.PictureBox productLogo;
        public System.Windows.Forms.Label versionText;
    }
}