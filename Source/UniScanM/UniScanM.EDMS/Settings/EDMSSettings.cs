using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanM.EDMS.Settings
{
    public class EDMSSettings : UniScanM.Settings.UniScanMSettings
    {
        private bool sheetOnlyMode;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Sheet Only Mode"),
        LocalizedDescriptionAttributeEDMS("Sheet Only Mode")]
        public bool SheetOnlyMode { get => sheetOnlyMode; set => sheetOnlyMode = value; }

        private int skipLength;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Skip Length"),
        LocalizedDescriptionAttributeEDMS("Skip Length")]
        public int SkipLength { get => skipLength; set => skipLength = value; }

        private int zeroingCount;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Zeroing Count"),
        LocalizedDescriptionAttributeEDMS("Zeroing Count")]
        public int ZeroingCount { get => zeroingCount; set => zeroingCount = value; }

        private bool autoLight;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Auto Light"),
        LocalizedDescriptionAttributeEDMS("Auto Light")]
        public bool AutoLight { get => autoLight; set => autoLight = value; }

        private int autoLightOffsetTop;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Auto Light Offset Front"),
        LocalizedDescriptionAttributeEDMS("Auto Light Offset Front")]
        public int AutoLightOffsetTop { get => autoLightOffsetTop; set => autoLightOffsetTop = value; }

        private int autoLightOffsetBottom;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Auto Light Offset Back"),
        LocalizedDescriptionAttributeEDMS("Auto Light Offset Back")]
        public int AutoLightOffsetBottom { get => autoLightOffsetBottom; set => autoLightOffsetBottom = value; }

        private int maxMeasCountPerSec;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Maximum Measure Count per sec"),
        LocalizedDescriptionAttributeEDMS("Maximum Measure Count per sec")]
        public int MaxMeasCountPerSec { get => maxMeasCountPerSec; set => maxMeasCountPerSec = value; }

        private int imageSavingInterval;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Image Save Interval"),
        LocalizedDescriptionAttributeEDMS("Image Save Interval")]
        public int ImageSavingInterval { get => imageSavingInterval; set => imageSavingInterval = value; }

        private bool isFrontPosition;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Front Position"),
        LocalizedDescriptionAttributeEDMS("Front Position")]
        public bool IsFrontPosition { get => isFrontPosition; set => isFrontPosition = value; }

        private bool useLineStop;
        [LocalizedCategoryAttributeEDMS("2. Chart"),
        LocalizedDisplayNameAttributeEDMS("Use Line Stop"),
        LocalizedDescriptionAttributeEDMS("Use Line Stop")]
        public bool UseLineStop { get => useLineStop; set => useLineStop = value; }

        private double lineStop;
        [LocalizedCategoryAttributeEDMS("2. Chart"),
        LocalizedDisplayNameAttributeEDMS("Line Stop Range (um)"),
        LocalizedDescriptionAttributeEDMS("Line Stop Range (um)")]
        public double LineStop { get => lineStop; set => lineStop = value; }

        private bool useLineWarning;
        [LocalizedCategoryAttributeEDMS("2. Chart"),
        LocalizedDisplayNameAttributeEDMS("Use Line Warning"),
        LocalizedDescriptionAttributeEDMS("Use Line Warning")]
        public bool UseLineWarning { get => useLineWarning; set => useLineWarning = value; }

        private double lineWarning;
        [LocalizedCategoryAttributeEDMS("2. Chart"),
        LocalizedDisplayNameAttributeEDMS("Line Warning Range (um)"),
        LocalizedDescriptionAttributeEDMS("Line Warning Range (um)")]
        public double LineWarning { get => lineWarning; set => lineWarning = value; }

        private int xAxisDisplayDistance;
        [LocalizedCategoryAttributeEDMS("2. Chart"),
        LocalizedDisplayNameAttributeEDMS("X Axis Display Distance (M)"),
        LocalizedDescriptionAttributeEDMS("X Axis Display Distance (M)")]
        public int XAxisDisplayDistance { get => xAxisDisplayDistance; set => xAxisDisplayDistance = value; }

        private int xAxisInterval;
        [LocalizedCategoryAttributeEDMS("2. Chart"),
        LocalizedDisplayNameAttributeEDMS("X Axis Interval"),
        LocalizedDescriptionAttributeEDMS("X Axis Interval")]
        public int XAxisInterval { get => xAxisInterval; set => xAxisInterval = value; }

        private float yAxisRangeMM;
        [LocalizedCategoryAttributeEDMS("2. Chart"),
        LocalizedDisplayNameAttributeEDMS("T100~ T104 Y Axis Range (mm)"),
        LocalizedDescriptionAttributeEDMS("T100~ T104 Y Axis Range (mm)")]
        public float YAxisRangeMM { get => yAxisRangeMM; set => yAxisRangeMM = value; }

        private float yAxisRangeUM;
        [LocalizedCategoryAttributeEDMS("2. Chart"),
        LocalizedDisplayNameAttributeEDMS("T105 Y Axis Range (um)"),
        LocalizedDescriptionAttributeEDMS("T105 Y Axis Range (um)")]
        public float YAxisRangeUM { get => yAxisRangeUM; set => yAxisRangeUM = value; }

        private int yAxisInterval;
        [LocalizedCategoryAttributeEDMS("2. Chart"),
        LocalizedDisplayNameAttributeEDMS("Y Axis Interval"),
        LocalizedDescriptionAttributeEDMS("Y Axis Interval")]
        public int YAxisInterval { get => yAxisInterval; set => yAxisInterval = value; }

        private Color axisColor;
        [LocalizedCategoryAttributeEDMS("3. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Axis Color"),
        LocalizedDescriptionAttributeEDMS("Axis Color")]
        public Color AxisColor { get => axisColor; set => axisColor = value; }

        private Color backColor;
        [LocalizedCategoryAttributeEDMS("3. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Background Color"),
        LocalizedDescriptionAttributeEDMS("Background Color")]
        public Color BackColor { get => backColor; set => backColor = value; }

        private Color graphColor;
        [LocalizedCategoryAttributeEDMS("3. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Graph Color"),
        LocalizedDescriptionAttributeEDMS("Graph Color")]
        public Color GraphColor { get => graphColor; set => graphColor = value; }

        private int graphThickness;
        [LocalizedCategoryAttributeEDMS("3. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Graph Thickness"),
        LocalizedDescriptionAttributeEDMS("Graph Thickness")]
        public int GraphThickness { get => graphThickness; set => graphThickness = value; }

        private Color lineStopColor;
        [LocalizedCategoryAttributeEDMS("3. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Line Stop Color"),
        LocalizedDescriptionAttributeEDMS("Line Stop Color")]
        public Color LineStopColor { get => lineStopColor; set => lineStopColor = value; }

        private int lineStopThickness;
        [LocalizedCategoryAttributeEDMS("3. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Line Stop Thickness"),
        LocalizedDescriptionAttributeEDMS("Line Stop Thickness")]
        public int LineStopThickness { get => lineStopThickness; set => lineStopThickness = value; }

        private Color lineWarningColor;
        [LocalizedCategoryAttributeEDMS("3. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Line Warning Color"),
        LocalizedDescriptionAttributeEDMS("Line Warning Color")]
        public Color LineWarningColor { get => lineWarningColor; set => lineWarningColor = value; }

        private int lineWarningThickness;
        [LocalizedCategoryAttributeEDMS("3. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Line Warning Thickness"),
        LocalizedDescriptionAttributeEDMS("Line Warning Thickness")]
        public int LineWarningThickness { get => lineWarningThickness; set => lineWarningThickness = value; }

        protected EDMSSettings()
        {
            sheetOnlyMode = false;
            skipLength = 20;
            zeroingCount = 10;
            autoLight = false;
            autoLightOffsetBottom = 0;
            autoLightOffsetTop = 0;
            maxMeasCountPerSec = 2;
            imageSavingInterval = 50;
            isFrontPosition = false;

            useLineStop = false;
            lineStop = 200;
            useLineWarning = true;
            lineWarning = 100;
            xAxisDisplayDistance = 30;
            xAxisInterval = 4;
            yAxisRangeMM = 0.5f;
            yAxisRangeUM = 300f;
            yAxisInterval = 6;

            // 5. Graph Color
            axisColor = Color.DarkGray;
            backColor = Color.Black;
            graphColor = Color.LawnGreen;
            graphThickness = 3;
            lineStopColor = Color.Red;
            lineStopThickness = 2;
            lineWarningColor = Color.Gold;
            lineWarningThickness = 2;

        }

        public static EDMSSettings Instance()
        {
            return instance as EDMSSettings;
        }

        public static new void CreateInstance()
        {
            if (instance == null)
                instance = new EDMSSettings();
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
                return;

            XmlElement dataElement = xmlElement.OwnerDocument.CreateElement("", "Data", "");
            xmlElement.AppendChild(dataElement);
            XmlHelper.SetValue(dataElement, "SheetOnlyMode", sheetOnlyMode.ToString());
            XmlHelper.SetValue(dataElement, "SkipLength", skipLength.ToString());
            XmlHelper.SetValue(dataElement, "ZeroingCount", zeroingCount.ToString());
            XmlHelper.SetValue(dataElement, "AutoLight", autoLight.ToString());
            XmlHelper.SetValue(dataElement, "AutoLightOffsetBottom", autoLightOffsetBottom.ToString());
            XmlHelper.SetValue(dataElement, "AutoLightOffsetTop", autoLightOffsetTop.ToString());
            XmlHelper.SetValue(dataElement, "MaxMeasCountPerSec", maxMeasCountPerSec.ToString());
            XmlHelper.SetValue(dataElement, "ImageSavingInterval", imageSavingInterval.ToString());
            XmlHelper.SetValue(dataElement, "IsFrontPosition", isFrontPosition.ToString());

            // 3. Vibration Graph
            XmlElement vibrationGraphElement = xmlElement.OwnerDocument.CreateElement("", "Chart", "");
            xmlElement.AppendChild(vibrationGraphElement);

            XmlHelper.SetValue(vibrationGraphElement, "UseLineStop", useLineStop.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "UseLineWarning", useLineWarning.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "LineStop", lineStop.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "LineWarning", lineWarning.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "XAxisDisplayDistance", xAxisDisplayDistance.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "XAxisInterval", xAxisInterval.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "YAxisRangeMM", yAxisRangeMM.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "YAxisRangeUM", yAxisRangeUM.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "YAxisInterval", yAxisInterval.ToString());

            // 5. Graph Color
            XmlElement graphColorElement = xmlElement.OwnerDocument.CreateElement("", "Appearance", "");
            xmlElement.AppendChild(graphColorElement);

            XmlHelper.SetValue(graphColorElement, "AxisColor", axisColor.Name);
            XmlHelper.SetValue(graphColorElement, "BackColor", backColor.Name);
            XmlHelper.SetValue(graphColorElement, "GraphColor", graphColor.Name);
            XmlHelper.SetValue(graphColorElement, "GraphThickness", graphThickness.ToString());
            XmlHelper.SetValue(graphColorElement, "LineStopColor", lineStopColor.Name);
            XmlHelper.SetValue(graphColorElement, "LineStopThickness", lineStopThickness.ToString());
            XmlHelper.SetValue(graphColorElement, "LineWarningColor", lineWarningColor.Name);
            XmlHelper.SetValue(graphColorElement, "LineWarningThickness", lineWarningThickness.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            if (xmlElement == null)
                return;

            XmlElement dataElement = xmlElement["Data"];
            if (dataElement != null)
            {
                sheetOnlyMode = XmlHelper.GetValue(dataElement, "SheetOnlyMode", sheetOnlyMode);
                skipLength = XmlHelper.GetValue(dataElement, "SkipLength", skipLength);
                zeroingCount = XmlHelper.GetValue(dataElement, "ZeroingCount", zeroingCount);
                autoLight = XmlHelper.GetValue(dataElement, "AutoLight", autoLight);
                autoLightOffsetBottom = XmlHelper.GetValue(dataElement, "AutoLightOffsetBottom", autoLightOffsetBottom);
                autoLightOffsetTop = XmlHelper.GetValue(dataElement, "AutoLightOffsetTop", autoLightOffsetTop);
                maxMeasCountPerSec = XmlHelper.GetValue(dataElement, "MaxMeasCountPerSec", maxMeasCountPerSec);
                imageSavingInterval = XmlHelper.GetValue(dataElement, "ImageSavingInterval", imageSavingInterval);
                isFrontPosition = XmlHelper.GetValue(dataElement, "IsFrontPosition", isFrontPosition);
            }
            

           // 3. Vibration Graph
           XmlElement vibrationGraphElement = xmlElement["Chart"];
            if (vibrationGraphElement != null)
            {
                //useLineWarning = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseLineWarning", "false"));
                //lineWarningLSL = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineWarningLSL", "-0.5"));
                //lineWarningUSL = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineWarningUSL", "0.5"));
                useLineStop = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseLineStop", "false"));
                useLineWarning = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseLineWarning", "false"));
                lineStop = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineStop", "0.7"));
                lineWarning = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineWarning", "0.6"));
                xAxisDisplayDistance = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "XAxisDisplayDistance", "1"));
                xAxisInterval = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "XAxisInterval", "6"));
                yAxisRangeMM = float.Parse(XmlHelper.GetValue(vibrationGraphElement, "YAxisRangeMM", "-1"));
                yAxisRangeUM = float.Parse(XmlHelper.GetValue(vibrationGraphElement, "YAxisRangeUM", "-1"));
                yAxisInterval = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "YAxisInterval", "6"));
            }

            // 5. Graph Color
            XmlElement graphColorElement = xmlElement["Appearance"];
            if (graphColorElement != null)
            {
                ColorConverter converter = new ColorConverter();

                axisColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "AxisColor", "DarkGray"));
                backColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "BackColor", "White"));
                graphColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "GraphColor", "Black"));
                graphThickness = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "GraphThickness", "3"));
                lineStopColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "LineStopColor", "Red"));
                lineStopThickness = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineStopThickness", "2"));
                lineWarningColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "LineWarningColor", "Gold"));
                lineWarningThickness = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineWarningThickness ", "2"));
            }
        }
    }
}
