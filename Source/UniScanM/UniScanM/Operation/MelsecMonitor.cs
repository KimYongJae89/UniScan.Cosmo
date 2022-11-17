using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.MachineInterface;
using UniScanM.Data;
using UniScanM.MachineIF;
using UniScanM.Operation;
using UniScanM.Settings;

namespace UniScanM.Operation
{
    public enum PLCIndexPos
    {
        StillImage_OnStart = 0, ColorSenseor_OnStart = 2, EDMS_OnStart = 4, Pinhole_OnStart = 6, RVMS_OnStart = 8,
        SP_Speed = 10, PV_Speed = 12, PV_Postion = 16,
        LotNo = 20, Model = 40, Worker = 60, Paste = 80,
        RollDia = 100, RewinderCut = 104, RVMSPreOven = 300, RVMSAfterOven = 304,
        MAX_BYTES = 306
    }

    public delegate void MelsecInfoMationChanged(MachineState state);

    [SecurityPermission(SecurityAction.Demand)]
    public class MelsecMonitor : IDisposable
    {
        IpcClientChannel ipcClientChannel = null;

        Random virtualRandom = new Random();
        DateTime lastReadTime = DateTime.MinValue;

        MachineState state = null;
        public MachineState State { get => state; }

        //public MelsecInfoMationChanged MelsecInfoMationChanged;

        public MelsecMonitor()
        {
            this.ipcClientChannel = new IpcClientChannel();
            ChannelServices.RegisterChannel(ipcClientChannel, false);
            RemotingConfiguration.RegisterWellKnownClientType(typeof(MachineState), "ipc://remote/GAE");

            this.state = new MachineState();
        }

        public void ReadMachineState()
        {
            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            if (machineIf == null)
                return;

            if (state.IsConnected && machineIf.IsConnected == false)
                Displaynotify();    // 연결 끊어짐

            DateTime curTime = DateTime.Now;
            state.IsConnected = machineIf.IsConnected;

            if (state.IsConnected == false)
            {
                state.Reset();
            }
            else if (machineIf is IVirtualMachineIf)
            {
                double timeSpanMin = (curTime - this.lastReadTime).TotalMinutes;
                double distance = state.SpSpeed * timeSpanMin;
                state.PvPosition += distance;

                double rand1 = virtualRandom.NextDouble() * 0.1 - 0.05;
                double rand2 = virtualRandom.NextDouble() * 0.1 - 0.05;

                state.Rvms_BeforePattern = 48000 + (int)Math.Round(rand1 * 100);
                state.Rvms_AfterPattern = 48000 + (int)Math.Round(rand2 * 100);
            }
            else if (SystemManager.Instance().DeviceBox.MachineIf.IsIdle)
            {
                MachineIfProtocolResponce protocolResponce = SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfCommonCommand.GET_MACHINE_STATE);
                if (protocolResponce != null && protocolResponce.IsGood)
                    ProcessPacket(protocolResponce.ReciveData);
            }
            this.lastReadTime = curTime;
        }

        public void ProcessPacket(string receivedData, ReceivedPacket packet = null)
        {
            try
            {
                byte[] data = StringHelper.HexStringToByteArray(receivedData);

                state.StillImageOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.StillImage_OnStart, data));
                state.ColorSensorOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.ColorSenseor_OnStart, data));
                state.EdmsOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.EDMS_OnStart, data));
                state.PinholeOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.Pinhole_OnStart, data));
                state.RvmsOnStart = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.RVMS_OnStart, data));

                state.SpSpeed = MelsecDataConverter.GetShort((int)PLCIndexPos.SP_Speed, data)/10.0;
                state.PvSpeed = MelsecDataConverter.GetShort((int)PLCIndexPos.PV_Speed, data) / 10.0;
                state.PvPosition = MelsecDataConverter.GetInt((int)PLCIndexPos.PV_Postion, data);
                state.RollDia = MelsecDataConverter.GetInt((int)PLCIndexPos.RollDia, data) / 100.0;

                string modelName = MelsecDataConverter.GetString_LittleEndian((int)PLCIndexPos.Model, 20, data).Trim();
                state.ModelName = modelName;

                string lotNo = MelsecDataConverter.GetString_LittleEndian((int)PLCIndexPos.LotNo, 20, data).Trim();
                state.LotNo = lotNo;

                string worker = MelsecDataConverter.GetString_LittleEndian((int)PLCIndexPos.Worker, 20, data).Trim();
                state.Worker = worker;

                state.RewinderCut = Convert.ToBoolean(MelsecDataConverter.GetShort((int)PLCIndexPos.RewinderCut, data));
                state.Rvms_BeforePattern = MelsecDataConverter.GetInt((int)PLCIndexPos.RVMSPreOven, data);
                state.Rvms_AfterPattern = MelsecDataConverter.GetInt((int)PLCIndexPos.RVMSAfterOven, data);
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Error, "ProcessPacket - Responce buffer length is invalid.");
            }
        }

        public void Dispose()
        {
            
        }

        protected void Displaynotify()
        {
            try
            {
                //NotifyIcon notifyIcon = new NotifyIcon();
                //notifyIcon.Text = "Export Datatable Utlity";
                //notifyIcon.Visible = true;
                //notifyIcon.BalloonTipTitle = "Error Our System PLC disconnected.";
                //notifyIcon.BalloonTipText = "Click Here to see details";
                //notifyIcon.ShowBalloonTip(10000);
                //notifyIcon.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
    }
}

public class MachineState
{

    // UseStillImage = 0, UseColorSenseor = 2, UseEDMS = 4, UsePinhole = 6, UseRVMS = 8
    // SP_Speed = 10, PV_Speed = 12, PV_Postion = 14,
    // LotNo = 18, Model = 38, Worker = 58, Paste = 78,
    // RollDia = 98, RewinderCut = 102
    bool isConnected = false;
    public bool IsConnected { get => isConnected; set => isConnected = value; }

    bool stillImageOnStart = false;
    public bool StillImageOnStart { get => stillImageOnStart; set => stillImageOnStart = value; }

    bool colorSensorOnStart = false;
    public bool ColorSensorOnStart { get => colorSensorOnStart; set => colorSensorOnStart = value; }

    bool edmsOnStart = false;
    public bool EdmsOnStart { get => edmsOnStart; set => edmsOnStart = value; }

    bool pinholeOnStart = false;
    public bool PinholeOnStart { get => pinholeOnStart; set => pinholeOnStart = value; }

    bool rvmsOnStart = false;
    public bool RvmsOnStart { get => rvmsOnStart; set => rvmsOnStart = value; }

    //  Target SetPoint speed m/min
    double spSpeed = 0; 
    public double SpSpeed { get => spSpeed; set => spSpeed = value; }

    //speed m/min
    double pvSpeed = 0;
    public double PvSpeed { get => pvSpeed; set => pvSpeed = value; }

    //m
    double pvPosition = 0;
    public double PvPosition { get => pvPosition; set => pvPosition = value; }

    string lotNo = "";
    public string LotNo
    {
        get
        {
            return lotNo;
            //if (string.IsNullOrEmpty(lotNo))
            //    lotNo = string.Format("Unkown/{0}", DateTime.Now.ToString("yyMMddHHmm"));

            //else if ( lotNo.IndexOf("Unkown/") >= 0)
            //    lotNo = string.Format("Unkown/{0}", DateTime.Now.ToString("yyMMddHHmm"));

            //return lotNo;
        }
        set { lotNo = value; }
    }

    string modelName = "";
    public string ModelName { get => modelName; set => modelName = value; }

    string worker = "";
    public string Worker { get => worker; set => worker = value; }

    string paste = "";
    public string Paste { get => paste; set => paste = value; }

    double rollDia =0;
    public double RollDia { get => rollDia; set => rollDia = value; }

    bool rewinderCut = false;
    public bool RewinderCut { get => rewinderCut; set => rewinderCut = value; }
    

    int rvms_BeforePattern = 0;
    public int Rvms_BeforePattern { get => rvms_BeforePattern; set => rvms_BeforePattern = value; }
    
    int rvms_AfterPattern = 0;
    public int Rvms_AfterPattern { get => rvms_AfterPattern; set => rvms_AfterPattern = value; }


    public MachineState()
    {

    }

    public void Set()
    {
        stillImageOnStart = true;
        colorSensorOnStart = true;
        edmsOnStart = true;
        pinholeOnStart = true;
        rvmsOnStart = true;
        rewinderCut = true;
    }

    public void Reset()
    {
        stillImageOnStart = false;
        colorSensorOnStart = false;
        edmsOnStart = false;
        pinholeOnStart = false;
        rvmsOnStart = false;
        rewinderCut = false;
    }

    public void Toggle()
    {
        stillImageOnStart = !stillImageOnStart;
        colorSensorOnStart = !colorSensorOnStart;
        edmsOnStart = !edmsOnStart;
        pinholeOnStart = !pinholeOnStart;
        rvmsOnStart = !rvmsOnStart;
        rewinderCut = !rewinderCut;
    }

    public virtual void Load()
    {
        // 현재는 필요 없어 보이나 마지막 러닝 상태 굳이 확인 한다면 넣어주세요
    }
}

