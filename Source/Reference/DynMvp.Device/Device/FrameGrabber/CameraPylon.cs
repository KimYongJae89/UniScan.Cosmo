using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using PylonC.NET;
using PylonC.NETSupportLibrary;

using DynMvp.Base;
using System.Drawing.Imaging;
using DynMvp.Devices.Dio;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Timers;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraPylon : Camera, IDigitalIo
    {
        bool grabStared = false;
        private ImageProvider imageProvider = new ImageProvider();

        List<ImageD> imageBufferList = new List<ImageD>();
        protected System.Timers.Timer restartTimer = new System.Timers.Timer();

        ~CameraPylon()
        {

        }

        string modelName;

        public string GetName() { return Name; }
        public int GetNumInPort() { return 1; }
        public int GetNumOutPort() { return 1; }
        public int GetNumInPortGroup() { return 1; }
        public int GetNumOutPortGroup() { return 1; }
        public int GetInPortStartGroupIndex() { return 0; }
        public int GetOutPortStartGroupIndex() { return 0; }

        public override void Initialize(CameraInfo cameraInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize Pylon Camera");
            
            base.Initialize(cameraInfo);

            CameraInfoPylon cameraInfoPylon = (CameraInfoPylon)cameraInfo;

            modelName = cameraInfoPylon.ModelName;

            LogHelper.Debug(LoggerType.StartUp, String.Format("Open pylon camera - Device Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3}", cameraInfoPylon.DeviceIndex, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo));

            try
            {
                imageProvider.ImageReadyEvent += new ImageProvider.ImageReadyEventHandler(ImageReadyEventCallback);
                imageProvider.DeviceOpenedEvent += new ImageProvider.DeviceOpenedEventHandler(DeviceOpenedEventHandler);
                imageProvider.GrabbingStartedEvent += ImageProvider_GrabbingStartedEvent;
                imageProvider.GrabbingStoppedEvent += ImageProvider_GrabbingStoppedEvent;
                imageProvider.DeviceRemovedEvent += DeviceRemoved;
                imageProvider.GrabErrorEvent += ImageProvider_GrabErrorEvent;

                imageProvider.Open(cameraInfoPylon.DeviceIndex);
                //Pylon.StreamGrabberSetMaxNumBuffer(hGrabber, NUM_BUFFERS);
                Pylon.DeviceSetIntegerFeature(imageProvider.DeviceHandle, "GevSCPSPacketSize", 1500);
                SetupImageFormat(cameraInfoPylon.UpdateDeviceFeature);
                 
                cameraInfo.Width = ImageSize.Width;
                cameraInfo.Height = ImageSize.Height;

                if (NumOfBand == 1)
                    cameraInfo.PixelFormat = PixelFormat.Format8bppIndexed;
                else
                    cameraInfo.PixelFormat = PixelFormat.Format32bppRgb;
            }
            catch (Exception ex)
            {
                ImageSize = new Size (cameraInfo.Width, cameraInfo.Height);

                if (cameraInfo.PixelFormat == PixelFormat.Format8bppIndexed)
                    NumOfBand = 1;
                else
                    NumOfBand = 3;

                MessageBox.Show(String.Format("Can't open camera. Index : {0} / Device User Id : {1} / IP Address : {2}, Serial No : {3} / Message : {4} ", cameraInfoPylon.DeviceIndex, cameraInfoPylon.DeviceUserId, cameraInfoPylon.IpAddress, cameraInfoPylon.SerialNo, imageProvider.GetLastErrorMessage()));
                return;
            }

            PYLON_DEVICE_HANDLE deviceHandle = imageProvider.DeviceHandle;
            Pylon.DeviceFeatureFromString(deviceHandle, "ExposureMode", "Timed");
            Pylon.DeviceFeatureFromString(deviceHandle, "ExposureAuto", "Off");

            if (cameraInfoPylon.UseLineTrigger)
            {
                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSelector", "LineStart");

                switch (cameraInfoPylon.LineTriggerSourceType)
                {
                    case TrigerSourceType.Software:
                        Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "TriggerSoftware");
                        break;
                    case TrigerSourceType.Line1:
                        Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "Line1");
                        break;
                    case TrigerSourceType.Line2:
                        Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "Line2");
                        break;
                    case TrigerSourceType.Line3:
                        Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "Line3");
                        break;
                    case TrigerSourceType.FrequencyConverter:
                        Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "FrequencyConverter");
                        break;
                }

                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "On");
            }

            if (cameraInfoPylon.UseFrameTrigger)
            {
                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSelector", "FrameStart");
                
                switch (cameraInfoPylon.FrameTriggerSourceType)
                {
                    case TrigerSourceType.Software:
                        Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "TriggerSoftware");
                        break;
                    case TrigerSourceType.Line1:
                        Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "Line1");
                        break;
                    case TrigerSourceType.Line2:
                        Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "Line2");
                        break;
                    case TrigerSourceType.Line3:
                        Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "Line3");
                        break;
                    case TrigerSourceType.FrequencyConverter:
                        Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "FrequencyConverter");
                        break;
                }

                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "On");
                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSelector", "LineStart");
            }
        }

        private void ImageProvider_GrabErrorEvent(Exception grabException, string additionalErrorMessage)
        {
            if (IsOnLive())
            {
                restartTimer.Interval = 10;
                restartTimer.Elapsed += new System.Timers.ElapsedEventHandler(restartTimer_Elapsed);
                restartTimer.Start();
                LogHelper.Error(LoggerType.Grab, "Grab Error. Restart Continuous Grab.");
            }
            else
            {
                LogHelper.Error(LoggerType.Grab, string.Format("Grab Error. {0}", grabException.Message));
            }
        }

        private void restartTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (grabStared == false)
            {
                GrabMulti(-1);
                Thread.Sleep(10);
            }
            else
            {
                restartTimer.Stop();
            }
        }

        private void DeviceRemoved()
        {
            
        }

        public bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            return true;
        }

        public void ExcuteTriggerSoftware()
        {
            Pylon.DeviceExecuteCommandFeature(imageProvider.DeviceHandle, "TriggerSoftware");
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType = TriggerType.RisingEdge)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            PYLON_DEVICE_HANDLE deviceHandle = imageProvider.DeviceHandle;

            Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSelector", "FrameStart");

            if (triggerMode == TriggerMode.Software)
            {
                //Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "Off");
                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "On");
                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "Software");
                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerActivation", triggerType.ToString());
            }
            else
            {
                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "On");
                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerSource", "LineIn0");
                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerActivation", triggerType.ToString());
            }
        }

        public void SetTriggerMode(bool on)
        {
            PYLON_DEVICE_HANDLE deviceHandle = imageProvider.DeviceHandle;
            if (on)
                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "On");
            else
                Pylon.DeviceFeatureFromString(deviceHandle, "TriggerMode", "Off");
        }

        public override void SetTriggerDelay(int triggerDelayUs)
        {
            NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("TriggerDelayAbs");
            //GenApi.FloatSetValue(nodeHandle, (double)triggerDelayUs);
        }

        private void SetupImageFormat(bool updateDeviceFeature)
        {
            Size imageSize = new Size(this.cameraInfo.Width, this.cameraInfo.Height);
            if (updateDeviceFeature)
            {
                Pylon.DeviceSetIntegerFeature(imageProvider.DeviceHandle, "Width", imageSize.Width);
                Pylon.DeviceSetIntegerFeature(imageProvider.DeviceHandle, "Height", imageSize.Height);
            }
            else
            {
                imageSize.Width = (int)Pylon.DeviceGetIntegerFeature(imageProvider.DeviceHandle, "Width");
                imageSize.Height = (int)Pylon.DeviceGetIntegerFeature(imageProvider.DeviceHandle, "Height");
            }
            ImageSize = imageSize;

            string imageFormat = Pylon.DeviceFeatureToString(imageProvider.DeviceHandle, "PixelFormat");
            if (imageFormat == "Mono8")
                NumOfBand = 1;
            else
                NumOfBand = 3;

            ImagePitch = (int)imageSize.Width * NumOfBand;

            LogHelper.Debug(LoggerType.Grab, String.Format("Setup Image - W{0} / H{1} / P{2} / F{3}", imageSize.Width, imageSize.Height, ImagePitch, imageFormat));
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraPylon - GetGrabbedImage : " + cameraInfo.Index);
            
            ImageD image;

            lock (imageBufferList)
                image = imageBufferList.Find(imageBuffer => ((Image2D)imageBuffer).ImageData.DataPtr == ptr);
            
            Debug.Assert(image != null);

            return image;
        }

        private void ImageReadyEventCallback()
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("ImageReadyEventCallback {0}", Index));
            this.isGrabbed.Set();

            Image2D image2d = new Image2D();
            ImageProvider.Image pylonImage = imageProvider.GetLatestImage();
            if (pylonImage != null)
            {
                if (cameraInfo.UseNativeBuffering == true)
                {
                    if (pylonImage.Buffer != null)
                    {
                        GCHandle gcHandle = GCHandle.Alloc(pylonImage.Buffer, GCHandleType.Pinned);
                        image2d.Initialize(ImageSize.Width, ImageSize.Height,
                            (CameraInfo.PixelFormat == PixelFormat.Format8bppIndexed ? 1 : 3),
                            ImagePitch, gcHandle.AddrOfPinnedObject());

                        gcHandle.Free();
                    }
                }
                else
                {
                    image2d.Initialize(ImageSize.Width, ImageSize.Height,
                        (CameraInfo.PixelFormat == PixelFormat.Format8bppIndexed ? 1 : 3),
                        ImagePitch, pylonImage.Buffer);
                }

                if (rotateFlipType != RotateFlipType.RotateNoneFlipNone && rotateFlipType != RotateFlipType.RotateNoneFlipY)
                    image2d.RotateFlip(rotateFlipType);
            }

            lock (imageBufferList)
            {
                ImageD oldImage = imageBufferList.Find(imageBuffer => ((Image2D)imageBuffer).ImageData.DataPtr == image2d.DataPtr);
                if (oldImage == null)
                    imageBufferList.Add(image2d);
            }

            ImageGrabbedCallback(image2d.DataPtr);
        }

        public override void Release()
        {
            base.Release();

            imageProvider.ReleaseImage();
            imageProvider.Close();
        }

        public override bool IsGrabDone()
        {
            return imageProvider.m_grabThread == null ? false : !imageProvider.m_grabThread.IsAlive;
        }

        public override bool SetupGrab()
        {
            lock (imageBufferList)
                imageBufferList.Clear();

            return base.SetupGrab();
        }

        public override void GrabOnce()
        {
            if (SetupGrab() == false)
            {
                MessageBox.Show("Setup Grab is Failed");
                return;
            }

            this.grabCount = 1;

            LogHelper.Debug(LoggerType.Grab, String.Format("Single Shot {0}", Index));

            try
            {
                imageProvider.OneShot();
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, String.Format("Single Shot Error : {0} \n {1}", e.Message, imageProvider.GetLastErrorMessage()));
            }
        }

        public override void GrabMulti(int grabCount)
        {
            if (SetupGrab() == false)
               return;

            this.grabCount = grabCount;

            LogHelper.Debug(LoggerType.Grab, String.Format("Start Continuous {0}", Index));

            grabStared = false;

            try
            {
                imageProvider.ContinuousShot();

                //while (grabStared == false) // 
                //{
                //    Thread.Sleep(0);
                //}
                LogHelper.Debug(LoggerType.Grab, String.Format("GrabMulti - grabStared"));

            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Grab, String.Format("StartContinuous Error : {0} \n {1}", e.Message, imageProvider.GetLastErrorMessage()));
            }
        }

        private void ImageProvider_GrabbingStartedEvent()
        {
            grabStared = true;
        }

        private void ImageProvider_GrabbingStoppedEvent()
        {
            grabStared = false;
        }

        public override void SetStopFlag()
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("Stop Continuous {0}", Index));
            Stop();
        }

        public override void Stop()
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("Stop Continuous {0}", Index));
            //imageProvider.CleanUpGrab();
            imageProvider.Stop();
            base.Stop();
        }

        private void DeviceOpenedEventHandler()
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("Device Is Opened {0}", Index));
        }

        public override void SetImageSize(int width, int height)
        {
            //NODE_HANDLE widthNodeHandle = imageProvider.GetNodeFromDevice("Width");
            //NODE_HANDLE heightNodeHandle = imageProvider.GetNodeFromDevice("Height");
            try
            {
                LogHelper.Debug(LoggerType.Grab, String.Format("Change Size {0} - Width : {1} / Height : {2}", Index, width, height));
                //GenApi.IntegerSetValue(widthNodeHandle, width);
                //GenApi.IntegerSetValue(heightNodeHandle, height);
                this.cameraInfo.Width = width;
                this.cameraInfo.Height = height;
                SetupImageFormat(true);
            }
            catch
            {
            }
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            float exposureTimeUs = exposureTimeMs * 1000;
            NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("ExposureTimeRaw");
            try
            {
                LogHelper.Debug(LoggerType.Grab, String.Format("Change Exposure {0} - {1}", Index, exposureTimeUs));

//                if (modelName.Contains("acA2500") == true)
                    GenApi.IntegerSetValue(nodeHandle, (int)(exposureTimeUs / 35) * 35);
                //else
                //    GenApi.IntegerSetValue(nodeHandle, (int)exposureTimeUs);
            }
            catch(Exception ex)
            {
                LogHelper.Debug(LoggerType.Error,string.Format("CameraPylon::SetDeviceExposure - {0}", ex.Message));
                return false;
            }

            return true;
        }

        public bool IsReady()
        {
            return true;
        }

        public void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("UserOutputValue");

            bool value = (outputPortStatus & 0x1) == 1;
            LogHelper.Debug(LoggerType.Grab, String.Format("User Output Value {0} - {1}", 0, value));
            GenApi.BooleanSetValue(nodeHandle, value);
        }

        public uint ReadOutputGroup(int groupNo)
        {
            NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("UserOutputValue");

            bool value = GenApi.BooleanGetValue(nodeHandle);
            if (value == true)
                return 1;

            return 0;
        }

        public uint ReadInputGroup(int groupNo)
        {
            return 0;
        }

        public void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public void WriteOutputPort(int groupNo, int portNo, bool value)
        {
            throw new NotImplementedException();
        }
        
        public override List<ImageD> GetImageBufferList()
        {
            return imageBufferList;
        }

        public override float GetDeviceExposureMs()
        {
             NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("ExposureTimeAbs");
            double value = GenApi.FloatGetValue(nodeHandle); //us 단위 읽기
            return (float)(value/1000.0);

        }
        public override bool SetExposureTime(float exposureTimeUs)
        {
            if (exposureTimeUs <= 0) return false;
            double exp_us = exposureTimeUs;
            NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("ExposureTimeAbs");
            GenApi.FloatSetValue(nodeHandle, exp_us); //us 단위 쓰기
            return true;
        }


        public override float GetAcquisitionLineRate()
        {
            //NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("AcquisitionLineRateAbs"); //그저 설정한 값.. 실제 설정대로 작동되지 않을수도 있음.
            NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("ResultingLineRateAbs"); //실제 카메라가 그랩할수 있는 성능
            float hz = (float)GenApi.FloatGetValue(nodeHandle);
            return hz;
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            if (hz <= 0) return false;

            LogHelper.Debug(LoggerType.Grab, String.Format("CameraPylon::SetAcquisitionLineRate {0:F3}kHz", hz / 1000f));
            try
            {
                NODE_HANDLE nodeHandle = imageProvider.GetNodeFromDevice("AcquisitionLineRateAbs");
                GenApi.FloatSetValue(nodeHandle, hz);
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Error, string.Format("CameraPylon::SetDeviceExposure - {0}", ex.Message));
                return false;
            }
            return true;
        }

        public override void SetScanMode(ScanMode scanMode)
        {
        }

        public uint ReadOutputGroup(int groupNo, int portNo)
        {
            throw new NotImplementedException();
        }

        public uint ReadInputGroup(int groupNo, int portNo)
        {
            throw new NotImplementedException();
        }

        public void Reverse(bool reverseX, bool reverseY)
        {
            Pylon.DeviceSetBooleanFeature(imageProvider.DeviceHandle, "ReverseX", reverseX);
            Pylon.DeviceSetBooleanFeature(imageProvider.DeviceHandle, "ReverseY", reverseY);
        }
    }
}
