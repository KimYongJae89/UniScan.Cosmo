using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using UniScanG.Data;

namespace UniScanG.Screen.Data
{
    public class ProductionS : UniScanG.Data.Production
    {
        int sheetIndex;
        public int SheetIndex
        {
            get { return sheetIndex; }
            set { sheetIndex = value; }
        }

        float thickness;
        public float Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }

        string paste;
        public string Paste
        {
            get { return paste; }
            set { paste = value; }
        }

        int sheetAttackNum;
        public int SheetAttackNum
        {
            get { return sheetAttackNum; }
            set { sheetAttackNum = value; }
        }

        int poleLineNum;
        public int PoleLineNum
        {
            get { return poleLineNum; }
            set { poleLineNum = value; }
        }

        int poleCircleNum;
        public int PoleCircleNum
        {
            get { return poleCircleNum; }
            set { poleCircleNum = value; }
        }

        int dielectricNum;
        public int DielectricNum
        {
            get { return dielectricNum; }
            set { dielectricNum = value; }
        }

        int pinHoleNum;
        public int PinHoleNum
        {
            get { return pinHoleNum; }
            set { pinHoleNum = value; }
        }

        int shapeNum;
        public int ShapeNum
        {
            get { return shapeNum; }
            set { shapeNum = value; }
        }

        public ProductionS(string name, float thickness, string paste, string lotNo) : base(name, lotNo, thickness, paste)
        {
            this.thickness = thickness;
            this.paste = paste; 
        }

        public ProductionS(XmlElement productionElement) : base(productionElement)
        {
            
        }

        public override void Load(XmlElement productionElement)
        {
            base.Load(productionElement);

            sheetIndex = Convert.ToInt32(XmlHelper.GetValue(productionElement, "SheetIndex", "0"));

            thickness = Convert.ToSingle(XmlHelper.GetValue(productionElement, "Thickness", "0"));
            paste = XmlHelper.GetValue(productionElement, "Paste", "");

            sheetAttackNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "SheetAttackNum", "0"));
            poleLineNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "PoleLineNum", "0"));
            poleCircleNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "PoleCircleNum", "0"));
            dielectricNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "DielectricNum", "0"));
            pinHoleNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "PinHoleNum", "0"));
            shapeNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "ShapeNum", "0"));
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);

            XmlHelper.SetValue(productionElement, "SheetIndex", sheetIndex.ToString());

            XmlHelper.SetValue(productionElement, "Thickness", thickness);
            XmlHelper.SetValue(productionElement, "Paste", paste);

            XmlHelper.SetValue(productionElement, "SheetAttackNum", sheetAttackNum.ToString());
            XmlHelper.SetValue(productionElement, "PoleLineNum", poleLineNum.ToString());
            XmlHelper.SetValue(productionElement, "PoleCircleNum", poleCircleNum.ToString());
            XmlHelper.SetValue(productionElement, "DielectricNum", dielectricNum.ToString());
            XmlHelper.SetValue(productionElement, "PinHoleNum", pinHoleNum.ToString());
            XmlHelper.SetValue(productionElement, "ShapeNum", shapeNum.ToString());
        }

        public override string GetResultPath()
        {
            string resultPath = Path.Combine(   
                PathSettings.Instance().Result,
                this.StartTime.ToString("yy-MM-dd"),
                this.Name,
                this.Thickness.ToString(),
                this.Paste,
                this.LotNo);

            return resultPath;
        }

        public override void Update(SheetResult sheetResult)
        {
            throw new NotImplementedException();
        }

        public override string GetResultPath(string root)
        {
            throw new NotImplementedException();
        }
    }
}
