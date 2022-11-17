using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Standard.DynMvp.Base.Helpers
{
    public enum SettingDataType
    {
        String, Numeric, Boolean, Enum, Table
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SettingDataAttribute : Attribute
    {
        SettingDataType _type;
        public SettingDataType Type { get => _type; }

        public SettingDataAttribute(SettingDataType type)
        {
            _type = type;
        }

        public static ObservableCollection<SettingData> GetProperties(object obj)
        {
            var settingDataList = new ObservableCollection<SettingData>();

            if (obj != null)
            {
                var propertieInfos = obj.GetType().GetProperties();

                foreach (var propertyInfo in propertieInfos)
                {
                    var attributes = propertyInfo.GetCustomAttributes(typeof(SettingDataAttribute), true);
                    if (attributes.Length > 0)
                        settingDataList.Add(new SettingData(((SettingDataAttribute)attributes[0]).Type, propertyInfo, ref obj));
                }
            }

            return settingDataList;
        }
    }

    public class SettingData
    {
        SettingDataType _type;
        object _obj;
        PropertyInfo _info;

        public SettingDataType Type { get => _type; }
        public PropertyInfo Info { get => _info; }
        public object Name { get => _info.Name; }

        [JsonIgnore]
        public object Value
        {
            get
            {
                switch (_type)
                {
                    case SettingDataType.String:
                    case SettingDataType.Numeric:
                    case SettingDataType.Boolean:
                        return _info.GetValue(_obj);
                    case SettingDataType.Enum:
                        return _info.GetValue(_obj).ToString();
                    case SettingDataType.Table:
                        return SettingDataAttribute.GetProperties(_info.GetValue(_obj));
                }

                return null;
            }
            set
            {
                switch (_type)
                {
                    case SettingDataType.String:
                    case SettingDataType.Boolean:
                        _info.SetValue(_obj, value);
                        break;
                    case SettingDataType.Numeric:
                        _info.SetValue(_obj, Convert.ChangeType(value.ToString(), _info.PropertyType));
                        break;
                    case SettingDataType.Enum:
                        var enumValue = Enum.Parse(_info.GetValue(_obj).GetType(), value as string);
                        _info.SetValue(_obj, enumValue);
                        break;
                    case SettingDataType.Table:
                        break;
                }
            }
        }

        [JsonIgnore]
        public string[] ItemSource { get => _info.PropertyType.GetEnumNames(); }

        public SettingData(SettingDataType type, PropertyInfo info, ref object obj)
        {
            _type = type;
            _info = info;
            _obj = obj;
        }
    }
}
