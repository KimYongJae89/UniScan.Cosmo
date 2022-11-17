using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DynMvp.Devices;
using MahApps.Metro.SimpleChildWindow;
using WPF.Base.Helpers;
using WPF.Base.Services;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Controls.ViewModel
{
    public enum MoveMode
    {
        Jog, Relative
    }
    
    public class MicroscopeViewModel : Observable
    {
        #region 변수

        ChildWindow view;

        CosmoLotNoInfo _lotNoInfo;

        public double[] StepDistanceList { get; } =
        {
            1, 5, 10, 20, 30, 50, 100, 200, 300, 500, 1000
        };

        public MoveMode MoveMode { get; set; }

        public ZoomService ZoomService { get; set; }

        private ImageSource microscopeImage;
        public ImageSource MicroscopeImage
        {
            get => microscopeImage;
            set
            {
                Set(ref microscopeImage, value);
            }
        }
        
        public double ExposureTime
        {
            get => MicroscopeGrabService.Settings.Exposure;
            set
            {
                MicroscopeGrabService.SetExposure(value);
                MicroscopeGrabService.Settings.Exposure = value;
            }
        }
        public double FocusValue
        {
            get => MicroscopeGrabService.Settings.FocusValue;
            set
            {
                MicroscopeGrabService.SetFocusValue(value);
                MicroscopeGrabService.Settings.FocusValue = value;
            }
        }
        
        public int LightValue
        {
            get => MicroscopeGrabService.Settings.LightValue;
            set
            {
                MicroscopeGrabService.TurnOnLight(value);
                MicroscopeGrabService.Settings.LightValue = value;
            }
        }
        
        public int RobotSpeed
        {
            get => MicroscopeGrabService.Settings.MoveVelocity;
            set => MicroscopeGrabService.Settings.MoveVelocity = value;
        }

        private double stepMoveDistanceMM = 1;
        public double StepMoveDistanceMM
        {
            get => stepMoveDistanceMM;
            set => Set(ref stepMoveDistanceMM, value);
        }

        System.Windows.Point ptStart;

        private Rect dragRegion;
        public Rect DragRegion
        {
            get => dragRegion;
            set => Set(ref dragRegion, value);
        }

        private Tuple<int, int, ImageSource> selectedThumbnail;
        public Tuple<int, int, ImageSource> SelectedThumbnail
        {
            get => selectedThumbnail;
            set => Set(ref selectedThumbnail, value);
        }
        
        public double MinCurrent => MicroscopeGrabService.GetMinCurrent();
        public double MaxCurrent => MicroscopeGrabService.GetMaxCurrent();
        public bool IsAutoFocus => MicroscopeGrabService.State == MicroscopeState.AutoFocus;
        public bool IsLive => MicroscopeGrabService.State == MicroscopeState.Live;

        bool isJogMove = true;
        public bool IsJogMode
        {
            get => isJogMove;
            set => Set(ref isJogMove, value);
        }
        
        public ObservableCollection<Tuple<int, int, ImageSource>> ClipImageList { get; } = new ObservableCollection<Tuple<int, int, ImageSource>>();

        #endregion

        #region Command

        private ICommand addClipCommand;
        public ICommand AddClipCommand { get => addClipCommand ?? (addClipCommand = new RelayCommand(AddClip)); }
                
        private ICommand deleteClipCommand;
        public ICommand DeleteClipCommand { get => deleteClipCommand ?? (deleteClipCommand = new RelayCommand(DeleteClip)); }

        private ICommand clearClipCommand;
        public ICommand ClearClipCommand { get => clearClipCommand ?? (clearClipCommand = new RelayCommand(ClearClip)); }

        private ICommand grabCommand;
        public ICommand GrabCommand { get => grabCommand ?? (grabCommand = new RelayCommand(Grab)); }

        private ICommand liveGrabCommand;
        public ICommand LiveGrabCommand { get => liveGrabCommand ?? (liveGrabCommand = new RelayCommand<bool>(LiveGrab)); }
        
        private ICommand leftMoveStartCommand;
        public ICommand LeftMoveStartCommand { get => leftMoveStartCommand ?? (leftMoveStartCommand = new RelayCommand(LeftMoveStart)); }
                
        private ICommand rightMoveStartCommand;
        public ICommand RightMoveStartCommand { get => rightMoveStartCommand ?? (rightMoveStartCommand = new RelayCommand(RightMoveStart)); }

        private ICommand topMoveStartCommand;
        public ICommand TopMoveStartCommand { get => topMoveStartCommand ?? (topMoveStartCommand = new RelayCommand(TopMoveStart)); }

        private ICommand bottomMoveStartCommand;
        public ICommand BottomMoveStartCommand { get => bottomMoveStartCommand ?? (bottomMoveStartCommand = new RelayCommand(BottomMoveStart)); }
        
        private ICommand moveEndCommand;
        public ICommand MoveEndCommand { get => moveEndCommand ?? (moveEndCommand = new RelayCommand(StopMove)); }

        private ICommand saveResultCommand;
        public ICommand SaveResultCommand { get => saveResultCommand ?? (saveResultCommand = new RelayCommand(SaveResult)); }
        
        private ICommand folderOpenCommand;
        public ICommand FolderOpenCommand { get => folderOpenCommand ?? (folderOpenCommand = new RelayCommand(FolderOpen)); }

        private ICommand closeCommand;
        public ICommand CloseCommand { get => closeCommand ?? (closeCommand = new RelayCommand(Close)); }

        private ICommand fitToSizeCommand;
        public ICommand FitToSizeCommand { get => fitToSizeCommand ?? (fitToSizeCommand = new RelayCommand(FitToSize)); }

        private ICommand autoFocusCommand;
        public ICommand AutoFocusCommand { get => autoFocusCommand ?? (autoFocusCommand = new RelayCommand<bool>(AutoFocus)); }

        private ICommand moveDefectCommand;
        public ICommand MoveDefectCommand { get => moveDefectCommand ?? (moveDefectCommand = new RelayCommand<Defect>(MoveDefect)); }

        

        private async void MoveDefect(Defect defect)
        {
            if (defect == null)
                return;

            if (AxisGrabService.CheckDoorLock() == false)
                return;

            if (await MicroscopeGrabService.MoveDefectPos(defect) == false)
                return;
        }

        private void AddClip()
        {
            BitmapSource bmp = ClipImage(MicroscopeImage as BitmapSource, DragRegion);

            var xIndex = MicroscopeGrabService.Settings.XAxisIndex;
            var yIndex = MicroscopeGrabService.Settings.YAxisIndex;

            var axisInfo = AxisGrabService.Settings.AxisGrabInfoList.First(info => info.AxisIndex == xIndex);

            //var xStartPos = axisInfo.MinX + axisInfo.OffsetX;

            int xPos = (int)(MotionService.AxisPosition[xIndex] + 623042) / 1000;
            int yPos = (int)(MotionService.AxisPosition[yIndex] - MicroscopeGrabService.Settings.OffsetY) / 1000;
            if (bmp != null)
                ClipImageList.Add(new Tuple<int, int, ImageSource>(xPos, yPos, bmp));
        }

        private void DeleteClip()
        {
            ClipImageList.Remove(SelectedThumbnail);
        }

        private void ClearClip()
        {
            ClipImageList.Clear();
        }

        private void Grab()
        {
            MicroscopeGrabService.Grab();
        }

        private void LiveGrab(bool isChecked)
        {
            if (isChecked)
                MicroscopeGrabService.LiveGrab();
            else
                MicroscopeGrabService.StopGrab();

            OnPropertyChanged("IsLive");
        }

        private void LeftMoveStart()
        {
            if (IsJogMode)
                MicroscopeGrabService.JogMoveX(true);
            else
                MicroscopeGrabService.StepMoveX(Convert.ToSingle(-StepMoveDistanceMM * 1000));
        }

        private void RightMoveStart()
        {
            if (IsJogMode)
                MicroscopeGrabService.JogMoveX(false);
            else
                MicroscopeGrabService.StepMoveX(Convert.ToSingle(StepMoveDistanceMM * 1000));
        }

        private void TopMoveStart()
        {
            if (IsJogMode)
                MicroscopeGrabService.JogMoveY(true);
            else
                MicroscopeGrabService.StepMoveY(Convert.ToSingle(-StepMoveDistanceMM * 1000));
        }

        private void BottomMoveStart()
        {
            if (IsJogMode)
                MicroscopeGrabService.JogMoveY(false);
            else
                MicroscopeGrabService.StepMoveY(Convert.ToSingle(StepMoveDistanceMM * 1000));
        }

        private void StopMove()
        {
            if (IsJogMode)
                MicroscopeGrabService.StopMove();
        }

        private async void SaveResult()
        {
            if (ClipImageList == null || ClipImageList.Count == 0)
                return;

            await ExcelExportService.ClipImageExport(_lotNoInfo, ClipImageList.ToList());
        }

        private void FolderOpen()
        {

        }

        private async void Close()
        {
            await MicroscopeGrabService.ReleaseCameara();
            view.Close();
        }

        private void FitToSize()
        {
            if (microscopeImage != null)
                ZoomService.FitToSize(microscopeImage.Width, microscopeImage.Height);
        }

        private void AutoFocus(bool isChecked)
        {
            if (isChecked)
            {
                MicroscopeGrabService.MicroscopeGrabbed += ImageGrabbedAutoFocus;
                MicroscopeGrabService.AutoFocusGrab();
            }
            else
            {
                MicroscopeGrabService.MicroscopeGrabbed -= ImageGrabbedAutoFocus;
                MicroscopeGrabService.StopGrab();
            }

            OnPropertyChanged("IsAutoFocus");
        }

        #endregion

        List<Defect> _defects = new List<Defect>();
        public List<Defect> Defects
        {
            get => _defects;
            set => Set(ref _defects, value);
        }

        public void Initilaize(ChildWindow _view, FrameworkElement _canvas, CosmoLotNoInfo lotNoInfo, IEnumerable<Defect> defects)
        {
            view = _view;

            foreach (var defect in defects)
                Defects.Add(defect);
                
            _lotNoInfo = lotNoInfo;
            ZoomService = new ZoomService(_canvas);
            ZoomService.MouseLeftDown += OnMouseLeftDown;
            ZoomService.MouseLeftUp += OnMouseLeftUp;
            ZoomService.MouseDragRegion += OnMouseDragRegion;

            MicroscopeGrabService.MicroscopeGrabbed = null;
            MicroscopeGrabService.MicroscopeGrabbed += ImageGrabbed;
            
            MicroscopeGrabService.InitializeCameara();
        }

        private void OnMouseLeftDown(System.Windows.Point pt)
        {
            ptStart = pt;
            DragRegion = new Rect(ptStart, ptStart);
        }

        private void OnMouseLeftUp(System.Windows.Point pt)
        {
            DragRegion = new Rect(ptStart, pt);
        }

        private void OnMouseDragRegion(Rect region)
        {
            DragRegion = region;
        }
        
        private void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            bool requiredFit = microscopeImage == null;

            var image2D = imageDevice.GetGrabbedImage(ptr) as DynMvp.Base.Image2D;

            BitmapSource image = BitmapSource.Create(image2D.Width, image2D.Height,
                                                    96, 96, System.Windows.Media.PixelFormats.Gray8, null,
                                                    image2D.DataPtr, image2D.Width * image2D.Height, image2D.Width);
            
            image.Freeze();
            MicroscopeImage = image;

            if (requiredFit)
                FitToSize();
        }

        private void ImageGrabbedAutoFocus(ImageDevice imageDevice, IntPtr ptr)
        {
            var image2D = imageDevice.GetGrabbedImage(ptr) as DynMvp.Base.Image2D;
            var clone = image2D.Clone() as DynMvp.Base.Image2D;
            clone.ConvertFromDataPtr();

            if (MicroscopeGrabService.CalculateAutoFocus(clone.ToBitmap()) == false)
            {
                MicroscopeGrabService.AutoFocusGrab();
            }
            else
            {
                Grab();
                MicroscopeGrabService.MicroscopeGrabbed -= ImageGrabbedAutoFocus;
                OnPropertyChanged("IsAutoFocus");
            }

            OnPropertyChanged("FocusValue");
        }

        //private void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        //{
        //    bool requiredFit = microscopeImage == null;


        //    var image2D = imageDevice.GetGrabbedImage(ptr) as DynMvp.Base.Image2D;

        //    var clone = image2D.Clone() as DynMvp.Base.Image2D;
        //    clone.ConvertFromDataPtr();

        //    BitmapSource image = BitmapSource.Create(clone.Width, clone.Height,
        //                                            96, 96, System.Windows.Media.PixelFormats.Gray8, null,
        //                                            clone.ImageData.Data, clone.Width);

        //    if (isOnAutoFocus)
        //    {
        //        isOnAutoFocus = false;
        //        var imageSize = new System.Drawing.Size(clone.Width, clone.Height);
        //        var rect = new System.Drawing.Rectangle(imageSize.Width / 8 * 3, imageSize.Height / 8 * 3, imageSize.Width / 4, imageSize.Height / 4);

        //        MicroscopeGrabService.SetFocusRegion(rect);
        //        MicroscopeGrabService.SetReady();
        //    }

        //    if (IsStartAutoFocus)
        //    {
        //        if (MicroscopeGrabService.CalculateAutoFocus(clone.ToBitmap()))
        //            IsStartAutoFocus = false;

        //        FocusValue = MicroscopeGrabService.GetFocusValue();
        //    }

        //    image.Freeze();
        //    MicroscopeImage = image;

        //    if (requiredFit)
        //        FitToSize();
        //}

        public BitmapSource ClipImage(BitmapSource src, Rect region)
        {
            if (src == null)
                return null;

            Rect imageRect = new Rect(0, 0, src.PixelWidth, src.PixelHeight);
            imageRect.Intersect(region);

            if (imageRect == Rect.Empty)
                return null;

            Int32Rect rect = new Int32Rect(
            Convert.ToInt32(imageRect.Left),
            Convert.ToInt32(imageRect.Top),
            Convert.ToInt32(imageRect.Width),
            Convert.ToInt32(imageRect.Height));

            // Create a CroppedBitmap based off of a xaml defined resource.
            CroppedBitmap cb = new CroppedBitmap(
               src, rect);       //select region rect

            cb.Freeze();

            return cb;
        }
    }
}
