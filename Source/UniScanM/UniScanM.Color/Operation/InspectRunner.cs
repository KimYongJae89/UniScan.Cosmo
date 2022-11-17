﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


using DynMvp.Base;
using DynMvp.Devices;
//using DynMvp.InspData;
using DynMvp.Devices.Light;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Vision;
using DynMvp.UI.Touch;

using UniEye.Base.Data;

using UniEye.Base.Settings;
using UniEye.Base.Inspect;

using UniScanM.Algorithm;

using UniScanM.ColorSens.Algorithm;
using UniScanM.ColorSens.Settings;
using UniScanM.ColorSens.Data;

using System.Collections.Concurrent;
using System.Threading;
using UniScanM.UI.MenuPage.AutoTune;
using DynMvp.UI;
using System.ComponentModel;
using DynMvp.Data;

namespace UniScanM.ColorSens.Operation
{
    public class InspectRunner : UniScanM.Operation.InspectRunner
    {
        private InspectSheetDeltaBrightness inspector_SheetBrightness = null;

        int inspectCount = 0;
        //ThreadHandler runningThreadHandler = null;  // 검사시 While 돌릴 쓰레드.

        double startSheetPosition = 0;

        public InspectRunner() : base()
        {
            SystemManager.Instance().InspectStarter.OnStartInspection = EnterWaitInspection;
            SystemManager.Instance().InspectStarter.OnStopInspection = ExitWaitInspection;
            SystemManager.Instance().InspectStarter.OnRewinderCutting = OnRewinderCutting;
            SystemManager.Instance().InspectStarter.OnLotChanged = OnLotChanged;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        void OnLotChanged()
        {
            inspectCount = SystemManager.Instance().ProductionManager.CurProduction.LastInspectionNo+1;// 검사 인덱스 초기화
        }

        void OnRewinderCutting()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }

        void Setup_Start_Condition()
        {
            //02. Load Lot No
            //PLC 연결 되었을때와 , 그렇지 않을시 모델정보.
            string model = SystemManager.Instance().InspectStarter.GetModelName();
            string lot = SystemManager.Instance().InspectStarter.GetLotNo();
            string worker = SystemManager.Instance().InspectStarter.GetWorker();
            string paste = SystemManager.Instance().InspectStarter.GetPaste();
            string mode = SystemManager.Instance().InspectStarter.StartMode.ToString();
            int rewinderSite = SystemManager.Instance().InspectStarter.GetRewinderSite();
            int position = SystemManager.Instance().InspectStarter.GetPosition();

            //int sameCount = SystemManager.Instance().ProductionManager.LotExistCount(DateTime.Now, model, worker, lot, paste, mode, rewinderSite);
            //if (sameCount > 0)
            //    lot = string.Format("{0}_{1}", lot, sameCount);
            //SystemManager.Instance().ProductionManager.LotChange(model, worker, lot, paste, mode, rewinderSite, position);
            //SystemManager.Instance().MainForm.OnLotChanged();

            // 03
            //SystemManager.Instance().CurrentModel.Name = inspectStarter.GetMachineState().ModelName;
            //SystemManager.Instance().MainForm.OnModelChanged();
            //SystemManager.Instance().MainForm.WorkerChanged(SystemManager.Instance().InspectStarter.GetWorker());

            //0. PLC 정보 가져오기 (사용자, 모델, 페이스트, 
            startSheetPosition = SystemManager.Instance().InspectStarter.GetPosition();

            UniScanM.ColorSens.Data.Model curmodel = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;
            if (curmodel == null) return;

            ColorSensorParam colorSensorParam = (ColorSensorParam)curmodel.InspectParam;
            if (colorSensorParam == null) return;
            //if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto)
            //{
            //    // Update Roller Dia from PLC when Auto-Mode
            //    double rollerDia = SystemManager.Instance().InspectStarter.GetRollerDia();
            //    if (rollerDia > 0)
            //    {
            //        rollerDia = rollerDia < 50 ? 50 : rollerDia;
            //        rollerDia = rollerDia > 200 ? 200 : rollerDia;
            //        colorSensorParam.PatternPeriod = rollerDia * Math.PI;
            //    }
            //    SystemManager.Instance().ModelManager.SaveModel(curmodel);
            //}

            double PatternPeriod = 330;
            double lineSpeed = 50;
            //04. 알고리즘 설정  
            PatternPeriod = colorSensorParam.PatternPeriod;


            //inspector_SheetBrightness = new InspectSheetBrightness(
            //    curmodel.InspectParam.AverageBrightnessSheet,
            //    PatternPeriod,
            //    ConfigColorSensor.Instance().OpticResolution_umPerPixel);

            inspector_SheetBrightness = new InspectSheetDeltaBrightness(
               PatternPeriod,
               ColorSensorSettings.Instance().OpticResolution_umPerPixel);

            //05. 라인속도에 따른 카메라설정
            lineSpeed = SystemManager.Instance().InspectStarter.GetLineSpeed();
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            Camera camera = (Camera)imageDeviceHandler.GetImageDevice(0);
            if (camera != null)
            {
                camera.SetExposureTime((float)100.0); //us 130um@100m/min
                camera.SetTriggerMode(TriggerMode.Software);
                double Hz = lineSpeed * 1000000 / 60 / ColorSensorSettings.Instance().OpticResolution_umPerPixel;
                if (Hz > 0)
                    camera.SetAcquisitionLineRate((float)Hz);//7092.1985(100m/min)
            }
        }

        autoLightTune lighttune;
        FinderSheetPeriod brightCtrl;
        SimpleProgressForm progressForm;
        
        public override bool EnterWaitInspection()
        {                                                                  
            inspectCount = SystemManager.Instance().ProductionManager.CurProduction.Total;

            LogHelper.Debug(LoggerType.Function, "InspectRunner::EnterWaitInspection");
            //if (SystemManager.Instance().CurrentModel == null)
            //    SystemManager.Instance().LoadDefaultModel();

            SystemState.Instance().SetInspect();

            //2
            Setup_Start_Condition();

            //3
            ClearImageBuffer();

            //4 UI
            //SystemState.Instance().SetIdle();
            SystemManager.Instance().MainForm?.InspectPage?.InspectionPanelList.ForEach(panel => panel.EnterWaitInspection());

            //start inspection Thread !!!
            //runningThreadHandler = new ThreadHandler("InspectRunner", new Thread(Thread_Inspect), false);
            //requestStop = false;
            //runningThreadHandler.Start();

            UniScanM.ColorSens.Data.Model curmodel = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;
            if (curmodel == null) return false;

            ColorSensorParam colorSensorParam = (ColorSensorParam)curmodel.InspectParam;
            if (colorSensorParam == null) return false;
            
            if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto)
            {
                // Update Roller Dia from PLC when Auto-Mode
                double rollerDia = SystemManager.Instance().InspectStarter.GetRollerDia();
                if (rollerDia > 0)
                {
                    rollerDia = rollerDia < 50 ? 50 : rollerDia;
                    rollerDia = rollerDia > 200 ? 200 : rollerDia;
                    colorSensorParam.PatternPeriod = rollerDia * Math.PI;
                    SystemManager.Instance().ModelManager.SaveModel(curmodel);
                }

                progressForm = new SimpleProgressForm();
                progressForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                progressForm.TopMost = true;
                progressForm.Text = "Tune";
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                SystemState.Instance().SetInspectState(InspectState.Tune);
                progressForm.Show(() =>
                {
                    lighttune = new autoLightTune(0.01);
                    brightCtrl = new FinderSheetPeriod();
                    foreach (ImageDevice imageDevice in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
                        imageDevice.ImageGrabbed = TuneImageGrabbed;
                    colorSensorParam.LightValue = (uint)lighttune.tune();

                    //colorSensorParam.AverageBrightnessSheet = brightCtrl.CalculateOnePatternBright(colorSensorParam.PatternPeriod);
                    SystemManager.Instance().ModelManager.SaveModel(curmodel);

                }, cancellationTokenSource);

                if (cancellationTokenSource.IsCancellationRequested)
                    return false;

                foreach (ImageDevice imageDevice in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
                    imageDevice.ImageGrabbed = null;
            }

            if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop)
                return false;

            return PostEnterWaitInspection();
        }

        protected void TuneImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::ImageGrabbed");

            ImageD imageD = imageDevice.GetGrabbedImage(ptr);
            Data.InspectionResult result = inspectRunnerExtender.BuildInspectionResult() as Data.InspectionResult;
            result.DisplayBitmap = imageD.ToBitmap();
            result.SetSkip();

            float lightTunePercentage = (float)lighttune.CurTryCount / (float)lighttune.TryCount * 100.0f;
            if (lightTunePercentage != 100)
                progressForm.MessageText = string.Format("Light Tune : {0:0.0} %", lightTunePercentage);
            else
                progressForm.MessageText = string.Format("Calc Value : {0:0.0} %", (float)brightCtrl.GrabbedCount / (float)brightCtrl.TargetGrabCount * 100.0f);
            
            SystemManager.Instance().MainForm.InspectPage.ProductInspected(result);
        }

        public override bool PostEnterWaitInspection()
        {
            //07. 원시트 분리기 
            //((GrabProcesserG)this.grabProcesser).Algorithm_sheetfinder = new SheetFinder_whole(); //AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName);
            //if (grabProcesser != null) ((GrabProcesserG)grabProcesser).Start();

            //09. Start Camera Grab
            LightTurnOn();

            foreach (ImageDevice imageDevice in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
                imageDevice.ImageGrabbed = ImageGrabbed;

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();

            SystemState.Instance().SetInspectState(InspectState.Run);

            //SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();

            return true;
        }


        public override void ExitWaitInspection()
        {
            //1. Camera Off
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler?.Stop();
            //2. Light Off
            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            lightCtrlHandler?.TurnOff();

            base.ExitWaitInspection();

            SystemState.Instance().SetIdle();
            
            SystemState.Instance().SetInspectState(InspectState.Stop); 
        }

        private bool requestStop = false;
        public override void PreExitWaitInspection()
        {
            requestStop = true;
            base.PreExitWaitInspection();

            //if (runningThreadHandler != null)
            //{
            //    SimpleProgressForm form = new SimpleProgressForm();
            //    form.Show(() => runningThreadHandler.Stop());
            //}

            ClearImageBuffer();
        }

        private void LightTurnOn()
        {
            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            // lightCtrlHandler.TurnOn(SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0]);
            //this.LightParamSet.LightParamList[0].LightValue.Value[0] = (int)colorSensorParam.LightValue;

            if (SystemManager.Instance().DeviceBox.LightCtrlHandler.Count > 0)
            {
                UniScanM.ColorSens.Data.Model model = SystemManager.Instance().CurrentModel as UniScanM.ColorSens.Data.Model;

                model.LightParamSet.LightParamList[0].LightValue.Value[0] = (int)model.InspectParam.LightValue;
                lightCtrlHandler.TurnOn(model.LightParamSet.LightParamList[0]);
                Thread.Sleep(100);
            }
        }

        ConcurrentQueue<AlgoImage> grabbedImageQueue = new ConcurrentQueue<AlgoImage>();
        static int ImageGrabCount = 0;
        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::ImageGrabbed");

            ImageD imageD = imageDevice.GetGrabbedImage(ptr);
            Image2D imgBuf = (Image2D)imageD.Clone(); 
            imgBuf.Tag = imageD.Tag;
            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imgBuf, ImageType.Grey);

            grabbedImageQueue.Enqueue(algoImage);

            DeviceImageSet imgdevset = new DeviceImageSet(imgBuf);
            this.Inspect(null, ptr, null, null);

            //Task workingTask = new Task(new Action(Inspect2));
            //workingTask.Start();

            //DynMvp.InspData.InspectionResult inspectionResult = BuildInspectionResult();
            //if (inspectionResult == null)        return;
            //InspectionOption inspectionOption = new InspectionOption(imageDevice);
            //this.Inspect(imageDevice, ptr, inspectionResult, inspectionOption);

            //string debugImagePath = Path.Combine(@"d:\", "CameraGrabImage", "Frame");
            //string debugImageFile = Path.Combine(debugImagePath, string.Format("Frame {0}.bmp", ImageGrabCount));
            //imgBuf.SaveImage(debugImageFile, ImageFormat.Bmp);

            //((GrabProcesserG)grabProcesser).ImageGrabbed(imageDevice, ptr);
            ImageGrabCount++;
            return;
            //Bitmap bitmap = imgBuf.ToBitmap();
            //ImageHelper.SaveImage(bitmap, "d:\\image.bmp", ImageFormat.Bmp);
            //bitmap.Dispose();

        }

        void ClearImageBuffer()
        {
            if (grabbedImageQueue.IsEmpty == false)
            {
                AlgoImage temp;
                while (grabbedImageQueue.TryDequeue(out temp)) ;
            }
            GC.Collect();
            GC.WaitForFullGCComplete();
        }


        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, DynMvp.InspData.InspectionResult inspectionResult, InspectionOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Operation, "InspectRunner::Inspect");
            AlgoImage algoImage = null;
            try
            {
                if (grabbedImageQueue.TryDequeue(out algoImage) ==false)
                {
                    Thread.Sleep(0);
                    return;
                }

                inspectionResult = BuildInspectionResult();
                if (inspectionResult == null) return;

                Debug.Assert(algoImage != null);
                inspectRunnerExtender.OnPreInspection();
                {
                    ColorSens.Data.InspectionResult colorInspectionResult = (ColorSens.Data.InspectionResult)inspectionResult;
                    UniScanM.ColorSens.Data.Model model = (UniScanM.ColorSens.Data.Model)(SystemManager.Instance().CurrentModel);

                    //1. 컬러센서 검사조건도 같이 
                    colorInspectionResult.DisplayBitmap = algoImage.ToImageD().ToBitmap();

                    ColorSensorParam param = (ColorSensorParam)model.InspectParam;
                    colorInspectionResult.SetInspectionParameter(ref param);

                    //2. 검사 & 결과 정리
                    bool bValid = inspector_SheetBrightness.AddImage(algoImage); //한주기성부이 다 쌓여야정상데이터
                    if (bValid)
                    {
                        //SystemState.Instance().SetInspectState(InspectState.Run);
                        float[] result = inspector_SheetBrightness.Inspect(colorInspectionResult);
                        colorInspectionResult.SetLocalBrightness(result);
                        colorInspectionResult.SheetBrightness = result.Average();
                        colorInspectionResult.GrabImageList.Add(algoImage.ToImageD());
                        colorInspectionResult.Judge();
                        //최초시작시간에서 진행되는 거리 계산.
                        //TimeSpan time = colorInspectionResult.InspectionStartTime - SystemManager.Instance().ProductionManager.CurProduction.StartTime;
                        //double lineSpeed = SystemManager.Instance().InspectStarter.GetLineSpeed();
                        //colorInspectionResult.RollDistance = (int)(startSheetPosition + (time.TotalSeconds * lineSpeed / 60));

                        //ㄴ
                        //colorInspectionResult.InspectionCount = ++inspectCount;
                        colorInspectionResult.InspectionNo = inspectCount.ToString();
                        inspectCount++;

                        //3. 영상 메인화면에 뿌리기.
                        //DeviceImageSet imgdevset = new DeviceImageSet(imgBuf);
                        //SystemManager.Instance().MainForm.MonitoringPage.InspectionPanel.UpdateImage(imgdevset, 0, inspectionResult);
                        algoImage.Dispose();

                        inspectRunnerExtender.OnPostInspection(); // do nothing

                        //4. 최종 결과 InspectPage, PLC, CSV 보내기.
                        ProductInspected(inspectionResult);
                    }
                    else
                    {
                        inspectionResult.SetSkip();
                        //SystemState.Instance().SetInspectState(InspectState.Tune);
                        ProductInspected(inspectionResult);//화면 갱신용
                    }
                }
            }
            catch(Exception ex)
            { 
                LogHelper.Error(LoggerType.Inspection, ex.Message);
                //inspectionResult.SetDefect();
            }
            finally
            {
                algoImage?.Dispose();
                //SystemState.Instance().SetInspectState(InspectState.Wait);
            }
        }
        public void Inspect2()
        {
            DynMvp.InspData.InspectionResult inspectionResult = null;
            LogHelper.Debug(LoggerType.Operation, "InspectRunner::Inspect");
            SystemState.Instance().SetInspectState(InspectState.Run);
            AlgoImage algoImage = null;
            try
            {
                if (grabbedImageQueue.TryDequeue(out algoImage) == false)
                {
                    Thread.Sleep(0);
                    return;
                }

                inspectionResult = BuildInspectionResult();
                if (inspectionResult == null) return;

                Debug.Assert(algoImage != null);
                inspectRunnerExtender.OnPreInspection();
                {
                    ColorSens.Data.InspectionResult colorInspectionResult = (ColorSens.Data.InspectionResult)inspectionResult;
                    UniScanM.ColorSens.Data.Model model = (UniScanM.ColorSens.Data.Model)(SystemManager.Instance().CurrentModel);

                    //1. 컬러센서 검사조건 결과도 같이 넘김
                    ColorSensorParam param = (ColorSensorParam)model.InspectParam;
                    colorInspectionResult.SetInspectionParameter(ref param);

                    //2. 검사 & 결과 정리
                    inspector_SheetBrightness.AddImage(algoImage);
                    float[] result = inspector_SheetBrightness.Inspect(colorInspectionResult);
                    //colorInspectionResult.SetLocalBrightness(result);
                    //colorInspectionResult.SheetBrightness = result.Average();
                    //colorInspectionResult.Judge();
                    colorInspectionResult.GrabImageList.Add(algoImage.ToImageD());

                    //ㄴ
                    //colorInspectionResult.InspectionCount = ++inspectCount;
                    colorInspectionResult.InspectionNo = (++inspectCount).ToString();

                    //3. 영상 메인화면에 뿌리기.
                    //DeviceImageSet imgdevset = new DeviceImageSet(imgBuf);
                    //SystemManager.Instance().MainForm.MonitoringPage.InspectionPanel.UpdateImage(imgdevset, 0, inspectionResult);
                    algoImage.Dispose();
                }
                inspectRunnerExtender.OnPostInspection();

                //4. 최종 결과 InspectPage, PLC, CSV 보내기.
                ProductInspected(inspectionResult);

            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, ex.Message);
                //inspectionResult.SetDefect();
            }
            finally
            {
                algoImage?.Dispose();
                //SystemState.Instance().SetInspectState(InspectState.Wait);
            }
        }

        //private void Thread_Inspect()
        //{
        //    while (runningThreadHandler.RequestStop == false)
        //    {
        //        if(grabbedImageQueue.IsEmpty == true)
        //        {
        //            GC.Collect();
        //            GC.WaitForFullGCComplete();
        //            Thread.Sleep(0);
        //            continue;
        //        }
        //        IntPtr ptr= new IntPtr(0);
        //        this.Inspect(null, ptr, null, null);
        //    }
        //}

        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::ProductInspected");
            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = (inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime);

            // Update UI
            InspectionResult colorSensorInspectionResult = (InspectionResult)inspectionResult;
            //SystemManager.Instance().MainForm.MonitoringPage.InspectionPanel.UpdateImage(new DeviceImageSet((Image2D)inspectionResult2.DisplayImageD), 0, inspectionResult);
            SystemManager.Instance().MainForm.InspectPage.ProductInspected(inspectionResult);
            
            //Save CSV   
            // Save Data or Send data to PLC
            //5. 양부 판정 PLC로 보내기
            if (inspectionResult.Judgment != DynMvp.InspData.Judgment.Skip)
            {
                this.UpdateProduction(colorSensorInspectionResult);
                //UniScanM.Data.Production p = SystemManager.Instance().ProductionManager.GetProduction(colorSensorInspectionResult);
                //lock(p)
                //    p.Update(colorSensorInspectionResult);

                SystemManager.Instance().ExportData(inspectionResult);
             }
        }
    }
}
