using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanG.Screen.Vision
{
    class AlgorithmSetting
    {
        static AlgorithmSetting _instance;
        public static AlgorithmSetting Instance()
        {
            if (_instance == null)
                _instance = new AlgorithmSetting();

            return _instance;
        }

        int removalNum;
        public int RemovalNum
        {
            get { return removalNum; }
            set { removalNum = value; }
        }

        int maxDefectNum;
        public int MaxDefectNum
        {
            get { return maxDefectNum; }
            set { maxDefectNum = value; }
        }
        
        int poleLowerWeight;
        public int PoleLowerWeight
        {
            get { return poleLowerWeight; }
            set { poleLowerWeight = value; }
        }

        int poleUpperWeight;
        public int PoleUpperWeight
        {
            get { return poleUpperWeight; }
            set { poleUpperWeight = value; }
        }

        float dielectricCompactness;
        public float DielectricCompactness
        {
            get { return dielectricCompactness; }
            set { dielectricCompactness = value; }
        }

        float poleCompactness;
        public float PoleCompactness
        {
            get { return poleCompactness; }
            set { poleCompactness = value; }
        }

        int dielectricLowerWeight;
        public int DielectricLowerWeight
        {
            get { return dielectricLowerWeight; }
            set { dielectricLowerWeight = value; }
        }

        int dielectricUpperWeight;
        public int DielectricUpperWeight
        {
            get { return dielectricUpperWeight; }
            set { dielectricUpperWeight = value; }
        }

        float xPixelCal;
        public float XPixelCal
        {
            get { return xPixelCal; }
            set { xPixelCal = value; }
        }

        float yPixelCal;
        public float YPixelCal
        {
            get { return yPixelCal; }
            set { yPixelCal = value; }
        }

        int sheetAttackMinSize;
        public int SheetAttackMinSize
        {
            get { return sheetAttackMinSize; }
            set { sheetAttackMinSize = value; }
        }

        int poleMinSize;
        public int PoleMinSize
        {
            get { return poleMinSize; }
            set { poleMinSize = value; }
        }

        int dielectricMinSize;
        public int DielectricMinSize
        {
            get { return dielectricMinSize; }
            set { dielectricMinSize = value; }
        }

        int pinHoleMinSize;
        public int PinHoleMinSize
        {
            get { return pinHoleMinSize; }
            set { pinHoleMinSize = value; }
        }

        int gridColNum;
        public int GridColNum
        {
            get { return gridColNum; }
            set { gridColNum = value; }
        }

        int gridRowNum;
        public int GridRowNum
        {
            get { return gridRowNum; }
            set { gridRowNum = value; }
        }

        bool isFiducial;
        public bool IsFiducial
        {
            get { return isFiducial; }
            set { isFiducial = value; }
        }

        public AlgorithmSetting()
        {
            removalNum = 6;
            maxDefectNum = 1000;
            
            poleLowerWeight = 100;
            poleUpperWeight = 100;

            dielectricLowerWeight = 100;
            dielectricUpperWeight = 100;

            xPixelCal = 10;
            yPixelCal = 10;

            sheetAttackMinSize = 40;
            poleMinSize = 40;
            dielectricMinSize = 40;
            pinHoleMinSize = 40;

            gridColNum = 10;
            gridRowNum = 10;

            poleCompactness = 1.5f;
            dielectricCompactness = 1.5f;

            isFiducial = false;
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\AlgorithmSetting.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;

            XmlElement configXmlDocument = xmlDocument["Algorithm"];

            removalNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "RemovalNum", "6"));
            maxDefectNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "MaxDefectNum", "1000"));
            
            poleLowerWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PoleLowerWeight", "100"));
            poleUpperWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PoleUpperWeight", "100"));

            dielectricLowerWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "DielectricLowerWeight", "100"));
            dielectricUpperWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "DielectricUpperWeight", "100"));

            xPixelCal = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "XPixelCal", "10"));
            yPixelCal = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "YPixelCal", "10"));

            sheetAttackMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "SheetAttackMinSize", "40"));
            poleMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PoleMinSize", "40"));
            dielectricMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "DielectricMinSize", "40"));
            pinHoleMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PinHoleMinSize", "40"));
            
            gridColNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "GridColNum", "10"));
            gridRowNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "GridRowNum", "10"));

            isFiducial = Convert.ToBoolean(XmlHelper.GetValue(configXmlDocument, "IsFiducial", "false"));

            poleCompactness = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "PoleCompactness", "1"));
            dielectricCompactness = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "DielectricCompactness", "1"));
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\AlgorithmSetting.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement configXmlDocument = xmlDocument.CreateElement("Algorithm");
            xmlDocument.AppendChild(configXmlDocument);

            XmlHelper.SetValue(configXmlDocument, "RemovalNum", removalNum.ToString());
            XmlHelper.SetValue(configXmlDocument, "MaxDefectNum", maxDefectNum.ToString());
            
            XmlHelper.SetValue(configXmlDocument, "PoleLowerWeight", poleLowerWeight.ToString());
            XmlHelper.SetValue(configXmlDocument, "PoleUpperWeight", poleUpperWeight.ToString());

            XmlHelper.SetValue(configXmlDocument, "DielectricLowerWeight", dielectricLowerWeight.ToString());
            XmlHelper.SetValue(configXmlDocument, "DielectricUpperWeight", dielectricUpperWeight.ToString());

            XmlHelper.SetValue(configXmlDocument, "SheetAttackMinSize", sheetAttackMinSize.ToString());
            XmlHelper.SetValue(configXmlDocument, "PoleMinSize", poleMinSize.ToString());
            XmlHelper.SetValue(configXmlDocument, "DielectricMinSize", dielectricMinSize.ToString());
            XmlHelper.SetValue(configXmlDocument, "PinHoleMinSize", pinHoleMinSize.ToString());

            XmlHelper.SetValue(configXmlDocument, "GridColNum", gridColNum.ToString());
            XmlHelper.SetValue(configXmlDocument, "GridRowNum", gridRowNum.ToString());

            XmlHelper.SetValue(configXmlDocument, "XPixelCal", xPixelCal.ToString());
            XmlHelper.SetValue(configXmlDocument, "YPixelCal", yPixelCal.ToString());

            XmlHelper.SetValue(configXmlDocument, "IsFiducial", isFiducial.ToString());

            XmlHelper.SetValue(configXmlDocument, "PoleCompactness", poleCompactness.ToString());
            XmlHelper.SetValue(configXmlDocument, "DielectricCompactness", dielectricCompactness.ToString());

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}
