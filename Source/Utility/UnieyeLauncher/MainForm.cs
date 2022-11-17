using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace UnieyeLauncher
{
    public partial class MainForm : Form
    {
        private bool useAutoPatch = false;
        private string patchPath = "";
        private List<string> patchFiles = new List<string>();
        
        public MainForm()
        {
            InitializeComponent();

            this.patchPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..", @"Update"));
            //string curDirectory = Directory.GetCurrentDirectory();

            LoadSettings();
            UpdateData();
        }

        private void LoadSettings()
        {
            string curDir = Environment.CurrentDirectory;
            LoadSettings(Path.Combine(curDir, "UniEyeLauncher.xml"));
        }

        private void LoadSettings(string xmlFile)
        {
            if (File.Exists(xmlFile) == false)
                return;

            patchFiles.Clear();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlFile);

            XmlElement xmlElement = xmlDocument["Settings"];
            if (xmlElement == null)
                return;
            
            useAutoPatch = Convert.ToBoolean(xmlElement["Use"].InnerText);

            XmlElement xmlFilesElement = xmlElement["Files"];
            if (xmlFilesElement != null)
            {
                foreach (XmlElement xmlFileElement in xmlFilesElement)
                {
                    if (xmlFileElement.Name == "File")
                        patchFiles.Add(xmlFileElement.InnerText);
                }
            }
        }

        private void SaveSettings()
        {
            string curDir = Environment.CurrentDirectory;
            SaveSettings(Path.Combine(curDir, "UniEyeLauncher.xml"));
        }

        private void SaveSettings(string xmlFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElement = xmlDoc.CreateElement("Settings");
            xmlDoc.AppendChild(xmlElement);

            XmlElement useElement = xmlElement.OwnerDocument.CreateElement("", "Use", "");
            xmlElement.AppendChild(useElement);
            useElement.InnerText = useAutoPatch.ToString();
            //XmlHelper.SetValue(xmlElement, "Path", patchPath);

            XmlElement xmlFilesElement = xmlDoc.CreateElement("Files");
            xmlElement.AppendChild(xmlFilesElement);

            foreach (string patchFile in patchFiles)
            {
                XmlElement fileElement = xmlElement.OwnerDocument.CreateElement("", "File", "");
                xmlFilesElement.AppendChild(fileElement);

                fileElement.InnerText = patchFile;
            }

            xmlDoc.Save(xmlFile);
        }

        private delegate void UpdateDataDelegate();
        private void UpdateData()
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UpdateDataDelegate(UpdateData));
                return;
            }

            this.autoPatchCheckBox.Checked = useAutoPatch;
            this.textBoxPatchPath.Text = patchPath;
            this.listBoxPatchFiles.Items.Clear();
            foreach(string patchFile in patchFiles)
            {
                this.listBoxPatchFiles.Items.Add(patchFile);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void buttonPatchFileAdd_Click(object sender, EventArgs e)
        {
            string newFile = textBoxPatchFile.Text;
            if (string.IsNullOrWhiteSpace(newFile))
                return;

            this.patchFiles.Add(newFile);
            textBoxPatchFile.Text = "";
            UpdateData();
        }

        private void buttonPatchFileDel_Click(object sender, EventArgs e)
        {
            if (listBoxPatchFiles.SelectedIndex < 0)
                return;

            this.patchFiles.RemoveAt(listBoxPatchFiles.SelectedIndex);
            UpdateData();
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            initTimer.Start();
        }

        bool toolStriopExitClicked = false;
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStriopExitClicked = true;
            Close();
        }

        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                this.watchdogTimer.Stop();
            else if (autoPatchCheckBox.Checked)
                this.watchdogTimer.Start();
        }

        Task workTask = null;
        private void WatchdogTimer_Tick(object sender, EventArgs e)
        {
            if (this.patchFiles.Count == 0)
                return;

            if (workTask != null && workTask.IsCompleted == false)
                return;
                       
            workTask?.Dispose();
            workTask = new Task(WorkTask);
            workTask.Start();
        }

        bool initialStartUp = true;
        private void WorkTask()
        {
            bool allExist = this.patchFiles.TrueForAll(f =>
        {
            string patchFile = Path.Combine(patchPath, f);
            return File.Exists(patchFile);
            });

            try
            {
                if (allExist)
                {
                    string[] exeFiles = patchFiles.FindAll(f => Path.GetExtension(f).ToLower() == ".exe")?.ToArray();
                    CloseApp(exeFiles);
                    CopyPatch();
                    StartApp(exeFiles);
                }
                else
                {
                    string[] exeFiles = patchFiles.FindAll(f => Path.GetExtension(f).ToLower() == ".exe")?.ToArray();
                    bool isAppRun = IsAppRun(exeFiles);
                    bool isAppDown = !isAppRun && LockFileExist();
                    if (!isAppRun && (isAppDown || initialStartUp))
                        StartApp(exeFiles);
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("============================");
                sb.AppendLine(string.Format("[{0}] {1}", DateTime.Now, ex.Message));
                sb.AppendLine(ex.StackTrace);

                File.AppendAllText(@".\UniScanLog.log", sb.ToString());
                MessageBox.Show(ex.Message);
            }

            initialStartUp = false;
            workTask = null;
        }

        private bool IsAppRun(string[] exeFiles)
        {
            return Array.TrueForAll(exeFiles, f =>
            {
                string processName = Path.GetFileNameWithoutExtension(f);
                Process[] processes = Process.GetProcessesByName(processName);
                return processes.Length > 0;
            });
        }

        private bool LockFileExist()
        {
            string lockFile = Path.Combine(Environment.CurrentDirectory, "..", "Temp", "~UniEye.lock");
            return File.Exists(lockFile);
        }

        private void StartApp(string[] exeFiles)
        {
            string binPath = Path.GetFullPath(Path.Combine(patchPath, "..", "Bin"));
            Array.ForEach(exeFiles, f => Process.Start(Path.Combine(binPath, f)));
        }

        private void CopyPatch()
        {
            int errCount = 0;
            string binPath = Path.GetFullPath(Path.Combine(patchPath, "..", "Bin"));
            string bakPath = Path.GetFullPath(Path.Combine(patchPath, "..", "BackUp", DateTime.Now.ToString("yyyyMMdd_HHmmss")));
            Directory.CreateDirectory(bakPath);

            List<string> copyList = new List<string>(patchFiles);
            List<string> doneList = new List<string>();
            while (copyList.Count > 0)
            {
                string patchFile = copyList[0];
                string srcFile = Path.Combine(patchPath, patchFile);
                string dstFile = Path.Combine(binPath, patchFile);
                string bakFile = Path.Combine(bakPath, patchFile);
                try
                {
                    if (File.Exists(dstFile))
                        File.Move(dstFile, bakFile);

                    Thread.Sleep(100);
                    File.Copy(srcFile, dstFile);
                    Thread.Sleep(100);
                    doneList.Add(srcFile);
                    copyList.RemoveAt(0);
                    errCount = 0;
                }
                catch (Exception ex)
                {
                    errCount++;
                    if (errCount > 100)
                    {
                        throw ex;
                    }
                }
                Thread.Sleep(10);
            }

            doneList.ForEach(f => File.Delete(f));
        }

        private void CloseApp(string[] exeFiles)
        {
            List<string> exeFileList = exeFiles.ToList();
            bool waitKillDone = false;
            while (exeFileList.Count > 0)
            {
                string processName = Path.GetFileNameWithoutExtension(exeFileList[0]);
                Process[] processes = Process.GetProcessesByName(processName);
                if (processes.Length > 0)
                {
                    if (waitKillDone == false)
                        Array.ForEach(processes, g => g.Kill());
                    Thread.Sleep(500);
                    waitKillDone = true;
                }
                else
                {
                    exeFileList.RemoveAt(0);
                    waitKillDone = false;
                }
            }
        }

        private void autoPatchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            useAutoPatch = autoPatchCheckBox.Checked;
        }

        private void initTimer_Tick(object sender, EventArgs e)
        {
            initTimer.Stop();
            if (useAutoPatch)
            {
                this.Visible = false;
            }
        }

        private void listBoxPatchFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            Array.ForEach(files, f => {
                string item = Path.GetFileName(f);
                
                if (this.patchFiles.Contains(item) == false)
                    this.patchFiles.Add(item);
                });
            UpdateData();
        }

        private void listBoxPatchFiles_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void listBoxPatchFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Delete)
            {
                string[] a = new string[listBoxPatchFiles.SelectedItems.Count];
                listBoxPatchFiles.SelectedItems.CopyTo(a, 0);
                Array.ForEach(a, f => listBoxPatchFiles.Items.Remove(f));
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool force = (e.CloseReason == CloseReason.WindowsShutDown);
            if (toolStriopExitClicked || force)
            {
                this.watchdogTimer.Stop();
                return;
            }
            e.Cancel = true;
            this.Hide();
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
