using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using System.Runtime.InteropServices;

using Standard.DynMvp.Base;
using System.Diagnostics;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Standard.DynMvp.Devices.ImageDevices.Virtual
{
    public class CameraInfoVirtual : CameraInfo
    {
        public string Directory { get; set; }
        public uint Interval { get; set; }

        public CameraInfoVirtual(string name) : base(name, GrabberType.Virtual)
        {
        }
    }

    public class CameraVirtual : Camera
    {
        Timer callbackTimer = new Timer();
        IEnumerable<string> imagePaths;
        IEnumerator<string> enumerator;

        public CameraVirtual(CameraInfo cameraInfo) : base(cameraInfo)
        {

        }

        protected override void Initialize()
        {
            CameraInfoVirtual info = DeviceInfo as CameraInfoVirtual;

            imagePaths = Directory.GetFiles(info.Directory, " *.*", SearchOption.TopDirectoryOnly)
                .Where(s => s.EndsWith(".bmp") || s.EndsWith(".png") || s.EndsWith(".jpg"));

            enumerator = imagePaths.GetEnumerator();

            callbackTimer.Interval = info.Interval;
            callbackTimer.Elapsed += new ElapsedEventHandler(callbackTimer_Elapsed);
        }

        protected override void Release()
        {
            
        }

        public override void GrabOnce()
        {
            SetupGrab(1);
            enumerator.Reset();

            CameraInfoVirtual info = DeviceInfo as CameraInfoVirtual;
            callbackTimer.Interval = info.Interval;
            callbackTimer.Start();
        }

        public override void GrabMulti(int grabCount = CONTINUOUS)
        {
            SetupGrab(grabCount);
            enumerator.Reset();

            CameraInfoVirtual info = DeviceInfo as CameraInfoVirtual;
            callbackTimer.Interval = info.Interval;
            callbackTimer.Start();
        }

        public override void Stop()
        {
            callbackTimer.Stop();
        }

        async void callbackTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            callbackTimer.Stop();

            IntPtr ptr = await GetPtr(enumerator.Current);

            callbackTimer.Start();

            ImageGrabbDone(ptr);

            enumerator.MoveNext();

            if (enumerator.Current == null)
                enumerator.Reset();
        }

        async Task<IntPtr> GetPtr(string imagePath)
        {
            Image2D image = new Image2D(imagePath);
            image.ConvertFromData();
            return image.DataPtr;
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {

        }
    }
}
