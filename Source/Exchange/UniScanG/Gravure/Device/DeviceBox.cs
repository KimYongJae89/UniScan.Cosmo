using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Device;

namespace UniScanG.Gravure.Device
{

    public class DeviceBox : UniEye.Base.Device.DeviceBox
    {
        enum VirtualCameraType { Area, Line }
        public DeviceBox(UniEye.Base.Device.PortMap portMap) : base(portMap)
        {
        }

        public override Camera CreateCamera(Grabber grabber)
        {
            VirtualCameraType virtualCameraType = VirtualCameraType.Area;
            switch (grabber.Type)
            {
                case DynMvp.Devices.FrameGrabber.GrabberType.GenTL:
                    return new CameraGenTL();
                case DynMvp.Devices.FrameGrabber.GrabberType.Virtual:
                    {
                        string virtualSourceImageNameFilter = string.Format("Image_C{0:00}_*.bmp", SystemManager.Instance().ExchangeOperator.GetCamIndex());
                        Camera camera = null;
                        switch (virtualCameraType)
                        {
                            case VirtualCameraType.Area:
                                camera = new CameraVirtual() { VirtualSourceImageNameFilter = virtualSourceImageNameFilter };
                                break;
                            case VirtualCameraType.Line:
                                camera = new CameraVirtualMS() { VirtualSourceImageNameFilter = virtualSourceImageNameFilter };
                                break;
                        }
                        camera.SetScanMode(ScanMode.Line);
                        return camera;
                    }
                default:
                    return base.CreateCamera(grabber);
            }
        }
    }
}
