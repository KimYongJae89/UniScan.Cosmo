using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using UniScanWPF.Helper;
using UniScanWPF.Screen.PinHoleColor.Color.Data;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.Color.Inspect
{
    public enum ColorDefectType
    {
        Blot, NoPrint
    }
    
    public class ColorDetectorResult : DetectorResult
    {
        BitmapSource sheetImage;
        float average;
        
        public BitmapSource SheetImage { get => sheetImage; set => sheetImage = value; }
        public float Average { get => average; set => average = value; }
        
        public override void ExportResult(string resultPath, XmlElement detectorElement)
        {
            XmlHelper.SetValue(detectorElement, "Average", average.ToString());

            if (sheetImage != null)
            {
                string sheetPath = Path.Combine(resultPath, "SheetImage.jpg");
                WPFImageHelper.SaveBitmapSource(sheetPath, sheetImage);
            }

            base.ExportResult(resultPath, detectorElement);
        }

        public override void ImportResult(string resultPath, XmlElement detectorElement)
        {
            average = Convert.ToSingle(XmlHelper.GetValue(detectorElement, "Average", "0"));
            string sheetPath = Path.Combine(resultPath, "SheetImage.jpg");
            if (File.Exists(sheetPath) == true)
                sheetImage = WPFImageHelper.LoadBitmapSource(sheetPath);

            base.ImportResult(resultPath, detectorElement);
        }

        public override List<UIElement> GetFigures()
        {
            List<UIElement> figureList = new List<UIElement>();

            if (sheetImage == null)
            {
                foreach (ColorDefect defect in defectList)
                {
                    System.Windows.Shapes.Rectangle rectangle = new System.Windows.Shapes.Rectangle();
                    rectangle.StrokeThickness = 3;
                    
                    TextBlock textBlock = new TextBlock();
                    textBlock.TextAlignment = TextAlignment.Center;
                    textBlock.FontSize = defect.Rectangle.Width / 4;
                    textBlock.Text = string.Format("{0:0.0}", defect.DiffValue);

                    switch (defect.Type)
                    {
                        case ColorDefectType.Blot:
                            rectangle.Stroke = new SolidColorBrush(Colors.Red);
                            textBlock.Foreground = new SolidColorBrush(Colors.Crimson);
                            break;
                        case ColorDefectType.NoPrint:
                            rectangle.Stroke = new SolidColorBrush(Colors.Yellow);
                            textBlock.Foreground = new SolidColorBrush(Colors.Yellow);
                            break;
                    }

                    Canvas.SetLeft(rectangle, defect.Rectangle.X);
                    Canvas.SetTop(rectangle, defect.Rectangle.Y);
                    Canvas.SetLeft(textBlock, defect.Rectangle.X);
                    Canvas.SetTop(textBlock, defect.Rectangle.Y + ((defect.Rectangle.Height - textBlock.FontSize) / 2));

                    rectangle.Width = defect.Rectangle.Width;
                    rectangle.Height = defect.Rectangle.Height;
                    textBlock.Width = defect.Rectangle.Width;

                    figureList.Add(rectangle);
                    if (textBlock.Foreground != null)
                        figureList.Add(textBlock);
                }
            }

            return figureList;
        }

        public override DetectorType GetDetectorType()
        {
            return DetectorType.Color;
        }
    }
}
