using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Navigation;
using UWP.Base.ViewModels;

namespace UWP.Base.Views
{
    partial class ExtendedSplashPage
    {
        public ExtendedSplashViewModel ViewModel = new ExtendedSplashViewModel();

        public ExtendedSplashPage()
        {
            InitializeComponent();
        }

        public void Initialize(LaunchActivatedEventArgs args)
        {
            ViewModel.Initialize(args.SplashScreen, splashImage, KeyboardAccelerators);
        }
    }
}