using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.Device;
using UniEye.Base;
using DynMvp.Devices;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.InspData;
using UniScanM.Pinhole.Data;
using DynMvp.UI;
using DynMvp.Vision;
using UniEye.Base.Settings;
using System.Reflection;
using DynMvp.Devices.Light;
using UniScanM.Pinhole.UI.MenuPage;
using System.Threading;
using UniScanM.Pinhole.Settings;
using UniScanM.Pinhole.UI.MenuPanel;

namespace UniScanM.Pinhole.UI
{
    public partial class InspectionPanelLeft : UserControl, IInspectionPanel, IMultiLanguageSupport
    {
        static int numOfResultView = 2;
        CanvasPanel[] canvasPanel;
        LastDefectPanel[] lastDefectPanel;

        int lastUpdatedCamIndex = -1;
                
        private Judgment leftState;
        private Judgment rightState;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public InspectionPanelLeft()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            InitViewPanel();
            InitLastDefectPanel();

            StringManager.AddListener(this);
        }

        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
        
        void InitLastDefectPanel()
        {
            LogHelper.Debug(LoggerType.StartUp, "Insepct Page - Init InitLastDefectPanel Start.");

            DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
            ImageDeviceHandler imageDeviceHandler = deviceBox.ImageDeviceHandler;

            lastDefectPanel = new LastDefectPanel[numOfResultView];
            for (int i = 0; i < numOfResultView; i++)
            {
                //Task task = Task.Factory.StartNew(() => this.lastDefectPanel[i] = new LastDefectPanel(i + 1));
                //task.Wait();
                this.lastDefectPanel[i] = new LastDefectPanel(i+1);
                this.lastDefectPanel[i].Dock = DockStyle.Fill;
                this.lastDefectPanel[i].Margin = new Padding(0, 0, 0, 0);
                layoutLastImage.Controls.Add(lastDefectPanel[i], i, 0);
            }

            LogHelper.Debug(LoggerType.StartUp, "Insepct Page - Init InitLastDefectPanelEnd.");
        }

        private void InitViewPanel()
        {
            LogHelper.Debug(LoggerType.StartUp, "Insepct Page - Init Result View Panel Start.");

            DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
            ImageDeviceHandler imageDeviceHandler = deviceBox.ImageDeviceHandler;

            canvasPanel = new CanvasPanel[numOfResultView];
            for (int i = 0; i < numOfResultView; i++)
            {
                //Task task = Task.Factory.StartNew(() => this.canvasPanel[i] = new CanvasPanel());
                //task.Wait();
                this.canvasPanel[i] = new CanvasPanel();
                this.canvasPanel[i].ShowCenterGuide = false;
                this.canvasPanel[i].ShowToolbar = false;
                this.canvasPanel[i].Dock = DockStyle.Fill;
                this.canvasPanel[i].NoneClickMode = true;
                Image2D tempImage = new Image2D(2048, 1024, 3);
                if (i == 0)
                    panelCam1.Controls.Add(canvasPanel[i]);
                else
                    panelCam2.Controls.Add(canvasPanel[i]);
            }

            canvasPanel[0].HorizontalAlignment = HorizontalAlignment.Right;
            canvasPanel[1].HorizontalAlignment = HorizontalAlignment.Left;
            LogHelper.Debug(LoggerType.StartUp, "Insepct Page - Init Result View Panel End.");
        }
        
        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            UpdateResult2((Data.InspectionResult)inspectionResult);
        }

        IAsyncResult asyncUpdatePage;
        delegate void UpdateResultDelegate(Data.InspectionResult inspectResult);
        public void UpdateResult(Data.InspectionResult inspectResult)
        {
            if (InvokeRequired)
            {
                if (asyncUpdatePage != null)
                {
                    if (asyncUpdatePage.IsCompleted == false)
                    {
                        return;
                    }
                }

                asyncUpdatePage = BeginInvoke(new UpdateResultDelegate(UpdateResult), inspectResult);// = BeginInvoke(new UpdateResultDelegate(UpdateResult), inspectResult);
                return;
            }

            if (inspectResult == null)
                return;

            FigureGroup figureGroup = new FigureGroup();
            ///*
            foreach (DefectInfo defectInfo in inspectResult.LastDefectInfoList)
            {
                Figure defectMark;
                RectangleF boundingRect = defectInfo.BoundingRect;
                if (boundingRect.Width < 10 || boundingRect.Height < 10)
                {
                    defectMark = new EllipseFigure(DrawingHelper.CenterPoint(boundingRect), 10, new Pen(Color.Red));
                }
                else if (boundingRect.Width > 1000 || boundingRect.Height > 1000)
                {
                    defectMark = new XRectFigure(DrawingHelper.CenterPoint(boundingRect), 10, new Pen(Color.Red));
                }
                else
                {
                    defectMark = new RectangleFigure(boundingRect, new Pen(Color.Red));
                }

                defectMark.Tag = defectInfo;

                figureGroup.AddFigure(defectMark);
            }
            //*/

            TextFigure textFigure;
            int point = 200;
            if (inspectResult.DeviceIndex == 1)
                point = 1848;

            if (inspectResult.Judgment == Judgment.Accept)
                textFigure = new TextFigure(StringManager.GetString("OK"), new Point(point, 10), new Font("Arial", 150), Color.Green);
            else if (inspectResult.Judgment == Judgment.Skip)
                textFigure = new TextFigure(StringManager.GetString("Skip"), new Point(point, 10), new Font("Arial", 150), Color.Green);
            else
                textFigure = new TextFigure(StringManager.GetString("NG"), new Point(point, 10), new Font("Arial", 150), Color.Red);

            figureGroup.AddFigure(textFigure);
            figureGroup.Selectable = false;

            CanvasPanel curDrawBox = canvasPanel[inspectResult.DeviceIndex];
            if (curDrawBox.Image == null)
                curDrawBox.UpdateImage(inspectResult.DisplayBitmap);

            curDrawBox.WorkingFigures.Clear();
            curDrawBox.WorkingFigures.AddFigure(figureGroup);
            curDrawBox.Invalidate();
        }

        bool[] onUpdateResult = new bool[2];
        bool onUpdate = false;
        delegate void UpdateResultListDelegate(List<Data.InspectionResult> inspectResult);
        void UpdateResult2(Data.InspectionResult inspectResult)
        {
            Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;
            List<Data.InspectionResult> inspectResultList = new List<Data.InspectionResult>();
            inspectResultList.Add(inspectResult);
            
            if (InvokeRequired)
            {
                if (asyncUpdatePage != null)
                {
                    if (asyncUpdatePage.IsCompleted == false)
                    {
                        return;
                    }
                }

                asyncUpdatePage = BeginInvoke(new UpdateResultDelegate(UpdateResult2), inspectResult);// = BeginInvoke(new UpdateResultDelegate(UpdateResult), inspectResult);
                return;
            }

            UpdateResultList(inspectResultList);
        }

        void UpdateResultList(List<Data.InspectionResult> inspectResultList)
        {
            foreach (Data.InspectionResult inspectionResult in inspectResultList)
            {
                //if (lastUpdatedCamIndex != inspectionResult.DeviceIndex)
                {
                    UpdateResult4(inspectionResult);
                    //UpdateResultWithOutCanvasPanel(inspectionResult);
                    break;
                }
            }
        }

        void UpdateResult3(Data.InspectionResult inspectResult)
        { 
            if (inspectResult == null)
                return;

            lastUpdatedCamIndex = inspectResult.DeviceIndex;

            FigureGroup figureGroup = new FigureGroup();
            ///*
            foreach (DefectInfo defectInfo in inspectResult.LastDefectInfoList)
            {
                Figure defectMark;
                RectangleF boundingRect = defectInfo.BoundingRect;
                if (boundingRect.Width < 10 || boundingRect.Height < 10)
                {
                    defectMark = new EllipseFigure(DrawingHelper.CenterPoint(boundingRect), 10, new Pen(Color.Red));
                }
                else if (boundingRect.Width > 1000 || boundingRect.Height > 1000)
                {
                    defectMark = new XRectFigure(DrawingHelper.CenterPoint(boundingRect), 10, new Pen(Color.Red));
                }
                else
                {
                    defectMark = new RectangleFigure(boundingRect, new Pen(Color.Red));
                }

                defectMark.Tag = defectInfo;

                figureGroup.AddFigure(defectMark);
            }
            //*/

            TextFigure textFigure;
            int point = 200;
            if (inspectResult.DeviceIndex == 1)
                point = 1848;

            if (inspectResult.Judgment == Judgment.Accept)
                textFigure = new TextFigure("OK", new Point(point, 10), new Font("Arial", 150), Color.Green);
            else if (inspectResult.Judgment == Judgment.Skip)
                textFigure = new TextFigure("Skip", new Point(point, 10), new Font("Arial", 150), Color.Green);
            else
                textFigure = new TextFigure("NG", new Point(point, 10), new Font("Arial", 150), Color.Red);

            figureGroup.AddFigure(textFigure);

            CanvasPanel curDrawBox = canvasPanel[inspectResult.DeviceIndex];
            if (curDrawBox.Image == null)
            {
                curDrawBox.UpdateImage(ImageHelper.Resize(inspectResult.DisplayBitmap, 0.2f, 0.2f));
                curDrawBox.ZoomFit();
            }
            else
            {
                curDrawBox.UpdateImage(ImageHelper.Resize(inspectResult.DisplayBitmap, 0.2f, 0.2f));
            }

            curDrawBox.WorkingFigures.Clear();
            curDrawBox.WorkingFigures.AddFigure(figureGroup);
            curDrawBox.Invalidate();

            lastDefectPanel[inspectResult.DeviceIndex].UpdateLastDefect(inspectResult);
        }

        void UpdateResult4(Data.InspectionResult inspectResult)
        {
            if (inspectResult == null)
                return;

            lastUpdatedCamIndex = inspectResult.DeviceIndex;

            FigureGroup figureGroup = new FigureGroup();

            TextFigure textFigure;
            int point = 40;
            if (inspectResult.DeviceIndex == 1)
                point = 360;
            //float zoomSize = 
            float resizeRatio = PinholeSettings.Instance().ResizeRatio;
            if (inspectResult.Judgment == Judgment.Accept)
                textFigure = new TextFigure("OK", new Point(point, 30), new Font("Arial", 150 * resizeRatio), Color.Green);
            else if (inspectResult.Judgment == Judgment.Skip)
                textFigure = new TextFigure("Skip", new Point(point, 30), new Font("Arial", 150 * resizeRatio), Color.Green);
            else
                textFigure = new TextFigure("NG", new Point(point, 30), new Font("Arial", 150 * resizeRatio), Color.Red);

            PointF startPt;
            PointF endPt;
            LineFigure interestRegionFigure = null;
            if (inspectResult != null)
            {
                if(inspectResult.InterestRegion != null)
                {
                    if (inspectResult.DeviceIndex == 1)
                    {
                        startPt = new PointF(inspectResult.InterestRegion.Right * 0.2f, 0);
                        endPt = new PointF(inspectResult.InterestRegion.Right * 0.2f, inspectResult.InterestRegion.Height* 0.2f);
                    }
                    else
                    {
                        startPt = new PointF(inspectResult.InterestRegion.X * 0.2f, 0);
                        endPt = new PointF(inspectResult.InterestRegion.X * 0.2f, inspectResult.InterestRegion.Height*0.2f);
                    }
                    interestRegionFigure = new LineFigure(startPt, endPt, new Pen(Color.Yellow, 2));
                }
            }
            
            figureGroup.AddFigure(textFigure);
            figureGroup.Selectable = false;
            if (interestRegionFigure != null)
                figureGroup.AddFigure(interestRegionFigure);

            CanvasPanel curDrawBox = canvasPanel[inspectResult.DeviceIndex];
            lock(inspectResult.DisplayBitmap)
                curDrawBox.UpdateImage(inspectResult.DisplayBitmap);
            curDrawBox.WorkingFigures.Clear();
            curDrawBox.WorkingFigures.AddFigure(figureGroup);
            curDrawBox.ZoomFit();
            //curDrawBox.Invalidate();

            lastDefectPanel[inspectResult.DeviceIndex].UpdateLastDefect(inspectResult);
        }

        void UpdateResultWithOutCanvasPanel(Data.InspectionResult inspectResult)
        {
            if (inspectResult == null)
                return;

            lastUpdatedCamIndex = inspectResult.DeviceIndex;

            lastDefectPanel[inspectResult.DeviceIndex].UpdateLastDefect(inspectResult);
        }
        
        private void process1_Exited(object sender, EventArgs e)
        {

        }
        
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize() { }
        public delegate void ClearPanelDelegate();
        public void ClearPanel()
        {
            if(InvokeRequired)
            {
                Invoke(new ClearPanelDelegate(ClearPanel));
                return;
            }

            Array.ForEach(canvasPanel, f =>
            {
                f.ClearFigure();
                f.UpdateImage(null);
            });

            Array.ForEach(lastDefectPanel, f =>
            {
                f.Clear();
            });
        }

        public void EnterWaitInspection() { }
        public void ExitWaitInspection() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, DynMvp.InspData.InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, DynMvp.InspData.InspectionResult targetInspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(DynMvp.Data.Model model = null) { }
        public void InfomationChanged(object obj = null) { }
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        public static void DoubleBuffered(this Label label, bool setting)
        {
            Type dgvType = label.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(label, setting, null);
        }
    }
}
