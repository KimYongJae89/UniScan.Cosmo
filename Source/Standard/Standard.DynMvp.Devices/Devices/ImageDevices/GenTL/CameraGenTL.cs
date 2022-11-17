using System;
using System.Collections.Generic;
using System.Threading;

using Standard.DynMvp.Base;
using System.Diagnostics;

using Euresys;

namespace Standard.DynMvp.Devices.ImageDevices.GenTL
{
    public class CameraGenTL : Camera
    {
        Euresys.GenTL genTL = null;
        Euresys.RGBConverter.RGBConverter converter = null;
        protected Euresys.EGrabberCallbackSingleThread grabber = null;
        Euresys.Buffer currentBuffer = null;

        MatroxBuffer mBuffer = null;
        IntPtr lastGrabbedImgaePtr = IntPtr.Zero;
        
        private Mutex imageMutex = new Mutex();

        public CameraGenTL(CameraInfo cameraInfo) : base(cameraInfo)
        {
        }
        
        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)DeviceInfo;
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

        //public override void SetScanMode(ScanMode scanMode)
        //{
        //    switch (scanMode)
        //    {
        //        case ScanMode.Area:
        //            SetAreaMode();
        //            break;
        //        case ScanMode.Line:
        //            SetLineScanMode();
        //            break;
        //    }
        //}

        //private void SetAreaMode()
        //{
        //    CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)this.CameraInfo;
        //    if (cameraInfoGenTL.ClientType == CameraInfoGenTL.EClientType.Master)
        //    {
        //        string mode = grabber.getStringRemoteModule("OperationMode");
        //        if (mode != "Area")
        //            grabber.setStringRemoteModule("OperationMode", "Area");
        //        grabber.setStringRemoteModule("TDIStages", "TDI128");
        //    }
        //    UpdateBuffer(0, 128, 128, 100, 0);
        //}

        private void SetLineScanMode()
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)DeviceInfo;
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

        protected override void Initialize()
        {
            try
            {
                CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)DeviceInfo;

                genTL = new Euresys.GenTL();
                converter = new Euresys.RGBConverter.RGBConverter(genTL);
                grabber = new Euresys.EGrabberCallbackSingleThread(genTL);

                UpdateBuffer((int)cameraInfoGenTL.ImageWidth, (int)cameraInfoGenTL.ImageHeight, cameraInfoGenTL.ScanLength, cameraInfoGenTL.FrameNum, cameraInfoGenTL.OffsetX, true);
                
                SetDevice();

                grabber.runScript("var p = grabbers[0].StreamPort; for (var s of p.$ee('EventSelector')) {p.set('EventNotification['+s+']', true);}");

                grabber.onNewBufferEvent = GenTLCamCallback;

                grabber.enableAllEvent();

                //Euresys.SizeT width = grabber.getWidth();
                //Euresys.SizeT height = grabber.getHeight();

                //string imgFormat = grabber.getPixelFormat();
            }
            catch (Euresys.gentl_error ex)
            {
                if (this.mBuffer != null)
                    this.mBuffer.Dispose();
                this.mBuffer = null;

                string message = string.Format("Exception\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);

                throw new CameraInitializeFailException("GenTL Exception : " + ex.Message);
            }
            catch(Exception ex)
            {
                string message = string.Format("Exception\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);
            }
            finally
            {
            }
        }

        protected virtual void SetDevice()
        {
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)DeviceInfo;
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
                    //grabber.setStringRemoteModule("ReverseX", cameraInfoGenTL.MirrorX.ToString());
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
            CameraInfoGenTL cameraInfoGenTL = (CameraInfoGenTL)DeviceInfo;

            if (width <= 0)
                width = (int)cameraInfoGenTL.ImageWidth;//17824;

            if (height <= 0)
                height = (int)cameraInfoGenTL.ImageHeight;

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

            //Euresys.SizeT bufferIndex = bufferIndexRange.begin;
            //while (bufferIndex != bufferIndexRange.end)
            //{
            //    IntPtr imgPtr;
            //    grabber.getBufferInfo(bufferIndex, Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out imgPtr);

            //    //Image2D grabbedImage = new Image2D();
            //    //if (UseNativeBuffering)
            //    //{
            //    //    grabbedImage.Initialize(width, height, 1, width, imgPtr);
            //    //}
            //    //else
            //    //{
            //    //    grabbedImage.Initialize(width, height, 1, width);
            //    //}

            //    //grabbedImage.Tag = new CameraBufferTag(grabbedImageList.Count, 0);
            //    //grabbedImageList.Add(grabbedImage);
                
            //    //bufferIndex++;
            //}

            //ImageSize = new System.Drawing.Size(width, height);

            return true;
        }

        public virtual void ReleaseBuffer()
        {
            //foreach (Image2D grabbedImage in grabbedImageList)
            //    grabbedImage.Dispose();
            //grabbedImageList.Clear();
        }

        public void GenTLCamCallback(Euresys.EGrabberCallbackSingleThread g, Euresys.NewBufferData data)
        {
            using (Euresys.ScopedBuffer buffer = new Euresys.ScopedBuffer((Euresys.EGrabberCallbackSingleThread)g, data))
            {
                buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out IntPtr ptr);
                ImageGrabbDone(ptr);
                //buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out ptr);
                //buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_FRAMEID, out frameId);
                //buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_WIDTH, out width);
                ////buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_HEIGHT, out height);
                //buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_SIZE, out size);
                //height = size / width;
                //int bufferId = grabbedImageList.FindIndex(f => f.DataPtr == ptr);

                //CameraBufferTag cameraBufferTag = grabbedImageList[bufferId].Tag as CameraBufferTag;
                //cameraBufferTag.UpdateFrameId(frameId);

                //Debug.WriteLine(string.Format("[{0}] CameraGenTL::GenTLCamCallback, Id{1}, W{2}, H{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), frameId, width, height));

                //UpdateBufferTag(buffer);
                //lastGrabbedImgaePtr = ptr;

            }


            //ImageGrabbedCallback(ptr);
        }
        
        private void Start()
        {
            grabber.resetBufferQueue();
            //grabber.flushBuffers();
            grabber.start();
        }

        protected override void Release()
        {
            mBuffer?.Dispose();
            mBuffer = null;

            grabber?.runScript("var p = grabbers[0].StreamPort; for (var s of p.$ee('EventSelector')) {p.set('EventNotification['+s+']', false);}");
            grabber?.stop();
            grabber?.Dispose();
            grabber = null;

            converter?.Dispose();
            converter = null;

            genTL?.Dispose();
            genTL = null;
        }
        
        public override void GrabOnce()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraGenTL::GrabOnce");

            try
            {
                SetupGrab(1);
                Start();
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
                SetupGrab(grabCount);


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

        public override void Stop()
        {
            grabber?.stop();
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
    }
}
