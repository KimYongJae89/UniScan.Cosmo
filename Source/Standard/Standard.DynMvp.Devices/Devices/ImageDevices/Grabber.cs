using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Standard.DynMvp.Base;
using System.Xml;
using System.Collections.Concurrent;
using Standard.DynMvp.Devices.ImageDevices.MultiCam;
using Standard.DynMvp.Devices.ImageDevices.GenTL;
using Standard.DynMvp.Devices.ImageDevices.Virtual;
using Standard.DynMvp.Devices.Helpers;
using Standard.DynMvp.Base.Helpers;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Standard.DynMvp.Devices.ImageDevices
{
    public abstract class Grabber : Device
    {
        IEnumerable<Camera> _cameras;
        public IEnumerable<Camera> Cameras { get => _cameras; }

        protected abstract Camera CreateCamera(CameraInfo cameraInfo);
        protected abstract bool Initialize();

        public EventHandler<ImageGrabbedEventArgs> ImageGrabbed
        {
            set
            {
                foreach (var camera in _cameras)
                    camera.ImageGrabbed = value;
            }
        }

        public static Grabber Create(GrabberInfo grabberInfo)
        {
            switch (grabberInfo.GrabberType)
            {
                case GrabberType.MultiCam:
                    return new GrabberMultiCam(grabberInfo);
                case GrabberType.GenTL:
                    return new GrabberGenTL(grabberInfo);
            }

            return new GrabberVirtual(grabberInfo);
        }

        protected Grabber(GrabberInfo grabberInfo) : base(grabberInfo)
        {
            Initialize();

            foreach (var cameraInfo in grabberInfo.CameraInfos)
                _cameras.Concat(new[] { CreateCamera(cameraInfo) });
        }

        public void GrabOnce()
        {
            foreach (var camera in _cameras)
                camera.GrabOnce();
        }

        public void GrabMulti()
        {
            foreach (var camera in _cameras)
                camera.GrabMulti();
        }

        public void Stop()
        {
            foreach (var camera in _cameras)
                camera.Stop();
        }
    }
}
