using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Devices;
using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UniEye.Base;
using UniEye.Base.Util;
using WPF.Base.Controls;
using WPF.Base.Services;
using WPF.Base.Views;
using WPF.COSMO.Offline.Controls.View;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;
using WPF.COSMO.Offline.Views;
using static WPF.COSMO.Offline.Models.PortMap;

namespace WPF.COSMO.Offline
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

            //DefectListControl defectListControl = new DefectListControl();

            //Application.Current.title

            //Window1 window1 = new Window1();
            //Application.Current.MainWindow = window1;
            //Application.Current.MainWindow.ShowDialog();
            _activationService = new Lazy<ActivationService>(CreateActivationService);
            await ActivationService.ActivateAsync(e);
            //ActivationService.Activate(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LogHelper.Debug(LoggerType.Debug, "Exit");

            MatroxHelper.FreeApplication();

            var servoOnPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutServoOn);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(servoOnPort, false);

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            var doorPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.OutPortName.OutDoor);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(doorPort, false);

            var lightPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.OutPortName.OutInnerLight);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(lightPort, false);

            MicroscopeGrabService.ReleaseOptotune();

            base.OnExit(e);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, BuildSystemManager, Initialize, 
                new Lazy<ShellWindow>(CreateShell), 
                new Lazy<IEnumerable<NavigationMenuItem>>(CreateMenuItems), 
                new Lazy<IEnumerable<NavigationMenuItem>>(CreateOptionMenuItems),
                new Lazy<ICustomSettingControl>(CreateCustomSettingControl));
        }

        private void BuildSystemManager()
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

            ApplicationHelper.InitLogSystem();
        }

        private async Task Initialize()
        {
            MatroxHelper.InitApplication();

            ThreadPool.SetMinThreads(64, 16);

            var servoOnPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutServoOn);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(servoOnPort, true);

            var greenPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutGreen);
            var yellowPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutYellow);
            var redPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(OutPortName.OutRed);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(greenPort, false);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(yellowPort, true);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(redPort, false);
            
            var doorPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.OutPortName.OutDoor);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(doorPort, false);

            var lightPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.OutPortName.OutInnerLight);
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(lightPort, false);

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            SystemManager.Instance().DeviceController.Initialize(SystemManager.Instance().DeviceBox);

            await AxisGrabService.InitializeAsync();
            await MicroscopeGrabService.LoadMicroscopeServiceSettings();
            await SectionService.LoadSectionSettings();
            await LotNoService.LoadAxisGrabInfos();
            ModelService_COSMO.Initialize();

            await Model_COSMO.LoadParam();


            if (await MicroscopeGrabService.InitializeOptotune() == false)
                throw new Exception("Optotune");
        }

        private ShellWindow CreateShell()
        {
            return new ShellWindow();
        }


        private ICustomSettingControl CreateCustomSettingControl()
        {
            return new CustomSettingControl();
        }

        private IEnumerable<NavigationMenuItem> CreateMenuItems()
        {
            return new NavigationMenuItem[]
            {
                new NavigationMenuItem("Model", char.ConvertFromUtf32(0xE82D), new ModelPage()),
                new NavigationMenuItem("Inspect", char.ConvertFromUtf32(0xEF3B), new InspectPage()),
                new NavigationMenuItem("Report", char.ConvertFromUtf32(0xEADF), new ReportPage())
            };
        }

        private IEnumerable<NavigationMenuItem> CreateOptionMenuItems()
        {
            var user = UserHandler.Instance().CurrentUser;
            if (user.UserType != UserType.Admin)
                return null;
            
            return new NavigationMenuItem[]
            {
//#if DEBUG
                new NavigationMenuItem("Developer", char.ConvertFromUtf32(0xE7EE), new DeveloperPage(), true)
//#endif          
            };
        }


    }
}
