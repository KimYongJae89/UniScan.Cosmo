using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using DynMvp.Base;
using DynMvp.UI;
using System.Threading.Tasks;
using System.IO;
using UniScanG.Screen.Data;
using UniScanG.Data;
using DynMvp.UI.Touch;
using DynMvp.Data;

namespace UniScanG.UI.Report
{
    public partial class ReportProgressForm : Form
    {         
        private string messageText;
        public string MessageText
        {
          set { messageText = value; }
        }

        List<DataGridViewRow> sheetDataList;

        int maxCount = 0;

        bool isCancle = false;
        bool searchDone = false;
        
        public ReportProgressForm(string message)
        {
            InitializeComponent();
            messageText = message;

#if DEBUG
            this.ControlBox = true;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
#endif

            this.TopMost = true;
            this.TopLevel = true;
            if (messageText != null)
            {
                this.labelMessage.Text = StringManager.GetString(this.GetType().FullName, message);           
            }
        }

        public void SetLabelMessage(string message)
        {
            this.labelMessage.Text = StringManager.GetString(this.GetType().FullName, messageText);
        }

        public void Show(string resultPath, List<DataGridViewRow> sheetDataList, List<FileInfo> teachInfoList)
        {
            if (Directory.Exists(UniEye.Base.Settings.PathSettings.Instance().Result) == false)
                return;

            string actualPath = GetActualPath(resultPath);
            if (string.IsNullOrEmpty(actualPath) || Directory.Exists(actualPath)==false)
            {
                MessageForm.Show(null, StringManager.GetString("Can not found Result Path"));
                return;
            }

            this.sheetDataList = sheetDataList;

            DirectoryInfo directoryInfo = new DirectoryInfo(actualPath);
            DirectoryInfo[] subDirectoryInfoList = directoryInfo.GetDirectories();
            maxCount = subDirectoryInfoList.Count();
            if (maxCount == 0)
                return;

            teachInfoList.AddRange(directoryInfo.GetFiles("TeachData_*.xml"));

            Array.Sort(subDirectoryInfoList, (f, g) =>
            {
                int a, b;
                bool aa = int.TryParse(f.Name, out a);
                bool bb = int.TryParse(g.Name, out b);
                if (aa && bb)
                    return a.CompareTo(b);
                else
                    return f.Name.CompareTo(g.Name);
            });
            
            progressBar.Value = sheetDataList.Count / maxCount * 100;
            labelRatio.Text = string.Format("{0} / {1}", sheetDataList.Count, maxCount);

            Task task = new Task(new Action(() => Search(subDirectoryInfoList)));
            task.Start();

            base.ShowDialog();
        }

        private string GetActualPath(string resultPath)
        {
            string actualPath = resultPath;

            if (File.Exists(Path.Combine(resultPath, DataCopier.FlagFileName)))
            {
                actualPath = File.ReadAllText(Path.Combine(resultPath, DataCopier.FlagFileName));
            }
            else if (Directory.Exists(resultPath) == false)
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(resultPath);
                List<DriveInfo> driveInfoList = DataCopier.GetTargetDriveInfoList();
                foreach (DriveInfo driveInfo in driveInfoList)
                {
                    string ss = driveInfo.Name;
                    string resultPath2 = ss + resultPath.Substring(ss.Length);
                    if (Directory.Exists(resultPath2))
                        return resultPath2;
                }
            }
            return actualPath;
        }

        private void Search(DirectoryInfo[] directoryInfoList)
        {
            //Parallel.ForEach(directoryInfoList, subDirectory =>
            try
            {
                foreach (DirectoryInfo subDirectory in directoryInfoList)
                {
                    if (isCancle == true)
                        return;

                    bool parseResult = false;
                    int index = -1;
                    parseResult = int.TryParse(subDirectory.Name, out index);

                    if (parseResult == false)
                        return;

                    //SheetResult sheetResult = SheetCombiner.resultCollector.CreateSheetResult();
                    //sheetResult.Import(subDirectory.FullName);
                    MergeSheetResult mergeSheetResult = new MergeSheetResult(index, subDirectory.FullName);

                    DataGridViewRow dataGridViewRow = new DataGridViewRow();

                    DataGridViewTextBoxCell nameCell = new DataGridViewTextBoxCell() { Value = mergeSheetResult.Index + 1 };
                    DataGridViewTextBoxCell qtyCell = new DataGridViewTextBoxCell() { Value = mergeSheetResult.SheetSubResultList.Count };

                    dataGridViewRow.Cells.Add(nameCell);
                    dataGridViewRow.Cells.Add(qtyCell);

                    dataGridViewRow.Tag = mergeSheetResult;

                    lock (sheetDataList)
                        sheetDataList.Add(dataGridViewRow);
                    
                }
            }
            finally
            {
                searchDone = true;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            isCancle = true;
        }

        private void taskCheckTimer_Tick(object sender, EventArgs e)
        {
            if (searchDone == true)
                Close();

            if (maxCount != 0)
            {
                int newValue = (int)((float)sheetDataList.Count / (float)maxCount * 100.0f);
                newValue = Math.Max(Math.Min(newValue, progressBar.Maximum), progressBar.Minimum);
                progressBar.Value = newValue;
                labelRatio.Text = string.Format("{0} / {1}", sheetDataList.Count, maxCount);
                progressBar.Invalidate();
            }
        }

        private void SimpleProgressForm_Load(object sender, EventArgs e)
        {
            taskCheckTimer.Start();
        }
    }
}
