using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DynMvp.UI;
using System.Xml;
using DynMvp.Base;
using Matrox.MatroxImagingLibrary;

namespace DynMvp.Vision
{
    public class BlobBox
    {
        int size = 0;

        public bool selectCenterPt = false;

        public double[] leftArray = new double[0];
        public double[] rightArray = new double[0];
        public double[] topArray = new double[0];
        public double[] bottomArray = new double[0];

        public double[] centerXArray = new double[0];
        public double[] centerYArray = new double[0];

        public double[] areaArray = new double[0];
        public double[] labelNumArray = new double[0];
        public double[] sigmaValueArray = new double[0];
        public double[] minValueArray = new double[0];
        public double[] maxValueArray = new double[0];
        public double[] meanValueArray = new double[0];
        public double[] compactnessArray = new double[0];
        public double[] convexFillRatioArray = new double[0];
        public double[] aspectRetioArray = new double[0];
        public double[] rectangularityArray = new double[0];
        public double[] roughnessArray = new double[0];

        public double[] minFeretArray = new double[0];
        public double[] maxFeretArray = new double[0];

        //RotateRect
        public double[] rotateRectAngle = new double[0];
        public double[] rotateRectX1 = new double[0];
        public double[] rotateRectX2 = new double[0];
        public double[] rotateRectX3 = new double[0];
        public double[] rotateRectX4 = new double[0];
        public double[] rotateRectY1 = new double[0];
        public double[] rotateRectY2 = new double[0];
        public double[] rotateRectY3 = new double[0];
        public double[] rotateRectY4 = new double[0];
        public double[] rotateRectWidth = new double[0];
        public double[] rotateRectHeight = new double[0];
        public double[] rotateRectCenterX = new double[0];
        public double[] rotateRectCenterY = new double[0];
        public MIL_INT[] numberOfHoles = new MIL_INT[0];

        public BlobBox(int size)
        {
            Init(size);
        }

        public void Resize(int newSize)
        {
            if (this.size >= newSize)
                return;

            Init(newSize);
        }

        private void Init(int size)
        {
            this.size = size;
            leftArray = new double[size];
            rightArray = new double[size];
            topArray = new double[size];
            bottomArray = new double[size];

            centerXArray = new double[size];
            centerYArray = new double[size];

            areaArray = new double[size];
            labelNumArray = new double[size];
            sigmaValueArray = new double[size];
            minValueArray = new double[size];
            maxValueArray = new double[size];
            meanValueArray = new double[size];
            compactnessArray = new double[size];
            convexFillRatioArray = new double[size];
            aspectRetioArray = new double[size];
            rectangularityArray = new double[size];
            roughnessArray = new double[size];

            minFeretArray = new double[size];
            maxFeretArray = new double[size];

            //RotateRect
            rotateRectAngle = new double[size];
            rotateRectX1 = new double[size];
            rotateRectX2 = new double[size];
            rotateRectX3 = new double[size];
            rotateRectX4 = new double[size];
            rotateRectY1 = new double[size];
            rotateRectY2 = new double[size];
            rotateRectY3 = new double[size];
            rotateRectY4 = new double[size];
            rotateRectWidth = new double[size];
            rotateRectHeight = new double[size];
            rotateRectCenterX = new double[size];
            rotateRectCenterY = new double[size];
            numberOfHoles = new MIL_INT[size];
        }

        public BlobRect GetBlobRect(int i)
        {
            BlobRect blobRect = new BlobRect();
            blobRect.LabelNumber = (int)labelNumArray[i];
            blobRect.Area = (long)areaArray[i];
            blobRect.BoundingRect = new RectangleF((float)leftArray[i], (float)topArray[i], (float)(rightArray[i] - leftArray[i] + 1), (float)(bottomArray[i] - topArray[i] + 1));

            if (selectCenterPt)
                blobRect.CenterPt = new PointF((float)centerXArray[i], (float)centerYArray[i]);
            else
                blobRect.CenterPt = DrawingHelper.CenterPoint(blobRect.BoundingRect);
            blobRect.CalcCenterOffset();

            blobRect.SigmaValue = (float)sigmaValueArray[i];
            blobRect.MinValue = (float)minValueArray[i];
            blobRect.MaxValue = (float)maxValueArray[i];
            blobRect.MeanValue = (float)meanValueArray[i];
            blobRect.Compactness = (float)compactnessArray[i];
            blobRect.NumberOfHoles = (int)numberOfHoles[i];
            blobRect.SawToothArea = (float)convexFillRatioArray[i] - (float)labelNumArray[i];
            blobRect.AspectRetio = (float)aspectRetioArray[i];
            blobRect.Rectangularity = (float)rectangularityArray[i];
            blobRect.Roughness = (float)roughnessArray[i];
            blobRect.MinFeretDiameter = (float)minFeretArray[i];
            blobRect.MaxFeretDiameter = (float)maxFeretArray[i];

            blobRect.RotateWidth = (float)rotateRectWidth[i];
            blobRect.RotateHeight = (float)rotateRectHeight[i];
            blobRect.RotateAngle = (float)rotateRectAngle[i];
            blobRect.RotateXArray = new float[] { (float)rotateRectX1[i], (float)rotateRectX2[i], (float)rotateRectX4[i], (float)rotateRectX3[i] };
            blobRect.RotateYArray = new float[] { (float)rotateRectY1[i], (float)rotateRectY2[i], (float)rotateRectY4[i], (float)rotateRectY3[i] };
            blobRect.RotateCenterPt = new PointF((float)rotateRectCenterX[i], (float)rotateRectCenterY[i]);

            return blobRect;
        }
    }

    public class BlobRect
    {
        float area;
        public float Area
        {
            get { return area; }
            set { area = value; }
        }

        float circularity;
        public float Circularity
        {
            get { return circularity; }
            set { circularity = value; }
        }

        RectangleF boundingRect;
        public RectangleF BoundingRect
        {
            get { return boundingRect; }
            set { boundingRect = value; }
        }

        float[] rotateXArray;
        public float[] RotateXArray
        {
            get { return rotateXArray; }
            set { rotateXArray = value; }
        }

        float[] rotateYArray;
        public float[] RotateYArray
        {
            get { return rotateYArray; }
            set { rotateYArray = value; }
        }

        float rotateWidth;
        public float RotateWidth
        {
            get { return rotateWidth; }
            set { rotateWidth = value; }
        }

        float rotateHeight;
        public float RotateHeight
        {
            get { return rotateHeight; }
            set { rotateHeight = value; }
        }

        float rotateAngle;
        public float RotateAngle
        {
            get { return rotateAngle; }
            set { rotateAngle = value; }
        }

        PointF rotateCenterPt;
        public PointF RotateCenterPt
        {
            get { return rotateCenterPt; }
            set { rotateCenterPt = value; }
        }

        PointF centerPt;
        public PointF CenterPt
        {
            get { return centerPt; }
            set { centerPt = value; }
        }

        PointF centerOffset;
        public PointF CenterOffset
        {
            get { return centerOffset; }
            set { centerOffset = value; }
        }

        int labelNumber;
        public int LabelNumber
        {
            get { return labelNumber; }
            set { labelNumber = value; }
        }

        float sigmaValue;
        public float SigmaValue
        {
            get { return sigmaValue; }
            set { sigmaValue = value; }
        }

        float maxValue;
        public float MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        float minValue;
        public float MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        float meanValue;
        public float MeanValue
        {
            get { return meanValue; }
            set { meanValue = value; }
        }

        float compactness;
        public float Compactness
        {
            get { return compactness; }
            set { compactness = value; }
        }

        float rectangularity;
        public float Rectangularity
        {
            get { return rectangularity; }
            set { rectangularity = value; }
        }

        float roughness;
        public float Roughness
        {
            get { return roughness; }
            set { roughness = value; }
        }

        int numberOfHoles;
        public int NumberOfHoles
        {
            get { return numberOfHoles; }
            set { numberOfHoles = value; }
        }

        float sawToothArea;
        public float SawToothArea
        {
            get { return sawToothArea; }
            set { sawToothArea = value; }
        }

        float aspectRetio;
        public float AspectRetio
        {
            get { return aspectRetio; }
            set { aspectRetio = value; }
        }
        
        float minFeretDiameter;
        public float MinFeretDiameter
        {
            get { return minFeretDiameter; }
            set { minFeretDiameter = value; }
        }

        float maxFeretDiameter;
        public float MaxFeretDiameter
        {
            get { return maxFeretDiameter; }
            set { maxFeretDiameter = value; }
        }

        object measureData;
        public object MeasureData
        {
            get { return measureData; }
            set { measureData = value; }
        }

        public float AreaRatio
        {
            get { return this.area / (this.boundingRect.Width * this.boundingRect.Height) * 100; }
        }

        public void CalcCenterOffset()
        {
            PointF boundCenter = Base.DrawingHelper.CenterPoint(boundingRect);
            centerOffset = new PointF(boundCenter.X - centerPt.X, boundCenter.Y - centerPt.Y);
        }

        public void MoveOffset(int x, int y) { MoveOffset(new Point(x, y)); }
        public void MoveOffset(Point offset)
        {
            boundingRect.Offset(offset);
            centerPt = PointF.Add(centerPt, (Size)offset);
        }

        public static BlobRect operator +(BlobRect blobRect1, BlobRect blobRect2)
        {
            BlobRect mergeBlobRect = new BlobRect();

            float areaRatio1 = blobRect1.Area / (blobRect1.Area + blobRect2.Area);
            float areaRatio2 = 1.0f - areaRatio1;
            
            mergeBlobRect.Area = blobRect1.Area + blobRect2.Area;
            mergeBlobRect.BoundingRect = RectangleF.Union(blobRect1.BoundingRect, blobRect2.BoundingRect);
            mergeBlobRect.CenterPt = new PointF(blobRect1.CenterPt.X * areaRatio1 + blobRect2.CenterPt.X * areaRatio2, blobRect1.CenterPt.Y * areaRatio1 + blobRect2.CenterPt.Y * areaRatio2);
            //mergeBlobRect.Circularity = blobRect1.Circularity * areaRatio1 + blobRect2.Circularity * areaRatio2;
            mergeBlobRect.SigmaValue = blobRect1.SigmaValue * areaRatio1 + blobRect2.SigmaValue * areaRatio2;
            mergeBlobRect.compactness = (blobRect1.compactness + blobRect2.compactness) / 2.0f;

            mergeBlobRect.MinValue = Math.Min(blobRect1.MinValue, blobRect2.MinValue);
            mergeBlobRect.MaxValue = Math.Max(blobRect1.MaxValue, blobRect2.MaxValue);

            return mergeBlobRect;
        }

        public BlobRect Clone()
        {
            BlobRect clone = new BlobRect();
            clone.Copy(this);

            return clone;
        }

        public void Copy(BlobRect srcBlob)
        {
            this.area = srcBlob.area;
            this.circularity = srcBlob.circularity;
            this.boundingRect = srcBlob.boundingRect;
            this.centerPt = srcBlob.centerPt;
            this.centerOffset = srcBlob.centerOffset;
            this.labelNumber = srcBlob.labelNumber;
            this.sigmaValue = srcBlob.sigmaValue;
            this.minValue = srcBlob.minValue;
            this.maxValue = srcBlob.maxValue;
            this.meanValue = srcBlob.meanValue;
            this.compactness = srcBlob.compactness;
            this.rectangularity = srcBlob.rectangularity;
            this.roughness = srcBlob.roughness;
            this.numberOfHoles = srcBlob.numberOfHoles;
            this.sawToothArea = srcBlob.sawToothArea;
            this.aspectRetio = srcBlob.aspectRetio;
                
            this.minFeretDiameter = srcBlob.minFeretDiameter;
            this.maxFeretDiameter = srcBlob.maxFeretDiameter;
        }

        public virtual void SaveXml(XmlElement xmlElement, string key=null)
        {
            if(string.IsNullOrEmpty(key)==false)
            {
                XmlElement sumElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(sumElement);
                SaveXml(sumElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "area", this.area);
            XmlHelper.SetValue(xmlElement, "circularity", this.circularity);
            XmlHelper.SetValue(xmlElement, "boundingRect", this.boundingRect);
            XmlHelper.SetValue(xmlElement, "centerPt", this.centerPt);
            XmlHelper.SetValue(xmlElement, "centerOffset", this.centerOffset);
            XmlHelper.SetValue(xmlElement, "labelNumber", this.labelNumber);
            XmlHelper.SetValue(xmlElement, "sigmaValue", this.sigmaValue);
            XmlHelper.SetValue(xmlElement, "minValue", this.minValue);
            XmlHelper.SetValue(xmlElement, "maxValue", this.maxValue);
            XmlHelper.SetValue(xmlElement, "meanValue", this.meanValue);
            XmlHelper.SetValue(xmlElement, "compactness", this.compactness);
            XmlHelper.SetValue(xmlElement, "rectangularity", this.rectangularity);
            XmlHelper.SetValue(xmlElement, "roughness", this.roughness);
            XmlHelper.SetValue(xmlElement, "numberOfHoles", this.numberOfHoles);
            XmlHelper.SetValue(xmlElement, "SawToothArea", this.sawToothArea);
            XmlHelper.SetValue(xmlElement, "aspectRetio", this.aspectRetio);
            XmlHelper.SetValue(xmlElement, "minFeretDiameter", this.minFeretDiameter);
            XmlHelper.SetValue(xmlElement, "maxFeretDiameter", this.maxFeretDiameter);
        }

        public virtual void LoadXml(XmlElement xmlElement, string key = null)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement sumElement = xmlElement[key];
                if (sumElement != null)
                    LoadXml(sumElement);
                return;
            }

            XmlHelper.GetValue(xmlElement, "area", this.area, ref this.area);
            XmlHelper.GetValue(xmlElement, "circularity", this.circularity, ref this.circularity);
            XmlHelper.GetValue(xmlElement, "boundingRect", ref this.boundingRect);
            XmlHelper.GetValue(xmlElement, "centerPt", ref this.centerPt);
            XmlHelper.GetValue(xmlElement, "centerOffset", ref this.centerOffset);
            XmlHelper.GetValue(xmlElement, "labelNumber", this.labelNumber, ref this.labelNumber);
            XmlHelper.GetValue(xmlElement, "sigmaValue", this.sigmaValue, ref this.sigmaValue);
            XmlHelper.GetValue(xmlElement, "minValue", this.minValue, ref this.minValue);
            XmlHelper.GetValue(xmlElement, "maxValue", this.maxValue, ref this.maxValue);
            XmlHelper.GetValue(xmlElement, "meanValue", this.meanValue, ref this.meanValue);
            XmlHelper.GetValue(xmlElement, "compactness", this.compactness, ref this.compactness);
            XmlHelper.GetValue(xmlElement, "rectangularity", this.rectangularity, ref this.rectangularity);
            XmlHelper.GetValue(xmlElement, "roughness", this.roughness, ref this.roughness);
            XmlHelper.GetValue(xmlElement, "numberOfHoles", this.numberOfHoles, ref this.numberOfHoles);
            XmlHelper.GetValue(xmlElement, "convexFillRatio", this.sawToothArea, ref this.sawToothArea);
            XmlHelper.GetValue(xmlElement, "aspectRetio", this.aspectRetio, ref this.aspectRetio);
            XmlHelper.GetValue(xmlElement, "minFeretDiameter", this.minFeretDiameter, ref this.minFeretDiameter);
            XmlHelper.GetValue(xmlElement, "maxFeretDiameter", this.maxFeretDiameter, ref this.maxFeretDiameter);
        }
    }

    public class BlobRectList : IDisposable
    {
        bool isReached = false;
        public bool IsReached
        {
            get { return isReached; }
            set { isReached = value; }
        }

        private List<BlobRect> blobRectList = new List<BlobRect>();

        public void SetBlobRectList(List<BlobRect> blobRectList)
        {
            this.blobRectList = blobRectList;
        }

        public List<BlobRect> GetList()
        {
            return blobRectList;
        }

        public void Append(BlobRect blobRect)
        {
            blobRectList.Add(blobRect);
        }

        public void Append(BlobRectList blobRectList)
        {
            this.blobRectList.AddRange(blobRectList.GetList());
        }

        public IEnumerator<BlobRect> GetEnumerator()
        {
            return blobRectList.GetEnumerator();
        }

        public void Clear()
        {
            blobRectList.Clear();
        }

        public BlobRect GetMaxAreaBlob()
        {
            if (blobRectList.Count == 0)
                return null;

            return blobRectList.OrderByDescending(x => x.Area).First();
        }

        public RectangleF GetUnionRect()
        {
            BlobRect maxBlobRect = GetMaxAreaBlob();
            if (maxBlobRect == null)
                return new RectangleF();

            RectangleF unionRect = maxBlobRect.BoundingRect;

            foreach (BlobRect blobRect in blobRectList)
            {
                unionRect = RectangleF.Union(unionRect, blobRect.BoundingRect);
            }

            return unionRect;
        }

        public virtual void Dispose()
        {
            //foreach (BlobRect blobRect in blobRectList)
            //    blobRect.Dispose();
        }

        public void MoveOffset(Point offset)
        {
            foreach (BlobRect blobRect in blobRectList)
                blobRect.MoveOffset(offset);
        }
    }

    public class DrawBlobOption
    {
        bool selectBlob = false;
        public bool SelectBlob
        {
            get { return selectBlob; }
            set { selectBlob = value; }
        }

        bool selectBlobContour = false;
        public bool SelectBlobContour
        {
            get { return selectBlobContour; }
            set { selectBlobContour = value; }
        }

        bool selectHoles = false;
        public bool SelectHoles
        {
            get { return selectHoles; }
            set { selectHoles = value; }
        }

        bool selectHolesContour = false;
        public bool SelectHolesContour
        {
            get { return selectHolesContour; }
            set { selectHolesContour = value; }
        }
    }

    public class BlobParam
    {
        int maxCount;
        public int MaxCount
        {
            get { return maxCount; }
            set { maxCount = value; }
        }

        bool selectArea;
        public bool SelectArea
        {
            get { return selectArea; }
            set { selectArea = value; }
        }

        bool selectBoundingRect;
        public bool SelectBoundingRect
        {
            get { return selectBoundingRect; }
            set { selectBoundingRect = value; }
        }

        bool selectRotateRect;
        public bool SelectRotateRect
        {
            get { return selectRotateRect; }
            set { selectRotateRect = value; }
        }

        bool selectCenterPt;
        public bool SelectCenterPt
        {
            get { return selectCenterPt; }
            set { selectCenterPt = value; }
        }

        bool selectLabelValue;
        public bool SelectLabelValue
        {
            get { return selectLabelValue; }
            set { selectLabelValue = value; }
        }

        bool selectNumberOfHoles;
        public bool SelectNumberOfHoles
        {
            get { return selectNumberOfHoles; }
            set { selectNumberOfHoles = value; }
        }

        double areaMin;
        public double AreaMin
        {
            get { return areaMin; }
            set { areaMin = value; }
        }

        double avgMin;
        public double AvgMin
        {
            get { return avgMin; }
            set { avgMin = value; }
        }

        double avgMax;
        public double AvgMax
        {
            get { return avgMax; }
            set { avgMax = value; }
        }

        double sigmaMin;
        public double SigmaMin
        {
            get { return sigmaMin; }
            set { sigmaMin = value; }
        }

        double areaMax;
        public double AreaMax
        {
            get { return areaMax; }
            set { areaMax = value; }
        }

        double boundingRectMinX;
        public double BoundingRectMinX
        {
            get { return boundingRectMinX; }
            set { boundingRectMinX = value; }
        }

        double boundingRectMinY;
        public double BoundingRectMinY
        {
            get { return boundingRectMinY; }
            set { boundingRectMinY = value; }
        }

        double boundingRectMaxX;
        public double BoundingRectMaxX
        {
            get { return boundingRectMaxX; }
            set { boundingRectMaxX = value; }
        }

        double boundingRectMaxY;
        public double BoundingRectMaxY
        {
            get { return boundingRectMaxY; }
            set { boundingRectMaxY = value; }
        }
        
        bool selectMinValue;
        public bool SelectMinValue
        {
            get { return selectMinValue; }
            set { selectMinValue = value; }
        }

        bool selectMeanValue;
        public bool SelectMeanValue
        {
            get { return selectMeanValue; }
            set { selectMeanValue = value; }
        }

        bool selectMaxValue;
        public bool SelectMaxValue
        {
            get { return selectMaxValue; }
            set { selectMaxValue = value; }
        }

        bool selectSigmaValue;
        public bool SelectSigmaValue
        {
            get { return selectSigmaValue; }
            set { selectSigmaValue = value; }
        }

        bool selectCompactness;
        public bool SelectCompactness
        {
            get { return selectCompactness; }
            set { selectCompactness = value; }
        }

        bool selectRoughness;
        public bool SelectRoughness
        {
            get { return selectRoughness; }
            set { selectRoughness = value; }
        }

        bool selectAspectRatio;
        public bool SelectAspectRatio
        {
            get { return selectAspectRatio; }
            set { selectAspectRatio = value; }
        }

        bool selectRectangularity;
        public bool SelectRectangularity
        {
            get { return selectRectangularity; }
            set { selectRectangularity = value; }
        }
       
        bool selectSawToothArea;
        public bool SelectSawToothArea
        {
            get { return selectSawToothArea; }
            set { selectSawToothArea = value; }
        }

        bool eraseBorderBlobs;
        public bool EraseBorderBlobs
        {
            get { return eraseBorderBlobs; }
            set { eraseBorderBlobs = value; }
        }

        bool selectBorderBlobs;
        public bool SelectBorderBlobs
        {
            get { return selectBorderBlobs; }
            set { selectBorderBlobs = value; }
        }

        bool selectFeretDiameter;
        public bool SelectFeretDiameter
        {
            get { return selectFeretDiameter; }
            set { selectFeretDiameter = value; }
        }

        double rotateWidthMin;
        public double RotateWidthMin
        {
            get { return rotateWidthMin; }
            set { rotateWidthMin = value; }
        }

        double rotateWidthMax;
        public double RotateWidthMax
        {
            get { return rotateWidthMax; }
            set { rotateWidthMax = value; }
        }

        double rotateHeightMin;
        public double RotateHeightMin
        {
            get { return rotateHeightMin; }
            set { rotateHeightMin = value; }
        }

        double rotateHeightMax;
        public double RotateHeightMax
        {
            get { return rotateHeightMax; }
            set { rotateHeightMax = value; }
        }

        public BlobParam()
        {
            maxCount = 0;
            selectArea = true;
            selectBoundingRect = true;
        }
    }
}
