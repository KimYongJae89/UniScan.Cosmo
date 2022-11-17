using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.InspData;
using System.Xml;

namespace UniEye.Base.MachineInterface
{
    /*     < Single Trigger >
     *      VisionReady On  ->
     *      CommandWait On  ->
     *                     <-   Trigger On
     *      CommandWait Off ->
     *                     <-   Trigger Off
     *      Result On       ->                  
     *      Complete On     ->                  
     *                     <-   CommandDone On // Done
     *      Result Off      ->                  
     *      Complete Off    ->                  
     *                     <-   CommandDone Off // Ready
     *      CommandWait On  ->
     *      VisionReady Off ->
     */

    /*     < Multi Trigger >
     *      VisionReady On  ->
     *      CommandWait On  ->
     *                     <-   InspStart On
     *      CommandWait Off ->
     *                     <-   InspStart Off
     *      Complete On     ->                  
     *                     <-   CommandDone On
     *      Complete Off    ->                  
     *                     <-   CommandDone Off
     *      CommandWait On  ->
     *                     <-   Trigger On
     *      CommandWait Off ->
     *                     <-   Trigger Off
     *      Complete On     ->                  
     *                     <-   CommandDone On
     *      Complete Off    ->                  
     *                     <-   CommandDone Off
     *      CommandWait On  ->
     *                     <-   InspEnd On
     *      CommandWait Off ->
     *                     <-   InspEnd Off
     *      Result On       ->                  
     *      Complete On     ->                  
     *                     <-   CommandDone On
     *      Result Off      ->                  
     *      Complete Off    ->                  
     *                     <-   CommandDone Off
     *      CommandWait On  ->
     *      VisionReady Off ->
     */

    public class IoMachineIfProtocol : MachineIfProtocol
    {
        IoPort ioPort = null;
        public IoPort IoPort
        {
            get { return ioPort; }
        }

        bool ioValue;
        public bool IoValue
        {
            get { return ioValue; }
            set { ioValue = value; }
        }

        public IoMachineIfProtocol(Enum command) : base(command, false, -1)
        {
            this.ioPort = null;
            this.ioValue = false;
        }

        public IoMachineIfProtocol(Enum command, bool use, int waitResponceMs, IoPort ioPort, bool ioValue) : base(command, use, waitResponceMs)
        {
            this.ioPort = ioPort;
            this.ioValue = ioValue;
        }

        public override MachineIfProtocol Clone()
        {
            throw new NotImplementedException();
        }
    }

    //public enum IoMachineIfCommand { OutVisionAlive, InMachineAlive, OutVisionReadyPort, InTriggerPort, OutOnWorking, OutCompletePort, OutResultNgPort, InCommandDone };
    public enum ActiveLevel { Low, High, Toggle };
    //public class IoMachineIfProtocolListItem : ProtocolListItem
    //{
    //    ActiveLevel activeLevel = ActiveLevel.High;
    //    IoPort ioPort;

    //    public ActiveLevel ActiveLevel
    //    {
    //        get { return activeLevel; }
    //        set { activeLevel = value; }
    //    }

    //    public IoPort IoPort
    //    {
    //        get { return ioPort; }
    //        set { ioPort = value; }
    //    }

    //    protected override void SaveXml(XmlElement element)
    //    {
    //        XmlHelper.SetValue(element, "ActiveLevel", activeLevel.ToString());
    //        ioPort.SaveXml(element);
    //    }

    //    protected override void LoadXml(XmlElement element)
    //    {
    //        activeLevel = (ActiveLevel)Enum.Parse(typeof(ActiveLevel), XmlHelper.GetValue(element, "ActiveLevel", activeLevel.ToString()));
    //        ioPort = IoPort.Load(element);
    //    }
    //}

    //public class IoMachineIfProcotolHandler : MachineIfProtocolHandler
    //{
    //    public IoMachineIfProcotolHandler() : base()
    //    {
    //    }

    //    public override MachineIfProtocol GetProtocol(Enum command)
    //    {
    //        IoMachineIfProtocol item = (IoMachineIfProtocol)machineIfProtocolList.GetProtocol(command);
    //        IoPort ioPort = item.IoPort;
    //        return item;
    //    }

    //    //public override void Load(XmlElement xmlElement, string subKey = null)
    //    //{
    //    //    if (xmlElement == null)
    //    //        return;

    //    //    if (string.IsNullOrEmpty(subKey) == false)
    //    //    {
    //    //        XmlElement subElement = xmlElement[subKey];
    //    //        Save(subElement, null);
    //    //        return;
    //    //    }

    //    //    XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("Item");
    //    //    foreach (XmlElement subElement in xmlNodeList)
    //    //    {
    //    //        Enum e = (Enum)Enum.Parse(this.commandType, XmlHelper.GetValue(subElement, "Command", ""));
    //    //        IoPort ioPort = IoPort.Load(subElement, "IoPort");

    //    //        dictionary.Add(e, ioPort);
    //    //    }
    //    //}

    //    //public override void Save(XmlElement xmlElement, string subKey = null)
    //    //{
    //    //    if (xmlElement == null)
    //    //        return;

    //    //    if (string.IsNullOrEmpty(subKey) == false)
    //    //    {
    //    //        XmlElement subElement = xmlElement.OwnerDocument.CreateElement(subKey);
    //    //        xmlElement.AppendChild(subElement);
    //    //        Save(subElement, null);
    //    //        return;
    //    //    }

    //    //    foreach (KeyValuePair<Enum, IoPort> pair in dictionary)
    //    //    {
    //    //        XmlElement subElement = xmlElement.OwnerDocument.CreateElement("Item");
    //    //        xmlElement.AppendChild(subElement);

    //    //        XmlHelper.SetValue(subElement, "Command", pair.Key.ToString());
    //    //        pair.Value.SaveXml(xmlElement, "IoPort");
    //    //    }
    //    //}
    //}

    public class IoMachineIfExcuter : MachineIfExecuter
    {
        protected override bool Execute(string command)
        {
            throw new NotImplementedException();
        }
    }

    public class IoMachineIf : MachineIf
    {
        enum IfState { Idle, WaitTrigger1Off, WaitTrigger2Off, WaitCommandDoneOn, WaitCommandDoneOff }

        DigitalIoHandler digitalIoHandler;
        IoMonitor ioMonitor;

        IoPort visionReadyPort;
        IoPort commandWaitPort;
        IoPort inspStartPort;
        IoPort complete1Port;
        IoPort complete2Port;
        IoPort trigger1Port;
        IoPort trigger2Port;
        IoPort inspEndPort;
        IoPort commandDonePort;
        IoPort resultNgPort;

        int lastTriggerNo;

        IfState ifState;

        public override bool IsConnected => true;

        public IoMachineIf(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
            ioMonitor = new IoMonitor(digitalIoHandler, null);
            ioMonitor.ProcessInitial += ProcessIoInit;
            ioMonitor.ProcessInputChanged += ProcessIo;

            this.AddExecuter(new IoMachineIfExcuter());

            ifState = IfState.Idle;
        }

        public override void Initialize()
        {
            
        }

        public override void Release()
        {
            ioMonitor.Stop();
        }
        
        bool ProcessIoInit(DioValue value)
        {
            return true;
        }

        // 작업 더 필요함 - SG
        bool ProcessIo(DioValue oldValue, DioValue newValue)
        {
            bool trigger1 = IoMonitor.CheckInput(newValue, trigger1Port);
            bool trigger2 = IoMonitor.CheckInput(newValue, trigger2Port);
            bool inspStart = IoMonitor.CheckInput(newValue, inspStartPort);
            bool inspEnd = IoMonitor.CheckInput(newValue, inspEndPort);
            bool commandDone = IoMonitor.CheckInput(newValue, commandDonePort);

            if (ifState == IfState.WaitTrigger1Off)
            {
                if (trigger1 == false && commandDone == false)
                    ExecuteTrigger(0);
            }
            else if (ifState == IfState.WaitTrigger2Off)
            {
                if (trigger2 == false && commandDone == false)
                    ExecuteTrigger(1);
            }
            else if (ifState == IfState.WaitCommandDoneOn)
            {
                digitalIoHandler.WriteOutput(complete1Port, false);
                digitalIoHandler.WriteOutput(complete2Port, false);

                ifState = IfState.WaitCommandDoneOff;
            }
            else if (ifState == IfState.WaitCommandDoneOff)
            {
                SetReadyState(true);
            }
            else if (trigger1)
            {
                LogHelper.Debug(LoggerType.IO, "Trigger 1 Activated");
                TriggerActivated(0);
            }
            else if (trigger2)
            {
                LogHelper.Debug(LoggerType.IO, "Trigger 2 Activated");
                TriggerActivated(1);
            }
            else if (inspStart)
            {
                LogHelper.Debug(LoggerType.IO, "Inspection Start Activated");
                InspStartActivated();
            }
            else if (inspEnd)
            {
                LogHelper.Debug(LoggerType.IO, "Inspection End Activated");
                InspEndActivated();
            }

            return true;
        }

        private void TriggerActivated(int triggerNo)
        {
            if (visionReadyPort != null)
            {
                digitalIoHandler.WriteOutput(commandWaitPort, false);

                if (triggerNo == 0)
                {
                    if (inspStartPort == null)
                        ExecuteCommand("START");

                    ifState = IfState.WaitTrigger1Off;
                }
                else
                {
                    ifState = IfState.WaitTrigger2Off;
                }

                lastTriggerNo = triggerNo;
            }
            else
            {
                ExecuteCommand("TRIG,0");

                lastTriggerNo = 0;
            }
        }

        private void ExecuteTrigger(int triggerNo)
        {
            ExecuteResult executeResult = ExecuteCommand(String.Format("TRIG,{0}", triggerNo));

            //LogHelper.Debug(LoggerType.Inspection, String.Format("Trigger No : {0} / Result : {1}", triggerNo, executeResult.inspection));

            if (triggerNo == 0)
            {
                digitalIoHandler.WriteOutput(complete1Port, true);
            }
            else
            {
                digitalIoHandler.WriteOutput(complete2Port, true);
            }

            ifState = IfState.WaitCommandDoneOn;
        }

        public void SetReadyState(bool flag)
        {
            LogHelper.Debug(LoggerType.Inspection, "Set Ready State");

            digitalIoHandler.WriteOutput(complete1Port, false);
            digitalIoHandler.WriteOutput(complete2Port, false);
            digitalIoHandler.WriteOutput(resultNgPort, false);

            if (flag)
            {
                digitalIoHandler.WriteOutput(commandWaitPort, true);
                digitalIoHandler.WriteOutput(visionReadyPort, true);
            }
            else
            {
                digitalIoHandler.WriteOutput(commandWaitPort, false);
                digitalIoHandler.WriteOutput(visionReadyPort, false);
            }
        }

        private void InspStartActivated()
        {
            digitalIoHandler.WriteOutput(commandWaitPort, false);

            ExecuteCommand("START");
        }

        private void InspEndActivated()
        {
            if (lastTriggerNo == 1)
            {
                Thread.Sleep(500);
                ExecuteCommand("END");
            }

            SetReadyState(true);

            if (lastTriggerNo == 1)
                ExecuteCommand("FINALIZE");
        }

        public override void Start()
        {
            if (ioMonitor != null)
            {
                if (visionReadyPort != null)
                    SetReadyState(true);

                ioMonitor.Start();
            }
        }

        public override void Stop()
        {
            if (ioMonitor != null)
            {
                SetReadyState(false);

                ioMonitor.Stop();
            }
        }

        protected override bool Send(MachineIfProtocol protocol)
        {
            IoMachineIfProtocol ioMachineIfProcotol = (IoMachineIfProtocol)protocol;
            this.digitalIoHandler.WriteOutput(ioMachineIfProcotol.IoPort, ioMachineIfProcotol.IoValue);
            return true;
        }

        public override void SendCommand(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
