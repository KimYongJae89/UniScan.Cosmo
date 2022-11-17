using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using UniEye.Base;
using UniEye.Base.Settings;
using WPF.Base.Helpers;
using WPF.Base.Services;
using WPF.COSMO.Offline.Controls;
using WPF.COSMO.Offline.Controls.View;
using WPF.COSMO.Offline.Controls.ViewModel;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;
using static WPF.COSMO.Offline.Models.PortMap;

namespace WPF.COSMO.Offline.ViewModels
{
    public struct DefectSectionSize
    {
        public int sectionIndex;
        public int sizeIndex;
        public ImageSource image;
        public double defectSize;
    }
    
    public class InspectViewModel : Observable
    {
        public IEnumerable<DefectViewMode> ResultViewModeEnum => Enum.GetValues(typeof(DefectViewMode)).Cast<DefectViewMode>();
        
        private IEnumerable<EdgeDefect> defectList;
        public IEnumerable<EdgeDefect> DefectList
        {
            get => defectList;
            set => Set(ref defectList, value);
        }

        private EdgeDefect selectedDefect;
        public EdgeDefect SelectedDefect
        {
            get => selectedDefect;
            set => Set(ref selectedDefect, value);
        }

        private bool isInnerLightOn = false;
        public bool IsInnerLightOn
        {
            get => isInnerLightOn;
            set => Set(ref isInnerLightOn, value);
        }

        private bool isDoorLock = true;
        public bool IsDoorLock
        {
            get => isDoorLock;
            set => Set(ref isDoorLock, value);
        }

        public Model_COSMO CurrentModel => ModelService_COSMO.Instance.Current;

        private IDialogCoordinator _dialogCoordinator;

        ICommand _InspectCommand;
        public ICommand InspectCommand => _InspectCommand ?? (_InspectCommand = new RelayCommand(Inspect));

        ICommand _hommingCommand;
        public ICommand HommingCommand => _hommingCommand ?? (_hommingCommand = new RelayCommand(Homming));
        
        ICommand exportResultCommand;
        public ICommand ExportResultCommand => exportResultCommand ?? (exportResultCommand = new RelayCommand(ExportResult));

        ICommand innerLightOnCommand;
        public ICommand InnerLightOnCommand => innerLightOnCommand ?? (innerLightOnCommand = new RelayCommand(InnerLightOn));

        ICommand doorLockCommand;
        public ICommand DoorLockCommand => doorLockCommand ?? (doorLockCommand = new RelayCommand(DoorLock));

        ICommand loadCommand;
        public ICommand LoadCommand => loadCommand ?? (loadCommand = new RelayCommand(Load));

        public DefectStorage DefectStorage { get; set; }

        MetroTabItem _selected;
        public MetroTabItem Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }

        MetroTabItem[] _tabControls;

        public void Initialize(IDialogCoordinator dialogCoordinator, DefectStorage defectStorage, params MetroTabItem[] tabControls)
        {
            _tabControls = tabControls;

            _dialogCoordinator = dialogCoordinator;
            DefectStorage = defectStorage;

            Selected = _tabControls.First();

            AxisGrabService.Initialized += Initialized;
            Services.ResultService.Initialized += Initialized;

            InspectService.Inspected += Inspected;
            Services.ResultService.Inspected += Inspected;

            Services.ResultService.ReportInfoLoaded += ReportInfoLoaded;

            DefectStorage.FilterChanged += FilterChanged;
        }

        void FilterChanged()
        {
            Selected = _tabControls.First();
        }

        void ReportInfoLoaded(ReportInfo reportInfo)
        {
            DefectStorage.LotNoInfo = reportInfo.LotNoInfo;
        }

        void Initialized()
        {
            DefectStorage.Selected = null;
            DefectStorage.Defects.Clear();
        }

        void Inspected(ScanResult inspectResult)
        {
            DefectStorage.Defects.AddRange(inspectResult.Defects);
        }

        async void Load()
        {
            ResultSearchWindow resultSearchWindow = new ResultSearchWindow();
            await Application.Current.MainWindow.ShowChildWindowAsync(resultSearchWindow, ChildWindowManager.OverlayFillBehavior.FullWindow);

        }

        private async void Inspect()
        {
            MetroDialogSettings settings = new MetroDialogSettings();
            settings.DialogMessageFontSize = 24;
            settings.DialogTitleFontSize = 36;

            if (CurrentModel == null)
            {
                await _dialogCoordinator.ShowMessageAsync(this, TranslationHelper.Instance.Translate("Warning"), TranslationHelper.Instance.Translate("Inspect_Model_Null"), MessageDialogStyle.Affirmative, settings);
                return;
            }

            if (await _dialogCoordinator.ShowMessageAsync(this, TranslationHelper.Instance.Translate("Warning"), string.Format(TranslationHelper.Instance.Translate("Inspect_Model_Check"), CurrentModel.Name, CurrentModel.Thickness, CurrentModel.Width), MessageDialogStyle.AffirmativeAndNegative, settings) == MessageDialogResult.Negative)
                return;

            CosmoLotNoInfo lotNoInfo = await Application.Current.MainWindow.ShowChildWindowAsync<CosmoLotNoInfo>(new LotWindow(), ChildWindowManager.OverlayFillBehavior.FullWindow);
            if (lotNoInfo == null)
            {
                ExitRun();
                return;
            }

            lotNoInfo.ModelName = CurrentModel.Name;
            
            AxisGrabService.Release();
            CalibrationService.Release();
            AlignService.Release();
            InspectService.Release();

            CancellationTokenSource cts = new CancellationTokenSource();

            if (EnterRun() == false)
            {
                await _dialogCoordinator.ShowMessageAsync(this, TranslationHelper.Instance.Translate("Warning"), TranslationHelper.Instance.Translate("Check_DoorLock"), MessageDialogStyle.Affirmative, settings);
                return;
            }

            if (await Homming(cts) == false)
            {
                ExitRun();
                return;
            }

            AxisGrabService.Initialize(CurrentModel, cts.Token);

            DefectStorage.LotNoInfo = lotNoInfo;

            CalibrationWindow calibrationWindow = new CalibrationWindow(CurrentModel, cts);
                
            if (await Application.Current.MainWindow.ShowChildWindowAsync<bool>(calibrationWindow, ChildWindowManager.OverlayFillBehavior.FullWindow) == false)
            {
                ExitRun();
                return;
            }

            await ModelService_COSMO.Instance.SaveModel(CurrentModel);

            AlignWindow alignWindow = new AlignWindow(CurrentModel, cts);
            if (await Application.Current.MainWindow.ShowChildWindowAsync<bool>(alignWindow, ChildWindowManager.OverlayFillBehavior.FullWindow) == false)
            {
                ExitRun();
                return;
            }

            InspectWindow inspectWindow = new InspectWindow(CurrentModel, cts);
            if (await Application.Current.MainWindow.ShowChildWindowAsync<bool>(inspectWindow, ChildWindowManager.OverlayFillBehavior.FullWindow) == false)
            {
                ExitRun();
                return;
            }

            lotNoInfo.InspectEndTime = DateTime.Now;
            var reportInfo =
                new ReportInfo(lotNoInfo,
                SectionService.Settings.Selected,
                DefectStorage.GetFilteredDefect(DefectViewMode.Right).Count(),
                DefectStorage.GetFilteredDefect(DefectViewMode.Left).Count(),
                DefectStorage.GetFilteredDefect(DefectViewMode.Inner).Count());
            
            ResultSaveWindow resultWindow = new ResultSaveWindow(reportInfo, InspectService.InspectResult);
            await Application.Current.MainWindow.ShowChildWindowAsync<bool>(resultWindow, ChildWindowManager.OverlayFillBehavior.FullWindow);

            ExitRun();
        }


        bool EnterRun()
        {
            if (AxisGrabService.CheckDoorLock() == false)
                return false;

            var greenPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutGreen);
            var yellowPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutYellow);
            var redPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutRed);
            var ionizerPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutIonizerPowerOn);
            var airPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutIonizerAir);

            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(greenPort, true);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(yellowPort, false);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ionizerPort, true);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(airPort, true);

            return true;
        }

        async void ExitRun()
        {
            AlignService.Release();
            CalibrationService.Release();
            InspectService.Release();
            AxisGrabService.Release();

            AxisGrabService.IsSingleMode = false;

            await Homming(new CancellationTokenSource());

            var greenPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutGreen);
            var yellowPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutYellow);
            var redPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutRed);
            var ionizerPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutIonizerPowerOn);
            var airPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutIonizerAir);

            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(greenPort, false);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(yellowPort, true);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(ionizerPort, false);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(airPort, false);
        }

        private async void Homming()
        {
            if (AxisGrabService.CheckDoorLock() == false)
            {
                MetroDialogSettings settings = new MetroDialogSettings();
                settings.DialogMessageFontSize = 24;
                settings.DialogTitleFontSize = 36;
                await _dialogCoordinator.ShowMessageAsync(this, TranslationHelper.Instance.Translate("Warning"), TranslationHelper.Instance.Translate("Check_DoorLock"), MessageDialogStyle.Affirmative, settings);
            }

            await Homming(new CancellationTokenSource());
        }

        private async Task<bool> Homming(CancellationTokenSource cts)
        {
            HommingWindow hommingWindow = new HommingWindow(cts);
            return await Application.Current.MainWindow.ShowChildWindowAsync<bool>(hommingWindow, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }
        
        private async void ExportResult()
        {
            MetroDialogSettings settings = new MetroDialogSettings();
            settings.DialogMessageFontSize = 24;
            settings.DialogTitleFontSize = 36;

            ExcelExportWindow excelExportWindow = new ExcelExportWindow(DefectStorage);
            await Application.Current.MainWindow.ShowChildWindowAsync<bool>(excelExportWindow, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void InnerLightOn()
        {
            var deviceBox = SystemManager.Instance().DeviceBox;
            var port = deviceBox.PortMap.GetOutPort(PortMap.OutPortName.OutInnerLight);
            deviceBox.DigitalIoHandler.WriteOutput(port, IsInnerLightOn);

            IsInnerLightOn = !IsInnerLightOn;
        }

        private void DoorLock()
        {
            var deviceBox = SystemManager.Instance().DeviceBox;
            var port = deviceBox.PortMap.GetOutPort(PortMap.OutPortName.OutDoor);
            deviceBox.DigitalIoHandler.WriteOutput(port, IsDoorLock);

            IsDoorLock = !IsDoorLock;
        }
    }
}
