using System;
using System.Collections.Generic;
using System.Threading;

using DynMvp.Base;
using System.Diagnostics;

using Euresys;
using System.Windows.Forms;
using DynMvp.Device.Device.FrameGrabber;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraGenTL : Camera
    {
        Euresys.GenTL genTL = null;
        Euresys.RGBConverter.RGBConverter converter = null;
        protected Euresys.EGrabberCallbackSingleThread grabber = null;
        Euresys.Buffer currentBuffer = null;

        MatroxBuffer mBuffer = null;
        IntPtr lastGrabbedImgaePtr = IntPtr.Zero;

        protected List<Image2D> grabbedImageList = new List<Image2D>();

        private Mutex imageMutex = new Mutex();

        public int GetBufferIndex(IntPtr ptr)
        {
            return grabbedImageList.FindIndex(f => f.DataPtr == ptr);
        }

        public override bool IsBinningVirtical()
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)cameraInfo;
            return cameraInfoGenTL.BinningVertical;
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            base.SetTriggerMode(triggerMode, triggerType);

            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)this.CameraInfo;
            if (cameraInfoGenTL.ClientType == CameraInfoGenTL.EClientType.Master)
            {
                string mode = grabber.getStringRemoteModule("OperationMode");
                if (mode != "TDI")
                    return;
                try
                {
                    if (triggerMode == TriggerMode.Hardware)
                    {
                        grabber.setStringRemoteModule("TriggerMode", "On");
                    }
                    else
                    {
                        grabber.setStringRemoteModule("TriggerMode", "Off");
                    }
                }
                catch (Euresys.gentl_error ex)
                {
                    LogHelper.Error(LoggerType.Error, ex.Message);
                    return;
                }
            }

            UpdateBuffer(0, 0, 0, 0, 0);
        }

        public override void SetScanMode(ScanMode scanMode)
        {
            switch (scanMode)
            {
                case ScanMode.Area:
                    SetAreaMode();
                    break;
                case ScanMode.Line:
                    SetLineScanMode();
                    break;
            }
        }

        private void SetAreaMode()
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)this.CameraInfo;
            if (cameraInfoGenTL.ClientType == CameraInfoGenTL.EClientType.Master)
            {
                string mode = grabber.getStringRemoteModule("OperationMode");
                if (mode != "Area")
                    grabber.setStringRemoteModule("OperationMode", "Area");
                grabber.setStringRemoteModule("TDIStages", "TDI128");
            }
            UpdateBuffer(0, 128, 128, 100, 0);
        }

        private void SetLineScanMode()
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)this.CameraInfo;
            if (cameraInfoGenTL.ClientType == CameraInfoGenTL.EClientType.Master)
            {
                string mode = grabber.getStringRemoteModule("OperationMode");
                if (mode != "TDI")
                    grabber.setStringRemoteModule("OperationMode", "TDI");

                grabber.setStringRemoteModule("TDIStages", "TDI128");
                grabber.setStringRemoteModule("ScanDirection", cameraInfoGenTL.DirectionType.ToString());
            }

            UpdateBuffer(0, 0, 0, 0, 0);
        }

        public override void Initialize(CameraInfo cameraInfo)
        {
            try
            {
                base.Initialize(cameraInfo);

                CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)cameraInfo;

                UseNativeBuffering = true;


                genTL = new Euresys.GenTL();
                converter = new Euresys.RGBConverter.RGBConverter(genTL);
                grabber = new Euresys.EGrabberCallbackSingleThread(genTL);

                //grabber.setIntegerStreamModule("BufferHeight", cameraInfoGenTL.Height);
                //UpdateBufferSize(17824, cameraInfoGenTL.Height, (int)cameraInfoGenTL.FrameNum);
                UpdateBuffer(cameraInfoGenTL.Width, cameraInfoGenTL.Height, cameraInfoGenTL.ScanLength, cameraInfoGenTL.FrameNum, cameraInfoGenTL.OffsetX, true);

                //grabber.setStringRemoteModule("OperationMode", "TDI");

                // Remote Module - ImageFormatControl
                //grabber.setIntegerRemoteModule("Width", 17824);

                SetDevice();

                grabber.runScript("var p = grabbers[0].StreamPort; for (var s of p.$ee('EventSelector')) {p.set('EventNotification['+s+']', true);}");

                grabber.onNewBufferEvent = GenTLCamCallback;

                grabber.enableAllEvent();

                Euresys.SizeT width = grabber.getWidth();
                Euresys.SizeT height = grabber.getHeight();

                string imgFormat = grabber.getPixelFormat();
                this.ImageSize = new System.Drawing.Size(cameraInfoGenTL.Width, (int)cameraInfoGenTL.ScanLength);
            }
            catch (Euresys.gentl_error ex)
            {
                if (this.mBuffer != null)
                    this.mBuffer.Dispose();
                this.mBuffer = null;

                string message = string.Format("Exception\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);

                throw new CameraInitializeFailException("GenTL Exception : " + ex.Message);
            }
            catch(Exception ex)
            {
                string message = string.Format("Exception\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
                System.Windows.Forms.MessageBox.Show(message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }

        protected virtual void SetDevice()
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)CameraInfo;
            grabber.setStringRemoteModule("PixelFormat", "Mono8");
            switch (cameraInfoGenTL.ClientType)
            {
                case CameraInfoGenTL.EClientType.Master:
                    // Interface
                    grabber.setStringInterfaceModule("LineInputToolSelector", "LIN1");
                    grabber.setStringInterfaceModule("LineInputToolSource", cameraInfoGenTL.LineInputToolSource.ToString());
                    grabber.setStringInterfaceModule("LineInputToolActivation", "RisingEdge");
                    
                    // Device
                    grabber.setStringDeviceModule("CameraControlMethod", "RC");
                    grabber.setFloatDeviceModule("CycleMinimumPeriod", 3.36);

                    grabber.setStringDeviceModule("CycleTriggerSource", "Immediate");

                    grabber.setStringDeviceModule("StartOfSequenceTriggerSource", "LIN1");
                    grabber.setStringDeviceModule("EndOfSequenceTriggerSource", "SequenceLength");
                    grabber.setStringDeviceModule("SequenceLength", "1");

                    grabber.setStringDeviceModule("CxpTriggerMessageFormat", "Pulse");

                    // Romote Module
                    grabber.setStringRemoteModule("TriggerSource", "CXPin");
                    grabber.setStringRemoteModule("TriggerActivation", "RisingEdge");

                    grabber.setStringRemoteModule("TriggerRescalerMode", cameraInfoGenTL.TriggerRescalerMode ? "On" : "Off");
                    grabber.setStringRemoteModule("TriggerRescalerRate", cameraInfoGenTL.TriggerRescalerRate.ToString());
                    grabber.setStringRemoteModule("BinningVertical", cameraInfoGenTL.BinningVertical ? "X2" : "X1");
                    grabber.setStringRemoteModule("ReverseX", cameraInfoGenTL.MirrorX.ToString());
                    grabber.setStringRemoteModule("AnalogGain", cameraInfoGenTL.AnalogGain.ToString());
                    grabber.setFloatRemoteModule("DigitalGain", cameraInfoGenTL.DigitalGain);
                    grabber.setIntegerRemoteModule("OffsetX", cameraInfoGenTL.OffsetX);
                    
                    SetLineScanMode();

                    break;
                case CameraInfoGenTL.EClientType.Slave:
                    grabber.setStringInterfaceModule("DelayToolSelector", "DEL1");
                    grabber.setStringInterfaceModule("DelayToolSource1", "EIN1");
                    grabber.setStringInterfaceModule("EventInputToolSelector", "EIN1");
                    grabber.setStringInterfaceModule("EventInputToolSource", "A");
                    grabber.setStringInterfaceModule("EventInputToolActivation", "StartOfScan");

                    grabber.setStringStreamModule("StartOfScanTriggerSource", "EIN1");
                    break;
            }
        }

        public virtual bool UpdateBuffer(int width, int height, uint scanLines, uint count, uint offsetX, bool forceRealloc = false)
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)CameraInfo;

            if (width <= 0)
                width = cameraInfoGenTL.Width;//17824;

            if (height <= 0)
                height = cameraInfoGenTL.Height;

            if (scanLines <= 0)
                scanLines = cameraInfoGenTL.ScanLength;

            if (count <= 0)
                count = cameraInfoGenTL.FrameNum;

            int curWidth = (int)grabber.getIntegerRemoteModule("Width");
            int curBufferHeight = (int)grabber.getIntegerStreamModule("BufferHeight");
            int curScanLength = (int)grabber.getIntegerStreamModule("ScanLength");
            if (forceRealloc == false && curWidth == width && curBufferHeight == scanLines && curScanLength == scanLines)
                return false;

            grabber.setIntegerRemoteModule("Width", width);
            grabber.setIntegerStreamModule("BufferHeight", scanLines);
            grabber.setIntegerStreamModule("ScanLength", scanLines);
         
            Thread.Sleep(200);

            ReleaseBuffer();
            grabber.flushBuffers( Euresys.gc.ACQ_QUEUE_TYPE.ACQ_QUEUE_ALL_DISCARD);
            grabber.resetBufferQueue();

            BufferIndexRange bufferIndexRange;
            if (cameraInfoGenTL.UseMilBuffer)
            {
                long bufWidth = width;
                long bufHeight = height * count;
                this.mBuffer?.Dispose();
                this.mBuffer = new MatroxBuffer(bufWidth, bufHeight);

                UserMemoryArray userMemoryArray = new UserMemoryArray();
                userMemoryArray.memory.@base = mBuffer.Ptr;
                userMemoryArray.memory.size = (ulong)(width * height * count);
                userMemoryArray.bufferSize = (ulong)(width * height);

                bufferIndexRange = grabber.announceAndQueue(userMemoryArray);
            }
            else
            {
                bufferIndexRange = grabber.reallocBuffers((uint)count); 
            }

            Euresys.SizeT bufferIndex = bufferIndexRange.begin;
            while (bufferIndex != bufferIndexRange.end)
            {
                IntPtr imgPtr;
                grabber.getBufferInfo(bufferIndex, Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out imgPtr);

                Image2D grabbedImage = new Image2D();
                if (UseNativeBuffering)
                {
                    grabbedImage.Initialize(width, height, 1, width, imgPtr);
                }
                else
                {
                    grabbedImage.Initialize(width, height, 1, width);
                }

                grabbedImage.Tag = new CameraBufferTag(grabbedImageList.Count, 0);
                grabbedImageList.Add(grabbedImage);
                
                bufferIndex++;
            }

            ImageSize = new System.Drawing.Size(width, height);

            return true;
        }

        public virtual void ReleaseBuffer()
        {
            foreach (Image2D grabbedImage in grabbedImageList)
                grabbedImage.Dispose();
            grabbedImageList.Clear();
        }

        public void GenTLCamCallback(Euresys.EGrabberCallbackSingleThread g, Euresys.NewBufferData data)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GenTLCamCallback Start");
            isGrabbed.Set();

            if (grabCount != CONTINUOUS)
            {
                if (grabCount != 0)
                    grabCount--;
                if (grabCount == 0)
                    Stop();
            }

            IntPtr ptr;
            ulong frameId;
            long width, height;
            long size;
            using (Euresys.ScopedBuffer buffer = new Euresys.ScopedBuffer((Euresys.EGrabberCallbackSingleThread)g, data))
            {
                buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out ptr);
                buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_FRAMEID, out frameId);
                buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_WIDTH, out width);
                //buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_HEIGHT, out height);
                buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_SIZE, out size);
                height = size / width;
                int bufferId = grabbedImageList.FindIndex(f => f.DataPtr == ptr);

                CameraBufferTag cameraBufferTag = grabbedImageList[bufferId].Tag as CameraBufferTag;
                cameraBufferTag.UpdateFrameId(frameId);

                //Debug.WriteLine(string.Format("[{0}] CameraGenTL::GenTLCamCallback, Id{1}, W{2}, H{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), frameId, width, height));

                //UpdateBufferTag(buffer);
                lastGrabbedImgaePtr = ptr;

            }


            ImageGrabbedCallback(ptr);
        }

        static Int64 fpsToMicroseconds(Int64 fps)
        {
            if (fps == 0)
            {
                return 0;
            }
            else
            {
                return (1000000 + fps - 1) / fps;
            }
        }

        private void Start()
        {
            grabber.resetBufferQueue();
            //grabber.flushBuffers();
            grabber.start();
        }
        

        public override void SetTriggerDelay(int triggerDelayUs)
        {

        }

        public override void Release()
        {
            base.Release();

            if (mBuffer != null)
                mBuffer.Dispose();
            mBuffer = null;

            if (grabber != null)
                grabber.runScript("var p = grabbers[0].StreamPort; for (var s of p.$ee('EventSelector')) {p.set('EventNotification['+s+']', false);}");

            if (grabber != null)
            {
                grabber.stop();
                grabber.Dispose();
            }
            grabber = null;

            if (converter != null)
            {
                converter.Dispose();
            }
            converter = null;

            if (genTL != null)
            {
                genTL.Dispose();
            }
            genTL = null;
        }

        public override void SetupNativeBuffering()
        {
            base.SetupNativeBuffering();
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                ptr = this.lastGrabbedImgaePtr;

            int bufferIndex = GetBufferIndex(ptr);

            //Debug.Assert(bufferIndex >= 0);
            if (bufferIndex < 0)
                return null;

            LogHelper.Debug(LoggerType.Grab, string.Format("CameraGenTL::GetGrabbedImage - {0}", bufferIndex));
            return grabbedImageList[bufferIndex].Clone();
        }

        public override List<ImageD> GetImageBufferList()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GetImageBufferList");
            return new List<ImageD>(grabbedImageList);
        }
        
        public override void GrabOnce()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GrabOnce");

            try
            {
                if (SetupGrab() == false)
                    return;

                this.grabCount = 1;
                
                Start();

                //LogHelper.Debug(LoggerType.Grab, "CameraGenTL - Channel Activated");
            }
            catch (Euresys.gentl_error exc)
            {
                LogHelper.Error(LoggerType.Error, "GenTL Exception : " + exc.Message);
            }
            catch (Exception e)
            {

            }
        }

        public override void GrabMulti(int grabCount)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GrabMulti");

            try
            {
                if (SetupGrab() == false)
                    return;

                this.grabCount = grabCount;

                //grabber.setStringDeviceModule("EndOfSequenceTriggerSource", "StopSequence");
                Start();

                //LogHelper.Debug(LoggerType.Grab, "CameraGenTL - Channel Activated");
            }
            catch (Euresys.gentl_error exc)
            {
                LogHelper.Error(LoggerType.Error, "GenTL Exception : " + exc.Message);
            }
            catch (Exception e)
            {

            }
        }
        
        public override void SetStopFlag()
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("CameraGenTL::SetStopFlag. {0}", Index));

            Stop();
        }

        public override void Stop()
        {
            base.Stop();
            grabber?.stop();
            grabCount = 0;
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            if (exposureTimeMs <= 0)
                return false;

            LogHelper.Debug(LoggerType.Grab, String.Format("CameraGenTL::SetDeviceExposure"));
            try
            {
                if (grabber.getStringRemoteModule("OperationMode") == "Area")
                {
                    grabber.setFloatRemoteModule("ExposureTime", exposureTimeMs * 1000);
                    return true;
                }
                return false;

            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraGenTL::SetDeviceExposure Fail. " + e.Message);
                return false;
            }
        }

        public override float GetDeviceExposureMs()
        {
            CameraInfoGenTL cameraInfoGenTL = this.cameraInfo as CameraInfoGenTL;
            if (cameraInfoGenTL.ClientType == CameraInfoGenTL.EClientType.Slave)
                return -1;

            if (grabber.getStringRemoteModule("OperationMode") == "Area")
            {
                return (float)grabber.getFloatRemoteModule("ExposureTime") / 1000f;
            }
            return -1;
            //else
            //{
            //    float grabHz = (float)grabber.getFloatRemoteModule("AcquisitionLineRate"); // [1/s]
            //    return (float)((1 / grabHz) * 1000f);
            //}
        }

        public string GetPropertyData(string itemName)
        {
            switch (itemName)
            {
                case "DeviceModelName":
                case "DeviceSerialNumber":
                case "OperationMode":
                case "TDIStages":
                case "ScanDirection":
                    return grabber.getStringRemoteModule(itemName);

                case "AcquisitionLineRate":
                    return grabber.getFloatRemoteModule(itemName).ToString();

                case "Width":
                    return grabber.getIntegerRemoteModule(itemName).ToString();

                case "ScanLength":
                    return grabber.getIntegerStreamModule(itemName).ToString();

                default:
                    return null;
            }
        }

        public void SetPropertyData(string itemName, string value)
        {
            switch (itemName)
            {
                case "OperationMode":
                case "TDIStages":
                case "ScanDirection":
                    grabber.setStringRemoteModule(itemName, value);
                    break;
                    
                case "AcquisitionLineRate":
                    grabber.setFloatRemoteModule(itemName, Convert.ToSingle(value));
                    break;

                case "Width":
                case "ScanLength":
                    grabber.setIntegerRemoteModule(itemName, Convert.ToInt32(value));
                    break;

                default:
                    break;
            }
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            if (hz <= 0)
                return false;

            LogHelper.Debug(LoggerType.Grab, String.Format("CameraGenTL::SetAcquisitionLineRate {0:F3}kHz", hz / 1000f));
            try
            {
                if (grabber.getStringRemoteModule("OperationMode") == "TDI")
                {
                    //if (((CameraInfoGenTL)this.cameraInfo).BinningVertical)
                    //    hz *= 2;
                    grabber.setFloatRemoteModule("AcquisitionLineRate", hz);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "CameraGenTL::SetDeviceExposure Fail. " + e.Message);
                return false;
            }
        }

        public override float GetAcquisitionLineRate()
        {
            double hz = grabber.getFloatRemoteModule("AcquisitionLineRate");
            return (float)hz;
        }
    }
}
