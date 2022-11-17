using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Matrox.MatroxImagingLibrary;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Devices;
using System.Diagnostics;

namespace DynMvp.Vision.Matrox
{
    public class MilPattern : Pattern, MilObject
    {
        private MIL_ID patternId = MIL.M_NULL;
        public MIL_ID PatternId
        {
            get { return patternId; }
            set { patternId = value; }
        }

        ~MilPattern()
        {
            Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();
            MilObjectManager.Instance.ReleaseObject(this);
        }

        public MilPattern() : base()
        {
            LogHelper.Debug(LoggerType.Operation, "MilPattern::MilPattern");
            this.imagingLibrary = ImagingLibrary.MatroxMIL;
            MilObjectManager.Instance.AddObject(this);
        }

        public override Pattern Clone()
        {
            MilPattern milImage = new MilPattern();
            milImage.Copy(this);

            return milImage;
        }

        public override void Train(AlgoImage algoImage, PatternMatchingParam patternMatchingParam)
        {
            MilImage.CheckGreyImage(algoImage, "MilPattern.Train", "Source");

            if (patternId != MIL.M_NULL)
                MIL.MpatFree(patternId);

            if (PatternImage != null)
                PatternImage.Dispose();
            
            MilImage greyImage = (MilImage)algoImage;
            
            patternImage = (Image2D)MilImageBuilder.ConvertImage((MilGreyImage)greyImage);

            patternId = MIL.MpatAlloc(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_NULL);

            MIL.MpatDefine(patternId, MIL.M_REGULAR_MODEL, greyImage.Image, 0, 0, greyImage.Width, greyImage.Height, MIL.M_DEFAULT);

            //0일 경우 Test 요망
            if (patternMatchingParam.NumToFind == 0)
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_NUMBER, MIL.M_ALL);
            else
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_NUMBER, patternMatchingParam.NumToFind);
                
            
            MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_ACCEPTANCE, patternMatchingParam.MatchScore); //Default 70
            MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_CERTAINTY, patternMatchingParam.MatchScore); //Default 80

            switch (patternMatchingParam.SpeedType)
            {
                //Default
                case 0:
                    MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_ACCURACY, MIL.M_MEDIUM);
                    MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SPEED, MIL.M_MEDIUM);
                    break;
                //Very Quick
                case 1:
                    MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_ACCURACY, MIL.M_LOW);
                    MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SPEED, MIL.M_VERY_HIGH);
                    break;
                //ACCURACY    
                case 2:
                    MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_ACCURACY, MIL.M_HIGH);
                    MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SPEED, MIL.M_VERY_HIGH);
                    break;
            }
            
            if (patternMatchingParam.UseAngle == true)
            {
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_MODE, MIL.M_ENABLE);
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_DELTA_POS, patternMatchingParam.MaxAngle);
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_DELTA_NEG, -patternMatchingParam.MinAngle);
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_ACCURACY, 0.1);
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_INTERPOLATION_MODE, MIL.M_NEAREST_NEIGHBOR);
            }

            MIL.MpatPreprocess(patternId, MIL.M_DEFAULT, MIL.M_NULL);
        }

        public override void UpdateMaskImage()
        {
            if (MaskFigures.FigureExist == false)
                return;

            Image2D maskImage = GetMaskedImage();
            MilGreyImage greyImage = (MilGreyImage)ImageBuilder.MilImageBuilder.Build(maskImage, ImageType.Grey, ImageBandType.Luminance);
            MIL.MpatMask(patternId, MIL.M_DEFAULT, greyImage.Image, MIL.M_DONT_CARE, MIL.M_DEFAULT);

            PreprocModel();

            maskImage.Dispose();
        }

        private void PreprocModel()
        {
            if (patternId == MIL.M_NULL)
            {
                throw new InvalidOperationException();
            }

            MIL.MpatPreprocess(MIL.M_NULL, patternId, MIL.M_DEFAULT);
        }

        public override Image2D GetMaskedImage()
        {
            Bitmap rgbImage = new Bitmap(PatternImage.Width, PatternImage.Height);
            ImageHelper.Clear(rgbImage, 0);

            Bitmap maskImage;
            if (MaskFigures.FigureExist == false)
            {
                maskImage = ImageHelper.MakeGrayscale(rgbImage);
            }
            else
            {
                Graphics g = Graphics.FromImage(rgbImage);

                MaskFigures.SetTempBrush(new SolidBrush(Color.White));

                MaskFigures.Draw(g, new CoordTransformer(), true);

                MaskFigures.ResetTempProperty();

                g.Dispose();

                maskImage = ImageHelper.MakeGrayscale(rgbImage);
            }

            rgbImage.Dispose();

            return Image2D.ToImage2D(maskImage);
        }

        public override PatternResult Inspect(AlgoImage targetClipImage, PatternMatchingParam patternMatchingParam, DebugContext debugContext)
        {
            MilImage.CheckGreyImage(targetClipImage, "MilPattern.Inspect", "Source");

            MilGreyImage greyImage = (MilGreyImage)targetClipImage;
            
            PatternResult pmResult = new PatternResult();

            if (patternId == MIL.M_NULL)
            {
                pmResult.NotTrained = true;
                pmResult.Good = false;
                return pmResult;
            }

            MIL_ID patResultId = MIL.MpatAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_NULL);
            
            MIL.MpatFind(patternId, greyImage.Image, patResultId);

            Size patternSize = patternImage.Size;
            RectangleF imageRect = new RectangleF(0, 0, targetClipImage.Width, targetClipImage.Height);

            double numOccurDbl = 0;
            MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_NUMBER, ref numOccurDbl);
            long numOccurrences = (int)numOccurDbl;
            if (numOccurrences > 0)
            {
                double[] posX = new double[numOccurrences];
                double[] posY = new double[numOccurrences];
                double[] score = new double[numOccurrences];
                double[] angle = new double[numOccurrences];

                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_SCORE, score);
                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_POSITION_X, posX);
                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_POSITION_Y, posY);
                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_ANGLE, angle);

                for (int i = 0; i < numOccurrences; i++)
                {
                    MatchPos matchPos = new MatchPos();
                    matchPos.Score = (float)score[i] / 100;
                    matchPos.Pos = new PointF((float)posX[i], (float)posY[i]);
                    matchPos.PatternSize = patternSize;
                    matchPos.PatternType = PatternType;
                    matchPos.Angle = (float)angle[i];

                    RectangleF patternRect = DrawingHelper.FromCenterSize(matchPos.Pos, new SizeF(patternSize.Width, patternSize.Height));

                    if (imageRect.Contains(patternRect) && matchPos.Score >= patternMatchingParam.MatchScore / 100.0f)
                    {
                        if (matchPos.Score > pmResult.MaxMatchPos.Score)
                        {
                            pmResult.ResultRect = new RotatedRect(patternRect, matchPos.Angle);
                            pmResult.Good = true;
                        }
                        
                        pmResult.AddMatchPos(matchPos);
                    }
                }
            }
            
            MIL.MpatFree(patResultId);

            return pmResult;
        }

        public void Free()
        {
            if (patternId != MIL.M_NULL)
                MIL.MpatFree(patternId);
            patternId = MIL.M_NULL;
        }

        public void AddTrace()
        {
        }

        public StackTrace GetTrace()
        {
            return null;
        }
    }
}

