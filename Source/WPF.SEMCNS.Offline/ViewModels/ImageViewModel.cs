using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using UniEye.Base;
using WPF.Base.Helpers;
using WPF.Base.Services;
using WPF.SEMCNS.Offline.Models;
using WPF.SEMCNS.Offline.Services;

namespace WPF.SEMCNS.Offline.ViewModels
{
    public class ImageViewModel : Observable
    {
        double _y;
        public double Y
        {
            get { return _y; }
            set { Set(ref _y, value); }
        }

        double _x;
        public double X
        {
            get { return _x; }
            set { Set(ref _x, value); }
        }

        byte _value;
        public byte Value
        {
            get { return _value; }
            set { Set(ref _value, value); }
        }

        public TargetParam Param { get => TargetService.Current; }
        
        ImageSource _grabbedImage;
        public ImageSource GrabbedImage
        {
            get { return _grabbedImage; }
            set
            {
                Set(ref _grabbedImage, value);
                _zoomService.FitToSize(_grabbedImage.Width, _grabbedImage.Height);
                OnPropertyChanged("TargetService");
                OnPropertyChanged("EndY");
                OnPropertyChanged("EndHeight");
            }
        }

        ICommand _zoomFitCommand;

        public ICommand ZoomFitCommand => _zoomFitCommand ?? (_zoomFitCommand = new RelayCommand(() =>
            {
                if (_grabbedImage != null)
                    _zoomService.FitToSize(_grabbedImage.Width, _grabbedImage.Height);
            }));

        DefectOverlay _defectOverlay;
        public DefectOverlay DefectOverlay
        {
            get { return _defectOverlay; }
            set
            {
                Set(ref _defectOverlay, value);
            }
        }

        private ZoomService _zoomService;
        public ZoomService ZoomService
        {
            get { return _zoomService; }
        }

        public ImageViewModel()
        {
            
        }

        private void ZoomFit()
        {

        }

        public void Initialize(ZoomService zoomService, bool isGrabControl)
        {
            _zoomService = zoomService;
            
            if (isGrabControl)
                SystemManager.Instance().DeviceBox.ImageDeviceHandler.AddImageGrabbed(ImageGrabbed);
        }

        private async void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            GrabbedImage = await Task<ImageSource>.Run(() =>
            {
                DynMvp.Base.Image2D image = imageDevice.GetGrabbedImage(ptr) as DynMvp.Base.Image2D;
                AlgoImage algoImage = ImageBuilder.GetInstance(ImagingLibrary.MatroxMIL).Build(image, ImageType.Grey);

                var source = algoImage.ToBitmapSource();
                algoImage.Dispose();

                return source;
            });
        }

        public void UpdateDefectOverlay(Defect defect)
        {
            var defectCenterX = defect.Region.X + defect.Region.Width / 2;
            var defectCenterY = defect.Region.Y + defect.Region.Height / 2;
            var lines = new DefectLine[]
            {
                new DefectLine { StartX = defectCenterX, StartY = 0, EndX = defectCenterX, EndY = defect.Region.Y},
                new DefectLine { StartX = 0, StartY = defectCenterY, EndX = defect.Region.X, EndY = defectCenterY},
                new DefectLine { StartX = defectCenterX, StartY = defect.Region.Bottom, EndX = defectCenterX, EndY = _grabbedImage.Height},
                new DefectLine { StartX = defect.Region.Right, StartY = defectCenterY, EndX = _grabbedImage.Width, EndY = defectCenterY}
            };

            Point[] rotatePoints = new Point[4];
            for (int i = 0; i < 4; i++)
                rotatePoints[i] = new Point(defect.RotateX[i], defect.RotateY[i]);
            
            var overlay = new DefectOverlay { Rect = defect.Region, Lines = lines, PointCollection = new PointCollection(rotatePoints) };

            DefectOverlay = overlay;
        }
    }
}
