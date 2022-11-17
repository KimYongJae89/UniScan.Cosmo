using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UniEye.Base;
using WPF.Base.Controls;
using WPF.Base.Helpers;
using WPF.Base.Models;
using WPF.Base.Services;

namespace WPF.Base.ViewModels
{
    public class ModelViewModel : Observable
    {
        private IDialogCoordinator _dialogCoordinator;

        string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set
            {
                Set(ref _searchText, value);
                OnPropertyChanged("Source");
            }
        }

        ICommand _removeCommand;
        public ICommand RemoveCommand => _removeCommand ?? (_removeCommand = new RelayCommand<Model>(Remove));

        ICommand _addCommand;
        public ICommand AddCommand => _addCommand ?? (_addCommand = new RelayCommand(Add));

        ICommand _selectCommand;
        public ICommand SelectCommand => _selectCommand ?? (_selectCommand = new RelayCommand<Model>(Select));
        
        public IEnumerable<Model> Source
        {
            get => ModelService.ModelList.Where(model => model.Name.Contains(_searchText)).OrderByDescending(model => model.RegisteredDate);
        }

        public void Initialize(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
        }

        private async void Remove(Model model)
        {
            if (model == null)
                return;

            MetroDialogSettings settings = new MetroDialogSettings();
            settings.DialogMessageFontSize = 24;
            settings.DialogTitleFontSize = 36;

            string header = TranslationHelper.Instance.Translate("Remove");
            var result = await _dialogCoordinator.ShowMessageAsync(this, header, string.Format(TranslationHelper.Instance.Translate("Model_Remove"), model.Name), MessageDialogStyle.AffirmativeAndNegative, settings);
            
            if (result == MessageDialogResult.Affirmative)
            {
                await ModelService.Instance.RemoveModel(model);

                OnPropertyChanged("Source");
            }
        }

        private async void Select(Model model)
        {
            if (model == null)
                return;

            string header = TranslationHelper.Instance.Translate("Select");
            MetroDialogSettings settings = new MetroDialogSettings();
            settings.DialogMessageFontSize = 24;
            settings.DialogTitleFontSize = 36;
            var result = await _dialogCoordinator.ShowMessageAsync(this, header, string.Format(TranslationHelper.Instance.Translate("Model_Select"), model.Name), MessageDialogStyle.AffirmativeAndNegative, settings);
            if (result == MessageDialogResult.Affirmative)
                ModelService.Instance.Current = model;
        }


        private async void Add()
        {
            var result = await Application.Current.MainWindow.ShowChildWindowAsync<Model>(new ModelWindow(), ChildWindowManager.OverlayFillBehavior.FullWindow);
            
            if (result != null)
            {
                if (ModelService.Instance.IsContains(result) == false)
                {
                    await ModelService.Instance.AddModel(result);
                    OnPropertyChanged("Source");
                }
                else
                {
                    MetroDialogSettings settings = new MetroDialogSettings();
                    settings.DialogMessageFontSize = 24;
                    settings.DialogTitleFontSize = 36;

                    string header = TranslationHelper.Instance.Translate("Warning");
                    await _dialogCoordinator.ShowMessageAsync(this, header, TranslationHelper.Instance.Translate("Model_Duplicate"), MessageDialogStyle.Affirmative, settings);
                }
            }
        }
    }
}
