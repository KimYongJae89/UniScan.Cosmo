using Standard.DynMvp.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.Base.Settings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWP.Base.Selectors
{
    public class SettingDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StringDataTemplate { get; set; }
        public DataTemplate BooleanDataTemplate { get; set; }
        public DataTemplate NumericDataTemplate { get; set; }
        public DataTemplate EnumDataTemplate { get; set; }
        public DataTemplate TableDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var data = item as SettingData;
            switch (data.Type)
            {
                case SettingDataType.String:
                    return StringDataTemplate;
                case SettingDataType.Numeric:
                    return NumericDataTemplate;
                case SettingDataType.Boolean:
                    return BooleanDataTemplate;
                case SettingDataType.Enum:
                    return EnumDataTemplate;
                case SettingDataType.Table:
                    return TableDataTemplate;
            }

            return null;
        }
    }
}
