namespace UniScan.Monitor.Device.Gravure.Laser
{
    partial class HanbitLaserControlForm
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
            this.setEmergencyTog = new System.Windows.Forms.Button();
            this.setResetTog = new System.Windows.Forms.Button();
            this.setReadyOn = new System.Windows.Forms.Button();
            this.setReadyOff = new System.Windows.Forms.Button();
            this.setErrorOn = new System.Windows.Forms.Button();
            this.setErrorOff = new System.Windows.Forms.Button();
            this.setOutofrangeOn = new System.Windows.Forms.Button();
            this.setOutofrangeOff = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmStateReset = new System.Windows.Forms.CheckBox();
            this.cmStateAlive = new System.Windows.Forms.CheckBox();
            this.cmStateEmergency = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.setAliveOn = new System.Windows.Forms.Button();
            this.setAliveOff = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.laserStateOutofrange = new System.Windows.Forms.CheckBox();
            this.laserStateError = new System.Windows.Forms.CheckBox();
            this.laserStateReady = new System.Windows.Forms.CheckBox();
            this.laserStateAlive = new System.Windows.Forms.CheckBox();
            this.cmStateRun = new System.Windows.Forms.CheckBox();
            this.cmStateNg = new System.Windows.Forms.CheckBox();
            this.setNgTog = new System.Windows.Forms.Button();
            this.setRunTog = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // setEmergencyTog
            // 
            this.setEmergencyTog.Location = new System.Drawing.Point(8, 27);
            this.setEmergencyTog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setEmergencyTog.Name = "setEmergencyTog";
            this.setEmergencyTog.Size = new System.Drawing.Size(141, 50);
            this.setEmergencyTog.TabIndex = 0;
            this.setEmergencyTog.Text = "EMERGENCY\r\nTOG";
            this.setEmergencyTog.UseVisualStyleBackColor = true;
            this.setEmergencyTog.Click += new System.EventHandler(this.Set_Click);
            // 
            // setResetTog
            // 
            this.setResetTog.Location = new System.Drawing.Point(157, 27);
            this.setResetTog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setResetTog.Name = "setResetTog";
            this.setResetTog.Size = new System.Drawing.Size(141, 50);
            this.setResetTog.TabIndex = 0;
            this.setResetTog.Text = "RESET\r\nTOG";
            this.setResetTog.UseVisualStyleBackColor = true;
            this.setResetTog.Click += new System.EventHandler(this.Set_Click);
            // 
            // setReadyOn
            // 
            this.setReadyOn.Location = new System.Drawing.Point(7, 220);
            this.setReadyOn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setReadyOn.Name = "setReadyOn";
            this.setReadyOn.Size = new System.Drawing.Size(141, 50);
            this.setReadyOn.TabIndex = 0;
            this.setReadyOn.Text = "READY\r\nON";
            this.setReadyOn.UseVisualStyleBackColor = true;
            this.setReadyOn.Click += new System.EventHandler(this.Set_Click);
            // 
            // setReadyOff
            // 
            this.setReadyOff.Location = new System.Drawing.Point(156, 220);
            this.setReadyOff.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setReadyOff.Name = "setReadyOff";
            this.setReadyOff.Size = new System.Drawing.Size(141, 50);
            this.setReadyOff.TabIndex = 0;
            this.setReadyOff.Text = "READY\r\nOFF";
            this.setReadyOff.UseVisualStyleBackColor = true;
            this.setReadyOff.Click += new System.EventHandler(this.Set_Click);
            // 
            // setErrorOn
            // 
            this.setErrorOn.Location = new System.Drawing.Point(7, 278);
            this.setErrorOn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setErrorOn.Name = "setErrorOn";
            this.setErrorOn.Size = new System.Drawing.Size(141, 50);
            this.setErrorOn.TabIndex = 0;
            this.setErrorOn.Text = "ERROR\r\nON";
            this.setErrorOn.UseVisualStyleBackColor = true;
            this.setErrorOn.Click += new System.EventHandler(this.Set_Click);
            // 
            // setErrorOff
            // 
            this.setErrorOff.Location = new System.Drawing.Point(156, 278);
            this.setErrorOff.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setErrorOff.Name = "setErrorOff";
            this.setErrorOff.Size = new System.Drawing.Size(141, 50);
            this.setErrorOff.TabIndex = 0;
            this.setErrorOff.Text = "ERROR\r\nOFF";
            this.setErrorOff.UseVisualStyleBackColor = true;
            this.setErrorOff.Click += new System.EventHandler(this.Set_Click);
            // 
            // setOutofrangeOn
            // 
            this.setOutofrangeOn.Location = new System.Drawing.Point(8, 336);
            this.setOutofrangeOn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setOutofrangeOn.Name = "setOutofrangeOn";
            this.setOutofrangeOn.Size = new System.Drawing.Size(141, 50);
            this.setOutofrangeOn.TabIndex = 0;
            this.setOutofrangeOn.Text = "OUT OF RANGE\r\nON";
            this.setOutofrangeOn.UseVisualStyleBackColor = true;
            this.setOutofrangeOn.Click += new System.EventHandler(this.Set_Click);
            // 
            // setOutofrangeOff
            // 
            this.setOutofrangeOff.Location = new System.Drawing.Point(157, 336);
            this.setOutofrangeOff.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setOutofrangeOff.Name = "setOutofrangeOff";
            this.setOutofrangeOff.Size = new System.Drawing.Size(141, 50);
            this.setOutofrangeOff.TabIndex = 0;
            this.setOutofrangeOff.Text = "OUT OF RANGE\r\nOFF";
            this.setOutofrangeOff.UseVisualStyleBackColor = true;
            this.setOutofrangeOff.Click += new System.EventHandler(this.Set_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmStateNg);
            this.groupBox1.Controls.Add(this.cmStateReset);
            this.groupBox1.Controls.Add(this.cmStateAlive);
            this.groupBox1.Controls.Add(this.cmStateRun);
            this.groupBox1.Controls.Add(this.cmStateEmergency);
            this.groupBox1.Location = new System.Drawing.Point(13, 194);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(183, 212);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CM State";
            // 
            // cmStateReset
            // 
            this.cmStateReset.AutoSize = true;
            this.cmStateReset.Location = new System.Drawing.Point(26, 100);
            this.cmStateReset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmStateReset.Name = "cmStateReset";
            this.cmStateReset.Size = new System.Drawing.Size(70, 25);
            this.cmStateReset.TabIndex = 2;
            this.cmStateReset.Text = "Reset";
            this.cmStateReset.UseVisualStyleBackColor = true;
            // 
            // cmStateAlive
            // 
            this.cmStateAlive.AutoSize = true;
            this.cmStateAlive.Location = new System.Drawing.Point(26, 30);
            this.cmStateAlive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmStateAlive.Name = "cmStateAlive";
            this.cmStateAlive.Size = new System.Drawing.Size(65, 25);
            this.cmStateAlive.TabIndex = 2;
            this.cmStateAlive.Text = "Alive";
            this.cmStateAlive.UseVisualStyleBackColor = true;
            // 
            // cmStateEmergency
            // 
            this.cmStateEmergency.AutoSize = true;
            this.cmStateEmergency.Location = new System.Drawing.Point(26, 65);
            this.cmStateEmergency.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmStateEmergency.Name = "cmStateEmergency";
            this.cmStateEmergency.Size = new System.Drawing.Size(110, 25);
            this.cmStateEmergency.TabIndex = 2;
            this.cmStateEmergency.Text = "Emergency";
            this.cmStateEmergency.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.setAliveOn);
            this.groupBox2.Controls.Add(this.setRunTog);
            this.groupBox2.Controls.Add(this.setEmergencyTog);
            this.groupBox2.Controls.Add(this.setNgTog);
            this.groupBox2.Controls.Add(this.setAliveOff);
            this.groupBox2.Controls.Add(this.setResetTog);
            this.groupBox2.Controls.Add(this.setOutofrangeOff);
            this.groupBox2.Controls.Add(this.setReadyOn);
            this.groupBox2.Controls.Add(this.setOutofrangeOn);
            this.groupBox2.Controls.Add(this.setReadyOff);
            this.groupBox2.Controls.Add(this.setErrorOff);
            this.groupBox2.Controls.Add(this.setErrorOn);
            this.groupBox2.Location = new System.Drawing.Point(204, 14);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(307, 392);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Commmand";
            // 
            // setAliveOn
            // 
            this.setAliveOn.Location = new System.Drawing.Point(7, 162);
            this.setAliveOn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setAliveOn.Name = "setAliveOn";
            this.setAliveOn.Size = new System.Drawing.Size(141, 50);
            this.setAliveOn.TabIndex = 0;
            this.setAliveOn.Text = "ALIVE\r\nON";
            this.setAliveOn.UseVisualStyleBackColor = true;
            this.setAliveOn.Click += new System.EventHandler(this.Set_Click);
            // 
            // setAliveOff
            // 
            this.setAliveOff.Location = new System.Drawing.Point(156, 162);
            this.setAliveOff.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setAliveOff.Name = "setAliveOff";
            this.setAliveOff.Size = new System.Drawing.Size(141, 50);
            this.setAliveOff.TabIndex = 0;
            this.setAliveOff.Text = "ALIVE\r\nOFF";
            this.setAliveOff.UseVisualStyleBackColor = true;
            this.setAliveOff.Click += new System.EventHandler(this.Set_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.laserStateOutofrange);
            this.groupBox3.Controls.Add(this.laserStateError);
            this.groupBox3.Controls.Add(this.laserStateReady);
            this.groupBox3.Controls.Add(this.laserStateAlive);
            this.groupBox3.Location = new System.Drawing.Point(13, 14);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(183, 170);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Laser State";
            // 
            // laserStateOutofrange
            // 
            this.laserStateOutofrange.AutoSize = true;
            this.laserStateOutofrange.Location = new System.Drawing.Point(26, 134);
            this.laserStateOutofrange.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserStateOutofrange.Name = "laserStateOutofrange";
            this.laserStateOutofrange.Size = new System.Drawing.Size(131, 25);
            this.laserStateOutofrange.TabIndex = 2;
            this.laserStateOutofrange.Text = "Out Of Range";
            this.laserStateOutofrange.UseVisualStyleBackColor = true;
            // 
            // laserStateError
            // 
            this.laserStateError.AutoSize = true;
            this.laserStateError.Location = new System.Drawing.Point(26, 100);
            this.laserStateError.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserStateError.Name = "laserStateError";
            this.laserStateError.Size = new System.Drawing.Size(65, 25);
            this.laserStateError.TabIndex = 2;
            this.laserStateError.Text = "Error";
            this.laserStateError.UseVisualStyleBackColor = true;
            // 
            // laserStateReady
            // 
            this.laserStateReady.AutoSize = true;
            this.laserStateReady.Location = new System.Drawing.Point(26, 66);
            this.laserStateReady.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserStateReady.Name = "laserStateReady";
            this.laserStateReady.Size = new System.Drawing.Size(74, 25);
            this.laserStateReady.TabIndex = 2;
            this.laserStateReady.Text = "Ready";
            this.laserStateReady.UseVisualStyleBackColor = true;
            // 
            // laserStateAlive
            // 
            this.laserStateAlive.AutoSize = true;
            this.laserStateAlive.Location = new System.Drawing.Point(26, 32);
            this.laserStateAlive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.laserStateAlive.Name = "laserStateAlive";
            this.laserStateAlive.Size = new System.Drawing.Size(65, 25);
            this.laserStateAlive.TabIndex = 2;
            this.laserStateAlive.Text = "Alive";
            this.laserStateAlive.UseVisualStyleBackColor = true;
            // 
            // cmStateRun
            // 
            this.cmStateRun.AutoSize = true;
            this.cmStateRun.Location = new System.Drawing.Point(26, 135);
            this.cmStateRun.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmStateRun.Name = "cmStateRun";
            this.cmStateRun.Size = new System.Drawing.Size(57, 25);
            this.cmStateRun.TabIndex = 2;
            this.cmStateRun.Text = "Run";
            this.cmStateRun.UseVisualStyleBackColor = true;
            // 
            // cmStateNg
            // 
            this.cmStateNg.AutoSize = true;
            this.cmStateNg.Location = new System.Drawing.Point(26, 170);
            this.cmStateNg.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmStateNg.Name = "cmStateNg";
            this.cmStateNg.Size = new System.Drawing.Size(52, 25);
            this.cmStateNg.TabIndex = 2;
            this.cmStateNg.Text = "NG";
            this.cmStateNg.UseVisualStyleBackColor = true;
            // 
            // setNgTog
            // 
            this.setNgTog.Location = new System.Drawing.Point(157, 86);
            this.setNgTog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setNgTog.Name = "setNgTog";
            this.setNgTog.Size = new System.Drawing.Size(141, 50);
            this.setNgTog.TabIndex = 0;
            this.setNgTog.Text = "NG\r\nTOG";
            this.setNgTog.UseVisualStyleBackColor = true;
            this.setNgTog.Click += new System.EventHandler(this.Set_Click);
            // 
            // setRunTog
            // 
            this.setRunTog.Location = new System.Drawing.Point(8, 86);
            this.setRunTog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.setRunTog.Name = "setRunTog";
            this.setRunTog.Size = new System.Drawing.Size(141, 50);
            this.setRunTog.TabIndex = 0;
            this.setRunTog.Text = "RUN\r\nTOG";
            this.setRunTog.UseVisualStyleBackColor = true;
            this.setRunTog.Click += new System.EventHandler(this.Set_Click);
            // 
            // HanbitLaserControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 415);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HanbitLaserControlForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "HanbitLaserControlForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HanbitLaserControlForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button setEmergencyTog;
        private System.Windows.Forms.Button setResetTog;
        private System.Windows.Forms.Button setReadyOn;
        private System.Windows.Forms.Button setReadyOff;
        private System.Windows.Forms.Button setErrorOn;
        private System.Windows.Forms.Button setErrorOff;
        private System.Windows.Forms.Button setOutofrangeOn;
        private System.Windows.Forms.Button setOutofrangeOff;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cmStateReset;
        private System.Windows.Forms.CheckBox cmStateEmergency;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox laserStateOutofrange;
        private System.Windows.Forms.CheckBox laserStateError;
        private System.Windows.Forms.CheckBox laserStateReady;
        private System.Windows.Forms.CheckBox laserStateAlive;
        private System.Windows.Forms.Button setAliveOn;
        private System.Windows.Forms.Button setAliveOff;
        private System.Windows.Forms.CheckBox cmStateAlive;
        private System.Windows.Forms.CheckBox cmStateNg;
        private System.Windows.Forms.CheckBox cmStateRun;
        private System.Windows.Forms.Button setRunTog;
        private System.Windows.Forms.Button setNgTog;
    }
}