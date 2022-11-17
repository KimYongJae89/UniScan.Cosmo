using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPF.Base.Helpers;
using WPF.Base.Services;

namespace WPF.Base.Controls
{
    /// <summary>
    /// NavigationButton.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NavigationMenuItem : HamburgerMenuItem, IInitializable
    {
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
          "Text", typeof(string), typeof(NavigationMenuItem), new PropertyMetadata(""));
        
        public string Glyph
        {
            get { return (string)this.GetValue(GlyphProperty); }
            set { this.SetValue(GlyphProperty, value); }
        }
        
        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
          "Glyph", typeof(string), typeof(NavigationMenuItem), new PropertyMetadata(""));

        public bool IsDeveloperMenu { get; }

        public NavigationMenuItem()
        {
            InitializeComponent();
        }

        public NavigationMenuItem(string text, string glyph, object tag, bool isDeveloperMenu = false)
        {
            InitializeComponent();

            Text = text;
            Glyph = glyph;
            Tag = tag;
            IsDeveloperMenu = isDeveloperMenu;
        }

        public void Initialize()
        {
            if (Tag == null)
                return;

            (Tag as IInitializable)?.Initialize();
        }
    }
}
