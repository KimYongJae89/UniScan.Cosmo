using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using DynMvp.Base;
using Matrox.MatroxImagingLibrary;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraMil : Camera
    {
        MIL_ID whiteKernel = MIL.M_NULL;

        MIL_ID digitizerId;
        GCHandle thisHandle;
        MIL_DIG_HOOK_FUNCTION_PTR frameTransferEndPtr = null;
        MIL_DIG_HOOK_FUNCTION_PTR processingFunctionPtr = null;

        MIL_ID[] grabImageBuffer = new MIL_ID[2];
        MIL_ID sourceImage = MIL.M_NULL;
        MIL_ID grabbedImage = MIL.M_NULL;

        public override void Initialize(CameraInfo cameraInfo)
        {
            CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;

            base.Initialize(cameraInfo);

            cameraInfoMil.MilSystem = GrabberMil. GetMilSystem(cameraInfoMil.SystemType, cameraInfoMil.SystemNum);

            if (cameraInfoMil.MilSystem == null)
            {
                LogHelper.Error(LoggerType.Error, "MilSystem is empty. Skip create the digitizer.");
                return;
            }

            string dcfFileName = GetDcfFile(cameraInfoMil.CameraType);
            MIL.MdigAlloc(cameraInfoMil.MilSystem.SystemId, cameraInfoMil.DigitizerNum, dcfFileName, MIL.M_DEFAULT, ref digitizerId);
            if (digitizerId == null)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Digitizer Allocation is Failed.{0}, {1}, {2}, {3}",
                    cameraInfoMil.SystemType.ToString(), cameraInfoMil.MilSystem.SystemId, cameraInfoMil.DigitizerNum, cameraInfoMil.CameraType.ToString()));
                return;
            }

            MIL.MdigControl(digitizerId, MIL.M_GRAB_MODE, MIL.M_ASYNCHRONOUS);
            MIL.MdigControl(digitizerId, MIL.M_GRAB_TIMEOUT, MIL.M_INFINITE);

            thisHandle = GCHandle.Alloc(this);

            //frameExposureEndPtr = new MIL_DIG_HOOK_FUNCTION_PTR(FrameExposureEnd);
            //MIL.MdigHookFunction(digitizerId, MIL.M_GRAB_FRAME_START, frameExposureEndPtr, GCHandle.ToIntPtr(thisHandle));

            frameTransferEndPtr = new MIL_DIG_HOOK_FUNCTION_PTR(FrameTransferEnd);
            MIL.MdigHookFunction(digitizerId, MIL.M_GRAB_END, frameTransferEndPtr, GCHandle.ToIntPtr(thisHandle));

            MIL_INT tempValue = 0;
            MIL_INT width = 0;
            MIL_INT height = 0;

            MIL.MdigInquire(digitizerId, MIL.M_SIZE_X, ref width);
            MIL.MdigInquire(digitizerId, MIL.M_SIZE_Y, ref height);

            ImageSize = new Size((int)width, (int)height);
            MIL.MdigInquire(digitizerId, MIL.M_SIZE_BAND, ref tempValue);
            NumOfBand = (int)tempValue;

            ImagePitch = (int)width * NumOfBand;

            if (NumOfBand == 1)
            {
                grabImageBuffer[0] = MIL.MbufAlloc2d(cameraInfoMil.MilSystem.SystemId, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_GRAB, MIL.M_NULL);
                grabImageBuffer[1] = MIL.MbufAlloc2d(cameraInfoMil.MilSystem.SystemId, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_GRAB, MIL.M_NULL);
            }
            else
            {
                grabImageBuffer[0] = MIL.MbufAllocColor(cameraInfoMil.MilSystem.SystemId, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_GRAB, MIL.M_NULL);
                grabImageBuffer[1] = MIL.MbufAllocColor(cameraInfoMil.MilSystem.SystemId, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC + MIL.M_GRAB, MIL.M_NULL);
            }

            grabbedImage = grabImageBuffer[0];

            if (cameraInfoMil.BayerCamera == true)
            {
                sourceImage = MIL.MbufAllocColor(cameraInfoMil.MilSystem.SystemId, 3, width, height, MIL.M_UNSIGNED + 8, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);

                MIL.MbufPut(whiteKernel, cameraInfoMil.WhiteBalanceCoefficient);
                BayerType = cameraInfoMil.BayerType;
            }
            else
            {
                sourceImage = grabImageBuffer[0];
            }
        }

        public override void SetTriggerDelay(int exposureTimeUs)
        {
            MIL.MdigControl(digitizerId, MIL.M_GRAB_TRIGGER_DELAY, exposureTimeUs*1000);
        }

        public override void Release()
        {
            base.Release();

            LogHelper.Debug(LoggerType.Grab, "CameraMil - Release Mil System");
            MIL.MdigFree(digitizerId);

            MIL.MbufFree(grabImageBuffer[0]);
            MIL.MbufFree(grabImageBuffer[1]);

            if (whiteKernel != MIL.M_NULL)
            {
                MIL.MbufFree(whiteKernel);
                MIL.MbufFree(sourceImage);
            }
        }

        public MIL_INT FrameExposureEnd(MIL_INT HookType, MIL_ID EventId, IntPtr UserDataPtr)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - FrameExposureEnd");

            ExposureDoneCallback();

            return MIL.M_NULL;
        }

        public MIL_INT FrameTransferEnd(MIL_INT HookType, MIL_ID EventId, IntPtr UserDataPtr)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - Begin FrameTransferEnd");

            MIL_ID currentImageId = MIL.M_NULL;
            MIL.MdigGetHookInfo(EventId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref currentImageId);

            grabbedImage = currentImageId;

            ImageGrabbedCallback(IntPtr.Zero);

            LogHelper.Debug(LoggerType.Grab, "CameraMil - End FrameTransferEnd");

            return MIL.M_NULL;
        }

        private string GetDcfFile(CameraType cameraType)
        {
            switch (cameraType)
            {
                case CameraType.PrimeTech_PXCB120VTH:
                    return "MIL90_PXCB120VTH1_1tap_HW.dcf";
                case CameraType.Crevis_MC_D500B:
                    return "MIL10_SOL_5MCREVIS_2TAP_HWTRIG.dcf";
                case CameraType.PrimeTech_PXCB16QWTPM:
                    return "HWTRIG.dcf";
                case CameraType.PrimeTech_PXCB16QWTPMCOMPACT:
                    return "HWTRIG2.dcf";
                case CameraType.HV_B550CTRG1:
                    return "HV_B550C_TRG1.dcf";
                case CameraType.HV_B550CTRG2:
                    return "HV_B550C_TRG2.dcf";

            }

            return "";
        }

        static MIL_INT ProcessingFunction(MIL_INT HookType, MIL_ID HookId, IntPtr HookDataPtr)
        {
            if (IntPtr.Zero.Equals(HookDataPtr) == true)
                return MIL.M_NULL;

            MIL_ID currentImageId = MIL.M_NULL;
            MIL.MdigGetHookInfo(HookId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref currentImageId);

            GCHandle hUserData = GCHandle.FromIntPtr(HookDataPtr);

            // get a reference to the DigHookUserData object
            CameraMil cameraMil = hUserData.Target as CameraMil;
            cameraMil.grabbedImage = currentImageId;

            cameraMil.ImageGrabbedCallback(IntPtr.Zero);

            return MIL.M_NULL;
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        { 
            LogHelper.Debug(LoggerType.Grab, "CameraMil - Begin SetTriggerMode");

            base.SetTriggerMode(triggerMode, triggerType);

            if (triggerMode == TriggerMode.Software)
            {
                MIL.MdigControl(digitizerId, MIL.M_TIMER_TRIGGER_SOURCE + MIL.M_TIMER1, MIL.M_SOFTWARE);
                MIL.MdigControl(digitizerId, MIL.M_TIMER_TRIGGER_SOFTWARE + MIL.M_TIMER1, MIL.M_ACTIVATE);
            }
            else
            {
                if (triggerChannel == 0)
                    MIL.MdigControl(digitizerId, MIL.M_TIMER_TRIGGER_SOURCE + MIL.M_TIMER1, MIL.M_AUX_IO6);
                else
                    MIL.MdigControl(digitizerId, MIL.M_TIMER_TRIGGER_SOURCE + MIL.M_TIMER1, MIL.M_AUX_IO5);

                if (triggerType == TriggerType.RisingEdge)
                    MIL.MdigControl(digitizerId, MIL.M_TIMER_TRIGGER_ACTIVATION + MIL.M_TIMER1, MIL.M_EDGE_RISING);
                else
                    MIL.MdigControl(digitizerId, MIL.M_TIMER_TRIGGER_ACTIVATION + MIL.M_TIMER1, MIL.M_EDGE_FALLING);
            }

            LogHelper.Debug(LoggerType.Grab, "CameraMil - End SetTriggerMode");
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            if (grabbedImage != null)
            {
                if (NumOfBand == 1)
                {
                    if (BayerCamera == true)
                    {
                        MIL.MbufBayer(grabbedImage, sourceImage, whiteKernel, (long)BayerType);
                        return CreateGrabbedImage(true);
                    }
                    else
                    {
                        sourceImage = grabbedImage;
                        return CreateGrabbedImage(false);
                    }
                }
                else if (NumOfBand == 3)
                {
                    sourceImage = grabbedImage;
                    return CreateGrabbedImage(true);
                }
            }

            return null;
        }
        
        public ImageD CreateGrabbedImage(bool colorImage)
        {
            if (sourceImage == MIL.M_NULL)
                return null;

            IntPtr hostAddress = IntPtr.Zero;
            MIL.MbufInquire(sourceImage, MIL.M_HOST_ADDRESS, hostAddress);

            Image2D image2d = new Image2D();
            image2d.Initialize(ImageSize.Width, ImageSize.Height, colorImage ? 3 : 1, ImagePitch, hostAddress);

            return image2d;
        }

        public bool CopyGrayImage(ImageD image)
        {
            if (sourceImage == MIL.M_NULL)
                return false;

            Image2D image2d = (Image2D)image;

            byte[] milBuf = new byte[ImageSize.Width * ImageSize.Height];

            MIL.MbufGet(sourceImage, milBuf);

            image2d.SetData(milBuf);

            return true;
        }

        public bool CopyColorImage(ImageD image)
        {
            if (sourceImage == MIL.M_NULL)
                return false;

            Image2D image2d = (Image2D)image;

            byte[] milBuf = new byte[ImageSize.Width * ImageSize.Height * 3];

            MIL.MbufGetColor(sourceImage, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BAND, milBuf);

            image2d.SetData(milBuf);

            return true;
        }

        public override void GrabOnce()
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - GrabOnce");

            if (SetupGrab() == false)
                return;

                this.isStopped.Reset();
            grabCount = 1;
            MIL.MdigGrab(digitizerId, grabbedImage);
        }

        public override void GrabMulti(int grabCount)
        {
            LogHelper.Debug(LoggerType.Grab, "CameraMil - GrabContinuous");

            if (SetupGrab() == false)
                return;

            this.grabCount = grabCount;

            if (triggerMode == TriggerMode.Software)
            {
                MIL.MdigGrabContinuous(digitizerId, grabbedImage);
            }
            else
            {
                if (processingFunctionPtr != null)
                    processingFunctionPtr = new MIL_DIG_HOOK_FUNCTION_PTR(ProcessingFunction);

                MIL.MdigProcess(digitizerId, grabImageBuffer, 2, MIL.M_START, MIL.M_DEFAULT, processingFunctionPtr, GCHandle.ToIntPtr(thisHandle));
            }
        }

        public override void SetStopFlag()
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("Stop Continuous {0}", Index));

            Stop();
        }

        public override void Stop()
        {
            base.Stop();
            if (triggerMode == TriggerMode.Software)
            {
                MIL.MdigHalt(digitizerId);
            }
            else
            {
                MIL.MdigProcess(digitizerId, grabImageBuffer, 2, MIL.M_STOP, MIL.M_DEFAULT, processingFunctionPtr, GCHandle.ToIntPtr(thisHandle));
            }
            Thread.Sleep(50);
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            MIL.MdigControl(digitizerId, MIL.M_TIMER_DURATION + MIL.M_TIMER1, exposureTimeMs * 1000);
            return true;
        }

        public override List<ImageD> GetImageBufferList()
        {
            throw new NotImplementedException();
        }

        public override float GetDeviceExposureMs()
        {
            throw new NotImplementedException();
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            throw new NotImplementedException();
        }

        public override float GetAcquisitionLineRate()
        {
            throw new NotImplementedException();
        }

        public override void SetScanMode(ScanMode scanMode)
        {
        }
    }
}
