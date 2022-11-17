//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Drawing;

//using DynMvp.UI;
//using DynMvp.Base;
//using DynMvp.Vision;
//using UniScanG.Operation.Inspect;

//namespace UniScanG.Temp
//{
//    public enum SheetDefectType
//    {
//        BlackDefect, WhiteDefect
//    }

//    public class SheetCheckerSubResult : SubResult
//    {
//        int index;
//        public int Index
//        {
//            get { return index; }
//            set { index = value; }
//        }

//        Image image;
//        public Image Image
//        {
//            get { return image; }
//            set { image = value; }
//        }

//        SheetDefectType defectType;
//        public SheetDefectType DefectType
//        {
//            get { return defectType; }
//            set { defectType = value; }
//        }

//        BlobRect defectBlob;
//        public BlobRect DefectBlob
//        {
//            get { return defectBlob; }
//            set { defectBlob = value; }
//        }

//        float x;
//        public float X
//        {
//            get { return x; }
//            set { x = value; }
//        }

//        float y;
//        public float Y
//        {
//            get { return y; }
//            set { y = value; }
//        }

//        float width;
//        public float Width
//        {
//            get { return width; }
//            set { width = value; }
//        }

//        float height;
//        public float Height
//        {
//            get { return height; }
//            set { height = value; }
//        }

//        int area;
//        public int Area
//        {
//            get { return area; }
//            set { area = value; }
//        }

//        public void BuildMessage()
//        {
//            StringBuilder sb = new StringBuilder();
//            sb.AppendLine(string.Format("Index : {0}", this.index));
//            sb.AppendLine(string.Format("Size [um] : {0:F2} * {1:F2}", width, height));
//            sb.AppendLine(string.Format("Pos [mm] : X{0:F2}, Y{1:F2}", x / 1000, y / 1000));
//            sb.AppendLine(string.Format("Area (px) : {0}", area));
//            this.Message = sb.ToString();
//        }
//    }

//    public enum InspectionError
//    {
//        BlankSheet, DifferenceModel, PatternRegionInvalidParam, WhiteRegionInvalidParam, InfiniteDefect, None, LightOff
//    }

//    public class SheetCheckerAlgorithmResult : AlgorithmResult
//    {
//        InspectionError error = InspectionError.None;
//        public InspectionError Error
//        {
//            get { return error; }
//            set { error = value; }
//        }

//        public SheetCheckerAlgorithmResult(string algorithmName = "SheetChecker") : base(algorithmName)
//        {
//        }

//        AlgorithmResult fiducialFinderResult;
//        public AlgorithmResult FiducialFinderResult
//        {
//            get { return fiducialFinderResult; }
//            set { fiducialFinderResult = value; }
//        }

//        AlgorithmResult calculateResult;
//        public AlgorithmResult CalculateResult
//        {
//            get { return calculateResult; }
//            set { calculateResult = value; }
//        }

//        AlgorithmResult detectResult;
//        public AlgorithmResult DetectResult
//        {
//            get { return detectResult; }
//            set { detectResult = value; }
//        }

//        AlgorithmResult saveResult;
//        public AlgorithmResult SaveResult
//        {
//            get { return saveResult; }
//            set { saveResult = value; }
//        }

//        public override void AppendResultFigures(FigureGroup figureGroup, PointF offset)
//        {
//            Figure figure = (Figure)this.ResultFigureGroup.Clone();

//            figure.Offset(offset.X, offset.Y);
//            figureGroup.AddFigure(figure);

//            FigureGroup algorithmResultFigure = new FigureGroup();
//            foreach (SheetCheckerSubResult sheetCheckerSubResult in subResultList)
//            {
//                if (sheetCheckerSubResult == null)
//                    continue;

//                Figure subFigure = UniScanGUtil.Instance().GetDefectFigure(sheetCheckerSubResult.DefectType, sheetCheckerSubResult.ResultRect.ToRectangle());
                
//                subFigure.Offset(offset.X, offset.Y);
//                algorithmResultFigure.AddFigure(subFigure);
//            }

//            figureGroup.AppendFigure(algorithmResultFigure);
//        }
//    }
//}
