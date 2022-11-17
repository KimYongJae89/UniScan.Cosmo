using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Base;
using DynMvp.Device.Serial;

namespace DynMvp.Devices.UI
{
    public partial class SerialPortSettingForm : Form
    {
        private SerialDeviceInfo serialDeviceInfo;
        public SerialDeviceInfo SerialDeviceInfo
        {
            get { return serialDeviceInfo; }
            set { serialDeviceInfo = value; }
        }

        bool enablePortNo = false;
        public bool EnablePortNo
        {
            get { return enablePortNo; }
            set { enablePortNo = value; }
        }

        string portName = "";
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }
        
        public SerialPortSettingForm()
        {
            InitializeComponent();

            comboDeviceType.Items.AddRange(Enum.GetNames(typeof(ESerialDeviceType)));
            groupBoxProperty.Text = StringManager.GetString(this.GetType().FullName,groupBoxProperty.Text);
            labelPortNo.Text = StringManager.GetString(this.GetType().FullName,labelPortNo.Text);
            labelBaudRate.Text = StringManager.GetString(this.GetType().FullName,labelBaudRate.Text);
            labelDataBits.Text = StringManager.GetString(this.GetType().FullName,labelDataBits.Text);
            labelParity.Text = StringManager.GetString(this.GetType().FullName,labelParity.Text);
            labelStopBits.Text = StringManager.GetString(this.GetType().FullName,labelStopBits.Text);
            labelHandshake.Text = StringManager.GetString(this.GetType().FullName,labelHandshake.Text);
            checkBoxRtsEnable.Text = StringManager.GetString(this.GetType().FullName,checkBoxRtsEnable.Text);
            checkBoxDtrEnable.Text = StringManager.GetString(this.GetType().FullName,checkBoxDtrEnable.Text);
            btnOK.Text = StringManager.GetString(this.GetType().FullName,btnOK.Text);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName,btnCancel.Text);
        }

        private void SerialPortSettingForm_Load(object sender, EventArgs e)
        {
            textBoxName.Text = serialDeviceInfo.DeviceName;
            comboDeviceType.SelectedItem = serialDeviceInfo.DeviceType.ToString();

            SerialPortManager.FillComboAllPort(comboPortName);
            if (string.IsNullOrEmpty(PortName))
                comboPortName.Text = serialDeviceInfo.SerialPortInfo.PortName;
            else
            {
                comboPortName.Text = portName;
                serialDeviceInfo.SerialPortInfo.PortName = comboPortName.Text;
            }

            //comboPortName.Enabled = enablePortNo;

            baudRate.Text = serialDeviceInfo.SerialPortInfo.BaudRate.ToString();
            parity.Text = serialDeviceInfo.SerialPortInfo.Parity.ToString();

            StopBits[] stopBitsValues = new StopBits[] { StopBits.One, StopBits.OnePointFive, StopBits.Two };
            stopBits.SelectedIndex = stopBitsValues.ToList().IndexOf(serialDeviceInfo.SerialPortInfo.StopBits);

            dataBits.Text = serialDeviceInfo.SerialPortInfo.DataBits.ToString();

            Handshake[] handShakes = new Handshake[] { Handshake.None, Handshake.RequestToSend, Handshake.RequestToSendXOnXOff, Handshake.XOnXOff };
            comboBoxHandshake.SelectedIndex = handShakes.ToList().IndexOf(serialDeviceInfo.SerialPortInfo.Handshake);

            checkBoxRtsEnable.Checked = serialDeviceInfo.SerialPortInfo.RtsEnable;
            checkBoxDtrEnable.Checked = serialDeviceInfo.SerialPortInfo.DtrEnable;
        }

        public void EditableDevice(bool editable)
        {
            panel2.Visible = editable;
        }

        private void comboDeviceType_SelectedValueChanged(object sender, EventArgs e)
        {
            textBoxName.Text = comboDeviceType.SelectedItem.ToString();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            serialDeviceInfo.DeviceName = textBoxName.Text;
            serialDeviceInfo.DeviceType = (ESerialDeviceType)comboDeviceType.SelectedIndex;
            serialDeviceInfo.SerialPortInfo.PortName = comboPortName.Text;
            serialDeviceInfo.SerialPortInfo.BaudRate = Convert.ToInt32(baudRate.Text);
            serialDeviceInfo.SerialPortInfo.Parity = (Parity)Enum.Parse(typeof(Parity), parity.Text);
            serialDeviceInfo.SerialPortInfo.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBits.Text);
            serialDeviceInfo.SerialPortInfo.DataBits = Convert.ToInt32(dataBits.Text);

            StopBits[] stopBitsValues = new StopBits[] { StopBits.One, StopBits.OnePointFive, StopBits.Two };
            serialDeviceInfo.SerialPortInfo.StopBits = stopBitsValues[stopBits.SelectedIndex];

            Handshake[] handShakes = new Handshake[] { Handshake.None, Handshake.RequestToSend, Handshake.RequestToSendXOnXOff, Handshake.XOnXOff };
            serialDeviceInfo.SerialPortInfo.Handshake = handShakes[comboBoxHandshake.SelectedIndex];

            serialDeviceInfo.SerialPortInfo.RtsEnable = checkBoxRtsEnable.Checked;
            serialDeviceInfo.SerialPortInfo.DtrEnable = checkBoxDtrEnable.Checked;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
