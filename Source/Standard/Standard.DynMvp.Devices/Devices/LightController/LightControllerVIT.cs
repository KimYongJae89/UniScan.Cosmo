using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Standard.DynMvp.Base;
using System.Xml;
using Standard.DynMvp.Devices.Dio;
using Standard.DynMvp.Base.Helpers;

namespace Standard.DynMvp.Devices.LightController
{
    public class LightControllerVIT : LightController
    {
        public LightControllerVIT(LightControllerInfo info, LightControlExcuter lightControlExcuter) : base(info, lightControlExcuter)
        {
        }

        protected override string GetTrunOffMessage()
        {
            var lightControllerInfo = Info as LightControllerInfo;
            string packet = string.Empty;
            for (int i = 0; i < lightControllerInfo.NumChannel; i++)
                packet += String.Format("C{0}{1:000}\r\n", i + 1, 0);

            return packet;
        }

        protected override string GetTrunOnMessage(uint lightValue)
        {
            var lightControllerInfo = Info as LightControllerInfo;
            string packet = string.Empty;
            for (int i = 0; i < lightControllerInfo.NumChannel; i++)
                packet += String.Format("C{0}{1:000}\r\n", i + 1, lightValue);

            return packet;
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
