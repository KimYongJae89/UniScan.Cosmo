using Standard.DynMvp.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.DynMvp.Devices.Serial
{
    public enum ComPort
    {
        COM1, COM2, COM3, COM4, COM5, COM6, COM7, COM8, COM9, COM10, COM11, COM12
    }

    public enum SerialParity
    {
        None = 0,
        Odd = 1,
        Even = 2,
        Mark = 3,
        Space = 4
    }
    
    public enum SerialStopBitCount
    {
        One = 0,
        OnePointFive = 1,
        Two = 2
    }
    
    public enum SerialHandshake
    {
        None = 0,
        RequestToSend = 1,
        XOnXOff = 2,
        RequestToSendXOnXOff = 3
    }

    public class SerialInfo : ProtocolInfo
    {
        [SettingData(SettingDataType.Enum)]
        public ComPort Comport { get; set; } = ComPort.COM1;

        [SettingData(SettingDataType.Numeric)]
        public UInt32 BaudRate { get; set; } = 9600;

        [SettingData(SettingDataType.Enum)]
        public SerialParity Parity { get; set; } = SerialParity.None;

        [SettingData(SettingDataType.Enum)]
        public SerialStopBitCount StopBits { get; set; } = SerialStopBitCount.One;

        [SettingData(SettingDataType.Numeric)]
        public UInt16 DataBits { get; set; } = 8;

        [SettingData(SettingDataType.Enum)]
        public SerialHandshake Handshake { get; set; } = SerialHandshake.XOnXOff;

        public const Boolean BreakSignalState_false = false;
        public const Boolean BreakSignalState_true = true;

        public const Boolean IsDataTerminalReady_false = false;
        public const Boolean IsDataTerminalReady_true = true;

        public const Boolean IsRequestToSendEnabled_false = false;
        public const Boolean IsRequestToSendEnabled_true = true;

        public SerialInfo() : base(ProtocolType.Serial)
        {
        }
    }
}
