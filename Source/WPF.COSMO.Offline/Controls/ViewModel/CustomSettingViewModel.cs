using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Controls.ViewModel
{
    public class CustomSettingViewModel : Observable
    {
        SectionServiceSetting _sectionSettings = SectionService.Settings;
        public SectionServiceSetting SectionSettings
        {
            get => _sectionSettings;
            set
            {
                Set(ref _sectionSettings, null);
                Set(ref _sectionSettings, value);
            }
        }


        public LotNoCollections Collections => LotNoService.Collections;
        
        public Param_COSMO Param => Model_COSMO.Param;

        public double NewSection { get; set; }
        public double NewSize { get; set; }
        public double NewPosition { get; set; }

        public string NewCoatingDeviceKey { get; set; }
        public string NewCoatingDeviceValue { get; set; }

        public string NewSlitterDeviceKey { get; set; }
        public string NewSlitterDeviceValue { get; set; }

        ICommand _sectionAddCommand;
        public ICommand SectionAddCommand => _sectionAddCommand ?? (_sectionAddCommand = new RelayCommand(AddSection));

        ICommand _sizeAddCommand;
        public ICommand SizeAddCommand => _sizeAddCommand ?? (_sizeAddCommand = new RelayCommand(AddSize));

        ICommand _sectionRemoveCommand;
        public ICommand SectionRemoveCommand => _sectionRemoveCommand ?? (_sectionRemoveCommand = new RelayCommand<double>(RemoveSection));

        ICommand _sizeRemoveCommand;
        public ICommand SizeRemoveCommand => _sizeRemoveCommand ?? (_sizeRemoveCommand = new RelayCommand<double>(RemoveSize));

        ICommand _positionAddCommand;
        public ICommand PositionAddCommand => _positionAddCommand ?? (_positionAddCommand = new RelayCommand(AddPosition));

        ICommand _positionRemoveCommand;
        public ICommand PositionRemoveCommand => _positionRemoveCommand ?? (_positionRemoveCommand = new RelayCommand<double>(RemovePosition));
        
        ICommand _coatingDeviceAddCommand;
        public ICommand CoatingDeviceAddCommand => _coatingDeviceAddCommand ?? (_coatingDeviceAddCommand = new RelayCommand(AddCoatngDevice));

        ICommand _coatingDeviceRemoveCommand;
        public ICommand CoatingDeviceRemoveCommand => _coatingDeviceRemoveCommand ?? (_coatingDeviceRemoveCommand = new RelayCommand<KeyValuePair<string, string>>(RemoveCoatingDevice));

        ICommand _slitterDeviceAddCommand;
        public ICommand SlitterDeviceAddCommand => _slitterDeviceAddCommand ?? (_slitterDeviceAddCommand = new RelayCommand(AddSlitterDevice));

        ICommand _slitterDeviceRemoveCommand;
        public ICommand SlitterDeviceRemoveCommand => _slitterDeviceRemoveCommand ?? (_slitterDeviceRemoveCommand = new RelayCommand<KeyValuePair<string, string>>(RemoveSlitterDevice));

        ICommand _presetAddCommand;
        public ICommand PresetAddCommand => _presetAddCommand ?? (_presetAddCommand = new RelayCommand<string>(AddPreset));

        ICommand _presetSelectCommand;
        public ICommand PresetSelectCommand => _presetSelectCommand ?? (_presetSelectCommand = new RelayCommand<string>(SelectPreset));

        ICommand _presetRemoveCommand;
        public ICommand PresetRemoveCommand => _presetRemoveCommand ?? (_presetRemoveCommand = new RelayCommand<string>(RemovePreset));

        public async void Save()
        {
            await SectionService.SaveSectionSettings();
            await LotNoService.SaveAxisGrabInfos();
            await Model_COSMO.SaveParam();
        }

        private void AddCoatngDevice()
        {
            if (string.IsNullOrEmpty(NewCoatingDeviceKey) || string.IsNullOrEmpty(NewCoatingDeviceValue))
                return;

            if (Collections.CoatingDeviceList.Any(tuple => tuple.Key == NewCoatingDeviceKey))
                return;

            Collections.CoatingDeviceList.Add(new KeyValuePair<string, string>(NewCoatingDeviceKey, NewCoatingDeviceValue));
        }

        private void RemoveCoatingDevice(KeyValuePair<string, string> pair)
        {
            Collections.CoatingDeviceList.Remove(pair);
        }

        private void AddSlitterDevice()
        {
            if (string.IsNullOrEmpty(NewSlitterDeviceKey) || string.IsNullOrEmpty(NewSlitterDeviceValue))
                return;

            if (Collections.SlitterDeviceList.Any(tuple => tuple.Key == NewSlitterDeviceKey))
                return;

            Collections.SlitterDeviceList.Add(new KeyValuePair<string, string>(NewSlitterDeviceKey, NewSlitterDeviceValue));
        }

        private void RemoveSlitterDevice(KeyValuePair<string, string> pair)
        {
            Collections.SlitterDeviceList.Remove(pair);
        }

        private void AddSection()
        {
            if (SectionSettings.Selected.DefectDistanceList.Contains(NewSection))
                return;

            if (SectionSettings.Selected.DefectDistanceList.Count == 0)
            {
                SectionSettings.Selected.DefectDistanceList.Add(NewSection);
                return;
            }

            if (SectionSettings.Selected.DefectDistanceList.Count == 0)
                SectionSettings.Selected.DefectDistanceList.Add(NewSection);
            else if (SectionSettings.Selected.DefectDistanceList.Max() < NewSection)
                SectionSettings.Selected.DefectDistanceList.Add(NewSection);
            else if (SectionSettings.Selected.DefectDistanceList.Min() > NewSection)
                SectionSettings.Selected.DefectDistanceList.Insert(0, NewSection);
            else
            {
                double first = SectionSettings.Selected.DefectDistanceList.First(dist => dist > NewSection);
                int firstIndex = SectionSettings.Selected.DefectDistanceList.IndexOf(first);
                SectionSettings.Selected.DefectDistanceList.Insert(firstIndex, NewSection);
            }
        }

        private void RemoveSection(double section)
        {
            SectionSettings.Selected.DefectDistanceList.Remove(section);
        }

        private void AddSize()
        {
            if (SectionSettings.Selected.DefectSizeList.Contains(NewSize))
                return;

            if (SectionSettings.Selected.DefectSizeList.Count == 0)
            {
                SectionSettings.Selected.DefectSizeList.Add(NewSize);
                return;
            }

            if (SectionSettings.Selected.DefectSizeList.Max() < NewSize)
                SectionSettings.Selected.DefectSizeList.Add(NewSize);
            else if (SectionSettings.Selected.DefectSizeList.Min() > NewSize)
                SectionSettings.Selected.DefectSizeList.Insert(0, NewSize);
            else
            {
                double first = SectionSettings.Selected.DefectSizeList.First(s => s > NewSize);
                int firstIndex = SectionSettings.Selected.DefectSizeList.IndexOf(first);
                SectionSettings.Selected.DefectSizeList.Insert(firstIndex, NewSize);
            }
        }

        private void RemoveSize(double size)
        {
            SectionSettings.Selected.DefectSizeList.Remove(size);
        }

        private void AddPosition()
        {
            if (SectionSettings.Selected.InspectPositionList.Contains(NewPosition))
                return;

            if (SectionSettings.Selected.InspectPositionList.Count == 0)
            {
                SectionSettings.Selected.InspectPositionList.Add(NewPosition);
                return;
            }

            if (SectionSettings.Selected.InspectPositionList.Max() < NewPosition)
                SectionSettings.Selected.InspectPositionList.Add(NewPosition);
            else if (SectionSettings.Selected.InspectPositionList.Min() > NewPosition)
                SectionSettings.Selected.InspectPositionList.Insert(0, NewPosition);
            else
            {
                double first = SectionSettings.Selected.InspectPositionList.First(s => s > NewPosition);
                int firstIndex = SectionSettings.Selected.InspectPositionList.IndexOf(first);
                SectionSettings.Selected.InspectPositionList.Insert(firstIndex, NewPosition);
            }
        }

        private void RemovePosition(double position)
        {
            SectionSettings.Selected.InspectPositionList.Remove(position);
        }

        private void AddPreset(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (SectionSettings.SectionSettings.ContainsKey(key))
            {
                SectionSettings = SectionService.Settings;
                return;
            }

            Section selected = SectionSettings.Selected;

            Section newSection = new Section();
            
            newSection.InspectScanLength = selected.InspectScanLength;

            foreach (var distance in selected.DefectDistanceList)
                newSection.DefectDistanceList.Add(distance);

            foreach (var size in selected.DefectSizeList)
                newSection.DefectSizeList.Add(size);

            foreach (var position in selected.InspectPositionList)
                newSection.InspectPositionList.Add(position);

            SectionSettings.SectionSettings.Add(key, newSection);

            SectionSettings = SectionService.Settings;
        }

        private void SelectPreset(string key)
        {
            if (SectionSettings.SectionSettings.ContainsKey(key))
            {
                var section = SectionSettings.SectionSettings[key];

                Section newSection = new Section();

                newSection.InspectScanLength = section.InspectScanLength;

                foreach (var distance in section.DefectDistanceList)
                    newSection.DefectDistanceList.Add(distance);

                foreach (var size in section.DefectSizeList)
                    newSection.DefectSizeList.Add(size);

                foreach (var position in section.InspectPositionList)
                    newSection.InspectPositionList.Add(position);

                SectionSettings.Selected = newSection;
            }
        }

        private void RemovePreset(string key)
        {
            SectionSettings.SectionSettings.Remove(key);

            SectionSettings = SectionService.Settings;
        }
    }
}
