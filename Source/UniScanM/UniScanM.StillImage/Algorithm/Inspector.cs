using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;
using UniScanM.StillImage.Data;
using UniScanM.Algorithm;
using System.Drawing;
using DynMvp.Base;

namespace UniScanM.StillImage.Algorithm
{
    public abstract class Inspector
    {
        public static Inspector Create(int version)
        {
            switch (version)
            {
                case 0:
                    return new InspectorV1();
                default:
                    throw new Exception("Version is invalid");
            }
        }
        public abstract void Inspect(AlgoImage fovImage, InspectParam inspectParam, InspectionResult inspectionResult);

    }

    public class InspectorV1 : Inspector
    {
        protected FeatureExtractor featureExtractor = null;

        public InspectorV1() : base()
        {
            this.featureExtractor = FeatureExtractor.Create(0);
        }

        public override void Inspect(AlgoImage inspImage, InspectParam inspectParam, InspectionResult inspectionResult)
        {
            inspectionResult.SetGood();
            TeachData teachData = inspectionResult.TeachData;
            List<MatchResult> matchResultList = featureExtractor.Match(inspImage, teachData, inspectParam.MatchRatio);

            List<ProcessResult> processResults = new List<ProcessResult>();
            List<Rectangle> defectResults = new List<Rectangle>();
            foreach (MatchResult matchResult in matchResultList)
            {
                PatternInfo inspPatternInfo = matchResult.InspPatternInfo;
                PatternInfo refPatternInfo = null;
                if (matchResult.RefPatternInfo != null)
                // 티칭된 패턴 -> 패턴 Margin, Blot 검사
                {
                    refPatternInfo = matchResult.RefPatternInfo;
                    if (refPatternInfo.TeachInfo.Inspectable == false)
                        continue;

                    ProcessResult patternResult = new ProcessResult(refPatternInfo, inspPatternInfo, inspectParam);
                    processResults.Add(patternResult);
                }
                else
                // 티칭되지 않은 패턴 -> 이물(Defect) 검사
                {
                    defectResults.Add(inspPatternInfo.ShapeInfo.BaseRect);
                }
            }
            ProcessResultList processResultList = new ProcessResultList(inspImage.ToImageD());
            processResultList.ResultList.AddRange(processResults);
            processResultList.DefectRectList.AddRange(defectResults);

            // 중앙에 가장 가까운 검사 패턴 찾기
            List<PatternInfo> list = processResults.ConvertAll<PatternInfo>(f => f.InspPatternInfo);
            PatternInfoGroup patternInfoGroup = new PatternInfoGroup(-1, list);
            ShapeOfInterest shapeOfInterest = patternInfoGroup.GetShapeOfInterest(new Rectangle(Point.Empty, inspImage.Size), false);

            if (shapeOfInterest.IsEmpty == false)
            {
                int ii = processResultList.ResultList.FindIndex(f => f.InspPatternInfo.ShapeInfo.Id == shapeOfInterest.ShapeInfo.Id);
                processResultList.InterestResultId = ii;

                PointF centerPt = DrawingHelper.CenterPoint(shapeOfInterest.ShapeInfo.BaseRect);
                float maxSizeW2 = Math.Min(inspImage.Width - centerPt.X, centerPt.X);
                float maxSizeH2 = Math.Min(inspImage.Height - centerPt.Y, centerPt.Y);
                SizeF maxSize = new SizeF(maxSizeW2 * 2, maxSizeH2 * 2);

                if (inspectParam.MaxDefectSize > 0)
                    processResultList.DefectRectList.RemoveAll(f => (f.Width * f.Height) > inspectParam.MaxDefectSize);

            }

            //fovImage.Save(@"d:\temp\temp.bmp");
            inspectionResult.ProcessResultList = processResultList;
            inspectionResult.UpdateJudgement();
        }
    }
}
