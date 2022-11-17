using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WPF.Base.Controls;
using WPF.Base.Helpers;
using WPF.Base.Services;

namespace WPF.SEMCNS.Offline.ViewModels
{
    public class ShellViewModel : Observable
    {
        //private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        //private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);
        
        private bool _isBackEnabled;
        private Page _selected;

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { Set(ref _isBackEnabled, value); }
        }
        
        public ShellViewModel()
        {
        }

        public void Initialize(UIElementCollection collection, Type defualtPageType)
        {
            foreach (var element in collection)
            {
                var navigationButton = element as NavigationButton;
                if (navigationButton?.NavigateToProperty != null)
                {
                    var page = navigationButton.NavigateToProperty;
                    if (defualtPageType == page.GetType())
                    {
                        navigationButton.Selected = true;
                        ShellNavigationService.Navigate(page, navigationButton);
                    }
                }
            }
        }

        //private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        //{
        //    var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
        //    return pageType == sourcePageType;0
        //}

        //private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        //{
        //    var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
        //    if (modifiers.HasValue)
        //    {
        //        keyboardAccelerator.Modifiers = modifiers.Value;
        //    }

        //    keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
        //    return keyboardAccelerator;
        //}

        //private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        //{
        //    var result = NavigationService.GoBack();
        //    args.Handled = result;
        //}
    }
}
