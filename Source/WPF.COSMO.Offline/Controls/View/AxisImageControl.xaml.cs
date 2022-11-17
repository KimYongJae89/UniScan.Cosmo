using System;
using System.Collections.Generic;
using System.Globalization;
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
using WPF.Base.Services;
using WPF.Base.ViewModels;
using WPF.COSMO.Offline.Controls;
using WPF.COSMO.Offline.Controls.ViewModel;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;
using WPF.COSMO.Offline.ViewModels;

namespace WPF.COSMO.Offline.Controls.Views
{
    public class TopOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness(0, -System.Convert.ToDouble(value), 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// ImagePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AxisImageControl : UserControl
    {
        public AxisImageViewModel ViewModel = new AxisImageViewModel();

        private double _verticalOffset;
        private double _horizontalOffset;
        
        public AxisImageControl()
        {
            InitializeComponent();
        }

        public void Initialize(DefectStorage defectStorage)
        {
            ViewModel.Initialize(MainCanvas);

            DataContext = ViewModel;
        }

        private void Thumb_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point pt = e.GetPosition(MainCanvas);
            ViewModel.ZoomService.ExecuteZoom(pt.X, pt.Y, e.Delta > 0 ? 1.1 : 0.9);
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (ViewModel.ZoomService != null)
            {
                ViewModel.ZoomService.TranslateX = _horizontalOffset + (e.HorizontalChange / ViewModel.ZoomService.Scale);
                ViewModel.ZoomService.TranslateY = _verticalOffset + (e.VerticalChange / ViewModel.ZoomService.Scale);
            }
        }

        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            if (ViewModel.ZoomService != null)
            {
                _verticalOffset = ViewModel.ZoomService.TranslateY;
                _horizontalOffset = ViewModel.ZoomService.TranslateX;
            }
        }

        private void Thumb_MouseMove(object sender, MouseEventArgs e)
        {
            //if (ViewModel.GrabbedImage == null)
            //    return;

            //var position = e.GetPosition(ImageCanvas);
            //if (position.X < 0 || position.X >= ViewModel.GrabbedImage.Width
            //    || position.Y < 0 || position.Y >= ViewModel.GrabbedImage.Height)
            //    return;

            //var bitmapSource = ViewModel.GrabbedImage as BitmapSource;

            //var bytes = new byte[1];
            //var rect = new Int32Rect((int)position.X, (int)position.Y, 1, 1);

            //bitmapSource.CopyPixels(rect, bytes, 1, 0);

            //ViewModel.X = position.X;
            //ViewModel.Y = position.Y;
            //ViewModel.Value = bytes[0];
        }

        private void ResultUpdateControl_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.ZoomService.FitToSize(ViewModel.AxisWidth + ViewModel.Inflate, ViewModel.AxisHeight + ViewModel.Inflate);
        }
    }
}
