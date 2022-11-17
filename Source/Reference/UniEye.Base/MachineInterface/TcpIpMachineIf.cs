using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace UniEye.Base.MachineInterface
{
    public class TcpIpMachineIfProtocol : MachineIfProtocol
    {
        protected string[] args;
        public string[] Args
        {
            get { return args; }
            set { args = value; }
        }

        public TcpIpMachineIfProtocol(Enum command) : base(command, false, 500)
        {
        }

        public TcpIpMachineIfProtocol(Enum command, bool use, int waitResponceMs) : base(command, use, waitResponceMs)
        {
        }

        public override MachineIfProtocol Clone()
        {
            TcpIpMachineIfProtocol tcpIpMachineIfProtocol = new TcpIpMachineIfProtocol(this.command, this.use, this.waitResponceMs);
            if(this.args!=null)
                tcpIpMachineIfProtocol.args = (string[])this.args.Clone();

            return tcpIpMachineIfProtocol;
        }

        public override void SetArgument(params string[] args)
        {
            this.args = (string[])args.Clone();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(command);
            foreach (string arg in args)
                sb.AppendFormat(",{0}", arg);
                    
            return sb.ToString();
        }

    }
    
    public class TcpIpMachineIfSettings : MachineIfSetting
    {
        protected TcpIpInfo tcpIpInfo;

        public TcpIpInfo TcpIpInfo
        {
            get { return tcpIpInfo; }
            set { tcpIpInfo = value; }
        }

        public TcpIpMachineIfSettings(MachineIfType machineIfType) : base(machineIfType)
        {
            tcpIpInfo = new TcpIpInfo("", 0);
        }

        public override object Clone()
        {
            TcpIpMachineIfSettings newSettings = new TcpIpMachineIfSettings(this.machineIfType);
            newSettings.tcpIpInfo = new TcpIpInfo(tcpIpInfo);

            return newSettings;
        }

        protected override void LoadXml(XmlElement xmlElement)
        {
            tcpIpInfo.Load(xmlElement, "TcpIpInfo");
        }

        protected override void SaveXml(XmlElement xmlElement)
        {
            tcpIpInfo.Save(xmlElement, "TcpIpInfo");
        }
    }

    public class TcpIpMachineIfPacketParser : SimplePacketParser
    {
        public TcpIpMachineIfPacketParser()
        {
        }

        public virtual string MakePacket(MachineIfProtocol machineIfProtocol)
        {
            TcpIpMachineIfProtocol tcpIpMachineIfProcotol = (TcpIpMachineIfProtocol)machineIfProtocol;
            StringBuilder sb = new StringBuilder();
            sb.Append(tcpIpMachineIfProcotol.Command.ToString());
            foreach (string arg in tcpIpMachineIfProcotol.Args)
                sb.AppendFormat(",{0}", arg);
            
            string packet = sb.ToString();
            return packet;
        }

        public virtual MachineIfProtocol BreakPacket(string packet)
        {
            string[] token = packet.Split(',');

            //Enum e = (Enum)Enum.Parse(SystemManager.Instance().MachineIfProtocolList.ProtocolListType, token[0]);
            Enum e = SystemManager.Instance().MachineIfProtocolList.GetEnum(token[0]);
            TcpIpMachineIfProtocol tcpIpMachineIfProcotol
                = (TcpIpMachineIfProtocol)SystemManager.Instance().MachineIfProtocolList.GetProtocol(e);
            token = token.Skip(1).ToArray();
            tcpIpMachineIfProcotol.Args = token;

            return tcpIpMachineIfProcotol;
        }
    }

    public delegate string MakePacketDelegate(MachineIfProtocol machineIfProtocol);
    public delegate MachineIfProtocol BreakPacketDelegate(string pakcet);
    public abstract class TcpIpMachineIf : MachineIf
    {
        protected MakePacketDelegate MakePacket = null;
        protected BreakPacketDelegate BreakPacket = null;
        
        public TcpIpMachineIf(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
        }

        protected virtual TcpIpMachineIfPacketParser CreatePacketParser()
        {
            return new TcpIpMachineIfPacketParser();
        }

        protected virtual void BuildPacketParser(PacketParser packetParser)
        {
            TcpIpMachineIfPacketParser tcpIpMachineIfPacketParser = (TcpIpMachineIfPacketParser)packetParser;
            tcpIpMachineIfPacketParser.StartChar = Encoding.ASCII.GetBytes("<START>");
            tcpIpMachineIfPacketParser.EndChar = Encoding.ASCII.GetBytes("<END>");

            MakePacket = tcpIpMachineIfPacketParser.MakePacket;
            BreakPacket = tcpIpMachineIfPacketParser.BreakPacket;
        }
    }

    public class TcpIpMachineIfClient : TcpIpMachineIf
    {
        protected SinglePortSocket clientSocket;
        
        public override bool IsConnected
        {
            get { return clientSocket == null ? false : clientSocket.Connected; }
        }

        public TcpIpMachineIfClient(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
        }

        public override void Initialize()
        {
            TcpIpInfo tcpIpInfo = ((TcpIpMachineIfSettings)this.machineIfSetting).TcpIpInfo;

            clientSocket = new SinglePortSocket();

            TcpIpMachineIfPacketParser tcpIpMachineIfPacketParser = CreatePacketParser();
            BuildPacketParser(tcpIpMachineIfPacketParser);
            tcpIpMachineIfPacketParser.OnDataReceived = tcpIpMachineIfPacketParser_OnDataReceived;
            clientSocket.PacketHandler.PacketParser = tcpIpMachineIfPacketParser;
            clientSocket.Init(tcpIpInfo);
        }

        protected void tcpIpMachineIfPacketParser_OnDataReceived(ReceivedPacket receivedPacket)
        {
            string receivedString = Encoding.Default.GetString(receivedPacket.ReceivedData);
            LogHelper.Debug(LoggerType.Function, "TcpIpMachineIfClient::OnDataReceived");
            LogHelper.Debug(LoggerType.Debug, string.Format("receivedPacket - {0}", receivedString));

            MachineIfProtocol machineIfProtocol = BreakPacket(receivedString);
            this.ExecuteCommand(machineIfProtocol.ToString());
        }

        public override void Release()
        {
            Stop();
         
            clientSocket.Close(true);
        }

        public override void Start()
        {
            clientSocket.StartonnectionThread();
        }

        public override void Stop()
        {
            clientSocket.StopConnectionThread();
        }


        protected override bool Send(MachineIfProtocol protocol)
        {
             if (MakePacket == null)
                return false;

            string packetString = MakePacket(protocol);
            if (string.IsNullOrEmpty(packetString))
                return false;

            byte[] packetByte = this.clientSocket.PacketHandler.PacketParser.EncodePacket(packetString);
            return clientSocket.SendCommand(packetByte);
        }

        public override void SendCommand(byte[] bytes)
        {
            clientSocket.SendCommand(bytes);
        }
    }

    public class TcpIpMachineIfServer : TcpIpMachineIf
    {
        protected SimpleServerSocket serverSocket;

        /// <summary>
        /// Server Open State
        /// </summary>
        public override bool IsConnected
        {
            get { return true; }
        }

        public TcpIpMachineIfServer(MachineIfSetting machineIfSetting) : base(machineIfSetting) { }

        ~TcpIpMachineIfServer()
        {
            Release();
        }

        public override void Initialize()
        {
            TcpIpInfo tcpIpInfo = ((TcpIpMachineIfSettings)this.machineIfSetting).TcpIpInfo;

            serverSocket = new SimpleServerSocket();

            TcpIpMachineIfPacketParser tcpIpMachineIfPacketParser = CreatePacketParser();
            BuildPacketParser(tcpIpMachineIfPacketParser);
            tcpIpMachineIfPacketParser.OnDataReceived = tcpIpMachineIfPacketParser_OnDataReceived;
            serverSocket.ListeningPacketHandler.PacketParser = tcpIpMachineIfPacketParser;
            
            serverSocket.Setup(tcpIpInfo);
        }
        
        private void tcpIpMachineIfPacketParser_OnDataReceived(ReceivedPacket receivedPacket)
        {
            string receivedString = Encoding.Default.GetString(receivedPacket.ReceivedData);
            //Debug.WriteLine(string.Format("TcpIpMachineIfServer::OnDataReceived - {0}", receivedString));

            MachineIfProtocol machineIfProtocol = BreakPacket(receivedString);
            //protocolResponce = new MachineIfProtocolResponce();
            //protocolResponce.SetRecivedData(receivedString, false);
            this.ExecuteCommand(machineIfProtocol?.ToString());
        }

        public override void Release()
        {
            Stop();
            serverSocket?.Close();
        }

        public override void Start()
        {
            serverSocket.StartListening();
        }

        public override void Stop()
        {
            serverSocket?.Stop();
        }

        protected override bool Send(MachineIfProtocol protocol)
        {
            string packetString = MakePacket(protocol);
            if (string.IsNullOrEmpty(packetString))
                return false;
            
            byte[] packetByte = this.serverSocket.ListeningPacketHandler.PacketParser.EncodePacket(packetString);
            
            return serverSocket.SendCommand(packetByte);
        }

        public override void SendCommand(byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
