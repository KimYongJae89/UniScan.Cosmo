using DynMvp.Devices;
using DynMvp.Vision;
using DynMvp.Vision.Cuda;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UniEye.Base;
using UniEye.Base.Settings;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Controls;
using WPF.COSMO.Offline.Controls.ViewModel;
using WPF.COSMO.Offline.Models;

namespace WPF.COSMO.Offline.Services
{
    public class CalibrationProcess : Observable
    {
        public Dictionary<AxisGrabInfo, ProcessUnit> ProcessUnitDictionary { get; set; } = new Dictionary<AxisGrabInfo, ProcessUnit>();
       
        public bool IsError => Error != null;

        string _error;
        public string Error
        {
            get => _error;
            set
            {
                Set(ref _error, value);
                OnPropertyChanged("IsError");
            }
        }

        public CalibrationProcess()
        {
            foreach (var info in AxisGrabService.Settings.AxisGrabInfoList)
                ProcessUnitDictionary.Add(info, new ProcessUnit());
        }
    }

    public static class CalibrationService
    {
        static CancellationToken _token;
        static Dictionary<AxisGrabInfo, ConcurrentQueue<double[]>> _profileQueueDictionary = new Dictionary<AxisGrabInfo, ConcurrentQueue<double[]>>();
        public static Dictionary<AxisGrabInfo, double> CalibrationValueDictionary { get; set; } = new Dictionary<AxisGrabInfo, double>();

        static Model_COSMO _model;

        static CalibrationProcess _process;

        public static void Initialize(Model_COSMO model, CalibrationProcess process, CancellationToken token)
        {
            Release();

            _process = process;

            _token = token;
            _model = model;

            foreach (var info in AxisGrabService.Settings.AxisGrabInfoList)
            {
                _profileQueueDictionary.Add(info, new ConcurrentQueue<double[]>());
                info.NextX = info.ScanDirection == ScanDirection.LeftToRight ? info.MinX : info.MaxX;
            }

            AxisGrabService.CalibrationMode = true;
            AxisGrabService.AxisImageGrabbed += AxisImageGrabbed;
        }

        public static void Release()
        {
            _model = null;
            _process = null;

            _profileQueueDictionary.Clear();

            AxisGrabService.AxisImageGrabbed -= AxisImageGrabbed;
            AxisGrabService.CalibrationMode = false;
        }

        public static async Task<bool> CalibrationAsync()
        {
            var settings = AxisGrabService.Settings;

            AxisGrabService.NextY = settings.IonizerY;
            foreach (var info in  settings.AxisGrabInfoList)
                info.NextX = 0;

            await AxisGrabService.IonizerMoveNextPos();

            foreach (var info in settings.AxisGrabInfoList)
            {
                _process.ProcessUnitDictionary[info].Processing = true;

                AxisGrabService.NextY = settings.CenterY - (settings.CalibrationScanLength / 2);

                info.NextX = info.CenterX;
                if (await AxisGrabService.MoveNextPos() == false)
                {
                    _process.ProcessUnitDictionary[info].Fail = true;
                    return false;
                }
                
                AxisGrabService.NextY = settings.CenterY + (settings.CalibrationScanLength / 2);

                if (await AxisGrabService.GrabNextPos(info) == false)
                {
                    _process.ProcessUnitDictionary[info].Fail = true;
                    return false;
                }
                
                info.NextX = info.ScanDirection == ScanDirection.LeftToRight ? info.MinX : info.MaxX;

                if (Calibration(info) == false)
                {
                    _process.ProcessUnitDictionary[info].Fail = true;
                    _process.Error = TranslationHelper.Instance.Translate("Calibration_Error");
                    return false;
                }

                _process.ProcessUnitDictionary[info].Success = true;
            }

            return true;
        }
        
        private static void AxisImageGrabbed(object sender, AxisImage axisImage)
        {
            var info = sender as AxisGrabInfo;

            var ratio = axisImage.Data.Count(d => d > info.EmptyValue) / (double)axisImage.Data.Count();

            if (ratio < AxisGrabService.Settings.CalibrationRatio)
                return;
            
            var width = info.ImageWidth;
            var height = info.ImageHeight;

            var profile = new double[width];
            for (int y = 0, index = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    profile[x] += axisImage.Data[index++];
            }

            for (int x = 0; x < width; x++)
                profile[x] /= height;

            _profileQueueDictionary[info].Enqueue(profile);
        }

        private static bool Calibration(AxisGrabInfo info)
        {
            var queue = _profileQueueDictionary[info];

            if (queue.IsEmpty)
                return false;

            var width = info.ImageWidth;
            var calibrationData = new double[width];

            var count = queue.Count;
            while (queue.IsEmpty == false)
            {
                if (_token.IsCancellationRequested)
                    return false;

                queue.TryDequeue(out double[] profile);
                for (int x = 0; x < width; x++)
                    calibrationData[x] += profile[x];
            }

            for (int x = 0; x < width; x++)
                calibrationData[x] /= count;

            for (int x = 0; x < width; x++)
                calibrationData[x] = Model_COSMO.CalibrationValue / calibrationData[x];

            CalibrationValueDictionary[info] = calibrationData.Average();

            _model.SetCalibrationData(info, Array.ConvertAll<double, float > (calibrationData, data => (float)data));

            return true;
        }
    }
}
