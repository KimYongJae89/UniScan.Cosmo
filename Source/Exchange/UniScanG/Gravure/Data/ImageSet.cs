using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Data
{
    public delegate void OnSheetImageSetDisposeDelegate(SheetImageSet sheetImageSet);
    public delegate void ImageGrabCompleteDelegate();
    public class SheetImageSet : AlgoImage, IDisposable
    {
        public int SheetNo { get => this.sheetNo; }
        private int sheetNo = -1;

        public int RawImageHeight { get => this.rawImageHeight; }
        int rawImageHeight = -1;

        public List<AlgoImage> PartImageList { get => partImageList; }
        private List<AlgoImage> partImageList;

        public int FidXPos { get => this.fidXPos; }
        private int fidXPos;

        public OnSheetImageSetDisposeDelegate OnSheetImageSetDispose;

        public override int Width
        {
            get { return partImageList.Max(f => f.Width); }
        }

        public override int Height
        {
            get { return partImageList.Sum(f => f.Height); }
        }

        public override int Pitch => throw new NotImplementedException();
        public override int BitPerPixel => throw new NotImplementedException();

        public int Count
        {
            get { return partImageList.Count;}
        }

        public SheetImageSet(int sheetNo)
        {
            this.sheetNo = sheetNo;
            this.partImageList = new List<AlgoImage>();
            this.fidXPos = 0;
        }

        public SheetImageSet(int sheetNo, List<AlgoImage> subImageList, int fidXPos)
        {
            this.sheetNo = sheetNo;
            this.partImageList = new List<AlgoImage>(subImageList);
            this.fidXPos = fidXPos;
            this.LibraryType = subImageList[0].LibraryType;
        }

        ~SheetImageSet()
        {
            this.Dispose();
        }

        public override void Dispose()
        {
            foreach (AlgoImage subImage in this.partImageList)
                subImage.Dispose();

            partImageList.Clear();

            if (OnSheetImageSetDispose != null)
                OnSheetImageSetDispose(this);
        }

        public void AddSubImage(AlgoImage algoImage )
        {
            partImageList.Add(algoImage);
            if(partImageList.Count==1)
                this.LibraryType = partImageList[0].LibraryType;
        }

        //public ImagingLibrary LibraryType
        //{
        //    get { return subImageList[0].LibraryType; }
        //}


        public override AlgoImage Clone()
        {
            List<AlgoImage> subImageList = new List<AlgoImage>();
            foreach (AlgoImage subImage in this.partImageList)
            {
                subImageList.Add(subImage.Clone());
            }
            SheetImageSet sheetImageSet = new SheetImageSet(this.sheetNo, subImageList, this.fidXPos);
            sheetImageSet.Tag = this.Tag;
            return sheetImageSet;
        }

        //public override AlgoImage Clone(ImageType imageType)
        //{
        //    List<AlgoImage> subImageList = new List<AlgoImage>();
        //    foreach (AlgoImage subImage in this.partImageList)
        //    {
        //        subImageList.Add(subImage.Clone(imageType));
        //    }
        //    return new SheetImageSet(this.sheetNo, subImageList, this.fidXPos);
        //}

        public AlgoImage GetChildImage(int i)
        {
            return this.partImageList[i];
        }

        public Size GetImageSize()
        {
            return new Size(this.Width, this.Height);
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            Point rectOffset = rectangle.Location;
            Size rectSize = rectangle.Size;
            int accHeigth = 0;
            List<AlgoImage> partialImageList = new List<AlgoImage>();
            foreach (AlgoImage subImage in this.partImageList)
            {
                Point offset = new Point(0, accHeigth);
                accHeigth += subImage.Height;
                Rectangle globalImageRect = new Rectangle(offset.X, offset.Y, subImage.Width, subImage.Height);

                Rectangle globalIntersectRect = Rectangle.Intersect(globalImageRect, rectangle);

                if (globalIntersectRect.Width == 0 || globalIntersectRect.Height == 0)
                    continue;

                Rectangle imageIntersectRect = globalIntersectRect;
                imageIntersectRect.Offset(-offset.X, -offset.Y);

                //if (imageIntersectRect.Size == rectangle.Size)
                //{
                //    dstImage = subImage.GetSubImage(imageIntersectRect, syncDispose);
                //}

                partialImageList.Add(subImage.GetSubImage(imageIntersectRect));
            }

            if (partialImageList.Count == 0)
            {
                dstImage = null;
            }
            else if (partialImageList.Count == 1)
            {
                dstImage = partialImageList[0];
            }
            else
            {
                AlgoImage baseAlgoImage = this.partImageList[0];
                AlgoImage subAlgoImage = ImageBuilder.Build(baseAlgoImage.LibraryType, baseAlgoImage.ImageType, rectSize.Width, rectSize.Height);
                Point dstPoint = new Point();
                foreach (AlgoImage partialAlgoImage in partialImageList)
                {
                    subAlgoImage.Copy(partialAlgoImage, Point.Empty, dstPoint, partialAlgoImage.Size);
                    dstPoint.Y += partialAlgoImage.Height;
                }
                partialImageList.ForEach(f => f.Dispose());
                //subAlgoImage.Save(@"D:\temp\ttt.bmp");
                dstImage = subAlgoImage;
            }
        }

        public ImageD ToImageD(float scale = 1)
        {
            AlgoImage algoImage = this.GetFullImage(scale);
            ImageD imageD = (Image2D)algoImage.ToImageD();
            algoImage.Dispose();
            return imageD;
        }

        public AlgoImage GetFullImage(float scale = 1)
        {
            return GetFullImage(new SizeF(scale, scale));
        }

        public AlgoImage GetFullImage(SizeF scale)
        {
            //LogHelper.Debug(LoggerType.Function, "SheetImageSet::GenerateWholeImage Start");

            Size size = this.GetImageSize();
            Size resizeFull = new Size();
            resizeFull.Width = (int)Math.Truncate(size.Width * scale.Width);
            resizeFull.Height = (int)Math.Truncate(size.Height * scale.Height);
            AlgoImage fullResizeAlgoImage = ImageBuilder.Build(this.LibraryType, ImageType.Grey, resizeFull.Width, resizeFull.Height);
            //LogHelper.Debug(LoggerType.Function, string.Format("resizeFull: {0}", resizeFull));

            int offsetY = 0;
            foreach (AlgoImage subImage in this.partImageList)
            {
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(subImage);

                int resizeSubWidth = (int)Math.Round(subImage.Width * scale.Width, MidpointRounding.AwayFromZero);
                int resizeSubHeight = (int)Math.Round(subImage.Height * scale.Height, MidpointRounding.AwayFromZero);
                Rectangle resizeSubRect = new Rectangle(0, offsetY, resizeSubWidth, resizeSubHeight);
                if (resizeSubRect.Bottom > resizeFull.Height)
                    resizeSubRect.Height -= (resizeSubRect.Bottom - resizeFull.Height);

                LogHelper.Debug(LoggerType.Function, string.Format("resizeSubRect: L{0} T{1} R{2} B{3}", resizeSubRect.Left, resizeSubRect.Top, resizeSubRect.Right, resizeSubRect.Bottom));
                if (resizeSubRect.Height == 0)
                    continue;

                Debug.Assert(resizeSubRect.Bottom <= resizeFull.Height);

                AlgoImage partResizeAlgoImage = fullResizeAlgoImage.GetSubImage(resizeSubRect);
                imageProcessing.Resize(subImage, partResizeAlgoImage);
                partResizeAlgoImage.Dispose();

                offsetY = resizeSubRect.Bottom;
            }
            Debug.Assert(offsetY == resizeFull.Height);

            //LogHelper.Debug(LoggerType.Function, "SheetImageSet::GenerateWholeImage End");
            return fullResizeAlgoImage;
        }

        public override byte[] CloneByte()
        {
            throw new NotImplementedException();
        }

        public override void PutByte(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            throw new NotImplementedException();
        }

        public override ImageD ToImageD()
        {
            throw new NotImplementedException();
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            throw new NotImplementedException();
        }

        public override void Save(string fileName)
        {
            AlgoImage fullImage = this.GetFullImage();
            fullImage.Save(fileName);
            fullImage.Dispose();
        }

        public override void Clear(byte initVal = 0)
        {
            throw new NotImplementedException();
        }

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            throw new NotImplementedException();
        }

        public override IntPtr GetImagePtr()
        {
            throw new NotImplementedException();
        }

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
