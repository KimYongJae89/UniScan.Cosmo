using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// ImagePanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public delegate void NotifyDelegate(ImagePanel sender, int index);
    public delegate void OnZoomChangedDelegate(ScaleTransform scaleTransform);
    public delegate void OnTranslateChangedDelegate(TranslateTransform translateTransform);

    public partial class ImagePanel : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public NotifyDelegate Notify;
        public OnZoomChangedDelegate OnZoomChanged;
        public OnTranslateChangedDelegate OnTranslateChanged;

        TranslateTransform originTranslateTransform = new TranslateTransform();

        int maxLoadItem;
        int curLoadItem;
        int minLoadItem;
        
        bool translatable = true;
        bool zoomable = true;
        bool onUpdate = false;

        public ExtractOperatorResult[] ExtractOperatorResultArray { get => this.extractOperatorResultArray; }
        ExtractOperatorResult[] extractOperatorResultArray = null;

        public ScaleTransform ScaleTransform
        {
            get => this.scaleTransform;
            set
            {
                this.scaleTransform.ScaleX = value.ScaleX;
                this.scaleTransform.ScaleY = value.ScaleY;
                UpdateZoom();
            }
        }

        public TranslateTransform TranslateTransform
        {
            get => this.translateTransform;
            set
            {
                translateTransform.X = value.X;
                translateTransform.Y = value.Y;
            }
        }

        public bool IsCanPrev { get => curLoadItem < 0 ? false : (curLoadItem > minLoadItem); }
        public bool IsCanNext { get => curLoadItem < 0 ? false : (curLoadItem < maxLoadItem); }
        public bool FigureVisible { get => FigureLayoutCanvas.Visibility == Visibility.Visible; set => FigureLayoutCanvas.Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        public float ResizeRatio { get => 1 / UniScanWPF.Table.Operation.Operator.ResizeRatio; }
        public Visibility Zoomable { get => zoomable ? Visibility.Visible : Visibility.Collapsed; }

        public int CurLoadItem
        {
            get => curLoadItem;
            set
            {
                if (curLoadItem != value)
                {
                    curLoadItem = value;
                    OnPropertyChanged("CurLoadItem");
                    OnPropertyChanged("IsCanPrev");
                    OnPropertyChanged("IsCanNext");
                }
            }
        }

        public ImagePanel(bool translatable, bool zoomable)
        {
            InitializeComponent();

            this.translatable = translatable;
            this.zoomable = zoomable;
            this.DataContext = this;
            BuildImageCanvas();
        }

        private void BuildImageCanvas()
        {
            this.extractOperatorResultArray = new ExtractOperatorResult[DeveloperSettings.Instance.ScanNum];
            for (int i = 0; i < DeveloperSettings.Instance.ScanNum; i++)
            {
                this.extractOperatorResultArray[i] = new ExtractOperatorResult(null, new ScanOperatorResult(null, ""), "");

                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.SetBinding(System.Windows.Controls.Image.SourceProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ExtractOperatorResultArray[{0}].ScanOperatorResult.TopLightBitmap", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                image.SetBinding(Canvas.LeftProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ExtractOperatorResultArray[{0}].ScanOperatorResult.CanvasAxisPosition.Position[0]", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                image.SetBinding(Canvas.TopProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ExtractOperatorResultArray[{0}].ScanOperatorResult.CanvasAxisPosition.Position[1]", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                image.DataContext = this;
                //image.SetValue(Canvas.HeightProperty, "10000");

                imageCanvas.Children.Add(image);
            }

            RobotWorkingRectangle.DataContext = InfoBox.Instance;            
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (translatable == false)
                return;

            translateTransform.X = this.originTranslateTransform.X + (e.HorizontalChange / scaleTransform.ScaleX);
            translateTransform.Y = this.originTranslateTransform.Y + (e.VerticalChange / scaleTransform.ScaleY);

            this.OnTranslateChanged?.Invoke(translateTransform);
        }

        private void Thumb_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (zoomable)
            {
                System.Windows.Point prevPt = e.GetPosition(this.mainCanvas);

                scaleTransform.ScaleX *= e.Delta > 0 ? 1.2 : 0.8;
                scaleTransform.ScaleY *= e.Delta > 0 ? 1.2 : 0.8;
                this.OnZoomChanged?.Invoke(scaleTransform);

                System.Windows.Point nextPt = e.GetPosition(this.mainCanvas);

                translateTransform.X += nextPt.X - prevPt.X;
                translateTransform.Y += nextPt.Y - prevPt.Y;
                this.OnTranslateChanged?.Invoke(translateTransform);

                UpdateZoom();
            }
        }

        internal void SetSelection(CanvasDefect selectedDefect)
        {
            if (selectedDefect == null)
                return;

            Rect rect = selectedDefect.GetRect();
            System.Drawing.PointF centerPt = new System.Drawing.PointF((float)(rect.Left + rect.Right) / 2.0f, (float)(rect.Top + rect.Bottom) / 2.0f);
            Point pt = ConvertToScreen(centerPt);
            if (translatable)
            {
                translateTransform.X = pt.X;
                translateTransform.Y = pt.Y;
                this.OnTranslateChanged?.Invoke(translateTransform);
            }

            Rect selectionRect = selectedDefect.GetRect(500);
            Polygon polygon = new Polygon()
            {
                Stroke = new SolidColorBrush(Colors.LightGreen),
                StrokeThickness = 3 / Math.Max(scaleTransform.ScaleX, scaleTransform.ScaleY),
                Opacity = 1,
                Points = new PointCollection(new Point[] { selectionRect.TopLeft, selectionRect.TopRight, selectionRect.BottomRight, selectionRect.BottomLeft })
            };
            this.SelectionLayoutCanvas.Children.Add(polygon);
        }

        internal void VisiblePatternCanvas(bool visible)
        {
            this.PatternLayoutCanvas.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        internal void VisibleMarginCanvas(bool visible)
        {
            this.MarginLayoutCanvas.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }

        internal void VisibleShapeCanvas(bool visible)
        {
            this.ShapeLayoutCanvas.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
        }
        private Point ConvertToScreen(System.Drawing.PointF centerPt)
        {
            Point convert = new Point();
            //convert.X = -centerPt.X * DeveloperSettings.Instance.Resolution + (MainCanvas.ActualWidth / 2 / scaleX) + (30700);
            //convert.Y = -centerPt.Y * DeveloperSettings.Instance.Resolution + (MainCanvas.ActualHeight / 2 / scaleY);

            convert.X = -centerPt.X + (InfoBox.Instance.DispScanRegion.Width / 2) / (scaleTransform.ScaleX / (this.mainCanvas.ActualWidth / InfoBox.Instance.DispScanRegion.Width));
            convert.Y = -centerPt.Y + (InfoBox.Instance.DispScanRegion.Height / 2) / (scaleTransform.ScaleY / (this.mainCanvas.ActualHeight / InfoBox.Instance.DispScanRegion.Height));

            return convert;
        }

        internal void ClearSelection()
        {
            this.SelectionLayoutCanvas.Children.Clear();
        }

        private void MainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //ZoomFit();
        }

        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            this.originTranslateTransform = translateTransform.Clone();
        }

        private void ZoomIn_Button_Click(object sender, RoutedEventArgs e)
        {
            if (zoomable)
            {
                scaleTransform.ScaleX *= 1.2;
                scaleTransform.ScaleY *= 1.2;
                this.OnZoomChanged?.Invoke(scaleTransform);

                UpdateZoom();
            }
        }

        private void ZoomOut_Button_Click(object sender, RoutedEventArgs e)
        {
            if (zoomable)
            {
                scaleTransform.ScaleX *= 0.8;
                scaleTransform.ScaleY *= 0.8;
                this.OnZoomChanged?.Invoke(scaleTransform);

                UpdateZoom();
            }
        }

        private void ZoomFit_Button_Click(object sender, RoutedEventArgs e)
        {
            if (zoomable)
                ZoomFit();
        }

        public void ZoomFit()
        {
            translateTransform.X = InfoBox.Instance.DispScanRegion.Width * 0.05;
            translateTransform.Y = InfoBox.Instance.DispScanRegion.Height * 0.05;
            this.OnTranslateChanged?.Invoke(translateTransform);

            scaleTransform.ScaleX = Math.Max(1, this.mainCanvas.ActualWidth) / (InfoBox.Instance.DispScanRegion.Width * 1.1);
            scaleTransform.ScaleY = Math.Max(1, this.mainCanvas.ActualHeight) / (InfoBox.Instance.DispScanRegion.Height * 1.1);
            this.OnZoomChanged?.Invoke(scaleTransform);

            UpdateZoom();
        }

        private void UpdateZoom()
        {
            if (Math.Min(scaleTransform.ScaleX, scaleTransform.ScaleY) > 0.2)
                RenderOptions.SetBitmapScalingMode(imageCanvas, BitmapScalingMode.HighQuality);
            else
                RenderOptions.SetBitmapScalingMode(imageCanvas, BitmapScalingMode.Unspecified);

            AdaptiveFigureScale();
        }

        private void AdaptiveFigureScale()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (scaleTransform.ScaleX != 0 && scaleTransform.ScaleY != 0)
                {
                    double thickness = 5 / Math.Max(scaleTransform.ScaleX, scaleTransform.ScaleY);
                    this.RobotWorkingRectangle.StrokeThickness = thickness;

                    foreach (Shape shape in this.PatternLayoutCanvas.Children)
                        shape.StrokeThickness = thickness / 2;

                    foreach (Shape shape in this.MarginLayoutCanvas.Children)
                        shape.StrokeThickness = thickness / 2;

                    foreach (Shape shape in this.ShapeLayoutCanvas.Children)
                        shape.StrokeThickness = thickness / 2;

                    foreach (Shape shape in this.MeanderLayoutCanvas.Children)
                        shape.StrokeThickness = thickness;

                    foreach (Shape shape in this.SelectionLayoutCanvas.Children)
                        shape.StrokeThickness = thickness / 2;
                }
            }));
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentIndex(curLoadItem - 1);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentIndex(curLoadItem + 1);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            if (onUpdate == false)
                Notify?.Invoke(this, (int)e.AddedItems[0]);
        }

        private void SetCurrentIndex(int index)
        {
            int newIndex = Math.Min(this.maxLoadItem, Math.Max(this.minLoadItem, index));
            this.CurLoadItem = index;
        }

        private void CheckResultIndex()
        {
            OnPropertyChanged("IsCanNext");
            OnPropertyChanged("IsCanPrev ");
        }

        internal void ProductionChange(LoadItem loadItem)
        {
            this.PatternLayoutCanvas.Children.Clear();
            this.MarginLayoutCanvas.Children.Clear();
            this.ShapeLayoutCanvas.Children.Clear();
            this.MeanderLayoutCanvas.Children.Clear();
            this.SelectionLayoutCanvas.Children.Clear();

           
            int count = Math.Min(extractOperatorResultArray.Length, loadItem.ExtractOperatorResultList.Count);
            for (int i = 0; i < count; i++)
                this.extractOperatorResultArray[i] = loadItem.ExtractOperatorResultList[i];

            foreach (CanvasDefect canvasDefect in loadItem.CanvasDefectList)
            {
                Shape shape = canvasDefect.GetShape();
                switch (canvasDefect.Defect.ResultObjectType)
                {
                    case DefectType.Pattern:
                        this.PatternLayoutCanvas.Children.Add(shape);
                        break;
                    case DefectType.Margin:
                        this.MarginLayoutCanvas.Children.Add(shape);
                        break;
                    case DefectType.Shape:
                        this.ShapeLayoutCanvas.Children.Add(shape);
                        break;

                    case MeasureType.Length:
                        if (((LengthMeasure)canvasDefect.Defect).IsValid)
                            this.MeanderLayoutCanvas.Children.Add(shape);
                        break;
                    case MeasureType.Meander:
                        this.MeanderLayoutCanvas.Children.Add(shape);
                        break;
                    case MeasureType.Extra:
                        this.MeanderLayoutCanvas.Children.Add(shape);
                        break;
                }
            }
            AdaptiveFigureScale();
            
            OnPropertyChanged("ScanOperatorResultArray");
            OnPropertyChanged("DefectList");
            OnPropertyChanged("");
        }

        internal void UpdateCombobox(Production production)
        {
            onUpdate = true;

            this.CurLoadItem = -1;

            List<int> list = new List<int>();
            for (int i = 0; i < production.Count; i++)
                list.Add(i+1);
            
            comboBox.ItemsSource = list;

            if (list.Count > 0)
            {
                this.minLoadItem = list.Min();
                this.maxLoadItem = list.Max();
                this.CurLoadItem = minLoadItem;
            }

            onUpdate = false;
        }
    }
}
