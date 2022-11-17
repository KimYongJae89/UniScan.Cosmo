//using System;
//using System.Collections.Generic;
//using System.IO;

//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.Devices;

//using DynMvp.InspData;
//using UniEye.Base.Settings;
//using UniEye.Base;
//using UniScanG.Operation.UI;

//namespace UniScanG.Temp
//{
//    class InspectRunnerExtenderInspector : UniEye.Base.Inspect.InspectRunnerExtender
//    {
//        string receivedInspectionNo;
//        public string ReceivedInspectionNo
//        {
//            get { return receivedInspectionNo; }
//            set { receivedInspectionNo = value; }
//        }

//        public override string GetInspectionNo(DynMvp.InspData.InspectionResult inspectionResult)
//        {
//            if (string.IsNullOrEmpty(receivedInspectionNo))
//            {
//                production.LastSequenceNo++;
//                return production.LastSequenceNo.ToString();
//            }

//            string inspectionNo = receivedInspectionNo;

//            receivedInspectionNo = "";

//            return inspectionNo;
//        }

//        public override DynMvp.InspData.InspectionResult CreateInspectionResult()
//        {
//            return new Operation.Data.InspectionResult();
//        }

//        public override void OnPreInspection()
//        {
//            base.OnPreInspection();
//        }

//        public override void PreStepInspection(InspectionStep inspectionStep, DynMvp.InspData.InspectionResult inspectionResult)
//        {

//        }

//        public override void PostStepInspection(InspectionStep inspectionStep, DynMvp.InspData.InspectionResult inspectionResult)
//        {

//        }

//        public override bool EnableInspectionStep(InspectionStep inspectionStep)
//        {
//            return true;
//        }

//        public override InspectionResult BuildInspectionResult()
//        {
//            LogHelper.Debug(LoggerType.Inspection, "BuildInspectionResult");
//            string inspectioNo = GetInspectionNo(null);
//            if (int.Parse(inspectioNo) % 2 == UniScanGSettings.Instance().InspectorInfo.ClientIndex)
//                return null;


//            Operation.Data.InspectionResult inspectionResult = (Operation.Data.InspectionResult)CreateInspectionResult();

//            inspectionResult.ModelName = SystemManager.Instance().CurrentModel.Name;

//            inspectionResult.InspectionTime = new TimeSpan(0);
//            inspectionResult.InspectionStartTime = DateTime.Now;
//            inspectionResult.InspectionNo = inspectioNo;
//            inspectionResult.JobOperator = SystemManager.Instance().UserHandler.CurrentUser.Id;
//            inspectionResult.GrabImageList = new List<ImageD>();

//            inspectionResult.ModelName = SystemManager.Instance().CurrentModel.Name;
//            inspectionResult.LotNo = production.LotNo;

//            ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);

//            for (int i = 0; i < SystemManager.Instance().DeviceBox.ImageDeviceHandler.Count; i++)
//            {
//                inspectionResult.GrabImageList.Add(null);
//            }
            
//            string shortTime = inspectionResult.InspectionStartTime.ToString("yyyyMMdd");
//            string hourStr = inspectionResult.InspectionStartTime.ToString("HH");

//            inspectionResult.InputBarcode = "";

//            Production curProduction = (SystemManager.Instance().MainForm as MainForm).InspectionPage.GetCurrentProduction();
//            //Production curProduction = (Operation.Data.Production)SystemManager.Instance().CurrentModel.ProductionList.GetLastProduction();
//            LogHelper.Debug(LoggerType.Inspection, String.Format("Create Inspection Result {0}", inspectionResult.InspectionNo));

//            inspectionResult.MonitorResultPath = Path.Combine(curProduction.StartTime.ToString("yyyy-MM-dd"), SystemManager.Instance().CurrentModel.Name, production.LotNo, inspectionResult.InspectionNo);
//            inspectionResult.ResultPath = Path.Combine(PathSettings.Instance().Result, inspectionResult.MonitorResultPath);

//            inspectionResult.ResultViewIndex = resultViewIndex;
//            resultViewIndex = (resultViewIndex + 1) % CustomizeSettings.Instance().NumOfResultView;

//            //SystemManager.Instance().MainForm.UpdateInspectionNo(inspectionResult.InspectionNo);

//            Directory.CreateDirectory(inspectionResult.ResultPath);

//            LogHelper.Debug(LoggerType.Inspection, String.Format("Create Inspection Result Finished return Inspectio Result{0}", inspectionResult.InspectionNo));

//            return inspectionResult;
//        }

//        public override void InspectionStepInspected(InspectionStep inspectionStep, int inspectCount, DynMvp.InspData.InspectionResult inspectionResult)
//        {
//            LogHelper.Debug(LoggerType.Inspection, "InspectionStepInspected");

//            base.InspectionStepInspected(inspectionStep, inspectCount, inspectionResult);
//        }

//        public override void OnPostInspection()
//        {
//            LogHelper.Debug(LoggerType.Inspection, "OnPostInspection");

//            base.OnPostInspection();
//        }

//        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
//        {
//            //SystemManager.Instance().ExportData(inspectionResult);

//            SystemManager.Instance().MainForm.InspectionPage.InspectionPanel.ProductInspected(inspectionResult);
//        }
//    }
//}
