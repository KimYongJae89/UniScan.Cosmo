using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// WPFCanvasPanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectPage : UserControl, INotifyPropertyChanged, IMultiLanguageSupport
    {
        double originTranslateX;
        double originTranslateY;

        bool measureMarginMode = false;
        double originMeasureMarginX;
        double originMeasureMarginY;

        BitmapSource bigDefectBitmapSource = null;
        public BitmapSource BigDefectBitmapSource { get => bigDefectBitmapSource;
            set
            {
                if (this.bigDefectBitmapSource != value)
                {
                    this.bigDefectBitmapSource = value;
                    OnPropertyChanged("BigDefectBitmapSource");
                }
            }
        }

        double scaleX = 1;
        public double ScaleX
        {
            get => scaleX;
            set
            {
                if (scaleX != value)
                {
                    scaleX = value;
                    if (scaleX > 0 && scaleY > 0)
                    {
                        OnPropertyChanged("ScaleX");
                        OnPropertyChanged("MarkSize");
                        OnPropertyChanged("HomeMarkPos");
                        OnPropertyChanged("MarkFontSize");
                    }
                }
            }
        }

        double scaleY = 1;
        public double ScaleY
        {
            get => scaleY;
            set
            {
                if (scaleY != value)
                {
                    if (scaleX> 0 && scaleY > 0)
                    {
                        scaleY = value;
                        OnPropertyChanged("ScaleY");
                        OnPropertyChanged("MarkSize");
                        OnPropertyChanged("HomeMarkPos");
                        OnPropertyChanged("MarkFontSize");
                    }
                }
            }
        }

        double translateX;
        public double TranslateX
        {
            get => translateX;
            set
            {
                if (translateX != value)
                {
                    translateX = value;
                    OnPropertyChanged("TranslateX");
                }
            }
        }

        double translateY;
        public double TranslateY
        {
            get => translateY;
            set
            {
                if (translateY != value)
                {
                    translateY = value;
                    OnPropertyChanged("TranslateY");
                }
            }
        }

        public System.Windows.Size MarkSize
        {
            get
            {
                double scale = scaleY / scaleX;
                double x = Math.Min(3500 * scale, 100.0 / scaleX);
                double y = Math.Min(3500 / scale, 100.0 / scaleY);
                return new System.Windows.Size(x, y);
            }
        }

        public double MarkFontSize { get => Math.Min(35791, Math.Min(MarkSize.Width, MarkSize.Height) / 2); }

        public System.Drawing.PointF HomeMarkPos
        {
            get
            {
                PointF homePos = InfoBox.Instance.DispHomePos;
                System.Windows.Size markSize = MarkSize;
                return new PointF((float)(homePos.X - markSize.Width / 2), (float)(homePos.Y - markSize.Height / 2));
            }
        }

        public System.Windows.Point CurMarkPos
        {
            get => new System.Windows.Point(
                HomeMarkPos.X - (SystemManager.Instance().MachineObserver.MotionBox.ActualPositionX / DeveloperSettings.Instance.Resolution),
                HomeMarkPos.Y + (SystemManager.Instance().MachineObserver.MotionBox.ActualPositionY / DeveloperSettings.Instance.Resolution)
                );
        }

        public SolidColorBrush CurMarkBrush { get => SystemManager.Instance().MachineObserver.MotionBox.OnMoveBrush; }

        public bool FigureVisible {
            get => FigureLayoutCanvas.Visibility == Visibility.Visible;
            set
            {
                FigureLayoutCanvas.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                if(FigureLayoutCanvas.Visibility == Visibility.Visible)
                    AdaptiveFigureScale();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public InspectPage()
        {
            InitializeComponent();

            for (int i = 0; i < DeveloperSettings.Instance.ScanNum; i++)
            {
                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.SetBinding(System.Windows.Controls.Image.SourceProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ScanOperatorResultArray[{0}].TopLightBitmap", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                image.SetBinding(Canvas.LeftProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ScanOperatorResultArray[{0}].CanvasAxisPosition.Position[0]", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                image.SetBinding(Canvas.TopProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ScanOperatorResultArray[{0}].CanvasAxisPosition.Position[1]", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                image.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;
                //image.SetValue(Canvas.HeightProperty, "10000");

                ImageCanvas.Children.Add(image);
            }

            this.DataContext = this;

            //this.imageCanvas.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;
            this.defectListBox.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;

            this.bgCanvas.DataContext = InfoBox.Instance;
            //this.ScanRegionLabel.DataContext = SystemManager.Instance().OperatorManager.ScanOperator;
            //this.RobotWorkingRectangle.DataContext = SystemManager.Instance().OperatorManager.ScanOperator;

            this.ImageCanvas.DataContext = InfoBox.Instance;
            //this.LightTuneMessage.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;
            //this.LightTuneImage.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;

            this.MenuPanel.DataContext = InfoBox.Instance;
            this.InfoGrid.DataContext = InfoBox.Instance;
            this.ScanOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ScanOperator;
            this.ExtractOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ExtractOperator;
            this.InspectOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.InspectOperator;
            this.StoringOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner.StoringOperator;

            SystemManager.Instance().OperatorManager.ResultCombiner.CombineCompleted += CombineCompleted;
            SystemManager.Instance().OperatorManager.ResultCombiner.CombineClear += CombineClear;
            LocalizeHelper.AddListener(this);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Tick;
            timer.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            OperatorManager operatorManager = SystemManager.Instance().OperatorManager;
            if (SystemManager.Instance().OperatorManager.IsRun)
            {
                loopModeCheckCount = 0;
                LoadingImage.Visibility = Visibility.Visible;
                rotation.Angle += 15;
            }
            else
            {
                LoadingImage.Visibility = Visibility.Collapsed;

                if (loopMode && operatorManager.ResultCombiner.StoringOperator.OperatorState == OperatorState.Idle)
                {
                    loopModeCheckCount++;
                    if (loopModeCheckCount == 10)
                        loopMode = SystemManager.Instance().OperatorManager.Start(false);
                }
            }

            OnPropertyChanged("CurMarkPos");
            OnPropertyChanged("CurMarkBrush");         
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        private void ParamButton_Click(object sender, RoutedEventArgs e)
        {
            //PropertyWIndow propertyWIndow = new PropertyWIndow("Inspector", SystemManager.Instance().OperatorManager.InspectOperator.Settings);
            InspectParamWindow inspectParamWindow = new InspectParamWindow();
            inspectParamWindow.Show();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemManager.Instance().MachineObserver.IoBox.InOnDoor1 == false || SystemManager.Instance().MachineObserver.IoBox.InOnDoor2 == false)
            {
                CustomMessageBox.Show("Door is open !!", "Cancle", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                return;
            }

            MachineOperator.MoveHome(0, null, new CancellationTokenSource());
        }

        private void MainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Filter = Filter;

            TranslateX = -InfoBox.Instance.DispRobotRegion.X + 10000.0;
            TranslateY = -InfoBox.Instance.DispRobotRegion.Y + 10000.0;

            ScaleX = Math.Max(1, mainCanvas.ActualWidth) / ((InfoBox.Instance.DispRobotRegion.Width) + 20000.0);
            ScaleY = Math.Max(1, mainCanvas.ActualHeight) / ((InfoBox.Instance.DispRobotRegion.Height) + 20000.0);

            ZoomUpdated();
        }

        public void Clear()
        {

        }
        
        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (measureMarginMode)
            {
                System.Windows.Point origin = new System.Windows.Point(originMeasureMarginX, originMeasureMarginY);
                System.Windows.Point current = new System.Windows.Point(originMeasureMarginX + e.HorizontalChange, originMeasureMarginY + e.VerticalChange);
                
                if (Math.Abs(origin.X - current.X) > Math.Abs(origin.Y - current.Y))
                {
                    Rect visualRect = new Rect(-Math.Min(origin.X, current.X), -origin.Y, 2500, 2500);//Math.Abs(origin.X - current.X), 200);
                    //Rect visualRect = new Rect(Math.Min(originMeasureMarginX, current.X), Math.Min(originMeasureMarginY, current.Y), Math.Abs(origin.X - current.X), 1);

                    DrawingVisual visual = new DrawingVisual();
                    using (var dc = visual.RenderOpen())
                    {
                        dc.DrawRectangle(new VisualBrush(mainCanvas), null, visualRect);
                    }

                    var bitmapSource = new RenderTargetBitmap((int)Math.Abs(origin.X - current.X), 100, 96, 96, PixelFormats.Default);
                    bitmapSource.Render(visual);

                    BitmapEncoder encoder = new BmpBitmapEncoder();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                        encoder.Save(stream);
                        byte[] byteArray = stream.ToArray();
                        AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Color, (int)bitmapSource.Width, 100, ImageBandType.Luminance);

                        //byteArray.
                        algoImage.SetByte(byteArray);
                        
                        //StripeCheckerResult result =  StripeChecker.Check(algoImage, new PointF(0, 0), algoImage.Width, 100, 0);
                        //MeasureMarginLayoutCanvas.Children.Clear();

                        //foreach (var line in result.GetStripeLineList())
                        //    MeasureMarginLayoutCanvas.Children.Add(line);

                        algoImage.Dispose();
                    }
                }
                else
                {
                    //var bitmapSource = new RenderTargetBitmap(1, (int)Math.Abs(origin.Y - current.Y), 96, 96, PixelFormats.Indexed8);
                }

            }
            else
            {
                TranslateX = originTranslateX + (e.HorizontalChange / scaleX);
                TranslateY = originTranslateY + (e.VerticalChange / scaleY);
            }
        }
        
        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            if (measureMarginMode)
            {
                originMeasureMarginX = e.HorizontalOffset;
                originMeasureMarginY = e.VerticalOffset;
            }
            else
            {
                originTranslateX = translateX;
                originTranslateY = translateY;
            }
        }
        
        private void Thumb_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            System.Windows.Point prevPt = e.GetPosition(mainCanvas);
            
            ScaleX *= e.Delta > 0 ? 1.2 : 0.8;
            ScaleY *= e.Delta > 0 ? 1.2 : 0.8;

            System.Windows.Point nextPt = e.GetPosition(mainCanvas);

            TranslateX += nextPt.X - prevPt.X;
            TranslateY += nextPt.Y - prevPt.Y;

            ZoomUpdated();
        }

        private void ZoomIn_Button_Click(object sender, RoutedEventArgs e)
        {
            ScaleX *= 1.2;
            ScaleY *= 1.2;

            ZoomUpdated();
        }

        private void ZoomOut_Button_Click(object sender, RoutedEventArgs e)
        {
            ScaleX *= 0.8;
            ScaleY *= 0.8;

            ZoomUpdated();
        }

        private void ZoomUpdated()
        {
            if (scaleX > 0.2)
                RenderOptions.SetBitmapScalingMode(ImageCanvas, BitmapScalingMode.HighQuality);
            else
                RenderOptions.SetBitmapScalingMode(ImageCanvas, BitmapScalingMode.Unspecified);

            AdaptiveFigureScale();
        }

        private void ZoomFit_Button_Click(object sender, RoutedEventArgs e)
        {
            TranslateX = InfoBox.Instance.DispScanRegion.Width * 0.05 + 3500;
            TranslateY = InfoBox.Instance.DispScanRegion.Height * 0.05;

            ScaleX = Math.Max(1, mainCanvas.ActualWidth) / (InfoBox.Instance.DispScanRegion.Width * 1.1);
            ScaleY = Math.Max(1, mainCanvas.ActualHeight) / ((InfoBox.Instance.DispScanRegion.Height * 1.1));

            ZoomUpdated();
        }

        private void DefectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectionLayoutCanvas.Children.Clear();
            if (defectListBox.SelectedItem == null)
                return;

            CanvasDefect selectedDefect = (CanvasDefect)defectListBox.SelectedItem;
            this.BigDefectBitmapSource = selectedDefect.Defect.GetBitmapSource();
            
            RectangleF dispScanRegion = InfoBox.Instance.DispScanRegion;
            Rect rect = selectedDefect.GetRect();
            PointF centerPt = new PointF((float)(rect.Left + rect.Right) / 2.0f, (float)(rect.Top + rect.Bottom) / 2.0f);

            this.TranslateX = -centerPt.X + (dispScanRegion.Width / 2) / (ScaleX / (mainCanvas.ActualWidth / dispScanRegion.Width));
            this.TranslateY = -centerPt.Y + (dispScanRegion.Height / 2) / (ScaleY / (mainCanvas.ActualHeight / dispScanRegion.Height));

            Rect selectionRect = selectedDefect.GetRect(500);
            Polygon polygon = new Polygon()
            {
                Stroke = new SolidColorBrush(Colors.LightGreen),
                StrokeThickness = 3 / Math.Max(scaleY, scaleX),
                Opacity = 1,
                Points = new PointCollection(new System.Windows.Point[] { selectionRect.TopLeft, selectionRect.TopRight, selectionRect.BottomRight, selectionRect.BottomLeft })
            };
            this.SelectionLayoutCanvas.Children.Add(polygon);
        }

        public void CombineCompleted(List<CanvasDefect> canvasDefectList)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Array temp = null;
                lock (canvasDefectList)
                    temp = canvasDefectList.ToArray();
                foreach (CanvasDefect canvasDefect in temp)
                {
                    Shape shape = canvasDefect.GetShape();
                    Enum resObjType = canvasDefect.Defect.ResultObjectType;
                    switch (resObjType)
                    {
                        case MeasureType.Meander:
                        //case MeasureType.Length:
                        case MeasureType.Extra:
                            this.MeanderLayoutCanvas.Children.Add(shape);
                            break;
                        case DefectType.Pattern:
                            this.PatternLayoutCanvas.Children.Add(shape);
                            break;
                        case DefectType.Margin:
                            this.MarginLayoutCanvas.Children.Add(shape);
                            break;
                        case DefectType.Shape:
                            this.ShapeLayoutCanvas.Children.Add(shape);
                            break;
                    }
                }

                AdaptiveFigureScale();
            }));
        }

        private void AdaptiveFigureScale()
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (scaleX != 0 && scaleY != 0)
                {
                    double thickness = 5 / Math.Max(scaleY, scaleX);

                    RobotWorkingRectangle.StrokeThickness = thickness;

                    foreach (Shape shape in this.MeanderLayoutCanvas.Children)
                        shape.StrokeThickness = thickness / 3 * 2;

                    foreach (Shape shape in this.PatternLayoutCanvas.Children)
                        shape.StrokeThickness = thickness / 2;

                    foreach (Shape shape in this.MarginLayoutCanvas.Children)
                        shape.StrokeThickness = thickness / 2;

                    foreach (Shape shape in this.ShapeLayoutCanvas.Children)
                        shape.StrokeThickness = thickness / 2;
                }
            }));
        }

        private void CombineClear()
        {
            this.BigDefectBitmapSource = null;

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.PatternLayoutCanvas.Children.Clear();
                this.MarginLayoutCanvas.Children.Clear();
                this.ShapeLayoutCanvas.Children.Clear();
                this.MeanderLayoutCanvas.Children.Clear();
            }));
        }
        
        private void MeasureMargin_Checked(object sender, RoutedEventArgs e)
        {
            measureMarginMode = true;
        }

        private void MeasureMargin_Unchecked(object sender, RoutedEventArgs e)
        {
            measureMarginMode = false;
        }

        private bool Filter(object item)
        {
            Enum type = ((CanvasDefect)item).Defect.ResultObjectType;
            
            switch (type)
            {
                case DefectType.Pattern:
                    if (PatternCheckBox.IsChecked == true)
                        return true;
                    break;
                case DefectType.Margin:
                    if (MarginCheckBox.IsChecked == true)
                        return true;
                    break;
                case DefectType.Shape:
                    if (ShapeCheckBox.IsChecked == true)
                        return true;
                    break;
            }
           
            return false;
        }

        private void PatternCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.PatternLayoutCanvas != null)
            {
                this.PatternLayoutCanvas.Opacity = 0.4;
                CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Refresh();
            }
        }

        private void PatternCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.PatternLayoutCanvas.Opacity = 0.0;

            CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Refresh();
        }

        private void MarginCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.MarginLayoutCanvas != null)
            {
                this.MarginLayoutCanvas.Opacity = 0.4;

                CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Refresh();
            }
        }

        private void MarginCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.MarginLayoutCanvas.Opacity = 0.0;

            CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Refresh();
        }

        private void ShapeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.ShapeLayoutCanvas != null)
            {
                this.ShapeLayoutCanvas.Opacity = 0.4;

                CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Refresh();
            }
        }

        private void ShapeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.ShapeLayoutCanvas.Opacity = 0.0;

            CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Refresh();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            this.loopMode = false;
            SystemManager.Instance().OperatorManager.Cancle("Operation Cancled");
        }

        private void TeachButton_Click(object sender, RoutedEventArgs e)
        {

        }

        bool loopMode = false;
        int loopModeCheckCount = 0;
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            DynMvp.Data.ProductionBase production = SystemManager.Instance().ProductionManager.CurProduction;
            if (production == null)
                production= SystemManager.Instance().ProductionManager.GetLastProduction(SystemManager.Instance().CurrentModel);

            string prev = production?.LotNo;

            Tuple<MessageBoxResult, string> result = CustomInputForm.Show("Enter Lot No.", "Lot No.", MessageBoxImage.Question, prev);
            if (result.Item1 == MessageBoxResult.OK)
            {
                string lotNo = result.Item2;
                if(lotNo.Contains("LOOPTEST"))
                {
                    MessageBoxResult messageBoxResult = CustomMessageBox.Show("Is Loop Test Mode?", "Select Mode", MessageBoxButton.YesNo);
                    loopMode = messageBoxResult == MessageBoxResult.Yes;
                }
                SystemManager.Instance().ProductionManager.LotChange(SystemManager.Instance().CurrentModel, lotNo);
                loopModeCheckCount = 0;

                SystemManager.Instance().OperatorManager.Start(false);
            }
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.PatternCheckBox.Background = Defect.GetBrush(DefectType.Pattern);
            this.MarginCheckBox.Background = Defect.GetBrush(DefectType.Margin);
            this.ShapeCheckBox.Background = Defect.GetBrush(DefectType.Shape);

            AdaptiveFigureScale();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                OnPropertyChanged("HomeMarkPos");
                OnPropertyChanged("CurMarkPos");
            }

        }
    }
}
