using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Device.Device;

namespace DynMvp.Devices.Dio
{
    public delegate bool IoMonitorEventHandler0();
    public delegate bool IoMonitorEventHandler1(DioValue value);
    public delegate bool IoMonitorEventHandler2(DioValue oldValue, DioValue newValue);

    public class IoMonitor : ThreadHandler
    {
        List<IoEventHandler> ioEventHandlerList;
        DioValue nowInputValue = new DioValue();
        DioValue preInputValue = new DioValue();
        DioValue nowOutputValue = new DioValue();

        bool preAlarmed = false;

        private DigitalIoHandler digitalIoHandler = null;

        public IoMonitorEventHandler1 ProcessInitial;
        public IoMonitorEventHandler1 ProcessIdle;
        public IoMonitorEventHandler2 ProcessInputChanged;
        public IoMonitorEventHandler2 ProcessOutputChanged;

        public IoMonitor(string name, DigitalIoHandler digitalIoHandler, List<IoEventHandler> ioEventHandlerList = null) : base(name)
        {
            this.digitalIoHandler = digitalIoHandler;
            this.ioEventHandlerList = ioEventHandlerList;
            this.ioEventHandlerList.ForEach(f => f.Update());
        }

        public IoMonitor(DigitalIoHandler digitalIoHandler, List<IoEventHandler> ioEventHandlerList = null) : base("IoMonitor")
        {
            this.digitalIoHandler = digitalIoHandler;
            this.ioEventHandlerList = ioEventHandlerList;
        }

        public override void Start()
        {
            this.requestStop = false;
            if (digitalIoHandler.Count > 0)
            {
                WorkingThread = new Thread(new ThreadStart(IOMonitorThreadFunc));
                WorkingThread.IsBackground = true;
                base.Start();
            }
        }

        public bool CheckOutput(IoPort ioPort)
        {
            return CheckInput(nowOutputValue, ioPort);
        }

        public bool CheckInput(IoPort ioPort)
        {
            return CheckInput(nowInputValue, ioPort);
        }

        public static bool CheckInput(DioValue inputValue, IoPort ioPort)
        {
            if (ioPort == null)
                return false;

            if (ioPort.PortNo == IoPort.UNUSED_PORT_NO)
                return false;

            uint channelValue = inputValue.GetValue(ioPort.DeviceNo, ioPort.GroupNo);

            return ((channelValue >> ioPort.PortNo) & 1) == (ioPort.ActiveLow ? 0 : 1);
        }

        private void IOMonitorThreadFunc()
        {
            bool initial = true;

            while (RequestStop == false)
            {
                nowOutputValue = digitalIoHandler.ReadOutput();
                nowInputValue = digitalIoHandler.ReadInput();

                if (initial == true)
                {
                    if (ProcessInitial != null)
                        ProcessInitial(nowInputValue);
                    initial = false;
                }
                else
                {
                    if (ProcessIdle != null)
                        ProcessIdle(nowInputValue);
                }

                if (ErrorManager.Instance().IsAlarmed() != preAlarmed)
                // Alarm Occure or Clear
                {
                    preAlarmed = ErrorManager.Instance().IsAlarmed();
                    Thread.Sleep(50);
                    continue;
                }

                if (nowInputValue.Equals(preInputValue) == false)
                {
                    if (ProcessInputChanged != null)
                    {
                        ProcessInputChanged(preInputValue, nowInputValue);
                    }
                    preInputValue.Copy(nowInputValue);
                }
                Thread.Sleep(100);

                if (ioEventHandlerList != null)
                {
                    foreach (IoEventHandler ioEventHandler in ioEventHandlerList)
                    {
                        ioEventHandler.CheckState();
                    }
                }
            }

            LogHelper.Debug(LoggerType.Shutdown, "Thread Stopped : IOMonitorThreadFunc");
        }
    }
}
