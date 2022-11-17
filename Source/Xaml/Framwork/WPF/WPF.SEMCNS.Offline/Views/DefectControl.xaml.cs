using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
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
using WPF.Base.Services;
using WPF.SEMCNS.Offline.ViewModels;

namespace WPF.SEMCNS.Offline.Views
{
    /// <summary>
    /// DefectPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DefectControl : UserControl
    {
        public DefectViewModel ViewModel = new DefectViewModel();

        bool _isInspectedControl;
        public bool IsInspectedControl
        {
            get { return _isInspectedControl; }
            set { _isInspectedControl = value; }
        }
        
        public DefectControl()
        {
            InitializeComponent();
        }

        public void Initialize(ImageViewModel imageViewModel)
        {
            ViewModel.Initialize(DialogCoordinator.Instance, new ZoomService(MainCanvas), imageViewModel, _isInspectedControl);
            DataContext = ViewModel;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            ViewModel.Selected = e.AddedItems[0] as Models.Defect;
        }
    }
}
