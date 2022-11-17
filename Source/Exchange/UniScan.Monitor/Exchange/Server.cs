using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Threading;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using UniScan.Monitor.Settings.Monitor;

namespace UniScan.Monitor.Exchange
{
    internal class Server : TcpIpMachineIfServer
    {
        ExchangeProtocolList exchangeProtocolList;

        List<InspectorObj> inspectorList = new List<InspectorObj>();
        internal List<InspectorObj> InspectorList
        {
            get { return inspectorList; }
        }

        public Server(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
            exchangeProtocolList = (ExchangeProtocolList)MonitorSystemSettings.Instance().ServerSetting.MachineIfProtocolList;
        }

        public override void Initialize()
        {
            List<InspectorInfo> inspectorInfoList = MonitorSystemSettings.Instance().InspectorInfoList;
            foreach (InspectorInfo inspectorInfo in inspectorInfoList)
                inspectorList.Add(new InspectorObj(inspectorInfo));

            AddExecuter(new JobExecuter());
            AddExecuter(new StateExecuter());
            AddExecuter(new InspectExcuter());
            AddExecuter(new CommExecuter());

            base.Initialize();

            serverSocket.ClientConnected = ClienctConnected;
            serverSocket.ClientDisconnected = ClienctDisConnected;

            Start();
        }

        private void ClienctConnected(ClientHandlerSocket clientHandlerSocket)
        {
            foreach (InspectorObj inspector in InspectorList)
            {
                if (inspector.Info.Address == clientHandlerSocket.GetIpAddress())
                {
                    inspector.CommState = CommState.CONNECTED;
                    inspector.InspectState = UniEye.Base.Data.InspectState.Done;
                    inspector.OpState = UniEye.Base.Data.OpState.Idle;

                    SendCommand(ExchangeCommand.U_CHANGE, UserHandler.Instance().CurrentUser.Id);

                    //if (SystemManager.Instance().CurrentModel != null)
                    //    SendCommand(ExchangeCommand.M_SELECT, SystemManager.Instance().CurrentModel.ModelDescription.GetArgs());
                }
            }
        }

        private void ClienctDisConnected(ClientHandlerSocket clientHandlerSocket)
        {
            InspectorObj targetInspector = null;
            foreach (InspectorObj inspector in InspectorList)
            {
                if (inspector.Info.Address == clientHandlerSocket.GetIpAddress())
                {
                    targetInspector = inspector;
                    inspector.CommState = CommState.DISCONNECTED;
                    inspector.InspectState = UniEye.Base.Data.InspectState.Done;
                    inspector.OpState = UniEye.Base.Data.OpState.Idle;
                }
            }

            if (SystemState.Instance().GetOpState() != OpState.Idle)
            {
                SystemManager.Instance().InspectRunner.ExitWaitInspection();
                ErrorManager.Instance().Report(ErrorSection.Inspect, ErrorSubSection.CommonReason, ErrorLevel.Fatal,
                    "", "Inspector Disconnected", string.Format("Inspector {0} was disconnected", targetInspector.Info.GetName()));
            }
        }



        protected override TcpIpMachineIfPacketParser CreatePacketParser()
        {
            return new ExchangePacketParser(exchangeProtocolList);
        }

        public bool ModelTrained(ModelDescription modelDescription)
        {
            bool trained = true;
            
            foreach (InspectorObj inspector in InspectorList)
            {
                if (inspector.IsTrained(modelDescription) == false)
                {
                    trained = false;
                    break;
                }
            }

            return trained;
        }

        public bool ModelTrained(int camIndex, int clientIndex, ModelDescription modelDescription)
        {
            foreach (InspectorObj inspector in InspectorList)
            {
                if (camIndex == inspector.Info.CamIndex && clientIndex == inspector.Info.ClientIndex)
                {
                    if (inspector.IsTrained(modelDescription) == true)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ModelExist(ModelDescription modelDescription)
        {
            bool exist = true;

            foreach (InspectorObj inspector in InspectorList)
            {
                if (inspector.Exist(modelDescription) == false)
                {
                    exist = false;
                    break;
                }
            }

            return exist;
        }

        public void NewModel(ModelDescription modelDescription)
        {
            foreach (InspectorObj inspector in InspectorList)
                inspector.NewModel(modelDescription);

            SendCommand(exchangeProtocolList, ExchangeCommand.M_REFRESH);
        }

        public void SelectModel(ModelDescription modelDescription)
        {
            List<string> argList = new List<string>();
            argList.Add("-1");
            argList.Add("-1");
            argList.AddRange(modelDescription.GetArgs());
            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.M_SELECT, argList.ToArray());
        }

        public void DeleteModel(ModelDescription modelDescription)
        {
            SendCommand(exchangeProtocolList, ExchangeCommand.M_CLOSE);

            foreach (InspectorObj inspector in InspectorList)
                inspector.DeleteModel(modelDescription);

            SendCommand(exchangeProtocolList, ExchangeCommand.M_REFRESH);
        }

        public void SendVisit(ExchangeCommand eVisit)
        {
            SendCommand(exchangeProtocolList, eVisit);
        }

        public void SendCommand(ExchangeCommand eCommand, params string[] args)
        {
            SendCommand(exchangeProtocolList, eCommand, args);
        }

        public bool WaitJobDone(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            bool success = false;

            SimpleProgressForm jobWaitForm = new SimpleProgressForm(message);
            jobWaitForm.TopMost = true;

            jobWaitForm.Show(new Action(() =>
            {
                bool jobDoneAll = false;

                while (jobDoneAll == false)
                {
                    jobDoneAll = true;

                    foreach (InspectorObj inspector in InspectorList)
                    {
                        //if (inspector.JobState == JobState.RUN)
                        //    jobDoneAll = false;
                    }

                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        success = false;
                        return;
                    }

                    Thread.Sleep(10);
                }

                success = true;

            }), cancellationTokenSource);

            //foreach (InspectorObj inspector in InspectorList)
            //{
            //    if (inspector.JobState == JobState.ERROR)
            //    {
            //        success = false;
            //        break;
            //    }
            //}

            return true;
        }
    }
}
