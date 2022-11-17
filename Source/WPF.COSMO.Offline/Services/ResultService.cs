using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using WPF.Base.Extensions;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Controls;
using WPF.COSMO.Offline.Converters;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.ViewModels;

namespace WPF.COSMO.Offline.Services
{
    public class ReportInfo
    {
        public Param_COSMO Param_COSMO { get; set; } = new Param_COSMO();
        public Section Section { get; set; } = new Section();
        public CosmoLotNoInfo LotNoInfo { get; set; } = new CosmoLotNoInfo();
        
        public int RightDefectNum { get; set; }
        public int LeftDefectNum { get; set; }

        public int InnerDefectNum { get; set; }

        [JsonIgnore]
        public int EdgeDefectNum => RightDefectNum + LeftDefectNum;

        [JsonIgnore]
        public int TotalDefectNum => RightDefectNum + LeftDefectNum + InnerDefectNum;

        public ReportInfo()
        {

        }

        public ReportInfo(CosmoLotNoInfo lotNoInfo, Section section, int rightDefectNum, int leftDefectNum, int innerDefectNum)
        {
            LotNoInfo = lotNoInfo;
            RightDefectNum = rightDefectNum;
            LeftDefectNum = leftDefectNum;
            InnerDefectNum = innerDefectNum;
        }
    }

    public class ResultSaveProcess : Observable
    {
        public ProcessUnit ProcessUnit { get; set; } = new ProcessUnit();

        double _scanImagePercentage;
        public double ScanImagePercentage
        {
            get => _scanImagePercentage;
            set => Set(ref _scanImagePercentage, value);
        }

        double _defectImagePercentage;
        public double DefectImagePercentage
        {
            get => _defectImagePercentage;
            set => Set(ref _defectImagePercentage, value);
        }
    }

    public class ResultLoadProcess : Observable
    {
        public ProcessUnit ReportUnit { get; set; } = new ProcessUnit();
        public ProcessUnit ResultUnit { get; set; } = new ProcessUnit();
        
        double _scanImagePercentage;
        public double ScanImagePercentage
        {
            get => _scanImagePercentage;
            set => Set(ref _scanImagePercentage, value);
        }

        double _defectImagePercentage;
        public double DefectImagePercentage
        {
            get => _defectImagePercentage;
            set => Set(ref _defectImagePercentage, value);
        }

        double _drawDefectPercentage;
        public double DrawDefectPercentage
        {
            get => _drawDefectPercentage;
            set => Set(ref _drawDefectPercentage, value);
        }
    }

    public delegate void ReportInfoLoadedDelegate(ReportInfo reportInfo);

    public static class ResultService
    {
        const string _reportInfoKey = "ReportInfo";
        const string _inspectResultKey = "InspectResult";
        const string _sizeInfoKey = "SizeInfo";

        public static EmptyDelegate Initialized;

        public static ReportInfoLoadedDelegate ReportInfoLoaded;
        public static EstimatedDelegate Estimated;
        
        public static InspectedDelegate Inspected;
        public static FilmGrabbedDelegate AxisImageLoaded;
        public static DrawDefectsDoneDelegate DrawDefectLoaded;
        
        public static EmptyDelegate LoadDone;

        public static async Task SaveResultAsync(ResultSaveProcess resultSaveProcess, ReportInfo reportInfo, InspectResult inspectResult)
        {
            //await Task.Run(() =>
            //{

            //});

            var directoryInfo = CreateResultFolder(reportInfo);

            await Task.Run(() =>
            {
                List<Task> taskList = new List<Task>();

                string defectPath = Path.Combine(directoryInfo.FullName, "Defect");
                double index = 0;
                foreach (var scanResult in inspectResult.ScanResultList)
                    foreach (var defect in scanResult.Defects)
                        defect.ImagePath = Path.Combine(defectPath, index++.ToString() + _fileExtension);

                taskList.Add(Task.Run(() =>
                {
                    resultSaveProcess.ProcessUnit.Processing = true;
                    directoryInfo.Save(_reportInfoKey, reportInfo);
                    directoryInfo.Save(_inspectResultKey, inspectResult);

                    resultSaveProcess.ProcessUnit.Success = true;
                }));

                //taskList.Add(SaveScanImage(directoryInfo, resultSaveProcess, inspectResult));
                taskList.Add(SaveDefectImage(directoryInfo, resultSaveProcess, inspectResult));

                Task.WaitAll(taskList.ToArray());
            });
        }

        static async Task SaveScanImage(DirectoryInfo directoryInfo, ResultSaveProcess resultSaveProcess, InspectResult inspectResult)
        {
            await Task.Run(() =>
            {
                double count = 0;
                double total = inspectResult.SourceImageList.Count;

                foreach (var image in inspectResult.SourceImageList)
                {
                    image.ImageSource.Save(directoryInfo, string.Format("{0}_{1}", image.X, image.Y));
                    count++;

                    resultSaveProcess.ScanImagePercentage = count / total * 100.0;
                }
            });
        }

        private const string _fileExtension = ".png";

        static async Task SaveDefectImage(DirectoryInfo directoryInfo, ResultSaveProcess resultSaveProcess, InspectResult inspectResult)
        {
            await Task.Run(() =>
            {
                string defectPath = Path.Combine(directoryInfo.FullName, "Defect");
                Directory.CreateDirectory(defectPath);

                DirectoryInfo defectDirectoryInfo = new DirectoryInfo(defectPath);

                double index = 0;
                double total = inspectResult.ScanResultList.Sum(scanResult => scanResult.Defects.Count);

                foreach (var scanResult in inspectResult.ScanResultList)
                {
                    foreach (var defect in scanResult.Defects)
                    {
                        defect.Image.Save(defectDirectoryInfo, index.ToString());
                        index++;
                        resultSaveProcess.DefectImagePercentage = index / total * 100.0;
                    }
                }
            });
        }

        public static DirectoryInfo CreateResultFolder(ReportInfo reportInfo)
        {
            string datePath = Path.Combine(PathSettings.Instance().Result, reportInfo.LotNoInfo.InspectStartTime.ToString("yyyyMMdd"));

            if (Directory.Exists(datePath) == false)
                Directory.CreateDirectory(datePath);

            int index = 1;
            string resultPath = Path.Combine(datePath, string.Format("{0}({1})_{2}", reportInfo.LotNoInfo.LotNo, index, reportInfo.LotNoInfo.ModelName));
            while (Directory.Exists(resultPath))
                resultPath = Path.Combine(datePath, string.Format("{0}({1})_{2}", reportInfo.LotNoInfo.LotNo, ++index, reportInfo.LotNoInfo.ModelName));

            Directory.CreateDirectory(resultPath);

            return new DirectoryInfo(resultPath);
        }
        
        public static Dictionary<CosmoLotNoInfo, DirectoryInfo> SearchInfos(DateTime startTime, DateTime endTime,
            List<KeyValuePair<string, string>?> coatingDeviceList, List<int?> coatingNoList,
            List<KeyValuePair<string, string>?> slitterDeviceList, List<int?> slitterNoList, List<int?> slitterLaneList)
        {
            var directoryInfo = new DirectoryInfo(PathSettings.Instance().Result);
            var infoList = new Dictionary<CosmoLotNoInfo, DirectoryInfo>();

            foreach (var dateDirectory in directoryInfo.GetDirectories())
            {
                if (DateTime.TryParseExact(dateDirectory.Name,
                                           "yyyyMMdd",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out DateTime temp) == false)
                    continue;

                foreach (var resultDirectory in dateDirectory.GetDirectories())
                {
                    if (resultDirectory.Name.Length < 14)
                        continue;

                    CosmoLotNoInfo info = CosmoLotNoInfo.Parse(resultDirectory.Name.Substring(0, 11));
                    if (info == null)
                        continue;
                    
                    if (info.ProductDate < startTime || info.ProductDate >= endTime.AddDays(1))
                        continue;

                    if (coatingDeviceList != null && coatingDeviceList.Contains(info.CoatingDevice) == false)
                        continue;

                    if (coatingNoList != null && coatingNoList.Contains(info.CoatingNo) == false)
                        continue;

                    if (slitterDeviceList != null && slitterDeviceList.Contains(info.SlitterDevice) == false)
                        continue;

                    if (slitterNoList != null && slitterNoList.Contains(info.SlitterNo) == false)
                        continue;

                    if (slitterLaneList != null && slitterLaneList.Contains(info.SlitterLane) == false)
                        continue;

                    if (resultDirectory.Name.Contains("_"))
                    {
                        var splited = resultDirectory.Name.Split('_');
                        if (splited.Count() > 1)
                        {
                            info.ModelName =  splited.Last();
                        }
                    }

                    infoList.Add(info, resultDirectory);
                }
            }

            return infoList;
        }

        public static IEnumerable<ReportInfo> LoadReportAsync(Dictionary<CosmoLotNoInfo, DirectoryInfo> dictionary)
        {
            List<ReportInfo> infoList = new List<ReportInfo>();

            foreach (var pair in dictionary)
            {
                var reportInfo = pair.Value.Read<ReportInfo>(_reportInfoKey);

                if (reportInfo != null)
                {
                    reportInfo.LotNoInfo = pair.Key;
                    infoList.Add(reportInfo);
                }
            }

            return infoList;
        }
        
        public static Dictionary<CosmoLotNoInfo, InspectResult> LoadResultAsync(Dictionary<CosmoLotNoInfo, DirectoryInfo> dictionary)
        {
            var results = new Dictionary<CosmoLotNoInfo, InspectResult>();

            foreach (var pair in dictionary)
            {
                var result = pair.Value.Read<InspectResult>(_inspectResultKey);

                if (result != null)
                    results[pair.Key] = result;
            }

            return results;
        }

        public static async Task LoadResultAsync(ResultLoadProcess resultLoadProcess, DirectoryInfo directoryInfo, CancellationToken token)
        {
            if (Initialized != null)
                Initialized();

            DefectJsonConverter defectJsonConverter = new DefectJsonConverter();

            resultLoadProcess.ReportUnit.Processing = true;

            ReportInfo reportInfo = directoryInfo.Read<ReportInfo>(_reportInfoKey);

            await Task.Run(() =>
            {
                if (reportInfo != null)
                {
                    if (ReportInfoLoaded != null)
                        ReportInfoLoaded(reportInfo);

                    resultLoadProcess.ReportUnit.Success = true;
                }
                else
                {
                    resultLoadProcess.ReportUnit.Fail = true;
                }

                if (token.IsCancellationRequested)
                    return;

                resultLoadProcess.ResultUnit.Processing = true;
                InspectResult inspectResult = directoryInfo.Read<InspectResult>(_inspectResultKey, defectJsonConverter);

                if (inspectResult == null)
                {
                    resultLoadProcess.ResultUnit.Fail = true;
                    return;
                }

                if (token.IsCancellationRequested)
                    return;

                if (Estimated != null)
                    Estimated(inspectResult.Lines);

                resultLoadProcess.ResultUnit.Success = true;

                List<Task> taskList = new List<Task>();
                taskList.Add(Task.Run(() =>
                {
                    double count = 0;
                    foreach (var sourceImage in inspectResult.SourceImageList)
                    {
                        if (token.IsCancellationRequested)
                            return;

                        sourceImage.ImageSource = ImageSourceExtensions.Read(directoryInfo, string.Format("{0}_{1}", sourceImage.X, sourceImage.Y));
                        if (AxisImageLoaded != null)
                            AxisImageLoaded(sourceImage);

                        resultLoadProcess.ScanImagePercentage = ++count / inspectResult.SourceImageList.Count * 100.0;
                    }
                }));

                taskList.Add(Task.Run(() =>
                {
                    double index = 0;

                    int total = inspectResult.ScanResultList.Sum(scanResult => scanResult.Defects.Count);

                    var defectDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, "Defect"));
                    foreach (var scanResult in inspectResult.ScanResultList)
                    {
                        foreach (var defect in scanResult.Defects)
                        {
                            if (token.IsCancellationRequested)
                                return;

                            string filePath = Path.Combine(directoryInfo.FullName, "Defect", index.ToString() + _fileExtension);
                            if (File.Exists(filePath))
                            {
                                defect.Image = ImageSourceExtensions.Read(defectDirectoryInfo, index.ToString());
                                defect.ImagePath = filePath;
                            }

                            resultLoadProcess.DefectImagePercentage = ++index / total * 100.0;
                        }

                        if (Inspected != null)
                            Inspected(scanResult);
                    }
                }));

                taskList.Add(Task.Run(() =>
                {
                    double count = 0;

                    foreach (var scanResult in inspectResult.ScanResultList)
                    {
                        if (token.IsCancellationRequested)
                            return;

                        if (DrawDefectLoaded != null)
                            DrawDefectLoaded(new AxisImageSource(scanResult.AxisPosition, scanResult.GetDefectImage()));

                        resultLoadProcess.DrawDefectPercentage = ++count / inspectResult.ScanResultList.Count * 100.0;
                    }
                }));

                if (token.IsCancellationRequested)
                    return;

                Task.WaitAll(taskList.ToArray());

                if (LoadDone != null)
                    LoadDone();
            });           
        }
    }
}
