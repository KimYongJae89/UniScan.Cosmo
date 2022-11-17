//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DynMvp.InspData;
//using DynMvp.Base;
//using DynMvp.Data;
//using System.Drawing;
//using UniScanG.Operation.Inspect;

//namespace UniScanG.Temp
//{
//    public struct SimpleInspectionResult
//    {
//        int sheetNo;
//        public int SheetNo { get { return sheetNo; } }
//        int defectCount;
//        public int DefectCount { get { return defectCount; } }
//        DateTime inspStartTime;
//        public DateTime InspTime { get { return inspStartTime; } }
//        string resultCsv;
//        public string ResultCsv { get { return resultCsv; } }
//        InspectionResult inspectionResult;
//        public InspectionResult InspectionResult { get { return inspectionResult; } }

//        public SimpleInspectionResult(int sheetNo, int defectCount, DateTime inspStartTime, InspectionResult inspectionResult, string resultCsv)
//        {
//            this.sheetNo = sheetNo;
//            this.defectCount = defectCount;
//            this.inspStartTime = inspStartTime;
//            this.inspectionResult = inspectionResult;
//            this.resultCsv = resultCsv;
//        }
//    }

//    public class InspectionResult : DynMvp.InspData.InspectionResult
//    {
//        string monitorResultPath;
//        public string MonitorResultPath
//        {
//            get { return monitorResultPath; }
//            set { monitorResultPath = value; }
//        }

//        int blackDefectNum;
//        public int BlackDefectNum
//        {
//            get { return blackDefectNum; }
//        }

//        int whiteDefectNum;
//        public int WhiteDefectNum
//        {
//            get { return whiteDefectNum; }
//        }
        
//        Bitmap wholeImage;
//        public Bitmap WholeImage
//        {
//            get { return wholeImage; }
//            set { wholeImage = value; }
//        }

//        public override void Clear(bool clearImage = true)
//        {
//            base.Clear(clearImage);
            
//            if (clearImage)
//            {
//                foreach (ImageD grabImage in grabImageList)
//                {
//                    if (grabImage != null)
//                        grabImage.Dispose();
//                }
//            }

//            if (imageDisposible)
//            {
//                /*foreach (TargetImage targetImage in targetImageList)
//                {
//                    targetImage.Clear();
//                }*/

//                targetImageList.Clear();
//            }
//        }

//        public void CalcDefectTypeCount()
//        {
//            blackDefectNum = 0;
//            whiteDefectNum = 0;

//            foreach (ProbeResult probeResult in probeResultList)
//            {
//                VisionProbeResult visionResult = (VisionProbeResult)probeResult;
//                foreach (Data.SheetCheckerSubResult subResult in visionResult.AlgorithmResult.SubResultList)
//                {
//                    if (subResult == null)
//                        continue;

//                    switch (subResult.DefectType)
//                    {
//                        case Data.SheetDefectType.BlackDefect:
//                            blackDefectNum++;
//                            break;
//                        case Data.SheetDefectType.WhiteDefect:
//                            whiteDefectNum++;
//                            break;
//                    }
//                }
//            }
//        }

//    }
//}
