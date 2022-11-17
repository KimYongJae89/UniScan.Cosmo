using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using System.IO;
using System;
using UniScanM.StillImage.Data;
using DynMvp.Base;

namespace UniScanM.StillImage.Algorithm
{
    public abstract class SheetFinder
    {
        static Dictionary<int, SheetFinder> _instance = new Dictionary<int, SheetFinder>();

        public static SheetFinder Create(int version)
        {
            SheetFinder instance = null;
            if (_instance.ContainsKey(version)==false)
            {
                switch (version)
                {
                    case 0:
                        instance = new SheetFinderV1();
                        break;
                }

                _instance.Add(version, instance);
            }
            else
            {
                instance = _instance[version];
            }
            
            return instance;
        }

        public static Point GetInspCenter(Size sheetSize)
        {
            //return GetFovRectTest(sheetSize);

            // Get FOV Conter
            float yPos = ((Data.Model)SystemManager.Instance().CurrentModel).FovYPos * sheetSize.Height;
            Point centerPoint = new Point(sheetSize.Width / 2, (int)Math.Round(yPos));

            return centerPoint;
        }

        public static Rectangle GetInspRect(Size sheetSize, Size fovSize)
        {
            Point center = SheetFinder.GetInspCenter(sheetSize);
            return DrawingHelper.FromCenterSize(center, fovSize);
        }

        private Rectangle GetFovRectTest(Size sheetSize)
        {
            Rectangle roiRect = Rectangle.FromLTRB(0, 0, sheetSize.Width, (int)(sheetSize.Height / 3.5f));
            return roiRect;
        }

        public abstract Rectangle FindSheet(AlgoImage algoImage);
    }

    public class SheetFinderV1 : SheetFinder
    {
        private Rectangle GetSheetRect(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, "SheetFinder"));
            //int skipRange = 500;
            //AlgoImage algoImage2 = null;
            try
            {
                //algoImage2 = algoImage.GetSubImage(Rectangle.FromLTRB(0, skipRange, algoImage.Width, algoImage.Height));
                //algoImage2.Save("algoImage2.bmp", debugContext);

                // Projection
                float[] projection = imageProcessing.Projection(algoImage, Direction.Vertical, ProjectionType.Mean);

                // Projection Binarize
                float threshold = projection.Average() * 1.2f;
                //float threshold = (projection.Max() + projection.Min()) / 2;
                if (threshold > 220 || threshold < 20)
                {
                    return Rectangle.Empty;
                }

                float[] projection2 = new float[projection.Length];
                Parallel.For(0, projection.Length, i =>
                {
                    projection2[i] = (projection[i] >= threshold ? 255 : 0);
                });
                //for (int i = 0; i < projection2.Length; i++)

                // Average Rising-edge Distance ( invalidate of upper 10% and lower 10%)
                List<int> distList = new List<int>();
                int edgeDist = 0;
                projection2.Aggregate((f, g) =>
                {
                    if (f != g && edgeDist > 0)
                    {
                        distList.Add(edgeDist);
                        edgeDist = 0;
                    }
                    else
                        edgeDist++;

                    return g;
                });
                //int skipCount = (int)(distList.Count * 0.1);
                //int takeCount = distList.Count - 2 * skipCount;
                //distList = distList.Skip(skipCount).Take(takeCount).ToList();
                if (distList.Count == 0)
                    return Rectangle.Empty;

                double avgDist = distList.Average();
                List<Point> foundPointList = new List<Point>();
                Point temppoint = new Point(-1, -1);
                List<Point> debugPointList = new List<Point>();
                for (int i = 0; i < projection2.Length; i++)
                {
                    if (projection2[i] == 0)
                    {
                        if (temppoint.X >= 0 && temppoint.Y >= 0 && (temppoint.Y - temppoint.X) > 2 * avgDist)
                        {
                            foundPointList.Add(temppoint);
                        }
                        temppoint.X = temppoint.Y = -1;
                    }
                    else
                    {
                        if (temppoint.X == -1)
                            temppoint.X = i;
                        temppoint.Y = i;
                    }
                    debugPointList.Add(temppoint);
                }

                // 가까운것끼리 합치기
                for(int i=0; i< foundPointList.Count-1; i++)
                {
                    int dist = foundPointList[i + 1].X - foundPointList[i].Y;
                    if (dist < 1000)
                    {
                        foundPointList[i] = new Point(foundPointList[i].X, foundPointList[i + 1].Y);
                        foundPointList.RemoveAt(i + 1);
                    }
                }

                // for Save Debug
                if (debugContext.SaveDebugImage)
                {
                    AlgoImage resizeImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, algoImage.Width / 5, algoImage.Height / 5);
                    imageProcessing.Resize(algoImage, resizeImage);
                    DebugHelper.SaveImage(resizeImage.ToImageD(), "resized.bmp", debugContext);
                    resizeImage.Dispose();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("Projection,Binarize,Edge(Src),Edge(Dst),Edge(len),Th,Dist"));
                    for (int i = 0; i < projection2.Length; i++)
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}", projection[i], projection2[i], debugPointList[i].X, debugPointList[i].Y, (debugPointList[i].Y - debugPointList[i].X), threshold, avgDist));
                    DebugHelper.SaveText(sb.ToString(), "Projection.txt", debugContext);
                }

                Rectangle sheetRect = Rectangle.Empty;
                if (foundPointList.Count == 2)
                {
                    int src = (foundPointList[0].X + foundPointList[0].Y) / 2;
                    int dst = (foundPointList[1].X + foundPointList[1].Y) / 2;
                    sheetRect = Rectangle.FromLTRB(0, src, algoImage.Width, dst);
                }
                else if (foundPointList.Count > 2)
                {
                    List<int> sheetLength = new List<int>();
                    foundPointList.Aggregate((f, g) =>
                    {
                        int dist = g.X - f.X;
                        sheetLength.Add(dist);
                        return g;
                    });
                    int maxLen = sheetLength.Max();
                    int maxIdx = sheetLength.FindIndex(f => f == maxLen);
                    int start = (foundPointList[maxIdx].X + foundPointList[maxIdx].Y) / 2;//Math.Min(foundPointList[maxIdx].Y, foundPointList[maxIdx + 1].Y);
                    int enddd = (foundPointList[maxIdx + 1].X + foundPointList[maxIdx + 1].Y) / 2;// Math.Max(foundPointList[maxIdx].X, foundPointList[maxIdx + 1].X);
                    //int start = (foundPointList[maxIdx].X + foundPointList[maxIdx].Y) / 2;
                    //int enddd = (foundPointList[maxIdx + 1].X + foundPointList[maxIdx + 1].Y) / 2;
                    sheetRect = Rectangle.FromLTRB(0, start, algoImage.Width, enddd);
                    
                    //algoImage.Save(@"d:\temp\tt.bmp");
                }
                foundPointList.Clear();

                //if(sheetRect.IsEmpty==false)
                //    sheetRect.Offset(0, skipRange);
                
                return sheetRect;
            }
            finally
            {
                //algoImage2?.Dispose();
            }

        }

        public override Rectangle FindSheet(AlgoImage algoImage)
        {
            Rectangle sheetRect = GetSheetRect(algoImage);
            Model model = SystemManager.Instance().CurrentModel as Model;
            if (model.SheetHeigthPx > 0)
            {
                float grabLengthMin = model.SheetHeigthPx * 0.95f;
                float grabLengthMax = model.SheetHeigthPx * 1.05f;
                if (grabLengthMin < sheetRect.Height && sheetRect.Height < grabLengthMax)
                    return sheetRect;
                else
                    return Rectangle.Empty;
            }
            return sheetRect;
        }
    }

    public class SheetFinderV2 : SheetFinder
    {
        private Rectangle GetSheetRect(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, "SheetFinder"));
            int skipRangeX = 0;
            int skipRangeY = 0;
            AlgoImage algoImage2 = null;
            try
            {
                algoImage2 = algoImage.GetSubImage(Rectangle.FromLTRB(0, skipRangeY, algoImage.Width, algoImage.Height));
                algoImage2.Save("algoImage2.bmp", debugContext);

                // Projection                
                float[] projection = imageProcessing.Projection(algoImage2, Direction.Vertical, ProjectionType.Mean);

                // Projection Binarize
                float threshold = projection.Average() * 1.2f;
                if (threshold > 220 || threshold < 50)
                {
                    return Rectangle.Empty;
                }

                float[] projection2 = new float[projection.Length];
                Parallel.For(0, projection.Length, i =>
                {
                    projection2[i] = (projection[i] >= threshold ? 255 : 0);
                });
                //for (int i = 0; i < projection2.Length; i++)

                // Average Rising-edge Distance ( invalidate of upper 10% and lower 10%)
                List<int> distList = new List<int>();
                int edgeDist = 0;
                projection2.Aggregate((f, g) =>
                {
                    if (f != g && edgeDist > 0)
                    {
                        distList.Add(edgeDist);
                        edgeDist = 0;
                    }
                    else
                        edgeDist++;

                    return g;
                });
                int skipCount = (int)(distList.Count * 0.1);
                int takeCount = distList.Count - 2 * skipCount;
                distList = distList.Skip(skipCount).Take(takeCount).ToList();

                double avgDist = distList.Average();
                List<Point> foundPointList = new List<Point>();
                Point temppoint = new Point(-1, -1);
                List<Point> debugPointList = new List<Point>();
                for (int i = 0; i < projection2.Length; i++)
                {
                    if (projection2[i] == 0)
                    {
                        if (temppoint.X >= 0 && temppoint.Y >= 0 && (temppoint.Y - temppoint.X) > 7 * avgDist)
                        {
                            foundPointList.Add(temppoint);
                        }
                        temppoint.X = temppoint.Y = -1;
                    }
                    else
                    {
                        if (temppoint.X == -1)
                            temppoint.X = i;
                        temppoint.Y = i;
                    }
                    debugPointList.Add(temppoint);
                }

                // for Save Debug
                //if (debugContext.SaveDebugImage)
                {
                    AlgoImage resizeImage = ImageBuilder.Build(algoImage2.LibraryType, algoImage2.ImageType, algoImage2.Width / 5, algoImage2.Height / 5);
                    imageProcessing.Resize(algoImage2, resizeImage);
                    DebugHelper.SaveImage(resizeImage.ToImageD(), "resized.bmp", debugContext);
                    resizeImage.Dispose();

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("Projection,Binarize,Edge(Src),Edge(Dst),Edge(len),Th,Dist"));
                    for (int i = 0; i < projection2.Length; i++)
                        sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}", projection[i], projection2[i], debugPointList[i].X, debugPointList[i].Y, (debugPointList[i].Y - debugPointList[i].X), threshold, avgDist));
                    DebugHelper.SaveText(sb.ToString(), "Projection.txt", debugContext);
                }

                Rectangle sheetRect = Rectangle.Empty;
                if (foundPointList.Count == 2)
                {
                    int src = (foundPointList[0].X + foundPointList[0].Y) / 2;
                    int dst = (foundPointList[1].X + foundPointList[1].Y) / 2;
                    sheetRect = Rectangle.FromLTRB(0, src, algoImage2.Width, dst);
                }
                else if (foundPointList.Count > 2)
                {
                    List<int> sheetLength = new List<int>();
                    foundPointList.Aggregate((f, g) =>
                    {
                        int dist = g.X - f.X;
                        sheetLength.Add(dist);
                        return g;
                    });
                    int maxLen = sheetLength.Max();
                    int maxIdx = sheetLength.FindIndex(f => f == maxLen);
                    //int start = Math.Min(foundPointList[maxIdx].Y, foundPointList[maxIdx + 1].Y);
                    //int enddd = Math.Max(foundPointList[maxIdx].X, foundPointList[maxIdx + 1].X);
                    int start = (foundPointList[maxIdx].X + foundPointList[maxIdx].Y) / 2;
                    int enddd = (foundPointList[maxIdx + 1].X + foundPointList[maxIdx + 1].Y) / 2;
                    sheetRect = Rectangle.FromLTRB(0, start, algoImage2.Width, enddd);
                }
                foundPointList.Clear();
                sheetRect.Offset(0, skipRangeY);
                return sheetRect;
            }
            finally
            {
                algoImage2?.Dispose();
            }

        }

        public override Rectangle FindSheet(AlgoImage algoImage)
        {
            Rectangle sheetRect = GetSheetRect(algoImage);
            return sheetRect;
        }
    }


}
