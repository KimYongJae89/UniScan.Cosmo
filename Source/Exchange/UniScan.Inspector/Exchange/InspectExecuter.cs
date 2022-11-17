using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScan.Common;
using UniEye.Base.MachineInterface;
using UniScan.Common.Exchange;
using UniEye.Base.Data;

namespace UniScan.Inspector.Exchange
{
    public class InspectExecuter : MachineIfExecuter
    {
        char separator = ';';
        Task teachTask = null;

        protected override bool Execute(string command)
        {
            InspectorOperator client = (InspectorOperator)SystemManager.Instance().ExchangeOperator;

            string[] splitCommand = command.Split(',');

            ExchangeCommand inspectCommand;
            bool result = Enum.TryParse(splitCommand[0], out inspectCommand);
            
            switch (inspectCommand)
            {
                case ExchangeCommand.I_LOTCHANGE:
                    break;
                case ExchangeCommand.I_PAUSE:
                    SystemManager.Instance().InspectRunner.EnterPauseInspection();
                    break;
                case ExchangeCommand.I_READY:
                    //client.PreparePanel(ExchangeCommand.V_INSPECT);
                    UniScanG.Data.ProductionManager productionManager = SystemManager.Instance().ProductionManager as UniScanG.Data.ProductionManager;
                    ProductionBase curProduction = null;
                    if (splitCommand.Length > 2)
                        curProduction = productionManager.LotChange(SystemManager.Instance().CurrentModel, splitCommand[1], Convert.ToSingle(splitCommand[2]));
                    else
                        curProduction = productionManager.LotChange(SystemManager.Instance().CurrentModel, splitCommand[1]);

                    curProduction.Reset();
                    SystemManager.Instance().InspectRunner.EnterWaitInspection();
                    break;
                case ExchangeCommand.I_START:
                    SystemManager.Instance().InspectRunner.PostEnterWaitInspection();
                    break;
                case ExchangeCommand.I_TEACH:
                    if (teachTask==null || teachTask.IsCompleted)
                        teachTask = Task.Run(() =>
                        {
                            client.PreparePanel(ExchangeCommand.V_TEACH);
                            SystemManager.Instance().ModellerPageExtender.AutoTeachProcess();
                        });
                    break;
                case ExchangeCommand.I_STOP:
                    if (SystemState.Instance().GetOpState() != OpState.Idle)
                        SystemManager.Instance().InspectRunner.ExitWaitInspection();
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
