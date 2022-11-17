//namespace UniScanG.Temp
//{
//    partial class LightControlPanel
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
//            this.groupBox5 = new System.Windows.Forms.GroupBox();
//            this.numericUpDownLightCh2 = new System.Windows.Forms.NumericUpDown();
//            this.numericUpDownLightCh1 = new System.Windows.Forms.NumericUpDown();
//            this.ultraTrackBarLightCh2 = new Infragistics.Win.UltraWinEditors.UltraTrackBar();
//            this.labelLightCh2 = new System.Windows.Forms.Label();
//            this.labelLightCh1 = new System.Windows.Forms.Label();
//            this.ultraTrackBarLightCh1 = new Infragistics.Win.UltraWinEditors.UltraTrackBar();
//            this.buttonOn = new System.Windows.Forms.Button();
//            this.buttonOff = new System.Windows.Forms.Button();
//            this.groupBox5.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLightCh2)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLightCh1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraTrackBarLightCh2)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraTrackBarLightCh1)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // groupBox5
//            // 
//            this.groupBox5.Controls.Add(this.numericUpDownLightCh2);
//            this.groupBox5.Controls.Add(this.numericUpDownLightCh1);
//            this.groupBox5.Controls.Add(this.ultraTrackBarLightCh2);
//            this.groupBox5.Controls.Add(this.labelLightCh2);
//            this.groupBox5.Controls.Add(this.labelLightCh1);
//            this.groupBox5.Controls.Add(this.ultraTrackBarLightCh1);
//            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
//            this.groupBox5.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.groupBox5.Location = new System.Drawing.Point(0, 0);
//            this.groupBox5.Name = "groupBox5";
//            this.groupBox5.Size = new System.Drawing.Size(356, 108);
//            this.groupBox5.TabIndex = 3;
//            this.groupBox5.TabStop = false;
//            this.groupBox5.Text = "Light";
//            // 
//            // numericUpDownLightCh2
//            // 
//            this.numericUpDownLightCh2.Location = new System.Drawing.Point(267, 63);
//            this.numericUpDownLightCh2.Name = "numericUpDownLightCh2";
//            this.numericUpDownLightCh2.Size = new System.Drawing.Size(74, 29);
//            this.numericUpDownLightCh2.TabIndex = 14;
//            this.numericUpDownLightCh2.Value = new decimal(new int[] {
//            50,
//            0,
//            0,
//            0});
//            this.numericUpDownLightCh2.ValueChanged += new System.EventHandler(this.numericUpDownLightCh2_ValueChanged);
//            // 
//            // numericUpDownLightCh1
//            // 
//            this.numericUpDownLightCh1.Location = new System.Drawing.Point(268, 26);
//            this.numericUpDownLightCh1.Name = "numericUpDownLightCh1";
//            this.numericUpDownLightCh1.Size = new System.Drawing.Size(74, 29);
//            this.numericUpDownLightCh1.TabIndex = 13;
//            this.numericUpDownLightCh1.Value = new decimal(new int[] {
//            50,
//            0,
//            0,
//            0});
//            this.numericUpDownLightCh1.ValueChanged += new System.EventHandler(this.numericUpDownLightCh1_ValueChanged);
//            // 
//            // ultraTrackBarLightCh2
//            // 
//            this.ultraTrackBarLightCh2.Location = new System.Drawing.Point(85, 63);
//            this.ultraTrackBarLightCh2.MaxValue = 100;
//            this.ultraTrackBarLightCh2.Name = "ultraTrackBarLightCh2";
//            this.ultraTrackBarLightCh2.Size = new System.Drawing.Size(177, 29);
//            this.ultraTrackBarLightCh2.SmallChange = 10;
//            this.ultraTrackBarLightCh2.TabIndex = 8;
//            this.ultraTrackBarLightCh2.Value = 50;
//            this.ultraTrackBarLightCh2.ValueObject = 50;
//            this.ultraTrackBarLightCh2.ValueChanged += new System.EventHandler(this.ultraTrackBarLightCh2_ValueChanged);
//            // 
//            // labelLightCh2
//            // 
//            this.labelLightCh2.AutoSize = true;
//            this.labelLightCh2.Location = new System.Drawing.Point(15, 65);
//            this.labelLightCh2.Name = "labelLightCh2";
//            this.labelLightCh2.Size = new System.Drawing.Size(65, 21);
//            this.labelLightCh2.TabIndex = 7;
//            this.labelLightCh2.Text = "Bottom";
//            // 
//            // labelLightCh1
//            // 
//            this.labelLightCh1.AutoSize = true;
//            this.labelLightCh1.Location = new System.Drawing.Point(15, 28);
//            this.labelLightCh1.Name = "labelLightCh1";
//            this.labelLightCh1.Size = new System.Drawing.Size(39, 21);
//            this.labelLightCh1.TabIndex = 6;
//            this.labelLightCh1.Text = "Top";
//            // 
//            // ultraTrackBarLightCh1
//            // 
//            this.ultraTrackBarLightCh1.Location = new System.Drawing.Point(85, 28);
//            this.ultraTrackBarLightCh1.MaxValue = 100;
//            this.ultraTrackBarLightCh1.Name = "ultraTrackBarLightCh1";
//            this.ultraTrackBarLightCh1.Size = new System.Drawing.Size(177, 29);
//            this.ultraTrackBarLightCh1.SmallChange = 10;
//            this.ultraTrackBarLightCh1.TabIndex = 5;
//            this.ultraTrackBarLightCh1.Value = 50;
//            this.ultraTrackBarLightCh1.ValueObject = 50;
//            this.ultraTrackBarLightCh1.ValueChanged += new System.EventHandler(this.ultraTrackBarLightCh1_ValueChanged);
//            // 
//            // buttonOn
//            // 
//            this.buttonOn.Location = new System.Drawing.Point(19, 114);
//            this.buttonOn.Name = "buttonOn";
//            this.buttonOn.Size = new System.Drawing.Size(128, 44);
//            this.buttonOn.TabIndex = 4;
//            this.buttonOn.Text = "OOON";
//            this.buttonOn.UseVisualStyleBackColor = true;
//            this.buttonOn.Click += new System.EventHandler(this.buttonOn_Click);
//            // 
//            // buttonOff
//            // 
//            this.buttonOff.Location = new System.Drawing.Point(213, 114);
//            this.buttonOff.Name = "buttonOff";
//            this.buttonOff.Size = new System.Drawing.Size(128, 44);
//            this.buttonOff.TabIndex = 5;
//            this.buttonOff.Text = "OOOFF";
//            this.buttonOff.UseVisualStyleBackColor = true;
//            this.buttonOff.Click += new System.EventHandler(this.buttonOff_Click);
//            // 
//            // LightControlPanel
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.AutoSize = true;
//            this.Controls.Add(this.buttonOff);
//            this.Controls.Add(this.buttonOn);
//            this.Controls.Add(this.groupBox5);
//            this.Name = "LightControlPanel";
//            this.Size = new System.Drawing.Size(356, 161);
//            this.VisibleChanged += new System.EventHandler(this.LightControlPanel_VisibleChanged);
//            this.groupBox5.ResumeLayout(false);
//            this.groupBox5.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLightCh2)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLightCh1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraTrackBarLightCh2)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraTrackBarLightCh1)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion
//        private System.Windows.Forms.GroupBox groupBox5;
//        private Infragistics.Win.UltraWinEditors.UltraTrackBar ultraTrackBarLightCh2;
//        private System.Windows.Forms.Label labelLightCh2;
//        private System.Windows.Forms.Label labelLightCh1;
//        private Infragistics.Win.UltraWinEditors.UltraTrackBar ultraTrackBarLightCh1;
//        private System.Windows.Forms.NumericUpDown numericUpDownLightCh2;
//        private System.Windows.Forms.NumericUpDown numericUpDownLightCh1;
//        private System.Windows.Forms.Button buttonOn;
//        private System.Windows.Forms.Button buttonOff;
//    }
//}
