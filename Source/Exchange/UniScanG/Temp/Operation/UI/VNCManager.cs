//using DynMvp.Base;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Forms;
//using UniEye.Base;

//namespace UniScanG.Temp
//{
//    class VNCProcess : ThreadHandler
//    {
//        System.Diagnostics.Process process;
//        public System.Diagnostics.Process Process
//        {
//            get { return process; }
//            set { process = value; }
//        }

//        int camIndex;
//        public int CamIndex
//        {
//            get { return camIndex; }
//            set { camIndex = value; }
//        }

//        int clientIndex;
//        public int ClientIndex
//        {
//            get { return clientIndex; }
//            set { clientIndex = value; }
//        }

//        string targetKey;
//        public string TargetKey
//        {
//            get { return targetKey; }
//            set { targetKey = value; }
//        }

//        public ThreadState state
//        {
//            get { return WorkingThread.ThreadState; }
//        }


//        public VNCProcess(int camIndex, int clientIndex, string targetKey)
//        {
//            this.camIndex = camIndex;
//            this.clientIndex = clientIndex;
//            this.targetKey = targetKey;
//        }

//        public void Start()
//        {
//            WorkingThread = new Thread(ProcessWatch);
//            WorkingThread.Start();
//        }

//        public void ProcessWatch()
//        {
//            if (process != null)
//            {
//                while (process.HasExited == false)
//                {
//                    Thread.Sleep(100);
//                }
//            }

//            if (targetKey != null)
//            {
//                //MonitoringServer server = (SystemManager.Instance().MachineIf as MonitoringServer);
//                server.SendTabDisable(camIndex, clientIndex, targetKey, true);
//            }
//        }

//        public void Close()
//        {
//            if (process.HasExited == false)
//                process.Kill();
//        }
//    }

//    class VNCManager
//    {
//        [DllImport("User32", EntryPoint = "FindWindow")]
//        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

//        [DllImport("user32.dll")]
//        public static extern void SetForegroundWindow(IntPtr hWnd);

//        [DllImport("user32.dll")]
//        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
//        private const int SW_SHOWNORMAL = 1;

//        string targetKey;
//        public string TargetKey
//        {
//            get { return targetKey; }
//            set { targetKey = value; }
//        }
        
//        List<VNCProcess> vncProcessList = new List<VNCProcess>();

//        public VNCManager(string targetKey)
//        {
//            this.targetKey = targetKey;
//        }
        
//        public void OpenVNC(int camIndex, int clientIndex, string ipAddress)
//        {
//            VNCProcess vncProcess = vncProcessList.Find(f=>f.CamIndex == camIndex && f.ClientIndex == clientIndex);
//            if (vncProcess != null)
//            {
//                if (vncProcess.Process.HasExited == false)
//                {
//                    IntPtr procHandler = FindWindow(null, vncProcess.Process.MainWindowTitle);
//                    // 활성화
//                    ShowWindow(procHandler, SW_SHOWNORMAL);
//                    SetForegroundWindow(procHandler);
//                    return;
//                }
//            }
//            vncProcess = new VNCProcess(camIndex, clientIndex, targetKey);
//            vncProcess.Process = new System.Diagnostics.Process();
//            vncProcess.Process.StartInfo.FileName = UniScanGSettings.Instance().VncPath;
//            vncProcess.Process.StartInfo.Arguments = String.Format(@"/password a /autoscaling /quickoption 2 {0}", ipAddress);

//            try
//            {
//                vncProcess.Process.Start();
//                vncProcessList.Add(vncProcess);
//                if (targetKey != null)
//                {
//                    //MonitoringServer server = (SystemManager.Instance().MachineIf as MonitoringServer);
//                    //server.SendTabDisable(camIndex, clientIndex, targetKey, false);
//                }
//            }
//            catch
//            {
//                Cursor.Current = Cursors.Default;
//                System.Windows.MessageBox.Show("Can't start VNC Viewer!");
//                vncProcess.Process = null;
//            }
//        }
        
//        public void Close()
//        {
//            foreach (VNCProcess vncProcess in vncProcessList)
//            {
//                vncProcess.Close();
//            }

//            while (vncProcessList.Count != 0)
//            {
//                VNCProcess removeVncProcess = null;
//                foreach (VNCProcess vncProcess in vncProcessList)
//                {
//                    if (vncProcess.WorkingThread.ThreadState != ThreadState.Running)
//                        removeVncProcess = vncProcess;
//                }

//                if (removeVncProcess != null)
//                    vncProcessList.Remove(removeVncProcess);

//                Thread.Sleep(100);
//            }
//        }
//    }
//}
