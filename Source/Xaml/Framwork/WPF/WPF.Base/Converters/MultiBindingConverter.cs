using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPF.Base.Helpers;

namespace WPF.Base.Converters
{
    public class MultiBindingConverter : IMultiValueConverter
    {
        List<object> _value = new List<object>();
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            _value.AddRange(values);
            return new RelayCommand<object>(GetCompoundExecute, GetCompoundCanExecute);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        private void GetCompoundExecute(object parameter)
        {
            foreach (RelayCommand command in _value)
            {
                if (command != default(RelayCommand))
                    command.Execute(parameter);
            }
        }

        private bool GetCompoundCanExecute(object parameter)
        {
            bool res = true;
            foreach (RelayCommand command in _value)
            {
                if (command != default(RelayCommand))
                    res &= command.CanExecute(parameter);
            }
            return res;
        }
    }
}
