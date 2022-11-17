using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace DynMvp.Device.AutoFocus
{
    public enum StepSize
    {
        Good, Fine, Middle, Coarse
    }

    public enum StepDirection
    {
        Forward, Stop, Backward
    }

    public enum CoarseToFindStep
    {
        Init, Coarse, Middle, Fine, End
    }

    public abstract class FocusDriver
    {
        public abstract void Step(StepSize stepType, StepDirection direction);
        public abstract void MoveTo(double fVal);
        public abstract double GetCurPos();
        public abstract double GetMinPos();
        public abstract double GetMaxPos();
        public abstract bool IsOnLowerLimit();
        public abstract bool IsOnUpperLimit();
        public abstract bool IsOnLimit();
        public abstract void Test();
    }

    public class AutoFocusSetting
    {
        public enum Params
        {
            FindFocusValueMethod = 0,
            OptimizationMethod,
            coarseThreshold,
            middleThreshold,
            fineThreshold,
            defocusLevel,
            ExposeTime,
            FrameSkip,
            ReferenceCount,
            RetryRate,
            ROI_CenterX,
            ROI_CenterY,
            ROI_Width,
            ROI_Height,
            PARAM_MAX,
            focusPosCoeffA,
            focusPosCoeffB,
            focusPosCoeffC,
            focusPosCoeffD
        }

        public enum CalculateMethod
        {
            Variance,
            SMD,
            SML,
            Tenengrad,
            STDDEV,
            METHOD_MAX
        };

        public enum OptimizationMethod
        {
            Global,
            CoarseToFine,
            BestFit,
            METHOD_MAX
        };

        private CalculateMethod findMethod = CalculateMethod.STDDEV;
        public CalculateMethod FindMethod
        {
            get { return findMethod; }
            set { findMethod = value; }
        }
        private OptimizationMethod optimizeMethod = OptimizationMethod.Global;
        public OptimizationMethod OptimizeMethod
        {
            get { return optimizeMethod; }
            set { optimizeMethod = value; }
        }

        double coarseThreshold = 0.950f;
        public double CoarseThreshold
        {
            get { return coarseThreshold; }
            set { coarseThreshold = value; }
        }

        double middleThreshold = 0.970f;
        public double MiddleThreshold
        {
            get { return middleThreshold; }
            set { middleThreshold = value; }
        }

        double fineThreshold = 0.990f;
        public double FineThreshold
        {
            get { return fineThreshold; }
            set { fineThreshold = value; }
        }

        double defocusLevel = 0.001f;
        public double DefocusLevel
        {
            get { return defocusLevel; }
            set { defocusLevel = value; }
        }

        double exposeTime = 30.0;
        public double ExposeTime
        {
            get { return exposeTime; }
            set { exposeTime = value; }
        }

        int frameSkip = 0;
        public int FrameSkip
        {
            get { return frameSkip; }
            set { frameSkip = value; }
        }

        int referenceCount = 0;
        public int ReferenceCount
        {
            get { return referenceCount; }
            set { referenceCount = value; }
        }

        double retryRate = 0.8;
        public double RetryRate
        {
            get { return retryRate; }
            set { retryRate = value; }
        }

        Point pointRoiCenter = new Point();
        public Point PointRoiCenter
        {
            get { return pointRoiCenter; }
            set { pointRoiCenter = value; }
        }

        Size sizeRoi = new Size();
        public Size SizeRoi
        {
            get { return sizeRoi; }
            set { sizeRoi = value; }
        }

        double[] focusPosCoeff = new double[4];
        public double[] FocusPosCoeff { get => focusPosCoeff; set => focusPosCoeff = value; }



        public string GetParamName(int iIdx)
        {
            string strName = "";

            switch ((Params)iIdx)
            {
                case Params.FindFocusValueMethod: strName = "FindMethod"; break;
                case Params.OptimizationMethod: strName = "OptimizeMethod"; break;
                case Params.coarseThreshold: strName = "Coarse"; break;
                case Params.middleThreshold: strName = "Middle"; break;
                case Params.fineThreshold: strName = "Fine"; break;
                case Params.defocusLevel: strName = "Defocus"; break;
                case Params.ExposeTime: strName = "ExposeTime"; break;
                case Params.FrameSkip: strName = "FrameSkip"; break;
                case Params.ReferenceCount: strName = "ReferenceCount"; break;
                case Params.RetryRate: strName = "RetryRate"; break;
                case Params.ROI_CenterX: strName = "FocusRegionCenterX"; break;
                case Params.ROI_CenterY: strName = "FocusRegionCenterY"; break;
                case Params.ROI_Width: strName = "FocusRegionWidth"; break;
                case Params.ROI_Height: strName = "FocusRegionHeight"; break;
            }

            return strName;
        }

        public string GetParamValue(string strParamName)
        {
            string strVal = "";
            string strName = "";
            bool bFind = false;
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strName = GetParamName(i);

                if (strName == strParamName)
                {
                    bFind = true;
                    switch ((Params)i)
                    {
                        case Params.FindFocusValueMethod: strVal = ((int)findMethod).ToString(); break;
                        case Params.OptimizationMethod: strVal = ((int)optimizeMethod).ToString(); break;
                        case Params.coarseThreshold: strVal = coarseThreshold.ToString(); break;
                        case Params.middleThreshold: strVal = middleThreshold.ToString(); break;
                        case Params.fineThreshold: strVal = fineThreshold.ToString(); break;
                        case Params.defocusLevel: strVal = defocusLevel.ToString(); break;
                        case Params.ExposeTime: strVal = exposeTime.ToString(); break;
                        case Params.FrameSkip: strVal = frameSkip.ToString(); break;
                        case Params.ReferenceCount: strVal = referenceCount.ToString(); break;
                        case Params.RetryRate: strVal = retryRate.ToString(); break;
                        case Params.ROI_CenterX: strVal = pointRoiCenter.X.ToString(); break;
                        case Params.ROI_CenterY: strVal = pointRoiCenter.Y.ToString(); break;
                        case Params.ROI_Width: strVal = sizeRoi.Width.ToString(); break;
                        case Params.ROI_Height: strVal = sizeRoi.Height.ToString(); break;
                        case Params.focusPosCoeffA: strVal = focusPosCoeff[0].ToString(); break;
                        case Params.focusPosCoeffB: strVal = focusPosCoeff[1].ToString(); break;
                        case Params.focusPosCoeffC: strVal = focusPosCoeff[2].ToString(); break;
                        case Params.focusPosCoeffD: strVal = focusPosCoeff[3].ToString(); break;
                        default: bFind = false; break;
                    }
                    break;
                }
            }
            if (bFind == false)
            {
                throw new InvalidOperationException();
            }

            return strVal;
        }

        public void SetParamValue(string strParamName, string strVal)
        {
            string strName = "";
            bool bFind = false;
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strName = GetParamName(i);

                if (strName == strParamName)
                {
                    bFind = true;
                    switch ((Params)i)
                    {
                        case Params.FindFocusValueMethod: findMethod = (CalculateMethod)Convert.ToInt32(strVal); break;
                        case Params.OptimizationMethod: optimizeMethod = (OptimizationMethod)Convert.ToInt32(strVal); break;
                        case Params.coarseThreshold: coarseThreshold = Convert.ToDouble(strVal); break;
                        case Params.middleThreshold: middleThreshold = Convert.ToDouble(strVal); break;
                        case Params.fineThreshold: fineThreshold = Convert.ToDouble(strVal); break;
                        case Params.defocusLevel: defocusLevel = Convert.ToDouble(strVal); break;
                        case Params.ExposeTime: exposeTime = Convert.ToDouble(strVal); break;
                        case Params.FrameSkip: frameSkip = Convert.ToInt16(strVal); break;
                        case Params.ReferenceCount: referenceCount = Convert.ToInt16(strVal); break;
                        case Params.RetryRate: retryRate = Convert.ToDouble(strVal); break;
                        case Params.ROI_CenterX: pointRoiCenter.X = Convert.ToInt16(strVal); break;
                        case Params.ROI_CenterY: pointRoiCenter.Y = Convert.ToInt16(strVal); break;
                        case Params.ROI_Width: sizeRoi.Width = Convert.ToInt16(strVal); break;
                        case Params.ROI_Height: sizeRoi.Height = Convert.ToInt16(strVal); break;

                        case Params.focusPosCoeffA: focusPosCoeff[0] = Convert.ToDouble(strVal); break;
                        case Params.focusPosCoeffB: focusPosCoeff[1] = Convert.ToDouble(strVal); break;
                        case Params.focusPosCoeffC: focusPosCoeff[2] = Convert.ToDouble(strVal); break;
                        case Params.focusPosCoeffD: focusPosCoeff[3] = Convert.ToDouble(strVal); break;
                        default: bFind = false; break;
                    }
                    break;
                }
            }
            if (bFind == false)
            {
                throw new InvalidOperationException();
            }
        }

        public void Load(XmlElement xmlElement)
        {
            string strParamName;
            string strParamVal;
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strParamName = GetParamName(i);
                strParamVal = XmlHelper.GetValue(xmlElement, strParamName, "");
                if (strParamVal != "")
                {
                    SetParamValue(strParamName, strParamVal);
                }
            }
        }

        public void Save(XmlElement xmlElement)
        {
            string strParamName = "";
            for (int i = 0; i < (int)Params.PARAM_MAX; i++)
            {
                strParamName = GetParamName(i);
                XmlHelper.SetValue(xmlElement, strParamName, GetParamValue(strParamName));
            }
        }

        public string GetCalculateMethodName(CalculateMethod findMethod)
        {
            string strName = null;
            switch (findMethod)
            {
                case CalculateMethod.Variance: strName = "Variance"; break;
                case CalculateMethod.SML: strName = "SML"; break;
                case CalculateMethod.SMD: strName = "SMD"; break;
                case CalculateMethod.Tenengrad: strName = "Tenengrad"; break;
                case CalculateMethod.STDDEV: strName = "STD_DEV"; break;
            }
            return strName;
        }

        public string GetOptimiseMethodName(OptimizationMethod optiMethod)
        {
            string strName = null;
            switch (optiMethod)
            {
                case OptimizationMethod.Global: strName = "Global"; break;
                case OptimizationMethod.CoarseToFine: strName = "CoarseToFine"; break;
                case OptimizationMethod.BestFit: strName = "BestFit"; break;
            }
            return strName;
        }
    }

    public class FocusValue
    {
        double nowLevel;
        public double NowLevel
        {
            get { return nowLevel; }
        }

        double nowPosition;
        public double NowPosition
        {
            get { return nowPosition; }
        }

        StepDirection moveDirection;
        public StepDirection MoveDirection
        {
            get { return moveDirection; }
        }

        StepSize moveSize;
        public StepSize MoveSize
        {
            get { return moveSize; }
        }

        public FocusValue(double level, double position, StepDirection direction, StepSize moveSize)
        {
            this.nowLevel = level;
            this.nowPosition = position;
            this.moveDirection = direction;
            this.moveSize = moveSize;
        }
    }

    public class AutoFocus
    {
        int iSkipCounter = 0;
        bool bFounded = false;
        double fReferenceLevel = 0;
        List<double> lstReference = new List<double>();

        FocusDriver focusDriver = null;
        AutoFocusSetting setting = new AutoFocusSetting();
        List<FocusValue> focusValueList = new List<FocusValue>();

        CoarseToFindStep stepCoarseToFind = CoarseToFindStep.Init;
        double[] CoarseToFindSearchRegion = new double[2] { 0, 0 };

        public bool Founded
        {
            get { return bFounded; }
            set { bFounded = value; }
        }

        public void Initialize(FocusDriver focusDriver, AutoFocusSetting setting)
        {
            this.focusDriver = focusDriver;
            this.setting = setting;
        }

        public void SetSetting(AutoFocusSetting setting)
        {
            this.setting = setting;
        }

        public void ListClear()
        {
            focusValueList.Clear();
        }

        public bool ValidCheck()
        {
            if (setting.FindMethod >= AutoFocusSetting.CalculateMethod.METHOD_MAX)
            {
                return false;
            }
            if (setting.OptimizeMethod >= AutoFocusSetting.OptimizationMethod.METHOD_MAX)
            {
                return false;
            }

            return true;
        }

        public Rectangle GetFocusRegion()
        {
            return new Rectangle(setting.PointRoiCenter.X - setting.SizeRoi.Width / 2, setting.PointRoiCenter.Y - setting.SizeRoi.Height / 2, setting.SizeRoi.Width, setting.SizeRoi.Height);
        }

        public void SetFocusRegion(Rectangle rect)
        {
            setting.PointRoiCenter = new Point((rect.Left + rect.Right) / 2, (rect.Top + rect.Bottom) / 2);
            setting.SizeRoi = rect.Size;
        }

        public void SetReady()
        {
            Founded = false;

            focusDriver.MoveTo(focusDriver.GetMinPos());
            ListClear();
            lstReference.Clear();
            fReferenceLevel = 0.0;
            stepCoarseToFind = CoarseToFindStep.Init;
        }

        public void SetReady4BestFit()
        {
            Founded = false;
            ListClear();
            lstReference.Clear();
            fReferenceLevel = 0.0;
            stepCoarseToFind = CoarseToFindStep.Init;
        }

        public void TestAction()
        {
            if (focusDriver.IsOnUpperLimit())
            {
                focusDriver.MoveTo(focusDriver.GetMinPos());
            }
            focusDriver.Step(StepSize.Middle, StepDirection.Forward);
        }

        public bool Action(Bitmap srcImage)
        {
            Rectangle roiRect = new Rectangle(setting.PointRoiCenter, new Size(1, 1));
            roiRect.Inflate(setting.SizeRoi.Width / 2, setting.SizeRoi.Height / 2);

            return Action(srcImage, roiRect);
        }
         
        public bool Action(Bitmap srcImage, Rectangle roiRect)
        {
            Bitmap algoImage = null;

            if (iSkipCounter++ < setting.FrameSkip)
            {
                return false;
            }
            iSkipCounter = 0;

            // Clip ROI
            if (roiRect == Rectangle.Empty)
                algoImage = srcImage;
            else
            {
                Bitmap clipImage = ImageHelper.ClipImage(srcImage, roiRect);
                algoImage = clipImage;
            }

            double fPosition = focusDriver.GetCurPos();

            // -------------------------------- //
            // Calculate method for focus level
            // -------------------------------- //
            double fFocusVal;
            switch (setting.FindMethod)
            {
                case AutoFocusSetting.CalculateMethod.Variance:
                    fFocusVal = CalcFocusLevelVariance(algoImage);
                    break;
                case AutoFocusSetting.CalculateMethod.SMD:
                    fFocusVal = CalcFocusLevelSMD(algoImage);
                    break;
                case AutoFocusSetting.CalculateMethod.SML:
                    fFocusVal = CalcFocusLevelSML(algoImage);
                    break;
                case AutoFocusSetting.CalculateMethod.Tenengrad:
                    fFocusVal = CalcFocusLevelTenengrad(algoImage);
                    break;
                case AutoFocusSetting.CalculateMethod.STDDEV:
                    fFocusVal = CalcFocusLevelSTDDEV(algoImage);
                    break;
                default:
                    throw new NotImplementedException();
            }

            Console.WriteLine(string.Format("Image grabbed. pos ({0:0.00}), level ({1:0.00})", fPosition, fFocusVal));
            //algoImage.Save(string.Format("D:\\({0:0.00}),({1:0.00}).JPG", fPosition, fFocusVal));
            //if (focusValueList.Count() < 1)
            //// Skip first grabed image
            //{
            //    focusValueList.Add(new FocusValue(fFocusVal, fPosition, StepDirection.Forward, StepType.Coarse));
            //    focusDriver.Step(StepType.Coarse, StepDirection.Forward);
            //    return false;
            //}

            // -------------------------------- //
            // Optimeze method
            // -------------------------------- //
            if (bFounded == true)
            {
                if (!IsOnFocus(fFocusVal, fPosition))
                // Focus를 놓치면 다시 탐색
                {
                    bFounded = false;
                    focusDriver.MoveTo(focusDriver.GetMinPos());
                }
            }
            else
            {
                if (setting.OptimizeMethod == AutoFocusSetting.OptimizationMethod.Global)
                // Global Method
                {
                    if (!bFounded)
                    {
                        bFounded = OptiGlobal(fFocusVal, fPosition);
                    }
                }
                else if (setting.OptimizeMethod == AutoFocusSetting.OptimizationMethod.CoarseToFine)
                {
                    if (!bFounded)
                    {
                        bFounded = OptiCoarseToFine(fFocusVal, fPosition);
                    }
                }
                else if (setting.OptimizeMethod == AutoFocusSetting.OptimizationMethod.BestFit)
                {
                    if (!bFounded)
                    {
                        bFounded = BestFit(fFocusVal, fPosition);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            return bFounded;
        }

        private bool OptiGlobal(double fFocusVal, double fPosition)
        {
            if (!focusDriver.IsOnUpperLimit())
            {
                StepDirection dirMove = StepDirection.Forward;
                StepSize dirSize = StepSize.Fine;
                focusValueList.Add(new FocusValue(fFocusVal, fPosition, dirMove, dirSize));
                focusDriver.Step(dirSize, StepDirection.Forward);
                return false;
            }
            else
            {
                FocusValue fFoundValue = focusValueList[GetMaximumFocusValueIndex()];
                Console.WriteLine(string.Format("\tOptimum FV is founded : pos ({0:0.00}). level ({1:0.00})", fFoundValue.NowPosition, fFoundValue.NowLevel));
                focusValueList.Add(new FocusValue(fFoundValue.NowLevel, fFoundValue.NowPosition, StepDirection.Stop, StepSize.Good));
                focusDriver.MoveTo(fFoundValue.NowPosition);
                fReferenceLevel = -1;
                return true;
            }
        }

        private bool OptiCoarseToFine(double fFocusVal, double fPosition)
        {
            if (stepCoarseToFind == CoarseToFindStep.Init)
            {
                CoarseToFindSearchRegion[0] = focusDriver.GetMinPos();
                CoarseToFindSearchRegion[1] = focusDriver.GetMaxPos();
                stepCoarseToFind = CoarseToFindStep.Coarse;
                Console.WriteLine(string.Format("\tScan Region [{0:0.00} {1:0.00}]", CoarseToFindSearchRegion[0], CoarseToFindSearchRegion[1]));
            }
            else
            {
                StepSize stepSize = StepSize.Good;
                double fCurPos = focusDriver.GetCurPos();

                //if(focusValueList.Count > 1)
                //{
                //    if(focusValueList[focusValueList.Count-1].NowLevel > fFocusVal && fFocusVal>30)
                //    {
                //        focusDriver.MoveTo(focusValueList[focusValueList.Count-1].NowPosition);
                //        stepCoarseToFind++;
                //        focusValueList.Clear();
                //        return true;
                //    }
                //}

                if (CoarseToFindSearchRegion[0] <= fCurPos && fCurPos < CoarseToFindSearchRegion[1]
                    && focusValueList.Any(value => value.NowPosition == fCurPos) == false)
                {
                    switch (stepCoarseToFind)
                    {
                        case CoarseToFindStep.Coarse: stepSize = StepSize.Coarse; break;
                        case CoarseToFindStep.Middle: stepSize = StepSize.Middle; Thread.Sleep(5); break;
                        case CoarseToFindStep.Fine: stepSize = StepSize.Fine; Thread.Sleep(5); break;
                    }
                    if (fPosition > 0)
                        focusValueList.Add(new FocusValue(fFocusVal, fPosition, StepDirection.Forward, stepSize));

                    focusDriver.Step(stepSize, StepDirection.Forward);
                }
                else
                {
                    if (focusValueList.Count() == 0)
                    {
                        fReferenceLevel = -1;
                        stepCoarseToFind = CoarseToFindStep.Init;
                        return true;
                    }
                    else
                    {
                        if (stepCoarseToFind == CoarseToFindStep.Fine)
                        {
                            FocusValue fFoundValue = focusValueList[GetMaximumFocusValueIndex()];
                            Console.WriteLine(string.Format("\tOptimum FV is founded : pos ({0:0.00}). level ({1:0.00})", fFoundValue.NowPosition, fFoundValue.NowLevel));
                            focusValueList.Add(new FocusValue(fFoundValue.NowLevel, fFoundValue.NowPosition, StepDirection.Stop, StepSize.Good));
                            int iMaxIndex = GetMaximumFocusValueIndex();
                            focusDriver.MoveTo(focusValueList[0].NowPosition);
                            Thread.Sleep(20);
                            focusDriver.MoveTo(fFoundValue.NowPosition);
                            Thread.Sleep(20);
                            fReferenceLevel = -1;
                            stepCoarseToFind = CoarseToFindStep.Init;
                            return true;
                        }
                        else
                        {
                            int iMaxIndex = GetMaximumFocusValueIndex();
                            if (iMaxIndex == 0)
                            {
                                CoarseToFindSearchRegion[0] = focusValueList[iMaxIndex].NowPosition;
                            }
                            else
                            {
                                CoarseToFindSearchRegion[0] = focusValueList[iMaxIndex - 1].NowPosition;
                            }

                            if (iMaxIndex == focusValueList.Count - 1)
                            {
                                CoarseToFindSearchRegion[1] = focusValueList[iMaxIndex].NowPosition;
                            }
                            else
                            {
                                CoarseToFindSearchRegion[1] = focusValueList[iMaxIndex + 1].NowPosition;
                            }

                            Console.WriteLine(string.Format("\tScan Region [{0:0.00} {1:0.00}]", CoarseToFindSearchRegion[0], CoarseToFindSearchRegion[1]));
                            focusDriver.MoveTo(focusValueList[0].NowPosition);
                            Thread.Sleep(20);
                            focusDriver.MoveTo(CoarseToFindSearchRegion[0]);
                            Thread.Sleep(20);
                            focusValueList.Clear();
                            stepCoarseToFind++;
                        }
                    }
                }
            }
            return false;
        }

        private bool BestFit(double fFocusVal, double fPosition)
        {
            double fCurPos = focusDriver.GetCurPos();
            if (stepCoarseToFind == CoarseToFindStep.Init)
            {
                CoarseToFindSearchRegion[0] = fCurPos - setting.CoarseThreshold;
                CoarseToFindSearchRegion[1] = fCurPos + setting.CoarseThreshold;

                focusDriver.MoveTo(focusDriver.GetMinPos());
                Thread.Sleep(5);
                focusDriver.MoveTo(CoarseToFindSearchRegion[0]);

                stepCoarseToFind = CoarseToFindStep.Middle;
            }
            else
            {
                StepSize stepSize = StepSize.Good;

                if (CoarseToFindSearchRegion[0] <= fCurPos && fCurPos < CoarseToFindSearchRegion[1])
                {
                    switch (stepCoarseToFind)
                    {
                        case CoarseToFindStep.Coarse: stepSize = StepSize.Coarse; break;
                        case CoarseToFindStep.Middle: stepSize = StepSize.Middle; Thread.Sleep(5); break;
                        case CoarseToFindStep.Fine: stepSize = StepSize.Fine; Thread.Sleep(5); break;
                    }
                    focusValueList.Add(new FocusValue(fFocusVal, fPosition, StepDirection.Forward, stepSize));
                    focusDriver.Step(stepSize, StepDirection.Forward);
                }
                else
                {
                    //Best Fit
                    double[] dataX = new double[focusValueList.Count];
                    double[] dataY = new double[focusValueList.Count];
                    for (int i = 0; i < focusValueList.Count; ++i)
                    {
                        dataX[i] = focusValueList[i].NowPosition;
                        dataY[i] = focusValueList[i].NowLevel;
                    }

                    PolyFit polyFit = new PolyFit(dataX, dataY, 2);
                    double newPos = -polyFit.Coeff[1] / (2.0 * polyFit.Coeff[2]);

                    focusDriver.MoveTo(CoarseToFindSearchRegion[0]);
                    Thread.Sleep(20);
                    focusDriver.MoveTo(newPos);
                    Thread.Sleep(20);
                    focusValueList.Clear();
                    fReferenceLevel = -1;
                    stepCoarseToFind = CoarseToFindStep.Init;
                    return true;
                }
            }
            return false;
        }

        private bool IsOnFocus(double fFocusVal, double fPosition)
        {
            if (lstReference.Count < setting.ReferenceCount)
            {
                lstReference.Add(focusValueList.Last().NowLevel);
                focusValueList.Add(new FocusValue(fFocusVal, fPosition, StepDirection.Stop, StepSize.Good));
                Console.WriteLine("\tAdd Reference value. pos ({0:0.00}). level ({1:0.00}).", fPosition, fFocusVal);
                return true;
            }
            else if (lstReference.Count == setting.ReferenceCount)
            {
                lstReference.Sort();
                fReferenceLevel = lstReference[lstReference.Count / 2];
                Console.WriteLine("\tSet Reference value. level ({0:0.00}).", fReferenceLevel);
                lstReference.Add(-1);
            }

            FocusValue PrevFocusValue = focusValueList.Last();

            double diffFocusValue = fFocusVal - PrevFocusValue.NowLevel;
            double diffAmplitude = Math.Abs(diffFocusValue);

            double fRate = diffFocusValue / fReferenceLevel;
            double fAbsRate = Math.Abs(fRate);

            double fRetry = fFocusVal / fReferenceLevel;
            if (fRetry > 1.0)
            {
                fRetry = 2 - fRetry;
            }
            Console.WriteLine("\tfRate : {0:+0.000;-0.000}, fRetry : {1:+0.000;-0.000}", fRate, fRetry);

            //return true;
            if (fRetry < setting.RetryRate)
            // 변화가 너무 크면 다시 Global 탐색
            {
                focusValueList.Clear();
                fReferenceLevel = 0;
                lstReference.Clear();
                return false;
            }

            return true;
        }

        private byte[] Image2Byte(Bitmap image)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        private double CalcFocusLevelSTDDEV(Bitmap srcImage)
        {
            int width = srcImage.Width;
            int height = srcImage.Height;
            byte[] imageData = Image2Byte(srcImage);
            double stddev = 0;
            double avg = 0;

            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {

                    avg += imageData[y * width + x];
                }
            }
            avg = avg / (width * height);

            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    stddev += Math.Abs(avg - imageData[y * width + x]);
                }
            }
            stddev = stddev / (width * height);


            return stddev;
        }

        private double CalcFocusLevelSMD(Bitmap srcImage)
        {
            int width = srcImage.Width;
            int height = srcImage.Height;
            byte[] imageData = Image2Byte(srcImage);

            double dX, dY, fSumX = 0, fSumY = 0;
            byte r, d, c;
            for (int y = 0; y < height - 1; y++)
            {
                for (int x = 0; x < width - 1; x++)
                {
                    c = imageData[y * width + x];
                    r = imageData[y * width + (x + 1)];
                    d = imageData[(y + 1) * width + x];
                    dX = c - r;
                    dY = c - d;
                    fSumX += Math.Abs(dX);
                    fSumY += Math.Abs(dY);
                }
            }
            return fSumX + fSumY;
        }

        private double CalcFocusLevelSML(Bitmap srcImage)
        {
            int width = srcImage.Width;
            int height = srcImage.Height;
            byte[] imageData = Image2Byte(srcImage);

            double fSum = 0;

            double d2X, d2Y, temp;
            byte l, r, u, d, c;
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    c = imageData[y * width + x];
                    l = imageData[y * width + (x - 1)];
                    r = imageData[y * width + (x + 1)];
                    u = imageData[(y + 1) * width + x];
                    d = imageData[(y - 1) * width + x];
                    d2X = 2 * c - (l + r);
                    d2Y = 2 * c - (u + d);
                    temp = Math.Abs(d2X) + Math.Abs(d2Y);
                    fSum += (temp * temp);
                }
            }
            return fSum;
        }

        private double CalcFocusLevelTenengrad(Bitmap srcImage)
        {
            int width = srcImage.Width;
            int height = srcImage.Height;
            byte[] imageData = Image2Byte(srcImage);

            double dX, dY, fSum = 0;
            byte l, r, u, d, c;
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    c = imageData[y * width + x];
                    l = imageData[y * width + (x - 1)];
                    r = imageData[y * width + (x + 1)];
                    u = imageData[(y + 1) * width + x];
                    d = imageData[(y - 1) * width + x];
                    dX = imageData[(y + 1) * width + (x - 1)] + imageData[(y + 1) * width + (x + 1)] -
                        imageData[(y - 1) * width + (x - 1)] - imageData[(y - 1) * width + (x + 1)] +
                        2 * imageData[(y + 1) * width + x] - 2 * imageData[(y - 1) * width + x];
                    dY = imageData[(y - 1) * width + (x + 1)] + imageData[(y + 1) * width + (x + 1)] -
                        imageData[(y - 1) * width + (x - 1)] - imageData[(y + 1) * width + (x - 1)] +
                        2 * imageData[y * width + (x + 1)] - 2 * imageData[y * width + (x - 1)];
                    fSum += Math.Sqrt(dX * dX + dY * dY);
                }
            }
            return fSum;
        }

        private double CalcFocusLevelVariance(Bitmap srcImage)
        {
            int width = srcImage.Width;
            int height = srcImage.Height;
            byte[] imageData = Image2Byte(srcImage);
            
            double avg = Array.ConvertAll(imageData, data => (double)data).Average();

            double sum = 0;
            foreach (byte data in imageData)
                sum += Math.Abs(data - avg);

            return sum / imageData.Count();

            //int nPixelPos = 0;
            //double fSum = 0;
            //int nCnt = 0;
            //for (int y = 1; y < height - 1; y++)
            //{
            //    for (int x = 1; x < width - 1; x++)
            //    {
            //        double fLaplace = (4 * imageData[y * width + x] - (imageData[y * width + (x - 1)] + imageData[y * width + (x + 1)] + imageData[(y - 1) * width + x] + imageData[(y + 1) * width + x])) / 6.0f;
            //        fSum += fLaplace;
            //    }
            //}

            //double fMean = fSum / ((width - 2) * (height - 2));

            //fSum = 0;
            //for (int y = 1; y < height - 1; y++)
            //{
            //    for (int x = 1; x < width - 1; x++)
            //    {
            //        nPixelPos = y * width + x;
            //        double fLaplace = (4 * imageData[y * width + x] - (imageData[y * width + (x - 1)] + imageData[y * width + (x + 1)] + imageData[(y - 1) * width + x] + imageData[(y + 1) * width + x])) / 6.0f;
            //        double fLapMean = fLaplace - fMean;
            //        fSum += (fLapMean * fLapMean);
            //        nCnt++;
            //    }
            //}

            //return fSum / nCnt;
        }

        private int GetMaximumFocusValueIndex()
        {
            if (focusValueList.Count == 0)
            {
                throw new InvalidOperationException();
            }

            int iFoundedIdx = 0;
            FocusValue FoundedValue = focusValueList[0];

            for (int i = 1; i < focusValueList.Count; i++)
            {
                FocusValue fValue = focusValueList[i];
                if (fValue.NowLevel > FoundedValue.NowLevel)
                {
                    FoundedValue = fValue;
                    iFoundedIdx = i;
                }
            }
            return iFoundedIdx;
        }

        public double GetFocusPos()
        {
            return focusDriver.GetCurPos();
        }

        public void SetFocusPos(double value)
        {
            focusDriver.MoveTo(value);
        }
    }

}
