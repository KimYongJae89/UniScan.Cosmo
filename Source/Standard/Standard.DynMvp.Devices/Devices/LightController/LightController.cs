using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Standard.DynMvp.Base;
using System.Xml;
using Standard.DynMvp.Devices.Dio;
using Standard.DynMvp.Base.Helpers;
using System.Threading.Tasks;

namespace Standard.DynMvp.Devices.LightController
{
    public abstract class LightControlExcuter : IDisposable
    {
        public abstract void Dispose();
        public abstract Task Excute(string message);
    }

    public abstract class LightController : Device
    {
        LightControlExcuter _lightControlExcuter;

        LightControllerInfo _info;
        public LightControllerInfo Info { get => _info; }

        protected LightController(LightControllerInfo info, LightControlExcuter lightControlExcuter) : base(info)
        {
            _info = info;
            _lightControlExcuter = lightControlExcuter;
        }

        protected abstract string GetTrunOnMessage(uint lightValue);
        protected abstract string GetTrunOffMessage();

        public static LightController Create(LightControllerInfo lightControllerInfo, LightControlExcuter lightControlExcuter)
        {
            switch (lightControllerInfo.LightControllerType)
            {
                case LightControllerType.VIT:
                    return new LightControllerVIT(lightControllerInfo, lightControlExcuter);
                case LightControllerType.Virtual:
                    break;
            }

            return null;
        }

        public async void TurnOn(uint lightValue)
        {
            await _lightControlExcuter?.Excute(GetTrunOnMessage(lightValue));
        }

        public async void TurnOff()
        {
            await _lightControlExcuter?.Excute(GetTrunOffMessage());
        }

        protected override void Release()
        {
            _lightControlExcuter?.Dispose();
        }
    }

    //public class LightCtrlInfoFactory
    //{
    //    public static LightCtrlInfo Create(LightCtrlType lightCtrlType)
    //    {
    //        switch (lightCtrlType)
    //        {
    //            case LightCtrlType.IO:
    //                return new IoLightCtrlInfo();
    //            case LightCtrlType.Serial:
    //                return new SerialLightCtrlInfo();
    //        }

    //        return null;
    //    }
    //}

    //public class LightCtrlFactory
    //{
    //    public static LightCtrl Create(LightCtrlInfo lightCtrlInfo, DigitalIoHandler digitalIoHandler, bool isVirtualMode)
    //    {
    //        LightCtrl lightCtrl = null;

    //        if (isVirtualMode)
    //        {
    //            lightCtrl = new LightCtrlVirtual(LightCtrlType.None, lightCtrlInfo.Name, lightCtrlInfo.NumChannel);
    //        }
    //        else
    //        {
    //            switch (lightCtrlInfo.Type)
    //            {
    //                case LightCtrlType.IO:
    //                    lightCtrl = new IoLightCtrl(lightCtrlInfo.Name, digitalIoHandler);
    //                    break;
    //                case LightCtrlType.Serial:
    //                    lightCtrl = new SerialLightCtrl(lightCtrlInfo.Name);
    //                    break;
    //            }
    //        }

    //        if (lightCtrl == null)
    //        {
    //            ErrorManager.Instance().Report((int)ErrorSection.Light, (int)CommonError.FailToCreate, ErrorLevel.Error,
    //                ErrorSection.Light.ToString(), CommonError.FailToCreate.ToString(), String.Format("Can't create light controller. {0}", lightCtrlInfo.Type.ToString()));
    //            return null;
    //        }

    //        if (lightCtrl.Initialize(lightCtrlInfo) == false)
    //        {
    //            ErrorManager.Instance().Report((int)ErrorSection.Light, (int)CommonError.FailToInitialize, ErrorLevel.Error,
    //                ErrorSection.Light.ToString(), CommonError.FailToInitialize.ToString(), String.Format("Can't initialize light controller. {0}", lightCtrlInfo.Type.ToString()));

    //            lightCtrl = new LightCtrlVirtual(lightCtrlInfo.Type, lightCtrlInfo.Name, lightCtrlInfo.NumChannel);
    //            lightCtrl.UpdateState(DeviceState.Error, "Light controller is invalid.");
    //        }
    //        else
    //        {
    //            lightCtrl.UpdateState(DeviceState.Ready, "Light controller initialization succeeded.");
    //        }

    //        //DeviceManager.Instance().AddDevice(lightCtrl);

    //        return lightCtrl;
    //    }
    //}


}
