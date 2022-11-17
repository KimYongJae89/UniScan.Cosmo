using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

//for LAN
using Gardasoft.Controller.API.Exceptions;
using Gardasoft.Controller.API.Interfaces;
using Gardasoft.Controller.API.Managers;
using Gardasoft.Controller.API.Model;
using Gardasoft.Controller.API.EventsArgs;

namespace DynMvp.Devices
{
    public class CRC16IBM
    {
        const ushort polynomial = 0xA001;
        ushort[] table = new ushort[256];

        public ushort ComputeChecksum(byte[] bytes)
        {
            ushort crc = 0;
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }
            return crc;
        }

        public byte[] ComputeChecksumBytes(byte[] bytes)
        {
            ushort crc = ComputeChecksum(bytes);
            return BitConverter.GetBytes(crc);
        }

        public CRC16IBM()
        {
            ushort value;
            ushort temp;
            for (ushort i = 0; i < table.Length; ++i)
            {
                value = 0;
                temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                    {
                        value = (ushort)((value >> 1) ^ polynomial);
                    }
                    else
                    {
                        value >>= 1;
                    }
                    temp >>= 1;
                }
                table[i] = value;
            }
        }
    }

    public class OptoTuneSetting
    {
        public enum Params
        {
            MinCurrent = 0,
            MaxCurrent,
            PARAM_MAX
        }

        double minCurrent = 0.0;
        public double MinCurrent
        {
            get { return minCurrent; }
            set { minCurrent = value; }
        }

        double maxCurrent = 290.0;
        public double MaxCurrent
        {
            get { return maxCurrent; }
            set { maxCurrent = value; }
        }

        public string GetParamName(int iIdx)
        {
            string strName = "";

            switch ((Params)iIdx)
            {
                case Params.MinCurrent: strName = "MinimumCurrent"; break;
                case Params.MaxCurrent: strName = "MaximumCurrent"; break;
            }

            return strName;
        }

        public string GetParamValue(string strParamName)
        {
            string strVal = "";
            string strName = "";
            bool bFind = false;
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strName = GetParamName(i);

                if (strName == strParamName)
                {
                    bFind = true;
                    switch ((Params)i)
                    {
                        case Params.MinCurrent: strVal = minCurrent.ToString(); break;
                        case Params.MaxCurrent: strVal = maxCurrent.ToString(); break;
                        default: bFind = false; break;
                    }
                    break;
                }
            }
            if (bFind == false)
            {
                throw new InvalidOperationException();
            }

            return strVal;
        }

        public void SetParamValue(string strParamName, string strVal)
        {
            string strName = "";
            bool bFind = false;
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strName = GetParamName(i);

                if (strName == strParamName)
                {
                    bFind = true;
                    switch ((Params)i)
                    {
                        case Params.MinCurrent: minCurrent = Convert.ToDouble(strVal); break;
                        case Params.MaxCurrent: maxCurrent = Convert.ToDouble(strVal); break;
                        default: bFind = false; break;
                    }
                    break;
                }
            }
            if (bFind == false)
            {
                throw new InvalidOperationException();
            }
        }

        public void Load(XmlElement xmlElement)
        {
            string strParamName;
            string strParamVal;
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strParamName = GetParamName(i);
                strParamVal = XmlHelper.GetValue(xmlElement, strParamName, "");
                if (strParamVal != "")
                {
                    SetParamValue(strParamName, strParamVal);
                }
            }
        }

        public void Save(XmlElement xmlElement)
        {
            string strParamName = "";
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strParamName = GetParamName(i);
                XmlHelper.SetValue(xmlElement, strParamName, GetParamValue(strParamName));
            }
        }
    }

    public abstract class OptoTune
    {
        protected OptoTuneSetting setting = null;
        protected double curCurrent;

        public void Initialize(OptoTuneSetting setting)
        {
            this.setting = setting;
        }

        public abstract bool IsConnected();
        // for Serial
        public virtual bool Connect(SerialPortInfo serialPortInfo) { return false; }
        // For Lan
        public virtual bool Connect() { return false; }

        public abstract void Disconnect();

        protected byte[] StringToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        protected virtual double LimitCheck(double checkValue)
        {
            if (checkValue > setting.MaxCurrent)
                checkValue = setting.MaxCurrent;

            if (checkValue < setting.MinCurrent)
                checkValue = setting.MinCurrent;

            return checkValue;
        }

        public virtual bool IsOnNegLimit()
        {
            return (curCurrent == setting.MinCurrent);
        }

        public virtual bool IsOnPosLimit()
        {
            return (curCurrent == setting.MaxCurrent);
        }

        public abstract void SetCurrent(double current);
        public abstract double GetCurrent();

        public virtual double GetMinCurrent()
        {
            return setting.MinCurrent;
        }

        public virtual double GetMaxCurrent()
        {
            return setting.MaxCurrent;
        }
    }

    public class OptoTuneSerial : OptoTune
    {
        SerialPortEx serialPort = null;
        CRC16IBM crc16ibm = new CRC16IBM();

        public override bool Connect(SerialPortInfo serialPortInfo)
        {
            serialPort = new SerialPortEx();
            return serialPort.Open("OptoTune", serialPortInfo);
        }

        public override void Disconnect()
        {
            if (serialPort.IsOpen)
                serialPort.Close();
        }

        public override bool IsConnected()
        {
            return serialPort != null && serialPort.IsOpen;
        }

        public void SetDcMode()
        {
            if (serialPort.IsOpen == false)
                return;

            string message = "MWDA";
            byte[] command = StringToByteArray(message);
            byte[] commandWithCRC = AddCRC(command);
            serialPort.WritePacket(commandWithCRC, 0, commandWithCRC.Length);

            Thread.Sleep(500);
        }

        public override double GetCurrent()
        {
            return curCurrent;
        }

        public override void SetCurrent(double current)
        {
            curCurrent = LimitCheck(current);

            //int value = (int)(curCurrent / setting.MaxCurrent * 4096);
            int value = (int)(curCurrent / 292.84 * 4096.0 + 0.5);

            string message = "Aw";

            byte[] command = StringToByteArray(message);

            //int testValue = 1402;
            int testValue = value; // 전류값 삽입
            byte[] testByte = BitConverter.GetBytes(testValue);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(testByte); // 요소 순서를 바꿔줌

            byte[] finalByteValue = new byte[2];
            Array.Copy(testByte, 2, finalByteValue, 0, 2);

            byte[] addCommand = Combine(command, finalByteValue); //
            byte[] commandWithCRC = AddCRC(addCommand); //byte command += testByte;                    

            serialPort.WritePacket(commandWithCRC, 0, commandWithCRC.Length); //입력한 데이터 전송 Current Value + CRC

            Thread.Sleep(0);
        }

        private byte[] AddCRC(byte[] command)
        {
            UInt16 CRC = 0;

            byte[] commandWithCRC = new byte[command.Length + 2];

            CRC = crc16ibm.ComputeChecksum(command);

            Array.Copy(command, 0, commandWithCRC, 0, command.Length);

            commandWithCRC[commandWithCRC.Length - 2] = (byte)(CRC & 0xFF);
            commandWithCRC[commandWithCRC.Length - 1] = (byte)(CRC >> 8);

            return commandWithCRC;
        }

        private byte[] Combine(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            System.Buffer.BlockCopy(a, 0, c, 0, a.Length);
            System.Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
            return c;
        }
    }

    public class OptoTuneLAN : OptoTune
    {
        private ControllerManager _controllerManager;
        private IChannel _activeChannel;
        private IController _activeController;

        public override bool Connect()
        {
            _controllerManager = ControllerManager.Instance();
            _controllerManager.DiscoverControllers();
            
            if (_controllerManager.Controllers.Count <= 0)
                return false;

            _activeController = _controllerManager.Controllers.First();
            //foreach (IController controller in _controllerManager.Controllers)
            //    _activeController = controller;

            try
            {
                //_activeController.StatusChanged += ActiveControllerStatusChanged;
                //_activeController.ConnectionStatusChanged += ActiveControllerConnectionStatusChanged;

                _activeController.Open();

                if (_activeController.IsTrinitiController)
                {

                    // Subscribe to register value changes to demonstrate monitoring dynamic registers
                    if (_activeController.Channels.Count > 0)
                    {
                        _activeChannel = _activeController.Channels[0];
                    }

                    //_activeChannel.Registers.Refresh();
                }
                else
                {
                    return false;
                }
            }
            catch (FailedToOpenControllerGardasoftException exception)
            {
                //_activeController.ConnectionStatusChanged -= ActiveControllerConnectionStatusChanged;
                //_activeController.StatusChanged -= ActiveControllerStatusChanged;
                //ResetForm();
                //LensControlUIReset();
                //MessageBox.Show(exception.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //SetCurrent(0.0);

            return true;
        }

        public override void Disconnect()
        {
            if (_controllerManager.Controllers.Count > 0)
            {
                // Populate controller ComboBox
                foreach (IController controller in _controllerManager.Controllers)
                {
                    if (controller.IsOpen)
                        controller.Close();
                }
            }
        }

        public override bool IsConnected()
        {
            if (_controllerManager.Controllers.Count <= 0)
                return false;

            // Populate controller ComboBox
            foreach (IController controller in _controllerManager.Controllers)
            {
                if (!controller.IsOpen)
                    return false;
            }

            return true;
        }

        public override double GetCurrent()
        {
            if (_activeChannel != null && _activeChannel.Registers["FocalPowerValue"] != null)
                return Convert.ToDouble(_activeChannel.Registers["FocalPowerValue"].CurrentValue) * 100.0F;

            return 0;
        }

        public override void SetCurrent(double current)
        {
            if (_activeChannel != null && _activeChannel.Registers["FocalPowerValue"] != null)
            {
                float minValue = Convert.ToSingle(_activeChannel.Registers["FocalPowerMin"].CurrentValue) * 100.0F;
                float maxValue = Convert.ToSingle(_activeChannel.Registers["FocalPowerMax"].CurrentValue) * 100.0F;

                if (minValue > current || maxValue < current)
                    return;

                curCurrent = current;

                float fpValue = Convert.ToSingle(current) / 100.0F;
                _activeChannel.Registers["FocalPowerValue"].CurrentValue = fpValue;
            }
        }

        public override double GetMaxCurrent()
        {
            if (_activeChannel != null && _activeChannel.Registers["FocalPowerMax"] != null)
                return Convert.ToSingle(_activeChannel.Registers["FocalPowerMax"].CurrentValue) * 100.0F;
            
            return 0;
        }

        public override double GetMinCurrent()
        {
            if (_activeChannel != null && _activeChannel.Registers["FocalPowerMin"] != null)
                return Convert.ToSingle(_activeChannel.Registers["FocalPowerMin"].CurrentValue) * 100.0F;

            return 0;
        }

        protected override double LimitCheck(double checkValue)
        {
            if (checkValue > GetMaxCurrent())
                checkValue = GetMaxCurrent();

            if (checkValue < GetMinCurrent())
                checkValue = GetMinCurrent();

            return checkValue;
        }

        public override bool IsOnNegLimit()
        {
            return curCurrent <= GetMinCurrent();
        }

        public override bool IsOnPosLimit()
        {
            return curCurrent >= GetMaxCurrent();
        }
    }
}
