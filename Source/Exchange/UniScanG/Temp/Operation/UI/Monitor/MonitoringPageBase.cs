//using DynMvp.Data;
//using DynMvp.Devices.Light;
//using DynMvp.UI.Touch;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using UniEye.Base;
//using UniEye.Base.Data;
//using DynMvp.Base;
//using DynMvp.UI;
//using System.Threading;
//using UniEye.Base.Settings;

//namespace UniScanG.Temp
//{
//    using Data;
//    using SimpleResultList = SortedList<int, Data.SimpleInspectionResult>;

//    delegate void UpdateClientStateDelegate(Client client);
//    class MonitoringPageBase
//    {
//        SimpleResultList[] resultPaths = new SortedList<int, Data.SimpleInspectionResult>[2];
//        DynMvp.Data.Production curProduction = null;
//        public DynMvp.Data.Production CurProduction
//        {
//            get { return curProduction; }
//        }

//        public MonitoringPageBase()
//        {
//            resultPaths[0] = new SortedList<int, Data.SimpleInspectionResult>();
//            resultPaths[1] = new SortedList<int, Data.SimpleInspectionResult>();
//        }

//        public SimpleResultList GetSimpleResultList(int i)
//        {
//            return resultPaths[i];
//        }

//        public void Reset()
//        {
//            ErrorManager.Instance().ResetAlarm();
//            ClearSimpleResult();

//            (SystemManager.Instance().MachineIf as MonitoringServer).SendReset();
//            //((MainForm)SystemManager.Instance().MainForm).WaitJobDone("RESET");

//            curProduction = null;
//        }

//        public string CheckLotNo()
//        {
//            string message = String.Format("is Screen Model \"{0}\" Correct?", SystemManager.Instance().CurrentModel.Name);
//            if (MessageForm.Show(null, message, MessageFormType.YesNo) == DialogResult.No)
//                return null;

//            DynMvp.Data.Production lastProduction = null;
//            List<DynMvp.Data.Production> todayProductionList = null;
//            if (SystemManager.Instance().CurrentModel.ProductionList.Count > 0)
//            {
//                lastProduction = SystemManager.Instance().CurrentModel.ProductionList.Last();
//                todayProductionList = SystemManager.Instance().CurrentModel.ProductionList.FindAll(f => (f.StartTime.Date == DateTime.Today));
//            }

//            InputForm form = new InputForm("Input Lot No", lastProduction?.LotNo);
//            //Point point = new Point((Screen.PrimaryScreen.Bounds.Width / 2) - (form.Width / 2), (Screen.PrimaryScreen.Bounds.Height / 2) - (form.Height / 2));
//            //form.ChangeLocation(point);
//            //if (form.ShowDialog() == DialogResult.Cancel)
//            //    return null;

//            //string newLotNo = form.InputText;
//            //if (string.IsNullOrEmpty(newLotNo) == true)
//            //    return null;

//            //// Lot번호가 직전과 같다. 또는, 금일 생산한 Product중 Lot번호가 중복된다.
//            //if (((lastProduction != null) && (newLotNo == lastProduction.LotNo))
//            //    || ((todayProductionList != null) && todayProductionList.Exists(f => f.LotNo == newLotNo)))
//            //{
//            //    DialogResult dialogResult = MessageForm.Show(null, "Lot No is Duplicated. \r\nShall I Continue Inspection with Previous?", MessageFormType.YesNo);
//            //    if (dialogResult == DialogResult.No)
//            //        return null;
//            //}

//            return null;// newLotNo;
//        }

//        public void ChangeLot(string lotNum)
//        {
//            DynMvp.Data.ProductionManager productionManager = ProductionManager.Instance();

//            Production lastProduction = null;
//            Production todayProduction = null;
//            if (SystemManager.Instance().CurrentModel.ProductionList.Count > 0)
//            {
//                lastProduction =(Production) SystemManager.Instance().CurrentModel.ProductionList.Last();
//                todayProduction = (Production)SystemManager.Instance().CurrentModel.ProductionList.Find(f => (f.StartTime.Date == DateTime.Now.Date) && (f.LotNo == lotNum));
//                if (todayProduction == lastProduction)
//                    todayProduction = null;
//            }

//            Production curProduction = null;
//            if (lastProduction != null && lastProduction.LotNo == lotNum)
//            {
//                curProduction = lastProduction;
//            }
//            else if (todayProduction != null)
//            {
//                curProduction = todayProduction;
//            }
//            else
//            {
//                curProduction = (Production)productionManager.CreatProduction("Monitor", lotNum);
//                SystemManager.Instance().CurrentModel.ProductionList.Add(curProduction);
//                curProduction.AddSubProduction(productionManager.CreatProduction("CAM1", lotNum));
//                curProduction.AddSubProduction(productionManager.CreatProduction("CAM2", lotNum));
//                SystemManager.Instance().CurrentModel.SaveProduction();
//            }

//            if(this.curProduction != curProduction)
//            {
//                ClearSimpleResult();
//            }
//            this.curProduction = curProduction;
//            SystemManager.Instance().InspectRunner.InspectRunnerExtender.Production = curProduction;

//            (SystemManager.Instance().MachineIf as MonitoringServer).SendLotChange(curProduction.LotNo);
//            //((MainForm)SystemManager.Instance().MainForm).WaitJobDone("Change LOT");
//        }

//        private void ClearSimpleResult()
//        {
//            foreach (SimpleResultList simpleResultList in resultPaths)
//                simpleResultList.Clear();
//        }

//        public bool StartInspect()
//        {
//            MonitoringServer monitoringServer = (SystemManager.Instance().MachineIf as MonitoringServer);
//            try
//            {
//                monitoringServer.SendCurrentTime(true);

//                string modelXml = Path.Combine("Model", SystemManager.Instance().CurrentModel.Name, "Model.xml");
//                monitoringServer.SendSyncModel(modelXml);
//                //((MainForm)SystemManager.Instance().MainForm).WaitJobDone("Sync");
                
//                int numLightType = SystemManager.Instance().CurrentModel.LightParamSet.NumLightType;
//                if (numLightType > 0)
//                {
//                    LightValue lightValue = SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue;
//                    SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(lightValue);
//                }

//                MachineControlPanel motionControlPanel = ((MainForm)SystemManager.Instance().MainForm).RemoteTeachingPage.MotionControlPanel;
//                motionControlPanel.MoveForward();

//                //bool acyncMode = UniScanGSettings.Instance().AsyncMode;
//                //float convSpeed = ((UniScanG.Operation.Data.ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).ConvayorSpeedMPS;
//                //monitoringServer.SendInspStandBy(acyncMode, convSpeed);
//                //((MainForm)SystemManager.Instance().MainForm).WaitJobDone("Inspection Standby");

//                SystemState.Instance().SetWait();
                
//                monitoringServer.SendInspGrab();
//                //((MainForm)SystemManager.Instance().MainForm).WaitJobDone("Wait Responce");
//                return true;
//            }
//            catch (OperationCanceledException)
//            {
//                monitoringServer.SendExitWait();
//                MachineControlPanel motionControlPanel = (SystemManager.Instance().MainForm as MainForm).RemoteTeachingPage.MotionControlPanel;
//                motionControlPanel.MoveStop();
//                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
//                MessageForm.Show(null, "Operation Canceled");
//                return false;
//            }
//        }

//        public DynMvp.InspData.InspectionResult LoadRemoteInspectionResult(InspectorInfo inspectorInfo, string resultPath)
//        {
//            LogHelper.Debug(LoggerType.Inspection, string.Format("MonitoringPageBase::CopyInspectionResult"));

//            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
//            sw.Start();

//            // Setting Page에서 경로 지정할 수 있어야 함.
//            string volumeLabel = string.Format("Result_{0}{1}", inspectorInfo.CamIndex + 1, (char)(inspectorInfo.ClientIndex + 'A'));
//            string drive = "";
//            List<DriveInfo> driveInfoList = DriveInfo.GetDrives().ToList();
//            DriveInfo driveInfo = driveInfoList.Find(f => f.DriveType == DriveType.Fixed && f.VolumeLabel == volumeLabel);
//            if (driveInfo == null)
//                drive = PathSettings.Instance().Result;
//            else
//                drive = driveInfo.Name;

//            string remoteResultPath = Path.Combine(inspectorInfo.Path, "Result", resultPath);
//            string localResultPath = Path.Combine(drive, volumeLabel, resultPath);

//            // Copy Remote to Local
//            Data.MpisInspResultArchiver archiver = new Data.MpisInspResultArchiver();
//            Data.InspectionResult inspectionResult = archiver.LoadInspResult(remoteResultPath) as Data.InspectionResult;
//            LogHelper.Debug(LoggerType.Operation, string.Format("MornitoringPage::InspectionDone::Task - Load remote inspection result {0}", remoteResultPath));
//            if (inspectionResult == null)
//                return null;

//            //AddProductionInfo(inspectorInfo.CamIndex, inspectionResult);

//            inspectionResult.ResultPath = localResultPath;
//            archiver.Save(inspectionResult, new CancellationToken());
//            LogHelper.Debug(LoggerType.Operation, string.Format("MornitoringPage::InspectionDone::Task - Save local inspection result - {0}", localResultPath));

//            int sheetNo = int.Parse(inspectionResult.InspectionNo);
//            int defectCount = (inspectionResult.ProbeResultList[0] as VisionProbeResult).AlgorithmResult.SubResultList.Count;
//            DateTime inspStartTime = inspectionResult.InspectionStartTime;
//            string resultCsv = inspectionResult.ResultPath;

//            AddSimpleResult(inspectorInfo.CamIndex, new SimpleInspectionResult(sheetNo, defectCount, inspStartTime, inspectionResult, resultCsv));

//            LogHelper.Debug(LoggerType.Operation, string.Format("MornitoringPage::InspectionDone::Task - Add and Sort inspection result"));
            
//            sw.Stop();
//            if ((UniScanGSettings.Instance().SaveInspectionDebugData & SaveDebugData.Text) > 0)
//                (SystemManager.Instance().MainForm as MainForm).WriteTimeLog(string.Format("MonitorUpdate{0}{1}", inspectorInfo.CamIndex, (char)(inspectorInfo.ClientIndex + 'A')), int.Parse(inspectionResult.InspectionNo), sw.ElapsedMilliseconds);
//            return inspectionResult;
//        }

//        public void AddProductionInfo(int camIndex, DynMvp.InspData.InspectionResult inspectionResult)
//        {
//            Data.Production production;
//            if (camIndex < 0)
//                production = (Data.Production)curProduction;
//            else
//                production = (Data.Production)curProduction.GetSubProduction(camIndex);

//            lock (production)
//            {
//                if (inspectionResult.IsGood())
//                    production.AddGood();
//                else
//                    production.AddNG();

//                production.ProductInspected();

//                if(inspectionResult is InspectionResult)
//                {
//                    InspectionResult samsungInspectionResult = inspectionResult as InspectionResult;
//                    production.BlackDefectNum += samsungInspectionResult.BlackDefectNum;
//                    production.WhiteDefectNum += samsungInspectionResult.WhiteDefectNum;
//                }
                
//            }
//        }

//        //public void GetResultFigure(DynMvp.InspData.InspectionResult inspectionResult, FigureGroup fgCam, FigureGroup fgAcc)
//        //{
//        //    if (inspectionResult == null)
//        //        return;

//        //    LogHelper.Debug(LoggerType.OpDebug, string.Format("MornitoringPage::GetResultFigure Start"));
//        //    foreach (VisionProbeResult visionProbeResult in inspectionResult.ProbeResultList)
//        //    {
//        //        Data.SheetCheckerAlgorithmResult algorithmResult = visionProbeResult.AlgorithmResult as Data.SheetCheckerAlgorithmResult;
//        //        if (algorithmResult == null)
//        //            return;

//        //        foreach (Data.SheetCheckerSubResult subResult in algorithmResult.SubResultList)
//        //        {
//        //            Rectangle defectPosRect = subResult.ResultRect.ToRectangle();
//        //            Rectangle checkPosRect = new Rectangle(defectPosRect.X / 10, defectPosRect.Y / 10, defectPosRect.Width / 10, defectPosRect.Height / 10);
//        //            Point checkPosCenter = DrawingHelper.CenterPoint(checkPosRect);

//        //            switch (subResult.DefectType)
//        //            {
//        //                case Data.SheetDefectType.BlackDefect:
//        //                    fgCam?.AddFigure(new CrossFigure(checkPosCenter, 5, new Pen(Color.Red, 5)));
//        //                    fgAcc?.AddFigure(new CrossFigure(checkPosCenter, 5, new Pen(Color.Red, 1)));
//        //                    //camResultView[resultInfo.CamIndex].FigureGroup.AddFigure(new CrossFigure(checkPosCenter, 5, new Pen(Color.Red, 5)));
//        //                    //defectView[resultInfo.CamIndex].FigureGroup.AddFigure(new CrossFigure(checkPosCenter, 5, new Pen(Color.Red, 1)));
//        //                    break;
//        //                case Data.SheetDefectType.WhiteDefect:
//        //                    fgCam?.AddFigure(new CrossFigure(checkPosCenter, 5, new Pen(Color.Yellow, 5)));
//        //                    fgAcc?.AddFigure(new CrossFigure(checkPosCenter, 5, new Pen(Color.Yellow, 1)));
//        //                    //camResultView[resultInfo.CamIndex].FigureGroup.AddFigure(new CrossFigure(checkPosCenter, 5, new Pen(Color.Yellow, 5)));
//        //                    //defectView[resultInfo.CamIndex].FigureGroup.AddFigure(new EllipseFigure(checkPosRect, new Pen(Color.Yellow, 1)));
//        //                    break;
//        //            }
//        //        }
//        //    }
//        //    LogHelper.Debug(LoggerType.OpDebug, string.Format("MornitoringPage::GetResultFigure End"));
//        //}

//        public void StopInspection()
//        {
//            MonitoringServer monitoringServer = (SystemManager.Instance().MachineIf as MonitoringServer);
//            try
//            {
//                monitoringServer.SendExitWait();
//                //((MainForm)SystemManager.Instance().MainForm).WaitJobDone("Stop");
//            }
//            catch (OperationCanceledException)
//            {

//            }

//            SimpleProgressForm form = new SimpleProgressForm("Stop");
//            form.Show(new Action(() =>
//            {
//                MachineControlPanel motionControlPanel = (SystemManager.Instance().MainForm as MainForm).RemoteTeachingPage.MotionControlPanel;
//                motionControlPanel.MoveStop();
//                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
//                SystemManager.Instance().CurrentModel.SaveProduction();
//            }));

//            SystemState.Instance().SetIdle();
//            SystemState.Instance().Pause = false;
//        }

//        public void AddSimpleResult(int camIndex, SimpleInspectionResult simpleResult)
//        {
//            SimpleResultList simpleResultList = resultPaths[camIndex];
//            lock (simpleResultList)
//            {
//                simpleResultList.Add(simpleResult.SheetNo, simpleResult);
//            }
//        }
//    }
//}
