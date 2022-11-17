using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.UI;
using System.Drawing;
using DynMvp.Base;
using System.IO;
using UniScanM.EDMS.Settings;
using UniEye.Base.Settings;

namespace UniScanM.EDMS.Data
{
    public enum DataType
    {
        FilmEdge, Coating_Film, Printing_Coating, FilmEdge_0, PrintingEdge_0, Printing_FilmEdge_0, ENUM_MAX
    }

    public class InspectionResult : UniScanM.Data.InspectionResult
    {
        bool isWaitState;
        public bool IsWaitState
        {
            get { return isWaitState; }
            set { isWaitState = value; }
        }

        int remainWaitDist = 0;
        public int RemainWaitDist
        {
            get { return remainWaitDist; }
            set { remainWaitDist = value; }
        }

        bool isZeroingState;
        public bool IsZeroingState
        {
            get { return isZeroingState; }
            set { isZeroingState = value; }
        }

        bool isMeasureState;
        public bool IsMeasureState
        {
            get { return isMeasureState; }
            set { isMeasureState = value; }
        }

        bool resetZeroing;
        public bool ResetZeroing
        {
            get { return resetZeroing; }
            set { resetZeroing = value; }
        }

        int zeroingNum;
        public int ZeroingNum
        {
            get { return zeroingNum; }
            set { zeroingNum = value; }
        }

        double[] edgePostionResult;
        public double[] EdgePositionResult
        {
            get { return edgePostionResult; }
            set { edgePostionResult = value; }
        }

        double[] totalEdgePostionResult;
        public double[] TotalEdgePositionResult
        {
            get { return totalEdgePostionResult; }
            set { totalEdgePostionResult = value; }
        }

        double firstFilmEdge = 0.0;
        public double FirstFilmEdge
        {
            get { return firstFilmEdge; }
            set { firstFilmEdge = value; }
        }

        double firstPrintingEdge = 0.0;
        public double FirstPrintingEdge
        {
            get { return firstPrintingEdge; }
            set { firstPrintingEdge = value; }
        }

        int waitRemain;
        public int WaitRemain { get => this.waitRemain; set => this.waitRemain = value; }

        public void AddEdgePositionResult(double[] edgeData, double pixelWidth)
        {
            edgePostionResult = edgeData;

            //double umPerPixel = 1.0;
            double umPerPixel = pixelWidth;

            double[] edgeResult = new double[6];
            edgeResult[(int)DataType.FilmEdge] = edgeData[0] * umPerPixel / 1000;
            edgeResult[(int)DataType.Coating_Film] = edgeData[1] <= 0 ? 0 : (edgeData[1] - edgeData[0]) * umPerPixel / 1000;
            edgeResult[(int)DataType.Printing_Coating] = edgeData[2] <= 0 ? 0 : (edgeData[2] - edgeData[1]) * umPerPixel / 1000;
            edgeResult[(int)DataType.FilmEdge_0] = firstFilmEdge < 0 ? 0 : (edgeData[0] - firstFilmEdge) * umPerPixel / 1000;
            edgeResult[(int)DataType.PrintingEdge_0] = firstPrintingEdge <= 0 ? 0 : (edgeData[2] - firstPrintingEdge) * umPerPixel / 1000;
            edgeResult[(int)DataType.Printing_FilmEdge_0] = firstPrintingEdge <= 0 ? 0 : (int)((edgeData[2] - firstPrintingEdge) - (edgeData[0] - firstFilmEdge)) * umPerPixel;

            totalEdgePostionResult = edgeResult;
        }

        public override void UpdateJudgement()
        {
            if (this.isWaitState == false && this.isZeroingState == false && this.Judgment != DynMvp.InspData.Judgment.Skip)
            {
                EDMSSettings setting = EDMSSettings.Instance();
                if (Math.Abs(this.TotalEdgePositionResult[(int)DataType.Printing_FilmEdge_0]) > Math.Abs(setting.LineStop))
                    this.Judgment = DynMvp.InspData.Judgment.Reject;
                else if (Math.Abs(this.TotalEdgePositionResult[(int)DataType.Printing_FilmEdge_0]) > Math.Abs(setting.LineWarning))
                    this.Judgment = DynMvp.InspData.Judgment.Warn;
            }
        }
    }
}

