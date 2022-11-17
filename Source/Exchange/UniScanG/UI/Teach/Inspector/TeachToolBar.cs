using DynMvp.Base;
using DynMvp.UI.Touch;
using System.Collections.Generic;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.UI.Teach;
using UniScanG.UI.Teach.Inspector;

namespace UniScanG.UI.Teach.Inspector
{
    public partial class TeachToolBar : UserControl, IModellerControl, IMultiLanguageSupport
    {
        ModellerPageExtender modellerPageExtender;

        public TeachToolBar()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;
        }

        public void SetModellerExtender(UniScanG.UI.Teach.ModellerPageExtender modellerPageExtender)
        {
            this.modellerPageExtender = (ModellerPageExtender)modellerPageExtender;
        }

        private void buttonGrab_Click(object sender, System.EventArgs e)
        {
            bool good = modellerPageExtender.GrabSheet(1);
            if (good == false)
                MessageForm.Show(null, "Grab Fail");
        }
        
        private void buttonAutoTeach_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.Teach(false);
        }

        private void buttonInspect_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.Inspect();
        }

        private void buttonSave_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.SaveModel();
        }

        private void buttonLoadImage_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.LoadImage();
        }

        private void buttonExportData_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.DataExport();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            
            foreach (ToolStripItem item in toolStripMain.Items)
                item.Text = StringManager.GetString(this.GetType().FullName, item.Text);
        }
    }
}
