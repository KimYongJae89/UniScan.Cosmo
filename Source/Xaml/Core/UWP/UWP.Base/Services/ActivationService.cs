using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UWP.Base.Activation;
using UWP.Base.Helpers;
using UWP.Base.Services;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWP.Base.Views;
using UWP.Base.ViewModels;
using Windows.UI.Xaml.Input;
using Windows.System;
using System.Threading;
using UWP.Base.Settings;
using Standard.DynMvp.Base.Helpers;
using UWP.Base.Services.UwpTemplate.Core.Services;
using UWP.Base.Models;

namespace UWP.Base.Services
{
    // For more information on application activation see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.md
    public class ActivationService
    {
        private ManualResetEvent setupEvent = new ManualResetEvent(true);

        private readonly Application _app;
        private readonly Type _defaultNavItem;
        private Lazy<UIElement> _shell;
       
        private object _lastActivationArgs;
        
        private UserDataService UserDataService => Singleton<UserDataService>.Instance;

        public ActivationService(Application app, Type defaultNavItem, Lazy<UIElement> shell = null)
        {
            _app = app;
            _shell = shell;
            _defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            var args = activationArgs as LaunchActivatedEventArgs;

            if (IsInteractive(args))
            {
                ExtendedSplashPage splashScreen = new ExtendedSplashPage();
                splashScreen.Initialize(args);

                Window.Current.Content = splashScreen;
                Window.Current.Activate();

                await InitializeAsync(splashScreen.ViewModel);

                Window.Current.Content = null;
                if (Window.Current.Content == null)
                {
                    Window.Current.Content = _shell?.Value ?? new Frame();
                }
            }

            await HandleActivationAsync(args);
            _lastActivationArgs = args;

            if (IsInteractive(args))
            {
                Window.Current.Activate();
                
                await StartupAsync();

                Window.Current.Content = _shell?.Value ?? new Frame();
            }
        }

        private async void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            setupEvent.Reset();

            var settingsPage = new SettingsPage(setupEvent);
            Window.Current.Content = settingsPage;
        }

        private async Task InitializeAsync(ExtendedSplashViewModel ViewModel)
        {
            ViewModel.Message = "System".GetLocalized() + " " + "Load".GetLocalized() + " .. ";

            WindowManagerService.Current.Initialize();
            await ThemeSelectorService.InitializeAsync();

            /////////////////////////////////////
            /// UniEye Code
            ///////////////////////////////////

            ViewModel.Message = "Setting".GetLocalized() + " " + "Load".GetLocalized() + " .. ";

            await Singleton<OperationSettings>.Instance.LoadFromSettingsAsync();
            await Singleton<DeviceSettings>.Instance.LoadFromSettingsAsync();
            
            var keyboardAccelerator = new KeyboardAccelerator() { Key = VirtualKey.F12 };
            keyboardAccelerator.Modifiers = VirtualKeyModifiers.Menu;
            keyboardAccelerator.Invoked += KeyboardAccelerator_Invoked;
            ViewModel.KeyboardAccelerators.Add(keyboardAccelerator);

            ViewModel.Message = "Startup".GetLocalized() + " .. 5";
            await Task.Delay(1000);
            ViewModel.Message = "Startup".GetLocalized() + " .. 4";
            await Task.Delay(1000);
            ViewModel.Message = "Startup".GetLocalized() + " .. 3";
            await Task.Delay(1000);
            ViewModel.Message = "Startup".GetLocalized() + " .. 2";
            await Task.Delay(1000);
            ViewModel.Message = "Startup".GetLocalized() + " .. 1";
            await Task.Delay(1000);

            await Task.Run(() => setupEvent.WaitOne());

            ViewModel.KeyboardAccelerators.Remove(keyboardAccelerator);

            ViewModel.Message = "Model".GetLocalized() + " " + "Initialze".GetLocalized() + " .. ";
            
            await TargetDataService.LoadFromSettingsAsync();

            ViewModel.Message = "Device".GetLocalized() + " " + "Initialze".GetLocalized() + " .. ";

            await DeviceService.Initialize();
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
            var activationHandler = GetActivationHandlers()
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));

            if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultLaunchActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandle(activationArgs))
                {
                    await defaultHandler.HandleAsync(activationArgs);
                }
            }
        }

        private async Task StartupAsync()
        {
            await ThemeSelectorService.SetRequestedThemeAsync();
            //await Singleton<PathSettings>.Instance.
            
            //Singleton<LiveTileService>.Instance.SampleUpdate();
            //await WhatsNewDisplayService.ShowIfAppropriateAsync();
        }

        private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            //yield return Singleton<SuspendAndResumeService>.Instance;
            //yield return Singleton<LiveTileService>.Instance;
            yield return Singleton<ToastNotificationsService>.Instance;
            //yield return Singleton<BackgroundTaskService>.Instance;
        }

        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }
    }
}
