using System;

using UWP.SEMCNS.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UWP.SEMCNS.Views
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
