using System;

using UWP.Offline.COSMO.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UWP.Offline.COSMO.Views
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
