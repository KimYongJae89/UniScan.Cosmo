//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DynMvp.InspData;
//using UniEye.Base.MachineInterface;
//using UniEye.Base.Inspect;
//using UniEye.Base;
//using System.Threading;
//using DynMvp.Devices.Comm;
//using UniEye.Base.Data;
//using DynMvp.Data;
//using UniScanG.Operation.UI;
//using DynMvp.Base;
//using System.IO;
//using UniScanG.Operation.Comm;

//namespace UniScanG.Temp
//{
//    internal class MonitoringClientExecuter : MachineIfExecuter<Type>
//    {
//        MonitoringClient monitoringClient;
//        ExecuteResult executeResult = new ExecuteResult();

//        Task excuteTask = null;

//        public MonitoringClientExecuter(Type typeName) : base(typeName)
//        {
//            //this.monitoringClient = monitoringClient;
//            executeResult.Operation = true;
//        }

//        public override ExecuteResult ExecuteCommand(string commandLine)
//        {
//            if (String.IsNullOrEmpty(commandLine) == true)
//                return new ExecuteResult();

//            string[] tokens = commandLine.Split(';');

//            Protocol protocol = SamsungProtocolFactory.Instance().Create(tokens);
//            //ECommand command = (ECommand)Enum.Parse(typeof(ECommand), tokens[0].ToUpper());

//            if (protocol.Command == ECommand.CHK)
//            // 처리 완료 여부 요청
//            {
//                monitoringClient.SendResponce(protocol.Command, executeResult.Operation, executeResult.Success, executeResult.Message);
//                return executeResult;
//            }
//            else if (this.executeResult.Operation == false)
//            // 이전 명령이 수행 중
//            {
//                monitoringClient.SendResponce(protocol.Command, false, false, "");
//                return executeResult;
//            }

//            // 새 명령 수행
//            this.executeResult = new ExecuteResult();
//            this.excuteTask = Task.Factory.StartNew(() =>
//            {
//                Excute(protocol);
//            });

//            monitoringClient.SendResponce(protocol.Command, true, false, "Command Excuted");

//            return executeResult;
//        }

//        private bool Excute(Protocol protocol)
//        {
//            bool excute = true;
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);

//            switch (protocol.Command)
//            {
//                // HeartBeat
//                case ECommand.TIME:
//                    {
//                        UpdateTime(protocol.Args[0]);
//                    }
//                    break;

//                // Commands without Model select 
//                case ECommand.CHANGE_MODEL:
//                    {
//                        ChangeModel(protocol.Args[0]);

//                        break;
//                    }

//                case ECommand.QUERY_MODEL_INFO:
//                    {
//                        QuaryModelInfo(protocol.Args[0]);

//                        break;
//                    }

//                case ECommand.DELETE_MODEL:
//                    {
//                        DeleteModel(protocol.Args[0]);
//                        break;

//                    }

//                case ECommand.NEW_MODEL:
//                    {
//                        NewModel(protocol.Args[0]);

//                        break;
//                    }

//                // Commands with Model select 
//                case ECommand.SAVE_MODEL:
//                    {
//                        SaveModel();

//                        break;
//                    }

//                case ECommand.LOT_CHANGE:
//                    {
//                        LotChange(protocol.Args[0]);

//                        break;
//                    }

//                case ECommand.CHANGE_MODE:
//                    {
//                        ChangeMode(protocol.Args[0]);

//                        break;
//                    }

//                case ECommand.RESET:
//                    {
//                        Reset();

//                        break;
//                    }

//                case ECommand.SETTING_SYNC:
//                    {
//                        SyncSetting();

//                        break;
//                    }

//                case ECommand.USERCHANGE:
//                    {
//                        UserChange(protocol.Args[0], protocol.Args[1]);

//                        break;
//                    }

//                case ECommand.ENTER_WAIT:
//                    {
//                        WaitInspect(bool.Parse(protocol.Args[0]), float.Parse(protocol.Args[1]));

//                        break;
//                    }

//                case ECommand.START_INSP:
//                    {
//                        StartInspect();

//                        break;
//                    }

//                case ECommand.EXIT_WAIT:
//                    {
//                        ExitWait();

//                        break;
//                    }

//                case ECommand.SET_INSPECTION_NO:
//                    {
//                        SetInspectionNo(protocol.Args[0]);

//                        break;
//                    }

//                case ECommand.SYNC:
//                    {
//                        SyncParam(protocol.Args[0]);

//                        break;
//                    }

//                case ECommand.TEACH:
//                    {
//                        Teach();

//                        break;
//                    }

//                case ECommand.GRAB:
//                    {
//                        Grab(float.Parse(protocol.Args[0]), protocol.Args[1]);

//                        break;
//                    }

//                case ECommand.START_TEST_GRAB:
//                    {
//                        TestGrab(float.Parse(protocol.Args[0]));

//                        break;
//                    }

//                case ECommand.TEACH_INSPECT:
//                    {
//                        TeachInsepct();

//                        break;
//                    }

//                case ECommand.STOP:
//                    {
//                        Stop();

//                        break;
//                    }

//                case ECommand.TAB_DISABLE:
//                    {
//                        TabDisable(protocol.Args[0], bool.Parse(protocol.Args[1]), int.Parse(protocol.Args[2]), int.Parse(protocol.Args[3]));
//                    }
//                    break;

//                default:
//                    executeResult.Success = false;
//                    executeResult.Operation = false;
//                    excute = false;
//                    break;
//            }

//            return excute;
//        }

//        private void TabDisable(string key, bool enable, int camIndex, int clientIndex)
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            if (camIndex == UniScanGSettings.Instance().InspectorInfo.CamIndex
//                        && clientIndex == UniScanGSettings.Instance().InspectorInfo.ClientIndex)
//            {

//                mainForm.EnableTabs(enable);
//                mainForm.EnableTabs(key, true);
//            }

//            //monitoringClient.SendJobDone("TAB_DISABLE", true);
//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void Stop()
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            mainForm.RcStop();
//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void TeachInsepct()
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            //string resultFileName = mainForm.RcInspect();
//            //executeResult.Message = Path.Combine("Result", "RemoteInspect", resultFileName);
//            //executeResult.Success = true;
//            //executeResult.Operation = true;
//        }

//        private void TestGrab(float convSpeed)
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            mainForm.RcTestGrab(convSpeed);
//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void Grab(float convSpeed, string xmlPath)
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);

//            bool grabNeed = (UniScanGSettings.Instance().InspectorInfo.ClientIndex == 0);
//            if (grabNeed)
//            {
//                if (string.IsNullOrEmpty(xmlPath))
//                    executeResult.Success = mainForm.RcGrab(convSpeed);
//                else
//                    executeResult.Success = mainForm.RcFiducialGrab(convSpeed, xmlPath);

//                if (executeResult.Success)
//                    executeResult.Message = Path.Combine("Model", SystemManager.Instance().CurrentModel.Name, "Image", "GrabbedImage.Jpeg");
//            }
//            else
//            {
//                executeResult.Success = true;
//                executeResult.Message = "";
//            }
//            executeResult.Operation = true;
//        }

//        private void Teach()
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            bool teachNeed = (UniScanGSettings.Instance().InspectorInfo.ClientIndex == 0);
//            if (teachNeed)
//            {
//                string teachResult = mainForm.RcTeach();
//                executeResult.Success = (teachResult == "OK");
//                if (executeResult.Success)
//                    executeResult.Message = Path.Combine("Model", SystemManager.Instance().CurrentModel.Name, "Model.xml");
//            }
//            else
//            {
//                executeResult.Success = true;
//                executeResult.Message = "";
//            }
//            executeResult.Operation = true;
//        }

//        private void SyncParam(string xmlPath)
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            mainForm.RcSync(xmlPath);
//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void SetInspectionNo(string inspectNo)
//        {
//            InspectRunner inspectRunner = SystemManager.Instance().InspectRunner;
//            ((Operation.Inspect.InspectRunnerExtenderInspector)inspectRunner.InspectRunnerExtender).ReceivedInspectionNo = inspectNo;
//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void ExitWait()
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            mainForm.InspectionPage.ButtonStopClick(true);
//            //monitoringClient.SendJobDone();
//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void StartInspect()
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            executeResult.Success = mainForm.RcStartInspGrab();
//            //monitoringClient.SendJobDone(command.ToString(), executeResult.success, executeResult.message);
//            executeResult.Operation = true;
//        }

//        private void WaitInspect(bool acyncMode, float convSpeed)
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            executeResult.Success = mainForm.RcEnterWait(acyncMode, convSpeed);
//            executeResult.Message = executeResult.Success ? "" : "Error,Can't do Inspection job";
//            executeResult.Operation = true;

//            //monitoringClient.SendJobDone(command.ToString(), executeResult.success, executeResult.message);
//        }

//        private void UserChange(string userId, string password)
//        {
//            DynMvp.Authentication.UserHandler.Instance().CurrentUser = DynMvp.Authentication.UserHandler.Instance().GetUser(userId, password + 8);
//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void SyncSetting()
//        {
//            //string configPath = System.IO.Path.Combine(UniScanGSettings.Instance().MonitorInfo.Path, "Config");
//            //SamsungElectroTransferSettings.Instance().Load(configPath);
//            //mainForm.LoadTransferSettings();
//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void Reset()
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            mainForm.Reset();

//            //monitoringClient.SendJobDone("RESET", true);

//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void ChangeMode(string modeStr)
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            mainForm?.ChangeMode(modeStr);

//            //monitoringClient.SendJobDone("CHANGE_MODE", true);

//            executeResult.Success = (mainForm != null);
//            executeResult.Operation = true;
//        }

//        private void LotChange(string lotNum)
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            mainForm.InspectionPage.ChangeLot(lotNum);

//            //monitoringClient.SendJobDone("LOT_CHANGE", true);

//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void SaveModel()
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            Model curModel = SystemManager.Instance().CurrentModel;

//            bool ok = ((MainForm)SystemManager.Instance().MainForm).TeachingPage.SaveModel(false);

//            //monitoringClient.SendJobDone("SAVE_MODEL", true);

//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void NewModel(string name)
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            ModelDescription modelDesc = SystemManager.Instance().ModelManager.CreateModelDescription();
//            modelDesc.Name = name;

//            mainForm.ModelManagerPage.NewModel(modelDesc);

//            //monitoringClient.SendJobDone("NEW_MODEL", true);

//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void DeleteModel(string name)
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            mainForm.ModelManagerPage.DeleteModel(name);

//            //monitoringClient.SendJobDone("DELETE_MODEL", true);

//            executeResult.Success = true;
//            executeResult.Operation = true;
//        }

//        private void QuaryModelInfo(string name)
//        {
//            //ModelDescription modelDescription = SystemManager.Instance().ModelManager.GetModelDescription(name);
//            //if (modelDescription != null)
//            //{
//            //    executeResult.Success = true;
//            //    Data.Model model = (Data.Model)SystemManager.Instance().ModelManager.GetModel(name);
//            //    if (model != null && model.IsTaught())
//            //        executeResult.Message = "TeachDone;" + modelDescription.LastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");
//            //    else
//            //        executeResult.Message = "NeedTeach" + modelDescription.LastModifiedDate.ToString("yyyy-MM-dd HH:mm:ss");
//            //}
//            //else
//            //{
//            //    executeResult.Success = false;
//            //    executeResult.Message = "Error,Can't find the model";
//            //}

//            ////monitoringClient.SendJobDone("QUERY_MODEL_INFO", executeResult.success, executeResult.message);

//            //executeResult.Operation = true;
//        }

//        private void ChangeModel(string name)
//        {
//            MainForm mainForm = (SystemManager.Instance().MainForm as MainForm);
//            ModelDescription modelDescription = SystemManager.Instance().ModelManager.GetModelDescription(name);
//            if (modelDescription != null)
//            {
//                executeResult.Success = mainForm.ModelManagerPage.SelectModel(name, true);
//                if (executeResult.Success)
//                {
//                    bool isTaught = SystemManager.Instance().CurrentModel.IsTaught();
//                    mainForm.UpdateMainTab(isTaught);
//                    executeResult.Message = isTaught ? "TeachDone" : "NeedTeach";
//                }
//            }
//            else
//            {
//                executeResult.Success = false;
//                executeResult.Message = "Can't find the model";
//            }

//            //monitoringClient.SendJobDone("CHANGE_MODEL", executeResult.success, executeResult.message);

//            executeResult.Operation = true;
//        }

//        private void UpdateTime(string v)
//        {
//            long tick;
//            if (long.TryParse(v, out tick))
//            {
//                DateTime newDateTime = new DateTime(tick);
//                executeResult.Success = SystemManager.Instance().SetSystemDateTIme(newDateTime);
//                executeResult.Operation = true;
//            }
//        }

//        public bool Execute(Enum operation, string[] args)
//        {
//            ECommand command = (ECommand)operation;
//            return false;
//        }

//        public override bool Execute(string operation, List<string> args)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public class MonitoringClient : TcpIpClient, ISystemStatusListener
//    {
//        DateTime lastSendAliveTime;

//        delegate void ConnectStateViewDelegate(bool? connected);
//        ConnectStateViewDelegate connectStateViewDelegate = null;

//        public MonitoringClient(TcpIpInfo tcpIpInfo) : base(tcpIpInfo)
//        {
//            connectStateViewDelegate += ConnectStateView;
//            AddExecuter(new MonitoringClientExecuter(typeof(ECommand)));

//            StandAloneModeChanged();

//            SystemState.Instance().AddListener(this);
//            StartThread();
//        }

//        public void StandAloneModeChanged()
//        {
//            if (UniScanGSettings.Instance().InspectorInfo.StandAlone)
//                this.clientSocket.StopConnectionThread();
//            else
//            {
//                this.clientSocket.StartConnectionThread();
//            }
//        }


//        private void ConnectStateView(bool? connected)
//        {
//            if (connected.HasValue)
//            {
//                (SystemManager.Instance().MainForm as MainForm)?.UpdateClientStateLamp(connected.Value);
//            }
//        }

//        public void SendStartClient()
//        {
//            string packet = String.Format("START;{0};{1}", UniScanGSettings.Instance().InspectorInfo.CamIndex, UniScanGSettings.Instance().InspectorInfo.ClientIndex);
//            AddCommand(packet);
//        }

//        public void SendAlive()
//        {
//            string stateStr = "Pause";
//            if (SystemState.Instance().Pause == true)
//            {
//                stateStr = "Pause";
//            }
//            else
//            {
//                stateStr = SystemState.Instance().GetOpState().ToString();
//            }

//            int camIndex = UniScanGSettings.Instance().InspectorInfo.CamIndex;
//            int clientIndex = UniScanGSettings.Instance().InspectorInfo.ClientIndex;
//            string packet = String.Format("STATE;{0};{1};{2}", camIndex, clientIndex, stateStr);
//            AddCommand(packet);
//            lastSendAliveTime = DateTime.Now;
//        }

//        public void SendResponce(ECommand command, bool operation, bool success, string message = "")
//        {
//            string packet = String.Format("ECHO;{0};{1};{2};{3};{4};{5}", UniScanGSettings.Instance().InspectorInfo.CamIndex, UniScanGSettings.Instance().InspectorInfo.ClientIndex,
//              command, operation.ToString(), success.ToString(), message);

//            AddCommand(packet);
//        }

//        public void SendInspDone(int camIndex, int clientIndex, string resultPath)
//        {
//            string packet = String.Format("INSP_DONE;{0};{1};{2};", camIndex, clientIndex, resultPath);

//            AddCommand(packet);
//        }
        
//        //private void SendAliveProc()
//        //{
//        //    while (!commandSendThreadFlag)
//        //    {
//        //        if (lastSendAliveTime != null)
//        //        {
//        //            TimeSpan timeSpan = DateTime.Now - this.lastSendAliveTime;
//        //            if (timeSpan.TotalMilliseconds > 1000)
//        //                SendAlive();
//        //        }

//        //        Thread.Sleep(100);
//        //    }
//        //}

//        public void ExitTeachJob()
//        {
//            string packet = String.Format("EXIT_TEACH_JOB;{0};{1}", UniScanGSettings.Instance().InspectorInfo.CamIndex, UniScanGSettings.Instance().InspectorInfo.ClientIndex);

//            AddCommand(packet);
//        }

//        public void StatusChanged(OpState curOpState, OpState prevOpState)
//        {
//            SendAlive();
//        }
//    }
//}
