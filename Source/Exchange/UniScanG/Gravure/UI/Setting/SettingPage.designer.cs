namespace UniScanG.Gravure.UI.Setting
{
    partial class SettingPage
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageAlarm = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageComm = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPageMonitoring = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.imageCheckInterval = new System.Windows.Forms.NumericUpDown();
            this.labelImageCheckInterval = new System.Windows.Forms.Label();
            this.labelSec = new System.Windows.Forms.Label();
            this.labelMs = new System.Windows.Forms.Label();
            this.settingTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingTabControl)).BeginInit();
            this.settingTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageAlarm
            // 
            this.ultraTabPageAlarm.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageAlarm.Margin = new System.Windows.Forms.Padding(0);
            this.ultraTabPageAlarm.Name = "ultraTabPageAlarm";
            this.ultraTabPageAlarm.Padding = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.ultraTabPageAlarm.Size = new System.Drawing.Size(1118, 625);
            // 
            // ultraTabPageComm
            // 
            this.ultraTabPageComm.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageComm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ultraTabPageComm.Name = "ultraTabPageComm";
            this.ultraTabPageComm.Size = new System.Drawing.Size(1118, 625);
            // 
            // ultraTabPageMonitoring
            // 
            this.ultraTabPageMonitoring.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageMonitoring.Margin = new System.Windows.Forms.Padding(0);
            this.ultraTabPageMonitoring.Name = "ultraTabPageMonitoring";
            this.ultraTabPageMonitoring.Padding = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.ultraTabPageMonitoring.Size = new System.Drawing.Size(1118, 625);
            // 
            // imageCheckInterval
            // 
            this.imageCheckInterval.Location = new System.Drawing.Point(211, 119);
            this.imageCheckInterval.Name = "imageCheckInterval";
            this.imageCheckInterval.Size = new System.Drawing.Size(68, 21);
            this.imageCheckInterval.TabIndex = 0;
            // 
            // labelImageCheckInterval
            // 
            this.labelImageCheckInterval.AutoSize = true;
            this.labelImageCheckInterval.Location = new System.Drawing.Point(15, 123);
            this.labelImageCheckInterval.Name = "labelImageCheckInterval";
            this.labelImageCheckInterval.Size = new System.Drawing.Size(151, 17);
            this.labelImageCheckInterval.TabIndex = 0;
            this.labelImageCheckInterval.Text = "Image Check Interval";
            // 
            // labelSec
            // 
            this.labelSec.AutoSize = true;
            this.labelSec.Location = new System.Drawing.Point(286, 126);
            this.labelSec.Name = "labelSec";
            this.labelSec.Size = new System.Drawing.Size(29, 17);
            this.labelSec.TabIndex = 0;
            this.labelSec.Text = "sec";
            // 
            // labelMs
            // 
            this.labelMs.Location = new System.Drawing.Point(0, 0);
            this.labelMs.Name = "labelMs";
            this.labelMs.Size = new System.Drawing.Size(100, 23);
            this.labelMs.TabIndex = 0;
            // 
            // settingTabControl
            // 
            appearance1.BackColor = System.Drawing.Color.Gray;
            this.settingTabControl.ActiveTabAppearance = appearance1;
            appearance2.BackColor = System.Drawing.SystemColors.Control;
            this.settingTabControl.ClientAreaAppearance = appearance2;
            this.settingTabControl.Controls.Add(this.ultraTabSharedControlsPage1);
            this.settingTabControl.Controls.Add(this.ultraTabPageAlarm);
            this.settingTabControl.Controls.Add(this.ultraTabPageMonitoring);
            this.settingTabControl.Controls.Add(this.ultraTabPageComm);
            this.settingTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingTabControl.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.settingTabControl.InterRowSpacing = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.InterTabSpacing = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.Location = new System.Drawing.Point(0, 0);
            this.settingTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.settingTabControl.Name = "settingTabControl";
            this.settingTabControl.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.settingTabControl.Size = new System.Drawing.Size(1217, 625);
            this.settingTabControl.SpaceAfterTabs = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.SpaceBeforeTabs = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.StateButtons;
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(226)))), ((int)(((byte)(241)))));
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "맑은 고딕";
            this.settingTabControl.TabHeaderAreaAppearance = appearance3;
            this.settingTabControl.TabIndex = 0;
            this.settingTabControl.TabLayoutStyle = Infragistics.Win.UltraWinTabs.TabLayoutStyle.SingleRowFixed;
            this.settingTabControl.TabOrientation = Infragistics.Win.UltraWinTabs.TabOrientation.LeftTop;
            this.settingTabControl.TabPadding = new System.Drawing.Size(10, 10);
            ultraTab3.Key = "Alarm";
            ultraTab3.TabPage = this.ultraTabPageAlarm;
            ultraTab3.Text = "Alarm";
            ultraTab1.Key = "Comm";
            ultraTab1.TabPage = this.ultraTabPageComm;
            ultraTab1.Text = "Comm";
            ultraTab4.Key = "General";
            ultraTab4.TabPage = this.ultraTabPageMonitoring;
            ultraTab4.Text = "General";
            this.settingTabControl.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab3,
            ultraTab1,
            ultraTab4});
            this.settingTabControl.TextOrientation = Infragistics.Win.UltraWinTabs.TextOrientation.Horizontal;
            this.settingTabControl.SelectedTabChanging += new Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventHandler(this.settingTabControl_SelectedTabChanging);
            this.settingTabControl.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabControlParam_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(99, 0);
            this.ultraTabSharedControlsPage1.Margin = new System.Windows.Forms.Padding(0);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1118, 625);
            // 
            // SettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.settingTabControl);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SettingPage";
            this.Size = new System.Drawing.Size(1217, 625);
            this.Load += new System.EventHandler(this.SettingPage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingTabControl)).EndInit();
            this.settingTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NumericUpDown imageCheckInterval;
        private System.Windows.Forms.Label labelImageCheckInterval;
        private System.Windows.Forms.Label labelSec;
        private System.Windows.Forms.Label labelMs;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl settingTabControl;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageAlarm;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageMonitoring;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageComm;
    }
}
