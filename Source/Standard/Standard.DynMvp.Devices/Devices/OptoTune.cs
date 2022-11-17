﻿//using Standard.DynMvp.Base;
//using Standard.DynMvp.Devices.Comm;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Xml;

//namespace DynMvp.Device
//{
//    public class CRC16IBM
//    {
//        const ushort polynomial = 0xA001;
//        ushort[] table = new ushort[256];

//        public ushort ComputeChecksum(byte[] bytes)
//        {
//            ushort crc = 0;
//            for (int i = 0; i < bytes.Length; ++i)
//            {
//                byte index = (byte)(crc ^ bytes[i]);
//                crc = (ushort)((crc >> 8) ^ table[index]);
//            }
//            return crc;
//        }

//        public byte[] ComputeChecksumBytes(byte[] bytes)
//        {
//            ushort crc = ComputeChecksum(bytes);
//            return BitConverter.GetBytes(crc);
//        }

//        public CRC16IBM()
//        {
//            ushort value;
//            ushort temp;
//            for (ushort i = 0; i < table.Length; ++i)
//            {
//                value = 0;
//                temp = i;
//                for (byte j = 0; j < 8; ++j)
//                {
//                    if (((value ^ temp) & 0x0001) != 0)
//                    {
//                        value = (ushort)((value >> 1) ^ polynomial);
//                    }
//                    else
//                    {
//                        value >>= 1;
//                    }
//                    temp >>= 1;
//                }
//                table[i] = value;
//            }
//        }
//    }

//    public class OptoTuneSetting
//    {
//        public enum Params
//        {
//            MinCurrent = 0,
//            MaxCurrent,
//            PARAM_MAX
//        }

//        double minCurrent = 0.0;
//        public double MinCurrent
//        {
//            get { return minCurrent; }
//            set { minCurrent = value; }
//        }

//        double maxCurrent=290.0;
//        public double MaxCurrent
//        {
//            get { return maxCurrent; }
//            set { maxCurrent = value; }
//        }


//        public string GetParamName(int iIdx)
//        {
//            string strName = "";

//            switch ((Params)iIdx)
//            {
//                case Params.MinCurrent: strName = "MinimumCurrent"; break;
//                case Params.MaxCurrent: strName = "MaximumCurrent"; break;
//            }

//            return strName;
//        }

//        public string GetParamValue(string strParamName)
//        {
//            string strVal = "";
//            string strName = "";
//            bool bFind = false;
//            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
//            {
//                strName = GetParamName(i);

//                if (strName == strParamName)
//                {
//                    bFind = true;
//                    switch ((Params)i)
//                    {
//                        case Params.MinCurrent: strVal = minCurrent.ToString(); break;
//                        case Params.MaxCurrent: strVal = maxCurrent.ToString(); break;
//                        default: bFind = false; break;
//                    }
//                    break;
//                }
//            }
//            if (bFind == false)
//            {
//                throw new InvalidOperationException();
//            }

//            return strVal;
//        }

//        public void SetParamValue(string strParamName, string strVal)
//        {
//            string strName = "";
//            bool bFind = false;
//            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
//            {
//                strName = GetParamName(i);

//                if (strName == strParamName)
//                {
//                    bFind = true;
//                    switch ((Params)i)
//                    {
//                        case Params.MinCurrent: minCurrent = Convert.ToDouble(strVal); break;
//                        case Params.MaxCurrent: maxCurrent= Convert.ToDouble(strVal); break;
//                        default: bFind = false; break;
//                    }
//                    break;
//                }
//            }
//            if (bFind == false)
//            {
//                throw new InvalidOperationException();
//            }
//        }

//        public void Load(XmlElement xmlElement)
//        {
//            string strParamName;
//            string strParamVal;
//            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
//            {
//                strParamName = GetParamName(i);
//                strParamVal = XmlHelper.GetValue(xmlElement, strParamName, "");
//                if (strParamVal != "")
//                {
//                    SetParamValue(strParamName, strParamVal);
//                }
//            }
//        }

//        public void Save(XmlElement xmlElement)
//        {
//            string strParamName = "";
//            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
//            {
//                strParamName = GetParamName(i);
//                XmlHelper.SetValue(xmlElement, strParamName, GetParamValue(strParamName));
//            }
//        }
//    }

//    public class OptoTune
//    {
//        SerialPortEx serialPort = null;
//        CRC16IBM crc16ibm = new CRC16IBM();
//        OptoTuneSetting setting = null;
//        double curCurrent;

//        public void Initialize(SerialPortInfo serialPortInfo, OptoTuneSetting setting)
//        {
//            this.setting = setting;
//            serialPort = new SerialPortEx();
//            serialPort.Open("OptoTune", serialPortInfo);
//        }

//        private byte[] StringToByteArray(string str)
//        {
//            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
//            return encoding.GetBytes(str);
//        }

//        public void SetDcMode()
//        {
//            if (serialPort.IsOpen == false)
//                return;

//            string message = "MWDA";
//            byte[] command = StringToByteArray(message);
//            byte[] commandWithCRC = AddCRC(command);
//            serialPort.WritePacket(commandWithCRC, 0, commandWithCRC.Length);

//            Thread.Sleep(500);
//        }

//        public double GetCurrent()
//        {
//            return curCurrent;
//        }

//        public bool IsOnNegLimit()
//        {
//            return (curCurrent == setting.MinCurrent);
//        }

//        public bool IsOnPosLimit()
//        {
//            return (curCurrent == setting.MaxCurrent);
//        }

//        public void SetCurrent(double current)
//        {
//            if(current > setting.MaxCurrent)
//            {
//                current = setting.MaxCurrent;
//            }

//            if (current < setting.MinCurrent)
//            {
//                current = setting.MinCurrent;
//            }
//            curCurrent = current;
            
//            //int value = (int)(curCurrent / setting.MaxCurrent * 4096);
//            int value = (int)(curCurrent / 292.84 * 4096.0+0.5);

//            string message = "Aw";

//            byte[] command = StringToByteArray(message);

//            //int testValue = 1402;
//            int testValue = value; // 전류값 삽입
//            byte[] testByte = BitConverter.GetBytes(testValue);
//            if (BitConverter.IsLittleEndian)
//                Array.Reverse(testByte); // 요소 순서를 바꿔줌

//            byte[] finalByteValue = new byte[2];
//            Array.Copy(testByte, 2, finalByteValue, 0, 2);

//            byte[] addCommand = Combine(command, finalByteValue); //
//            byte[] commandWithCRC = AddCRC(addCommand); //byte command += testByte;                    

//            serialPort.WritePacket(commandWithCRC, 0, commandWithCRC.Length); //입력한 데이터 전송 Current Value + CRC

//            Thread.Sleep(0);
//        }

//        private byte[] AddCRC(byte[] command)
//        {
//            UInt16 CRC = 0;

//            byte[] commandWithCRC = new byte[command.Length + 2];

//            CRC = crc16ibm.ComputeChecksum(command);

//            Array.Copy(command, 0, commandWithCRC, 0, command.Length);

//            commandWithCRC[commandWithCRC.Length - 2] = (byte)(CRC & 0xFF);
//            commandWithCRC[commandWithCRC.Length - 1] = (byte)(CRC >> 8);

//            return commandWithCRC;
//        }

//        private byte[] Combine(byte[] a, byte[] b)
//        {
//            byte[] c = new byte[a.Length + b.Length];
//            System.Buffer.BlockCopy(a, 0, c, 0, a.Length);
//            System.Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
//            return c;
//        }
//    }
//}
