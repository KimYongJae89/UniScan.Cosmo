using System;

using UWP.SEMCNS.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UWP.SEMCNS.Views
{
    public sealed partial class InspectPage : Page
    {
        public InspectViewModel ViewModel { get; } = new InspectViewModel();

        public InspectPage()
        {
            InitializeComponent();
        }
    }
}
