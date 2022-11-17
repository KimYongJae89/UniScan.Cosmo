using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UniEye.Base;
using WPF.Base.Controls;
using WPF.Base.Helpers;
using WPF.Base.Services;
using WPF.Base.Views;

namespace WPF.Base.ViewModels
{
    public class SettingViewModel : Observable
    {
        IDialogCoordinator _dialogCoordinator;
        public TranslationHelper TranslationHelper { get => TranslationHelper.Instance; }

        private ICustomSettingControl customSettingPage;
        public ICustomSettingControl CustomSettingPage
        {
            get => customSettingPage;
            set => Set(ref customSettingPage, value);
        }

        ICommand _languageChangedCommand;
        public ICommand LanguageChangedCommand => _languageChangedCommand ?? (_languageChangedCommand = new RelayCommand<CultureInfo>(LanguageChanged));

        ICommand towerLampSettingCommand;
        public ICommand TowerLampSettingCommand => towerLampSettingCommand ?? (towerLampSettingCommand = new RelayCommand(TowerLampSetting));

        ICommand saveCommand;
        public ICommand SaveCommand => saveCommand ?? (saveCommand = new RelayCommand(Save));

        public void Initialize(IDialogCoordinator dialogCoordinator, ICustomSettingControl userControl)
        {
            _dialogCoordinator = dialogCoordinator;
            CustomSettingPage = userControl;
        }

        public async void LanguageChanged(CultureInfo cultureInfo)
        {
            MetroDialogSettings settings = new MetroDialogSettings();
            settings.DialogMessageFontSize = 24;
            settings.DialogTitleFontSize = 36;

            ProgressDialogController controller =  await _dialogCoordinator.ShowProgressAsync(this, "Language", "Change..", false, settings);
            controller.SetIndeterminate();

            controller.SetProgress(0.5);

            await Task.Delay(500);

            TranslationHelper.Instance.CurrentCultureInfo = cultureInfo;

            controller.SetProgress(1);

            await controller.CloseAsync(); 
        }

        private async void TowerLampSetting()
        {
            TowerLampSettingWindow towerLampSettingWindow = new TowerLampSettingWindow();
            await Application.Current.MainWindow.ShowChildWindowAsync(towerLampSettingWindow);
        }

        public AppTheme AppTheme { get => ThemeSelectorService.Theme; set => ThemeSelectorService.Theme = value; }
        public Accent Accent { get => ThemeSelectorService.Accent; set => ThemeSelectorService.Accent = value; }

        public static IEnumerable<Accent> Accents { get => ThemeSelectorService.Accents; }
        public static IEnumerable<AppTheme> AppThemes { get => ThemeSelectorService.Themes; }

        void Save()
        {
            var towerLamp = SystemManager.Instance().DeviceController.TowerLamp;

            var actionList = new List<Action>();
            actionList.Add(() => towerLamp?.Save(UniEye.Base.Settings.PathSettings.Instance().Config));
            actionList.Add(() => CustomSettingPage?.Save());
            actionList.Add(() => ThemeSelectorService.SaveThemeInSettingsAsync());

            MessageWindowHelper.ShowProgress("저장", "설정파일을 저장중입니다.", actionList, new System.Threading.CancellationTokenSource());
        }
    }
}
