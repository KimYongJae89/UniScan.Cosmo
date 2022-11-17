﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using DynMvp.Base;
using DynMvp.UI;

using Cognex.VisionPro;
using Cognex.VisionPro.OCRMax;

namespace DynMvp.Vision.Cognex
{
    public class CogCharPosition : CharPosition
    {
        CogOCRMaxPositionResult cogPositionResult;
        public CogOCRMaxPositionResult CogPositionResult
        {
            get { return cogPositionResult; }
            set { cogPositionResult = value; }
        }

        public CogCharPosition(int charCode, RotatedRect position, CogOCRMaxPositionResult cogPositionResult) : base(charCode, position)
        {
            this.cogPositionResult = cogPositionResult;
        }
    }

    public class CognexCharReader : CharReader
    {
        DateTime lastFontFileTIme;

        CogOCRMaxTool OCRMaxTool = new CogOCRMaxTool();

        public CognexCharReader()
        {

        }

        public override Algorithm Clone()
        {
            CognexCharReader charReader = new CognexCharReader();
            charReader.CopyFrom(this);

            return charReader;
        }

        public override void PrepareInspection()
        {
            CharReaderParam charReaderParam = (CharReaderParam)param;

            Train(charReaderParam.FontFileName);
        }

        public override void Train(string fontFileName)
        {
            if (File.Exists(fontFileName) == false)
                return;

            DateTime fontFileTime = File.GetLastWriteTime(fontFileName);
            if (fontFileTime != lastFontFileTIme)
            {
                OCRMaxTool.Classifier.Font.Import(fontFileName);
                OCRMaxTool.Classifier.Train();

                lastFontFileTIme = File.GetLastWriteTime(fontFileName);
            }
        }

        public override void SaveFontFile(string fontFileName)
        {
            OCRMaxTool.Classifier.Font.Export(fontFileName);
        }

        public override bool Trained
        {
            get { return OCRMaxTool.Classifier.Trained; }
        }

        public override CharReaderResult Extract(AlgoImage algoImage, RectangleF characterRegion, int threshold, DebugContext debugContext)
        {
            List<int> thresholdList = new List<int>();
            thresholdList.Add(threshold);

            return Read(algoImage, characterRegion, thresholdList, debugContext);
        }

        public override CharReaderResult Read(AlgoImage algoImage, RectangleF characterRegion, DebugContext debugContext)
        {
            CharReaderParam charReaderParam = (CharReaderParam)param;

            return Read(algoImage, characterRegion, charReaderParam.ThresholdList, debugContext);
        }

        public CharReaderResult Read(AlgoImage algoImage, RectangleF characterRegion, List<int> thresholdList, DebugContext debugContext)
        {
            PointF centerPt = DrawingHelper.CenterPoint(characterRegion);

            CogRectangleAffine cogRectangle = new CogRectangleAffine();
            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, characterRegion.Width, characterRegion.Height, 0, 0);

            CognexGreyImage greyImage = (CognexGreyImage)algoImage; //  ImageBuilder.Build(AlgorithmType.CharReader, algoImage, ImageType.Grey, ImageBandType.Luminance);

            OCRMaxTool.InputImage = greyImage.Image;
            OCRMaxTool.Region = cogRectangle;

            CharReaderResult result = new CharReaderResult();
            result.Good = false;

            CharReaderParam charReaderParam = (CharReaderParam)param;

            if (charReaderParam.CharacterMaxHeight < 1 || charReaderParam.CharacterMaxHeight > 2147483647 || charReaderParam.CharacterMinHeight < 1 || charReaderParam.CharacterMinHeight > 2147483647 ||
                charReaderParam.CharacterMaxWidth < 1 || charReaderParam.CharacterMaxWidth > 2147483647 || charReaderParam.CharacterMinWidth < 1 || charReaderParam.CharacterMinWidth > 2147483647)
                return result;

            OCRMaxTool.Segmenter.UseCharacterMaxWidth = true;
            OCRMaxTool.Segmenter.UseCharacterMaxHeight = true;
            OCRMaxTool.Segmenter.CharacterFragmentMinXOverlap = ((double)charReaderParam.XOverlapRatio) / 100;
            OCRMaxTool.Segmenter.CharacterMaxHeight = charReaderParam.CharacterMaxHeight;
            OCRMaxTool.Segmenter.CharacterMinHeight = charReaderParam.CharacterMinHeight;
            OCRMaxTool.Segmenter.CharacterMaxWidth = charReaderParam.CharacterMaxWidth;
            OCRMaxTool.Segmenter.CharacterMinWidth = charReaderParam.CharacterMinWidth;
            OCRMaxTool.Segmenter.Polarity = GetCogPolarity(charReaderParam.CharactorPolarity);
            OCRMaxTool.Segmenter.AngleHalfRange = MathHelper.DegToRad(2);
            OCRMaxTool.Segmenter.SkewHalfRange = MathHelper.DegToRad(2);

            int minNumCharDiff = charReaderParam.DesiredNumCharacter;
            int maxScoreThreshold = 0;

            foreach (int threshold in thresholdList)
            {
                OCRMaxTool.Segmenter.ForegroundThresholdFrac = ((double)threshold) / 256;

                OCRMaxTool.Run();

                if (OCRMaxTool.RunStatus.Result != CogToolResultConstants.Accept)
                {
                    // 문자 분리의 문제가 아니므로, 바로 빠져 나간다.
                    break;
                }
                else
                {
                    string stringRead = OCRMaxTool.LineResult.ResultString;
                    int numValidChar = stringRead.Length - stringRead.Count(ch => { return ch == '?'; });
                    //int numCharDiff = Math.Abs(stringRead.Length - Param.DesiredNumCharacter);
                    int numCharDiff = Math.Abs(numValidChar - charReaderParam.DesiredNumCharacter);

                    result.TrialResult.Add(String.Format("Threshold {0} - {1}", threshold, stringRead));

                    if (minNumCharDiff > numCharDiff)
                    {
                        maxScoreThreshold = threshold;
                        minNumCharDiff = numCharDiff;
                    }

                    if (stringRead.IndexOf(charReaderParam.DesiredString) > -1)
                    {
                        maxScoreThreshold = threshold;
                        minNumCharDiff = 0;
                    }

                    //if (OCRMaxTool.LineResult.Status != CogOCRMaxLineResultStatusConstants.Read)
                    //{
                    //}
                    //else if (numCharDiff == 0)
                    //{
                    //    maxScoreThreshold = threshold;
                    //    break;
                    //}
                }
            }

            if (maxScoreThreshold > 0 || charReaderParam.ThresholdList.Count == 0)
            {
                OCRMaxTool.Segmenter.ForegroundThresholdFrac = ((double)maxScoreThreshold) / 256;
                OCRMaxTool.Run();
            }

            if (OCRMaxTool.RunStatus.Result != CogToolResultConstants.Accept)
            {
                result.ErrorMessage = "OCRMax tool failed: " + OCRMaxTool.RunStatus.Message;
            }
            else if (OCRMaxTool.LineResult.Status != CogOCRMaxLineResultStatusConstants.Read)
            {
                result.ErrorMessage = "OCRMax tool failed to read the line of text";
            }
            else
            {
                result.Good = true;
            }

            result.StringRead = OCRMaxTool.LineResult.ResultString;

            foreach(CogOCRMaxPositionResult positionResult in OCRMaxTool.LineResult)
            {
                RotatedRect charPosition = new RotatedRect((float)positionResult.CellRect.CornerOriginX, (float)positionResult.CellRect.CornerOriginY,
                                                (float)positionResult.CellRect.SideXLength, (float)positionResult.CellRect.SideYLength, (float)(MathHelper.RadToDeg(positionResult.CellRect.Rotation * (-1))));
                result.AddCharPosition(new CogCharPosition(positionResult.CharacterCode, charPosition, positionResult));
            }

            result.DesiredString = charReaderParam.DesiredString;
            if (charReaderParam.DesiredString != "" && result.StringRead != null)
                result.Good = (result.StringRead.IndexOf(charReaderParam.DesiredString) > -1);

            result.ResultValueList.Add(new AlgorithmResultValue("String Read", charReaderParam.DesiredString, result.StringRead));

            return result;
        }

        private CogOCRMaxPolarityConstants GetCogPolarity(CharactorPolarity charactorPolarity)
        {
            switch (charactorPolarity)
            {
                case CharactorPolarity.DarkOnLight: return CogOCRMaxPolarityConstants.DarkOnLight;
                case CharactorPolarity.LightOnDark: return CogOCRMaxPolarityConstants.LightOnDark;
            }

            return CogOCRMaxPolarityConstants.DarkOnLight;
        }

        public override void AutoSegmentation(AlgoImage algoImage, RotatedRect rotatedRect, string desiredString)
        {
            CognexGreyImage greyImage = (CognexGreyImage)algoImage;

            PointF centerPt = DrawingHelper.CenterPoint(rotatedRect);

            CharReaderParam charReaderParam = (CharReaderParam)param;

            CogRectangleAffine cogRectangle = new CogRectangleAffine();
            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, rotatedRect.Width, rotatedRect.Height, MathHelper.DegToRad(-rotatedRect.Angle), 0);

            if (charReaderParam.CharacterMaxHeight < 1 || charReaderParam.CharacterMaxHeight > 2147483647 || charReaderParam.CharacterMinHeight < 1 || charReaderParam.CharacterMinHeight > 2147483647 ||
                charReaderParam.CharacterMaxWidth < 1 || charReaderParam.CharacterMaxWidth > 2147483647 || charReaderParam.CharacterMinWidth < 1 || charReaderParam.CharacterMinWidth > 2147483647)
                return;

            OCRMaxTool.Segmenter.CharacterFragmentMinXOverlap = ((double)charReaderParam.XOverlapRatio) / 100;
            OCRMaxTool.Segmenter.UseCharacterMaxWidth = true;
            OCRMaxTool.Segmenter.UseCharacterMaxHeight = true;
            OCRMaxTool.Segmenter.CharacterMaxHeight = charReaderParam.CharacterMaxHeight;
            OCRMaxTool.Segmenter.CharacterMinHeight = charReaderParam.CharacterMinHeight;
            OCRMaxTool.Segmenter.CharacterMaxWidth = charReaderParam.CharacterMaxWidth;
            OCRMaxTool.Segmenter.CharacterMinWidth = charReaderParam.CharacterMinWidth;
            OCRMaxTool.Segmenter.Polarity = GetCogPolarity(charReaderParam.CharactorPolarity);
            OCRMaxTool.Segmenter.AngleHalfRange = MathHelper.DegToRad(2);
            OCRMaxTool.Segmenter.SkewHalfRange = MathHelper.DegToRad(2);

            OCRMaxTool.InputImage = greyImage.Image;
            OCRMaxTool.Region = cogRectangle;

            // Run the OCRMaxTool on the image
            OCRMaxTool.Run();

            CogOCRMaxTuneRecord extractedRecord = OCRMaxTool.LineResult.CreateTuneRecordFromResult(greyImage.Image, cogRectangle);

            CogOCRMaxCharKeyCollection characterCodes = new CogOCRMaxCharKeyCollection(desiredString, "?");
            extractedRecord.CharacterCodes = characterCodes;

//            CogOCRMaxTuneRecordCollection autoCorrectResult = OCRMaxTool.AutoCorrect(extractedRecord);
            OCRMaxTool.AutoCorrect(extractedRecord);
        }

        public override void AddCharactor(CharPosition charPosition, int charactorCode)
        {
            CogOCRMaxPositionResult positionResult = ((CogCharPosition)charPosition).CogPositionResult;
            CogOCRMaxChar font = positionResult.GetCharacter();
            font.CharacterCode = charactorCode;
            OCRMaxTool.Classifier.Font.Add(font);

            OCRMaxTool.Classifier.Train();
        }

        public override List<CharFont> GetFontList()
        {
            List<CharFont> fontList = new List<CharFont>();

            foreach (CogOCRMaxChar character in OCRMaxTool.Classifier.Font)
            {
                CharFont charFont = new CharFont(CognexImageBuilder.ConvertImage(character.Image), ((char)character.CharacterCode).ToString(), character);
                fontList.Add(charFont);
            }

            fontList.Sort(delegate(CharFont charFont1, CharFont charFont2)
            {
                return charFont1.Character.CompareTo(charFont2.Character);
            });
            return fontList;
        }

        public override void RemoveFont(CharFont charFont)
        {
            OCRMaxTool.Classifier.Font.Remove((CogOCRMaxChar)charFont.AlgorithmCharObject);
        }

        public override void AddCharactor(AlgoImage charImage, string charactorCode)
        {
            throw new NotImplementedException();
        }

        public override int CalibrateFont(AlgoImage charImage, string calibrationString)
        {
            throw new NotImplementedException();
        }
    }
}
