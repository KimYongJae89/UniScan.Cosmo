using System;

using UWP.SEMCNS.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UWP.SEMCNS.Views
{
    public sealed partial class ResultPage : Page
    {
        public ResultViewModel ViewModel { get; } = new ResultViewModel();

        public ResultPage()
        {
            InitializeComponent();
        }
    }
}
