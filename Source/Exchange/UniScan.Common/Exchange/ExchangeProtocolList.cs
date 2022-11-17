using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;

namespace UniScan.Common.Exchange
{
    public enum ExchangeCommand
    {
        None = 0, U_CHANGE, C_SYNC,
        I_READY = 100, I_TEACH, I_START, I_STOP, I_DONE, I_PAUSE, I_LOTCHANGE,//Inspect
        M_SELECT = 200, M_RESELECT, M_REFRESH, M_CLOSE, M_TEACH_DONE,// Model
        J_DONE = 300, J_ERROR, // Job
        C_CONNECTED = 400, C_DISCONNECTED, //Command
        S_IDLE = 500, S_OpWait, S_INSPECT, S_PAUSE, S_ALARM, S_TEACH, S_InspWAIT, S_RUN, S_DONE, S_OpErr, //State
        V_INSPECT = 600, V_MODEL, V_TEACH, V_REPORT, V_SETTING, V_DONE, //Visit
        
        //G용
        F_FOUNDED = 700, F_SET // Fiducial
    }

    public class ExchangeProtocolList : MachineIfProtocolList
    {
        public ExchangeProtocolList(Type protocolListType) : base(protocolListType)
        {

        }

        public override void Initialize(MachineIfType machineIfType)
        {
            base.Initialize(machineIfType);
            for(int i=0; i< this.dic.Count;i++)
            {
                this.dic.ElementAt(i).Value.WaitResponceMs = 0;
            }
        }

        protected override void LoadXml(XmlElement xmlElement)
        {
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("Item");
            foreach (XmlElement subElement in xmlNodeList)
            {
                ExchangeCommand key;
                bool ok = Enum.TryParse<ExchangeCommand>(XmlHelper.GetValue(subElement, "Command", ""), out key);
                MachineIfProtocol value = null;
                if (ok) 
                {
                    value = this.dic[key];
                    if (value != null)
                    {
                        value.Load(subElement, "Protocol");
                        value.WaitResponceMs = 0;

                        this.dic[key] = value;
                    }
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
                pair.Value?.Save(subElement, "Protocol");
            }
        }

    }
}
