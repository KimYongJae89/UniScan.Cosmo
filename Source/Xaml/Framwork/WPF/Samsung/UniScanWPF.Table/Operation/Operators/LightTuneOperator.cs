﻿using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation.Operators
{
    public enum LightTuneFlow
    {
        Check, Back, Top
    }

    public class LightTuneOperator : GrabOperator
    {
        const float tuneWeight = 0.1f;
        
        LightTuneFlow flowPosition;

        public LightTuneOperatorSettings Settings { get => this.settings; }
        LightTuneOperatorSettings settings;
        LightValue lightValue = new LightValue(2);

        float baseValue;
        int iteration;

        public LightTuneOperator() : base()
        {
            settings = new LightTuneOperatorSettings();
        }

        public override bool Initialize(ResultKey resultKey, CancellationTokenSource cancellationTokenSource)
        {
            iteration = 0;
            flowPosition = LightTuneFlow.Check;

            lightValue.Value[0] = settings.InitialTopLightValue;
            lightValue.Value[1] = settings.InitialBackLightValue;

            return base.Initialize(resultKey, cancellationTokenSource);
        }
        
        protected override bool PrepereGrab()
        {
            WaitMoveDone();

            if (cancellationTokenSource.IsCancellationRequested)
                return false;

            Task.Factory.StartNew(() =>
            {
                if (this.flowPosition == LightTuneFlow.Back)
                    SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetStepLight(2, 0);
                else
                    SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetStepLight(2, 1);

                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(lightValue);
                SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();
                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(grabPort, true);
                
                float targetY = settings.ScanPosition[1] + settings.ScanLengthUm;
                axisHandler.StartCmp(axisName, (int)settings.ScanPosition[1], (int)Math.Round(DeveloperSettings.Instance.Resolution), true);
                axisHandler.StartMultipleMove(new AxisPosition(new float[] { settings.ScanPosition[0], targetY }), settings.Velocity);
                
                WaitMoveDone();

                axisHandler.EndCmp(axisName);
                SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(grabPort, false);

                //SystemManager.Instance().DeviceBox.ImageDeviceHandler.WaitGrabDone();

                SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            }, cancellationTokenSource.Token);
            
            return !(cancellationTokenSource.IsCancellationRequested);
        }

        protected override bool PostGrab()
        {
            WaitMoveDone();
            axisHandler.StartMultipleMove(settings.ScanPosition, settings.Velocity);

            return !(cancellationTokenSource.IsCancellationRequested);
        }

        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            ImageD grabbedImage = imageDevice.GetGrabbedImage(ptr);
            AlgoImage sourceImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, grabbedImage, ImageType.Grey);
            Rectangle sourceImageRect = new Rectangle(Point.Empty, sourceImage.Size);

            DebugContext debugContext = this.GetDebugContext("LightTuneOperator");

            sourceImage.Save("sourceImage.bmp", debugContext);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(sourceImage);

            Size subSize = new Size(sourceImage.Width, (int)Math.Floor(settings.ScanLengthUm / DeveloperSettings.Instance.Resolution));
            Rectangle subRect = Rectangle.Empty;
            if (imageDevice.IsVirtual)
            {
                Point subCenter = Point.Round(DrawingHelper.CenterPoint(sourceImageRect));
                subRect = DrawingHelper.FromCenterSize(subCenter, subSize);
            }
            else
            {
                subRect = new Rectangle(Point.Empty, subSize);
            }
            subRect.Intersect(sourceImageRect);
            if(subRect.Width==0 || subRect.Height==0)
            {
                SystemManager.Instance().OperatorCompleted(new LightTuneResult(resultKey, null, "Light tune: Clip SubImage Fail.."));
                return;
            }

            AlgoImage subImage = sourceImage.GetSubImage(subRect);
            subImage.Save("subImage.bmp", debugContext);

            AlgoImage resizeImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size((int)(subImage.Width * resizeRatio), (int)(subImage.Height * resizeRatio)));
            imageProcessing.Resize(subImage, resizeImage, resizeRatio);
            BitmapSource resizeBitmap = resizeImage.ToBitmapSource();
            resizeImage.Dispose();

            if (PostGrab() == false)
                SystemManager.Instance().OperatorCompleted(new LightTuneResult(resultKey, resizeBitmap, "Light tune: post grab fail.."));

            LightTuneResult lightTuneResult = null;
            bool beEnd = false;

            float res = DeveloperSettings.Instance.Resolution;
            float fovWidthUm = SystemManager.Instance().DeviceBox.ImageDeviceHandler[0].ImageSize.Width * DeveloperSettings.Instance.Resolution;
            AxisPosition[] limitPos = axisHandler.GetLimitPos();
            AxisPosition canvasAxisPosition = new AxisPosition(
                (limitPos[1].Position[0] - settings.ScanPosition[0] - fovWidthUm / 2) * resizeRatio / res,
                (-limitPos[0].Position[1] + settings.ScanPosition[1]) * resizeRatio / res);
            switch (flowPosition)
            {
                case LightTuneFlow.Check:
                    if (Check(subImage))
                    {
                        lightTuneResult = new LightTuneResult(resultKey, flowPosition, iteration, resizeBitmap, lightValue, 0, 0, canvasAxisPosition);
                        lightValue.Value[0] = 0;
                        flowPosition++;
                    }
                    else
                    {
                        lightTuneResult = new LightTuneResult(resultKey, resizeBitmap, "Can't find sheet..");
                        beEnd = true;
                    }
                    break;
                case LightTuneFlow.Back:
                    SystemManager.Instance().CurrentModel.BinarizeValueBack = GetBinValue(subImage);
                    lightValue.Value[0] = settings.InitialTopLightValue;
                    lightTuneResult = new LightTuneResult(resultKey, flowPosition, iteration, resizeBitmap, lightValue, 0, 0, canvasAxisPosition);
                    flowPosition++;
                    break;
                case LightTuneFlow.Top:
                    if (TuneTop(subImage, out float patternAvg, out float marginAvg))
                    {
                        SystemManager.Instance().CurrentModel.BinarizeValueTop = GetBinValue(subImage);
                        beEnd = true;
                    }
                    iteration++;
                    lightTuneResult = new LightTuneResult(resultKey, flowPosition, this.iteration, resizeBitmap, lightValue, patternAvg, marginAvg, canvasAxisPosition);
                    break;
            }
            
            sourceImage.Clear();

            subImage.Dispose();
            sourceImage.Dispose();

            SystemManager.Instance().OperatorProcessed(lightTuneResult);

            if (beEnd == true)
            {
                switch (flowPosition)
                {
                    case LightTuneFlow.Check:
                        break;
                    case LightTuneFlow.Back:
                        break;
                    case LightTuneFlow.Top:
                        SystemManager.Instance().CurrentModel.LightValueTop = lightValue.Value[0];
                        SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
                        break;
                }

                SystemManager.Instance().OperatorCompleted(new LightTuneResult(resultKey, resizeBitmap, "Completed"));

                this.OperatorState = OperatorState.Idle;
                return;
            }
            
            if (PrepereGrab() == false)
                SystemManager.Instance().OperatorCompleted(new LightTuneResult(resultKey, resizeBitmap, "Light tune prepere grab fail.."));
        }

        private int GetBinValue(AlgoImage subImage)
        {
            DebugContext debugContext = this.GetDebugContext("LightTuneOperator");
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(subImage);
            subImage.Save("subImage.bmp", debugContext);

            int thValue = (int)Math.Min(255, Math.Round(imageProcessing.Li(subImage)));
            AlgoImage binImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(subImage.Width, subImage.Height));
            imageProcessing.Binarize(subImage, binImage, thValue);
            binImage.Save("binImage.bmp", debugContext);
            baseValue = imageProcessing.GetGreyAverage(subImage, binImage);
            binImage.Dispose();
            return thValue; 
        }

        private bool TuneTop(AlgoImage subImage, out float patternAvg, out float marginAvg)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(subImage);

            int liValue = (int)Math.Min(255, Math.Round(imageProcessing.Li(subImage)));
            AlgoImage binImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(subImage.Width, subImage.Height));

            imageProcessing.Binarize(subImage, binImage, liValue);
            imageProcessing.Erode(binImage, settings.RemoveNoiseIter);
            marginAvg = imageProcessing.GetGreyAverage(subImage, binImage);
            
            imageProcessing.Binarize(subImage, binImage, liValue, true);
            imageProcessing.Erode(binImage, settings.RemoveNoiseIter);
            patternAvg = imageProcessing.GetGreyAverage(subImage, binImage);

            binImage.Dispose();

            if (marginAvg >= settings.TargetMarginValue)
                return true;
                
            float multiplyValue = settings.TargetMarginValue / (marginAvg - baseValue);

            multiplyValue = multiplyValue >= 1 ? multiplyValue + tuneWeight : multiplyValue - tuneWeight;

            double nextValue = lightValue.Value[0] * multiplyValue;
            if (Math.Round(Math.Max(0.0, Math.Min(255.0, nextValue))) == lightValue.Value[0])
                return true;

            lightValue.Value[0] = (int)Math.Round(Math.Max(0.0, Math.Min(255.0, nextValue)));

            if (lightValue.Value[0] == 255)
                return true;

            return false;
        }

        // 시트든 뭐든 뭔가 있는지 확인
        private bool Check(AlgoImage subImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(subImage);
            
            for (int i = 0; i < settings.RemoveNoiseIter; i++)
                imageProcessing.Average(subImage);

            float stdValue = imageProcessing.GetStdDev(subImage);
            //MessageBox.Show(string.Format("stdValue is {0}", stdValue));

            int thValue = (int)Math.Min(255, Math.Round(imageProcessing.Li(subImage)));
            //MessageBox.Show(string.Format("thValue is {0}", thValue));

            if (stdValue >= settings.CheckStd)
            {
                AlgoImage binImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(subImage.Width, subImage.Height));
                imageProcessing.Binarize(subImage, binImage, thValue);
                binImage.Dispose();
                return true;
            }
            
            return false;
        }
    }

    public class LightTuneResult : OperatorResult
    {
        LightTuneFlow flowPosition;
        int iteration;
        BitmapSource resizeImage;
        LightValue lightValue;
        float patternAvg;
        float marginAvg;
        AxisPosition canvasAxisPosition;

        public LightTuneFlow FlowPosition { get => flowPosition; }
        public int Iteration { get => iteration; }
        public BitmapSource ResizeImage { get => resizeImage; }
        public LightValue LightValue { get => lightValue; }
        public float PatternAvg { get => patternAvg; }
        public float MarginAvg { get => marginAvg; }
        public AxisPosition CanvasAxisPosition { get => canvasAxisPosition; }

        public LightTuneResult(ResultKey resultKey, LightTuneFlow flowPosition, int iteration, BitmapSource resizeImage, LightValue lightValue, float patternAvg, float marginAvg, AxisPosition canvasAxisPosition)
            : base(ResultType.LightTune, resultKey, null)
        {
            this.flowPosition = flowPosition;
            this.iteration = iteration;
            this.resizeImage = resizeImage;
            this.lightValue = lightValue;
            this.patternAvg = patternAvg;
            this.marginAvg = marginAvg;
            this.canvasAxisPosition = canvasAxisPosition;
        }

        public LightTuneResult(ResultKey resultKey, BitmapSource resizeImage, string exceptionMessage) : base(ResultType.LightTune, resultKey, exceptionMessage)
        {
            this.resizeImage = resizeImage;
        }
    }

    public class LightTuneOperatorSettings : GrabOperatorSettings
    {
        //Light
        int targetMarginValue;
        int initialTopLightValue;
        int initialBackLightValue;
        
        //Processing
        float checkStd;
        int removeNoiseIter;

        //Motion
        AxisPosition scanPosition;
        int scanLengthUm;

        public AxisPosition ScanPosition { get => scanPosition; }

        [CatecoryAttribute("Light"), NameAttribute("Target Value")]
        public int TargetMarginValue { get => targetMarginValue; set => targetMarginValue = value; }

        [CatecoryAttribute("Light"), NameAttribute("Initial TopLight Value")]
        public int InitialTopLightValue { get => initialTopLightValue; set => initialTopLightValue = value; }

        public int InitialBackLightValue { get => initialBackLightValue; set => initialBackLightValue = value; }
        
        [CatecoryAttribute("Process"), NameAttribute("Check Std")]
        public float CheckStd { get => checkStd; set => checkStd = value; }

        [CatecoryAttribute("Process"), NameAttribute("Remove Noise Iter")]
        public int RemoveNoiseIter { get => removeNoiseIter; set => removeNoiseIter = value; }
        
        [CatecoryAttribute("Machine"), NameAttribute("Pos X")]
        public float LightTunePositionX { get => scanPosition.Position[0]; set => scanPosition.Position[0] = value; }

        [CatecoryAttribute("Machine"), NameAttribute("Pos Y")]
        public float LightTunePositionY { get => scanPosition.Position[0]; set => scanPosition.Position[0] = value; }

        [CatecoryAttribute("Machine"), NameAttribute("Scan Length")]
        public int ScanLengthUm { get => scanLengthUm; set => scanLengthUm = value; }

        protected override void Initialize()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "LightTune");

            scanPosition = new AxisPosition(new float[] { 0, 0 });
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            targetMarginValue = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "TargetMarginValue", "127"));
            initialTopLightValue = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "InitialTopLightValue", "127"));
            initialBackLightValue = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "InitialBackLightValue", "250"));

            int scanPositionX = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "ScanPositionX", "0"));
            int scanPositionY = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "ScanPositionY", "0"));
            scanPosition = new AxisPosition(new float[] { scanPositionX, scanPositionY });
            scanLengthUm = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "ScanLengthUm", "10000"));

            checkStd = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "CheckStd", "10"));
            removeNoiseIter = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "RemoveNoiseIter", "10"));

            // 구 버전 호환성
            if (xmlElement.GetElementsByTagName("TargetValue").Count > 0)
                targetMarginValue = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "TargetValue", "127"));

            if(xmlElement.GetElementsByTagName("InitialValue").Count > 0)
                initialTopLightValue = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "InitialValue", "127"));
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "TargetMarginValue", targetMarginValue.ToString());
            XmlHelper.SetValue(xmlElement, "InitialTopLightValue", initialTopLightValue.ToString());
            XmlHelper.SetValue(xmlElement, "InitialBackLightValue", initialBackLightValue.ToString());
            
            XmlHelper.SetValue(xmlElement, "ScanPositionX", scanPosition[0].ToString());
            XmlHelper.SetValue(xmlElement, "ScanPositionY", scanPosition[1].ToString());
            XmlHelper.SetValue(xmlElement, "ScanLengthUm", scanLengthUm.ToString());

            XmlHelper.SetValue(xmlElement, "CheckStd", checkStd.ToString());
            XmlHelper.SetValue(xmlElement, "RemoveNoiseIter", removeNoiseIter.ToString());
        }
    }
}
