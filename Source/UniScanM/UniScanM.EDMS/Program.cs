﻿using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.Device;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniEye.Base.Util;

namespace UniScanM.EDMS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool bNew;
            Mutex mutex = new Mutex(true, "UniEye", out bNew);
            if (bNew)
            {
                string paths = System.Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
                string[] pp = paths.Split(';');

                System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                try
                {
                    ApplicationHelper.KillProcesses();

                    BuildSystemManager();

                    ApplicationHelper.LoadStyleLibrary();
                    ApplicationHelper.LoadSettings();

                    PathManager.DataPathType = DataPathType.Model_Day_Time;

                    ErrorManager.Instance().LoadErrorList(PathSettings.Instance().Config);

                    ApplicationHelper.InitLogSystem();

                    LockFile lockFile = UniEye.Base.ProgramCommon.CreateLockFile(PathSettings.Instance().Temp);
                    if (lockFile.IsLocked == false)
                        return;

                    LogHelper.Info(LoggerType.Operation, "Start Up");

                    ApplicationHelper.InitStringTable();
                    ApplicationHelper.InitAuthentication();

                    //BuildSystemManager();

                    LogHelper.Debug(LoggerType.StartUp, "Start Setup.");

                    if (SystemManager.Instance().Setup() == true)
                    {
                        LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");
                        SystemManager.Instance().ProductionManager.Load();

                        IMainForm mainForm = SystemManager.Instance().UiChanger.CreateMainForm();
                        SystemManager.Instance().MainForm = mainForm;
                        SystemManager.Instance().LoadDefaultModel();

                        Screen[] screens = Screen.AllScreens;
                        if (screens.Count() > 2)
                        {
                            Rectangle bounds = screens[2].Bounds;
                            ((Form)mainForm).Location = new Point(bounds.X, bounds.Y);
                            ((Form)mainForm).StartPosition = FormStartPosition.Manual;
                        }

                        Application.Run((Form)mainForm);
                    }

                    SystemManager.Instance().Release();

                    lockFile.Dispose();

                    Application.ExitThread();
                    Environment.Exit(0);
                }
                catch (DllNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "UniScan", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        public static void BuildSystemManager()
        {
            SystemManager systemManager = new SystemManager();
            SystemManager.SetInstance(systemManager);

            Type[] type = new Type[2] { typeof(UniScanM.MachineIF.UniScanMMachineIfCommonCommand), typeof(UniScanM.EDMS.MachineIF.UniScanMMachineIfEDMSCommand) };
            MachineIfProtocolList machineIfProtocolList = new UniScanM.EDMS.MachineIF.MachineIfProtocolList(type);
            systemManager.Init(
                new UniScanM.EDMS.Data.ModelManager(),
                new UniScanM.EDMS.UI.UiChanger(),
                new AlgorithmArchiver(),
                new DeviceBox(new PortMap()),
                new DeviceController(),
                new UniScanM.Data.ProductionManager(PathSettings.Instance().Result),
                machineIfProtocolList);
        }
    }
}
