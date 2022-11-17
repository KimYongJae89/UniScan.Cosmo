using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using Standard.DynMvp.Base;

namespace Standard.DynMvp.Devices.ImageDevices.Virtual
{
    public class GrabberVirtual : Grabber
    {
        public GrabberVirtual(GrabberInfo grabberInfo) : base(grabberInfo)
        {

        }

        protected override Camera CreateCamera(CameraInfo cameraInfo)
        {
            return new CameraVirtual(cameraInfo);
        }

        protected override bool Initialize()
        {
            return true;
        }

        protected override void Release()
        {

        }
    }
}
