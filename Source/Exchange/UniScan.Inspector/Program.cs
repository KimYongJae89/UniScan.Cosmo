using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniEye.Base.Util;
using UniScan.Common;
using UniScan.Common.Settings;
using UniScan.Inspector.Settings.Inspector;
using UniScan.Inspector.UI;

namespace UniScan.Inspector
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
            Mutex mutex = new Mutex(true, "UniScanInsp", out bNew);
            if (bNew)
            {
                Application.SetCompatibleTextRenderingDefault(false);
                Application.EnableVisualStyles();

                InspectorConfigHelper.Instance().BuildSystemManager();

                SystemTypeSettings.Instance().Load();
                InspectorSystemSettings.Instance().Load();
                
                ApplicationHelper.LoadStyleLibrary();
                ApplicationHelper.LoadSettings();
                ApplicationHelper.InitLogSystem();

                LockFile lockFile = UniEye.Base.ProgramCommon.CreateLockFile(PathSettings.Instance().Temp);
                if (lockFile.IsLocked == false)
                    return;

                PathManager.DataPathType = DataPathType.Model_Day_Time;
                ErrorManager.Instance().LoadErrorList(PathSettings.Instance().Config);
                
                LogHelper.Info(LoggerType.Operation, "Start Up");
                //if (ApplicationHelper.CheckLicense() == false)
                //    return;

                ApplicationHelper.InitStringTable();
                ApplicationHelper.InitAuthentication();

                //BuildSystemManager();

                LogHelper.Debug(LoggerType.StartUp, "Start Setup.");

                if (InspectorConfigHelper.Instance().Setup() == true)
                {
                    SystemManager.Instance().ProductionManager.Load();
                    //int resultStoringDays = OperationSettings.Instance().ResultStoringDays;
                    //if (resultStoringDays > 0)
                    //{
                    //    SystemManager.Instance().DataRemover = new DynMvp.Data.DataRemover(DynMvp.Data.DataStoringType.Date, PathSettings.Instance().Result, resultStoringDays, "yyyy-MM-dd", true);
                    //    SystemManager.Instance().DataRemover.Start();
                    //}

                    LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");

                    Form mainForm = InspectorConfigHelper.Instance().GetMainForm();
                    Application.Run(mainForm);
                }

                LogHelper.Debug(LoggerType.StartUp, "Terminating Program.");
                AlgorithmPool.Instance().Dispose();

                if (SystemManager.Instance() != null)
                    SystemManager.Instance().Release();

                lockFile.Dispose();
                LogHelper.Debug(LoggerType.StartUp, "Program Terminated");

                Application.ExitThread();
                Environment.Exit(0);
            }
        }
    }
}
