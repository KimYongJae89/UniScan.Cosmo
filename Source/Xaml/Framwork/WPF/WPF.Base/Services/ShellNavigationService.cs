using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using WPF.Base.Controls;
using WPF.Base.Helpers;

namespace WPF.Base.Services
{
    public static class ShellNavigationService
    {
        public static event NavigatedEventHandler Navigated;

        public static event NavigationFailedEventHandler NavigationFailed;

        private static Frame _frame;
        private static object _lastParamUsed;

        public static Frame Frame
        {
            get
            {
                if (_frame == null)
                {
                    //_frame = Window.Current.Content as Frame;
                    RegisterFrameEvents();
                }

                return _frame;
            }

            set
            {
                UnregisterFrameEvents();
                _frame = value;
                RegisterFrameEvents();
            }
        }

        public static bool CanGoBack => Frame.CanGoBack;

        public static bool CanGoForward => Frame.CanGoForward;

        public static bool GoBack()
        {
            if (CanGoBack)
            {
                Frame.GoBack();
                return true;
            }

            return false;
        }

        public static void GoForward() => Frame.GoForward();

        //public static bool Navigate(InitializablePage page, object parameter = null)
        //{
        //    // Don't open the same page multiple times
        //    if (Frame.Content != page || (parameter != null && !parameter.Equals(_lastParamUsed)))
        //    {
        //        var navigationResult = Frame.Navigate(page, parameter);

        //        if (navigationResult)
        //        {
        //            if (_lastParamUsed != null)
        //                (_lastParamUsed as NavigationButton).Selected = false;

        //            if (parameter != null)
        //                (parameter as NavigationButton).Selected = true;

        //            _lastParamUsed = parameter;
        //        }

        //        return navigationResult;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        private static void RegisterFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated += Frame_Navigated;
                _frame.NavigationFailed += Frame_NavigationFailed;
            }
        }

        private static void UnregisterFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigated -= Frame_Navigated;
                _frame.NavigationFailed -= Frame_NavigationFailed;
            }
        }

        private static void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e) => NavigationFailed?.Invoke(sender, e);
        private static void Frame_Navigated(object sender, NavigationEventArgs e) => Navigated?.Invoke(sender, e);
    }
}
