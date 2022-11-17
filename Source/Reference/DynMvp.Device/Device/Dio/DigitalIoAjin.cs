﻿using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.Dio
{
    public class DigitalIoAjin : DigitalIo
    {
        bool exist = false;
        bool initialized = false;

        public DigitalIoAjin(string name)
            : base(DigitalIoType.Ajin, name)
        {
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            bool ok = base.Initialize(digitalIoInfo);
            if (!ok)
                return false;

            uint upStatus = 0;
            uint uResult;

            int result = CAXL.AxlIsOpened();
            if (result == 0)
                uResult = CAXL.AxlOpen(7);

            uResult = CAXD.AxdInfoIsDIOModule(ref upStatus);
            if (uResult == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                exist = (upStatus == (uint)AXT_EXISTENCE.STATUS_EXIST);
            }
            else
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToInitialize,
                    ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToInitialize.ToString(), "Fail to find digital I/O.", ((AXT_FUNC_RESULT)uResult).ToString());
                return false;
            }
            initialized = true;

            return true;

        }

        public override bool IsReady()
        {
            return initialized;
        }

        public override void Release()
        {
            base.Release();
            if (CAXL.AxlClose() == 0)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToRelease,
                    ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToRelease.ToString(), "Fail to release Ajin library.");
                return;
            }
            UpdateState(DeviceState.Idle, "Device unloaded");

        }

        public override uint ReadInputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            throw new NotImplementedException();
        }

        public override uint ReadOutputGroup(int groupNo, int portNo)
        {
            throw new NotImplementedException();
        }

        public override uint ReadInputGroup(int groupNo, int portNo)
        {
            throw new NotImplementedException();
        }
    }
}
