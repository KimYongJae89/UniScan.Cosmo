﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Standard.DynMvp.Base
{
    public class MathHelper
    {
        public static bool IsPrimitive(object o)
        {
            return IsNumeric(o) || IsBoolean(o);
        }

        public static bool IsNumeric(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsBoolean(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Boolean:
                    return true;
                default:
                    return false;
            }
        }

        public static double arctan(double y, double x)
        {
            double value;
            if (x == 0)
            {
                if (y > 0)
                    value = Math.PI / 2;
                else
                    value = Math.PI * 3 / 2;
            }
            else
            {
                value = Math.Atan(y / x);
                if (x < 0)
                    value += Math.PI;
            }

            return value;
        }

        public static double DegToRad(double deg)
        {
            return deg / 180.0 * Math.PI;
        }

        public static double RadToDeg(double rad)
        {
            return rad * 180.0 / Math.PI;
        }

        public static double GetAngle(Point basePt, Point point1, Point point2)
        {
            return GetAngle(DrawingHelper.ToPointF(basePt), DrawingHelper.ToPointF(point1), DrawingHelper.ToPointF(point2));
        }

        public static double GetAngle(PointF basePt, PointF point1, PointF point2)
        {
            double theta1 = arctan(basePt.Y - point1.Y, point1.X - basePt.X);
            double theta2 = arctan(basePt.Y - point2.Y, point2.X - basePt.X);

            double angle = RadToDeg(theta2 - theta1);
            if (angle < 0)
                angle = 360 + angle;

            return angle%360;
        }

        public static Point Rotate(Point point, Point centerPt, double angleDegree)
        {
            return DrawingHelper.ToPoint(Rotate(DrawingHelper.ToPointF(point), DrawingHelper.ToPointF(centerPt), angleDegree));
        }

        public static PointF Rotate(PointF point, PointF centerPt, double angleDegree)
        {
            if (angleDegree == 0)
                return point;

            PointF tempPoint = new PointF(point.X, point.Y);

            tempPoint.X -= centerPt.X;
            tempPoint.Y -= centerPt.Y;

            tempPoint.Y *= -1;

            double angleRad = DegToRad(angleDegree);
            double X = (double)((tempPoint.X * Math.Cos(angleRad)) - (tempPoint.Y * Math.Sin(angleRad)));
            double Y = (double)((tempPoint.X * Math.Sin(angleRad)) + (tempPoint.Y * Math.Cos(angleRad)));

            tempPoint.X = (float)(X + centerPt.X);
            //            tempPoint.Y = Y + centerPt.Y;
            tempPoint.Y = (float)((Y * (-1)) + centerPt.Y);

            return tempPoint;
        }

        public static SizeF Rotate(Size size, double angleDegree)
        {
            SizeF tempSize = new SizeF();

            double angleRad = DegToRad(angleDegree);
            tempSize.Width = (float)((size.Width * Math.Cos(angleRad)) - (size.Height * Math.Sin(angleRad)));
            tempSize.Height = (float)((size.Width * Math.Sin(angleRad)) + (size.Height * Math.Cos(angleRad)));

            return tempSize;
        }

        public static SizeF Rotate(SizeF size, double angleDegree)
        {
            SizeF tempSize = new SizeF();

            double angleRad = DegToRad(angleDegree);
            tempSize.Width = (float)((size.Width * Math.Cos(angleRad)) - (size.Height * Math.Sin(angleRad)));
            tempSize.Height = (float)((size.Width * Math.Sin(angleRad)) + (size.Height * Math.Cos(angleRad)));

            return tempSize;
        }

        public static float GetLength(PointF pt1, PointF pt2)
        {
            float deltaX = pt2.X - pt1.X;
            float deltaY = pt2.Y - pt1.Y;

            return (float)Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
        }

        public static float GetLength(SizeF size)
        {
            return (float)Math.Sqrt((size.Width * size.Width) + (size.Height * size.Height));
        }

        public static float Bound(float value, float minValue, float maxValue)
        {
            return Math.Min(Math.Max(value, minValue), maxValue);
        }

        public static int Bound(int value, int minValue, int maxValue)
        {
            return Math.Min(Math.Max(value, minValue), maxValue);
        }

        public static float Median(List<int> list)
        {
            List<int> copiedList = new List<int>(list);
            copiedList.Sort();
            if (copiedList.Count % 2 == 1)
            {
                int midIdx = copiedList.Count / 2;
                int value = copiedList[midIdx];
                return value;
            }
            else
            {
                int midIdx = copiedList.Count / 2 - 1;
                List<int> subList = copiedList.GetRange(midIdx, 2);
                return (float)subList.Average();
            }
        }
    }
}
