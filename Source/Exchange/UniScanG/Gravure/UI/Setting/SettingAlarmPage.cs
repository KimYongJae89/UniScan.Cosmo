using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.Vision;
using DynMvp.Base;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.UI.Setting
{
    public partial class SettingAlarmPage : UserControl, IMultiLanguageSupport
    {
        private bool onUpdate;

        public SettingAlarmPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize()
        {
            onUpdate = true;
            normalN.Minimum = repeatedN.Minimum = continuedN.Minimum = sheetLengthN.Minimum = 1;
            normalN.Maximum = repeatedN.Maximum = continuedN.Maximum = sheetLengthN.Maximum = int.MaxValue;

            normalR.Minimum = repeatedR.Minimum = continuedR.Minimum = 1;
            sheetLengthD.Minimum = (decimal)0.001;

            normalR.Minimum = repeatedR.Maximum = continuedR.Maximum = 100;
            sheetLengthD.Maximum = decimal.MaxValue;

            onUpdate = false;

            UpdateData();
        }

        public void UpdateData()
        {
            onUpdate = true;

            AdditionalSettings settings = (AdditionalSettings)AdditionalSettings.Instance();

            this.useNormalDefectAlarm.Checked = settings.NormalDefectAlarm.Use;
            this.normalN.Value = Math.Max(this.normalN.Minimum, (decimal)settings.NormalDefectAlarm.Count);
            this.normalR.Value = Math.Max(this.normalR.Minimum, (decimal)settings.NormalDefectAlarm.Ratio);

            this.useRepeatedDefectAlarm.Checked = settings.RepeatedDefectAlarm.Use;
            this.repeatedN.Value = Math.Max(this.repeatedN.Minimum, (decimal)settings.RepeatedDefectAlarm.Count);
            this.repeatedR.Value = Math.Max(this.repeatedR.Minimum, (decimal)settings.RepeatedDefectAlarm.Ratio);

            this.useContinuedDefectAlarm.Checked = settings.ContinuousDefectAlarm.Use;
            this.continuedN.Value = Math.Max(this.continuedN.Minimum, (decimal)settings.ContinuousDefectAlarm.Count);
            this.continuedR.Value = Math.Max(this.continuedR.Minimum, (decimal)settings.ContinuousDefectAlarm.Ratio);

            this.useSheetLengthAlarm.Checked = settings.SheetLengthAlarm.Use;
            this.sheetLengthN.Value = Math.Max(this.sheetLengthN.Minimum, (decimal)settings.SheetLengthAlarm.Count);
            this.sheetLengthD.Value = Math.Max(this.sheetLengthD.Minimum, (decimal)settings.SheetLengthAlarm.Value);

            onUpdate = false;
        }

        private void ApplyData()
        {
            AdditionalSettings settings = (AdditionalSettings)AdditionalSettings.Instance();

            settings.NormalDefectAlarm.Use = this.useNormalDefectAlarm.Checked;
            settings.NormalDefectAlarm.Count = (int)this.normalN.Value;
            settings.NormalDefectAlarm.Ratio = (float)this.normalR.Value;

            settings.RepeatedDefectAlarm.Use = this.useRepeatedDefectAlarm.Checked;
            settings.RepeatedDefectAlarm.Count = (int)this.repeatedN.Value;
            settings.RepeatedDefectAlarm.Ratio = (float)this.repeatedR.Value;

            settings.ContinuousDefectAlarm.Use = this.useContinuedDefectAlarm.Checked;
            settings.ContinuousDefectAlarm.Count = (int)this.continuedN.Value;
            settings.ContinuousDefectAlarm.Ratio = (float)this.continuedR.Value;

            settings.SheetLengthAlarm.Use = this.useSheetLengthAlarm.Checked;
            settings.SheetLengthAlarm.Count = (int)this.sheetLengthN.Value;
            settings.SheetLengthAlarm.Value = (float)this.sheetLengthD.Value;

            settings.Save();
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            ApplyData();
        }
    }
}
