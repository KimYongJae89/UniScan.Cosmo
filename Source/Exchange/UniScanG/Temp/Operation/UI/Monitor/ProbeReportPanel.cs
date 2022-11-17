//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Windows.Forms;
//using DynMvp.Base;
//using DynMvp.Data.UI;
//using DynMvp.UI;
//using System.IO;
//using System.Linq;
////using UniScanG.Algorithms;
//using UniEye.Base.UI;

//namespace UniScanG.Temp
//{
//    public partial class ProbeReportPanel : UserControl, IReportPanel
//    {
//        DefectFilterType[] defectFilter;

//        bool onUpdateData = false;
        
//        private DrawBox camResultView1;
//        private DrawBox camResultView2;
        
//        List<KeyValuePair<int, int>>[] defectNumList;
//        List<KeyValuePair<int, int>>[] defectBlackNumList;
//        List<KeyValuePair<int, int>>[] defectWhiteNumList;
//        List<KeyValuePair<int, int>>[] defectShapeNumList;
//        List<KeyValuePair<int, int>>[] defectPinHoleNumList;

//        List<DirectoryInfo>[] directoryList = null;
//        List<DataGridViewRow>[] rowList = null;

//        public ProbeReportPanel()
//        {
//            InitializeComponent();
            
//            camResultView1 = new DrawBox();
//            camResultView2 = new DrawBox();

//            this.cam1ResultLeftPanel.Controls.Add(this.camResultView1);

//            this.camResultView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.camResultView1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.camResultView1.Location = new System.Drawing.Point(3, 3);
//            this.camResultView1.Name = "camResultView1";
//            this.camResultView1.Size = new System.Drawing.Size(409, 523);
//            this.camResultView1.TabIndex = 8;
//            this.camResultView1.TabStop = false;
//            this.camResultView1.Enable = false;
//            this.camResultView1.pictureBox.Dock = DockStyle.Fill;
            
//            this.cam2ResultLeftPanel.Controls.Add(this.camResultView2);

//            this.camResultView2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.camResultView2.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.camResultView2.Location = new System.Drawing.Point(3, 3);
//            this.camResultView2.Name = "camResultView2";
//            this.camResultView2.Size = new System.Drawing.Size(409, 523);
//            this.camResultView2.TabIndex = 8;
//            this.camResultView2.TabStop = false;
//            this.camResultView2.Enable = false;
//            this.camResultView2.pictureBox.Dock = DockStyle.Fill;
            
//            buttonOpenDirectory1.Enabled = false;
//            buttonOpenDirectory2.Enabled = false;

//            defectFilter = new DefectFilterType[2];

//            directoryList = new List<DirectoryInfo>[2];
//            rowList = new List<DataGridViewRow>[2];

//            defectNumList = new List<KeyValuePair<int, int>>[2];
//            defectBlackNumList = new List<KeyValuePair<int, int>>[2];
//            defectWhiteNumList = new List<KeyValuePair<int, int>>[2];
//            defectPinHoleNumList = new List<KeyValuePair<int, int>>[2];
//            defectShapeNumList = new List<KeyValuePair<int, int>>[2];
//        }

//        public void UpdateInfo(string resultPath, Object hintObj)
//        {
//            onUpdateData = true;

//            defectFilter[0] = DefectFilterType.Total;
//            defectFilter[1] = DefectFilterType.Total;
            
//            List<string> updateInfoHint = (List<string>)hintObj;

//            modelName.Text = updateInfoHint[0];
//            lotNo.Text = updateInfoHint[1];

//            DirectoryInfo directoryInfo1 = new DirectoryInfo(updateInfoHint[2]);
//            DirectoryInfo directoryInfo2 = new DirectoryInfo(updateInfoHint[3]);
            
//            rowList[0] = new List<DataGridViewRow>();
            
//            DynMvp.UI.Touch.SimpleProgressForm loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Cam1 Result Loding..");
            
//            loadingForm.Show(new Action(() =>
//            {
//                directoryList[0] = new List<DirectoryInfo>(directoryInfo1.GetDirectories());
//                defectNumList[0] = GetDefectNumList(directoryInfo1.FullName, DefectFilterType.Total);
//                defectBlackNumList[0] = GetDefectNumList(directoryInfo1.FullName, DefectFilterType.Black);
//                defectWhiteNumList[0] = GetDefectNumList(directoryInfo1.FullName, DefectFilterType.White);
//                defectPinHoleNumList[0] = GetDefectNumList(directoryInfo1.FullName, DefectFilterType.PinHole);
//                defectShapeNumList[0] = GetDefectNumList(directoryInfo1.FullName, DefectFilterType.Shape);

//                DefectFiltering(0);
//            }));

//            UpdateProductionInfo(0);

//            rowList[1] = new List<DataGridViewRow>();
//            loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Cam2 Result Loding..");
//            loadingForm.Show(new Action(() =>
//            {
//                directoryList[1] = new List<DirectoryInfo>(directoryInfo2.GetDirectories());
//                defectNumList[1] = GetDefectNumList(directoryInfo2.FullName, DefectFilterType.Total);
//                defectBlackNumList[1] = GetDefectNumList(directoryInfo2.FullName, DefectFilterType.Black);
//                defectWhiteNumList[1] = GetDefectNumList(directoryInfo2.FullName, DefectFilterType.White);
//                defectPinHoleNumList[1] = GetDefectNumList(directoryInfo2.FullName, DefectFilterType.PinHole);
//                defectShapeNumList[1] = GetDefectNumList(directoryInfo2.FullName, DefectFilterType.Shape);

//                DefectFiltering(1);
//            }));

//            UpdateProductionInfo(1);

//            UpdateSheetList1();
//            UpdateSheetList2();

//            onUpdateData = false;

//            SelectSheetList(0);
//            SelectSheetList(1);
//        }

//        private void UpdateProductionInfo(int camIndex)
//        {
//            if (directoryList[camIndex] == null)
//                return;

//            List<KeyValuePair<int, int>> tempDefectNumList = null;
//            switch (defectFilter[camIndex])
//            {
//                case DefectFilterType.Total:
//                    tempDefectNumList = defectNumList[camIndex];
//                    break;
//                case DefectFilterType.Black:
//                    tempDefectNumList = defectBlackNumList[camIndex];
//                    break;
//                case DefectFilterType.White:
//                    tempDefectNumList = defectWhiteNumList[camIndex];
//                    break;
//                case DefectFilterType.PinHole:
//                    tempDefectNumList = defectPinHoleNumList[camIndex];
//                    break;
//                case DefectFilterType.Shape:
//                    tempDefectNumList = defectShapeNumList[camIndex];
//                    break;
//            }

//            int ngNum = 0;
//            foreach (DataGridViewRow row in rowList[camIndex])
//            {
//                if ((bool)row.Cells[1].Tag == false)
//                    ngNum++;
//            }

//            if (camIndex == 0)
//            {
//                productionIndex1.Text = directoryList[camIndex].Count.ToString();
                
//                productionNg1.Text = ngNum.ToString();//tempDefectNumList == null ? "0" : tempDefectNumList.Count.ToString();
//                float ratio1 = ((float)(tempDefectNumList == null ? 0 : ngNum) / (float)directoryList[0].Count) * 100.0f;
//                productionRatio1.Text = string.Format("{0:0.00} %", ratio1);
//            }
//            else
//            {
//                productionNg2.Text = ngNum.ToString();//tempDefectNumList == null ? "0" : tempDefectNumList.Count.ToString();
//                productionIndex2.Text = directoryList[1].Count.ToString();
//                float ratio2 = ((float)(tempDefectNumList == null ? 0 : ngNum) / (float)directoryList[1].Count) * 100.0f;
//                productionRatio2.Text = string.Format("{0:0.00} %", ratio2);
//            }
//        }

//        private void UpdateSheetList1()
//        {
//            if (rowList[0] == null)
//                return;

//            camResultView1.UpdateImage(null);
//            inspector1Image.Image = null;
//            defectListCam1.Rows.Clear();
//            sheetList1.Rows.Clear();

//            foreach (DataGridViewRow row in rowList[0])
//            {
//                if (filterOK1.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == true)
//                        sheetList1.Rows.Add(row);
//                }

//                if (filterNG1.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == false)
//                        sheetList1.Rows.Add(row);
//                }
//            }
//        }

//        private void UpdateSheetList2()
//        {
//            if (rowList[1] == null)
//                return;

//            camResultView2.UpdateImage(null);
//            inspector2Image.Image = null;
//            defectListCam2.Rows.Clear();
//            sheetList2.Rows.Clear();
            
//            foreach (DataGridViewRow row in rowList[1])
//            {
//                if (filterOK2.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == true)
//                        sheetList2.Rows.Add(row);
//                }

//                if (filterNG2.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == false)
//                        sheetList2.Rows.Add(row);
//                }
//            }
//        }

//        private void DefectFiltering(int camIndex)
//        {
//            if (directoryList[camIndex] == null)
//                return;

//            rowList[camIndex].Clear();

//            foreach (DirectoryInfo dir in directoryList[camIndex])
//            {
//                string name = dir.Name;
//                string[] names = name.Split('_');
//                string sheetIndex = null;
//                if (names.Count() == 1)
//                    sheetIndex = names[0];
//                else
//                    sheetIndex = names[1];

//                DataGridViewRow row = new DataGridViewRow();
//                DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();
//                DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
//                cell1.Value = sheetIndex;

//                bool result = true;

//                List<KeyValuePair<int, int>> tempDefectNumList = null;

//                switch(defectFilter[camIndex])
//                {
//                    case DefectFilterType.Total:
//                        tempDefectNumList = defectNumList[camIndex];
//                        break;
//                    case DefectFilterType.Black:
//                        tempDefectNumList = defectBlackNumList[camIndex];
//                        break;
//                    case DefectFilterType.White:
//                        tempDefectNumList = defectWhiteNumList[camIndex];
//                        break;
//                    case DefectFilterType.PinHole:
//                        tempDefectNumList = defectPinHoleNumList[camIndex];
//                        break;
//                    case DefectFilterType.Shape:
//                        tempDefectNumList = defectShapeNumList[camIndex];
//                        break;
//                }

//                if (tempDefectNumList != null)
//                {
//                    int defectNum = tempDefectNumList.Find(x => x.Key == int.Parse(sheetIndex)).Value;
//                    cell2.Value = defectNum;

//                    if (defectNum > 0)
//                        result = false;
//                }

//                cell2.Tag = result;
//                row.Cells.AddRange(cell1, cell2);
//                row.Tag = dir.FullName;

//                if (result == true)
//                    row.DefaultCellStyle.BackColor = Color.LightGreen;
//                else
//                    row.DefaultCellStyle.BackColor = Color.Red;

//                rowList[camIndex].Add(row);
//            }
//        }

//        private List<KeyValuePair<int, int>> GetDefectNumList(string path, DefectFilterType type)
//        {
//            string resultPath = Path.Combine(path, "OverView.csv");
//            if (File.Exists(resultPath) == false)
//                return null;

//            List<KeyValuePair<int, int>> defectNumList = new List<KeyValuePair<int, int>>();
//            string[] lines = File.ReadAllLines(resultPath, System.Text.Encoding.Default); ;
            
//            foreach (string line in lines)
//            {
//                string[] words = line.Split(new char[] { ';' });

//                switch(type)
//                {
//                    case DefectFilterType.Total:
//                        if (words.Count() < 2)
//                            continue;
//                        if (string.IsNullOrEmpty(words[0]) == false && string.IsNullOrEmpty(words[1]) == false)
//                            defectNumList.Add(new KeyValuePair<int, int>(int.Parse(words[0]), int.Parse(words[1])));
//                        break;
//                    case DefectFilterType.Black:
//                        if (words.Count() < 3)
//                            continue;
//                        if (string.IsNullOrEmpty(words[0]) == false && string.IsNullOrEmpty(words[2]) == false)
//                            defectNumList.Add(new KeyValuePair<int, int>(int.Parse(words[0]), int.Parse(words[2])));
//                        break;
//                    case DefectFilterType.White:
//                        if (words.Count() < 4)
//                            continue;
//                        if (string.IsNullOrEmpty(words[0]) == false && string.IsNullOrEmpty(words[3]) == false)
//                            defectNumList.Add(new KeyValuePair<int, int>(int.Parse(words[0]), int.Parse(words[3])));
//                        break;
//                    case DefectFilterType.Shape:
//                        if (words.Count() < 5)
//                            continue;
//                        if (string.IsNullOrEmpty(words[0]) == false && string.IsNullOrEmpty(words[4]) == false)
//                            defectNumList.Add(new KeyValuePair<int, int>(int.Parse(words[0]), int.Parse(words[4])));
//                        break;
//                    case DefectFilterType.PinHole:
//                        if (words.Count() < 6)
//                            continue;
//                        if (string.IsNullOrEmpty(words[0]) == false && string.IsNullOrEmpty(words[5]) == false)
//                            defectNumList.Add(new KeyValuePair<int, int>(int.Parse(words[0]), int.Parse(words[5])));
//                        break;
//                }   
//            }
//            return defectNumList;
//        }

//        private List<SheetCheckerSubResult> GetSubResultList(string path)
//        {
//            List<SheetCheckerSubResult> resultList = new List<SheetCheckerSubResult>(); ;

//            string resultPath = Path.Combine(path, "result.csv");
//            if (File.Exists(resultPath) == false)
//                return resultList;

//            using (StreamReader sr = File.OpenText(resultPath))
//            {
//                string lineStr;
//                int readLines = 0;

//                string result = null;
//                string time = null;

//                while ((lineStr = sr.ReadLine()) != null)
//                {
//                    readLines++;

//                    string[] words = lineStr.Split(new char[] { ';' });
//                    if (words.Length < 2)
//                        continue;

//                    if (readLines == 1)
//                    {
//                        result = words[1];
//                        time = words[2];
//                    }
//                    else
//                    {
//                        if (words.Length < 6)
//                            continue;

//                        float scale = 0.1f;

//                        string defectIndex = words[0].Trim();
//                        string defectType = words[1].Trim();
//                        RectangleF rect = new RectangleF(float.Parse(words[2]), float.Parse(words[3]), float.Parse(words[4]), float.Parse(words[5]));
//                        rect.X *= scale;
//                        rect.Y *= scale;
//                        rect.Width *= scale;
//                        rect.Height *= scale;

//                        SheetCheckerSubResult sheetCheckerSubResult = new SheetCheckerSubResult();

//                        sheetCheckerSubResult.DefectBlob = new DynMvp.Vision.BlobRect();
//                        sheetCheckerSubResult.DefectBlob.BoundingRect = rect;

//                        switch (defectType)
//                        {
//                            case "BlackDefect":
//                                sheetCheckerSubResult.DefectType = SheetDefectType.BlackDefect;
//                                break;
//                            case "WhiteDefect":
//                                sheetCheckerSubResult.DefectType = SheetDefectType.WhiteDefect;
//                                break;
//                        }

//                        resultList.Add(sheetCheckerSubResult);
//                    }
//                }
//            }

//            return resultList;
//        }

//        private void ShowResult(int camIndex, string path)
//        {
//            onUpdateData = true;

//            int blackDefectNum1 = 0;
//            int whiteDefectNum1 = 0;
//            int shapeNum1 = 0;
//            int pinHoleNum1 = 0;

//            int blackDefectNum2 = 0;
//            int whiteDefectNum2 = 0;
//            int shapeNum2 = 0;
//            int pinHoleNum2 = 0;

//            string resultImagePath = Path.Combine(path, "WholeImage.jpg");

//            if (camIndex == 0)
//                camResultView1.UpdateImage((Bitmap)ImageHelper.LoadImage(resultImagePath));
//            else
//                camResultView2.UpdateImage((Bitmap)ImageHelper.LoadImage(resultImagePath));

//            List<SheetCheckerSubResult> resultList = GetSubResultList(path);

//            int defectIndex = 1;
            
//            foreach (SheetCheckerSubResult result in resultList)
//            {
//                if (camIndex == 0)
//                {
//                    string defectType = null;

//                    switch (result.DefectType)
//                    {
//                        case SheetDefectType.BlackDefect:
//                            if (defectFilter[0] != DefectFilterType.Total && defectFilter[0] != DefectFilterType.Black)
//                                continue;
//                            blackDefectNum1++;
//                            defectType = "전극";
//                            break;
//                        case SheetDefectType.WhiteDefect:
//                            if (defectFilter[0] != DefectFilterType.Total && defectFilter[0] != DefectFilterType.White)
//                                continue;
//                            whiteDefectNum1++;
//                            defectType = "성형";
//                            break;
//                    }

//                    int rowIndex = defectListCam1.Rows.Add(defectIndex, defectType);
                    
//                    defectListCam1.Rows[rowIndex].Tag = result.DefectBlob.BoundingRect;
//                    defectListCam1.Rows[rowIndex].Cells[1].Tag = result.DefectType;
//                }
//                else
//                {
//                    string defectType = null;
//                    switch (result.DefectType)
//                    {
//                        case SheetDefectType.BlackDefect:
//                            if (defectFilter[1] != DefectFilterType.Total && defectFilter[1] != DefectFilterType.Black)
//                                continue;
//                            blackDefectNum2++;
//                            defectType = "전극";
//                            break;
//                        case SheetDefectType.WhiteDefect:
//                            if (defectFilter[1] != DefectFilterType.Total && defectFilter[1] != DefectFilterType.White)
//                                continue;
//                            whiteDefectNum2++;
//                            defectType = "성형";
//                            break;
//                    }

//                    int rowIndex = defectListCam2.Rows.Add(defectIndex, defectType);
//                    defectListCam2.Rows[rowIndex].Tag = result.DefectBlob.BoundingRect;

//                    defectListCam2.Rows[rowIndex].Cells[1].Tag = result.DefectType;
//                }

//                defectIndex++;
//            }
                            
//            if (camIndex == 0)
//            {
//                cam1BalckDefect.Text = blackDefectNum1.ToString();
//                cam1WhiteDefect.Text = whiteDefectNum1.ToString();
//                cam1Shape.Text = shapeNum1.ToString();
//                cam1PinHole.Text = pinHoleNum1.ToString();
//                defectListCam1DrawFigure(true);
//            }
//            else
//            {
//                cam2BalckDefect.Text = blackDefectNum2.ToString();
//                cam2WhiteDefect.Text = whiteDefectNum2.ToString();
//                cam2Shape.Text = shapeNum2.ToString();
//                cam2PinHole.Text = pinHoleNum2.ToString();
//                defectListCam2DrawFigure(true);
//            }

//            onUpdateData = false;

//            if (camIndex == 0)
//                DefectImageView1();
//            else
//                DefectImageView2();
//        }

//        private void SelectSheetList(int Index)
//        {
//            buttonOpenDirectory1.Enabled = true;
//            buttonOpenDirectory2.Enabled = true;

//            if (Index == 0)
//            {
//                if (sheetList1.SelectedRows.Count == 0)
//                    return;

//                defectListCam1.Rows.Clear();
//                camResultView1.UpdateImage(null);
//                inspector1Image.Image = null;

//                ShowResult(0, (string)sheetList1.SelectedRows[0].Tag);
//            }
//            else
//            {
//                if (sheetList2.SelectedRows.Count == 0)
//                    return;

//                defectListCam2.Rows.Clear();
//                camResultView2.UpdateImage(null);
//                inspector2Image.Image = null;

//                ShowResult(1, (string)sheetList2.SelectedRows[0].Tag);
//            }
//        }

//        private void defectListCam1DrawFigure(bool whole = true)
//        {
//            FigureGroup figureGroup = new FigureGroup();

//            foreach (DataGridViewRow row in defectListCam1.Rows)
//            {
//                RectangleF rect = (RectangleF)row.Tag;
//                switch ((SheetDefectType)row.Cells[1].Tag)
//                {
//                    case SheetDefectType.BlackDefect:
//                        rect.Inflate(25, 25);
//                        figureGroup.AddFigure(new CrossFigure(rect, new Pen(Color.Red, 3)));
//                        break;
//                    case SheetDefectType.WhiteDefect:
//                        rect.Inflate(25, 25);
//                        figureGroup.AddFigure(new EllipseFigure(rect, new Pen(Color.Yellow, 3)));
//                        break;
//                }
//            }

//            if (whole == false)
//            {
//                foreach (DataGridViewRow row in defectListCam1.SelectedRows)
//                {
//                    RectangleF rect = (RectangleF)row.Tag;
//                    rect.Inflate(50, 50);
//                    figureGroup.AddFigure(new EllipseFigure(rect, new Pen(Color.White, 3)));
//                }
//            }

//            camResultView1.TempFigureGroup = figureGroup;
//            camResultView1.Invalidate();
//            camResultView1.Update();
//        }

//        private void defectListCam2DrawFigure(bool whole = true)
//        {
//            FigureGroup figureGroup = new FigureGroup();

//            foreach (DataGridViewRow row in defectListCam2.Rows)
//            {
//                RectangleF rect = (RectangleF)row.Tag;
//                switch ((SheetDefectType)row.Cells[1].Tag)
//                {
//                    case SheetDefectType.BlackDefect:
//                        rect.Inflate(25, 25);
//                        figureGroup.AddFigure(new CrossFigure(rect, new Pen(Color.Red, 3)));
//                        break;
//                    case SheetDefectType.WhiteDefect:
//                        rect.Inflate(25, 25);
//                        figureGroup.AddFigure(new EllipseFigure(rect, new Pen(Color.Yellow, 3)));
//                        break;
//                }
//            }

//            if (whole == false)
//            {
//                foreach (DataGridViewRow row in defectListCam2.SelectedRows)
//                {
//                    RectangleF rect = (RectangleF)row.Tag;
//                    rect.Inflate(50, 50);
//                    figureGroup.AddFigure(new EllipseFigure(rect, new Pen(Color.White, 3)));
//                }
//            }
//            camResultView2.TempFigureGroup = figureGroup;
//            camResultView2.Invalidate();
//            camResultView2.Update();
//        }

        

//        private void defectListCam1_SelectionChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            DefectImageView1();
//        }

//        private void DefectImageView1()
//        {
//            if (defectListCam1.SelectedRows.Count == 0)
//                return;

//            if (sheetList1.SelectedRows.Count == 0)
//                return;

//            string defectImagePath = Path.Combine((string)sheetList1.SelectedRows[0].Tag, defectListCam1.SelectedRows[0].Cells[0].Value.ToString() + ".Bmp");
//            inspector1Image.Image = ImageHelper.LoadImage(defectImagePath);
//            defectListCam1DrawFigure(false);
//        }

//        private void DefectImageView2()
//        {
//            if (defectListCam2.SelectedRows.Count == 0)
//                return;

//            if (sheetList2.SelectedRows.Count == 0)
//                return;

//            string defectImagePath = Path.Combine((string)sheetList2.SelectedRows[0].Tag, defectListCam2.SelectedRows[0].Cells[0].Value.ToString() + ".Bmp");
//            inspector2Image.Image = ImageHelper.LoadImage(defectImagePath);
//            defectListCam2DrawFigure(false);
//        }

//        private void defectListCam2_SelectionChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            DefectImageView2();
//        }

//        private void sheetList2_SelectionChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            DefectImageView2();
//            if (sheetList2.SelectedRows.Count < 2)
//            {
//                SelectSheetList(1);   
//            }
//        }

//        private void sheetList1_SelectionChanged(object sender, EventArgs e)
//        {
//            if (onUpdateData == true)
//                return;

//            DefectImageView1();
//            if (sheetList1.SelectedRows.Count < 2)
//            {
//                SelectSheetList(0);
//            }
//        }

//        public void Clear()
//        {
//            sheetList1.Rows.Clear();
//            sheetList2.Rows.Clear();
//            defectListCam1.Rows.Clear();
//            defectListCam2.Rows.Clear();
//            camResultView1.UpdateImage(null);
//            camResultView2.UpdateImage(null);
//            inspector1Image.Image = null;
//            inspector2Image.Image = null;
//        }

//        private void buttonOpenDirectory1_Click(object sender, EventArgs e)
//        {
//            if (sheetList1.SelectedRows.Count != 1)
//                return;

//            System.Diagnostics.Process.Start(sheetList1.SelectedRows[0].Tag.ToString());
//        }

//        private void buttonOpenDirectory2_Click(object sender, EventArgs e)
//        {
//            if (sheetList2.SelectedRows.Count != 1)
//                return;

//            System.Diagnostics.Process.Start(sheetList2.SelectedRows[0].Tag.ToString());
//        }

//        private void slectAllButton1_Click(object sender, EventArgs e)
//        {
//            sheetList1.SelectAll();
//        }

//        private void slectAllButton2_Click(object sender, EventArgs e)
//        {
//            sheetList2.SelectAll();
//        }
        
//        private void filterOK1_CheckedChanged(object sender, EventArgs e)
//        {
//            sheetList1.Rows.Clear();

//            foreach (DataGridViewRow row in rowList[0])
//            {
//                if (filterOK1.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == true)
//                        sheetList1.Rows.Add(row);
//                }

//                if (filterNG1.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == false)
//                        sheetList1.Rows.Add(row);
//                }
//            }
//        }

//        private void filterNG1_CheckedChanged(object sender, EventArgs e)
//        {
//            sheetList1.Rows.Clear();

//            foreach (DataGridViewRow row in rowList[0])
//            {
//                if (filterOK1.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == true)
//                        sheetList1.Rows.Add(row);
//                }

//                if (filterNG1.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == false)
//                        sheetList1.Rows.Add(row);
//                }
//            }
//        }

//        private void filterOK2_CheckedChanged(object sender, EventArgs e)
//        {
//            sheetList2.Rows.Clear();

//            foreach (DataGridViewRow row in rowList[1])
//            {
//                if (filterOK2.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == true)
//                        sheetList2.Rows.Add(row);
//                }

//                if (filterNG2.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == false)
//                        sheetList2.Rows.Add(row);
//                }
//            }
//        }

//        private void filterNG2_CheckedChanged(object sender, EventArgs e)
//        {
//            sheetList2.Rows.Clear();

//            foreach (DataGridViewRow row in rowList[1])
//            {
//                if (filterOK2.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == true)
//                        sheetList2.Rows.Add(row);
//                }

//                if (filterNG2.Checked == true)
//                {
//                    if ((bool)row.Cells[1].Tag == false)
//                        sheetList2.Rows.Add(row);
//                }
//            }
//        }

//        private void radioTotal1_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                defectFilter[0] = DefectFilterType.Total;
//                DynMvp.UI.Touch.SimpleProgressForm loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Filtering...");
//                loadingForm.Show(new Action(() =>
//                {
//                    DefectFiltering(0);
//                }));
//                UpdateSheetList1();
//                UpdateProductionInfo(0);
//            }
//        }

//        private void radioBlack1_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                defectFilter[0] = DefectFilterType.Black;
//                DynMvp.UI.Touch.SimpleProgressForm loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Filtering...");
//                loadingForm.Show(new Action(() =>
//                {
//                    DefectFiltering(0);
//                }));
//                UpdateSheetList1();
//                UpdateProductionInfo(0);
//            }
//        }

//        private void radioWhite1_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                defectFilter[0] = DefectFilterType.White;
//                DynMvp.UI.Touch.SimpleProgressForm loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Filtering...");
//                loadingForm.Show(new Action(() =>
//                {
//                    DefectFiltering(0);
//                }));
//                UpdateSheetList1();
//                UpdateProductionInfo(0);
//            }
//        }

//        private void radioPinHole1_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                defectFilter[0] = DefectFilterType.PinHole;
//                DynMvp.UI.Touch.SimpleProgressForm loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Filtering...");
//                loadingForm.Show(new Action(() =>
//                {
//                    DefectFiltering(0);
//                }));
//                UpdateSheetList1();
//                UpdateProductionInfo(0);
//            }
//        }

//        private void radioShape1_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                defectFilter[0] = DefectFilterType.Shape;
//                DynMvp.UI.Touch.SimpleProgressForm loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Filtering...");
//                loadingForm.Show(new Action(() =>
//                {
//                    DefectFiltering(0);
//                }));
//                UpdateSheetList1();
//                UpdateProductionInfo(0);
//            }
//        }

//        private void radioTotal2_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                defectFilter[1] = DefectFilterType.Total;
//                DynMvp.UI.Touch.SimpleProgressForm loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Filtering...");
//                loadingForm.Show(new Action(() =>
//                {
//                    DefectFiltering(1);
//                }));
//                UpdateSheetList2();
//                UpdateProductionInfo(1);
//            }
//        }

//        private void radioBlack2_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                defectFilter[1] = DefectFilterType.Black;
//                DynMvp.UI.Touch.SimpleProgressForm loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Filtering...");
//                loadingForm.Show(new Action(() =>
//                {
//                    DefectFiltering(1);
//                }));
//                UpdateSheetList2();
//                UpdateProductionInfo(1);
//            }
//        }

//        private void radioWhite2_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                defectFilter[1] = DefectFilterType.White;
//                DynMvp.UI.Touch.SimpleProgressForm loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Filtering...");
//                loadingForm.Show(new Action(() =>
//                {
//                    DefectFiltering(1);
//                }));
//                UpdateSheetList2();
//                UpdateProductionInfo(1);
//            }
//        }

//        private void radioPinHole2_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                defectFilter[1] = DefectFilterType.PinHole;
//                DynMvp.UI.Touch.SimpleProgressForm loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Filtering...");
//                loadingForm.Show(new Action(() =>
//                {
//                    DefectFiltering(1);
//                }));
//                UpdateSheetList2();
//                UpdateProductionInfo(1);
//            }
//        }

//        private void radioShape2_CheckedChanged(object sender, EventArgs e)
//        {
//            if (((RadioButton)sender).Checked == true)
//            {
//                defectFilter[1] = DefectFilterType.Shape;
//                DynMvp.UI.Touch.SimpleProgressForm loadingForm = new DynMvp.UI.Touch.SimpleProgressForm("Filtering...");
//                loadingForm.Show(new Action(() =>
//                {
//                    DefectFiltering(1);
//                }));
//                UpdateSheetList2();
//                UpdateProductionInfo(1);
//            }
//        }

//        public void Initialize()
//        {
//            throw new NotImplementedException();
//        }

//        public void RefreshReportPage()
//        {
//            throw new NotImplementedException();
//        }

//        public void ModelAutoSelector()
//        {
//            throw new NotImplementedException();
//        }

//        public void UpdateControl(string item, object value)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
