//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Windows.Forms;

//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.Data.UI;
//using DynMvp.Devices;
//using DynMvp.Devices.Comm;
//using DynMvp.Devices.Light;
//using System.IO;
//using DynMvp.Inspection;
//using DynMvp.InspData;
//using DynMvp.UI.Touch;
//using UniEye.Base.Settings;
//using DynMvp.UI;
//using UniEye.Base.Data;
//using UniEye.Base;

//using System.Net;
//using System.Net.Sockets;
//using System.Xml;
//using UniScanG.Operation.UI;
//using DynMvp.Devices.FrameGrabber;
//using System.Text;
//using System.Threading;
//using Infragistics.Win.Misc;
//using System.Runtime.InteropServices;
//using System.Threading.Tasks;
//using UniScanG.Operation.UI.Manual;
//using UniScanG.Algorithms;
//using DynMvp.Vision;
//using System.Diagnostics;

//namespace UniScanG.Temp
//{
//    using Data;
//    using UniEye.Base.UI;
//    using SimpleResultList = SortedList<int, Data.SimpleInspectionResult>;

//    public partial class MonitoringPageV2 : UserControl, IMainTabPage, IMonitoringPage
//    {
//        MonitoringPageBase monitoringPageBase = new MonitoringPageBase();

//        DrawBox cameraDrawBox = null;
//        Production[] camProduction = new Production[2];

//        public MonitoringPageV2()
//        {
//            LogHelper.Debug(LoggerType.StartUp, "Begin Constructor MonitoringPageV2");
//            InitializeComponent();

//            cameraDrawBox= new DrawBox();
//            cameraDrawBox.Dock = DockStyle.Fill;
//            cameraPanel.Controls.Add(cameraDrawBox);

//            buttonStart.Enabled = true;
//            buttonStop.Enabled = false;
//        }

//        public void EnableControls()
//        {
//            throw new NotImplementedException();
//        }

//        public void InspectionDone(Client client, string resultPath)
//        {
//            InspectorInfo inspectorInfo = UniScanGSettings.Instance().ClientInfoList.Find(f => f.CamIndex == client.CamIndex && f.ClientIndex == client.ClientIndex);
//            MonitorInfo monitorInfo = UniScanGSettings.Instance().MonitorInfo;

//            if (inspectorInfo == null)
//                return;

//            Task task = Task.Factory.StartNew((Action)(() =>
//            {
//                LogHelper.Debug(LoggerType.Inspection, string.Format("MonitoringPageV2::InspectionDone::Task Start"));

//                // 검사기로부터 결과 로드
//                this.monitoringPageBase.LoadRemoteInspectionResult(inspectorInfo, resultPath);
                
//                // 0 - 검사 완료되어 Done신호를 받은 결과. 
//                // 1 - 이전에 검사되어 Done을 받았으나, 짝을 찾지 못해 Merge하지 못한 결과.
//                SimpleResultList simpleResultList0 = this.monitoringPageBase.GetSimpleResultList(client.CamIndex);
//                SimpleResultList simpleResultList1 = this.monitoringPageBase.GetSimpleResultList(1 - client.CamIndex);

//                SimpleInspectionResult simpleResult0 = simpleResultList0.Last().Value;
//                if (simpleResultList1.ContainsKey(simpleResult0.SheetNo))
//                {
//                    SimpleInspectionResult simpleResult1 = simpleResultList1[simpleResult0.SheetNo];
//                    //SimpleResult simpleResult1 = simpleResultList0[simpleResult0.SheetNo];
//                    InspectionResult inspectionResultCam1 = (Data.InspectionResult)((client.CamIndex == 0) ? simpleResult0.InspectionResult : simpleResult1.InspectionResult);
//                    InspectionResult inspectionResultCam2 = (Data.InspectionResult)((client.CamIndex == 1) ? simpleResult0.InspectionResult : simpleResult1.InspectionResult);
//                    InspectionResult inspectionResult = MergeResult(inspectionResultCam1, inspectionResultCam2, monitorInfo.OverlapAreaPx);

//                    this.monitoringPageBase.AddProductionInfo(-1, inspectionResult);

//                    UpdateProductionInfo();
//                    UpdateChart();
//                    UpdateCameraPanelFigure(inspectionResult);

//                    inspectionResultCam1.Clear();
//                    inspectionResultCam2.Clear();
//                    inspectionResult.Clear();
//                }
//                LogHelper.Debug((LoggerType)LoggerType.Operation, string.Format("MonitoringPageV2::InspectionDone::Task End"));
//            }));
            
//            LogHelper.Debug(LoggerType.Inspection, string.Format("MonitoringPageV2::InspectionDone - Inspector {0}{1}", client.CamIndex + 1, (char)(client.ClientIndex + 'A')));
//            //throw new NotImplementedException();
//        }

//        private void UpdateChart()
//        {
            
//        }

//        private void UpdateCameraPanelFigure(InspectionResult inspectionResult)
//        {
//            FigureGroup figureGroup = new FigureGroup();
//            inspectionResult.AppendResultFigures(figureGroup, FigureDrawOption.Default);
//            figureGroup.Scale(.1f, .1f);
//            cameraDrawBox.FigureGroup = figureGroup;
//            cameraDrawBox.Invalidate();
//        }

//        public void StartInspect()
//        {
//            if (this.monitoringPageBase.StartInspect())
//            {
//                buttonStart.Enabled = false;
//                buttonStop.Enabled = true;
//            }
//        }

//        public void StopInspection()
//        {
//            this.monitoringPageBase.StopInspection();

//            buttonStart.Enabled = true;
//            buttonStop.Enabled = false;

//        }

//        public void TabPageVisibleChanged(bool visibleFlag)
//        {
//            if (visibleFlag)
//            {
//                UpdateCameraPanelImage();
//                UpdateProductionInfo(-1);
//            }
//        }

//        private void UpdateCameraPanelImage()
//        {
//            //Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;
//            //if (model == null)
//            //    return;

//            //SheetCheckerParam param1 = model.GetVisionProbe(0).InspAlgorithm.Param as SheetCheckerParam;
//            //Image2D image1 = param1.TrainerParam.RefferenceImage;
//            //if(image1==null)
//            //    image1 = param1.TrainerParam.InspectRegionInfoImage;
//            ////image1.SaveImage(@"D:\temp\tt1.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

//            //SheetCheckerParam param2 = model.GetVisionProbe(1).InspAlgorithm.Param as SheetCheckerParam;
//            //Image2D image2 = param2.TrainerParam.RefferenceImage;
//            //if (image2 == null)
//            //    image2 = param2.TrainerParam.InspectRegionInfoImage;
//            //image2.SaveImage(@"D:\temp\tt2.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

//            // Temp - 이미지 중 하나가 없으면 다른 하나로 대체
//            // 둘 다 없으면 노답
//            //if (image1 == null && image2 == null)
//            //    return;
//            //else if (image1 == null)
//            //    image1 = image2;
//            //else if (image2 == null)
//            //    image2 = image1;

//            //Image2D fullImage = MergeImage(image1, image2, UniScanGSettings.Instance().MonitorInfo.OverlapAreaPx);
//            //fullImage.SaveImage(@"D:\temp\tt3.bmp", System.Drawing.Imaging.ImageFormat.Bmp);

//            //cameraDrawBox.UpdateImage(fullImage.ToBitmap());
//        }

//        private Image2D MergeImage(Image2D imageL, Image2D imageR, int overlapAreaPx)
//        {
//            int width = imageL.Width + imageR.Width - overlapAreaPx;
//            int height = Math.Max(imageL.Height, imageR.Height);
//            int numBand = Math.Max(imageL.NumBand, imageR.NumBand);
//            Image2D image2D = new Image2D(width, height, numBand);

//            Point dstPoint = Point.Empty;
//            image2D.CopyFrom(imageL, Rectangle.FromLTRB(0, 0, imageL.Width, imageL.Height), imageL.Pitch, dstPoint);
//            dstPoint.Offset(imageL.Width,0);
//            image2D.CopyFrom(imageR, Rectangle.FromLTRB(overlapAreaPx, 0, imageR.Width, imageR.Height), imageR.Pitch, dstPoint);

//            return image2D;
//        }

//        private InspectionResult MergeResult(InspectionResult resultCam1, InspectionResult resultCam2, int overlapAreaPx)
//        {
//            InspectionResult inspectionResult = (InspectionResult)SystemManager.Instance().InspectRunner.InspectRunnerExtender.BuildInspectionResult();
//            SheetCheckerAlgorithmResult algorithmResultL = (resultCam1.ProbeResultList[0] as VisionProbeResult).AlgorithmResult as SheetCheckerAlgorithmResult;
//            SheetCheckerAlgorithmResult algorithmResultR = (resultCam2.ProbeResultList[0] as VisionProbeResult).AlgorithmResult as SheetCheckerAlgorithmResult;

//            algorithmResultR.Offset(17824 - overlapAreaPx, 0);
//            //algorithmResultR.SubResultList.ForEach(f =>
//            //{
//            //    if (f is SheetCheckerSubResult)
//            //    {
//            //        (f as SheetCheckerSubResult).ResultFigureGroup(17824, 0);
//            //    }
//            //});
//            inspectionResult.AddProbeResult(resultCam1);
//            inspectionResult.AddProbeResult(resultCam2);
//            inspectionResult.CalcDefectTypeCount();
            
//            return inspectionResult;
//        }

//        private void UpdateProductionInfo(int camIndex = -1)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateProductionInfoDelegate(UpdateProductionInfo), camIndex);
//                return;
//            }

//            if (SystemManager.Instance().CurrentModel != null)
//                labelModelName.Text = SystemManager.Instance().CurrentModel.Name;

//            Production curProduction =(Data.Production)this.monitoringPageBase.CurProduction;
//            if (curProduction == null)
//            {
//                labelLotNo.Text = "";
//                labelProductGood.Text = labelProductNG.Text = "0";
//                labelProductRate.Text = "0.0 %";
//            }else
//            {
//                labelLotNo.Text = curProduction.LotNo;
//                labelProductGood.Text = curProduction.Good.ToString();
//                labelProductNG.Text = curProduction.Ng.ToString();
//                if (curProduction.Total == 0)
//                    labelProductRate.Text = "- %";
//                else
//                    labelProductRate.Text = curProduction.NgRatio.ToString("F1") + " %";
//            }
//        }

//        public void UpdateClientState(Client client)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateClientStateDelegate(UpdateClientState), client);
//                return;
//            }

//            ClientState state = (SystemState.Instance().GetOpState() == OpState.Idle) ? ClientState.Idle : ClientState.Wait;
//            (SystemManager.Instance().MachineIf as MonitoringServer).ClientList.ForEach(f =>
//            {
//                if (f.State == ClientState.Inspect)
//                    state = ClientState.Inspect;
//            });

//            //if (SystemState.Instance().OpState == OpState.Inspect)
//            //    state = ClientState.Inspect;
//            //else if(SystemState.Instance().OpState == OpState.Wait)
//            //    state = ClientState.Wait;

//            labelStatus.Text = state.ToString();
//            switch (state)
//            {
//                case ClientState.Idle:
//                    labelStatus.BackColor = UniScanGUtil.Instance().Connected;
//                    break;
//                case ClientState.Wait:
//                    labelStatus.BackColor = UniScanGUtil.Instance().Wait;
//                    break;
//                case ClientState.Inspect:
//                    labelStatus.BackColor = UniScanGUtil.Instance().Inspect;
//                    break;
//            }    
//        }

//        private void buttonStart_Click(object sender, EventArgs e)
//        {
//            //MpisMonitorSystemManager systemManager = (MpisMonitorSystemManager)SystemManager.Instance();

//            if (SystemState.Instance().GetOpState() == OpState.Idle)
//            {
//                string lotNo = this.monitoringPageBase.CheckLotNo();
//                if (string.IsNullOrEmpty(lotNo))
//                    return;

//                this.monitoringPageBase.ChangeLot(lotNo);
//                UpdateProductionInfo();

//                StartInspect();
//            }
//        }

//        public void Reset()
//        {
//            this.monitoringPageBase.Reset();

//            Clear();
//            UpdateProductionInfo();
//        }

//        private void Clear()
//        {
//            cameraDrawBox.FigureGroup.Clear();
//            cameraDrawBox.UpdateImage(null);
//        }

//        private void buttonResetCount_Click(object sender, EventArgs e)
//        {
//            if (MessageForm.Show(null, "Reset?", MessageFormType.YesNo) == DialogResult.Yes)
//                Reset();
//        }

//        private void buttonStop_Click(object sender, EventArgs e)
//        {
//            StopInspection();
//        }
//    }
//}