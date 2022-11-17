using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UinScanGTest.Operation
{
    public class Processer
    {
        public void Start(Image2D image)
        {
            Stopwatch sw = Stopwatch.StartNew();
            
            AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, image, ImageType.Grey);
            AlgoImage buffer = ImageBuilder.Build(ImagingLibrary.MatroxMIL, algoImage.ImageType, algoImage.Size);
            DebugWriteLine("Image Build", sw.Elapsed.TotalMilliseconds);

            if(algoImage != null && buffer != null)
            {
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

                ip.Binarize(algoImage, buffer, true);
                DebugWriteLine("Threshold", sw.Elapsed.TotalMilliseconds);

                List<BlobRect> blobRectList = BlobProcess(buffer, algoImage);
                DebugWriteLine("Blob", sw.Elapsed.TotalMilliseconds);

                AlgoImage colorImage = algoImage.ConvertTo(ImageType.Color);
                blobRectList.ForEach(f => ip.DrawRect(colorImage, DrawingHelper.FromCenterSize(System.Drawing.Point.Round(f.CenterPt), new System.Drawing.Size(10, 10)), (double)System.Drawing.Color.Red.ToArgb(), true));
                DebugWriteLine("Draw", sw.Elapsed.TotalMilliseconds);

                colorImage.Save(@"C:\temp\colorImage.bmp");
                DebugWriteLine("Save", sw.Elapsed.TotalMilliseconds);
            }

            algoImage.Dispose();
            buffer.Dispose();
            DebugWriteLine("Image Dispose", sw.Elapsed.TotalMilliseconds);

        }

        private List<BlobRect> BlobProcess(AlgoImage binalImage, AlgoImage greyImagee)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(binalImage);

            BlobParam blobParam = new BlobParam();
            blobParam.EraseBorderBlobs = true;
            blobParam.SelectArea = true;
            //blobParam.SelectAspectRatio= true;
            blobParam.SelectBoundingRect = true;
            blobParam.SelectMeanValue = true;

            BlobRectList blobRectList = ip.Blob(binalImage, blobParam, greyImagee);
            List<BlobRect> blobRectList2 = blobRectList.GetList();
            blobRectList.Dispose();
            return blobRectList2;
        }

        private void DebugWriteLine(string message, double elapsedTimeMs)
        {
            Debug.WriteLine(string.Format("{0}, {1}, {2:F3}", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff"), message, elapsedTimeMs));
        }
    }
}
