//using Standard.DynMvp.Base;
//using Standard.DynMvp.Devices.Comm;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;

//namespace Standard.DynMvp.Devices.Dio
//{
//    public enum DigitalIoType
//    {
//        None, Virtual, Adlink7230, Adlink7432, AlphaMotion302, AlphaMotion304, AlphaMotion314, SusiGpio, FastechEziMotionPlusR,
//        Modubus, ComizoaSd424f, TmcAexxx, NIMax, Ajin, KM6050, AlphaMotionBx
//    }

//    public enum DigitalIoError
//    {
//        CantFindMasterMotion,
//        InvalidMasterMotion
//    }

//    public class DigitalIoInfo
//    {
//        int index;
//        public int Index
//        {
//            get { return index; }
//            set { index = value; }
//        }

//        string name;
//        public string Name
//        {
//            get { return name; }
//            set { name = value; }
//        }

//        DigitalIoType type;
//        public DigitalIoType Type
//        {
//            get { return type; }
//            set { type = value; }
//        }

//        int numInPortGroup;
//        public int NumInPortGroup
//        {
//            get { return numInPortGroup; }
//            set { numInPortGroup = value; }
//        }

//        int inPortStartGroupIndex;
//        public int InPortStartGroupIndex
//        {
//            get { return inPortStartGroupIndex; }
//            set { inPortStartGroupIndex = value; }
//        }

//        int numOutPortGroup;
//        public int NumOutPortGroup
//        {
//            get { return numOutPortGroup; }
//            set { numOutPortGroup = value; }
//        }

//        int outPortStartGroupIndex;
//        public int OutPortStartGroupIndex
//        {
//            get { return outPortStartGroupIndex; }
//            set { outPortStartGroupIndex = value; }
//        }

//        int numInPort;
//        public int NumInPort
//        {
//            get { return numInPort; }
//            set { numInPort = value; }
//        }

//        int numOutPort;
//        public int NumOutPort
//        {
//            get { return numOutPort; }
//            set { numOutPort = value; }
//        }

//        SerialPortInfo serialPortInfo;
//        public SerialPortInfo SerialPortInfo
//        {
//            get { return serialPortInfo; }
//            set { serialPortInfo = value; }
//        }

//        public DigitalIoInfo()
//        {

//        }

//        public DigitalIoInfo(DigitalIoType digitalIoType, int index, int numInPortGroup, int inPortStartGroupIndex, int numInPort, int numOutPortGroup, int outPortStartGroupIndex, int numOutPort)
//        {
//            this.index = index;
//            this.type = digitalIoType;
//            this.numInPortGroup = numInPortGroup;
//            this.inPortStartGroupIndex = inPortStartGroupIndex;
//            this.numOutPortGroup = numOutPortGroup;
//            this.outPortStartGroupIndex = outPortStartGroupIndex;
//            this.numInPort = numInPort;
//            this.numOutPort = numOutPort;
//        }

//        public virtual void LoadXml(XmlElement digitalIoInfoElement)
//        {
//            name = XmlHelper.GetValue(digitalIoInfoElement, "Name", "");
//            index = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "Index", "0"));

//            string digitalIoTypeStr = XmlHelper.GetValue(digitalIoInfoElement, "Type", "AlphaMotion302");
//            try
//            {
//                type = (DigitalIoType)Enum.Parse(typeof(DigitalIoType), digitalIoTypeStr);
//            }
//            catch (Exception)
//            {
//                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.InvalidType, ErrorLevel.Error, 
//                        ErrorSection.DigitalIo.ToString(), CommonError.InvalidType.ToString(), String.Format("Invalid Digital I/O Type : {0}", digitalIoTypeStr));
//                type = DigitalIoType.AlphaMotion302;
//            }

//            inPortStartGroupIndex = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "InPortStartGroupIndex", "0"));
//            numInPortGroup = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "NumInPortGroup", "1"));
//            outPortStartGroupIndex = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "OutPortStartGroupIndex", "0"));
//            numOutPortGroup = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "NumOutPortGroup", "1"));
//            numInPort = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "NumInPort", "16"));
//            numOutPort = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "NumOutPort", "16"));
//        }

//        public virtual void SaveXml(XmlElement digitalIoInfoElement)
//        {
//            XmlHelper.SetValue(digitalIoInfoElement, "Name", name);
//            XmlHelper.SetValue(digitalIoInfoElement, "Index", index.ToString());
//            XmlHelper.SetValue(digitalIoInfoElement, "Type", type.ToString());
//            XmlHelper.SetValue(digitalIoInfoElement, "InPortStartGroupIndex", inPortStartGroupIndex.ToString());
//            XmlHelper.SetValue(digitalIoInfoElement, "NumInPortGroup", numInPortGroup.ToString());
//            XmlHelper.SetValue(digitalIoInfoElement, "OutPortStartGroupIndex", outPortStartGroupIndex.ToString());
//            XmlHelper.SetValue(digitalIoInfoElement, "NumOutPortGroup", numOutPortGroup.ToString());
//            XmlHelper.SetValue(digitalIoInfoElement, "NumInPort", numInPort.ToString());
//            XmlHelper.SetValue(digitalIoInfoElement, "NumOutPort", numOutPort.ToString());
//        }

//        public virtual DigitalIoInfo Clone()
//        {
//            DigitalIoInfo digitalIoInfo = new DigitalIoInfo();
//            digitalIoInfo.Copy(this);

//            return digitalIoInfo;
//        }

//        public virtual void Copy(DigitalIoInfo srcDigitalIoInfo)
//        {
//            name = srcDigitalIoInfo.name;
//            type = srcDigitalIoInfo.type;
//            inPortStartGroupIndex = srcDigitalIoInfo.inPortStartGroupIndex;
//            numInPortGroup = srcDigitalIoInfo.numInPortGroup;
//            outPortStartGroupIndex = srcDigitalIoInfo.outPortStartGroupIndex;
//            numOutPortGroup = srcDigitalIoInfo.numOutPortGroup;
//            numInPort = srcDigitalIoInfo.numInPort;
//            numOutPort = srcDigitalIoInfo.NumOutPort;
//        }
//    }

//    public class PciDigitalIoInfo : DigitalIoInfo
//    {
//        public override void LoadXml(XmlElement digitalIoInfoElement)
//        {
//            base.LoadXml(digitalIoInfoElement);

//            Index = Convert.ToInt32(XmlHelper.GetValue(digitalIoInfoElement, "Index", ""));
//        }

//        public override void SaveXml(XmlElement digitalIoInfoElement)
//        {
//            base.SaveXml(digitalIoInfoElement);

//            XmlHelper.SetValue(digitalIoInfoElement, "Index", Index.ToString());
//        }

//        public override DigitalIoInfo Clone()
//        {
//            PciDigitalIoInfo pciDigitalIoInfo = new PciDigitalIoInfo();
//            pciDigitalIoInfo.Copy(this);

//            return pciDigitalIoInfo;
//        }

//        public override void Copy(DigitalIoInfo srcDigitalIoInfo)
//        {
//            base.Copy(srcDigitalIoInfo);

//            PciDigitalIoInfo pciDigitalIoInfo = (PciDigitalIoInfo)srcDigitalIoInfo;
//            Index = pciDigitalIoInfo.Index;
//        }
//    }

//    public class SlaveDigitalIoInfo : DigitalIoInfo
//    {
//        string masterDeviceName;
//        public string MasterDeviceName
//        {
//            get { return masterDeviceName; }
//            set { masterDeviceName = value; }
//        }

//        public override void LoadXml(XmlElement digitalIoInfoElement)
//        {
//            base.LoadXml(digitalIoInfoElement);

//            masterDeviceName = XmlHelper.GetValue(digitalIoInfoElement, "MasterDeviceName", "");
//        }

//        public override void SaveXml(XmlElement digitalIoInfoElement)
//        {
//            base.SaveXml(digitalIoInfoElement);

//            XmlHelper.SetValue(digitalIoInfoElement, "MasterDeviceName", masterDeviceName);
//        }

//        public override DigitalIoInfo Clone()
//        {
//            SlaveDigitalIoInfo slaveDigitalIoInfo = new SlaveDigitalIoInfo();
//            slaveDigitalIoInfo.Copy(this);

//            return slaveDigitalIoInfo;
//        }

//        public override void Copy(DigitalIoInfo srcDigitalIoInfo)
//        {
//            base.Copy(srcDigitalIoInfo);

//            SlaveDigitalIoInfo slaveDigitalIoInfo = (SlaveDigitalIoInfo)srcDigitalIoInfo;
//            masterDeviceName = slaveDigitalIoInfo.MasterDeviceName;
//        }
//    }

//    public class NiDigitalIoInfo : DigitalIoInfo
//    {
//        string inputChannelLine;
//        public string InputChannelLine
//        {
//            get { return inputChannelLine; }
//            set { inputChannelLine = value; }
//        }

//        string outputChannelLine;
//        public string OutputChannelLine
//        {
//            get { return outputChannelLine; }
//            set { outputChannelLine = value; }
//        }

//        public override void LoadXml(XmlElement digitalIoInfoElement)
//        {
//            base.LoadXml(digitalIoInfoElement);

//            inputChannelLine = XmlHelper.GetValue(digitalIoInfoElement, "InputChannelLine", "Dev2/port0/line0:3");
//            outputChannelLine = XmlHelper.GetValue(digitalIoInfoElement, "OutputChannelLine", "Dev2/port1/line0:3");
//        }

//        public override void SaveXml(XmlElement digitalIoInfoElement)
//        {
//            base.SaveXml(digitalIoInfoElement);

//            XmlHelper.SetValue(digitalIoInfoElement, "InputChannelLine", inputChannelLine);
//            XmlHelper.SetValue(digitalIoInfoElement, "OutputChannelLine", outputChannelLine);
//        }

//        public override DigitalIoInfo Clone()
//        {
//            NiDigitalIoInfo niDigitalIoInfo = new NiDigitalIoInfo();
//            niDigitalIoInfo.Copy(this);

//            return niDigitalIoInfo;
//        }

//        public override void Copy(DigitalIoInfo srcDigitalIoInfo)
//        {
//            base.Copy(srcDigitalIoInfo);

//            NiDigitalIoInfo niDigitalIoInfo = (NiDigitalIoInfo)srcDigitalIoInfo;
//            inputChannelLine = niDigitalIoInfo.InputChannelLine;
//            outputChannelLine = niDigitalIoInfo.OutputChannelLine;
//        }
//    }

//    public class SerialDigitalIoInfo : DigitalIoInfo
//    {
//        public override void LoadXml(XmlElement motionElement)
//        {
//            base.LoadXml(motionElement);

//            SerialPortInfo.Load(motionElement, "SerialPortInfo");
//        }

//        public override void SaveXml(XmlElement motionElement)
//        {
//            base.SaveXml(motionElement);

//            SerialPortInfo.Save(motionElement, "SerialPortInfo");
//        }

//        public override DigitalIoInfo Clone()
//        {
//            SerialDigitalIoInfo serialDigitalIoInfo = new SerialDigitalIoInfo();
//            serialDigitalIoInfo.Copy(this);

//            return serialDigitalIoInfo;
//        }

//        public override void Copy(DigitalIoInfo srcDigitalIoInfo)
//        {
//            base.Copy(srcDigitalIoInfo);

//            SerialDigitalIoInfo serialDigitalIoInfo = (SerialDigitalIoInfo)srcDigitalIoInfo;
//            SerialPortInfo = serialDigitalIoInfo.SerialPortInfo.Clone();
//        }
//    }

//    public class DigitalIoInfoList : List<DigitalIoInfo>
//    {
//        public DigitalIoInfoList Clone()
//        {
//            DigitalIoInfoList newDigitalIoInfoList = new DigitalIoInfoList();

//            foreach (DigitalIoInfo digitalIoInfo in this)
//            {
//                newDigitalIoInfoList.Add(digitalIoInfo.Clone());
//            }

//            return newDigitalIoInfoList;
//        }

//    }

//    public class DigitalIoInfoFactory
//    {
//        public static DigitalIoInfo Create(DigitalIoType digitalIoType)
//        {
//            DigitalIoInfo digitalIoInfo = null;
//            switch (digitalIoType)
//            {
//                case DigitalIoType.AlphaMotion302:
//                case DigitalIoType.AlphaMotion304:
//                case DigitalIoType.AlphaMotion314:
//                case DigitalIoType.FastechEziMotionPlusR:
//                case DigitalIoType.Ajin:
//                case DigitalIoType.AlphaMotionBx:
//                    digitalIoInfo = new SlaveDigitalIoInfo();
//                    break;
//                case DigitalIoType.SusiGpio:
//                case DigitalIoType.Adlink7230:
//                case DigitalIoType.Adlink7432:
//                case DigitalIoType.TmcAexxx:
//                case DigitalIoType.ComizoaSd424f:
//                    digitalIoInfo = new PciDigitalIoInfo();
//                    break;
//                case DigitalIoType.Modubus:
//                    digitalIoInfo = new SerialDigitalIoInfo();
//                    break;
//                case DigitalIoType.NIMax:
//                    digitalIoInfo = new NiDigitalIoInfo();
//                    break;
//                case DigitalIoType.Virtual:
//                default:
//                    digitalIoInfo = new DigitalIoInfo();
//                    break;
//            }

//            digitalIoInfo.Type = digitalIoType;

//            return digitalIoInfo;
//        }
//    }

//    public interface IDigitalIo
//    {
//        string GetName();
//        int GetNumInPortGroup();
//        int GetNumOutPortGroup();
//        int GetInPortStartGroupIndex();
//        int GetOutPortStartGroupIndex();
//        int GetNumInPort();
//        int GetNumOutPort();

//        bool Initialize(DigitalIoInfo digitalIoInfo);
//        void Release();
//        bool IsReady();
        
//        void WriteOutputGroup(int groupNo, uint outputPortStatus);
//        void WriteInputGroup(int groupNo, uint inputPortStatus);
//        uint ReadOutputGroup(int groupNo);
//        uint ReadInputGroup(int groupNo);

//        void WriteOutputPort(int groupNo, int portNo, bool value);
//    }

//    public abstract class DigitalIo : Device, IDigitalIo
//    {
//        protected DigitalIoType digitalIoType;
//        public DigitalIoType DigitalIoType
//        {
//            get { return digitalIoType; }
//        }

//        private int numInPort;
//        public int NumInPort
//        {
//            get { return numInPort; }
//            set { numInPort = value; }
//        }

//        private int numOutPort;
//        public int NumOutPort
//        {
//            get { return numOutPort; }
//            set { numOutPort = value; }
//        }

//        int numInPortGroup;
//        public int NumInPortGroup
//        {
//            get { return numInPortGroup; }
//            set { numInPortGroup = value; }
//        }

//        int inPortStartGroupIndex;
//        public int InPortStartGroupIndex
//        {
//            get { return inPortStartGroupIndex; }
//            set { inPortStartGroupIndex = value; }
//        }

//        int numOutPortGroup;
//        public int NumOutPortGroup
//        {
//            get { return numOutPortGroup; }
//            set { numOutPortGroup = value; }
//        }

//        int outPortStartGroupIndex;
//        public int OutPortStartGroupIndex
//        {
//            get { return outPortStartGroupIndex; }
//            set { outPortStartGroupIndex = value; }
//        }

//        public const int UnusedPortNo = 255;

//        public DigitalIo(DigitalIoType digitalIoType, string name)
//        {
//            if (name == "")
//                Name = digitalIoType.ToString();
//            else
//                Name = name;

//            this.digitalIoType = digitalIoType;

//            DeviceType = DeviceType.DigitalIo;
//            UpdateState(DeviceState.Idle);
//        }

//        public string GetName() { return Name;  }

//        public int GetNumInPortGroup() { return numInPortGroup; }
//        public int GetNumOutPortGroup() { return numOutPortGroup; }
//        public int GetInPortStartGroupIndex() { return inPortStartGroupIndex; }
//        public int GetOutPortStartGroupIndex() { return outPortStartGroupIndex; }
//        public int GetNumInPort() { return numInPort; }
//        public int GetNumOutPort() { return numOutPort; }

//        public abstract bool Initialize(DigitalIoInfo digitalIoInfo);
//        public abstract void WriteOutputGroup(int groupNo, uint outputPortStatus);
//        public abstract void WriteInputGroup(int groupNo, uint inputPortStatus); // 가상 입력용
//        public abstract uint ReadOutputGroup(int groupNo);
//        public abstract uint ReadInputGroup(int groupNo);
//        public abstract bool IsReady();
//        public virtual void WriteOutputPort(int groupNo, int portNo, bool value)
//        {
//            uint current = ReadOutputGroup(groupNo);
//            if (value)
//                current |= (uint)(0x01 << portNo);
//            else
//                current &= (uint)(~(0x01 << portNo));

//            WriteOutputGroup(groupNo, current);
//        }
        
//        public static void UpdateBitFlag(ref uint portStatus, int bitPos, bool value)
//        {
//            if (bitPos == UnusedPortNo)
//                return;

//            uint nSift = 0x0001;

//            nSift <<= bitPos;

//            if (value == true)
//            {
//                portStatus |= nSift;
//            }
//            else
//            {
//                nSift = ~nSift;
//                portStatus &= nSift;
//            }
//        }
//    }
//}
