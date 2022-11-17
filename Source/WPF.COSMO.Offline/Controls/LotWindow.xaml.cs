using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
using WPF.Base.Helpers;
using WPF.Base.Models;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;
using Xceed.Wpf.Toolkit;
using ChildWindow = MahApps.Metro.SimpleChildWindow.ChildWindow;

namespace WPF.COSMO.Offline.Controls
{
    /// <summary>
    /// ModelWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LotWindow : ChildWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        string _lotNo;

        string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        public bool IsSingleMode { get; set; }

        public LotNoCollections Collections => LotNoService.Collections;

        ICommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel));

        ICommand _acceptCommand;
        public ICommand AcceptCommand => _acceptCommand ?? (_acceptCommand = new RelayCommand(Accept));

        public CosmoLotNoInfo CosmoLotNoInfo { get; } = new CosmoLotNoInfo();
        
        public LotWindow()
        {
            DataContext = this;
            InitializeComponent();

            LotNoTextBox.Loaded += LotWindow_Loaded;
        }

        private void LotWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (LotNoTextBox.ActualWidth > 0 && LotNoTextBox.ActualHeight > 0)
            {
                LotNoTextBox.Focus();
                Keyboard.Focus(LotNoTextBox);
            }
        }

        void Accept()
        {
            if (CosmoLotNoInfo.ProductDate == null
                || CosmoLotNoInfo.CoatingDevice == null
                || CosmoLotNoInfo.CoatingNo == null
                || CosmoLotNoInfo.SlitterDevice == null
                || CosmoLotNoInfo.SlitterNo == null
                || CosmoLotNoInfo.SlitterLane == null)
            {
                return;
            }
            
            CosmoLotNoInfo.InspectStartTime = DateTime.Now;
            CosmoLotNoInfo.LotNo = _lotNo;
            AxisGrabService.IsSingleMode = IsSingleMode;
            Close(CosmoLotNoInfo);
        }

        void Cancel()
        {
            Close();
        }

        private void LotNoChange(string lotNo)
        {
            char[] yearStr = lotNo.Substring(0, 2).ToCharArray();
            string monthStr = lotNo.Substring(2, 1);
            char[] dayStr = lotNo.Substring(3, 2).ToCharArray();

            if (yearStr[0] != '_' && yearStr[1] != '_'
                && monthStr[0] != '_'
                && dayStr[0] != '_' && dayStr[1] != '_')
            {
                int year = 2000 + Convert.ToInt32(new string(yearStr));
                int month = CosmoLotNoInfo.GetMonthFromLotName(monthStr[0].ToString());
                if (month > 12)
                    month = 12;

                int day = Convert.ToInt32(new string(dayStr));

                CosmoLotNoInfo.ProductDate = new DateTime(year, month, day);
            }
            else
                CosmoLotNoInfo.ProductDate = null;

            string coatingDeviceStr = lotNo.Substring(5, 1);
            if (coatingDeviceStr[0] != '_')
                CosmoLotNoInfo.CoatingDevice = Collections.CoatingDeviceList.First(pair => pair.Key == coatingDeviceStr);
            else
                CosmoLotNoInfo.CoatingDevice = null;

            string coatingNoStr = lotNo.Substring(6, 2);
            if (coatingNoStr[0] != '_' && coatingNoStr[1] != '_')
            {
                int coatingNo = Convert.ToInt32(coatingNoStr);
                if (coatingNo >= 1 && coatingNo <= 99)
                    CosmoLotNoInfo.CoatingNo = coatingNo;
                else
                {
                    CosmoLotNoInfo.CoatingNo = null;
                    ErrorMessage = TranslationHelper.Instance.Translate("CotingNo_Error");
                    return;
                }
            }
            else
                CosmoLotNoInfo.CoatingNo = null;

            string slitterDeviceStr = lotNo.Substring(8, 1);
            if (slitterDeviceStr[0] != '_')
                CosmoLotNoInfo.SlitterDevice = Collections.SlitterDeviceList.First(pair => pair.Key == slitterDeviceStr);
            else
                CosmoLotNoInfo.SlitterDevice = null;

            string slitterNoStr = lotNo.Substring(9, 1);
            if (slitterNoStr[0] != '_')
            {
                int slitterCut = Convert.ToInt32(slitterNoStr);
                if (slitterCut >= 1 && slitterCut <= Collections.SlitterCut)
                    CosmoLotNoInfo.SlitterNo = slitterCut;
                else
                {
                    CosmoLotNoInfo.SlitterNo = null;
                    ErrorMessage = string.Format(TranslationHelper.Instance.Translate("SlitterCut_Error"), Collections.SlitterCut);
                    return;
                }
            }
            else
                CosmoLotNoInfo.SlitterNo = null;

            string slitterLaneStr = lotNo.Substring(10, 1);
            if (slitterLaneStr[0] != '_')
            {
                int slitterLane = Convert.ToInt32(slitterLaneStr);

                if (slitterLane >= 1 && slitterLane < Collections.SlitterLane)
                    CosmoLotNoInfo.SlitterLane = slitterLane;
                else
                {
                    CosmoLotNoInfo.SlitterLane = null;
                    ErrorMessage = string.Format(TranslationHelper.Instance.Translate("SlitterLane_Error"), Collections.SlitterLane);
                    return;
                }
            }
            else
                CosmoLotNoInfo.SlitterLane = null;

            ErrorMessage = string.Empty;
        }

        private void MaskedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MaskedTextBox textBox = e.Source as MaskedTextBox;
                if (textBox == null)
                    return;

                string lotNo = textBox.Text.Replace(" ", "");
                LotNoChange(lotNo);
                _lotNo = lotNo;
            }
            catch(Exception exception)
            {
                ErrorMessage = exception.Message;
                
                CosmoLotNoInfo.ProductDate = null;
                CosmoLotNoInfo.CoatingDevice = null;
                CosmoLotNoInfo.CoatingNo = null;
                CosmoLotNoInfo.SlitterDevice = null;
                CosmoLotNoInfo.SlitterNo = null;
                CosmoLotNoInfo.SlitterLane = null;
            }
        }
    }
}
