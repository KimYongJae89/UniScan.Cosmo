using System;
using System.Collections.Generic;
using System.Diagnostics;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using UniScan.Common.UI;
using UniScan.Inspector.Exchange;
using UniScan.Inspector.Settings.Inspector;

namespace UniScan.Inspector
{
    public class InspectorOperator : ExchangeOperator, IClientExchangeOperator, IInspectStateListener, IOpStateListener
    {
        Client client;
        Server server;

        List<IVisitListener> visitListenerList = new List<IVisitListener>();
        
        public bool IsConnected { get { return client.IsConnected; } }

        public InspectorOperator()
        {
            
        }

        public override void Initialize()
        {
            client = new Client(InspectorSystemSettings.Instance().ClientSetting);

            if (InspectorSystemSettings.Instance().ClientIndex == 0 && InspectorSystemSettings.Instance().SlaveInfoList.Count > 0)
            {
                server = new Server(InspectorSystemSettings.Instance().ServerSetting);
                server.Initialize();
            }
            SystemState.Instance().SetIdle();

            SystemState.Instance().AddInspectListener(this);
            SystemState.Instance().AddOpListener(this);
        }

        public void AddVisitListener(IVisitListener visitListener)
        {
            visitListenerList.Add(visitListener);
        }

        public override void ModelTeachDone(int camId)
        {
            base.ModelTeachDone(camId);

            if(camId == InspectorSystemSettings.Instance().CamIndex)
                client.SendCommand(ExchangeCommand.M_TEACH_DONE, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
        }

        public void PreparePanel(ExchangeCommand eVisit)
        {
            foreach (IVisitListener listener in visitListenerList)
                listener.PreparePanel(eVisit);
        }

        public void ClearPanel()
        {
            foreach (IVisitListener listener in visitListenerList)
                listener.Clear();
        }

        public void SendAlive()
        {
            client.SendCommand(ExchangeCommand.C_CONNECTED, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
        }

        public override void SendCommand(ExchangeCommand exchangeCommand, params string[] args)
        {
            client.SendCommand(exchangeCommand, args);
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            switch (curOpState)
            {
                case OpState.Idle:
                    client.SendCommand(ExchangeCommand.S_IDLE, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
                    break;
                case OpState.Inspect:
                    client.SendCommand(ExchangeCommand.S_INSPECT, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
                    break;
                case OpState.Teach:
                    client.SendCommand(ExchangeCommand.S_TEACH, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
                    break;
                case OpState.Wait:
                    client.SendCommand(ExchangeCommand.S_OpWait, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
                    break;
                case OpState.Alarm:
                    client.SendCommand(ExchangeCommand.S_ALARM, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
                    break;
            }
        }

        public void InspectStateChanged(UniEye.Base.Data.InspectState curInspectState)
        {
            switch (curInspectState)
            {
                case InspectState.Run:
                    client.SendCommand(ExchangeCommand.S_RUN, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
                    break;
                case InspectState.Pause:
                    client.SendCommand(ExchangeCommand.S_PAUSE, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
                    break;
                case InspectState.Wait:
                    client.SendCommand(ExchangeCommand.S_InspWAIT, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
                    break;
                case InspectState.Done:
                    client.SendCommand(ExchangeCommand.S_DONE, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString());
                    break;
            }
        }

        public void SendInspectDone(string inspectionNo, string time)
        {
            client.SendCommand(ExchangeCommand.I_DONE, InspectorSystemSettings.Instance().CamIndex.ToString(), InspectorSystemSettings.Instance().ClientIndex.ToString(), inspectionNo, time);
        }

        public override int GetCamIndex()
        {
            return InspectorSystemSettings.Instance().CamIndex;
        }

        public override int GetClientIndex()
        {
            return InspectorSystemSettings.Instance().ClientIndex;
        }
    }
}