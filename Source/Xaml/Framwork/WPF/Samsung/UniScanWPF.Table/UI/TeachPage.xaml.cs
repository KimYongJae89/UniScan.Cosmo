using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    public partial class TeachPage : Page, INotifyPropertyChanged
    {
        public System.Windows.Size MarkSize
        {
            get
            {
                double scale = Scale.ScaleY / Scale.ScaleX;
                double x = Math.Min(2500 * scale, 100.0 / Scale.ScaleX);
                double y = Math.Min(2500 / scale, 100.0 / Scale.ScaleY);
                return new System.Windows.Size(x, y);
            }
        }

        public double StrokeThickness { get => 5 / Math.Max(Scale.ScaleY, Scale.ScaleX); }
        public double MarkFontSize { get => Math.Min(35791, Math.Min(MarkSize.Width, MarkSize.Height) / 2); }
        public System.Drawing.PointF HomeMarkPos
        {
            get
            {
                System.Drawing.PointF homePos = InfoBox.Instance.DispHomePos;
                System.Windows.Size markSize = MarkSize;
                return new System.Drawing.PointF((float)(homePos.X - markSize.Width / 2), (float)(homePos.Y - markSize.Height / 2));
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

        public TeachPage()
        {
            InitializeComponent();

            this.DataContext = this;

            this.dockPanel1.DataContext = InfoBox.Instance;

            this.DifferenceTextBlock.DataContext = SystemManager.Instance().OperatorManager.TeachOperator.Settings;
            this.MainCanvas.DataContext = InfoBox.Instance;
            //this.machineCircleLabel.DataContext = SystemManager.Instance().MachineObserver.MotionBox;

            this.HomeLabel.DataContext = this;
            this.MachineCircleLabel.DataContext = this;

            this.LightTuneImage.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;
            this.LightTuneMessage.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;

            this.LightTuneOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.LightTuneOperator;
            this.ScanOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ScanOperator;
            this.ExtractOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ExtractOperator;
            this.TeachOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.TeachOperator;

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

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            List<PatternGroup> pgList = GetSelectedPattern();

            pgList.ForEach(pg => SystemManager.Instance().CurrentModel.CandidatePatternList.Remove(pg));
            pgList.ForEach(pg => SystemManager.Instance().CurrentModel.InspectPatternList.Add(pg));
            
            InfoBox.Instance.ModelChanged();
            SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            PatternGroup pg = (PatternGroup)InspectPatternListView.SelectedItem;
            SystemManager.Instance().CurrentModel?.InspectPatternList.Remove(pg);
            SystemManager.Instance().CurrentModel?.CandidatePatternList.Add(pg);
            
            InfoBox.Instance.ModelChanged();
            SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }
        
        private void TeachButton_Click(object sender, RoutedEventArgs e)
        {
            SystemManager.Instance().OperatorManager.Teach();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            if (CustomMessageBox.Show("Are you really going to delete the patterns ?", "Delete", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            List<PatternGroup> pgList = GetSelectedPattern();

            pgList.ForEach(pg => SystemManager.Instance().CurrentModel?.CandidatePatternList.Remove(pg));
            InfoBox.Instance.ModelChanged();
            SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }

        private List<PatternGroup> GetSelectedPattern()
        {
            List<PatternGroup> pgList = new List<PatternGroup>();
            foreach (object obj in CandidatePatternListView.SelectedItems)
            {
                if (obj is PatternGroup)
                    pgList.Add((PatternGroup)obj);
            }

            return pgList;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            SystemManager.Instance().OperatorManager.Start(true);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            SystemManager.Instance().OperatorManager.Cancle(null);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (SystemManager.Instance().OperatorManager.IsRun)
                TeachBorder.Visibility = Visibility.Visible;
            else
                TeachBorder.Visibility = Visibility.Hidden;

            OnPropertyChanged("CurMarkPos");
            OnPropertyChanged("CurMarkBrush");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Translate.X = 0;
            Translate.Y = 0;

            Scale.ScaleX = MainCanvas.ActualWidth / ((double)(InfoBox.Instance.DispRobotRegion.Width));
            Scale.ScaleY = MainCanvas.ActualHeight / ((double)(InfoBox.Instance.DispRobotRegion.Height));
            RobotWorkingRectangle.StrokeThickness = 1 / Math.Min(Scale.ScaleX, Scale.ScaleY);

            OnPropertyChanged("MarkSize");
            OnPropertyChanged("HomeMarkPos");
            OnPropertyChanged("MarkFontSize");
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
    }
    public class NumericValidationRule : ValidationRule
    {
        public Type ValidationType { get; set; }

        public float Min { get => min; set => min = value; }
        float min;

        public float Max { get => max; set => max = value; }
        float max;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                float f = float.Parse(value.ToString());
                if (f<min)
                    return new ValidationResult(false, string.Format("input number is Less than {0}",min));
                else if (f > max)
                    return new ValidationResult(false, string.Format("input number is Greater than {0}", max));

                return ValidationResult.ValidResult;
            }
            catch(Exception ex)
            {
                return new ValidationResult(false, "is not a number");
            }
        }
    }
}