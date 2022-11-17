using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using DynMvp.Vision;
using DynMvp.Vision.Planbss;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Base;
using DynMvp.Data.Forms;
using UniEye.Base;
using DynMvp.Authentication;
using UniScanG.UI.Teach.Inspector;
using UniScanG.UI.Teach;
using UniScanG.Vision;
using UniScanG.Gravure.Vision.Trainer;
using UniEye.Base.UI;
using UniScan.Common;
using UniScanG.Screen.Data;
using UniScanG.Gravure.Vision;
using DynMvp.Devices;
//using UniEye.Base.Settings;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.SheetFinder.FiducialMarkBase;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.SheetFinder.SheetBase;
using UniScanG.Temp;
using Infragistics.Win.UltraWinTabControl;
using UniScanG.Gravure.Vision.Watcher;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.UI.Teach.Inspector
{
    public enum ParamTabKey
    {
        Fiducial, Train, Inspect, Misc
    }

    public partial class ParamController : UserControl, IModellerControl, IModelListener, IUserHandlerListener, IMultiLanguageSupport
    {
        private SheetFinderBaseParam FiducialFinderParam
        {
            get { return AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName)?.Param as SheetFinderBaseParam; }
        }

        private TrainerParam TrainerParam
        {
            get { return AlgorithmPool.Instance().GetAlgorithm(Trainer.TypeName)?.Param as TrainerParam; }
        }

        private CalculatorParam CalculatorParam
        {
            get { return AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName)?.Param as CalculatorParam; }
        }

        private DetectorParam DetectorParam
        {
            get { return AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName)?.Param as DetectorParam; }
        }

        private WatcherParam WatcherParam
        {
            get { return AlgorithmPool.Instance().GetAlgorithm(Watcher.TypeName)?.Param as WatcherParam; }
        }

        bool updataData = false;

        ModellerPageExtenderG modellerPageExtender;
        Size imageSize = Size.Empty;

        public ParamController()
        {
            InitializeComponent();

            StringManager.AddListener(this);

            updataData = true;
            
            //this.searchDirXBase.DataSource = Enum.GetValues(typeof(BaseXSearchDir));
            //this.groupDirection.DataSource = Enum.GetValues(typeof(Direction));

            this.defectMaxCount.Minimum = this.dielectricMinSize.Minimum = this.electricMinSize.Minimum = 0;
            this.defectMaxCount.Maximum = this.dielectricMinSize.Maximum = this.electricMinSize.Maximum = int.MaxValue;

            int[] speedArray = new int[] { 10, 20, 40, 60, 80, 100, 120 };
            Array.ForEach(speedArray, f => this.grabSpeedMpM.Items.Add(f));
            
            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            this.logLevel.Items.AddRange(Enum.GetNames(typeof(LogLevel)));

            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            UserHandler.Instance().AddListener(this);
            updataData = false;

            UpdateData();
        }

        public void SetModellerExtender(UniScanG.UI.Teach.ModellerPageExtender modellerPageExtender)
        {
            this.modellerPageExtender = modellerPageExtender as ModellerPageExtenderG;

            this.modellerPageExtender.ImageUpdated = modellerPageExtender_ImageUpdated;
        }

        private void modellerPageExtender_LoadImage()
        {
            throw new NotImplementedException();
        }

        private void modellerPageExtender_ImageUpdated(Image image)
        {
            if (image == null)
                this.imageSize = Size.Empty;
            else
                this.imageSize = image.Size;

            //UpdateData();
            //UpdateOverlayFigure();
        }


        delegate void UpdateDataDelegate();
        public void UpdateData()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            if (SystemManager.Instance().CurrentModel == null)
            {
                tabControlParam.Tabs[0].Visible = false;
                tabControlParam.Tabs[1].Visible = false;
                tabControlParam.Tabs[2].Visible = false;
            }
            else
            {
                tabControlParam.Tabs[0].Visible = true;
                tabControlParam.Tabs[1].Visible = true;
                tabControlParam.Tabs[2].Visible = true;

                UpdatePatternSelector();
                UpdateRegionSelector(true);
                UpdateMonitoringSelector();

                UpdateFiducialFinderParamControl();
                UpdateTrainerParamControl();
                UpdateInspectorParamControl();

                UpdateOverlayFigure();
            }
            UpdateMiscParamControl();
        }

        private void UpdateOverlayFigure()
        {
            FigureGroup figureGroup = new FigureGroup();

            AppendFigureBase(figureGroup);

            UltraTab selectedTab = tabControlTarget.SelectedTab;
            if (selectedTab != null)
            {
                switch (selectedTab.Index)
                {
                    case 0:
                        // 패턴 그리기
                        AppendFigurePattern(figureGroup);
                        break;
                    case 1:
                        // 영역 그리기
                        AppendFigureRegion(figureGroup);
                        break;
                    case 2:
                        // 모니터링 영역 그리기
                        AppendFigureWatch(figureGroup);
                        break;
                }
            }
            
            if (SystemManager.Instance().CurrentModel != null)
            {
                float scaleFactor = SystemManager.Instance().CurrentModel.ScaleFactor * 1.0f / ImageController.RescaleFactor;
                figureGroup.Scale(scaleFactor, scaleFactor);
            }
            modellerPageExtender?.UpdateFigure(figureGroup);
        }

        private void AppendFigureBase(FigureGroup figureGroup)
        {
            SheetFinderBaseParam sheetFinderBaseParam = FiducialFinderParam;
            if (sheetFinderBaseParam is SheetFinderPJParam)
            {
                SheetFinderPJParam sheetFinderPJParam = sheetFinderBaseParam as SheetFinderPJParam;
                int fidSearchL = (int)(imageSize.Width * sheetFinderPJParam.FidSearchLBound);
                int fidSearchR = (int)(imageSize.Width * sheetFinderPJParam.FidSearchRBound);
                if (fidSearchR > fidSearchL)
                    figureGroup.AddFigure(new RectangleFigure(Rectangle.FromLTRB(fidSearchL, 0, fidSearchR, imageSize.Height), new Pen(Color.Red, 5)));
            }

            if (CalculatorParam != null)
            {
                figureGroup.AddFigure(new LineFigure(new PointF(CalculatorParam.BasePosition.X, 0), new PointF(CalculatorParam.BasePosition.X, CalculatorParam.BasePosition.Y), new Pen(Color.Cyan)));
                figureGroup.AddFigure(new LineFigure(new PointF(0, CalculatorParam.BasePosition.Y), new PointF(CalculatorParam.BasePosition.X, CalculatorParam.BasePosition.Y), new Pen(Color.Cyan)));
            }
        }

        private void AppendFigurePattern(FigureGroup figureGroup)
        {
            if (patternSelector.SelectedRows.Count > 0)
            {
                SheetPatternGroup patternGroup = patternSelector.SelectedRows[0].Tag as SheetPatternGroup;
                Figure figure = patternGroup.CreateFigureGroup();
                figureGroup.AddFigure(figure);
                int edgeWidth = CalculatorParam.EdgeWidth;
                if (edgeWidth > 0)
                {
                    Figure figure2 = (Figure)figure.Clone();
                    figure2.Inflate(-edgeWidth * 2, -edgeWidth * 2);
                    figureGroup.AddFigure(figure2);

                    Figure figure3 = (Figure)figure.Clone();
                    figure3.Inflate(edgeWidth, edgeWidth);
                    figureGroup.AddFigure(figure3);
                }
            }
        }

        private void AppendFigureRegion(FigureGroup figureGroup)
        {
            if (regionSelector.SelectedRows.Count > 0)
            {
                RegionInfoG regionInfo = regionSelector.SelectedRows[0].Tag as RegionInfoG;
                if (regionInfo != null)
                {
                    Figure figure = regionInfo.GetFigure();
                    figure.Offset(regionInfo.Region.Location);
                    figureGroup.AddFigure(figure);
                }
            }
        }

        private void AppendFigureWatch(FigureGroup figureGroup)
        {
            WatcherParam watcherParam = this.WatcherParam;
            if (watcherParam == null)
                return;

            foreach(WatchItem watchItem in watcherParam.WatchItemList)
            {
                figureGroup.AddFigure(watchItem.GetFigure());
            }
        }

        #region UpdateUI
        private void UpdatePatternSelector()
        {
            updataData = true;

            patternCount.Text = "0";
            patternSelector.Rows.Clear();

            List<SheetPatternGroup> patternGroupList = CalculatorParam.PatternGroupList;
            patternCount.Text = patternGroupList.Count.ToString();

            List<DataGridViewRow> rowList = new List<DataGridViewRow>();
            for (int i = 0; i < patternGroupList.Count; i++)
            {
                SheetPatternGroup patternGroup = patternGroupList[i] as SheetPatternGroup;
                if (patternGroup == null)
                    continue;

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(patternSelector);

                row.Cells[0].Value = i + 1;
                row.Cells[1].Value = patternGroup.Use;
                row.Cells[2].Value = patternGroup.MasterImage?.ToBitmap();
                row.Cells[3].Value = patternGroup.GetInfoText();

                row.Tag = patternGroup;
                row.Height = 150;
                rowList.Add(row);
            }

            patternSelector.Rows.AddRange(rowList.ToArray());

            updataData = false;
        }
        
        private void UpdateRegionSelector(bool clearAndRedraw)
        {
            updataData = true;

            regionCount.Text = "0";

            CalculatorParam calculatorParam = this.CalculatorParam;
            List<RegionInfoG> regionInfoList = calculatorParam.RegionInfoList;
            regionCount.Text = regionInfoList.Count.ToString();
            basePos.Text = string.Format("{0},{1}", calculatorParam.BasePosition.X, calculatorParam.BasePosition.Y);

            if (clearAndRedraw)
            {
                regionSelector.Rows.Clear();

                for (int i = 0; i < regionInfoList.Count; i++)
                {
                    RegionInfoG regionInfoG = regionInfoList[i] as RegionInfoG;
                    if (regionInfoG == null)
                        continue;

                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(regionSelector);

                    FillRow(i, row, regionInfoG);
                    //row.Cells[0].Value = i + 1;
                    //((DataGridViewCheckBoxCell)row.Cells[1]).Value = regionInfoG.Use;
                    //row.Cells[2].Value = regionInfoG.PatternImage?.ToBitmap();
                    //row.Cells[3].Value = regionInfoG.GetInfoString();
                    //row.Height = 150;
                    //row.Tag = regionInfoG;
                    int idx = regionSelector.Rows.Add(row);
                }
            }
            else
            {
                for (int i = 0; i < regionSelector.Rows.Count; i++)
                {
                    DataGridViewRow row = regionSelector.Rows[i];
                    RegionInfoG regionInfoG = row.Tag as RegionInfoG;
                    if (regionInfoG == null)
                        return;

                    FillRow(row.Index, row, regionInfoG);
                    //((DataGridViewCheckBoxCell)row.Cells[1]).Value = regionInfoG.Use;
                    //row.Cells[2].Value = regionInfoG.PatternImage?.ToBitmap();
                    //row.Cells[3].Value = regionInfoG.GetInfoString();
                    //row.Tag = regionInfoG;
                    //row.Height = 150;
                }
            }
            updataData = false;
        }

        private void FillRow(int idx, DataGridViewRow row, RegionInfoG regionInfoG)
        {
            row.Cells[0].Value = idx + 1;
            ((DataGridViewCheckBoxCell)row.Cells[1]).Value = regionInfoG.Use;
            
            row.Cells[2].Value = regionInfoG.Thumbnail?.ToBitmap();
            row.Cells[3].Value = regionInfoG.GetInfoString();
            row.Height = 150;
            row.Tag = regionInfoG;
        }

        private void UpdateMonitoringSelector()
        {
            updataData = true;

            monitoringSelector.Rows.Clear();
            monitoringCount.Text = "0";

            List<WatchItem> watchItemList = this.WatcherParam?.WatchItemList;
            if (watchItemList != null)
            {
                Rectangle imageRect = Rectangle.Empty;
                Image2D curImage = this.modellerPageExtender?.CurrentImage;
                if (curImage != null)
                    imageRect = new Rectangle(Point.Empty, this.modellerPageExtender.CurrentImage.Size);

                foreach (WatchItem watchItem in  watchItemList)
                {
                    DataGridViewRow newRow = new DataGridViewRow();
                    newRow.CreateCells(this.monitoringSelector);
                    newRow.Cells[0].Value = watchItem.Index;
                    newRow.Cells[1].Value = watchItem.Use;
                    newRow.Cells[3].Value = watchItem.WatchType;

                    Rectangle adjustRect = Rectangle.Intersect(imageRect, watchItem.Rectangle);
                    if (adjustRect.Width > 0 && adjustRect.Height > 0)
                    {
                        Image2D image2D = (Image2D)this.modellerPageExtender.CurrentImage.ClipImage(adjustRect);
                        newRow.Cells[2].Value = image2D.ToBitmap();
                        image2D.Dispose();
                    }
                    newRow.Height = 150;

                    monitoringSelector.Rows.Add(newRow);
                }
            }

            monitoringCount.Text = monitoringSelector.Rows.Count.ToString();
            updataData = false;

        }

        private void UpdateFiducialFinderParamControl()
        {
            updataData = true;

            AlgorithmParam algorithmParam = FiducialFinderParam;
            if (algorithmParam is SheetFinderPJParam)
            {
                this.layoutFiducial1.Visible = true;
                SheetFinderPJParam fiducialFinderPJParam = (SheetFinderPJParam)algorithmParam;
                this.fiducialSearchRangeL.Value = (decimal)fiducialFinderPJParam.FidSearchLBound * 100;
                this.fiducialSearchRangeR.Value = (decimal)fiducialFinderPJParam.FidSearchRBound * 100;
                this.fiducialSizeW.Value = fiducialFinderPJParam.FidSize.Width;
                this.fiducialSizeH.Value = fiducialFinderPJParam.FidSize.Height;

                this.searchDirXBase.SelectedIndex = (int)fiducialFinderPJParam.BaseXSearchDir;
            }
            else
            {
                this.layoutFiducial1.Visible = false;
            }

            if (algorithmParam is SheetFinderV2Param)
            {
                this.layoutFiducial2.Visible = true;
                SheetFinderV2Param sheetFinderV2Param = (SheetFinderV2Param)algorithmParam;
                this.fiducialThresholdMul.Value = (decimal)sheetFinderV2Param.ProjectionBinalizeMul;
                this.fiducialBlackLenMul.Value = (decimal)sheetFinderV2Param.BlankLengthMul;

                this.searchDirXBase.SelectedIndex = (int)sheetFinderV2Param.BaseXSearchDir;
                int camIndex = SystemManager.Instance().ExchangeOperator.GetCamIndex();
                this.searchDirXBase.Enabled = UserHandler.Instance().CurrentUser.SuperAccount || camIndex < 0;
            }
            else
            {
                this.layoutFiducial2.Visible = false;
            }

            updataData = false;
        }

        private void UpdateTrainerParamControl()
        {
            updataData = true;

            TrainerParam sheetTrainerParam = this.TrainerParam;
            labelTeachedBinalizeValue.Text = sheetTrainerParam.BinValue.ToString();
            binValueOffset.Value = (decimal)sheetTrainerParam.BinValueOffset;
            groupTolerance.Value = (decimal)sheetTrainerParam.SheetPatternGroupThreshold;
            minPatternArea.Value = (decimal)sheetTrainerParam.MinPatternArea;
            this.groupDirection.SelectedIndex= (int)sheetTrainerParam.SplitLineDirection;
            
            this.isCrisscross.Checked = sheetTrainerParam.IsCrisscross;
            this.kernalSize.Value = (decimal)sheetTrainerParam.KernalSize;
            this.diffrentialThreshold.Value = (decimal)sheetTrainerParam.DiffrentialThreshold;

            this.kernalSize.Enabled = this.diffrentialThreshold.Enabled = sheetTrainerParam.IsCrisscross;

            updataData = false;
        }

        private void UpdateInspectorParamControl()
        {
            updataData = true;

            CalculatorParam calculatorParam = this.CalculatorParam as CalculatorParam;
            if (calculatorParam != null)
            {
                this.calculateEdgeWidth.Value = calculatorParam.EdgeWidth;
                this.calculateEdgeValue.Value = calculatorParam.EdgeValue;
                this.calculateEdgeMethod.SelectedIndex = (int)calculatorParam.EdgeFindMethod;
                this.calculateEdgeValue.Enabled = calculatorParam.EdgeFindMethod == CalculatorParam.EEdgeFindMethod.Projection;

                this.inBarAlignX.Checked = calculatorParam.InBarAlignX;
                this.inBarAlignY.Checked = calculatorParam.InBarAlignY;

                this.adaptivePairing.Checked = calculatorParam.AdaptivePairing;
                this.boundaryPairStep.Value = calculatorParam.BoundaryPairStep;
                this.ignoreMethod.SelectedIndex = (int)calculatorParam.IgnoreMethod;
                this.ignoreLastBoundary.Checked = calculatorParam.IgnoreSideLine;

                this.calcParallelOperation.Checked = calculatorParam.ParallelOperation;
            }

            DetectorParam detectorParam =  this.DetectorParam as DetectorParam;
            if (detectorParam != null)
            {
                this.defectMaxCount.Value = detectorParam.MaximumDefectCount;
                this.detectThresholdBase.Value = detectorParam.DetectThresholdBase;
                this.fineSizeMeasure.Checked = detectorParam.FineSizeMeasure;
                this.fineSizeMeasureSizeMul.Value = (decimal)detectorParam.FineSizeMeasureSizeMul;
                this.fineSizeMeasureThresholdMul.Value = (decimal)detectorParam.FineSizeMeasureThresholdMul;
                this.fineSizeMeasureThresholdMul.Enabled = this.fineSizeMeasureSizeMul.Enabled = detectorParam.FineSizeMeasure;
                this.detectParallelOperation.Checked = detectorParam.ParallelOperation;
            }

            AlgorithmSetting algorithmSetting = AlgorithmSetting.Instance();
            this.electricMinSize.Value = algorithmSetting.MinBlackDefectLength;
            this.dielectricMinSize.Value = algorithmSetting.MinWhiteDefectLength;
            updataData = false;
        }
        
        private void UpdateMiscParamControl()
        {
            updataData = true;

            bufferSize.Value = AlgorithmSetting.Instance().InspBufferCount;
            monitoringPeriod.Value = AlgorithmSetting.Instance().MonitoringPeriod;

            if (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count > 0)
            {
                SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;
                xPixelCal.Value = (decimal)pelSize.Width;
                yPixelCal.Value = (decimal)pelSize.Height;
            }

            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;
            asyncMode.Checked = additionalSettings.UseAsyncMode;
            grabSpeedkHz.Value = (decimal)(additionalSettings.AsyncGrabHz / 1000);
            grabSpeedkHz.Enabled = additionalSettings.UseAsyncMode;

            if (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count > 0)
            {
                float resUm = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height;
                float spdMpm = additionalSettings.AsyncGrabHz * resUm / 1000 / 1000 * 60;
                grabSpeedMpM.Text = spdMpm.ToString("0.00");
                grabSpeedMpM.Enabled = additionalSettings.UseAsyncMode;
            }

            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            this.imageRescaler.Enabled = (model != null);
            if (model != null)
                this.imageRescaler.Value = model.ScaleFactor;


            logLevel.Text = UniEye.Base.Settings.OperationSettings.Instance().LogLevel.ToString();
            debugMode.Checked = UniEye.Base.Settings.OperationSettings.Instance().SaveDebugImage;

            updataData = false;
        }
        #endregion

        #region Apply Value
        private void SetFiducialFinderParam(bool updateFigure)
        {
            if (updataData == true)
                return;
            
            SystemManager.Instance().CurrentModel.Modified = true;
            AlgorithmParam algorithmParam = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName).Param;
            if (algorithmParam is SheetFinderPJParam)
            {
                this.layoutFiducial1.Visible = true;
                SheetFinderPJParam fiducialFinderPJParam = (SheetFinderPJParam)algorithmParam;
                fiducialFinderPJParam.FidSearchLBound = ((float)this.fiducialSearchRangeL.Value) / 100.0f;
                fiducialFinderPJParam.FidSearchRBound = ((float)this.fiducialSearchRangeR.Value) / 100.0f;
                int fidSizeW = (int)this.fiducialSizeW.Value;
                int fidSizeH = (int)this.fiducialSizeH.Value;
                fiducialFinderPJParam.FidSize = new Size(fidSizeW, fidSizeH);

                fiducialFinderPJParam.BaseXSearchDir = (BaseXSearchDir)this.searchDirXBase.SelectedIndex;
            }

            if (algorithmParam is SheetFinderV2Param)
            {
                this.layoutFiducial2.Visible = true;
                SheetFinderV2Param sheetFinderV2Param = (SheetFinderV2Param)algorithmParam;
                sheetFinderV2Param.ProjectionBinalizeMul = (float)fiducialThresholdMul.Value;
                sheetFinderV2Param.BlankLengthMul = (float)fiducialBlackLenMul.Value;

                sheetFinderV2Param.BaseXSearchDir = (BaseXSearchDir)this.searchDirXBase.SelectedIndex;
            }

            if(updateFigure)
                UpdateOverlayFigure();
            //UpdateRegionSelector();
            //UpdateData();
        }

        private void SetTrainerParam()
        {
            if (updataData == true)
                return;

            SystemManager.Instance().CurrentModel.Modified = true;
            TrainerParam sheetTrainerParam = (TrainerParam)AlgorithmPool.Instance().GetAlgorithm(Trainer.TypeName).Param;
            sheetTrainerParam.BinValueOffset = (int)binValueOffset.Value;
            sheetTrainerParam.SheetPatternGroupThreshold = (float)groupTolerance.Value;
            sheetTrainerParam.MinPatternArea = (int)minPatternArea.Value;
            sheetTrainerParam.SplitLineDirection = (Direction)this.groupDirection.SelectedIndex;

            sheetTrainerParam.IsCrisscross = isCrisscross.Checked;
            sheetTrainerParam.MinLineIntensity = (int)minLineIntens.Value;
            sheetTrainerParam.KernalSize = (int)kernalSize.Value;
            sheetTrainerParam.DiffrentialThreshold = (int)diffrentialThreshold.Value;

            UpdateTrainerParamControl();
        }

        private void SetInspectorParam(bool updateFigure)
        {
            if (updataData == true)
                return;

            SystemManager.Instance().CurrentModel.Modified = true;
            CalculatorParam calculatorParam = this.CalculatorParam as CalculatorParam;
            if (calculatorParam != null)
            {
                calculatorParam.EdgeWidth = (int)this.calculateEdgeWidth.Value;
                calculatorParam.EdgeValue = (int)this.calculateEdgeValue.Value;
                calculatorParam.EdgeFindMethod = (CalculatorParam.EEdgeFindMethod)this.calculateEdgeMethod.SelectedIndex;

                calculatorParam.InBarAlignX = this.inBarAlignX.Checked;
                calculatorParam.InBarAlignY = this.inBarAlignY.Checked;

                calculatorParam.AdaptivePairing = this.adaptivePairing.Checked;
                calculatorParam.BoundaryPairStep = (int)this.boundaryPairStep.Value;
                calculatorParam.IgnoreMethod = (CalculatorParam.EIgnoreMethod)this.ignoreMethod.SelectedIndex;
                calculatorParam.IgnoreSideLine = this.ignoreLastBoundary.Checked;

                calculatorParam.ParallelOperation = this.calcParallelOperation.Checked;

                if (updateFigure)
                    UpdateOverlayFigure();
            }

            DetectorParam detectorParam = this.DetectorParam as DetectorParam;
            if (detectorParam != null)
            {
                detectorParam.MaximumDefectCount = (int)this.defectMaxCount.Value;
                detectorParam.DetectThresholdBase = (int)this.detectThresholdBase.Value;
                detectorParam.FineSizeMeasure = this.fineSizeMeasure.Checked;
                detectorParam.FineSizeMeasureThresholdMul = (float)this.fineSizeMeasureThresholdMul.Value;
                detectorParam.FineSizeMeasureSizeMul = (int)this.fineSizeMeasureSizeMul.Value;
                detectorParam.ParallelOperation = this.detectParallelOperation.Checked;
            }

            AlgorithmSetting algorithmSetting = AlgorithmSetting.Instance();
            algorithmSetting.MinBlackDefectLength = (int)this.electricMinSize.Value;
            algorithmSetting.MinWhiteDefectLength = (int)this.dielectricMinSize.Value;
            //AlgorithmSetting.Instance().Save();
        }

        private void grabSpeedMpM_TextChanged(object sender, EventArgs e)
        {
            if (updataData == true)
                return;
            
            ComboBox comboBox = (ComboBox)sender;
            float spdMpm;
            bool ok = float.TryParse(comboBox.Text, out spdMpm);
            if (ok == false)
                return;

            float spdMps = spdMpm / 60;
            float resUm = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height;
            float kGrabPerSec = spdMps / (resUm / 1000 / 1000) / 1000;
            if (kGrabPerSec > 0)
                grabSpeedkHz.Value = (decimal)kGrabPerSec;
            return;
        }

        private void SetMiscParam()
        {
            if (updataData == true)
                return;
            
            AlgorithmSetting.Instance().InspBufferCount = (int)this.bufferSize.Value;

            bool asyncMode = this.asyncMode.Checked;
            float asyncGrabHz = (float)(grabSpeedkHz.Value * 1000);
            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;
            additionalSettings.UseAsyncMode = asyncMode;
            additionalSettings.AsyncGrabHz = asyncGrabHz;

            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            if (model != null)
            {
                model.ScaleFactor = (int)this.imageRescaler.Value;
                model.Modified = true;
            }

            //float exposureTimeS = 1.0f/((float)grabSpeedkHz.Value * 1000);
            //SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetExposureTime(exposureTimeS*1E6f);
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetAquationLineRate(asyncGrabHz);

            LogLevel selLogLevel = (LogLevel)Enum.Parse(typeof(LogLevel), logLevel.Text);
            UniEye.Base.Settings.OperationSettings.Instance().LogLevel = selLogLevel;
            LogHelper.ChangeLevel(selLogLevel.ToString());

            UniEye.Base.Settings.OperationSettings.Instance().DebugMode = debugMode.Checked;
            UniEye.Base.Settings.OperationSettings.Instance().SaveDebugImage = debugMode.Checked;
        }
        
        private void SetPattern()
        {
            if (updataData == true)
                return;

            List<SheetPattern> tempList = new List<SheetPattern>();

            foreach (DataGridViewRow row in regionSelector.Rows)
            {
                SheetPattern pattern = (SheetPattern)row.Tag;
                tempList.Add(pattern);
            }

            //InspectorParam.ShapeParam.PatternList.Clear();
            //InspectorParam.ShapeParam.PatternList.AddRange(tempList);
        }
        #endregion
        
        delegate void UserChangedDelegatge();
        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegatge(UserChanged));
                return;
            }

            //tabControlParam.Tabs["Misc"].Visible = UserHandler.Instance().CurrentUser.Id == "developer";
            tabControlParam.Tabs["Misc"].Visible = UserHandler.Instance().CurrentUser.SuperAccount;
            UpdateData();
        }

        private void ParamController_ClientSizeChanged(object sender, EventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            UpdateRegionSelector(true);
        }

        public void ModelChanged()
        {
            UpdateData();
        }

        public void ModelTeachDone(int camId)
        {
            UpdateData();
        }
        public void ModelRefreshed() { }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            // Infragistics Tab Controls
            //foreach (string tabKey in Enum.GetNames(typeof(ParamTabKey)))
            //    tabControlParam.Tabs[tabKey].Text = StringManager.GetString(this.GetType().FullName, tabControlParam.Tabs[tabKey].Text);
            foreach (UltraTab ultraTab in tabControlParam.Tabs)
                ultraTab.Text = StringManager.GetString(this.GetType().FullName, ultraTab.Text);

            foreach (UltraTab ultraTab in tabControlTarget.Tabs)
                ultraTab.Text = StringManager.GetString(this.GetType().FullName, ultraTab.Text);

            this.searchDirXBase.Items.Clear();
            Array.ForEach(Enum.GetNames(typeof(BaseXSearchDir)), f => this.searchDirXBase.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            this.groupDirection.Items.Clear();
            Array.ForEach(Enum.GetNames(typeof(Direction)), f => this.groupDirection.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            this.calculateEdgeMethod.Items.Clear();
            Array.ForEach(Enum.GetNames(typeof(CalculatorParam.EEdgeFindMethod)), f => this.calculateEdgeMethod.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            this.ignoreMethod.Items.Clear();
            Array.ForEach(Enum.GetNames(typeof(CalculatorParam.EIgnoreMethod)), f => this.ignoreMethod.Items.Add(StringManager.GetString(this.GetType().FullName, f)));

            UpdateData();
        }

        private void fiducialSearch_ValueChanged(object sender, EventArgs e)
        {
            if (this.updataData)
                return;

            Control control = sender as Control;
            bool update = false;
            if (control != null)
                update = (control.Name == fiducialSearchRangeL.Name || control.Name == fiducialSearchRangeR.Name);

            SetFiducialFinderParam(update);
        }

        private void Train_ValueChanged(object sender, EventArgs e)
        {
            if (this.updataData)
                return;

            SetTrainerParam();
        }

        private void Inspect_ValueChanged(object sender, EventArgs e)
        {
            if (this.updataData)
                return;

            Control control = sender as Control;
            bool update = false;
            if (control != null)
                update = (control.Name == calculateEdgeWidth.Name);

            SetInspectorParam(update);
            UpdateInspectorParamControl();
        }

        private void MiscParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetMiscParam();
            if(sender is NumericUpDown)
            {
                NumericUpDown numericUpDown = (NumericUpDown)sender;
                if (numericUpDown.Name == this.imageRescaler.Name)
                    UpdateOverlayFigure();
            }
            UpdateMiscParamControl();
        }

        private void regionSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (updataData)
                return;

            if (regionSelector.Visible == false)
                return;
            
            UpdateOverlayFigure();
        }

        private void regionSelector_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                RegionInfoG regionInfo = regionSelector.Rows[e.RowIndex].Tag as RegionInfoG;
                ((DataGridViewCheckBoxCell)regionSelector.Rows[e.RowIndex].Cells[e.ColumnIndex]).Value = regionInfo.Use = !regionInfo.Use;
                SystemManager.Instance().CurrentModel.Modified = true;
            }
            else if (e.ColumnIndex == 2)
            {
                if (regionSelector.SelectedRows.Count == 0)
                    return;

                if (e.RowIndex == regionSelector.SelectedRows[0].Index)
                {
                    Model model = SystemManager.Instance().CurrentModel;
                    if (model == null)
                        return;

                    RegionInfoG regionInfo = regionSelector.SelectedRows[0].Tag as RegionInfoG;
                    modellerPageExtender.UpdateZoom(regionInfo.Region);
                    UpdateOverlayFigure();
                }
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            if (regionSelector.SelectedRows.Count == 0)
                return;

            RegionInfoG selRegionInfo = CalculatorParam.RegionInfoList[regionSelector.SelectedRows[0].Index];
            modellerPageExtender.Inspect(selRegionInfo);
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (regionSelector.SelectedRows.Count == 0)
                return;


            int selIndex = regionSelector.SelectedRows[0].Index;
            RegionInfoG selRegionInfo = CalculatorParam.RegionInfoList[selIndex];

            Trainer trainer = AlgorithmPool.Instance().GetAlgorithm(Trainer.TypeName) as Trainer;
            AlgoImage trainImage = ImageBuilder.Build(Trainer.TypeName, this.modellerPageExtender.CurrentImage, ImageType.Grey);
            AlgoImage majorPatternImage = trainer.GetMajorPatternImage(trainImage);

            AlgoImage regionTrainImage = trainImage.Clip(selRegionInfo.Region);
            AlgoImage regionPatternImage = majorPatternImage.Clip(selRegionInfo.Region);

            ImageD regionTrainImageD = regionTrainImage.ToImageD();
            ImageD regionPatternImageD = regionPatternImage.ToImageD();

            RegionEditor regionEditor = new RegionEditor(regionTrainImageD, regionPatternImageD);
            regionEditor.RegionInfo = selRegionInfo;
            regionEditor.SplitLineDirection = TrainerParam.SplitLineDirection;
            regionEditor.Dock = DockStyle.Fill;
            regionEditor.ApplyAll = regionEditor_ApplyAll;

            Form form = new Form();
            form.WindowState = FormWindowState.Maximized;
            form.Controls.Add(regionEditor);
            form.ShowDialog();

            //selRegionInfo.Dispose();
            regionEditor.RegionInfo.BuildInspRegion(regionTrainImageD, regionPatternImageD, TrainerParam.SplitLineDirection);
            CalculatorParam.RegionInfoList[selIndex] = regionEditor.RegionInfo;
            regionSelector.SelectedRows[0].Tag = regionEditor.RegionInfo;
            UpdateRegionSelector(false);
            UpdateOverlayFigure();

            regionTrainImageD.Dispose();
            regionPatternImageD.Dispose();

            regionTrainImage.Dispose();
            regionPatternImage.Dispose();

            trainImage.Dispose();
            majorPatternImage.Dispose();
        }

        private void regionEditor_ApplyAll(RegionInfoG regionInfo)
        {
            Trainer trainer = AlgorithmPool.Instance().GetAlgorithm(Trainer.TypeName) as Trainer;
            AlgoImage trainImage = ImageBuilder.Build(Trainer.TypeName, this.modellerPageExtender.CurrentImage, ImageType.Grey);
            AlgoImage majorPatternImage = trainer.GetMajorPatternImage(trainImage);

            CalculatorParam.RegionInfoList.ForEach(f =>
            {
                f.LinePair = regionInfo.LinePair;
                f.OddEvenPair = regionInfo.OddEvenPair;

                //Point offset = Point.Empty;
                //{
                //    offset.X = f.PatRegionList[0, 0].X - regionInfo.PatRegionList[0, 0].X;
                //    offset.Y = f.PatRegionList[0, 0].Y - regionInfo.PatRegionList[0, 0].Y;
                //}
                //Rectangle srcRect = new Rectangle(Point.Empty, regionInfo.ContourImage.Size);
                //Rectangle dstRect = new Rectangle(Point.Empty, f.ContourImage.Size);
                //dstRect.Offset(offset);
                //dstRect.Intersect(srcRect);
                //srcRect.Offset(Math.Max(-offset.X, 0), Math.Max(-offset.Y, 0));
                //srcRect.Size = dstRect.Size;

                //ImageD contourImage = regionInfo.ContourImage;
                
                //f.ContourImage.Clear();`
                //f.ContourImage.CopyFrom(contourImage, srcRect, contourImage.Pitch, dstRect.Location);

                f.DontcareLocationList.Clear();
                f.DontcareLocationList.AddRange(regionInfo.DontcareLocationList);

                AlgoImage regionTrainImage = trainImage.GetSubImage(f.Region);
                AlgoImage regionPatternImage = majorPatternImage.GetSubImage(f.Region);
                f.BuildInspRegion(regionTrainImage.ToImageD(), regionPatternImage.ToImageD(), TrainerParam.SplitLineDirection);
                regionTrainImage.Dispose();
                regionPatternImage.Dispose();
            });

            trainImage.Dispose();
            majorPatternImage.Dispose();
            //UpdateRegionSelector(true);

        }
        
        private void patternSelector_SelectionChanged(object sender, EventArgs e)
        {
            if (updataData)
                return;

            if (patternSelector.SelectedRows.Count == 0)
                return;

            SheetPatternGroup patternGroup = patternSelector.SelectedRows[0].Tag as SheetPatternGroup;
            UpdateOverlayFigure();
        }

        private void ultraButtonPatternUpdate_Click(object sender, EventArgs e)
        {
            modellerPageExtender.Teach("Pattern");
        }

        private void ultraButtonPatternApply_Click(object sender, EventArgs e)
        {
            modellerPageExtender.Teach("Region");
            tabControlTarget.SelectedTab = tabControlTarget.Tabs["Region"];
        }

        private void patternSelector_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;

            SheetPatternGroup patternGroup = patternSelector.Rows[e.RowIndex].Tag as SheetPatternGroup;
            patternGroup.Use = !patternGroup.Use;
            ((DataGridViewCheckBoxCell)patternSelector.Rows[e.RowIndex].Cells[1]).Value = patternGroup.Use;
        }

        bool isPatternVisible = true;
        private void tabControlTarget_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            bool drawMode = (e.Tab.Index == 2);
            UpdateOverlayFigure();
        }

        private void buttonMonitoringEdit_Click(object sender, EventArgs e)
        {
            Image2D curImage = this.modellerPageExtender.CurrentImage;
            Bitmap curBitmap = curImage?.ToBitmap();
            List<WatchItem> watchItemList = this.WatcherParam?.WatchItemList;
            if (curBitmap == null || watchItemList == null)
                return;

            WatchEditor watchEditor = new WatchEditor();
            watchEditor.Dock = DockStyle.Fill;
            watchEditor.Initialize(curBitmap, watchItemList);
            
            Form form = new Form();
            form.WindowState = FormWindowState.Maximized;
            form.Controls.Add(watchEditor);
            form.ShowDialog();

            if (watchEditor.WatchItemListDone != null)
            {
                this.WatcherParam.WatchItemList.Clear();
                this.WatcherParam.WatchItemList.AddRange(watchEditor.WatchItemListDone);
                SystemManager.Instance().CurrentModel.Modified = true;
                UpdateMonitoringSelector();
                UpdateOverlayFigure();
            }
        }
    }
}
