namespace AlphaMotionMove
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonMove = new System.Windows.Forms.Button();
            this.buttonEMG = new System.Windows.Forms.Button();
            this.buttonServo = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxAcc = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxSpd = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxEndPos = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxStartPos = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxRetVel = new System.Windows.Forms.ComboBox();
            this.comboBoxArrVel = new System.Windows.Forms.ComboBox();
            this.comboBoxMovBVel = new System.Windows.Forms.ComboBox();
            this.labelCurIoVal = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxMovFVel = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.checkBoxRet = new System.Windows.Forms.CheckBox();
            this.checkBoxMovB = new System.Windows.Forms.CheckBox();
            this.checkBoxArr = new System.Windows.Forms.CheckBox();
            this.checkBoxMovF = new System.Windows.Forms.CheckBox();
            this.timerUiUpdate = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(39, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonMove);
            this.groupBox1.Controls.Add(this.buttonEMG);
            this.groupBox1.Controls.Add(this.buttonServo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBoxAcc);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxSpd);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxEndPos);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxStartPos);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(714, 130);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Motion";
            // 
            // buttonMove
            // 
            this.buttonMove.Location = new System.Drawing.Point(474, 75);
            this.buttonMove.Name = "buttonMove";
            this.buttonMove.Size = new System.Drawing.Size(99, 32);
            this.buttonMove.TabIndex = 15;
            this.buttonMove.Text = "Move";
            this.buttonMove.UseVisualStyleBackColor = true;
            this.buttonMove.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // buttonEMG
            // 
            this.buttonEMG.Location = new System.Drawing.Point(579, 37);
            this.buttonEMG.Name = "buttonEMG";
            this.buttonEMG.Size = new System.Drawing.Size(99, 70);
            this.buttonEMG.TabIndex = 14;
            this.buttonEMG.Text = "OMG!";
            this.buttonEMG.UseVisualStyleBackColor = true;
            this.buttonEMG.Click += new System.EventHandler(this.buttonEMG_Click);
            // 
            // buttonServo
            // 
            this.buttonServo.Location = new System.Drawing.Point(474, 37);
            this.buttonServo.Name = "buttonServo";
            this.buttonServo.Size = new System.Drawing.Size(99, 32);
            this.buttonServo.TabIndex = 13;
            this.buttonServo.Text = "ON/OFF";
            this.buttonServo.UseVisualStyleBackColor = true;
            this.buttonServo.Click += new System.EventHandler(this.buttonServo_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(393, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 25);
            this.label7.TabIndex = 12;
            this.label7.Text = "ms";
            // 
            // textBoxAcc
            // 
            this.textBoxAcc.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBoxAcc.Location = new System.Drawing.Point(311, 75);
            this.textBoxAcc.Name = "textBoxAcc";
            this.textBoxAcc.Size = new System.Drawing.Size(81, 32);
            this.textBoxAcc.TabIndex = 11;
            this.textBoxAcc.Text = "100";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(241, 79);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 25);
            this.label8.TabIndex = 10;
            this.label8.Text = "Acc";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(393, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 25);
            this.label5.TabIndex = 9;
            this.label5.Text = "mm/s";
            // 
            // textBoxSpd
            // 
            this.textBoxSpd.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBoxSpd.Location = new System.Drawing.Point(311, 37);
            this.textBoxSpd.Name = "textBoxSpd";
            this.textBoxSpd.Size = new System.Drawing.Size(81, 32);
            this.textBoxSpd.TabIndex = 8;
            this.textBoxSpd.Text = "100";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(241, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 25);
            this.label6.TabIndex = 7;
            this.label6.Text = "Speed";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(177, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "mm";
            // 
            // textBoxEndPos
            // 
            this.textBoxEndPos.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBoxEndPos.Location = new System.Drawing.Point(95, 75);
            this.textBoxEndPos.Name = "textBoxEndPos";
            this.textBoxEndPos.Size = new System.Drawing.Size(81, 32);
            this.textBoxEndPos.TabIndex = 5;
            this.textBoxEndPos.Text = "300";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(39, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 25);
            this.label4.TabIndex = 4;
            this.label4.Text = "End";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(177, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "mm";
            // 
            // textBoxStartPos
            // 
            this.textBoxStartPos.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBoxStartPos.Location = new System.Drawing.Point(95, 37);
            this.textBoxStartPos.Name = "textBoxStartPos";
            this.textBoxStartPos.Size = new System.Drawing.Size(81, 32);
            this.textBoxStartPos.TabIndex = 2;
            this.textBoxStartPos.Text = "0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxRetVel);
            this.groupBox2.Controls.Add(this.comboBoxArrVel);
            this.groupBox2.Controls.Add(this.comboBoxMovBVel);
            this.groupBox2.Controls.Add(this.labelCurIoVal);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.comboBoxMovFVel);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.checkBoxRet);
            this.groupBox2.Controls.Add(this.checkBoxMovB);
            this.groupBox2.Controls.Add(this.checkBoxArr);
            this.groupBox2.Controls.Add(this.checkBoxMovF);
            this.groupBox2.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox2.Location = new System.Drawing.Point(12, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(714, 264);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Digital I/O";
            // 
            // comboBoxRetVel
            // 
            this.comboBoxRetVel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRetVel.FormattingEnabled = true;
            this.comboBoxRetVel.Items.AddRange(new object[] {
            "High",
            "Low"});
            this.comboBoxRetVel.Location = new System.Drawing.Point(474, 157);
            this.comboBoxRetVel.Name = "comboBoxRetVel";
            this.comboBoxRetVel.Size = new System.Drawing.Size(88, 33);
            this.comboBoxRetVel.TabIndex = 12;
            // 
            // comboBoxArrVel
            // 
            this.comboBoxArrVel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxArrVel.FormattingEnabled = true;
            this.comboBoxArrVel.Items.AddRange(new object[] {
            "High",
            "Low"});
            this.comboBoxArrVel.Location = new System.Drawing.Point(474, 118);
            this.comboBoxArrVel.Name = "comboBoxArrVel";
            this.comboBoxArrVel.Size = new System.Drawing.Size(88, 33);
            this.comboBoxArrVel.TabIndex = 11;
            // 
            // comboBoxMovBVel
            // 
            this.comboBoxMovBVel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMovBVel.FormattingEnabled = true;
            this.comboBoxMovBVel.Items.AddRange(new object[] {
            "High",
            "Low"});
            this.comboBoxMovBVel.Location = new System.Drawing.Point(474, 79);
            this.comboBoxMovBVel.Name = "comboBoxMovBVel";
            this.comboBoxMovBVel.Size = new System.Drawing.Size(88, 33);
            this.comboBoxMovBVel.TabIndex = 10;
            // 
            // labelCurIoVal
            // 
            this.labelCurIoVal.AutoSize = true;
            this.labelCurIoVal.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCurIoVal.Location = new System.Drawing.Point(265, 208);
            this.labelCurIoVal.Name = "labelCurIoVal";
            this.labelCurIoVal.Size = new System.Drawing.Size(107, 25);
            this.labelCurIoVal.TabIndex = 9;
            this.labelCurIoVal.Text = "{CurValue}";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.Location = new System.Drawing.Point(81, 208);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(157, 25);
            this.label11.TabIndex = 8;
            this.label11.Text = "Current Value is";
            // 
            // comboBoxMovFVel
            // 
            this.comboBoxMovFVel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMovFVel.FormattingEnabled = true;
            this.comboBoxMovFVel.Items.AddRange(new object[] {
            "High",
            "Low"});
            this.comboBoxMovFVel.Location = new System.Drawing.Point(474, 40);
            this.comboBoxMovFVel.Name = "comboBoxMovFVel";
            this.comboBoxMovFVel.Size = new System.Drawing.Size(88, 33);
            this.comboBoxMovFVel.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(62, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(176, 25);
            this.label9.TabIndex = 5;
            this.label9.Text = "Set Active When...";
            // 
            // checkBoxRet
            // 
            this.checkBoxRet.AutoSize = true;
            this.checkBoxRet.Location = new System.Drawing.Point(270, 159);
            this.checkBoxRet.Name = "checkBoxRet";
            this.checkBoxRet.Size = new System.Drawing.Size(113, 29);
            this.checkBoxRet.TabIndex = 3;
            this.checkBoxRet.Text = "Returned";
            this.checkBoxRet.UseVisualStyleBackColor = true;
            // 
            // checkBoxMovB
            // 
            this.checkBoxMovB.AutoSize = true;
            this.checkBoxMovB.Location = new System.Drawing.Point(270, 81);
            this.checkBoxMovB.Name = "checkBoxMovB";
            this.checkBoxMovB.Size = new System.Drawing.Size(195, 29);
            this.checkBoxMovB.TabIndex = 2;
            this.checkBoxMovB.Text = "Moving Backword";
            this.checkBoxMovB.UseVisualStyleBackColor = true;
            // 
            // checkBoxArr
            // 
            this.checkBoxArr.AutoSize = true;
            this.checkBoxArr.Location = new System.Drawing.Point(270, 120);
            this.checkBoxArr.Name = "checkBoxArr";
            this.checkBoxArr.Size = new System.Drawing.Size(95, 29);
            this.checkBoxArr.TabIndex = 1;
            this.checkBoxArr.Text = "Arrived";
            this.checkBoxArr.UseVisualStyleBackColor = true;
            // 
            // checkBoxMovF
            // 
            this.checkBoxMovF.AutoSize = true;
            this.checkBoxMovF.Location = new System.Drawing.Point(270, 42);
            this.checkBoxMovF.Name = "checkBoxMovF";
            this.checkBoxMovF.Size = new System.Drawing.Size(180, 29);
            this.checkBoxMovF.TabIndex = 0;
            this.checkBoxMovF.Text = "Moving Forwrad";
            this.checkBoxMovF.UseVisualStyleBackColor = true;
            // 
            // timerUiUpdate
            // 
            this.timerUiUpdate.Tick += new System.EventHandler(this.timerUiUpdate_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 426);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "AlpahMotion State Mover";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonEMG;
        private System.Windows.Forms.Button buttonServo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxAcc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxSpd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxEndPos;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxStartPos;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelCurIoVal;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxMovFVel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox checkBoxRet;
        private System.Windows.Forms.CheckBox checkBoxMovB;
        private System.Windows.Forms.CheckBox checkBoxArr;
        private System.Windows.Forms.CheckBox checkBoxMovF;
        private System.Windows.Forms.Button buttonMove;
        private System.Windows.Forms.Timer timerUiUpdate;
        private System.Windows.Forms.ComboBox comboBoxRetVel;
        private System.Windows.Forms.ComboBox comboBoxArrVel;
        private System.Windows.Forms.ComboBox comboBoxMovBVel;
    }
}

