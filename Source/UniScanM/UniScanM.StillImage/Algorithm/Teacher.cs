using DynMvp.Vision;
using UniScanM.StillImage.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using DynMvp.Base;

namespace UniScanM.StillImage.Algorithm
{
    public abstract class Teacher 
    {
        protected FeatureExtractor featureExtractor = null;

        public static Teacher Create(int version)
        {
            switch (version)
            {
                case 0:
                    return new TeacherV1();
            }
            return null;
        }
        
        public abstract void Teach(AlgoImage algoImage, InspectionResult inspectionResult);
    }

    public class TeacherV1 : Teacher
    {
        public TeacherV1()
        {
            this.featureExtractor = FeatureExtractor.Create(0);
        }

        public override void Teach(AlgoImage sheetImage, InspectionResult inspectionResult)
        {
            TeachData teachData = featureExtractor.Extract(sheetImage);
            Rectangle imageRect = new Rectangle(Point.Empty, sheetImage.Size);
            if (teachData.TeachDone)
            {
                PatternInfoGroup majorPatternInfoGroup = teachData.PatternInfoGroupList.Find(f => f.TeachInfo.Inspectable);
                if (majorPatternInfoGroup != null)
                {
                    float inspSizeW = Math.Min(imageRect.Width, (float)majorPatternInfoGroup.ShapeInfoList.Average(f => f.BaseRect.Width) * 3);
                    float inspSizeH = Math.Min(imageRect.Height, (float)majorPatternInfoGroup.ShapeInfoList.Average(f => f.BaseRect.Height) * 3);
                    float size = Math.Max(inspSizeW, inspSizeH);
                    Size fovSize = Size.Round(new SizeF(size, size));
                    teachData.InspSize = fovSize;

                    Rectangle inspRect = SheetFinder.GetInspRect(sheetImage.Size, fovSize);

                    ShapeInfo centerShapeInfo = majorPatternInfoGroup.GetCenterShapeInfo(inspRect);

                    //int sameNeighbor = centerShapeInfo.Neighborhood.Count(f => ShapeInfo.IsSimilar(centerShapeInfo, f, 0.8f));
                    //if (sameNeighbor == 4)
                    //    teachData.IsInspectable = true;

                    bool left = ShapeInfo.IsSimilar(centerShapeInfo, centerShapeInfo.Neighborhood[0], 0.8f);
                    bool right = ShapeInfo.IsSimilar(centerShapeInfo, centerShapeInfo.Neighborhood[2], 0.8f);
                    teachData.IsInspectable = left && right;

                    if (centerShapeInfo != null)
                    {
                        Point newCenterPt = Point.Round(DrawingHelper.CenterPoint(centerShapeInfo.BaseRect));
                        inspRect = Rectangle.Intersect(imageRect, DrawingHelper.FromCenterSize(newCenterPt, fovSize));
                    }
                    inspectionResult.InspRectInSheet = inspRect;

                    AlgoImage displayImage = sheetImage.GetSubImage(inspRect);
                    inspectionResult.DisplayBitmap = displayImage.ToImageD().ToBitmap();
                    displayImage.Dispose();

                    inspectionResult.SetGood();
                }
            }
            
            if(inspectionResult.InspRectInSheet.IsEmpty)
            {
                Point clipRectCenter = SheetFinder.GetInspCenter(sheetImage.Size);
                int aSize = Math.Min(sheetImage.Width, sheetImage.Height);
                Rectangle clipRect = Rectangle.Round(DrawingHelper.FromCenterSize(clipRectCenter, new Size(aSize, aSize)));
                clipRect.Intersect(imageRect);
                inspectionResult.InspRectInSheet = clipRect;

                AlgoImage displayImage = sheetImage.GetSubImage(clipRect);
                inspectionResult.DisplayBitmap = displayImage.ToImageD().ToBitmap();
                displayImage.Dispose();

                inspectionResult.SetDefect();
            }

            inspectionResult.TeachData = teachData;
        }
    }
}
