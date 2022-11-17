using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Drawing.Imaging;

using DynMvp.Base;
using System.Collections;

namespace DynMvp.Devices.FrameGrabber
{
    public enum BayerType
    {
        GB, BG, RG, GR
    }
    
    public enum ScanMode
    {
        Area, Line
    }

    public class CameraSpec 
    {
        private int imageWidth;
        public int ImageWidth
        {
            get { return imageWidth; }
            set { imageWidth = value; }
        }

        private int imageHeight;
        public int ImageHeight
        {
            get { return imageHeight; }
            set { imageHeight = value; }
        }

        private int imageDepth;
        public int ImageDepth
        {
            get { return imageDepth; }
            set { imageDepth = value; }
        }
    }

    public enum CameraType
    {
        Jai_GO_5000, PrimeTech_PXCB120VTH, Crevis_MC_D500B, PrimeTech_PXCB16QWTPM, PrimeTech_PXCB16QWTPMCOMPACT, HV_B550CTRG1, HV_B550CTRG2, RaL12288_66km, ELIIXAp_16kCL_L16384RP
    }

    public class CameraInitializeFailedException : ApplicationException
    {
    }

    public delegate IntPtr BufferAllocatorDelegate(int size);
    public abstract class Camera : ImageDevice
    {
        public BufferAllocatorDelegate BufferAllocator = null;

        protected CameraInfo cameraInfo;
        public CameraInfo CameraInfo
        {
            get { return cameraInfo; }
            set { cameraInfo = value; }
        }

        private SizeF fovSize;
        public SizeF FovSize
        {
            get { return fovSize; }
            set { fovSize = value; }
        }

        protected int numOfBand;
        public int NumOfBand
        {
            get { return numOfBand; }
            set { numOfBand = value; }
        }

        private bool bayerCamera;
        public bool BayerCamera
        {
            get { return bayerCamera; }
            set { bayerCamera = value; }
        }

        BayerType bayerType;
        public BayerType BayerType
        {
            get { return bayerType; }
            set { bayerType = value; }
        }

        bool useNativeBuffering;
        public bool UseNativeBuffering
        {
            get { return useNativeBuffering; }
            set { useNativeBuffering = value; }
        }
        
        private object grabCountLockObj = new object();
        protected int grabCount = 0;
        protected Stopwatch grabTimer = new Stopwatch();

        /// <summary>
        /// What is it??
        /// </summary>
        protected ManualResetEvent exposureDoneEvent = new ManualResetEvent(true);

        /// <summary>
        /// for GrabDone() check
        /// </summary>
        protected ManualResetEvent isGrabbed = new ManualResetEvent(false);
        protected ManualResetEvent isGrabDone = new ManualResetEvent(false);
        protected ManualResetEvent isStopped = new ManualResetEvent(true);

        /// <summary>
        /// for ImageGrabbed() done check
        /// </summary>
        //protected ManualResetEvent imageGrabbedDoneEvent = new ManualResetEvent(true);

        protected float exposureTimeUs = 0;

        protected RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;
        public RotateFlipType RotateFlipType
        {
            get { return rotateFlipType; }
            set { rotateFlipType = value; }
        }

        public override bool IsVirtual => false;
        Mutex grabCallBackMutex = new Mutex();

        public Camera()
        {
            Name = "Camera";

            DeviceType = DeviceType.Camera;
            UpdateState(DeviceState.Idle);

            ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmStatus;
        }

        public override bool SetStepLight(int stepNo, int lightNo) { return true; }

        //public virtual bool SetFreeMode(float grabHz) { return true; }
        //public virtual float GetGrabHz() { return float.NaN; }
        public abstract void SetScanMode(ScanMode scanMode);
        
        public virtual void Initialize(CameraInfo cameraInfo)
        {
            this.cameraInfo = cameraInfo;
            Index = cameraInfo.Index;
            Enabled = cameraInfo.Enabled;
            rotateFlipType = cameraInfo.RotateFlipType;

            if (cameraInfo.UseNativeBuffering == true)
                SetupNativeBuffering();
        }

        public void UpdateFovSize(SizeF pelSize)
        {
            fovSize.Width = ImageSize.Width * pelSize.Width;
            fovSize.Height = ImageSize.Height * pelSize.Height;
        }

        // 각 Grabber별로 제공되는 버퍼링 기능을 이용하여 Grab 속도를 올리고자 할 때 사용
        // 세부 구현은 각 그래버별로 구현해야 함.
        public virtual void SetupNativeBuffering()
        {
            useNativeBuffering = true;
        }

        public override bool IsOnLive()
        {
            return grabCount == CONTINUOUS;
        }

        public override bool IsCompatibleImage(ImageD image)
        {
            //LogHelper.Debug(LoggerType.Grab, "Camera - IsCompatibleBitmap");

            if (image == null)
                return false;

            return (image.NumBand == numOfBand && image.DataSize == 1 && IsCompatibleSize(new Size(image.Width, image.Height)));
        }

        protected bool IsCompatibleSize(Size bitMapSize)
        {
            if (Is90Or270Rotated() == false)
                return bitMapSize.Width == ImageSize.Width && bitMapSize.Height == ImageSize.Height;
            else
                return bitMapSize.Width == ImageSize.Height && bitMapSize.Height == ImageSize.Width;
        }

        public bool Is90Or270Rotated()
        {
            return (rotateFlipType == RotateFlipType.Rotate90FlipNone || rotateFlipType == RotateFlipType.Rotate90FlipX || rotateFlipType == RotateFlipType.Rotate90FlipXY || rotateFlipType == RotateFlipType.Rotate90FlipY
                  || rotateFlipType == RotateFlipType.Rotate270FlipNone || rotateFlipType == RotateFlipType.Rotate270FlipX || rotateFlipType == RotateFlipType.Rotate270FlipXY || rotateFlipType == RotateFlipType.Rotate270FlipY);
        }

        public override ImageD CreateCompatibleImage()
        {
            LogHelper.Debug(LoggerType.Grab, "Camera - CreateCompatibleImage");

            Image2D image2d = new Image2D();
            if (UseNativeBuffering == false)
                image2d.Initialize(ImageSize.Width, ImageSize.Height, (cameraInfo.PixelFormat == PixelFormat.Format8bppIndexed ? 1 : 3), ImagePitch);
            else
                image2d.Initialize(ImageSize.Width, ImageSize.Height, (cameraInfo.PixelFormat == PixelFormat.Format8bppIndexed ? 1 : 3), ImagePitch,null);


            return image2d;
        }

        public override bool IsGrabbed()
        {
            return isGrabbed.WaitOne(0, false);
            //return isGrabbed.WaitOne(0);
        }

        public override bool IsGrabDone()
        {
            return isGrabDone.WaitOne(0, false);
            //return isGrabDone.WaitOne(0);
        }

        public override bool IsStopped()
        {
            return isStopped.WaitOne(0, false);
            //return isStopped.WaitOne(0);
        }

        public override bool WaitGrabDone(int timeoutMs = 0)
        {
            if (timeoutMs == 0)
                timeoutMs = ImageDeviceHandler.DefaultTimeoutMs;

            LogHelper.Debug(LoggerType.Grab, "Camera::WaitGrabDone");
            bool ok = false;

            while (timeoutMs > 10 && ok == false)
            {
                Thread.Sleep(10);
                bool isGrabbed = this.isGrabbed.WaitOne(0);
                bool isGrabDone = this.isGrabDone.WaitOne(0);
                bool isStopped = this.isStopped.WaitOne(0);
                if (isGrabbed)
                {
                    ok = isGrabDone;
                }
                else
                {
                    ok = isStopped;
                }
                timeoutMs -= 10;
            }

            return ok;
        }

        public override void Reset()
        {
            exposureDoneEvent.Set();
            isGrabbed.Reset();
            isStopped.Reset();
            GrabFailed = false;
        }

        public override bool SetExposureTime(float exposureTimeUs)
        {
            if (exposureTimeUs <= 0)
                return false;

            LogHelper.Debug(LoggerType.Grab, string.Format("Camera::SetExposureTime - {0} um", exposureTimeUs));

            bool result = true;
            if (this.exposureTimeUs != exposureTimeUs)
            {
                result = SetDeviceExposure(exposureTimeUs / 1000);
                if (result == true)
                    this.exposureTimeUs = exposureTimeUs;
            }
            return result;
        }
        
        public virtual bool SetupGrab()
        {
            if (Enabled == false)
                return false;

            if (grabFailed)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Grabber, (int)CommonError.InvalidState, 
                    ErrorLevel.Warning, ErrorSection.Grabber.ToString(), CommonError.InvalidState.ToString(), "Grab Fail State");
                return false;
            }

            exposureDoneEvent.Reset();
            isGrabDone.Reset();
            isGrabbed.Reset();
            isStopped.Reset();
            //grabTimer.Reset();
            //grabTimer.Restart();

            LogHelper.Debug(LoggerType.Grab, String.Format("Camera[{0}]::SetupGrab", Index));

            return true;
        }




        public override void Stop()
        {
            isStopped.Set();
        }

        protected void ExposureDoneCallback()
        {
            if (grabCount != CONTINUOUS && grabCount == 1)
            {
                exposureDoneEvent.Set();
            }
        }

        protected void ImageGrabbedCallback(IntPtr ptr)
        {
            LogHelper.Debug(LoggerType.Function, "Camera::ImageGrabbedCallback - Begin");
            isGrabbed.Set();

            exposureDoneEvent.Set();
            //long grabTime = grabTimer.ElapsedMilliseconds;

            lock (grabCountLockObj)
            {
                if (grabCount != CONTINUOUS)
                {
                    grabCount--;
                    if (grabCount < 0)
                        grabCount = 0;

                    if (grabCount == 0)
                    {
                        SetStopFlag();
                    }
                    else
                    {
                    }
                }
            }

            if (ImageGrabbed != null)
            {
                //LogHelper.Debug(LoggerType.Function, String.Format("ImageGrabbed - [{0}] Start. {1}ms", Index, grabTime));
                LogHelper.Debug(LoggerType.Function, String.Format("ImageGrabbed - [{0}] Start", Index));

                ImageGrabbed(this, ptr);

                LogHelper.Debug(LoggerType.Function, String.Format("ImageGrabbed - [{0}] End.", Index));
            }

            ////GC.Collect();
            ////GC.WaitForFullGCComplete(); // 여기서 간헐적으로 무한루프에 빠지는 경우 있음

            //ImageProvider
            isGrabDone.Set();

            LogHelper.Debug(LoggerType.Function, "Camera::ImageGrabbedCallback - End");
        }

        private void ErrorManager_OnResetAlarmStatus()
        {
            this.grabFailed = false;
        }

        public abstract void SetStopFlag();
        public abstract bool SetDeviceExposure(float exposureTimeMs);
        public abstract float GetDeviceExposureMs();

        public virtual void SetImageSize(int width, int height)
        {

        }
    }
}
