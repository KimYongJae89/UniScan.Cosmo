using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Device.Serial
{
    public enum ESerialDeviceType    { SerialEncoder, BarcodeReader, SerialSensor }

    public class SerialDeviceInfo
    {
        string deviceName;
        public string DeviceName
        {
            get { return deviceName; }
            set { deviceName = value; }
        }

        ESerialDeviceType deviceType;
        public ESerialDeviceType DeviceType
        {
            get { return deviceType; }
            set { deviceType = value; }
        }

        SerialPortInfo serialPortInfo = new SerialPortInfo();
        public SerialPortInfo SerialPortInfo
        {
            get { return serialPortInfo; }
            set { serialPortInfo = value; }
        }

        public bool IsVirtual { get { return serialPortInfo.PortNo < 0; } }

        public virtual SerialDevice BuildSerialDevice(bool virtualMode)
        {
            throw new NotImplementedException();
        }

        public virtual SerialDeviceInfo Clone()
        {
            SerialDeviceInfo serialDeviceInfo = new SerialDeviceInfo();
            serialDeviceInfo.CopyFrom(this);

            return serialDeviceInfo;
        }

        public virtual void CopyFrom(SerialDeviceInfo serialDeviceInfo)
        {
            this.deviceName = serialDeviceInfo.deviceName;
            this.deviceType = serialDeviceInfo.deviceType;
            this.serialPortInfo = serialDeviceInfo.serialPortInfo.Clone();
        }

        public void Save(XmlElement xmlElement, string subKey = null)
        {
            if (xmlElement == null) return;
            if(string.IsNullOrEmpty(subKey)==false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(subKey);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }
            SaveXml(xmlElement);
        }

        public virtual void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "DeviceName", deviceName);
            XmlHelper.SetValue(xmlElement, "DeviceType", deviceType.ToString());
            serialPortInfo.Save(xmlElement, "SerialPortInfo");
        }

        public void Load(XmlElement xmlElement, string subKey = null)
        {
            if (xmlElement == null) return;
            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = xmlElement[subKey];
                Load(subElement);
                return;
            }
            LoadXml(xmlElement);
        }

        public virtual void LoadXml(XmlElement xmlElement)
        {
            deviceName = XmlHelper.GetValue(xmlElement, "DeviceName", "");
            deviceType = (ESerialDeviceType)Enum.Parse(typeof(ESerialDeviceType), XmlHelper.GetValue(xmlElement, "DeviceType", ""));
            serialPortInfo.Load(xmlElement, "SerialPortInfo");
        }
    }

    public class SerialDeviceInfoList : List<SerialDeviceInfo>
    {
        public SerialDeviceInfoList Clone()
        {
            SerialDeviceInfoList newSerialPortInfoList = new SerialDeviceInfoList();

            foreach (SerialDeviceInfo serialPortInfo in this)
            {
                newSerialPortInfoList.Add(serialPortInfo.Clone());
            }

            return newSerialPortInfoList;
        }
    }

    public class SerialDevice 
    {
        //public EncodePacketDelegate EncodePacket = null;
        //public DecodePacketDelegate DecodePacket = null;

        protected SerialDeviceInfo deviceInfo = null;
        public SerialDeviceInfo DeviceInfo
        {
            get { return deviceInfo; }
            set { deviceInfo = value; }
        }
        
        protected bool isAlarmed;
        public bool IsAlarmed
        {
            get { return isAlarmed; }
        }

        protected SerialPortEx serialPortEx = null;
        public SerialPortEx SerialPortEx
        {
            get { return serialPortEx; }
        }

        object lockObject = new object();
        protected ManualResetEvent waitResponce = new ManualResetEvent(false);
        protected string lastResponce = null;
        protected TimeOutTimer timeOutTimer = null;

        public SerialDevice(SerialDeviceInfo deviceInfo)
        {
            this.deviceInfo = deviceInfo;
            ErrorManager.Instance().OnStartAlarmState += ErrorManager_OnStartAlarm;
            ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmStatus;
        }

        private void ErrorManager_OnStartAlarm()
        {
            this.isAlarmed = true;
        }

        private void ErrorManager_OnResetAlarmStatus()
        {
            this.isAlarmed = false;
        }

        public bool IsReady()
        {
            return this.deviceInfo.IsVirtual || (this.serialPortEx != null && this.serialPortEx.IsOpen);
        }

        public virtual bool Initialize()
        {
            serialPortEx = new SerialPortEx();

            PacketParser packetParser = CreatePacketParser();
            serialPortEx.PacketHandler = new PacketHandler(packetParser);
            packetParser.OnDataReceived += packetParser_OnDataReceived;

            if (deviceInfo.IsVirtual)
            {
                return true;
            }

            bool ok = serialPortEx.Open(deviceInfo.DeviceName, deviceInfo.SerialPortInfo);
            if (ok)
            {
                serialPortEx.StartListening();
            }
            else
            {
                ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)CommonError.FailToInitialize,
                    ErrorLevel.Error, ErrorSection.Machine.ToString(), CommonError.FailToInitialize.ToString(), "Fail to Open Port");
            }
            return ok;
        }

        protected virtual void packetParser_OnDataReceived(ReceivedPacket receivedPacket)
        {
            lastResponce = serialPortEx.PacketHandler.PacketParser.DecodePacket(receivedPacket.ReceivedData);
            waitResponce.Set();
        }

        public virtual void Release()
        {
            serialPortEx.StopListening();
            serialPortEx.Close();
        }

        public virtual Enum GetCommand(string command) { throw new NotImplementedException(); }
        public virtual string MakePacket(string command, params string[] args) { throw new NotImplementedException(); }

        public string[] ExcuteCommand(Enum command, params string[] args)
        {
            return ExcuteCommand(command.ToString(), args);
        }

        public string[] ExcuteCommand(string command, params string[] args)
        {
            string packetString = MakePacket(command, args);
            return ExcuteCommand(packetString);
        }

        public string[] ExcuteCommand(string packetString)
        {
            if (this.isAlarmed)
                return null;

            lock (lockObject)
            {
                lastResponce = null;
                waitResponce.Reset();
                bool sendOk = SendCommand(packetString);
                if (sendOk == false)
                    return null;

                bool waitOk = WaitResponce(100);
                if (waitOk == false)
                {
                    LogHelper.Error(LoggerType.Serial, string.Format("SerialDevice({0} responce timeout)", this.deviceInfo.DeviceName));
                    //this.isAlarmed = true;
                    //ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)MachineError.Serial,
                    //   ErrorLevel.Fatal, MachineError.Serial.ToString(), this.deviceInfo.DeviceName, "Serial Device Responce Timeout");
                    //throw new TimeoutException(string.Format("SerialDevice {0}", this.deviceInfo.DeviceName));
                    return null;
                //return new string[0];
                }

                string[] token = lastResponce.Split(',');
                lastResponce = null;
                return token;
            }
        }

        protected virtual bool SendCommand(string packetString)
        {
            if (this.isAlarmed)
                return false;

            byte[] packet = serialPortEx.PacketHandler.PacketParser.EncodePacket(packetString);
            return serialPortEx.WritePacket(packet, 0, packet.Length);
        }

        private bool WaitResponce(int waitTimeMs=-1)
        {
            return waitResponce.WaitOne(waitTimeMs);
        }

        public virtual PacketParser CreatePacketParser()
        {
            SimplePacketParser simplePacketParser= new SimplePacketParser();
            return simplePacketParser;
        }
    }

    public class SerialDeviceHandler
    {
        List<SerialDevice> serialDeviceList = new List<SerialDevice>();

        public void Add(SerialDevice serialDevice)
        {
            serialDeviceList.Add(serialDevice);
        }

        public SerialDevice Find(Predicate<SerialDevice> p)
        {
            return serialDeviceList.Find(p);
        }

        public void Release()
        {
            foreach (SerialDevice serialDevice in serialDeviceList)
            {
                if (serialDevice.IsReady())
                    serialDevice.Release();
            }
            serialDeviceList.Clear();
        }
    }
}
