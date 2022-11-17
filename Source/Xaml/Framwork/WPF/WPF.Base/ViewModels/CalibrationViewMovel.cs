using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using UniEye.Base;
using WPF.Base.Helpers;

namespace WPF.Base.ViewModels
{
    public class CalibrationViewMovel : Observable
    {
        public IEnumerable<ImageDevice> ImageDevices => SystemManager.Instance().DeviceBox.ImageDeviceHandler.ImageDeviceList;

        BitmapSource _curImage;
        public BitmapSource CurImage
        {
            get => _curImage;
            set => Set(ref _curImage, value);
        }

        ImageDevice _imageDevice;
        public ImageDevice ImageDevice
        {
            get => _imageDevice;
            set => Set(ref _imageDevice, value);
        }

        public int Threshold { get; set; } = 127;
        public int Scale { get; set; } = 1000;

        ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        double _contrast;
        public double Contrast
        {
            get => _contrast;
            set => Set(ref _contrast, value);
        }

        double _resolution;
        public double Resolution
        {
            get => _resolution;
            set => Set(ref _resolution, value);
        }

        ICommand _grabCommand;
        public ICommand GrabCommand => _grabCommand ?? (_grabCommand = new RelayCommand(Grab));

        ICommand _stopCommand;
        public ICommand StopCommand => _stopCommand ?? (_stopCommand = new RelayCommand(Stop));

        public CalibrationViewMovel()
        {
             ImageDevices.First();
        }

        private void Grab()
        {
            if (_imageDevice.ImageGrabbed != null)
                return;

            manualResetEvent.Reset();

            _imageDevice.ImageGrabbed += ImageGrabbed;
            _imageDevice.SetTriggerMode(TriggerMode.Software);
            _imageDevice.GrabMulti();
        }

        private void Stop()
        {
            if (_imageDevice.ImageGrabbed == null)
                return;
            
            _imageDevice.ImageGrabbed -= ImageGrabbed;
            _imageDevice.Stop();

            _imageDevice.SetTriggerMode(TriggerMode.Hardware);
        }

        private async void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (manualResetEvent.WaitOne(0))
                return;

            manualResetEvent.Set();

            await Task.Run(() =>
            {
                int width = imageDevice.ImageSize.Width;
                int height = imageDevice.ImageSize.Height;
                int pitch = imageDevice.ImagePitch;

                byte[] data = new byte[width * height];

                for (int src = 0, dest = 0; src < pitch * height; src += pitch, dest += width)
                    Marshal.Copy(ptr + src, data, dest, width);

                Task[] tasks = new Task[3];

                var resolutionTask = GetResolution(data, width, height);
                var bitmapSourceTask = GetBitmapSource(data, width, height);
                var contrastTask = GetContrast(data);

                Task.WaitAll(contrastTask, resolutionTask);

                CurImage = bitmapSourceTask.Result;
                Contrast = contrastTask.Result;
                Resolution = resolutionTask.Result;

                Task.Delay(10);
            });

            manualResetEvent.Reset();
        }

        private Task<double> GetContrast(byte[] data)
        {
            return Task.Run(() =>
            {
                double maxV = (double)data.Max();
                double minV = (double)data.Min();
                return Math.Abs((maxV - minV) / (maxV + minV));
            });
        }

        private Task<double> GetResolution(byte[] data, int width, int height)
        {
            return Task.Run(() =>
            {
                double[] profile = new double[width];

                for (int y = 0, index = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        profile[x] += data[index++];
                    }
                }

                bool founded = false;
                int startIndex = 0;

                List<int> centerList = new List<int>();
                for (int x = 0; x < width; x++)
                {
                    if (profile[x] / height < Threshold)
                    {
                        if (founded == false)
                        {
                            founded = true;
                            startIndex = x;
                        }
                    }
                    else
                    {
                        if (founded)
                        {
                            founded = false;
                            centerList.Add((x - startIndex) / 2 + startIndex);
                        }
                    }
                }

                if (centerList.Count > 2)
                {
                    centerList.Remove(centerList.First());
                    centerList.Remove(centerList.Last());
                }

                double sum = 0;
                for (int i = 0; i < centerList.Count - 1; i++)
                    sum += centerList[i + 1] - centerList[i];

                double length = sum / (centerList.Count - 1);
                
                if (length == 0)
                    return -1;

                return Scale / length;
            });
        }

        private Task<BitmapSource> GetBitmapSource(byte[] data, int width, int height)
        {
            return Task.Run(() =>
            {
                BitmapSource bitmapSource = BitmapSource.Create(width, height,
                            96, 96, System.Windows.Media.PixelFormats.Gray8, null,
                            data, width);

                bitmapSource.Freeze();

                return bitmapSource;
            });
        }
    }
}
