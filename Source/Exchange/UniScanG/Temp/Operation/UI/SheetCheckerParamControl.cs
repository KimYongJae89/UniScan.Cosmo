//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;
//using DynMvp.Data.Forms;
//using DynMvp.Authentication;
//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.UI;
//using DynMvp.Vision;
//using UniEye.Base.Settings;
//using DynMvp.UI.Touch;

//namespace UniScanG.Temp
//{
//    public partial class SheetCheckerParamControl : UserControl, IAlgorithmParamControl, IUserHandlerListener
//    {
//        //TeachingPageController pageController;
//        List<GravureSheetChecker> selectedList = new List<GravureSheetChecker>();
//        AlgorithmValueChangedDelegate valueChanged = null;
//        ImageD targetGroupImage = null;
//        bool onUpdate = false;

//        public SheetCheckerParamControl()
//        {
//            InitializeComponent();

//            MainForm mainForm = (MainForm)SystemManager.Instance().MainForm;

//            //if (UniScanGSettings.Instance().SystemType == SystemType)
//            {
//                //pageController = mainForm.TeachingPage.PageController;
//                //mainForm.TeachingPage.ParamControl = this;
//                progressBarTrain.Visible = true;
//            }
//            //else
//            {
//                //pageController = mainForm.RemoteTeachingPage.PageController;
//                progressBarTrain.Visible = false;
//            }
                
//            //if (pageController is InspectorTeachingPageController)
//            //    ((InspectorTeachingPageController)pageController).ParamControl = this;

//            UserHandler.Instance().AddListener(this);

//            ChangeLanguage();

//            fidPosition.Items.AddRange(Enum.GetNames(typeof(FiducialPosition)));

//            UserChanged();
//        }

//        private void ChangeLanguage()
//        {
//        }

//        public void AddSelectedProbe(Probe probe)
//        {
//            if(probe is VisionProbe)
//            {
//                VisionProbe visionProbe = (VisionProbe)probe;
//                if(visionProbe.InspAlgorithm is GravureSheetChecker)
//                    selectedList.Add((GravureSheetChecker)visionProbe.InspAlgorithm);
//            }
//            UpdateData();
//        }

//        public void ClearSelectedProbe()
//        {
//            selectedList.Clear();
//            UpdateData();
//        }

//        delegate void UpdateDataDelegate();
//        public void UpdateData()
//        {
//            if (selectedList.Count == 0)
//                return;

//            if (InvokeRequired)
//            {
//                Invoke(new UpdateDataDelegate(UpdateData));
//                return;
//            }

//            onUpdate = true;
//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;

//            sheetWidth.Value = (decimal)(param.SheetSizeMm.Width);
//            sheetHeigth.Value = (decimal)(param.SheetSizeMm.Height);

//            patternDefectArea.Value = param.TrainerParam.BlackDefectMinArea;
//            whiteRegionDefectArea.Value = param.TrainerParam.WhiteDefectMinArea;

//            defectThreshold.Value = param.TrainerParam.DefectThreshold;

//            patternMinArea.Value = param.TrainerParam.MinPatternArea;
//            patternGroupTh.Value = (decimal)param.TrainerParam.PatternGroupThreshold;

//            searchRangeL.Value = Math.Min((decimal)(param.FiducialFinderParam.FidSearchLBound * 100),100);
//            searchRangeR.Value = Math.Min((decimal)(param.FiducialFinderParam.FidSearchRBound * 100),100);

//            fidSizeWidth.Value = param.FiducialFinderParam.FidSizeWidth;
//            fidSizeHeight.Value = param.FiducialFinderParam.FidSizeHeight;

//            fidPosition.Text = param.TrainerParam.FidPosition.ToString();
//            adaptiveFidSearchRange.Checked = param.AdaptiveFidSearchRange;

//            if (param.FiducialFinderParam.FiducialPatternEdgeImage != null)
//                pictureBox1.Image = param.FiducialFinderParam.FiducialPatternEdgeImage.ToBitmap();
//            else
//                pictureBox1.Image = null;

//            onUpdate = false;
//        }

//        public string GetTypeName()
//        {
//            return GravureSheetChecker.TypeName;
//        }

//        public void PointSelected(Point clickPos, ref bool processingCancelled)
//        {
//            throw new NotImplementedException();
//        }

//        public void SetTargetGroupImage(ImageD image)
//        {
//            this.targetGroupImage = image;
//        }

//        public void SetValueChanged(AlgorithmValueChangedDelegate valueChanged)
//        {
//            this.valueChanged = valueChanged;
//        }

//        public void UpdateProbeImage()
//        {
//            throw new NotImplementedException();
//        }

//        public void UserChanged()
//        {
            
//        }

//        public void TrainFailMessage()
//        {
//            MessageForm.Show(ParentForm, String.Format("Please image grab."), "Train Fail");
//        }

//        public void UpdateTeachProgress(ProgressType type)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new TeachProgressUpdate(UpdateTeachProgress), type);
//                return;
//            }

//            switch (type)
//            {
//                case ProgressType.Start:
//                    progressBarTrain.Value = 0;
//                    break;
//                case ProgressType.Average:
//                    progressBarTrain.Value = 10;
//                    break;
//                case ProgressType.FindPattern:
//                    progressBarTrain.Value = 20;
//                    break;
//                case ProgressType.CreateMask:
//                    progressBarTrain.Value = 60;
//                    break;
//                case ProgressType.End:
//                    progressBarTrain.Value = 100;
//                    UpdateData();
//                    break;
//            }
            
//            progressBarTrain.Invalidate();
//        }

//        private void patternMinArea_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - patternMinArea_ValueChanged");

//            if (selectedList.Count == 0)
//                return;

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.TrainerParam.MinPatternArea = (int)(patternMinArea.Value);
//            param.TrainerParam.Traind = false;
//            //pageController.SetTaughtLabel(false);
            
//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.ImageProcessing, selectedList[0], param, true);
//        }

//        public bool IsTaught()
//        {
//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            return param.TrainerParam.Traind;
//        }
        
//        private void patternDefectArea_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - patternDefectArea_ValueChanged");

//            if (selectedList.Count == 0)
//                return;

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.TrainerParam.BlackDefectMinArea = (int)(patternDefectArea.Value);

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.ImageProcessing, selectedList[0], param, true);
//        }

//        private void whiteRegionDefectArea_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - whiteRegionDefectArea_ValueChanged");

//            if (selectedList.Count == 0)
//                return;

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.TrainerParam.WhiteDefectMinArea = (int)(whiteRegionDefectArea.Value);

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.ImageProcessing, selectedList[0], param, true);
//        }

//        private void searchRangeWidth_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - searchRangeWidth_ValueChanged");

//            if (selectedList.Count == 0)
//                return;

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.FiducialFinderParam.FidSearchLBound = (int)(searchRangeL.Value);

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.ImageProcessing, selectedList[0], param, true);
//        }

//        private void fidSizeWidth_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - fidSizeWidth_ValueChanged");

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.FiducialFinderParam.FidSizeWidth = (int)(fidSizeWidth.Value);

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.None, selectedList[0], param, true);
//        }

//        private void fidSizeHeight_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - fidSizeHeight_ValueChanged");

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.FiducialFinderParam.FidSizeHeight = (int)(fidSizeHeight.Value);

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.None, selectedList[0], param, true);
//        }

//        private void searchRangeL_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - searchRangeL_ValueChanged");

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.FiducialFinderParam.FidSearchLBound = (float)searchRangeL.Value / 100.0f;

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.Position, selectedList[0], param, true);
//        }

//        private void searchRangeR_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - searchRangeR_ValueChanged");

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.FiducialFinderParam.FidSearchRBound = (float)searchRangeR.Value / 100.0f;

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.Position, selectedList[0], param, true);
//        }

//        private void patternGroupTh_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - patternGroupTh_ValueChanged");

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.TrainerParam.PatternGroupThreshold = (int)(patternGroupTh.Value);

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.ImageProcessing, selectedList[0], param, true);
//        }

//        private void fidPosition_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - fidPosition_SelectedIndexChanged");

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.TrainerParam.FidPosition = (FiducialPosition)Enum.Parse(typeof(FiducialPosition), fidPosition.Items[fidPosition.SelectedIndex].ToString());

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.ImageProcessing, selectedList[0], param, true);
//        }

//        private void adaptiveFidSearchRange_CheckedChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - adaptiveFidSearchRange_CheckedChanged");

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.AdaptiveFidSearchRange = adaptiveFidSearchRange.Checked;

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.None, selectedList[0], param, true);
//        }

//        private void buttonFidTest_Click(object sender, EventArgs e)
//        {
//            GravureSheetChecker sheetChecker = selectedList[0];
//            AlgorithmResult algorithmResult = null;
//            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
//            sw.Start();

//            SimpleProgressForm form = new SimpleProgressForm();
//            form.Show(() =>
//            {
//                AlgoImage algoImage = ImageBuilder.Build(this.GetTypeName(), targetGroupImage, ImageType.Gpu);
//                AlgoImage patternImage = null;
//                ImageD patternImageD = (sheetChecker.Param as SheetCheckerParam).FiducialFinderParam.FiducialPatternEdgeImage;
//                if (patternImageD != null)
//                    patternImage = ImageBuilder.Build(this.GetTypeName(), patternImageD, ImageType.Gpu);

//                DebugContext debugContext = new DebugContext(true, PathSettings.Instance().Temp);
//                FiducialFinderInspectParam fiducialFinderInspectParam = new FiducialFinderInspectParam(algoImage, false,
//                    new AlgorithmInspectParam(null, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, debugContext));
//                FiducialFinder sheetCheckerFiducial = new FiducialFinder(sheetChecker.Param);
//                algorithmResult = sheetCheckerFiducial.Inspect(fiducialFinderInspectParam);
//                sheetCheckerFiducial.Dispose();
//                //algorithmResult = sheetChecker.FindFiducialGravua(algoImage, patternImage, patternImage==null, 0, debugContext);
//                algoImage.Dispose();
//                patternImage?.Dispose();
//            });

//            sw.Stop();
//            StringBuilder sb = new StringBuilder();
//            if (algorithmResult == null)
//            {
//                sb.AppendLine(string.Format("Can not Found Fiducial"));
//            }
//            else
//            {
//                List<AlgorithmResultValue> itemList = algorithmResult.GetResultValues("FidRect");
//                sb.AppendLine(string.Format("Founded Fiducial(s): {0}. {1}[ms]", itemList.Count, sw.ElapsedMilliseconds));
//                itemList.ForEach(f =>
//                {
//                    Rectangle fidRect = (Rectangle)f.Value;
//                    sb.AppendLine(string.Format("{0}", fidRect.ToString()));
//                });
//            }
//            MessageBox.Show(sb.ToString());

//            this.UpdateData();
//        }

//        private void pictureBox1_DoubleClick(object sender, EventArgs e)
//        {
//            if(MessageForm.Show(null, "Do you want Remove the Trained Image?", MessageFormType.YesNo) == DialogResult.Yes)
//            {
//                SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//                if (param.FiducialFinderParam.FiducialPatternEdgeImage != null)
//                    param.FiducialFinderParam.FiducialPatternEdgeImage.Dispose();
//                param.FiducialFinderParam.FiducialPatternEdgeImage = null;

//                UpdateData();
//            }
//        }

//        private void sheetWidth_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - sheetWidth_ValueChanged");

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            SizeF sheetSizeMm = param.SheetSizeMm;
//            sheetSizeMm.Width = (float)this.sheetWidth.Value;
//            param.SheetSizeMm = sheetSizeMm;

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.None, selectedList[0], param, true);
//        }

//        private void sheetHeigth_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - sheetHeigth_ValueChanged");

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            SizeF sheetSizeMm = param.SheetSizeMm;
//            sheetSizeMm.Height = (float)this.sheetHeigth.Value;
//            param.SheetSizeMm = sheetSizeMm;

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.None, selectedList[0], param, true);
//        }

//        private void defectThreshold_ValueChanged(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamGravureControl - adaptiveFidSearchRange_CheckedChanged");

//            if (onUpdate)
//                return;

//            SheetCheckerParam param = (SheetCheckerParam)selectedList[0].Param;
//            param.TrainerParam.DefectThreshold = (int)defectThreshold.Value;

//            if (this.valueChanged != null)
//                this.valueChanged(ValueChangedType.None, selectedList[0], param, true);
//        }
//    }
//}
