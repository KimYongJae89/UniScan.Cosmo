using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DynMvp.Devices
{
    public delegate void ImageDeviceEventDelegate(ImageDevice imageDevice, IntPtr ptr);

    public enum TriggerMode
    {
        Software, Hardware
    }
    public enum TriggerType
    {
        RisingEdge, FallingEdge
    }

    public abstract class ImageDevice : Device
    {
        public const int CONTINUOUS = -1;

        ImageDeviceEventDelegate imageGrabbed = null;
        public ImageDeviceEventDelegate ImageGrabbed
        {
            get { return imageGrabbed; }
            set { imageGrabbed = value; }
        }

        public ImageDeviceEventDelegate exposureDone = null;
        public ImageDeviceEventDelegate ExposureDone
        {
            get { return exposureDone; }
            set { exposureDone = value; }
        }

        private int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        private Size imageSize;
        public Size ImageSize
        {
            get { return imageSize; }
            set { imageSize = value; }
        }
        
        int imagePitch;
        public int ImagePitch
        {
            get { return imagePitch; }
            set { imagePitch = value; }
        }

        protected TriggerMode triggerMode;
        public TriggerMode TriggerMode
        {
            get { return triggerMode; }
        }

        protected int triggerChannel = 0;
        public int TriggerChannel
        {
            get { return triggerChannel; }
            set { triggerChannel = value; }
        }

        protected TriggerType triggerType;
        public TriggerType TriggerType
        {
            get { return triggerType; }
        }

        protected bool grabFailed = false;
        public bool GrabFailed
        {
            get { return grabFailed; }
            set { grabFailed = value; }
        }

        public abstract bool IsVirtual { get; }

        public int TestCounter = 0;

        public override string ToString()
        {
            if(this.Name== null)
                return base.ToString();
            return string.Format("[{0}]{1}", this.index, this.Name);
        }

        // Image
        public abstract bool IsCompatibleImage(ImageD image);
        public abstract ImageD CreateCompatibleImage();
        public abstract ImageD GetGrabbedImage(IntPtr ptr);
        public virtual List<ImageD> GetGrabbedImageList()
        {
            return new List<ImageD>() { GetGrabbedImage(IntPtr.Zero) };
        }

        public virtual bool IsBinningVirtical()
        {
            return false;
        }

        public abstract List<ImageD> GetImageBufferList();

        public virtual bool IsDepthScanner()
        {
            return false;
        }

        public virtual void Grab2D()
        {

        }

        public virtual void Grab3D()
        {

        }

        public virtual Image3D Calculate(Rectangle rectangle, float pixelRes)
        {
            return null;
        }

        public virtual Image3D Calculate(Rectangle rectangle, TransformDataList transformDataList, float pixelRes)
        {
            return null;
        }

        public virtual bool IsOnLive()
        {
            return false;
        }

        // Camera
        public virtual void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            this.triggerMode = triggerMode;
            this.triggerType = triggerType;
        }

        public abstract void SetTriggerDelay(int triggerDelayUs);
        public abstract bool SetExposureTime(float exposureTimeUs);
        public abstract bool SetAcquisitionLineRate(float hz);
        public abstract float GetAcquisitionLineRate();

        public virtual bool SetExposureTime3d(float exposureTime3dUs)
        {
            return true;
        }

        public virtual bool WaitGrabDone(int timeoutMs = 0)
        {
            throw new NotImplementedException();
            //Thread.Sleep(10);

            //if (timeoutMs == 0)
            //    timeoutMs = ImageDeviceHandler.DefaultTimeoutMs;

            //LogHelper.Debug(LoggerType.Grab, "Wait Grab Done");

            //for (int i = 0; timeoutMs < 0 || i < timeoutMs / 10; i++)
            //{
            //    if (IsGrabDone())
            //    {
            //        LogHelper.Debug(LoggerType.Grab, "Success to Wait Grab Done");

            //        return true;
            //    }

            //    Thread.Sleep(10);
            //}

            //LogHelper.Debug(LoggerType.Grab, "Fail to Wait Grab Done");

            ////if (IsGrabDone() == false)
            //{
            //    grabFailed = true;

            //    ErrorManager.Instance().Report((int)ErrorSection.Grabber, (int)CommonError.InvalidState,
            //        ErrorLevel.Fatal, ErrorSection.Grabber.ToString(), CommonError.InvalidState.ToString(), "WaitGrabDone Timeout");
            //}

            //return false;
        }

        /// <summary>
        /// 동기 Grab 동작을 수행한다.
        /// </summary>
        public abstract void GrabOnce();
        /// <summary>
        /// 지정한 개수만큼의 영상을 얻어 온다.
        /// </summary>
        public abstract void GrabMulti(int grabCount = CONTINUOUS);

        public abstract void Stop();
        public abstract void Reset();
        public abstract bool IsGrabbed();
        public abstract bool IsGrabDone();
        public abstract bool IsStopped();
        public abstract bool SetStepLight(int stepNo, int lightNo);
    }

    public class ImageDeviceHandler
    {
        static int defaultTimeoutMs = 5000;
        public static int DefaultTimeoutMs
        {
            get { return defaultTimeoutMs; }
            set { defaultTimeoutMs = value; }
        }

        public ImageDevice this[int i]
        {
            get { return imageDeviceList[i]; }
        }

        protected List<ImageDevice> imageDeviceList = new List<ImageDevice>();
        public List<ImageDevice> ImageDeviceList => imageDeviceList;

        public IEnumerator<ImageDevice> GetEnumerator()
        {
            return imageDeviceList.GetEnumerator();
        }

        public int Count
        {
            get { return imageDeviceList.Count; }
        }

        public bool IsVirtual { get => this.imageDeviceList.All(f => f.IsVirtual); }

        public void AddCamera(Camera camera, CameraInfo cameraInfo)
        {
            bool cameraInitializeFailed = false;

            LogHelper.Debug(LoggerType.StartUp, String.Format("Initialize camera [{0}]", cameraInfo.Index));

            if (cameraInfo.Enabled == false)
                return ;

            //try
            //{
                camera.Initialize(cameraInfo);

                DeviceManager.Instance().AddDevice(camera);
            //}
            //catch (CameraInitializeFailedException)
            //{
            //    LogHelper.Debug(LoggerType.StartUp, String.Format("Can't find device. Virtual camera[{0}] opened.", cameraInfo.Index));

            //    //camera = null;
            //    //cameraInfo.Enabled = false;
            //    //cameraInitializeFailed = true;
            //    return ;
            //}
            //catch (Exception ex)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendLine("Exception in ImageDeviceHandler::AddCamera");
            //    sb.AppendLine(ex.TargetSite.Name);
            //    sb.AppendLine(ex.GetType().ToString());
            //    sb.AppendLine(ex.Message);
            //    sb.AppendLine(ex.StackTrace);
            //    string message = sb.ToString();
            //    MessageBox.Show(message);

            //    return ;
            //}


            if (camera == null && (cameraInitializeFailed == true || cameraInfo.GrabberType == GrabberType.Virtual || cameraInfo.Enabled == false))
            {
                camera = new CameraVirtual();
                camera.Initialize(cameraInfo);

                LogHelper.Debug(LoggerType.StartUp, String.Format("Virtual camera[{0}] opened", cameraInfo.Index));
            }

            if (camera != null)
            {
                AddImageDevice(camera);
                camera.Name = String.Format("Camera {0}", cameraInfo.Index);
            }
            return ;
        }

        public void AddCamera(Grabber grabber, CameraConfiguration cameraConfiguration)
        {
            int index = 1;
            foreach (CameraInfo cameraInfo in cameraConfiguration)
            {
                bool cameraInitializeFailed = false;

                Camera camera = null;

                LogHelper.Debug(LoggerType.StartUp, String.Format("Initialize camera [{0}]", cameraInfo.Index));

                if (cameraInfo.Enabled == true)
                {
                    camera = grabber.CreateCamera();
                    if (camera != null)
                    {
                        grabber.UpdateCameraInfo(cameraInfo);

#if Debug
                        try
                        {
#endif
                            camera.Initialize(cameraInfo);

                            DeviceManager.Instance().AddDevice(camera);
#if Debug
                        }
                        catch (CameraInitializeFailedException)
                        {
                            LogHelper.Debug(LoggerType.StartUp, String.Format("Can't find device. Virtual camera[{0}] opened.", cameraInfo.Index));

                            camera = null;
                            cameraInfo.Enabled = false;
                            cameraInitializeFailed = true;
                        }
#endif
                    }
                }

                if (camera == null && (cameraInitializeFailed == true || cameraInfo.GrabberType == GrabberType.Virtual || cameraInfo.Enabled == false))
                {
                    camera = new CameraVirtual();
                    camera.Initialize(cameraInfo);

                    LogHelper.Debug(LoggerType.StartUp, String.Format("Virtual camera[{0}] opened", cameraInfo.Index));
                }

                if (camera != null)
                {
                    AddImageDevice(camera);
                    camera.Name = String.Format("Camera {0}", index);
                    index++;
                }
            }
        }

        public void AddImageDevice(ImageDevice imageDevice)
        {
            imageDevice.Index = imageDeviceList.Count;
            imageDeviceList.Add(imageDevice);
        }

        public void AddImageDevice(ImageDeviceHandler imageDeviceHandler)
        {
            foreach(ImageDevice imageDevice in imageDeviceHandler)
            {
                AddImageDevice(imageDevice);
            }
        }

        public ImageDevice GetImageDevice(int deviceIndex)
        {
            if (deviceIndex >= 0 && deviceIndex < imageDeviceList.Count)
                return imageDeviceList[deviceIndex];

            return null;
        }

        public void AddImageGrabbed(ImageDeviceEventDelegate imageGrabbed)
        {
            this.imageDeviceList.ForEach(f => f.ImageGrabbed += imageGrabbed);
        }

        public void RemoveImageGrabbed(ImageDeviceEventDelegate imageGrabbed)
        {
            this.imageDeviceList.ForEach(f => f.ImageGrabbed -= imageGrabbed);
        }

        public void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            foreach (ImageDevice imageDevice in imageDeviceList)
            {
                imageDevice.SetTriggerMode(triggerMode, triggerType);
            }
        }

        public void SetAquationLineRate(float Hz)
        {
            foreach (ImageDevice imageDevice in imageDeviceList)
                imageDevice.SetAcquisitionLineRate(Hz);
        }

        public void SetTriggerDelay(int triggerDelay)
        {
            foreach (ImageDevice imageDevice in imageDeviceList)
            {
                imageDevice.SetTriggerDelay(triggerDelay);
            }
        }

        public void SetSkipFrame(int skipFrame)
        {
            foreach (ImageDevice imageDevice in imageDeviceList)
            {
                CameraVirtual cv = imageDevice as CameraVirtual;
                if (cv != null)
                    cv.SkipFrame = skipFrame;
            }
        }

        public void GrabOnce(int deviceIndex = -1)
        {
            LogHelper.Debug(LoggerType.Grab, "ImageDeviceHandler::GrabOnce");

            if (deviceIndex != -1)
            {
                imageDeviceList[deviceIndex].GrabOnce();
            }
            else
            {
                foreach (ImageDevice imageDevice in imageDeviceList)
                {
                    imageDevice.GrabOnce();
                }
            }
        }

        public void GrabMulti(int grabCount = ImageDevice.CONTINUOUS, int deviceIndex = -1)
        {
            if (deviceIndex != -1)
            {
                imageDeviceList[deviceIndex].GrabMulti(grabCount);
            }
            else
            {
                foreach (ImageDevice imageDevice in imageDeviceList)
                {
                    imageDevice.GrabMulti(grabCount);
                }
            }
        }

        public void Stop()
        {
            foreach (ImageDevice imageDevice in imageDeviceList)
            {
                imageDevice.Stop();
            }
        }

        /// <summary>
        /// 디바이스 Stop 상태
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        public bool IsStopped(int deviceNo = -1)
        {
            if (deviceNo < 0)
            {
                foreach (ImageDevice imageDevice in imageDeviceList)
                {
                    if (imageDevice.IsStopped())
                        return true;
                }
                return false;
            }
            else
            {
                ImageDevice imageDevice = imageDeviceList[deviceNo];
                return imageDevice.IsStopped();                
            }
        }

        /// <summary>
        /// 이미지 callback 받음. ImageGrabbed 함수는 종료되지 않음.
        /// </summary>
        /// <returns></returns>
        public bool IsGrabbed()
        {
            foreach (ImageDevice imageDevice in imageDeviceList)
            {
                if (imageDevice.IsGrabbed() == false)
                    return false;
            }
            
            return true;
        }

        /// <summary>
        /// ImageGrabbed 함수까지 종료됨.
        /// </summary>
        /// <returns></returns>
        public bool IsGrabDone(int deviceIndex = -1)
        {
            if (deviceIndex != -1)
            {
                if (imageDeviceList[deviceIndex].IsGrabDone() == false)
                    return false;
            }
            else
            {
                foreach (ImageDevice imageDevice in imageDeviceList)
                {
                    if (imageDevice.IsGrabDone() == false)
                        return false;
                }
            }

            return true;
        }

        public bool WaitGrabDone(int timeoutMs = 0)
         {
            if (timeoutMs == 0)
                timeoutMs = ImageDeviceHandler.DefaultTimeoutMs;

            for (int i = 0; i < timeoutMs / 10 || timeoutMs == -1; i++)
            {
                Thread.Sleep(10);
                bool isStopped = IsStopped();
                if (isStopped)
                {
                    if (IsGrabbed() == false)
                        return false;
                }
                if (IsGrabDone())
                    return true;
            }

            foreach (ImageDevice imageDevice in imageDeviceList)
            {
                if (imageDevice.IsGrabDone() == false)
                    imageDevice.GrabFailed = true;
            }

            return false;
        }

        public void BuildImageList(List<ImageD> imageList)
        {
            imageList.Clear();

            foreach (ImageDevice imageDevice in imageDeviceList)
            {
                imageList.Add(imageDevice.CreateCompatibleImage());
            }
        }

        public void GetGrabImage(List<ImageD> imageList)
        {
            if (imageList.Count != imageDeviceList.Count)
                return;

            for (int deviceIndex = 0; deviceIndex < imageDeviceList.Count; deviceIndex++)
            {
                Debug.Assert(imageList[deviceIndex] != null);
                ImageD grabbedImage = imageDeviceList[deviceIndex].GetGrabbedImage(IntPtr.Zero);
                imageList[deviceIndex].CopyFrom(grabbedImage);
            }
        }
        
        public void SetExposureTime(float exposureUs)
        {
            foreach (ImageDevice imageDevice in imageDeviceList)
            {
                imageDevice.SetExposureTime(exposureUs);
            }
        }

        public bool IsGrabFailed()
        {
            foreach (ImageDevice imageDevice in imageDeviceList)
            {
                if (imageDevice.GrabFailed)
                    return true;
            }

            return false;
        }

        public void Reset()
        {
            foreach (ImageDevice imageDevice in imageDeviceList)
            {
                imageDevice.Reset();
            }
        }

        public virtual void Release()
        {
            foreach (ImageDevice imageDevice in this)
            {
                if(imageDevice.IsReady())
                    imageDevice.Release();
            }
            this.imageDeviceList.Clear();
        }

        public bool IsDepthScannerExist()
        {
            foreach (ImageDevice imageDevice in imageDeviceList)
            {
                if (imageDevice.IsDepthScanner())
                    return true;
            }

            return false;
        }

        public bool SetStepLight(int stepNo, int lightNo)
        {
            bool ok = true;
            this.imageDeviceList.ForEach(f => ok &= f.SetStepLight(stepNo, lightNo));
            return ok;
        }
        
        public virtual void SetImageSize(int width, int height)
        {

        }
    }
}
