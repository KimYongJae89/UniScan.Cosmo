using System;

using UWP.Offline.COSMO.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UWP.Offline.COSMO.Views
{
    public sealed partial class ReportPage : Page
    {
        public ReportViewModel ViewModel { get; } = new ReportViewModel();

        public ReportPage()
        {
            InitializeComponent();
        }
    }
}
