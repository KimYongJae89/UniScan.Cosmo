using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Screen.Vision.Detector;

namespace UniScanG.Data
{
    public class MergeSheetResult : SheetResult
    {
        bool importResult = false;
        public bool ImportResult
        {
            get { return importResult; }
        }

        int index;
        public int Index
        {
            get { return index; }
        }
        
        public bool IsNG
        {
            get { return SheetSubResultList.Count != 0; }
        }

        public string resultPath;


        public MergeSheetResult(int index, string path, bool import = true)
        {
            this.index = index;
            resultPath = path;
            this.prevImagePath = Path.Combine(resultPath, "Prev.jpg");
            if (import == true)
                this.Import(path);
        }

        public MergeSheetResult(int index, SheetResult sheetResult)
        {
            this.index = index;
            this.Copy(sheetResult);
        }

        public void AdjustSizeFilter(float minSize, float maxSize)
        {
            sheetSubResultList = sheetSubResultList.FindAll
                (s => Math.Max(s.RealRegion.Width , s.RealRegion.Height) >= minSize
                && Math.Max(s.RealRegion.Width, s.RealRegion.Height) <= maxSize);
        }

        public override void Export(string path, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(path);
            string fileName = Path.Combine(path, string.Format("{0}.csv", SheetInspector.TypeName));
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format("{0},{1},{2},{3}", this.index, SpandTime, this.sheetSize.Width, this.sheetSize.Height));

            if (PrevImage != null)
            {
                Bitmap newBitmap = null;
                lock (prevImage)
                    newBitmap = (Bitmap)prevImage.Clone();
                ImageHelper.SaveImage(newBitmap, Path.Combine(path, string.Format("Prev.jpg")));
                newBitmap.Dispose();
            }

            List<SheetSubResult> sheetSubResultList = SheetSubResultList;
            if (sheetSubResultList.Count > 0)
                stringBuilder.AppendLine(sheetSubResultList.First().GetExportHeader());

            foreach (SheetSubResult subResult in sheetSubResultList)
            {
                stringBuilder.AppendFormat(subResult.ToExportData());
                stringBuilder.AppendLine();
            }

            try
            {
                File.WriteAllText(fileName, stringBuilder.ToString());
                foreach (SheetSubResult subResult in sheetSubResultList)
                {
                    if (subResult.Image != null)
                    {
                        string savePath = Path.Combine(path, string.Format("S{0}_C{1}_I{2}.jpg", this.index, subResult.CamIndex, subResult.Index));
                        Image2D image2D = Image2D.ToImage2D(subResult.Image);
                        image2D.SaveImage(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        image2D.Dispose();
                    }

                    if (subResult.BufImage != null)
                    {
                        string savePath = Path.Combine(path, string.Format("S{0}_C{1}_I{2}B.jpg", this.index, subResult.CamIndex, subResult.Index));
                        Image2D image2D = Image2D.ToImage2D(subResult.BufImage);
                        image2D.SaveImage(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        image2D.Dispose();
                    }
                }
            }
            catch (IOException)
            { }
        }

        public void ImportPrevImage()
        {
            prevImage  = this.PrevImage;
            //string imageName = Path.Combine(resultPath, "Prev.jpg");
            //if (File.Exists(imageName) == true)
            //{
            //    //Bitmap image = (Bitmap)ImageHelper.LoadImage(imageName);
            //    prevImage = (Bitmap)ImageHelper.LoadImage(imageName);
            //}
        }

        public override bool Import(string path)
        {
            string fileName = Path.Combine(path, string.Format("{0}.csv", SheetInspector.TypeName));

            if (File.Exists(fileName) == false)
                return false;

            string[] lines = File.ReadAllLines(fileName);
            int lineCount = lines.Length;
            string[] token = lines[0].Split(',');
            if (token.Length == 1)
            {
                this.spandTime = TimeSpan.Parse(lines[0]);
            }
            else
            {
                this.index = int.Parse(token[0]);
                this.spandTime = TimeSpan.Parse(token[1]);
                if (token.Length == 4)
                {
                    this.sheetSize.Width = float.Parse(token[2]);
                    this.sheetSize.Height = float.Parse(token[3]);
                }
            }

            this.prevImagePath = Path.Combine(path, string.Format("Prev.Jpg"));
            //if (File.Exists(prevImagePath))
            //prevImage = ImageHelper.LoadImage(prevImagePath) as Bitmap;

            for (int i = 2; i < lineCount; i++)
            {
                string line = lines[i];

                SheetSubResult subResult = new UniScanG.Gravure.Data.SheetSubResult();
                subResult.FromExportData(line);

                string imagePath = Path.Combine(path, string.Format("S{0}_C{1}_I{2}.jpg", this.index, subResult.CamIndex, subResult.Index));
                if (File.Exists(imagePath) == false) // 하위 호환성
                {
                    string imagePathOld = Path.Combine(path, string.Format("C{0}I{1}.jpg", subResult.CamIndex, subResult.Index));
                    if (File.Exists(imagePathOld))
                        imagePath = imagePathOld;
                }

                subResult.ImagePath = imagePath;

                string bufImagePath = Path.Combine(path, string.Format("S{0}_C{1}_I{2}B.jpg", this.index, subResult.CamIndex, subResult.Index));
                if (File.Exists(bufImagePath))
                    subResult.BufImagePath = bufImagePath;

                sheetSubResultList.Add(subResult);

                //subResult.RealLength = Math.Max(sheetSubResult.RealRegion.Width, sheetSubResult.RealRegion.Height) * 1000.0f;
            }

            importResult = true;
            return true;
        }

        public void ImportImage()
        {
            string prevImagePath = Path.Combine(this.resultPath, string.Format("Prev.Jpg"));
            if (File.Exists(prevImagePath))
                prevImage = ImageHelper.LoadImage(prevImagePath) as Bitmap;

            foreach (SheetSubResult subResult in sheetSubResultList)
            {
                if (string.IsNullOrEmpty(subResult.ImagePath))
                {
                    string imagePath = Path.Combine(this.resultPath, string.Format("C{0}I{1}.jpg", subResult.CamIndex, subResult.Index));
                    if (File.Exists(imagePath))
                        subResult.ImagePath = imagePath;

                    string bufImagePath = Path.Combine(this.resultPath, string.Format("C{0}I{1}B.jpg", subResult.CamIndex, subResult.Index));
                    if (File.Exists(bufImagePath))
                        subResult.BufImagePath = bufImagePath;
                }
            }
        }
    }
}
