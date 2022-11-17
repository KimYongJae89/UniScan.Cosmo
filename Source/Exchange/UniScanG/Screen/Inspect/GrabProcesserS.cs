using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.Inspection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniScanG.Inspect;
using UniScanG.Screen.Vision;
using UniScanG.Vision;

namespace UniScanG.Screen.Inspect
{
    public class GrabProcesserS : GrabProcesser
    {
        ImageDevice imageDevice = null;
        public ImageDevice ImageDevice
        {
            get { return imageDevice; }
        }

        List<IntPtr> ptrList = new List<IntPtr>();

        public GrabProcesserS(ImageDevice imageDevice)
        {
            this.imageDevice = imageDevice;
        }

        public override void Dispose()
        {
            this.ptrList.Clear();
        }

        public void AddGrabbedImagePtr(IntPtr ptr)
        {
            lock (ptrList)
            {
                if (ptrList.Find(p => p == ptr) != null)
                    ptrList.Remove(ptr);

                ptrList.Add(ptr);
            }
        }

        public override IntPtr GetGrabbedImagePtr()
        {
            lock (ptrList)
            {
                if (ptrList.Count != 0)
                {
                    IntPtr ptr = ptrList.First();
                    ptrList.Remove(ptr);

                    return ptr;
                }
            }

            return IntPtr.Zero;
        }
        
        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (this.imageDevice != imageDevice)
                return;

            SystemState.Instance().SetInspectState(InspectState.Run);

            Image2D image = (Image2D)imageDevice.GetGrabbedImage(ptr);

            if (ptr == IntPtr.Zero)
                image.ConvertFromData();

            AddGrabbedImagePtr(image.DataPtr);
            if (AlgorithmSetting.Instance().IsFiducial == true)
                Task.Run(() => StartInspectionDelegate(imageDevice, ptr));
        }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
