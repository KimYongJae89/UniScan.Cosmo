using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class SectionChartViewModel : Observable
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

        const int overSize = -1;
        DefectStorage _defectStorage;

        #region Method
        
        public void Initialize(DefectStorage defectStorage)
        {
            _defectStorage = defectStorage;

            AxisGrabService.Initialized += Initialized;
            InspectService.InspectDone += UpdateData;

            ResultService.Initialized += Initialized;
            ResultService.LoadDone += UpdateData;
            
            SectionService.SectionChanged += Changed;
            _defectStorage.DefectViewModeChanged += Changed;

            ChartIntialize();
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

        void ChartIntialize()
        {
            SeriesCollection seriesCollection = new SeriesCollection();

            Section section = _defectStorage.Section;

            var defectDistanceList = section.DefectDistanceList;
            var defectSizeList = section.DefectSizeList;
            var inspectPositionList = section.InspectPositionList;

            List<string> labels = new List<string>();

            switch (_defectStorage.DefectViewMode)
            {
                case DefectViewMode.Total:
                case DefectViewMode.Left:
                case DefectViewMode.Right:
                case DefectViewMode.Edge:
                    labels.Add("Edge");
                    foreach (var dist in defectDistanceList)
                        labels.Add(dist.ToString());
                    break;
            }

            switch (_defectStorage.DefectViewMode)
            {
                case DefectViewMode.Total:
                case DefectViewMode.Inner:
                    foreach (var position in inspectPositionList)
                        labels.Add(position.ToString());
                    break;
            }

            foreach (var size in defectSizeList)
                seriesCollection.Add(CreatSeries(size));

            seriesCollection.Add(CreatSeries(overSize));

            Labels = labels.ToArray();
            SeriesCollection = seriesCollection;
        }

        StackedColumnSeries CreatSeries(double curSize)
        {
            Section section = _defectStorage.Section;

            var defectDistanceList = section.DefectDistanceList;
            var defectSizeList = section.DefectSizeList;
            var inspectPositionList = section.InspectPositionList;

            dictionary.Add(curSize, new List<ObservableValue>());

            var stackedColumnSeries = new StackedColumnSeries();
            var chartValues = new ChartValues<ObservableValue>();

            switch (_defectStorage.DefectViewMode)
            {
                case DefectViewMode.Total:
                case DefectViewMode.Left:
                case DefectViewMode.Right:
                case DefectViewMode.Edge:
                    var edgeValue = new ObservableValue();
                    chartValues.Add(edgeValue);
                    dictionary[curSize].Add(edgeValue);
                    foreach (var dist in defectDistanceList)
                    {
                        var observableValue = new ObservableValue();
                        chartValues.Add(observableValue);
                        dictionary[curSize].Add(observableValue);
                    }
                    break;
            }

            switch (_defectStorage.DefectViewMode)
            {
                case DefectViewMode.Total:
                case DefectViewMode.Inner:
                    foreach (var position in inspectPositionList)
                    {
                        var observableValue = new ObservableValue();
                        chartValues.Add(observableValue);
                        dictionary[curSize].Add(observableValue);
                    }
                    break;
            }

            string sizeString = string.Empty;

            if (curSize == overSize)
                sizeString = "Over";
            else
                sizeString = curSize.ToString();

            stackedColumnSeries.Title = sizeString;
            stackedColumnSeries.Values = chartValues;

            return stackedColumnSeries;
        }
        
        protected void UpdateData()
        {
            IEnumerable<Defect> filteredDefects = _defectStorage.GetFilteredDefect();
            
            if (filteredDefects == null || filteredDefects.Count() == 0)
                return;

            Section section = _defectStorage.Section;

            var defectDistanceList = section.DefectDistanceList;
            var defectSizeList = section.DefectSizeList;
            var inspectPositionList = section.InspectPositionList;

            var maxSize = defectSizeList.Count > 0 ? defectSizeList.Max() : 0; ;

            foreach (var defect in filteredDefects)
            {
                if (defect is EdgeDefect)
                {
                    if (maxSize <= defect.Major)
                    {
                        dictionary[overSize].First().Value++;
                    }
                    else
                    {
                        foreach (var pair in dictionary)
                        {
                            if (defect.Major <= pair.Key)
                            {
                                pair.Value.First().Value++;
                                break;
                            }
                        }
                    }
                }
                else if (defect is InnerDefect)
                {
                    IDistanceDefect distDefect = defect as IDistanceDefect;
                    int positionIndex = 0;
                    double minDist = double.MaxValue;

                    for (int i = 0; i < inspectPositionList.Count(); i++)
                    {
                        double dist = Math.Abs(inspectPositionList[i] - distDefect.Distance);
                        if (dist < minDist)
                        {
                            positionIndex = inspectPositionList.Count() - i - 1;
                            minDist = dist;
                        }
                    }

                    if (maxSize <= defect.Major)
                    {
                        dictionary[overSize][dictionary[overSize].Count - 1].Value++;
                    }
                    else
                    {
                        foreach (var pair in dictionary)
                        {
                            if (defect.Major <= pair.Key)
                            {
                                pair.Value[pair.Value.Count - 1].Value++;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    IDistanceDefect distDefect = defect as IDistanceDefect;
                    if (maxSize <= defect.Major)
                    {
                        for (int i = 0; i < defectDistanceList.Count(); i++)
                        {
                            if (distDefect.Distance <= defectDistanceList[i])
                            {
                                dictionary[overSize][i + 1].Value++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var pair in dictionary)
                        {
                            if (defect.Major <= pair.Key)
                            {
                                for (int i = 0; i < defectDistanceList.Count(); i++)
                                {
                                    if (distDefect.Distance <= defectDistanceList[i])
                                    {
                                        pair.Value[i + 1].Value++;
                                        break;
                                    }
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
