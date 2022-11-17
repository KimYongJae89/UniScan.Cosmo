using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.UI;
using System.Drawing.Drawing2D;
using DynMvp.Base;
using System.Drawing.Imaging;
using DynMvp.Properties;

namespace DynMvp.UI
{
    public enum DragMode { Add, Select, Pan, Zoom, Measure, Custom };

    public partial class CanvasPanel : UserControl
    {
        const long maxSizeByte = 2147483648;    // 2GB
        public class Option
        {
            private Pen pen = null;
            public Pen Pen
            {
                get { return pen; }
                set { pen = value; }
            }

            private bool showNumber;
            public bool ShowProbeNumber
            {
                get { return showNumber; }
                set { showNumber = value; }
            }

            private int probeNumberSize = 20;
            public int ProbeNumberSize
            {
                get { return probeNumberSize; }
                set { probeNumberSize = value; }
            }

            private bool includeProbe;
            public bool IncludeProbe
            {
                get { return includeProbe; }
                set { includeProbe = value; }
            }
        }

        public delegate Figure CreateCustomFigureDelegate(PointF pt1, PointF pt2);
        public delegate void FigureCreatedDelegate(Figure figure, CoordMapper coordMapper, FigureGroup workingFigures, FigureGroup backgroundFigures);
        public delegate void FigureSelectedDelegate(List<Figure> figureList);
        public delegate void FigureDeletedDelegate(List<Figure> figureList);
        public delegate void FigureCopiedDelegate(List<Figure> figureList);
        public delegate void FigurePastedDelegate(List<Figure> figureList, FigureGroup workingFigures, FigureGroup backgroundFigures, SizeF pasteOffset);
        public delegate void FigureModifiedDelegate(List<Figure> figureList);
        public delegate void FigureFocusedDelegate(Figure figure);
        public delegate void FigureClickedDelegate(Figure figure);
        public delegate void MouseClickedDelegate(PointF point, ref bool processingCancelled);
        public delegate void MouseDblClickedDelegate();
        public delegate void MouseLeavedDelegate();

        /// <summary>
        /// 내부에서 개체를 생성하고, 생성된 객체를 참조하는 Figure를 반환한다.
        /// </summary>
        public CreateCustomFigureDelegate CreateCustomFigure;
        /// <summary>
        /// Drag완료 후 새로운 Figure를 생성한 후 호출한다. FigureCreated는 Figure에 연관된 객체를 생성하여 Tag에 저장한다.
        /// 추가적인 Figure의 생성이 필요할 경우 새로운 Figure를 생성하여 additionalFIgureList에 추가한다.
        /// </summary>
        public FigureCreatedDelegate FigureCreated;
        public FigureSelectedDelegate FigureSelected;
        public FigureCopiedDelegate FigureCopied;
        public FigureDeletedDelegate FigureDeleted;
        public FigureModifiedDelegate FigureModified;
        public FigureFocusedDelegate FigureFocused;
        public FigureClickedDelegate FigureClicked;
        /// <summary>
        /// 새로운 Figure의 목록이 생성된 후, 이 Delegation이 호출된다.
        /// 필요에 따라 Figure.Tag에 등록된 정보를 이용하여 Data 객체를 생성해야 한다.
        /// </summary>
        public FigurePastedDelegate FigurePasted;
        /// <summary>
        /// Mouse Clicked
        /// </summary>
        public MouseClickedDelegate MouseClicked;
        public MouseDblClickedDelegate MouseDblClicked;
        public MouseLeavedDelegate MouseLeaved;

        DragMode curDragMode = DragMode.Select;
        DragMode dragMode = DragMode.Select;
        public DragMode DragMode
        {
            get { return dragMode; }
            set { dragMode = value; }
        }

        Bitmap image;
        public Bitmap Image
        {
            get { return image; }
        }

        RectangleF imageRegion;
        public RectangleF ImageRegion
        {
            get { return imageRegion; }
            set { imageRegion = value; }
        }

        RectangleF canvasRegion;
        public RectangleF CanvasRegion
        {
            get { return canvasRegion; }
        }

        private Bitmap overlayImage;
        public Bitmap OverlayImage
        {
            get { return overlayImage; }
            set
            {
                overlayImage = value;
                overlayPos = new Point(0, 0);
            }
        }

        private Point overlayPos;
        public Point OverlayPos
        {
            get { return overlayPos; }
            set { overlayPos = value; }
        }

        FigureGroup workingFigures = new FigureGroup();
        public FigureGroup WorkingFigures
        {
            get { return workingFigures; }
            set { workingFigures = value; }
        }

        FigureGroup backgroundFigures = new FigureGroup();
        public FigureGroup BackgroundFigures
        {
            get { return backgroundFigures; }
        }

        FigureGroup tempFigures = new FigureGroup();
        public FigureGroup TempFigures
        {
            get { return tempFigures; }
        }

        /// <summary>
        /// 마우스가 지나가고 있는 위치에 있는 Figure
        /// </summary>
        Figure focusedFigure = null;
        Figure lastFocusedFigure = null;

        bool showRuler;
        public bool ShowRuler
        {
            get { return showRuler; }
            set { showRuler = value; }
        }

        bool showCenterGuide = true;
        public bool ShowCenterGuide
        {
            get { return showCenterGuide; }
            set { showCenterGuide = value; }
        }

        Point centerGuidePos;
        public Point CenterGuidePos
        {
            get { return centerGuidePos; }
            set { centerGuidePos = value; }
        }

        int centerGuideThickness;
        public int CenterGuideThickness
        {
            get { return centerGuideThickness; }
            set { centerGuideThickness = value; }
        }

        bool hideFigure = false;
        public bool HideFigure
        {
            get { return hideFigure; }
            set { hideFigure = value; }
        }

        private bool useZoom = true;
        public bool UseZoom
        {
            get { return useZoom; }
            set { useZoom = value; }
        }

        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
        public HorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set { horizontalAlignment = value; }
        }

        bool invertY = false;
        public bool InvertY
        {
            get { return invertY; }
            set { invertY = value; }
        }

        private bool enable = false;
        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        private bool readOnly = false;
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        private bool editable = false;
        public bool Editable
        {
            get { return editable; }
            set { editable = value; }
        }

        public bool ShowToolbar
        {
            get { return statusBar.Visible; }
            set { statusBar.Visible = value; }
        }
        
        public bool ShowModeToolBar
        {
            set
            {
                statusBar.Panels["Mode"].Visible = value;
                statusBar.Panels["Add"].Visible = value;
                statusBar.Panels["Pan"].Visible = value;
                statusBar.Panels["Select"].Visible = value;
                statusBar.Panels["Measure"].Visible = value;
                statusBar.Panels["Cross"].Visible = value;
            }
        }
        
        public bool ShowEditToolBar
        {
            set
            {
                statusBar.Panels["Edit"].Visible = value;
                statusBar.Panels["Copy"].Visible = value;
                statusBar.Panels["Paste"].Visible = value;
            }
        }
        
        public bool ShowShapeToolBar
        {
            set
            {
                statusBar.Panels["Shape"].Visible = value;
                statusBar.Panels["Rectangle"].Visible = value;
                statusBar.Panels["Circle"].Visible = value;
                statusBar.Panels["Polygon"].Visible = value;
            }
        }
        
        public bool ShowZoomToolBar
        {
            set
            {
                statusBar.Panels["Zoom"].Visible = value;
                statusBar.Panels["ZoomIn"].Visible = value;
                statusBar.Panels["ZoomOut"].Visible = value;
                statusBar.Panels["ZoomFit"].Visible = value;
                statusBar.Panels["ZoomRange"].Visible = value;
            }
        }

        private bool rotationLocked = false;
        public bool RotationLocked
        {
            get { return rotationLocked; }
            set { rotationLocked = value; }
        }

        private bool singleAxisTracking = false;
        public bool SingleAxisTracking
        {
            get { return singleAxisTracking; }
            set { singleAxisTracking = value; }
        }

        private bool noneClickMode = false;
        public bool NoneClickMode
        {
            get { return noneClickMode; }
            set { noneClickMode = value; }
        }

        private List<PointF> trackPointList = new List<PointF>();
        public List<PointF> TrackPointList
        {
            get { return trackPointList; }
        }
        
        private FigureType trackerShape = FigureType.Rectangle;
        public FigureType TrackerShape
        {
            get { return trackerShape; }
            set { trackerShape = value; }
        }

        public int ClientHeight
        {
            get { return Height - (statusBar.Visible ? statusBar.Height : 0); }
        }

        float zoomScale = 1;

        bool onDrag = false;
        PointF dragStart;
        PointF dragEnd;
        SizeF dragOffset;

        SelectionContainer selectionContainer = new SelectionContainer();
        private bool onUpdateStateButton;
        private TrackPos curTrackPos;
        List<Figure> copyBuffer = new List<Figure>();
        private int copyCount;

        public CanvasPanel()
        {
            InitializeComponent();
        }

        public CanvasPanel(bool noneClickMode)
        {
            InitializeComponent();
            this.noneClickMode = noneClickMode;
        }

        public void ClearFigure()
        {
            workingFigures.Clear();
            backgroundFigures.Clear();
            tempFigures.Clear();
            selectionContainer.ClearSelection();

            Invalidate();
        }

        public void SetAddMode(FigureType trackerShape)
        {
            Cursor = Cursors.Cross;
            dragMode = DragMode.Add;
            this.trackerShape = trackerShape;
        }

        public RectangleF GetBoundRect()
        {
            RectangleF boundRect = workingFigures.GetRectangle().ToRectangleF();
            if (backgroundFigures != null)
                boundRect = DrawingHelper.GetUnionRect(boundRect, backgroundFigures.GetRectangle().ToRectangleF());
            if (tempFigures != null)
                boundRect = DrawingHelper.GetUnionRect(boundRect, tempFigures.GetRectangle().ToRectangleF());

            return DrawingHelper.GetUnionRect(boundRect, imageRegion);
        }

        public void AddFigure(Figure figure)
        {
            selectionContainer.AddFigure(figure);
        }

        public void SelectFigure(Figure figure)
        {
            if (noneClickMode)
                return;
            selectionContainer.ClearSelection();
            selectionContainer.AddFigure(figure);
        }

        public void SelectFigure(List<Figure> figureList)
        {
            if (noneClickMode)
                return;
            selectionContainer.ClearSelection();
            selectionContainer.AddFigure(figureList);
        }

        public void SelectFigureByTag(List<Object> tagList)
        {
            if (noneClickMode)
                return;

            foreach(Object tag in tagList)
                selectionContainer.AddFigure(workingFigures.GetFigureByTag(tag));
        }

        public void SelectFigureByTag(Object tag)
        {
            if (noneClickMode)
                return;

            selectionContainer.AddFigure(workingFigures.GetFigureByTag(tag));
        }

        public void DeleteSelection()
        {
            List<Figure> figureList = selectionContainer.GetRealFigures();
            //foreach (Figure selectedFigure in figureList)
            //{
            //    workingFigures.RemoveFigure(selectedFigure);
            //}

            if (FigureDeleted != null)
                FigureDeleted(figureList);

            selectionContainer.ClearSelection();
            Invalidate(true);
        }

        public void ClearSelection()
        {
            selectionContainer.ClearSelection();
            Invalidate(true);
        }

        public void UpdateImage(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                this.image?.Dispose();
                this.image = null;
                
                Invalidate();

                return;
            }

            //if(bitmap.Height>30120)
            //    return;
            UpdateImage(bitmap, new RectangleF(0, 0, bitmap.Width, bitmap.Height));
        }

        public void UpdateImage(Bitmap image, RectangleF imageRegion)
        {
            bool zoomFitRequired;
            lock (this)
            {
                zoomFitRequired = (this.image == null) || this.image.PixelFormat == PixelFormat.DontCare || (this.image.Size != image.Size);
                //this.image?.Dispose();
                //this.image = ImageHelper.CloneImage(image);
                this.image = image;
                this.imageRegion = imageRegion;
            }
            //ZoomFit();
            //zoomFitRequired = false;
            if (zoomFitRequired)
                ZoomFit();
            else
                Invalidate();
        }

        private List<GraphicsPath> GetTrackPath()
        {
            RotatedRect trackRect;
            List<GraphicsPath> trackPathList = new List<GraphicsPath>();

            CoordMapper coordMapper = GetCoordMapper();
            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);
            SizeF scaledDragOffset = new SizeF(0, 0); // coordMapper.PixelToWorld(dragOffset);

            GraphicsPath graphicsPath = new GraphicsPath();

            if (curDragMode == DragMode.Pan)
            {
                graphicsPath.AddLine(scaledDragStart, scaledDragEnd);
                trackPathList.Add(graphicsPath);

                return trackPathList;
            }
            else if (curDragMode == DragMode.Select && curTrackPos.PosType != TrackPosType.None)
            {
                selectionContainer.GetTrackPath(trackPathList, scaledDragOffset, curTrackPos);

                return trackPathList;
            }

            trackRect = new RotatedRect();
            trackRect.FromLTRB(Math.Min(scaledDragStart.X, scaledDragEnd.X), Math.Min(scaledDragStart.Y, scaledDragEnd.Y),
                            Math.Max(scaledDragStart.X, scaledDragEnd.X), Math.Max(scaledDragStart.Y, scaledDragEnd.Y));

            switch (trackerShape)
            {
                default:
                case FigureType.Rectangle:
                    graphicsPath.AddRectangle(trackRect.ToRectangleF());
                    break;
                case FigureType.Ellipse:
                    graphicsPath.AddEllipse(trackRect.ToRectangleF());
                    break;
                case FigureType.Line:
                    if (singleAxisTracking)
                    {
                        if (Math.Abs(dragOffset.Width) > Math.Abs(dragOffset.Height))
                            graphicsPath.AddLine(scaledDragStart, new PointF(scaledDragEnd.X, scaledDragStart.Y));
                        else
                            graphicsPath.AddLine(scaledDragStart, new PointF(scaledDragStart.X, scaledDragEnd.Y));
                    }
                    else
                    {
                        graphicsPath.AddLine(scaledDragStart, scaledDragEnd);
                    }
                    break;
                case FigureType.Polygon:
                    graphicsPath.AddLines(this.trackPointList.ToArray());
                    break;
            }

            trackPathList.Add(graphicsPath);

            return trackPathList;
        }

        CoordMapper GetCoordMapper()
        {
            Matrix m = new Matrix();

            if (invertY)
            {
                m.Scale(zoomScale, -zoomScale);
                m.Translate(-canvasRegion.X, -(canvasRegion.Height + canvasRegion.Y));
            }
            else
            {
                m.Scale(zoomScale, zoomScale);
                m.Translate(-canvasRegion.X, -canvasRegion.Y);
            }

            if (curDragMode == DragMode.Pan)
                m.Translate(dragOffset.Width, dragOffset.Height, MatrixOrder.Append);

            return new CoordMapper(m);
        }

        private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            if (System.Threading.Monitor.IsEntered(this))
            {
                System.Diagnostics.Debug.WriteLine("lock힝");
                return;
            }

            System.Threading.Monitor.Enter(this);

            int clientHeight = this.ClientHeight;

            Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);

            e.Graphics.SetClip(clientRect, CombineMode.Replace);

            Matrix preTransform = e.Graphics.Transform;
            e.Graphics.Transform = GetCoordMapper().Matrix;

            if (image != null)
            {
                //lock (this)
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;

                try
                {
                    e.Graphics.DrawImage(image, imageRegion);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Operation, string.Format("CanvasPanel::CanvalPanel_Paint - {0}", ex.Message));
                }

                    //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                    //e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
                
            }

            if (overlayImage != null)
            {
                ColorMatrix colorMatrix = new ColorMatrix();
                ImageAttributes ia = new ImageAttributes();

                Rectangle imgRect = new Rectangle(overlayPos.X, overlayPos.Y, overlayImage.Width, overlayImage.Height);
                colorMatrix.Matrix33 = 0.50f;
                ia.SetColorMatrix(colorMatrix);

                e.Graphics.DrawImage(overlayImage, imgRect, 0, 0, overlayImage.Width, overlayImage.Height, GraphicsUnit.Pixel, ia);
            }

            if (showCenterGuide)
            {
                e.Graphics.Transform = preTransform;

                Pen pen = new Pen(Color.Blue, centerGuideThickness)
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                };

                e.Graphics.DrawLine(pen, new PointF(0, clientHeight / 2 + centerGuidePos.X), new PointF(Width, clientHeight / 2 + centerGuidePos.X));
                e.Graphics.DrawLine(pen, new PointF(Width / 2 + centerGuidePos.Y, 0), new PointF(Width / 2 + centerGuidePos.Y, clientHeight));

                e.Graphics.Transform = GetCoordMapper().Matrix;
            }

            if (onDrag)
            {
                using (Pen p = new Pen(Color.Red, 3))
                {
                    p.DashStyle = DashStyle.Dot;

                    List<GraphicsPath> trackPathList = GetTrackPath();
                    foreach (GraphicsPath graphicsPath in trackPathList)
                    {
                        e.Graphics.DrawPath(p, graphicsPath);
                    }
                }
            }

            if (hideFigure == false)
            {
                if (workingFigures != null)
                    lock(workingFigures)
                    workingFigures.Draw(e.Graphics, null, false);
                if (tempFigures != null)
                    lock(tempFigures)
                        tempFigures.Draw(e.Graphics, null, false);
                if (backgroundFigures != null)
                    lock(backgroundFigures)
                        backgroundFigures.Draw(e.Graphics, null, false);
            }

            if (focusedFigure != null)
            {
                using (Pen p = new Pen(Color.OrangeRed, 0))
                {
                    p.DashStyle = DashStyle.Dot;

                    GraphicsPath focusedFigurePath = focusedFigure.GetGraphicsPath();
                    e.Graphics.DrawPath(p, focusedFigurePath);
                }
            }

            e.Graphics.Transform = preTransform;

            selectionContainer.Draw(e.Graphics, GetCoordMapper(), rotationLocked);

            System.Threading.Monitor.Exit(this);
        }

        private void CanvasPanel_SizeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void UpdateCursor(DragMode dragMode)
        {
            switch (dragMode)
            {
                case DragMode.Add:
                    Cursor = Cursors.Cross;
                    break;
                case DragMode.Pan:
                    Cursor = Cursors.Hand;
                    break;
                case DragMode.Measure:
                case DragMode.Zoom:
                    //Cursor = new Cursor(new System.IO.MemoryStream(DynMvp.Properties.Resources.zoom_in));
                    //Cursor = new Cursor(GetType(), "zoom_in.cur");
                    //Cursor = new Cursor(@"D:\Project\PrintEye\Source\DynMvp\Resources\zoom-in.cur");
                    break;
                default:
                    Cursor = Cursors.Arrow;
                    break;
            }
        }

        private void CanvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (noneClickMode)
                return;

            dragStart = dragEnd = new PointF(e.X, e.Y);
            dragOffset = new SizeF(0, 0);
            onDrag = true;

            if (Control.ModifierKeys == Keys.Shift)
                curDragMode = DragMode.Pan;
            else if (Control.ModifierKeys == Keys.Alt)
                curDragMode = DragMode.Select;
            else
                curDragMode = dragMode;

            if (curDragMode == DragMode.Select)
            {
                CoordMapper coordMapper = GetCoordMapper();
                curTrackPos = selectionContainer.GetTrackPos(coordMapper, dragStart, rotationLocked);
            }

            UpdateCursor(curDragMode);
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateCursor(dragMode);

            CoordMapper coordMapper = GetCoordMapper();

            if (onDrag == true)
            {
                dragOffset = new SizeF(e.X - dragStart.X, e.Y - dragStart.Y);
                SizeF dragOffset2 = new SizeF(e.X - dragEnd.X, e.Y - dragEnd.Y);
                dragEnd = new PointF(e.X, e.Y);

                SizeF size = coordMapper.PixelToWorld(dragOffset);
                statusBar.Panels["Size"].Text = String.Format("{0:0.00}, {1:0.00}", size.Width, size.Height);

                SizeF size2 = coordMapper.PixelToWorld(dragOffset2);
                selectionContainer.TrackMove(curTrackPos, size2, true, false);
                Invalidate(true);
            }
            else
            {
                PointF point = coordMapper.PixelToWorld(new PointF(e.X, e.Y));
                statusBar.Panels["Pos"].Text = String.Format("{0:0.00}, {1:0.00}", point.X, point.Y);

                focusedFigure = workingFigures.Select(point);
                if (focusedFigure != lastFocusedFigure)
                {
                    FigureFocused?.Invoke(focusedFigure);

                    Invalidate(true);
                    lastFocusedFigure = focusedFigure;
                }
            }
        }

        private void CanvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (noneClickMode)
                return;

            dragEnd = new PointF(e.X, e.Y);
            dragOffset = new SizeF(e.X - dragStart.X, e.Y - dragStart.Y);

            CoordMapper coordMapper = GetCoordMapper();
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

            bool processingCancelled = false;
            if (dragStart == dragEnd)
            {
                MouseClicked?.Invoke(scaledDragEnd, ref processingCancelled);
                Figure selectedFigure = this.workingFigures.FigureList.Find(f => f.GetRectangle().GetBoundRect().Contains(scaledDragEnd));
                if (selectedFigure != null)
                    FigureClicked?.Invoke(selectedFigure);
            }
            if (processingCancelled == true)
                return;

            if (onDrag == true)
            {
                switch (curDragMode)
                {
                    case DragMode.Add:
                        AddFigure();
                        break;
                    case DragMode.Pan:
                        Pan();
                        break;
                    case DragMode.Select:
                        if (/*(focusedFigure != null && selectionContainer.IsSelected(focusedFigure) == false) ||*/ curTrackPos.PosType == TrackPosType.None)
                            SelectRange();
                        else
                            ModifyFigure();
                        break;
                    case DragMode.Measure:
                        Measure();
                        break;
                    case DragMode.Zoom:
                        ZoomRange();
                        break;
                }

                statusBar.Panels["Size"].Text = "";
                dragOffset = new SizeF(0, 0);
                Invalidate();
                onDrag = false;
                curDragMode = DragMode.Select;
            }
        }

        Figure CreateFigure(PointF pt1, PointF pt2)
        {
            Figure figure = null;
            switch (trackerShape)
            {
                case FigureType.Ellipse:
                    figure = new EllipseFigure(DrawingHelper.FromPoints(pt1, pt2), new Pen(Color.Red));
                    break;
                case FigureType.Rectangle:
                    figure = new RectangleFigure(DrawingHelper.FromPoints(pt1, pt2), new Pen(Color.Red));
                    break;
            }

            return figure;
        }

        void AddFigure()
        {
            selectionContainer.ClearSelection();

            if (Math.Abs(dragOffset.Width) < 5 && Math.Abs(dragOffset.Height) < 5)
                return;

            CoordMapper coordMapper = GetCoordMapper();
            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

            Figure figure = null;
            
            // 적용 X
            //if (trackerShape == FigureType.Custom)
            //{
            //    if (CreateCustomFigure != null)
            //        figure = CreateCustomFigure(scaledDragStart, scaledDragEnd);
            //}
            //else
            //{
            //    figure = CreateFigure(scaledDragStart, scaledDragEnd);
            //}

            figure = CreateFigure(scaledDragStart, scaledDragEnd);

            if (figure != null)
            {
                //FigureGroup workingFigures = new FigureGroup();
                FigureGroup backgroundFigures = new FigureGroup();
                if (FigureCreated != null)
                {
                    FigureCreated(figure, coordMapper, workingFigures, backgroundFigures);

                    // 여기는 뭔가요... 컬랙션 수정됨 Exception...
                    //foreach(Figure createdFigure in workingFigures)
                    //    this.workingFigures.AddFigure(createdFigure);

                    //foreach(Figure createdFigure in backgroundFigures)
                    //    this.backgroundFigures.AddFigure(createdFigure);

                    //selectionContainer.AddFigure(workingFigures[0]);
                    selectionContainer.AddFigure(figure);
                }
                else
                {
                    workingFigures.AddFigure(figure);
                    selectionContainer.AddFigure(figure);
                }
                //figure.Tag = DateTime.Now.ToLongTimeString();
            }

            Invalidate();
        }

        void Pan()
        {
            int clientHeight = this.ClientHeight;

            Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);

            if (invertY)
                canvasRegion = new RectangleF(canvasRegion.X - dragOffset.Width / zoomScale, canvasRegion.Y + dragOffset.Height / zoomScale,
                                        canvasRegion.Width, canvasRegion.Height);
            else
                canvasRegion = new RectangleF(canvasRegion.X - dragOffset.Width / zoomScale, canvasRegion.Y - dragOffset.Height / zoomScale,
                                        canvasRegion.Width, canvasRegion.Height);
        }

        void ModifyFigure()
        {
            CoordMapper coordMapper = GetCoordMapper();
            SizeF scaledDragOffset = coordMapper.PixelToWorld(dragOffset);

            if (curTrackPos.PosType == TrackPosType.Inner)
                selectionContainer.Offset(scaledDragOffset);
            else
                selectionContainer.TrackMove(curTrackPos, scaledDragOffset, rotationLocked, true);

            if (FigureModified != null)
            {
                FigureModified(selectionContainer.GetRealFigures());
            }

            Invalidate(true);
        }

        void SelectRange()
        {
            if (Control.ModifierKeys != Keys.Control)
                selectionContainer.ClearSelection();

            CoordMapper coordMapper = GetCoordMapper();

            List<Figure> figureList = new List<Figure>();

            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);

            // 선택 영역이 작으면 시작위치에 있는 객체를 얻어오고
            // 선택 영역이 크면 시작 위치와 종료 위치의 Rectangle 영역안의 객체 목록을 얻어 온다.
            if (Math.Abs(dragOffset.Width) > 5 && Math.Abs(dragOffset.Height) > 5)
            {
                PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

                RectangleF selectedRect = DrawingHelper.FromPoints(scaledDragStart, scaledDragEnd);
                figureList = workingFigures.Select(Rectangle.Round(selectedRect));
                figureList.Sort( (x, y) => x.ObjectLevel - y.ObjectLevel );

                selectionContainer.AddFigure(figureList);
            }
            else
            {
                Figure figure = workingFigures.Select(scaledDragStart);
                if (figure != null)
                {
                    selectionContainer.AddFigure(figure);

                    figureList.Add(figure);
                }
            }

            if (FigureSelected != null)
                FigureSelected(figureList);
        }

        void Measure()
        {

        }

        void ZoomRange()
        {
            CoordMapper coordMapper = GetCoordMapper();

            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

            ZoomRange(DrawingHelper.FromPoints(scaledDragStart, scaledDragEnd));
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (noneClickMode)
                return;
            base.OnMouseWheel(e);

            UpdateZoom((float)(e.Delta > 0 ? 1.5 : 0.5), new PointF(e.X, e.Y));
        }

        private void UpdateZoom(float zoomOffset, PointF zoomCenter)
        {
            float newZoomScale = zoomScale * zoomOffset; // (float)Math.Pow(2, zoomStep);
            if (newZoomScale == 0)
                return;

            int clientHeight = this.ClientHeight;

            Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);

            SizeF newCanvasSize = new SizeF(clientRect.Width / newZoomScale, clientRect.Height / newZoomScale);

            PointF curZoomPos;
            PointF newLeftTopPos;

            if (invertY)
            {
                curZoomPos = new PointF(canvasRegion.X + zoomCenter.X / zoomScale, canvasRegion.Y + (clientRect.Height - zoomCenter.Y) / zoomScale);
                newLeftTopPos = new PointF(curZoomPos.X - zoomCenter.X / newZoomScale, curZoomPos.Y - (clientRect.Height - zoomCenter.Y) / newZoomScale);
            }
            else
            {
                curZoomPos = new PointF(canvasRegion.X + zoomCenter.X / zoomScale, canvasRegion.Y + zoomCenter.Y / zoomScale);
                newLeftTopPos = new PointF(curZoomPos.X - zoomCenter.X / newZoomScale, curZoomPos.Y - zoomCenter.Y / newZoomScale);
            }
            canvasRegion = new RectangleF(newLeftTopPos, newCanvasSize);

            zoomScale = newZoomScale;

            Invalidate();
        }

        public void ZoomFit()
        {
            RectangleF boundRect = GetBoundRect();
            ZoomRange(boundRect);
        }

        public void ZoomRange(RectangleF zoomRange)
        {
            if (zoomRange.Width == 0 || zoomRange.Height == 0)
                return;

            int clientHeight = this.ClientHeight;

            float scaleX = Width / zoomRange.Width;
            float scaleY = clientHeight / zoomRange.Height;
            if (scaleX <= 0 || scaleY <= 0)
                return;

            RectangleF canvasRegion = RectangleF.Empty;

            if (scaleX < scaleY)
            {
                // Width 일치
                //canvasRegion = new RectangleF(zoomRange.X, zoomRange.Y, zoomRange.Width, clientHeight / scaleX);
                zoomScale = scaleX;

                float canvasRegionH = clientHeight / scaleX;
                float canvasRegionCenterY = (canvasRegionH - zoomRange.Height) / 2;
                if (canvasRegionCenterY < 0)
                    canvasRegionCenterY = 0;

                canvasRegionH -= canvasRegionCenterY;
                canvasRegion = RectangleF.FromLTRB(zoomRange.X, zoomRange.Y- canvasRegionCenterY, canvasRegionH, zoomRange.Height);
            }
            else
            {
                // Height 일치

                //canvasRegion = new RectangleF(zoomRange.X, zoomRange.Y, Width / scaleY, zoomRange.Height);
                zoomScale = scaleY;

                float canvasRegionW = Width / scaleY;
                float canvasRegionCenterX = (canvasRegionW - zoomRange.Width) / 2;
                if (canvasRegionCenterX < 0)
                    canvasRegionCenterX = 0;

                canvasRegionW -= canvasRegionCenterX;

                if (horizontalAlignment == HorizontalAlignment.Center)
                {
                    canvasRegion = RectangleF.FromLTRB(zoomRange.X - canvasRegionCenterX, zoomRange.Y, Width / scaleY, zoomRange.Height);
                }
                else if (horizontalAlignment == HorizontalAlignment.Right)
                {
                    canvasRegion = RectangleF.FromLTRB(zoomRange.X - canvasRegionCenterX * 2, zoomRange.Y, Width / scaleY, zoomRange.Height);
                }
                else
                {
                    canvasRegion = RectangleF.FromLTRB(zoomRange.X, zoomRange.Y, Width / scaleY, zoomRange.Height);
                }

            }

            if (zoomScale != scaleX || zoomScale != scaleY || canvasRegion != this.canvasRegion)
            {
                this.canvasRegion = canvasRegion;
                Invalidate();
            }
        }

        public void ZoomIn()
        {
            int clientHeight = this.ClientHeight;
            Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);

            UpdateZoom(1.2f, DrawingHelper.CenterPoint(clientRect));
        }

        public void ZoomOut()
        {
            int clientHeight = this.ClientHeight;
            Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);

            UpdateZoom(0.8f, DrawingHelper.CenterPoint(clientRect));
        }

        private void CanvasPanel_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Delete)
            //{
            //    workingFigures.Delete(selectionContainer.Figures);
            //    selectionContainer.ClearSelection();
            //    return;
            //}

            //Size offset = new Size(0, 0);
            //if (e.KeyCode == Keys.Down)
            //    offset.Height = 1;
            //else if (e.KeyCode == Keys.Up)
            //    offset.Height = -1;
            //else if (e.KeyCode == Keys.Left)
            //    offset.Width = -1;
            //else if (e.KeyCode == Keys.Right)
            //    offset.Width = 1;

            //if (offset != new Size(0, 0))
            //{
            //    CoordMapper coordMapper = GetCoordMapper();
            //    selectionContainer.Offset(coordMapper.PixelToWorld(offset));
            //}
            //else
            //{
            //    int clientHeight = Height - statusBar.Height;

            //    Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);

            //    if (e.KeyData == Keys.Z)
            //        UpdateZoom(1.2f, DrawingHelper.CenterPoint(clientRect));
            //    else if (e.KeyData == Keys.X)
            //        UpdateZoom(0.8f, DrawingHelper.CenterPoint(clientRect));
            //}
        }

        private void statusBar_ButtonClick(object sender, Infragistics.Win.UltraWinStatusBar.PanelEventArgs e)
        {
            if (onUpdateStateButton == true)
                return;

            switch(e.Panel.Key)
            {
                case "Add":
                    dragMode = DragMode.Add;
                    Cursor = Cursors.Cross;
                    break;
                case "Pan":
                    Cursor = Cursors.Hand;
                    dragMode = DragMode.Pan;
                    break;
                case "None":
                case "Select":
                    Cursor = Cursors.Arrow;
                    dragMode = DragMode.Select;
                    break;
                case "Measure":
                    Cursor = Cursors.Arrow;
                    dragMode = DragMode.Measure;
                    break;
                case "ZoomRange":
                    Cursor = Cursors.Arrow;
                    dragMode = DragMode.Zoom;
                    break;
                case "Cross":
                    showCenterGuide = !showCenterGuide;
                    Invalidate();
                    break;
                case "ZoomFit":
                    ZoomFit();
                    break;
                case "ZoomIn":
                    ZoomIn();
                    break;
                case "ZoomOut":
                    ZoomOut();
                    break;
                case "Copy":
                    Copy();
                    break;
                case "Paste":
                    Paste();
                    break;
                case "Delete":
                    Delete();
                    break;
            }

            UpdateStateButton();
        }

        public void Copy()
        {
            copyCount = 1;
            copyBuffer.Clear();
            foreach (Figure selectedFigure in selectionContainer)
            {
                copyBuffer.Add(selectedFigure.TagFigure);
            }

            FigureCopied?.Invoke(copyBuffer);
        }

        public void Paste()
        {
            CoordMapper coordMapper = GetCoordMapper();
            SizeF pasteOffset = coordMapper.PixelToWorld(new Size(10 * copyCount, 10 * copyCount));

            FigureGroup workingFigures = new FigureGroup();
            FigureGroup backgroundFigures = new FigureGroup();
            if (FigurePasted != null)
                FigurePasted(copyBuffer, workingFigures, backgroundFigures, pasteOffset);

            foreach (Figure figure in workingFigures)
            {
                this.workingFigures.AddFigure(figure);
            }

            foreach (Figure figure in backgroundFigures)
            {
                this.backgroundFigures.AddFigure(figure);
            }

            copyCount++;

            Invalidate(true);
        }

        void Delete()
        {
            DeleteSelection();
        }

        void UpdateStateButton()
        {
            onUpdateStateButton = true;

            if (statusBar.Panels["Add"].Checked != (dragMode == DragMode.Add))
                statusBar.Panels["Add"].Checked = dragMode == DragMode.Add;
            if (statusBar.Panels["Pan"].Checked != (dragMode == DragMode.Pan))
                statusBar.Panels["Pan"].Checked = dragMode == DragMode.Pan;
            if (statusBar.Panels["Select"].Checked != (dragMode == DragMode.Select))
                statusBar.Panels["Select"].Checked = dragMode == DragMode.Select;
            if (statusBar.Panels["Measure"].Checked != (dragMode == DragMode.Measure))
                statusBar.Panels["Measure"].Checked = dragMode == DragMode.Measure;
            if (statusBar.Panels["ZoomRange"].Checked != (dragMode == DragMode.Zoom))
                statusBar.Panels["ZoomRange"].Checked = dragMode == DragMode.Zoom;

            onUpdateStateButton = false;
        }

        private void CanvasPanel_Leave(object sender, EventArgs e)
        {
            MouseLeaved?.Invoke();
        }

        private void CanvasPanel_DoubleClick(object sender, EventArgs e)
        {
            //Cursor = Cursors.Arrow;
            //dragMode = DragMode.Select;

            TempFigures.Clear();
            Invalidate();

            MouseDblClicked?.Invoke();
        }

        private void CanvasPanel_Load(object sender, EventArgs e)
        {
            //if (hideToolbar == true)
            //    statusBar.Height = 0;
            //statusBar.Visible = (hideToolbar == false);
        }

        private void CanvasPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.KeyCode == Keys.C)
                {
                    Copy();
                }
                else if (e.KeyCode == Keys.V)
                {
                    Paste();
                }
            }
            else
            {
                int accel = (Control.ModifierKeys == Keys.Alt ? 5 : 1);

                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        Delete();
                        break;
                    case Keys.Z:
                        hideFigure = !hideFigure;
                        Invalidate();
                        break;
                    case Keys.Q:
                        ZoomIn();
                        break;
                    case Keys.W:
                        ZoomOut();
                        break;
                    case Keys.Left:
                        Offset(new SizeF(-accel, 0));
                        break;
                    case Keys.Right:
                        Offset(new SizeF(accel, 0));
                        break;
                    case Keys.Up:
                        Offset(new SizeF(0, -accel));
                        break;
                    case Keys.Down:
                        Offset(new SizeF(0, accel));
                        break;
                }
            }
        }

        void Offset(SizeF size)
        {
            CoordMapper coordMapper = GetCoordMapper();
            SizeF scaledDragOffset = coordMapper.PixelToWorld(size);

            selectionContainer.Offset(scaledDragOffset);
            selectionContainer.TrackMove(new TrackPos(TrackPosType.Inner, 0), scaledDragOffset, true, false);

            if (FigureModified != null)
            {
                FigureModified(selectionContainer.GetRealFigures());
            }

            Invalidate(true);
            Update();
        }
    }
}
