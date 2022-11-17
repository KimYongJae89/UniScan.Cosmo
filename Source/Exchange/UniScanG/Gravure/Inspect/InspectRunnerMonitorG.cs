using System;
using System.Drawing;
using System.IO;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using UniEye.Base.Data;
//using UniEye.Base.Settings;
using UniEye.Base.Inspect;
using System.Threading;
using System.Windows.Forms;
using DynMvp.Vision;
using DynMvp.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Device;
using UniScanG.Inspect;
using System.Collections.Generic;
using UniScanG.Vision;
using UniScan.Common.Exchange;
using DynMvp.Authentication;
using UniScanG.Data;
using System.Diagnostics;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Settings;
using System.Threading.Tasks;
using DynMvp.Device.Serial;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;
using UniScanG.Gravure.UI.Inspect.Monitor;
using System.Text;
using UniScanG.Gravure.Data;
using UniEye.Base.MachineInterface;
using System.ComponentModel;
using DynMvp.Devices.MotionController;
using System.Linq;
using UniScanG.Gravure.MachineIF;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.Inspect
{
    internal class InspectRunnerMonitorG : UniScanG.Inspect.InspectRunner, IModelListener
    {
        public const int MAX_BUFFER_SIZE = 1000;
        
        bool stopThread = true;
        Thread thread = null;
        Dictionary<int, List<Tuple<string, string>>> resultDic = new Dictionary<int, List<Tuple<string, string>>>();
        List<InspectorObj> InspectorList = new List<InspectorObj>();
        Dictionary<int, bool> modelTeachedDic = new Dictionary<int, bool>();

        List<InspectionResult> InspectionResultList = new List<InspectionResult>();

        List<Task> sheetCombinerTaskList = new List<Task>();
        public List<Task> SheetCombinerTaskList
        {
            get { return sheetCombinerTaskList; }
        }

        public InspectRunnerMonitorG() : base()
        {
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            InspectorList = server.GetInspectorList(-1);

            SystemManager.Instance().DeviceController.OnExitWaitInspection();

            this.AddInspectDoneDelegate(MonitorInspectDone);

            this.inspectObserver = new InspectObserver(InspectorList);
        }

        protected override void SetupUnitManager() { }

        public override bool EnterWaitInspection()
        {
            try
            {
                if (SystemState.Instance().OnInspectOrWait == true)
                    return false;

                if (SystemManager.Instance().CurrentModel == null || SystemManager.Instance().ProductionManager.CurProduction == null)
                    return false;

                // 검사기 상태 확인
                bool isInspectable = IsIsnpectable();
                if (isInspectable == false)
                {
                    throw new AlarmException(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Error,
                        StringManager.GetString("Inspector Is NOT Ready, or Model is Unusable."));
                    //ErrorManager.Instance().Report(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Error,
                    //    StringManager.GetString("Inspector Is NOT Ready, or Model is Unusable."));
                    //MessageForm.Show(null, StringManager.GetString("Inspector Is NOT Ready, or Model is Unusable."));
                    return false;
                }

                // 엔코더분배기 작동. 조명 켬.
                SystemManager.Instance().DeviceController.OnEnterWaitInspection();
                
                ProductionG pg = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
                if (pg != null)
                    pg.UpdateLineSpeedMpm(-1);

                SystemState.Instance().SetWait();

                return true;
            }
            catch (AlarmException ex)
            {
                ex.Report();
                return false;
            }
        }

        private bool WaitUntil(string message, int waitTimeMs, OpState waitUntil)
        {
            bool onError = false;
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm(message);
            simpleProgressForm.Show(() =>
            {
                TimeOutTimer tot = new TimeOutTimer();
                tot.Start(waitTimeMs);
                bool waitDone = false;
                while (onError == false && waitDone == false)
                {
                    List<InspectorObj> connectedInspObj = InspectorList.FindAll(f => f.CommState == CommState.CONNECTED);
                    onError = connectedInspObj.Exists(f => f.OpState == OpState.Alarm) || tot.TimeOut;
                    waitDone = connectedInspObj.TrueForAll(f => f.OpState == waitUntil);
                }
                if (tot.TimeOut)
                    Debug.WriteLine("WaitUntil::TimeOutTimer.TimeOut");
            });

            return onError == false;
        }

        public override bool PostEnterWaitInspection()
        {
            try
            {
                // 신규 티칭
                if (AdditionalSettings.Instance().AutoTeach)
                {
                    bool ok = AutoTeach();
                    if (ok == false)
                        throw new AlarmException(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Error,
                            StringManager.GetString("AutoTeaching Process Failure."));
                }

                // 검사기, 검사화면으로 전환
                SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.V_INSPECT);

                // 티칭 정보 가지고 옴.
                SimpleProgressForm simpleProgressForm2 = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Get Teach Data"));
                simpleProgressForm2.Show(() => CopyTeachParam());

                // 버퍼 할당
                int timeoutMs = ((AdditionalSettings)AdditionalSettings.Instance()).BufferAllocTimeout;
                ProductionG curProduction = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
                SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_READY, curProduction.LotNo, curProduction.LineSpeedMpm.ToString());
                bool waitDone = WaitUntil(StringManager.GetString(this.GetType().FullName, "Buffer Ready"), timeoutMs, OpState.Wait);
                if (waitDone == false)
                {
                    SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_STOP);
                    //MessageForm.Show(null, StringManager.GetString("Inspector Alarm Occure."));
                    throw new AlarmException(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Error,
                            StringManager.GetString("IM Alarm Occure."));
                    return false;
                }

                // 결과 수집용 스레드 작동
                InitResultCollectThread();

                // 검사 시작
                OpState opState = SystemState.Instance().GetOpState();
                if (opState != OpState.Wait)
                    throw new AlarmException(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Error,
                        StringManager.GetString("CM Alarm Occure."));

                this.inspectObserver.Clear();
                string infoString = string.Format("Monitoring Start, Model,{0}, Lot,{1}", SystemManager.Instance().CurrentModel.Name, SystemManager.Instance().ProductionManager.CurProduction.LotNo);
                LogHelper.Info(LoggerType.Inspection, infoString);

                SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_START, SystemManager.Instance().ProductionManager.CurProduction.LotNo);

                SystemState.Instance().SetInspect();
                SystemState.Instance().SetInspectState(InspectState.Run);
                return true;
            }
            catch (AlarmException ex)
            {
                ExitWaitInspection();
                ex.Report();
                return false;
            }
        }

        private void CopyTeachParam()
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList().FindAll(f => f.Info.ClientIndex <= 0);
            foreach (InspectorObj inspectorObj in inspectorList)
            {
                UniScanG.Data.Model.ModelDescription md = SystemManager.Instance().CurrentModel.ModelDescription as UniScanG.Data.Model.ModelDescription;
                UniScanG.Data.Production curProduction = SystemManager.Instance().ProductionManager.CurProduction as UniScanG.Data.Production;
                string resultPath = curProduction.GetResultPath(inspectorObj.Info.Path);
                //string resultPath = Path.Combine(inspectorObj.Info.Path, "Result", curProduction.StartTime.ToString("yy-MM-dd"), md.Name, md.Thickness.ToString(), md.Paste, curProduction.LotNo);
                string teachDataFile = Path.Combine(resultPath, "TeachData.xml");
                if (File.Exists(teachDataFile))
                {
                    string dstPath = curProduction.GetResultPath();
                    Directory.CreateDirectory(dstPath);
                    string dst = Path.Combine(dstPath, string.Format("TeachData_C{0}.xml", inspectorObj.Info.CamIndex));
                    FileHelper.CopyFile(teachDataFile, dst, true);
                }
            }
        }

        private void InitResultCollectThread()
        {
            foreach (InspectorObj inspector in InspectorList)
            {
                if (resultDic.ContainsKey(inspector.Info.CamIndex) == false)
                    resultDic.Add(inspector.Info.CamIndex, new List<Tuple<string, string>>());
            }

            thread?.Abort();
            stopThread = false;
            thread = new Thread(InspectionResultTask);
            thread.Start();
        }

        public bool AutoTeach()
        {
            // 티칭 화면으로 전환
            //SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.V_MODEL);

            // 티칭 시작
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorObjList = server.GetInspectorList().FindAll(f => f.Info.ClientIndex <= 0 && f.CommState == CommState.CONNECTED);
            this.modelTeachedDic = inspectorObjList.ToDictionary(f => f.Info.CamIndex, f => false);

            int timeoutMs = ((AdditionalSettings)AdditionalSettings.Instance()).AutoTeachTimeout;
            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_TEACH);
            Thread.Sleep(1000);
            bool waitDone = WaitUntil(StringManager.GetString(this.GetType().FullName, "Teaching"), timeoutMs, OpState.Idle);
            if (waitDone == false)
                return false;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            TimeOutTimer tot = new TimeOutTimer();
            tot.Start(timeoutMs);
            new SimpleProgressForm().Show(() =>
            {
                while (this.modelTeachedDic.Values.All(f => f) == false)
                {
                    Application.DoEvents();
                    if (tot.TimeOut)
                        break;
                }
                waitDone = true;
            }, cancellationTokenSource);
            tot.Stop();

            bool good = !tot.TimeOut && !cancellationTokenSource.IsCancellationRequested;
            if (good)
            {
                SystemManager.Instance().ExchangeOperator.SaveModel();
                SystemManager.Instance().ExchangeOperator.ModelTeachDone(-1);
            }

            return good;
        }

        private bool IsIsnpectable()
        {
            ModelDescription modelDescription = SystemManager.Instance().CurrentModel.ModelDescription;
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorObjList = server.GetInspectorList();
            return inspectorObjList.TrueForAll(f => f.IsTrained(modelDescription) && f.OpState == OpState.Idle);
        }

        //private bool IsSelectNeed()
        //{
        //    // VNC 접속된적이 있으면 항상 업데이트
        //    Model model = SystemManager.Instance().CurrentModel;
        //    if (model.Modified)
        //        return true;

        //    // Salve Inspector 중 티칭되지 않은것이 있으면 업데이트
        //    ModelDescription modelDescription = SystemManager.Instance().CurrentModel.ModelDescription;
        //    IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
        //    List<InspectorObj> inspectorObjList = server.GetInspectorList().FindAll(f => f.Info.ClientIndex != 0);
        //    return !inspectorObjList.TrueForAll(f => f.IsTrained(modelDescription));
        //}

        //bool syncDone = false;
        //private void SyncModel(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker worker = sender as BackgroundWorker;

        //    UniScanG.Data.Model.Model curModel = SystemManager.Instance().CurrentModel;
        //    IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
        //    List<InspectorObj> inspectorObjList = server.GetInspectorList();
        //    int cpyCnt = 0;

        //    worker?.ReportProgress(0, string.Format("0 / {0}", inspectorObjList.Count));
        //    for (int i = 0; i < inspectorObjList.Count; i++)
        //    {
        //        worker?.ReportProgress(i * 100 / inspectorObjList.Count, string.Format("{0} / {1}", i, inspectorObjList.Count));
        //        InspectorObj inspectorObj = inspectorObjList[i];
        //        bool exist = inspectorObj.ModelManager.IsModelExist(curModel.ModelDescription);
        //        if (exist == false)
        //        {
        //            e.Result = string.Format(StringManager.GetString("Model Path is NOT exist in {0}"), inspectorObj.Info.GetName());
        //            return ;
        //        }

        //        bool isTrained = inspectorObj.IsTrained(curModel.ModelDescription);
        //        if (isTrained == false || true)
        //        {
        //            InspectorObj baseInspectorObj = inspectorObjList.Find(f => f.Info.CamIndex == inspectorObj.Info.CamIndex && f.Info.ClientIndex == 0);
        //            if (baseInspectorObj == null)
        //            {
        //                e.Result = string.Format(StringManager.GetString("Can not found master device of {0}"), inspectorObj.Info.GetName());
        //                return;

        //            }

        //            string srcModelPath = baseInspectorObj.ModelManager.GetModelPath(curModel.ModelDescription);
        //            string dstModelPath = inspectorObj.ModelManager.GetModelPath(curModel.ModelDescription);
        //            if (srcModelPath == dstModelPath)
        //            {
        //                continue;
        //            }

        //                bool copied = CopyDirectory(srcModelPath, dstModelPath);
        //                if (copied == false)
        //                {
        //                    e.Result = string.Format(StringManager.GetString("Data copy fail in {0}"), inspectorObj.Info.GetName());
        //                    return;
        //                }
        //                cpyCnt++;
        //            //    string[] args = curModel.ModelDescription.GetArgs();
        //            //    SystemManager.Instance().ExchangeOperator.SelectModel(args);
        //            //    inspectorObj.ModelManager.LoadModel(args, null);
        //        }
        //    }
        //}

        //private void SyncModelComplete(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    Exception ex = e.Error;
        //    string message = (string)e.Result;
        //    if (string.IsNullOrEmpty(message) && ex == null)
        //        syncDone = true;
        //    else
        //        syncDone = false;
        //}

        private bool CopyDirectory(string srcModelPath, string dstModelPath)
        {
            if (Directory.Exists(dstModelPath) == false)
                Directory.CreateDirectory(dstModelPath);

            string[] directopries = Directory.GetDirectories(srcModelPath);
            foreach (string directoprie in directopries)
                CopyDirectory(directoprie, Path.Combine(dstModelPath, Path.GetFileName(directoprie)));

            string[] files = Directory.GetFiles(srcModelPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                //try
                //{
                    File.Copy(file, Path.Combine(dstModelPath, fileName), true);
                //}
                //catch (IOException ex)
                //{
                //    LogHelper.Error(LoggerType.Operation, string.Format("InspectPage::CopyDirectory. {0}", ex.Message));
                //    return false;
                //}
            }
            return true;
        }


        public override void PreExitWaitInspection()
        {
            // Encoder/Ligth Disable
            SystemManager.Instance().DeviceController.OnExitWaitInspection();
        }

        public override void ExitWaitInspection()
        {
            //this.observerForm.Close();
            SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanGMachineIfCommon.SET_VISION_RESULT_GRAVURE_INSP, "0");
            SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_NORDEF, "0");
            PreExitWaitInspection();

            SimpleProgressForm simpleProgressForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Stop"));
            simpleProgressForm.Show(() =>
            {

                stopThread = true;
                bool stopDone = false;
                while (stopDone == false)
                {
                    SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_STOP);
                    Thread.Sleep(1000);

                    stopDone = true;
                    foreach (InspectorObj inspector in InspectorList)
                    {
                        if (inspector.OpState != OpState.Idle )
                        {
                            stopDone = false;
                            break;
                        }
                    }
                }

                thread?.Join();
            });
            thread = null;
            resultDic.Clear();
            this.InspectionResultList.Clear();

            //SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.V_MODEL);

            SystemManager.Instance().ProductionManager.Save();

            SystemState.Instance().SetInspectState(InspectState.Wait);
            SystemState.Instance().SetIdle();

            DynMvp.Data.ProductionBase production = SystemManager.Instance().ProductionManager.CurProduction;
            string infoString = string.Format("Monitoring Stop, Total,{0}", production == null ? -1 : production.Total);
            LogHelper.Info(LoggerType.Inspection, infoString);
        }

        public override void EnterPauseInspection()
        {
            SystemState.Instance().Pause = true;
        }

        public void MonitorInspectDone(InspectionResult inspectionResult)
        {
            //SystemState.Instance().SetInspectState(UniEye.Base.Data.InspectState.Done);

            string isNg = (inspectionResult.IsGood() ? "0" : "1");
            SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_RESULT, isNg);

            Settings.AdditionalSettings additionalSettings = (Settings.AdditionalSettings)AdditionalSettings.Instance();
            if (additionalSettings.NormalDefectAlarm.Use)
            {
                DynMvp.Data.ProductionBase productionBase = SystemManager.Instance().ProductionManager.CurProduction;
                //string writeValue = (productionBase.Total > additionalSettings.NormalN && productionBase.NgRatio > additionalSettings.NormalR) ? "1" : "0";
                //SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_NORDEF, 0, writeValue);
                if (productionBase.Total > additionalSettings.NormalDefectAlarm.Count && productionBase.NgRatio > additionalSettings.NormalDefectAlarm.Ratio)
                    SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_NORDEF, "1");
            }
        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectionOption inspectionOption = null)
        {
            if (SystemState.Instance().GetOpState() == OpState.Idle)
                return;

            object cam = inspectionResult.ExtraResult["Cam"];
            object client = inspectionResult.ExtraResult["Client"];
            object no = inspectionResult.ExtraResult["No"];

            string infoString = string.Format("Inspector Call, CamId,{0}, ClientId,{1}, SheetNo,{2}", cam, client, no);
            LogHelper.Info(LoggerType.Inspection, infoString);

            lock (InspectionResultList)
                InspectionResultList.Add(inspectionResult);
        }

        public void InspectionResultTask()
        {
            bool exitable = false;
            while (stopThread == false || exitable == false)
            {
                if (InspectionResultList.Count > 0)
                {
                    exitable = false;
                    lock (InspectionResultList)
                    {
                        foreach (InspectionResult result in InspectionResultList)
                        {
                            int camIndex = Convert.ToInt32(result.GetExtraResult("Cam"));
                            int clientIndex = Convert.ToInt32(result.GetExtraResult("Client"));
                            string inspectionNo = (string)result.GetExtraResult("No");
                            string inspectionTime = (string)result.GetExtraResult("Time");

                            lock (resultDic[camIndex])
                            {
                                resultDic[camIndex].Add(new Tuple<string, string>(inspectionNo, inspectionTime));
                                //resultDic[camIndex].Sort();
                                this.inspectObserver.AddData(camIndex, clientIndex, int.Parse(inspectionNo));
                            }
                        }

                        InspectionResultList.Clear();
                    }
                }

                //if (resultDic.Count > 0)
                {
                    Tuple<string, string> foundedT = null;
                    if (sheetCombinerTaskList.Count < MAX_BUFFER_SIZE)
                    {
                        lock (resultDic)
                        {
                            // 같은거 찾기
                            // 0번 카메라에서 넘어온 시트 번호를 각 카메라에서 넘어온 시트 번호와 매칭한다.
                            int camCnt = resultDic.Count;
                            List<Tuple<string, string>> searchFrom = resultDic[0];
                            int[] idxArray = new int[camCnt];
                            for (int itemIdx = 0; itemIdx < searchFrom.Count; itemIdx++)
                            {
                                string target = searchFrom[itemIdx].Item1;
                                idxArray[0] = itemIdx;
                                for (int camIdx = 1; camIdx < camCnt; camIdx++)
                                {
                                    idxArray[camIdx] = resultDic[camIdx].FindIndex(f => f.Item1 == target);
                                    if (idxArray[camIdx] < 0)
                                        break;
                                }

                                if (Array.TrueForAll(idxArray, f => f >= 0))
                                {
                                    foundedT = searchFrom[itemIdx];
                                    for (int camIdx = 0; camIdx < camCnt; camIdx++)
                                        resultDic[camIdx].RemoveAt(idxArray[camIdx]);
                                }
                            }
                        }
                    }

                    if (foundedT != null)
                    {
                        exitable = false;
                        Task sheetCombinerTask = Task.Run(() =>
                         {
                             Stopwatch stopwatch = new Stopwatch();
                             stopwatch.Start();

                             SheetResult sheetResult = SheetCombiner.CombineResult(foundedT);
                             InspectionResult inspectionResult = BuildInspectionResult();
                             MergeSheetResult mergeSheetResult = new MergeSheetResult(int.Parse(inspectionResult.InspectionNo), sheetResult);

                             inspectionResult.AlgorithmResultLDic.Add(SheetCombiner.TypeName, mergeSheetResult);
                             inspectionResult.InspectionTime = mergeSheetResult.SpandTime;

                             if (mergeSheetResult.Good == false)
                                 inspectionResult.SetDefect();

                             UniScanG.Data.Production production = SystemManager.Instance().ProductionManager.CurProduction as UniScanG.Data.Production;
                             lock (production)
                             {
                                 production?.Update(mergeSheetResult);
                                 SystemManager.Instance().ProductionManager.Save(UniEye.Base.Settings.PathSettings.Instance().Result);
                             }

                             SystemManager.Instance().ExportData(inspectionResult);

                             stopwatch.Stop();
                             inspectionResult.ExportTime = stopwatch.Elapsed;

                             //string infoString = string.Format("Monitor Done, SheetNo,{0}, InspectTimeMs,{1}, ImportTimeMs,{2}",
                             //     inspectionResult.InspectionNo, inspectionResult.InspectionTime.ToString("ss\\.fff"), stopwatch.ElapsedMilliseconds);
                             //LogHelper.Info(LoggerType.Inspection, infoString);

                             InspectDone(inspectionResult);
                         });
                        //sheetCombinerTask.Wait();
                        sheetCombinerTaskList.Add(sheetCombinerTask);
                    }
                    else
                    {
                        exitable = true;
                    }

                }

                sheetCombinerTaskList.RemoveAll(f => f.IsCompleted || f.IsFaulted);

                if (exitable == true)
                {
                    exitable = sheetCombinerTaskList.Count == 0;
                    Thread.Sleep(10);
                }
            }
            sheetCombinerTaskList.Clear();
        }

        public void ModelRefreshed()
        {
            throw new NotImplementedException();
        }

        public void ModelChanged() { }

        public void ModelTeachDone(int camId)
        {
            if (this.modelTeachedDic.ContainsKey(camId))
                this.modelTeachedDic[camId] = true;
        }
    }
}
