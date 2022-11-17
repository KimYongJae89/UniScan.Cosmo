//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using DynMvp.Devices.MotionController;
//using DynMvp.Base;
//using DynMvp.Devices.Light;
//using UniEye.Base;
//using UniScanG.Operation.Data;
//using DynMvp.Devices.Dio;

//namespace UniScanG.Temp
//{
//    public partial class LightControlPanel : UserControl
//    {
//        bool onUpdate = false;

//        public LightControlPanel()
//        {
//            InitializeComponent();
//        }

//        private void ultraTrackBarLightCh1_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdate)
//                return;

//            onUpdate = true;
//            numericUpDownLightCh1.Value = ultraTrackBarLightCh1.Value;
//            onUpdate = false;
//        }

//        private void ultraTrackBarLightCh2_ValueChanged(object sender, EventArgs e)
//        {
//            if (onUpdate)
//                return;

//            onUpdate = true;
//            numericUpDownLightCh2.Value = ultraTrackBarLightCh2.Value;
//            onUpdate = false;
//        }

//        private void numericUpDownLightCh1_ValueChanged(object sender, EventArgs e)
//        {
//            //ultraTrackBarLightCh1.Value = (int)numericUpDownLightCh1.Value;

//            //Model curModel = (Model)SystemManager.Instance().CurrentModel;
//            //if (curModel == null)
//            //    return;

//            //LightCtrl lightCtrl = SystemManager.Instance().DeviceBox.LightCtrlHandler.GetLightCtrl(0);

//            //int lightValue = (int)Math.Round((float)numericUpDownLightCh1.Value / 100f * lightCtrl.GetMaxLightLevel());
//            //int lightNum = lightCtrl.NumChannel;
//            //for (int i = 0; i < lightNum - 1; i++)
//            //    curModel.LightParamSet.LightParamList[0].LightValue.Value[i] = lightValue;

//            //TurnOnLight(true);
//        }

//        private void numericUpDownLightCh2_ValueChanged(object sender, EventArgs e)
//        {
//            //ultraTrackBarLightCh2.Value = (int)numericUpDownLightCh2.Value;

//            //Model curModel = (Model)SystemManager.Instance().CurrentModel;
//            //if (curModel == null)
//            //    return;

//            //LightCtrl lightCtrl = SystemManager.Instance().DeviceBox.LightCtrlHandler.GetLightCtrl(0);

//            //int lightValue = (int)Math.Round((float)numericUpDownLightCh2.Value / 100f * lightCtrl.GetMaxLightLevel());
//            //int lightNum = lightCtrl.NumChannel;
//            //curModel.LightParamSet.LightParamList[0].LightValue.Value[lightNum - 1] = lightValue;

//            //TurnOnLight(true);
//        }

//        private void TurnOnLight(bool on)
//        {
//            if (on)
//            {
//                LightValue lightValue = SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue;
//                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(lightValue);
//            }
//            else
//            {
//                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
//            }
//        }

//        private void UpdateData()
//        {
//            //if (SystemManager.Instance().CurrentModel == null)
//            //    return;

//            //onUpdate = true;
//            //Model curModel = (Model)SystemManager.Instance().CurrentModel;
//            //ModelDescription modelDescription = (ModelDescription)curModel.ModelDescription;

//            //// Light
//            //if (curModel.LightParamSet.LightParamList.Count ==0)
//            //{
//            //    this.Visible = false;
//            //}else
//            //{
//            //    this.Visible = true;
//            //    LightCtrl lightCtrl = SystemManager.Instance().DeviceBox.LightCtrlHandler.GetLightCtrl(0);
//            //    float maxLightLevel = lightCtrl.GetMaxLightLevel();
//            //    numericUpDownLightCh1.Value = (int)Math.Round(curModel.LightParamSet.LightParamList[0].LightValue.Value[0] / maxLightLevel * 100.0f);
//            //    numericUpDownLightCh2.Value = (int)Math.Round(curModel.LightParamSet.LightParamList[0].LightValue.Value[lightCtrl.NumChannel-1] / maxLightLevel * 100.0f);

//            //}

//            //onUpdate = false;
//        }

//        private void LightControlPanel_VisibleChanged(object sender, EventArgs e)
//        {
//            if (this.Visible == true)
//                UpdateData();
//        }

//        private void buttonOn_Click(object sender, EventArgs e)
//        {
//            TurnOnLight(true);
//        }

//        private void buttonOff_Click(object sender, EventArgs e)
//        {
//            TurnOnLight(false);
//        }
//    }
//}
