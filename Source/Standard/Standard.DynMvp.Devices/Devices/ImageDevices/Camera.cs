using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;

using Standard.DynMvp.Base;
using System.Collections;
using System.ComponentModel;
using Standard.DynMvp.Devices.ImageDevices.Virtual;
using Standard.DynMvp.Devices.ImageDevices.MultiCam;
using Standard.DynMvp.Devices.ImageDevices.GenTL;
using Standard.DynMvp.Base.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Standard.DynMvp.Devices.ImageDevices;

namespace Standard.DynMvp.Devices.ImageDevices
{
    public abstract class Camera : ImageDevice
    {
        protected int GrabCount { get; set; } = CONTINUOUS;
        protected int CurGrabCount { get; set; } = 0;

        protected abstract void Initialize();

        public abstract void GrabOnce();
        public abstract void GrabMulti(int grabCount = CONTINUOUS);
        public abstract void Stop();

        public abstract void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType);

        protected Camera(CameraInfo cameraInfo) : base(cameraInfo)
        {
            Initialize();
        }

        protected void SetupGrab(int grabCount)
        {
            CurGrabCount = 0;
            GrabCount = 1;
        }

        protected void ImageGrabbDone(IntPtr intPtr)
        {
            CurGrabCount++;
            if (GrabCount <= CurGrabCount)
                Stop();

            ImageGrabbed.Invoke(this, new ImageGrabbedEventArgs(intPtr));
        }
    }
}
