using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;

namespace UniScanG.Gravure.MachineIF
{
    public enum UniScanGMachineIfCommon
    {
        // common command
        GET_MACHINE_STATE,
        GET_START_STILLIMAGE,
        GET_START_COLORSENSOR,
        GET_START_EDMS,
        GET_START_PINHOLE,
        GET_START_RVMS,
        GET_TARGET_SPEED,
        GET_PRESENT_SPEED,
        GET_PRESENT_POSITION,
        GET_LOT,
        GET_MODEL,
        GET_WORKER,
        GET_PASTE,
        GET_ROLL_DIAMETER,
        GET_REWINDER_CUT,
        GET_START_GRAVURE_INSPECTOR,
        GET_START_GRAVURE_MONITOR,

        GET_MACHINE_STATE2,
        GET_USE_GRAVURE_ERASER,
        GET_USE_GRAVURE_ERASER_FORCE,

        SET_VISION_STATE_GRAVURE_INSP,
        SET_VISION_GRAVURE_INSP_READY,
        SET_VISION_GRAVURE_INSP_RUNNING,

        SET_VISION_RESULT_GRAVURE_INSP,
        SET_VISION_GRAVURE_INSP_RESULT,
        SET_VISION_GRAVURE_INSP_NG_REPDEF_P,
        SET_VISION_GRAVURE_INSP_NG_REPDEF_N,
        SET_VISION_GRAVURE_INSP_NG_NORDEF,
        SET_VISION_GRAVURE_INSP_NG_SHTLEN,

        SET_VISION_STATE_GRAVURE_INSP2,
        SET_VISION_GRAVURE_INSP_READY2,
        SET_VISION_GRAVURE_ERASER_READY,
        SET_VISION_GRAVURE_ERASER_CNT,
    };

    public class UniScanGMachineIfProtocolList : UniEye.Base.MachineInterface.MachineIfProtocolList
    {
        public UniScanGMachineIfProtocolList(params Type[] protocolListType) : base(protocolListType) { }

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
                    case UniScanGMachineIfCommon.GET_MACHINE_STATE:
                        address = "D1600"; sizeWord = 54; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_START_STILLIMAGE:
                        address = "D1600"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_START_COLORSENSOR:
                        address = "D1601"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_START_EDMS:
                        address = "D1602"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_START_PINHOLE:
                        address = "D1603"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_START_RVMS:
                        address = "D1604"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_TARGET_SPEED:
                        address = "D1605"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_PRESENT_SPEED:
                        address = "D1606"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_PRESENT_POSITION:
                        address = "D1607"; sizeWord = 2; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_LOT:
                        address = "D1610"; sizeWord = 10; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_MODEL:
                        address = "D1620"; sizeWord = 10; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_WORKER:
                        address = "D1630"; sizeWord = 10; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_PASTE:
                        address = "D1640"; sizeWord = 10; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_ROLL_DIAMETER:
                        address = "D1650"; sizeWord = 2; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_REWINDER_CUT:
                        address = "D1652"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_START_GRAVURE_INSPECTOR:
                        address = "D1653"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_START_GRAVURE_MONITOR:
                        address = "D1654"; sizeWord = 1; isReadCommand = true; break;

                    case UniScanGMachineIfCommon.GET_MACHINE_STATE2:
                        address = "D1655"; sizeWord = 2; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_USE_GRAVURE_ERASER:
                        address = "D1655"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.GET_USE_GRAVURE_ERASER_FORCE:
                        address = "D1656"; sizeWord = 1; isReadCommand = true; break;

                    case UniScanGMachineIfCommon.SET_VISION_STATE_GRAVURE_INSP:
                        address = "D1850"; sizeWord = 2; isReadCommand = false; break;
                    case UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_READY:
                        address = "D1850"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_RUNNING:
                        address = "D1851"; sizeWord = 1; isReadCommand = false; break;

                    case UniScanGMachineIfCommon.SET_VISION_RESULT_GRAVURE_INSP:
                        address = "D1852"; sizeWord = 5; isReadCommand = false; break;
                    case UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_RESULT:
                        address = "D1852"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_REPDEF_P:
                        address = "D1853"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_REPDEF_N:
                        address = "D1854"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_NORDEF:
                        address = "D1855"; sizeWord = 1; isReadCommand = false; break;
                    case UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_NG_SHTLEN:
                        address = "D1856"; sizeWord = 1; isReadCommand = false; break;

                    case UniScanGMachineIfCommon.SET_VISION_STATE_GRAVURE_INSP2:
                        address = "D1857"; sizeWord = 3; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.SET_VISION_GRAVURE_INSP_READY2:
                        address = "D1857"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.SET_VISION_GRAVURE_ERASER_READY:
                        address = "D1858"; sizeWord = 1; isReadCommand = true; break;
                    case UniScanGMachineIfCommon.SET_VISION_GRAVURE_ERASER_CNT:
                        address = "D1859"; sizeWord = 1; isReadCommand = true; break;

                    default:
                        isValid = false; address = ""; sizeWord = 0; isReadCommand = true; break;
                }

                //if (isValid == true)
                {
                    melsecMachineIfProtocol.Use = isValid;
                    melsecMachineIfProtocol.Address = address;
                    melsecMachineIfProtocol.SizeWord = sizeWord;
                    melsecMachineIfProtocol.IsReadCommand = isReadCommand;
                }
            }
        }

        public override MachineIfProtocol GetProtocol(Enum command)
        {
            if (command == null)
                return GetProtocol(UniScanGMachineIfCommon.GET_MACHINE_STATE);

            return base.GetProtocol(command);
        }

        protected override void LoadXml(XmlElement xmlElement)
        {
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("Item");
            foreach (XmlElement subElement in xmlNodeList)
            {
                UniScanGMachineIfCommon key;
                bool ok = Enum.TryParse<UniScanGMachineIfCommon>(XmlHelper.GetValue(subElement, "Command", ""), out key);
                MachineIfProtocol value = null;
                if (ok)
                {
                    value = this.dic[key];
                    value.Load(subElement, "Protocol");
                }
            }
        }

        protected override void SaveXml(XmlElement xmlElement)
        {
            foreach (KeyValuePair<Enum, MachineIfProtocol> pair in this.dic)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement("Item");
                xmlElement.AppendChild(subElement);

                XmlHelper.SetValue(subElement, "Command", pair.Key.ToString());
                pair.Value.Save(subElement, "Protocol");
            }
        }
    }
}
