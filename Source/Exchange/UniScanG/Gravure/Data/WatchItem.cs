using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Gravure.Data
{
    public enum WatchType {NONE, CHIP, FP, INDEX }
    public class WatchItem
    {
        int index;
        bool use;
        string name;
        WatchType watchType;
        Rectangle rectangle;

        public int Index { get => index; set => index = value; }
        public bool Use { get => use; set => use = value; }
        public string Name{ get => name; set => name = value; }
        public WatchType WatchType { get => watchType; set => watchType = value; }
        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }

        public WatchItem()
        {
            this.use = true;
        }

        public void Offset(Point pt)
        {
            this.rectangle.Offset(pt);
        }

        public void Offset(int x, int y)
        {
            this.rectangle.Offset(x, y);
        }

        public static Color GetBgColor(WatchType watchType)
        {
            switch(watchType)
            {
                case WatchType.CHIP:
                    return Color.Yellow;
                case WatchType.FP:
                    return Color.LightGreen;
                case WatchType.INDEX:
                    return Color.LightBlue;
            }
            return Color.Red;
        }

        public Figure GetFigure()
        {
            Figure figure = new RectangleFigure(this.rectangle, new Pen(GetBgColor(this.watchType), 5));
            figure.Tag = this;
            return figure;
        }

        public void Export(XmlElement element)
        {
            XmlHelper.SetValue(element, "Index", this.index.ToString());
            XmlHelper.SetValue(element, "Use", this.use);
            XmlHelper.SetValue(element, "Name", this.name);
            XmlHelper.SetValue(element, "watchType", this.watchType.ToString());
            XmlHelper.SetValue(element, "Rectangle", rectangle);
        }

        public bool Import(XmlElement element)
        {
            this.index = XmlHelper.GetValue(element, "Index", this.index);
            this.use = XmlHelper.GetValue(element, "Use", this.use);
            this.name = XmlHelper.GetValue(element, "Name", this.name);
            this.watchType = (WatchType)Enum.Parse(typeof(WatchType), XmlHelper.GetValue(element, "watchType", this.watchType.ToString()));
            XmlHelper.GetValue(element, "Rectangle", ref rectangle);
            return true;
        }

        public void CopyFrom(WatchItem watchItem)
        {
            this.index = watchItem.index;
            this.name = watchItem.name;
            this.watchType = watchItem.watchType;
            this.rectangle = watchItem.rectangle;
        }
        public WatchItem Clone()
        {
            WatchItem newWatchItem = new WatchItem();
            newWatchItem.CopyFrom(this);
            return newWatchItem;
        }

        internal string GetFileName()
        {
            return string.Format("{0}.{1}.{2}.bmp", watchType.ToString(), this.index, this.name);
        }
    }
}
