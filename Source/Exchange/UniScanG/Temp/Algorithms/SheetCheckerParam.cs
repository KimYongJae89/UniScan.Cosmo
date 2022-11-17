//using System.Xml;
//using DynMvp.Vision;
//using DynMvp.Base;
//using System;
//using System.Drawing;
//using System.Drawing.Imaging;
//using System.Collections.Generic;

//namespace UniScanG.Temp
//{
//    public class SheetCheckerParam : AlgorithmParam
//    {
//        SizeF sheetSizeMm = new SizeF();
//        public SizeF SheetSizeMm
//        {
//            get { return sheetSizeMm; }
//            set { sheetSizeMm = value; }
//        }

//        int maximumDefectCount = 1000;
//        public int MaximumDefectCount
//        {
//            get { return maximumDefectCount; }
//            set { maximumDefectCount = value; }
//        }

//        SizeF minDefectSizeMm = new SizeF();
//        public SizeF MinDefectSizeMm
//        {
//            get { return minDefectSizeMm; }
//            set { minDefectSizeMm = value; }
//        }

//        private FiducialFinderParam fiducialFinderParam = new FiducialFinderParam();
//        public FiducialFinderParam FiducialFinderParam
//        {
//            get { return fiducialFinderParam; }
//            set { fiducialFinderParam = value; }
//        }

//        private TrainerParam trainerParam = new TrainerParam();
//        public TrainerParam TrainerParam
//        {
//            get { return trainerParam; }
//            set { trainerParam = value; }
//        }

//        private bool adaptiveFidSearch = false;
//        public bool AdaptiveFidSearchRange
//        {
//            get { return adaptiveFidSearch; }
//            set { adaptiveFidSearch = value; }
//        }

//        public Size GetSheetSizePx()
//        {
//            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList[0];

//            int width = (int)Math.Round((this.sheetSizeMm.Width * 1000) / calibration.PelSize.Width);
//            if (width == 0)
//                width = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0).ImageSize.Width;

//            int height = (int)Math.Round((this.sheetSizeMm.Height * 1000) / calibration.PelSize.Height);
//            if (height == 0)
//                height = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0).ImageSize.Height;

//            return new Size(width, height);
//        }

//        public override AlgorithmParam Clone()
//        {
//            SheetCheckerParam newParam = new SheetCheckerParam();
//            newParam.FiducialFinderParam = (FiducialFinderParam)this.fiducialFinderParam.Clone();
//            newParam.TrainerParam = (TrainerParam)this.trainerParam.Clone();

//            return newParam;
//        }

//        public override void Copy(AlgorithmParam srcAlgorithmParam)
//        {
//            base.Copy(srcAlgorithmParam);

//            SheetCheckerParam copyParam = (SheetCheckerParam)srcAlgorithmParam;
//            fiducialFinderParam.Copy(copyParam.FiducialFinderParam);
//            trainerParam.Copy(copyParam.trainerParam);
//        }

//        public override void SyncParam(AlgorithmParam srcAlgorithmParam)
//        {
//            base.Copy(srcAlgorithmParam);

//            SheetCheckerParam syncParam = (SheetCheckerParam)srcAlgorithmParam;
//            fiducialFinderParam.SyncParam(syncParam.FiducialFinderParam);
//            trainerParam.SyncParam(syncParam.trainerParam);
//        }

//        public override void Clear()
//        {
//            base.Clear();

//            fiducialFinderParam.Clear();
//            trainerParam.Clear();
//        }

//        public override void LoadParam(XmlElement paramElement)
//        {
//            base.LoadParam(paramElement);

//            string[] sheetSize = XmlHelper.GetValue(paramElement, "SheetSize", "0/0").Split('/');
//            float sheetWidth = float.Parse(sheetSize[0]);
//            float sheetHeight= float.Parse(sheetSize[1]);
//            this.sheetSizeMm = new SizeF(sheetWidth, sheetHeight);

//            adaptiveFidSearch =Convert.ToBoolean(XmlHelper.GetValue(paramElement, "AdaptiveFidSearch", adaptiveFidSearch.ToString()));

//            XmlElement trainParamElement = paramElement["TrainParam"];
//            if (trainParamElement != null)
//                trainerParam.LoadParam(trainParamElement);
            
//            XmlElement fiducialParamElement = paramElement["FiducialParam"];
//            if (fiducialParamElement != null)
//                fiducialFinderParam.LoadParam(fiducialParamElement);
//        }

//        public override void SaveParam(XmlElement paramElement)
//        {
//            base.SaveParam(paramElement);

//            XmlHelper.SetValue(paramElement, "SheetSize", string.Format("{0}/{1}", sheetSizeMm.Width, sheetSizeMm.Height));
//            XmlHelper.SetValue(paramElement, "AdaptiveFidSearch", adaptiveFidSearch.ToString());

//            XmlElement trainParamElement = paramElement.OwnerDocument.CreateElement("TrainParam");
//            paramElement.AppendChild(trainParamElement);
//            trainerParam.SaveParam(trainParamElement);

//            XmlElement fiducialParamElement = paramElement.OwnerDocument.CreateElement("FiducialParam");
//            paramElement.AppendChild(fiducialParamElement);
//            fiducialFinderParam.SaveParam(fiducialParamElement);
//        }
//    }

//    public class FiducialFinderParam : AlgorithmParam
//    {
//        int fidSizeWidth = 500;
//        public int FidSizeWidth
//        {
//            get { return fidSizeWidth; }
//            set { fidSizeWidth = value; }
//        }

//        int fidSizeHeight = 150;
//        public int FidSizeHeight
//        {
//            get { return fidSizeHeight; }
//            set { fidSizeHeight = value; }
//        }

//        float fidSearchLBound;
//        public float FidSearchLBound
//        {
//            get { return fidSearchLBound; }
//            set { fidSearchLBound = value; }
//        }

//        float fidSearchRBound;
//        public float FidSearchRBound
//        {
//            get { return fidSearchRBound; }
//            set { fidSearchRBound = value; }
//        }

//        Image2D fiducialPatternEdgeImage;
//        public Image2D FiducialPatternEdgeImage
//        {
//            get { return fiducialPatternEdgeImage; }
//            set { fiducialPatternEdgeImage = value; }
//        }

//        public FiducialFinderParam()
//        {
//            //fidPosition = (UniScanGSettings.Instance().InspectorInfo.CamIndex == 0 ? FiducialPosition.Left : FiducialPosition.Right);
//            //fidSearchLBound = (fidPosition == FiducialPosition.Left ? 0.00f : 0.92f);
//            //fidSearchRBound = (fidPosition == FiducialPosition.Left ? 0.05f : 0.98f);
//        }

//        ~FiducialFinderParam()
//        {
//            Clear();
//        }

//        public override AlgorithmParam Clone()
//        {
//            FiducialFinderParam param = new FiducialFinderParam();

//            param.Copy(this);

//            return param;
//        }

//        public override void Copy(AlgorithmParam srcAlgorithmParam)
//        {
//            FiducialFinderParam srcParam = srcAlgorithmParam as FiducialFinderParam;

//            base.Copy(srcParam);

//            fidSizeWidth = srcParam.fidSizeWidth;
//            fidSizeHeight = srcParam.fidSizeHeight;

//            fidSearchLBound = srcParam.fidSearchLBound;
//            fidSearchRBound = srcParam.fidSearchRBound;

//            fiducialPatternEdgeImage = (Image2D)srcParam.fiducialPatternEdgeImage?.Clone();
//        }

//        public override void SyncParam(AlgorithmParam srcAlgorithmParam)
//        {
//            Copy(srcAlgorithmParam);
//        }

//        public override void Clear()
//        {
//            base.Clear();
//            if (fiducialPatternEdgeImage != null)
//                fiducialPatternEdgeImage.Dispose();
//            fiducialPatternEdgeImage = null;
//        }

//        public override void LoadParam(XmlElement paramElement)
//        {
//            base.LoadParam(paramElement);

//            //float sheetWidth = Convert.ToSingle(XmlHelper.GetValue(paramElement, "SheetWidth", sheetSizeMm.Width.ToString()));
//            //float sheetHeigth = Convert.ToSingle(XmlHelper.GetValue(paramElement, "SheetHeight", sheetSizeMm.Height.ToString()));
//            //this.sheetSizeMm = new SizeF(sheetWidth, sheetHeigth);

//            //fidPosition = (FiducialPosition)Enum.Parse(typeof(FiducialPosition), XmlHelper.GetValue(paramElement, "FidPosition", fidPosition.ToString()));

//            this.fidSearchLBound = Convert.ToSingle(XmlHelper.GetValue(paramElement, "FidSearchLBound", fidSearchLBound.ToString()));
//            this.fidSearchRBound = Convert.ToSingle(XmlHelper.GetValue(paramElement, "FidSearchRBound", fidSearchRBound.ToString()));

//            this.fidSizeWidth = Convert.ToInt32(XmlHelper.GetValue(paramElement, "FidSizeWidth", fidSizeWidth.ToString()));
//            this.fidSizeHeight = Convert.ToInt32(XmlHelper.GetValue(paramElement, "FidSizeHeight", fidSizeHeight.ToString()));
//            //this.adaptiveFidSearchRange = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "AdaptiveFidSearchRange", adaptiveFidSearchRange.ToString()));

//            string fiducialPatternEdgeBitmapString = XmlHelper.GetValue(paramElement, "FiducialPatternEdgeImage", "");
//            if (string.IsNullOrEmpty(fiducialPatternEdgeBitmapString) == false)
//                fiducialPatternEdgeImage = Image2D.ToImage2D((Bitmap)ImageHelper.Base64StringToImage(fiducialPatternEdgeBitmapString, ImageFormat.Bmp));
//        }

//        public override void SaveParam(XmlElement paramElement)
//        {
//            base.SaveParam(paramElement);

//            //XmlHelper.SetValue(paramElement, "SheetWidth", sheetSizeMm.Width.ToString());
//            //XmlHelper.SetValue(paramElement, "SheetHeight", sheetSizeMm.Height.ToString());


//            XmlHelper.SetValue(paramElement, "FidSearchLBound", fidSearchLBound.ToString());
//            XmlHelper.SetValue(paramElement, "FidSearchRBound", fidSearchRBound.ToString());


//            XmlHelper.SetValue(paramElement, "FidSizeWidth", fidSizeWidth.ToString());
//            XmlHelper.SetValue(paramElement, "FidSizeHeight", fidSizeHeight.ToString());

//            if (fiducialPatternEdgeImage != null)
//            {
//                string imageString = ImageHelper.ImageToBase64String(fiducialPatternEdgeImage.ToBitmap(), ImageFormat.Bmp);
//                XmlHelper.SetValue(paramElement, "FiducialPatternEdgeImage", imageString);
//            }
//        }
//    }

//    public class TrainerParam : AlgorithmParam
//    {
//        //GravuaSheetImageSet sheetImageSet;
//        //public GravuaSheetImageSet SheetImageSet
//        //{
//        //    get { return sheetImageSet; }
//        //    set { sheetImageSet = value; }
//        //}
//        FiducialPosition fidPosition = FiducialPosition.Left;
//        public FiducialPosition FidPosition
//        {
//            get { return fidPosition; }
//            set { fidPosition = value; }
//        }

//        protected SheetPattern refPattern = new SheetPattern();
//        public SheetPattern RefPattern
//        {
//            get { return refPattern; }
//            set { refPattern = value; }
//        }
        
//        int defectThreshold = 20;
//        public int DefectThreshold
//        {
//            get { return defectThreshold; }
//            set { defectThreshold = value; }
//        }

//        int blackDefectMinArea;
//        public int BlackDefectMinArea
//        {
//            get { return blackDefectMinArea; }
//            set { blackDefectMinArea = value; }
//        }

//        int whiteDefectMinArea;
//        public int WhiteDefectMinArea
//        {
//            get { return whiteDefectMinArea; }
//            set { whiteDefectMinArea = value; }
//        }

//        bool useSIMD;
//        public bool UseSIMD
//        {
//            get { return useSIMD; }
//            set { useSIMD = value; }
//        }

//        protected bool traind = false;
//        public bool Traind
//        {
//            get { return traind; }
//            set { traind = value; }
//        }

//        protected int minPatternArea;
//        public int MinPatternArea
//        {
//            get { return minPatternArea; }
//            set { minPatternArea = value; }
//        }

//        protected float patternGroupThreshold;
//        public float PatternGroupThreshold
//        {
//            get { return patternGroupThreshold; }
//            set { patternGroupThreshold = value; }
//        }

//        protected List<SheetPattern> patternList = new List<SheetPattern>();
//        public List<SheetPattern> PatternList
//        {
//            get { return patternList; }
//            set { patternList = value; }
//        }

//        int autoThresholdValue = 0;
//        public int AutoThresholdValue
//        {
//            get { return autoThresholdValue; }
//            set { autoThresholdValue = value; }
//        }

//        Image2D refferenceImage;
//        public Image2D RefferenceImage
//        {
//            get { return refferenceImage; }
//            set { refferenceImage = value; }
//        }

//        Image2D inspectRegionInfoImage;
//        public Image2D InspectRegionInfoImage
//        {
//            get { return inspectRegionInfoImage; }
//            set { inspectRegionInfoImage = value; }
//        }

//        int fiducialXPos = 0;
//        public int FiducialXPos
//        {
//            get { return fiducialXPos; }
//            set { fiducialXPos = value; }
//        }
//        List<ProjectionRegion> projectionRegionList = null;
//        public List<ProjectionRegion> ProjectionRegionList
//        {
//            get { return projectionRegionList; }
//            set { projectionRegionList = value; }
//        }

//        public TrainerParam()
//        {
//            traind = false;

//            minPatternArea = 250;

//            patternGroupThreshold = UniScanGSettings.Instance().PatternGroupTheshold;
//            patternGroupThreshold = 1;

//            fiducialXPos = 0;

//            blackDefectMinArea = 40;
//            whiteDefectMinArea = 40;

//            defectThreshold = 20;

//            projectionRegionList = new List<ProjectionRegion>();
//        }

//        ~TrainerParam()
//        {
//            //refferenceImage?.Dispose();
//            inspectRegionInfoImage?.Dispose();
//        }

//        public override AlgorithmParam Clone()
//        {
//            TrainerParam param = new TrainerParam
//            {
//                fiducialXPos = this.fiducialXPos,

//                blackDefectMinArea = this.blackDefectMinArea,
//                whiteDefectMinArea = this.whiteDefectMinArea,
//                defectThreshold = this.defectThreshold,

//                refferenceImage = (Image2D)this.refferenceImage?.Clone(),
//                inspectRegionInfoImage = (Image2D)this.inspectRegionInfoImage?.Clone(),

//                projectionRegionList = new List<ProjectionRegion>(this.projectionRegionList)
//            };

//            return param;
//        }

//        public override void Copy(AlgorithmParam srcAlgorithmParam)
//        {
//            base.Copy(srcAlgorithmParam);
//        }

//        public override void SyncParam(AlgorithmParam srcAlgorithmParam)
//        {
//            base.Copy(srcAlgorithmParam);

//            TrainerParam param = (TrainerParam)srcAlgorithmParam;
//        }

//        public override void Clear()
//        {
//            base.Clear();

//            //refferenceImage?.Dispose();
//            inspectRegionInfoImage?.Dispose();
//        }

//        public override void LoadParam(XmlElement paramElement)
//        {
//            base.LoadParam(paramElement);

//            traind = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "Traind", "false"));
//            minPatternArea = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MinPatternArea", "250"));
//            patternGroupThreshold = Convert.ToSingle(XmlHelper.GetValue(paramElement, "PatternGroupThreshold", "15"));

//            fiducialXPos = Convert.ToInt32(XmlHelper.GetValue(paramElement, "FiducialXPos", "0"));

//            blackDefectMinArea = Convert.ToInt32(XmlHelper.GetValue(paramElement, "BlackDefectMinArea", "40"));
//            whiteDefectMinArea = Convert.ToInt32(XmlHelper.GetValue(paramElement, "WhiteDefectMinArea", "40"));
//            defectThreshold = Convert.ToInt32(XmlHelper.GetValue(paramElement, "DefectThreshold", "20"));
//            autoThresholdValue = Convert.ToInt32(XmlHelper.GetValue(paramElement, "AutoThresholdValue", "0"));

//            fidPosition = (FiducialPosition)Enum.Parse(typeof(FiducialPosition), XmlHelper.GetValue(paramElement, "FidPosition", FiducialPosition.Left.ToString()));

//            string refferenceString = XmlHelper.GetValue(paramElement, "RefferenceImage", "");
//            if (string.IsNullOrEmpty(refferenceString) == false)
//                refferenceImage = Image2D.ToImage2D((Bitmap)ImageHelper.Base64StringToImage(refferenceString, ImageFormat.Bmp));

//            string projectionBitmapString = XmlHelper.GetValue(paramElement, "ProjectionImage", "");
//            if (string.IsNullOrEmpty(projectionBitmapString) == false)
//                inspectRegionInfoImage = Image2D.ToImage2D((Bitmap)ImageHelper.Base64StringToImage(projectionBitmapString, ImageFormat.Bmp));

//            XmlElement projectionRegionListElement = paramElement["ProjectionRegionList"];
//            if (projectionRegionListElement != null)
//            {
//                foreach (XmlElement projectionRegionElement in projectionRegionListElement)
//                {
//                    if (projectionRegionElement.Name != "ProjectionRegion")
//                        continue;

//                    ProjectionRegion projectionRegion = new ProjectionRegion(-1);
//                    projectionRegion.Load(projectionRegionElement);

//                    projectionRegionList.Add(projectionRegion);
//                }
//            }

//            XmlElement patternParamElement = paramElement["PatternParam"];
//            if (patternParamElement != null)
//                LoadPatternList(patternParamElement);
//        }
//        public override void SaveParam(XmlElement paramElement)
//        {
//            base.SaveParam(paramElement);

//            XmlHelper.SetValue(paramElement, "Traind", traind.ToString());
//            XmlHelper.SetValue(paramElement, "MinPatternArea", minPatternArea.ToString());
//            XmlHelper.SetValue(paramElement, "PatternGroupThreshold", patternGroupThreshold.ToString());

//            XmlHelper.SetValue(paramElement, "FiducialXPos", fiducialXPos.ToString());

//            XmlHelper.SetValue(paramElement, "BlackDefectMinArea", blackDefectMinArea.ToString());
//            XmlHelper.SetValue(paramElement, "WhiteDefectMinArea", whiteDefectMinArea.ToString());
//            XmlHelper.SetValue(paramElement, "DefectThreshold", defectThreshold.ToString());
//            XmlHelper.SetValue(paramElement, "AutoThresholdValue", autoThresholdValue.ToString());

//            XmlHelper.SetValue(paramElement, "FidPosition", fidPosition.ToString());

//            if (refferenceImage != null)
//            {
//                string imageString = ImageHelper.ImageToBase64String(refferenceImage.ToBitmap(), ImageFormat.Bmp);
//                XmlHelper.SetValue(paramElement, "RefferenceImage", imageString);
//            }
//            if (inspectRegionInfoImage != null)
//            {
//                string imageString = ImageHelper.ImageToBase64String(inspectRegionInfoImage.ToBitmap(), ImageFormat.Bmp);
//                XmlHelper.SetValue(paramElement, "ProjectionImage", imageString);
//            }

//            XmlElement projectionRegionListElement = paramElement.OwnerDocument.CreateElement("ProjectionRegionList");
//            paramElement.AppendChild(projectionRegionListElement);
//            foreach (ProjectionRegion projectionRegion in projectionRegionList)
//            {
//                XmlElement projectionRegionElement = paramElement.OwnerDocument.CreateElement("ProjectionRegion");
//                projectionRegionListElement.AppendChild(projectionRegionElement);

//                projectionRegion.Save(projectionRegionElement);
//            }

//            XmlElement patternParamElement = paramElement.OwnerDocument.CreateElement("PatternParam");
//            paramElement.AppendChild(patternParamElement);
//            SavePatternList(patternParamElement);
//        }

//        public void LoadPatternList(XmlElement paramElement)
//        {
//            XmlElement refPatternElement = paramElement["RefPattern"];
//            if (refPatternElement != null)
//                refPattern.LoadParam(refPatternElement);

//            XmlNodeList patternElementList = paramElement.GetElementsByTagName("Pattern");
//            foreach (XmlElement patternElement in patternElementList)
//            {
//                SheetPattern pattern = new SheetPattern();
//                pattern.LoadParam(patternElement);
//                patternList.Add(pattern);
//            }
//        }

//        public void SavePatternList(XmlElement patternParamElement)
//        {
//            XmlElement refPatternElement = patternParamElement.OwnerDocument.CreateElement("RefPattern");
//            patternParamElement.AppendChild(refPatternElement);
//            refPattern.SaveParam(refPatternElement);

//            foreach (SheetPattern pattern in patternList)
//            {
//                XmlElement patternElement = patternParamElement.OwnerDocument.CreateElement("Pattern");
//                patternParamElement.AppendChild(patternElement);

//                pattern.SaveParam(patternElement);
//            }
//        }
//    }
//}
