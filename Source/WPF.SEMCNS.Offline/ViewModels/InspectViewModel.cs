using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UniEye.Base;
using WPF.Base.Helpers;
using WPF.Base.Services;
using WPF.SEMCNS.Offline.Models;
using WPF.SEMCNS.Offline.Services;
using WPF.SEMCNS.Offline.Views;

namespace WPF.SEMCNS.Offline.ViewModels
{
    public class InspectViewModel
    {
        public TargetParam Param { get => TargetService.Current; }
        
        private IDialogCoordinator _dialogCoordinator;
        DynMvp.Base.Image2D _grabbedImage;

        ICommand _grabCommand;
        public ICommand GrabCommand => _grabCommand ?? (_grabCommand = new RelayCommand(Grab));

        ICommand _inspectCommand;
        public ICommand InspectCommand => _inspectCommand ?? (_inspectCommand = new RelayCommand(Inspect));

        BufferService _bufferService;

        ICommand _grabInspectCommand;
        public ICommand GrabInspectCommand => _grabInspectCommand ?? (_grabInspectCommand = new RelayCommand(GrabInspect));

        ICommand _hommingCommand;
        public ICommand HommingCommand => _hommingCommand ?? (_hommingCommand = new RelayCommand(Homming));

        public void Initialize(IDialogCoordinator dialogCoordinator)
        {
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.AddImageGrabbed(ImageGrabbed);
            _dialogCoordinator = dialogCoordinator;

            if (TargetService.Current == null)
            {
                if (TargetService.TargetList.Count == 0)
                {
                    TargetService.TargetList.Add(new TargetParam());
                }

                TargetService.Current = TargetService.TargetList.First();
            }

            foreach (var camera in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
            {
                _bufferService = new BufferService(camera as Camera);
                break;
            }
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

        public async void Grab()
        {
            string grabHedear = "Grab";

            ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, grabHedear, "Initialize..");
            controller.SetIndeterminate();

            var cancellationTokenSource = new CancellationTokenSource();

            var axisHandler = SystemManager.Instance().DeviceController.RobotStage;

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
            controller.SetMessage("Init Postion..");

            if (axisHandler != null)
            {
                axisHandler.StartMultipleMove(TargetAxis.Start);
                await WaitMoveDone(axisHandler, cancellationTokenSource);
            }

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(new LightValue(new int[] { (int)Param.LightValue, }));
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();

            controller.SetProgress(0.6);
            controller.SetMessage("Grab..");

            if (axisHandler != null)
            {
                axisHandler.StartCmp("X", (int)TargetAxis.Start.Position[0], 26, true);
                axisHandler.StartMultipleMove(TargetAxis.End);
                await WaitMoveDone(axisHandler, cancellationTokenSource);
            }
            
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            axisHandler?.EndCmp("X");

            controller.SetProgress(1);
            controller.SetMessage("Grab Done..");
            
            await controller.CloseAsync();
        }

        private void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            _grabbedImage = imageDevice.GetGrabbedImage(ptr) as DynMvp.Base.Image2D;
        }

        private async void Inspect()
        {
            string inspectHedear = "Inspect";

            ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, inspectHedear, "Initialize..");
            controller.SetIndeterminate();

            if (_grabbedImage == null)
            {
                await _dialogCoordinator.ShowMessageAsync(this, inspectHedear, "Please, Image Grab.");
                await controller.CloseAsync();
                return;
            }
            
            controller.SetProgress(0.1);
            controller.SetMessage("Processing Start..");

            var width = _grabbedImage.Width;
            var height = _grabbedImage.Height;
            
            controller.SetProgress(0.2);
            controller.SetMessage("Get Source Array..");

            AlgoImage algoImage = ImageBuilder.GetInstance(ImagingLibrary.MatroxMIL).Build(_grabbedImage, ImageType.Grey);

            var sourceArray = await InspectService.GetTransposeBuffer(_grabbedImage);
            _bufferService.TransposeImage.SetByte(sourceArray);

            controller.SetProgress(0.3);
            controller.SetMessage("Get Mask Array..");
            
            var maskArray = await InspectService.GetMaskArray(_bufferService.TransposeImage, _bufferService.TransposeBuffer, Param);
             
            controller.SetProgress(0.4);
            controller.SetMessage("Get Profile..");

            var profile = await InspectService.GetProfile(sourceArray, maskArray, width, height);

            controller.SetProgress(0.5);
            controller.SetMessage("Get Defect Map..");
            await InspectService.GetDefectArray(Param, sourceArray, maskArray, _bufferService.LowerArray, _bufferService.UpperArray, profile, width, height);
          
            controller.SetProgress(0.6);
            controller.SetMessage("Get Dust List..");
            await InspectService.GetInversTransposeImage(_bufferService.DefectBuffer, _bufferService.LowerArray);
            var blobList = await InspectService.GetBlobList(Param, _grabbedImage, _bufferService.DefectBuffer);

            controller.SetProgress(0.7);
            controller.SetMessage("Get Pin Hole List..");
            await InspectService.GetInversTransposeImage(_bufferService.DefectBuffer, _bufferService.UpperArray);
            blobList = blobList.Concat(await InspectService.GetBlobList(Param, _grabbedImage, _bufferService.DefectBuffer));

            blobList = blobList.OrderByDescending(blob => blob.Area);

            controller.SetProgress(0.8);
            controller.SetMessage("Get Result..");
            var defectList = await InspectService.GetDefectList(Param, _grabbedImage, blobList, profile);

            await InspectService.inspectedProc(defectList);

            controller.SetProgress(0.9);
            controller.SetMessage("Result Save..");

            await ResultService.SaveResultAsync(new Result() { InspectTime = DateTime.Now, Defects = defectList, ImageSource = algoImage.ToBitmapSource(), TargetParam = Param });

            controller.SetProgress(1);
            controller.SetMessage("Inspect Done..");

            algoImage.Dispose();

            await controller.CloseAsync();
        }

        private async void GrabInspect()
        {
            string grabHedear = "Grab_Inspect";

            ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, grabHedear, "Initialize..");
            controller.SetIndeterminate();

            var cancellationTokenSource = new CancellationTokenSource();

            var axisHandler = SystemManager.Instance().DeviceController.RobotStage;

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

            controller.SetProgress(0.1);
            controller.SetMessage("Init Postion..");

            if (axisHandler != null)
            {
                axisHandler.StartMultipleMove(TargetAxis.Start);
                await WaitMoveDone(axisHandler, cancellationTokenSource);
            }

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(new LightValue(new int[] { (int)Param.LightValue, }));
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();

            controller.SetProgress(0.2);
            controller.SetMessage("Grab..");

            if (axisHandler != null)
            {
                axisHandler.StartCmp("X", (int)TargetAxis.Start.Position[0], 26, true);
                axisHandler.StartMultipleMove(TargetAxis.End);
                await WaitMoveDone(axisHandler, cancellationTokenSource);
            }

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            axisHandler?.EndCmp("X");

            controller.SetProgress(0.3);
            controller.SetMessage("Grab Done..");

            string inspectHedear = "Inspect";

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (stopwatch.ElapsedMilliseconds < 5000 && _grabbedImage == null)
                await Task.Delay(100);

            stopwatch.Stop();

            if (_grabbedImage == null)
            {
                await _dialogCoordinator.ShowMessageAsync(this, inspectHedear, "Please, Image Grab.");
                await controller.CloseAsync();
                return;
            }

            controller.SetProgress(0.4);
            controller.SetMessage("Processing Start..");

            var width = _grabbedImage.Width;
            var height = _grabbedImage.Height;

            controller.SetProgress(0.5);
            controller.SetMessage("Get Source Array..");

            AlgoImage algoImage = ImageBuilder.GetInstance(ImagingLibrary.MatroxMIL).Build(_grabbedImage, ImageType.Grey);

            var sourceArray = await InspectService.GetTransposeBuffer(_grabbedImage);
            _bufferService.TransposeImage.SetByte(sourceArray);

            controller.SetProgress(0.6);
            controller.SetMessage("Get Mask Array..");

            var maskArray = await InspectService.GetMaskArray(_bufferService.TransposeImage, _bufferService.TransposeBuffer, Param);

            controller.SetProgress(0.7);
            controller.SetMessage("Get Profile..");

            var profile = await InspectService.GetProfile(sourceArray, maskArray, width, height);

            controller.SetProgress(0.8);
            controller.SetMessage("Get Defect Map..");
            await InspectService.GetDefectArray(Param, sourceArray, maskArray, _bufferService.LowerArray, _bufferService.UpperArray, profile, width, height);

            controller.SetProgress(0.85);
            controller.SetMessage("Get Dust List..");
            await InspectService.GetInversTransposeImage(_bufferService.DefectBuffer, _bufferService.LowerArray);
            var blobList = await InspectService.GetBlobList(Param, _grabbedImage, _bufferService.DefectBuffer);

            controller.SetProgress(0.9);
            controller.SetMessage("Get Pin Hole List..");
            await InspectService.GetInversTransposeImage(_bufferService.DefectBuffer, _bufferService.UpperArray);
            blobList = blobList.Concat(await InspectService.GetBlobList(Param, _grabbedImage, _bufferService.DefectBuffer));
            blobList = blobList.OrderByDescending(blob => blob.Area);

            controller.SetProgress(0.95);
            controller.SetMessage("Get Result..");
            var defectList = await InspectService.GetDefectList(Param, _grabbedImage, blobList, profile);

            await InspectService.inspectedProc(defectList);

            controller.SetProgress(1);
            controller.SetMessage("Result Save..");

            await ResultService.SaveResultAsync(new Result() { InspectTime = DateTime.Now, Defects = defectList, ImageSource = algoImage.ToBitmapSource(), TargetParam = Param });

            algoImage.Dispose();

            await controller.CloseAsync();
        }

        private async void Homming()
        {
            string grabHedear = "Home";

            ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, grabHedear, "Initialize..");
            controller.SetIndeterminate();

            var cancellationTokenSource = new CancellationTokenSource();

            var axisHandler = SystemManager.Instance().DeviceController.RobotStage;

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

            controller.SetProgress(0.5);
            controller.SetMessage("Homming..");

            if (axisHandler != null)
            {
                axisHandler.StartMultipleMove(TargetAxis.End);
                await WaitMoveDone(axisHandler, cancellationTokenSource);
            }

            await controller.CloseAsync();
        }
    }
}
