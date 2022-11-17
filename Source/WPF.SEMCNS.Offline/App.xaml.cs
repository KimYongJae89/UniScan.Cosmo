using DynMvp.Devices;
using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using UniEye.Base;
using UniEye.Base.Device;
using WPF.Base.Services;
using WPF.Base.ViewModels;
using WPF.SEMCNS.Offline.Services;
using WPF.SEMCNS.Offline.Views;

namespace WPF.SEMCNS.Offline
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private Lazy<ActivationService> _activationService;
        private ActivationService ActivationService { get => _activationService.Value; }

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ThemeManager.ChangeAppStyle(Application.Current,ThemeManager.GetAccent("Blue"), ThemeManager.GetAppTheme("BaseDark"));
            
            _activationService = new Lazy<ActivationService>(CreateActivationService);
            await ActivationService.ActivateAsync(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(InspectPage), CreateShell, Setup);
        }
        
        private async void Setup(SplashViewModel viewModel)
        {
            SystemManager systemManager = new SystemManager();

            systemManager.Init(
                new TargetService(),
                null,
                new DynMvp.Vision.AlgorithmArchiver(),
                new UniEye.Base.Device.DeviceBox(new PortMap()),
                new UniEye.Base.Device.DeviceController(),
                null);

            SystemManager.SetInstance(systemManager);

            await TargetService.LoadFromSettingsAsync();
            await ResultService.LoadListAsync();
        }

        private Window CreateShell()
        {
            var shell = new Views.ShellWindow();
            shell.Initialize(typeof(InspectPage));
            return shell;
        }
    }
}
