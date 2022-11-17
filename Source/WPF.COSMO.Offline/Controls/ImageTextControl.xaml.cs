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

namespace WPF.COSMO.Offline.Controls
{
    /// <summary>
    /// ImageTextControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageTextControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ImageTextControl));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty TextStyleProperty =
            DependencyProperty.Register("TextStyle", typeof(Style), typeof(ImageTextControl));

        public Style TextStyle
        {
            get => (Style)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public ImageTextControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private double controlWidth = 128;
        public double ControlWidth
        {
            get => controlWidth;
            set
            {
                if (Equals(controlWidth, value))
                    return;

                controlWidth = value;
                OnPropertyChanged("ControlWidth");

                TextWidth = controlWidth * 0.546875;
            }
        }

        private double controlHeight = 128;
        public double ControlHeight
        {
            get => controlHeight;
            set
            {
                if (Equals(controlHeight, value))
                    return;

                controlHeight = value;
                OnPropertyChanged("ControlHeight");

                TextHeight = controlHeight * 0.546875;
                TextMargin = new Thickness(0, 0, 0, (controlHeight / 2) * 0.46875);
            }
        }

        private double textWidth = 70;
        public double TextWidth
        {
            get => textWidth;
            set
            {
                if (Equals(textWidth, value))
                    return;

                textWidth = value;
                OnPropertyChanged("TextWidth");
            }
        }

        private double textHeight = 80;
        public double TextHeight
        {
            get => textHeight;
            set
            {
                if (Equals(textHeight, value))
                    return;

                textHeight = value;
                OnPropertyChanged("TextHeight");
            }
        }

        private Thickness textMargin;
        public Thickness TextMargin
        {
            get => textMargin;
            set
            {
                if (Equals(textMargin, value))
                    return;

                textMargin = value;
                OnPropertyChanged("TextMargin");
            }
        }
    }
}
