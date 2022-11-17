using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;

using Standard.DynMvp.Base;

using System.Threading;
using Standard.DynMvp.Base.Helpers;

namespace Standard.DynMvp.Devices.MotionController
{
    public enum MotionType
    {
        None, Virtual, AlphaMotion302, AlphaMotion304, AlphaMotion314, AlphaMotionBx, FastechEziMotionPlusR, PowerPmac, Ajin
    }
    
    public class MotionInfo : DeviceInfo
    {
        MotionType _type;
        
        public MotionType Type { get => _type; }

        [SettingData(SettingDataType.Numeric)]
        public uint AxisNum { get; set; }

        public MotionInfo(string name, MotionType type) : base(DeviceType.MotionController, name)
        {
            _type = type;
        }
    }
}