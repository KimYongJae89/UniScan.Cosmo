//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DynMvp.InspData;
//using UniEye.Base.MachineInterface;
//using UniEye.Base.Inspect;
//using UniEye.Base;
//using DynMvp.Devices.Comm;
//using DynMvp.Base;
//using System.Diagnostics;
//using System.IO;
//using System.Threading;
//using UniScanG.Operation.Comm;
//using UniScanG.Operation.UI;
//using UniEye.Base.Settings;

//namespace UniScanG.Temp
//{
//    public enum ClientState
//    {
//        Connected, Disconnected, Idle, Wait, Inspect, Pause
//    }

//    public struct JobDone
//    {
//        bool done;
//        public bool Done
//        {
//            get { return done; }
//            set { done = value; }
//        }

//        bool operate;
//        public bool Operate
//        {
//            get { return operate; }
//            set { operate = value; }
//        }

//        bool success;
//        public bool Success
//        {
//            get { return success; }
//            set { success = value; }
//        }

//        string message;
//        public string Message
//        {
//            get { return message; }
//            set { message = value; }
//        }
//    }

//    public class ClientList : List<Client>
//    {

//        public void SendCommand(Protocol protocol)
//        {
//            this.ForEach(f => f.SendCommand(protocol));
//        }

//        public void WaitEcho(int timeoutMs, CancellationTokenSource cancellationTokenSource)
//        {
//            TimeOutTimer timeOutTimer = new TimeOutTimer();
//            if (timeoutMs >= 0)
//                timeOutTimer.Start(timeoutMs);

//            bool isDone = false;
//            do
//            {
//                //try
//                //{
//                Thread.Sleep(1);
//                if (timeOutTimer.TimeOut)
//                    throw new TimeoutException();

//                if (cancellationTokenSource.IsCancellationRequested)
//                    throw new OperationCanceledException();

//                isDone = IsEchoDone();
//                //}
//            } while (isDone == false);
//        }

//        public void WaitJobDone(int timeoutMs = -1)
//        {

//        }

//        public bool IsEchoDone()
//        {
//            bool notDone= this.Exists(f => f.IsDone() == false);
//            return !notDone;
//        }

//        public bool IsOperate()
//        {
//            bool isOperate = !this.Exists(f => f.IsOperate() == false);
//            return isOperate;
//        }

//        public bool IsSuccess()
//        {
//            bool isNotSuccess = this.Exists(f => ((f.State != ClientState.Disconnected) && (f.IsSuccess() == false)));
//            return isNotSuccess == false;
//        }
        
//        public void ClearEcho()
//        {
//            this.ForEach(f => f.ClearJobDone());
//        }

//        public string[] GetMessages()
//        {
//            string[] messages = new string[this.Count];
//            for (int i = 0; i < this.Count; i++)
//                messages[i] = this[i].GetMessage();

//            return messages;
//        }
//    }

//    public class Client
//    {
//        InspectorInfo clientInfo;
//        public InspectorInfo ClientInfo
//        {
//            get { return clientInfo; }
//        }

//        ClientHandlerSocket clientHandlerSocket;
//        public ClientHandlerSocket ClientHandlerSocket
//        {
//            get { return clientHandlerSocket; }
//        }

//        ClientState state;
//        public ClientState State
//        {
//            get { return state; }
//            set { state = value; }
//        }

//        public string IpAddress
//        {
//            get { return clientInfo.IpAddress; }
//        }

//        public int CamIndex
//        {
//            get { return clientInfo.CamIndex; }
//        }

//        public int ClientIndex
//        {
//            get { return clientInfo.ClientIndex; }
//        }

//        ECommand lastSentcommand;
//        public ECommand LastSentcommand
//        {
//            get { return lastSentcommand; }
//        }

//        JobDone jobDone;

//        public Client(InspectorInfo inspectorInfo, ClientHandlerSocket clientHandlerSocket)
//        {
//            this.clientInfo = inspectorInfo;
//            this.clientHandlerSocket = clientHandlerSocket;
//            this.state = ClientState.Connected;
//        }

//        public void SetState(string state)
//        {
//            this.State = (ClientState)Enum.Parse(typeof(ClientState), state);
//        }

//        public void SetEcho(ECommand command,bool operate, bool success, string message)
//        {
//            if (this.lastSentcommand == command)
//            {
//                jobDone.Done = true;
//                jobDone.Operate = operate;
//                jobDone.Success = success;
//                jobDone.Message = message;
//            }
//        }

//        public void ClearJobDone()
//        {
//            this.lastSentcommand = ECommand.NULL;
//            this.jobDone.Done = false;
//            this.jobDone.Operate = false;
//            this.jobDone.Success = false;
//            this.jobDone.Message = "";
//        }

//        public void SendCommand(Protocol protocol)
//        {
//            this.lastSentcommand = protocol.Command;
//            string commandString = string.Format("<START>{0}<END>", protocol.ToString());
//            this.clientHandlerSocket.SendCommand(Encoding.ASCII.GetBytes(commandString));
//        }

//        public bool WaitJobDone(int timeoutMs = -1)
//        {
//            Stopwatch sw = new Stopwatch();
//            if(timeoutMs>=0)
//                sw.Start();

//            bool isDone = false;
//            do
//            {
//                isDone = this.IsDone();
//            } while (isDone == false && sw.ElapsedMilliseconds < timeoutMs);
//            sw.Stop();
//            return isDone;
//        }

//        public bool IsOperate()
//        {
//            return this.jobDone.Operate;
//        }

//        public bool IsDone()
//        {
//            return this.jobDone.Done;
//        }

//        public bool IsSuccess()
//        {
//            return this.jobDone.Success;
//        }

//        public string GetMessage()
//        {
//            return this.jobDone.Message;
//        }
//    }

//    public interface IMonitoringServerListener
//    {
//        void ClientConnected(Client client);
//        void ClientDisconnected(Client client);
//        void ClientAlive(Client client);

//        void InspectionDone(Client client, string resultPath);
//        void TeachInspectionDone(Client client, string resultPath);
//        void ExitTeachJob(Client client);
//        void StartClient(Client client);
//    }

//    public class MonitoringServer : TcpIpServer
//    {
//        IMonitoringServerListener listener;
//        public IMonitoringServerListener Listener
//        {
//            set { listener = value; }
//        }

//        ClientList clientList = new ClientList();
//        public ClientList ClientList
//        {
//            get { return clientList; }
//        }

//        System.Windows.Forms.Timer sendTimeTimer = new System.Windows.Forms.Timer();

//        string commLogPath;
//        public string CommLogPath
//        {
//            get { return commLogPath; }
//            set { commLogPath = value; }
//        }

//        public MonitoringServer(TcpIpInfo tcpIpInfo)
//        {
//            sendTimeTimer.Tick += sendTimeTimer_Tick;
//            sendTimeTimer.Interval = 60000;
            
//            Start(tcpIpInfo);
//        }

//        ~MonitoringServer()
//        {
//            sendTimeTimer.Stop();
//            sendTimeTimer.Dispose();
//        }

//        private void sendTimeTimer_Tick(object sender, EventArgs e)
//        {
//            sendTimeTimer.Stop();

//            SendCurrentTime(false);

//            sendTimeTimer.Start();
//        }

//        public Client GetClient(int camIndex, int clientIndex)
//        {
//            return clientList.Find(f => ((f.CamIndex == camIndex) && (f.ClientIndex == clientIndex)));
//        }

//        public Client GetClient(string ipAddress)
//        {
//            Client client = this.clientList.Find(f => ipAddress == f.IpAddress);
//            return client;
//        }

//        public bool SendCommand(Protocol protocol, bool showProgressForm = true)
//        {
//            lock (clientList)
//            {
//                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

//                MainForm mainForm = null;
//                if (showProgressForm)
//                    mainForm = SystemManager.Instance().MainForm as MainForm;


//                bool done = false;

//                try
//                {
//                    mainForm?.StartWaitJobDone("Wait", cancellationTokenSource);

//                    int retry = 0;
//                    bool send = false;
//                    do
//                    {
//                        // 명령 보냄
//                        try
//                        {
//                            clientList.ClearEcho();
//                            clientList.SendCommand(protocol);
//                            clientList.WaitEcho(1000, cancellationTokenSource);
//                            send = clientList.IsOperate();
//                            if (send == false)
//                            {
//                                StringBuilder sb = new StringBuilder();
//                                sb.AppendLine("Client is Busy");
//                                List<Client> list = clientList.FindAll(f => f.IsOperate() == false);
//                                list.ForEach(f => sb.AppendLine(string.Format("Cam{0}Client{1}", f.CamIndex, f.ClientIndex)));

//                                retry++;

//                                if (retry > 5)
//                                    break;
//                                //throw new InvalidOperationException(sb.ToString());
//                            }
//                        }
//                        catch (TimeoutException)
//                        {
//                            retry++;
//                            //if (retry > protocol.Retry)
//                            //throw new InvalidOperationException("ECHO timeout");

//                            continue;
//                        }
//                    } while (send == false);

//                    if (send == true)
//                    {
//                        TimeOutTimer timeOutTimer = new TimeOutTimer();
//                        if (protocol.TimeoutMS >= 0)
//                            timeOutTimer.Start(protocol.TimeoutMS);

//                        // 완료 확인

//                        do
//                        {
//                            if (timeOutTimer.TimeOut)
//                            {
//                                //throw new TimeoutException(string.Format("Protocol TImeout: {0}", protocol.Command));
//                                break;
//                            }

//                            try
//                            {
//                                clientList.ClearEcho();
//                                clientList.SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.CHK));
//                                clientList.WaitEcho(-1, cancellationTokenSource);
//                                done = clientList.IsOperate();
//                            }
//                            catch (TimeoutException)
//                            { }
//                        } while (done == false);
//                    }
//                }
//                finally
//                {
//                    if (done == true)
//                        mainForm?.EndWaitJobDone();
//                }
//            }

//            return clientList.IsSuccess();
//        }

//        //public override bool SendCommand(string commandString)
//        //{
//        //    if (string.IsNullOrEmpty(this.commLogPath) == false)
//        //    {
//        //        using (System.IO.StreamWriter writer = System.IO.File.AppendText(Path.Combine(this.commLogPath, "CommTest_TX.txt")))
//        //        {
//        //            writer.WriteLine(string.Format("SEND {0} {1} {2}", System.Threading.Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), commandString));
//        //        }
//        //    }
//        //    return base.SendCommand(commandString);
//        //}

//        //public override void ClientConnected(ClientHandlerSocket clientHandlerSocket)
//        //{
//        //    string ipAddress = clientHandlerSocket.GetIpAddress();
//        //    InspectorInfo inspectorInfo = UniScanGSettings.Instance().ClientInfoList.Find(f => f.IpAddress == ipAddress);
//        //    if (inspectorInfo == null)
//        //        return;

//        //    Client client = new Client(inspectorInfo, clientHandlerSocket);
//        //    client.State = ClientState.Connected;

//        //    lock (clientList)
//        //        clientList.Add(client);

//        //    listener?.ClientConnected(client);

//        //    SendCurrentTime(false);
//        //}

//        //public override void ClientDisconnected(ClientHandlerSocket clientHandlerSocket)
//        //{
//        //    string ipAddress = clientHandlerSocket.GetIpAddress();
//        //    Client client = GetClient(ipAddress);
//        //    client.State = ClientState.Disconnected;

//        //    lock (clientList)
//        //        clientList.Remove(client);

//        //    listener?.ClientDisconnected(client);
//        //}
        
//        public bool SendSyncModel(string xmlPath = "")
//        {
//            if (string.IsNullOrEmpty(xmlPath))
//                xmlPath = Path.Combine("Model", SystemManager.Instance().CurrentModel.Name, "Model.xml");

//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.SYNC, xmlPath));
//        }

//        public bool SendInspStandBy(bool acyncMode, float convSpeed)
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.ENTER_WAIT, acyncMode.ToString(), convSpeed.ToString()));
//        }

//        public bool SendTestGrab(float convSpeed)
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.START_TEST_GRAB, convSpeed.ToString()));
//        }

//        public bool SendInspGrab()
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.START_INSP));
//        }

//        public bool SendExitWait()
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.EXIT_WAIT));
//        }

//        public bool SendLotChange(string lotNo)
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.LOT_CHANGE, lotNo.ToString()));
//        }

//        public bool SendStart()
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.START));
//        }

//        public bool SendStop()
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.STOP));
//        }

//        public bool SendModel_Change(string modelName)
//        {
//            bool ok = false;
//            try
//            {
//                ok = SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.CHANGE_MODEL, modelName));
//            }
//            catch (InvalidOperationException)
//            {
//            }
//            catch (TimeoutException)
//            {
//                ErrorManager.Instance().Report(0, ErrorLevel.Error, "Section", "Error", "Message", "Reason", "Solution");
//            }
//            return ok;
//        }

//        public bool SendQueryModelInfo(string modelName)
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.QUERY_MODEL_INFO, modelName));
//        }

//        public bool SendModel_Delete(string modelName)
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.DELETE_MODEL, modelName));
//        }

//        public bool SendModel_New(string modelName)
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.NEW_MODEL, modelName));
//        }

//        public bool SendSave()
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.SAVE_MODEL));
//        }

//        public bool SendTeach()
//        {
//           return  SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.TEACH));
//        }

//        public bool SendTeach_Inspect()
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.TEACH_INSPECT));
//        }
        
//        public bool SendCurrentTime(bool showProgressForm)
//        {
//            DateTime dateTime = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);
//            string timeTick = dateTime.Ticks.ToString();
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.TIME, timeTick), showProgressForm);
//        }

//        public bool SendChangeMode(string modeStr)
//        {
//            bool ok = false;
//            try
//            {
//                ok = SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.CHANGE_MODE, modeStr));
//            }
//            catch (InvalidOperationException)
//            {
//            }
//            catch (TimeoutException)
//            {
//                ErrorManager.Instance().Report(0, ErrorLevel.Error, "Section", "Error", "Message", "Reason", "Solution");
//            }
//            return ok;

//        }

//        public bool SendStartGrab(float curSpeed, string xmlPath = "")
//        {
//            return  SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.GRAB, curSpeed.ToString(), xmlPath));
//        }

//        public bool SendReset()
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.RESET));
//        }

//        public bool SendBarcodeSerialNo(string barcodeSerialNo)
//        {
//            //return base.SendCommand(barcodeSerialNo);
//            return false;
//        }

//        public bool SendTabDisable(int camIndex, int clientIndex, string key, bool enable)
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.TAB_DISABLE, key, enable.ToString().ToUpper()));
//        }

//        public bool SendSetting_Sync()
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.SETTING_SYNC));
//        }

//        public bool SendUserChange()
//        {
//            return SendCommand(SamsungProtocolFactory.Instance().Create(ECommand.USERCHANGE, DynMvp.Authentication.UserHandler.Instance().CurrentUser.Id));
//        }

//        // Client -> Server
//        // {Command} ; {CamId} ; {ClientId} ; {Param1} ; {Parma2} ; ....
//        //public override void ParsePacket(string packetString)
//        //{
//        //    if (String.IsNullOrEmpty(packetString) == true)
//        //        return;

//        //    string[] tokens = packetString.Split(';');
//        //    int camIndex = Convert.ToInt32(tokens[1]);
//        //    int clientIndex = Convert.ToInt32(tokens[2]);

//        //    if (string.IsNullOrEmpty(this.commLogPath) == false)
//        //    {
//        //        using (System.IO.StreamWriter writer = System.IO.File.AppendText(Path.Combine(this.commLogPath, string.Format("CommTest_RX_{0}{1}.txt", camIndex + 1, (char)(clientIndex + 'A')))))
//        //        {
//        //            writer.WriteLine(string.Format("RECEIVE {0} {1} {2}", System.Threading.Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), packetString));
//        //        }
//        //    }

//        //    Client client = this.GetClient(camIndex, clientIndex);
//        //    Protocol protocol = SamsungProtocolFactory.Instance().Create(tokens);

//        //    Process(client, protocol);
            
//        //}

//        private void Process(Client client, Protocol protocol)
//        {
//            switch (protocol.Command)
//            {
//                case ECommand.STATE:
//                    // State 변경시 수신.
//                    client.SetState(protocol.Args[2]);
//                    listener?.ClientAlive(client);
//                    break;

//                case ECommand.ECHO:
//                    // 요청한 작업 완료.
//                    client.SetEcho((ECommand)Enum.Parse(typeof(ECommand), protocol.Args[2]), Convert.ToBoolean(protocol.Args[3]), Convert.ToBoolean(protocol.Args[4]), protocol.Args[5]);
//                    break;

//                case ECommand.START:
//                    // 클라이언트 연결됨.
//                    listener?.StartClient(client);
//                    break;

//                case ECommand.INSP_DONE:
//                    // 검사 완료.
//                    listener?.InspectionDone(client, protocol.Args[2]);
//                    break;
//            }
//        }
//    }
//}
