using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using System.Security.Principal;
using System.Diagnostics;
using System.Windows.Forms;

namespace UniEye.Base.Util
{
    public static class ApplicationHelper
    {
        public static bool CheckLicense()
        {
            string vendorCode =
                "6VA968FjuJZ9cWlNImhn0fNYeNLGq7bia6jkjtCUaBxdIzu6W7zUZNeQkvhu1Gd9y0rWaEYNimHuNLcl" +
                "Tctl0t8D9d/WIJqH9Vr4X+fX9As+ch9qKy204l53j6pgwv/kPsqbuM8tcBlixMyxevpNTAgRk+k2nZYG" +
                "41Hlds7yBOzAMG+u7jSuesxItBZ3bTKsihbI8TAv3H6CZX1q5EPl/dInETCK1oUgnOj2yFzQYpIx3/jW" +
                "0o9EyvV6FNXzPvd2tNmmsqckZv2l5s95jyxL9REA4O9PxCkxT2mzO0SSK5NBjO6Gqg0cafVYXMwXjI7l" +
                "RgVV0xDipgPLdnklujau4U341UzX2CNm/hif13GIsujzpVRAKNAkvqwIqfcoYUhVJepI0Bj9hjkXJmN4" +
                "cK8Ag+Swr3a7aDWjjUQMDkJ3v8+N+GonXArEiw6pTRnHjAed2Er/kVrRSJ1k18xlwPbmswc1ANsTTFTI" +
                "rjwHMAlI1nKwGoHFeGJQq1K+pa6bte+yCd6SjnAaK2DihNHbGMTvGHIrNC2v27hxcOc8HMAlRXxRTF11" +
                "Zk4lvA5PAj6VfWyvE9EaE0Lk6EdIkmO6s7O6fJTBFZO8PxVBSck6mBbYTddRHKDfaRPeKlALz/fk9YOt" +
                "tmLwfciybMk77LyTFmM0FqCnZF5t//3ucJYumY6qw4esnxgTenSmTBOztXn7RL3rK8acbcnrMUGXgOJz" +
                "8wSYzmarN0DMECjW6OnefyvjtCjKMMJRCVZ3epQrCu19+ckOSJTBMj88dOSWur+lM/swNrFf6pR9enzu" +
                "ioR6zGHqn6JO+gzPh5kI3XxBb9UvAj92mZKplz+q13mI2AzppAJ1UyFzlgEa3Yp1iLyObmh9hvHNgm1d" +
                "5g4BBG7+4jQB019JU/oi6qQVYETYA23CSFQFzNwl5vMB2F2I8uCb9AVfpvDZxjca31ZksjDGoYgtYWqh" +
                "cuZME1ZDpzrvdnhqhPmWqQ==";


//#if !DEBUG
            if (RegistryHelper.IsValidSoftwareKey())
                return true;

            
            HaspHelper haspHelper = new HaspHelper();
            int featureCode = haspHelper.GetFeatureCode(vendorCode);

            if (featureCode == -1)
                return false;

            //            Settings.Instance().Operation.SystemType = (SystemType)featureCode;
//#endif
            return true;
        }

        public static void InitAuthentication()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init User.");

            string configPath = PathSettings.Instance().Config;

            string fileName = String.Format("{0}\\UserList.dat", configPath);
            if (File.Exists(fileName))
            {
                UserHandler.Instance().Initialize(fileName);
            }

#if DEBUG
            UserHandler.Instance().CurrentUser = UserHandler.Instance().GetUser("developer");
            //UserHandler.Instance().CurrentUser = UserHandler.Instance().GetUser("op");
#else
            UserHandler.Instance().CurrentUser = UserHandler.Instance().GetUser("op");
            //UserHandler.Instance().CurrentUser = UserHandler.Instance().GetUser("developer");
#endif

            string taskTableName = String.Format("{0}\\taskTable.xml", configPath);
            if (File.Exists(taskTableName))
                TaskAuthManager.Instance().LoadTaskAuthTable(taskTableName);
        }

        public static void LoadStyleLibrary()
        {
            // Load the style library
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string styleLibraryPath = new List<string>(assembly.GetManifestResourceNames()).Find(i => i.EndsWith(".isl"));
            if (string.IsNullOrEmpty(styleLibraryPath) == false)
            {
                using (System.IO.Stream stream = assembly.GetManifestResourceStream(styleLibraryPath))
                {
                    if (stream != null)
                        Infragistics.Win.AppStyling.StyleManager.Load(stream);
                }
            }
        }

        public static void InitStringTable()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init StringTable.");
            string localeCode = OperationSettings.Instance().GetLocaleCode();
            string configPath = PathSettings.Instance().Config;

#if DEBUG
            string debugConfigPath = @"D:\Project_UniScan\UniScan\Runtime\Config";
            if (Directory.Exists(debugConfigPath))
            {
                StringManager.Clear();
                bool ok = StringManager.Load(debugConfigPath, localeCode);
                if (ok)
                    return;
            }
#endif
            StringManager.Clear();
            StringManager.Load(configPath, localeCode);
            //StringManager.AddStringTable(stringTable);
        }

        public static void LoadSettings()
        {
            PathSettings.Instance().Load();
            OperationSettings.Instance().Load();
            CustomizeSettings.Instance().Load();
            MachineSettings.Instance().Load();
            //dditionalSettings.Instance().Load();
        }

        public static bool InitLogSystem()
        {
            string logPath = PathSettings.Instance().Log;
            string[] logFiles = Directory.GetFiles(logPath, "*.log");
            if (logFiles.Length > 0)
            {
                string bakupPath = Path.Combine(logPath, DateTime.Now.ToString("yyyyMMddHHmmss"));
                try
                {
                    Directory.CreateDirectory(bakupPath);
                    string[] files = Directory.GetFiles(logPath);
                    Array.ForEach(files, f => File.Move(f, Path.Combine(bakupPath, Path.GetFileName(f))));
                }
                catch (Exception e)
                {
                    MessageBox.Show(string.Format(StringManager.GetString("Error on move debug log. [{0}]"), e.Message));
                    return false;
                }
            }

            string logConfigFile = String.Format("{0}\\log4net.xml", PathSettings.Instance().Config);
#if DEBUG
            string debugLogConfigFile = String.Format("{0}\\log4net.xml", @"D:\Project_UniScan\UniScan\Runtime\Config");
            if (File.Exists(debugLogConfigFile))
                logConfigFile = debugLogConfigFile;
#endif
            if (File.Exists(logConfigFile) == false)
            {
                MessageForm.Show(null, "Can't find log configuration file.\n\n" + logConfigFile);
                return false;
            }

            LogHelper.InitializeLogSystem(logConfigFile);
            LogLevel logLevel = OperationSettings.Instance().LogLevel;
            LogHelper.ChangeLevel(OperationSettings.Instance().LogLevel.ToString());

            return true;
        }

        public static bool IsAdmin()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (null != identity)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return false;
        }

        public static bool RunAsAdmin()
        {
            if (IsAdmin() == false)
            {
                try
                {
                    ProcessStartInfo procInfo = new ProcessStartInfo();
                    procInfo.UseShellExecute = true;
                    procInfo.FileName = Application.ExecutablePath;
                    procInfo.WorkingDirectory = Environment.CurrentDirectory;
                    procInfo.Verb = "runas";
                    Process.Start(procInfo);

                    return true;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            return false;
        }

        public static void KillProcesses()
        {
            List<Process> uniScanList = new List<Process>();

            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.Contains("UniEye") || process.ProcessName.Contains("UniScan"))
                    uniScanList.Add(process);
            }

            if (uniScanList.Count > 1)
            {
                uniScanList = uniScanList.OrderByDescending(p => p.StartTime).ToList();
                uniScanList.Remove(uniScanList[0]);

                uniScanList.ForEach(u => u.Kill());
            }
        }
    }
}
