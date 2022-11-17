using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;
using DynMvp.Devices.Comm;
using DynMvp.Base;
using DynMvp.Device.Serial;

namespace DynMvp.Devices.UI
{
    public partial class LightConfigForm : Form
    {
        DigitalIoHandler digitalIoHandler;
        public DigitalIoHandler DigitalIoHandler
        {
            get { return digitalIoHandler; }
            set { digitalIoHandler = value; }
        }

        LightCtrlInfo lightCtrlInfo;
        public LightCtrlInfo LightCtrlInfo
        {
            get { return lightCtrlInfo; }
            set { lightCtrlInfo = value; }
        }

        string lightCtrlName;
        public string LightCtrlName
        {
            get { return lightCtrlName; }
            set { lightCtrlName = value; }
        }

        SerialPortInfo serialPortInfo = new SerialPortInfo();

        bool initialized = false;

        public LightConfigForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(this.GetType().FullName, labelName.Text);
            labelNumLight.Text = StringManager.GetString(this.GetType().FullName, labelNumLight.Text);
            useIoLightCtrl.Text = StringManager.GetString(this.GetType().FullName, useIoLightCtrl.Text);
            useSerialLightCtrl.Text = StringManager.GetString(this.GetType().FullName, useSerialLightCtrl.Text);
            buttonEditLightCtrlPort.Text = StringManager.GetString(this.GetType().FullName, buttonEditLightCtrlPort.Text);
            buttonTestLightController.Text = StringManager.GetString(this.GetType().FullName, buttonTestLightController.Text);

            buttonOk.Text = StringManager.GetString(this.GetType().FullName,buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

            buttonEditLightCtrlPort.Enabled = false;
            comboLightControllerVender.Enabled = false;

            labelSerialPortInfo.Text = "";
            lightCtrlName = "LightController";

            comboLightControllerVender.DataSource = Enum.GetNames(typeof(LightControllerVender));
        }

        private void LightConfigForm_Load(object sender, EventArgs e)
        {
            if (lightCtrlInfo != null)
            {
                LoadInfo();
            }
            else
            {
                txtName.Text = lightCtrlName;
                numLight.Value = 1;
                useIoLightCtrl.Checked = true;
            }

            initialized = true;
        }

        private void LoadInfo()
        { 
            if (lightCtrlInfo.Type == LightCtrlType.Serial)
            {
                SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)lightCtrlInfo;

                useSerialLightCtrl.Checked = true;
                checkResponce.Checked = serialLightCtrlInfo.ResponceTimeoutMs > 0;

                comboLightControllerVender.Enabled = true;
                buttonEditLightCtrlPort.Enabled = true;

                comboLightControllerVender.SelectedIndex = (int)serialLightCtrlInfo.ControllerVender;

                serialPortInfo = serialLightCtrlInfo.SerialPortInfo;
                labelSerialPortInfo.Text = serialPortInfo.ToString();
            }
            else if (lightCtrlInfo.Type == LightCtrlType.IO)
            {
                useIoLightCtrl.Checked = true;
            }

            txtName.Text = lightCtrlInfo.Name;
            numLight.Value = lightCtrlInfo.NumChannel;
        }

        private void useIoLightCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (initialized == false)
                return;

            buttonEditLightCtrlPort.Enabled = false;
            comboLightControllerVender.Enabled = false;
            checkResponce.Enabled = false;
        }

        private void buttonEditLightCtrlPort_Click(object sender, EventArgs e)
        {
            SerialPortSettingForm form = new SerialPortSettingForm();
            form.SerialDeviceInfo = new LightSerialDeviceInfo();
            form.SerialDeviceInfo.SerialPortInfo = serialPortInfo.Clone();
            form.EnablePortNo = true;
            form.EditableDevice(false);

            if (form.ShowDialog() == DialogResult.OK)
            {
                serialPortInfo = form.SerialDeviceInfo.SerialPortInfo;
                labelSerialPortInfo.Text = serialPortInfo.ToString();
            }
        }

        private LightCtrlInfo CreateLightCtrlInfo()
        {
            LightCtrlInfo lightCtrlInfo = null;
            if (useIoLightCtrl.Checked)
                lightCtrlInfo = LightCtrlInfoFactory.Create(LightCtrlType.IO);
            else if (useSerialLightCtrl.Checked)
            {
                SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)LightCtrlInfoFactory.Create(LightCtrlType.Serial);

                serialLightCtrlInfo.ResponceTimeoutMs = this.checkResponce.Checked ? 200 : 0;
                serialLightCtrlInfo.ControllerVender = (LightControllerVender)comboLightControllerVender.SelectedIndex;
                serialLightCtrlInfo.SerialPortInfo = serialPortInfo;

                lightCtrlInfo = serialLightCtrlInfo;
            }

            if (lightCtrlInfo != null)
            {
                lightCtrlInfo.Name = txtName.Text;
                lightCtrlInfo.NumChannel = (int)numLight.Value;
            }

            return lightCtrlInfo;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            lightCtrlInfo = CreateLightCtrlInfo();
        }

        private void useSerialLightCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if (initialized == false)
                return;

            buttonEditLightCtrlPort.Enabled = true;
            comboLightControllerVender.Enabled = true;
            checkResponce.Enabled = true;
        }

        private void buttonTestLightController_Click(object sender, EventArgs e)
        {
            LightCtrlInfo lightCtrlInfo = CreateLightCtrlInfo();
            LightCtrl lightCtrl = LightCtrlFactory.Create(lightCtrlInfo, digitalIoHandler, false);
            if (lightCtrl == null)
                return;

            LightValue lightValue = new LightValue(lightCtrlInfo.NumChannel, lightCtrl.GetMaxLightLevel());
            lightCtrl.TurnOn(lightValue);

            MessageBox.Show("Please, check the light");

            lightCtrl.TurnOff();

            lightCtrl.Release();
        }
    }
}
