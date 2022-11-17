using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UniEye.Base.Settings;
using UniEye.Base.Util;
using UniScanWPF.Settings;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool bNew;
            Mutex mutex = new Mutex(true, "UniEye", out bNew);

            if (bNew)
            {
                try
                {
                    App.Current.MainWindow = new MainWindow();

                    ApplicationHelper.LoadSettings();

                    string logConfigFile = String.Format("{0}\\log4net.xml", PathSettings.Instance().Config);
                    if (File.Exists(logConfigFile) == true)
                    {
                        LogHelper.InitializeLogSystem(logConfigFile);
                        LogLevel logLevel = OperationSettings.Instance().LogLevel;
                        logLevel = LogLevel.Debug;
                        LogHelper.ChangeLevel(OperationSettings.Instance().LogLevel.ToString());
                    }
                    else
                    {
                        CustomMessageBox.Show("Can't find log configuration file", "UniScan", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    LogHelper.Info(LoggerType.StartUp, "Start Up");

                    InitStringTable();
                    ApplicationHelper.InitAuthentication();

                    ConfigHelper configHelper = new UniScanWPF.Table.Settings.ConfigHelper();
                    if (configHelper.Setup() == true)
                    {
                        App.Current.MainWindow.Show();
                        LogHelper.Debug(LoggerType.StartUp, "Finish Setup.");
                    }
                    else
                    {
                        Application.Current.Shutdown();
                    }
                }
                catch (DllNotFoundException ex)
                {
                    CustomMessageBox.Show(ex.Message, "UniScan", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
            }
            else
            {
                CustomMessageBox.Show("Application already started.", "UniScan", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        public void InitStringTable()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init StringTable.");
            string localeCode = OperationSettings.Instance().GetLocaleCode();
            string configPath = PathSettings.Instance().Config;

            string fileName;
            if (localeCode == "")
                fileName = String.Format("LocalizeHelper.xml", configPath);
            else
                fileName = String.Format("LocalizeHelper_{1}.xml", configPath, localeCode);

#if DEBUG
            string debugConfigPath = @"D:\Project_UniScan\UniScan\Runtime\Config";
            string debugStringTablepath = Path.Combine(debugConfigPath, fileName);
            if (File.Exists(debugStringTablepath))
                configPath = debugConfigPath;
#endif

            LocalizeHelper.Clear();
            LocalizeHelper.Load(Path.Combine(configPath, fileName));
        }
    }
}
