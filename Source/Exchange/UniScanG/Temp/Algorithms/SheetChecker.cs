//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.UI;
//using DynMvp.Vision;
//using System;
//using System.Collections.Generic;
//using System.Drawing;

//namespace UniScanG.Temp
//{
//    public enum ProgressType
//    {
//        Start, ImageAlloc, CreateMask,
//        WholeRegionTeach, FindPattern, PatternRegionTeach, WhiteRegionTeach, //MLCC
//        FindFiducial, Average, //Gravure
//        End
//    }

//    public delegate void TeachProgressUpdate(ProgressType type);

//    public class FiducialFinderInspectParam : AlgorithmInspectParam
//    {
//        AlgoImage frameImage;
//        public AlgoImage FrameImage
//        {
//            get { return frameImage; }
//            set { frameImage = value; }
//        }

//        bool updatePatternImage;
//        public bool UpdatePatternImage
//        {
//            get { return updatePatternImage; }
//            set { updatePatternImage = value; }
//        }

//        public FiducialFinderInspectParam(AlgoImage frameImage, bool updatePatternImage, AlgorithmInspectParam param = null) : base(param)
//        {
//            this.frameImage = frameImage;
//            this.updatePatternImage = updatePatternImage;
//        }

//        public override void Dispose()
//        {
//            base.Dispose();
//            frameImage?.Dispose();
//        }
//    }

//    public class TrainerInspectParam : AlgorithmInspectParam
//    {
//        AlgoImage trainImage;
//        public AlgoImage TrainImage
//        {
//            get { return trainImage; }
//            set { trainImage = value; }
//        }

//        TeachProgressUpdate teachProgressUpdate;
//        public TeachProgressUpdate TeachProgressUpdate
//        {
//            get { return teachProgressUpdate; }
//            set { teachProgressUpdate = value; }
//        }

//        public TrainerInspectParam(AlgoImage trainImage, TeachProgressUpdate teachProgressUpdate, AlgorithmInspectParam param = null) : base(param)
//        {
//            this.trainImage = trainImage;
//            this.teachProgressUpdate = teachProgressUpdate;
//        }

//    }

//    internal class SheetCheckerInspectParam : AlgorithmInspectParam
//    {
//        SheetImageSet inspImageSet;
//        public SheetImageSet InspImageSet
//        {
//            get { return inspImageSet; }
//            set { inspImageSet = value; }
//        }

//        ProcessBuffer processBuffer;
//        public ProcessBuffer ProcessBuffer
//        {
//            get { return processBuffer; }
//            set { processBuffer = value; }
//        }

//        SheetCheckerAlgorithmResult sheetCheckerAlgorithmResult;
//        public SheetCheckerAlgorithmResult SheetCheckerAlgorithmResult
//        {
//            get { return sheetCheckerAlgorithmResult; }
//            set { sheetCheckerAlgorithmResult = value; }
//        }

//        InspectionResult inspectionResult;
//        public InspectionResult InspectionResult
//        {
//            get { return inspectionResult; }
//            set { inspectionResult = value; }
//        }

//        public SheetCheckerInspectParam(SheetImageSet inspImageSet, ProcessBuffer processBuffer, InspectionResult inspectionResult, AlgorithmInspectParam param) : base(param)
//        {
//            this.inspImageSet = inspImageSet;
//            this.processBuffer = processBuffer;
//            this.inspectionResult = inspectionResult;
//            this.sheetCheckerAlgorithmResult = new SheetCheckerAlgorithmResult();
//        }

//        public override void Dispose()
//        {
//            base.Dispose();

//            inspImageSet?.Dispose();
//            processBuffer?.Dispose();
//        }

//    }

//    public class GravureSheetChecker : DynMvp.Vision.Algorithm
//    {
//        //SheetCheckerFiducial sheetCheckerFiducial = null;
//        //public SheetCheckerFiducial SheetCheckerFiducial
//        //{
//        //    get { return sheetCheckerFiducial; }
//        //    set { sheetCheckerFiducial = value; }
//        //}

//        //SheetCheckerTrain sheetCheckerTrain = null;
//        //public SheetCheckerTrain SheetCheckerTrain
//        //{
//        //    get { return sheetCheckerTrain; }
//        //    set { sheetCheckerTrain = value; }
//        //}

//        public static string TypeName
//        {
//            get { return "SheetChecker"; }
//        }

//        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
//        {
//            return;
//        }

//        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
//        {
//        }

//        public override DynMvp.Vision.Algorithm Clone()
//        {
//            return null;
//        }

//        public override string GetAlgorithmType()
//        {
//            return TypeName;
//        }

//        public override string GetAlgorithmTypeShort()
//        {
//            throw new NotImplementedException();
//        }

//        public override List<AlgorithmResultValue> GetResultValues()
//        {
//            throw new NotImplementedException();
//        }

//        public override AlgorithmResult CreateAlgorithmResult()
//        {
//            return new SheetCheckerAlgorithmResult(TypeName);
//        }

//        public GravureSheetChecker()
//        {
//            param = new SheetCheckerParam();

//            //SheetCheckerParam sheetCheckerParam = (SheetCheckerParam)param;

//            //sheetCheckerFiducial = new SheetCheckerFiducial(sheetCheckerParam.FiducialParam);
//            //sheetCheckerTrain = new SheetCheckerTrain(sheetCheckerParam.TrainParam);
//        }
        
//        //public bool Train(ImageD trainImage, TeachProgressUpdate teachProgressUpdate, out string message, DebugContext debugContext)
//        //{
//        //    return sheetCheckerTrain.Train(trainImage, teachProgressUpdate, sheetCheckerFiducial, out message, debugContext);
//        //}

//        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
//        {
//            SheetCheckerParam param = (SheetCheckerParam)this.param;

//            SheetCheckerInspectParam inspectParam = algorithmInspectParam as SheetCheckerInspectParam;
//            SheetImageSet inspectImage = inspectParam.InspImageSet;
//            DebugContext debugContext = inspectParam.DebugContext;
//            InspectionResult inspectionResult = inspectParam.InspectionResult;

//            LogHelper.Debug(LoggerType.Inspection, "SheetCheckerV2::InspectGravua Start");

//            SheetCheckerAlgorithmResult sheetCheckerAlgorithmResult = (SheetCheckerAlgorithmResult)CreateAlgorithmResult();
//            sheetCheckerAlgorithmResult.Good = false;

//            ProcessBuffer buffer = null;
//            FiducialFinderInspectParam fiducialFinderInspectParam = null;
//            try
//            {
//                AlgoImage algoImage = inspectImage.GetFullImage();
//                algoImage.Save("SheetImage.bmp", debugContext);

//                // Find Fiducial
//                fiducialFinderInspectParam = new FiducialFinderInspectParam(algoImage, false, algorithmInspectParam);
//                FiducialFinder sheetCheckerFiducial = new FiducialFinder(param);
//                AlgorithmResult fiducialResult = sheetCheckerFiducial.Inspect(fiducialFinderInspectParam);
//                sheetCheckerAlgorithmResult.FiducialFinderResult = fiducialResult;
//                sheetCheckerFiducial.Dispose();
//                //sheetCheckerAlgorithmResult.SpandTime += fiducialResult.SpandTime;

//                if (fiducialResult.Good == false)
//                {
//                    //System.Windows.Forms.MessageBox.Show("Can't find fiducial.");
//                    sheetCheckerAlgorithmResult.Message = "Can't find fiducial.";
//                    return sheetCheckerAlgorithmResult;
//                }

//                // Calculate Step
//                Rectangle fidRect = (Rectangle)fiducialResult.GetResultValue("FidRect").Value;
//                inspectImage.fidXPos = fidRect.Location.X;

//                Size imageSize = inspectImage.GetImageSize();
//                buffer = new ProcessBuffer();
//                buffer.BuildBuffers(this.GetAlgorithmType(), ImageType.Gpu, imageSize.Width, imageSize.Height);
//                inspectParam.ProcessBuffer = buffer;

//                SheetCheckerStep stepCalculate = new SheetCheckerStepCalculate(param);
//                AlgorithmResult calculateStepResult = stepCalculate.Inspect(inspectParam);
//                if (calculateStepResult == null)
//                    return sheetCheckerAlgorithmResult;

//                sheetCheckerAlgorithmResult.CalculateResult = calculateStepResult;
//                sheetCheckerAlgorithmResult.SpandTime += calculateStepResult.SpandTime;
 

//                // Detect Step
//                SheetCheckerStep stepDetect = new SheetCheckerStepDetect(param);
//                AlgorithmResult detectStepResult = stepDetect.Inspect(inspectParam);
//                if (detectStepResult == null)
//                    return sheetCheckerAlgorithmResult;

//                sheetCheckerAlgorithmResult.DetectResult = detectStepResult;
//                sheetCheckerAlgorithmResult.SpandTime += detectStepResult.SpandTime;
//                sheetCheckerAlgorithmResult.SubResultList.AddRange(detectStepResult.SubResultList);
//                inspectionResult.AddProbeResult(new VisionProbeResult(null, sheetCheckerAlgorithmResult, null));
            

//                // Save Step
//                SheetCheckerStep stepsave = new SheetCheckerStepSave(param);
//                AlgorithmResult saveStepResult = stepsave.Inspect(inspectParam);
//                if (saveStepResult == null)
//                    return sheetCheckerAlgorithmResult;

//                sheetCheckerAlgorithmResult.SaveResult = saveStepResult;
//                sheetCheckerAlgorithmResult.SpandTime += saveStepResult.SpandTime;

//                //algorithmResult.SubResultList.AddRange(detectStepResult.SubResultList);
//                sheetCheckerAlgorithmResult.Good = saveStepResult.Good;
//            }
//            finally
//            {
//                // Dispose
//                buffer.Dispose();
//                inspectImage?.Dispose();

//                fiducialFinderInspectParam?.Dispose();
//            }
//            return sheetCheckerAlgorithmResult;
//        }

//        public static List<SheetRange> GetProjectionRangeList(float[] data, int patternHeight, float fillRatio, float width)
//        {
//            List<SheetRange> projectionRangeList = new List<SheetRange>();

//            int startPos = 0;
//            int endPos = 0;

//            bool beConnected = false;

//            for (int i = 0; i < data.Length; i++)
//            {
//                float pixelCount = data[i] / 255.0f;
//                float lineRatio = pixelCount / width;
//                if (lineRatio < fillRatio)
//                {
//                    // 성형을 만나면
//                    if (beConnected == true)
//                    {
//                        // 이전에 전극이면
//                        int height = endPos - startPos;
//                        if (height > patternHeight * 0.9f && height < patternHeight * 1.1f)
//                            projectionRangeList.Add(new SheetRange(RangeType.Horizontal, startPos, endPos));

//                        beConnected = false;
//                    }

//                    startPos = i;
//                }
//                else
//                {
//                    // 전극을 만나면
//                    beConnected = true;
//                    endPos = i;
//                }
//            }

//            if (beConnected == true)
//            {
//                int height = endPos - startPos;
//                if (height > patternHeight * 0.9f && height < patternHeight * 1.1f)
//                    projectionRangeList.Add(new SheetRange(RangeType.Horizontal, startPos, endPos));

//                beConnected = false;
//            }

//            return projectionRangeList;
//        }
//    }
//}
