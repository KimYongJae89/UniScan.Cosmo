using DynMvp.Base;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanG.Data;
using UniScanG.Gravure.MachineIF;

namespace UniScanG.Gravure.Data
{
    public class ProductionG : UniScanG.Data.Production
    {
        int sheetAttackPatternNum;
        public int SheetAttackPatternNum { get { return sheetAttackPatternNum; } }

        int noPrintPatternNum;
        public int NoPrintPatternNum { get { return noPrintPatternNum; } }

        int dielectricPatternNum;
        public int DielectricPatternNum { get { return dielectricPatternNum; } }

        int pinHolePatternNum;
        public int PinHolePatternNum { get { return pinHolePatternNum; } }

        int sheetAttackNum;
        public int SheetAttackNum { get { return sheetAttackNum; } }

        int noPrintNum;
        public int NoPrintNum { get { return noPrintNum; } }

        int dielectricNum;
        public int DielectricNum { get { return dielectricNum; } }

        int pinHoleNum;
        public int PinHoleNum { get { return pinHoleNum; } }

        float lineSpeedMpm;
        public float LineSpeedMpm { get { return lineSpeedMpm; } }

        public ProductionG(string name, string lotNo, float thickness, string paste, float lineSpeedMpm) : base(name, lotNo, thickness, paste)
        {
            if (lineSpeedMpm < 0)
                UpdateLineSpeedMpm(-1);
            else
                this.lineSpeedMpm = lineSpeedMpm;
        }

        public ProductionG(XmlElement productionElement) : base(productionElement) { }

        public override bool Equals(object obj)
        {
            bool eq = base.Equals(obj);
            if (eq == true)
            {
                Production production = obj as Production;
                eq = (this.thickness == production.Thickness && this.paste == production.Paste);
            }
            return eq;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void Load(XmlElement productionElement)
        {
            base.Load(productionElement);

            thickness = Convert.ToSingle(XmlHelper.GetValue(productionElement, "Thickness", ""));
            paste = XmlHelper.GetValue(productionElement, "Paste", "");

            sheetAttackNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "SheetAttackNum", "0"));
            noPrintNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "NoPrintNum", "0"));
            dielectricNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "DielectricNum", "0"));
            pinHoleNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "PinHoleNum", "0"));

            sheetAttackPatternNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "SheetAttackPatternNum", "0"));
            noPrintPatternNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "NoPrintPatternNum", "0"));
            dielectricPatternNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "DielectricPatternNum", "0"));
            pinHolePatternNum = Convert.ToInt32(XmlHelper.GetValue(productionElement, "PinHolePatternNum", "0"));

            lineSpeedMpm = XmlHelper.GetValue(productionElement, "lineSpeedMpm", -1);
            if (lineSpeedMpm < 0)
                lineSpeedMpm = Convert.ToSingle(XmlHelper.GetValue(productionElement, "ProductionTargetSpeedMpm", lineSpeedMpm.ToString()));
        }

        public override void Save(XmlElement productionElement)
        {
            base.Save(productionElement);
            
            XmlHelper.SetValue(productionElement, "Thickness", thickness);
            XmlHelper.SetValue(productionElement, "Paste", paste);

            XmlHelper.SetValue(productionElement, "SheetAttackNum", sheetAttackNum.ToString());
            XmlHelper.SetValue(productionElement, "NoPrintNum", noPrintNum.ToString());
            XmlHelper.SetValue(productionElement, "DielectricNum", dielectricNum.ToString());
            XmlHelper.SetValue(productionElement, "PinHoleNum", pinHoleNum.ToString());

            XmlHelper.SetValue(productionElement, "SheetAttackPatternNum", sheetAttackPatternNum.ToString());
            XmlHelper.SetValue(productionElement, "NoPrintPatternNum", noPrintPatternNum.ToString());
            XmlHelper.SetValue(productionElement, "DielectricPatternNum", dielectricPatternNum.ToString());
            XmlHelper.SetValue(productionElement, "PinHolePatternNum", pinHolePatternNum.ToString());

            XmlHelper.SetValue(productionElement, "lineSpeedMpm", lineSpeedMpm.ToString());
        }

        public override void Update(SheetResult sheetResult)
        {
            lock (this)
            {
                // BuildInspectionResult 할 때 Add 했음.
                //AddTotal();

                if (sheetResult == null)
                    return;

                if (sheetResult.SheetSubResultList.Count > 0)
                    AddNG();
                else
                    AddGood();
                
                int sheetAttackNum = sheetResult.SheetSubResultList.FindAll(sub => sub.GetDefectType() == DefectType.SheetAttack).Count;
                int noPrintNum = sheetResult.SheetSubResultList.FindAll(sub => sub.GetDefectType() == DefectType.Noprint).Count;
                int dielectricNum = sheetResult.SheetSubResultList.FindAll(sub => sub.GetDefectType() == DefectType.Dielectric).Count;
                int pinHoleNum = sheetResult.SheetSubResultList.FindAll(sub => sub.GetDefectType() == DefectType.PinHole).Count;

                if (sheetAttackNum > 0)
                {
                    this.sheetAttackNum += sheetAttackNum;
                    this.sheetAttackPatternNum++;
                }

                if (noPrintNum > 0)
                {
                    this.noPrintNum += noPrintNum;
                    this.noPrintPatternNum++;
                }

                if (dielectricNum > 0)
                {
                    this.dielectricNum += dielectricNum;
                    this.dielectricPatternNum++;
                }

                if (pinHoleNum > 0)
                {
                    this.pinHoleNum += pinHoleNum;
                    this.pinHolePatternNum++;
                }
            }
        }

        public void UpdateLineSpeedMpm(float lineSpeedMpm)
        {
            if (lineSpeedMpm >= 0)
            {
                this.lineSpeedMpm = lineSpeedMpm;
            }
            else
            {
                // Get Line Speed
                MachineIfProtocol protocol = SystemManager.Instance().MachineIfProtocolList?.GetProtocol(UniScanGMachineIfCommon.GET_TARGET_SPEED);
                MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(protocol);
                if (responce != null && responce.IsGood)
                    this.lineSpeedMpm = Convert.ToInt32(responce.ReciveData, 16) / 10.0f;
                else
                    this.lineSpeedMpm = 0;
            }
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

        public override string GetResultPath(string root)
        {
            string resultPath = Path.Combine(
                root,"Result",
                this.StartTime.ToString("yy-MM-dd"),
                this.Name,
                this.Thickness.ToString(),
                this.Paste,
                this.LotNo);

            return resultPath;
        }
    }
}
