using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace Standard.DynMvp.Base
{
    public enum LoggerType
    {
        // Program Initialize
        StartUp,    
        Shutdown,

        // Communicate message. ex) timeout, disconnect, ...
        Network,
        Serial,
        IO,

        // H/W message. ex) motion control, light command, ...
        Machine,
        Device,         // 장비 운영 로그 ( Motion / Grab 제외 )

        // S/W message
        UserNotify, // user notify. ex) messageBox
        Operation,  // S/W operation. ex) model load, result save, update control, ...
        Inspection, // inspection algorithm. ex) inspection step trace, ...
        Grab,   // grab step
        ValueChanged,   // user parametor changed
        DataRemover,    // old data removed

        Debug,  // any developer log
        Error,  // dummy
        Function,       // 함수 호출. Start / End
    }

    public enum LogLevel
    {
        Fatal, Error, Warn, Info, Debug,
    }

    public static class ILogExtentions
    {
        public static void Trace(this ILog log, string message, Exception exception)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                log4net.Core.Level.Trace, message, exception);
        }

        public static void Trace(this ILog log, string message)
        {
            log.Trace(message, null);
        }

        public static void Verbose(this ILog log, string message, Exception exception)
        {
            log.Logger.Log(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType,
                log4net.Core.Level.Verbose, message, exception);
        }

        public static void Verbose(this ILog log, string message)
        {
            log.Verbose(message, null);
        }
    }

    public delegate void LogDelegate(string message);

    public interface LoggingTarget
    {
        void Log(string message);
    }

    public class LogHelper
    {
        public static string BackupPathForamt = "yyyyMMddHHmmss";

        static LoggingTarget loggingTarget = null;
        public static LoggingTarget LoggingTarget
        {
            get { return loggingTarget;  }
            set { loggingTarget = value; }
        }

        public static void InitializeLogSystem(string logConfigFile)
        {
            //System.IO.FileInfo fileInfo = new System.IO.FileInfo(logConfigFile);
            //log4net.Config.XmlConfigurator.Configure(fileInfo);    
        }

        public static void Warn(LoggerType loggerType, string message)
        {
            //LogManager.GetLogger(loggerType.ToString()).Warn(message);
            //WriteOutput(LogLevel.Warn, loggerType, message);
            //if (loggingTarget != null)
            //{
            //    loggingTarget.Log(String.Format("{0} [{1}] {2}", DateTime.Now, loggerType.ToString(), message));
            //}
        }

        public static void Info(LoggerType loggerType, string message)
        {
            //LogManager.GetLogger(loggerType.ToString()).Info(message);
            //WriteOutput(LogLevel.Info, loggerType, message);
            //if (loggingTarget != null)
            //{
            //    loggingTarget.Log(String.Format("{0} [{1}] {2}", DateTime.Now, loggerType.ToString(), message));
            //}
        }

        public static void Debug(LoggerType loggerType, string message)
        {
            //LogManager.GetLogger(loggerType.ToString()).Debug(message);
            //WriteOutput(LogLevel.Debug, loggerType, message);
            //if (loggingTarget != null)
            //{
            //    loggingTarget.Log(String.Format("{0} [{1}] {2}", DateTime.Now, loggerType.ToString(), message));
            //}
        }

        public static void Fatal(LoggerType loggerType,string message)
        {
            //LogManager.GetLogger(loggerType.ToString()).Fatal(message);
            //WriteOutput(LogLevel.Fatal, loggerType, message);
            //if (loggingTarget != null)
            //{
            //    loggingTarget.Log(String.Format("{0} [Fatal] {1}", DateTime.Now, message));
            //}
        }

        public static void Error(LoggerType loggerType, string message)
        {
            //LogManager.GetLogger(loggerType.ToString()).Error(message);
            //WriteOutput(LogLevel.Error, loggerType, message);
            //if (loggingTarget != null)
            //{
            //    loggingTarget.Log(message);
            //}
        }

        private static void WriteOutput(LogLevel logLevel, LoggerType loggerType, string message)
        {
#if DEBUG
            //System.Diagnostics.Debug.WriteLine(String.Format("[{0}] {1} [{2}] {3}", logLevel.ToString(), DateTime.Now.ToString("HH:mm:ss.fff"), loggerType.ToString(), message));
#endif
        }

        public static void TestLog()
        {
            //Array array = Enum.GetValues(typeof(LoggerType));
            //foreach (LoggerType type in array)
            //{
            //    LogHelper.Debug(type, string.Format("{0} - DebugLogTest", type));
            //    LogHelper.Info(type, string.Format("{0} - InfoLogTest", type));
            //    LogHelper.Warn(type, string.Format("{0} - WarnLogTest", type));
            //    LogHelper.Error(type, string.Format("{0} - ErrorLogTest", type));
            //    LogHelper.Fatal(type, string.Format("{0} - FatalLogTest", type));
            //}
        }

        public static void ChangeLevel(string logLevel)
        {
            //log4net.Repository.ILoggerRepository[] repositories = log4net.LogManager.GetAllRepositories();

            ////Configure all loggers to be at the debug level.
            //foreach (log4net.Repository.ILoggerRepository repository in repositories)
            //{
            //    repository.Threshold = repository.LevelMap[logLevel];
            //    log4net.Repository.Hierarchy.Hierarchy hier = (log4net.Repository.Hierarchy.Hierarchy)repository;
            //    log4net.Core.ILogger[] loggers = hier.GetCurrentLoggers();
            //    foreach (log4net.Core.ILogger logger in loggers)
            //    {
            //        ((log4net.Repository.Hierarchy.Logger)logger).Level = hier.LevelMap[logLevel];
            //    }
            //}
            
            ////Configure the root logger.
            //log4net.Repository.Hierarchy.Hierarchy h = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            //log4net.Repository.Hierarchy.Logger rootLogger = h.Root;
            //rootLogger.Level = h.LevelMap[logLevel];

            //TestLog();
        }
    }
}
