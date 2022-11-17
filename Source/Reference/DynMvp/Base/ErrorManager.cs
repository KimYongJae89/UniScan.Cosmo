using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynMvp.Base
{
    public class ErrorItem
    {
        int errorCode;
        public int ErrorCode
        {
            get { return errorCode; }
        }

        string sectionStr;
        public string SectionStr
        {
            get { return sectionStr; }
        }

        string errorStr;
        public string ErrorStr
        {
            get { return errorStr; }
        }

        string message;
        public string Message
        {
            get { return message; }
        }

        string resasonMsg;
        public string ResasonMsg
        {
            get { return resasonMsg; }
        }

        string solutionMsg;
        public string SolutionMsg
        {
            get { return solutionMsg; }
        }

        ErrorLevel errorLevel;
        public ErrorLevel ErrorLevel
        {
            get { return errorLevel; }
        }

        bool displayed;
        public bool Displayed
        {
            get { return displayed; }
            set { displayed = value; }
        }

        bool alarmed;
        public bool Alarmed
        {
            get { return alarmed; }
            set { alarmed = value; }
        }

        DateTime errorTime;
        public DateTime ErrorTime
        {
            get { return errorTime; }
        }

        public ErrorItem(int errorCode, ErrorLevel errorLevel, string sectionStr, string errorStr, string message, string resasonMsg = "", string solutionMsg = "")
        {
            errorTime = DateTime.Now;
            this.errorCode = errorCode;
            this.errorLevel = errorLevel;
            this.sectionStr = sectionStr;
            this.errorStr = errorStr;
            this.message = message;
            this.resasonMsg = resasonMsg;
            this.solutionMsg = solutionMsg;
            displayed = false;
            alarmed = true;
        }

        public ErrorItem(bool isAlarmed, string str)
        {
            Alarmed = isAlarmed;
            message = str;
        }

        public ErrorItem(string valueStr)
        {
            SetData(valueStr);
        }

        public void SetData(string valueStr)
        {
            string[] tokens = valueStr.Split(';');
            try
            {
                errorTime = DateTime.Parse(tokens[0]);
                errorCode = Convert.ToInt32(tokens[1]);
                errorLevel = (ErrorLevel)Enum.Parse(typeof(ErrorLevel), tokens[2]);
                sectionStr = tokens[3];
                errorStr = tokens[4];
                message = tokens[5];
                resasonMsg = tokens[6];
                solutionMsg = tokens[7];

                displayed = true;
            }
#if DEGUG == true
            catch (FormatException)
            { }
#endif
            finally { }
        }

        public override string ToString()
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6};{7}",
                errorTime.ToString("yyyy/MM/dd HH:mm:ss"), errorCode, errorLevel, sectionStr, errorStr,
                message, resasonMsg, solutionMsg);
        }

        public override bool Equals(object obj)
        {
            ErrorItem errorItem = obj as ErrorItem;
            if (errorItem == null)
                return base.Equals(obj);

            return errorItem.errorCode == this.errorCode && errorItem.errorLevel == this.errorLevel;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class AlarmException : ApplicationException
    {
        ErrorSection errorSection;
        ErrorSubSection errorType;
        ErrorLevel errorLevel;
        string sectionStr;
        string errorStr;
        string message;
        string reasonMsg;
        string solutionMsg;

        public AlarmException(ErrorSection errorSection, ErrorSubSection errorType, ErrorLevel errorLevel, string sectionStr, string errorStr, string message, string reasonMsg, string solutionMsg ) : base(message)
        {
            this.errorSection = errorSection;
            this.errorType = errorType;
            this.errorLevel = errorLevel;
            this.sectionStr = sectionStr;
            this.errorStr = errorStr;
            this.message = message;
            this.reasonMsg = reasonMsg;
            this.solutionMsg = solutionMsg;
            
            LogHelper.Error(LoggerType.Error, string.Format("[E{0}] {1}", ErrorManager.GetErrorCode(errorSection, errorType), message));
        }

        public AlarmException(ErrorSection errorSection, ErrorSubSection errorType, ErrorLevel errorLevel, string sectionStr, string errorStr, string message) : base(message)
        {
            this.errorSection = errorSection;
            this.errorType = errorType;
            this.errorLevel = errorLevel;
            this.sectionStr = sectionStr;
            this.errorStr = errorStr;
            this.message = message;
            this.reasonMsg = "";
            this.solutionMsg = "";

            LogHelper.Error(LoggerType.Error, string.Format("[E{0}] {1}", ErrorManager.GetErrorCode(errorSection, errorType), message));
        }

        public AlarmException(ErrorSection errorSection, ErrorSubSection errorType, ErrorLevel errorLevel, string message) : base(message)
        {
            this.errorSection = errorSection;
            this.errorType = errorType;
            this.errorLevel = errorLevel;
            this.sectionStr = errorSection.ToString();
            this.errorStr = errorType.ToString();
            this.message = message;
            this.reasonMsg = "";
            this.solutionMsg = "";

            LogHelper.Error(LoggerType.Error, string.Format("[E{0}] {1}", ErrorManager.GetErrorCode(errorSection, errorType), message));
        }

        public void Report()
        {
            ErrorManager.Instance().Report(this.errorSection, this.errorType, this.errorLevel, this.sectionStr, this.errorStr, this.message, this.reasonMsg, this.solutionMsg);
        }
    }

    public delegate void AlarmEventDelegate();
    
    public class ErrorManager
    {
        static ErrorManager instance = null;

        public event AlarmEventDelegate OnStartAlarmState;
        public event AlarmEventDelegate OnResetAlarmState;
        public event AlarmEventDelegate OnStopRunProcess;

        internal void StopProcess()
        {
            foreach (ErrorItem errorItem in errorItemList)
            {
                errorItem.Alarmed = false;
            }

            if (OnStopRunProcess != null)
                OnStopRunProcess();
        }

        string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        bool buzzerOn;
        public bool BuzzerOn
        {
            get { return buzzerOn; }
            set { buzzerOn = value; }
        }

        List< ErrorItem> errorItemList = new List<ErrorItem>();
        public List<ErrorItem> ErrorItemList
        {
            get { return errorItemList; }
        }        

        object lockObject = new object();

        private ErrorManager()
        {

        }

        public static ErrorManager Instance()
        {
            if (instance == null)
            {
                instance = new ErrorManager();
            }

            return instance;
        }

        public static int GetErrorCode(ErrorSection errorSection, ErrorSubSection errorType)
        {
            return ((int)errorSection + (int)errorType);
        }

        public void ResetAlarm()
        {
            if (OnResetAlarmState != null)
                OnResetAlarmState();

            lock (errorItemList)
            {
                List<ErrorItem> alarmedList = errorItemList.FindAll(f => f.Alarmed);
                if (alarmedList.Count == 0)
                    return;

                alarmedList.ForEach(f => f.Alarmed = false);

                this.buzzerOn = true;
            }
        }

        public bool IsAlarmed()
        {
            lock (errorItemList)
                return errorItemList.Any(item => item.Alarmed);
        }

        public void ThrowIfAlarm()
        {
            if (IsAlarmed())
                throw new Exception();
        }

        public bool IsError()
        {
            lock (errorItemList)
                return errorItemList.Any(item => item.ErrorLevel == ErrorLevel.Error);
        }

        public bool IsWarning()
        {
            lock (errorItemList)
                return errorItemList.Any(item => item.ErrorLevel == ErrorLevel.Warning);
        }

        public void Report(ErrorSection errorSection, ErrorSubSection errorType, ErrorLevel errorLevel, string message, string reasonMsg = "", string solutionMsg = "")
        {
            Report(errorSection, errorType, errorLevel, errorSection.ToString(), errorType.ToString(), message, reasonMsg, solutionMsg);
        }

        public void Report(int errorSection, int errorType, ErrorLevel errorLevel, string sectionStr, string errorStr, string message, string reasonMsg = "", string solutionMsg = "")
        {
            Report(errorSection + errorType, errorLevel, sectionStr, errorStr, message, reasonMsg, solutionMsg);
        }

        public void Report(ErrorSection errorSection, ErrorSubSection errorType, ErrorLevel errorLevel, string sectionStr, string errorStr, string message, string reasonMsg = "", string solutionMsg = "")
        {
            Report(GetErrorCode(errorSection, errorType), errorLevel, sectionStr, errorStr, message, reasonMsg, solutionMsg);
        }

        protected void Report(int errorCode, ErrorLevel errorLevel, string sectionStr, string errorStr, string message, string reasonMsg = "", string solutionMsg = "")
        {
            lock (lockObject)
            {
                ErrorItem foundErrorItem = errorItemList.Find(item => item.ErrorCode == errorCode && item.ErrorLevel == errorLevel);
                if (foundErrorItem != null)
                {
                    TimeSpan elapsedTime = DateTime.Now - foundErrorItem.ErrorTime;
                    if (elapsedTime.TotalMilliseconds < 100)
                        return;
                }

                this.buzzerOn = true;
                lock (errorItemList)
                    errorItemList.Add(new ErrorItem(errorCode, errorLevel, sectionStr, errorStr, message, reasonMsg, solutionMsg));

                if (errorLevel != ErrorLevel.Warning)
                    Task.Run(() => OnStartAlarmState?.Invoke());

                SaveErrorList();
            }
        }

        public void LoadErrorList(string configPath)
        {
            fileName = configPath + "\\ErrorList.txt";
            if (File.Exists(fileName) == false)
                return;

            string[] stringLines = File.ReadAllLines(fileName);

            foreach (string line in stringLines)
            {
                ErrorItem errorItem = new ErrorItem(line);
                errorItemList.Add(errorItem);
            }
        }

        public void SaveErrorList()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (ErrorItem errorItem in errorItemList)
            {
                stringBuilder.Append(errorItem.ToString());
                stringBuilder.AppendLine();
            }

            if (string.IsNullOrEmpty(fileName))
                return;

            File.WriteAllText(fileName, stringBuilder.ToString());
        }
    }
}
