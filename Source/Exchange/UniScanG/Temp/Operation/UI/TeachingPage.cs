//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;
//using System.IO;

//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.Data.UI;
//using DynMvp.Vision;
//using DynMvp.UI;
//using DynMvp.Data.Forms;
//using System.Threading;
//using DynMvp.Devices.MotionController;
//using DynMvp.Device.UI;
//using UniEye.Base.UI;
//using DynMvp.Devices;

//using System.Drawing.Imaging;
//using DynMvp.UI.Touch;
//using UniEye.Base.Settings;
//using System.Timers;
//using System.Text;
//using DynMvp.Authentication;
//using UniEye.Base.UI.ParamControl;

//namespace UniScanG.Temp
//{
//    public partial class TeachingPage : UserControl, IMainTabPage, IUserHandlerListener
//    {
//        //CommandManager commandManager = new CommandManager();

//        bool onMaskingMode = false;
//        SheetRange dummyRange = null;
//        bool onEndPos = false;
//        string imagePath;
//        public string ImagePath
//        {
//            get { return imagePath; }
//            set { imagePath = value; }
//        }

//        private TeachBox teachBox;
//        public TeachBox TeachBox
//        {
//            get { return teachBox; }
//        }

//        public TeachHandlerProbe teachHandlerProbe;

//        private TargetParamControl targetParamControl = null;
//        public TeachControl teachControl = null;

//        private TryInspectionResultView2 tryInspectionResultView;
//        private ILightParamForm lightParamForm;

//        //public Data.InspectionResult modellerInspectionResult = new Data.InspectionResult();
//        private DynMvp.InspData.InspectionResult lastInspectionResult = null;
//        private const int padding = 3;

//        public ValueChangedDelegate ValueChanged = null;

//        bool modified = false;
//        private bool lockGrab = true;
//        bool onValueUpdate = false;

//        Image2D sourceImage2d;
//        public Image2D SourceImage2d
//        {
//            get { return sourceImage2d; }
//            set { sourceImage2d = value; }
//        }

//        System.Timers.Timer grabTimer;
//        DefectImageViewer defectImageViewer = new DefectImageViewer();

//        int deviceIndex = 0;
//        int previewIndex = 0;

//        TargetGroup curTargetGroup;

//        bool onPreviewMode = false;
//        bool onLiveGrab = false;

//        private int offlineImageIndex = 0;
//        private string[] offlineImagePathList = null;

//        //int sequenceImageNum;

//        ContextMenu objectContextMenu = new ContextMenu();

//        //ModellerPageExtender modellerPageExtender;
//        public ModellerPageExtender ModellerPageExtender
//        {
//            //get { return modellerPageExtender; }
//        }

//        JoystickAxisForm joystick;
//        private AxisPosition currentPosition = new AxisPosition();

//        CancellationTokenSource cancellationTokenSource;

//        List<System.Diagnostics.Process> camVncProcess = new List<System.Diagnostics.Process>();

//        bool onWaitGrab = false;
//        bool bShowAll = true;

//        List<ImageD> sequenceImages = new List<ImageD>();

//        public TeachingPage()
//        {
//            InitializeComponent();

//            this.tryInspectionResultView = new TryInspectionResultView2();
//            this.teachBox = new TeachBox();
//            this.targetParamControl = new TargetParamControl();

//            teachHandlerProbe = new TeachHandlerProbe();

//            this.SuspendLayout();

//            cameraImagePanel.Controls.Add(this.teachBox);

//            this.teachBox.TeachHandler = this.teachHandlerProbe;
//            this.teachBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.teachBox.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.teachBox.Location = new System.Drawing.Point(3, 3);
//            this.teachBox.Name = "targetImage";
//            this.teachBox.Size = new System.Drawing.Size(409, 523);
//            this.teachBox.TabIndex = 8;
//            this.teachBox.TabStop = false;
//            this.teachBox.Enable = true;
//            this.teachBox.RotationLocked = false;
//            this.teachBox.ContextMenu = objectContextMenu;
//            this.teachBox.InspectionResultSelected = modellerInspectionResult;
//            this.teachBox.DrawBox.Enable = false;
//            //this.teachBox.MouseClicked += teachBox_MouseClicked;
//            this.teachBox.MouseDoubleClicked += teachBox_MouseDoubleClicked;

//            this.tabPageDefectList.Controls.Add(this.tryInspectionResultView);

//            this.tryInspectionResultView.Location = new System.Drawing.Point(96, 95);
//            this.tryInspectionResultView.Name = "tryInspectionResultView";
//            this.tryInspectionResultView.Size = new System.Drawing.Size(74, 101);
//            this.tryInspectionResultView.TabIndex = 0;
//            this.tryInspectionResultView.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tryInspectionResultView.TeachHandlerProbe = teachHandlerProbe;

//            this.splitContainer.Panel2.Controls.Add(this.targetParamControl);
//            // 
//            // chargerTargetParamControl
//            // 
//            this.targetParamControl.Location = new System.Drawing.Point(96, 95);
//            this.targetParamControl.Name = "targetParamControl";
//            this.targetParamControl.Size = new System.Drawing.Size(74, 101);
//            this.targetParamControl.TabIndex = 0;
//            this.targetParamControl.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.targetParamControl.TeachHandlerProbe = teachHandlerProbe;
//            this.targetParamControl.ValueChanged = new ValueChangedDelegate(ParamControl_ValueChanged);
//            this.targetParamControl.CommandManager = commandManager;
//            this.targetParamControl.VisionParamControl.TabVisibleChange(false, false, true);
//            this.ResumeLayout(false);

//            // change language
//            //deleteProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,deleteProbeToolStripButton.Text);
//            fiducialGrabProcessToolStripButton.Text = StringManager.GetString(this.GetType().FullName,fiducialGrabProcessToolStripButton.Text);
//            loadImageSetToolStripButton.Text = StringManager.GetString(this.GetType().FullName,loadImageSetToolStripButton.Text);

//            fiducialGrabProcessToolStripButton.ToolTipText = StringManager.GetString(this.GetType().FullName,fiducialGrabProcessToolStripButton.ToolTipText);

//            // Camera
//            fiducialGrabProcessToolStripButton.Text = StringManager.GetString(this.GetType().FullName,fiducialGrabProcessToolStripButton.Text);
//            loadImageSetToolStripButton.Text = StringManager.GetString(this.GetType().FullName,loadImageSetToolStripButton.Text);

//            // Probe
//            //deleteProbeToolStripButton.Text = StringManager.GetString(this.GetType().FullName,deleteProbeToolStripButton.Text);

//            teachHandlerProbe.SingleTargetMode = true;

//            modellerPageExtender = new UniScanGModellerPageExtender(this);
//            //modellerPageExtender.UpdateImage += GravureUpdateImage;

//            teachControl = new TeachControl(teachHandlerProbe, teachBox, null);

//            if (teachControl != null)
//            {
//                ((UserControl)teachControl).Dock = DockStyle.Fill;
//                teachControlPanel.Controls.Add((UserControl)teachControl);
//            }

//            defectList.RowTemplate.Height = defectList.Height / 5;

//            teachBox.DrawBox.MouseMoved += drawBox_MouseMoved;
//            teachBox.DrawBox.pictureBox.MouseLeave += PictureBox_MouseLeaveClient;
//            teachBox.DrawBox.pictureBox.KeyUp += KeyUp_KeyUped;

//            UserHandler.Instance().AddListener(this);
//            UserChanged();
//        }

//        private void KeyUp_KeyUped(object sender, KeyEventArgs e)
//        {
//            if (onMaskingMode == true)
//            {
//                if (e.KeyData == Keys.Escape)
//                {
//                    onMaskingMode = false;
//                    onEndPos = false;

//                    FigureGroup figureGroup = new FigureGroup();

//                    VisionProbe curProbe = (VisionProbe)teachHandlerProbe.GetSelectedProbe()[0];
//                    GravureSheetChecker sheetChecker = (GravureSheetChecker)curProbe.InspAlgorithm;

//                    SheetCheckerParam param = (SheetCheckerParam)sheetChecker.Param;

//                    teachBox.DrawBox.TempFigureGroup.Clear();
//                    teachBox.DrawBox.BackgroundFigures.Clear();
//                    teachBox.DrawBox.FigureGroup.Clear();

//                    AddDummyRangeFigure();

//                    teachBox.DrawBox.Invalidate();
//                    teachBox.DrawBox.Update();
//                }
//            }
//        }

//        private void PictureBox_MouseLeaveClient(object sender, EventArgs e)
//        {
//            defectImageViewer.Hide();
//        }

//        public void MaskSetting(SheetRange dummyRange, bool onMaskingMode)
//        {
//            FigureGroup figureGroup = new FigureGroup();

//            this.onMaskingMode = onMaskingMode;
//            this.dummyRange = dummyRange;

//            if (onMaskingMode == true)
//            {
//                TextFigure textFigure = new TextFigure("Masking Mode...", new Point(100, 100), new Font("Arial", 700), Color.LightGreen);
//                textFigure.Alignment = StringAlignment.Near;
//                figureGroup.AddFigure(textFigure);
//            }
//            else
//            {
//                onEndPos = false;
//            }

//            teachBox.DrawBox.BackgroundFigures = figureGroup;
//            teachBox.DrawBox.Invalidate(true);
//            teachBox.DrawBox.Update();
//        }

//        private void drawBox_MouseMoved(DrawBox senderView, Point movedPos, Image image, MouseEventArgs e, ref bool processingCancelled)
//        {
//            if (image == null)
//                return;

//            Bitmap original = (Bitmap)image;
//            if (original.Width <= movedPos.X || original.Height <= movedPos.Y || 0 > movedPos.X || 0 > movedPos.Y)
//                return;

//            int value = (original.GetPixel(movedPos.X, movedPos.Y).R
//                + original.GetPixel(movedPos.X, movedPos.Y).G
//                + original.GetPixel(movedPos.X, movedPos.Y).B) / 3;

//            double xPosP = movedPos.X * 100.0 / image.Width;
//            double yPosP = movedPos.Y * 100.0 / image.Height;
//            xPos.Text = string.Format("{0}({1:0.0}%)", movedPos.X, xPosP);
//            yPos.Text = string.Format("{0}({1:0.0}%)", movedPos.Y, yPosP);
//            greyValue.Text = value.ToString();

//            //teachBox.DrawCrossLine(e.Location);

//            if (onMaskingMode == true)
//            {
//                FigureGroup figureGroup = new FigureGroup();

//                RectangleF rect = new RectangleF();

//                switch (dummyRange.Type)
//                {
//                    case RangeType.Left:
//                        dummyRange.EndPos = movedPos.X;
//                        rect = new RectangleF(0, 0, dummyRange.EndPos, image.Height);
//                        break;
//                    case RangeType.Right:
//                        dummyRange.StartPos = movedPos.X;
//                        rect = new RectangleF(dummyRange.StartPos, 0, dummyRange.EndPos - dummyRange.StartPos, image.Height);
//                        break;
//                    case RangeType.Middle:
//                        dummyRange.StartPos = movedPos.Y;
//                        rect = new RectangleF(0, dummyRange.StartPos, 0, dummyRange.EndPos - dummyRange.StartPos);
//                        break;
//                    case RangeType.Vertical:
//                        if (onEndPos == false)
//                        {
//                            dummyRange.StartPos = movedPos.X;
//                            dummyRange.EndPos = movedPos.X + 1;
//                        }
//                        else
//                        {
//                            dummyRange.EndPos = movedPos.X;
//                        }
//                        rect = new RectangleF(dummyRange.StartPos, 0, dummyRange.EndPos - dummyRange.StartPos, image.Height);
//                        break;
//                    case RangeType.Horizontal:
//                        if (onEndPos == false)
//                        {
//                            dummyRange.StartPos = movedPos.Y;
//                            dummyRange.EndPos = movedPos.Y + 1;
//                        }
//                        else
//                        {
//                            dummyRange.EndPos = movedPos.Y;
//                        }
//                        rect = new RectangleF(0, dummyRange.StartPos, image.Width, dummyRange.EndPos - dummyRange.StartPos);
//                        break;
//                }

//                RectangleFigure rectangleFigure = new RectangleFigure(rect, new Pen(Color.White, 1), Brushes.White);
//                figureGroup.AddFigure(rectangleFigure);
//                teachBox.DrawBox.FigureGroup.Clear();
//                teachBox.DrawBox.FigureGroup = figureGroup;
//                teachBox.DrawBox.Invalidate(true);
//                teachBox.DrawBox.Update();
//                return;
//            }

//            if (defectList.Rows.Count == 0)
//                return;

//            const float validBound = 500;
//            float minDistance = float.MaxValue;
//            int minIndex = 0;
//            int index = -1;
//            SheetCheckerSubResult minDistanceSheetCheckerSubResult = null;
//            foreach (DataGridViewRow dataGridViewRow in defectList.Rows)
//            {
//                SheetCheckerSubResult sheetCheckerSubResult = (SheetCheckerSubResult)(dataGridViewRow.Tag);
//                PointF centerPt = DrawingHelper.CenterPoint(sheetCheckerSubResult.ResultRect);
//                //PointF centerPt = new PointF(sheetCheckerSubResult.ResultRect.DefectBlob.CenterPt.X, sheetCheckerSubResult.DefectBlob.CenterPt.Y);

//                float lenght = MathHelper.GetLength(centerPt, movedPos);
//                if (lenght < minDistance)
//                {
//                    minDistance = lenght;
//                    minIndex = dataGridViewRow.Index;
//                    minDistanceSheetCheckerSubResult = sheetCheckerSubResult;
//                }

//                index++;
//            }

//            if (minDistance < validBound)
//            {
//                Point screenPoint = teachBox.DrawBox.pictureBox.PointToScreen(new Point(e.X, e.Y));

//                Point location = new Point(screenPoint.X + cameraImagePanel.Location.X + mainContainer.Location.X + 10,
//                    screenPoint.Y + 10);

//                defectImageViewer.Location = location;// PointToScreen(new Point(e.X + cameraImagePanel.Location.X, e.Y + cameraImagePanel.Location.Y));
//                defectImageViewer.Show();
//                defectImageViewer.UpdateDefectInfo(minDistanceSheetCheckerSubResult.Image, (int)minDistanceSheetCheckerSubResult.X,
//                        (int)minDistanceSheetCheckerSubResult.Y, (int)minDistanceSheetCheckerSubResult.Width,
//                        (int)minDistanceSheetCheckerSubResult.Height);
//            }
//            else
//            {
//                defectImageViewer.Hide();
//            }
//        }

//        delegate void ParamControl_ValueChangedDelegate(ValueChangedType valueChangedType, bool modified);
//        private void ParamControl_ValueChanged(ValueChangedType valueChangedType, bool modified)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new ParamControl_ValueChangedDelegate(ParamControl_ValueChanged), valueChangedType, modified);
//                return;
//            }

//            AddDummyRangeFigure();

//            if (modified == true && sourceImage2d != null)
//            {
//                AddFiducialRegionFigure();
//            }

//            if (valueChangedType == ValueChangedType.Position)
//                teachBox.DrawBox.Invalidate(true);
//            //teachBox.DrawBox.Update();
//        }

//        public void AddFiducialRegionFigure()
//        {
//            VisionProbe curProbe = (VisionProbe)teachHandlerProbe.GetSelectedProbe()[0];
//            GravureSheetChecker sheetChecker = (GravureSheetChecker)curProbe.InspAlgorithm;
//            SheetCheckerParam param = (SheetCheckerParam)sheetChecker.Param;

//            int posL = (int)Math.Round(param.FiducialFinderParam.FidSearchLBound * teachBox.Image.Width);
//            int posR = (int)Math.Round(param.FiducialFinderParam.FidSearchRBound * teachBox.Image.Width);

//            LineFigure lineFigureL = (LineFigure)teachBox.DrawBox.FigureGroup.GetFigureByTag("lineFigureL");
//            if (lineFigureL == null)
//            {
//                lineFigureL = new LineFigure(Point.Empty, Point.Empty, new Pen(Color.Green, 3));
//                lineFigureL.Tag = "lineFigureL";
//                teachBox.DrawBox.FigureGroup.AddFigure(lineFigureL);
//            }
//            lineFigureL.StartPoint = new PointF(posL, 0);
//            lineFigureL.EndPoint = new PointF(posL, sourceImage2d.Height);

//            LineFigure lineFigureR = (LineFigure)teachBox.DrawBox.FigureGroup.GetFigureByTag("lineFigureR");
//            if (lineFigureR == null)
//            {
//                lineFigureR = new LineFigure(Point.Empty, Point.Empty, new Pen(Color.Green, 3));
//                lineFigureR.Tag = "lineFigureR";
//                teachBox.DrawBox.FigureGroup.AddFigure(lineFigureR);
//            }
//            lineFigureR.StartPoint = new PointF(posR, 0);
//            lineFigureR.EndPoint = new PointF(posR, sourceImage2d.Height);
//        }

//        public void AddDummyRangeFigure(List<int> indexList = null)
//        {
//            //teachBox.DrawBox.BackgroundFigures.Clear();

//            /*VisionProbe curProbe = (VisionProbe)teachHandlerProbe.GetSelectedProbe()[0];
//            SheetChecker sheetChecker = (SheetChecker)curProbe.InspAlgorithm;

//            SheetCheckerParam param = (SheetCheckerParam)sheetChecker.Param;
            
//            if (teachBox.DrawBox.Image == null)
//                return;

//            int imageWidth = teachBox.DrawBox.Image.Width;
//            int imageHeight = teachBox.DrawBox.Image.Height;

//            FigureGroup figureGroup = teachBox.DrawBox.BackgroundFigures;

//            if (indexList == null || indexList.Count == 0)
//            {
//                foreach (DummyRange dummyRange in param.DummyRangeList)
//                {
//                    switch (dummyRange.Type)
//                    {
//                        case MaskingType.Middle:
//                        case MaskingType.Horizontal:
//                            figureGroup.AddFigure(new RectangleFigure(new Rectangle(0, dummyRange.StartPos, imageWidth, dummyRange.EndPos - dummyRange.StartPos), new Pen(Color.Green), Brushes.Green));
//                            break;
//                        case MaskingType.Left:
//                        case MaskingType.Right:
//                        case MaskingType.Vertical:
//                            figureGroup.AddFigure(new RectangleFigure(new Rectangle(dummyRange.StartPos, 0, dummyRange.EndPos - dummyRange.StartPos, imageHeight), new Pen(Color.Green), Brushes.Green));
//                            break;
//                    }
//                }
//            }
//            else
//            {
//                foreach (int index in indexList)
//                {
//                    DummyRange tempDummyRange = param.DummyRangeList[index];

//                    switch (tempDummyRange.Type)
//                    {
//                        case MaskingType.Middle:
//                        case MaskingType.Horizontal:
//                            figureGroup.AddFigure(new RectangleFigure(new Rectangle(0, tempDummyRange.StartPos, imageWidth, tempDummyRange.EndPos - tempDummyRange.StartPos), new Pen(Color.Green), Brushes.Green));
//                            break;
//                        case MaskingType.Left:
//                        case MaskingType.Right:
//                        case MaskingType.Vertical:
//                            figureGroup.AddFigure(new RectangleFigure(new Rectangle(tempDummyRange.StartPos, 0, tempDummyRange.EndPos - tempDummyRange.StartPos, imageHeight), new Pen(Color.Green), Brushes.Green));
//                            break;
//                    }
//                }
//            }*/
//        }

//        private void teachBox_MouseDoubleClicked(DrawBox senderView)
//        {
//            teachBox.DrawBox.FigureGroup.Clear();
//            teachBox.DrawBox.TempFigureGroup.Clear();

//            teachBox.DrawBox.Invalidate();
//        }

//        //private void GravureUpdateImage(ImageD grabImage = null)
//        //{
//        //    grabTimer?.Stop();
//        //    grabTimer?.Dispose();

//        //    teachBox.DrawBox.FigureGroup.Clear();

//        //    if (grabImage == null)
//        //        return;

//        //    Size imageSize = grabImage.Size;
//        //    RotatedRect inspRegion = new RotatedRect(0, 0, imageSize.Width, imageSize.Height, 0);

//        //    Operation.Data.Model curModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;
//        //    InspectionStep inspectionStep = curModel.GetInspectionStep(UniScanGSettings.Instance().InspectorInfo.CamIndex);
//        //    TargetGroup curTargetGroup = inspectionStep.GetTargetGroup(0);
//        //    Target curTarget = curTargetGroup.GetTarget(1);
//        //    curTarget.BaseRegion = inspRegion;
//        //    curTarget.AlignedRegion = inspRegion;

//        //    VisionProbe curProbe = (VisionProbe)curTarget.GetProbe(1);
//        //    curProbe.BaseRegion = inspRegion;
//        //    curProbe.AlignedRegion = inspRegion;

//        //    ImageD showImage = grabImage;
//        //    SheetCheckerParam param = curProbe.InspAlgorithm.Param as SheetCheckerParam;
//        //    if (param.TrainerParam.InspectRegionInfoImage != null)
//        //    {
//        //        showImage = MakeOverlayImage(grabImage, param.TrainerParam.InspectRegionInfoImage);
//        //    }

//        //    UpdateImage(showImage);
//        //    teachBox.DrawBox.ZoomFit();

//        //    if (cancellationTokenSource != null)
//        //        cancellationTokenSource.Cancel();
//        //}

//        private ImageD MakeOverlayImage(ImageD grabImage, ImageD projectionImage)
//        {
//            return grabImage;
//            //AlgoImage grabAlgoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, grabImage, ImageType.Color);
//            //AlgoImage projAlgoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, projectionImage, ImageType.Color);

//            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, grabImage, ImageType.Color);
//            AlgoImage maskImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, projectionImage, ImageType.Color);
//            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
//            imageProcessing.Clear(algoImage, maskImage, Color.Red);
//            ImageD image = algoImage.ToImageD();
//            algoImage.Dispose();
//            algoImage.Dispose();
//            algoImage.Dispose();
//            return image;
//        }

//        public void CommonUpdateImage(ImageD grabImage = null)
//        {
//            grabTimer.Stop();
//            grabTimer.Dispose();

//            UpdateImage(grabImage);
//            teachBox.DrawBox.ZoomFit();

//            cancellationTokenSource.Cancel();
//        }

//        private void UpdateImage(ImageD grabImage = null)
//        {
//            if (InvokeRequired)
//            {
//                LogHelper.Debug(LoggerType.Operation, "Invoke UpdateImage");

//                Invoke(new UpdateImageDelegate(UpdateImage), grabImage);
//                return;
//            }

//            LogHelper.Debug(LoggerType.Operation, "ModellerPage - UpdateImage 1");

//            if (grabImage != null)
//            {
//                ((Image2D)grabImage).ConvertFromDataPtr();
//                SourceImage2d = (Image2D)grabImage.Clone();

//                //grabImage.FreeImageData();
//            }
//            else
//            {
//                if (SourceImage2d == null)
//                    return;

//                if (SourceImage2d.Width == 0 || SourceImage2d.Height == 0)
//                    return;
//            }

//            onWaitGrab = false;
//            teachBox.DrawBox.TempFigureGroup = new FigureGroup();

//            LogHelper.Debug(LoggerType.Operation, "ModellerPage - UpdateImage 2");

//            if (onPreviewMode)
//            {
//                ImageD cloneImage = SourceImage2d.Clone();

//                List<Probe> probeList = teachHandlerProbe.GetSelectedProbe();
//                foreach (Probe probe in probeList)
//                {
//                    if (probe != null && (cloneImage is Image2D) && onPreviewMode)
//                    {
//                        cloneImage = probe.PreviewFilterResult(cloneImage, previewIndex, false);
//                    }
//                }

//                teachBox.UpdateImage(cloneImage);
//            }
//            else
//            {
//                teachBox.UpdateImage(SourceImage2d);
//            }

//            AddFiducialRegionFigure();
//            targetParamControl.UpdateTargetGroupImage(SourceImage2d, 0);

//            fiducialGrabProcessToolStripButton.BackColor = Color.Transparent;

//            //if (UniScanGSettings.Instance().SystemType == SystemType.Inspector)
//            //{
//            //    string imagePath = Path.Combine("Model", "Image");
//            //    ((MonitoringClient)(SystemManager.Instance().MachineIf)).SendGrabDone(imagePath);
//            //}
//        }

//        //delegate void GrabDoneDelegate(int camIndex, int clientIndex, string imagePath);
//        //public void GrabDone(int camIndex, int clientIndex, string imagePath)
//        //{
//        //    if (InvokeRequired)
//        //    {
//        //        Invoke(new GrabDoneDelegate(GrabDone), camIndex, clientIndex, imagePath);
//        //        return;
//        //    }

//        //    string rcImagePath = null;
//        //    foreach (InspectorInfo info in UniScanGSettings.Instance().ClientInfoList)
//        //    {
//        //        rcImagePath = Path.Combine(info.Path, imagePath);
//        //    }

//        //    string resultImagePath = Path.Combine(rcImagePath, "GrabImage.jpg");

//        //    Bitmap bitmap = (Bitmap)ImageHelper.LoadImage(rcImagePath);
//        //    if (bitmap != null)
//        //    {
//        //        ImageD imageD = Image2D.ToImage2D(bitmap);
//        //        UpdateImage(imageD);
//        //    }
//        //}

//        public void Scan()
//        {
//            throw new NotImplementedException();
//        }

//        //private Image2D LoadImage(Operation.Data.Model model, int stepIndex, int lightIndex)
//        //{
//        //    string searchImageFileName = String.Format("Image_C{0:00}_S{1:000}_L{2:00}.*", deviceIndex, stepIndex, lightIndex);

//        //    string[] imageFiles = null;
//        //    imagePath = String.Format(model.ModelPath + "\\Image");//String.Format("D:\\Project\\MPIS\\UniEye\\Runtime\\bin");

//        //    if (String.IsNullOrEmpty(imagePath) == false && Directory.Exists(imagePath) == true)
//        //    {
//        //        imageFiles = Directory.GetFiles(imagePath, searchImageFileName);
//        //    }

//        //    if (imageFiles != null && imageFiles.Count() == 0)
//        //    {
//        //        string modelImgaePath = string.Format(@"{0}\Image", SystemManager.Instance().CurrentModel.ModelPath);
//        //        if (Directory.Exists(modelImgaePath))
//        //            imageFiles = Directory.GetFiles(modelImgaePath, searchImageFileName);
//        //    }

//        //    if (imageFiles != null && imageFiles.Count() > 0)
//        //    {
//        //        string imageFilePath = Path.Combine(Path.GetFullPath(imagePath), Path.GetFileName(imageFiles[0]));

//        //        Image2D fileImage = new Image2D(imageFilePath);
//        //        //if (imageDevice.IsCompatibleImage(fileImage) == true)
//        //        {
//        //            //    SourceImage2d = fileImage;
//        //            //    teachBox.UpdateImage(SourceImage2d);
//        //        }
//        //        return fileImage;
//        //    }
//        //    else
//        //    {
//        //        //teachBox.UpdateImage(null);
//        //        //sourceImage2d.Clear();
//        //        return null;
//        //    }
//        //}

//        private void loadImageSetToolStripButton_Click(object sender, EventArgs e)
//        {
//            OpenFileDialog dlg = new OpenFileDialog();
//            if (dlg.ShowDialog() == DialogResult.OK)
//            {
//                //Bitmap bitmap = (Bitmap)ImageHelper.LoadImage(dlg.FileName);

//                SourceImage2d = new Image2D(dlg.FileName);// Image2D.ToImage2D(bitmap);
//                //SourceImage2d.ConverFromDataPtr();
//                //bitmap.Dispose();

//                if (SourceImage2d != null)
//                {
//                    //AddFiducialRegionFigure();
//                    teachBox.UpdateImage(sourceImage2d);
//                    targetParamControl.UpdateTargetGroupImage(SourceImage2d, 0);
//                }
//            }

//            //MPIS.Data.Model curModel = (MPIS.Data.Model)SystemManager.Instance().CurrentModel;
//            //if (curModel != null)
//            //{
//            //    SourceImage2d = LoadImage(curModel);

//            //    if (SourceImage2d != null)
//            //    {
//            //        teachBox.UpdateImage(sourceImage2d);
//            //        targetParamControl.UpdateTargetGroupImage(SourceImage2d, 0);
//            //    }
//            //}

//#if DEBUG
//            //string saveDebugImagePath = string.Format("{0}\\{1}.bmp", Configuration.TempFolder, "Load2dImage");
//            //sourceImage2d.SaveImage(saveDebugImagePath, ImageFormat.Bmp);
//#endif
//            return;
//        }

//        private void inspectionToolStripButton_Click(object sender, EventArgs e)
//        {
//            if (MessageForm.Show(null, "Test Inspection?", MessageFormType.YesNo) == DialogResult.Yes)
//                Inspect();
//        }

//        private delegate void ChangeToolStripButtonBackColorDelegate(ToolStripButton button, Color color);
//        private void ChangeToolStripButtonBackColor(ToolStripButton button, Color color)
//        {
//            if (InvokeRequired)
//            {
//                this.Invoke(new ChangeToolStripButtonBackColorDelegate(ChangeToolStripButtonBackColor), button, color);
//                return;
//            }
//            button.BackColor = color;
//        }

//        public delegate void InspectDelegate();
//        public void Inspect()
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new InspectDelegate(Inspect));
//                return;
//            }

//            if (teachBox.DrawBox.Image == null || this.SourceImage2d == null)
//            {
//                MessageForm.Show(this.ParentForm, "Image가 필요합니다. (Grab)", "UniEye");
//                return;
//            }

//            VisionProbe curProbe = (VisionProbe)this.teachHandlerProbe.GetSelectedProbe()[0];
//            GravureSheetChecker sheetChecker = (GravureSheetChecker)curProbe.InspAlgorithm;
//            if (((SheetCheckerParam)sheetChecker.Param).TrainerParam.Traind == false)
//            {
//                MessageForm.Show(this.ParentForm, "Not Trained yet.");
//                return;
//            }

//            int msgPos = teachBox.DrawBox.Image.Width / 50;
//            int charSize = teachBox.DrawBox.Image.Width / 10;

//            teachBox.DrawBox.BackgroundFigures.Clear();
//            teachBox.DrawBox.TempFigureGroup.Clear();
//            teachBox.DrawBox.FigureGroup.Clear();
//            teachBox.DrawBox.Invalidate(true);
//            teachBox.DrawBox.Update();
//            this.defectList.Rows.Clear();

//            SimpleProgressForm form = new SimpleProgressForm("Test Inspection");
//            form.Show((Action)(() =>
//            {
//                this.TeachBox.DrawBox.FigureGroup.Clear();
//                this.AddFiducialRegionFigure();

//                AlgoImage algoImage = ImageBuilder.Build(sheetChecker.GetAlgorithmType(), this.SourceImage2d, ImageType.Gpu);

//                int h = algoImage.Height / 3;
//                AlgoImage firstSubImage = algoImage.GetSubImage(Rectangle.FromLTRB(0, 0, algoImage.Width, h));
//                AlgoImage secondSubImage = algoImage.GetSubImage(Rectangle.FromLTRB(0, h, algoImage.Width, algoImage.Height));

//                SheetImageSet sheetImageSet = new SheetImageSet(new List<AlgoImage>(new AlgoImage[] { firstSubImage, secondSubImage }), -1);
//                DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, PathSettings.Instance().Temp);

//                modellerInspectionResult.Clear();
//                modellerInspectionResult.ResultPath = Path.Combine(PathSettings.Instance().Result, "ModellerPage");

//                try
//                {
//                    SheetCheckerInspectParam sheetCheckerInspectParam = new SheetCheckerInspectParam(sheetImageSet, null, this.modellerInspectionResult,
//                        new AlgorithmInspectParam(null, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, debugContext));

//                    SheetCheckerAlgorithmResult algorithmResult = (SheetCheckerAlgorithmResult)curProbe.InspAlgorithm.Inspect(sheetCheckerInspectParam);
//                    //modellerInspectionResult.AddProbeResult(new VisionProbeResult(null, algorithmResult, null));

//                    DynMvp.UI.TextFigure resultTextFigure = new DynMvp.UI.TextFigure(algorithmResult.Message.ToString(), new Point(100, 100), new Font("Arial", 700), Color.Red);
//                    resultTextFigure.Alignment = StringAlignment.Near;
//                    this.TeachBox.DrawBox.BackgroundFigures.AddFigure(resultTextFigure);

//                    modellerInspectionResult.AppendResultFigures(teachBox.DrawBox.FigureGroup, FigureDrawOption.Default);

//                    foreach (SheetCheckerSubResult subResult in algorithmResult.SubResultList)
//                    {
//                        if (radioButtonAll.Checked == true)
//                        {
//                            AddDefect(subResult as SheetCheckerSubResult);
//                        }
//                        else if (radioButtonBlack.Checked == true)
//                        {
//                            if (subResult.DefectType == SheetDefectType.BlackDefect)
//                                AddDefect(subResult as SheetCheckerSubResult);
//                        }
//                        else if (radioButtonWhite.Checked == true)
//                        {
//                            if (subResult.DefectType == SheetDefectType.WhiteDefect)
//                                AddDefect(subResult as SheetCheckerSubResult);
//                        }
//                    }

//                    //algorithmResult.SubResultList.ForEach(f => AddDefect(f as SheetCheckerSubResult));
//                    UpdateLabel(this.labelTotalDefect, algorithmResult.SubResultList.Count.ToString());//this.defectList.Rows.Count.ToString());
//                    UpdateLabel(this.labelSpendTime, string.Format("{0:00}.{1:000}", algorithmResult.SpandTime.Seconds, algorithmResult.SpandTime.Milliseconds));//this.defectList.Rows.Count.ToString());
//                }
//                finally
//                {
//                    algoImage.Dispose();
//                    sheetImageSet.Dispose();
//                }
//            }));

//            teachBox.DrawBox.TempFigureGroup = new FigureGroup();

//            teachBox.DrawBox.Invalidate(true);
//            teachBox.DrawBox.Update();
//        }

//        private delegate void UpdateLabelDelegate(Label labelTotalDefect, string v);
//        private void UpdateLabel(Label labelTotalDefect, string v)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UpdateLabelDelegate(UpdateLabel), labelTotalDefect, v);
//                return;
//            }
//            labelTotalDefect.Text = v;
//        }

//        private delegate void AddDefectDelegate(SheetCheckerSubResult sheetCheckerSubResult);
//        private void AddDefect(SheetCheckerSubResult sheetCheckerSubResult)
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new AddDefectDelegate(AddDefect), sheetCheckerSubResult);
//                return;
//            }

//            if (sheetCheckerSubResult == null)
//                return;

//            int rowIndex = this.defectList.Rows.Add(UniScanGUtil.Instance().GetDefectString(sheetCheckerSubResult.DefectType), null, sheetCheckerSubResult.Image);
//            this.defectList.Rows[rowIndex].Tag = sheetCheckerSubResult;

//            if (sheetCheckerSubResult.Image != null && (sheetCheckerSubResult.Image.Width * 3 < sheetCheckerSubResult.Image.Height))
//            {
//                this.defectList.Rows[rowIndex].Height = this.defectList.RowTemplate.Height * 2;
//            }

//            Rectangle rect = sheetCheckerSubResult.ResultRect.ToRectangle();
//            rect = DrawingHelper.FromCenterSize(Point.Round(DrawingHelper.CenterPoint(sheetCheckerSubResult.ResultRect.ToRectangle())), new Size(5, 5));
//            //teachBox.DrawBox.FigureGroup.AddFigure(new EllipseFigure(rect, new Pen(Color.Yellow, 3)));
//        }

//        //public string RcInspect()
//        //{
//        //    Inspect();

//        //    //teachBox.DrawBox.Invalidate(true);
//        //    //teachBox.DrawBox.Update();

//        //    Operation.Data.Model curModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;
//        //    string resultPath = Path.Combine(PathSettings.Instance().Result, "RemoteInspect");

//        //    if (Directory.Exists(resultPath) == false)
//        //        Directory.CreateDirectory(resultPath);

//        //    FileHelper.ClearFolder(resultPath);

//        //    //int index = 0;
//        //    //List<AlgorithmResult> subResultList = ((VisionProbeResult)(modellerInspectionResult.ProbeResultList[0])).AlgorithmResult.SubResultList;
//        //    //foreach (SheetCheckerSubResult subResult in subResultList)
//        //    //{
//        //    //    if (subResult == null)
//        //    //        continue;

//        //    //    string fileName = string.Format(@"{0}\{1}.bmp", rcResultPath, index);
//        //    //    ImageHelper.SaveImage(subResult.Image, fileName);
//        //    //    index++;
//        //    //}

//        //    // Get Result Figure
//        //    FigureGroup figureGroup = new FigureGroup();
//        //    modellerInspectionResult.AppendResultFigures(figureGroup, FigureDrawOption.Default);

//        //    // Draw Color bitmap
//        //    Bitmap bitmap = new Bitmap(teachBox.Image.Width, teachBox.Image.Height, PixelFormat.Format24bppRgb);
//        //    Graphics g = Graphics.FromImage(bitmap);
//        //    g.DrawImage(teachBox.Image, Point.Empty);
//        //    figureGroup.Draw(g, null, false);

//        //    // Save Bitmap
//        //    // image2dNameFormat = "Image_C{0:00}_S{1:000}_L{2:00}.{3}";
//        //    string fileName = string.Format(ImageBuffer.Image2dNameFormat, 0, UniScanGSettings.Instance().InspectorInfo.CamIndex, 0, "jpg");
//        //    string fullPath = Path.Combine(resultPath, fileName);
//        //    ImageHelper.SaveImage(bitmap, fullPath, ImageFormat.Jpeg);

//        //    return fileName;
//        //}

//        private void addProbeToolStripButton_Click(object sender, EventArgs e)
//        {
//        }

//        public void EnableControls()
//        {

//        }

//        //delegate void UpdateParamControlDelegate();
//        //public void UpdateParamControl()
//        //{
//        //    if (InvokeRequired)
//        //    {
//        //        Invoke(new UpdateParamControlDelegate(UpdateParamControl));
//        //        return;
//        //    }

//        //    int stepIndex = UniScanGSettings.Instance().InspectorInfo.CamIndex;
//        //    Operation.Data.Model curModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;
//        //    InspectionStep curInspectionStep = curModel.GetInspectionStep(stepIndex);
//        //    TargetGroup curTargetGroup = curInspectionStep.GetTargetGroup(0);
//        //    Target curTarget = curTargetGroup.GetTarget(1);
//        //    VisionProbe curProbe = (VisionProbe)curTarget.GetProbe(1);

//        //    teachHandlerProbe.ClearSelection();
//        //    teachHandlerProbe.Select(curProbe);

//        //    targetParamControl.ClearProbeData();
//        //    targetParamControl.SelectProbe(curProbe);
//        //    targetParamControl.UpdateTargetGroupImage(SourceImage2d, 0);

//        //    teachBox.TargetGroup = SystemManager.Instance().CurrentModel.GetInspectionStep(stepIndex).GetTargetGroup(0);

//        //    //Size imageSize = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0).ImageSize;
//        //    //RotatedRect inspRegion = new RotatedRect(0, 0, imageSize.Width, imageSize.Height, 0);
//        //    //curTarget.BaseRegion = inspRegion;
//        //    //curTarget.AlignedRegion = inspRegion;

//        //    //curProbe.BaseRegion = inspRegion;
//        //    //curProbe.AlignedRegion = inspRegion;
//        //}

//        public void TabPageVisibleChanged(bool visibleFlag)
//        {
//            //Operation.Data.Model curModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;

//            //if (visibleFlag == true)
//            //{
//            //    defectList.Rows.Clear();
//            //    defectPicture.Image = null;
//            //    labelTotalDefect.Text = defectList.Rows.Count.ToString();

//            //    if (!SystemState.Instance().OnInspectOrWaitOrPause)
//            //        UpdateControls(true);

//            //    if (teachControl != null)
//            //        teachControl.TabPageVisibleChanged(true);
//            //}
//            //else
//            //{
//            //    SimpleProgressForm loadingForm = new SimpleProgressForm("Save Model");
//            //    loadingForm.Show(new Action(() =>
//            //    {
//            //        SystemManager.Instance().ModelManager.SaveModel(curModel);
//            //        UniScanGSettings.Instance().Save();
//            //    }));
//            //}
//        }

//        public delegate void UpdateControlsDelegate(bool updateImage);
//        public void UpdateControls(bool updateImage)
//        {
//            //if (InvokeRequired)
//            //{
//            //    BeginInvoke(new UpdateControlsDelegate(UpdateControls), updateImage);
//            //    return;
//            //}

//            //Operation.Data.Model curModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;
//            //if (curModel != null)
//            //{
//            //    labelModelName.Text = curModel.Name;
//            //    SetTaughtLabel(curModel.IsTaught());

//            //    int camNo = UniScanGSettings.Instance().InspectorInfo.CamIndex;

//            //    if (updateImage)
//            //    {
//            //        try
//            //        {
//            //            UniScanGModellerPageExtender extender = modellerPageExtender as UniScanGModellerPageExtender;
//            //            if (SourceImage2d == null)
//            //                SourceImage2d = extender.LastGrabbedImage;

//            //            if (SourceImage2d == null)
//            //                SourceImage2d = LoadImage(curModel, camNo, 0);

//            //            teachBox.UpdateImage(SourceImage2d);
//            //        }
//            //        catch (OutOfMemoryException)
//            //        { }
//            //    }

//            //    UpdateParamControl();

//            //    teachBox.ZoomFit();

//            //    if (teachControl != null)
//            //        teachControl.UpdateData();
//            //}
//        }

//        //delegate void StopGrabDelegate();
//        public void StopGrab()
//        {
//            //if (InvokeRequired)
//            //{
//            //    BeginInvoke(new StopGrabDelegate(StopGrab));
//            //    return;
//            //}

//            modellerPageExtender.StopGrab();
//        }

//        delegate void ImageGrabedDelegate();
//        void ImageGrabed()
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new ImageGrabedDelegate(ImageGrabed));
//                return;
//            }

//            grabTimer.Stop();
//            grabTimer.Dispose();

//            StopGrab();

//            //teachBox.DrawBox.TempFigureGroup = null;
//            //teachBox.DrawBox.Invalidate(true);
//            //teachBox.DrawBox.Update();

//            cancellationTokenSource = null;

//            onWaitGrab = false;
//        }

//        delegate void GrabTimerCallBackDelegate(object sender, ElapsedEventArgs e);
//        public void GrabTimerCallBack(object sender, ElapsedEventArgs e)
//        {
//            if (InvokeRequired)
//            {
//                LogHelper.Debug(LoggerType.Inspection, "Begin Invoke GrabTimerCallBack");
//                BeginInvoke(new GrabTimerCallBackDelegate(GrabTimerCallBack), sender, e);
//                return;
//            }

//            if (cancellationTokenSource != null)
//                cancellationTokenSource.Cancel();

//            MessageForm.Show(this.ParentForm, "설비 가동 상태를 확인해 주세요.", "UniEye");
//        }

//        public void GrabDone()
//        {
//            LogHelper.Debug(LoggerType.Operation, "TeachingPage::GrabDone");
//            if (cancellationTokenSource != null)
//                cancellationTokenSource.Cancel();
//        }

//        public bool IsTeached()
//        {
//            return SystemManager.Instance().CurrentModel.IsTaught();
//        }

//        Image2D[] grabSaveImage = new Image2D[10];
//        int subIndex = 0;
//        int wholeIndex = 0;

//        public void LiveImageCaptured(ImageDevice imageDevice)
//        {
//            //ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
//            //ImageD grabImage = imageDevice.GetGrabbedImage(IntPtr.Zero);

//            //LogHelper.Debug(LoggerType.Operation, "ModellerPage - ImageCaptured");

//            //((Image2D)grabImage).ConvertFromDataPtr();
//            //grabSaveImage[subIndex] = (Image2D)grabImage.Clone();
//            //if (subIndex == 2)
//            //{
//            //    for (int i = 0; i < 3; i++)
//            //    {
//            //        Operation.Data.Model currentModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;

//            //        string imagePath = Path.Combine(currentModel.ModelPath, "CaptureImages");
//            //        if (Directory.Exists(imagePath) == false)
//            //        {
//            //            Directory.CreateDirectory(imagePath);
//            //        }

//            //        string fileName = String.Format("{0}_{1}.bmp", wholeIndex, i); //ImageBuffer.GetImage2dFileName(deviceIndex, 0, 0, "jpg");
//            //        string filePath = Path.Combine(imagePath, fileName);

//            //        //AlgoImage algoImage = ImageBuilder.MilImageBuilder.Build(grabSaveImage[i], ImageType.Grey);
//            //        grabSaveImage[i].SaveImage(filePath, ImageFormat.Bmp);
//            //        //algoImage.Save(filePath, null);
//            //        //algoImage.Dispose();
//            //    }

//            //    wholeIndex++;
//            //    subIndex = 0;
//            //}
//            //else
//            //{
//            //    subIndex++;
//            //}
//        }

//        private void buttonShowAll_Click(object sender, EventArgs e)
//        {
//            bShowAll = !bShowAll;
//        }

//        private void toolStripButtonSaveModel_Click(object sender, EventArgs e)
//        {
//            SaveModel(true);
//        }

//        public bool SaveModel(bool showMessage)
//        {
//            SimpleProgressForm loadingForm = new SimpleProgressForm("Save Model");
//            bool ok = true;
//            loadingForm.Show(new Action(() =>
//            {
//                DynMvp.Data.Model curModel = SystemManager.Instance().CurrentModel;
//                SystemManager.Instance().ModelManager.SaveModel(curModel);
//            }));

//            if (ok == false && showMessage)
//                MessageForm.Show(null, "Save Fail");

//            return ok;
//        }

//        private void defectList_CellClick(object sender, DataGridViewCellEventArgs e)
//        {
//            //if (defectList.SelectedRows.Count > 0)
//            //{
//            //    SheetCheckerSubResult sheetCheckerSubResult = (SheetCheckerSubResult)defectList.Rows[defectList.SelectedRows[0].Index].Tag;
//            //    if (sheetCheckerSubResult != null)
//            //    {
//            //        PointF centerPt = DrawingHelper.CenterPoint(sheetCheckerSubResult.ResultRect);

//            //        //teachBox.DrawCrossLine(centerPt);
//            //        Rectangle resultRect = new Rectangle((int)sheetCheckerSubResult.ResultRect.X, (int)sheetCheckerSubResult.ResultRect.Y,
//            //            (int)sheetCheckerSubResult.ResultRect.Width, (int)sheetCheckerSubResult.ResultRect.Height);

//            //        resultRect.Inflate(500, 500);
//            //        teachBox.DrawEllipse(resultRect);
//            //    }

//            //    defectPicture.Image = (Bitmap)defectList.SelectedRows[0].Cells[2].Value;
//            //}
//        }

//        private void defectList_Paint(object sender, PaintEventArgs e)
//        {
//            Brush defectBrush = new SolidBrush(Color.Black);
//            Font font = new Font("Arial", 9);
//            StringFormat stringFormat = new StringFormat();
//            stringFormat.Alignment = StringAlignment.Near;
//            stringFormat.LineAlignment = StringAlignment.Near;

//            Brush typeBrush = new SolidBrush(Color.Blue);

//            for (int i = 0; i < defectList.Rows.Count; i++)
//            {
//                if (defectList.Rows[i].Cells[0].Displayed == true)
//                {
//                    SheetCheckerSubResult sheetCheckerSubResult = (SheetCheckerSubResult)defectList.Rows[i].Tag;
//                    if (sheetCheckerSubResult != null)
//                    {
//                        Rectangle cellRect = defectList.GetCellDisplayRectangle(1, i, false);
//                        e.Graphics.DrawString(sheetCheckerSubResult.Message, font, defectBrush, new Point(cellRect.Left + padding, cellRect.Top + padding), stringFormat);

//                        //cellRect = defectList.GetCellDisplayRectangle(0, i, false);
//                        //if (string.IsNullOrEmpty(sheetCheckerSubResult.Message) == false)
//                        //    e.Graphics.DrawString(sheetCheckerSubResult.Message, font, typeBrush, new Point(cellRect.Left + padding, cellRect.Bottom - padding * 6), stringFormat);
//                    }
//                }
//            }
//        }

//        public bool Teach(out string message, bool remoteCall)
//        {
//            MonitoringClient monitoringClient = (MonitoringClient)SystemManager.Instance().MachineIf;
//            bool result = false;
//            message = "";

//            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, "Teach"));
//            AlgorithmResult teachResult = null;
//            cancellationTokenSource = new CancellationTokenSource();
//            Thread teachingThread = new Thread(new ThreadStart(() =>
//           {
//               //Data.Model curModel = (Data.Model)SystemManager.Instance().CurrentModel;
//               VisionProbe visionProbe = null;// curModel.GetVisionProbe();
//               GravureSheetChecker sheetChecker = visionProbe.InspAlgorithm as GravureSheetChecker;
//               SheetCheckerParam sheetCheckerParam = sheetChecker.Param as SheetCheckerParam;
//               IAlgorithmParamControl paramControl = ((VisionParamControl)targetParamControl.SelectedAlgorithmParamControl).SelectedAlgorithmParamControl;
//               SheetCheckerParamControl sheetCheckerParamControl = (SheetCheckerParamControl)paramControl;
//               TeachProgressUpdate teachProgressUpdate = sheetCheckerParamControl.UpdateTeachProgress;
//               SheetCheckerTrain trainer = new SheetCheckerTrain(sheetCheckerParam);

//               //ImageD teachImage = SourceImage2d;
//               AlgoImage teachImage = ImageBuilder.Build(sheetChecker.GetAlgorithmType(), SourceImage2d, ImageType.Gpu);
//               TrainerInspectParam trainerInspectParam = new TrainerInspectParam(teachImage, teachProgressUpdate,
//                   new AlgorithmInspectParam(null, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, debugContext));
//               AlgorithmResult algorithmResult = trainer.Inspect(trainerInspectParam);
//               teachResult = algorithmResult;

//               teachImage.Dispose();
//           }));
//            teachingThread.Start();

//            SimpleProgressForm loadingForm = new SimpleProgressForm("Auto Teach");
//            loadingForm.Show(new Action(() =>
//            {
//                while (teachingThread.IsAlive == true)
//                {
//                    Thread.Sleep(100);
//                    if (cancellationTokenSource.IsCancellationRequested)
//                    {
//                        teachingThread.Abort();
//                        //break;
//                    }
//                }
//            }), cancellationTokenSource);

//            if (cancellationTokenSource.IsCancellationRequested)
//            {
//                result = false;
//                message = "Operation Canceled.";
//                return false;
//            }

//            result = teachResult.Good;
//            message = teachResult.Message;

//            if (result == false)
//                return false;

//            if (teachControl != null)
//                teachControl.UpdateData();

//            bool trained = IsTeached();
//            SetTaughtLabel(trained);
//            ((MainForm)ParentForm).EnableTabs("Inspect", trained);

//            defectList.Rows.Clear();
//            labelTotalDefect.Text = defectList.Rows.Count.ToString();

//            teachBox.DrawBox.FigureGroup.Clear();
//            AddFiducialRegionFigure();
//            AddDummyRangeFigure();

//            //teachBox.DrawBox.Invalidate(true);
//            //teachBox.DrawBox.Update();

//            if (trained)
//            {
//                SimpleProgressForm saveProgressForm = new SimpleProgressForm("Save Model");
//                saveProgressForm.Show(new Action(() =>
//                {
//                    DynMvp.Data.Model curModel = SystemManager.Instance().CurrentModel;
//                    SystemManager.Instance().ModelManager.SaveModel(curModel);
//                }));
//            }
//            message = "OK";
//            return true;
//        }

//        private void toolStripButtonAutoTeach_Click(object sender, EventArgs e)
//        {
//            LogHelper.Debug(LoggerType.Operation, "SheetCheckerParamControl - buttonTeach_Click");
//            string message = "";
//            bool ok = Teach(out message, false);
//            StringBuilder sb = new StringBuilder();
//            if (ok)
//            {
//                sb.AppendLine("Train OK");
//            }
//            else
//            {
//                sb.AppendLine("Train Fail");
//                sb.AppendLine(message);
//            }

//            MessageForm.Show(null, sb.ToString());
//        }

//        private void buttonZoomIn_Click(object sender, EventArgs e)
//        {
//            teachBox.ZoomIn();
//        }

//        private void buttonZoomOut_Click(object sender, EventArgs e)
//        {
//            teachBox.ZoomOut();
//        }

//        private void buttonZoomFit_Click(object sender, EventArgs e)
//        {
//            teachBox.ZoomFit();
//            teachBox.DrawBox.BackgroundFigures.Clear();
//            teachBox.DrawBox.TempFigureGroup.Clear();

//            AddDummyRangeFigure();
//            foreach (DataGridViewRow row in defectList.Rows)
//            {
//                SheetCheckerSubResult sheetCheckerSubResult = (SheetCheckerSubResult)row.Tag;
//                teachBox.DrawBox.TempFigureGroup.AddFigure(sheetCheckerSubResult.ResultFigureGroup);
//            }

//            teachBox.DrawBox.Invalidate();
//            teachBox.DrawBox.Update();
//        }

//        private void defectList_SelectionChanged(object sender, EventArgs e)
//        {
//            if (defectList.SelectedRows.Count > 0)
//            {
//                SheetCheckerSubResult sheetCheckerSubResult = (SheetCheckerSubResult)defectList.Rows[defectList.SelectedRows[0].Index].Tag;
//                if (sheetCheckerSubResult != null)
//                {
//                    PointF centerPt = DrawingHelper.CenterPoint(sheetCheckerSubResult.ResultRect);

//                    Rectangle resultRect = new Rectangle((int)sheetCheckerSubResult.ResultRect.X, (int)sheetCheckerSubResult.ResultRect.Y,
//                        (int)sheetCheckerSubResult.ResultRect.Width, (int)sheetCheckerSubResult.ResultRect.Height);

//                    resultRect.Inflate(500, 500);
//                    teachBox.DrawEllipse(resultRect);
//                }

//                defectPicture.Image = (Bitmap)defectList.SelectedRows[0].Cells[2].Value;
//            }
//        }

//        private void toolStripButtonExportData_Click(object sender, EventArgs e)
//        {
//            string exportDataWhitePath = Path.Combine(SystemManager.Instance().CurrentModel.ModelPath, "ExportData(White)");
//            string exportDataBlackPath = Path.Combine(SystemManager.Instance().CurrentModel.ModelPath, "ExportData(Black)");

//            if (Directory.Exists(exportDataWhitePath) == false)
//                Directory.CreateDirectory(exportDataWhitePath);
//            else
//            {
//                Directory.Delete(exportDataWhitePath, true);
//                Thread.Sleep(500);
//                Directory.CreateDirectory(exportDataWhitePath);
//            }

//            if (Directory.Exists(exportDataBlackPath) == false)
//                Directory.CreateDirectory(exportDataBlackPath);
//            else
//            {
//                Directory.Delete(exportDataBlackPath, true);
//                Thread.Sleep(500);
//                Directory.CreateDirectory(exportDataBlackPath);
//            }

//            StringBuilder resultStringBuilder1 = new StringBuilder();
//            StringBuilder resultStringBuilder2 = new StringBuilder();

//            string fileName1 = Path.Combine(exportDataWhitePath, "data.txt");
//            string fileName2 = Path.Combine(exportDataBlackPath, "data.txt");

//            foreach (DataGridViewRow row in defectList.Rows)
//            {
//                SheetCheckerSubResult subResult = (SheetCheckerSubResult)row.Tag;
//                if (subResult.DefectType == SheetDefectType.BlackDefect)
//                    continue;

//                ((Bitmap)row.Cells[2].Value).Save(Path.Combine(exportDataWhitePath, row.Index.ToString() + ".bmp"));
//                resultStringBuilder1.AppendLine(String.Format("index: {0} - {1}", row.Index, subResult.Message));
//            }

//            File.WriteAllText(fileName1, resultStringBuilder1.ToString());

//            foreach (DataGridViewRow row in defectList.Rows)
//            {
//                SheetCheckerSubResult subResult = (SheetCheckerSubResult)row.Tag;
//                if (subResult.DefectType == SheetDefectType.WhiteDefect)
//                    continue;

//                ((Bitmap)row.Cells[2].Value).Save(Path.Combine(exportDataBlackPath, row.Index.ToString() + ".bmp"));
//                resultStringBuilder2.AppendLine(String.Format("index: {0} - {1}", row.Index, subResult.Message));
//            }

//            File.WriteAllText(fileName2, resultStringBuilder2.ToString());

//            System.Diagnostics.Process.Start(exportDataWhitePath);
//            System.Diagnostics.Process.Start(exportDataBlackPath);
//        }

//        private void buttonTeachGuide_Click(object sender, EventArgs e)
//        {
//            TeachingManualForm teachingManualForm = new TeachingManualForm();
//            teachingManualForm.Show();
//        }

//        private void buttonMasterTeachGuide_Click(object sender, EventArgs e)
//        {
//            MasterTeachingManualForm masterTeachingManualForm = new MasterTeachingManualForm();
//            masterTeachingManualForm.Show();
//        }

//        delegate void UserChangedDelegatge();
//        public void UserChanged()
//        {
//            if (InvokeRequired)
//            {
//                BeginInvoke(new UserChangedDelegatge(UserChanged));
//                return;
//            }
//        }

//        public bool RcGrab(float cvySpeedMPS)
//        {
//            return Grab(cvySpeedMPS, GrabType.Normal);
//        }

//        public bool RcFiducialGrab(float cvySpeedMPS)
//        {
//            return Grab(cvySpeedMPS, GrabType.Fiducial);
//        }

//        public bool RcTestGrab(float cvySpeedMPS)
//        {
//            return Grab(cvySpeedMPS, GrabType.Test);
//        }

//        public void RcStop()
//        {
//            ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
//            imageDevice.Stop();

//            StopGrab();

//            if (cancellationTokenSource != null)
//                cancellationTokenSource.Cancel();
//        }

//        public string RcTeach()
//        {
//            string message;
//            bool ok = Teach(out message, true);
//            return (ok ? "OK" : message);
//        }

//        public enum GrabType { Normal, Fiducial, Sequence, Test }
//        public bool Grab(float cvySpeedMPS, GrabType grabType)
//        {
//            float grabHz = 0;
//            if (cvySpeedMPS > 0)
//            {
//                Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList[0];
//                grabHz = cvySpeedMPS / calibration.PelSize.Height * 1000000;  // Line per sec
//            }

//            bool grabOk = false;
//            bool isCancel = false;
//            cancellationTokenSource = new CancellationTokenSource();
//            SimpleProgressForm jobWaitForm = new SimpleProgressForm("Grab");
//            jobWaitForm.Show(new Action(() =>
//            {
//                grabOk = Grab(SystemManager.Instance().CurrentModel?.LightParamSet, grabHz, grabType);
//                if (grabOk)
//                {
//                    grabTimer = new System.Timers.Timer();
//                    grabTimer.Interval = 60 * 1000;
//                    grabTimer.Elapsed += new ElapsedEventHandler(GrabTimerCallBack);
//                    if (grabType != GrabType.Test)
//                        grabTimer.Start();

//                    while (cancellationTokenSource.IsCancellationRequested == false)
//                    {
//                        Thread.Sleep(100);
//                    }
//                }
//                ImageGrabed();
//            }), cancellationTokenSource);

//            if (isCancel)
//                grabOk = false;

//            if (grabOk == false || grabType == GrabType.Test)
//                return grabOk;

//            //cancellationTokenSource = new CancellationTokenSource();
//            SimpleProgressForm jobWaitForm2 = new SimpleProgressForm("Update");
//            jobWaitForm2.Show(new Action(() =>
//            {
//                Image2D sheetImage = null;
//                sheetImage = (modellerPageExtender as UniScanGModellerPageExtender).LastGrabbedImage as Image2D;
//                //if (grabType == GrabType.Normal)
//                //{
//                //    ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
//                //    ImageD grabbedImage = imageDevice.GetGrabbedImage(IntPtr.Zero).Clone();
//                //    grabbedImage.ConverFromDataPtr();
//                //    sheetImage = (Image2D)grabbedImage;
//                //}
//                //else if (grabType == GrabType.Fiducial)
//                //{
//                //    ((SamsungElectroGravureModellerPageExtender)modellerPageExtender).UpdateGrabbedImage();
//                //    sheetImage = ((SamsungElectroGravureModellerPageExtender)modellerPageExtender).FullBufferImage;
//                //}
//                ModellerPageExtender.UpdateImage(sheetImage);

//                if (sheetImage != null)
//                {
//                    // Save
//                    string imagePath = Path.Combine(SystemManager.Instance().CurrentModel.ModelPath, "Image");
//                    if (Directory.Exists(imagePath) == false)
//                        Directory.CreateDirectory(imagePath);

//                    int camIndex = UniScanGSettings.Instance().InspectorInfo.CamIndex;
//                    string fullBufferFileName = ImageBuffer.GetImage2dFileName(0, camIndex, 0, ImageFormat.Jpeg.ToString());
//                    sheetImage.SaveImage(Path.Combine(imagePath, fullBufferFileName), ImageFormat.Jpeg);

//                    Image2D resizeImage = (Image2D)sheetImage.Resize(sheetImage.Width / 10, sheetImage.Height / 10);
//                    resizeImage.SaveImage(Path.Combine(imagePath, "GrabbedImage.jpeg"), ImageFormat.Jpeg);
//                }
//            }), null/*cancellationTokenSource*/);

//            return true;
//        }

//        public bool Grab(LightParamSet lightParamSet, float grabHz, TeachingPage.GrabType grabType)
//        {
//            ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
//            //DynMvp.Devices.FrameGrabber.CameraGenTL cameraGenTL = (DynMvp.Devices.FrameGrabber.CameraGenTL)imageDevice;
//            DynMvp.Devices.FrameGrabber.Camera cameraGenTL = (DynMvp.Devices.FrameGrabber.Camera)imageDevice;
//            cameraGenTL.SetLineScanMode();
//            if (grabHz > 0)
//            {
//                cameraGenTL.SetTriggerMode(TriggerMode.Software);
//                cameraGenTL.SetExposureTime(1 / grabHz * 1000000);
//            }
//            else
//            {
//                cameraGenTL.SetTriggerMode(TriggerMode.Hardware);
//                cameraGenTL.SetLineScanMode();
//            }
//            int camIndex = UniScanGSettings.Instance().InspectorInfo.CamIndex;

//            //Operation.Data.Model curModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;
//            //TargetGroup curTargetGroup = curModel.GetInspectionStep(camIndex).GetTargetGroup(0);
//            //Target curTarget = curTargetGroup.GetTarget(1);
//            //VisionProbe curProbe = (VisionProbe)curTarget.GetProbe(1);

//            //Size imageSize = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0).ImageSize;
//            //RotatedRect inspRegion = new RotatedRect(0, 0, imageSize.Width, imageSize.Height, 0);
//            //curTarget.BaseRegion = inspRegion;
//            //curTarget.AlignedRegion = inspRegion;

//            //curProbe.BaseRegion = inspRegion;
//            //curProbe.AlignedRegion = inspRegion;

//            ((UniScanGModellerPageExtender)modellerPageExtender).GrabProcess(camIndex, 0, 0, lightParamSet, grabType);
//            return true;
//        }

//        private void grabProcessToolStripButton_Click(object sender, EventArgs e)
//        {
//            if (onWaitGrab == false)
//            {

//                float cvySpeedMPS = ((MainForm)SystemManager.Instance().MainForm).QuaryConvSpeed();
//                if (cvySpeedMPS < 0)
//                    return;

//                Grab(cvySpeedMPS, GrabType.Normal);
//                ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
//                imageDevice.Stop();
//                onWaitGrab = false;
//            }
//        }

//        private void fiducialGrabProcessToolStripButton_Click(object sender, EventArgs e)
//        {
//            if (onWaitGrab == false)
//            {
//                float cvySpeedMPS = ((MainForm)SystemManager.Instance().MainForm).QuaryConvSpeed();
//                if (cvySpeedMPS < 0)
//                    return;

//                onWaitGrab = true;
//                Grab(cvySpeedMPS, GrabType.Fiducial);
//                ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
//                imageDevice.Stop();
//            }
//        }

//        private void SequenceGrab(float cvySpeedMPS, int sequenceImageNum)
//        {
//            float grabHz = 0;
//            if (cvySpeedMPS > 0)
//            {
//                Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList[0];
//                grabHz = cvySpeedMPS / calibration.PelSize.Height * 1000000;
//            }

//            cancellationTokenSource = new CancellationTokenSource();
//            SimpleProgressForm jobWaitForm = new SimpleProgressForm("Wait Grab...");
//            jobWaitForm.Show(new Action(() =>
//            {
//                Grab(SystemManager.Instance().CurrentModel.LightParamSet, grabHz, GrabType.Sequence);

//                while (sequenceImages.Count < sequenceImageNum)
//                {
//                    Thread.Sleep(5);
//                }

//                ImageGrabed();
//                StopGrab();
//            }), cancellationTokenSource);

//            onWaitGrab = false;
//            string imagePath = Path.Combine(SystemManager.Instance().CurrentModel.ModelPath, "Image");
//            if (Directory.Exists(imagePath) == false)
//                Directory.CreateDirectory(imagePath);

//            int imageNum = 0;
//            cancellationTokenSource = new CancellationTokenSource();
//            SimpleProgressForm jobWaitForm2 = new SimpleProgressForm("Save...");
//            jobWaitForm2.Show(new Action(() =>
//            {
//                lock (sequenceImages)
//                {
//                    int width = sequenceImages.Max(f => f.Width);
//                    int height = sequenceImages.Sum(f => f.Height);
//                    //Image2D wholeImage = new Image2D(width, height, 1, 0);
//                    //if (wholeImage.ImageData.Data == null)
//                    //    wholeImage = null;

//                    Point dstPoint = new Point(0, 0);
//                    foreach (ImageD image in sequenceImages)
//                    {
//                        //wholeImage?.CopyFrom(image, new Rectangle(0, 0, image.Width, image.Height), image.Pitch, dstPoint);
//                        dstPoint.Y += image.Height;

//                        string fileName = imageNum.ToString() + "." + ImageFormat.Bmp.ToString();
//                        ImageHelper.SaveImage(image.ToBitmap(), Path.Combine(imagePath, fileName), ImageFormat.Bmp);
//                        image.Dispose();
//                        imageNum++;
//                    }
//                    //wholeImage?.SaveImage(Path.Combine(imagePath, "SequenceImage.bmp"), ImageFormat.Bmp);
//                }

//            }), cancellationTokenSource);
//            sequenceImages.Clear();
//        }

//        private void saveImageSetToolStripButton_Click(object sender, EventArgs e)
//        {
//            if (onWaitGrab)
//            {
//                MessageForm.Show(null, "Image is grabbing");
//                return;
//            }

//            float cvySpeedMPS = ((MainForm)SystemManager.Instance().MainForm).QuaryConvSpeed();
//            if (cvySpeedMPS < 0)
//                return;

//            int seqCount = GetSequencialImageCount();
//            if (seqCount <= 0)
//                return;

//            onWaitGrab = true;
//            SequenceGrab(cvySpeedMPS, seqCount);

//            ImageDevice imageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
//            imageDevice.Stop();
//        }

//        private int GetSequencialImageCount()
//        {
//            DynMvp.UI.InputForm form = new DynMvp.UI.InputForm("Sequence image num?");
//            if (form.ShowDialog() == DialogResult.Cancel)
//                return 0;

//            int seqCount;
//            if (int.TryParse(form.InputText, out seqCount) == false)
//            {
//                MessageForm.Show(null, "Wrong Input.");
//                return 0;
//            }

//            return seqCount;
//        }

//        //delegate void ImageSequnceGrabedDelegate();
//        //void ImageSequnceGrabed()
//        //{
//        //    if (InvokeRequired)
//        //    {
//        //        BeginInvoke(new ImageSequnceGrabedDelegate(ImageSequnceGrabed));
//        //        return;
//        //    }

//        //    StopGrab();

//        //    cancellationTokenSource = null;

//        //    onWaitGrab = false;
//        //}

//        public void SequenceImageAdd(ImageD grabImage = null)
//        {
//            if (onWaitGrab == false)
//                return;

//            lock (sequenceImages)
//                sequenceImages.Add(grabImage);
//        }


//        public void SaveModel()
//        {
//            //Operation.Data.Model curModel = (Operation.Data.Model)SystemManager.Instance().CurrentModel;
//            //if (curModel != null && curModel.IsEmpty() == false)
//            //{
//            //    if (curModel.Modified)
//            //    {
//            //        SystemManager.Instance().ModelManager.SaveModel(curModel);
//            //    }
//            //}
//        }

//        public void SetTaughtLabel(bool traind)
//        {
//            if (traind == true)
//            {
//                labelTaught.Text = "Finished";
//                labelTaught.BackColor = Color.Green;
//            }
//            else
//            {
//                labelTaught.Text = "Unfinished";
//                labelTaught.BackColor = Color.Red;
//            }
//        }

//        public void ClearTestInspectionResult()
//        {
//            this.modellerInspectionResult.ImageDisposible = false;
//            this.modellerInspectionResult.Clear(false);
//        }

//        private void Kimminjung()
//        {
//            SaveFileDialog dlg = new SaveFileDialog();
//            dlg.DefaultExt = ".csv";
//            dlg.FileName = "AllData.csv";
//            if (dlg.ShowDialog() == DialogResult.Cancel)
//                return;

//            string path = Path.GetDirectoryName(dlg.FileName);
//            //string path = @"D:\20171121김민정\80\80BV120";// Path.Combine(PathSettings.Instance().Result, DateTime.Now.ToString("yyyy-MM-dd"), SystemManager.Instance().CurrentModel.Name, "41");

//            List<string> allLines = new List<string>();
//            string[] dirList = Directory.GetDirectories(path);

//            foreach (string dirName in dirList)
//            {
//                string filePath = Path.Combine(dirName, "result.csv");
//                if (File.Exists(filePath))
//                {
//                    string[] fileLines = File.ReadAllLines(filePath);

//                    string serialNo = fileLines[0].Split(';')[0];

//                    for (int i = 1; i < fileLines.Count(); i++)
//                    {
//                        allLines.Add(serialNo + ';' + fileLines[i]);
//                    }
//                }
//            }
//            File.WriteAllLines(dlg.FileName, allLines);
//            MessageBox.Show("OK");
//        }
//    }
//}
