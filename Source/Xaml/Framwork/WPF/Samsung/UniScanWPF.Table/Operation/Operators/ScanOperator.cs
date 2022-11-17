using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation.Operators
{
    public enum ScanDirection
    {
        Forward, Backward
    }

    public class ScanOperator : GrabOperator
    {
        Task scanTask = null;

        ScanDirection scanDirection;
        int flowPosition;
        float fovWidthUm;
        float moveLengthUm;

        public ScanOperatorSettings Settings { get => this.settings; }
        ScanOperatorSettings settings;

        public ScanOperator() : base()
        {
            settings = new ScanOperatorSettings();

            InfoBox.Instance.UpdateRegion(settings);
        }

        public override bool Initialize(ResultKey resultKey, CancellationTokenSource cancellationTokenSource)
        {
            BufferManager.Instance().Clear();
            
            flowPosition = 0;
            scanDirection = ScanDirection.Forward;

            float resolution = DeveloperSettings.Instance.Resolution;

            this.fovWidthUm = SystemManager.Instance().DeviceBox.ImageDeviceHandler[0].ImageSize.Width * resolution;
            float overlap = settings.OverlapUm;
            this.moveLengthUm = (int)Math.Floor(this.fovWidthUm - overlap);

            return base.Initialize(resultKey, cancellationTokenSource);
        }
        
        protected override bool PrepereGrab()
        {
            DebugWriteLine("ScanOperator::PrepereGrab");
            WaitMoveDone();

            if (cancellationTokenSource.IsCancellationRequested)
                return false;

            scanTask = Task.Factory.StartNew(() =>
            {
                DebugWriteLine("ScanOperator::PrepereGrabTask Start");
                LightValue lightValue = new LightValue(2);

                float targetX = settings.Dst.X - (moveLengthUm * flowPosition) - this.fovWidthUm / 2;
                float targetY = 0;
                float sourceY = 0;
                bool increase = true;
                switch (scanDirection)
                {
                    case ScanDirection.Forward:
                        sourceY = settings.Src.Y;
                        increase = true;
                        lightValue.Value[0] = 0;
                        lightValue.Value[1] = SystemManager.Instance().OperatorManager.LightTuneOperator.Settings.InitialBackLightValue;
                        targetY = settings.Dst.Y+ DeveloperSettings.Instance.MoveOffset;
                        break;
                    case ScanDirection.Backward:
                        sourceY = settings.Dst.Y;
                        increase = false;
                        lightValue.Value[0] = SystemManager.Instance().CurrentModel.LightValueTop;
                        lightValue.Value[1] = SystemManager.Instance().OperatorManager.LightTuneOperator.Settings.InitialBackLightValue;
                        targetY = settings.Src.Y - DeveloperSettings.Instance.MoveOffset;
                        break;
                }

                if (lightValue.Value[1] <= 0)
                    lightValue.Value[1] = SystemManager.Instance().OperatorManager.LightTuneOperator.Settings.InitialBackLightValue;

                axisHandler.StartCmp("Y", (int)sourceY, (int)Math.Round(DeveloperSettings.Instance.Resolution), increase);
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(lightValue);
                imageDeviceHandler.SetStepLight(flowPosition, (int)scanDirection);
                imageDeviceHandler.GrabMulti();
                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(grabPort, true);

                axisHandler.StartMultipleMove(new AxisPosition(new float[] { targetX, targetY }), settings.Velocity);
                WaitMoveDone();

                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(grabPort, false);

                imageDeviceHandler.Stop();
                //SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
                axisHandler.EndCmp("Y");

                DebugWriteLine("ScanOperator::PrepereGrabTask End");
            }, cancellationTokenSource.Token);

            return true;
        }

        protected override bool PostGrab()
        {
            DebugWriteLine("ScanOperator::PostGrab");
            WaitMoveDone();

            scanTask?.Wait();

            if (scanDirection == ScanDirection.Forward)
            {
                float targetX = settings.Dst.X - (moveLengthUm * flowPosition) - this.fovWidthUm / 2;
                float targetY = settings.Src.Y;
                axisHandler.StartMultipleMove(new AxisPosition(new float[] { targetX, targetY }), settings.Velocity);
            }
            
            return !cancellationTokenSource.IsCancellationRequested;
        }

        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            DebugWriteLine("ScanOperator::ImageGrabbed");
            if (cancellationTokenSource.IsCancellationRequested)
                return;

            ImageD grabbedImage = imageDevice.GetGrabbedImage(ptr);
            AlgoImage sourceImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, grabbedImage, ImageType.Grey);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(sourceImage);

            ScanBuffer scanBuffer = BufferManager.Instance().GetScanBuffer(flowPosition);
            scanBuffer.AddImage(sourceImage, scanDirection);
            
            sourceImage.Clear();
            sourceImage.Dispose();

            if (cancellationTokenSource.IsCancellationRequested)
                return;

            if (scanBuffer.IsFull == true)
            {
                scanTask?.Wait();

                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

                scanDirection = ScanDirection.Forward;
                
                int width = (int)(scanBuffer.TopLightBuffer.Image.Width * resizeRatio);
                int height = (int)(scanBuffer.TopLightBuffer.Image.Height * resizeRatio);

                AlgoImage backLightImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(width, height));
                AlgoImage topLightImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(width, height));

                imageProcessing.Resize(scanBuffer.BackLightBuffer.Image, backLightImage, resizeRatio, resizeRatio);
                imageProcessing.Resize(scanBuffer.TopLightBuffer.Image, topLightImage, resizeRatio, resizeRatio);

                string toplightImagePath = SystemManager.Instance().CurrentModel.GetImagePathName(0, flowPosition, 1);
                string backlightImagePath = SystemManager.Instance().CurrentModel.GetImagePathName(0, flowPosition, 0);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                topLightImage.Save(toplightImagePath);
                backLightImage.Save(backlightImagePath);
                sw.Stop();
                DebugWriteLine(string.Format("Model Image {0} Save: {1}[ms]", flowPosition, sw.ElapsedMilliseconds));

                float res = DeveloperSettings.Instance.Resolution;
                float realPosX = settings.Dst.X - (moveLengthUm * flowPosition) - this.fovWidthUm / 2;
                float realPosY = settings.Src.Y;
                float dispPosX = InfoBox.Instance.DispScanRegion.X * resizeRatio + (moveLengthUm * flowPosition) * resizeRatio / res;
                float dispPosY = InfoBox.Instance.DispScanRegion.Y * resizeRatio;

                AxisPosition[] limitPos = axisHandler.GetLimitPos();

                ScanOperatorResult scanOperatorResult = new ScanOperatorResult(resultKey, backLightImage.ToBitmapSource(), topLightImage.ToBitmapSource(),
                    flowPosition, scanBuffer.BackLightBuffer.Image, scanBuffer.TopLightBuffer.Image,
                    new AxisPosition(realPosX, realPosY),
                    new AxisPosition(dispPosX, dispPosY));

                backLightImage.Dispose();
                topLightImage.Dispose();

                SystemManager.Instance().OperatorProcessed(scanOperatorResult);

                flowPosition++;

                float nextFovStartX = settings.Dst.X - (moveLengthUm * flowPosition);

                if (flowPosition == DeveloperSettings.Instance.ScanNum || nextFovStartX < settings.Src.X)
                {
                    SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
                    SystemManager.Instance().OperatorCompleted(new ScanOperatorResult(resultKey, "Completed"));
                    return;
                }

                if (PostGrab() == false)
                    return;

                if (PrepereGrab() == false)
                    return;
            }
            else if (scanBuffer.BackLightBuffer.IsFull == true && scanBuffer.TopLightBuffer.IsEmpty == true)
            {
                scanTask?.Wait();
                scanDirection = ScanDirection.Backward;

                if (PostGrab() == false)
                    return;

                if (PrepereGrab() == false)
                    return;
            }
        }

        private void DebugWriteLine(string v)
        {
            Debug.WriteLine(string.Format("[{0}] {1}", DateTime.Now.ToString("yyyy.MM.dd. HH:mm:ss.fff"), v));
        }
    }

    public class ScanOperatorResult : OperatorResult
    {
        DateTime scanTime = DateTime.Now;

        int flowPosition;
        AlgoImage backLightImage;
        AlgoImage topLightImage;

        AxisPosition axisPosition;
        AxisPosition canvasAxisPosition;
        
        BitmapSource backLightBitmap;
        BitmapSource topLightBitmap;


        public int FlowPosition { get => flowPosition; }
        public AxisPosition AxisPosition { get => axisPosition; }
        public AlgoImage BackLightImage { get => backLightImage; }
        public AlgoImage TopLightImage { get => topLightImage; }
        public BitmapSource TopLightBitmap { get => topLightBitmap; }
        public BitmapSource BackLightBitmap { get => backLightBitmap; }
        public DateTime ScanTime { get => scanTime; }
        public AxisPosition CanvasAxisPosition { get => canvasAxisPosition; }

        public ScanOperatorResult(ResultKey resultKey, BitmapSource backLightBitmap, BitmapSource topLightBitmap, int flowPosition, AlgoImage backLightImage, AlgoImage topLightImage, AxisPosition axisPosition, AxisPosition canvasAxisPosition)
            : base(ResultType.Scan, resultKey, null)
        {
            this.backLightBitmap = backLightBitmap;
            this.topLightBitmap = topLightBitmap;
            this.flowPosition = flowPosition;
            this.backLightImage = backLightImage;
            this.topLightImage = topLightImage;
            this.axisPosition = axisPosition;
            this.canvasAxisPosition = canvasAxisPosition;
        }

        public ScanOperatorResult(ResultKey resultKey, string exceptionMassage) : base(ResultType.Scan, resultKey, exceptionMassage)
        {

        }
    }

    public class ScanOperatorSettings : GrabOperatorSettings, INotifyPropertyChanged
    {
        PointF src;
        PointF dst;
        
        float overlapUm;

        public event PropertyChangedEventHandler PropertyChanged;

        public PointF Src { get => src; }
        public PointF Dst { get => dst; }
        public float OverlapUm
        {
            get => overlapUm;
            set
            {
                overlapUm = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Width"));
            }
        }

        public float SrcX
        {
            get => src.X;
            set
            {
                src.X = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Width"));
            }
        }
        public float SrcY
        {
            get => src.Y;
            set
            {
                src.Y = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Height"));
            }
        }
        public float DstX
        {
            get => dst.X;
            set
            {
                dst.X = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Width"));
            }
        }
        public float DstY
        {
            get => dst.Y;
            set
            {
                dst.Y = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Height"));
            }
        }

        public float Width { get => (dst.X - src.X) - (overlapUm * (DeveloperSettings.Instance.ScanNum - 1)); }
        public float Height { get => dst.Y - src.Y; }

        protected override void Initialize()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Scan");

            this.src = PointF.Empty;
            this.dst= PointF.Empty;
            overlapUm = 0; 
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            XmlHelper.GetValue(xmlElement, "Src", ref this.src);
            XmlHelper.GetValue(xmlElement, "Dst", ref this.dst);
            overlapUm = XmlHelper.GetValue(xmlElement, "OverlapUm", this.overlapUm);
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "Src", this.src);
            XmlHelper.SetValue(xmlElement, "Dst", this.dst);
            XmlHelper.SetValue(xmlElement, "OverlapUm", this.overlapUm);
        }

    }
}
