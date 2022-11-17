using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Standard.DynMvp.Base;
using System.Xml;
using Standard.DynMvp.Devices.Dio;
using Standard.DynMvp.Base.Helpers;
using Standard.DynMvp.Devices.Serial;

namespace Standard.DynMvp.Devices.LightController
{
    public enum LightControllerType
    {
        VIT, Virtual
    }

    public enum LightControllerProtocol
    {
        IO, Serial
    }

    public class LightControllerInfo : DeviceInfo
    {
        LightControllerType _lightControllerType;
        public LightControllerType LightControllerType { get => _lightControllerType; }

        LightControllerProtocol _protocol;
        [SettingData(SettingDataType.Enum)]
        public LightControllerProtocol Protocol
        {
            get => _protocol;
            set
            {
                _protocol = value;
                ProtocolChanged();
            }
        }
        
        [SettingData(SettingDataType.Numeric)]
        public int NumChannel { get; set; }

        [SettingData(SettingDataType.Table)]
        public ProtocolInfo ProtocolInfo { get; set; }

        public LightControllerInfo(string name, LightControllerType lightControllerType) : base(DeviceType.LightController, name)
        {
            _lightControllerType = lightControllerType;
            ProtocolChanged();
        }

        public void ProtocolChanged()
        {
            switch (Protocol)
            {
                case LightControllerProtocol.IO:
                    break;
                case LightControllerProtocol.Serial:
                    ProtocolInfo = new SerialInfo();
                    break;
            }

            _settings.Clear();
            foreach (var data in SettingDataAttribute.GetProperties(this))
                _settings.Add(data);
        }
    }
}