using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Settings;
using UniEye.Base;
using UniEye.Base.UI;
using DynMvp.Authentication;

namespace UniScanG.Temp
{
    public partial class ReportPanel : UserControl, IMainTabPage
    {
        List<DirectoryInfo>[] lotList = new List<DirectoryInfo>[2];
        List<DirectoryInfo>[] findedLotList = new List<DirectoryInfo>[2];
        bool onUpdateList = false;

        IReportPanel probeReportPanel;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public ReportPanel()
        {
            InitializeComponent();
            labelTilda.Text = StringManager.GetString(this.GetType().FullName, labelTilda.Text);
            labelTilda.Text = StringManager.GetString(this.GetType().FullName, labelTilda.Text);
            btnSearch.Text = StringManager.GetString(this.GetType().FullName, btnSearch.Text);            

            AddPictureBox();
            VisibleChangeControl();

            Initialize();

            startDate.CustomFormat = "yyyy-MM-dd";
            endDate.CustomFormat = "yyyy-MM-dd";
        }

        public void EnableControls(UserType userType)
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == true)
            {
                RefreshReportPage();
                findModel.Text = "";
                //ModelAutoSelector();

                Search();
            }
        }

        public void Initialize()
        {
            lotList[0] = new List<DirectoryInfo>();
            lotList[1] = new List<DirectoryInfo>();
            findedLotList[0] = new List<DirectoryInfo>();
            findedLotList[1] = new List<DirectoryInfo>();
        }

        private void RefreshModelList()
        {
            modelList.Rows.Clear();

            List<ModelDescription> mdList = SystemManager.Instance().ModelManager.ToList();
            mdList.Sort((x, y) => y.RegistrationDate.CompareTo(x.RegistrationDate));

            int index = 1;
            foreach (ModelDescription md in mdList)
            {
                modelList.Rows.Add(index, md.Name, md.RegistrationDate.ToString("yyyy-MM-dd"));
                index++;
            }

            totalModel.Text = string.Format("Total : {0}", SystemManager.Instance().ModelManager.Count());
            modelList.Sort(modelList.Columns[2], System.ComponentModel.ListSortDirection.Descending);
        }

        public void RefreshReportPage()
        {
            SystemManager.Instance().ModelManager.Refresh();
            RefreshModelList();
        }

        private void VisibleChangeControl()
        {
            
        }

        private void GetLotList()
        {

        }

        private void Search()
        {
        //    if (onUpdateList == true)
        //        return;

        //    onUpdateList = true;
        //    probeReportPanel.Clear();
        //    lotList[0].Clear();
        //    lotList[1].Clear();

        //    findedLotList[0].Clear();
        //    findedLotList[1].Clear();

        //    this.lotNoList.Rows.Clear(); //모델이 바뀌면 데이터 리스트 초기화           
        //    //serialDataGridView.Columns.Clear();
        //    //this.lotNoCombo.Items.Clear();

        //    List<List<string>> defaultPaths = new List<List<string>>();
        //    List<string> modelList = new List<string>();
        //    List<string> resultPaths = new List<string>();

        //    if (UniScanGSettings.Instance().SystemType == SystemType.Inspector)
        //    {
        //        resultPaths.Add(PathSettings.Instance().Result);
        //        List<string> resultPath = new List<string>();
        //    }
        //    else
        //    {
        //        foreach (InspectorInfo info in UniScanGSettings.Instance().ClientInfoList)
        //        {
        //            resultPaths.Add(Path.Combine(info.Path, "Result"));
        //        }
        //    }

        //    DateTime startTime = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, 0, 0, 0);
        //    DateTime endTime = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);


        //    foreach (string path in resultPaths)
        //    {
        //        DirectoryInfo directoryInfo = new DirectoryInfo(path);

        //        List<string> defaultPath = new List<string>();

        //        if (directoryInfo.Exists == true)
        //        {
        //            System.Text.RegularExpressions.Regex rgx = new System.Text.RegularExpressions.Regex(@"[0-9-]");
        //            foreach (DirectoryInfo dateDirectory in directoryInfo.GetDirectories())
        //            {
        //                if (dateDirectory.Name.Count() == 10)
        //                {
        //                    System.Text.RegularExpressions.MatchCollection matches = rgx.Matches(dateDirectory.Name);
        //                    if (matches.Count == dateDirectory.Name.Count())
        //                    {
        //                        DateTime folderDateTime = Convert.ToDateTime(dateDirectory.ToString());
        //                        if (folderDateTime.Date >= startTime.Date && folderDateTime <= endTime.Date)
        //                        {
        //                            foreach (DirectoryInfo modelDirectory in dateDirectory.GetDirectories())
        //                            {
        //                                if (findModel.Text != "" && !modelDirectory.Name.Contains(findModel.Text.ToString().ToUpper()))
        //                                {
        //                                    continue;
        //                                }

        //                                defaultPath.Add(modelDirectory.FullName);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (defaultPath.Count != 0)
        //            defaultPaths.Add(defaultPath);
        //    }

        //    if (defaultPaths.Count > 0)
        //    {
        //        for (int i = 0; i < 2 /*defaultPaths.Count*/; i++)
        //        {
        //            foreach (string path in defaultPaths[i])
        //            {
        //                DirectoryInfo directoryInfo = new DirectoryInfo(path);
        //                //List<DirectoryInfo> searchDirector = new List<DirectoryInfo>();

        //                if (directoryInfo.Exists == true)
        //                {
        //                    foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
        //                    {
        //                        lotList[i].Add(directory);
        //                        //int rowIndex = serialDataGridView.Rows.Add(directory.Name);
        //                        //serialDataGridView.Rows[rowIndex].Tag = directory.FullName;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (UniScanGSettings.Instance().SystemType == SystemType.Inspector)
        //    {
        //        findedLotList[0].AddRange(lotList[0]);

        //        foreach (DirectoryInfo directory in findedLotList[0])
        //        {
        //            int rowIndex = lotNoList.Rows.Add(directory.Parent.Parent.Name, directory.Parent.Name, directory.Name);
        //            lotNoList.Rows[rowIndex].Tag = directory.FullName;
        //        }
        //    }
        //    else
        //    {
        //        foreach (DirectoryInfo directory in lotList[0])
        //        {
        //            foreach (DirectoryInfo directory2 in lotList[1])
        //            {
        //                if (directory.Name == directory2.Name && directory.Parent.Name == directory2.Parent.Name && directory.Parent.Parent.Name == directory2.Parent.Parent.Name)
        //                {
        //                    findedLotList[0].Add(directory);
        //                    findedLotList[1].Add(directory2);

        //                    int rowIndex = lotNoList.Rows.Add(directory.Parent.Parent.Name, directory.Parent.Name, directory.Name);
        //                    lotNoList.Rows[rowIndex].Tag = directory.FullName;
        //                    lotNoList.Rows[rowIndex].Cells[0].Tag = directory2.FullName;
        //                }
        //            }
        //        }
        //    }

        //    totalLot.Text = string.Format("Total : {0}", lotNoList.Rows.Count);
        //    onUpdateList = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Search();
            SerialDataGridViewCellEvent();
        }

        public void ModelAutoSelector()
        {
            Model curModel = SystemManager.Instance().CurrentModel;

            if (curModel == null)
            {
                findModel.Text = "";
                return;
            }
                
            if (curModel.IsEmpty() == true)
            {
                findModel.Text = "";
                return;
            }

            findModel.Text = curModel.Name;
        }
        
        private List<string> GetImagePathList(string resultPath, string surfix = "")
        {
            List<string> searchPathList = new List<string>();

            List<string> imagePathList = new List<string>();
            //for (int i = 0; i < viewIndex; i++)
            {
                //string imageName = String.Format("Image_C{0:00}_S{1:000}_L00{2}", i, , surfix);
                //imagePathList.Add(string.Format("{0}\\{1}.bmp", resultPath, imageName));
                //imagePathList.Add(string.Format("{0}\\{1}.jpeg", resultPath, imageName));
                //imagePathList.Add(string.Format("{0}\\{1}.png", resultPath, imageName));
                //imagePathList.Add(string.Format("{0}\\Image_C{1:00}_S{2:000}.3d", resultPath, i, Convert.ToInt32(stepNo.Text)));
            }

            string imagePath = "";
            foreach (string searchImagePath in imagePathList)
            {
                if (File.Exists(searchImagePath) == true)
                {
                    LogHelper.Debug(LoggerType.Operation, "Search Path - " + searchImagePath);
                    searchPathList.Add(searchImagePath);
                }
            }
            imagePathList.Clear();

            LogHelper.Debug(LoggerType.Operation, "Found path - " + imagePath);

            return searchPathList;
        }

        private void serialDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (lotNoList.SelectedRows.Count < 1)
                return;

            findLotNo.Text = (string)lotNoList.SelectedRows[0].Cells[2].Value;
        }

        private void modelCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void AddPictureBox()
        {
            //int numOfResultView = 0;

            //switch (UniScanGSettings.Instance().SystemType)
            //{
            //    case SystemType.Inspector:
            //        //probeReportPanel = new Inspector.ProbeReportPanel();
            //        //numOfResultView = 1;
            //        break;
            //    case SystemType.Monitor:
            //        probeReportPanel = new Monitor.ProbeReportPanel();
            //        //numOfResultView = 2;
            //        ((Control)probeReportPanel).Dock = DockStyle.Fill;
            //        resultImageTable.Controls.Add((Control)probeReportPanel);
            //        resultImageTable.Dock = DockStyle.Fill;
            //        break;
            //}
      

            /*
            resultImageTable.ColumnStyles.Clear();
            resultImageTable.RowStyles.Clear();

            int numCount = (int)Math.Ceiling(Math.Sqrt(numOfResultView));
            resultImageTable.ColumnCount = numCount;
            resultImageTable.RowCount = (int)Math.Ceiling(Math.Sqrt(numOfResultView));

            for (int i = 0; i < resultImageTable.ColumnCount; i++)
            {
                resultImageTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / resultImageTable.ColumnCount));
            }

            for (int i = 0; i < resultImageTable.RowCount; i++)
            {
                resultImageTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / resultImageTable.RowCount));
            }

            float widthPct = (float)100 / numOfResultView;

            resultImageTable.Controls.Clear();
            resultImageTable.Dock = DockStyle.None;
            resultImageTable.Controls.Add((Control)probeReportPanel);
            resultImageTable.Dock = DockStyle.Fill;*/
        }

        private void SerialDataGridViewCellEvent()
        {
            //if (onUpdateList == true)
            //    return;

            //if (lotNoList.SelectedRows.Count <= 0)
            //    return;
            
            //List<string> updateInfoHints = new List<string>();

            //string fullPath = lotNoList.SelectedRows[0].Tag.ToString();
            //string modelName = lotNoList.SelectedRows[0].Cells[1].Value.ToString();
            //string lotNo = lotNoList.SelectedRows[0].Cells[2].Value.ToString();

            //updateInfoHints.Add(modelName);
            //updateInfoHints.Add(lotNo);

            //if (UniScanGSettings.Instance().SystemType == SystemType.Inspector)
            //{
            //    //updateInfoHints.Add(fullPath);
            //    //probeReportPanel.UpdateInfo(null, updateInfoHints);
            //}
            //else
            //{
            //    string fullPath2 = lotNoList.SelectedRows[0].Cells[0].Tag.ToString();

            //    updateInfoHints.Add(fullPath);
            //    updateInfoHints.Add(fullPath2);
            //    probeReportPanel.UpdateControl(null, updateInfoHints);
            //}            
        }

        private void findLotNo_TextChanged(object sender, EventArgs e)
        {
            //onUpdateList = true;

            //if (string.IsNullOrEmpty(findLotNo.Text) == true)
            //    return;

            //lotNoList.Rows.Clear();

            //if (UniScanGSettings.Instance().SystemType == SystemType.Inspector)
            //{
            //    foreach (DirectoryInfo directory in findedLotList[0])
            //    {
            //        if (directory.Name.Contains(findLotNo.Text))
            //        {
            //            int rowIndex = lotNoList.Rows.Add(directory.Parent.Parent.Name, directory.Parent.Name, directory.Name);
            //            lotNoList.Rows[rowIndex].Tag = directory.FullName;
            //        }
            //    }
            //}
            //else
            //{
            //    for (int i = 0; i < findedLotList[0].Count; i++)
            //    {
            //        if (findedLotList[0][i].Name.Contains(findLotNo.Text))
            //        {
            //            int rowIndex = lotNoList.Rows.Add(findedLotList[0][i].Parent.Parent.Name, findedLotList[0][i].Parent.Name, findedLotList[0][i].Name);
            //            lotNoList.Rows[rowIndex].Tag = findedLotList[0][i].FullName;
            //            lotNoList.Rows[rowIndex].Cells[0].Tag = findedLotList[1][i].FullName;
            //        }
            //    }
            //}

            //onUpdateList = false;
        }

        private void findModel_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void lotNoList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (lotNoList.SelectedRows.Count < 1)
                return;

            findLotNo.Text = (string)lotNoList.SelectedRows[0].Cells[2].Value;
            //SerialDataGridViewCellEvent();
        }

        private void modelList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ModelListClick();
        }

        private void modelList_SelectionChanged(object sender, EventArgs e)
        {
            ModelListClick();
        }

        private void ModelListClick()
        {
            if (modelList.SelectedRows.Count == 0)
                return;

            findModel.Text = modelList.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void startDate_ValueChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void endDate_ValueChanged(object sender, EventArgs e)
        {
            Search();
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            throw new NotImplementedException();
        }
    }
}
