using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScan.Common;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.Data.Vision;
using UniScanG.Screen.Data;
using UniScanG.Screen.Vision;
using UniScanG.Screen.Vision.Detector;
using UniScanG.Screen.Vision.FiducialFinder;
using UniScanG.Screen.Vision.Trainer;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach;
using UniScanG.Vision;

namespace UniScanG.Screen.UI.Teach
{
    public class ModellerPageExtenderS : UniScanG.UI.Teach.ModellerPageExtender
    {
        public ModellerPageExtenderS()
        {
            // Base 생성자에서 추가됨
            //SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        public override string GetModelImageName()
        {
            return string.Format("Image.{0}", ImageFormat.Bmp.ToString());
        }

        public override void GrabFrame()
        {
            Grab();
        }

        public override bool GrabSheet(int count)
        {
            Grab();
            return true;
        }

        private void Grab()
        {
            System.Threading.CancellationTokenSource token = new System.Threading.CancellationTokenSource();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.AddImageGrabbed(ImageGrabbed);
            imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);
            imageDeviceHandler.GrabOnce();

            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() => imageDeviceHandler.WaitGrabDone(30000));

            imageDeviceHandler.Stop();
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);
            imageDeviceHandler.RemoveImageGrabbed(ImageGrabbed);
        }

        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (currentImage != null)
                currentImage.Dispose();

            currentImage = (Image2D)imageDevice.GetGrabbedImage(ptr).Clone();
            currentImage.ConvertFromDataPtr();

            if (UpdateImage != null)
                UpdateImage(currentImage, false);

            string imagePath = SystemManager.Instance().CurrentModel.GetImagePath();
            if (Directory.Exists(imagePath) == false)
                Directory.CreateDirectory(imagePath);

            Bitmap bitmap = currentImage.ToBitmap();
            ImageHelper.SaveImage(bitmap, GetModelImageName(), ImageFormat.Bmp);
            bitmap.Dispose();
        }

        protected override void TeachBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SheetTrainer trainer = (SheetTrainer)AlgorithmPool.Instance().GetAlgorithm(SheetTrainer.TypeName);

            if (trainer == null)
                return;

            trainer.Teach((BackgroundWorker)sender, currentImage, e);
        }

        protected override void TeachRunWorkerCompleted(bool result)
        {
            if (result == true)
            {
                if (SystemManager.Instance().CurrentModel.IsTrained == false)
                {
                    SheetInspector sheetInspector = (SheetInspector)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName);
                    SheetInspectorParam sheetInspectorParam = (SheetInspectorParam)sheetInspector.Param;

                    SheetTrainer sheetTrainer = (SheetTrainer)AlgorithmPool.Instance().GetAlgorithm(SheetTrainer.TypeName);
                    SheetTrainerParam sheetTrainerParam = (SheetTrainerParam)sheetTrainer.Param;
                    sheetInspectorParam.PoleParam.LowerThreshold = sheetTrainerParam.PoleRecommendLowerTh;
                    sheetInspectorParam.PoleParam.UpperThreshold = sheetTrainerParam.PoleRecommendUpperTh;
                    sheetInspectorParam.DielectricParam.LowerThreshold = sheetTrainerParam.DielectricRecommendLowerTh;
                    sheetInspectorParam.DielectricParam.UpperThreshold = sheetTrainerParam.DielectricRecommendUpperTh;
                }

                if (currentImage != null)
                {
                    Bitmap prevImage = SheetCombiner.CreatePrevImage(currentImage.ToBitmap());

                    string imagePath = SystemManager.Instance().CurrentModel.GetImagePath();

                    if (Directory.Exists(imagePath) == false)
                        Directory.CreateDirectory(imagePath);

                    string fileName = string.Format("Prev.{0}", ImageFormat.Bmp.ToString());
                    string filePath = Path.Combine(imagePath, fileName);

                    ImageHelper.SaveImage(prevImage, filePath, ImageFormat.Bmp);
                }
                    
                SystemManager.Instance().CurrentModel.IsTrained = true;
            }

            SaveModel();
        }

        //public void SelectedPattern(SheetPattern pattern)
        //{
        //    if (UpdatePatternFigure != null)
        //        UpdatePatternFigure(pattern);
        //}

        //public void SelectedFiducialPattern(FiducialPattern pattern)
        //{
        //    if (UpdateFiducialPatternFigure != null)
        //        UpdateFiducialPatternFigure(pattern);
        //}

        //public void SelectedRegionInfo(RegionInfo regionInfo)
        //{
        //    if (UpdateRegionInfo != null)
        //        UpdateRegionInfo(regionInfo);
        //}

        public override void Inspect(RegionInfo regionInfo)
        {
            throw new NotImplementedException();
        }

        public override void Inspect()
        {
            if (currentImage == null)
                return;

            SizeF offset = new SizeF();
            ProcessBufferSetS bufferSet = new ProcessBufferSetS(currentImage.Width, currentImage.Height);
            bufferSet.BuildBuffers();

            FiducialFinderAlgorithmResult finderResult = new FiducialFinderAlgorithmResult();
            if (AlgorithmSetting.Instance().IsFiducial == true)
            {
                SimpleProgressForm fiducialForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Find Fiducial"));
                fiducialForm.Show(new Action(() =>
                {
                    FiducialFinderS fiducialFinder = (FiducialFinderS)AlgorithmPool.Instance().GetAlgorithm(FiducialFinderS.TypeName);
                    SheetInspectParam inspectParam = new SheetInspectParam(currentImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
                    inspectParam.ClipImage = currentImage;
                    inspectParam.ProcessBufferSet = bufferSet;
                    finderResult = (FiducialFinderAlgorithmResult)fiducialFinder.Inspect(inspectParam);
                    offset = finderResult.OffsetFound;
                }));
            }

            Stopwatch stopwatch = new Stopwatch();

            SheetResult sheetResult = new ScreenResult();

            SimpleProgressForm inspectorForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Inspect"));
            inspectorForm.Show(new Action(() =>
            {
                SheetInspector sheetInspector = (SheetInspector)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName);
                SheetInspectParam inspectParam = new SheetInspectParam(currentImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
                inspectParam.ClipImage = currentImage;
                inspectParam.ProcessBufferSet = bufferSet;
                inspectParam.FidOffset = finderResult.OffsetFound;
                stopwatch.Start();
                sheetResult = (SheetResult)sheetInspector.Inspect(inspectParam);
                stopwatch.Stop();
            }));

            UpdateSheetResult(sheetResult);

            bufferSet.Dispose();
        }
    }
}