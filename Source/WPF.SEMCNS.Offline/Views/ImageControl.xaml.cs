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
using WPF.Base.Services;
using WPF.SEMCNS.Offline.ViewModels;

namespace WPF.SEMCNS.Offline.Views
{
    /// <summary>
    /// ImagePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageControl : UserControl
    {
        public ImageViewModel ViewModel = new ImageViewModel();

        double _verticalOffset;
        double _horizontalOffset;

        bool _isGrabControl;
        public bool IsGrabControl
        {
            get { return _isGrabControl; }
            set { _isGrabControl = value; }
        }

        public ImageControl()
        {
            InitializeComponent();
        }

        public void Intialize()
        {
            ViewModel.Initialize(new ZoomService(MainCanvas), IsGrabControl);
            DataContext = ViewModel;
        }

        private void Thumb_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point pt = e.GetPosition(MainCanvas);
            ViewModel.ZoomService.ExecuteZoom(pt.X, pt.Y, e.Delta > 0 ? 1.1 : 0.9);
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            ViewModel.ZoomService.TranslateX = _horizontalOffset + (e.HorizontalChange / ViewModel.ZoomService.Scale);
            ViewModel.ZoomService.TranslateY = _verticalOffset + (e.VerticalChange / ViewModel.ZoomService.Scale);
        }

        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            _verticalOffset = ViewModel.ZoomService.TranslateY;
            _horizontalOffset = ViewModel.ZoomService.TranslateX;
        }

        private void Thumb_MouseMove(object sender, MouseEventArgs e)
        {
            if (ViewModel.GrabbedImage == null)
                return;

            var position = e.GetPosition(ImageCanvas);
            if (position.X < 0 || position.X >= ViewModel.GrabbedImage.Width
                || position.Y < 0 || position.Y >= ViewModel.GrabbedImage.Height)
                return;

            var bitmapSource = ViewModel.GrabbedImage as BitmapSource;
            
            var bytes = new byte[1];
            var rect = new Int32Rect((int)position.X, (int)position.Y, 1, 1);
            
            bitmapSource.CopyPixels(rect, bytes, 1, 0);

            ViewModel.X = position.X;
            ViewModel.Y = position.Y;
            ViewModel.Value = bytes[0];
        }
    }
}
