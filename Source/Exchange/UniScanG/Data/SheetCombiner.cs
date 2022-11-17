using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Settings;

namespace UniScanG.Data
{
    public static class SheetCombiner
    {
        public static ResultCollector resultCollector = null;

        public static string TypeName { get { return "SheetCombiner"; } }

        public static void SetCollector(ResultCollector collector)
        {
            resultCollector = collector;
        }

        public static Bitmap CreatePrevImage(Bitmap bitmap)
        {
            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;

            return ImageHelper.Resize(bitmap, resizeRatio, resizeRatio);
        }

        public static SheetResult CombineResult(Tuple<string, string> foundedT)
        {
            if (SystemManager.Instance().ExchangeOperator is IServerExchangeOperator == false)
                return null;

            int sheetNo = int.Parse(foundedT.Item1);
            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            Size mergeSize = Size.Round(new SizeF(SystemTypeSettings.Instance().MonitorFov.Width * resizeRatio, SystemTypeSettings.Instance().MonitorFov.Height * resizeRatio));
            Production curProduction = SystemManager.Instance().ProductionManager.CurProduction as Production;

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList(sheetNo);

            TimeSpan maxInspectTimeSpan = TimeSpan.Zero;

            Image2D image = new Image2D(mergeSize.Width, mergeSize.Height, 1);
            SheetResult[] sheetResults = new SheetResult[inspectorList.Count];
            Parallel.For(0, inspectorList.Count, i =>
            {
                UniScanG.Data.Model.ModelDescription md = SystemManager.Instance().CurrentModel.ModelDescription;
                //string resultPath = Path.Combine(inspectorList[i].Info.Path, "Result", foundedT.Item2, md.Name, md.Thickness.ToString(), md.Paste, SystemManager.Instance().ProductionManager.CurProduction.LotNo, foundedT.Item1);
                string resultPath = Path.Combine(curProduction.GetResultPath(inspectorList[i].Info.Path), foundedT.Item1);
                sheetResults[i] = resultCollector.Collect(resultPath);

                SheetResult sheetResult = sheetResults[i];
                sheetResult.UpdateSubResultImage();
                
                int offset = 0;
                for (int offsetIndex = 0; offsetIndex < i; offsetIndex++)
                    offset += (int)inspectorList[offsetIndex].Info.Fov.Width;
                sheetResult.Offset(offset - (int)inspectorList[i].Info.Fov.X, 0 /*-(int)inspectorList[i].Info.Fov.Y*/);

                if (sheetResult.PrevImage != null)
                {
                    Rectangle srcRect = Rectangle.Empty;
                    if (inspectorList[i].Info.Fov.IsEmpty)
                    {
                        srcRect = new Rectangle(0, 0, sheetResult.PrevImage.Width, sheetResult.PrevImage.Height);
                    }
                    else
                    {
                        int resizeFovX = (int)(inspectorList[i].Info.Fov.X * resizeRatio);
                        int resizeFovY = (int)(inspectorList[i].Info.Fov.Y * resizeRatio);
                        int resizeFovWidth = (int)(inspectorList[i].Info.Fov.Width * resizeRatio);
                        int resizeFovHeight = (int)(inspectorList[i].Info.Fov.Height * resizeRatio);
                        srcRect = new Rectangle(resizeFovX, resizeFovY, resizeFovWidth, resizeFovHeight);
                    }

                    Size fidOffset = Size.Round(sheetResult.OffsetFound);
                    //srcRect.Offset(fidOffset.Width, fidOffset.Height);
                    srcRect.Intersect(new Rectangle(Point.Empty, sheetResult.PrevImage.Size));

                    if (srcRect.Width > 0 && srcRect.Height > 0)
                    {
                        Image2D orgImage2D = Image2D.ToImage2D(sheetResult.PrevImage);
                        lock (image)
                        {
                            Rectangle dstRect = new Rectangle(new Point((int)(offset * resizeRatio), 0), srcRect.Size);
                            dstRect.Intersect(new Rectangle(Point.Empty, mergeSize));
                            srcRect.Size = dstRect.Size;
                            image.CopyFrom(orgImage2D, srcRect, orgImage2D.Pitch, dstRect.Location);
                        }
                    }
                }
                else
                {
                    //LogHelper.Error(LoggerType.Error, string.Format("SheetCombiner::CombineResult - Inspector {0} prevImage is null", inspectorList[i].Info.GetName()));
                }
            });

            SheetResult mergeSheetResult = SheetResult.Union(sheetResults);
            mergeSheetResult.Good = (mergeSheetResult.SheetSubResultList.Count == 0);

            if (image == null || image.Size.Width == 0 || image.Size.Height == 0)
                mergeSheetResult.PrevImage = null;
            else
                mergeSheetResult.PrevImage = image.ToBitmap();

            return mergeSheetResult;
        }

        public static Bitmap CreateModelImage(UniScan.Common.Data.ModelDescription modelDescription)
        {
            if (SystemManager.Instance().ExchangeOperator is IServerExchangeOperator == false)
                return null;

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList(-1).FindAll(f=>f.Info.ClientIndex<=0);

            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            RectangleF monitorFov = SystemTypeSettings.Instance().MonitorFov;
            
            int mergeWidth = (int)(monitorFov.Width * resizeRatio);
            int mergeHeight = (int)(monitorFov.Height * resizeRatio);
            if (mergeWidth == 0 || mergeHeight == 0)
                return null;
            Image2D image = new Image2D(mergeWidth, mergeHeight, 1);

            Parallel.For(0, inspectorList.Count, i =>
            {
                if (inspectorList[i].IsTrained(modelDescription) == false)
                    return;

                Bitmap prevImage = inspectorList[i].GetPreviewImage(modelDescription);
                if (prevImage == null)
                    return;

                int offset = 0;
                for (int offsetIndex = 0; offsetIndex < i; offsetIndex++)
                    offset += (int)inspectorList[offsetIndex].Info.Fov.Width;

                int resizeFovX = (int)(inspectorList[i].Info.Fov.X * resizeRatio);
                int resizeFovY = (int)(inspectorList[i].Info.Fov.Y * resizeRatio);
                int resizeFovWidth = (int)(inspectorList[i].Info.Fov.Width * resizeRatio);
                int resizeFovHeight = (int)(inspectorList[i].Info.Fov.Height * resizeRatio);

                if (resizeFovWidth == 0 || resizeFovHeight == 0)
                    return;

                Rectangle srcRect = new Rectangle(resizeFovX, resizeFovY, resizeFovWidth, resizeFovHeight);
                srcRect.Intersect(new Rectangle(Point.Empty, prevImage.Size));
                if (srcRect.Width > 0 && srcRect.Height > 0)
                {
                    Image2D prevImage2D = Image2D.ToImage2D(prevImage);
                    lock (image)
                        image.CopyFrom(prevImage2D, srcRect, prevImage2D.Pitch, new Point((int)(offset * resizeRatio), 0));
                    prevImage2D.Dispose();
                }
            });

            return image.ToBitmap();
        }
    }
}
