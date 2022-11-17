﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

using DynMvp.Base;

namespace DynMvp.Devices.Comm
{
    public delegate void PacketReceivedDelegate(byte[] dataByte);

    public class SerialPortEx
    {
        SerialPort serialPort = new SerialPort();

        bool virtualPort = false;
        public bool VirtualPort
        {
            get { return virtualPort; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string PortName
        {
            get { return serialPort.PortName; }
            set { serialPort.PortName = value; }
        }

        private PacketData packetData = new PacketData();
        public PacketData PacketData
        {
            get { return packetData; }
        }

        private PacketHandler packetHandler = new PacketHandler();
        public PacketHandler PacketHandler
        {
            get { return packetHandler; }
            set { packetHandler = value; }
        }

        public PacketReceivedDelegate PacketReceived;

        public bool Open(string name, SerialPortInfo serialPortInfo)
        {
            if (serialPortInfo.PortName != "None")
            {
                if (serialPortInfo.PortName != "Virtual")
                {
                    this.name = name;
                    serialPort.PortName = serialPortInfo.PortName;
                    serialPort.BaudRate = serialPortInfo.BaudRate;
                    serialPort.DataBits = serialPortInfo.DataBits;
                    serialPort.StopBits = serialPortInfo.StopBits;
                    serialPort.Parity = serialPortInfo.Parity;
                    serialPort.RtsEnable = serialPortInfo.RtsEnable;
                    serialPort.DtrEnable = serialPortInfo.DtrEnable;
                   
                    try
                    {
                        if (SerialPortManager.Instance().IsPortAvailable(serialPortInfo.PortName) == false)
                            throw new InvalidResourceException("Port is Not available");

                        serialPort.Open();

                        SerialPortManager.Instance().AddSerialPort(this);
                    }
                    catch (Exception ex)
                    {
                        ErrorManager.Instance().Report((int)ErrorSection.Initialize, (int)CommonError.FailToInitialize,
    ErrorLevel.Error, ErrorSection.Machine.ToString(), CommonError.FailToInitialize.ToString(), string.Format("Serial Port \"{0}\" Open Failed", this.name), ex.Message, "");
                        return false;
                    }
                }
                else
                {
                    virtualPort = true;
                }
            }
            else
            {
                virtualPort = true;
            }

            return true;
        }

        public bool IsOpen
        {
            get { return serialPort.IsOpen; }
        }

        public void Close()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                SerialPortManager.Instance().ReleaseSerialPort(this);
            }
        }

        public void StartListening()
        {
            if (serialPort.IsOpen)
            {
                serialPort.DiscardInBuffer();

                if (virtualPort == false)
                    serialPort.DataReceived += PortDataReceived;
            }
        }

        public void Purge()
        {
            if (serialPort.IsOpen)
            {
                serialPort.DiscardInBuffer();
            }
        }

        public void StopListening()
        {
            if (virtualPort == false)
                serialPort.DataReceived -= PortDataReceived;
        }

        // MSYS : 시리얼 데이터 받는 위치
        private void PortDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                int byteToRead = serialPort.BytesToRead;

                byte[] dataByte = new byte[byteToRead];
#if DEBUG
                serialPort.Read(dataByte, 0, byteToRead);
                ProcessPacket(dataByte);
#else
                try
                {
                    serialPort.Read(dataByte, 0, byteToRead);
                    ProcessPacket(dataByte);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, String.Format("Serial Port Read Error : Name - {0} / Msg - {1} ", name, ex.Message));
                }
#endif
            }
        }

        // 가상 모드 동작을 위해 별도의 함수로 분리
        public void ProcessPacket(byte[] dataByte)
        {
            if (PacketReceived != null)
                PacketReceived(dataByte);
            else if (packetHandler != null)
                packetHandler.ProcessPacket(dataByte, packetData);
            else
                LogHelper.Debug(LoggerType.Serial, "Packet Handler is Empty.");
        }

        public void SendRequest()
        {
            PacketParser packetParser = packetHandler.PacketParser;

            if (packetParser != null)
            {
                byte[] dataByte = packetParser.EncodePacket("Request");
                serialPort.Write(dataByte, 0, dataByte.Length);
         
                string writePacket = System.Text.Encoding.Default.GetString(dataByte.ToArray());
                LogHelper.Debug(LoggerType.Serial, "Write Packet : " + writePacket);
            }
            else
            {
                LogHelper.Debug(LoggerType.Serial, "Packet Parser is Empty.");
            }
        }

        public void ReleaseEventHandler()
        {
            // DataReceived -= new SerialDataReceivedEventHandler(PortDataReceived);
        }

        public bool WritePacket(byte[] buffer, int offset, int count)
        {
            try
            {
                if (virtualPort == false && serialPort.IsOpen)
                {
                    serialPort.Write(buffer, offset, count);
                    return true;
                }
                return false;
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public void WritePacket(string packet)
        {
            if (virtualPort == false && this.IsOpen)
            {
                serialPort.Write(packet);
                //LogHelper.Debug(LoggerType.Serial, string.Format("{0} send: {1}", this.PortName, packet.Trim(new char[] { '\r', '\n' })));
            }
        }
    }
}
