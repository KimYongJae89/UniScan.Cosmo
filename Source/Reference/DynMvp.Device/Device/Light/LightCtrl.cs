using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynMvp.Base;
using System.Xml;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Comm;

namespace DynMvp.Devices.Light 
{
    public enum LightCtrlType
    {
        None, IO, Serial
    }

    public class InvalidLightSizeException : ApplicationException
    {
    
    }

    public abstract class LightCtrlInfo
    {
        string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        LightCtrlType type;
        public LightCtrlType Type
        {
            get { return type; }
            set { type = value; }
        }

        int numChannel;
        public int NumChannel
        {
            get { return numChannel; }
            set { numChannel = value; }
        }

        public virtual void SaveXml(XmlElement lightElement)
        {
            XmlHelper.SetValue(lightElement, "Name", name.ToString());
            XmlHelper.SetValue(lightElement, "LightCtrlType", type.ToString());
            XmlHelper.SetValue(lightElement, "NumChannel", numChannel.ToString());
        }

        public virtual void LoadXml(XmlElement lightInfoElement)
        {
            name = XmlHelper.GetValue(lightInfoElement, "Name", "");
            type = (LightCtrlType)Enum.Parse(typeof(LightCtrlType), XmlHelper.GetValue(lightInfoElement, "LightCtrlType", LightCtrlType.IO.ToString()));
            numChannel = Convert.ToInt32(XmlHelper.GetValue(lightInfoElement, "NumChannel", ""));
        }

        public abstract LightCtrlInfo Clone();

        public virtual void Copy(LightCtrlInfo srcInfo)
        {
            name = srcInfo.name;
            type = srcInfo.type;
            numChannel = srcInfo.numChannel;
        }
    }

    public class LightCtrlInfoList : List<LightCtrlInfo>
    {
        public LightCtrlInfoList Clone()
        {
            LightCtrlInfoList newLightCtrlInfoList = new LightCtrlInfoList();

            foreach (LightCtrlInfo lightCtrlInfo in this)
            {
                newLightCtrlInfoList.Add(lightCtrlInfo.Clone());
            }

            return newLightCtrlInfoList;
        }
    }

    public class LightCtrlInfoFactory
    {
        public static LightCtrlInfo Create(LightCtrlType lightCtrlType)
        {
            switch(lightCtrlType)
            {
                case LightCtrlType.IO:
                    return new IoLightCtrlInfo();
                case LightCtrlType.Serial:
                    return new SerialLightCtrlInfo();
            }

            return null;
        }
    }

    public class LightCtrlFactory
    {
        public static LightCtrl Create(LightCtrlInfo lightCtrlInfo, DigitalIoHandler digitalIoHandler, bool isVirtualMode)
        {
            LightCtrl lightCtrl = null;

            if (isVirtualMode)
            {
                lightCtrl = new LightCtrlVirtual(LightCtrlType.None, lightCtrlInfo.Name, lightCtrlInfo.NumChannel);
            }
            else
            {
                switch (lightCtrlInfo.Type)
                {
                    case LightCtrlType.IO:
                        lightCtrl = new IoLightCtrl(lightCtrlInfo.Name, digitalIoHandler);
                        break;
                    case LightCtrlType.Serial:
                        lightCtrl = new SerialLightCtrl(lightCtrlInfo.Name);
                        break;
                }
            }

            if (lightCtrl == null)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Light, (int)CommonError.FailToCreate, ErrorLevel.Error,
                    ErrorSection.Light.ToString(), CommonError.FailToCreate.ToString(), String.Format("Can't create light controller. {0}", lightCtrlInfo.Type.ToString()));
                return null;
            }

            if (lightCtrl.Initialize(lightCtrlInfo) == false)
            {
                ErrorManager.Instance().Report((int)ErrorSection.Light, (int)CommonError.FailToInitialize, ErrorLevel.Error,
                    ErrorSection.Light.ToString(), CommonError.FailToInitialize.ToString(), String.Format("Can't initialize light controller. {0}", lightCtrlInfo.Type.ToString()));

                lightCtrl = new LightCtrlVirtual(lightCtrlInfo.Type, lightCtrlInfo.Name, lightCtrlInfo.NumChannel);
                lightCtrl.UpdateState(DeviceState.Error, "Light controller is invalid.");
            }
            else
            {
                lightCtrl.UpdateState(DeviceState.Ready, "Light controller initialization succeeded.");
            }

            DeviceManager.Instance().AddDevice(lightCtrl);

            return lightCtrl;
        }
    }

    public abstract class LightCtrl : Device
    {
        LightCtrlType lightCtrlType;
        public LightCtrlType LightCtrlType
        {
            get { return lightCtrlType; }
            set { lightCtrlType = value; }
        }

        protected LightControllerVender lightControllerVender;
        public LightControllerVender LightControllerVender
        {
            get { return lightControllerVender; }
        }

        protected int lightStableTimeMs;
        public int LightStableTimeMs
        {
            get { return lightStableTimeMs;  }
            set { lightStableTimeMs = value; }
        }

        int startChannelIndex;
        public int StartChannelIndex
        {
            get { return startChannelIndex; }
            set { startChannelIndex = value; }
        }

        public int EndChannelIndex
        {
            get { return startChannelIndex + NumChannel; }
        }

        protected LightValue lastLightValue;
        public LightValue LastLightValue
        {
            get
            {
                if (lastLightValue == null)
                    lastLightValue = new LightValue(NumChannel);
                return lastLightValue;
            }
        }

        protected bool isToggleLight;
        public bool IsToggleLight
        {
            get { return isToggleLight; }
        }

        public LightCtrl(LightCtrlType lightCtrlType, string name)
        {
            if (name == "")
                Name = lightCtrlType.ToString();
            else
                Name = name;

            DeviceType = DeviceType.LightController;
            this.lightCtrlType = lightCtrlType;

            UpdateState(DeviceState.Idle);
        }

        public abstract int GetMaxLightLevel();
        public abstract int NumChannel { get; }
        public abstract bool Initialize(LightCtrlInfo lightCtrlInfo);
        public void TurnOn()
        {
            LogHelper.Debug(LoggerType.Grab, "Turn on light");

            if (lastLightValue == null)
            {
                lastLightValue = new LightValue(NumChannel);
                for (int i = 0; i < NumChannel; i++)
                    lastLightValue.Value[i] = GetMaxLightLevel();
            }

            //lastLightValue.TurnOn();

            TurnOn(lastLightValue);
        }
        public abstract void TurnOn(LightValue lightValue);
        public void TurnOff()
        {
            //LogHelper.Debug(LoggerType.Grab, "Turn off light");

            TurnOn(new LightValue(this.NumChannel));
        }
    }
}
