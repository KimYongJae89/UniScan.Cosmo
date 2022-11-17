using System;

using UWP.Offline.COSMO.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UWP.Offline.COSMO.Views
{
    public sealed partial class ModelPage : Page
    {
        public ModelViewModel ViewModel { get; } = new ModelViewModel();

        public ModelPage()
        {
            InitializeComponent();
        }
    }
}
