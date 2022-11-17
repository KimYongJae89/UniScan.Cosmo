﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace DynMvp.Base
{
    public class TimeOutHandler
    {
        public static void Wait(int timeoutTime, ManualResetEvent eventHandler)
        {
            if (eventHandler.WaitOne(timeoutTime) == false)
            {
                throw new TimeoutException();
            }
        }
    }

    public class TimeOutTimer
    {
        System.Timers.Timer timer = null;
        object container = null;

        int targetTimeCnt;
        int elapseTimeCnt;

        public EventHandler OnTimeout = null;

        public bool TimeOut { get => targetTimeCnt < elapseTimeCnt; }
        public int ReaminTimeMs { get => Math.Max(0, targetTimeCnt - elapseTimeCnt) * 100; }

        public TimeOutTimer(object container = null)
        {
            this.container = container;
            this.timer = new System.Timers.Timer();
            this.timer.Elapsed += timer_Elapsed;
        }

        public void Start(int timeOutMs)
        {
            this.elapseTimeCnt = 0;
            this.targetTimeCnt = timeOutMs / 100;

            timer.Interval = 100;
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Reset()
        {
            this.elapseTimeCnt = 0;
            this.targetTimeCnt = 0;

            timer.Stop();
        }

        public void Restart(int timeOutMs)
        {
            Reset();
            Start(timeOutMs);
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            elapseTimeCnt++;
            if (TimeOut)
            {
                timer.Stop();
                OnTimeout?.Invoke(this.container, new EventArgs());
            }
        }
    }
}
