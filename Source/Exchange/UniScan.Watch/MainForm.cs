using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniScan.Watch.Data;
using UniScan.Watch.Panel;
using UniScan.Watch.Vision;
using UniScanG.Gravure.Vision.Watcher;

namespace UniScan.Watch
{
    public partial class MainForm : Form, IMultiLanguageSupport
    {
        List<string>[] pathList = new List<string>[1];
        Task task = null;
        ImagePanel[] imagePanelChip = new ImagePanel[9];
        ImagePanel[] imagePanelIndex = new ImagePanel[6];
        ImagePanel[] imagePanelFP = new ImagePanel[6];

        public MainForm()
        {
            InitializeComponent();

            for (int i = 0; i < imagePanelChip.Length; i++)
            {
                this.imagePanelChip[i] = new UniScan.Watch.Panel.ImagePanel();
                tableLayoutPanelChip.Controls.Add(this.imagePanelChip[i], i % 3, i / 3);
            }

            for (int i = 0; i < imagePanelIndex.Length; i++)
            {
                this.imagePanelIndex[i] = new UniScan.Watch.Panel.ImagePanel();
                this.imagePanelIndex[i].SetInfoPanelVisible(false);
                tableLayoutPanelIndex.Controls.Add(this.imagePanelIndex[i], i % 2, i / 2);
            }

            for (int i = 0; i < imagePanelFP.Length; i++)
            {
                this.imagePanelFP[i] = new UniScan.Watch.Panel.ImagePanel();
                this.imagePanelFP[i].SetInfoPanelVisible(false);
                tableLayoutPanelFP.Controls.Add(this.imagePanelFP[i], i % 2, i / 2);
            }

            pathList[0] = new List<string>();
//#if DEBUG == true
            //pathList[0].Add(@"\\127.0.0.1\Inspector\Result\Monitoring");
//#else
            pathList[0].Add(@"\\CAM1A\Gravure_Inspector\Result\Monitoring");
            pathList[0].Add(@"\\CAM2A\Gravure_Inspector\Result\Monitoring");
//#endif

            //pathList[1] = new List<string>();
            //pathList[1].Add(@"\\CAM1B\Gravure_Inspector\Result\Monitoring");
            //pathList[1].Add(@"\\CAM2B\Gravure_Inspector\Result\Monitoring");

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (task!= null && task.IsCompleted == false)
                return;

            task = Task.Run(new Action(TaskProc));
        }

        private void TaskProc()
        {
            try
            {
                for (int i = 0; i < pathList.Length; i++)
                {
                    bool ok = IsWriteDone(pathList[i]);
                    if (ok)
                    {
                        Clear();
                        LoadAndUpdate(pathList[i]);
                        ClearWriteDone(pathList[i]);
                    }
                }
            }
            finally
            {
            }
        }

        private void Clear()
        {
            Array.ForEach(imagePanelFP, f => f.Clear());
            Array.ForEach(imagePanelIndex, f => f.Clear());
            Array.ForEach(imagePanelChip, f => f.Clear());
        }

        private void ClearWriteDone(List<string> list)
        {
            foreach(string path in list)
            {
                string file = Path.Combine(path, Watcher.LockFileName);
                File.Delete(file);
            }
        }

        private void LoadAndUpdate(List<string> list)
        {
            ClearRow();
            int chipCnt = 0, fpCnt = 0, indexCnt = 0;
            for (int i = 0; i < list.Count; i++)
            {
                string path = list[i];
                SizeF pelSize = GetPelSize(path);
                string[] files = Directory.GetFiles(path, "*.bmp");
                foreach (string file in files)
                {
                    Bitmap bitmap = (Bitmap)ImageHelper.LoadImage(file);
                    string[] token = Path.GetFileNameWithoutExtension(file).Split('.');
                    if (token.Length < 2)
                        continue;

                    string name = token[0];
                    int index = Convert.ToInt32(token[1]);
                    string title = string.Format(StringManager.GetString(this.GetType().FullName, "Cam{0}.{1}{2}"), i + 1, name, index + 1);
                    switch (token[0])
                    {
                        case "CHIP":
                            if (chipCnt >= imagePanelChip.Length)
                                continue;

                            if (bitmap != null)
                            {
                                MarginCheckerResult marginCheckerResult = MarginChecker.Inspect(title, bitmap, pelSize);
                                float marginL = marginCheckerResult.Margin.Left * pelSize.Width;
                                float marginT = marginCheckerResult.Margin.Top * pelSize.Height;
                                float marginR = marginCheckerResult.Margin.Right * pelSize.Width;
                                float marginB = marginCheckerResult.Margin.Bottom * pelSize.Height;

                                string resultPath = Path.Combine(PathSettings.Instance().Result, DateTime.Now.ToString("yyyyMMdd"));
                                DataExporter dataExporter = new DataExporter();
                                dataExporter.ExportData(resultPath, "Result.csv", marginCheckerResult);
                                //imagePanelChip[chipCnt].UpdateResult(marginL, marginT, marginR, marginB);

                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataGridView);
                                row.DefaultCellStyle.Font = new Font("Malgun Gothic", 12);
                                row.Cells[0].Value = title;
                                row.Cells[1].Value = Math.Min(marginL, marginR);
                                row.Cells[2].Value = Math.Min(marginT, marginB);
                                AddRow(row);
                            }
                            imagePanelChip[chipCnt].UpdateImage(bitmap);
                            imagePanelChip[chipCnt].UpdateText(title);
                            chipCnt++;
                            break;

                        case "FP":
                            if (fpCnt >= imagePanelFP.Length)
                                continue;

                            imagePanelFP[fpCnt].UpdateImage(bitmap);
                            imagePanelFP[fpCnt].UpdateText(title);
                            fpCnt++;
                            break;
                        case "INDEX":
                            if (indexCnt >= imagePanelIndex.Length)
                                continue;
                            imagePanelIndex[indexCnt].UpdateImage(bitmap);
                            imagePanelIndex[indexCnt].UpdateText(title);
                            indexCnt++;
                            break;
                    }
                    File.Delete(file);
                }
            }
            UpdateTitle();
        }
        private delegate void ClearRowDelegate();
        private void ClearRow()
        {
            if (dataGridView.InvokeRequired)
            {
                Invoke(new ClearRowDelegate(ClearRow));
                return;
            }

            dataGridView.Rows.Clear();
        }

        private delegate void AddRowDelegate(DataGridViewRow row);
        private void AddRow(DataGridViewRow row)
        {
            if(dataGridView.InvokeRequired)
            {
                Invoke(new AddRowDelegate(AddRow), row);
                return;
            }

            dataGridView.Rows.Add(row);
        }

        private SizeF GetPelSize(string path)
        {
            SizeF pelSize = SizeF.Empty;

            string file = Path.Combine(path, Watcher.LockFileName);
            FileStream fileStream = new FileStream(file, FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream);
            while (streamReader.EndOfStream == false)
            {
                string line = streamReader.ReadLine();
                string[] tokens = line.Split(',');
                WatcherLockFile e;
                bool ok = Enum.TryParse<WatcherLockFile>(tokens[0], out e);
                if (ok == false)
                    continue;

                switch (e)
                {
                    case WatcherLockFile.PelWidth:
                        pelSize.Width = Convert.ToSingle(tokens[1]);
                        break;
                    case WatcherLockFile.PelHeight:
                        pelSize.Height = Convert.ToSingle(tokens[1]);
                        break;
                }
            }
            streamReader.Close();
            fileStream.Close();
            return pelSize;
        }

        private bool IsWriteDone(List<string> list)
        {
            return list.TrueForAll(f =>
            {
                string file = Path.Combine(f, Watcher.LockFileName);
                return File.Exists(file);
            });
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            InitGridView();
        

            string badPath = CheckPath();
            if (string.IsNullOrEmpty(badPath) == false)
            {
                MessageForm.Show(null, string.Format("Cannot find path {0}", badPath));
                this.Close();
                return;
            }

            StringManager.AddListener(this);
            UpdateTitle();
            this.WindowState = FormWindowState.Maximized;
            timer1.Interval = 500;
            timer1.Start();
        }

        private void InitGridView()
        {
            this.dataGridView.Columns.Clear();

            this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "columnTarget", HeaderText = "Target", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "columnMarginW", HeaderText = "Width", AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader });
            this.dataGridView.Columns.Add(new DataGridViewTextBoxColumn() { Name = "columnMarginH", HeaderText = "Height", AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader });

            for (int i = 1; i < 3; i++)
            {
                DataGridViewColumn column = this.dataGridView.Columns[i];
                column.DefaultCellStyle.Format = "F0";
                column.DefaultCellStyle.Font = new Font("Malgun Gothic", 12);
            }
        }

        private delegate void UpdateTitleDelegate();
        private void UpdateTitle()
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UpdateTitleDelegate(UpdateTitle));
                return;
            }

            string title = CustomizeSettings.Instance().ProgramTitle;
            string copyright = CustomizeSettings.Instance().Copyright;
            this.Text = string.Format("{0} @ {1}, Version {2} Build {3}", "Gravure Watcher", "2019 UniEye", VersionHelper.Instance().VersionString, VersionHelper.Instance().BuildString);

            //this.Text = string.Format("{0} - Build@{1} - Update@{2}", "Gravure Monitoring", VersionHelper.GetBuildDateTime().ToString("yyMMdd.HHmm"), DateTime.Now.ToString("yyMMdd.HHmmss"));
        }

        private string CheckPath()
        {
            string badString = null;
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                for (int i = 0; i < pathList.Length && string.IsNullOrEmpty(badString); i++)
                {
                    for (int j = 0; j < pathList[i].Count && string.IsNullOrEmpty(badString); j++)
                    {
                        string path = pathList[i][j];
                        if (Directory.Exists(path) == false)
                            badString = path;
                    }
                }
            });
            return badString;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
