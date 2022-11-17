using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniEye.Base.Util;

namespace UniScan.Watch
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationHelper.LoadStyleLibrary();
            ApplicationHelper.LoadSettings();

            //PathManager.DataPathType = DataPathType.Model_Day_Time;

            ErrorManager.Instance().LoadErrorList(PathSettings.Instance().Config);

            LockFile lockFile = UniEye.Base.ProgramCommon.CreateLockFile(PathSettings.Instance().Temp);
            if (lockFile.IsLocked == false)
                return;

            ApplicationHelper.InitLogSystem();

            LogHelper.Info(LoggerType.Operation, "Start Up");

            ApplicationHelper.InitStringTable();
            ApplicationHelper.InitAuthentication();


            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
