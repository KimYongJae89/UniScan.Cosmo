using System;
using System.Drawing;
using System.IO;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniEye.Base.Inspect;
using System.Threading;
using System.Windows.Forms;
using DynMvp.Vision;
using DynMvp.UI; 
using DynMvp.UI.Touch;
using UniEye.Base.Device;
using UniScanG.Inspect;
using UniScanG.Screen.Vision.Detector;
using UniScanG.Screen.Vision;
using System.Collections.Generic;
using UniScanG.Vision;
using UniScan.Common.Exchange;
using UniScanG.Screen.Data;
using DynMvp.Authentication;
using UniScanG.Data;
using System.Diagnostics;
using UniScan.Common;
using UniScanG.Screen.Vision.FiducialFinder;

namespace UniScanG.Screen.Inspect
{
    internal class InspectRunnerInspectorS : UniScanG.Inspect.InspectRunner
    {
        public InspectRunnerInspectorS() : base()
        {
            foreach (ImageDevice imageDevice in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
            {
                Vision.ProcessBufferSetS processBufferSetS = new Vision.ProcessBufferSetS(imageDevice.ImageSize.Width, imageDevice.ImageSize.Height);
                processBufferSetS.BuildBuffers();

                this.processBufferManager.AddProcessBufferSet(processBufferSetS, 5);
                
                this.grabProcesser = new GrabProcesserS(imageDevice);
                this.grabProcesser.StartInspectionDelegate = StartFiducialInspect;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            processBufferManager.Dispose();
        }

        protected override void SetupUnitManager() { }

        public override bool EnterWaitInspection()
        {
            if (SystemState.Instance().GetOpState() != OpState.Idle)
                return false;

            if (SystemManager.Instance().CurrentModel == null)
                return false;
            
            if (SystemManager.Instance().CurrentModel.ModelDescription.IsTrained == false)
            {
                MessageForm.Show(null, "There is no data or teach state is invalid.");
                return false;
            }

            if (SystemManager.Instance().ProductionManager.CurProduction == null)
                return false;

            FiducialFinderS fiducialFinder = (FiducialFinderS)AlgorithmPool.Instance().GetAlgorithm(FiducialFinderS.TypeName);
            SheetInspector sheetInspector = (SheetInspector)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName);

            if (AlgorithmSetting.Instance().IsFiducial == true)
            {
                InspectUnit fiducialUnit = new InspectUnit(FiducialFinderS.TypeName, fiducialFinder);
                fiducialUnit.UnitInspected = FiducialInspected;
                inspectUnitManager.Add(fiducialUnit);
            }

            InspectUnit inspectUnit = new InspectUnit(sheetInspector.GetAlgorithmType(), sheetInspector);
            inspectUnit.UnitInspected = DetectorInspected;
            inspectUnitManager.Add(inspectUnit);

            inspectUnitManager.AllUnitInspected = InspectDone;

            inspectUnitManager.Run();
            
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.AddImageGrabbed(grabProcesser.ImageGrabbed);
            
            imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);
            imageDeviceHandler.SetExposureTime(2500);
            imageDeviceHandler.GrabMulti();
            SystemState.Instance().SetWait();

            LogHelper.Info(LoggerType.Inspection, string.Format("Start - Model : {0}, Lot : {1}", SystemManager.Instance().CurrentModel.Name, SystemManager.Instance().ProductionManager.CurProduction.LotNo));

            return true;
        }

        public override void ExitWaitInspection()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            imageDeviceHandler.Stop();

            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);
            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed = null;
            
            inspectUnitManager.Stop();
            SimpleProgressForm form = new SimpleProgressForm();
            form.Show(() =>
            {
                while(inspectUnitManager.IsRunning() == true)
                {
                    Thread.Sleep(100);
                }
            });

            inspectUnitManager.Dispose();

            processBufferManager.Return(null);

            SystemState.Instance().SetInspectState(InspectState.Done);
            SystemState.Instance().SetIdle();

            LogHelper.Info(LoggerType.Inspection, string.Format("Stop - Total {0}", SystemManager.Instance().ProductionManager.CurProduction.Total));
        }

        public override void EnterPauseInspection()
        {
            SystemState.Instance().SetInspectState(InspectState.Pause);
        }
        
        public void FiducialInspected(UnitInspectItem unitInspectParam)
        {
            if (unitInspectParam.InspectionResult.ProbeResultList.Count == 0)
                return;

            if (unitInspectParam.InspectionResult.ProbeResultList[0] is DynMvp.Data.VisionProbeResult == false)
                return;

            AlgorithmResult algorithmResult = unitInspectParam.InspectionResult.AlgorithmResultLDic[FiducialFinderS.TypeName];
            SizeF fidOffset = algorithmResult.OffsetFound;
            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.F_FOUNDED, fidOffset.Width.ToString(), fidOffset.Height.ToString());
            
            StartSheetInspector(unitInspectParam, fidOffset);
        }

        public void DetectorInspected(UnitInspectItem unitInspectParam)
        {
            
        }

        public void StartSheetInspector(UnitInspectItem unitInspectParam, SizeF fidOffset)
        {
            SheetInspectParam sheetAlgorithmInspectParam = (SheetInspectParam)unitInspectParam.AlgorithmInspectParam;
            sheetAlgorithmInspectParam.FidOffset = fidOffset;

            InspectionResult inspectionResult = BuildInspectionResult();
            UnitInspectItem unitInspectItem = CreateInspectItem(sheetAlgorithmInspectParam, inspectionResult, unitInspectParam.InspectionOption);

            inspectUnitManager.StartInspect(SheetInspector.TypeName, unitInspectItem);
        }

        public override void InspectDone(UnitInspectItem unitInspectItem)
        {
            SheetInspectParam sheetAlgorithmInspectParam = (SheetInspectParam)unitInspectItem.AlgorithmInspectParam;
            UniScanG.Inspect.ProcessBufferSet processBufferSet = sheetAlgorithmInspectParam.ProcessBufferSet;
            processBufferManager.Return(processBufferSet);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            base.InspectDone(unitInspectItem);
            SystemManager.Instance().ExportData(unitInspectItem.InspectionResult);
            IClientExchangeOperator clientExchangeOperator = (IClientExchangeOperator)SystemManager.Instance().ExchangeOperator;
            UniScanG.Data.Production production = (UniScanG.Data.Production)SystemManager.Instance().ProductionManager.CurProduction;
            clientExchangeOperator.SendInspectDone(unitInspectItem.InspectionResult.InspectionNo, production.StartTime.ToString("yy-MM-dd"));
            stopwatch.Stop();
            unitInspectItem.InspectionResult.ExportTime = stopwatch.Elapsed;
            
            LogHelper.Info(LoggerType.Inspection, string.Format("Sheet No : {0}, Spend Time : {1} ms, Export Time : {2} ms", 
                unitInspectItem.InspectionResult.InspectionNo,
                unitInspectItem.InspectionResult.AlgorithmResultLDic[SheetInspector.TypeName].SpandTime.ToString("ss\\.fff"),
                unitInspectItem.InspectionResult.ExportTime.ToString("ss\\.fff")));
            InspectDone(unitInspectItem.InspectionResult);

            SystemState.Instance().SetInspectState(InspectState.Wait);
        }

        public void StartSheetInspector(SizeF fidOffset)
        {
            InspectionResult inspectionResult = BuildInspectionResult();
            ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);

            InspectionOption inspectionOption = new InspectionOption(imageDevice);

            UniScanG.Inspect.ProcessBufferSet processBufferSet = processBufferManager.Request(imageDevice);
            if (processBufferSet == null)
                return;

            IntPtr intPtr = grabProcesser.GetGrabbedImagePtr();
            if (intPtr == IntPtr.Zero)
                return;

            ImageD image = imageDevice.GetGrabbedImage(intPtr);
            if (image == null)
                return;

            SheetInspectParam sheetAlgorithmInspectParam = new SheetInspectParam(image, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
            sheetAlgorithmInspectParam.ClipImage = image;
            sheetAlgorithmInspectParam.ProcessBufferSet = processBufferSet;
            sheetAlgorithmInspectParam.FidOffset = fidOffset;

            UnitInspectItem unitInspectItem = CreateInspectItem(sheetAlgorithmInspectParam, inspectionResult, inspectionOption);

            inspectUnitManager.StartInspect(SheetInspector.TypeName, unitInspectItem);
        }

        public void StartFiducialInspect(ImageDevice imageDevice, IntPtr ptr)
        {
            InspectionResult inspectionResult = BuildInspectionResult();
            InspectionOption inspectionOption = new InspectionOption(imageDevice);

            UniScanG.Inspect.ProcessBufferSet processBufferSet = null;

            processBufferSet = processBufferManager.Request(imageDevice);

            if (processBufferSet == null)
                return;

            ImageD image = imageDevice.GetGrabbedImage(ptr);
            SheetInspectParam sheetAlgorithmInspectParam = new SheetInspectParam(image, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
            sheetAlgorithmInspectParam.ClipImage = image;
            sheetAlgorithmInspectParam.ProcessBufferSet = processBufferSet;

            UnitInspectItem unitInspectItem = CreateInspectItem(sheetAlgorithmInspectParam, inspectionResult, inspectionOption);

            inspectUnitManager.StartInspect(FiducialFinderS.TypeName, unitInspectItem);
        }
    }
}