using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniScanM.MachineIF;

namespace UniScanM.StillImage.MachineIF
{
    public enum UniScanMMachineIfStillImageCommand
    {
        SET_STILLIMAGE,
        SET_STILLIMAGE_READY,
        SET_STILLIMAGE_RUN,
        SET_STILLIMAGE_GOOD,
        SET_MARGIN_W,
        SET_MARGIN_L,
        SET_BLOT_W,
        SET_BLOT_L,
        SET_DEFECT_W,
        SET_DEFECT_L    // StillImage
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
                        address = "D1800"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE:
                        address = "D1802"; sizeWord = 14; isReadCommand = false; break;
                    case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_READY:
                        address = "D1800"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_RUN:
                        address = "D1801"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_GOOD:
                        address = "D1802"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfStillImageCommand.SET_MARGIN_W:
                        address = "D1804"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfStillImageCommand.SET_MARGIN_L:
                        address = "D1806"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfStillImageCommand.SET_BLOT_W:
                        address = "D1808"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfStillImageCommand.SET_BLOT_L:
                        address = "D1810"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfStillImageCommand.SET_DEFECT_W:
                        address = "D1812"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfStillImageCommand.SET_DEFECT_L:
                        address = "D1814"; sizeWord = 2; isReadCommand = false; break;

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
