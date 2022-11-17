using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UniEye.Base;
using WPF.Base.Helpers;
using WPF.Base.Models;
using WPF.Base.Services;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Models
{
    public enum ScanDirection
    {
        LeftToRight, RightToLeft
    }
    
    public class AxisGrabEventArgs : EventArgs
    {
        AxisImage _axisImage;
        public AxisImage AxisImage => _axisImage;

        public AxisGrabEventArgs(AxisImage axisImage)
        {
            _axisImage = axisImage;
        }
    }

    public class BindingValue<T>
    {
        public T Value { get; set; }
    }
    
    //public class MicroscopeInfo :

    public class AxisGrabInfo : Observable
    {
        string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ScanDirection ScanDirection { get; set;}

        [JsonRequired]
        public ImageDevice ImageDevice { get; set; }

        public ImageDeviceEventDelegate ImageGrabbed { get => ImageDevice.ImageGrabbed; set => ImageDevice.ImageGrabbed = value; }

        public IoPort TriggerPort { get; set; }
        public int AxisIndex { get; set; }

        public ObservableCollection<BindingValue<int>> LightIndexList { get; } = new ObservableCollection<BindingValue<int>>();

        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int CenterX => (int)((MaxX + MinX) / 2);

        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

        public int LineOffset { get; set; }

        public double EmptyValue { get; set; }

        [JsonIgnore]
        public int NextX { get; set; }
        
        [JsonIgnore]
        public int ImageWidth => ImageDevice.ImageSize.Width;

        [JsonIgnore]
        public int ImagePitch => ImageDevice.ImagePitch;

        [JsonIgnore]
        public int ImageHeight => ImageDevice.ImageSize.Height;

        [JsonIgnore]
        public double GrabWidth => ImageDevice.ImageSize.Width * AxisGrabService.Settings.Resolution;


        int _curX;
        [JsonIgnore]
        public int CurX
        {
            get => _curX;
            set => Set(ref _curX, value);
        }
        
        [JsonIgnore]
        public LineModel LineModel { get; set; }

        [JsonIgnore]
        public LineModel LeftLineModel { get; set; }

        [JsonIgnore]
        public LineModel RightLineModel { get; set; }

        [JsonIgnore]
        public LineModel TopLineModel { get; set; }

        [JsonIgnore]
        public LineModel BottomLineModel { get; set; }
        
        int _curY;
        [JsonIgnore]
        public int CurY
        {
            get => _curY;
            set => Set(ref _curY, value);
        }


        ICommand _lightIndexAddCommand;

        [JsonIgnore]
        public ICommand LightIndexAddCommand => _lightIndexAddCommand ?? (_lightIndexAddCommand = new RelayCommand(new Action(() => LightIndexList.Add(new BindingValue<int>()))));
        
        public AxisGrabInfo(ImageDevice imageDevice)
        {
            ImageDevice = imageDevice;
            
            MotionService.PositionChanged += MotionService_PositionChanged;
        }

        public void Initialize()
        {
            //int margin = 100;

            //int x = ScanDirection == ScanDirection.LeftToRight ? Math.Max(AlignLT.X, AlignLB.X) + margin : MinX;
            //int y = Math.Min(AlignLT.Y, AlignRT.Y) + margin;
            //double width = ScanDirection == ScanDirection.LeftToRight ? MaxX - x : Math.Min(AlignRT.X, AlignRB.X) - margin;
            //int height = Math.Min(AlignLB.Y, AlignRB.Y) - y - margin;

            //_moveLength = width / (width / ScanWidth);
            //_innerRegion = new Rectangle(x, y, (int)width, height);

            //InspectMode = InspectMode.Edge;
        }

        private void MotionService_PositionChanged(object sender, EventArgs e)
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            if (MotionService.AxisPosition != null && AxisGrabService.Settings != null)
            {
                CurX = (int)MotionService.AxisPosition.Position[AxisIndex] + OffsetX;
                CurY = (int)MotionService.AxisPosition.Position[AxisGrabService.Settings.AxisIndex] + OffsetY;
            }
        }

        public void GrabMulti()
        {
            ImageDevice.GrabMulti();
        }

        public void GrabOnce()
        {
            ImageDevice.GrabOnce();
        }

        public void Stop()
        {
            ImageDevice.Stop();
            SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(TriggerPort, false);
        }
    }
}
