namespace UniScanG.Screen.UI.Teach.Monitor
{
    partial class SettingPanel
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.layoutParam = new System.Windows.Forms.TableLayoutPanel();
            this.ultraButton2 = new Infragistics.Win.Misc.UltraButton();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this.buttonTurn = new Infragistics.Win.Misc.UltraButton();
            this.labelLightTitle = new System.Windows.Forms.Label();
            this.lightValue = new System.Windows.Forms.NumericUpDown();
            this.emptyPanel = new System.Windows.Forms.Panel();
            this.layoutParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lightValue)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutParam
            // 
            this.layoutParam.ColumnCount = 5;
            this.layoutParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layoutParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutParam.Controls.Add(this.ultraButton2, 4, 1);
            this.layoutParam.Controls.Add(this.ultraButton1, 2, 1);
            this.layoutParam.Controls.Add(this.buttonTurn, 0, 1);
            this.layoutParam.Controls.Add(this.labelLightTitle, 0, 0);
            this.layoutParam.Controls.Add(this.lightValue, 3, 1);
            this.layoutParam.Controls.Add(this.emptyPanel, 0, 2);
            this.layoutParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutParam.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Bold);
            this.layoutParam.Location = new System.Drawing.Point(0, 0);
            this.layoutParam.Margin = new System.Windows.Forms.Padding(0);
            this.layoutParam.Name = "layoutParam";
            this.layoutParam.RowCount = 3;
            this.layoutParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutParam.Size = new System.Drawing.Size(314, 434);
            this.layoutParam.TabIndex = 27;
            // 
            // ultraButton2
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.ultraButton2.Appearance = appearance1;
            this.ultraButton2.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.ultraButton2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraButton2.ImageSize = new System.Drawing.Size(34, 40);
            this.ultraButton2.Location = new System.Drawing.Point(269, 50);
            this.ultraButton2.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.ultraButton2.Name = "ultraButton2";
            this.ultraButton2.Size = new System.Drawing.Size(40, 40);
            this.ultraButton2.TabIndex = 175;
            this.ultraButton2.Text = "+";
            this.ultraButton2.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // ultraButton1
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.ultraButton1.Appearance = appearance2;
            this.ultraButton1.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.ultraButton1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraButton1.ImageSize = new System.Drawing.Size(34, 40);
            this.ultraButton1.Location = new System.Drawing.Point(144, 50);
            this.ultraButton1.Margin = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(40, 40);
            this.ultraButton1.TabIndex = 175;
            this.ultraButton1.Text = "-";
            this.ultraButton1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // buttonTurn
            // 
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.Image = global::UniScanG.Properties.Resources.Light;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.buttonTurn.Appearance = appearance3;
            this.buttonTurn.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonTurn.ImageSize = new System.Drawing.Size(34, 40);
            this.buttonTurn.Location = new System.Drawing.Point(0, 40);
            this.buttonTurn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTurn.Name = "buttonTurn";
            this.buttonTurn.Size = new System.Drawing.Size(60, 60);
            this.buttonTurn.TabIndex = 174;
            this.buttonTurn.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonTurn.Click += new System.EventHandler(this.buttonTurn_Click);
            // 
            // labelLightTitle
            // 
            this.labelLightTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutParam.SetColumnSpan(this.labelLightTitle, 5);
            this.labelLightTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightTitle.Font = new System.Drawing.Font("Malgun Gothic", 16F, System.Drawing.FontStyle.Bold);
            this.labelLightTitle.Location = new System.Drawing.Point(0, 0);
            this.labelLightTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightTitle.Name = "labelLightTitle";
            this.labelLightTitle.Size = new System.Drawing.Size(314, 40);
            this.labelLightTitle.TabIndex = 26;
            this.labelLightTitle.Text = "Light";
            this.labelLightTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lightValue
            // 
            this.lightValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightValue.Location = new System.Drawing.Point(189, 55);
            this.lightValue.Margin = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.lightValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.lightValue.Name = "lightValue";
            this.lightValue.Size = new System.Drawing.Size(75, 32);
            this.lightValue.TabIndex = 0;
            this.lightValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.lightValue.ValueChanged += new System.EventHandler(this.lightValue_ValueChanged);
            // 
            // emptyPanel
            // 
            this.layoutParam.SetColumnSpan(this.emptyPanel, 5);
            this.emptyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.emptyPanel.Location = new System.Drawing.Point(0, 100);
            this.emptyPanel.Margin = new System.Windows.Forms.Padding(0);
            this.emptyPanel.Name = "emptyPanel";
            this.emptyPanel.Size = new System.Drawing.Size(314, 334);
            this.emptyPanel.TabIndex = 177;
            // 
            // SettingPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutParam);
            this.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SettingPanel";
            this.Size = new System.Drawing.Size(314, 434);
            this.layoutParam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lightValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutParam;
        private System.Windows.Forms.NumericUpDown lightValue;
        private System.Windows.Forms.Label labelLightTitle;
        private System.Windows.Forms.Panel emptyPanel;
        private Infragistics.Win.Misc.UltraButton buttonTurn;
        private Infragistics.Win.Misc.UltraButton ultraButton2;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
    }
}
