using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Euresys.MultiCam;
using Standard.DynMvp.Base;
using Standard.DynMvp.Base.Helpers;

namespace Standard.DynMvp.Devices.ImageDevices.MultiCam
{
    public class GrabberMultiCam : Grabber
    {
        public GrabberMultiCam(GrabberInfo grabberInfo) : base(grabberInfo)
        {
        }

        protected override Camera CreateCamera(CameraInfo cameraInfo)
        {
            return new CameraMultiCam(cameraInfo);
        }
        
        protected override bool Initialize()
        {
            var grabberInfo = DeviceInfo as GrabberInfo;

            MC.OpenDriver();
            MC.SetParam(MC.CONFIGURATION, "ErrorLog", "error.log");

            return true;
        }

        protected override void Release()
        {
            MC.CloseDriver();
        }
    }
}
