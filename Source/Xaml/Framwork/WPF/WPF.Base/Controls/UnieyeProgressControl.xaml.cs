using DynMvp.UI;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPF.Base.Helpers;

namespace WPF.Base.Controls
{
    /// <summary>
    /// UnieyeProgressControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UnieyeProgressControl : ChildWindow, INotifyPropertyChanged
    {
        #region Observable

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        #endregion

        public delegate bool ProgressActionDelegate(IReportProgress report);
        public ProgressActionDelegate ProgressAction;

        private string titleText;
        public string TitleText
        {
            get => titleText;
            set => Set(ref titleText, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => Set(ref description, value);
        }

        private string percentText;
        public string PercentText
        {
            get => percentText;
            set => Set(ref percentText, value);
        }

        private bool isComplete = false;
        public bool IsComplete
        {
            get => isComplete;
            set => Set(ref isComplete, value);
        }

        private bool isCanceled = false;
        public bool IsCanceled
        {
            get => isCanceled;
            set => Set(ref isCanceled, value);
        }

        private bool isShowProgressRing = false;
        public bool IsShowProgressRing
        {
            get => isShowProgressRing;
            set => Set(ref isShowProgressRing, value);
        }

        private bool isShowCompleteImage = false;
        public bool IsShowCompleteImage
        {
            get => isShowCompleteImage;
            set => Set(ref isShowCompleteImage, value);
        }

        private bool isShowCancelButton = false;
        public bool IsShowCancelButton
        {
            get => isShowCancelButton;
            set => Set(ref isShowCancelButton, value);
        }
        private bool isShowCloseButton = false;
        public bool IsShowCloseButton
        {
            get => isShowCloseButton;
            set => Set(ref isShowCloseButton, value);
        }

        //public bool IsShowProgressRing { get => !IsComplete && !IsCanceled; }
        //public bool IsShowCompleteImage { get => IsComplete && !IsCanceled; }
        //public bool IsShowCancelButton { get => cancellationToken != null && !IsShowCloseButton; }
        //public bool IsShowCloseButton { get => IsComplete || IsCanceled; }

        private object progressContent;
        public object ProgressContent
        {
            get => progressContent;
            set => Set(ref progressContent, value);
        }

        private ICommand cancelCommand;
        public ICommand CancelCommand { get => (cancelCommand ?? (cancelCommand = new RelayCommand(CancelAction))); }

        private void CancelAction()
        {
            cancellationToken.Cancel();
        }

        private ICommand closeCommand;
        public ICommand CloseCommand { get => (closeCommand ?? (closeCommand = new RelayCommand(CloseAction))); }

        private void CloseAction()
        {
            Close();
        }

        DispatcherTimer timer = new DispatcherTimer();
        public UnieyeProgressControl()
        {
            Initialize();
        }

        int currentActionNo;
        int maxActionCount;
        bool IsAutoClose { get; set; } =false;

        Task actionTask;
        CancellationTokenSource cancellationToken;
        Queue<Action> actionQueue = new Queue<Action>();

        public UnieyeProgressControl(string title, string _description, Action action, CancellationTokenSource token = null, bool isAutoClose = false)
        {
            Initialize();

            TitleText = title;
            Description = _description;
            IsAutoClose = isAutoClose;

            currentActionNo = 0;
            maxActionCount = 1;

            actionQueue.Enqueue(action);

            cancellationToken = token;
        }

        private void Initialize()
        {
            InitializeComponent();

            DataContext = this;
            IsComplete = false;

            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += ProgressTimer;
            timer.Start();
        }

        public UnieyeProgressControl(string title, string _description, List<Action> actionList, CancellationTokenSource token=null, bool isAutoClose = false)
        {
            Initialize();

            TitleText = title;
            Description = _description;
            IsAutoClose = isAutoClose;

            currentActionNo = 0;
            maxActionCount = actionList.Count;

            foreach (var action in actionList)
                actionQueue.Enqueue(action);

            cancellationToken = token;
        }

        private void ProgressTimer(object sender, EventArgs e)
        {
            if (cancellationToken != null && cancellationToken.IsCancellationRequested)
            {
                PercentText = string.Format("Canceled");
                IsCanceled = true;
                timer.Stop();
            }
            else
            {
                string header = "Running";

                if (actionTask == null)
                {
                    Action action = actionQueue.Dequeue();
                    if (action != null)
                        actionTask = Task.Run(action);
                }
                else if (actionTask.IsCompleted)
                {
                    currentActionNo++;
                    actionTask = null;
                }

                if (currentActionNo == maxActionCount)
                {
                    IsComplete = true;
                    timer.Stop();
                    header = "Complete";
                    if(IsAutoClose)
                        Close();
                }

                PercentText = string.Format("{0} ... {1:0} %", header, Convert.ToDouble(currentActionNo / (double)maxActionCount) * 100);
            }

            IsShowProgressRing = !IsComplete && !IsCanceled;
            IsShowCompleteImage = IsComplete && !IsCanceled;
            IsShowCloseButton = (IsComplete || IsCanceled) && !IsAutoClose;
            IsShowCancelButton = cancellationToken != null && !IsShowCloseButton;
        }
    }
}
