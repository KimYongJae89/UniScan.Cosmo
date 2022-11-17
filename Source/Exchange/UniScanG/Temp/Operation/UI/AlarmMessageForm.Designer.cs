namespace UniScanG.Temp
{
    partial class AlarmMessageForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmMessageForm));
            this.SettingPage_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.messsage = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.imageIcon = new System.Windows.Forms.PictureBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonReset = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelAlarm = new System.Windows.Forms.Label();
            this.checkSaveTargetImage = new System.Windows.Forms.CheckBox();
            this.errorCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.buttonBuzzerOff = new System.Windows.Forms.Button();
            this.SettingPage_Fill_Panel.ClientArea.SuspendLayout();
            this.SettingPage_Fill_Panel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageIcon)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingPage_Fill_Panel
            // 
            // 
            // SettingPage_Fill_Panel.ClientArea
            // 
            this.SettingPage_Fill_Panel.ClientArea.Controls.Add(this.panel1);
            this.SettingPage_Fill_Panel.ClientArea.Controls.Add(this.panelBottom);
            this.SettingPage_Fill_Panel.ClientArea.Controls.Add(this.panelTop);
            this.SettingPage_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.SettingPage_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingPage_Fill_Panel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SettingPage_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.SettingPage_Fill_Panel.Name = "SettingPage_Fill_Panel";
            this.SettingPage_Fill_Panel.Size = new System.Drawing.Size(625, 222);
            this.SettingPage_Fill_Panel.TabIndex = 161;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.imageIcon);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(625, 120);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.messsage);
            this.panel2.Controls.Add(this.labelMessage);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(93, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(532, 120);
            this.panel2.TabIndex = 2;
            // 
            // messsage
            // 
            this.messsage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.messsage.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.messsage.Location = new System.Drawing.Point(141, 3);
            this.messsage.Name = "messsage";
            this.messsage.Size = new System.Drawing.Size(387, 114);
            this.messsage.TabIndex = 2;
            this.messsage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMessage
            // 
            this.labelMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.labelMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMessage.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMessage.Location = new System.Drawing.Point(3, 3);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(137, 114);
            this.labelMessage.TabIndex = 2;
            this.labelMessage.Text = "Message";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageIcon
            // 
            this.imageIcon.BackColor = System.Drawing.Color.Transparent;
            this.imageIcon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("imageIcon.BackgroundImage")));
            this.imageIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.imageIcon.Dock = System.Windows.Forms.DockStyle.Left;
            this.imageIcon.Location = new System.Drawing.Point(0, 0);
            this.imageIcon.Name = "imageIcon";
            this.imageIcon.Size = new System.Drawing.Size(93, 120);
            this.imageIcon.TabIndex = 0;
            this.imageIcon.TabStop = false;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelBottom.Controls.Add(this.buttonBuzzerOff);
            this.panelBottom.Controls.Add(this.buttonReset);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 168);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(625, 54);
            this.panelBottom.TabIndex = 2;
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(523, 6);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(95, 38);
            this.buttonReset.TabIndex = 1;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panelTop.Controls.Add(this.labelAlarm);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(625, 48);
            this.panelTop.TabIndex = 0;
            // 
            // labelAlarm
            // 
            this.labelAlarm.AutoSize = true;
            this.labelAlarm.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAlarm.ForeColor = System.Drawing.Color.White;
            this.labelAlarm.Location = new System.Drawing.Point(12, 9);
            this.labelAlarm.Name = "labelAlarm";
            this.labelAlarm.Size = new System.Drawing.Size(72, 30);
            this.labelAlarm.TabIndex = 0;
            this.labelAlarm.Text = "Alarm";
            // 
            // checkSaveTargetImage
            // 
            this.checkSaveTargetImage.AutoSize = true;
            this.checkSaveTargetImage.Location = new System.Drawing.Point(15, 188);
            this.checkSaveTargetImage.Name = "checkSaveTargetImage";
            this.checkSaveTargetImage.Size = new System.Drawing.Size(157, 24);
            this.checkSaveTargetImage.TabIndex = 160;
            this.checkSaveTargetImage.Text = "Save target image";
            this.checkSaveTargetImage.UseVisualStyleBackColor = true;
            // 
            // buttonBuzzerOff
            // 
            this.buttonBuzzerOff.Location = new System.Drawing.Point(9, 6);
            this.buttonBuzzerOff.Name = "buttonBuzzerOff";
            this.buttonBuzzerOff.Size = new System.Drawing.Size(95, 38);
            this.buttonBuzzerOff.TabIndex = 2;
            this.buttonBuzzerOff.Text = "Sound Off";
            this.buttonBuzzerOff.UseVisualStyleBackColor = true;
            this.buttonBuzzerOff.Click += new System.EventHandler(this.buttonBuzzerOff_Click);
            // 
            // AlarmMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 222);
            this.Controls.Add(this.SettingPage_Fill_Panel);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlarmMessageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alarm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AlarmMessageForm_FormClosing);
            this.SettingPage_Fill_Panel.ClientArea.ResumeLayout(false);
            this.SettingPage_Fill_Panel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageIcon)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.Misc.UltraPanel SettingPage_Fill_Panel;
        private System.Windows.Forms.CheckBox checkSaveTargetImage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox imageIcon;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label messsage;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.Timer errorCheckTimer;
        private System.Windows.Forms.Label labelAlarm;
        private System.Windows.Forms.Button buttonBuzzerOff;
    }
}
