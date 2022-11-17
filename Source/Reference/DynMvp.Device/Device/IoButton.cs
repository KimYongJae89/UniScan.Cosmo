using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device
{
    public delegate bool IoEvent(IoEventHandler eventSource);

    public enum IoEventHandlerDirection { In, Out }
    public class IoEventHandler
    {
        protected string name;
        public string Name
        {
            get { return name; }
        }

        protected DigitalIoHandler digitalIoHandler;
        protected IoPort ioPort;
        protected bool curState = false;

        public IoEvent OnActivate;
        public IoEvent OnDeactivate;
        public IoEvent OnChanged;
        IoEventHandlerDirection isInputEvent = IoEventHandlerDirection.In;

        public bool IsActivate { get => curState; }

        public IoEventHandler(string name, DigitalIoHandler digitalIoHandler, IoPort ioPort, IoEventHandlerDirection inOut = IoEventHandlerDirection.In)
        {
            this.name = name;
            this.digitalIoHandler = digitalIoHandler;
            this.ioPort = ioPort;
            isInputEvent = inOut;
        }

        public bool CheckState()
        {
            return CheckState(GetValue());
        }

        private DioValue GetValue()
        {
            DioValue inputValue;
            if (isInputEvent == IoEventHandlerDirection.In)
                inputValue = digitalIoHandler.ReadInput();
            else
                inputValue = digitalIoHandler.ReadOutput();

            return inputValue;
        }

        public bool GetCurrentValue()
        {
            DioValue inputValue;
            inputValue = digitalIoHandler.ReadInput();
            bool buttonState = IoMonitor.CheckInput(inputValue, ioPort);
            return buttonState;
        }

        public bool CheckState(DioValue inputValue)
        {
            bool processOk1 = true;
            bool processOk2 = true;
            bool newState = IoMonitor.CheckInput(inputValue, ioPort);

            if (newState != curState)
            {
                LogHelper.Debug(LoggerType.IO, String.Format("{0} is Changed", name));

                curState = newState;

                if (OnChanged != null)
                    processOk1 = OnChanged(this);

                if (newState)
                {
                    LogHelper.Debug(LoggerType.IO, String.Format("{0} is Turned On", name));
                    if (OnActivate != null)
                        processOk2 = OnActivate(this);
                }
                else
                {
                    if (OnDeactivate != null)
                    {
                        LogHelper.Debug(LoggerType.IO, String.Format("{0} is Turned Off", name));
                        processOk2 = OnDeactivate(this);
                    }
                }
            }

            bool processOk = processOk1 && processOk2;
            //if (processOk)
            //    curState = newState;

            return processOk;
        }

        public void Update()
        {
            DioValue value = GetValue();
            if (value.Count > 0)
            {
                this.curState = IoMonitor.CheckInput(value, ioPort);
                //bool activate = IoMonitor.CheckInput(value, ioPort);
                OnChanged?.Invoke(this);
                if (this.curState)
                    OnActivate?.Invoke(this);
                else
                    OnDeactivate?.Invoke(this);
            }
        }
    }

    //public delegate void IoButtonHandler(IoButton eventSource);

    public class IoButtonEventHandler : IoEventHandler
    {
        IoPort lampOutPort;
        //public IoButtonHandler ButtonPushed;
        //public IoButtonHandler ButtonPulled;

        public IoButtonEventHandler(string name, DigitalIoHandler digitalIoHandler, IoPort buttonInPort, IoPort lampOutPort)
            :base(name, digitalIoHandler, buttonInPort)
        {
            this.lampOutPort = lampOutPort;
        }

        public void TurnOn()
        {
            if (lampOutPort != null)
                digitalIoHandler.WriteOutput(lampOutPort, true);
        }

        public void TurnOff()
        {
            if (lampOutPort != null)
                digitalIoHandler.WriteOutput(lampOutPort, false);
        }

        public void ResetState()
        {
            curState = false;
        }

        public new bool CheckState(DioValue inputValue)
        {
            bool buttonState = IoMonitor.CheckInput(inputValue, ioPort);
            if (buttonState)
            {
                if (curState == false)
                {
                    curState = true;
                    LogHelper.Debug(LoggerType.IO, String.Format("{0} Button Pushed", name));
                    if (OnActivate != null)
                        OnActivate(this);
                }
            }
            else
            {
                if (curState == true)
                {
                    if (OnDeactivate != null)
                    {
                        LogHelper.Debug(LoggerType.IO, String.Format("{0} Button Pulled", name));
                        OnDeactivate(this);
                    }
                }

                curState = false;
            }

            return curState;
        }
    }
}
