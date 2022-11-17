using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Base.Helpers;
using WPF.Base.Models;
using WPF.Base.Services;

namespace WPF.Base.Controls
{
    /// <summary>
    /// ModelWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelWindow : ChildWindow
    {
        public Model Model { get; } = ModelService.Instance.CreateModel();

        ICommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new RelayCommand(Cancel));

        ICommand _acceptCommand;
        public ICommand AcceptCommand => _acceptCommand ?? (_acceptCommand = new RelayCommand(Accept));
        
        public ModelWindow()
        {
            InitializeComponent();
        }

        async void Accept()
        {
            if (string.IsNullOrEmpty(Model.Name) || string.IsNullOrWhiteSpace(Model.Name))
            {
                await ((MetroWindow)Window.GetWindow(this)).ShowMessageAsync(
                    TranslationHelper.Instance.Translate("Error"),
                    TranslationHelper.Instance.Translate("ModelWindow_Error"));

                return;
            }

            Close(Model);
        }

        void Cancel()
        {
            Close();
        }
    }
}
