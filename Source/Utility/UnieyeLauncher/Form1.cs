using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Xml;

using DynMvp;
using DynMvp.Base;

namespace UnieyeLauncher
{
    public partial class launcherForm : Form
    {
        const string programName = "Unieye.exe";
        string lockFilePath = Directory.GetCurrentDirectory() + "\\..\\Temp";
        string patchFilePath = "";
        bool autoPatchMode = true;

        public launcherForm()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            this.Visible = false;
            this.trayIcon.Visible = true;
            trayIcon.ContextMenuStrip = menuStrip;

            if (File.Exists("pathConfig.xml") == false)
            {
                patchFilePath = "";
                autoPatchMode = false;
            }
            else
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load("pathConfig.xml");
                XmlElement element = xmlDocument.DocumentElement;

                patchFilePath = XmlHelper.GetValue(element, "PatchFilePath", "");
                autoPatchMode = Convert.ToBoolean(XmlHelper.GetValue(element, "AutoPatchMode", "True"));
            }
        }

        private void launcherForm_Load(object sender, EventArgs e)
        {
            watchDogTimer.Start();
        }

        private void watchDogTimer_Tick(object sender, EventArgs e)
        {
            Process[] currentProcess = Process.GetProcesses();

            int processIndex = -1, loopIndex = 0;

            foreach (Process currentPs in currentProcess)
            {
                string processName = currentPs.ProcessName;

                if (processName == programName)
                {
                    processIndex = loopIndex;
                    break;
                }
                loopIndex++;
            }
            //1. 프로세스가 살아 있으면 ==> return
            if (processIndex >= 0)
                return;
            //2. 프로세스가 죽으면
            else
            {
                //2-1. 패치가 있으면
                if (patchFilePath != "")
                {
                    DirectoryInfo patchDir = new DirectoryInfo(patchFilePath);
                    FileInfo[] patchFiles = patchDir.GetFiles("*.*", SearchOption.AllDirectories);
                    //2-1-1. patch 폴더에 파일이 있다면
                    if (patchFiles.Count() != 0)
                    {
                        FileInfo patchExeFile = new FileInfo(patchDir + "\\" + programName);
                        FileInfo currentExeFile = new FileInfo(programName);

                        //2-1-1-1. 파일의 날짜가 현재 프로그램보다 더 최신이라면 ==> 기존 것 카피 후 패치 파일을 기존것에 덮어쓰고 다시 실행
                        if (patchExeFile.LastWriteTime > currentExeFile.LastWriteTime && autoPatchMode == true)
                        {
                            DirectoryInfo currentFIleDir = new DirectoryInfo(Directory.GetCurrentDirectory());
                            FileInfo[] currentFIles = currentFIleDir.GetFiles("*.*", SearchOption.AllDirectories);
                            foreach (FileInfo patchFile in patchFiles)
                            {
                                foreach (FileInfo currentFIle in currentFIles)
                                {
                                    if (patchFile.Name == currentFIle.Name)
                                    {
                                        string backupFileName = currentFIle.Name + "~";
                                        if (File.Exists(backupFileName))
                                            File.Delete(backupFileName);

                                        File.Copy(currentFIle.Name, backupFileName);
                                        File.Delete(currentFIle.Name);
                                        File.Move(patchFilePath + "\\" + patchFile.Name, currentFIle.Name);

                                        break;
                                    }
                                }
                            }

                            Process ps = new Process();
                            ps.StartInfo.FileName = programName;
                            ps.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                            ps.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

                            ps.Start();

                            return;

                        }
                        //2-1-1-2. 파일의 날짜가 현재 프로그램과 동일 또는 그 전이라면 ==> 그대로 종료
                        else
                        {
                            return;
                        }
                    }
                    //2-1-2. 패치가 없다면
                    else
                    {
                        //2-1-2-1. lock 파일이 있으면 ==> 다시 프로그램 실행 후 return
                        if (lockFilePath != "")
                        {
                            DirectoryInfo dir = new DirectoryInfo(lockFilePath);
                            FileInfo[] files = dir.GetFiles("*.lock", SearchOption.AllDirectories);
                            if (files.Count() != 0)
                            {
                                Process ps = new Process();
                                ps.StartInfo.FileName = programName;
                                ps.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                                ps.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;

                                ps.Start();

                                return;
                            }
                            //2-1-2-2. lock도 없고 패치도 없다 ==> 정상종료
                            else
                            {
                                return;
                            }
                        }
                    }

                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditForm editForm = new EditForm(patchFilePath, autoPatchMode);
            editForm.ShowDialog();
            if (editForm.DialogResult == DialogResult.OK)
            {
                this.patchFilePath = editForm.patchFilePath;
                this.autoPatchMode = editForm.autoPatchMode;

                XmlDocument xmlDocument = new XmlDocument();

                XmlElement element = xmlDocument.CreateElement("", "Path", "");

                xmlDocument.AppendChild(element);

                XmlHelper.SetValue(element, "PatchFilePath", patchFilePath);
                XmlHelper.SetValue(element, "AutoPatchMode", autoPatchMode.ToString());

                xmlDocument.Save("pathConfig.xml");
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement element = xmlDocument.CreateElement("", "Path", "");

            xmlDocument.AppendChild(element);

            XmlHelper.SetValue(element, "PatchFilePath", patchFilePath);
            XmlHelper.SetValue(element, "AutoPatchMode", autoPatchMode.ToString());

            xmlDocument.Save("pathConfig.xml");

            watchDogTimer.Stop();
            Application.Exit();
            //다른 방법 : System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
