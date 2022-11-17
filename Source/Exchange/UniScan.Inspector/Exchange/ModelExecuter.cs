using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScan.Common;
using UniScan.Common.Exchange;
using UniScanG.UI;

namespace UniScan.Inspector.Exchange
{
    public class ModelExecuter : MachineIfExecuter
    {
        protected override bool Execute(string command)
        {
            string[] splitCommand = command.Split(',');

            ExchangeCommand modelCommand = ExchangeCommand.None;
            bool result = Enum.TryParse(splitCommand[0], out modelCommand);
            try
            {
                switch (modelCommand)
                {
                    case ExchangeCommand.M_SELECT:
                    case ExchangeCommand.M_RESELECT:
                        int camId = int.Parse(splitCommand[1]);
                        int clientId = int.Parse(splitCommand[2]);
                        if ((camId < 0 || camId == SystemManager.Instance().ExchangeOperator.GetCamIndex()) &&
                            (clientId < 0 || clientId == SystemManager.Instance().ExchangeOperator.GetClientIndex()))
                        {
                            if (modelCommand == ExchangeCommand.M_SELECT)
                            {
                                SystemManager.Instance().ExchangeOperator.SelectModel(splitCommand.Skip(3).ToArray());
                                ((InspectorOperator)SystemManager.Instance().ExchangeOperator).PreparePanel(ExchangeCommand.V_INSPECT);
                            }
                            else
                            {
                                string[] modelDiscArgs = SystemManager.Instance().CurrentModel.ModelDescription.GetArgs();
                                SystemManager.Instance().ExchangeOperator.SelectModel(modelDiscArgs);
                            }
                        }
                        break;

                    case ExchangeCommand.M_REFRESH:
                        //SystemManager.Instance().ModelManager.Refresh();
                        SystemManager.Instance().ExchangeOperator.UpdateModelList();
                        //SystemManager.Instance().UiController.ChangeTab("Model");
                        break;

                    case ExchangeCommand.M_CLOSE:
                        SystemManager.Instance().ModelManager.CloseModel();
                        //SystemManager.Instance().UiController.ChangeTab("Model");
                        break;

                    case ExchangeCommand.U_CHANGE:
                        UserHandler.Instance().CurrentUser = UserHandler.Instance().GetUser(splitCommand[1]);
                        break;

                    case ExchangeCommand.C_SYNC:
                        AdditionalSettings.Instance().Load();
                        SystemManager.Instance().ExchangeOperator.ModelTeachDone(-1);
                        break;
                    default:
                        result = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Operation, string.Format("ModelExecuter::Execute({0}), {1}", modelCommand.ToString(), ex.Message));
            }
            return result;
        }
    }
}
