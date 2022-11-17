using System;

using UWP.Offline.COSMO.ViewModels;

using Windows.UI.Xaml.Controls;

namespace UWP.Offline.COSMO.Views
{
    public sealed partial class MicroScopePage : Page
    {
        public MicroScopeViewModel ViewModel { get; } = new MicroScopeViewModel();

        public MicroScopePage()
        {
            InitializeComponent();
        }
    }
}
