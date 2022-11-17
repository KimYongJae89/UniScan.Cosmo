using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Util;
using UniEye.Base.Settings;
using DynMvp.Base;
using System.IO;
using UniScan.Common.Settings;
using UniEye.Base.Data;
using DynMvp.Data;
using UniEye.Base.MachineInterface;
using UniScan.Monitor.UI;
using UniScan.Monitor.Exchange;
using UniScan.Common;
using DynMvp.Vision;
using DynMvp.Devices;
using UniScan.Monitor.Settings.Monitor;

namespace UniScan.Monitor
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
            Mutex mutex = new Mutex(true, "UniScanMon", out bNew);
            if (bNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                MonitorConfigHelper.Instance().BuildSystemManager();

                SystemTypeSettings.Instance().Load();
                MonitorSystemSettings.Instance().Load();
                
                ApplicationHelper.LoadStyleLibrary();
                ApplicationHelper.LoadSettings();
                
                PathManager.DataPathType = DataPathType.Model_Day_Time;

                ErrorManager.Instance().LoadErrorList(PathSettings.Instance().Config);

                LockFile lockFile = UniEye.Base.ProgramCommon.CreateLockFile(PathSettings.Instance().Temp);
                if (lockFile.IsLocked == false)
                    return;

                ApplicationHelper.InitLogSystem();

                LogHelper.Info(LoggerType.Operation, "Start Up");

                ApplicationHelper.InitStringTable();
                ApplicationHelper.InitAuthentication();

                LogHelper.Debug(LoggerType.StartUp, "Start Setup.");

                if (MonitorConfigHelper.Instance().Setup() == true)
                {
                    LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");
                    SystemManager.Instance().ProductionManager.Load();

                    Application.Run(MonitorConfigHelper.Instance().GetMainForm());
                }

                LogHelper.Debug(LoggerType.StartUp, "Terminating Program.");
                AlgorithmPool.Instance().Dispose();

                SystemManager.Instance()?.Release();

                lockFile.Dispose();

                LogHelper.Debug(LoggerType.StartUp, "Program Terminated");
                Application.ExitThread();
                Environment.Exit(0);
            }
        }
    }
}
