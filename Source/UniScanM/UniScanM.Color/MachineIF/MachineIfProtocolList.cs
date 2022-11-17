using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniScanM.MachineIF;

namespace UniScanM.ColorSens.MachineIF
{
    public enum UniScanMMachineIfColorSensorCommand
    {
        SET_COLORSENSOR,
        SET_COLORSENSOR_READY,
        SET_COLORSENSOR_RUN,
        SET_COLORSENSOR_NG,
        SET_SHEET_BRIGHTNESS
    };

    public class MachineIfProtocolList : UniScanM.MachineIF.MachineIfProtocolList
    {
        public MachineIfProtocolList(params Type[] protocolListType) : base(protocolListType) { }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            for (int i = 0; i < this.dic.Count; i++)
            {
                Enum key = dic.ElementAt(i).Key;
                MelsecMachineIfProtocol melsecMachineIfProtocol = this.dic[key] as MelsecMachineIfProtocol;
                if (melsecMachineIfProtocol == null)
                    continue;

                string address;
                int sizeWord;
                bool isReadCommand;
                bool isValid = true;
                switch (key)
                {
                    case UniScanMMachineIfCommonCommand.SET_VISION_STATE:
                        address = "D1710"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_READY:
                        address = "D1710"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_RUN:
                        address = "D1711"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR:
                        address = "D1712"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_NG:
                        address = "D1712"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfColorSensorCommand.SET_SHEET_BRIGHTNESS:
                        address = "D1713"; sizeWord = 1; isReadCommand = false; break;
                    default:
                        isValid = false; address = ""; sizeWord = 0; isReadCommand = true; break;
                }

                if (isValid == true)
                {
                    melsecMachineIfProtocol.Address = address;
                    melsecMachineIfProtocol.SizeWord = sizeWord;
                    melsecMachineIfProtocol.IsReadCommand = isReadCommand;
                    melsecMachineIfProtocol.WaitResponceMs = 500;
                    melsecMachineIfProtocol.Use = true;
                }
            }
        }
    }
}
