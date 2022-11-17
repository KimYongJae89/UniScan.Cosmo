using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

using DynMvp.UI;
using System.IO;

namespace DynMvp.Base
{
    public interface IStoring
    {
        void Save(XmlElement xmlElement);
        void Load(XmlElement xmlElement);
    }

    public class XmlHelper
    {
        public static XmlDocument Load(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                string tempFileName = fileName + ".bak";
                if (File.Exists(tempFileName) == true)
                {
                    File.Move(tempFileName, fileName);
                }
                else
                {
                    LogHelper.Error(LoggerType.Error, String.Format("There is no config file : {0}", fileName));
                    return null;
                }
            }

            XmlDocument xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(fileName);
            }
            catch(Exception)
            {
                LogHelper.Error(LoggerType.Error, String.Format("There is som error in config file : {0}", fileName));
                xmlDocument = null;
            }

            return xmlDocument;
        }

        public static void Save(XmlDocument xmlDocument, string fileName)
        {
            string tempFileName = fileName + "~";
            string bakName = fileName + ".bak";

            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "\t";
            xmlSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlSettings.NewLineChars = "\r\n";

            XmlWriter xmlWriter = XmlWriter.Create(tempFileName, xmlSettings);

            xmlDocument.Save(xmlWriter);
            xmlWriter.Flush();
            xmlWriter.Close();

            FileHelper.SafeSave(tempFileName, bakName, fileName);
        }
        
        public static string GetAttributeValue(XmlElement xmlElement, string attributeName, string defaultValue)
        {
            string attributeValue = xmlElement.GetAttribute(attributeName);
            if (attributeValue == "")
                return defaultValue;

            return attributeValue;
        }

        public static void SetAttributeValue(XmlElement xmlElement, string attributeName, string value)
        {
            xmlElement.SetAttribute(attributeName, value);
        }

        public static bool Exist(XmlElement xmlElement, string keyName)
        {
            XmlElement subElement = xmlElement[keyName];
            return (subElement != null);
        }

        public static void SetValue(XmlElement xmlElement, string keyName, string value)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            subElement.InnerText = value;
        }

        public static string GetValue(XmlElement xmlElement, string keyName, string defaultValue)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];

            if (subElement == null)
                return defaultValue;

            return subElement.InnerText;
        }

        // DateTime
        public static void SetValue(XmlElement xmlElement, string keyName, DateTime value, string format)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            subElement.InnerText = value.ToString(format);
        }

        public static DateTime GetValue(XmlElement xmlElement, string keyName, string format, DateTime defaultValue)
        {
            string parse = GetValue(xmlElement, keyName, defaultValue.ToString(format));
            return DateTime.ParseExact(parse, format, null);
        }

        // int
        public static void SetValue(XmlElement xmlElement, string keyName, int value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }

        public static int GetValue(XmlElement xmlElement, string keyName, int defaultValue)
        {
            return int.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, int defaultValue, ref int getValue)
        {
            getValue = int.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        // float
        public static void SetValue(XmlElement xmlElement, string keyName, float value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }

        public static float GetValue(XmlElement xmlElement, string keyName, float defaultValue)
        {
            return float.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, float defaultValue, ref float getValue)
        {
            getValue = float.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        // double
        public static void SetValue(XmlElement xmlElement, string keyName, double value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }

        public static double GetValue(XmlElement xmlElement, string keyName, double defaultValue)
        {
            return double.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, int defaultValue, ref double getValue)
        {
            getValue = double.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        // bool
        public static void SetValue(XmlElement xmlElement, string keyName, bool value)
        {
            SetValue(xmlElement, keyName, value.ToString());
        }
        
        public static bool GetValue(XmlElement xmlElement, string keyName, bool defaultValue)
        {
            return bool.Parse(GetValue(xmlElement, keyName, defaultValue.ToString()));
        }

        public static void GetValue(XmlElement xmlElement, string keyName, bool defaultValue, ref bool getValue)
        {
            bool ok = bool.TryParse(GetValue(xmlElement, keyName, defaultValue.ToString()), out getValue);
            if (ok == false)
                getValue = defaultValue;
        }



        // Rectangle
        public static void SetValue(XmlElement xmlElement, string keyName, Rectangle rectangle)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", rectangle.X.ToString());
            XmlHelper.SetValue(subElement, "Y", rectangle.Y.ToString());
            XmlHelper.SetValue(subElement, "Width", rectangle.Width.ToString());
            XmlHelper.SetValue(subElement, "Height", rectangle.Height.ToString());
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Rectangle[] rectangles)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            foreach (Rectangle rectangle in rectangles)
                SetValue(subElement, "Rectangle", rectangle);
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Rectangle[,] rectangles)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            int length0 = rectangles.GetLength(0);
            int length1 = rectangles.GetLength(1);
            XmlHelper.SetValue(subElement, "Length0", length0.ToString());
            XmlHelper.SetValue(subElement, "Length1", length1.ToString());

            for (int l0 = 0; l0 < length0; l0++)
                for (int l1 = 0; l1 < length1; l1++)
                    SetValue(subElement, string.Format("R{0}C{1}", l0, l1), rectangles[l0, l1]);
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Rectangle rectangle)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return false;

            rectangle.X = Convert.ToInt32(XmlHelper.GetValue(subElement, "X", "0"));
            rectangle.Y = Convert.ToInt32(XmlHelper.GetValue(subElement, "Y", "0"));
            rectangle.Width = Convert.ToInt32(XmlHelper.GetValue(subElement, "Width", "0"));
            rectangle.Height = Convert.ToInt32(XmlHelper.GetValue(subElement, "Height", "0"));

            return true;
        }

        public static Rectangle GetValue(XmlElement xmlElement, string keyName, Rectangle defaultValue)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return defaultValue;

            Rectangle rectangle = new Rectangle();
            rectangle.X = Convert.ToInt32(XmlHelper.GetValue(subElement, "X", "0"));
            rectangle.Y = Convert.ToInt32(XmlHelper.GetValue(subElement, "Y", "0"));
            rectangle.Width = Convert.ToInt32(XmlHelper.GetValue(subElement, "Width", "0"));
            rectangle.Height = Convert.ToInt32(XmlHelper.GetValue(subElement, "Height", "0"));
            return rectangle;
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Rectangle[] rectangles)
        {
            rectangles = new Rectangle[0];
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return false;

            XmlNodeList xmlNodeList = subElement.GetElementsByTagName("Rectangle");
            Array.Resize(ref rectangles, xmlNodeList.Count);

            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement subElement2 = (XmlElement)xmlNodeList[i];
                GetValue(subElement2, "", ref rectangles[i]);
            }

            return true;
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Rectangle[,] rectangles)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return false;

            int length0 = Convert.ToInt32(XmlHelper.GetValue(subElement, "Length0", "0"));
            int length1 = Convert.ToInt32(XmlHelper.GetValue(subElement, "Length1", "0"));

            rectangles = new Rectangle[length0, length1];
            for (int l0 = 0; l0 < length0; l0++)
                for (int l1 = 0; l1 < length1; l1++)
                    GetValue(subElement, string.Format("R{0}C{1}", l0, l1), ref rectangles[l0, l1]);

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, RectangleF rectangle)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", rectangle.X.ToString());
            XmlHelper.SetValue(subElement, "Y", rectangle.Y.ToString());
            XmlHelper.SetValue(subElement, "Width", rectangle.Width.ToString());
            XmlHelper.SetValue(subElement, "Height", rectangle.Height.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref RectangleF rectangle)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            rectangle.X = Convert.ToSingle(XmlHelper.GetValue(subElement, "X", "0"));
            rectangle.Y = Convert.ToSingle(XmlHelper.GetValue(subElement, "Y", "0"));
            rectangle.Width = Convert.ToSingle(XmlHelper.GetValue(subElement, "Width", "0"));
            rectangle.Height = Convert.ToSingle(XmlHelper.GetValue(subElement, "Height", "0"));

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, RotatedRect rectangle)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", rectangle.X.ToString());
            XmlHelper.SetValue(subElement, "Y", rectangle.Y.ToString());
            XmlHelper.SetValue(subElement, "Width", rectangle.Width.ToString());
            XmlHelper.SetValue(subElement, "Height", rectangle.Height.ToString());
            XmlHelper.SetValue(subElement, "Angle", rectangle.Angle.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref RotatedRect rectangle)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            rectangle.X = Convert.ToSingle(XmlHelper.GetValue(subElement, "X", "0"));
            rectangle.Y = Convert.ToSingle(XmlHelper.GetValue(subElement, "Y", "0"));
            rectangle.Width = Convert.ToSingle(XmlHelper.GetValue(subElement, "Width", "0"));
            rectangle.Height = Convert.ToSingle(XmlHelper.GetValue(subElement, "Height", "0"));
            rectangle.Angle = Convert.ToSingle(XmlHelper.GetValue(subElement, "Angle", "0"));

            return true;
        }

        // Pen
        public static void SetValue(XmlElement xmlElement, string keyName, Pen pen)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "Color", pen.Color);
            XmlHelper.SetValue(subElement, "Width", pen.Width.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Pen pen)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            Color color = new Color();
            XmlHelper.GetValue(subElement, "Color", ref color);
            int width = Convert.ToInt32(XmlHelper.GetValue(subElement, "Width", "1"));

            pen = new Pen(color, width);

            return true;
        }

        // Brush
        public static void SetValue(XmlElement xmlElement, string keyName, Brush brush)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            SolidBrush solidBrush = brush as SolidBrush;

            XmlHelper.SetValue(subElement, "Color", solidBrush.Color);
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Brush brush)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            Color color = new Color();
            XmlHelper.GetValue(subElement, "Color", ref color);

            brush = new SolidBrush(color);

            return true;
        }

        // Point
        public static void SetValue(XmlElement xmlElement, string keyName, Point point)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", point.X.ToString());
            XmlHelper.SetValue(subElement, "Y", point.Y.ToString());
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Point[] Points)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            foreach (Point rectangle in Points)
                SetValue(subElement, "Point", rectangle);
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Point point)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            point.X = Convert.ToInt32(XmlHelper.GetValue(subElement, "X", "0"));
            point.Y = Convert.ToInt32(XmlHelper.GetValue(subElement, "Y", "0"));

            return true;
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Point[] points)
        {
            points = new Point[0];

            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            XmlNodeList xmlNodeList = subElement.GetElementsByTagName("Point");
            Array.Resize(ref points, xmlNodeList.Count);

            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                XmlElement subElement2 = (XmlElement)xmlNodeList[i];
                GetValue(subElement2, "", ref points[i]);
            }

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, PointF point)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", point.X.ToString());
            XmlHelper.SetValue(subElement, "Y", point.Y.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref PointF point)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            point.X = Convert.ToSingle(XmlHelper.GetValue(subElement, "X", "0"));
            point.Y = Convert.ToSingle(XmlHelper.GetValue(subElement, "Y", "0"));

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Point3d point3d)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "X", point3d.X.ToString());
            XmlHelper.SetValue(subElement, "Y", point3d.Y.ToString());
            XmlHelper.SetValue(subElement, "Z", point3d.Z.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Point3d point3d)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            point3d.X = Convert.ToSingle(XmlHelper.GetValue(subElement, "X", "0"));
            point3d.Y = Convert.ToSingle(XmlHelper.GetValue(subElement, "Y", "0"));
            point3d.Z = Convert.ToSingle(XmlHelper.GetValue(subElement, "Z", "0"));

            return true;
        }

        // Size
        public static void SetValue(XmlElement xmlElement, string keyName, Size size)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "Width", size.Width.ToString());
            XmlHelper.SetValue(subElement, "Height", size.Height.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Size size)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            size.Width = Convert.ToInt32(XmlHelper.GetValue(subElement, "Width", "0"));
            size.Height = Convert.ToInt32(XmlHelper.GetValue(subElement, "Height", "0"));

            return true;
        }

        public static Size GetValue(XmlElement xmlElement, string keyName, Size defaultValue)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return defaultValue;

            Size size = new Size();
            size.Width = Convert.ToInt32(XmlHelper.GetValue(subElement, "Width", "0"));
            size.Height = Convert.ToInt32(XmlHelper.GetValue(subElement, "Height", "0"));
            return size;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, SizeF size)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "Width", size.Width.ToString());
            XmlHelper.SetValue(subElement, "Height", size.Height.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref SizeF size)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            size.Width = Convert.ToSingle(XmlHelper.GetValue(subElement, "Width", "0"));
            size.Height = Convert.ToSingle(XmlHelper.GetValue(subElement, "Height", "0"));

            return true;
        }

        // Font
        public static void SetValue(XmlElement xmlElement, string keyName, Font font)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            XmlHelper.SetValue(subElement, "Family", font.FontFamily.GetName(0));
            XmlHelper.SetValue(subElement, "Size", font.SizeInPoints.ToString());
            XmlHelper.SetValue(subElement, "Style", font.Style.ToString());
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Font font)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            string family = XmlHelper.GetValue(subElement, "Family", "Arial");
            float size = Convert.ToSingle(XmlHelper.GetValue(subElement, "Size", "10"));
            FontStyle style = (FontStyle)Enum.Parse(typeof(FontStyle), XmlHelper.GetValue(subElement, "Style", "Regular"));

            font = new Font(family, size, style);

            return true;
        }

        public static void SetValue(XmlElement xmlElement, string keyName, Color color)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(subElement);

            subElement.InnerText = System.Drawing.ColorTranslator.ToHtml(color);
        }

        public static bool GetValue(XmlElement xmlElement, string keyName, ref Color color)
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            //XmlElement subElement = xmlElement[keyName];
            if (subElement == null)
                return false;

            color = System.Drawing.ColorTranslator.FromHtml(subElement.InnerText);

            return true;
        }

        //Enum
        public static TEnum GetValue<TEnum>(XmlElement xmlElement, string keyName, TEnum defaultEnum)
             where TEnum : struct
        {
            XmlElement subElement = string.IsNullOrEmpty(keyName) ? xmlElement : xmlElement[keyName];
            if (subElement == null)
                return defaultEnum;

            string stringValue = XmlHelper.GetValue(xmlElement, keyName, defaultEnum.ToString());
            TEnum result;

            if (Enum.TryParse<TEnum>(stringValue, out result))
                return result;

            return defaultEnum;
        }
    }
}
