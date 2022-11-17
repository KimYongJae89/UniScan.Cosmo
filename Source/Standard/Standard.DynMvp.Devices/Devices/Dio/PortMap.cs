using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Standard.DynMvp.Base;
using Standard.DynMvp.Devices;
using System.Diagnostics;

namespace Standard.DynMvp.Devices.Dio
{
    public class PortList
    {
        protected List<IoPort> ioPortList = new List<IoPort>();
        public List<IoPort> IoPortList
        {
            get { return ioPortList; }
        }

        public IEnumerator<IoPort> GetEnumerator()
        {
            return ioPortList.GetEnumerator();
        }
        
        public IoPort GetIoPort(string portName)
        {
            IoPort ioPort = ioPortList.Find(x => x.Name == portName);

            //Debug.Assert(ioPort != null, String.Format("I/O Port {0} is not defined.", portName));

            return ioPort;
        }

        public List<IoPort> GetIoPorts(string portName)
        {
            List<IoPort> ioPortList = this.ioPortList.FindAll(x => x.Name.Contains(portName));

            Debug.Assert(ioPortList != null, String.Format("I/O Port {0} is not defined.", portName));

            return ioPortList;
        }

        public List<IoPort> GetIoPorts(IoGroup group)
        {
            List<IoPort> ioPortList = this.ioPortList.FindAll(x => x.Group == group);

            //Debug.Assert(ioPortList.Count > 0, String.Format("I/O Port {0} is not defined.", group.ToString()));

            return ioPortList;
        }

        public IoPort GetIoPort(int portNo, int groupNo = 0, int deviceNo = 0)
        {
            IoPort ioPort = ioPortList.Find(x =>
            {
                return (x.PortNo == portNo) && (x.GroupNo == groupNo) && (x.DeviceNo == deviceNo);
            });

            return ioPort;
        }

        public void AddIoPort(IoPort ioPort)
        {
            //if (ioPortList.Exists(f => f.Equals(ioPort) == false))
                ioPortList.Add(ioPort);
        }

        public string GetPortName(int deviceNo, int portNo)
        {
            IoPort ioPort = ioPortList.Find(x => x.DeviceNo == deviceNo && x.PortNo == portNo);

            if (ioPort == null)
                return "";

            return ioPort.Name;
        }

        public bool SetPort(string portName, int deviceNo, int portNo, bool activeLow)
        {
            IoPort ioPort = ioPortList.Find(x => x.Name == portName);

            if (ioPort == null)
            {
                ioPort = new IoPort(portName);
                this.AddIoPort(ioPort);
            }

            ioPort.DeviceNo = deviceNo;
            ioPort.PortNo = portNo;
            ioPort.ActiveLow = activeLow;

            return true;
        }

        public void ClearPort()
        {
            ioPortList.Clear();
        }

        public void ClearPort(string portName)
        {
            ioPortList.RemoveAll(x => x.Name == portName);
        }

        public void ClearPort(int portNo)
        {
            ioPortList.RemoveAll(x => x.PortNo == portNo);
        }

        private int GetMaxPortNo()
        {
            return ioPortList.Max(x => x.PortNo);
        }

        public List<string> GetPortNames(int deviceIndex, int groupIndex, int numPorts)
        {
            string[] portNames = new string[numPorts];
            ioPortList.ForEach(x => { if (x.PortNo != IoPort.UNUSED_PORT_NO && x.GroupNo == groupIndex && x.DeviceNo == deviceIndex) portNames[x.PortNo] = x.Name; });

            return portNames.ToList();
        }

        public List<string> GetPortNames()
        {
            List<string> portNames = new List<string>();
            ioPortList.ForEach(x => { portNames.Add(x.Name); });

            return portNames;
        }

        public void Save(XmlElement portListElement)
        {
            foreach (IoPort ioPort in ioPortList)
            {
                XmlElement portElement = portListElement.OwnerDocument.CreateElement("", "Port", "");
                portListElement.AppendChild(portElement);

                XmlHelper.SetValue(portElement, "Name", ioPort.Name);
                XmlHelper.SetValue(portElement, "DeviceNo", ioPort.DeviceNo.ToString());
                XmlHelper.SetValue(portElement, "PortNo", ioPort.PortNo.ToString());
                XmlHelper.SetValue(portElement, "ActiveLow", ioPort.ActiveLow.ToString());
            }
        }

        public void Load(XmlElement portListElement)
        {
            foreach (XmlElement portElement in portListElement)
            {
                if (portElement.Name == "Port")
                {
                    string portName = XmlHelper.GetValue(portElement, "Name", "");
                    int deviceNo = Convert.ToInt32(XmlHelper.GetValue(portElement, "DeviceNo", "0"));
                    int portNo = Convert.ToInt32(XmlHelper.GetValue(portElement, "PortNo", "-1"));
                    bool activeLow = Convert.ToBoolean(XmlHelper.GetValue(portElement, "ActiveLow", "false"));

                    // ioPortList에 존재하는 것만 로드함 -> 정의되지 않은 포트 이름은 무시
                    //if (String.IsNullOrEmpty(portName) == false)
                    //    SetPort(portName, deviceNo, portNo, invert);

                    IoPort ioPort = ioPortList.Find(f => f.Name == portName);
                    if(ioPort!=null)
                    {
                        ioPort.DeviceNo = deviceNo;
                        ioPort.PortNo = portNo;
                        ioPort.ActiveLow = activeLow;
                    }
                }
            }
        }

        internal void Clear()
        {
            ioPortList.Clear();
        }
    }



    public class PortMapBase
    {
        //Type ioPortNameType=null;
        //public Type IoPortNameType
        //{
        //    get { return ioPortNameType; }
        //    set { ioPortNameType = value; }
        //}

        PortList inPortList = new PortList();
        public PortList InPortList
        {
            get { return inPortList; }
            set { inPortList = value; }
        }

        PortList outPortList = new PortList();
        public PortList OutPortList
        {
            get { return outPortList; }
            set { outPortList = value; }
        }

        public virtual void Initialize(Type ioPortNameType)
        {
            AddInPorts(ioPortNameType);
            AddOutPorts(ioPortNameType);
        }
        
        private void AddInPorts(Type ioPortNameType)
        {
            string[] names = Enum.GetNames(ioPortNameType);
            int index = 0;
            foreach(string name in names)
                if(name.Substring(0,2)=="In")
                    AddInPort(new IoPort(name, index++));
        }

        private void AddOutPorts(Type ioPortNameType)
        {
            string[] names = Enum.GetNames(ioPortNameType);
            foreach (string name in names)
                if(name.Substring(0,3)=="Out")
                    AddOutPort(new IoPort(name));
        }

        public IoPort GetInPort(Enum ioPortName)
        {
            return inPortList.GetIoPort(ioPortName.ToString());
        }

        public IoPort GetInPort(string portName)
        {
            return inPortList.GetIoPort(portName);
        }

        public IoPort GetInPort(int portNo, int groupNo = 0, int deviceNo = 0)
        {
            return inPortList.GetIoPort(portNo, groupNo, deviceNo);
        }

        public IoPort GetOutPort(Enum ioPortName)
        {
            return outPortList.GetIoPort(ioPortName.ToString());
        }

        public IoPort GetOutPort(string portName)
        {
            return outPortList.GetIoPort(portName);
        }
        
        public IoPort GetOutPort(int portNo, int groupNo = 0, int deviceNo = 0)
        {
            return outPortList.GetIoPort(portNo, groupNo, deviceNo);
        }

        public IoPort AddInPort(IoPort ioPort)
        {
            inPortList.AddIoPort(ioPort);
            return ioPort;
        }

        public IoPort AddOutPort(IoPort ioPort)
        {
            outPortList.AddIoPort(ioPort);
            return ioPort;
        }

        public virtual IoPort[] GetTowerLampPort()
        {
            return null;
        }

        public void Save(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement portListElement = xmlDocument.CreateElement("", "PortList", "");
            xmlDocument.AppendChild(portListElement);

            if (inPortList != null)
            {
                XmlElement inportListElement = xmlDocument.CreateElement("", "Inport", "");
                portListElement.AppendChild(inportListElement);

                inPortList.Save(inportListElement);
            }

            if (outPortList != null)
            {
                XmlElement outportListElement = xmlDocument.CreateElement("", "Outport", "");
                portListElement.AppendChild(outportListElement);

                outPortList.Save(outportListElement);
            }

            XmlHelper.Save(xmlDocument, fileName);
        }

        public void Load(string fileName)
        {
            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
                return;

            XmlElement portListElement = xmlDocument.DocumentElement;

            if (inPortList != null)
                inPortList.Load(portListElement["Inport"]);

            if (outPortList != null)
                outPortList.Load(portListElement["Outport"]);
        }

        public virtual void Load()
        {

        }

        public virtual void Save()
        {
            // 상속 클래스에서 파일명을 지정해서 저장한다.
            // 기존 UniEye 코드와의 호환성 때문에, 여기서 저장하지 않도록 했다.
        }

        public virtual void GetIoLightPorts(List<IoPort> lightPortList)
        {

        }

        public virtual void GetInDoorPorts(List<IoPort> doorPortList)
        {
            doorPortList.AddRange(InPortList.GetIoPorts(IoGroup.Door));
            doorPortList.AddRange(InPortList.GetIoPorts("Door"));
        }

        public virtual void GetOutDoorPorts(List<IoPort> doorPortList)
        {
            doorPortList.AddRange(outPortList.GetIoPorts(IoGroup.Door));
            doorPortList.AddRange(outPortList.GetIoPorts("Door"));
        }

    }

    public class PortMap : PortMapBase
    {
        public enum IoPortNameBase
        {
            InEmergency, InStartSw, InStopSw, InResetSw, InDoorOpen1, InDoorOpen2, InDoorOpen3, InAirPressure,
            OutInspStart, OutInspEnd, InInspTrig, InInspTrig2, Done, OutStartLamp, OutStopLamp, OutResetLamp, OutTowerRed, OutTowerYellow, OutTowerGreen, OutTowerBuzzer, OutIoLight1, OutIoLight2, OutIoLight3,
            OutVaccum, OutIonizer, OutIonizerSol,
            OutVisionReady, OutCommandWait, OutComplete, OutResultNg, OutOnWorking
        }

        //protected IoPort inEmergency = new IoPort("Emergency");
        public IoPort InEmergency { get { return GetInPort(IoPortNameBase.InEmergency); } }

        //protected IoPort inStartSw = new IoPort("Start Switch");
        public IoPort InStartSw { get { return GetInPort(IoPortNameBase.InStartSw); } }

        //protected IoPort inStopSw = new IoPort("Stop Switch");
        public IoPort InStopSw { get { return GetInPort(IoPortNameBase.InStopSw); } }

        //protected IoPort inResetSw = new IoPort("Reset Switch");
        public IoPort InResetSw { get { return GetInPort(IoPortNameBase.InResetSw); } }

        //protected IoPort inDoorOpen1 = new IoPort("Door Open1");
        public IoPort InDoorOpen1 { get { return GetInPort(IoPortNameBase.InDoorOpen1); } }

        //protected IoPort inDoorOpen2 = new IoPort("Door Open2");
        public IoPort InDoorOpen2 { get { return GetInPort(IoPortNameBase.InDoorOpen2); } }

        //protected IoPort inDoorOpen3 = new IoPort("Door Open3");
        public IoPort InDoorOpen3 { get { return GetInPort(IoPortNameBase.InDoorOpen3); } }

        //protected IoPort inAirPressureLow = new IoPort("Air Pressure Low");
        public IoPort InAirPressureLow { get { return GetInPort(IoPortNameBase.InAirPressure); } }

        //protected IoPort outStartLamp = new IoPort("Start Lamp");
        public IoPort OutStartLamp { get { return GetOutPort(IoPortNameBase.OutStartLamp); } }

        //protected IoPort outStopLamp = new IoPort("Stop Lamp");
        public IoPort OutStopLamp { get { return GetOutPort(IoPortNameBase.OutStopLamp); } }

        //protected IoPort outResetLamp = new IoPort("Reset Lamp");
        public IoPort OutResetLamp { get { return GetOutPort(IoPortNameBase.OutResetLamp); } }

        //protected IoPort outTowerLampRed = new IoPort("Tower Lamp Red");
        public IoPort OutTowerLampRed { get { return GetOutPort(IoPortNameBase.OutTowerRed); } }

        //protected IoPort outTowerLampYellow = new IoPort("Tower Lamp Yellow");
        public IoPort OutTowerLampYellow { get { return GetOutPort(IoPortNameBase.OutTowerYellow); } }

        //protected IoPort outTowerLampGreen = new IoPort("Tower Lamp Green");
        public IoPort OutTowerLampGreen { get { return GetOutPort(IoPortNameBase.OutTowerGreen); } }

        //protected IoPort outTowerBuzzer = new IoPort("Tower Buzzer");
        public IoPort OutTowerBuzzer { get { return GetOutPort(IoPortNameBase.OutTowerBuzzer); } }

        //protected IoPort outLight1 = new IoPort("Light 1");
        public IoPort OutLight1 { get { return GetOutPort(IoPortNameBase.OutIoLight1); } }

        //protected IoPort outLight2 = new IoPort("Light 2");
        public IoPort OutLight2 { get { return GetOutPort(IoPortNameBase.OutIoLight2); } }

        //protected IoPort outLight3 = new IoPort("Light 3");
        public IoPort OutLight3 { get { return GetOutPort(IoPortNameBase.OutIoLight3); } }

        //protected IoPort outVisionReady = new IoPort("Vision Ready");
        public IoPort OutVisionReady { get { return GetOutPort(IoPortNameBase.OutVisionReady); } }

        public IoPort OutOnWorking { get { return GetOutPort(IoPortNameBase.OutOnWorking); } }

        //protected IoPort outCommandWait = new IoPort("Command Wait");
        public IoPort OutCommandWait { get { return GetOutPort(IoPortNameBase.OutCommandWait); } }

        //protected IoPort inInspStart = new IoPort("Insp Start");
        public IoPort InInspStart { get { return GetInPort(IoPortNameBase.OutInspStart); } }

        //protected IoPort inInspEnd = new IoPort("Insp End");
        public IoPort InInspEnd { get { return GetInPort(IoPortNameBase.OutInspEnd); } }

        //protected IoPort inTrigger = new IoPort("Trigger");
        public IoPort InTrigger { get { return GetInPort(IoPortNameBase.InInspTrig); } }

        //protected IoPort inTriggerCh1 = new IoPort("Trigger Ch1");
        //public IoPort InTriggerCh1 { get { return inTriggerCh1; } }

        //protected IoPort inTriggerCh2 = new IoPort("Trigger Ch2");
        public IoPort InTriggerCh2 { get { return GetInPort(IoPortNameBase.InInspTrig2); } }

        //protected IoPort outComplete = new IoPort("Complete");
        public IoPort OutComplete { get { return GetOutPort(IoPortNameBase.OutComplete); } }

        //protected IoPort inCommandDone = new IoPort("Command Done");
        public IoPort InCommandDone { get { return GetInPort(IoPortNameBase.Done); } }

        //protected IoPort outResultNg = new IoPort("Result NG");
        public IoPort OutResultNg { get { return GetOutPort(IoPortNameBase.OutResultNg); } }

        public PortMap(bool init = true)
        {
            if (init)
                Initialize(typeof(IoPortNameBase));
        }

        public virtual void SetupPorts()
        {
            Load();
        }

        public override void Load()
        {
            //String filePath = String.Format("{0}\\PortMap.xml", PathSettings.Instance().Config);
            //if (File.Exists(filePath))
            //{
            //    Load(filePath);
            //}
            //else
            //{
            //    //Initialize(typeof(IoPortName));
            //    Save();
            //}
        }

        public override void Save()
        {
            //String filePath = String.Format("{0}\\PortMap.xml", PathSettings.Instance().Config);
            //Save(filePath);
        }

        public override void GetIoLightPorts(List<IoPort> lightPortList)
        {
            if (OutLight1.PortNo != -1)
                lightPortList.Add(OutLight1);
            if (OutLight2.PortNo != -1)
                lightPortList.Add(OutLight2);
            if (OutLight3.PortNo != -1)
                lightPortList.Add(OutLight3);

            lightPortList.AddRange(InPortList.GetIoPorts(IoGroup.Light));
        }
    }

    public class PortMapFactory
    {
        public static PortMap Create()
        {
            return new PortMap();
        }
    }
}
