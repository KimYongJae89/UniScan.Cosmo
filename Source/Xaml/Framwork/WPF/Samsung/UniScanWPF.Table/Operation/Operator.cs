using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml;
using UniEye.Base.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation
{
    public enum ResultType
    {
        LightTune, Scan, Extract, Inspect, Train
    }

    public enum OperatorState
    {
        Run, Wait, Idle, Pause
    }

    public abstract class Operator : INotifyPropertyChanged
    {
        protected const float resizeRatio = 0.2f;
        public static float ResizeRatio { get => resizeRatio; }

        protected ResultKey resultKey;

        protected CancellationTokenSource cancellationTokenSource;

        OperatorState operatorState;

        SolidColorBrush runBrush = new SolidColorBrush(Colors.LightGreen);
        SolidColorBrush waitBrush = new SolidColorBrush(Colors.CornflowerBlue);
        SolidColorBrush idleBrush = new SolidColorBrush(Colors.LightGray);
        SolidColorBrush pauseBrush = new SolidColorBrush(Colors.Gold);

        public event PropertyChangedEventHandler PropertyChanged;

        public SolidColorBrush StateBrush
        {
            get
            {
                switch (operatorState)
                {
                    case OperatorState.Run:
                        return runBrush;
                    case OperatorState.Wait:
                        return waitBrush;
                    case OperatorState.Idle:
                        return idleBrush;
                    case OperatorState.Pause:
                        return pauseBrush;
                }

                return null;
            }
        }

        public OperatorState OperatorState
        {
            get => operatorState;
            set
            {
                if (operatorState != value)
                {
                    operatorState = value;
                    OnPropertyChanged("StateBrush");
                    OnPropertyChanged("OperatorState");

                    Table.Data.InfoBox.Instance.OperatorStateChanged();
                }
            }
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public Operator()
        {
            OperatorState = OperatorState.Idle;
        }

        public virtual void Release()
        {
            OperatorState = OperatorState.Idle;
        }

        public virtual bool Initialize(ResultKey resultKey, CancellationTokenSource cancellationTokenSource)
        {
            this.resultKey = resultKey;
            this.cancellationTokenSource = cancellationTokenSource;
            OperatorState = OperatorState.Wait;
           
            return true;
        }

        protected DebugContext GetDebugContext(string subPath = null)
        {
            string fullPath = PathSettings.Instance().Temp;
            //fullPath = @"C:\temp";
            if (string.IsNullOrEmpty(subPath) == false)
                fullPath = Path.Combine(fullPath, subPath);
            //Directory.CreateDirectory(fullPath);
            return new DebugContext(OperationSettings.Instance().SaveDebugImage, fullPath);
        }
    }

    public abstract class OperatorResult
    {
        ResultType type;
        ResultKey resultKey;
        protected string exceptionMessage;

        public string ExceptionMessage { get => exceptionMessage; }
        public ResultType Type { get => type; }
        public ResultKey ResultKey { get => resultKey; }

        public OperatorResult(ResultType type, ResultKey resultKey, string exceptionMessage)
        {
            this.type = type;
            this.resultKey = resultKey;
            this.exceptionMessage = exceptionMessage;
        }
    }

    public abstract class OperatorSettings : ISavableObj
    {
        protected string fileName;

        public OperatorSettings()
        {
            Initialize();
            Load();
        }

        public void Load(string fileName = "")
        {
            bool ok = false;
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    fileName = this.fileName;

                XmlDocument xmlDocument = XmlHelper.Load(fileName);
                if (xmlDocument == null)
                    return;

                XmlElement operationElement = xmlDocument["Settings"];
                if (operationElement == null)
                    return;

                this.Load(operationElement);
                ok = true;
            }
            finally
            {
                //if (ok == false)
                //    Save();
            }
        }

        public void Save(string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = this.fileName;

            string superDirectory = Path.GetDirectoryName(fileName);
            if (Directory.Exists(superDirectory) == false)
                Directory.CreateDirectory(superDirectory);

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement operationElement = xmlDocument.CreateElement("Settings");
            xmlDocument.AppendChild(operationElement);

            this.Save(operationElement);

            xmlDocument.Save(fileName);
        }

        protected abstract void Initialize();
        public abstract void Load(XmlElement xmlElement);
        public abstract void Save(XmlElement xmlElement);
    }
}
