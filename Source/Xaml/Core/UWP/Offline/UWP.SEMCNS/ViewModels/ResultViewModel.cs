using Standard.DynMvp.Base.Helpers;
using System;
using System.Windows.Input;
using UWP.Base.Helpers;
using UWP.Base.Services;

namespace UWP.SEMCNS.ViewModels
{
    public class ResultViewModel : Observable
    {
        private ZoomService _zoomService;

        ICommand _zoomInCommand;
        ICommand _zoomOutCommand;
        ICommand _zoomResetCommand;
        ICommand _zoomFitCommand;

        public ICommand ZoomInCommand => _zoomInCommand ?? (_zoomInCommand = new RelayCommand(() => _zoomService.ZoomIn()));
        public ICommand ZoomOutCommand => _zoomOutCommand ?? (_zoomOutCommand = new RelayCommand(() => _zoomService.ZoomOut()));
        public ICommand ZoomResetCommand => _zoomResetCommand ?? (_zoomResetCommand = new RelayCommand(() => _zoomService?.ResetZoom()));
        public ICommand ZoomFitCommand => _zoomFitCommand ?? (_zoomFitCommand = new RelayCommand(() => _zoomService?.FitToScreen()));

        public ResultViewModel()
        {
        }

        public void Initialize(ZoomService zoomService)
        {
            _zoomService = zoomService;
        }
    }
}
