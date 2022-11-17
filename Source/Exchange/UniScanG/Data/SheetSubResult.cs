using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data.Vision;
using UniScanG.Screen.Vision;
using UniScanG.Vision;

namespace UniScanG.Data
{
    public abstract class SheetSubResult : AlgorithmResult
    {
        protected int camIndex;
        public int CamIndex
        {
            get { return camIndex; }
            set { camIndex = value; }
        }

        protected int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        protected float lowerDiffValue;
        public float LowerDiffValue
        {
            get { return lowerDiffValue; }
            set { lowerDiffValue = value; }
        }

        protected float upperDiffValue;
        public float UpperDiffValue
        {
            get { return upperDiffValue; }
            set { upperDiffValue = value; }
        }

        protected float compactness;
        public float Compactness
        {
            get { return compactness; }
            set { compactness = value; }
        }

        protected Rectangle region;
        public Rectangle Region
        {
            get { return region; }
            set { region = value; }
        }

        protected RectangleF realRegion;
        public RectangleF RealRegion
        {
            get { return realRegion; }
            set { realRegion = value; }
        }

        protected PointF realCenterPos;
        public PointF RealCenterPos
        {
            get { return realCenterPos; }
            set { realCenterPos = value; }
        }

        protected float length;
        public float Length
        {
            get { return length; }
            set { length = value; }
        }

        protected float realLength;
        public float RealLength
        {
            get { return realLength; }
            set { realLength = value; }
        }

        protected float fillRate;
        public float FillRate
        {
            get { return fillRate; }
            set { fillRate = value; }
        }

        protected string imagePath;
        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }

        protected Bitmap image;
        public Bitmap Image
        {
            get
            {
                if (image == null)
                {
                    if (File.Exists(this.imagePath))
                    {
                        image = (Bitmap)ImageHelper.LoadImage(this.imagePath);
                        this.imagePath = null;
                    }
                }
                return image;
            }
            set { image = value; }
        }


        private string bufImagePath;
        public string BufImagePath
        {
            get { return bufImagePath; }
            set { bufImagePath = value; }
        }

        protected Bitmap bufImage;
        public Bitmap BufImage
        {
            get
            {
                if (bufImage == null)
                {
                    if (File.Exists(this.bufImagePath))
                    {
                        bufImage = (Bitmap)ImageHelper.LoadImage(this.bufImagePath);
                        this.bufImagePath = null;
                    }
                }
                return bufImage;
            }
            set { bufImage = value; }
        }

        public void Offset(int x, int y, float ratio)
        {
            resultRect.Offset(x, y);
            //realRegion.Offset((float)x * ratio, (float)y * ratio);
        }
        
        public Figure GetFigure(int width, float ratio = 1.0f)
        {
            int inflate = 30;
            Color defectColor = GetColor();

            Rectangle inflateRegion = region;
            inflateRegion.Inflate(inflate, inflate);
            inflateRegion.X = (int)Math.Round(inflateRegion.X * ratio);
            inflateRegion.Y = (int)Math.Round(inflateRegion.Y * ratio);
            inflateRegion.Width = (int)Math.Round(inflateRegion.Width * ratio);
            inflateRegion.Height = (int)Math.Round(inflateRegion.Height * ratio);
            RectangleFigure rectangleFigure = new RectangleFigure(inflateRegion, new Pen(defectColor, width));
            rectangleFigure.Tag = this;

            return rectangleFigure;
        }

        public abstract Color GetColor();
        public abstract Color GetBgColor();
        public abstract DefectType GetDefectType();
        public abstract string GetDefectTypeDiscription();

        public abstract string GetExportHeader();
        public abstract string ToExportData();
        public abstract bool FromExportData(string line);
        public abstract void ImportImage();


    }
}
