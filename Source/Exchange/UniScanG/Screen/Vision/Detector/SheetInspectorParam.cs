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
using UniScanG.Screen.Data;
using UniScanG.Screen.Vision.Detector.Dielectric;
using UniScanG.Screen.Vision.Detector.Pole;
using UniScanG.Screen.Vision.Detector.Shape;
using UniScanG.Vision;

namespace UniScanG.Screen.Vision.Detector
{
    public class SheetInspectorParam : AlgorithmParam
    {
        int emptyRegionPos;
        public int EmptyRegionPos
        {
            get { return emptyRegionPos; }
            set { emptyRegionPos = value; }
        }
        
        List<RegionInfo> regionInfoList = new List<RegionInfo>();
        public List<RegionInfo> RegionInfoList
        {
            get { return regionInfoList; }
            set { regionInfoList = value; }
        }

        DielectricInspectorParam dielectricParam = new DielectricInspectorParam();
        public DielectricInspectorParam DielectricParam
        {
            get { return dielectricParam; }
            set { dielectricParam = value; }
        }

        PoleInspectorParam poleParam = new PoleInspectorParam();
        public PoleInspectorParam PoleParam
        {
            get { return poleParam; }
            set { poleParam = value; }
        }

        ShapeInspectorParam shapeParam = new ShapeInspectorParam();
        public ShapeInspectorParam ShapeParam
        {
            get { return shapeParam; }
            set { shapeParam = value; }
        }

        public override AlgorithmParam Clone()
        {
            SheetInspectorParam clone = new SheetInspectorParam();

            clone.CopyFrom(this);

            return clone;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            SheetInspectorParam srcParam = (SheetInspectorParam)srcAlgorithmParam;

            this.poleParam.Copy(srcParam.poleParam);
            this.dielectricParam.Copy(srcParam.dielectricParam);
            this.shapeParam.Copy(srcParam.shapeParam);
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            poleParam.SaveParam(algorithmElement);
            dielectricParam.SaveParam(algorithmElement);
            shapeParam.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "EmptyRegionPos", emptyRegionPos.ToString());
            
            foreach (RegionInfo regionInfo in regionInfoList)
            {
                XmlElement regionInfoElement = algorithmElement.OwnerDocument.CreateElement("RegionInfo");
                algorithmElement.AppendChild(regionInfoElement);
                regionInfo.SaveParam(regionInfoElement);
            }
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            poleParam.LoadParam(algorithmElement);
            dielectricParam.LoadParam(algorithmElement);
            shapeParam.LoadParam(algorithmElement);

            emptyRegionPos = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "EmptyRegionPos", "0"));
            
            foreach (XmlElement regionInfoElement in algorithmElement)
            {
                if (regionInfoElement.Name == "RegionInfo")
                {
                    RegionInfo regionInfo = new RegionInfoS();
                    regionInfo.LoadParam(regionInfoElement);
                    regionInfoList.Add(regionInfo);
                }
            }
        }

        public override void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
