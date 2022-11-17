using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Settings;

namespace UniScanWPF.Table.Operation.Operators
{
    public class CanvasDefect
    {
        IResultObject defect;
        Point[] rotateRectPointList;
        //System.Drawing.RectangleF boundingRect;

        //public System.Drawing.RectangleF BoundingRect { get => boundingRect; }
        public Point[] RotateRectPointList { get => rotateRectPointList; set => rotateRectPointList = value; }
        public IResultObject Defect { get => defect; }

        public CanvasDefect(IResultObject defect, ScanOperatorResult scanOperatorResult)
        {
            this.defect = defect;
            //Rect rect = defect.GetRect(1);
            Point[] points = defect.GetPoints(1);
            this.rotateRectPointList = new Point[points.Length];
            //this.boundingRect = new System.Drawing.RectangleF(
            //      (float)rect.X,
            //       (float)rect.Y,
            //       (float)rect.Width,
            //       (float)rect.Height);

            if (scanOperatorResult != null)
            {
                float offsetX = scanOperatorResult.CanvasAxisPosition.Position[0] / Operator.ResizeRatio;
                float offsetY = scanOperatorResult.CanvasAxisPosition.Position[1] / Operator.ResizeRatio;
                for (int i = 0; i < points.Length; i++)
                    points[i].Offset(offsetX, offsetY);
                //boundingRect.Offset(offsetX, offsetY);
            }

            Array.Copy(points, rotateRectPointList, points.Length);
        }
        
        public Shape GetShape()
        {
            Shape shape = null;
            if (rotateRectPointList.Length == 2)
            {
                Line lineShape = new Line();

                lineShape.X1 = rotateRectPointList[0].X;
                lineShape.Y1 = rotateRectPointList[0].Y;
                lineShape.X2 = rotateRectPointList[1].X;
                lineShape.Y2 = rotateRectPointList[1].Y;
                shape = lineShape;
            }
            else //if(rotateRectPointList.Length == 4)
            {
                Polygon polygon = new Polygon();

                polygon.Points = new PointCollection();
                Array.ForEach(rotateRectPointList, f => polygon.Points.Add(f));
                shape = polygon;
            }

            Brush brush = this.defect.GetBrush();
            shape.Fill = brush;
            shape.Stroke = brush;
            return shape;
        }

        public Rect GetRect(int inflate=0)
        {
            if (this.rotateRectPointList.Length == 0)
                return Rect.Empty;

            Point pt0 = this.rotateRectPointList[0];
            Rect rect = new Rect(pt0.X, pt0.Y, 0, 0);
            Array.ForEach(this.rotateRectPointList, f => rect = Rect.Union(rect, f));
            rect.Inflate(inflate, inflate);
            return rect;
        }
    }

    public delegate void CombineCompletedEventHandler(List<CanvasDefect> canvasDefectList);
    public delegate void CombineClearEventHandler();

    public class ResultCombiner : INotifyPropertyChanged
    {
        //List<CanvasDefect>[] canvasDefectListArray;
        List<CanvasDefect> overallCanvasDefectList;

        LightTuneResult lightTuneResult;
        ObservableCollection<CanvasDefect> combineDefectList = new ObservableCollection<CanvasDefect>();
        ScanOperatorResult[] scanOperatorResultArray;
        ExtractOperatorResult[] extractOperatorResultArray;

        StoringOperator storingOperator = new StoringOperator();
        public StoringOperator StoringOperator { get => storingOperator; }

        public ScanOperatorResult[] ScanOperatorResultArray { get => scanOperatorResultArray; }
        public ExtractOperatorResult[] ExtractOperatorResultArray { get => extractOperatorResultArray; }
        public ObservableCollection<CanvasDefect> CombineDefectList { get => combineDefectList; }
        public LightTuneResult LightTuneResult { get => lightTuneResult; }

        public event CombineClearEventHandler CombineClear;
        public event CombineCompletedEventHandler CombineCompleted;
        public event PropertyChangedEventHandler PropertyChanged;

        public ResultCombiner()
        {
            scanOperatorResultArray = new ScanOperatorResult[DeveloperSettings.Instance.ScanNum];
            extractOperatorResultArray = new ExtractOperatorResult[DeveloperSettings.Instance.ScanNum];
            overallCanvasDefectList = new List<CanvasDefect>();
            //canvasDefectListArray = new List<CanvasDefect>[DeveloperSettings.Instance.ScanNum];

            //for (int i = 0; i < DeveloperSettings.Instance.ScanNum; i++)
            //    canvasDefectListArray[i] = new List<CanvasDefect>();
        }
        
        public void AddResult(OperatorResult operatorResult)
        {
            switch (operatorResult.Type)
            {
                case ResultType.LightTune:
                    lightTuneResult = operatorResult as LightTuneResult;
                    OnPropertyChanged("LightTuneResult");
                    break;
                case ResultType.Scan:
                    scanOperatorResultArray[((ScanOperatorResult)operatorResult).FlowPosition] = operatorResult as ScanOperatorResult;
                    OnPropertyChanged("ScanOperatorResultArray");
                    break;
                case ResultType.Extract:
                    extractOperatorResultArray[((ExtractOperatorResult)operatorResult).ScanOperatorResult.FlowPosition] = operatorResult as ExtractOperatorResult;
                    OnPropertyChanged("ExtractOperatorResultArray");
                    break;
                case ResultType.Inspect:
                    InspectOperatorResult inspectOperatorResult = (InspectOperatorResult)operatorResult;
                    List<CanvasDefect> list = RemoveIntersectDefect(inspectOperatorResult);
                    this.overallCanvasDefectList.AddRange(list);
                    //if (list.Count > 500)
                    //{
                    //    list.Sort((f, g) => g.Defect.DefectBlob.Area.CompareTo(f.Defect.DefectBlob.Area));
                    //    list.RemoveRange(500, list.Count - 500);
                    //}

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Array temp = list.ToArray();

                        lock (combineDefectList)
                        {
                            foreach (CanvasDefect canvasDefect in temp)
                                combineDefectList.Add(canvasDefect);
                        }

                        OnPropertyChanged("CombineDefectList");
                    }));

                    CombineCompleted(list);
                    break;

                //InspectOperatorResult inspectOperatorResult = (InspectOperatorResult)operatorResult;
                //List<CanvasDefect> canvasDefectList = RemoveIntersectDefect(inspectOperatorResult);

                //overallCanvasDefectList.AddRange(canvasDefectList);
                //Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                //{
                //    combineDefectList = new ObservableCollection<CanvasDefect>(overallCanvasDefectList);
                //    OnPropertyChanged("CombineDefectList");
                //}));

                //CombineCompleted(canvasDefectList);
                //break;
                case ResultType.Train:
                    break;
            }
        }

        public void SaveInspectOperatorResult()
        {
            List<ExtractOperatorResult> extractOperatorResultList = this.extractOperatorResultArray.Cast<ExtractOperatorResult>().ToList();
            List<CanvasDefect> canvasDefectList = new List<CanvasDefect>(this.overallCanvasDefectList);
            storingOperator.Save(extractOperatorResultList, canvasDefectList);
        }

        private List<CanvasDefect> RemoveIntersectDefect(InspectOperatorResult inspectOperatorResult)
        {
            int? currentPosition = inspectOperatorResult.ExtractOperatorResult?.ScanOperatorResult?.FlowPosition;
            //int prevPosition = currentPosition - 1;
            //int nextPosition = currentPosition + 1;

            List<CanvasDefect> temp = inspectOperatorResult.DefectList.ConvertAll(f => new CanvasDefect(f, inspectOperatorResult.ExtractOperatorResult?.ScanOperatorResult));
            return temp;
            //List<CanvasDefect> temp = new List<CanvasDefect>();
            //foreach (Defect defect in inspectOperatorResult.DefectList)
            //    temp.Add(new CanvasDefect(defect, inspectOperatorResult.ExtractOperatorResult.ScanOperatorResult));
            //temp.AddRange(.ToArray());

            //if (prevPosition >= 0)
            //    RemoveIntersectDefect(inspectOperatorResult.DefectList, inspectOperatorResult.ExtractOperatorResult.ScanOperatorResult, prevPosition, ref temp);

            //if (nextPosition < DeveloperSettings.Instance.ScanNum)
            //    RemoveIntersectDefect(inspectOperatorResult.DefectList, inspectOperatorResult.ExtractOperatorResult.ScanOperatorResult, nextPosition, ref temp);

            //lock (canvasDefectListArray[currentPosition])
            //    canvasDefectListArray[currentPosition].AddRange(temp);
        }

        private void RemoveIntersectDefect(int position, ref List<CanvasDefect> temp)
        {

            //temp.RemoveAll(canvasDefectListArray[position].Any(intersectDefect => defect.BoundingRect.IntersectsWith(intersectDefect.BoundingRect)));
            //foreach (CanvasDefect defect in temp)
            //{
            //    if ()
            //    {

            //        continue;
            //    }
            //        continue;

            //    temp.Add(canvasDefect);
            //}
        }

        public void Clear(bool clearImage = true)
        {
            if (clearImage == true)
            {
                for (int i = 0; i < DeveloperSettings.Instance.ScanNum; i++)
                    scanOperatorResultArray[i] = null;

                lightTuneResult = null;
            }

            overallCanvasDefectList.Clear();

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                combineDefectList.Clear();
            }));

            CombineClear();
            
            OnPropertyChanged("ScanOperatorResultArray");
            OnPropertyChanged("LightTuneResult");
            OnPropertyChanged("CombineDefectList");
        }

        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}