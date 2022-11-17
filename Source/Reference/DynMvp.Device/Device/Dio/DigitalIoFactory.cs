using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynMvp.Base;
using DynMvp.Devices.MotionController;

namespace DynMvp.Devices.Dio
{
    public class DigitalIoFactory
    {
        public static bool IsSlaveDevice(DigitalIoType type)
        {
            switch (type)
            {
                case DigitalIoType.AlphaMotion302:
                case DigitalIoType.AlphaMotion304:
                case DigitalIoType.AlphaMotion314:
                case DigitalIoType.FastechEziMotionPlusR:
                case DigitalIoType.Ajin:
                case DigitalIoType.AlphaMotionBx:
                case DigitalIoType.AlphaMotionBBx:
                    return true;
            }

            return false;
        }

        public static bool IsMasterDevice(MotionType type)
        {
            switch (type)
            {
                case MotionType.AlphaMotion302:
                case MotionType.AlphaMotion304:
                case MotionType.AlphaMotion314:
                case MotionType.FastechEziMotionPlusR:
                case MotionType.Ajin:
                case MotionType.AlphaMotionBx:
                case MotionType.AlphaMotionBBx:
                    return true;
            }

            return false;
        }

        public static DigitalIo Create(DigitalIoInfo digitalIoInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Create Digital I/O");

            DigitalIo digitalIo = null;
            switch (digitalIoInfo.Type)
            {
                case DigitalIoType.Adlink7230:
                case DigitalIoType.Adlink7432:
                    digitalIo = new DigitalIoDASK(digitalIoInfo.Type, digitalIoInfo.Name);
                    break;
                case DigitalIoType.Virtual:
                    digitalIo = new DigitalIoVirtual(digitalIoInfo.Name);
                    break;
                case DigitalIoType.Modubus:
                    digitalIo = new DigitalIoModubus(digitalIoInfo.Name);
                    break;
                case DigitalIoType.ComizoaSd424f:
                    digitalIo = new DigitalIoComizoa(digitalIoInfo.Type, digitalIoInfo.Name);
                    break;
                case DigitalIoType.TmcAexxx:
                    digitalIo = new DigitalIoTmcAexxx(digitalIoInfo.Name);
                    break;
                case DigitalIoType.NIMax:
                    digitalIo = new DigitalIoNIMax(digitalIoInfo.Name);
                    break;
                case DigitalIoType.KM6050:
                    digitalIo = new DigitalIoKM6050(digitalIoInfo.Name);
                    break;
                case DigitalIoType.Ajin:
                    digitalIo = new DigitalIoAjin(digitalIoInfo.Name);
                    break;
            }

            if (digitalIo == null)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo , (int)CommonError.FailToCreate, 
                    ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToCreate.ToString(), String.Format("Can't create digital I/O. {0}", digitalIoInfo.Type.ToString()));
                return null;
            }

            if (digitalIo.Initialize(digitalIoInfo) == false)
            {
                ErrorManager.Instance().Report((int)ErrorSection.DigitalIo, (int)CommonError.FailToInitialize, 
                    ErrorLevel.Error, ErrorSection.DigitalIo.ToString(), CommonError.FailToInitialize.ToString(), String.Format("Fail to initialize Digital I/O. {0}", digitalIoInfo.Type.ToString()));
                digitalIo.UpdateState(DeviceState.Error, "DigitalIo is invalid.");
                return null;
            }

            DeviceManager.Instance().AddDevice(digitalIo);

            return digitalIo;
        }
    }
}
