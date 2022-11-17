using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniScanM.MachineIF;

namespace UniScanM.EDMS.MachineIF
{
    public enum UniScanMMachineIfEDMSCommand
    {
        SET_EDMS,
        SET_EDMS_READY,
        SET_EDMS_RUN,
        SET_EDMS_GOOD,
        SET_BASIC_EDGE,
        SET_ROLL_COATING_EDGE,
        SET_COATING_PRINTING_EDGE,
        SET_ROLL_PRINTING_EDGE,
        SET_TOTAL_EDGE,
        SET_FILM_PRINTING_EDGE,
    };

    public class MachineIfProtocolList : UniScanM.MachineIF.MachineIfProtocolList
    {
        public MachineIfProtocolList(params Type[] protocolListType) : base(protocolListType) { }
        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);

            for(int i=0; i< this.dic.Count;i++)
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
                        address = "D1720"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_READY:
                        address = "D1720"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_RUN:
                        address = "D1721"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS:
                        address = "D1722"; sizeWord = 7; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_EDMS_GOOD:
                        address = "D1722"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_BASIC_EDGE:
                        address = "D1723"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_ROLL_COATING_EDGE:
                        address = "D1724"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_COATING_PRINTING_EDGE:
                        address = "D1725"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_ROLL_PRINTING_EDGE:
                        address = "D1726"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_TOTAL_EDGE:
                        address = "D1727"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanMMachineIfEDMSCommand.SET_FILM_PRINTING_EDGE:
                        address = "D1728"; sizeWord = 1; isReadCommand = false; break;
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
