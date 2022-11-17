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

namespace UniScanG.Gravure.Vision.Calculator
{
    public class EdgeParam
    {
        public int EdgeValue { get => edgeValue;}
        int edgeValue;

        public int EdgeWidth { get => edgeWidth;}
        int edgeWidth;

        public CalculatorParam.EEdgeFindMethod EdgeFindMethod { get => edgeFindMethod; }
        CalculatorParam.EEdgeFindMethod edgeFindMethod;

        public EdgeParam(int edgeValue, int edgeWidth, CalculatorParam.EEdgeFindMethod edgeFindMethod)
        {
            this.edgeValue = edgeValue;
            this.edgeWidth = edgeWidth;
            this.edgeFindMethod = edgeFindMethod;
        }

        public void SaveParam(XmlElement algorithmElement)
        {

        }

        public void LoadParam(XmlElement algorithmElement)
        {

        }
    }

    public class LineSetParam
    {
        public bool AdaptivePairing { get => adaptivePairing; }
        bool adaptivePairing;

        public int BoundaryPairStep { get => boundaryPairStep;}
        int boundaryPairStep;

        public CalculatorParam.EIgnoreMethod IgnoreMethod { get => ignoreMethod; }
        CalculatorParam.EIgnoreMethod ignoreMethod;

        public bool IgnoreSideLine { get => ignoreSideLine; }
        bool ignoreSideLine;

        public LineSetParam(bool adaptivePairing, int boundaryPairStep, CalculatorParam.EIgnoreMethod ignoreMethod, bool ignoreSideLine)
        {
            this.adaptivePairing = adaptivePairing;
            this.boundaryPairStep = boundaryPairStep;
            this.ignoreMethod = ignoreMethod;
            this.ignoreSideLine = ignoreSideLine;
        }
        public void SaveParam(XmlElement algorithmElement)
        {

        }

        public void LoadParam(XmlElement algorithmElement)
        {

        }
    }


    public class CalculatorParam : AlgorithmParam
    {
        public enum EEdgeFindMethod { Projection, Soble }
        public enum EIgnoreMethod { Basic, Neighborhood }

        int binValue = 0;
        Point basePosition = Point.Empty;
        Size sheetSize = Size.Empty;
        List<SheetPatternGroup> patternGroupList = null;
        List<RegionInfoG> regionInfoList = null;

        int edgeValue;
        int edgeWidth;
        EEdgeFindMethod edgeFindMethod;

        bool inBarAlignY;
        bool inBarAlignX;

        bool adaptivePairing;
        int boundaryPairStep;
        EIgnoreMethod ignoreMethod;
        bool ignoreSideLine;

        bool parallelOperation;

        public int BinValue { get => binValue; set => binValue = value; }
        public Point BasePosition { get => basePosition; set => basePosition = value; }
        public Size SheetSize { get => sheetSize; set => sheetSize = value; }
        public List<SheetPatternGroup> PatternGroupList { get => patternGroupList; set => patternGroupList = value; }
        public List<RegionInfoG> RegionInfoList { get => regionInfoList; set => regionInfoList = value; }

        public int EdgeValue { get => edgeValue; set => edgeValue = value; }
        public int EdgeWidth { get => edgeWidth; set => edgeWidth = value; }
        public EEdgeFindMethod EdgeFindMethod { get => edgeFindMethod; set => edgeFindMethod = value; }

        public bool InBarAlignY { get => inBarAlignY; set => inBarAlignY = value; }
        public bool InBarAlignX { get => inBarAlignX; set => inBarAlignX = value; }

        public bool AdaptivePairing { get => adaptivePairing; set => adaptivePairing = value; }
        public int BoundaryPairStep { get => boundaryPairStep; set => boundaryPairStep = value; }
        public EIgnoreMethod IgnoreMethod { get => ignoreMethod; set => ignoreMethod = value; }
        public bool IgnoreSideLine { get => ignoreSideLine; set => ignoreSideLine = value; }
        public bool ParallelOperation { get => parallelOperation; set => parallelOperation = value; }


        public CalculatorParam()
        {
            this.binValue = 0;
            this.basePosition = Point.Empty;
            this.patternGroupList = new List<SheetPatternGroup>();
            this.regionInfoList = new List<RegionInfoG>();
            this.sheetSize = Size.Empty;

            this.edgeValue = 50;
            this.edgeWidth = 10;
            this.edgeFindMethod = EEdgeFindMethod.Projection;

            this.inBarAlignX = false;
            this.inBarAlignY = false;

            this.adaptivePairing = true;
            this.boundaryPairStep = 2;
            this.ignoreMethod = EIgnoreMethod.Basic;
            this.ignoreSideLine = false;

            this.parallelOperation = true;
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

            XmlHelper.SetValue(algorithmElement, "BinValue", binValue.ToString());
            XmlHelper.SetValue(algorithmElement, "BasePosition", basePosition);
            XmlHelper.SetValue(algorithmElement, "SheetSize", sheetSize);

            foreach (SheetPatternGroup sheetPatternGroup in this.patternGroupList)
            {
                XmlElement subElement = algorithmElement.OwnerDocument.CreateElement("SheetPatternGroup");
                algorithmElement.AppendChild(subElement);
                sheetPatternGroup.SaveParam(subElement);
            }

            foreach (RegionInfo regionInfo in regionInfoList)
            {
                XmlElement subElement = algorithmElement.OwnerDocument.CreateElement("RegionInfo");
                algorithmElement.AppendChild(subElement);

                regionInfo.SaveParam(subElement);
            }

            XmlHelper.SetValue(algorithmElement, "EdgeWidth", this.edgeWidth);
            XmlHelper.SetValue(algorithmElement, "EdgeValue", this.edgeValue);
            XmlHelper.SetValue(algorithmElement, "EdgeFindMethod", this.edgeFindMethod.ToString());

            XmlHelper.SetValue(algorithmElement, "InBarAlignX", this.inBarAlignX);
            XmlHelper.SetValue(algorithmElement, "InBarAlignY", this.inBarAlignY);

            XmlHelper.SetValue(algorithmElement, "AdaptivePairing", this.adaptivePairing);
            XmlHelper.SetValue(algorithmElement, "BoundaryPairStep", this.boundaryPairStep);
            XmlHelper.SetValue(algorithmElement, "IgnoreMethod", this.ignoreMethod.ToString());
            XmlHelper.SetValue(algorithmElement, "IgnoreSideLine", this.ignoreSideLine.ToString());
            
            XmlHelper.SetValue(algorithmElement, "ParallelOperation", this.parallelOperation);

            SaveExtraParam(algorithmElement);
        }

        private void SaveExtraParam(XmlElement algorithmElement)
        {

            string baseUri = algorithmElement.OwnerDocument.BaseURI;

            //string configFileName = String.Format(@"{0}\ExtraParam.xml", PathSettings.Instance().Config);
            //XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            //if (xmlDocument == null)
            //    return;

            //XmlElement configXmlDocument = xmlDocument["Algorithm"];

            //inspBufferCount = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "InspBufferCount", "10"));
            ////maxDefectNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "MaxDefectNum", "1000"));

            //poleLowerWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PoleLowerWeight", "100"));
            //poleUpperWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PoleUpperWeight", "100"));

            //dielectricLowerWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "DielectricLowerWeight", "100"));
            //dielectricUpperWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "DielectricUpperWeight", "100"));

            //xPixelCal = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "XPixelCal", "10"));
            //yPixelCal = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "YPixelCal", "10"));

            //sheetAttackMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "SheetAttackMinSize", "40"));
            //poleMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PoleMinSize", "40"));
            //dielectricMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "DielectricMinSize", "40"));
            //pinHoleMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PinHoleMinSize", "40"));

            //gridColNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "GridColNum", "10"));
            //gridRowNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "GridRowNum", "10"));

            //isFiducial = Convert.ToBoolean(XmlHelper.GetValue(configXmlDocument, "IsFiducial", "false"));
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            foreach (RegionInfo regionInfo in regionInfoList)
                regionInfo.Dispose();
            regionInfoList.Clear();

            binValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "BinValue", binValue.ToString()));
            XmlHelper.GetValue(algorithmElement, "BasePosition", ref basePosition);
            XmlHelper.GetValue(algorithmElement, "SheetSize", ref sheetSize);

            XmlNodeList sheetPatternGroupNodeList = algorithmElement.GetElementsByTagName("SheetPatternGroup");
            foreach (XmlElement subElement in sheetPatternGroupNodeList)
            {
                SheetPatternGroup sheetPatternGroup = new SheetPatternGroup();
                sheetPatternGroup.LoadParam(subElement);
                this.patternGroupList.Add(sheetPatternGroup);
            }

            XmlNodeList regionInfoNodeList = algorithmElement.GetElementsByTagName("RegionInfo");
            foreach (XmlElement subElement in regionInfoNodeList)
            {
                RegionInfoG regionInfoG = RegionInfoG.Load(subElement);
                regionInfoList.Add(regionInfoG);
            }

            this.edgeWidth = XmlHelper.GetValue(algorithmElement, "EdgeWidth", this.edgeWidth);
            this.edgeValue = XmlHelper.GetValue(algorithmElement, "EdgeValue", this.edgeValue);
            this.edgeFindMethod = XmlHelper.GetValue(algorithmElement, "EdgeFindMethod", this.edgeFindMethod);

            this.inBarAlignX = XmlHelper.GetValue(algorithmElement, "InBarAlignX", this.inBarAlignX);
            this.inBarAlignY = XmlHelper.GetValue(algorithmElement, "InBarAlignY", this.inBarAlignY);

            this.adaptivePairing = XmlHelper.GetValue(algorithmElement, "AdaptivePairing", this.adaptivePairing);
            this.boundaryPairStep = XmlHelper.GetValue(algorithmElement, "BoundaryPairStep", this.boundaryPairStep);

            this.ignoreMethod = XmlHelper.GetValue(algorithmElement, "IgnoreMethod", this.ignoreMethod);
            this.ignoreSideLine = XmlHelper.GetValue(algorithmElement, "IgnoreSideLine", this.ignoreSideLine);
            
            this.parallelOperation = XmlHelper.GetValue(algorithmElement, "ParallelOperation", this.parallelOperation);
        }
    }
}
