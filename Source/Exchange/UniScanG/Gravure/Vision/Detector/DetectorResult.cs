using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.UI;
using System.Drawing;
using System.Diagnostics;
//using UniEye.Base.Settings;
using DynMvp.Data;
using System.Threading;
using System.Runtime.InteropServices;
using UniEye.Base;
using System.IO;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Data;
using UniScanG.Vision;
using UniScanG.Data;
using UniScan.Common.Settings;
using UniScan.Common;
using UniScan.Common.Data;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.Vision.Detector
{
    public class DetectorResult : SheetResult, IExportable
    {
        public DetectorResult()
        {
            resultCollector = new Gravure.Data.ResultCollector();
        }

        public override void Export(string path, CancellationToken cancellationToken)
        {
            DetectorParam detectorParam = AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName).Param as DetectorParam;
            AlgorithmSetting algorithmSetting = AlgorithmSetting.Instance();

            // 시트 1장에 대한 검사 결과 저장.
            string fileName = Path.Combine(path, string.Format("{0}.csv", "Result"));
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format("DETECTION TEACH"));
            stringBuilder.AppendLine(string.Format("MaximumDefectCount,{0}", detectorParam.MaximumDefectCount));//티칭값
            stringBuilder.AppendLine(string.Format("DetectThresholdBase,{0}", detectorParam.DetectThresholdBase));//티칭값
            stringBuilder.AppendLine(string.Format("MinBlackDefectArea,{0}", algorithmSetting.MinBlackDefectLength));//티칭값
            stringBuilder.AppendLine(string.Format("MinWhiteDefectArea,{0}", algorithmSetting.MinWhiteDefectLength));//티칭값
            stringBuilder.AppendLine(string.Format("Offset,{0},{1}", this.offsetFound.Width, this.offsetFound.Height));//티칭값

            stringBuilder.AppendLine(string.Format("DETECTION RESULT"));
            stringBuilder.AppendLine(string.Format("SpandTime,{0}", this.spandTime));//검사값
            stringBuilder.AppendLine(string.Format("SheetSize,{0},{1}", this.sheetSize.Width, sheetSize.Height));//검사값
            stringBuilder.AppendLine(string.Format("DefectCount,{0}", this.sheetSubResultList.Count));//검사값

            stringBuilder.AppendLine(string.Format("DEFECT INFO LIST"));
            stringBuilder.AppendLine(string.Format("CAM Index,Image Index,Defect Type 1,Defect Type 2,Defect Type 3," +
                "X,Real X,"+
                "Y,Real Y,"+
                "Width,Real Width,"+
                "Heigh,Real height,"+
                "LowerDiffValue,UpperDiffValue,Compactness,SubtractValue,FillRate"));

            IClientExchangeOperator client = (IClientExchangeOperator)SystemManager.Instance()?.ExchangeOperator;
            int camIndex = client == null ? 0 : client.GetCamIndex();
            for (int i = 0; i < SheetSubResultList.Count; i++)
            {
                Gravure.Data.SheetSubResult subResult = (Gravure.Data.SheetSubResult)SheetSubResultList[i];
                subResult.Index = i;
                subResult.CamIndex = camIndex;
                string exportString = subResult.ToExportData();
                exportString =exportString.Replace('\t', ',');
                stringBuilder.AppendLine(exportString);
            }
            File.AppendAllText(fileName, stringBuilder.ToString());

            //Parallel.For(0, SheetSubResultList.Count, idx =>
            for (int i = 0; i < SheetSubResultList.Count; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                Gravure.Data.SheetSubResult subResult = (Gravure.Data.SheetSubResult)SheetSubResultList[i];
                lock (subResult.Image)
                {
                    ImageHelper.SaveImage(subResult.Image, Path.Combine(path, string.Format("{0}.jpg", i)));
                    ImageHelper.SaveImage(subResult.BufImage, Path.Combine(path, string.Format("{0}B.jpg", i)));
                }
            }
            //);
        }

        public override bool Import(string path)
        {
            string fileName = Path.Combine(path, string.Format("{0}.csv", "Result"));

            if (File.Exists(fileName) == false)
                return false;

            string imageName = Path.Combine(path, "Prev.jpg");
            if (File.Exists(imageName) == true)
            {
                Bitmap image = (Bitmap)ImageHelper.LoadImage(imageName);
                if (image != null)
                {
                    if (image.PixelFormat == System.Drawing.Imaging.PixelFormat.Format24bppRgb)
                        prevImage = ImageHelper.MakeGrayscale(image);
                    else
                        prevImage = image;
                }
                image?.Dispose();
            }
            else
            {
                //LogHelper.Error(LoggerType.Error, string.Format("DetectorResult::Import - prevImage {0} is noe exist", imageName));
            }

            try
            {
                List<string> lines = File.ReadAllLines(fileName).ToList();

                int startIdxTeach = lines.FindIndex(f => f == "DETECTION TEACH");
                if (startIdxTeach >= 0)
                {
                    DetectorParam detectorParam = new DetectorParam();
                    detectorParam.MaximumDefectCount = int.Parse(lines[startIdxTeach + 1].Split(',')[1]);
                    detectorParam.DetectThresholdBase = int.Parse(lines[startIdxTeach + 2].Split(',')[1]);
                    detectorParam.MinBlackDefectLength = int.Parse(lines[startIdxTeach + 3].Split(',')[1]);
                    detectorParam.MinWhiteDefectLength = int.Parse(lines[startIdxTeach + 4].Split(',')[1]);

                    string[] offsetFoundedToken = lines[startIdxTeach + 5].Split(',');
                    this.offsetFound.Width = int.Parse(offsetFoundedToken[1]);
                    this.offsetFound.Height = int.Parse(offsetFoundedToken[2]);
                }

                int startIdxResult = lines.FindIndex(f => f == "DETECTION RESULT");
                if (startIdxResult >= 0)
                {
                    this.spandTime = TimeSpan.Parse(lines[startIdxResult + 1].Split(',')[1]);

                    string[] sheetSizeToken = lines[startIdxResult + 2].Split(',');
                    this.sheetSize.Width = float.Parse(sheetSizeToken[1]);
                    this.sheetSize.Height = float.Parse(sheetSizeToken[2]);
                }

                int startIdxList = lines.FindIndex(f => f == "DEFECT INFO LIST");
                if (startIdxList >= 0)
                {
                    int index = 0;
                    while (startIdxList < lines.Count)
                    {
                        index++;
                        int lineNo = startIdxList + index + 1;
                        if (lineNo >= lines.Count)
                            break;

                        string line = lines[startIdxList + index + 1];
                        string[] token = line.Split(new char[] { ',', '\t' });
                        try
                        {
                            //DefectType defectType = SheetSubResult.GetDefectType(token[2]);
                            Data.SheetSubResult ssr = new Data.SheetSubResult();
                            bool ok = ssr.FromExportData(line);

                            //string subImageName = Path.Combine(path, string.Format("{0}.jpg", token[1]));
                            //ssr.Image = (Bitmap)ImageHelper.LoadImage(subImageName);

                            //Task imageLoadTask = Task.Run(()=> ssr.Image = (Bitmap)ImageHelper.LoadImage(subImageName));
                            //imageLoadTaskList.Add(imageLoadTask);

                            if (ok == false)
                                break;

                            this.sheetSubResultList.Add(ssr);
                        }
                        catch
                        {
                            break;
                        }
                    }

                    Parallel.ForEach(this.sheetSubResultList, f =>
                    {
                        f.ImagePath = Path.Combine(path, string.Format("{0}.jpg", f.Index));
                        f.BufImagePath = Path.Combine(path, string.Format("{0}B.jpg", f.Index));
                    });

                    this.good = this.sheetSubResultList.Count == 0;
                }
            }
            catch { }
            return true;
        }
    }
}
