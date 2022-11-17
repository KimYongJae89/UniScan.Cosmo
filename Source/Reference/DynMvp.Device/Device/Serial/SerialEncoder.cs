using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.Device.Serial;
using DynMvp.Devices.Comm;

namespace DynMvp.Device.Serial
{
    public class SerialEncoderInfo : SerialDeviceInfo
    {
        /// <summary>
        /// um per pulse
        /// </summary>
        double resolution = 7.0;
        public double Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }

        public override SerialDevice BuildSerialDevice(bool virtualMode)
        {
            if (virtualMode)
                this.SerialPortInfo.PortName = "Virtual";

            return SerialEncoder.Create(this);
        }

        public override SerialDeviceInfo Clone()
        {
            SerialEncoderInfo serialEncoderInfo = new SerialEncoderInfo();
            serialEncoderInfo.CopyFrom(this);
            return serialEncoderInfo;
        }

        public override void CopyFrom(SerialDeviceInfo serialDeviceInfo)
        {
            SerialEncoderInfo serialEncoderInfo = (SerialEncoderInfo)serialDeviceInfo;

            base.CopyFrom(serialDeviceInfo);
            this.resolution = serialEncoderInfo.resolution;
        }

        public override void SaveXml(XmlElement xmlElement)
        {
            base.SaveXml(xmlElement);
            XmlHelper.SetValue(xmlElement, "Resolution", resolution.ToString());
        }

        public override void LoadXml(XmlElement xmlElement)
        {
            base.LoadXml(xmlElement);
            resolution=Convert.ToDouble(XmlHelper.GetValue(xmlElement, "Resolution", resolution.ToString()));
        }
    }

    public abstract class SerialEncoder : SerialDevice
    {
        public abstract string Version { get; }

        public SerialEncoder(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
        }

        public abstract UInt32 GetPositionPls();
        public abstract double GetSpeedPlsPerMs();

        public abstract bool IsCompatible(string command);
        public bool IsCompatible(Enum command)
        {
            return IsCompatible(command.ToString());
        }

        public static SerialEncoder Create(SerialDeviceInfo deviceInfo)
        {
            if (deviceInfo.IsVirtual)
                return new SerialEncoderVirtual(deviceInfo);

            string errorMessage = "";
            SerialEncoder serialEncoder = null;
            switch (CheckVersion(deviceInfo))
            {
                default:
                case "1.07":
                case "1.06":
                    serialEncoder =new SerialEncoderV107(deviceInfo);
                    break;

                case "1.05":
                    serialEncoder = new SerialEncoderV105(deviceInfo);
                    break;

                case null:
                    errorMessage = "SerialEncoder Initialize Fail (Not Responce)";
                    break;
                //default:
                //    errorMessage = "SerialEncoder Initialize Fail (Version in not compitable)";
                //    break;
            }

            if (string.IsNullOrEmpty(errorMessage) == false)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)ErrorSubSection.CommonReason, ErrorLevel.Error, ErrorSection.Machine.ToString(), ErrorSubSection.CommonReason.ToString(), errorMessage);
                LogHelper.Error(LoggerType.Device, errorMessage);
            }

            if (serialEncoder == null)
                serialEncoder = new SerialEncoderVirtual(deviceInfo);

            return serialEncoder;
        }

        private static string CheckVersion(SerialDeviceInfo deviceInfo)
        {
            SerialDevice serialDevice= new SerialDevice(deviceInfo);
            if (serialDevice.Initialize() == false)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Machine, (int)MachineError.Serial,
                    ErrorLevel.Fatal, MachineError.Serial.ToString(), deviceInfo.DeviceName, "Serial Device initialize fail.");
                return null;
            }

            string[] token = serialDevice.ExcuteCommand("VR\r\n");
            serialDevice.Release();

            if (token == null)
                return null;
            else if (token[0] == "ER")
                return "1.05";
            return token[1].Trim();
            //return "1.05";
            return "1.07";
        }
    }

    public class SerialEncoderV105 : SerialEncoder
    {
        public enum ECommand
        {
            AP,
            CP,
            DL, PW, DV, FQ, ED, OS, EN, AR, IN,
            GR
        }

        /// <summary>
        /// PosDiff per Ms
        /// </summary>
        double[] speedBuffer = new double[9];
        int speedBufferIdx = 0;
        ThreadHandler threadHandler = null;

        public override string Version
        {
            get { return "1.05"; }
        }

        public override Enum GetCommand(string command)
        {
            ECommand res;
            if (Enum.TryParse<ECommand>(command, out res))
                return res;
            return null;
        }

        public override bool IsCompatible(string command)
        {
            return Enum.GetNames(typeof(ECommand)).Contains(command);
        }

        //public bool IsCompatible(Enum command)
        //{
        //    //return command.GetType() == typeof(ECommand);
        //    return IsCompatible(command.ToString());
        //}

        public SerialEncoderV105(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
            timeOutTimer = new TimeOutTimer();
        }

        public override bool Initialize()
        {
            bool ok = base.Initialize();
            if (ok)
            {
                ExcuteCommand(ECommand.DL, "0");
                ExcuteCommand(ECommand.PW, "50");
                ExcuteCommand(ECommand.IN, "0");
                ExcuteCommand(ECommand.ED, "0");
                ExcuteCommand(ECommand.DV, "4");
                ExcuteCommand(ECommand.CP);
                ExcuteCommand(ECommand.EN, "1");

                threadHandler = new ThreadHandler("SerialEncoderV105", new Thread(SpeedMeasureProc));
                threadHandler.Start();
            }
            return ok;
        }

        public override void Release()
        {
            threadHandler?.Stop(1000);
            threadHandler = null;
            base.Release();
        }

        public override string MakePacket(string command, params string[] args)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(command.ToString());
            foreach (string arg in args)
            {
                sb.AppendFormat(",{0}", arg);
            }
            return sb.ToString();
        }

        public override PacketParser CreatePacketParser()
        {
            SimplePacketParser packetParser = new SimplePacketParser();
            packetParser.EndChar = new byte[2] { (byte)'\r', (byte)'\n' };
            //this.EncodePacket = packetParser.EncodePacket;
            //this.DecodePacket = packetParser.DecodePacket;
            return packetParser;
        }
        
        public override uint GetPositionPls()
        {
            string[] token = ExcuteCommand(ECommand.AP);
            uint pos;
            if (token==null || token.Length<2 || (uint.TryParse(token[1], out pos) == false))
                return 0;
            return pos;
        }

        private void SpeedMeasureProc()
        {
            DateTime startTime = DateTime.Now;
            DateTime prevDateTime = DateTime.Now;
            long prevPos = GetPositionPls();
            System.Diagnostics.Stopwatch commStopwatch = new System.Diagnostics.Stopwatch();

            while (threadHandler.RequestStop==false)
            {
                Thread.Sleep(20);
                long curPos;
                commStopwatch.Restart();
                try
                {
                    curPos = GetPositionPls();
                }
                catch (TimeoutException)
                {
                    continue;
                }
                DateTime curDateTime = DateTime.Now;
                commStopwatch.Stop();
                long commTime = commStopwatch.ElapsedMilliseconds;

                //long posDiff = Math.Abs(curPos - prevPos);
                //if (Math.Abs(posDiff) > uint.MaxValue / 4)    // overflow or underflow
                //{
                //    prevDateTime = curDateTime;
                //    prevPos = curPos;
                //    continue;
                //}
                long posDiff = curPos - prevPos;
                if (posDiff >= 0)
                {
                    double timeDiff = (curDateTime - prevDateTime).TotalMilliseconds;
                    double curVel = posDiff / timeDiff; // [pulse/milisec]
                                                        //LogHelper.Debug(LoggerType.Grab, string.Format("curVelosity: {0} = {1} / {2}, CommTime: {3}", curVel, posDiff, timeDiff, commTime));

                    // Median Filter
                    this.speedBuffer[this.speedBufferIdx] = curVel;
                    this.speedBufferIdx = (this.speedBufferIdx + 1) % speedBuffer.Count();
                }
                prevDateTime = curDateTime;
                prevPos = curPos;
            }
        }

        private double Median(double[] filterBuffer)
        {
            double[] sortedFilterBuffer = (double[])filterBuffer.Clone();
            Array.Sort(sortedFilterBuffer);

            int halfLen = filterBuffer.Length / 2;
            if ((filterBuffer.Length % 2) == 1)
                return sortedFilterBuffer[halfLen];
            else
                return (sortedFilterBuffer[halfLen] + sortedFilterBuffer[halfLen + 1]) / 2;
        }

        public override double GetSpeedPlsPerMs()
        {
            return Median(this.speedBuffer);
        }
    }

    public class SerialEncoderVirtual : SerialEncoderV105
    {
        Dictionary<ECommand, string> dic = new Dictionary<ECommand, string>();
        ThreadHandler virtualModeThread = null;

        public SerialEncoderVirtual(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
            deviceInfo.SerialPortInfo.PortName = "Virtual";

            dic.Add(ECommand.AP, "0");
            dic.Add(ECommand.DL, "0");
            dic.Add(ECommand.PW, "0");
            dic.Add(ECommand.DV, "0");
            dic.Add(ECommand.FQ, "0");
            dic.Add(ECommand.ED, "0");
            dic.Add(ECommand.OS, "0");
            dic.Add(ECommand.EN, "0");
            dic.Add(ECommand.AR, "0,1000000");
            dic.Add(ECommand.IN, "0");
        }

        public override bool Initialize()
        {
            bool ok = base.Initialize();
            if (ok)
            {
                virtualModeThread = new ThreadHandler("SerialEncoderVirtual", new Thread(Thread_WorkingThread));
                virtualModeThread.WorkingThread.Start();
            }
            return ok;
        }

        public override void Release()
        {
            base.Release();
            virtualModeThread?.Stop();
            virtualModeThread = null;
        }

        private void Thread_WorkingThread()
        {
            // 80 [m/m]
            // 1.333 [m/s]
            // 7 [um/line]
            // 1142857.142857143 [line/1000ms]
            long virtualCount = 0;
            while (virtualModeThread.RequestStop == false)
            {
                Thread.Sleep(10);

                double step = double.Parse(dic[ECommand.FQ]);
                step /= 100;
                if (step == 0)
                    step = 80.0 / 60.0 / 0.000007 / 100.0;

                if (dic[ECommand.IN] == "1")
                {
                    virtualCount++;
                    dic[ECommand.AP] = ((long)(virtualCount * step)).ToString();
                }
            }
        }

        protected override bool SendCommand(string v)
        {
            Task.Factory.StartNew(() =>
            {
                ProcessCommand(v);
                this.serialPortEx.PacketHandler.PacketParser.OnDataReceived(CreateReceivedPacket(v));
            });
            return true;
        }

        private void ProcessCommand(string v)
        {
            string[] token = v.Trim().Split(',');
            ECommand command = (ECommand)Enum.Parse(typeof(ECommand), token[0]);

            switch (command)
            {
                case ECommand.CP:
                    dic[ECommand.AP] = "0";
                    break;
                case ECommand.AR:
                    dic[command] = token[1]+","+ token[2];
                    break;
                case ECommand.AP:
                case ECommand.GR:
                    break;
                default:
                    dic[command] = token[1];
                    break;
            }
        }

        private ReceivedPacket CreateReceivedPacket(string wirtePacket)
        {
            StringBuilder sb = new StringBuilder();
            string[] token = wirtePacket.Trim().Split(',');
            ECommand command = (ECommand)Enum.Parse(typeof(ECommand), token[0]);
            switch (command)
            {
                case ECommand.CP:
                case ECommand.AP:
                    sb.AppendLine(string.Format("{0},{1}", ECommand.AP, dic[ECommand.AP]));
                    break;

                case ECommand.GR:
                    sb.Append(string.Format("{0},{1},", ECommand.DL, dic[ECommand.DL]));
                    sb.Append(string.Format("{0},{1},", ECommand.PW, dic[ECommand.PW]));
                    sb.Append(string.Format("{0},{1},", ECommand.DV, dic[ECommand.DV]));
                    sb.Append(string.Format("{0},{1},", ECommand.FQ, dic[ECommand.FQ]));
                    sb.Append(string.Format("{0},{1},", ECommand.ED, dic[ECommand.ED]));
                    sb.Append(string.Format("{0},{1},", ECommand.OS, dic[ECommand.OS]));
                    sb.Append(string.Format("{0},{1},", ECommand.EN, dic[ECommand.EN]));
                    sb.Append(string.Format("{0},{1},", ECommand.AR, dic[ECommand.AR]));
                    sb.AppendLine(string.Format("{0},{1}", ECommand.IN, dic[ECommand.IN]));
                    break;

                default:
                    sb.Append(wirtePacket);
                    break;
            }
            byte[] packet = this.serialPortEx.PacketHandler.PacketParser.EncodePacket(sb.ToString());
            string packetString = sb.ToString();
            ReceivedPacket receivedPacket = new ReceivedPacket(packet);

            return receivedPacket;
        }
    }

    public class SerialEncoderV107 : SerialEncoderV105
    {
        public enum ECommandV2 
        {
            CY, PC, CC, // Send
            RC  // Responce
        }

        public override string Version
        {
            get { return "1.07"; }

        }
        public override Enum GetCommand(string command)
        {
            Enum e = base.GetCommand(command);
            if (e == null)
            {
                ECommandV2 res;
                if (Enum.TryParse<ECommandV2>(command, out res))
                    return res;
                return null;
            }
            return e;
        }

        public override bool IsCompatible(string command)
        {
            if (base.IsCompatible(command))
                return true;

            return Enum.GetNames(typeof(ECommandV2)).Contains(command);
        }

        public SerialEncoderV107(SerialDeviceInfo deviceInfo) : base(deviceInfo)
        {
        }

        public override bool Initialize()
        {
            bool ok = base.Initialize();
            if (ok)
            {
                ExcuteCommand(ECommandV2.CC);
                ExcuteCommand(ECommandV2.CY, "5000000");  // 5000000 * 20ns = 100ms
            }
            return ok;
        }

        public override double GetSpeedPlsPerMs()
        {
            string[] paramArray = ExcuteCommand(ECommand.GR);
            if (paramArray == null || paramArray.Length < 25)
                return -1;
            double timeMs = int.Parse(paramArray[24]) / 50e6 * 1000;


            string[] token = ExcuteCommand(ECommandV2.PC);
            if (token == null || token.Length != 2)
                return -1;

            int pls = int.Parse(token[1]);

            return pls / timeMs;
        }
    }

}
