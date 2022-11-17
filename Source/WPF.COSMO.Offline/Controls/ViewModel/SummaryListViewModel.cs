using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Controls;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Controls.ViewModel
{
    public class SummaryListViewModel : Observable
    {
        int _count;
        public int Count
        {
            get => _count;
            set => Set(ref _count, value);
        }

        Dictionary<double, int[]> summaryData = new Dictionary<double, int[]>();

        DataTable _summaryDataTable = new DataTable();
        public DataTable SummaryDataTable
        {
            get => _summaryDataTable;
            set
            {
                Set(ref _summaryDataTable, value);
            }
        }

        public int SelectedIndex { get; set; }
        
        const int overSize = -1;

        DefectStorage _defectStorage;

        public void Initialize(DefectStorage defectStorage)
        {
            AxisGrabService.Initialized += Initialized;
            InspectService.InspectDone += UpdateData;
            
            ResultService.Initialized += Initialized;
            ResultService.LoadDone += UpdateData;

            SectionService.SectionChanged += Changed;

            _defectStorage = defectStorage;
            _defectStorage.DefectViewModeChanged += Changed;

            DataTableInitialize();
        }
        
        public void SelectedChanged(string header, string rowHeader)
        {
            

            if (header == "Edge")
                _defectStorage.SectionFilter = -1;
            else
            {
                if (header.Contains("um"))
                    _defectStorage.SectionFilter = double.Parse(header.Replace("um", "")) / 1000.0;
                else
                    _defectStorage.SectionFilter = double.Parse(header);
            }

            if (rowHeader == "Over")
                _defectStorage.SizeFilter = -1;
            else
                _defectStorage.SizeFilter = double.Parse(rowHeader);
        }

        protected void Initialized()
        {
            DataTableInitialize();
        }

        void Changed()
        {
            DataTableInitialize();
            UpdateData();
        }

        public void DataTableInitialize()
        {
            Section section = _defectStorage.Section;

            var defectDistanceList = section.DefectDistanceList;
            var defectSizeList = section.DefectSizeList;
            var inspectPositionList = section.InspectPositionList;

            summaryData.Clear();

            var newDataTable = new DataTable();

            newDataTable.Columns.Add("Size", typeof(string));
            CreateColumn("Sum", newDataTable);

            switch (_defectStorage.DefectViewMode)
            {
                case DefectViewMode.Total:
                case DefectViewMode.Left:
                case DefectViewMode.Right:
                case DefectViewMode.Edge:
                    CreateColumn("Edge", newDataTable);
                    foreach (var dist in defectDistanceList)
                    {
                        if (Math.Round(dist) != dist)
                            CreateColumn(string.Format("{0}um", dist * 1000), newDataTable);
                        else
                            CreateColumn(dist.ToString(), newDataTable);
                    }
                        
                    break;
            }

            switch (_defectStorage.DefectViewMode)
            {
                case DefectViewMode.Total:
                case DefectViewMode.Inner:
                    foreach (var position in inspectPositionList)
                        CreateColumn(position.ToString(), newDataTable);
                    break;
            }

            foreach (var size in defectSizeList)
            {
                var newRow = newDataTable.NewRow();
                newRow[0] = size.ToString();
                newDataTable.Rows.Add(newRow);

                summaryData.Add(size, CreateArray());
            }

            var overRow = newDataTable.NewRow();
            overRow[0] = "Over";
            newDataTable.Rows.Add(overRow);

            SummaryDataTable = newDataTable;

            summaryData.Add(overSize, CreateArray());
        }

        private int[] CreateArray()
        {
            Section section = _defectStorage.Section;

            var defectDistanceList = section.DefectDistanceList;
            var defectSizeList = section.DefectSizeList;
            var inspectPositionList = section.InspectPositionList;

            switch (_defectStorage.DefectViewMode)
            {
                case DefectViewMode.Left:
                case DefectViewMode.Right:
                case DefectViewMode.Edge:
                    return new int[defectDistanceList.Count + 1];
                case DefectViewMode.Inner:
                    return new int[inspectPositionList.Count];
            }

            return new int[defectDistanceList.Count + inspectPositionList.Count + 1];
        }

        private void CreateColumn(string name, DataTable dataTable)
        {
            if (dataTable.Columns.Contains(name))
                return;

            var column = dataTable.Columns.Add(name, typeof(int));
            column.AllowDBNull = false;
            column.DefaultValue = 0;
        }
        
        protected void UpdateData()
        {
            IEnumerable<Defect> filteredDefects = _defectStorage.GetFilteredDefect();

            Count = filteredDefects.Count();

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
                        summaryData[overSize][0]++;
                    }
                    else
                    {
                        foreach (var pair in summaryData)
                        {
                            if (defect.Major <= pair.Key)
                            {
                                pair.Value[0]++;
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
                        summaryData[overSize][summaryData[overSize].Count() - positionIndex - 1]++;
                    }
                    else
                    {
                        foreach (var pair in summaryData)
                        {
                            if (defect.Major <= pair.Key)
                            {
                                pair.Value[summaryData[overSize].Count() - positionIndex - 1]++;
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
                                lock (summaryData[overSize])
                                    summaryData[overSize][i + 1]++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (var pair in summaryData)
                        {
                            if (defect.Major <= pair.Key)
                            {
                                for (int i = 0; i < defectDistanceList.Count(); i++)
                                {
                                    if (distDefect.Distance <= defectDistanceList[i])
                                    {
                                        lock (pair.Value)
                                            pair.Value[i + 1]++;
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }

            InspectDone();
        }

        protected void InspectDone()
        {
            //Total
            int index = 0;
            foreach (var array in summaryData.Values)
                SummaryDataTable.Rows[index++][1] = array.Sum();

            index = 0;
            foreach (var array in summaryData.Values)
            {
                for (int i = 0; i < array.Count(); i++)
                    SummaryDataTable.Rows[index][i + 2] = array[i];

                index++;
            }
        }
    }
}
