using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using UniScanG.Gravure.Vision.Calculator;

namespace UniScanG.Gravure.Vision
{
    public class AlgorithmSetting
    {
        static AlgorithmSetting _instance;
        public static AlgorithmSetting Instance()
        {
            if (_instance == null)
                _instance = new AlgorithmSetting();

            return _instance;
        }

        CalculatorBase.Version calculatorVersion;
        public CalculatorBase.Version CalculatorVersion
        {
            get { return calculatorVersion; }
            set { calculatorVersion = value; }
        }

        int inspBufferCount;
        public int InspBufferCount
        {
            get { return inspBufferCount; }
            set { inspBufferCount = value; }
        }

        int monitoringPeriod;
        public int MonitoringPeriod
        {
            get { return monitoringPeriod; }
            set { monitoringPeriod = value; }
        }


        // 불량 사이즈 -> 모델별 파라메터에서 글로벌 파라메터로 이동.
        int minBlackDefectLength = 80;
        int minWhiteDefectLength = 80;
        public int MinBlackDefectLength { get => minBlackDefectLength; set => minBlackDefectLength = value; }
        public int MinWhiteDefectLength { get => minWhiteDefectLength; set => minWhiteDefectLength = value; }

        int maxDefectNum;
        public int MaxDefectNum
        {
            get { return maxDefectNum; }
            set { maxDefectNum = value; }
        }

        //int poleLowerWeight;
        //public int PoleLowerWeight
        //{
        //    get { return poleLowerWeight; }
        //    set { poleLowerWeight = value; }
        //}

        //int poleUpperWeight;
        //public int PoleUpperWeight
        //{
        //    get { return poleUpperWeight; }
        //    set { poleUpperWeight = value; }
        //}

        //int dielectricLowerWeight;
        //public int DielectricLowerWeight
        //{
        //    get { return dielectricLowerWeight; }
        //    set { dielectricLowerWeight = value; }
        //}

        //int dielectricUpperWeight;
        //public int DielectricUpperWeight
        //{
        //    get { return dielectricUpperWeight; }
        //    set { dielectricUpperWeight = value; }
        //}

        //float xPixelCal;
        //public float XPixelCal
        //{
        //    get { return xPixelCal; }
        //    set { xPixelCal = value; }
        //}

        //float yPixelCal;
        //public float YPixelCal
        //{
        //    get { return yPixelCal; }
        //    set { yPixelCal = value; }
        //}

        //int sheetAttackMinSize;
        //public int SheetAttackMinSize
        //{
        //    get { return sheetAttackMinSize; }
        //    set { sheetAttackMinSize = value; }
        //}

        //int poleMinSize;
        //public int PoleMinSize
        //{
        //    get { return poleMinSize; }
        //    set { poleMinSize = value; }
        //}

        //int dielectricMinSize;
        //public int DielectricMinSize
        //{
        //    get { return dielectricMinSize; }
        //    set { dielectricMinSize = value; }
        //}

        //int pinHoleMinSize;
        //public int PinHoleMinSize
        //{
        //    get { return pinHoleMinSize; }
        //    set { pinHoleMinSize = value; }
        //}

        //int gridColNum;
        //public int GridColNum
        //{
        //    get { return gridColNum; }
        //    set { gridColNum = value; }
        //}

        //int gridRowNum;
        //public int GridRowNum
        //{
        //    get { return gridRowNum; }
        //    set { gridRowNum = value; }
        //}

        //bool isFiducial;
        //public bool IsFiducial
        //{
        //    get { return isFiducial; }
        //    set { isFiducial = value; }
        //}

        public AlgorithmSetting()
        {
            inspBufferCount = 10;

            minBlackDefectLength = 80;
            minWhiteDefectLength = 80;

            monitoringPeriod = 10;

            calculatorVersion = CalculatorBase.Version.V1;
            //maxDefectNum = 1000;

            //poleLowerWeight = 100;
            //poleUpperWeight = 100;

            //dielectricLowerWeight = 100;
            //dielectricUpperWeight = 100;

            //xPixelCal = 10;
            //yPixelCal = 10;

            //sheetAttackMinSize = 40;
            //poleMinSize = 40;
            //dielectricMinSize = 40;
            //pinHoleMinSize = 40;

            //gridColNum = 10;
            //gridRowNum = 10;

            //isFiducial = false;

            Load();
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\AlgorithmSetting.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
            {
                Save();
                return;
            }

            XmlElement configXmlDocument = xmlDocument["Algorithm"];

            inspBufferCount = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "InspBufferCount", "10"));
            monitoringPeriod = XmlHelper.GetValue(configXmlDocument, "MonitoringPeriod", 10);

            this.minBlackDefectLength = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "MinBlackDefectLength", minBlackDefectLength.ToString()));
            this.minWhiteDefectLength = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "MinWhiteDefectLength", minWhiteDefectLength.ToString()));

            this.calculatorVersion = XmlHelper.GetValue(configXmlDocument, "CalculatorVersion", Calculator.CalculatorBase.Version.V1);

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

        public void Save()
        {
            string configFileName = String.Format(@"{0}\AlgorithmSetting.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement configXmlDocument = xmlDocument.CreateElement("Algorithm");
            xmlDocument.AppendChild(configXmlDocument);

            XmlHelper.SetValue(configXmlDocument, "InspBufferCount", inspBufferCount.ToString());
            XmlHelper.SetValue(configXmlDocument, "MonitoringPeriod", monitoringPeriod.ToString());

            XmlHelper.SetValue(configXmlDocument, "MinBlackDefectLength", minBlackDefectLength.ToString());
            XmlHelper.SetValue(configXmlDocument, "MinWhiteDefectLength", minWhiteDefectLength.ToString());

            XmlHelper.SetValue(configXmlDocument, "CalculatorVersion", calculatorVersion.ToString());

            //XmlHelper.SetValue(configXmlDocument, "PoleLowerWeight", poleLowerWeight.ToString());
            //XmlHelper.SetValue(configXmlDocument, "PoleUpperWeight", poleUpperWeight.ToString());

            //XmlHelper.SetValue(configXmlDocument, "DielectricLowerWeight", dielectricLowerWeight.ToString());
            //XmlHelper.SetValue(configXmlDocument, "DielectricUpperWeight", dielectricUpperWeight.ToString());

            //XmlHelper.SetValue(configXmlDocument, "SheetAttackMinSize", sheetAttackMinSize.ToString());
            //XmlHelper.SetValue(configXmlDocument, "PoleMinSize", poleMinSize.ToString());
            //XmlHelper.SetValue(configXmlDocument, "DielectricMinSize", dielectricMinSize.ToString());
            //XmlHelper.SetValue(configXmlDocument, "PinHoleMinSize", pinHoleMinSize.ToString());

            //XmlHelper.SetValue(configXmlDocument, "GridColNum", gridColNum.ToString());
            //XmlHelper.SetValue(configXmlDocument, "GridRowNum", gridRowNum.ToString());

            //XmlHelper.SetValue(configXmlDocument, "XPixelCal", xPixelCal.ToString());
            //XmlHelper.SetValue(configXmlDocument, "YPixelCal", yPixelCal.ToString());

            //XmlHelper.SetValue(configXmlDocument, "IsFiducial", isFiducial.ToString());

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}
