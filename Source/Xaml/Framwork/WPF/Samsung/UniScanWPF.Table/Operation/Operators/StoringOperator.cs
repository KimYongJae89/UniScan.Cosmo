using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Helper;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Settings;

namespace UniScanWPF.Table.Operation.Operators
{
    public class StoringOperator : Operator
    {
        const string reportTamplateFile = "RawDataTemplate_Offline.xlsx";

        public StoringOperator()
        {
        }

        public static List<LoadItem> Load(Production production)
        {
            string resultPath = production.GetResultPath();

            List<LoadItem> list = new List<LoadItem>();

            for (int i = 0; i < production.Count; i++)
            {
                // resultPath 는 1-base
                string subResultPath = Path.Combine(resultPath, (i + 1).ToString());
                list.Add(new LoadItem(subResultPath));
            }

            return list;
        }

        public void Save(List<ExtractOperatorResult> extractOperatorResultList, List<CanvasDefect> defectList)
        {
            if (extractOperatorResultList == null || extractOperatorResultList.Count == 0 ||
                defectList == null)
                return;

            OperatorState = OperatorState.Run;

            Production production = SystemManager.Instance().ProductionManager.CurProduction;
            ResultKey resultKey = extractOperatorResultList.First().ResultKey;

            Task task = null;
            task = Task.Factory.StartNew(() =>
            {
                production.AddCount(defectList);
                SystemManager.Instance().ProductionManager.Save();

                // resultPath 는 1-base
                int repeatCount = resultKey.Production.Count;
                string resultPath = production.GetResultPath();
                string reportFolder = Path.Combine(PathSettings.Instance().Result.Replace("Result", "Report"), resultKey.DateTime.ToString("yyyy-MM-dd"));
                if (!Directory.Exists(reportFolder))
                    Directory.CreateDirectory(reportFolder);

                string reportPath = Path.Combine(reportFolder, string.Format("{0}_{1}.xlsx", resultKey.Model.Name, resultKey.LotNo));

                SaveReport(reportPath, repeatCount, resultKey, defectList);
                SaveReult(Path.Combine(resultPath, repeatCount.ToString()), extractOperatorResultList, defectList);
                OperatorState = OperatorState.Idle;

            });
        }

        public void SaveReport(string targetPath, int repeatCount, ResultKey resultKey, List<CanvasDefect> canvasDefectList)
        {
            string templateFilePaht = Path.Combine(PathSettings.Instance().Result, reportTamplateFile);
            //#if DEBUG
            //            string debugTemplateFilePath = Path.Combine(@"D:\Project_UniScan\UniScan\Runtime\Result", reportTamplateFile);
            //            if (File.Exists(debugTemplateFilePath))
            //                templateFilePaht = debugTemplateFilePath;
            //#endif
            if (File.Exists(templateFilePaht) == false)
                return;

            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Workbooks workbooks = null;
            Workbook workbook = null;
            Sheets sheets = null;
            Worksheet worksheet = null;
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                System.Diagnostics.Debug.WriteLine(string.Format("ExcelApp.Version is {0}", excelApp.Version));

                workbooks = excelApp.Workbooks;
                workbook = workbooks.Open(templateFilePaht);
                sheets = workbook.Worksheets;
                worksheet = sheets.get_Item("Sheet1");

                // 제목
                worksheet.Cells[4, 2].Value = resultKey.DateTime.ToString("yyyy.MM.dd HH:mm:ss");
                worksheet.Cells[4, 5].Value = resultKey.Production.Name;
                worksheet.Cells[4, 8].Value = resultKey.Production.LotNo;

                // 티칭값
                InspectOperatorSettings inspectOperatorSettings = SystemManager.Instance().OperatorManager.InspectOperator.Settings;
                worksheet.Cells[7, 2].Value = inspectOperatorSettings.PatternLower;
                worksheet.Cells[7, 3].Value = inspectOperatorSettings.PatternUpper;
                worksheet.Cells[7, 4].Value = inspectOperatorSettings.PatternMinDefectSize;
                worksheet.Cells[8, 2].Value = inspectOperatorSettings.MarginLower;
                worksheet.Cells[8, 3].Value = inspectOperatorSettings.MarginUpper;
                worksheet.Cells[8, 4].Value = inspectOperatorSettings.MarginMinDefectSize;
                worksheet.Cells[9, 4].Value = inspectOperatorSettings.DiffThreshold;

                //누적개수
                worksheet.Cells[7, 7].Value = resultKey.Production.PatternCount;
                worksheet.Cells[7, 8].Value = resultKey.Production.MarginCount;
                worksheet.Cells[7, 9].Value = resultKey.Production.ShapeCount;
                worksheet.Cells[7, 10].Value = resultKey.Production.PatternCount + resultKey.Production.MarginCount + resultKey.Production.ShapeCount;

                // 불량개수
                int[] defCount = new int[]
                {
                    canvasDefectList.Count(f => f.Defect.ResultObjectType.Equals(DefectType.Pattern)),
                    canvasDefectList.Count(f => f.Defect.ResultObjectType.Equals(DefectType.Margin)),
                    canvasDefectList.Count(f => f.Defect.ResultObjectType.Equals(DefectType.Shape))
                };

                worksheet.Cells[8, 7].Value = defCount[0];
                worksheet.Cells[8, 8].Value = defCount[1];
                worksheet.Cells[8, 9].Value = defCount[2];
                worksheet.Cells[8, 10].Value = defCount.Sum();

                // 시트 길이
                List<LengthMeasure> validLengthMeasureList = canvasDefectList.FindAll(f => f.Defect.ResultObjectType.Equals(MeasureType.Length)).ConvertAll(f => f.Defect as LengthMeasure).FindAll(f => f.IsValid);
                List<MeanderMeasure> meanderMeasure = canvasDefectList.FindAll(f => f.Defect.ResultObjectType.Equals(MeasureType.Meander)).ConvertAll(f => f.Defect as MeanderMeasure);
                List<LengthMeasure> verticalLength = validLengthMeasureList.FindAll(f => f.Direction == DynMvp.Vision.Direction.Vertical);
                List<LengthMeasure> horizontalLength = validLengthMeasureList.FindAll(f => f.Direction == DynMvp.Vision.Direction.Horizontal);
                for (int i = 0; i < 3; i++)
                {
                    if (horizontalLength.Count > i)
                        worksheet.Cells[12, 2 + i].Value = horizontalLength[i].LengthMm;

                    if (verticalLength.Count > i)
                        worksheet.Cells[13, 2 + i].Value = verticalLength[i].LengthMm;

                    if (meanderMeasure.Count > i)
                    {
                        worksheet.Cells[14, 2 + i].Value = meanderMeasure[i].SheetPrintDistMm;
                        worksheet.Cells[15, 2 + i].Value = meanderMeasure[i].CoatPrintDistMm;
                    }
                }

                // 마진길이
                List<ExtraMeasure> extraMeasureList = canvasDefectList.FindAll(f => f.Defect.ResultObjectType.Equals(MeasureType.Extra)).ConvertAll(f => f.Defect as ExtraMeasure);
                extraMeasureList.FindAll(f => f.ReferencePos != ReferencePos.None).ForEach(f =>
                {
                    worksheet.Cells[12, 7 + (int)f.ReferencePos].Value = f.Width;
                    worksheet.Cells[13, 7 + (int)f.ReferencePos].Value = f.Height;
                });

                // 불량 목록
                int row = 0;
                int rowStart = 20;
                for (int i = 0; i < canvasDefectList.Count; i++)
                {
                    Defect defect = canvasDefectList[i].Defect as Defect;
                    if (defect != null)
                    {
                        worksheet.Cells[rowStart + row, 1].Value = i + 1;
                        worksheet.Cells[rowStart + row, 2].Value = defect.Length;
                        worksheet.Cells[rowStart + row, 3].Value = defect.DiffValue;
                        worksheet.Cells[rowStart + row, 4].Value = defect.DefectType.ToString();
                        row++;
                    }
                }

                // 저장
                worksheet.Name = repeatCount.ToString();

                if (File.Exists(targetPath))
                {
                    Workbook workbook2 = excelApp.Workbooks.Open(targetPath);
                    Sheets sheets2 = workbook2.Sheets;
                    Worksheet worksheet2 = (Worksheet)sheets2[1];

                    worksheet.Copy(worksheet2);
                    workbook2.Save();
                    workbook2.Close();

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet2);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets2);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook2);
                }
                else
                {
                    workbook.SaveAs(targetPath);
                }

                workbook.Close(false);
            }
            finally
            {
                excelApp?.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sheets);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbooks);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
        }

        /// <summary>
        /// 엑셀 object에 빈값(null) 채움.
        /// </summary>
        /// <param name="obj"></param>
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
        }

        private void SaveReult(string path, List<ExtractOperatorResult> extractOperatorResultList, List<CanvasDefect> defectList)
        {
            SystemManager.Instance().OperatorManager.ScanOperator.Settings.Save(Path.Combine(path, "ScanSetting.xml"));
            SystemManager.Instance().OperatorManager.ExtractOperator.Settings.Save(Path.Combine(path, "ExtractSetting.xml"));
            SystemManager.Instance().OperatorManager.InspectOperator.Settings.Save(Path.Combine(path, "InspectSetting.xml"));

            string defectPath = Path.Combine(path, "Defect");
            if (Directory.Exists(defectPath) == false)
                Directory.CreateDirectory(defectPath);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement resultElement = xmlDocument.CreateElement("", "Result", "");
            xmlDocument.AppendChild(resultElement);

            for (int i = 0; i < defectList.Count; i++)
            {
                XmlElement defectElement = xmlDocument.CreateElement("", string.Format("Defect{0}", i), "");
                resultElement.AppendChild(defectElement);

                defectList[i].Defect.Save(defectElement);

                int length = defectList[i].RotateRectPointList.Length;
                for (int j = 0; j < length; j++)
                {
                    XmlHelper.SetValue(defectElement, string.Format("X{0}", j), defectList[i].RotateRectPointList[j].X.ToString());
                    XmlHelper.SetValue(defectElement, string.Format("Y{0}", j), defectList[i].RotateRectPointList[j].Y.ToString());
                }

                try
                {
                    if (defectList[i].Defect is Defect)
                    {
                        WPFImageHelper.SaveBitmapSource(Path.Combine(defectPath, string.Format("{0}.png", i + 1)), (defectList[i].Defect as Defect)?.Image);
                    }
                }
                catch { }
            }

            XmlDocument scanDocument = new XmlDocument();
            XmlElement scanResultElement = scanDocument.CreateElement("", "Result", "");
            scanDocument.AppendChild(scanResultElement);
            foreach (var extractResult in extractOperatorResultList)
            {
                var scanResult = extractResult?.ScanOperatorResult;
                if (extractResult == null || scanResult == null)
                    continue;

                string bitmapPath = Path.Combine(path, string.Format("{0}.png", scanResult.FlowPosition));
                WPFImageHelper.SaveBitmapSource(bitmapPath, scanResult.TopLightBitmap);

                //WPFImageHelper.SaveBitmapSource(
                // Path.Combine(resultPath, string.Format("{0}Mask.png",
                // scanResult.FlowPosition)),
                // extractResult.MaskBufferBitmap);

                //WPFImageHelper.SaveBitmapSource(
                //Path.Combine(resultPath, string.Format("{0}Sheet.png",
                //scanResult.FlowPosition)),
                //extractResult.SheetBufferBitmap);

                XmlHelper.SetValue(scanResultElement, string.Format("X{0}", scanResult.FlowPosition), scanResult.CanvasAxisPosition.Position[0].ToString());
                XmlHelper.SetValue(scanResultElement, string.Format("Y{0}", scanResult.FlowPosition), scanResult.CanvasAxisPosition.Position[1].ToString());

                XmlHelper.SetValue(scanResultElement, string.Format("C{0}", scanResult.FlowPosition), extractResult.PatternCount.ToString());
                XmlHelper.SetValue(scanResultElement, string.Format("H{0}", scanResult.FlowPosition), extractResult.SheetRect);
            }

            string scanXmlPath = Path.Combine(path, "Scan.xml");
            XmlHelper.Save(scanDocument, scanXmlPath);

            string xmlPath = Path.Combine(defectPath, "Defect.xml");
            XmlHelper.Save(xmlDocument, xmlPath);

        }

        public ImageSource DrawingDefect(ImageSource bitmapSource, InspectOperatorResult inspectOperatorResult)
        {
            var visual = new DrawingVisual();

            using (DrawingContext drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawImage(bitmapSource, new System.Windows.Rect(0, 0, bitmapSource.Width, bitmapSource.Height));

                for (int i = 0; i < inspectOperatorResult.DefectList.Count; i++)
                {
                    //BlobRect blobRect = inspectOperatorResult.DefectList[i].DefectBlob;
                    IResultObject resObj = inspectOperatorResult.DefectList[i];
                    SolidColorBrush brush = resObj.GetBrush();
                    //switch (inspectOperatorResult.DefectList[i].DefectType)
                    //{
                    //    case DefectType.Pattern:
                    //        brush = Brushes.Red;
                    //        break;
                    //    case DefectType.Margin:
                    //        brush = Brushes.Blue;
                    //        break;
                    //    case DefectType.Shape:
                    //        brush = Brushes.Gold;
                    //        break;
                    //}

                    //Point position = new Point(inspectOperatorResult.DefectList[i].DefectBlob.BoundingRect.X / 10.0, inspectOperatorResult.DefectList[i].DefectBlob.BoundingRect.Y / 10.0 - 48);
                    System.Windows.Point position = resObj.GetRect(10).Location;
                    drawingContext.DrawText(
                       new FormattedText(string.Format("{0}", i), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("malgun gothic"), 32, brush), position);

                    StreamGeometry streamGeometry = new StreamGeometry();
                    using (StreamGeometryContext geometryContext = streamGeometry.Open())
                    {
                        //geometryContext.BeginFigure(new Point(blobRect.RotateXArray[0] / 10.0, blobRect.RotateYArray[0] / 10.0), true, true);

                        //PointCollection points = new PointCollection { 
                        //   new Point(blobRect.RotateXArray[1] / 10.0, blobRect.RotateYArray[1] / 10.0),
                        //new Point(blobRect.RotateXArray[2] / 10.0, blobRect.RotateYArray[2] / 10.0),
                        //new Point(blobRect.RotateXArray[3] / 10.0, blobRect.RotateYArray[3] / 10.0)};

                        //geometryContext.PolyLineTo(points, true, true);

                        System.Windows.Point[] points = resObj.GetPoints(10);
                        geometryContext.BeginFigure(points[0], true, true);
                        geometryContext.PolyLineTo(points, true, true);
                    }

                    brush = new SolidColorBrush(brush.Color);
                    brush.Opacity = 0.25;
                    drawingContext.DrawGeometry(brush, new Pen(brush, 1), streamGeometry);
                }
            }

            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)bitmapSource.Width, (int)bitmapSource.Height, 96, 96, PixelFormats.Pbgra32);
            renderTargetBitmap.Render(visual);
            return renderTargetBitmap;
        }


    }

    public class StoringOperatorSettings : OperatorSettings
    {
        protected override void Initialize()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Storing");
        }

        public override void Load(XmlElement xmlElement)
        {

        }

        public override void Save(XmlElement xmlElement)
        {

        }
    }

    public class LoadItem
    {
        public string RootPath { get => this.rootPath; }
        string rootPath;

        public List<ExtractOperatorResult> ExtractOperatorResultList { get => this.extractOperatorResultList; }
        List<ExtractOperatorResult> extractOperatorResultList;

        public List<CanvasDefect> CanvasDefectList { get => this.canvasDefectList; }
        List<CanvasDefect> canvasDefectList;

        public List<OperatorSettings> OperatorSettingList { get => this.operatorSettingList; }
        List<OperatorSettings> operatorSettingList;

        public bool IsLoaded { get => extractOperatorResultList != null && canvasDefectList != null && operatorSettingList != null; }

        public LoadItem(string rootPath)
        {
            this.rootPath = rootPath;
        }

        public bool Load(CancellationToken? cancellationToken)
        {
            if (string.IsNullOrEmpty(this.rootPath))
                return false;

            try
            {
                cancellationToken?.ThrowIfCancellationRequested();

                // 티칭 정보 로드
                this.operatorSettingList = LoadTeachData(this.rootPath);

                // 불량 정보 로드
                string defectPath = Path.Combine(this.rootPath, "Defect");
                string xmlPath = Path.Combine(defectPath, "Defect.xml");
                XmlDocument xmlDocument = XmlHelper.Load(xmlPath);
                XmlElement resultElement = xmlDocument?["Result"];
                if (resultElement == null)
                    throw new Exception("Result Element is not exist");

                this.canvasDefectList = new List<CanvasDefect>();
                for (int i = 0; i < resultElement.ChildNodes.Count; i++)
                {
                    cancellationToken?.ThrowIfCancellationRequested();

                    XmlElement xmlElement = (XmlElement)resultElement.ChildNodes[i];
                    if (xmlElement.Name.Contains("Defect") == false)
                        continue;

                    BitmapSource image = WPFImageHelper.LoadBitmapSource(Path.Combine(defectPath, string.Format("{0}.png", i + 1)));
                    IResultObject defect = Defect.CreateDefect(image, xmlElement);
                    if (defect != null)
                    {
                        CanvasDefect canvasDefect = new CanvasDefect(defect, null);
                        List<System.Windows.Point> pointList = new List<System.Windows.Point>();
                        int j = 0;
                        while (true)
                        {
                            XmlElement childNodeX = xmlElement[string.Format("X{0}", j)];
                            XmlElement childNodeY = xmlElement[string.Format("Y{0}", j)];
                            if (childNodeX != null && childNodeY != null)
                            {
                                pointList.Add(new System.Windows.Point(
                                  XmlHelper.GetValue(childNodeX, "", 0.0),
                                  XmlHelper.GetValue(childNodeY, "", 0.0)));
                                j++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        canvasDefect.RotateRectPointList = pointList.ToArray();

                        this.canvasDefectList.Add(canvasDefect);
                    }
                }


                // 스캔 정보 로드
                string scanXmlPath = Path.Combine(this.rootPath, "Scan.xml");
                bool mul = (new FileInfo(scanXmlPath).CreationTime < new DateTime(2019, 3, 12, 15, 0, 0));
                XmlDocument scanDocument = XmlHelper.Load(scanXmlPath);
                XmlElement scanResultElement = scanDocument?["Result"];

                if (scanResultElement == null)
                    throw new Exception("Scan Element is not exist");

                this.extractOperatorResultList = new List<ExtractOperatorResult>();
                for (int j = 0; j < DeveloperSettings.Instance.ScanNum; j++)
                {
                    cancellationToken?.ThrowIfCancellationRequested();

                    BitmapSource source = WPFImageHelper.LoadBitmapSource(Path.Combine(this.rootPath, string.Format("{0}.png", j)));

                    AxisPosition axisPosition = new AxisPosition(2);
                    axisPosition.Position[0] = XmlHelper.GetValue(scanResultElement, string.Format("X{0}", j), 0.0f);
                    axisPosition.Position[1] = XmlHelper.GetValue(scanResultElement, string.Format("Y{0}", j), 0.0f);

                    if (mul)
                    {
                        axisPosition.Position[0] *= 0.2f;
                        axisPosition.Position[1] *= 0.2f;
                    }

                    System.Drawing.Rectangle sheetRect = System.Drawing.Rectangle.Empty;
                    sheetRect = XmlHelper.GetValue(scanResultElement, string.Format("S{0}", j), sheetRect);

                    ScanOperatorResult scanResult = new ScanOperatorResult(null, null, source, j, null, null, null, axisPosition);
                    ExtractOperatorResult extractResult = new ExtractOperatorResult(null, scanResult, null, null, null, null, sheetRect, null, null);
                    this.extractOperatorResultList.Add(extractResult);
                }


                return true;
            }
            catch (Exception ex)
            {
                this.extractOperatorResultList = null;
                this.canvasDefectList = null;
                this.operatorSettingList = null;
                return false;
            }
        }

        private static List<OperatorSettings> LoadTeachData(string path)
        {
            ScanOperatorSettings scanOperatorSettings = new ScanOperatorSettings();
            scanOperatorSettings.Load(Path.Combine(path, "ScanSetting.xml"));

            ExtractOperatorSettings extractOperatorSettings = new ExtractOperatorSettings();
            extractOperatorSettings.Load(Path.Combine(path, "ExtractSetting.xml"));

            InspectOperatorSettings inspectOperatorSettings = new InspectOperatorSettings();
            inspectOperatorSettings.Load(Path.Combine(path, "InspectSetting.xml"));

            return new UniScanWPF.Table.Operation.OperatorSettings[] { scanOperatorSettings, extractOperatorSettings, inspectOperatorSettings }.ToList();
        }
    }
}
