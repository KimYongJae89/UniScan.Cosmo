using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

using DynMvp.Base;

namespace DynMvp.Devices.Comm
{
    public class SinglePortSocket
    {
        private TcpIpInfo tcpIpInfo;

        private bool stopConnectionThreadFlag = false;
        private Thread connectionThread;

        private bool stopListeningThreadFlag = false;
        private Thread listeningThread;

        PacketData packetData = new PacketData();
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

        IPEndPoint ipEndPoint = null;

        Mutex commandMutex = new Mutex();

        Socket clientSocket = null;
        ManualResetEvent connectAsyncCompleted = new ManualResetEvent(true);
        public bool Connected
        {
            get { return clientSocket == null ? false : clientSocket.Connected; }
        }

        public SinglePortSocket()
        {

        }

        ~SinglePortSocket()
        {
            Close(false);
        }

        public void Init(TcpIpInfo tcpIpInfo)
        {
            stopConnectionThreadFlag = false;

            this.tcpIpInfo = tcpIpInfo;

            if (tcpIpInfo.IpAddress == null)
                return;

            //1.
            //IPHostEntry ipHost = Dns.GetHostEntry(tcpIpInfo.IpAddress);
            //IPAddress ipAddr = ipHost.AddressList[0];

            //2.
            //IPHostEntry ipHost = Dns.Resolve(tcpIpInfo.IpAddress);
            //IPAddress ipAddr = ipHost.AddressList[0];

            // 3.
            IPAddress ipAddr;
            bool ok = IPAddress.TryParse(tcpIpInfo.IpAddress, out ipAddr);
            if (ok == false)
            {
                DynMvp.UI.Touch.MessageForm.Show(null, "Host Ip is invalid");
                return;
            }

            ipEndPoint = new IPEndPoint(ipAddr, tcpIpInfo.PortNo);

            //clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartonnectionThread()
        {
            LogHelper.Debug(LoggerType.Function, "SinglePortSocket::StartonnectionThread");
            if (connectionThread != null)
                return;

            stopConnectionThreadFlag = false;
            connectionThread = new Thread(new ThreadStart(ConnectionProc));
            connectionThread.IsBackground = true;
            connectionThread.Start();
        }

        private void ConnectionProc()
        {
            LogHelper.Debug(LoggerType.Function, "SinglePortSocket::ConnectionProc - Start");
            while (!stopConnectionThreadFlag)
            {
                if (clientSocket == null || clientSocket.Connected == false)
                    ConnectAsync();
                
                Thread.Sleep(10000);
            }

            connectionThread = null;
            LogHelper.Debug(LoggerType.Function, "SinglePortSocket::ConnectionProc - End");
        }

        public void ConnectAsync()
        {
            if (connectAsyncCompleted.WaitOne(100))
            {
                if (clientSocket == null)
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.RemoteEndPoint = ipEndPoint;
                e.Completed += ConnectAsync_Completed;

                connectAsyncCompleted.Reset();
                clientSocket.ConnectAsync(e);
            }
        }

        private void ConnectAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            //LogHelper.Debug(LoggerType.Function, "SinglePortSocket::ConnectAsync_Completed");
            LogHelper.Debug(LoggerType.Network, string.Format("SocketError - {0}", e.SocketError.ToString()));
            
            if (e.SocketError == SocketError.Success)
                StartListening();

            connectAsyncCompleted.Set();
        }

        public void Connect()
        {
            if (tcpIpInfo.IpAddress == "" || tcpIpInfo.IpAddress == null)
                return;

            LogHelper.Debug(LoggerType.Network, "Connect Socket");

            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                clientSocket.Connect(ipEndPoint);
                StartListening();
                
                LogHelper.Debug(LoggerType.Network, "Socket Connected");
            }
            catch (SocketException ex)
            {
                LogHelper.Error(LoggerType.Error, "Socket Error : " + ex.Message);
                //throw ex;
            }
        }

        public void StopConnectionThread()
        {
            LogHelper.Debug(LoggerType.Function, "SinglePortSocket::StopConnectionThread");

            if (connectionThread == null)
                return;

            stopConnectionThreadFlag = true;

            //while (connectionThread.IsAlive)
            //    Thread.Sleep(100);

            //connectionThread = null;

        }

        public void Close(bool bFinalize)
        {
            LogHelper.Debug(LoggerType.Function, "SinglePortSocket::Close");

            if (bFinalize == true)
                StopConnectionThread();

            StopListening();

            if (clientSocket != null)
            {
                try
                {
                    clientSocket.Shutdown(SocketShutdown.Both);
                    
                }
                catch (SocketException)
                {
                    LogHelper.Error(LoggerType.Error, "Client socket shutdown fail.");
                }
                clientSocket.Close();
                clientSocket = null;
            }
        }

        public void ClearBuffer()
        {
            if (clientSocket != null)
            {
                int dataAvailable = clientSocket.Available;

                if (dataAvailable > 0)
                {
                    byte[] receiveBuf = new byte[dataAvailable];
                    clientSocket.Receive(receiveBuf);
                }
            }
        }

        private void StartListening()
        {
            LogHelper.Debug(LoggerType.Function, "SinglePortSocket::StartListening");

            stopListeningThreadFlag = false;

            //if (clientSocket != null)
            //{
                listeningThread = new Thread(new ThreadStart(ListeningProc));
                listeningThread.IsBackground = true;
                listeningThread.Start();
            //}
            //else
            //{
            //    LogHelper.Warn(LoggerType.Network, "Socket is null");
            //}
        }

        private void StopListening()
        {
            LogHelper.Debug(LoggerType.Function, "SinglePortSocket::StopListening");

            stopListeningThreadFlag = true;

            if (clientSocket != null)
            {
                if (listeningThread != null)
                    while (listeningThread.IsAlive)
                        Thread.Sleep(100);
            }
        }

        public bool IsMonitoring()
        {
            return (listeningThread != null && listeningThread.IsAlive);
        }

        public void SendCommand(PacketParser command)
        {
            packetHandler.PacketParser = command;
            try
            {
                clientSocket.Send(command.EncodePacket("Request"));
            }
            catch (SocketException)
            {
                Close(false);
            }
        }

        public void SendCommand(string commandString)
        {
            SendCommand(Encoding.ASCII.GetBytes(commandString));
        }

        public bool SendCommand(byte[] commandPacket)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false)
                    return false;

                bool ok = commandPacket.Length == clientSocket.Send(commandPacket);
                return ok;
            }
            catch (SocketException)
            {
                //Close(false);
                return false;
            }
        }

        public bool ProcessDataPacket(byte[] receiveBuf)
        {
            return packetHandler.ProcessPacket(receiveBuf, packetData);
        }

        private void ListeningProc()
        {
            const int maxBufferSize = 10240;
            bool packetCompleted = false;

            try
            {
                while (stopListeningThreadFlag == false)
                {
                    do
                    {
                        clientSocket.Send(new byte[0], 0, SocketFlags.None);   // 연결 확인 차원에서 0바이트 보냄.
                        if (clientSocket.Connected == false)
                            break;

                        if (stopListeningThreadFlag == true)
                            break;

                        if (clientSocket == null) //임시 코드 나중에 break로 처리해야하는 것이 맞음.
                            break;

                        int dataAvailable = clientSocket.Available;
                        if (dataAvailable == 0)
                            break;

                        //LogHelper.Debug(LoggerType.Inspection, String.Format("Data Available : {0}", dataAvailable));

                        int dataToRead = Math.Min(dataAvailable, maxBufferSize);
                        byte[] receiveBuf = new byte[dataToRead];

                        clientSocket.Receive(receiveBuf);

                        //LogHelper.Debug(LoggerType.Inspection, "Data Received");

                        packetCompleted = ProcessDataPacket(receiveBuf);

                        //LogHelper.Debug(LoggerType.Inspection, "Exit Process Data Packet");

                    } while (packetCompleted);

                    Thread.Sleep(10);
                }
            }
            catch(SocketException ex)
            {
                LogHelper.Error(LoggerType.Error, "Socket Error : " + ex.Message);
            }
            finally
            {
                if (clientSocket != null)
                {
                    try
                    {
                        clientSocket.Shutdown(SocketShutdown.Both); // clientSocket이 null이 아니더라도 문제가 발생될수 있음
                        //clientSocket.Disconnect(true);
                        clientSocket.Close();
                    }
                    catch (SocketException)
                    {
                    }

                    clientSocket = null;
                    stopListeningThreadFlag = true;
                }
            }
        }
    }
}
