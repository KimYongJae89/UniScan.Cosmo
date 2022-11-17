using Standard.DynMvp.Base.Helpers;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using UWP.Base.Helpers;
using UWP.Base.Services;
using UWP.SEMCNS.Services;

namespace UWP.SEMCNS.ViewModels
{
    public class InspectViewModel : Observable
    {
        ICommand _grabCommand;
        public ICommand GrabCommand => _grabCommand ?? (_grabCommand = new RelayCommand(() => Grab()));

        ICommand _inspectCommand;
        public ICommand InspectCommand => _inspectCommand ?? (_inspectCommand = new RelayCommand(() => Grab()));

        InspectService _inspectService;

        public InspectViewModel()
        {
        }

        public void Initialize(InspectService inspectService)
        {
            _inspectService = inspectService;
        }

        public async void Grab()
        {
            await Task.Run(() =>
            {
                foreach (var lightController in DeviceService.LightControllers)
                    lightController.TurnOn(255);

                foreach (var grabber in DeviceService.Grabbers)
                    grabber.GrabOnce();

                foreach (var lightController in DeviceService.LightControllers)
                    lightController.TurnOff();
                //foreach (var grabber in DeviceService.LightControllers)
                //    grabber.GrabOnce();
            });
        }

        public async void Inspect()
        {
            await _inspectService.Inspect();
            //await Task.Run(() =>
            //{
            //    foreach (var lightController in DeviceService.LightControllers)
            //        lightController.TurnOn();

            //    foreach (var grabber in DeviceService.Grabbers)
            //        grabber.GrabOnce();

            //    //foreach (var grabber in DeviceService.LightControllers)
            //    //    grabber.GrabOnce();
            //});
        }
    }
}
