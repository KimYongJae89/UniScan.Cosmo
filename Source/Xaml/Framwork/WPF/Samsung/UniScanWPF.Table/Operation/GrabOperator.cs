using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Device;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation
{
    public abstract class GrabOperator : MachineOperator
    {
        protected const string axisName = "Y";
        protected const int offset = 45;

        protected IoPort grabPort;
        protected ImageDeviceHandler imageDeviceHandler;

        public GrabOperator() : base()
        {
            if (SystemManager.Instance().DeviceBox.PortMap != null)
            {
                PortMap portMap = (PortMap)SystemManager.Instance().DeviceBox.PortMap;
                grabPort = portMap.GetOutPort(PortMap.OutPortName.OutGrab);
                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(grabPort, false);
            }

            imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
        }

        public override void Release()
        {
            foreach (ImageDevice device in imageDeviceHandler)
                device.ImageGrabbed = null;

            base.Release();
        }

        public virtual bool Start()
        {
            SystemManager.Instance().DeviceController.RobotStage.EndCmp(axisName);

            foreach (ImageDevice device in imageDeviceHandler)
                device.ImageGrabbed = ImageGrabbed;

            //SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();

            this.OperatorState = OperatorState.Run;
            
            return PostGrab() && PrepereGrab();
        }

        protected abstract bool PrepereGrab();
        protected abstract bool PostGrab();
        protected abstract void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr);
    }
    
    public abstract class GrabOperatorSettings : MachineOperatorSettings
    {
        public override void Load(XmlElement xmlElement)
        {
            velocity = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "Velocity", "150000"));
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Velocity", velocity.ToString());
        }
    }
}
