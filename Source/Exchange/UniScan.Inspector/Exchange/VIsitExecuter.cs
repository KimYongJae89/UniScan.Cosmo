using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScan.Common;
using UniScan.Common.Exchange;

namespace UniScan.Inspector.Exchange
{
    public class VisitExecuter : MachineIfExecuter
    {
        char separator = ';';
        
        protected override bool Execute(string command)
        {
            string[] splitCommand = command.Split(separator);

            ExchangeCommand visitCommand;
            bool result = Enum.TryParse(splitCommand[0], out visitCommand);

            InspectorOperator client = (InspectorOperator)SystemManager.Instance().ExchangeOperator;

            switch (visitCommand)
            {
                case ExchangeCommand.V_INSPECT:
                case ExchangeCommand.V_MODEL:
                case ExchangeCommand.V_REPORT:
                case ExchangeCommand.V_SETTING:
                case ExchangeCommand.V_TEACH:
                    client.PreparePanel(visitCommand);
                    break;
                case ExchangeCommand.V_DONE:    // VNC 접속 종료
                    //client.PreparePanel(ExchangeCommand.V_INSPECT);
                    //client.ClearPanel();
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
