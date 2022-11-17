using System;

using UWP.Base.Helpers;

using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.Graphics.Display;
using Windows.UI.Core;
using System.Collections.Generic;
using Windows.UI.Xaml.Input;
using Windows.System;

namespace UWP.Base.ViewModels
{
    public class ExtendedSplashViewModel : Observable
    {
        SplashScreen _splashScreen;
        Image _splashImage;
        Rect _splashImageRect;
        private double _scaleFactor;

        string _message;
        string _version;

        public String Message
        {
            get { return _message; }
            set { Set(ref _message, value); }
        }

        public String Version
        {
            get { return _version; }
            set { Set(ref _version, value); }
        }

        IList<KeyboardAccelerator> _keyboardAccelerators;
        public IList<KeyboardAccelerator> KeyboardAccelerators { get => _keyboardAccelerators; }

        public void Initialize(SplashScreen splashscreen, Image splashImage, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            _keyboardAccelerators = keyboardAccelerators;

            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

            _splashScreen = splashscreen;
            if (_splashScreen != null)
            {
                _splashScreen.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);
                _splashImage = splashImage;
                _splashImageRect = _splashScreen.ImageLocation;
                _scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                PositionImage();
            }

            Version = GetVersionDescription();

            //RestoreStateAsync(loadState);
        }

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            if (_splashScreen != null)
            {
                _splashImageRect = _splashScreen.ImageLocation;
                PositionImage();
            }
        }

        async void RestoreStateAsync(bool loadState)
        {
            //if (loadState)
            //await SuspensionManager.RestoreAsync();

            // Normally you should start the time consuming task asynchronously here and
            // dismiss the extended splash screen in the completed handler of that task
            // This sample dismisses extended splash screen  in the handler for "Learn More" button for demonstration
        }

        // Position the extended splash screen image in the same location as the system splash screen image.
        void PositionImage()
        {
            _splashImage.SetValue(Canvas.LeftProperty, _splashImageRect.Left);
            _splashImage.SetValue(Canvas.TopProperty, _splashImageRect.Top);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                _splashImage.Height = _splashImageRect.Height / _scaleFactor;
                _splashImage.Width = _splashImageRect.Width / _scaleFactor;
            }
            else
            {
                _splashImage.Height = _splashImageRect.Height;
                _splashImage.Width = _splashImageRect.Width;
            }
        }

        string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
        
         // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            //dismissed = true;

            // Navigate away from the app's extended splash screen after completing setup operations here...
            // This sample navigates away from the extended splash screen when the "Learn More" button is clicked.
        }
    }
}
