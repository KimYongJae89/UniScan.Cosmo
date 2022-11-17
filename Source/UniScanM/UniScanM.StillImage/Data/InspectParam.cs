using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanM.Data;

namespace UniScanM.StillImage.Data
{
    public class InspectParam : UniScanM.Data.InspectParam, IExportable, IImportable
    {
        bool isRelativeOffset = false;
        Feature offsetRange;
        int maxDefectSize = 1000;
        float matchRatio = 0.8f;

        public bool IsRelativeOffset
        {
            get { return isRelativeOffset; }
            set { isRelativeOffset = value; }
        }

        public Feature OffsetRange
        {
            get { return offsetRange; }
            set { offsetRange = value; }
        }

        public int MaxDefectSize
        {
            get { return maxDefectSize; }
            set { maxDefectSize = value; }
        }

        public float MatchRatio
        {
            get { return matchRatio; }
            set { matchRatio = value; }
        }

        SizeF sheetSize = new SizeF(0, 0);
        public SizeF SheetSize { get => sheetSize; set => sheetSize = value; }

        public InspectParam()
        {
            isRelativeOffset = false;
            offsetRange = new Feature();
            offsetRange.Area = 20;
            offsetRange.Margin = new SizeF(20, 20);
            offsetRange.Blot = new SizeF(20, 20);
        }

        public InspectParam(bool isRelative, Feature offsetRange)
        {
            this.isRelativeOffset = isRelative;
            this.offsetRange = offsetRange;
        }

        public override void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Import(subElement);
                return;
            }

            isRelativeOffset = Convert.ToBoolean(XmlHelper.GetValue(element, "IsRelative", "false"));
            maxDefectSize = Convert.ToInt32(XmlHelper.GetValue(element, "MaxDefectSize", maxDefectSize.ToString()));
            matchRatio = Convert.ToSingle(XmlHelper.GetValue(element, "MatchRatio", matchRatio.ToString()));
            offsetRange.Import(element, "OffsetRange");
        }

        public override void Export(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Export(subElement);
                return;
            }

            XmlHelper.SetValue(element, "IsRelative", isRelativeOffset.ToString());
            XmlHelper.SetValue(element, "MaxDefectSize", maxDefectSize.ToString());
            XmlHelper.SetValue(element, "MatchRatio", matchRatio.ToString());

            offsetRange.Export(element, "OffsetRange");
        }

        public static InspectParam Load(XmlElement paramElement, string subKey = null)
        {
            InspectParam param = new InspectParam();
            param.Import(paramElement, subKey);
            return param;
        }
    }
}
