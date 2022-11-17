using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using UniEye.Base;
using WPF.Base.Helpers;
using WPF.Base.Services;
using WPF.COSMO.Offline.Controls;
using WPF.COSMO.Offline.Controls.View;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;
using static WPF.COSMO.Offline.Models.PortMap;

namespace WPF.COSMO.Offline.Controls.ViewModel
{
    public class SizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if((double)value == -1)
                return "Over";
            
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "Over")
                return -1;

            return double.Parse(value.ToString());
        }
    }

    public class SectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((double)value == -1)
                return "Edge";

            if ((double)value == -2)
                return "Inner";
                    
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString() == "Edge")
                return -1;

            if (value.ToString() == "Inner")
                return -2;

            return double.Parse(value.ToString());
        }
    }

    class DefectListViewModel : Observable
    {
        #region 변수

        public ObservableCollection<Defect> DefectList { get; set; } = new ObservableCollection<Defect>();
        
        private ICommand microscopeCommand;
        public ICommand MicroscopeCommand => microscopeCommand ?? (microscopeCommand = new RelayCommand(ShowMicroscope));

        private ICommand _zoomCommand;
        public ICommand ZoomCommand => _zoomCommand ?? (_zoomCommand = new RelayCommand(Zoom));

        public DefectStorage DefectStorage { get; set; }
        #endregion

        #region Method

        string[] _sizeFilters;
        public string[] SizeFilters
        {
            get => _sizeFilters;
            set => Set(ref _sizeFilters, value);
        }

        string[] _sectionFilters;
        public string[] SectionFilters
        {
            get => _sectionFilters;
            set => Set(ref _sectionFilters, value);
        }

        ZoomService _zoomService;

        public void Initialize(DefectStorage defectStorage, ZoomService zoomService)
        {
            _zoomService = zoomService;

            BindingOperations.EnableCollectionSynchronization(DefectList, new object());

            DefectStorage = defectStorage;
            DefectStorage.DefectViewModeChanged += UpdateData;
            DefectStorage.FilterChanged += FilterChanged;

            AxisGrabService.Initialized += Initialized;
            InspectService.InspectDone += UpdateData;
            
            ResultService.Initialized += Initialized;
            ResultService.LoadDone += UpdateData;

            SectionService.SectionChanged += Changed;

            Changed();
        }
        
        void Initialized()
        {
            DefectList.Clear();
            DefectStorage.SectionFilterEnable = false;
            DefectStorage.SizeFilterEnable = false;
        }

        void FilterChanged()
        {
            UpdateData();
        }

        void Changed()
        {
            Section section = DefectStorage.Section;

            var defectDistanceList = section.DefectDistanceList;
            var defectSizeList = section.DefectSizeList;
            var inspectPositionList = section.InspectPositionList;

            string[] sectionFilters = new string[defectDistanceList.Count + inspectPositionList.Count + 1];
            sectionFilters[0] = "Edge";

            for (int i = 0; i < defectDistanceList.Count; i++)
                sectionFilters[i + 1] = defectDistanceList[i].ToString();

            for (int i = 0; i < inspectPositionList.Count; i++)
                sectionFilters[defectDistanceList.Count + i + 1] = inspectPositionList[i].ToString();
            
            string[] sizeFilters = new string[defectSizeList.Count + 1];

            for (int i = 0; i < defectSizeList.Count; i++)
                sizeFilters[i] = defectSizeList[i].ToString();

            sizeFilters[defectSizeList.Count] = "Over";

            SectionFilters = sectionFilters;
            SizeFilters = sizeFilters;
        }

        private async void ShowMicroscope()
        {
            if (AxisGrabService.CheckDoorLock() == false)
            {
                await MessageWindowHelper.ShowMessage(this, TranslationHelper.Instance.Translate("Warning"), TranslationHelper.Instance.Translate("Check_DoorLock"));
                return;
            }

            var controller = await MessageWindowHelper.ShowProgressAsync(this, TranslationHelper.Instance.Translate("Microscope"), "Move defect pos..");
            if (await MicroscopeInit() == false)
            {
                await controller.CloseAsync();

                await MessageWindowHelper.ShowMessage(this, "Microscope", "Microscope initilaize fail..");
                return;
            }

            await controller.CloseAsync();

            MicroscopeWindow microscopeWindow = new MicroscopeWindow();

            await MessageWindowHelper.ShowProgress(TranslationHelper.Instance.Translate("Microscope"), TranslationHelper.Instance.Translate("Initilaize"), new Action(() => microscopeWindow.Initialize(DefectStorage.LotNoInfo, DefectList)), null, true);
            await Application.Current.MainWindow.ShowChildWindowAsync(microscopeWindow);
        }

        private void Zoom()
        {
            if (DefectStorage.Selected == null)
                return;

            double inflate = 500;

            var defect = DefectStorage.Selected;

            double minX = defect.Points.Min(pt => pt.X);
            double maxX = defect.Points.Max(pt => pt.X);
            double minY = defect.Points.Min(pt => pt.Y);
            double maxY = defect.Points.Max(pt => pt.Y);
            
            int width = (int)(((maxX - minX) * AxisGrabService.Settings.Resolution) + (inflate * 2));
            int height = (int)(((maxY - minY) * AxisGrabService.Settings.Resolution) + (inflate * 2));

            _zoomService.Zoom(new System.Drawing.Rectangle((int)(defect.CenterPt.X - inflate), (int)(defect.CenterPt.Y - inflate), width, height));
        }

        async Task<bool> MicroscopeInit()
        {
            if (DefectStorage.Selected != null)
            {
                if (await MicroscopeGrabService.MoveDefectPos(DefectStorage.Selected) == false)
                {
                    var defect = DefectStorage.Selected;

                    string defectInfo = string.Format("Microscope move fail.. center x : {0}, center y : {1}", defect.CenterPt.X, defect.CenterPt.Y); 
                    await MessageWindowHelper.ShowMessage(this, "Microscope", defectInfo);

                    return false;
                }
            }

            if (MicroscopeGrabService.IsConnected == false || MicroscopeGrabService.IsConnected == null)
                return await MicroscopeGrabService.InitializeOptotune();

            return true;
        }

        void UpdateData()
        {
            DefectList.Clear();

            IEnumerable<Defect> filteredDefects = DefectStorage.GetFilteredDefect();
            
            foreach (var defect in Filtering(filteredDefects).OrderBy(x => x.Index))
                DefectList.Add(defect);
        }

        IEnumerable<Defect> Filtering(IEnumerable<Defect> filteredDefects)
        {
            IEnumerable<Defect> filteredDefects2 = filteredDefects;

            Section section = DefectStorage.Section;

            var defectDistanceList = section.DefectDistanceList.ToList();
            var defectSizeList = section.DefectSizeList.ToList();
            var inspectPositionList = section.InspectPositionList.ToList();

            double maxSize = defectSizeList.Count > 0 ? defectSizeList.Max() : 0;
            
            if (DefectStorage.SectionFilterEnable)
            {
                if (DefectStorage.SectionFilter == -1)
                {
                    filteredDefects2 = filteredDefects2.Where(defect => defect is EdgeDefect);
                }
                else
                {
                    var index = defectDistanceList.FindIndex(dist => dist == DefectStorage.SectionFilter);
                    if (index < 0)
                    {
                        index = inspectPositionList.FindIndex(position => position == DefectStorage.SectionFilter);
                        filteredDefects2 = filteredDefects2.Where(defect => defect is InnerDefect);
                        List<Defect> innerDefects = new List<Defect>();

                        foreach (var defect in filteredDefects2)
                        {
                            double minDist = double.MaxValue;
                            int minIndex = 0;
                            for (int i = 0; i < inspectPositionList.Count; i++)
                            {
                                double dist = Math.Abs(inspectPositionList[i] - ((InnerDefect)defect).Distance);
                                if (dist < minDist)
                                {
                                    dist = minDist;
                                    minIndex = i;
                                }
                            }
                            
                            if (index == minIndex)
                                innerDefects.Add(defect);
                        }

                        filteredDefects2 = innerDefects;
                    }
                    else
                    {
                        filteredDefects2 = filteredDefects2.Where(defect => defect is SectionDefect);
                        if (index == 0)
                            filteredDefects2 = filteredDefects2.Where(defect => (defect as IDistanceDefect).Distance < defectDistanceList[0]);
                        else
                            filteredDefects2 = filteredDefects2.Where(defect => (defect as IDistanceDefect).Distance < defectDistanceList[index] && (defect as IDistanceDefect).Distance >= defectDistanceList[index - 1]);
                    }
                }
            }

            if (DefectStorage.SizeFilterEnable)
            {
                if (DefectStorage.SizeFilter == -1)
                    filteredDefects2 = filteredDefects2.Where(defect => defect.Major > maxSize);
                else
                {
                    var sizeIndex = defectSizeList.FindIndex(size => size == DefectStorage.SizeFilter);
                    if (sizeIndex == 0)
                        filteredDefects2 = filteredDefects2.Where(defect => defect.Major < defectSizeList[0]);
                    else
                        filteredDefects2 = filteredDefects2.Where(defect => defect.Major < defectSizeList[sizeIndex] && defect.Major >= defectSizeList[sizeIndex - 1]);
                }
            }

            return filteredDefects2;
        }

        #endregion
    }
}
