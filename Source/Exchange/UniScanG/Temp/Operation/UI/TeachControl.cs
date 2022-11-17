//using System;
//using System.Collections.Generic;
//using System.Windows.Forms;
//using DynMvp.Data;
//using DynMvp.Data.UI;

//namespace UniScanG.Temp
//{
//    public delegate void DrawSearchRangeFigureDelegate(FiducialPattern fiducialPattern);

//    public partial class TeachControl : UserControl
//    {
//        //ITeachingPageController pageController;
//        TeachHandlerProbe teachHandlerProbe;
//        TeachBox teachBox;
//        bool onUpdatePatternList = false;

//        public TeachControl(/*ITeachingPageController pageController, */TeachHandlerProbe teachHandlerProbe, TeachBox teachBox, DrawSearchRangeFigureDelegate drawSearchRangeFigureDelegate)
//        {
//            InitializeComponent();

//            //this.pageController = pageController;
//            this.teachHandlerProbe = teachHandlerProbe;
//            this.teachBox = teachBox;
//        }

//        private SheetCheckerParam GetParam()
//        {
//            List<Probe> probeList = teachHandlerProbe.GetSelectedProbe();

//            if (probeList.Count == 0)
//                return null;

//            VisionProbe curProbe = (VisionProbe)probeList[0];

//            if (curProbe == null || curProbe.InspAlgorithm == null)
//                return null;

//            //GravureSheetChecker sheetChecker = (GravureSheetChecker)curProbe.InspAlgorithm;

//            return null;//(SheetCheckerParam)sheetChecker.Param;
//        }

//        delegate void UpdateDataDelegate();
//        public void UpdateData()
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new UpdateDataDelegate(UpdateData));
//                return;
//            }
            
//            SheetCheckerParam param = GetParam();
//            if (param == null)
//                return;

//            regionImage.Image = param.TrainerParam.InspectRegionInfoImage?.ToBitmap();

//            UpdatePatternImageSelector();
//        }

//        public void TabPageVisibleChanged(bool visibleFlag)
//        {

//        }

//        delegate void UpdatePatternImageSelectorDelegate();
//        private void UpdatePatternImageSelector()
//        {
//            if (InvokeRequired)
//            {
//                Invoke(new UpdatePatternImageSelectorDelegate(UpdatePatternImageSelector));
//                return;
//            }

//            SheetCheckerParam param = GetParam();
//            if (param == null)
//                return;

//            patternImageSelector.Rows.Clear();
//            onUpdatePatternList = true;

//            foreach (SheetPattern pattern in param.TrainerParam.PatternList)
//            {
//                string infoStr = string.Format("Area : {0:.0}\n\rWidth : {1:.0}\n\rHeight : {2:.0}\n\r", pattern.BlobRect.Area,
//                        pattern.BlobRect.BoundingRect.Width, pattern.BlobRect.BoundingRect.Height);

//                int index = patternImageSelector.Rows.Add(pattern.BitmapImage, pattern.Type == SheetPatternType.Care, pattern.Count, infoStr);
//                patternImageSelector.Rows[index].Tag = pattern;
//                if (pattern.BitmapImage.Width * 3 < pattern.BitmapImage.Height)
//                {
//                    patternImageSelector.Rows[index].Height = patternImageSelector.RowTemplate.Height * 3;
//                }
//            }

//            onUpdatePatternList = false;

//            patternImageSelector.Sort(patternImageSelector.Columns[2], System.ComponentModel.ListSortDirection.Descending);
//        }

//        private void patternImageSelector_SelectionChanged(object sender, EventArgs e)
//        {
//            if (onUpdatePatternList == true)
//                return;

//            if (patternImageSelector.SelectedRows.Count > 0)
//            {
//                int rowIndex = patternImageSelector.SelectedRows[0].Index;
//                SheetPattern sheetPattern = (SheetPattern)patternImageSelector.Rows[rowIndex].Tag;

//                if (sheetPattern != null)
//                {
//                    if (sheetPattern.PatternGroup != null)
//                    {
//                        teachBox.DrawBox.FigureGroup = sheetPattern.PatternGroup.CreateFigureGroup();
//                        teachBox.Invalidate(true);
//                        teachBox.Update();
//                    }
//                }
//            }
//        }
//    }
//}
