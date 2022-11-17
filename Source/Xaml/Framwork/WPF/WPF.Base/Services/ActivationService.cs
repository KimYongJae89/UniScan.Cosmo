using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UniEye.Base;
using UniEye.Base.Settings;
using UniEye.Base.Settings.UI;
using UniEye.Base.Util;
using WPF.Base.Controls;
using WPF.Base.Extensions;
using WPF.Base.Helpers;
using WPF.Base.ViewModels;
using WPF.Base.Views;

namespace WPF.Base.Services
{
    // For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.md
    public class ActivationService
    {
        private ManualResetEvent setupEvent = new ManualResetEvent(true);

        private Action _buildSystemManagerAction;
        private Func<Task> _initializeFunc;
        private Lazy<ShellWindow> _shell;
        private Lazy<IEnumerable<NavigationMenuItem>> _menuItems;
        private Lazy<IEnumerable<NavigationMenuItem>> _optionMenuItems;
        private Lazy<ICustomSettingControl> _customSettingControl;

        public ActivationService(
            Application app, 
            Action buildSystemManagerAction,
            Func<Task> initializeFunc, 
            Lazy<ShellWindow> shell, 
            Lazy<IEnumerable<NavigationMenuItem>> menuItems, 
            Lazy<IEnumerable<NavigationMenuItem>> optionMenuItems,
            Lazy<ICustomSettingControl> customSettingControl = null)
        {
            _buildSystemManagerAction = buildSystemManagerAction;
            _initializeFunc = initializeFunc;
            _shell = shell;
            _menuItems = menuItems;
            _optionMenuItems = optionMenuItems;
            _customSettingControl = customSettingControl;
        }

        public void Activate(StartupEventArgs e)
        {
            ApplicationHelper.LoadSettings();

            TranslationHelper.Instance.CurrentCultureInfo = CultureInfo.CurrentUICulture;
            Task t = Task.Run(async() => await ThemeSelectorService.InitializeAsync());
            t.Wait();

            var splashWindow = new SplashWindow();
            splashWindow.Initialize(new RelayCommand(SetupExecute));

            var viewModel = splashWindow.ViewModel;

            Application.Current.MainWindow = splashWindow;
            Application.Current.MainWindow.Show();

            t = Task.Run(() => setupEvent.WaitOne());

            _buildSystemManagerAction();

            t.Wait();

            t = Task.Run(async () => await PublicSetup(viewModel));
            t.Wait();

            t = Task.Run(async () => await _initializeFunc());
            t.Wait();

            t = Task.Run(async () => await ModelService.Instance.LoadFromSettingsAsync());
            t.Wait();

            viewModel.Message = "UI".GetLocalized() + " " + "Init".GetLocalized() + "...";

            if (_customSettingControl == null)
                _shell.Value.Initialize(_menuItems.Value, _optionMenuItems.Value);
            else
                _shell.Value.Initialize(_menuItems.Value, _optionMenuItems.Value, _customSettingControl.Value);

            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = _shell.Value;

            Application.Current.MainWindow.Show();
        }

        public async Task ActivateAsync(StartupEventArgs e)
        {
            ApplicationHelper.LoadSettings();

            TranslationHelper.Instance.CurrentCultureInfo = CultureInfo.CurrentUICulture;
            await ThemeSelectorService.InitializeAsync();
            
            var splashWindow = new SplashWindow();
            splashWindow.Initialize(new RelayCommand(SetupExecute));

            var viewModel = splashWindow.ViewModel;

            Application.Current.MainWindow = splashWindow;
            Application.Current.MainWindow.Show();

            viewModel.Message = "Startup".GetLocalized() + " .. 5";
            await Task.Delay(1000);
            viewModel.Message = "Startup".GetLocalized() + " .. 4";
            await Task.Delay(1000);
            viewModel.Message = "Startup".GetLocalized() + " .. 3";
            await Task.Delay(1000);
            viewModel.Message = "Startup".GetLocalized() + " .. 2";
            await Task.Delay(1000);
            viewModel.Message = "Startup".GetLocalized() + " .. 1";
            await Task.Delay(1000);

            await Task.Run(() => setupEvent.WaitOne());

            _buildSystemManagerAction();

            await PublicSetup(viewModel);

            await _initializeFunc();

            await ModelService.Instance.LoadFromSettingsAsync();

            viewModel.Message = "UI".GetLocalized() + " " + "Init".GetLocalized() + "...";

            if (_customSettingControl == null)
                _shell.Value.Initialize(_menuItems.Value, _optionMenuItems.Value);
            else
                _shell.Value.Initialize(_menuItems.Value, _optionMenuItems.Value, _customSettingControl.Value);

            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = _shell.Value;
            
            Application.Current.MainWindow.Show();
        }

        private async void SetupExecute()
        {
            ConfigForm configForm = new ConfigForm();
            await Task.Run(() =>
                {
                    setupEvent.Reset();

                    configForm.ShowDialog();

                    setupEvent.Set();
                });
        }
        
        private Task PublicSetup(SplashViewModel viewModel)
        {
            return Task.Run(() =>
            {
                viewModel.Message = "Algorithm".GetLocalized() + " " + "Init".GetLocalized() + "...";

                SystemManager.Instance().BuildAlgorithmStrategy();
                //SystemManager.Instance().SelectAlgorithmStrategy();
                //if (AlgorithmBuilder.LicenseErrorCount > 0)
                //{
                //    //throw new Exception("License Authorize Fail");
                //}

                AlgorithmPool.Instance().Initialize(SystemManager.Instance().AlgorithmArchiver);

                viewModel.Message = "Device".GetLocalized() + " " + "Init".GetLocalized() + "...";

                SystemManager.Instance().DeviceBox?.Initialize(null);

                //SystemManager.Instance().DeviceController?.Initialize(SystemManager.Instance().DeviceBox);
                MotionService.Initialize();

                viewModel.Message = "Etc".GetLocalized() + " " + "Init".GetLocalized() + "...";

                //SystemManager.Instance().InitializeResultManager();

                //SystemManager.Instance().InitializeDataExporter();

                //SystemManager.Instance().InitializeAdditionalUnits();

                //SystemManager.Instance().InitalizeInspectRunner();
            });

            
        }


        //private async void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        //{
        //    setupEvent.Reset();

        //    var settingsPage = new SettingsPage(setupEvent);
        //    Window.Current.Content = settingsPage;
        //}

        //private async Task InitializeAsync(ExtendedSplashViewModel ViewModel)
        //{
        //    ViewModel.Message = "System".GetLocalized() + " " + "Load".GetLocalized() + " .. ";

        //    WindowManagerService.Current.Initialize();
        //    await ThemeSelectorService.InitializeAsync();

        //    /////////////////////////////////////
        //    /// UniEye Code
        //    ///////////////////////////////////

        //    ViewModel.Message = "Setting".GetLocalized() + " " + "Load".GetLocalized() + " .. ";

        //    await Singleton<OperationSettings>.Instance.LoadFromSettingsAsync();
        //    await Singleton<DeviceSettings>.Instance.LoadFromSettingsAsync();

        //    var keyboardAccelerator = new KeyboardAccelerator() { Key = VirtualKey.F12 };
        //    keyboardAccelerator.Modifiers = VirtualKeyModifiers.Menu;
        //    keyboardAccelerator.Invoked += KeyboardAccelerator_Invoked;
        //    ViewModel.KeyboardAccelerators.Add(keyboardAccelerator);

        //    ViewModel.Message = "Startup".GetLocalized() + " .. 5";
        //    await Task.Delay(1000);
        //    ViewModel.Message = "Startup".GetLocalized() + " .. 4";
        //    await Task.Delay(1000);
        //    ViewModel.Message = "Startup".GetLocalized() + " .. 3";
        //    await Task.Delay(1000);
        //    ViewModel.Message = "Startup".GetLocalized() + " .. 2";
        //    await Task.Delay(1000);
        //    ViewModel.Message = "Startup".GetLocalized() + " .. 1";
        //    await Task.Delay(1000);

        //    await Task.Run(() => setupEvent.WaitOne());

        //    ViewModel.KeyboardAccelerators.Remove(keyboardAccelerator);

        //    ViewModel.Message = "Model".GetLocalized() + " " + "Initialze".GetLocalized() + " .. ";

        //    await TargetDataService.LoadFromSettingsAsync();

        //    ViewModel.Message = "Device".GetLocalized() + " " + "Initialze".GetLocalized() + " .. ";

        //    await DeviceService.Initialize();
        //}

        //private async Task HandleActivationAsync(object activationArgs)
        //{
        //    var activationHandler = GetActivationHandlers()
        //                                        .FirstOrDefault(h => h.CanHandle(activationArgs));

        //    if (activationHandler != null)
        //    {
        //        await activationHandler.HandleAsync(activationArgs);
        //    }

        //    if (IsInteractive(activationArgs))
        //    {
        //        var defaultHandler = new DefaultLaunchActivationHandler(_defaultNavItem);
        //        if (defaultHandler.CanHandle(activationArgs))
        //        {
        //            await defaultHandler.HandleAsync(activationArgs);
        //        }
        //    }
        //}

        //private async Task StartupAsync()
        //{
        //    await ThemeSelectorService.SetRequestedThemeAsync();
        //    //await Singleton<PathSettings>.Instance.

        //    //Singleton<LiveTileService>.Instance.SampleUpdate();
        //    //await WhatsNewDisplayService.ShowIfAppropriateAsync();
        //}

        //private IEnumerable<ActivationHandler> GetActivationHandlers()
        //{
        //    //yield return Singleton<SuspendAndResumeService>.Instance;
        //    //yield return Singleton<LiveTileService>.Instance;
        //    yield return Singleton<ToastNotificationsService>.Instance;
        //    //yield return Singleton<BackgroundTaskService>.Instance;
        //}

        //private bool IsInteractive(object args)
        //{
        //    return args is IActivatedEventArgs;
        //}
    }
}
