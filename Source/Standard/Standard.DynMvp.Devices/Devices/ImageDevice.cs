using Standard.DynMvp.Base;
using Standard.DynMvp.Devices.ImageDevices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Standard.DynMvp.Devices
{
    public enum TriggerMode
    {
        Software, Hardware
    }

    public enum TriggerType
    {
        RisingEdge, FallingEdge
    }

    public class ImageGrabbedEventArgs : EventArgs
    {
        IntPtr _intPtr;
        public IntPtr IntPtr { get => _intPtr; }

        public ImageGrabbedEventArgs(IntPtr intPtr)
        {
            _intPtr = intPtr;
        }
    }

    public abstract class ImageDevice : Device
    {
        protected const int CONTINUOUS = -1;

        public EventHandler<ImageGrabbedEventArgs> ImageGrabbed;
        
        protected ImageDevice(DeviceInfo deviceInfo) : base(deviceInfo)
        {

        }
    }
}
