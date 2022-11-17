using Standard.DynMvp.Devices;
using Standard.DynMvp.Base.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Standard.DynMvp.Devices.ImageDevices;

namespace Standard.DynMvp.Devices.Helpers
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class DeviceDataAttribute : Attribute
    {
        public static ObservableCollection<DeviceInfo> GetProperties(object obj)
        {
            var deviceInfoList = new ObservableCollection<DeviceInfo>();
            var propertieInfos = obj.GetType().GetProperties();

            foreach (var propertyInfo in propertieInfos)
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(DeviceDataAttribute), true);
                if (attributes.Length > 0)
                {
                    var value = propertyInfo.GetValue(obj);

                    if (value == null)
                        continue;

                    if (value is IEnumerable<DeviceInfo>)
                    {
                        var list = value as IEnumerable<DeviceInfo>;
                        foreach (var device in list)
                            deviceInfoList.Add(device as DeviceInfo);
                    }
                    else
                    {
                        deviceInfoList.Add(value as DeviceInfo);
                    }
                }
            }

            return deviceInfoList;
        }

        public static void SetProperties(object obj, IEnumerable<DeviceInfo> deviceInfos)
        {
            var propertieInfos = obj.GetType().GetProperties();

            foreach (var propertyInfo in propertieInfos)
            {
                var attributes = propertyInfo.GetCustomAttributes(typeof(DeviceDataAttribute), true);
                if (attributes.Length > 0)
                {
                    var value = propertyInfo.GetValue(obj);
                    propertyInfo.SetValue(obj, deviceInfos.ToList());
                }
            }
        }
    }
}