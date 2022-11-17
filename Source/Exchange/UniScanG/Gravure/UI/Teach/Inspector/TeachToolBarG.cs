using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.UI.Touch;
using System.Collections.Generic;
using System.Windows.Forms;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;
using UniScanG.UI.Teach;
using UniScanG.UI.Teach.Inspector;

namespace UniScanG.Gravure.UI.Teach.Inspector
{
    public partial class TeachToolBarG : UserControl, IModellerControl, IMultiLanguageSupport
    {
        ModellerPageExtender modellerPageExtender;

        public TeachToolBarG()
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

        private void buttonSheetGrab_Click(object sender, System.EventArgs e)
        {
            int grabCount = 1;
            if (UserHandler.Instance().CurrentUser.SuperAccount)
            {
                InputForm inputForm = new InputForm(StringManager.GetString("How many grab?"), "1");
                if (inputForm.ShowDialog() == DialogResult.Cancel)
                    return;

                bool ok = int.TryParse(inputForm.InputText, out grabCount);
                if (ok == false)
                    return;
            }

            bool good = modellerPageExtender.GrabSheet(grabCount);
            if (good == false)
                MessageForm.Show(null, "Grab Fail");
        }

        private void buttonFrameGrab_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.GrabFrame();
        }

        private void buttonAutoTeach_Click(object sender, System.EventArgs e)
        {
            modellerPageExtender.Teach(true);
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
        }


    }
}
