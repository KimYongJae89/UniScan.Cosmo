using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Settings;
using UniScanG.Data;
using UniScanG.Screen.Vision.Detector;

namespace UniScanG.Screen.Data
{
    public class ScreenResult : UniScanG.Data.SheetResult, IExportable
    {
        public bool poleReached;
        public bool dielectricReached;

        public void AddSheetSubResult(List<SheetSubResult> sheetAttackList, List<SheetSubResult> poleLineList, List<SheetSubResult> poleCircleList,
            List<SheetSubResult> dielectricList, List<SheetSubResult> pinHoleList, List<ShapeResult> shapeList)
        {
            this.subResultList.AddRange(sheetAttackList);
            this.subResultList.AddRange(poleLineList);
            this.subResultList.AddRange(poleCircleList);
            this.subResultList.AddRange(dielectricList);
            this.subResultList.AddRange(pinHoleList);
            this.subResultList.AddRange(shapeList);
        }

        public override void Export(string path, CancellationToken cancellationToken)
        {
            SheetInspector sheetInspector = (SheetInspector)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName);

            if (sheetInspector == null)
                return;

            SheetInspectorParam param = (SheetInspectorParam)sheetInspector.Param;

            string fileName = Path.Combine(path, string.Format("{0}.csv", SheetInspector.TypeName));
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}", this.spandTime);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("{0}\t{1}", param.PoleParam.LowerThreshold, param.PoleParam.UpperThreshold);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("{0}\t{1}", param.DielectricParam.LowerThreshold, param.DielectricParam.UpperThreshold);
            stringBuilder.AppendLine();
            stringBuilder.AppendFormat("{0}\t{1}\t{2}\t", param.ShapeParam.DiffTolerence, param.ShapeParam.UseHeightDiffTolerence, param.ShapeParam.HeightDiffTolerence);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            ImageHelper.SaveImage(prevImage, Path.Combine(path, string.Format("Prev.Jpg")));

            IClientExchangeOperator client = (IClientExchangeOperator)SystemManager.Instance().ExchangeOperator;

            int index = 0;
            foreach (SheetSubResult subResult in SheetSubResultList)
            {
                subResult.Index = index;
                subResult.CamIndex = client.GetCamIndex();
                stringBuilder.AppendFormat(subResult.ToExportData());
                stringBuilder.AppendLine();
                ImageHelper.SaveImage(subResult.Image, Path.Combine(path, string.Format("{0}.bmp", index)));
                index++;
            }

            File.WriteAllText(fileName, stringBuilder.ToString());
        }

        public override bool Import(string path)
        {
            string fileName = Path.Combine(path, string.Format("{0}.csv", SheetInspector.TypeName));

            if (File.Exists(fileName) == false)
                return false;

            string[] lines = File.ReadAllLines(fileName);

            string imageName = Path.Combine(path, "Prev.jpg");
            if (File.Exists(imageName) == true)
            {
                Bitmap image = (Bitmap)ImageHelper.LoadImage(imageName);
                prevImage = ImageHelper.MakeGrayscale(image);
            }

            SheetInspectorParam param = new SheetInspectorParam();

            List<string> resultList = new List<string>();
            for (int i = 0; i < 4; i++)
                resultList.AddRange(lines[i].Split('\t'));

            resultList.RemoveAll(s => s == "");

            if (resultList.Count < 8)
                return false;

            SpandTime = TimeSpan.Parse(resultList[0]);

            param.PoleParam.LowerThreshold = Convert.ToInt32(resultList[1]);
            param.PoleParam.UpperThreshold = Convert.ToInt32(resultList[2]);

            param.DielectricParam.LowerThreshold = Convert.ToInt32(resultList[3]);
            param.DielectricParam.UpperThreshold = Convert.ToInt32(resultList[4]);

            param.ShapeParam.DiffTolerence = Convert.ToSingle(resultList[5]);
            param.ShapeParam.UseHeightDiffTolerence = Convert.ToBoolean(resultList[6]);
            param.ShapeParam.HeightDiffTolerence = Convert.ToSingle(resultList[7]);

            lines = lines.Skip(5).ToArray();

            foreach (string line in lines)
            {
                resultList.Clear();
                resultList.AddRange(line.Split('\t'));

                string subImageName = Path.Combine(path, string.Format("{0}.bmp", resultList[1]));
                Bitmap subImage = null;
                if (File.Exists(subImageName) == true)
                    subImage = (Bitmap)ImageHelper.LoadImage(subImageName);

                DefectType defectType = SheetSubResult.GetDefectType(resultList[2]);

                SheetSubResult sheetSubResult;
                if (defectType == DefectType.Shape)
                    sheetSubResult = new ShapeResult();
                else
                    sheetSubResult = new SheetSubResult();

                sheetSubResult.Image = subImage;
                sheetSubResult.FromExportData(line);
                
                this.subResultList.Add(sheetSubResult);
            }
            return true;
        }
    }
}
