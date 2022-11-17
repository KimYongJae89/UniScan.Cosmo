using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data.Vision;

namespace UniScanG.Gravure.Vision.Trainer
{
    public class TrainerParam : AlgorithmParam
    {
        //public DateTime TeachDate
        //{
        //    get { return teachDate; }
        //    set { teachDate = value; }
        //}

        public int BinValue
        {
            get { return binValue; }
            set { binValue = value; }
        }

        public int BinValueOffset
        {
            get { return binValueOffset; }
            set { binValueOffset = value; }
        }

        public int MinPatternArea
        {
            get { return minPatternArea; }
            set { minPatternArea = value; }
        }

        public float SheetPatternGroupThreshold
        {
            get { return sheetPatternGroupThreshold; }
            set { sheetPatternGroupThreshold = value; }
        }

        public Direction SplitLineDirection
        {
            get { return splitLineDirection; }
            set { splitLineDirection = value; }
        }

        public int EdgeAverage
        {
            get { return edgeAverage; }
            set { edgeAverage = value; }
        }

        public Size EdgeDilate
        {
            get { return edgeDilate; }
            set { edgeDilate = value; }
        }

        // System.Windows.Forms.CheckedState
        public bool IsCrisscross
        {
            get { return isCrisscross; }
            set { isCrisscross = value; }
        }

        public int MinLineIntensity
        {
            get { return minLineIntensity; }
            set { minLineIntensity = value; }
        }
        public int KernalSize
        {
            get { return kernalSize; }
            set { kernalSize = value; }
        }
        public int DiffrentialThreshold
        {
            get { return diffrentialThreshold; }
            set { diffrentialThreshold = value; }
        }

        //DateTime teachDate;

        // Find pattern
        private int binValue;
        private int binValueOffset;
        private int minPatternArea;
        private float sheetPatternGroupThreshold;
        private Direction splitLineDirection;

        // Create Edge
        private int edgeAverage;
        private Size edgeDilate;

        // Create Region
        private bool isCrisscross;
        private int minLineIntensity;
        private int kernalSize;
        private int diffrentialThreshold;

        
        public TrainerParam()
        {
            this.binValue = 20;
            this.binValueOffset = 0;
            this.minPatternArea = 20;
            this.sheetPatternGroupThreshold = 10.0f;
            this.splitLineDirection = Direction.Vertical;

            this.edgeAverage = 5;
            this.edgeDilate = new Size(3, 3);

            this.isCrisscross = true;
            this.minLineIntensity = 10;
            this.kernalSize = 50;
            this.diffrentialThreshold = 5;
        }

        public override AlgorithmParam Clone()
        {
            TrainerParam clone = new TrainerParam();

            clone.CopyFrom(this);

            return clone;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            TrainerParam srcParam = (TrainerParam)srcAlgorithmParam;

            this.binValue = srcParam.binValue;
            this.binValueOffset = srcParam.binValueOffset;
            this.minPatternArea = srcParam.minPatternArea;
            this.sheetPatternGroupThreshold = srcParam.sheetPatternGroupThreshold;
            this.splitLineDirection = srcParam.splitLineDirection;
            //this.patternGroupList = new List<SheetPatternGroup>();
            //foreach (SheetPatternGroup sheetPatternGroup in srcParam.patternGroupList)
            //    this.patternGroupList.Add(sheetPatternGroup.Clone());
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            //XmlHelper.SetValue(algorithmElement, "TeachDate", teachDate, "yyyy-MM-dd HH:mm:ss");

            XmlHelper.SetValue(algorithmElement, "BinValue", binValue.ToString());
            XmlHelper.SetValue(algorithmElement, "BinValueOffset", binValueOffset.ToString());
            XmlHelper.SetValue(algorithmElement, "MinPatternArea", minPatternArea.ToString());
            XmlHelper.SetValue(algorithmElement, "SheetPatternGroupThreshold", sheetPatternGroupThreshold.ToString());
            XmlHelper.SetValue(algorithmElement, "SplitLineDirection", splitLineDirection.ToString());

            XmlHelper.SetValue(algorithmElement, "EdgeAverage", this.edgeAverage);
            XmlHelper.SetValue(algorithmElement, "EdgeDilate", this.edgeDilate);

            XmlHelper.SetValue(algorithmElement, "IsCrisscross", isCrisscross);
            XmlHelper.SetValue(algorithmElement, "MinLineIntensity", minLineIntensity);
            XmlHelper.SetValue(algorithmElement, "DiffrentialThreshold", diffrentialThreshold);
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            //teachDate = XmlHelper.GetValue(algorithmElement, "TeachDate", "yyyy-MM-dd HH:mm:ss", teachDate);

            binValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "BinValue", binValue.ToString()));
            binValueOffset = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "BinValueOffset", binValueOffset.ToString()));
            minPatternArea = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinPatternArea", minPatternArea.ToString()));
            sheetPatternGroupThreshold = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "SheetPatternGroupThreshold", sheetPatternGroupThreshold.ToString()));
            splitLineDirection = (Direction)Enum.Parse(typeof(Direction), XmlHelper.GetValue(algorithmElement, "SplitLineDirection", splitLineDirection.ToString()));

            XmlHelper.GetValue(algorithmElement, "EdgeAverage", this.edgeAverage, ref this.edgeAverage);
            XmlHelper.GetValue(algorithmElement, "EdgeDilate", ref this.edgeDilate);

            XmlHelper.GetValue(algorithmElement, "IsCrisscross", isCrisscross, ref isCrisscross);
            XmlHelper.GetValue(algorithmElement, "MinLineIntensity", minLineIntensity, ref minLineIntensity);
            XmlHelper.GetValue(algorithmElement, "DiffrentialThreshold", diffrentialThreshold, ref diffrentialThreshold);
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
    }
}
