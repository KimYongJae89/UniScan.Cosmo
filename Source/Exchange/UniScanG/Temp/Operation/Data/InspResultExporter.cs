//using DynMvp.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DynMvp.Devices.Comm;
//using DynMvp.InspData;
//using System.IO;
//using DynMvp.UI;
//using DynMvp.Base;
//using DynMvp.Vision;
//using UniEye.Base;
//using System.Drawing;
//using System.Threading;
//using System.Threading.Tasks;

//namespace UniScanG.Temp
//{
//    public class InspResultExporter : DataExporter
//    {
//        private MpisInspResultArchiver dataResult = new MpisInspResultArchiver();
//        public MpisInspResultArchiver DataResult
//        {
//            get { return dataResult; }
//        }

//        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
//        {
//            dataResult.Save(inspectionResult, cancellationToken);
//        }

//        public override bool UpdateResult(DynMvp.InspData.InspectionResult inspectionResult, PacketParser packetParser)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public class MpisInspResultArchiver : InspResultArchiver
//    {
//        public void Save(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
//        {
//            LogHelper.Debug(LoggerType.Operation, "MpisInspResultArchiver::Save Start");
//            Directory.CreateDirectory(inspectionResult.ResultPath);

//            string fileName = String.Format("{0}\\result.csv", inspectionResult.ResultPath);
//            if (Directory.Exists(inspectionResult.ResultPath) == false)
//                Directory.CreateDirectory(inspectionResult.ResultPath);

//            StringBuilder resultStringBuilder = new StringBuilder();

//            if (inspectionResult.ProbeResultList == null)
//                return;

//            if (inspectionResult.ProbeResultList.Count == 0)
//                return;

//            VisionProbeResult visionResult = (VisionProbeResult)inspectionResult.ProbeResultList[0];

//            if (visionResult.AlgorithmResult == null)
//                return;

//            DateTime exportStartTime = DateTime.Now;

//            SheetCheckerAlgorithmResult algorithmResult = (SheetCheckerAlgorithmResult)visionResult.AlgorithmResult;
//            resultStringBuilder.Append(String.Format("{0};{1};{2};{3};{4}",
//                inspectionResult.InspectionNo,
//                inspectionResult.Judgment.ToString(),
//                inspectionResult.InspectionStartTime.ToString("yyyy\\/MM\\/dd HH:mm:ss"),
//                //DateTime.Now.ToString("yyyy\\/MM\\/dd HH:mm:ss"),
//                algorithmResult.SubResultList.Count,
//                algorithmResult.Error.ToString()));

//            // Text first.
//            resultStringBuilder.AppendLine();
//            foreach (SheetCheckerSubResult subResult in algorithmResult.SubResultList)
//            {
//                if (subResult == null)
//                    continue;

//                resultStringBuilder.AppendLine(String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}",
//                    subResult.Index, // 0
//                    subResult.DefectType, // 1
//                    subResult.ResultRect.X, // 2
//                    subResult.ResultRect.Y,// 3
//                    subResult.ResultRect.Width,// 4 
//                    subResult.ResultRect.Height, // 5
//                    Math.Sqrt(subResult.Width * subResult.Height),// 6
//                    subResult.ShortResultMessage, // 7
//                    subResult.Width, // 8
//                    subResult.Height,// 9
//                    subResult.X,// 10
//                    subResult.Y, // 11
//                    subResult.Area // 12
//                    ));
//            }

//            File.WriteAllText(fileName, resultStringBuilder.ToString());

//            // Image Second.
//            try
//            {
//                Task tt = new Task(() =>
//            {
//                Parallel.ForEach(algorithmResult.SubResultList, f =>
//                //foreach (SheetCheckerSubResult subResult in algorithmResult.SubResultList)
//                {
//                    SheetCheckerSubResult subResult = (SheetCheckerSubResult)f;

//                    if (subResult.Image != null)
//                    {
//                        string subResultImageFile = Path.Combine(inspectionResult.ResultPath, String.Format("{0}.bmp", subResult.Index));
//                        ImageHelper.SaveImage(subResult.Image, subResultImageFile);

//                        if (UniEye.Base.Settings.MachineSettings.Instance().VirtualMode)
//                            File.Delete(subResultImageFile);
//                    }
//                }
//                );

//            }, cancellationToken);

//                if (cancellationToken.IsCancellationRequested == false)
//                    tt.Start();

//                tt.Wait(cancellationToken);
//                //bool ok = tt.Wait(400);
//            }
//            catch (OperationCanceledException)
//            {
//                LogHelper.Debug(LoggerType.Operation, "MpisInspResultArchiver::TaskCanceledException");
//            }
//            catch (AggregateException ex)
//            {
//                LogHelper.Debug(LoggerType.Operation, "MpisInspResultArchiver::TaskCanceledException");
//            }

//            LogHelper.Debug(LoggerType.Operation, "MpisInspResultArchiver::Save End");
//        }

//        public void GetProbeResult(DynMvp.InspData.InspectionResult inspectionResult)
//        {
//            string dataFile = String.Format("{0}\\result.csv", inspectionResult.ResultPath);

//            string[] lines = File.ReadAllLines(dataFile, Encoding.Default);

//            VisionProbeResult probeResult;
//            probeResult = (VisionProbeResult)ProbeResult.CreateProbeResult(ProbeType.Vision);

//            probeResult.StepNo = 0;
//            probeResult.GroupId = 0;
//            probeResult.TargetId = 0;
//            probeResult.TargetName = "";
//            probeResult.ProbeName = "";
//            probeResult.TargetType = "";
//            probeResult.Judgment = Judgment.Reject;

//            foreach (string line in lines)
//            {
//                string[] words = line.Split('=');
//                if (words[0].Trim() != "Defect")
//                    continue;

//                string[] valueWords = words[0].Trim().Split(',');

//                string defectType = words[0].Trim();
//                float fovPosX = Convert.ToSingle(words[1].Trim());
//                float fovPosY = Convert.ToSingle(words[2].Trim());
//                float width = Convert.ToSingle(words[3].Trim());
//                float height = Convert.ToSingle(words[4].Trim());

//                SubResult subResult = new SubResult();
//                subResult.ResultRect = new RotatedRect(fovPosX, fovPosY, width, height, 0);

//                probeResult.AlgorithmResult.AddSubResult(subResult);
//            }

//            if (probeResult.AlgorithmResult.SubResultList.Count == 0)
//                probeResult.Judgment = Judgment.Accept;

//            inspectionResult.AddProbeResult(probeResult);
//        }

//        public List<DynMvp.InspData.InspectionResult> Load(string dataPath, DateTime startDate, DateTime endDate)
//        {
//            DateTime dailyReportDate = new DateTime(startDate.Year, startDate.Month, startDate.Day);
//            DateTime loopEnd = endDate.Date + new TimeSpan(1, 0, 0, 0);

//            List<DynMvp.InspData.InspectionResult> inspectionResultList = new List<DynMvp.InspData.InspectionResult>();

//            for (; dailyReportDate < loopEnd; dailyReportDate += new TimeSpan(1, 0, 0, 0))
//            {
//                string shortDate = dailyReportDate.ToString("yyyy-MM-dd");
//                string searchPath = String.Format("{0}\\{1}", dataPath, shortDate);

//                if (Directory.Exists(searchPath) == false)
//                    continue;

//                string[] directoryNames = Directory.GetDirectories(searchPath);

//                foreach (string dirName in directoryNames)
//                {
//                    string defectPath = String.Format("{0}\\result.csv", dirName);

//                    try
//                    {
//                        DynMvp.InspData.InspectionResult inspectionResult = LoadInspResult(defectPath, startDate, endDate);

//                        if (inspectionResult != null)
//                            inspectionResultList.Add(inspectionResult);

//                    }
//                    catch (Exception ex)
//                    {
//                        LogHelper.Warn(LoggerType.Operation, "Fail to read result data. " + ex.Message);
//                    }
//                }
//            }

//            return inspectionResultList;
//        }

//        DynMvp.InspData.InspectionResult LoadInspResult(string dataPath, DateTime startDate, DateTime endDate)
//        {
//            if (File.Exists(dataPath) == false)
//                return null;

//            DynMvp.InspData.InspectionResult inspectionResult = new DynMvp.InspData.InspectionResult();

//            using (StreamReader reader = new StreamReader(dataPath))
//            {
//                reader.ReadLine(); // Skip
//                string[] words = reader.ReadLine().Split(new char[] { ',' });

//                if (words.Count() == 6)
//                {
//                    inspectionResult.ModelName = words[0].Trim();
//                    inspectionResult.InputBarcode = words[1].Trim();
//                    inspectionResult.InspectionNo = inspectionResult.InputBarcode;
//                    inspectionResult.InspectionStartTime = DateTime.Parse(words[2]);

//                    if (inspectionResult.InspectionStartTime < startDate || inspectionResult.InspectionStartTime >= endDate)
//                        return null;

//                    inspectionResult.Judgment = (Judgment)Enum.Parse(typeof(Judgment), words[3].Trim());
//                    inspectionResult.JobOperator = words[4].Trim();
//                }
//                else
//                {
//                    inspectionResult.ModelName = words[0].Trim();
//                    inspectionResult.InspectionNo = words[1].Trim();
//                    inspectionResult.InputBarcode = words[2].Trim();
//                    inspectionResult.InspectionStartTime = DateTime.Parse(words[3]);

//                    if (inspectionResult.InspectionStartTime < startDate || inspectionResult.InspectionStartTime >= endDate)
//                        return null;

//                    inspectionResult.Judgment = (Judgment)Enum.Parse(typeof(Judgment), words[4].Trim());
//                    inspectionResult.JobOperator = words[5].Trim();
//                }
//                inspectionResult.ResultPath = dataPath.Trim();
//            }

//            return inspectionResult;
//        }

//        public DynMvp.InspData.InspectionResult LoadInspResult(string dataPath)
//        {
//            string dataFile = String.Format("{0}\\result.csv", dataPath);

//            if (File.Exists(dataFile) == false)
//                return null;

//            Data.InspectionResult inspectionResult = new Data.InspectionResult();

//            SheetCheckerAlgorithmResult algorithmResult = new SheetCheckerAlgorithmResult();
//            inspectionResult.AddProbeResult(new VisionProbeResult(null, algorithmResult, null));
//            inspectionResult.ResultPath = dataPath;

//            using (StreamReader reader = new StreamReader(dataFile))
//            {
//                // first Line: Overall result
//                {
//                    string firstLine = reader.ReadLine();
//                    string[] tokens = firstLine.Split(';');

//                    inspectionResult.InspectionNo = tokens[0];
//                    inspectionResult.Judgment = (Judgment)Enum.Parse(typeof(Judgment), tokens[1]);
//                    inspectionResult.InspectionStartTime = DateTime.Parse(tokens[2]);
//                    //algorithmResult.defectCnt = int.Parse(tokens[3]);
//                    algorithmResult.Error = (Data.InspectionError)Enum.Parse(typeof(Data.InspectionError), tokens[4]);
//                }


//                // next Line: Defect Info
//                while (reader.EndOfStream == false)
//                {
//                    string line = reader.ReadLine();
//                    string[] tokens = line.Split(';');

//                    SheetCheckerSubResult subResult = new SheetCheckerSubResult();

//                    subResult.Index = int.Parse(tokens[0]);
//                    subResult.DefectType = (SheetDefectType)Enum.Parse(typeof(SheetDefectType), tokens[1]);

//                    float resultRectX = float.Parse(tokens[2]);
//                    float resultRectY = float.Parse(tokens[3]);
//                    float resultRectW = float.Parse(tokens[4]);
//                    float resultRectH = float.Parse(tokens[5]);
//                    subResult.ResultRect = new RotatedRect(resultRectX, resultRectY, resultRectW, resultRectH, 0);

//                    subResult.ShortResultMessage = tokens[7];

//                    subResult.Width = float.Parse(tokens[8]);
//                    subResult.Height = float.Parse(tokens[9]);
//                    subResult.X = float.Parse(tokens[10]);
//                    subResult.Y = float.Parse(tokens[11]);
//                    subResult.Area = int.Parse(tokens[12]);

//                    subResult.Image = ImageHelper.LoadImage(Path.Combine(dataPath, string.Format("{0}.bmp", subResult.Index)));

//                    subResult.BuildMessage();
//                    algorithmResult.AddSubResult(subResult);
//                }
//            }
//            inspectionResult.CalcDefectTypeCount();
//            return inspectionResult;
//        }
//    }

//    public class MPISTextProductOverviewDataExport : DataExporter
//    {
//        string resultPath;
//        object fileLock = new object();

//        public MPISTextProductOverviewDataExport(string resultPath)
//        {
//            this.resultPath = resultPath;
//        }

//        public override bool UpdateResult(DynMvp.InspData.InspectionResult inspectionResult, PacketParser packetParser)
//        {
//            return true;
//        }

//        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
//        {
//            Operation.Data.InspectionResult mpisInspectionResult = (Operation.Data.InspectionResult)inspectionResult;

//            DirectoryInfo resultFolder = new DirectoryInfo(inspectionResult.ResultPath);

//            if (resultFolder.Exists == false)
//                return;

//            string resultFile = String.Format("{0}\\OverView.csv", resultFolder.Parent.FullName);//

//            int blackDefectNum = 0;
//            int whiteDefectNum = 0;

//            foreach (ProbeResult probeResult in inspectionResult)
//            {
//                VisionProbeResult visionResult = (VisionProbeResult)probeResult;
//                AlgorithmResult algorithmResult = visionResult.AlgorithmResult;

//                if (algorithmResult == null)
//                    break;

//                foreach (SheetCheckerSubResult subResult in algorithmResult.SubResultList)
//                {
//                    if (subResult == null)
//                        continue;

//                    switch (subResult.DefectType)
//                    {
//                        case Data.SheetDefectType.BlackDefect:
//                            blackDefectNum++;
//                            break;
//                        case Data.SheetDefectType.WhiteDefect:
//                            whiteDefectNum++;
//                            break;
//                    }
//                }
//            }

//            string sheetIndex = inspectionResult.InspectionNo;
//            //string sheetIndex = inspectionResult.InspectionNo;

//            int totalNum = blackDefectNum + whiteDefectNum;

//            if (totalNum == 0)
//                return;

//            lock (fileLock)
//            {
//                FileStream fs = new FileStream(resultFile, FileMode.Append);

//                if (fs != null)
//                {
//                    fs.Seek(0, SeekOrigin.End);

//                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);

//                    string resultStr = String.Format("{0};{1};{2};{3};", sheetIndex, totalNum, blackDefectNum, whiteDefectNum);
//                    sw.WriteLine(resultStr);

//                    sw.Close();
//                    fs.Close();
//                }

//                //File.Copy(resultFile, string.Format("{0}.bak", resultFile));
//            }
//        }
//    }
//}
