using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Device;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation.Operators;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation
{
    public class OperatorManager : IOperatorListner
    {
        CancellationTokenSource cancellationTokenSource;

        IoPort doorPort;

        ResultCombiner resultCombiner;

        ResultStorage resultStorage;

        List<Operator> operatorList;
        LightTuneOperator lightTuneOperator;
        ScanOperator scanOperator;
        ExtractOperator extractOperator;
        InspectOperator inspectOperator;
        TeachOperator teachOperator;
        
        public LightTuneOperator LightTuneOperator { get => lightTuneOperator; }
        public ScanOperator ScanOperator { get => scanOperator; }
        public ExtractOperator ExtractOperator { get => extractOperator; }
        public InspectOperator InspectOperator { get => inspectOperator; }
        public TeachOperator TeachOperator { get => teachOperator; }
        public ResultCombiner ResultCombiner { get => resultCombiner; }
        public ResultStorage ResultStorage { get => resultStorage; }

        public bool IsRun
        {
            get => operatorList.Exists(op => op.OperatorState != OperatorState.Idle);
        }

        public bool IsIdle
        {
            get => operatorList.All(op => op.OperatorState == OperatorState.Idle);
        }

        public OperatorManager()
        {
            ThreadPool.GetMaxThreads(out int workerMaxThreads, out int ioMaxhreads);
            ThreadPool.SetMinThreads(workerMaxThreads, ioMaxhreads);

            operatorList = new List<Operator>();

            lightTuneOperator = new LightTuneOperator();
            scanOperator = new ScanOperator();
            extractOperator = new ExtractOperator();
            inspectOperator = new InspectOperator();
            teachOperator = new TeachOperator();

            resultCombiner = new ResultCombiner();
            resultStorage = new ResultStorage();

            operatorList.Add(lightTuneOperator);
            operatorList.Add(scanOperator);
            operatorList.Add(extractOperator);
            operatorList.Add(inspectOperator);
            operatorList.Add(teachOperator);

            ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmStatus;

            if (SystemManager.Instance().DeviceBox.PortMap != null)
            {
                PortMap portMap = (PortMap)SystemManager.Instance().DeviceBox.PortMap;
                doorPort = portMap.GetOutPort(PortMap.OutPortName.OutDoorLock);
                SystemManager.Instance().DeviceBox.DigitalIoHandler?.WriteOutput(doorPort, false);
            }

            SystemManager.Instance().AddOperatorListnerList(this);
        }

        private void ErrorManager_OnResetAlarmStatus()
        {
            Cancle();
        }

        public bool Cancle(string message = null)
        {
            LogHelper.Info(LoggerType.Inspection, "Stop Operation");

            cancellationTokenSource?.Cancel();
            SystemManager.Instance().DeviceController.RobotStage?.StopMove();
            SystemManager.Instance().DeviceBox.ImageDeviceHandler?.Stop();
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            operatorList.ForEach(oper => oper.Release());

            if (message != null)
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CustomMessageBox.Show(message, "Cancle", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                }));
            }

            return false;
        }
        
        public bool Start(bool teachMode)
        {
            if (operatorList.Exists(op => op.OperatorState != OperatorState.Idle))
                return Cancle("Machine is running !!");
            
            if (SystemManager.Instance().CurrentModel == null)
                return Cancle("Not selected model !!");
            
            if (SystemManager.Instance().MachineObserver.IoBox.InOnDoor1 == false || SystemManager.Instance().MachineObserver.IoBox.InOnDoor2 == false)
                return Cancle("Door is open !!");

            if (teachMode == false)
                if (SystemManager.Instance().ProductionManager.CurProduction == null)
                    return Cancle("Not input lot no !!");

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            if (imageDeviceHandler.IsVirtual)
                imageDeviceHandler.SetExposureTime(500000);

             cancellationTokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(() =>
            {
                if (teachMode == false)
                {
                    LogHelper.Info(LoggerType.Inspection, "Start Inspection");
                    
                    Production production = SystemManager.Instance().ProductionManager.CurProduction;
                    ResultKey resultKey = new ResultKey(DateTime.Now, SystemManager.Instance().CurrentModel, production);
                    
                    operatorList.ForEach(op => op.Initialize(resultKey, cancellationTokenSource));
                }
                else
                {
                    LogHelper.Info(LoggerType.Inspection, "Start Teach");

                    ResultKey resultKey = new ResultKey(DateTime.Now, SystemManager.Instance().CurrentModel, null);
                    operatorList.ForEach(op => op.Initialize(resultKey, cancellationTokenSource));
                }

                resultCombiner.Clear();
                resultStorage.Clear();
                SystemManager.Instance().OperatorCompleted(null);


                if (MachineOperator.IsHomeOK() == false)
                    if (MachineOperator.MoveHome(0, null, cancellationTokenSource) == false)
                    {
                        Cancle("Homing fail !!");
                        return;
                    }

                InfoBox.Instance.LastStartTime = DateTime.Now;
                
                if (teachMode == false)
                {
                    lightTuneOperator.Release();
                    teachOperator.Release();
                    scanOperator.Start();
                }
                else
                {
                    inspectOperator.Release();
                    lightTuneOperator.Start();
                }

            }, cancellationTokenSource.Token);

            return !cancellationTokenSource.IsCancellationRequested;
        }

        public void Processed(OperatorResult operatorResult)
        {
            //if (operatorResult.ExceptionMessage != null)
            //    return;

            if (cancellationTokenSource.IsCancellationRequested)
                return;

            resultCombiner.AddResult(operatorResult);

            switch (operatorResult.Type)
            {
                case ResultType.LightTune:
                    break;
                case ResultType.Scan:
                    extractOperator.StartExtract(operatorResult);
                    break;
                case ResultType.Extract:
                    resultStorage.AddResult(operatorResult);
                    if (operatorResult.ResultKey.Production != null)
                        inspectOperator.StartInspect((ExtractOperatorResult)operatorResult);
                    break;
                case ResultType.Inspect:
                    resultStorage.AddResult(operatorResult);
                    break;
                case ResultType.Train:
                    break;
            }
        }

        public void Completed(OperatorResult operatorResult)
        {
            if (operatorResult == null)
                return;

            if (string.IsNullOrEmpty(operatorResult.ExceptionMessage) == false && string.Compare("Completed", operatorResult.ExceptionMessage) != 0)
            {
                Cancle(operatorResult.ExceptionMessage);
                return;
            }

            if (cancellationTokenSource.IsCancellationRequested)
                return;

            switch (operatorResult.Type)
            {
                case ResultType.LightTune:
                    lightTuneOperator.Release();
                    scanOperator.Start();
                    break;
                case ResultType.Scan:
                    scanOperator.Release();
                    extractOperator.WaitExtract();
                    break;
                case ResultType.Extract:
                    extractOperator.Release();
                    if (operatorResult.ResultKey.Production != null)
                        inspectOperator.WaitInspect();
                    else
                        Teach();
                    break;
                case ResultType.Inspect:
                    inspectOperator.Inspect2();
                    inspectOperator.Release();
                    LogHelper.Info(LoggerType.Operation, "End Inspection");
                    resultCombiner.SaveInspectOperatorResult();
                    break;
                case ResultType.Train:
                    operatorResult.ResultKey.Model.ModelTraind(operatorResult);
                    LogHelper.Info(LoggerType.Inspection, "End Teach");
                    break;
            }
        }

        public bool Inspect()
        {
            ResultKey resultKey = resultStorage.LastResultKey;
            
            if (resultKey == null || resultStorage.ContainsKey(resultKey) == false)
                return false;

            if (resultStorage[resultKey].ContainsKey(ResultType.Extract) == false)
                return false;
            
            resultCombiner.Clear();

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SimpleProgressWindow inspectWindow = new SimpleProgressWindow("Inspecting");
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                inspectWindow.Show(() =>
                {
                    ResultDictinary dictionary = resultStorage[resultStorage.LastResultKey];
                    resultStorage[resultStorage.LastResultKey][ResultType.Inspect].Clear();

                    List<OperatorResult> opResult = dictionary[ResultType.Extract];
                    inspectOperator.Inspect(opResult.ConvertAll(extractOperatorResult => (ExtractOperatorResult)extractOperatorResult), cancellationTokenSource);
                }, cancellationTokenSource);
            }));

            return true;
        }

        public bool Teach()
        {
            ResultKey resultKey = resultStorage.LastResultKey;

            if (resultKey == null || resultStorage.ContainsKey(resultKey) == false)
                return false;

            if (resultStorage[resultKey].ContainsKey(ResultType.Extract) == false)
                return false;

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SimpleProgressWindow teachWindow = new SimpleProgressWindow("Trainning..");
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                teachWindow.Show(() =>
                {
                    ResultDictinary dictionary = resultStorage[resultStorage.LastResultKey];
                    //resultStorage[resultStorage.LastResultKey][ResultType.Train].Clear();

                    List<OperatorResult> opResult = dictionary[ResultType.Extract];
                    teachOperator.Train(opResult.ConvertAll(extractOperatorResult => (ExtractOperatorResult)extractOperatorResult));
                }, cancellationTokenSource);
            }));

            return true;
        }
    }
}