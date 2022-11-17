//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.Device.Device.FrameGrabber;
//using DynMvp.Devices;
//using DynMvp.Devices.FrameGrabber;
//using DynMvp.InspData;
//using DynMvp.UI;
//using DynMvp.Vision;
//using UniScanG.Algorithms;
//using UniScanG.Device;
//using UniScanG.Operation.Data;
//using UniScanG.Operation.Inspect;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using UniEye.Base;
//using UniEye.Base.Settings;
//using UniEye.Base.UI;
//using UniEye.Base.Device;

//namespace UniScanG.Temp
//{
//    class UniScanGViewerPageExtender : HwTriggerModellerPageExtender
//    {

//    }

//    class UniScanGModellerPageExtender : HwTriggerModellerPageExtender
//    {
//        string testGrabResultFile = "";
//        Image2D lastGrabbedImage;
//        public Image2D LastGrabbedImage
//        {
//            get { return lastGrabbedImage; }
//        }

//        GrabProcesser grabProcesser = null;

//        LightParamSet lightParamSet = null;

//        TeachingPage teachingPage = null;

//        Image2D fullBufferImage = null;
//        private List<float> grabPrepareValueList = new List<float>();
//        private List<Image2D> bufferImageList = new List<Image2D>();

//        public Image2D FullBufferImage
//        {
//            get { return fullBufferImage; }
//        }
        
//        public UniScanGModellerPageExtender(TeachingPage teachingPage) : base()
//        {
//            this.teachingPage = teachingPage;
//        }
        
//        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
//        {
//            LogHelper.Debug(LoggerType.Grab, "SamsungElectroGravureModellerPageExtender::ImageGrabbed");
            
//            StopGrab();

//            teachingPage.ClearTestInspectionResult();

//            this.teachingPage.GrabDone();
//        }

//        public void NormalImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
//        {
//            LogHelper.Debug(LoggerType.Grab, "SamsungElectroGravureModellerPageExtender::NormalImageGrabbed");
//            lastGrabbedImage = (Image2D)imageDevice.GetGrabbedImage(ptr).Clone();
//            lastGrabbedImage.ConvertFromDataPtr();

//            ImageGrabbed(imageDevice, ptr);
//        }

//        public void FiducialImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
//        {
//            LogHelper.Debug(LoggerType.Grab, "SamsungElectroGravureModellerPageExtender::FiducialImageGrabbed");

//            teachingPage.ClearTestInspectionResult();
            
//            ICameraExtender cameraExtender = imageDevice as ICameraExtender;
//            AlgoImage algoImage = cameraExtender.GetAlgoImage(ptr);
//            grabProcesser.OnNewImageArrived(algoImage);
//        }

//        public void SequenceImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
//        {
//            LogHelper.Debug(LoggerType.Grab, "SamsungElectroGravureModellerPageExtender::ImageGrabbed");

//            Task task = new Task(() =>
//            {
//                ImageD grabImage = imageDevice.GetGrabbedImage(ptr);
//                Image2D copyImage = (Image2D)grabImage.Clone();
//                copyImage.ConvertFromDataPtr();
//                teachingPage.SequenceImageAdd(copyImage);
//            });
//            task.Start();
//        }

//        public void TestImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
//        {
//            Image2D image2D =(Image2D)imageDevice.GetGrabbedImage(ptr);
//            CameraBufferTag tag = image2D.Tag as CameraBufferTag;
//            System.IO.StreamWriter writer = System.IO.File.AppendText(testGrabResultFile);
//            writer.WriteLine("{0} {1} {2} {3} {4}", DateTime.Now.ToString("[yyyy-MM-dd] HH:mm:ss"), image2D.DataPtr, tag.BufferId, tag.FrameId, tag.TimeStamp);
//            writer.Close();
//        }
        
//        public void FiducialImageGrabDone()
//        {
//            // 그랩 완료. 종료.
//            LogHelper.Debug(LoggerType.Grab, "SamsungElectroGravureModellerPageExtender::FiducialImageGrabDone");

//            if (grabProcesser != null)
//                grabProcesser.Stop();

//            SheetImageSet imageSet= grabProcesser.GetLastSheetImageSet();
//            lastGrabbedImage = (Image2D)imageSet.ToImageD();
//            //lastGrabbedImage.SaveImage(@"D:\temp\tt.bmp", ImageFormat.Bmp);
//            ImageGrabbed(null, IntPtr.Zero);
//        }

//        public override void StopGrab()
//        {
//            ImageDevice imageDevice = GetImageDevice(deviceIndex);
//            if (imageDevice == null)
//                return;

//            imageDevice.ImageGrabbed -= NormalImageGrabbed;
//            imageDevice.ImageGrabbed -= FiducialImageGrabbed;
//            imageDevice.ImageGrabbed -= SequenceImageGrabbed;
//            imageDevice.ImageGrabbed -= TestImageGrabbed;

//            if (grabProcesser != null)
//            {
//                grabProcesser.Stop();
//                grabProcesser.Dispose();
//                grabProcesser = null;
//            }
//        }

//        public bool UpdateGrabbedImage()
//        {
//            LogHelper.Debug(LoggerType.Operation, "SamsungElectroGravureModellerPageExtender::UpdateGrabbedImage");
//            if (fullBufferImage != null)
//                fullBufferImage.Dispose();
//            fullBufferImage = null;

//            // Fiducial Grab - Create Whole Image from GrabProcesser
//            if (grabProcesser == null)
//                return false;

//            if (grabProcesser.IsFullImageGrabbed() == false)
//                return false;

//            SheetImageSet imageSet = grabProcesser.GetLastSheetImageSet();
//            if (imageSet == null)
//                return false;

//            fullBufferImage = (Image2D)imageSet.ToImageD();
//            //fullBufferImage = SheetChecker.GenerateWholeImage(imageSet, new SizeF(1, 1), null);
//            imageSet.Dispose();

//            // Save
//            //string imagePath = Path.Combine(SystemManager.Instance().CurrentModel.ModelPath, "Image");
//            //if (Directory.Exists(imagePath) == false)
//            //    Directory.CreateDirectory(imagePath);

//            //string fullBufferFileName = ImageBuffer.GetImage2dFileName(deviceIndex, stepIndex, lightTypeIndex, ImageFormat.Jpeg.ToString());
//            //fullBufferImage.SaveImage(Path.Combine(imagePath, fullBufferFileName), ImageFormat.Jpeg);            

//            return true;
//        }

//        protected void SetupGrab(TeachingPage.GrabType grabType)
//        {
//            ImageDevice imageDevice = GetImageDevice(deviceIndex);
//            if (imageDevice == null)
//                return;

//            if (fullBufferImage != null)
//            {
//                fullBufferImage.Dispose();
//                fullBufferImage = null;
//            }

//            if (grabProcesser != null)
//                grabProcesser.Dispose();
//            grabProcesser = null;


//            switch (grabType)
//            {
//                case TeachingPage.GrabType.Normal:
//                    imageDevice.ImageGrabbed += NormalImageGrabbed;
//                    break;

//                case TeachingPage.GrabType.Fiducial:
//                    imageDevice.ImageGrabbed += FiducialImageGrabbed;
//                    SetupGrabProcesser();
//                    break;

//                case TeachingPage.GrabType.Sequence:
//                    imageDevice.ImageGrabbed += SequenceImageGrabbed;
//                    break;

//                case TeachingPage.GrabType.Test:
//                    string dateTime = DateTime.Now.ToString("yyyyMMdd_HHmm");
//                    InspectorInfo inspectorInfo = UniScanGSettings.Instance().InspectorInfo;
//                    testGrabResultFile = Path.Combine(PathSettings.Instance().Temp, string.Format("GrabTest_{0}_{1}{2}.txt", dateTime, inspectorInfo.CamIndex + 1, (char)(inspectorInfo.ClientIndex + 'A')));
//                    System.IO.File.Delete(testGrabResultFile);

//                    System.IO.StreamWriter writer = System.IO.File.AppendText(testGrabResultFile);
//                    writer.WriteLine("Date Time PTR BufferId FrameId TimeStamp[us]");
//                    writer.Close();

//                    imageDevice.ImageGrabbed += TestImageGrabbed;
//                    break;
//            }
//        }

//        private void SetupGrabProcesser()
//        {
//            VisionProbe visionProbe = teachingPage.TeachBox.TeachHandler.SelectedObjs[0] as VisionProbe;
//            SheetCheckerParam param = visionProbe.InspAlgorithm.Param as SheetCheckerParam;

//            grabProcesser = new GrabProcesser();
//            grabProcesser.ClientIndex = -1;
//            grabProcesser.ImageGrabComplete = FiducialImageGrabDone;

//            grabProcesser.Initialize(param);

//            grabProcesser.Start();
//        }

//        public void GrabProcess(int stepIndex, int deviceIndex, int lightTypeIndex, LightParamSet lightParamSet, TeachingPage.GrabType grabType)
//        {
//            this.lightParamSet = lightParamSet;
//            this.stepIndex = stepIndex;
//            this.deviceIndex = deviceIndex;
//            this.lightTypeIndex = lightTypeIndex;

//            SetupGrab(grabType);

//            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
//            imageDeviceHandler.GrabMulti();
//        }
//    }
//}
