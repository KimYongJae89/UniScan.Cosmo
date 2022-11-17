using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Exchange;

namespace UniScan.Monitor.Exchange
{
    public class StateExecuter : MachineIfExecuter
    {
        protected override bool Execute(string command)
        {
            string[] splitCommand = command.Split(',');

            ExchangeCommand stateCommand;
            bool result = Enum.TryParse(splitCommand[0], out stateCommand);

            if (result == false)
                return false;

            int camId = -1;
            if (splitCommand.Length > 1)
                camId = int.Parse(splitCommand[1]);
            int cliendId = -1;
            if (splitCommand.Length > 2)
                cliendId = int.Parse(splitCommand[2]);

            switch (stateCommand)
            {
                case ExchangeCommand.S_IDLE:
                case ExchangeCommand.S_OpWait:
                case ExchangeCommand.S_InspWAIT:
                case ExchangeCommand.S_RUN:
                case ExchangeCommand.S_INSPECT:
                case ExchangeCommand.S_PAUSE:
                case ExchangeCommand.S_TEACH:
                case ExchangeCommand.S_DONE:
                case ExchangeCommand.S_ALARM:
                    StateChanged(camId, cliendId, stateCommand);
                    result = true;
                    break;
                case ExchangeCommand.M_TEACH_DONE:
                    if (cliendId <= 0)
                    {
                        IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
                        server.SyncModel(camId);
                        SystemManager.Instance().ExchangeOperator.ModelTeachDone(camId);
                    }

                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        private void StateChanged(int camId, int clientId, ExchangeCommand command)
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList().FindAll(f=>f.Info.CamIndex == camId);

            foreach (InspectorObj inspector in inspectorList)
            {
                if (clientId < 0 || inspector.Info.ClientIndex == clientId)
                {
                    switch (command)
                    {
                        case ExchangeCommand.S_IDLE:
                            inspector.OpState = UniEye.Base.Data.OpState.Idle;
                            break;
                        case ExchangeCommand.S_OpWait:
                            inspector.OpState = UniEye.Base.Data.OpState.Wait;
                            break;
                        case ExchangeCommand.S_TEACH:
                            inspector.OpState = UniEye.Base.Data.OpState.Teach;
                            break;
                        case ExchangeCommand.S_INSPECT:
                            inspector.OpState = UniEye.Base.Data.OpState.Inspect;
                            break;
                        case ExchangeCommand.S_ALARM:
                            inspector.OpState = UniEye.Base.Data.OpState.Alarm;
                            break;

                        case ExchangeCommand.S_PAUSE:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Pause;
                            break;
                        case ExchangeCommand.S_RUN:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Run;
                            break;
                        case ExchangeCommand.S_InspWAIT:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Wait;
                            break;
                        case ExchangeCommand.S_DONE:
                            inspector.InspectState = UniEye.Base.Data.InspectState.Done;
                            break;
                    }
                }
            }
        }
    }
}