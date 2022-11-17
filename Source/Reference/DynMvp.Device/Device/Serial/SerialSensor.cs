using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.Device.Serial;
using DynMvp.Devices.Comm;

namespace DynMvp.Device.Serial
{
    public enum SensorType
    {
        GT2_70
    }

    public class SerialSensorInfo : SerialDeviceInfo
    {
        SensorType sensorType;
        public SensorType SensorType { get => sensorType; set => sensorType = value; }

        public override SerialDevice BuildSerialDevice(bool virtualMode)
        {
            if (virtualMode)
                this.SerialPortInfo.PortName= "Virtual";

            return SerialSensor.Create(this);
        }

        public override SerialDeviceInfo Clone()
        {
            SerialSensorInfo serialSensorInfo = new SerialSensorInfo();
            serialSensorInfo.CopyFrom(this);
            return serialSensorInfo;
        }

        public override void CopyFrom(SerialDeviceInfo serialDeviceInfo)
        {
            SerialSensorInfo serialSensorInfo = (SerialSensorInfo)serialDeviceInfo;
            base.CopyFrom(serialDeviceInfo);
        }

        public override void SaveXml(XmlElement xmlElement)
        {
            base.SaveXml(xmlElement);
            //XmlHelper.SetValue(xmlElement, "Resolution", resolution.ToString());
        }

        public override void LoadXml(XmlElement xmlElement)
        {
            base.LoadXml(xmlElement);
            //resolution = Convert.ToDouble(XmlHelper.GetValue(xmlElement, "Resolution", resolution.ToString()));
        }
    }

    public abstract class SerialSensor : SerialDevice
    {
        public SerialSensor(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
        }

        public static SerialSensor Create(SerialDeviceInfo deviceInfo)
        {
            if (deviceInfo.IsVirtual)
                return new SerialSensorVirtual(deviceInfo);

            SerialSensorInfo serialSensorInfo = (SerialSensorInfo)deviceInfo;

            switch (serialSensorInfo.SensorType)
            {
                case SensorType.GT2_70:
                    return new SerialSensorGT2_70(deviceInfo);
                default:
                    return new SerialSensorVirtual(deviceInfo);
            }
        }

        public abstract bool GetData(out float d1, out float d2);
    }

    public class SerialSensorGT2_70 : SerialSensor
    {
        public SerialSensorGT2_70(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
            //deviceInfo.DeviceName = "GT2_70";
        }

        public override bool Initialize()
        {
            serialPortEx = new SerialPortEx();

            PacketParser packetParser = CreatePacketParser();
            serialPortEx.PacketHandler = new PacketHandler(packetParser);

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

        public override PacketParser CreatePacketParser()
        {
            SimplePacketParser simplePacketParser = new SimplePacketParser();
            simplePacketParser.EndChar = new byte[2] { (byte)'\r', (byte)'\n' };
            simplePacketParser.OnDataReceived += packetParser_OnDataReceived;
            return simplePacketParser;
        }
        
        public override void Release()
        {
            serialPortEx.StopListening();
            serialPortEx.Close();
        }
        
        public override bool GetData(out float d1, out float d2)
        {
            d1 = d2 = 0;
            string[] tokens = ExcuteCommand("M0");
            if (tokens == null || tokens.Length < 3)
                return false;

            if (tokens[0] == "M0")
            {
                d1 = float.Parse(tokens[1]);
                d2 = float.Parse(tokens[2]);
                return true;
            }
            return false;
        }
    }

    public class SerialSensorVirtual : SerialSensorGT2_70
    {
        public SerialSensorVirtual(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
        }

        protected override bool SendCommand(string packetString)
        {
            if (this.isAlarmed)
                return false;

            byte[] packet = serialPortEx.PacketHandler.PacketParser.EncodePacket(packetString);
            byte[] virtualResponce = BuildVirtualResponce();
            Task.Run(()=>serialPortEx.ProcessPacket(virtualResponce));
            return true;
        }

        double coefficient = 0.000;
        private byte[] BuildVirtualResponce()
        {
            double[] data = new double[] { 0, 0 };
            if (true)
            {
                if (coefficient > 4)
                    coefficient = 0;

                if (coefficient > 2)
                {
                    data[0] = 0.03 * Math.Sin(coefficient * Math.PI);
                    data[1] = 0.03 * Math.Cos(coefficient * Math.PI);
                }
                coefficient += 0.1;
            }
            else
            {
                data[0] = data[1] = 0.0200;
            }

            List<string> list = new List<string>();
            list.Add("M0");
            Array.ForEach(data, f => list.Add(f.ToString("F6")));

            byte[] dummyData = serialPortEx.PacketHandler.PacketParser.EncodePacket(string.Join(",", list));
            return dummyData;
        }
    }
}
