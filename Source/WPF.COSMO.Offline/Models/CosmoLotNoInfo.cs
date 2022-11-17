using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Models
{
    public class LotNoDeviceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            KeyValuePair<string, string> device = (KeyValuePair<string, string>)value;
            return device.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public enum CosmoLotNoType
    {
        Unknown = 0,
        LotNo,
        Date,
        CoatingDevice,
        CoatingNo,
        SlitterDevice,
        SlitterCut,
        SlitterLane,
    }

    public class CosmoLotNoInfo : Observable
    {
        #region 변수

        string _modelName;
        public string ModelName
        {
            get => _modelName;
            set => Set(ref _modelName, value);
        }

        string _lotNo;
        public string LotNo
        {
            get => _lotNo;
            set => Set(ref _lotNo, value);
        }

        DateTime? _productDate;
        public DateTime? ProductDate
        {
            get => _productDate;
            set => Set(ref _productDate, value);
        }

        KeyValuePair<string, string>? _coatingDevice = null;
        public KeyValuePair<string, string>? CoatingDevice
        {
            get => _coatingDevice;
            set => Set(ref _coatingDevice, value);
        }

        int? _coatingNo = null;
        public int? CoatingNo
        {
            get => _coatingNo;
            set => Set(ref _coatingNo, value);
        }

        KeyValuePair<string, string>? _slitterDevice = null;
        public KeyValuePair<string, string>? SlitterDevice
        {
            get => _slitterDevice;
            set => Set(ref _slitterDevice, value);
        }

        int? _slitterNo = null;
        public int? SlitterNo
        {
            get => _slitterNo;
            set => Set(ref _slitterNo, value);
        }

        int? _slitterLane = null;
        public int? SlitterLane
        {
            get => _slitterLane;
            set => Set(ref _slitterLane, value);
        }

        public DateTime InspectStartTime { get; set; }
        public DateTime InspectEndTime { get; set; }

        #endregion

        #region 메소드

        public static int GetMonthFromLotName(string _month)
        {
            switch (_month)
            {
                case "A": return 1;
                case "B": return 2;
                case "C": return 3;
                case "D": return 4;
                case "E": return 5;
                case "F": return 6;
                case "G": return 7;
                case "H": return 8;
                case "J": return 9;
                case "K": return 10;
                case "L": return 11;
                case "M": return 12;
                default:
                    break;
            }

            return 0; // Exception
        }

        #endregion

        public CosmoLotNoInfo()
        {

        }
        
        private CosmoLotNoInfo(string lotNo)
        {
            string cvtStr = lotNo.Trim();
            cvtStr = lotNo.Replace(" ", "");
            LotNo = cvtStr;

            int year = Convert.ToInt32(cvtStr.Substring(0, 2)) + 2000;
            int month = GetMonthFromLotName(cvtStr.Substring(2, 1));
            int day = Convert.ToInt32(cvtStr.Substring(3, 2));
            ProductDate = new DateTime(year, month, day);

            CoatingDevice = LotNoService.Collections.CoatingDeviceList.First(x => x.Key == cvtStr.Substring(5, 1));

            CoatingNo = Convert.ToInt16(cvtStr.Substring(6, 2));
            SlitterDevice = LotNoService.Collections.SlitterDeviceList.First(x => x.Key == cvtStr.Substring(8, 1));
            SlitterNo = Convert.ToInt16(cvtStr.Substring(9, 1));
            SlitterLane = Convert.ToInt16(cvtStr.Substring(10, 1));

            InspectStartTime = DateTime.Now;
        }

        public static CosmoLotNoInfo Parse(string lotNo)
        {
            CosmoLotNoInfo info = null;
            try
            {
                info = new CosmoLotNoInfo(lotNo);
            }
            catch
            {

            }

            return info;
        }
    }
}
