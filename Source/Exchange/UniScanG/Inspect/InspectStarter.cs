using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.UI;
//using DynMvp.Data;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.Gravure.MachineIF;
using UniScanG.Gravure.Settings;

namespace UniScanG.Inspect
{
    public class InspectStarter : ThreadHandler
    {
        AdditionalSettings additionalSettings = null;

        public InspectStarter() : base("InspectStarter")
        {
            additionalSettings = (AdditionalSettings)AdditionalSettings.Instance();
            this.workingThread = new System.Threading.Thread(ThreadProc);
        }

        private void ThreadProc()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            bool autoStartEnable = true;
            Thread.Sleep(5000);
            while (this.requestStop == false)
            {
                Thread.Sleep(1000);
                bool isAlarmed = ErrorManager.Instance().IsAlarmed();
                if (isAlarmed)
                    continue;

                if (SystemManager.Instance().CurrentModel == null)
                    continue;

                if (additionalSettings.AutoOperation)
                {
                    bool startRequest = IsStartRequest();

                    if (SystemState.Instance().GetOpState() == OpState.Idle && startRequest)
                    {
                        if (autoStartEnable)
                        {
                            LogHelper.Info(LoggerType.Operation, "PLC set start the insepct");
                            autoStartEnable = false; // 설비동작 후 검사동작 '아니오' 시 설비정비 이전까지 자동시작 막기
                            cancellationTokenSource = new CancellationTokenSource();
                            Task.Run(() =>
                            {
                                bool ok = EnterWaitInspection(false, cancellationTokenSource.Token);
                                if (ok && cancellationTokenSource.IsCancellationRequested == false)
                                    SystemManager.Instance().InspectRunner.PostEnterWaitInspection();
                            }, cancellationTokenSource.Token);
                        }
                    }
                    else if (!startRequest)
                    {
                        autoStartEnable = true;
                        cancellationTokenSource?.Cancel();
                        if (SystemState.Instance().GetOpState() == OpState.Inspect)
                        {
                            LogHelper.Info(LoggerType.Operation, "PLC set stop the insepct");
                            ExitWaitInspection();
                        }
                    }
                }
            }
        }

        public bool EnterWaitInspection(bool userQuary, CancellationToken cancellationToken)
        {
            bool skipMessage = false; // (userQuary == false) && UserHandler.Instance().CurrentUser.SuperAccount;
            Model curModel = SystemManager.Instance().CurrentModel;
            if (curModel == null)
                return false;

            string modelName = curModel.Name;
            string machineModelname = GetMachineModelName();

            if (modelName != machineModelname)
            {
                StringBuilder modelCheckStringBuilder = new StringBuilder();
                modelCheckStringBuilder.AppendLine(string.Format(StringManager.GetString("Selected model is [{0}]."), modelName));

                if (string.IsNullOrEmpty(machineModelname))
                    modelCheckStringBuilder.AppendLine(StringManager.GetString("There is no progressing model."));
                else
                    modelCheckStringBuilder.AppendLine(string.Format(StringManager.GetString("Progressing model is [{0}]."), machineModelname));

                modelCheckStringBuilder.AppendLine(StringManager.GetString("Do you want to continue?"));

                if (skipMessage == false)
                {
                    bool userConfirm = MessageForm.Show(null, modelCheckStringBuilder.ToString().Trim(), MessageFormType.YesNo, cancellationToken) == DialogResult.Yes;
                    if (userConfirm == false)
                        return false;
                }
            }

            // LOT 읽음
            UniScanG.Data.Production oldProduction = (UniScanG.Data.Production)SystemManager.Instance().ProductionManager.GetLastProduction(curModel);
            string newLotName = GetMachineLotName()?.Trim(' ', '\0');
#if DEBUG
            if (string.IsNullOrEmpty(newLotName))
                newLotName = oldProduction == null ? "" : oldProduction.LotNo;
#endif
            // LOT 없음
            if (skipMessage == false)
            {
                UniScanG.Data.UI.InputForm inputForm = new UniScanG.Data.UI.InputForm("Lot No", newLotName);
                inputForm.ValidCheckFunc = new Data.UI.ValidCheckFunc(f => !f.Contains('\\') && !string.IsNullOrEmpty(f.Trim(' ', '\0')));

                DialogResult dialogResult = inputForm.ShowDialog();
                if (dialogResult == DialogResult.Cancel || cancellationToken.IsCancellationRequested)
                    return false;

                newLotName = inputForm.InputText.Trim(' ', '\0');
            }
            else
            {
                newLotName = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            }
            
            SystemManager.Instance().ProductionManager.LotChange(curModel, DateTime.Now.Date, newLotName);
            SystemManager.Instance().ProductionManager.Save();
            (SystemManager.Instance().UiChanger.InspectControl as UI.Inspect.InspectPage)?.InspectDefectPanel.Reset();

            bool ok = SystemManager.Instance().InspectRunner.EnterWaitInspection();
            if (ok && userQuary)
            {
                ok = SystemManager.Instance().InspectRunner.PostEnterWaitInspection();
                if (ok == false)
                    ExitWaitInspection();
            }
            return ok;
        }

        public string GetMachineModelName()
        {
            MachineIfProtocolResponce responce = SendCommand(UniScanGMachineIfCommon.GET_MODEL);
            if (responce == null)
                return SystemManager.Instance().CurrentModel.Name;

            if (responce.IsGood == false)
                return null;

            string machineModelName = responce.Convert2StringLE();
            return machineModelName;
        }

        public string GetMachineLotName()
        {
            MachineIfProtocolResponce responce = SendCommand(UniScanGMachineIfCommon.GET_LOT);
            if (responce == null)
                return null;

            if (responce.IsGood == false)
                return null;

            string machineLot = responce.Convert2StringLE();
            return machineLot;
        }

        private void ExitWaitInspection()
        {
            SystemManager.Instance().InspectRunner.ExitWaitInspection();
        }

        private bool IsStartRequest()
        {
            MachineIfProtocolResponce responce = SendCommand(UniScanGMachineIfCommon.GET_START_GRAVURE_INSPECTOR);
            if (responce.IsGood == false || string.IsNullOrEmpty(responce.ReciveData))
                return false; // 통신 실패

            return int.Parse(responce.ReciveData) == 1;
        }

        private MachineIfProtocolResponce SendCommand(Enum e)
        {
            MachineIfProtocol protocol = SystemManager.Instance().MachineIfProtocolList?.GetProtocol(e);
            MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(protocol);
            responce?.WaitResponce();
            return responce;
        }
    }
}
