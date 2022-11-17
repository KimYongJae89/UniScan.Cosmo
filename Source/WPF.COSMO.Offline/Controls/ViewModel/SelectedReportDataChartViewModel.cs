using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Controls.ViewModel
{
    class SelectedReportDataChartViewModel : Observable
    {
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

        private List<ReportInfo> reportInfoss;
        public void SetResultList(object itemsSource)
        {
            reportInfoss = itemsSource as List<ReportInfo>;
            UpdateResult();
        }

        public Models.DefectViewMode DefectViewMode { get; set; }

        void UpdateResult()
        {
            if (reportInfoss == null)
                return;

            var seriesCollection = new SeriesCollection();
            ColumnSeries columnSeries = new ColumnSeries();
            columnSeries.Title = TranslationHelper.Instance.Translate(ChartTitle);

            seriesCollection.Add(columnSeries);

            var chartValues = new ChartValues<double>();
            List<string> labels = new List<string>();
            
            foreach (var info in reportInfoss)
            {
                labels.Add(info.LotNoInfo.LotNo);

                switch (DefectViewMode)
                {
                    case Models.DefectViewMode.Total:
                        chartValues.Add(info.TotalDefectNum);
                        break;
                    case Models.DefectViewMode.Left:
                        chartValues.Add(info.LeftDefectNum);
                        break;
                    case Models.DefectViewMode.Right:
                        chartValues.Add(info.RightDefectNum);
                        break;
                    case Models.DefectViewMode.Edge:
                        chartValues.Add(info.EdgeDefectNum);
                        break;
                    case Models.DefectViewMode.Inner:
                        chartValues.Add(info.InnerDefectNum);
                        break;
                }
            }

            columnSeries.Values = chartValues;

            Labels = labels.ToArray();
            SeriesCollection = seriesCollection;
        }
    }
}
