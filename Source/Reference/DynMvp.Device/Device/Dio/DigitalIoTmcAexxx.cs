﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Shared;

using DynMvp.Base;

namespace DynMvp.Devices.Dio
{
    class DigitalIoTmcAexxx : DigitalIo
    {
        bool initialized = true;
        ushort cardNo = 0;

        public DigitalIoTmcAexxx(string name)
            : base(DigitalIoType.TmcAexxx, name)
        {
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            try
            {
                //PciDigitalIoInfo pciDigitalIoInfo = (PciDigitalIoInfo)digitalIoInfo;

                int retVal = TMCAEDLL.AIO_LoadDevice();

                if (retVal < 0)
                {
                    ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize, ErrorLevel.Error,
                        ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), String.Format("Can't load TMC AExxx Device. ( Type = {0} )", digitalIoType.ToString()));
                    return false;
                }

                //cardNo = (ushort)pciDigitalIoInfo.Index;
                cardNo = (ushort)digitalIoInfo.Index;

                uint uiModel = 0;
                uint uiComm = 0;
                uint uiDiNum = 0;
                uint uiDoNum = 0;

                TMCAEDLL.AIO_BoardInfo(cardNo, ref uiModel, ref uiComm, ref uiDiNum, ref uiDoNum);

                NumInPortGroup = digitalIoInfo.NumInPortGroup;
                NumOutPortGroup = digitalIoInfo.NumOutPortGroup;
                NumInPort = (int)uiDiNum;
                NumOutPort = (int)uiDoNum;

                initialized = true;

                return true;

            }
#if DEBUG == false
            catch (Exception ex)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize, ErrorLevel.Error,
                    ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), String.Format("TMC DIO Device initalization is failed. ( Type = {0} ) : {1}", digitalIoType.ToString(), ex.Message));
            }
#endif
            finally { }

            return false;
        }

        public override bool IsReady()
        {
            return initialized;
        }

        public override void Release()
        {
            base.Release();

            try
            {
                if (IsReady())
                    TMCAEDLL.AIO_UnloadDevice();
            }
            catch
            {
            }
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            TMCAEDLL.AIO_PutDODWord(cardNo, 0, outputPortStatus);
            byte[] bytes = BitConverter.GetBytes(outputPortStatus);
            LogHelper.Debug(LoggerType.IO, string.Format("IO WritetputGroup : {0}", BitConverter.ToString(bytes)));
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            uint outputPortStatus = 0;
            TMCAEDLL.AIO_GetDODWord(cardNo, 0, ref outputPortStatus);

            return outputPortStatus;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            uint inputPortStatus = 0;
            TMCAEDLL.AIO_GetDIDWord(cardNo, 0, ref inputPortStatus);

            return inputPortStatus;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            
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
