using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UniEye.Base;
using WPF.Base.Helpers;
using WPF.Base.Models;
using WPF.Base.Services;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;
using ResultService = WPF.COSMO.Offline.Services.ResultService;

namespace WPF.COSMO.Offline.Controls.ViewModel
{
    public class AxisImageViewModel : Observable
    {
        double _axisWidth = 0;
        public double AxisWidth
        {
            get { return _axisWidth; }
            set { Set(ref _axisWidth, value); }
        }

        double _axisHeight = 0;
        public double AxisHeight
        {
            get { return _axisHeight; }
            set { Set(ref _axisHeight, value); }
        }

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

        double _xMin = double.MaxValue;
        public double XMin
        {
            get { return _xMin; }
            set { Set(ref _xMin, value); }
        }

        double _yMin = double.MaxValue;
        public double YMin
        {
            get { return _yMin; }
            set { Set(ref _yMin, value); }
        }

        bool _showLines = true;
        public bool ShowLines
        {
            get { return _showLines; }
            set { Set(ref _showLines, value); }
        }

        bool _showDefects = true;
        public bool ShowDefects
        {
            get { return _showDefects; }
            set { Set(ref _showDefects, value); }
        }

        public ObservableCollection<EstimatedLine> Lines { get; } = new ObservableCollection<EstimatedLine>();
        public ServiceSettings Settings => AxisGrabService.Settings;

        ICommand _zoomFitCommand;
        public ICommand ZoomFitCommand => _zoomFitCommand ?? (_zoomFitCommand
            = new RelayCommand(() => _zoomService.FitToSize(_axisWidth + Inflate, _axisHeight + Inflate)));

        public ObservableCollection<AxisImageSource> ImageSources { get; } = new ObservableCollection<AxisImageSource>();

        public ObservableCollection<AxisImageSource> DefectImageSources { get; } = new ObservableCollection<AxisImageSource>();

        public IEnumerable<AxisGrabInfo> InfoSources => AxisGrabService.Settings.AxisGrabInfoList;

        private ZoomService _zoomService;
        public ZoomService ZoomService => _zoomService;

        public const int _inflate = 10000;
        public int Inflate => _inflate;
        
        public void Initialize(FrameworkElement element)
        {
            BindingOperations.EnableCollectionSynchronization(Lines, new object());
            BindingOperations.EnableCollectionSynchronization(ImageSources, new object());
            BindingOperations.EnableCollectionSynchronization(DefectImageSources, new object());

            double xMax = 0;
            double yMax = 0;

            foreach (var info in AxisGrabService.Settings.AxisGrabInfoList) 
            {
                XMin = Math.Min(_xMin, info.MinX + info.OffsetX);
                xMax = Math.Max(xMax, info.MaxX + info.OffsetX + info.GrabWidth);
            }

            YMin = Math.Min(_yMin, AxisGrabService.Settings.MinY);
            yMax = Math.Max(yMax, AxisGrabService.Settings.MaxY);

            _axisWidth = Math.Abs(xMax - _xMin);
            _axisHeight = yMax - _yMin;
            
            _zoomService = new ZoomService(element, _xMin - Inflate / 2, _yMin - Inflate / 2);
            
            AxisGrabService.Initialized += Initialized;
            AlignService.Estimated += Estimated;
            InspectService.FilmGrabbed += FilmGrabbed;
            InspectService.DrawDefectsDone += DrawDefectDone;
            ResultService.Initialized += Initialized;
            ResultService.Estimated += Estimated;
            ResultService.AxisImageLoaded += FilmGrabbed;
            ResultService.DrawDefectLoaded += DrawDefectDone;
        }


        void Estimated(IEnumerable<EstimatedLine> lines)
        {
            foreach (var line in lines)
                Lines.Add(line);
        }

        private void Initialized()
        {
            _zoomService.FitToSize(_axisWidth + Inflate, _axisHeight + Inflate);

            Lines.Clear();
            ImageSources.Clear();
            DefectImageSources.Clear();
        }

        private void FilmGrabbed(AxisImageSource axisImageSource)
        {
            lock (ImageSources)
                ImageSources.Add(axisImageSource);
        }

        private void DrawDefectDone(AxisImageSource axisImageSource)
        {
            lock (DefectImageSources)
                DefectImageSources.Add(axisImageSource);
        }
    }
}
