using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Temp
{
    public enum RangeType
    {
        Left, Right, Middle, Vertical, Horizontal
    }

    public enum FiducialPosition
    {
        Right, Left
    }

    public class SheetRange
    {
        RangeType type;
        public RangeType Type
        {
            get { return type; }
            set { type = value; }
        }

        int startPos = 0;
        public int StartPos
        {
            get { return startPos; }
            set { startPos = value; }
        }

        int endPos = 0;
        public int EndPos
        {
            get { return endPos; }
            set { endPos = value; }
        }

        int xFiducialPt = 0;
        public int XFiducialPt
        {
            get { return xFiducialPt; }
            set { xFiducialPt = value; }
        }

        public int Length
        {
            get { return this.endPos - this.startPos; }
        }

        public SheetRange(RangeType type, int startPos, int endPos, int xFiducialPt = 0)
        {
            this.type = type;
            this.startPos = startPos;
            this.endPos = endPos;
            this.xFiducialPt = xFiducialPt;
        }
    }

    public class ProjectionRegion
    {
        private int id = -1;
        public int Id
        {
            get { return id; }
        }

        Rectangle region;
        public Rectangle Region
        {
            get { return region; }
            set { region = value; }
        }

        List<Rectangle> inspRectList;
        public List<Rectangle> InspRectList
        {
            get { return inspRectList; }
            set { inspRectList = value; }
        }

        public List<SheetRange> this[int index]
        {
            get { return rangeListArray[index]; }
        }

        List<SheetRange>[] rangeListArray = new List<SheetRange>[2];
        public List<SheetRange>[] RangeList
        {
            get { return rangeListArray; }
            set { rangeListArray = value; }
        }

        public ProjectionRegion(int id)
        {
            this.id = id;
            rangeListArray[0] = new List<SheetRange>();
            rangeListArray[1] = new List<SheetRange>();
        }

        public void Save(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "Id", id.ToString());
            XmlHelper.SetValue(paramElement, "RegionX", region.X.ToString());
            XmlHelper.SetValue(paramElement, "RegionY", region.Y.ToString());
            XmlHelper.SetValue(paramElement, "RegionWidth", region.Width.ToString());
            XmlHelper.SetValue(paramElement, "RegionHeight", region.Height.ToString());
            
            foreach (List<SheetRange> rangeList in rangeListArray)
            {
                foreach (SheetRange range in rangeList)
                {
                    XmlElement projectionRangeElement = paramElement.OwnerDocument.CreateElement("ProjectionRange");
                    paramElement.AppendChild(projectionRangeElement);

                    XmlHelper.SetValue(projectionRangeElement, "RangeType", range.Type.ToString());
                    XmlHelper.SetValue(projectionRangeElement, "StartPos", range.StartPos.ToString());
                    XmlHelper.SetValue(projectionRangeElement, "EndPos", range.EndPos.ToString());
                    XmlHelper.SetValue(projectionRangeElement, "FiducialPt", range.XFiducialPt.ToString());
                }
            }

            if (inspRectList != null)
            {
                XmlElement inspRectElement = paramElement.OwnerDocument.CreateElement("InspRect");
                paramElement.AppendChild(inspRectElement);
                foreach (Rectangle inspRect in inspRectList)
                {
                    XmlHelper.SetValue(inspRectElement, "inspRect", string.Format("{0};{1};{2};{3}", inspRect.X, inspRect.Y, inspRect.Width, inspRect.Height));
                }
            }
        }

        public void Load(XmlElement paramElement)
        {
            id = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Id", "-1"));
            region.X = Convert.ToInt32(XmlHelper.GetValue(paramElement, "RegionX", "0"));
            region.Y = Convert.ToInt32(XmlHelper.GetValue(paramElement, "RegionY", "0"));
            region.Width = Convert.ToInt32(XmlHelper.GetValue(paramElement, "RegionWidth", "0"));
            region.Height = Convert.ToInt32(XmlHelper.GetValue(paramElement, "RegionHeight", "0"));
            
            foreach (XmlElement projectionRangeElement in paramElement)
            {
                if (projectionRangeElement.Name == "ProjectionRange")
                {
                    int startPos = Convert.ToInt32(XmlHelper.GetValue(projectionRangeElement, "StartPos", "0"));
                    int endPos = Convert.ToInt32(XmlHelper.GetValue(projectionRangeElement, "EndPos", "0"));
                    RangeType rangeType = (RangeType)Enum.Parse(typeof(RangeType), XmlHelper.GetValue(projectionRangeElement, "RangeType", "Middle"));
                    int xFiducialPt = Convert.ToInt32(XmlHelper.GetValue(projectionRangeElement, "FiducialPt", "0"));

                    rangeListArray[rangeType == RangeType.Left ? 0 : 1].Add(new SheetRange(rangeType, startPos, endPos, xFiducialPt));
                }
            }

            XmlElement inspRectElement = paramElement["InspRect"];
            if (inspRectElement != null)
            {
                if (inspRectList != null)
                    inspRectList.Clear();
                inspRectList = new List<Rectangle>();
                XmlNodeList nodeList = inspRectElement.GetElementsByTagName("inspRect");
                foreach (XmlElement nodeElement in nodeList)
                {
                    string value = nodeElement.InnerText;
                    if (string.IsNullOrEmpty(value))
                        continue;
                    string[] token = value.Split(';');
                    Rectangle rect = new Rectangle(int.Parse(token[0]), int.Parse(token[1]), int.Parse(token[2]), int.Parse(token[3]));
                    inspRectList.Add(rect);
                }
            }
        }
    }
}
