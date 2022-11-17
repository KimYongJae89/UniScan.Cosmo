using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using LiveCharts;
using LiveCharts.Wpf;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using UniEye.Base;
using WPF.Base.Helpers;
using WPF.Base.Services;
using WPF.COSMO.Offline.Controls.View;
using WPF.COSMO.Offline.Controls.ViewModel;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.ViewModels
{
    public class ReportViewModel : Observable
    {
        private IEnumerable<ReportInfo> loadResults;
        public IEnumerable<ReportInfo> LoadResults
        {
            get => loadResults;
            set => Set(ref loadResults, value);
        }

        private Dictionary<CosmoLotNoInfo, InspectResult> _resultDictionary;

        private List<InspectResult> loadInspectResults;
        public List<InspectResult> LoadInspectResults
        {
            get => loadInspectResults;
            set => Set(ref loadInspectResults, value);
        }
        
        private IDialogCoordinator _dialogCoordinator;
        
        private DefectViewMode defectViewMode;
        public DefectViewMode DefectViewMode
        {
            get => defectViewMode;
            set => Set(ref defectViewMode, value);
        }

        ICommand searchCommand;
        public ICommand SearchCommand => searchCommand ?? (searchCommand = new RelayCommand(Serach));

        ICommand exportCommand;
        public ICommand ExportCommand => exportCommand ?? (exportCommand = new RelayCommand(Export));

        ICommand searchSizeCommnad;
        public ICommand SearchSizeCommnad => searchSizeCommnad ?? (searchSizeCommnad = new RelayCommand(Serach));
        
        private SeriesCollection seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => seriesCollection;
            set => Set(ref seriesCollection, value);
        }

        private string[] labels;
        public string[] Labels
        {
            get => labels;
            set => Set(ref labels, value);
        }

        private string _cahrtTitle;
        public string ChartTitle
        {
            get => _cahrtTitle;
            set => Set(ref _cahrtTitle, value);
        }

        FilterViewModel _filterViewModel;

        private Dictionary<DateTime, List<InspectResult>> _results = new Dictionary<DateTime, List<InspectResult>>();

        public SectionServiceSetting SectionServiceSetting => SectionService.Settings;

        ICommand saveCommnad;
        public ICommand SaveCommnad => saveCommnad ?? (saveCommnad = new RelayCommand(Save));

        public void Initialize(IDialogCoordinator dialogCoordinator, FilterViewModel filterViewModel)
        {
            _dialogCoordinator = dialogCoordinator;
            _filterViewModel = filterViewModel;

            ChartTitle = TranslationHelper.Instance.Translate("Size");
        }

        async void Export()
        {
            if (_resultDictionary == null)
                return;

            Action action = new Action(() =>
            {
                ExcelExportService.InspectResultExport(_resultDictionary, _results, SectionService.Settings.Selected);
            });

            await MessageWindowHelper.ShowProgress("Result", "Export result files...", action, new CancellationTokenSource());
        }

        async void Save()
        {
            Action action = new Action(async () =>
            {
                await SectionService.SaveSectionSettings();
            });

            await MessageWindowHelper.ShowProgress("Save", "Save settings...", action, new CancellationTokenSource());
        }

        async void Serach()
        {
            _results.Clear();

            var seriesCollection = new SeriesCollection();
            ColumnSeries columnSeries = new ColumnSeries();
            columnSeries.Title = TranslationHelper.Instance.Translate("Size");
            seriesCollection.Add(columnSeries);
            var chartValues = new ChartValues<double>();

            Action action = new Action(() =>
            {
                var searchResult = _filterViewModel.Search();

                //if (ModelService_COSMO.Instance.Current != null)
                //    searchResult = searchResult.Where(s => s.Key.ModelName == null || s.Key.ModelName == ModelService_COSMO.Instance.Current.Name).ToDictionary(s => s.Key, s => s.Value);// (s => s.Key.ModelName ==).ToDictionary(k => k.Key);

                _resultDictionary = ResultService.LoadResultAsync(searchResult).Where(l => l.Value.ScanResultList.Sum(s => s.Defects.Count) <= SectionServiceSetting.FilterCount).ToDictionary(r => r.Key, r => r.Value);

                LoadInspectResults = _resultDictionary.Values.ToList();
                LoadResults = ResultService.LoadReportAsync(searchResult).Where(l => l.TotalDefectNum <= SectionServiceSetting.FilterCount);
                
                if (SectionService.Settings.Selected != null)
                {
                    Labels = SectionService.Settings.Selected.DefectSizeList.Select(s => s.ToString()).ToArray();
                }
    
                
                List<string> labels = new List<string>();

                var dateTimes = new List<DateTime>();
                var values = new List<int>();

                foreach (var size in SectionService.Settings.Selected.DefectSizeList)
                    values.Add(0);


                int index = 0;
                List<Tuple<CosmoLotNoInfo, InspectResult>> tuples = new List<Tuple<CosmoLotNoInfo, InspectResult>>();
                foreach (var searched in LoadResults)
                {
                    if (dateTimes.Any(d => d == searched.LotNoInfo.ProductDate) == false)
                        dateTimes.Add(searched.LotNoInfo.ProductDate.Value);

                    tuples.Add(new Tuple<CosmoLotNoInfo, InspectResult>(searched.LotNoInfo, LoadInspectResults[index++]));
                }

                values.Add(0);

                foreach (var time in dateTimes)
                    _results[time] = new List<InspectResult>();

                foreach (var tuple in tuples)
                {
                    var result = tuple.Item2;
                    _results[tuple.Item1.ProductDate.Value].Add(result);
                    foreach (var scanResult in result.ScanResultList)
                    {
                        foreach (var defect in scanResult.Defects)
                        {
                            bool founded = false;

                            for (int i = 0; i < SectionService.Settings.Selected.DefectSizeList.Count; i++)
                            {
                                if (defect.Major < SectionService.Settings.Selected.DefectSizeList[i])
                                {
                                    values[i]++;
                                    founded = true;
                                    break;
                                }
                            }

                            if (founded == false)
                            {
                                values[values.Count - 1]++;
                            }
                        }
                    }
                }

                foreach (var val in values)
                    chartValues.Add(val);
            });

            columnSeries.Values = chartValues;
            SeriesCollection = seriesCollection;

            await MessageWindowHelper.ShowProgress("Result", "Loading result files...", action, new CancellationTokenSource());
        }
    }
}
