using DynMvp.Authentication;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScan.Common;
using UniScanG.Data;
using UniScanG.Data.Inspect;
using UniScanG.Data.Model;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer;
using UniScanG.Inspect;
using UniScanG.Screen.Data;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach;
using UniScanG.Vision;

namespace UniScanG.Gravure.UI.Teach
{
    public class ModellerPageExtenderG : UniScanG.UI.Teach.ModellerPageExtender
    {
        GrabProcesserG grabProcesser = null;

        public ModellerPageExtenderG()
        {
            // Base 생성자에서 추가됨
            //SystemManager.Instance().ExchangeOperator.AddModelListener(this);

        }
        
        public override string GetModelImageName()
        {
            int camIdx = SystemManager.Instance().ExchangeOperator.GetCamIndex();
            return SystemManager.Instance().CurrentModel.GetImageName(camIdx, 0, 0);
        }

        // for multi sheet grab
        int remainGrabCount = 0;

        public override bool GrabSheet(int grabCount)
        {
            bool isTestMode = grabCount == -1;
            remainGrabCount = grabCount;

            SheetGrabProcesserG grabProcesserG = new SheetGrabProcesserG();
            grabProcesserG.Algorithm = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
            grabProcesserG.StartInspectionDelegate += ImageGrabbed;
            grabProcesserG.Start();
            this.grabProcesser = grabProcesserG;

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.AddImageGrabbed(grabProcesserG.ImageGrabbed);
            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;
            if (additionalSettings.UseAsyncMode)
            {
                imageDeviceHandler.SetTriggerMode(TriggerMode.Software);
                imageDeviceHandler.SetExposureTime(1E6f / additionalSettings.AsyncGrabHz);
            }
            else
                imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);

            CancellationTokenSource token = new CancellationTokenSource();
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            bool good = true;
            if (isTestMode == false)
            // 한장그랩
            {
                //grabCount = 5;
                imageDeviceHandler.GrabMulti();
                simpleProgressForm.Show(() =>
                {
                    while (remainGrabCount > 0)
                    {
                        Thread.Sleep(1000);
                        good = grabProcesserG.WaitFullImageGrabbed(30000);
                        if (good)
                            remainGrabCount--;
                        else
                            remainGrabCount = 0;
                    }
                }, token);
                imageDeviceHandler.Stop();
            }
            else
            {
                // 무한그랩(테스트)
                remainGrabCount = -1;
                bool ok = grabProcesserG.SetTestMode(true, @"D:\GrabTest\");
                if (ok)
                {
                    simpleProgressForm.Show(() =>
                    {
                        //imageDeviceHandler.SetSkipFrame(20410);
                        imageDeviceHandler.GrabMulti();
                        while (token.IsCancellationRequested == false)
                        {
                            //if (this.grabProcesser.IsBusy == false)
                            //{
                            //    while (imageDeviceHandler.IsGrabDone() == false)
                            System.Threading.Thread.Sleep(100);
                        //    //imageDeviceHandler.WaitGrabDone();
                        //}
                    }
                        imageDeviceHandler.Stop();
                    }, token);
                }
            }
            remainGrabCount = 0;

            imageDeviceHandler.Stop();
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);
            imageDeviceHandler.RemoveImageGrabbed(grabProcesserG.ImageGrabbed);

            grabProcesserG.StartInspectionDelegate -= ImageGrabbed;
            grabProcesserG.Stop();
            while (grabProcesserG.IsDisposable() == false)
                Thread.Sleep(10);
            grabProcesserG.Dispose();
            grabProcesserG = null;
            this.grabProcesser = null;

            return good;
        }

        public override void GrabFrame()
        {
            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.First();
            if (calibration == null)
                return;

            float lengthMm = (int)(calibration.ImageSize.Height * calibration.PelSize.Height / 1E3);
            InputForm inputForm = new InputForm(StringManager.GetString("Frame Length? [mm]"), lengthMm.ToString("F1"));
            if (inputForm.ShowDialog() == DialogResult.Cancel)
                return;

            int lengthPx = -1;
            bool ok = float.TryParse(inputForm.InputText, out lengthMm);
            if (ok)
                lengthPx = (int)Math.Round(lengthMm / calibration.PelSize.Height * 1E3);

            if (lengthPx < 0)
            {
                MessageForm.Show(null, StringManager.GetString("Wrong Input"));
                return;
            }

            FrameGrabProcesser frameGrabProcesser = new FrameGrabProcesser(lengthPx);

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.AddImageGrabbed(frameGrabProcesser.ImageGrabbed);
            imageDeviceHandler.GrabMulti();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                while (frameGrabProcesser.IsDone == false && cancellationTokenSource.Token.IsCancellationRequested == false)
                {
                    Thread.Sleep(100);
                    continue;
                }
            }, cancellationTokenSource);

            imageDeviceHandler.RemoveImageGrabbed(frameGrabProcesser.ImageGrabbed);
            imageDeviceHandler.Stop();
            if (frameGrabProcesser.IsDone)
            {
                ImageD grabbedImage = frameGrabProcesser.GetLastSheetImageSet().ToImageD();
                ShowImage(grabbedImage);
            }
            frameGrabProcesser.Dispose();
        }

        object lockObject = new object();
        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            //onGrabbedEvent.Set();
            LogHelper.Debug(LoggerType.Grab, "ModellerPageExtenderG::ImageGrabbed");
            ImageD grabbedImage = null;
            if (imageDevice != null)
            {
                LogHelper.Debug(LoggerType.Grab, string.Format("imageDevice: {0}, ptr: {1}", imageDevice.ToString(), ptr.ToString()));
                grabbedImage = imageDevice.GetGrabbedImage(ptr).Clone();
            }
            else
            {
                SheetImageSet sheetImageSet = grabProcesser.GetLastSheetImageSet();
                LogHelper.Debug(LoggerType.Grab, string.Format("SheetNo: {0} Width: {1} Height:{2}", sheetImageSet.SheetNo, sheetImageSet.Width, sheetImageSet.Height));
                grabbedImage = (Image2D)sheetImageSet.ToImageD();
                sheetImageSet.Dispose();
            }

            //remainGrabCount--;
            if (remainGrabCount < 0)
                return;

            int curGrabId = remainGrabCount;
            //Debug.Assert(grabbedImage.Height < 30120, "Image Height is Too large");
            if (remainGrabCount >= 0)
            {
                Task.Run(() =>
                {
                    Image2D saveImageD = (Image2D)grabbedImage.Clone();
                    string imagePath = SystemManager.Instance().CurrentModel.GetImagePath();
                    int camIdx = SystemManager.Instance().ExchangeOperator.GetCamIndex();
                    string imageName = SystemManager.Instance().CurrentModel.GetImageName(camIdx, curGrabId, 0);
                    saveImageD.SaveImage(Path.Combine(imagePath, imageName));
                });
            }
            if(curGrabId==0)
                ShowImage(grabbedImage);
        }

        private void ShowImage(ImageD imageD)
        {
            lock (lockObject)
            {
                this.currentImage?.Dispose();
                this.currentImage = (Image2D)imageD.Clone();

                if (UpdateImage != null)
                    UpdateImage(this.currentImage, false);

            }
        }

        protected override void TeachBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Trainer trainer = (Trainer)AlgorithmPool.Instance().GetAlgorithm(Trainer.TypeName);

            if (trainer == null)
                return;

            if (currentImage == null)
            {
                e.Result = new Exception("There is no Image");
                return;
            }

            trainer.Teach((BackgroundWorker)sender, currentImage, e);
        }

        protected override void TeachRunWorkerCompleted(bool result)
        {
            if (result == true)
            {
                if (currentImage != null)
                {
                    SystemManager.Instance().CurrentModel.IsTrained = true;
                    SystemManager.Instance().CurrentModel.Modified = true;
                    SaveModel();
                    //SystemManager.Instance().ExchangeOperator.ModelTeachDone();
                }
            }
        }

        public override void Inspect(RegionInfo regionInfo)
        {
            if (currentImage == null)
                return;

            CalculatorBase calculator = AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName) as CalculatorBase;
            Detector detector = AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName) as Detector;
            RegionInfoG regionInfoG = regionInfo as RegionInfoG;
            if (calculator == null || detector == null || regionInfoG == null)
                return;

            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, PathSettings.Instance().Temp);

            AlgoImage algoImage = null;
            ProcessBufferSetG bufferSet = null;
            bool isMultiLayerBuffer = Settings.AdditionalSettings.Instance().IsMultiLayerBuffer;
            float scaleFactorF = SystemManager.Instance().CurrentModel.ScaleFactorF;

            try
            {
                SimpleProgressForm readyForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Ready"));
                readyForm.Show(() =>
                {
                    Image2D clipImageD = (Image2D )currentImage.ClipImage(regionInfoG.Region);
                    algoImage = ImageBuilder.Build(CalculatorBase.TypeName, currentImage, ImageType.Grey);
                    bufferSet = calculator.CreateProcessingBuffer(scaleFactorF, isMultiLayerBuffer, currentImage.Width, currentImage.Height);
                    bufferSet.BuildBuffers();

                    clipImageD.Dispose();

                    CalculatorParam calculatorParam = calculator.Param as CalculatorParam;
                    DetectorParam detectorParam = detector.Param as DetectorParam;
                });

                //dynThresholdImage.Save(@"d:\temp\tt.bmp");
                InspectionResult inspectionResult = new InspectionResult();
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                Stopwatch sw = new Stopwatch();
                SimpleProgressForm inspectorForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Inspect"));
                inspectorForm.Show(new Action(() =>
                {
                    SheetInspectParam inspectParam = new SheetInspectParam(currentImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, debugContext);
                    inspectParam.AlgoImage = algoImage;
                    inspectParam.ProcessBufferSet = bufferSet;
                    inspectParam.RegionInfo = regionInfoG;
                    inspectParam.CancellationToken = cancellationTokenSource.Token;

                    //bufferSet.DynamicThreshold.Save(@"d:\temp\tt.bmp");
                    sw.Start();
                    inspectionResult.AlgorithmResultLDic.Add(calculator.GetAlgorithmType(), calculator.Inspect(inspectParam));
                    inspectionResult.AlgorithmResultLDic.Add(detector.GetAlgorithmType(), detector.Inspect(inspectParam));
                    sw.Stop();
                }), cancellationTokenSource);

                if (cancellationTokenSource.IsCancellationRequested)
                {
                    SimpleProgressForm waitForm = new SimpleProgressForm("");
                    waitForm.Show(() => inspectorForm.Task.Wait());
                }

                this.InspectionResult = inspectionResult;
                SheetResult sr = inspectionResult.AlgorithmResultLDic[detector.GetAlgorithmType()] as SheetResult;

                UpdateSheetResult(sr);
            }
#if DEBUG == false
            catch(Exception ex)
            {
                MessageForm.Show(null, ex.Message);
            }
#endif
            finally
            {
                algoImage.Dispose();
                bufferSet.Dispose();
            }

        }

        public override void Inspect()
        {
            if (currentImage == null)
                return;

            CalculatorBase calculator = AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName) as CalculatorBase;
            Algorithm detector = AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName);
            if (calculator == null || detector == null)
                return;

            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, PathSettings.Instance().Temp);

            AlgoImage fullImage = null, scaleImage = null;
            ProcessBufferSetG bufferSet = null;
            bool isMultiLayerBuffer = Settings.AdditionalSettings.Instance().IsMultiLayerBuffer;
            float scaleFactorF = SystemManager.Instance().CurrentModel.ScaleFactorF;

            try
            {
                SimpleProgressForm readyForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Ready"));
                bool readyOk = false;
                readyForm.Show(() =>
                {
                    fullImage = BuildTestInspectionImage(currentImage);
                    //fullImage = ImageBuilder.Build(Calculator.TypeName, currentImage);

                    CalculatorParam calculatorParam = calculator.Param as CalculatorParam;
                    DetectorParam detectorParam = detector.Param as DetectorParam;

                    UniScanG.Data.Model.Model curModel = SystemManager.Instance().CurrentModel;
                    bufferSet = calculator.CreateProcessingBuffer(scaleFactorF, isMultiLayerBuffer, fullImage.Width, fullImage.Height);
                    bufferSet.BuildBuffers();

                    calculator.PrepareInspection();
                    detector.PrepareInspection();
                    readyOk = true;
                });

                if (readyOk == false)
                {
                    MessageForm.Show(null, "There is No Teach data.");
                    return;
                }
                //dynThresholdImage.Save(@"d:\temp\tt.bmp");

                InspectionResult inspectionResult =  new InspectionResult();
                inspectionResult.InspectionStartTime = DateTime.Now;

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                Stopwatch sw = new Stopwatch();
                SimpleProgressForm inspectorForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Inspect"));
                inspectorForm.Show(new Action(() =>
                {
                    SheetInspectParam inspectParam = new SheetInspectParam(currentImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, debugContext);
                    inspectParam.AlgoImage = fullImage;
                    inspectParam.TestInspect = true;
                    inspectParam.ProcessBufferSet = bufferSet;

                    inspectParam.CancellationToken = cancellationTokenSource.Token;


                    sw.Start();

                    inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Upload"));
                    Stopwatch swUpload = Stopwatch.StartNew();
                    bufferSet.PreCalculate(fullImage);
                    Debug.WriteLine(string.Format("Upload: {0}[ms]", swUpload.ElapsedMilliseconds));

                    inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Calculate"));
                    Stopwatch swCalc = Stopwatch.StartNew();
                    inspectionResult.AlgorithmResultLDic.Add(calculator.GetAlgorithmType(), calculator.Inspect(inspectParam));
                    Debug.WriteLine(string.Format("Calculate: {0}[ms]", swCalc.ElapsedMilliseconds));
                    inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Download"));
                    Stopwatch swDownload = Stopwatch.StartNew();
                    bufferSet.PostCalculate();
                    Debug.WriteLine(string.Format("Download: {0}[ms]", swDownload.ElapsedMilliseconds));

                    inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Detect"));
                    Stopwatch swDetect = Stopwatch.StartNew();
                    inspectionResult.AlgorithmResultLDic.Add(detector.GetAlgorithmType(), detector.Inspect(inspectParam));
                    Debug.WriteLine(string.Format("Detect: {0}[ms]", swDetect.ElapsedMilliseconds));

                    sw.Stop();
                    inspectionResult.InspectionEndTime = DateTime.Now;

                    inspectorForm.SetLabelMessage(StringManager.GetString(this.GetType().FullName, "Done"));
                    Debug.WriteLine(string.Format("Total: {0}[ms]", sw.ElapsedMilliseconds));

                    calculator.ClearInspection();
                    detector.ClearInspection();
                    Thread.Sleep(1000);
                }), cancellationTokenSource);

                if (cancellationTokenSource.IsCancellationRequested)
                {
                    SimpleProgressForm waitForm = new SimpleProgressForm();
                    waitForm.Show(() => inspectorForm.Task.Wait());
                }

                this.InspectionResult = inspectionResult;
                SheetResult sr = inspectionResult.AlgorithmResultLDic[detector.GetAlgorithmType()] as SheetResult;
                sr.SpandTime = sw.Elapsed;
                UpdateSheetResult(sr);
            }
//#if DEBUG == false
            catch(Exception ex)
            {
                MessageForm.Show(null, ex.Message);
            }
//#endif
            finally
            {
                scaleImage?.Dispose();
                fullImage?.Dispose();
                bufferSet?.Dispose();
            }

            //SizeF offset = new SizeF();
            //ProcessBufferSetS bufferSet = new ProcessBufferSetS(SheetInspector.TypeName, currentImage.Width, currentImage.Height);

            //FiducialFinderAlgorithmResult finderResult = new FiducialFinderAlgorithmResult();
            //if (AlgorithmSetting.Instance().IsFiducial == true)
            //{
            //    SimpleProgressForm fiducialForm = new SimpleProgressForm("Find Fiducial");
            //    fiducialForm.Show(new Action(() =>
            //    {
            //        FiducialFinder fiducialFinder = (FiducialFinder)AlgorithmPool.Instance().GetAlgorithm(FiducialFinder.TypeName);
            //        SheetInspectParam inspectParam = new SheetInspectParam(currentImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
            //        inspectParam.ClipImage = currentImage;
            //        inspectParam.ProcessBufferSet = bufferSet;
            //        finderResult = (FiducialFinderAlgorithmResult)fiducialFinder.Inspect(inspectParam);
            //        offset = finderResult.OffsetFound;
            //    }));
            //}


            //SheetResult sheetResult = new SheetResult();

            //SimpleProgressForm inspectorForm = new SimpleProgressForm("Inspect");
            //inspectorForm.Show(new Action(() =>
            //{
            //    SheetInspector sheetInspector = (SheetInspector)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName);
            //    SheetInspectParam inspectParam = new SheetInspectParam(currentImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
            //    inspectParam.ClipImage = currentImage;
            //    inspectParam.ProcessBufferSet = bufferSet;
            //    inspectParam.FidOffset = finderResult.OffsetFound;
            //    stopwatch.Start();
            //    sheetResult = (SheetResult)sheetInspector.Inspect(inspectParam);
            //    stopwatch.Stop();
            //}));


            //bufferSet.Dispose();
        }

        private AlgoImage BuildTestInspectionImage(Image2D currentImage)
        {
            AlgorithmStrategy algorithmStrategy = AlgorithmBuilder.GetStrategy(Detector.TypeName);
            return ImageBuilder.Build(algorithmStrategy.LibraryType, currentImage, ImageType.Grey);
        }

        public override void DataExport()
        {
            if (InspectionResult == null)
                return;

            SimpleProgressForm form = new SimpleProgressForm();
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            form.Show(() =>
            {
                InspectionResult.ResultPath = Path.Combine(SystemManager.Instance().CurrentModel.ModelPath, DateTime.Now.ToString("yy-MM-dd hh-mm-ss"));
                SystemManager.Instance().ExportData(InspectionResult, cancellationTokenSource);
                Process.Start(InspectionResult.ResultPath);
            }, cancellationTokenSource);
            form.Task.Wait();
        }

        public override void AutoTeachProcess()
        {
            if (SystemManager.Instance().ExchangeOperator.GetClientIndex() > 0)
                return;

            SystemState.Instance().SetTeach();

            try
            {
                // 그랩
                int imageHeight = this.currentImage.Size.Height;
                int lower = (int)Math.Round(imageHeight * 0.9f);
                int upper = (int)Math.Round(imageHeight * 1.1f);
                do
                {
                    bool grabOk = this.GrabSheet(1);
                    if (grabOk == false)
                        throw new Exception("Sheet Grab Fail");

                    imageHeight = this.currentImage.Size.Height;
                } while (!(lower < imageHeight && imageHeight < upper));

                // 티칭
                this.Teach(true);

                SystemState.Instance().SetIdle();
            }
            catch (Exception ex)
            {
                SystemState.Instance().SetAlarm();
                MessageForm.Show(null, ex.Message);
                SystemState.Instance().SetIdle();
            }
        }
    }

}