using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using WPF.Base.Converters;
using WPF.Base.Helpers;

namespace WPF.Base.Extensions
{
    public class TranslationExtension : MarkupExtension
    {
        object _obj;

        public TranslationExtension(object obj)
        {
            _obj = obj;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var multiBinding = new MultiBinding();
            multiBinding.Converter = new TranslationConverter();
            multiBinding.Bindings.Add(new Binding("CurrentCultureInfo") { Source = TranslationHelper.Instance });

            if (_obj is string)
                multiBinding.Bindings.Add(new Binding() { Source = _obj });
            else if (_obj is Binding)
                multiBinding.Bindings.Add(_obj as Binding);

            return multiBinding.ProvideValue(serviceProvider);
        }
    }

    public class BitmapExtention : MarkupExtension
    {
        Bitmap _bitmap;

        public BitmapExtention(Bitmap bitmap)
        {
            _bitmap = bitmap;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding()
            {
                Source = _bitmap,
                Converter = new BitmapToImageSourceConverter()
            };

            return binding.ProvideValue(serviceProvider);
        }
    }
}
