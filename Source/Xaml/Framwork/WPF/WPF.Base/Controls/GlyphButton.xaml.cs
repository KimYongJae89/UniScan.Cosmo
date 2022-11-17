using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.Base.Controls
{
    /// <summary>
    /// ImageButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class GlyphButton : Button
    {
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
          "Text", typeof(string), typeof(GlyphButton), new PropertyMetadata(""));

        public string Glyph
        {
            get { return (string)this.GetValue(GlyphProperty); }
            set { this.SetValue(GlyphProperty, value); }
        }

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
          "Glyph", typeof(string), typeof(GlyphButton), new PropertyMetadata(""));

        public GlyphButton()
        {
            InitializeComponent();
        }
    }
}
