using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Data;

namespace UniScanWPF.Screen.PinHoleColor.PinHole.Inspect
{
    internal class PinHoleDetectorResult : DetectorResult
    {
        Rectangle interestRegion;
        public Rectangle InterestRegion { get => interestRegion; set => interestRegion = value; }

        public override void ExportResult(string resultPath, XmlElement detectorElement)
        {
            XmlHelper.SetValue(detectorElement, "InterestRegionX", interestRegion.X.ToString());
            XmlHelper.SetValue(detectorElement, "InterestRegionY", interestRegion.Y.ToString());
            XmlHelper.SetValue(detectorElement, "InterestRegionW", interestRegion.Width.ToString());
            XmlHelper.SetValue(detectorElement, "InterestRegionH", interestRegion.Height.ToString());

            base.ExportResult(resultPath, detectorElement);
        }

        public override void ImportResult(string resultPath, XmlElement detectorElement)
        {
            int x = Convert.ToInt32(XmlHelper.GetValue(detectorElement, "InterestRegionX", "0"));
            int y = Convert.ToInt32(XmlHelper.GetValue(detectorElement, "InterestRegionY", "0"));
            int width = Convert.ToInt32(XmlHelper.GetValue(detectorElement, "InterestRegionW", "0"));
            int height = Convert.ToInt32(XmlHelper.GetValue(detectorElement, "InterestRegionH", "0"));

            interestRegion = new Rectangle(x, y, width, height);
            
            base.ImportResult(resultPath, detectorElement);
        }

        public override DetectorType GetDetectorType()
        {
            return DetectorType.PinHole;
        }

        public override List<UIElement> GetFigures()
        {
            List<UIElement> figureList = new List<UIElement>();

            foreach (PinHoleDefect defect in defectList)
            {
                System.Windows.Shapes.Rectangle rectFigure = new System.Windows.Shapes.Rectangle();
                switch (defect.Type)
                {
                    case PinHoleDefectType.PinHole:
                        rectFigure.Stroke = new SolidColorBrush(Colors.Red);
                        break;
                    case PinHoleDefectType.Dust:
                        rectFigure.Stroke = new SolidColorBrush(Colors.Yellow);
                        break;
                }

                rectFigure.StrokeThickness = 10;
                Canvas.SetLeft(rectFigure, defect.Rectangle.X);
                Canvas.SetTop(rectFigure, defect.Rectangle.Y);

                rectFigure.Width = defect.Rectangle.Width;
                rectFigure.Height = defect.Rectangle.Height;

                figureList.Add(rectFigure);
            }
            
            System.Windows.Shapes.Line lineStart = new System.Windows.Shapes.Line();
            lineStart.Stroke = new SolidColorBrush(Colors.Green);
            lineStart.StrokeThickness = 20;
            lineStart.X1 = interestRegion.X;
            lineStart.Y1 = 0;
            lineStart.X2 = interestRegion.X;
            lineStart.Y2 = interestRegion.Height;

            System.Windows.Shapes.Line lineEnd = new System.Windows.Shapes.Line();
            lineEnd.Stroke = new SolidColorBrush(Colors.Green);
            lineEnd.StrokeThickness = 20;
            lineEnd.X1 = interestRegion.X + interestRegion.Width;
            lineEnd.Y1 = 0;
            lineEnd.X2 = interestRegion.X + interestRegion.Width;
            lineEnd.Y2 = interestRegion.Height;
            
            figureList.Add(lineStart);
            figureList.Add(lineEnd);

            return figureList;
        }
    }
}
