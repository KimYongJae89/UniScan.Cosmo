using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WPF.Base.Controls;
using WPF.Base.Helpers;
using WPF.Base.Services;
using WPF.Base.Views;

namespace WPF.Base.ViewModels
{
    public class ShellViewModel
    {
        ObservableCollection<NavigationMenuItem> _itemSource = new ObservableCollection<NavigationMenuItem>();
        public ObservableCollection<NavigationMenuItem> ItemSource { get => _itemSource; }

        HamburgerMenuItemCollection optionItemSource;

        IEnumerable<NavigationMenuItem> navigationMenuItems;
        IEnumerable<NavigationMenuItem> navigationOptionMenuItems;

        public ShellViewModel()
        {

        }

        public void Initialize(HamburgerMenuItemCollection _optionItemSource, IEnumerable<NavigationMenuItem> _navigationMenuItems, IEnumerable<NavigationMenuItem> _navigationOptionMenuItems)
        {
            optionItemSource = _optionItemSource;
            navigationMenuItems = _navigationMenuItems;
            navigationOptionMenuItems = _navigationOptionMenuItems;

            if (navigationOptionMenuItems != null)
            {
                foreach (var item in navigationOptionMenuItems)
                    optionItemSource.Insert(0, item);
            }

            if (optionItemSource != null)
            {
                foreach (var optionItem in optionItemSource)
                    (optionItem as IInitializable)?.Initialize();
            }

            if (navigationMenuItems != null)
            {
                foreach (var item in navigationMenuItems)
                {
                    item.Initialize();
                    _itemSource.Add(item);
                }
            }
        }
    }
}
