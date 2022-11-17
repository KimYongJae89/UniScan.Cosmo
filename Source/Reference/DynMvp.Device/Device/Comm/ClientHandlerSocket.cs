using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices.Comm
{
    public delegate void SocketClosedDelegate();

    public class ClientHandlerSocket
    {
        SimpleServerSocket serverSocket;
        public SimpleServerSocket ServerSocket
        {
            get { return serverSocket; }
        }

        Socket handlerSocket;
        private Task listeningTask;
        bool stopThreadFlag = false;

        private PacketData packetData = new PacketData();
        public PacketData PacketData
        {
            get { return packetData; }
        }

        private PacketHandler packetHandler = new PacketHandler();
        public PacketHandler PacketHandler
        {
            get { return packetHandler; }
        }

        public SocketClosedDelegate SocketClosed;

        public ClientHandlerSocket(SimpleServerSocket serverSocket)
        {
            this.serverSocket = serverSocket;
        }

        public void Start(Socket handlerSocket)
        {
            this.handlerSocket = handlerSocket;

            stopThreadFlag = false;
            listeningTask = new Task(new Action(ListeningProc));
            listeningTask.Start();
        }

        public void Stop()
        {
            stopThreadFlag = true;
        }

        public string GetIpAddress()
        {
            return handlerSocket.RemoteEndPoint.ToString().Split(':')[0];
        }

        public void SendCommand(PacketParser command)
        {
            try
            {
                if (handlerSocket != null)
                {
                    PacketHandler.PacketParser = command;
                    //LogHelper.Debug(LoggerType.Network, "Send Command : " + System.Text.Encoding.Default.GetString(command.GetRequestPacket()));

                    if (handlerSocket.Connected == true)
                    {
                        handlerSocket.Send(command.EncodePacket("Request"));
                    }
                }
            }
            catch (SocketException e)
            {
                LogHelper.Error(LoggerType.Error, "Socket Exception : " + e.Message);
            }
        }

        public bool SendCommand(byte[] commandPacket)
        {
            try
            {
                if (handlerSocket != null)
                {
                    //LogHelper.Debug(LoggerType.Network, "Send Command : " + System.Text.Encoding.Default.GetString(commandPacket));

                    if (handlerSocket.Connected == true)
                    {
                        int sendByte = handlerSocket.Send(commandPacket);
                        return sendByte == commandPacket.Length;
                    }
                }
                return false;
            }
            catch (SocketException e)
            {
                LogHelper.Error(LoggerType.Error, "Socket Exception : " + e.Message);
                return false;
            }
}

        private void ListeningProc()
        {
            const int maxBufferSize = 10240;

            try
            {
                while (stopThreadFlag == false)
                {
                    handlerSocket.Send(new byte[1], 1, SocketFlags.None);
                    if (handlerSocket.Connected == false)
                        break;

                    bool packetCompleted = false;
                    do
                    {
                        if (stopThreadFlag == true)
                            break;

                        int dataAvailable = handlerSocket.Available;
                        if (dataAvailable > 0)
                        {
                            int dataToRead = Math.Min(dataAvailable, maxBufferSize);
                            byte[] receiveBuf = new byte[dataToRead];

                            handlerSocket.Receive(receiveBuf);
                            
                            packetCompleted = packetHandler.ProcessPacket(receiveBuf, packetData);
                        }
                    } while (packetCompleted);

                    Thread.Sleep(1);
                }

            }
            catch (SocketException e)
            {
                LogHelper.Error(LoggerType.Error, "Socket Exception : " + e.Message);
            }

            serverSocket.RemoveClientSocket(this);

            handlerSocket.Shutdown(SocketShutdown.Both);
            handlerSocket.Close();


            if (SocketClosed != null)
                SocketClosed();
        }
    }
}
