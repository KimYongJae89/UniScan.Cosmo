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
    public delegate void ClientEventDelegate(ClientHandlerSocket clientHandlerSocket);

    public class SimpleServerSocket
    {
        private string host = "localhost";
        private int dataPort;

        private bool stopThreadFlag = false;
        
        private Thread listeningThread;

        public ManualResetEvent allDone = new ManualResetEvent(false);

        private PacketHandler listeningPacketHandler = new PacketHandler();
        public PacketHandler ListeningPacketHandler
        {
            get { return listeningPacketHandler; }
            set { listeningPacketHandler = value; }
        }

        public ClientEventDelegate ClientConnected;
        public ClientEventDelegate ClientDisconnected;

        Mutex commandMutex = new Mutex();

        Socket listeningSocket = null;
        SocketAsyncEventArgs acceptEventArgs = null;

        List<ClientHandlerSocket> clientList = new List<ClientHandlerSocket>();

        public SimpleServerSocket()
        {
        
        }

        public void Setup(TcpIpInfo tcpIpInfo)
        {
            Setup(tcpIpInfo.IpAddress, tcpIpInfo.PortNo);
        }

        public void Setup(string host, int dataPort)
        {
            if (String.IsNullOrEmpty(host))
            {
                LogHelper.Debug(LoggerType.Network, "Host address is empty");
                return;
            }

            this.host = host;
            this.dataPort = dataPort;
        }

        public void Close()
        {
            if (listeningSocket != null)
            {
                if (listeningSocket.Connected)
                    listeningSocket.Shutdown(SocketShutdown.Both);
                listeningSocket.Close();
            }
            listeningSocket = null;
        }

        public void StartListening()
        {
            stopThreadFlag = false;

            try
            {
                IPAddress ipAddr = IPAddress.Parse(host);
                
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, dataPort);
                LogHelper.Debug(LoggerType.Network, String.Format("Server IP : {0}", ipEndPoint.Address));

                listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listeningSocket.Bind(ipEndPoint);

                listeningSocket.Listen(50);

                listeningThread = new Thread(new ThreadStart(ListeningProc));
                listeningThread.IsBackground = true;
                listeningThread.Start();
            }
            catch (SocketException e)
            {
                LogHelper.Debug(LoggerType.Network, String.Format("Server Error : {0}", e.Message));
            }
        }

        public void StopListening()
        {
            stopThreadFlag = true;

            if (listeningSocket != null)
            {
                while (listeningThread != null && listeningThread.IsAlive)
                    Thread.Sleep(10);
            }
        }

        public void SendCommand(PacketParser command)
        {
            foreach (ClientHandlerSocket handlerSocket in clientList)
                handlerSocket.SendCommand(command);
        }

        public bool SendCommand(byte[] commandPacket)
        {
            lock (clientList)
            {
                // 1.
                //System.Threading.Tasks.Parallel.ForEach(clientList, f =>
                //  {
                //      f.SendCommand(commandPacket);
                //  });

                // 2.
                //clientList.ForEach(f => f.SendCommand(commandPacket));

                // 3.
                foreach (ClientHandlerSocket handlerSocket in clientList)
                    handlerSocket.SendCommand(commandPacket);
            }
            return true;
        }

        public void RemoveClientSocket(ClientHandlerSocket clientHandlerSocket)
        {
            lock (clientList)
                clientList.Remove(clientHandlerSocket);

            if (ClientDisconnected != null)
                ClientDisconnected(clientHandlerSocket);
        }

        public void Stop()
        {
            StopListening();
            //while(clientList.Count>0)
            //    clientList[0].Stop();
        }

        // 가상 모드의 동작을 위해 별도 함수로 분리
        public bool ProcessDataPacket(byte[] receiveBuf)
        {
            return true; //listeningPacketHandler.ProcessPacket(receiveBuf);
        }

        private void ListeningProc()
        {
            try
            {
                while (stopThreadFlag == false)
                {
                    // Accept 실행 중 스레드 묶임 -> stopFlag를 처리 못해 스레드 종료 안됨 -> 프로그램 종료 안 됨
                    //Socket handlerSocket = listeningSocket.Accept();

                    //ClientHandlerSocket clientHandlerSocket = new ClientHandlerSocket(this);
                    //clientHandlerSocket.PacketHandler.StartChar = (byte[])listeningPacketHandler.StartChar.Clone(); // Encoding.ASCII.GetBytes("<START>");
                    //clientHandlerSocket.PacketHandler.EndChar = (byte[])listeningPacketHandler.EndChar.Clone();  // Encoding.ASCII.GetBytes("<END>");
                    //clientHandlerSocket.PacketHandler.ListenPacketParser = listeningPacketHandler.ListenPacketParser.Clone();

                    //clientHandlerSocket.Start(handlerSocket);

                    //if (ClientConnected != null)
                    //    ClientConnected(clientHandlerSocket);

                    //clientList.Add(clientHandlerSocket);

                    //if (acceptEventArgs == null)
                    //{
                    //    acceptEventArgs = new SocketAsyncEventArgs();
                    //    acceptEventArgs.Completed += AcceptClient;
                    //    listeningSocket.AcceptAsync(acceptEventArgs);
                    //}

                    //Thread.Sleep(10);


                    //2
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    listeningSocket.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listeningSocket);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }
            }
            catch (SocketException e)
            {
                LogHelper.Error(LoggerType.Error, "Socket Exception : " + e.Message);

                return;
            }
            finally
            {
                // Stop 메서드로 이동
                //if(listeningSocket.Connected)
                //    listeningSocket.Shutdown(SocketShutdown.Both);
                //listeningSocket.Close();
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            Accept(handler);
        }

        private void Accept(Socket socket)
        {
            ClientHandlerSocket clientHandlerSocket = new ClientHandlerSocket(this);
            clientHandlerSocket.PacketHandler.PacketParser = listeningPacketHandler.PacketParser.Clone();

            clientHandlerSocket.Start(socket);

            acceptEventArgs?.Dispose();
            acceptEventArgs = null;

            lock (clientList)
                clientList.Add(clientHandlerSocket);

            if (ClientConnected != null)
                ClientConnected(clientHandlerSocket);
        }

        private void AcceptClient(object sender, SocketAsyncEventArgs e)
        {
            // AcceptClient 함수와 serverSocket_ClientConnected 함수 중 어느게 먼저 호출될까???

            Socket handlerSocket = e.AcceptSocket;
            if (handlerSocket.Connected == false)
                return;

            Accept(handlerSocket);
        }
    }
}
