using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Device.Dio.UI
{
    public partial class SlaveDigitalIoInfoForm : Form
    {
        SlaveDigitalIoInfo slaveDigitalIoInfo;
        public SlaveDigitalIoInfo SlaveDigitalIoInfo
        {
            get { return slaveDigitalIoInfo; }
            set { slaveDigitalIoInfo = value;  }
        }

        MotionInfoList motionInfoList;
        public MotionInfoList MotionInfoList
        {
            get { return motionInfoList; }
            set { motionInfoList = value;  }
        }

        public SlaveDigitalIoInfoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(this.GetType().FullName,labelName.Text);
            labelBoardIndex.Text = StringManager.GetString(this.GetType().FullName,labelBoardIndex.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

        }

        private void SlaveMotionInfoForm_Load(object sender, EventArgs e)
        {
            foreach (MotionInfo motionInfo in motionInfoList)
            {
                if (DigitalIoFactory.IsMasterDevice(motionInfo.Type))
                {
                    masterDeviceList.Items.Add(motionInfo.Name);
                }
            }

            txtName.Text = slaveDigitalIoInfo.Name;
            masterDeviceList.Text = slaveDigitalIoInfo.MasterDeviceName;

            numInPortGroup.Value = slaveDigitalIoInfo.NumInPortGroup;
            inPortStartGroupIndex.Value = slaveDigitalIoInfo.InPortStartGroupIndex;
            numInPort.Value = slaveDigitalIoInfo.NumInPort;
            numOutPortGroup.Value = slaveDigitalIoInfo.NumOutPortGroup;
            outPortStartGroupIndex.Value = slaveDigitalIoInfo.OutPortStartGroupIndex;
            numOutPort.Value = slaveDigitalIoInfo.NumOutPort;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            slaveDigitalIoInfo.Name = txtName.Text;
            slaveDigitalIoInfo.MasterDeviceName = masterDeviceList.Text;
            slaveDigitalIoInfo.NumInPortGroup = (int)numInPortGroup.Value;
            slaveDigitalIoInfo.InPortStartGroupIndex = (int)inPortStartGroupIndex.Value;
            slaveDigitalIoInfo.NumInPort = (int)numInPort.Value;
            slaveDigitalIoInfo.NumOutPortGroup = (int)numOutPortGroup.Value;
            slaveDigitalIoInfo.OutPortStartGroupIndex = (int)outPortStartGroupIndex.Value;
            slaveDigitalIoInfo.NumOutPort = (int)numOutPort.Value;
        }
    }
}
