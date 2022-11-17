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
using UniScanG.Gravure.Data;

namespace UniScanG.Gravure.Vision.Detector
{
    public class DetectorParam : AlgorithmParam
    {

        int maximumDefectCount;
        int detectThresholdBase;
        int minBlackDefectLength;
        int minWhiteDefectLength;
        bool fineSizeMeasure;
        float fineSizeMeasureThresholdMul;
        int fineSizeMeasureSizeMul;


        bool parallelOperation;

        public int MaximumDefectCount { get => maximumDefectCount; set => maximumDefectCount = value; }
        public int DetectThresholdBase { get => detectThresholdBase; set => detectThresholdBase = value; }
        public int MinBlackDefectLength { get => minBlackDefectLength; set => minBlackDefectLength = value; }
        public int MinWhiteDefectLength { get => minWhiteDefectLength; set => minWhiteDefectLength = value; }
        public bool FineSizeMeasure { get => fineSizeMeasure; set => fineSizeMeasure = value; }
        public float FineSizeMeasureThresholdMul { get => fineSizeMeasureThresholdMul; set => fineSizeMeasureThresholdMul = value; }
        public int FineSizeMeasureSizeMul { get => fineSizeMeasureSizeMul; set => fineSizeMeasureSizeMul = value; }
        public bool ParallelOperation { get => parallelOperation; set => parallelOperation = value; }

        public DetectorParam()
        {
            this.maximumDefectCount = 100;
            this.detectThresholdBase = 25;
            //this.detectThresholdEdge   = 40;
            //this.minBlackDefectLength = 200;
            //this.minWhiteDefectLength = 150;
            this.fineSizeMeasure = true;
            this.fineSizeMeasureThresholdMul = 0.5f;
            this.FineSizeMeasureSizeMul = 10;

            this.parallelOperation = false;
        }

        #region override
        public override AlgorithmParam Clone()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
        #endregion

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "MaximumDefectCount", maximumDefectCount.ToString());
            XmlHelper.SetValue(algorithmElement, "DetectThresholdBase", detectThresholdBase.ToString());
            //XmlHelper.SetValue(algorithmElement, "MinBlackDefectLength", minBlackDefectLength.ToString());
            //XmlHelper.SetValue(algorithmElement, "MinWhiteDefectLength", minWhiteDefectLength.ToString());
            XmlHelper.SetValue(algorithmElement, "FineSizeMeasure", fineSizeMeasure.ToString());
            XmlHelper.SetValue(algorithmElement, "FineSizeMeasureThresholdMul", fineSizeMeasureThresholdMul.ToString());
            XmlHelper.SetValue(algorithmElement, "FineSizeMeasureSizeMul", fineSizeMeasureSizeMul.ToString());
            XmlHelper.SetValue(algorithmElement, "ParallelOperation", parallelOperation.ToString());
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            this.maximumDefectCount = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaximumDefectCount", maximumDefectCount.ToString()));
            this.detectThresholdBase = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "DetectThresholdBase", detectThresholdBase.ToString()));
            //this.minBlackDefectLength = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinBlackDefectLength", minBlackDefectLength.ToString()));
            //this.minWhiteDefectLength = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinWhiteDefectLength", minWhiteDefectLength.ToString()));
            this.fineSizeMeasure = XmlHelper.GetValue(algorithmElement, "FineSizeMeasure", fineSizeMeasure);
            this.fineSizeMeasureThresholdMul = XmlHelper.GetValue(algorithmElement, "FineSizeMeasureThresholdMul", fineSizeMeasureThresholdMul);
            this.fineSizeMeasureSizeMul = XmlHelper.GetValue(algorithmElement, "FineSizeMeasureSizeMul", fineSizeMeasureSizeMul);
            this.parallelOperation = XmlHelper.GetValue(algorithmElement, "ParallelOperation", parallelOperation);

            AlgorithmSetting.Instance().Load();
            this.minBlackDefectLength = AlgorithmSetting.Instance().MinBlackDefectLength;
            this.minWhiteDefectLength = AlgorithmSetting.Instance().MinWhiteDefectLength;
        }
    }
}
