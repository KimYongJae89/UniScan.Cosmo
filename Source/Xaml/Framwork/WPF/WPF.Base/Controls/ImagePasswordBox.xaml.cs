using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.Base.Controls
{
    /// <summary>
    /// ImagePasswordBox.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImagePasswordBox : TextBox, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region DependencyProperty

        private static DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ImagePasswordBox));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImagePasswordBox));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register("PasswordChar", typeof(char), typeof(ImagePasswordBox));

        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        #endregion

        #region

        private string password;
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        #endregion

        public ImagePasswordBox()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
