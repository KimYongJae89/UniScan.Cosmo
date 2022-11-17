using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Devices.Light;
using DynMvp.Devices;

namespace UniScanG.UI.Teach.Monitor
{
    public partial class LightSettingPanel : UserControl
    {
        LightCtrl lightCtrl;
        LightParam userSetLightParam;
        LightParam autoSetLightParam;
        LightParam curLightParam;

        List<Label> labelList;
        List<Label> valueList;
        List<TrackBar> trackBarList;
        List<NumericUpDown> numericUpDownList;

        bool onUpdate = false;

        public LightSettingPanel(LightCtrl lightCtrl, LightParam userSetLightParam, LightParam autoSetLightParam)
        {
            InitializeComponent();

            this.lightCtrl = lightCtrl;
            this.userSetLightParam = userSetLightParam;
            this.autoSetLightParam = autoSetLightParam;
            this.curLightParam = new LightParam(userSetLightParam.LightValue.NumLight, 0);

            this.valueList = new List<Label>();
            this.valueList.AddRange(new Label[] { this.lightTopLeft, this.lightTopMiddle, this.lightTopRight, this.lightBottom });

            this.trackBarList = new List<TrackBar>();
            this.trackBarList.AddRange(new TrackBar[] { this.trackBarLightTopLeft, this.trackBarLightTopMiddle, this.trackBarLightTopRight, this.trackBarLightTopBottom });

            this.numericUpDownList = new List<NumericUpDown>();
            this.numericUpDownList.AddRange(new NumericUpDown[] { this.numericLightTopLeft, this.numericLightTopMiddle, this.numericLightTopRight, this.numericLightTopBottom });
        }

        private void LightSettingPanel_Load(object sender, EventArgs e)
        {
            //TurnOn();
            UpdateData(true);
        }

        public void UpdateData(bool updateAll)
        {
            onUpdate = true;
            int numLight = Math.Min(this.userSetLightParam.LightValue.NumLight, 4);

            for (int i = 0; i < numLight; i++)
            {
                numericUpDownList[i].Minimum = trackBarList[i].Minimum = 0;
                numericUpDownList[i].Maximum = trackBarList[i].Maximum = this.lightCtrl.GetMaxLightLevel();

                valueList[i].Text = curLightParam.LightValue.Value[i].ToString();

                if (updateAll)
                    numericUpDownList[i].Value = trackBarList[i].Value = userSetLightParam.LightValue.Value[i];
            }

            for (int i = numLight; i < 4; i++)
            {
                numericUpDownList[i].Enabled = trackBarList[i].Enabled = false;
            }

            //this.MinimumSize = new Size(300, labelList.Count(f => f.Visible) * 40);

            onUpdate = false;
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            TrackBar trackBar = sender as TrackBar;
            if (trackBar != null)
            {
                int idx = trackBarList.FindIndex(f => f == trackBar);
                if (idx < 0)
                    return;

                int newValue = (int)trackBarList[idx].Value;
                this.numericUpDownList[idx].Value = newValue;
                this.userSetLightParam.LightValue.Value[idx] = newValue;
                //TurnOn(idx, newValue, false);
            }
        }

        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            NumericUpDown numericUpDown = sender as NumericUpDown;
            if (numericUpDown != null)
            {
                int idx = numericUpDownList.FindIndex(f => f == numericUpDown);
                if (idx < 0)
                    return;

                int newValue = (int)numericUpDown.Value;
                this.trackBarList[idx].Value = newValue;
                this.userSetLightParam.LightValue.Value[idx] = newValue;
                //TurnOn(idx, newValue,false);
            }
        }

        private void buttonOn_Click(object sender, EventArgs e)
        {
            TurnOn();
        }

        private void TurnOn()
        {
            if (Array.TrueForAll(this.userSetLightParam.LightValue.Value, f => f == 0))
                TurnOn(this.autoSetLightParam);
            else
                TurnOn(this.userSetLightParam);
        }

        private void TurnOn(int idx, int value)
        {
            this.userSetLightParam.LightValue.Value[idx] = value;
            TurnOn(this.userSetLightParam);
        }

        private void TurnOn(LightParam lightParam)
        {
            curLightParam = lightParam;
            lightCtrl.TurnOn(curLightParam.LightValue);
            UpdateData(false);
        }

        private void buttonOff_Click(object sender, EventArgs e)
        {
            TurnOn(new LightParam(curLightParam.LightValue.NumLight, 0));
        }
    }
}
