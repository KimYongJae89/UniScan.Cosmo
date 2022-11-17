using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using UniEye.Base;
using WPF.Base.Services;
using WPF.COSMO.Offline.Models;

namespace WPF.COSMO.Offline.Converters
{
    public class IoPortConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ioPort = value as DynMvp.Devices.Dio.IoPort;

            if (ioPort != null)
                return ioPort.Name;
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = value as string;

            var outPortName = (PortMap.OutPortName)Enum.Parse(typeof(PortMap.OutPortName), name);

            return SystemManager.Instance().DeviceBox.PortMap.GetOutPort(outPortName);
        }
    }
}
