using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UniEye.Base.Settings;
using WPF.Base.Extensions;
using WPF.Base.Helpers;

namespace WPF.COSMO.Offline.Services
{
    public delegate void SectionChangedDelegate();
    
    public class Section
    {
        public double InspectScanLength { get; set; } = 100; //mm

        public ObservableCollection<double> InspectPositionList { get; set; } = new ObservableCollection<double>();

        public ObservableCollection<double> DefectSizeList { get; set; } = new ObservableCollection<double>();
        public ObservableCollection<double> DefectDistanceList { get; set; } = new ObservableCollection<double>();
        
        public Section()
        {
            //DefectSizeList.CollectionChanged += CollectionChanged;
            //DefectDistanceList.CollectionChanged += CollectionChanged;
        }
        
        //private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (SectionService.SectionChanged != null)
        //        SectionService.SectionChanged();
        //}
    }
    public class SectionServiceSetting : Observable
    {
        private int _filterCount = 30;
        public int FilterCount
        {
            get => _filterCount;
            set => Set(ref _filterCount, value);
        }

        private int _sizeStatistics = 60;
        public int SizeStatistics
        {
            get => _sizeStatistics;
            set => Set(ref _sizeStatistics, value);
        }

        public Dictionary<string, Section> SectionSettings { get; set; } = new Dictionary<string, Section>();

        private Section _selected;
        public Section Selected
        {
            get => _selected ?? (_selected = new Section());
            set
            {
                Set(ref _selected, value);
                if (SectionService.SelectedChanged != null)
                    SectionService.SelectedChanged();
            }
        }
    }
    
    public static class SectionService
    {
        public static SectionChangedDelegate SectionChanged;

        const string key = "Section";

        public static SectionServiceSetting Settings { get; set; }

        public static EmptyDelegate SelectedChanged;

        public static async Task SaveSectionSettings()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);
            await directoryInfo.SaveAsync(key, Settings);
        }

        public static async Task LoadSectionSettings()
        {
           DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);
           Settings = await directoryInfo.ReadAsync<SectionServiceSetting>(key) ?? new SectionServiceSetting();
        }
    }
}
