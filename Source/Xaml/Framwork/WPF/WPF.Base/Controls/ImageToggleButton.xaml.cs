﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    public partial class ImageToggleButton : ToggleButton, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region DependencyProperty

        private static DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(ToggleButton));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ToggleButton));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly DependencyProperty ImageBackgroundProperty =
            DependencyProperty.Register("ImageBackground", typeof(Brush), typeof(ToggleButton));

        public Brush ImageBackground
        {
            get => (Brush)GetValue(ImageBackgroundProperty);
            set => SetValue(ImageBackgroundProperty, value);
        }

        #endregion

        public ImageToggleButton()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
