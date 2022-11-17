using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Controls;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Controls.ViewModel
{
    class SizeChartViewModel : Observable
    {
        SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get
            {
                return _seriesCollection;
            }
            set
            {
                Set(ref _seriesCollection, value);
            }
        }

        string[] _labels;
        public string[] Labels
        {
            get
            {
                return _labels;
            }
            set
            {
                Set(ref _labels, value);
            }
        }

        Dictionary<double, List<ObservableValue>> dictionary = new Dictionary<double, List<ObservableValue>>();

        public Func<double, string> Formatter { get; set; }

        DefectStorage _defectStorage;

        #region Method

        const int edgeSection = -1;

        public void Initialize(DefectStorage defectStorage)
        {
            _defectStorage = defectStorage;
            _defectStorage.DefectViewModeChanged += Changed;

            AxisGrabService.Initialized += Initialized;
            InspectService.InspectDone += UpdateData;

            ResultService.Initialized += Initialized;
            ResultService.LoadDone += UpdateData;

            SectionService.SectionChanged += Changed;

            ChartIntialize();
        }

        void ChartIntialize()
        {
            SeriesCollection seriesCollection = new SeriesCollection();

            Section section = _defectStorage.Section;

            var defectDistanceList = section.DefectDistanceList;
            var defectSizeList = section.DefectSizeList;
            var inspectPositionList = section.InspectPositionList;

            string[] labels = new string[defectSizeList.Count + 1];

            int labelIndex = 0;
            foreach (var size in defectSizeList)
                labels[labelIndex++] = size.ToString();

            labels[labelIndex++] = "Over";

            switch (_defectStorage.DefectViewMode)
            {
                case DefectViewMode.Total:
                    CreatSeries(edgeSection, seriesCollection);
                    foreach (var dist in defectDistanceList)
                        CreatSeries(dist, seriesCollection);
                    foreach (var position in inspectPositionList)
                        CreatSeries(position, seriesCollection);
                    break;
                case DefectViewMode.Left:
                case DefectViewMode.Right:
                case DefectViewMode.Edge:
                    CreatSeries(edgeSection, seriesCollection);
                    foreach (var dist in defectDistanceList)
                        CreatSeries(dist, seriesCollection);
                    break;
                case DefectViewMode.Inner:
                    foreach (var position in inspectPositionList)
                        CreatSeries(position, seriesCollection);
                    break;
            }

            Labels = labels;
            SeriesCollection = seriesCollection;
        }

        protected void Initialized()
        {
            dictionary.Clear();
            ChartIntialize();
        }

        void Changed()
        {
            dictionary.Clear();
            ChartIntialize();
            UpdateData();
        }

        void CreatSeries(double curDist, SeriesCollection seriesCollection)
        {
            if (dictionary.ContainsKey(curDist))
                return;

            var defectSizeList = _defectStorage.Section.DefectSizeList;

            dictionary.Add(curDist, new List<ObservableValue>());

            var stackedColumnSeries = new StackedColumnSeries();
            var chartValues = new ChartValues<ObservableValue>();

            foreach (var size in defectSizeList)
            {
                var observableValue = new ObservableValue();
                chartValues.Add(observableValue);

                dictionary[curDist].Add(observableValue);
            }

            var overValue = new ObservableValue();
            chartValues.Add(overValue);
            dictionary[curDist].Add(overValue);

            string distString = string.Empty;

            if (curDist == edgeSection)
                distString = "Edge";
            else
                distString = curDist.ToString();

            stackedColumnSeries.Title = distString;
            stackedColumnSeries.Values = chartValues;

            seriesCollection.Add(stackedColumnSeries);
        }
        
        protected void UpdateData()
        {
            IEnumerable<Defect> filteredDefects = _defectStorage.GetFilteredDefect();

            Section section = _defectStorage.Section;

            var defectDistanceList = section.DefectDistanceList;
            var defectSizeList = section.DefectSizeList;
            var inspectPositionList = section.InspectPositionList;

            if (defectSizeList.Count == 0 || defectDistanceList.Count == 0)
                return;

            var maxSize = defectSizeList.Max();

            foreach (var defect in filteredDefects)
            {
                if (defect is EdgeDefect)
                {
                    if (maxSize <= defect.Major)
                    {
                        lock (dictionary[edgeSection].Last())
                            dictionary[edgeSection].Last().Value++;
                    }
                    else
                    {
                        for (int i = 0; i < defectSizeList.Count; i++)
                        {
                            if (defect.Major <= defectSizeList[i])
                            {
                                dictionary[edgeSection][i].Value++;
                                break;
                            }
                        }
                    }
                }
                else if (defect is InnerDefect)
                {
                    IDistanceDefect distDefect = defect as IDistanceDefect;
                    double minDist = double.MaxValue;
                    int minPosition = 0;

                    foreach (var position in inspectPositionList)
                    {
                        double dist = Math.Abs(position - distDefect.Distance);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            minPosition = (int)position;
                        }
                    }

                    if (defect.Major >= maxSize)
                    {
                        lock (dictionary[minPosition].Last())
                            dictionary[minPosition].Last().Value++;
                    }
                    else
                    {
                        for (int i = 0; i < defectSizeList.Count(); i++)
                        {
                            if (defect.Major <= defectSizeList[i])
                            {
                                lock (dictionary[minPosition][i])
                                    dictionary[minPosition][i].Value++;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    IDistanceDefect distDefect = defect as IDistanceDefect;

                    foreach (var pair in dictionary)
                    {
                        if (distDefect.Distance <= pair.Key)
                        {
                            if (defect.Major >= maxSize)
                            {
                                lock (pair.Value.Last())
                                    pair.Value.Last().Value++;
                            }
                            else
                            {
                                for (int i = 0; i < defectSizeList.Count(); i++)
                                {
                                    if (defect.Major <= defectSizeList[i])
                                    {
                                        lock (pair.Value[i])
                                            pair.Value[i].Value++;
                                        break;
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }

        #endregion
    }
}
