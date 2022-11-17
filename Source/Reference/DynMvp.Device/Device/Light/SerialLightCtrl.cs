using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DynMvp.Base;
using DynMvp.Devices.Comm;
using System.Xml;

namespace DynMvp.Devices.Light
{
    public enum LightControllerVender
    {
        Iovis, Movis, AltSystem, LFine, Lvs, PSCC, VIT
    }

    public class SerialLightCtrlInfo : LightCtrlInfo
    {
        int responceTimeoutMs = 0;
        public int ResponceTimeoutMs
        {
            get { return responceTimeoutMs; }
            set { responceTimeoutMs = value; }
        }

        LightControllerVender controllerVender;
        public LightControllerVender ControllerVender
        {
            get { return controllerVender; }
            set { controllerVender = value; }
        }

        SerialPortInfo serialPortInfo = new SerialPortInfo();
        public SerialPortInfo SerialPortInfo
        {
            get { return serialPortInfo; }
            set { serialPortInfo = value;}
        }

        public SerialLightCtrlInfo()
        {
            Type = LightCtrlType.Serial;
        }

        public override void SaveXml(XmlElement lightInfoElement)
        {
            base.SaveXml(lightInfoElement);

            XmlHelper.SetValue(lightInfoElement, "ResponceTimeoutMs", responceTimeoutMs.ToString());
            XmlHelper.SetValue(lightInfoElement, "LightControllerVender", controllerVender.ToString());
            serialPortInfo.Save(lightInfoElement, "SerialLightController");
        }

        public override void LoadXml(XmlElement lightInfoElement)
        {
            base.LoadXml(lightInfoElement);

            responceTimeoutMs = XmlHelper.GetValue(lightInfoElement, "ResponceTimeoutMs", responceTimeoutMs);
            controllerVender = (LightControllerVender)Enum.Parse(typeof(LightControllerVender), XmlHelper.GetValue(lightInfoElement, "LightControllerVender", LightControllerVender.Iovis.ToString()));
            serialPortInfo.Load(lightInfoElement, "SerialLightController");
        }

        public override LightCtrlInfo Clone()
        {
            SerialLightCtrlInfo serialLightCtrlInfo = new SerialLightCtrlInfo();
            serialLightCtrlInfo.Copy(this);

            return serialLightCtrlInfo;
        }

        public override void Copy(LightCtrlInfo srcInfo)
        {
            base.Copy(srcInfo);

            SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)srcInfo;

            controllerVender = serialLightCtrlInfo.controllerVender;
            serialPortInfo.Copy(serialLightCtrlInfo.SerialPortInfo);
        }
    }

    public class SerialLightCtrl : LightCtrl
    {
        SerialLightCtrlInfo serialLightCtrlInfo;

        SerialPortEx lightSerialPort = null;
        ManualResetEvent responseReceived;

        public override int NumChannel
        {
            get { return serialLightCtrlInfo.NumChannel; }
        }

        public SerialPortEx LightSerialPort
        {
            get { return lightSerialPort; }
        }

        public SerialLightCtrl(string name)
            : base(LightCtrlType.Serial, name)
        {
            responseReceived = new ManualResetEvent(false);
        }

        public override int GetMaxLightLevel()
        {
            switch (lightControllerVender)
            {
                case LightControllerVender.LFine:
                    return 1023;
                default:
                    return 255;
            }
        }

        //public override void TurnOn()
        //{
        //    if (lastLightValue == null)
        //    {
        //        lastLightValue = new LightValue(numChannel);
        //        for (int i = 0; i < numChannel; i++)
        //            lastLightValue.Value[i] = GetMaxLightLevel();
        //    }

        //    //lastLightValue.TurnOn();

        //    TurnOn(lastLightValue);
        //}

        //public override void TurnOff()
        //{
        //    if (lightSerialPort != null)
        //    {
        //        LogHelper.Debug(LoggerType.Grab, String.Format("Turn Off light"));

        //        responseReceived = false;

        //        TimeOutTimer timeOutTimer = new TimeOutTimer();
        //        timeOutTimer.Start(10000);
                
        //        this.TurnOn(new LightValue(this.NumChannel));
        //        Thread.Sleep(lightStableTimeMs);

        //        timeOutTimer.Stop();
        //    }
        //}

        public override bool Initialize(LightCtrlInfo lightCtrlInfo)
        {
            try
            {
                this.serialLightCtrlInfo = (SerialLightCtrlInfo)lightCtrlInfo;

                this.lightControllerVender = serialLightCtrlInfo.ControllerVender;
                lightSerialPort = new SerialPortEx();
                lightSerialPort.Open(serialLightCtrlInfo.Name, serialLightCtrlInfo.SerialPortInfo);
                lightSerialPort.StartListening();
                lightSerialPort.PacketReceived += lightSerialPort_PacketReceived;
                
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Can't open serial port. {0}", ex.Message));

                lightSerialPort = null;
            }

            return false;
        }

        private void lightSerialPort_PacketReceived(byte[] dataByte)
        {
            responseReceived.Set();
        }

        public override void Release()
        {
            base.Release();

            lightSerialPort.StopListening();
            lightSerialPort.Close();
        }

        public override void TurnOn(LightValue lightValue)
        {
            if (lightSerialPort != null)
            {
                int maxVal = GetMaxLightLevel();
                for (int i = 0; i < lightValue.NumLight; i++)
                {
                    lightValue.Value[i] = Math.Min(lightValue.Value[i], maxVal);
                    lightValue.Value[i] = Math.Max(lightValue.Value[i], 0);
                }

                if (lightValue.Value.All(f => f == 0) == false)
                    this.lastLightValue = lightValue.Clone();

                LogHelper.Debug(LoggerType.Grab, String.Format("Set light value: {0}", lightValue.KeyValue));

                responseReceived.Reset();
                
                try
                {
                    switch (serialLightCtrlInfo.ControllerVender)
                    {
                        case LightControllerVender.Iovis:

                            for (int i = 0; i < serialLightCtrlInfo.NumChannel; i++)
                            {
                                string packet = String.Format("#CH{0:00}BW{1:0000}E", i + 1, lightValue.Value[StartChannelIndex + i]);
                                lightSerialPort.WritePacket(packet);
                                //Thread.Sleep(10);
                            }
                            break;
                        case LightControllerVender.Movis:
                            for (int i = 0; i < serialLightCtrlInfo.NumChannel; i++)
                            {
                                byte[] bytePacket = new byte[6];
                                bytePacket[0] = 0x95;
                                bytePacket[1] = 0x2;
                                bytePacket[2] = (byte)(i + 1);
                                bytePacket[3] = (byte)(lightValue.Value[StartChannelIndex + i] > 0 ? 1 : 0);
                                bytePacket[4] = (byte)(lightValue.Value[StartChannelIndex + i]);
                                bytePacket[5] = (byte)(bytePacket[0] + bytePacket[1] + bytePacket[2] + bytePacket[3] + bytePacket[4]);

                                lightSerialPort.WritePacket(bytePacket, 0, 6);
                                //Thread.Sleep(25);
                            }
                            break;
                        case LightControllerVender.AltSystem:

                            for (int i = 1; i < serialLightCtrlInfo.NumChannel; i++)
                            {
                                string packet = String.Format("L{0}{1:000}\r\n", i, lightValue.Value[StartChannelIndex + i]);
                                byte[] StrByte = Encoding.UTF8.GetBytes(packet);
                                lightSerialPort.WritePacket(StrByte, 0, 7);
                                //Thread.Sleep(20);
                            }
                            break;
                        case LightControllerVender.Lvs:
                            for (int i = 0; i < serialLightCtrlInfo.NumChannel; i++)
                            {
                                string packet = String.Format("L{0}{1:000}\r\n", i + 1, lightValue.Value[StartChannelIndex + i]);
                                byte[] StrByte = Encoding.UTF8.GetBytes(packet);
                                lightSerialPort.WritePacket(StrByte, 0, 7);
                                //Thread.Sleep(20);
                            }
                            break;
                        case LightControllerVender.LFine:
                            for (int i = 0; i < serialLightCtrlInfo.NumChannel; i++)
                            {
                                string valueStr = lightValue.Value[StartChannelIndex + i].ToString("0000");

                                byte[] bytePacket = new byte[8];
                                bytePacket[0] = 0x02;
                                bytePacket[1] = (byte)(i + '0');
                                bytePacket[2] = (byte)'w';
                                bytePacket[3] = (byte)valueStr[0];
                                bytePacket[4] = (byte)valueStr[1];
                                bytePacket[5] = (byte)valueStr[2];
                                bytePacket[6] = (byte)valueStr[3];
                                bytePacket[7] = 0x03;

                                lightSerialPort.WritePacket(bytePacket, 0, 8);
                                //Thread.Sleep(10);
                            }
                            break;
                        case LightControllerVender.PSCC:
                            for (int i = 0; i < serialLightCtrlInfo.NumChannel; i++)
                            {
                                string turnOnPacket = String.Format("@00L1007D\r\n");
                                byte[] turnOnByte = Encoding.UTF8.GetBytes(turnOnPacket);
                                lightSerialPort.WritePacket(turnOnByte, 0, turnOnByte.Length);
                                Thread.Sleep(100);

                                string preparePacket = String.Format("@00F{0:000}00", lightValue.Value[StartChannelIndex + i]);

                                byte[] toByte = Encoding.Default.GetBytes(preparePacket);
                                int sum = 0;
                                for (int b = 0; b < toByte.Length; b++)
                                {
                                    sum += toByte[b];
                                }

                                string checkSum = string.Format("{0:x2}", sum);
                                checkSum = checkSum.ToUpper();
                                if (checkSum.Length > 2)
                                    checkSum = checkSum.Remove(0, 1);

                                string packet = string.Format("{0}{1}\r\n", preparePacket, checkSum);

                                byte[] StrByte = Encoding.UTF8.GetBytes(packet);
                                lightSerialPort.WritePacket(StrByte, 0, StrByte.Length);
                            }
                            break;

                        case LightControllerVender.VIT:
                            for (int i = 0; i < serialLightCtrlInfo.NumChannel; i++)
                            {
                                string packet = String.Format("C{0}{1:000}\r\n", i + 1, lightValue.Value[i]);
                                byte[] turnOnByte = Encoding.ASCII.GetBytes(packet);
                                lightSerialPort.WritePacket(turnOnByte, 0, turnOnByte.Length);
                            }
                            break;
                    }

                    if (serialLightCtrlInfo.ResponceTimeoutMs != 0 && this.responseReceived.WaitOne(serialLightCtrlInfo.ResponceTimeoutMs) == false)
                        throw new Exception("Light Contoller has no responce");
                }
                catch (Exception ex)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)MachineError.Light,
                ErrorLevel.Error, ErrorSection.Machine.ToString(), MachineError.Light.ToString(), ex.Message, "", "Please, Check the Light Controller");
                }
            }
            Thread.Sleep(lightStableTimeMs);
        }
    }
}
