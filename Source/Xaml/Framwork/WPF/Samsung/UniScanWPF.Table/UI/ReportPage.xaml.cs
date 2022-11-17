using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UniScanWPF.Table;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;
using UniScanWPF.Table.UI;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Screen.Table.UI
{
    /// <summary>
    /// ReportPage.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class ReportPage : Page, INotifyPropertyChanged, IMultiLanguageSupport
    {
        TranslateTransform originTranslateTransform = new TranslateTransform();
        ImagePanel[] imagePanels = new ImagePanel[2];

        public int[][] SummaryData { get => summaryData; }
        int[][] summaryData = new int[1][] { new int[] { 0, 0, 0 } };

        public double[] LengthHeight { get => this.lengthHeight; }
        double[] lengthHeight = new double[3];

        public double[] LengthWidth { get => this.lengthWidth; }
        double[] lengthWidth = new double[3];

        public MeanderMeasure[] MeanderMeasureData { get => this.meanderMeasureData; }
        MeanderMeasure[] meanderMeasureData = new MeanderMeasure[3];

        public ExtraMeasure[] MarginMeasureData { get => this.marginMeasureData; }
        ExtraMeasure[] marginMeasureData = new ExtraMeasure[4];

        public InspectOperatorSettings InspectOperatorSettings { get => this.inspectOperatorSettings; }
        InspectOperatorSettings inspectOperatorSettings = null;

        public BitmapSource BigDefectBitmapSource
        {
            get => bigDefectBitmapSource;
            set
            {
                if (this.bigDefectBitmapSource != value)
                {
                    this.bigDefectBitmapSource = value;
                    OnPropertyChanged("BigDefectBitmapSource");
                }
            }
        }
        BitmapSource bigDefectBitmapSource = null;

        public bool IsHideChecked
        {
            get => visibilitySecondImagePanel;
            set
            {
                visibilitySecondImagePanel = value;
                OnPropertyChanged("IsHideChecked");
            }
        }
        bool visibilitySecondImagePanel;

        ObservableCollection<CanvasDefect> defectList = new ObservableCollection<CanvasDefect>();
        List<LoadItem> curResultList = null;
        TranslateTransform offsetTranslateTransform = new TranslateTransform();

        public ObservableCollection<CanvasDefect> DefectList { get => defectList; set => defectList = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ReportPage()
        {
            InitializeComponent();
            this.DataContext = this;

            this.imagePanels[0] = new ImagePanel(true, true);
            this.imagePanels[0].Notify = SetCurrentIndex;
            this.imagePanels[0].OnTranslateChanged = ImagePanels_OnTranslateChanged0;
            this.imagePanels[0].OnZoomChanged = ImagePanels_OnZoomChanged;
            this.firstImagePanel.Navigate(this.imagePanels[0]);

            this.imagePanels[1] = new ImagePanel(true, false);
            this.imagePanels[1].Notify = SetCurrentIndex;
            this.imagePanels[1].OnTranslateChanged = ImagePanels_OnTranslateChanged1;
            this.secondImagePanelFrame.Navigate(this.imagePanels[1]);

            LocalizeHelper.AddListener(this);
        }

        private void ImagePanels_OnZoomChanged(ScaleTransform scaleTransform)
        {
            this.imagePanels[1].ScaleTransform = scaleTransform;
        }

        private void ImagePanels_OnTranslateChanged0(TranslateTransform translateTransform)
        {
            this.imagePanels[1].TranslateTransform.X = translateTransform.X + this.offsetTranslateTransform.X;
            this.imagePanels[1].TranslateTransform.Y = translateTransform.Y + this.offsetTranslateTransform.Y;
        }

        private void ImagePanels_OnTranslateChanged1(TranslateTransform translateTransform)
        {
            this.offsetTranslateTransform.X = this.imagePanels[1].TranslateTransform.X - this.imagePanels[0].TranslateTransform.X;
            this.offsetTranslateTransform.Y = this.imagePanels[1].TranslateTransform.Y - this.imagePanels[0].TranslateTransform.Y;
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

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StartDate == null || EndDate == null)
                return;

            if (StartDate.SelectedDate > EndDate.SelectedDate)
            {
                DatePicker datePicker = sender as DatePicker;
                if (datePicker.Name == "StartDate")
                    EndDate.SelectedDate = StartDate.SelectedDate;
                else
                    StartDate.SelectedDate = EndDate.SelectedDate;
                return;
            }

            SearchProduction();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            SearchProduction();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
                SearchProduction();
        }

        private void SearchProduction()
        {
            if (ProductionList == null)
                return;

            ProductionList.ItemsSource = null;

            ProductionList.ItemsSource = SystemManager.Instance().ProductionManager.List.FindAll(production => production.StartTime >= StartDate.SelectedDate && production.StartTime <= EndDate.SelectedDate.Value.AddDays(1));
        }
        
        private void DefectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CanvasDefect selectedDefect = defectListBox.SelectedItem as CanvasDefect;
            this.BigDefectBitmapSource = selectedDefect?.Defect.GetBitmapSource();

            Array.ForEach(this.imagePanels, f =>
             {
                 f.ClearSelection();
                 f.SetSelection(selectedDefect);
             });
        }

        private void PatternCheckBox_CheckChanged(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(defectListBox?.ItemsSource)?.Refresh();
            Array.ForEach(this.imagePanels, f =>f?.VisiblePatternCanvas(PatternCheckBox.IsChecked.Value));
        }

        private void MarginCheckBox_CheckeChanged(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(defectListBox?.ItemsSource)?.Refresh();
            Array.ForEach(this.imagePanels, f =>f?.VisiblePatternCanvas(MarginCheckBox.IsChecked.Value));
        }

        private void ShapeCheckBox_CheckeChanged(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(defectListBox?.ItemsSource)?.Refresh();
            Array.ForEach(this.imagePanels, f =>f?.VisiblePatternCanvas(ShapeCheckBox.IsChecked.Value));
        }

        private void ProductionChange(ImagePanel sender,int index)
        {
            this.BigDefectBitmapSource = null;

            if (curResultList != null && curResultList.Count != 0)
            {
                LoadItem tuple = curResultList[index];
                if (tuple.IsLoaded == false)
                {
                    bool loaded = false;
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    SimpleProgressWindow simpleProgressWindow = new SimpleProgressWindow("Load");
                    simpleProgressWindow.Show(() =>
                    {
                        loaded = tuple.Load(cancellationTokenSource.Token);
                    }, cancellationTokenSource);

                    if (cancellationTokenSource.IsCancellationRequested || loaded == false)
                        return;
                }

                if (sender == null || sender == this.imagePanels[0])
                {
                    defectList.Clear();
                    CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Filter = Filter;
                    UpdateLengthProperty(tuple.CanvasDefectList);
                    UpdateTeachDataProperty(tuple.OperatorSettingList);
                    tuple.CanvasDefectList.ForEach(f => defectList.Add(f));
                }

                if (sender != null)
                    sender.ProductionChange(tuple);
                else
                    Array.ForEach(this.imagePanels, f => f?.ProductionChange(tuple));
            }
            OnPropertyChanged("DefectList");
        }

        private void UpdateTeachDataProperty(List<OperatorSettings> operatorSettingList)
        {
            this.inspectOperatorSettings = (InspectOperatorSettings)operatorSettingList.Find(f => f is InspectOperatorSettings);
            OnPropertyChanged("InspectOperatorSettings");
        }

        private void UpdateLengthProperty(List<CanvasDefect> canvasDefectList)
        {
            UpdateLengthWHProperty(canvasDefectList);
            UpdateLengthMeanderProperty(canvasDefectList);
            UpdateLengthMarginProperty(canvasDefectList);
        }

        private void UpdateLengthMarginProperty(List<CanvasDefect> canvasDefectList)
        {
            List<ExtraMeasure> list = canvasDefectList.FindAll(f => f.Defect is ExtraMeasure).ConvertAll(f => (ExtraMeasure)f.Defect);
            list.RemoveAll(f => f.ReferencePos == UniScanWPF.Table.Inspect.ReferencePos.None);

            int length = Math.Min(list.Count, this.marginMeasureData.Length);
            for (int i = 0; i < this.marginMeasureData.Length; i++)
            {
                if (i < list.Count)
                    this.marginMeasureData[i] = list[i];
                else
                    this.marginMeasureData[i] = null;
            }
            OnPropertyChanged("MarginMeasureData"); 
            OnPropertyChanged("VisibleMarginMeasureData");
        }

        private void UpdateLengthMeanderProperty(List<CanvasDefect> canvasDefectList)
        {
            // Update Meander
            List<MeanderMeasure> list = canvasDefectList.FindAll(f => f.Defect is MeanderMeasure).ConvertAll(f => (MeanderMeasure)f.Defect);
            int length = Math.Min(list.Count, this.meanderMeasureData.Length);
            for (int i = 0; i < this.meanderMeasureData.Length; i++)
            {
                if (i < list.Count)
                    this.meanderMeasureData[i] = list[i];
                else
                    this.meanderMeasureData[i] = null;
            }
            OnPropertyChanged("MeanderMeasureData");
        }

        private void UpdateLengthWHProperty(List<CanvasDefect> canvasDefectList)
        {
            List<CanvasDefect> lengthCanvasDefectList = canvasDefectList.FindAll(f => f.Defect is LengthMeasure);
            
            // Update Height
            Array.Clear(this.lengthHeight, 0, this.lengthHeight.Length);
            List<CanvasDefect> verticalList = lengthCanvasDefectList.FindAll(f =>
            {
                LengthMeasure dd = f.Defect as LengthMeasure;
                return dd != null && dd.Direction == DynMvp.Vision.Direction.Vertical && dd.IsValid;
            });

            if (verticalList.Count > 0)
            {
                int indexSrc = verticalList.FindIndex(f => ((LengthMeasure)f.Defect).LengthMm > 0);
                int indexDst = verticalList.FindLastIndex(f => ((LengthMeasure)f.Defect).LengthMm > 0);
                int indexMid = (indexSrc + indexDst) / 2;

                if (indexSrc >= 0)
                {
                    LengthMeasure lengthMeasure = (LengthMeasure)verticalList[indexSrc].Defect;
                    this.lengthHeight[0] = lengthMeasure.LengthMm;
                }
                if (indexMid >= 0)
                {
                    LengthMeasure lengthMeasure = (LengthMeasure)verticalList[indexMid].Defect;
                    this.lengthHeight[1] = lengthMeasure.LengthMm;
                }
                if (indexDst >= 0)
                {
                    LengthMeasure lengthMeasure = (LengthMeasure)verticalList[indexDst].Defect;
                    this.lengthHeight[2] = lengthMeasure.LengthMm;
                }
            }
            OnPropertyChanged("LengthHeight");


            Array.Clear(this.lengthWidth, 0, this.lengthWidth.Length);
            List<CanvasDefect> horizontalList = lengthCanvasDefectList.FindAll(f => ((LengthMeasure)f.Defect).Direction == DynMvp.Vision.Direction.Horizontal && ((LengthMeasure)f.Defect).LengthMm > 0);
            if (horizontalList.Count > 0)
            {
                int indexSrc = horizontalList.FindIndex(f => ((LengthMeasure)f.Defect).LengthMm > 0);
                int indexDst = horizontalList.FindLastIndex(f => ((LengthMeasure)f.Defect).LengthMm > 0);
                int indexMid = (indexSrc + indexDst) / 2;

                if (indexSrc >= 0)
                {
                    this.lengthWidth[0] = ((LengthMeasure)horizontalList[indexSrc].Defect).LengthMm;
                    canvasDefectList.Add(horizontalList[indexSrc]);
                }
                if (indexMid >= 0)
                {
                    this.lengthWidth[1] = ((LengthMeasure)horizontalList[indexMid].Defect).LengthMm;
                    canvasDefectList.Add(horizontalList[indexMid]);
                }
                if (indexDst >= 0)
                {
                    this.lengthWidth[2] = ((LengthMeasure)horizontalList[indexDst].Defect).LengthMm;
                    canvasDefectList.Add(horizontalList[indexDst]);
                }
            }
            OnPropertyChanged("LengthWidth");
        }
        
        private void ProductionList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UniScanWPF.Table.Data.Production production = (UniScanWPF.Table.Data.Production)ProductionList.SelectedItem;
            if (production == null)
                return;

            SimpleProgressWindow loadProgressWindow = new SimpleProgressWindow("Load");
            loadProgressWindow.Show(new Action(() =>
            {
                curResultList = StoringOperator.Load(production);
            }));

            UpdateSummary(production);

            Array.ForEach(this.imagePanels, f => f?.UpdateCombobox(production));

            SetCurrentIndex(null,0);

            //ProductionChange();
        }

        private void SetCurrentIndex(ImagePanel sender, int index)
        {
            if (curResultList == null)
                return;

            int valid = Math.Min(curResultList.Count - 1, Math.Max(index, 0));
            ProductionChange(sender, valid);
        }

        private void UpdateSummary(Production production)
        {
            Array.Clear(summaryData[0], 0, summaryData[0].Length);
            summaryData[0][0] = production.PatternCount;
            summaryData[0][1] = production.MarginCount;
            summaryData[0][2] = production.ShapeCount;
            OnPropertyChanged("SummaryData");
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.PatternCheckBox.Background = Defect.GetBrush(DefectType.Pattern);
            this.MarginCheckBox.Background = Defect.GetBrush(DefectType.Margin);
            this.ShapeCheckBox.Background = Defect.GetBrush(DefectType.Shape);
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            Production production = (Production)ProductionList.SelectedItem;
            BuildSummary(production);
        }

        private void BuildSummary(Production production)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            new SimpleProgressWindow("Build Summary").Show(new Action(() =>
            {
                curResultList.ForEach(f => f.Load(cancellationTokenSource.Token));
                if (cancellationTokenSource.IsCancellationRequested)
                    return;

                production.UpdateCount(curResultList);
                SystemManager.Instance().ProductionManager.Save();
            }), cancellationTokenSource);

            UpdateSummary(production);
        }

        private void SyncButton_Click(object sender, RoutedEventArgs e)
        {
            this.offsetTranslateTransform.X = 0;
            this.offsetTranslateTransform.Y = 0;

            this.imagePanels[1].ScaleTransform = this.imagePanels[0].ScaleTransform;
            this.imagePanels[1].TranslateTransform = this.imagePanels[0].translateTransform;
        }

        private void HideButton_CheckedChanged(object sender, RoutedEventArgs e)
        {
            imagePanelGrid.RowDefinitions[1].Height = new GridLength(!this.visibilitySecondImagePanel ? 1 : 0, GridUnitType.Star);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Array.ForEach(this.imagePanels, f => f.ZoomFit());
        }

        private void ShowReportPathButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
