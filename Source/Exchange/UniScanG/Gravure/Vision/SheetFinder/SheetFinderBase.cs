using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System.IO;
using UniEye.Base.Settings;
using UniScanG.Inspect;
using UniScanG.Gravure.Inspect;

namespace UniScanG.Gravure.Vision.SheetFinder
{
    public enum BaseXSearchDir { Left2Right, Right2Left }
    public abstract class SheetFinderBaseParam : AlgorithmParam
    {
        protected BaseXSearchDir baseXSearchDir;
        protected System.Drawing.Size fidSize = new System.Drawing.Size(500, 150);

        public BaseXSearchDir BaseXSearchDir
        {
            get { return baseXSearchDir; }
            set { baseXSearchDir = value; }
        }

        public System.Drawing.Size FidSize
        {
            get { return fidSize; }
            set { fidSize = value; }
        }

        public SheetFinderBaseParam()
        {
            SystemManager systemManager = SystemManager.Instance();

            int camIndex = (systemManager == null ? 0 : systemManager.ExchangeOperator.GetCamIndex());
            this.baseXSearchDir = (camIndex == 0 ? BaseXSearchDir.Left2Right : BaseXSearchDir.Right2Left);
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            this.baseXSearchDir = (BaseXSearchDir)Enum.Parse(typeof(BaseXSearchDir), XmlHelper.GetValue(paramElement, "BaseXSearchDir", baseXSearchDir.ToString()));

            int fidSizeWidth = Convert.ToInt32(XmlHelper.GetValue(paramElement, "FidSizeWidth", fidSize.Width.ToString()));
            int fidSizeHeight = Convert.ToInt32(XmlHelper.GetValue(paramElement, "FidSizeHeight", fidSize.Height.ToString()));
            this.fidSize = new System.Drawing.Size(fidSizeWidth, fidSizeHeight);
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "BaseXSearchDir", baseXSearchDir.ToString());

            XmlHelper.SetValue(paramElement, "FidSizeWidth", fidSize.Width.ToString());
            XmlHelper.SetValue(paramElement, "FidSizeHeight", fidSize.Height.ToString());
        }

    }

    public abstract class SheetFinderBase : Algorithm
    {
        public static string TypeName { get { return "SheetFinder"; } }

        #region Abstract
        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            throw new NotImplementedException();
        }
        public override string GetAlgorithmType()
        {
            return TypeName;
        }
        public override string GetAlgorithmTypeShort()
        {
            throw new NotImplementedException();
        }
        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new NotImplementedException();
        }
        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region virtual
        #endregion

        public abstract int GetBoundSize();

        float[] projData = new float[0];
        public int FindBasePosition(AlgoImage algoImage, Direction direction, int length)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            SheetFinderBaseParam sheetFinderBaseParam = this.param as SheetFinderBaseParam;

            Rectangle subRect = new Rectangle(Point.Empty, algoImage.Size);
            Size inflate = Size.Empty;
            switch (direction)
            {
                case Direction.Horizontal:
                    inflate.Height = -(algoImage.Height * 3 / 8);
                    break;
                case Direction.Vertical:
                    inflate.Width = -(algoImage.Width * 3 / 8);
                    break;
            }
            subRect.Inflate(inflate);

            AlgoImage subImage = algoImage.GetSubImage(subRect);
            int nbEntry = imageProcessing.Projection(subImage, ref projData, direction, ProjectionType.Mean);
            float threshold = imageProcessing.GetGreyAverage(subImage);
            subImage.Dispose();
            int curLength = 0;

            if (direction == Direction.Vertical ||
                sheetFinderBaseParam.BaseXSearchDir == BaseXSearchDir.Left2Right)
            // Left -> Right 
            {
                bool level = projData[0] > threshold;
                for (int i = 1; i < nbEntry; i++)
                {
                    bool curLevel = projData[i] > threshold;
                    if (curLevel == level)
                        curLength++;

                    if (level == true && curLevel == false && curLength > length)
                        return i;

                    if (curLevel != level)
                        curLength = 0;

                    level = curLevel;
                }
            }
            else
            // Right -> Left
            {
                bool level = projData[nbEntry - 1] > threshold;
                for (int i = nbEntry - 2; i >= 0; i--)
                {
                    bool curLevel = projData[i] > threshold;

                    if (curLevel == level)
                        curLength++;

                    if (level == true && curLevel == false && curLength > length)
                        return i;

                    if (curLevel != level)
                        curLength = 0;

                    level = curLevel;
                }
            }


            //float[] projData = imageProcessing.Projection(subImage, direction, ProjectionType.Mean);
            //subImage.Dispose();
            //float threshold = projData.Average();
            //int curLength = 0;

            //if (direction == Direction.Vertical ||
            //    sheetFinderBaseParam.BaseXSearchDir == BaseXSearchDir.Left2Right)
            //    // Left -> Right 
            //{
            //    bool level = projData[0] > threshold;
            //    for (int i = 1; i < projData.Length; i++)
            //    {
            //        bool curLevel = projData[i] > threshold;
            //        if (curLevel == level)
            //            curLength++;

            //        if (level == true && curLevel == false && curLength > length)
            //            return i;

            //        if (curLevel != level)
            //            curLength = 0;

            //        level = curLevel;
            //    }
            //}
            //else
            //// Right -> Left
            //{
            //    bool level = projData.Last() > threshold;
            //    for (int i = projData.Length-2; i >=0; i--)
            //    {
            //        bool curLevel = projData[i] > threshold;

            //        if (curLevel == level)
            //            curLength++;

            //        if (level == true && curLevel == false && curLength > length)
            //            return i;

            //        if (curLevel != level)
            //            curLength = 0;

            //        level = curLevel;
            //    }
            //}

            return -1;
        }

    }
}
