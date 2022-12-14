using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.SEMCNS.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 사용자 정의 컨트롤 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=234236에 나와 있습니다.

namespace UWP.SEMCNS.Views
{
    public sealed partial class ModelPage : UserControl
    {
        public ModelViewModel ViewModel { get; } = new ModelViewModel();

        public ModelPage()
        {
            this.InitializeComponent();
        }
        
        private void ItemTarget_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void ItemTarget_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.RemoveTargetCommand.CanExecute((sender as Control).DataContext))
                ViewModel.RemoveTargetCommand.Execute(((sender as Control).DataContext));
        }
    }
}
