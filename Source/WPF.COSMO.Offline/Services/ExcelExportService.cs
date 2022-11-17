using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UniEye.Base.Settings;
using WPF.Base.Extensions;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Controls.ViewModel;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.ViewModels;

namespace WPF.COSMO.Offline.Services
{
    public class ExcelExportProcess : Observable
    {
        bool _isError;
        public bool IsError
        {
            get => _isError;
            set => Set(ref _isError, value);
        }

        string _error;
        public string Error
        {
            get => _error;
            set => Set(ref _error, value);
        }

        public ProcessUnit InitializeUnit { get; set; } = new ProcessUnit();
        public ProcessUnit LogUnit { get; set; } = new ProcessUnit();
        public Dictionary<long, ObservableTuple<ProcessUnit, double>> ThumbnailUnitDictionry { get; set; } = new Dictionary<long, ObservableTuple<ProcessUnit, double>>();

        public ProcessUnit SummaryUnit { get; set; } = new ProcessUnit();
        public ProcessUnit ChartUnit { get; set; } = new ProcessUnit();
        public ProcessUnit SaveUnit { get; set; } = new ProcessUnit();
        public Stopwatch Stopwatch { get; set; } = new Stopwatch();

        public ExcelExportProcess()
        {
            for (long i = 0; i < ExcelExportService.ThreadNum; i++)
                ThumbnailUnitDictionry.Add(i + 1, new ObservableTuple<ProcessUnit, double>(new ProcessUnit(), 0));
        }
    }

    public static class ExcelExportService
    {
        static CancellationToken _token;

        const int _threadNum = 4;
        public static int ThreadNum => _threadNum;

        const string _inspectTamplateName = "RawDataTemplate_CosmoOffline";
        const string _microscopeTamplateName = "RawDataTemplate_MicroScopeImage";
        const string _inspectTamplateName2 = "RawDataTemplate_CosmoOffline2";
        const string _extension = ".xlsx";

        static ExcelExportProcess _process;

        public static void Initialize(ExcelExportProcess process, CancellationToken token)
        {
            _process = process;
            _token = token;
        }

        private static object[,] GetLogCells(IEnumerable<Defect> defects, int logDataNum, List<DefectSectionSize> defectSectionSizes, Section section)
        {
            var defectDistanceList = section.DefectDistanceList.ToList();
            var defectSizeList = section.DefectSizeList.ToList();
            var inspectPositionList = section.InspectPositionList;

            object[,] logCells = new object[defects.Count(), logDataNum];

            int logDefectIndex = 1;
            int logRowIndex = 0;
            foreach (var defect in defects)
            {
                int cellIndex = 0;

                logCells[logRowIndex, cellIndex++] = logDefectIndex.ToString();
                logCells[logRowIndex, cellIndex++] = defect.CenterPt.X.ToString("0.000");
                logCells[logRowIndex, cellIndex++] = defect.CenterPt.Y.ToString("0.000");
                logCells[logRowIndex, cellIndex++] = defect.Area;
                logCells[logRowIndex, cellIndex++] = defect.Major;
                logCells[logRowIndex, cellIndex++] = defect.Minor;
                logCells[logRowIndex, cellIndex++] = defect.Min;
                logCells[logRowIndex, cellIndex++] = defect.Max;
                logCells[logRowIndex, cellIndex++] = defect.Mean;
                
                logDefectIndex++;
                logRowIndex++;

                //// Thumbnail
                float defectSize = (float)defect.Major;
                int sizeIndex = defectSizeList.FindIndex(x => defectSize < x);
                if (sizeIndex == -1)
                    sizeIndex = defectSizeList.Count;

                int sectionIndex = 0;
                
                if (defect is EdgeDefect)
                {
                    sectionIndex = 0;
                }
                else if (defect is InnerDefect)
                {
                    int minIndex = 0;
                    double minDist = double.MaxValue;
                    for (int i = 0; i < inspectPositionList.Count; i++)
                    {
                        double dist = Math.Abs(inspectPositionList[i] - (defect as InnerDefect).Distance);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            minIndex = i;
                        }
                    }

                    sectionIndex = defectDistanceList.Count + minIndex + 1;
                }
                else if (defect is SectionDefect)
                {
                    sectionIndex = defectDistanceList.FindIndex(x => ((IDistanceDefect)defect).Distance < x) + 1;
                }
                else
                {
                    sectionIndex = 0;
                }

                DefectSectionSize defectSectionSize = new DefectSectionSize();
                defectSectionSize.sizeIndex = sizeIndex;
                defectSectionSize.sectionIndex = sectionIndex;
                defectSectionSize.image = defect.Image;
                defectSectionSize.defectSize = defectSize;
                defectSectionSizes.Add(defectSectionSize);
            }

            return logCells;
        }

        private static object[,] GetSummaryCells(DefectViewMode defectViewMode, IEnumerable<DefectSectionSize> defectSectionSizes, Section section, bool isReportResult = false)
        {
            var defectDistanceList = section.DefectDistanceList;
            var defectSizeList = section.DefectSizeList;
            var inspectPositionList = section.InspectPositionList;

            int summaryColumn = 0;

            if (isReportResult == false)
            {
                switch (defectViewMode)
                {
                    case DefectViewMode.Total:
                        summaryColumn = defectDistanceList.Count + inspectPositionList.Count + 1;
                        break;
                    case DefectViewMode.Left:
                    case DefectViewMode.Right:
                    case DefectViewMode.Edge:
                        summaryColumn = defectDistanceList.Count + 1;
                        break;
                    case DefectViewMode.Inner:
                        summaryColumn = inspectPositionList.Count;
                        break;
                }
            }
            else
            {
                summaryColumn = 1;
            }

            object[,] summaryCells = new object[defectSizeList.Count + 1, summaryColumn];
           
            int rowNum = summaryCells.GetLength(0);
            int colNum = summaryCells.GetLength(1);

            for (int row = 0; row < rowNum; row++)
                for (int col = 0; col < colNum; col++)
                    summaryCells[row, col] = 0;

            if (defectViewMode == DefectViewMode.Inner)
            {
                foreach (var sizePair in defectSectionSizes.ToLookup(x => x.sizeIndex))
                    summaryCells[sizePair.Key, 0] = sizePair.Count();
            }
            else
            {
                foreach (var sectionPair in defectSectionSizes.ToLookup(x => x.sectionIndex))
                    foreach (var sizePair in sectionPair.ToLookup(x => x.sizeIndex))
                        summaryCells[sizePair.Key, sectionPair.Key] = sizePair.Count();
            }

            return summaryCells;
        }
        

        private static List<DefectSectionSize> CreateLogSheet(Worksheet logSheet, CosmoLotNoInfo lotNoInfo, IEnumerable<Defect> defects, Section section)
        {
            List<DefectSectionSize> defectSectionSizes = new List<DefectSectionSize>();

            if (lotNoInfo != null)
            {
                logSheet.Cells[4, 2] = lotNoInfo.InspectStartTime.ToString("yyyy/MM/dd HH:mm:ss");
                logSheet.Cells[4, 4] = lotNoInfo.InspectEndTime.ToString("yyyy/MM/dd HH:mm:ss");
                logSheet.Cells[4, 6] = lotNoInfo.LotNo;
                logSheet.Cells[4, 9] = defects.Count();
            }

            if (logSheet.ListObjects.Count > 0)
            {
                var logTable = logSheet.ListObjects[1] as ListObject;

                int logDataNum = 10;

                var logCells = GetLogCells(defects, logDataNum, defectSectionSizes, section);

                logSheet.get_Range((Range)(logSheet.Cells[logTable.Range.Row + 1, logTable.Range.Column]),
                    (Range)(logSheet.Cells[defects.Count() + logTable.Range.Row, logTable.Range.Column + logDataNum - 1])).Value = logCells;
            }

            return defectSectionSizes;
        }

        private static Task InsertPicture(Worksheet logSheet, DefectStorage defectStorage)
        {
            return Task.Run(() =>
            {
                var defectList = defectStorage.GetFilteredDefect().ToList();

                if (logSheet.ListObjects.Count > 0)
                {
                    var logTable = logSheet.ListObjects[1] as ListObject;

                    Range pictureColumn = logTable.DataBodyRange.Columns[logTable.DataBodyRange.Columns.Count];

                    Parallel.For(0, _threadNum, index =>
                    {
                        int rowNum = defectList.Count / _threadNum;
                        int rowIndex = rowNum * index;
                        if (rowNum == 0)
                        {
                            if (index != 0)
                            {
                                _process.ThumbnailUnitDictionry[index + 1].Item1.Success = true;
                                return;
                            }

                            rowNum = defectList.Count;
                        }
                        else if (index == _threadNum - 1)
                        {
                            rowNum += defectList.Count % _threadNum;
                        }

                        _process.ThumbnailUnitDictionry[index + 1].Item1.Processing = true;

                        double count = 0;
                        for (int i = rowIndex; i < rowIndex + rowNum; i++)
                        {
                            if (_token.IsCancellationRequested)
                            {
                                _process.ThumbnailUnitDictionry[index + 1].Item1.Fail = true;
                                return;
                            }

                            Range cell = pictureColumn.Rows[i + 1];
                            if (defectList[i].ImagePath != null)
                            {
                                //logSheet.Shapes.AddPicture(defectStorage.Defects[i].ImagePath, MsoTriState.msoTrue, MsoTriState.msoFalse, cell.Left + 1, cell.Top + 1, cell.Width - 2, cell.Height - 2);
                                var shape = logSheet.Shapes.AddShape(MsoAutoShapeType.msoShapeRectangle, cell.Left + 1, cell.Top + 1, cell.Width - 2, cell.Height - 2);
                                shape.Fill.UserPicture(defectList[i].ImagePath);
                            }

                            _process.ThumbnailUnitDictionry[index + 1].Item2 = (++count / rowNum) * 100.0;
                        }

                        _process.ThumbnailUnitDictionry[index + 1].Item1.Success = true;
                    });
                }
            });
        }
        private static void CreateModelSheet(Worksheet modelSheet, Section section, Dictionary<CosmoLotNoInfo, InspectResult> dic)
        {
            if (modelSheet.ListObjects.Count > 0)
            {
                var modelTable = modelSheet.ListObjects[1] as ListObject;
                var sizeList = section.DefectSizeList;

                for (int i = 0; i < sizeList.Count; i++)
                    modelSheet.Cells[modelTable.Range.Row, modelTable.Range.Column + i + 1] = sizeList[i].ToString();

                modelSheet.Cells[modelTable.Range.Row, modelTable.Range.Column + sizeList.Count + 1] = "Over";
                modelSheet.Cells[modelTable.Range.Row, modelTable.Range.Column + sizeList.Count + 2] = "Total";
                modelSheet.Cells[modelTable.Range.Row, modelTable.Range.Column + sizeList.Count + 3] = "Count";
                modelSheet.Cells[modelTable.Range.Row, modelTable.Range.Column + sizeList.Count + 4] = $"< {SectionService.Settings.SizeStatistics}";
                modelSheet.Cells[modelTable.Range.Row, modelTable.Range.Column + sizeList.Count + 5] = $"{SectionService.Settings.SizeStatistics} <=";

                var grouped = dic.GroupBy(d => d.Key.ModelName);

                object[,] lotCells = new object[grouped.Count() * 2 + dic.Count, sizeList.Count + 6];

                int index = 0;
                foreach (var group in grouped)
                {
                    lotCells[index, 0] = $"{group.Key} Total";
                    lotCells[index + 1, 0] = $"{group.Key} Avg";

                    var gSorted = group.SelectMany(g => g.Value.ScanResultList.SelectMany(s => s.Defects)).OrderBy(d => d.Major);
                    var gTotal = gSorted.Count();

                    var gCount = 0;
                    var gPrev = 0;

                    double groupNum = group.Count();

                    for (int i = 0, j = 1; i < sizeList.Count; i++, j++)
                    {
                        gPrev = gSorted.Skip(gCount).TakeWhile(d => d.Major < sizeList[i]).Count();

                        lotCells[index, j] = gPrev;
                        lotCells[index + 1, j] = gPrev / groupNum;
                        gCount += gPrev;
                    }

                    lotCells[index, sizeList.Count + 1] = gTotal - gCount;
                    lotCells[index, sizeList.Count + 2] = gTotal;
                    lotCells[index, sizeList.Count + 3] = group.Count();

                    double statisticsSizeMinus1 = 0;
                    for(int i = 0; i < sizeList.Count; i++)
                    {
                        if(SectionService.Settings.SizeStatistics <= sizeList[i])
                        {
                            if (i != 0)
                            {
                                statisticsSizeMinus1 = sizeList[i - 1];
                                break;
                            }
                            else
                            {
                                statisticsSizeMinus1 = sizeList[0];
                                break;
                            }
                        }
                    }

                    var lowers = gSorted.Where(d => d.Major <= statisticsSizeMinus1).Count();

                    lotCells[index, sizeList.Count + 4] = lowers;
                    lotCells[index, sizeList.Count + 5] = gTotal - lowers;

                    lotCells[index + 1, sizeList.Count + 1] = (gTotal - gCount) / groupNum;
                    lotCells[index + 1, sizeList.Count + 2] = gTotal / groupNum;
                    lotCells[index + 1, sizeList.Count + 4] = lowers / groupNum;
                    lotCells[index + 1, sizeList.Count + 5] = (gTotal - lowers) / groupNum;

                    index += 2;

                    foreach (var lot in group)
                    {
                        lotCells[index, 0] = lot.Key.LotNo;

                        var lSorted = lot.Value.ScanResultList.SelectMany(s => s.Defects).OrderBy(d => d.Major);
                        var lTotal = lSorted.Count();

                        var lCount = 0;
                        var lPrev = 0;

                        for (int i = 0, j = 1; i < sizeList.Count; i++, j++)
                        {
                            lPrev = lSorted.Skip(lCount).TakeWhile(d => d.Major < sizeList[i]).Count();

                            lotCells[index, j] = lPrev;
                            lCount += lPrev;
                        }

                        var eachLowers = lSorted.Where(d => d.Major <= statisticsSizeMinus1).Count();

                        lotCells[index, sizeList.Count + 1] = lTotal - lCount;
                        lotCells[index, sizeList.Count + 2] = lTotal;
                        lotCells[index, sizeList.Count + 4] = eachLowers;
                        lotCells[index, sizeList.Count + 5] = lTotal - eachLowers;

                        index++;
                    }
                }

                modelSheet.get_Range((Range)(modelSheet.Cells[modelTable.Range.Row + 1, modelTable.Range.Column]),
                    (Range)(modelSheet.Cells[grouped.Count() * 2 + dic.Count + modelTable.Range.Row, modelTable.Range.Column + sizeList.Count + 5])).Value = lotCells;
            }
        }
        private static void CreateLotSheet(Worksheet lotSheet, Section section, Dictionary<CosmoLotNoInfo, InspectResult> dic)
        {
            if (lotSheet.ListObjects.Count > 0)
            {
                var lotTable = lotSheet.ListObjects[1] as ListObject;
                var sizeList = section.DefectSizeList;

                for (int i= 0; i < sizeList.Count; i++)
                    lotSheet.Cells[lotTable.Range.Row, lotTable.Range.Column + i + 1] = sizeList[i].ToString();

                lotSheet.Cells[lotTable.Range.Row, lotTable.Range.Column + sizeList.Count + 1] = "Over";
                lotSheet.Cells[lotTable.Range.Row, lotTable.Range.Column + sizeList.Count + 2] = "Total";

                object[,] lotCells = new object[dic.Count(), sizeList.Count + 3];

                int index = 0;
                foreach (var tuple in dic)
                {
                    lotCells[index, 0] = tuple.Key.LotNo;
                    var sorted = tuple.Value.ScanResultList.SelectMany(s => s.Defects).OrderBy(d => d.Major);
                    var total = sorted.Count();

                    var count = 0;
                    var prev = 0;

                    for (int i = 0, j = 1; i < sizeList.Count; i++, j++)
                    {
                        prev = sorted.Skip(count).TakeWhile(d => d.Major < sizeList[i]).Count();
                        
                        lotCells[index, j] = prev;
                        count += prev;
                    }

                    lotCells[index, sizeList.Count + 1] = total - count;
                    lotCells[index, sizeList.Count + 2] = total;

                    index++;
                }

                lotSheet.get_Range((Range)(lotSheet.Cells[lotTable.Range.Row + 1, lotTable.Range.Column]),
                    (Range)(lotSheet.Cells[dic.Count() + lotTable.Range.Row, lotTable.Range.Column + sizeList.Count + 2])).Value = lotCells;
            }
        }

        private static void CreateMonthlySummarySheet(Worksheet summarySheet, Section section, Dictionary<DateTime, List<InspectResult>> dic)
        {
            if (summarySheet.ListObjects.Count > 0)
            {
                var summaryTable = summarySheet.ListObjects[1] as ListObject;
                var sizeList = section.DefectSizeList;

                for (int i = 0; i < sizeList.Count; i++)
                    summarySheet.Cells[summaryTable.Range.Row + i + 1, summaryTable.Range.Column] = sizeList[i].ToString();

                summarySheet.Cells[summaryTable.Range.Row + sizeList.Count + 1, summaryTable.Range.Column] = "Over" ;
                summarySheet.Cells[summaryTable.Range.Row + sizeList.Count + 2, summaryTable.Range.Column] = "Total";
                summarySheet.Cells[summaryTable.Range.Row + sizeList.Count + 3, summaryTable.Range.Column] = "Sheet";

                int index = summaryTable.Range.Column + 1;
                var groups = dic.GroupBy(d => new DateTime(d.Key.Year, d.Key.Month, 1)).OrderBy(o => o.Key);
                var dictionary = new Dictionary<DateTime, List<InspectResult>>();
                foreach (var g in groups)
                {
                    summarySheet.Cells[summaryTable.Range.Row, index++] = "T " + g.Key.ToString("yy.MM");
                    summarySheet.Cells[summaryTable.Range.Row, index++] = "A " +g.Key.ToString("yy.MM");
                    dictionary.Add(g.Key, new List<InspectResult>());
                    foreach (var pair in g)
                        dictionary[g.Key].AddRange(pair.Value);
                }

                summarySheet.Cells[summaryTable.Range.Row, index++] = "Total";
                summarySheet.Cells[summaryTable.Range.Row, index++] = "Average";

                var countDictionary = new Dictionary<DateTime, List<double>>();
                var defectNumDicionary = new List<double>();
                var resultNumDictionary = new Dictionary<DateTime, int>();
                var avgList = new List<double>();
                var totalList = new List<double>();
                var totalNum = 0;
                for (int i = 0; i < sizeList.Count + 1; i++)
                {
                    totalList.Add(0);
                    avgList.Add(0);
                }
                
                foreach (var pair in dictionary)
                {
                    countDictionary.Add(pair.Key, new List<double>());
                    for (int i = 0; i < sizeList.Count + 1; i++)
                        countDictionary[pair.Key].Add(0);

                    resultNumDictionary.Add(pair.Key, 0);
                    foreach (var result in pair.Value)
                    {
                        foreach (var scanResult in result.ScanResultList)
                        {
                            foreach (var defect in scanResult.Defects)
                            {
                                bool isFounded = false;
                                for (int i = 0; i < sizeList.Count; i++)
                                {
                                    if (defect.Major <= sizeList[i])
                                    {
                                        countDictionary[pair.Key][i]++;
                                        totalList[i]++;
                                        isFounded = true;
                                        break;
                                    }
                                }

                                if (isFounded == false)
                                {
                                    countDictionary[pair.Key][sizeList.Count]++;
                                    totalList[sizeList.Count]++;
                                }
                            }
                        }
                    }

                    totalNum += pair.Value.Count;

                    resultNumDictionary[pair.Key] = pair.Value.Count;
                }

                for (int i = 0; i < totalList.Count; i++)
                {
                    avgList[i] = totalList[i] / totalNum;
                }

                object[,] summaryCells = new object[sizeList.Count + 3, (countDictionary.Count * 2) + 2];
                int column = 0;
                foreach (var pair in countDictionary)
                {
                    for (int i = 0; i <= sizeList.Count; i++)
                        summaryCells[i, column] = pair.Value[i];

                    summaryCells[sizeList.Count + 1, column] = pair.Value.Sum();

                    summaryCells[sizeList.Count + 2, column] = resultNumDictionary[pair.Key];
                    column++;

                    for (int i = 0; i < sizeList.Count + 1; i++)
                        summaryCells[i, column] = pair.Value[i] / resultNumDictionary[pair.Key];

                    summaryCells[sizeList.Count + 1, column] = pair.Value.Sum() / resultNumDictionary[pair.Key];

                    column++;
                }

                for (int i = 0; i <= sizeList.Count; i++)
                    summaryCells[i, column] = totalList[i];

                summaryCells[sizeList.Count + 1, column] = totalList.Sum();

                summaryCells[sizeList.Count + 2, column] = totalNum;
                
                column++;

                for (int i = 0; i <= sizeList.Count; i++)
                    summaryCells[i, column] = avgList[i];

                summaryCells[sizeList.Count + 1, column] = totalList.Sum() / totalNum;

                int dataColumn = summaryTable.Range.Column + 1;
                int monthColumn = (countDictionary.Count * 2) + 1;

                Range dataStartRange = (Range)(summarySheet.Cells[summaryTable.DataBodyRange.Row, dataColumn]);
                Range dataEndRange = (Range)(summarySheet.Cells[summaryTable.DataBodyRange.Row + summaryTable.DataBodyRange.Rows.Count - 1, dataColumn + monthColumn]);

                Range dataRange = summarySheet.get_Range(dataStartRange, dataEndRange);

                dataRange.Value = summaryCells;

            }
        }

        private static Tuple<Range, Range> CreateSummarySheet(Worksheet summarySheet, Section section, DefectViewMode viewMode, IEnumerable<DefectSectionSize> defectSectionSizes, bool isReportResult = false)
        {
            if (summarySheet.ListObjects.Count > 0)
            {
                var summaryTable = summarySheet.ListObjects[1] as ListObject;
                
                var sizeList = section.DefectSizeList;
                var sectionList = section.DefectDistanceList;
                var positionList = section.InspectPositionList;

                for (int i = 0; i < sizeList.Count; i++)
                    summarySheet.Cells[summaryTable.Range.Row + i + 1, summaryTable.Range.Column] = sizeList[i];

                summarySheet.Cells[summaryTable.Range.Row + sizeList.Count + 1, summaryTable.Range.Column] = "Over";
                summarySheet.Cells[summaryTable.Range.Row + sizeList.Count + 2, summaryTable.Range.Column] = "Total";

                int dataColumn = summaryTable.Range.Column + 2;

                int sectionColumn = 0;
                if (isReportResult == false)
                {
                    switch (viewMode)
                    {
                        case DefectViewMode.Total:
                            summarySheet.Cells[summaryTable.HeaderRowRange.Row, dataColumn] = "Edge";

                            for (int i = 0; i < sectionList.Count; i++)
                                summarySheet.Cells[summaryTable.HeaderRowRange.Row, i + dataColumn + 1] = sectionList[i];

                            for (int i = 0; i < positionList.Count; i++)
                                summarySheet.Cells[summaryTable.HeaderRowRange.Row, dataColumn + sectionList.Count + i + 1] = positionList[i];

                            sectionColumn = sectionList.Count + positionList.Count + 1;
                            break;
                        case DefectViewMode.Left:
                        case DefectViewMode.Right:
                        case DefectViewMode.Edge:
                            summarySheet.Cells[summaryTable.HeaderRowRange.Row, dataColumn] = "Edge";

                            for (int i = 0; i < sectionList.Count; i++)
                                summarySheet.Cells[summaryTable.HeaderRowRange.Row, i + dataColumn + 1] = sectionList[i];

                            sectionColumn = sectionList.Count + 1;
                            break;
                        case DefectViewMode.Inner:
                            for (int i = 0; i < positionList.Count; i++)
                                summarySheet.Cells[summaryTable.HeaderRowRange.Row, dataColumn + i] = positionList[i];

                            sectionColumn = positionList.Count;
                            break;
                    }
                }
                else
                {
                    summarySheet.Cells[summaryTable.HeaderRowRange.Row, dataColumn] = "Total";
                    sectionColumn = 1;
                }

                var summaryCells = GetSummaryCells(viewMode, defectSectionSizes, section, isReportResult);

                Range dataStartRange = (Range)(summarySheet.Cells[summaryTable.DataBodyRange.Row, dataColumn]);
                Range dataEndRange = (Range)(summarySheet.Cells[summaryTable.DataBodyRange.Row + summaryTable.DataBodyRange.Rows.Count - 2, dataColumn + sectionColumn - 1]);

                Range dataRange = summarySheet.get_Range(dataStartRange, dataEndRange);

                dataRange.Value = summaryCells;

                Range sumRange = summaryTable.DataBodyRange.Columns[2];
                for (int i = 1; i < sumRange.Rows.Count + 1; i++)
                    sumRange.Rows[i].Formula = string.Format("=Sum({0})", dataRange.Rows[i].Address(false, false));

                Range dataTotalStartRange = (Range)(summarySheet.Cells[summaryTable.DataBodyRange.Row + summaryTable.DataBodyRange.Rows.Count - 1, dataColumn]);
                Range dataTotalEndRange = (Range)(summarySheet.Cells[summaryTable.DataBodyRange.Row + summaryTable.DataBodyRange.Rows.Count - 1, dataColumn + sectionColumn - 1]);
                Range totalRange = summarySheet.get_Range(dataTotalStartRange, dataTotalEndRange);

                for (int i = 1; i < totalRange.Columns.Count + 1; i++)
                    totalRange.Columns[i].Formula = string.Format("=Sum({0})", dataRange.Columns[i].Address(false, false));

                Range chartStartRange1 = (Range)(summarySheet.Cells[summaryTable.Range.Row, 1]);
                Range chartEndRange1 = (Range)(summarySheet.Cells[summaryTable.Range.Row + summaryTable.Range.Rows.Count - 2, 1]);

                Range chartStartRange2 = (Range)(summarySheet.Cells[summaryTable.Range.Row, dataColumn]);
                Range chartEndRange2 = (Range)(summarySheet.Cells[summaryTable.Range.Row + summaryTable.Range.Rows.Count - 2, dataColumn + sectionColumn - 1]);

                return new Tuple<Range, Range>(summarySheet.get_Range(chartStartRange1, chartEndRange1), summarySheet.get_Range(chartStartRange2, chartEndRange2));
            }

            return null;
        }

        public static bool InspectResultExport(Dictionary<CosmoLotNoInfo, InspectResult> resultDictionary, Dictionary<DateTime, List<InspectResult>> dic, Section section)
        {
            bool result;

            List<Defect> defects = new List<Defect>();

            foreach (var list in dic.Values)
            {
                foreach (var inspectResult in list)
                {
                    foreach (var scanResult in inspectResult.ScanResultList)
                    {
                        defects.AddRange(scanResult.Defects);
                    }
                }
            }
                

            string templateFilePath = Path.Combine(PathSettings.Instance().Config, _inspectTamplateName2 + _extension);
            if (File.Exists(templateFilePath) == false)
                return false;

            var excelApp = new Application();
            var workbook = excelApp.Workbooks.Open(templateFilePath);
            Worksheet logSheet = workbook.Worksheets.get_Item("LOG");
            Worksheet summarySheet = workbook.Worksheets.get_Item("Summary");
            Worksheet lotSheet = workbook.Worksheets.get_Item("Lot");
            Worksheet modelSheet = workbook.Worksheets.get_Item("Model");

            if (logSheet == null || summarySheet == null || lotSheet == null)
            {
                if (logSheet == null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(logSheet);

                if (summarySheet == null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(summarySheet);

                if (lotSheet == null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(lotSheet);

                if (modelSheet == null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(modelSheet);

                workbook.Close(false);
                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                return false;
            }

            try
            {
                string filePath = String.Format(@"{0}\{1}.xlsx", PathSettings.Instance().ExcelResult, DateTime.Now.ToString("yyyyMMddhhmmss"));
                
                var defectSectionSizes = CreateLogSheet(logSheet, null, defects, section);

                CreateModelSheet(modelSheet, section, resultDictionary);
                CreateLotSheet(lotSheet, section, resultDictionary);
                CreateMonthlySummarySheet(summarySheet, section, dic);
                
                workbook.SaveAs(filePath);

                System.Diagnostics.Process.Start(PathSettings.Instance().ExcelResult);

                return true;
            }
            catch (Exception e)
            {
                result = false;
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(logSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(summarySheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(lotSheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(modelSheet);
                
                workbook.Close(false);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);

                excelApp.Quit();

                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }

            return result;
        }

        public static async Task<bool> InspectResultExport(DefectStorage defectStorage)
        {
            return await Task.Run(async() =>
            {
                bool result;

                _process.InitializeUnit.Processing = true;

                string templateFilePath = Path.Combine(PathSettings.Instance().Config, _inspectTamplateName + _extension);
                if (File.Exists(templateFilePath) == false)
                {
                    _process.InitializeUnit.Fail = true;
                    return false;
                }
                
                var excelApp = new Application();
                var workbook = excelApp.Workbooks.Open(templateFilePath);
                Worksheet logSheet = workbook.Worksheets.get_Item("LOG");
                Worksheet summarySheet = workbook.Worksheets.get_Item("Summary");
                Worksheet chartSheet = workbook.Worksheets.get_Item("Chart");

                if (logSheet == null || summarySheet == null || chartSheet == null)
                {
                    if (logSheet == null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(logSheet);

                    if (summarySheet == null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(summarySheet);

                    if (chartSheet == null)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(chartSheet);

                    workbook.Close(false);
                    excelApp.Quit();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                    _process.InitializeUnit.Fail = true;
                    return false;
                }

                _process.InitializeUnit.Success = true;

                try
                {
                    _process.Stopwatch.Start();

                    string savePath = String.Format(@"{0}\{1}", PathSettings.Instance().ExcelResult, DateTime.Now.ToString("yyyyMMdd"));

                    if (!Directory.Exists(savePath))
                        Directory.CreateDirectory(savePath);

                    string filePath = String.Format(@"{0}\{1}_{2}.xlsx", savePath, defectStorage.LotNoInfo.LotNo, defectStorage.DefectViewMode);

                    //-------------------------------------------------------------------------------------------
                    // LOG Sheet
                    _process.LogUnit.Processing = true;

                    IEnumerable<Defect> defects = defectStorage.GetFilteredDefect();

                    //foreach (Defect defect in defects)
                    //{
                    //    PointF defectCenter = defect.CenterPt;

                    //    defectCenter.X = (float)(defect.CenterPt.X / 1000.0f);
                    //    defectCenter.Y = (float)(defect.CenterPt.Y / 1000.0f);

                    //    defect.CenterPt = defectCenter;
                    //}

                    var defectSectionSizes = CreateLogSheet(logSheet, defectStorage.LotNoInfo, defects, defectStorage.Section);
                    if (_token.IsCancellationRequested)
                    {
                        _process.LogUnit.Fail = true;
                        return false;
                    }

                    _process.LogUnit.Success = true;

                    await InsertPicture(logSheet, defectStorage);
                    if (_token.IsCancellationRequested)
                        return false;

                    //-------------------------------------------------------------------------------------------
                    // Summary Sheet
                    _process.SummaryUnit.Processing = true;

                    var tuple = CreateSummarySheet(summarySheet, defectStorage.Section, defectStorage.DefectViewMode, defectSectionSizes);
                    if (_token.IsCancellationRequested)
                    {
                        _process.SummaryUnit.Fail = true;
                        return false;
                    }

                    _process.SummaryUnit.Success = true;

                    //-------------------------------------------------------------------------------------------
                    // Chart Sheet
                    _process.ChartUnit.Processing = true;

                    Range unionRange = excelApp.Union(tuple.Item1, tuple.Item2);
                    ChartObjects chartObjects = (ChartObjects)chartSheet.ChartObjects(Type.Missing);
                    if (unionRange != null && chartObjects.Count >= 2)
                    {
                        ChartObject sectionObject = chartObjects.Item(1);
                        sectionObject.Chart.SetSourceData(unionRange);

                        ChartObject sizeObject = chartObjects.Item(2);
                        sizeObject.Chart.SetSourceData(unionRange, Microsoft.Office.Interop.Excel.XlRowCol.xlRows);
                    }

                    if (_token.IsCancellationRequested)
                    {
                        _process.ChartUnit.Fail = true;
                        return false;
                    }

                    _process.ChartUnit.Success = true;

                    //-------------------------------------------------------------------------------------------
                    // Save
                    _process.SaveUnit.Processing = true;
                    if (_token.IsCancellationRequested)
                    {
                        _process.SaveUnit.Fail = true;
                        return false;
                    }
                        
                    workbook.SaveAs(filePath);
                    _process.SaveUnit.Success = true;

                    System.Diagnostics.Process.Start(savePath);

                    return true;
                }
                catch (Exception e)
                {
                    _process.SaveUnit.Fail = true;

                    _process.IsError = true;
                    _process.Error = e.Message;

                    result = false;
                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(logSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(summarySheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(chartSheet);

                    workbook.Close(false);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);

                    excelApp.Quit();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }

                return result;
            });
        }

        private static object[,] GetMicroscopeLogCell(List<Tuple<int, int, string>> clipImageTuple)
        {
            object[,] logCells = new object[clipImageTuple.Count(), 4];

            int logDefectIndex = 1;
            int logRowIndex = 0;
            foreach (var pair in clipImageTuple)
            {
                int cellIndex = 0;

                logCells[logRowIndex, cellIndex++] = logDefectIndex.ToString();
                cellIndex++;
                //logCells[logRowIndex, cellIndex++] = pair.Item3;
                logCells[logRowIndex, cellIndex++] = pair.Item1;
                logCells[logRowIndex, cellIndex++] = pair.Item2;

                logDefectIndex++;
                logRowIndex++;
            }

            return logCells;
        }

        private static void CreateMicroscopeLogSheet(Worksheet logSheet, CosmoLotNoInfo cosmoLotNoInfo, List<Tuple<int, int, string>> clipImageTuple)
        {
            if (cosmoLotNoInfo != null)
                logSheet.Cells[4, 2] = cosmoLotNoInfo.LotNo;

            if (logSheet.ListObjects.Count > 0)
            {
                var logTable = logSheet.ListObjects[1] as ListObject;
                var logCells = GetMicroscopeLogCell(clipImageTuple);

                logSheet.Range[
                    logSheet.Cells[logTable.Range.Row + 1, logTable.Range.Column],
                    logSheet.Cells[logTable.Range.Row + clipImageTuple.Count, logTable.Range.Column + 3]]
                    .Value = logCells;

                Range pictureColumn = logTable.DataBodyRange.Columns[2];

                for (int i = 0; i < clipImageTuple.Count(); i++)
                {
                    Range cell = pictureColumn.Rows[i + 1];
                    if (clipImageTuple[i].Item3 != null)
                    {
                        var shape = logSheet.Shapes.AddShape(MsoAutoShapeType.msoShapeRectangle, cell.Left + 1, cell.Top + 1, cell.Width - 2, cell.Height - 2);
                        shape.Fill.UserPicture(clipImageTuple[i].Item3);
                    }
                }
            }
        }


        public static async Task ClipImageExport(CosmoLotNoInfo cosmoLotNoInfo, List<Tuple<int, int, ImageSource>> clipImageTuple)
        {
            await Task.Run(() =>
            {
                string templateFilePath = Path.Combine(PathSettings.Instance().Config, _microscopeTamplateName + _extension);
                if (File.Exists(templateFilePath) == false)
                    return;

                var excelApp = new Application();
                var workbook = excelApp.Workbooks.Open(templateFilePath);
                Worksheet logSheet = workbook.Worksheets.get_Item("LOG");

                if (logSheet == null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(logSheet);

                    workbook.Close(false);
                    excelApp.Quit();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                    return;
                }

                try
                {
                    string savePath1 = String.Format(@"{0}\{1}", PathSettings.Instance().RemoteResult, DateTime.Now.ToString("yyyyMMdd"));

                    if (!Directory.Exists(savePath1))
                        Directory.CreateDirectory(savePath1);

                    string savePath2 = savePath1;
                    if (cosmoLotNoInfo != null)
                        String.Format(@"{0}\{1}", savePath1, cosmoLotNoInfo?.LotNo);

                    if (!Directory.Exists(savePath2))
                        Directory.CreateDirectory(savePath2);

                    string savePath3 = String.Format(@"{0}\{1}", savePath2, DateTime.Now.ToString("hhmmss"));

                    if (!Directory.Exists(savePath3))
                        Directory.CreateDirectory(savePath3);

                    string filePath = String.Format(@"{0}\Microscope", savePath3);

                    var imagePathList = new List<Tuple<int, int, string>>();

                    for (int i = 0; i < clipImageTuple.Count; i++)
                    {
                        string path = Path.Combine(savePath3, $"{i}.png");
                        ImageSource imageSource = clipImageTuple[i].Item3;
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            try
                            {
                                BitmapEncoder encoder = new PngBitmapEncoder();

                                encoder.Frames.Add(BitmapFrame.Create(imageSource as BitmapSource));
                                encoder.Save(fileStream);
                            }
                            catch { }
                        }

                        imagePathList.Add(new Tuple<int, int, string>(clipImageTuple[i].Item1, clipImageTuple[i].Item2, path));
                    }

                    CreateMicroscopeLogSheet(logSheet, cosmoLotNoInfo, imagePathList);

                    //-------------------------------------------------------------------------------------------
                    // Save
                    workbook.SaveAs(filePath);

                    System.Diagnostics.Process.Start(savePath3);

                    return;
                }
                catch (Exception e)
                {

                }
                finally
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(logSheet);

                    workbook.Close(false);
                    excelApp.Quit();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }
            });
        }
    }
}
