using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Controls.View;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;
using SeriesCollection = LiveCharts.SeriesCollection;

namespace WPF.COSMO.Offline.Controls.ViewModel
{
    public class LotNoChartViewModel : Observable
    {
        #region

        public CosmoLotNoType CosmoLotNoType = CosmoLotNoType.Unknown;

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

        private string chartTitle;
        public string ChartTitle
        {
            get => chartTitle;
            set => Set(ref chartTitle, value);
        }
        
        private DefectViewMode defectViewMode;
        public DefectViewMode DefectViewMode
        {
            get => defectViewMode;
            set
            {
                Set(ref defectViewMode, value);
                UpdateResult();
            }
        }

        #endregion

        #region Command

        private ICommand dataClickCommand;
        public ICommand DataClickCommand { get => (dataClickCommand ?? (dataClickCommand = new RelayCommand<ChartPoint>(DataClick))); }

        private async void DataClick(ChartPoint chartPoint)
        {
            SelectedReportDataChartControl selectedChart = new SelectedReportDataChartControl();

            selectedChart.ChartTitle = labels[chartPoint.Key];
            selectedChart.ItemsSource = sortDefectList[labels[chartPoint.Key]].ToList();
            selectedChart.DefectViewMode = DefectViewMode;

            await Application.Current.MainWindow.ShowChildWindowAsync(selectedChart);

            //Console.WriteLine("[{0}] : {1}", labels[chartPoint.Key], chartPoint.Instance);
        }

        #endregion

        public void Initialize(CosmoLotNoType lotNoType)
        {
            CosmoLotNoType = lotNoType;
            CreateChart();
        }

        public void CreateChart()
        {
            switch (CosmoLotNoType)
            {
                case CosmoLotNoType.LotNo:
                    ChartTitle = TranslationHelper.Instance.Translate("Lot_No.");
                    break;
                case CosmoLotNoType.Date:
                    ChartTitle = TranslationHelper.Instance.Translate("Date");
                    break;
                case CosmoLotNoType.CoatingDevice:
                    ChartTitle = TranslationHelper.Instance.Translate("Coating_Device");
                    break;
                case CosmoLotNoType.CoatingNo:
                    ChartTitle = TranslationHelper.Instance.Translate("Coating_No");
                    break;
                case CosmoLotNoType.SlitterDevice:
                    ChartTitle = TranslationHelper.Instance.Translate("Slitter_Device");
                    break;
                case CosmoLotNoType.SlitterCut:
                    ChartTitle = TranslationHelper.Instance.Translate("Slitter_Cut");
                    break;
                case CosmoLotNoType.SlitterLane:
                    ChartTitle = TranslationHelper.Instance.Translate("Slitter_Lane");
                    break;
                default:
                    break;
            }
        }

        List<ReportInfo> resultInfos;
        ILookup<string, ReportInfo> sortDefectList;

        public void SetResultList(object itemsSource)
        {
            resultInfos = itemsSource as List<ReportInfo>;
            UpdateResult();
        }

        void UpdateResult()
        {
            if (resultInfos == null)
                return;

            var seriesCollection = new SeriesCollection();
            ColumnSeries columnSeries = new ColumnSeries();
            columnSeries.Title = ChartTitle;

            seriesCollection.Add(columnSeries);

            Dictionary<string, double> dictionary = new Dictionary<string, double>();

            switch (CosmoLotNoType)
            {
                case CosmoLotNoType.LotNo:
                    sortDefectList = resultInfos.ToLookup(x => x.LotNoInfo.LotNo);
                    break;
                case CosmoLotNoType.Date:
                    sortDefectList = resultInfos.ToLookup(x => x.LotNoInfo.ProductDate.Value.ToString("yyyyMMdd"));
                    break;
                case CosmoLotNoType.CoatingDevice:
                    sortDefectList = resultInfos.ToLookup(x => x.LotNoInfo.CoatingDevice.Value.Key);
                    break;
                case CosmoLotNoType.CoatingNo:
                    sortDefectList = resultInfos.ToLookup(x => x.LotNoInfo.CoatingNo.Value.ToString());
                    break;
                case CosmoLotNoType.SlitterDevice:
                    sortDefectList = resultInfos.ToLookup(x => x.LotNoInfo.SlitterDevice.Value.Key);
                    break;
                case CosmoLotNoType.SlitterCut:
                    sortDefectList = resultInfos.ToLookup(x => x.LotNoInfo.SlitterNo.Value.ToString());
                    break;
                case CosmoLotNoType.SlitterLane:
                    sortDefectList = resultInfos.ToLookup(x => x.LotNoInfo.SlitterLane.Value.ToString());
                    break;
                default:
                    sortDefectList = null;
                    break;
            }

            if (sortDefectList != null)
            {
                foreach (var pair in sortDefectList)
                {
                    string key = pair.Key;

                    // 정확한 불량 개수를 가져옴
                    double count = 0;
                    switch (DefectViewMode)
                    {
                        case DefectViewMode.Total:
                            count = pair.Average(x => x.TotalDefectNum);
                            break;
                        case DefectViewMode.Left:
                            count = pair.Average(x => x.LeftDefectNum);
                            break;
                        case DefectViewMode.Right:
                            count = pair.Average(x => x.RightDefectNum);
                            break;
                        case DefectViewMode.Edge:
                            count = pair.Average(x => x.EdgeDefectNum);
                            break;
                        case DefectViewMode.Inner:
                            count = pair.Average(x => x.InnerDefectNum);
                            break;
                    }

                    if (!dictionary.ContainsKey(key))
                        dictionary.Add(key, count);
                    else
                        dictionary[key] += count;
                }
            }

            var chartValues = new ChartValues<double>();
            string[] labels = new string[dictionary.Count];

            int index = 0;
            foreach (var pair in dictionary)
            {
                labels[index] = pair.Key;
                chartValues.Add(pair.Value);

                index++;
            }

            columnSeries.Values = chartValues;

            Labels = labels;
            SeriesCollection = seriesCollection;
        }
    }
}
