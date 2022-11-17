 using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.SEMCNS.Offline.Models;

namespace WPF.SEMCNS.Offline.Services
{
    public class InspectEventArgs : EventArgs
    {
        IEnumerable<Defect> _defectList;
        public IEnumerable<Defect> DefectList { get => _defectList; }

        public InspectEventArgs(IEnumerable<Defect> defectList)
        {
            _defectList = defectList;
        }
    }

    public delegate void InspectedEventHandler(InspectEventArgs e);

    public static class InspectService
    {
        public static event InspectedEventHandler Inspected;
        
        public static async Task inspectedProc(IEnumerable<Defect> defects)
        {
            await Task.Run(() => Inspected(new InspectEventArgs(defects)));
        }

        public static async Task<byte[]> GetTransposeBuffer(Image2D image)
        {
            return await Task.Run(() =>
            {
                AlgoImage algoImage = ImageBuilder.GetInstance(ImagingLibrary.MatroxMIL).Build(image, ImageType.Grey);
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

                var buffer = new byte[algoImage.Width * algoImage.Height];

                var source = algoImage.CloneByte();

                int bufferIndex = 0;
                for (int x = 0; x < algoImage.Width; x++)
                {
                    int sourceIndex = x;

                    for (int y = 0; y < algoImage.Height; y++)
                    {
                        buffer[bufferIndex++] = source[sourceIndex];
                        sourceIndex += algoImage.Width;
                    }
                }

                algoImage.Dispose();

                return buffer;
            });
        }

        public static async Task GetInversTransposeImage(AlgoImage defectImage, byte[] defectBuffer)
        {
            await Task.Run(() =>
            {
                var inverseBuffer = new byte[defectImage.Width * defectImage.Height];

                
                int bufferIndex = 0;

                for (int x = 0; x < defectImage.Width; x++)
                {
                    int inverseIndex = x;

                    for (int y = 0; y < defectImage.Height; y++)
                    {
                        inverseBuffer[inverseIndex] = defectBuffer[bufferIndex++];
                        inverseIndex += defectImage.Width;
                    }
                }

                defectImage.SetByte(inverseBuffer);
            });
        }

        public static async Task<Byte[]> GetSourceArray(Image2D image)
        {
            return await Task.Run(() =>
            {
                AlgoImage algoImage = ImageBuilder.GetInstance(ImagingLibrary.MatroxMIL).Build(image, ImageType.Grey);

                var sourceArray = algoImage.GetByte();
                algoImage.Dispose();

                return sourceArray;
            });
        }


        public static async Task<Byte[]> GetMaskArray(AlgoImage transposeImage, AlgoImage buffer, TargetParam targetParam)
        {
            return await Task.Run(() =>
            {
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(transposeImage);

                int binValue = (int)Math.Round(imageProcessing.Li(transposeImage));
                imageProcessing.Binarize(transposeImage, buffer, binValue, true);
                imageProcessing.FillHoles(buffer, buffer);

                var blobList = imageProcessing.Blob(buffer, new BlobParam { SelectArea = true, SelectLabelValue=true });

                var blobs = blobList.GetList();

                buffer.Clear();
                if (blobs.Count > 0)
                    imageProcessing.DrawBlob(buffer, blobList, blobs.Find(blob1 => blob1.Area == blobs.Max(blob => blob.Area)), new DrawBlobOption() { SelectBlob = true });

                blobList.Dispose();

                return buffer.CloneByte();
            });
        }

        public static async Task<float[]> GetProfile(byte[] sourceArray, byte[] maskArray, int width, int height)
        {
            return await Task.Run(() =>
            {
                var profile = new float[width];

                int index = 0;
                for (int x = 0; x < width; x++)
                {
                    double sum = 0;
                    int count = 0;
                    for (int y = 0; y < height; y++)
                    {
                        if (maskArray[index] > 0)
                        {
                            sum += sourceArray[index];
                            count++;
                        }
                        
                        index++;
                    }

                    if (count > 0)
                        profile[x] = (float)(sum / count);
                }

                return profile;
            });
        }

        public static async Task GetDefectArray(TargetParam targetParam, byte[] sourceArray, byte[] maskArray, byte[] lowerArray, byte[] upperArray, float[] profile, int width, int height)
        {
            await Task.Run(() =>
            {
                Array.Clear(lowerArray, 0, lowerArray.Length);
                Array.Clear(upperArray, 0, upperArray.Length);
                
                int index = 0;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (targetParam.StartYPixel < y && targetParam.EndHeightPixel > y && maskArray[index] > 0)
                        {
                            double sub = sourceArray[index] - profile[x];
                            if (sub < -targetParam.Lower)
                                lowerArray[index] = byte.MaxValue;
                            else if (sub > targetParam.Upper)
                                upperArray[index] = byte.MaxValue;
                        }

                        index++;
                    }
                }
            });
        }

        public static async Task<IEnumerable<BlobRect>> GetBlobList(TargetParam targetParam, Image2D image, AlgoImage buffer)
        {
            return await Task.Run(() =>
            {
                AlgoImage algoImage = ImageBuilder.GetInstance(ImagingLibrary.MatroxMIL).Build(image, ImageType.Grey);
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
                
                BlobParam blobParam = new BlobParam();
                blobParam.SelectArea = true;
                blobParam.SelectBoundingRect = true;
                blobParam.SelectMinValue = true;
                blobParam.SelectMaxValue = true;
                blobParam.SelectRotateRect = true;
                blobParam.SelectMeanValue = true;
                blobParam.SelectFeretDiameter = true;

                var collection = imageProcessing.Blob(buffer, blobParam, algoImage);

                var disposeTask = Task.Run(() =>
                {
                    collection.Dispose();
                });

                algoImage.Dispose();

                return collection.GetList().OrderByDescending(x => x.Area);
            });
        }

        public static async Task<IEnumerable<Defect>> GetDefectList(TargetParam targetParam, Image2D image, IEnumerable<BlobRect> blobRects, float[] profile)
        {
            return await Task.Run(() =>
            {
                AlgoImage algoImage = ImageBuilder.GetInstance(ImagingLibrary.MatroxMIL).Build(image, ImageType.Grey);

                var defectList = new List<Defect>();
                Rectangle sourceRect = new Rectangle(0, 0, algoImage.Width, algoImage.Height);
                foreach (var blobRect in blobRects)
                {
                    var rect = Rectangle.Round(blobRect.BoundingRect);
                    float diff = blobRect.MeanValue - profile[rect.X];
                    for (int i = rect.X; i < rect.Right ; i++)
                    {
                        if (diff < 0)
                            diff = Math.Min(blobRect.MinValue - profile[i], diff);
                        else
                            diff = Math.Max(blobRect.MaxValue - profile[i], diff);
                    }
                    
                    var defectType = diff > 0 ? DefectType.PInHole : DefectType.Dust;

                    var length = blobRect.MaxFeretDiameter * 26;

                    switch (defectType)
                    {
                        case DefectType.PInHole:
                            if (length < targetParam.UpperMinLength)
                                continue;
                            break;
                        case DefectType.Dust:
                            if (length < targetParam.LowerMinLength)
                                continue;
                            break;
                    }

                    var inflateRect = new Rectangle(rect.Location, rect.Size);

                    inflateRect.Inflate(50, 50);
                    inflateRect.Intersect(sourceRect);

                    var subImage = algoImage.GetSubImage(inflateRect);

                    defectList.Add(new Defect(blobRect, inflateRect, subImage.ToBitmapSource(), defectType, diff, 26));

                    subImage.Dispose();
                }

                algoImage.Dispose();

                return defectList;
            });
        }
    }
}
