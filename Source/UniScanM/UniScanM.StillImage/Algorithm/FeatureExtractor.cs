﻿using DynMvp.Base;
using DynMvp.Vision;
using UniScanM.StillImage.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniScanM.StillImage.Algorithm
{
    /// <summary>
    /// Extract Feature in Image
    /// </summary>
    public abstract class FeatureExtractor
    {
        public static FeatureExtractor Create(int version)
        {
            switch (version)
            {
                case 0:
                    return new FeatureExtractorV1();
                //case 1:
                //    return new FeatureExtractorV2();
                default:
                    throw new Exception("Version is invalid");
            }
        }

        /// <summary>
        /// Extract Feature in Image
        /// </summary>
        /// <param name="algoImage">Image</param>
        /// <param name="cropRect">Clip by rectangle</param>
        /// <param name="applyFilter">Clip by Size and Length</param>
        /// <returns>Extracted Feature List</returns>
        public abstract TeachData Extract(AlgoImage algoImage);
        public abstract List<MatchResult> Match(AlgoImage algoImage, TeachData teachData, float matchRatio);

    }

    public class FeatureExtractorV1 : FeatureExtractor
    {
        float rate = 0.10f;

        private List<ShapeInfo> GetShapeInfo(AlgoImage binalAlgoImage, DebugContext debugContext)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(binalAlgoImage);

            BlobRectList blobRectList = null;

            try
            {
                BlobParam blobParam = new BlobParam();
                blobParam.SelectArea = true;
                blobParam.SelectCenterPt = true;
                blobParam.SelectBoundingRect = true;

                // 블랍을 얻는다.
                blobRectList = imageProcessing.Blob(binalAlgoImage, blobParam);

                List<BlobRect> blobRects = blobRectList.GetList();
                Rectangle imageRect = new Rectangle(Point.Empty, binalAlgoImage.Size);
                imageRect.Inflate(-5, -5);

                List<ShapeInfo> shapeInfoList = new List<ShapeInfo>();
                int sId = 0;
                shapeInfoList.AddRange(blobRects.ConvertAll<ShapeInfo>(f =>
                {
                    Rectangle boundingRect = Rectangle.Round(f.BoundingRect);
                    int area = (int)f.Area;
                    double length = Math.Sqrt(Math.Pow(f.BoundingRect.Width, 2) + Math.Pow(f.BoundingRect.Height, 2));

                                Rectangle waistRect = Rectangle.Inflate(boundingRect, 0, -(boundingRect.Height - 2) / 2);
                    float centerX = waistRect.Width / 2;
                    AlgoImage waistImage = binalAlgoImage.GetSubImage(waistRect);
                    int waist = waistImage.Width;
                    float[] proj = imageProcessing.Projection(waistImage, Direction.Horizontal, ProjectionType.Mean);
                    List<Point> hills = FindHill(proj);
                    Point centerHill = hills.Find(g => g.X < centerX && centerX < g.Y);
                    if (!centerHill.IsEmpty)
                        waist = centerHill.Y - centerHill.X + 1;

                    //waistImage.Save(@"d:\temp\waistImage.bmp");
                    //AlgoImage boundingRectImage = binalAlgoImage.GetSubImage(boundingRect);
                    //boundingRectImage.Save(@"d:\temp\boundingRectImage.bmp");
                    //boundingRectImage.Dispose();

                    waistImage.Dispose();
                    return new ShapeInfo(sId++, boundingRect, area, waist, length);
                }));

                return shapeInfoList;
            }
            finally
            {
                blobRectList?.Dispose();
            }
        }

        private List<Point> FindHill(float[] proj)
        {
            List<Point> hillList = new List<Point>();
            float mean = 126;

            Point curHill = new Point(-1, -1);
            for (int i = 0; i < proj.Length; i++)
            {
                float cur = proj[i];
                if (cur > mean)
                {
                    if (curHill.X < 0)
                        curHill.X = i;
                }
                else
                {
                    if (curHill.X >= 0)
                    {
                        curHill.Y = i - 1;
                        hillList.Add(curHill);
                        curHill = new Point(-1, -1);
                    }
                }
            }

            if(curHill.X>=0)
            {
                curHill.Y = proj.Length - 1;
                hillList.Add(curHill);
            }
            return hillList;
        }

        public override TeachData Extract(AlgoImage algoImage)
        {
            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, "FeatureExtractorV1_Extract"));
            if (debugContext.SaveDebugImage)
            {
                string[] files = Directory.GetFiles(debugContext.FullPath);
                foreach (string file in files)
                    File.Delete(file);
            }

            if (algoImage == null)
            {
                return new TeachData(-1, null);
            }

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            AlgoImage tempImage = null;
            AlgoImage debugImage = null;
            try
            {
                algoImage.Save("algoImage.bmp", debugContext);
                tempImage = algoImage.Clone();
                int globalBinValue = (int)imageProcessing.Binarize(tempImage);

                if (globalBinValue > 200 || globalBinValue < 15)
                {
                    return new TeachData(-1, null);
                }
                imageProcessing.Binarize(tempImage, globalBinValue, true);
                tempImage.Save("binalImage.bmp", debugContext);

                // Get ShapeInfo
                List<ShapeInfo> shapeInfoList = GetShapeInfo(tempImage, debugContext);
                shapeInfoList.RemoveAll(f => f.Area < 100);
                if (true)
                {
                    //shapeInfoList.Clear();
                }
                else
                {
                    // Test
                    // 대상이 너무 작으면 제거.
                    // 대상이 원(타원)에 가까우면 제거.
                    // 사각형 넓이: S = W*H
                    // 동일 사각형에 접하는 타원의 넓이: 장축*단축*PI = W/2*H/2*PI = S*PI/4
                    shapeInfoList.RemoveAll(f =>
                    {
                        if (f.Length < 100)
                            return true;

                        float rectangleArea = f.BaseRect.Width * f.BaseRect.Height;
                        float elipseArea = (float)(rectangleArea * Math.PI / 4);
                        float mid = (rectangleArea + elipseArea) / 2;
                        float area = f.Area;
                        return f.Area - mid > 0; // 사각형에 더 가깝다!

                    });
                }

                if (shapeInfoList.Count == 0)
                {
                    return new TeachData(-1, null);
                }

                // Get PatternInfo
                List<PatternInfo> patternInfoList = GetPatternInfo(shapeInfoList, debugContext);

                // 모양 정보로 구분.
                PatternInfoGroupList patternInfoGroupList = Grouping(patternInfoList, debugContext);
                patternInfoGroupList.Sort();

                // 개수 n개 이하는 무시.
                patternInfoGroupList.RemoveAll(f => f.ShapeInfoList.Count <= 5);
                patternInfoGroupList.UpdateId();
                if (patternInfoGroupList.Count > 0)
                    patternInfoGroupList[0].TeachInfo.Use = true;

                //TeachData teachData = new TeachData(globalBinValue, debugImage.ToImageD().Resize(0.1f, 0.1f));
                AlgoImage resizeImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, new Size(algoImage.Width / 10, algoImage.Height / 10));
                imageProcessing.Resize(algoImage, resizeImage);
                ImageD imageD = resizeImage.ToImageD();
                //TeachData teachData = new TeachData(globalBinValue, imageD.Resize(0.1f, .1f));
                TeachData teachData = new TeachData(globalBinValue, imageD);
                resizeImage.Dispose();

                teachData.PatternInfoGroupList.AddRange(patternInfoGroupList);

                if (debugContext.SaveDebugImage)
                    teachData.SaveDebug(debugContext.FullPath);

                // 종료
                return teachData;
            }
            finally
            {
                tempImage?.Dispose();
                debugImage?.Dispose();
            }
        }

        private List<PatternInfo> GetPatternInfo(List<ShapeInfo> shapeInfoList, DebugContext debugContext)
        {
            List<PatternInfo> patternInfoList = new List<PatternInfo>();

            if (shapeInfoList.Count == 0)
                return patternInfoList;

            int xMax = shapeInfoList.Max(f => f.BaseRect.Right);
            int yMax = shapeInfoList.Max(f => f.BaseRect.Bottom);

            Parallel.ForEach(shapeInfoList, shapeInfo =>
            //foreach (ShapeInfo shapeInfo in shapeInfoList)
            {
                Point point = Point.Round(DrawingHelper.CenterPoint(shapeInfo.BaseRect));
                ShapeInfo left, right, up, down;

                // Get Neighbothood Info
                left = FindNearest(shapeInfoList, Rectangle.FromLTRB(0, shapeInfo.BaseRect.Top, shapeInfo.BaseRect.Left, shapeInfo.BaseRect.Bottom),
                    shapeInfo.BaseRect.Left, (f) => f.BaseRect.Right);
                up = FindNearest(shapeInfoList, Rectangle.FromLTRB(shapeInfo.BaseRect.Left, 0, shapeInfo.BaseRect.Right, shapeInfo.BaseRect.Top),
                    shapeInfo.BaseRect.Top, (f) => f.BaseRect.Bottom);
                right = FindNearest(shapeInfoList, Rectangle.FromLTRB(shapeInfo.BaseRect.Right, shapeInfo.BaseRect.Top, xMax, shapeInfo.BaseRect.Bottom),
                    shapeInfo.BaseRect.Right, (f) => f.BaseRect.Left);
                down = FindNearest(shapeInfoList, Rectangle.FromLTRB(shapeInfo.BaseRect.Left, shapeInfo.BaseRect.Bottom, shapeInfo.BaseRect.Right, yMax),
                    shapeInfo.BaseRect.Bottom, (f) => f.BaseRect.Top);

                shapeInfo.Neighborhood = new ShapeInfo[4] { left, up, right, down };

                // Update Teahcing Info
                int wBlot = 0;
                int lBlot = 0;
                int wMargine = 0;
                int lMargine = 0;
                if (left != null && right != null && up != null && down != null)
                {
                    wBlot = shapeInfo.BaseRect.Width;
                    lBlot = shapeInfo.BaseRect.Height;

                    int wMargine1 = Math.Abs(right.BaseRect.Left - shapeInfo.BaseRect.Right);
                    int wMargine2 = Math.Abs(shapeInfo.BaseRect.Left - left.BaseRect.Right);
                    wMargine = Math.Min(wMargine1, wMargine2);

                    int lMargine1 = Math.Abs(up.BaseRect.Bottom - shapeInfo.BaseRect.Top);
                    int lMargine2 = Math.Abs(shapeInfo.BaseRect.Bottom - down.BaseRect.Top);
                    lMargine = Math.Min(lMargine1, lMargine2);
                }
                TeachInfo teachInfo = new TeachInfo(shapeInfo.Id, shapeInfo.Area, new Size(wMargine, lMargine), new Size(wBlot, lBlot));
                lock (patternInfoList)
                    patternInfoList.Add(new PatternInfo(shapeInfo, teachInfo));
            }
            );

            return patternInfoList;
        }

        private ShapeInfo FindNearest(List<ShapeInfo> shapeInfoList, Rectangle searchRect, int baseValue, Func<ShapeInfo, int> func)
        {
            List<ShapeInfo> itemList = shapeInfoList.FindAll(f => searchRect.IntersectsWith(f.BaseRect));

            if (itemList.Count == 0)
                return null;

            Dictionary<ShapeInfo, int> distList = itemList.ToDictionary(f => f, f => Math.Abs(baseValue - func(f)));
            distList = distList.OrderBy(f => f.Value).ToDictionary(f => f.Key, f => f.Value);
            List<int> diff = new List<int>(); ;
            
            distList.Aggregate((f, g) =>
            {
                diff.Add(g.Value - f.Value);
                return g;
            });
            diff.Sort();

            if (diff.Count == 0)
                return distList.ElementAt(0).Key;

            double avgDist = diff.Average();
            double midDist = (diff.Count % 2 == 0 ? diff.Skip(diff.Count / 2).Take(2).Average() : diff[diff.Count / 2]);
            KeyValuePair<ShapeInfo, int> hubo = distList.First();
            //if (hubo.Value > ((avgDist + midDist) / 2) * 1.5)
            //    return null;

            return hubo.Key;
        }

        private PatternInfoGroupList Grouping(List<PatternInfo> patternInfoList, DebugContext debugContext)
        {
            PatternInfoGroupList newGroup = new PatternInfoGroupList();
            List<List<PatternInfo>> tempGroup = FilterByArea(patternInfoList, 1, debugContext);

            //tempGroup.ForEach(f => newGroup.Add(new PatternInfoGroup(-1, f)));
            //newGroup.UpdateId();
            //return newGroup;

            foreach (List<PatternInfo> temp in tempGroup)
            {
                List<List<PatternInfo>> tempGroup2 = FilterByLength(temp, 1, debugContext);
                tempGroup2.ForEach(f => newGroup.Add(new PatternInfoGroup(-1, f)));
            }

            return newGroup;
        }

        private List<List<PatternInfo>> FilterByArea(List<PatternInfo> blobRects, int minCount, DebugContext debugContext)
        {
            Func<PatternInfo, double> func = new Func<PatternInfo, double>(f => f.ShapeInfo.Area);
            return FilterBy(blobRects, minCount, func, debugContext);
        }

        private List<List<PatternInfo>> FilterByLength(List<PatternInfo> blobRects, int minCount, DebugContext debugContext)
        {
            Func<PatternInfo, double> func = new Func<PatternInfo, double>(f => f.ShapeInfo.Waist);
            return FilterBy(blobRects, minCount, func, debugContext);
        }

        private List<List<PatternInfo>> FilterBy(List<PatternInfo> blobRects, int minCount, Func<PatternInfo, double> featureFunc, DebugContext debugContext)
        {
            blobRects.Sort((f, g) => featureFunc(f).CompareTo(featureFunc(g)));

            if (debugContext.SaveDebugImage)
            {
                StreamWriter sw = File.AppendText(Path.Combine(debugContext.FullPath, "FilterBy.txt"));
                sw.WriteLine("=== Start Filter By ===");
                blobRects.ForEach(f => sw.WriteLine(featureFunc(f).ToString()));
                sw.WriteLine("=== End Filter By ===");
                sw.Close();
            }

            List<List<PatternInfo>> newGroup = new List<List<PatternInfo>>();
            List<PatternInfo> tempList = new List<PatternInfo>();
            foreach (PatternInfo patternInfo in blobRects)
            //blobRects.ForEach(f =>
            {
                if (tempList.Count > 0)
                {
                    double feature = featureFunc(patternInfo);

                    double average = tempList.Average(featureFunc);
                    double lower = average * (1 - rate);
                    double upper = average * (1 + rate);

                    if (!(lower < feature && feature < upper))
                    {
                        newGroup.Add(tempList);
                        tempList = new List<PatternInfo>();
                    }
                }
                tempList.Add(patternInfo);
            };
            if (tempList.Count > 0)
                newGroup.Add(tempList);

            newGroup.RemoveAll(f => f.Count <= minCount);

            return newGroup;
        }

        public override List<MatchResult> Match(AlgoImage algoImage, TeachData teachData, float matchRatio)
        {
            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, "FeatureExtractorV1_Match"));
            if (debugContext.SaveDebugImage)
            {
                string[] files = Directory.GetFiles(debugContext.FullPath);
                foreach (string file in files)
                    File.Delete(file);
            }

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            AlgoImage binalImage = null;
            try
            {
                algoImage.Save("algoImage.bmp", debugContext);
                binalImage = algoImage.Clone();
                int globalBinValue = teachData.BinValue;
                imageProcessing.Binarize(binalImage, globalBinValue, true);
                binalImage.Save("0_Binal_Image.bmp", debugContext);

                // 영상에서 패턴의 모양 정보를 얻음.
                List<ShapeInfo> shapeInfoList = GetShapeInfo(binalImage, debugContext);
                shapeInfoList.RemoveAll(f => f.Area < 5);
                if (debugContext.SaveDebugImage)
                {
                    AlgoImage tempImage = algoImage.Clone();

                    foreach (ShapeInfo sInfo in shapeInfoList)
                        imageProcessing.DrawRect(tempImage, sInfo.BaseRect, 255, false);

                    tempImage.Save("1_Found_Shape.bmp", debugContext);
                    tempImage.Dispose();
                }

                // 패턴 모양 정보로부터 패턴 그룹, Margin 정보 얻음.
                List<PatternInfo> patternInfoList = GetPatternInfo(shapeInfoList, debugContext);

                // 매! 칭!
                List<MatchResult> matchResultList = new List<MatchResult>();
                foreach (PatternInfo inspPatternInfo in patternInfoList)
                {
                    PatternInfo matchedPatternInfo = Match(inspPatternInfo, teachData, matchRatio, debugContext);
                    MatchResult matchResult;
                    if (matchedPatternInfo != null)
                    // 티칭된 패턴과 동일한 패턴임.
                    {
                        matchResult = new MatchResult(inspPatternInfo, matchedPatternInfo);
                    }
                    else
                    // 티칭된 패턴과 다른 패턴임 -> 이물 후보
                    {
                        matchResult = new MatchResult(inspPatternInfo, null);
                    }
                    matchResultList.Add(matchResult);
                }

                if (debugContext.SaveDebugImage)
                {
                    AlgoImage tempImage = algoImage.Clone();
                    foreach (MatchResult matchResult in matchResultList)
                        imageProcessing.DrawRect(tempImage, matchResult.InspPatternInfo.ShapeInfo.BaseRect, 255, false);
                    tempImage.Save("2_Match_Done.bmp", debugContext);
                    tempImage.Dispose();
                }

                return matchResultList;

                //matchResult.Sort((f, g) => f.TeachInfo.Id.CompareTo(g.TeachInfo.Id));
            }
            finally
            {
                binalImage?.Dispose();
            }
        }

        private PatternInfo Match(PatternInfo inspPatternInfo, TeachData teachData, float matchRatio, DebugContext debugContext)
        {
            ShapeInfo inspShapeInfo = inspPatternInfo.ShapeInfo;

            foreach (PatternInfoGroup group in teachData.PatternInfoGroupList)
            {
                //if (group.TeachInfo.Inspectable == false)
                //    continue;

                foreach (ShapeInfo shapeInfo2 in group.ShapeInfoList)
                {
                    if (ShapeInfo.IsSimilar(inspShapeInfo, shapeInfo2, matchRatio))
                    {
                        PatternInfo patternInfo = new PatternInfo(shapeInfo2, group.TeachInfo);
                        return patternInfo;
                    }
                }
            }
            return null;
        }
    }


    //public class FeatureExtractorV2 : FeatureExtractor
    //{
    //    public override TeachResult Extract(AlgoImage algoImage, Rectangle cropRect)
    //    {
    //        DebugContext debugContext = new DebugContext(true, Path.Combine(PathSettings.Instance().Temp, "FeatureExtractorV2"));
    //        ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

    //        if (debugContext.SaveDebugImage)
    //        {
    //            string[] files = Directory.GetFiles(debugContext.Path);
    //            foreach (string file in files)
    //                File.Delete(file);
    //        }

    //        AlgoImage binalImage = null;
    //        AlgoImage patternImage = null;
    //        BlobRectList blobRectList = null;
    //        try
    //        {
    //            algoImage.Save("0.algoImage.bmp", debugContext);
    //            binalImage = algoImage.Clone();
    //            int globalBinValue = (int)imageProcessing.Binarize(binalImage, true);
    //            binalImage.Save(string.Format("1.binalImage_B{0}.bmp", globalBinValue), debugContext);

    //            if (globalBinValue > 200 || globalBinValue < 15)
    //            {
    //                return null;
    //            }

    //            // Blob - For Pattern position
    //            //BlobParam blobParam = new BlobParam();
    //            //blobParam.SelectArea = true;
    //            //blobParam.SelectCenterPt = true;
    //            //blobRectList = imageProcessing.Blob(blobImage, blobParam);

    //            // Blob - For Pattern area intensity value
    //            patternImage = binalImage.Clone();
    //            patternImage.Save(string.Format("2.patternImage.bmp"), debugContext);
    //            float pattenGrayValue = imageProcessing.GetGreyAverage(algoImage, patternImage);
    //            patternImage.Save(string.Format("2.patternImage_B{0}.bmp", (int)pattenGrayValue), debugContext);

    //            // Blob - for non-pattern area intensity value
    //            imageProcessing.Not(patternImage);
    //            patternImage.Save("3.nonpatternImage.bmp", debugContext);
    //            float nonpattenGrayValue = imageProcessing.GetGreyAverage(algoImage, patternImage);
    //            patternImage.Save(string.Format("3.nonpatternImage_B{0}.bmp", (int)nonpattenGrayValue), debugContext);

    //            float margine = 0.25f;
    //            int edgeThMin = (int)Math.Round((nonpattenGrayValue - pattenGrayValue) * margine + pattenGrayValue);
    //            int edgeThMax = (int)Math.Round(nonpattenGrayValue - (nonpattenGrayValue - pattenGrayValue) * margine);
    //            patternImage.Copy(algoImage);
    //            imageProcessing.Binarize(patternImage, edgeThMin, edgeThMax, true);
    //            patternImage.Save(string.Format("4.EdgeImage_B{0}-(1).bmp", (int)edgeThMin, (int)edgeThMax), debugContext);

    //            AlgoImage temp1 = algoImage.Clone();
    //            AlgoImage temp2 = algoImage.Clone();
    //            AlgoImage temp3 = ImageBuilder.Build(algoImage);

    //            imageProcessing.Binarize(temp1, edgeThMin, true);
    //            imageProcessing.Clipping(temp1, 0, 128);
    //            imageProcessing.Binarize(temp2, edgeThMax, true);
    //            imageProcessing.Clipping(temp2, 0, 128);
    //            imageProcessing.Add(temp1, temp2, temp3);
    //            temp1.Save(string.Format("5.Processed_T1.bmp", (int)edgeThMin, (int)edgeThMax), debugContext);
    //            temp2.Save(string.Format("5.Processed_T2.bmp", (int)edgeThMin, (int)edgeThMax), debugContext);
    //            temp3.Save(string.Format("5.Processed.bmp", (int)edgeThMin, (int)edgeThMax), debugContext);

    //            TeachResult positionCheckerResult = new TeachResult(globalBinValue, algoImage.ToImageD());
    //            return positionCheckerResult;
    //        }
    //        finally
    //        {
    //            binalImage?.Dispose();
    //            patternImage?.Dispose();
    //            blobRectList?.Dispose();
    //        }
    //    }

    //    public override Dictionary<PatternInfoGroup, PatternInfoGroup> Find(AlgoImage algoImage, TeachResult teachedData)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
