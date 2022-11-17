using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Screen.Data;
using UniScanG.Screen.Vision.Detector;

namespace UniScanG.Inspect
{
    abstract class InspectRunner : UnitBaseInspectRunner
    {
        protected ProcessBufferManager processBufferManager = null;
        protected GrabProcesser grabProcesser = null;
        protected IInspectObserver inspectObserver = null;

        InspectStarter inspectStarter;
        public InspectStarter InspectStarter
        {
            get { return this.inspectStarter; }
            set { this.inspectStarter = value; }
        }

        public ProcessBufferManager ProcessBufferManager
        {
            get { return processBufferManager; }
        }

        public GrabProcesser GrabProcesser
        {
            get { return grabProcesser; }
        }

        public IInspectObserver InspectObserver
        {
            get { return inspectObserver; }
        }

        public InspectRunner() : base()
        {
            this.processBufferManager = new ProcessBufferManager();
            this.inspectStarter = new InspectStarter();
            this.inspectStarter.Start();
        }

        public override void Dispose()
        {
            this.inspectStarter.Stop();
            base.Dispose();
        }

        protected override void SetupUnitManager() { }
        
        protected override InspectionResult BuildInspectionResult(string inspectionNo = null)
        {
            InspectionResult inspectionResult = new InspectionResult();

            inspectionResult.ModelName = SystemManager.Instance().CurrentModel.Name;

            inspectionResult.InspectionTime = new TimeSpan(0);
            inspectionResult.ExportTime = new TimeSpan(0);
            inspectionResult.InspectionStartTime = DateTime.Now;
            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.JobOperator = UserHandler.Instance().CurrentUser.Id;
            inspectionResult.GrabImageList = new List<ImageD>();

            Production productionG = (Production)SystemManager.Instance().ProductionManager.CurProduction;
            lock (productionG)
            {
                inspectionResult.InspectionNo = string.IsNullOrEmpty(inspectionNo) ? productionG.Total.ToString() : inspectionNo;
                inspectionResult.ResultPath = Path.Combine(productionG.GetResultPath(), inspectionResult.InspectionNo);
                productionG.AddTotal();
            }

            return inspectionResult;
        }

        protected string GetResultPath(Production productionG, string inspectionNo)
        {
            return Path.Combine(
                PathSettings.Instance().Result,
                productionG.StartTime.ToString("yy-MM-dd"),
                productionG.Name,
                productionG.Thickness.ToString(),
                productionG.Paste,
                productionG.LotNo,
                string.IsNullOrEmpty(inspectionNo) ? productionG.Total.ToString() : inspectionNo);
        }

        public override void InspectDone(UnitInspectItem unitInspectItem)
        {
            if (unitInspectItem.InspectionResult.AlgorithmResultLDic.ContainsKey(SheetInspector.TypeName) == false)
                return;

            Production production = (Production)SystemManager.Instance().ProductionManager.CurProduction;

            production.Update(((SheetResult)unitInspectItem.InspectionResult.AlgorithmResultLDic[SheetInspector.TypeName]));

            base.InspectDone(unitInspectItem);
        }
    }
}
