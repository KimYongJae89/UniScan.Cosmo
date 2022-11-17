using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UniEye.Base;
using UniEye.Base.Device;
using WPF.Base.Services;
using WPF.Base.ViewModels;
using MahApps.Metro;
using WPF.SEMCNS.Offline.Services;
using WPF.SEMCNS.Offline.Views;
using System.Globalization;
using WPF.Base.Views;
using WPF.Base.Helpers;
using WPF.Base.Controls;
using System.Windows.Media;

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

            CultureInfo.CurrentUICulture = new CultureInfo("en-US");

            _activationService = new Lazy<ActivationService>(CreateActivationService);
            await ActivationService.ActivateAsync(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, CreateShell, Setup);
        }

        private async void Setup()
        {
            SystemManager systemManager = new SystemManager();

            systemManager.Init(
                null,
                null,
                new DynMvp.Vision.AlgorithmArchiver(),
                new UniEye.Base.Device.DeviceBox(new PortMap()),
                new UniEye.Base.Device.DeviceController(),
                null);

            SystemManager.SetInstance(systemManager);

            await ModelService.LoadFromSettingsAsync();
            //await ResultService.LoadListAsync();
        }

        private Window CreateShell()
        {
            var shell = new ShellWindow(new NavigationMenuItem[] 
            {
                new NavigationMenuItem("Inspect","ㅁ", new InspectPage()),
                new NavigationMenuItem("Report", "ㅁ", new ReportPage())
            });
            
            return shell;
        }
    }
}
