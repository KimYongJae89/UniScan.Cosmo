//namespace UniScanG.Temp
//{
//    partial class MachineControlPanel
//    {
//        /// <summary> 
//        /// 필수 디자이너 변수입니다.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// 사용 중인 모든 리소스를 정리합니다.
//        /// </summary>
//        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region 구성 요소 디자이너에서 생성한 코드

//        /// <summary> 
//        /// 디자이너 지원에 필요한 메서드입니다. 
//        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.groupBoxSpeed = new System.Windows.Forms.GroupBox();
//            this.comboBoxSpeedUnit = new System.Windows.Forms.ComboBox();
//            this.labelSpeed = new System.Windows.Forms.Label();
//            this.ultraTrackBarSpeed = new Infragistics.Win.UltraWinEditors.UltraTrackBar();
//            this.groupBoxDirection = new System.Windows.Forms.GroupBox();
//            this.buttonMoveBackward = new System.Windows.Forms.Button();
//            this.buttonMoveStop = new System.Windows.Forms.Button();
//            this.buttonMoveForward = new System.Windows.Forms.Button();
//            this.groupBoxAccel = new System.Windows.Forms.GroupBox();
//            this.labelAccel = new System.Windows.Forms.Label();
//            this.ultraTrackBarAccel = new Infragistics.Win.UltraWinEditors.UltraTrackBar();
//            this.groupBoxSpeed.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraTrackBarSpeed)).BeginInit();
//            this.groupBoxDirection.SuspendLayout();
//            this.groupBoxAccel.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraTrackBarAccel)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // groupBoxSpeed
//            // 
//            this.groupBoxSpeed.Controls.Add(this.comboBoxSpeedUnit);
//            this.groupBoxSpeed.Controls.Add(this.labelSpeed);
//            this.groupBoxSpeed.Controls.Add(this.ultraTrackBarSpeed);
//            this.groupBoxSpeed.Dock = System.Windows.Forms.DockStyle.Top;
//            this.groupBoxSpeed.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.groupBoxSpeed.Location = new System.Drawing.Point(0, 0);
//            this.groupBoxSpeed.Name = "groupBoxSpeed";
//            this.groupBoxSpeed.Size = new System.Drawing.Size(339, 80);
//            this.groupBoxSpeed.TabIndex = 1;
//            this.groupBoxSpeed.TabStop = false;
//            this.groupBoxSpeed.Text = "Speed";
//            // 
//            // comboBoxSpeedUnit
//            // 
//            this.comboBoxSpeedUnit.FormattingEnabled = true;
//            this.comboBoxSpeedUnit.Items.AddRange(new object[] {
//            "m/sec",
//            "m/min"});
//            this.comboBoxSpeedUnit.Location = new System.Drawing.Point(252, 30);
//            this.comboBoxSpeedUnit.Name = "comboBoxSpeedUnit";
//            this.comboBoxSpeedUnit.Size = new System.Drawing.Size(74, 29);
//            this.comboBoxSpeedUnit.TabIndex = 6;
//            this.comboBoxSpeedUnit.SelectedIndexChanged += new System.EventHandler(this.comboBoxTimeUnitType_SelectedIndexChanged);
//            // 
//            // labelSpeed
//            // 
//            this.labelSpeed.AutoSize = true;
//            this.labelSpeed.Location = new System.Drawing.Point(210, 33);
//            this.labelSpeed.Name = "labelSpeed";
//            this.labelSpeed.Size = new System.Drawing.Size(41, 21);
//            this.labelSpeed.TabIndex = 5;
//            this.labelSpeed.Text = "00.0";
//            this.labelSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//            // 
//            // ultraTrackBarSpeed
//            // 
//            this.ultraTrackBarSpeed.Location = new System.Drawing.Point(7, 32);
//            this.ultraTrackBarSpeed.MaxValue = 20;
//            this.ultraTrackBarSpeed.MinValue = 1;
//            this.ultraTrackBarSpeed.Name = "ultraTrackBarSpeed";
//            this.ultraTrackBarSpeed.Size = new System.Drawing.Size(197, 29);
//            this.ultraTrackBarSpeed.TabIndex = 4;
//            this.ultraTrackBarSpeed.Value = 1;
//            this.ultraTrackBarSpeed.ValueObject = 1;
//            this.ultraTrackBarSpeed.ValueChanged += new System.EventHandler(this.ultraTrackBarSpeed_ValueChanged);
//            // 
//            // groupBoxDirection
//            // 
//            this.groupBoxDirection.Controls.Add(this.buttonMoveBackward);
//            this.groupBoxDirection.Controls.Add(this.buttonMoveStop);
//            this.groupBoxDirection.Controls.Add(this.buttonMoveForward);
//            this.groupBoxDirection.Dock = System.Windows.Forms.DockStyle.Top;
//            this.groupBoxDirection.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.groupBoxDirection.Location = new System.Drawing.Point(0, 148);
//            this.groupBoxDirection.Name = "groupBoxDirection";
//            this.groupBoxDirection.Size = new System.Drawing.Size(339, 79);
//            this.groupBoxDirection.TabIndex = 2;
//            this.groupBoxDirection.TabStop = false;
//            this.groupBoxDirection.Text = "Test Move";
//            // 
//            // buttonMoveBackward
//            // 
//            this.buttonMoveBackward.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.buttonMoveBackward.Location = new System.Drawing.Point(221, 25);
//            this.buttonMoveBackward.Name = "buttonMoveBackward";
//            this.buttonMoveBackward.Size = new System.Drawing.Size(101, 44);
//            this.buttonMoveBackward.TabIndex = 1;
//            this.buttonMoveBackward.Text = "Backward";
//            this.buttonMoveBackward.UseVisualStyleBackColor = true;
//            this.buttonMoveBackward.Click += new System.EventHandler(this.buttonMoveBackward_Click);
//            // 
//            // buttonMoveStop
//            // 
//            this.buttonMoveStop.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.buttonMoveStop.Location = new System.Drawing.Point(114, 25);
//            this.buttonMoveStop.Name = "buttonMoveStop";
//            this.buttonMoveStop.Size = new System.Drawing.Size(101, 44);
//            this.buttonMoveStop.TabIndex = 1;
//            this.buttonMoveStop.Text = "S-Stop";
//            this.buttonMoveStop.UseVisualStyleBackColor = true;
//            this.buttonMoveStop.Click += new System.EventHandler(this.buttonMoveStop_Click);
//            // 
//            // buttonMoveForward
//            // 
//            this.buttonMoveForward.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.buttonMoveForward.Location = new System.Drawing.Point(7, 25);
//            this.buttonMoveForward.Name = "buttonMoveForward";
//            this.buttonMoveForward.Size = new System.Drawing.Size(101, 44);
//            this.buttonMoveForward.TabIndex = 1;
//            this.buttonMoveForward.Text = "Forward";
//            this.buttonMoveForward.UseVisualStyleBackColor = true;
//            this.buttonMoveForward.Click += new System.EventHandler(this.buttonMoveForward_Click);
//            // 
//            // groupBoxAccel
//            // 
//            this.groupBoxAccel.Controls.Add(this.labelAccel);
//            this.groupBoxAccel.Controls.Add(this.ultraTrackBarAccel);
//            this.groupBoxAccel.Dock = System.Windows.Forms.DockStyle.Top;
//            this.groupBoxAccel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.groupBoxAccel.Location = new System.Drawing.Point(0, 80);
//            this.groupBoxAccel.Name = "groupBoxAccel";
//            this.groupBoxAccel.Size = new System.Drawing.Size(339, 68);
//            this.groupBoxAccel.TabIndex = 8;
//            this.groupBoxAccel.TabStop = false;
//            this.groupBoxAccel.Text = "Acceleration";
//            // 
//            // labelAccel
//            // 
//            this.labelAccel.AutoSize = true;
//            this.labelAccel.Location = new System.Drawing.Point(284, 28);
//            this.labelAccel.Name = "labelAccel";
//            this.labelAccel.Size = new System.Drawing.Size(41, 21);
//            this.labelAccel.TabIndex = 5;
//            this.labelAccel.Text = "00.0";
//            this.labelAccel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
//            // 
//            // ultraTrackBarAccel
//            // 
//            this.ultraTrackBarAccel.Location = new System.Drawing.Point(7, 28);
//            this.ultraTrackBarAccel.MaxValue = 20;
//            this.ultraTrackBarAccel.MinValue = 1;
//            this.ultraTrackBarAccel.Name = "ultraTrackBarAccel";
//            this.ultraTrackBarAccel.Size = new System.Drawing.Size(271, 29);
//            this.ultraTrackBarAccel.TabIndex = 4;
//            this.ultraTrackBarAccel.Value = 1;
//            this.ultraTrackBarAccel.ValueObject = 1;
//            this.ultraTrackBarAccel.ValueChanged += new System.EventHandler(this.ultraTrackBarAccel_ValueChanged);
//            // 
//            // MachineControlPanel
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.AutoSize = true;
//            this.Controls.Add(this.groupBoxDirection);
//            this.Controls.Add(this.groupBoxAccel);
//            this.Controls.Add(this.groupBoxSpeed);
//            this.Name = "MachineControlPanel";
//            this.Size = new System.Drawing.Size(339, 227);
//            this.Load += new System.EventHandler(this.MachineControlPanel_Load);
//            this.VisibleChanged += new System.EventHandler(this.MachineControlPanel_VisibleChanged);
//            this.groupBoxSpeed.ResumeLayout(false);
//            this.groupBoxSpeed.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraTrackBarSpeed)).EndInit();
//            this.groupBoxDirection.ResumeLayout(false);
//            this.groupBoxAccel.ResumeLayout(false);
//            this.groupBoxAccel.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraTrackBarAccel)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion
//        private System.Windows.Forms.GroupBox groupBoxSpeed;
//        private System.Windows.Forms.GroupBox groupBoxDirection;
//        private System.Windows.Forms.Button buttonMoveBackward;
//        private System.Windows.Forms.Button buttonMoveForward;
//        private System.Windows.Forms.Label labelSpeed;
//        private Infragistics.Win.UltraWinEditors.UltraTrackBar ultraTrackBarSpeed;
//        private System.Windows.Forms.ComboBox comboBoxSpeedUnit;
//        private System.Windows.Forms.GroupBox groupBoxAccel;
//        private System.Windows.Forms.Label labelAccel;
//        private Infragistics.Win.UltraWinEditors.UltraTrackBar ultraTrackBarAccel;
//        private System.Windows.Forms.Button buttonMoveStop;
//    }
//}
