using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DynMvp.Base;
using UniEye.Base.Settings.UI;

namespace UniScan.Common.Settings.UI
{
    public interface ICustomConfigPage
    {
        void UpdateData();
        bool SaveData();
    }

    public partial class SystemTypeSettingPanel : UserControl, UniEye.Base.Settings.UI.ICustomConfigPage
    {
        ICustomConfigPage customConfigSubPage;

        public SystemTypeSettingPanel(ICustomConfigPage customConfigSubPage)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            
            labelSystemType.Text = StringManager.GetString(this.GetType().FullName, labelSystemType.Text);

            if (customConfigSubPage != null)
            {
                this.customConfigSubPage = customConfigSubPage;
                subPanel.Controls.Add((Control)customConfigSubPage);
            }
        }

        public void UpdateData()
        {
            systemType.SelectedItem = SystemTypeSettings.Instance().SystemType.ToString();
            resizeRatio.Value = (decimal)SystemTypeSettings.Instance().ResizeRatio;
            //checkStandalone.Checked = SystemTypeSettings.Instance().IsStandAlone;

            if (customConfigSubPage != null)
                customConfigSubPage.UpdateData();
        }

        public bool SaveData()
        {
            SystemTypeSettings.Instance().SystemType = (SystemType)Enum.Parse(typeof(SystemType), systemType.SelectedItem.ToString());
            SystemTypeSettings.Instance().ResizeRatio = (float)resizeRatio.Value;
            //SystemTypeSettings.Instance().IsStandAlone = checkStandalone.Checked;
            SystemTypeSettings.Instance().Save();

            return customConfigSubPage.SaveData();
        }

        private void resizeRatio_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}
