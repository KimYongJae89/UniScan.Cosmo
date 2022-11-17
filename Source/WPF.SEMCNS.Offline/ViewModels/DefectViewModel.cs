using DynMvp.Devices.MotionController;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UniEye.Base;
using WPF.Base.Helpers;
using WPF.Base.Services;
using WPF.SEMCNS.Offline.Models;
using WPF.SEMCNS.Offline.Services;

namespace WPF.SEMCNS.Offline.ViewModels
{
    public class DefectViewModel : Observable
    {
        const int viewInflate = 500;

        IEnumerable<Defect> _itemSource;
        public IEnumerable<Defect> ItemSource
        {
            get { return _itemSource; }
            set
            {
                Set(ref _itemSource, value);
                OnPropertyChanged("TotalCount");
                OnPropertyChanged("DustCount");
                OnPropertyChanged("PinHoleCount");
            }
        }

        public int? TotalCount { get => _itemSource?.Count(); }
        public int? DustCount { get => _itemSource?.Where(defect => defect.DefectType == DefectType.Dust).Count(); }
        public int? PinHoleCount { get => _itemSource?.Where(defect => defect.DefectType == DefectType.PInHole).Count(); }

        ImageViewModel _imageViewModel;

        private ZoomService _zoomService;
        public ZoomService ZoomService
        {
            get { return _zoomService; }
        }

        private IDialogCoordinator _dialogCoordinator;

        ICommand _moveDefectPosCommand;
        public ICommand MoveDefectPosCommand => _moveDefectPosCommand ?? (_moveDefectPosCommand = new RelayCommand<Defect>(MoveDefectPostion));

        Defect _selected;
        public Defect Selected
        {
            get { return _selected; }
            set
            {
                Set(ref _selected, value);
                _zoomService.FitToSize(_selected.Image.Width, _selected.Image.Height);
                
                _imageViewModel.UpdateDefectOverlay(_selected);
            }
        }

        ICommand _zoomCommand;
        public ICommand ZoomCommand => _zoomCommand ?? (_zoomCommand = new RelayCommand(() =>
        {
            if (_selected == null)
                return;

            var viewRect = _selected.Region;
            viewRect.Inflate(viewInflate, viewInflate);
            _imageViewModel.ZoomService.Zoom(viewRect);
        }));

        public DefectViewModel()
        {
            
        }

        public void Initialize(IDialogCoordinator dialogCoordinator, ZoomService zoomService, ImageViewModel imageViewModel, bool isInspectedControl)
        {
            _dialogCoordinator = dialogCoordinator;
            _zoomService = zoomService;
            _imageViewModel = imageViewModel;
            
            if (isInspectedControl)
                InspectService.Inspected += Inspected;
        }

        private void Inspected(InspectEventArgs e)
        {
            ItemSource = e.DefectList;
        }

        private async Task WaitMoveDone(AxisHandler axisHandler, CancellationTokenSource cancellationTokenSource)
        {
            await Task.Run(() =>
            {
                while (!axisHandler.IsMoveDone() || cancellationTokenSource.IsCancellationRequested)
                {

                }
            });
        }

        private async void MoveDefectPostion(Defect defect)
        {
            string defectHedear = "Defect";

            ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, defectHedear, "Initialize..");
            controller.SetIndeterminate();

            var axisHandler = SystemManager.Instance().DeviceController.RobotStage as AxisHandler;

            var cancellationTokenSource = new CancellationTokenSource();

            if (axisHandler != null)
            {
                if (!axisHandler.IsHomeDone())
                {
                    controller.SetMessage("Homming..");

                    axisHandler.StartMultipleHomeMove(cancellationTokenSource);
                    await WaitMoveDone(axisHandler, cancellationTokenSource);
                }
            }

            if (cancellationTokenSource.IsCancellationRequested)
            {
                await controller.CloseAsync();
                return;
            }

            controller.SetProgress(0.3);
            controller.SetMessage("Move..");

            if (axisHandler != null)
            {
                axisHandler.StartMultipleMove(new AxisPosition(new float[] { (float)(defect.Y * defect.Resolution) }));
                await WaitMoveDone(axisHandler, cancellationTokenSource);
            }

            controller.SetProgress(1);
            controller.SetMessage("Move Done..");

            await controller.CloseAsync();
        }
    }
}
