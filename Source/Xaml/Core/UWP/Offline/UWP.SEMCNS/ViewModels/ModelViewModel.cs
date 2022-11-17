using Standard.DynMvp.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UWP.Base.Helpers;
using UWP.Base.Services.UwpTemplate.Core.Services;
using UWP.SEMCNS.Models;
using UWP.SEMCNS.Views.Controls;
using Windows.UI.Xaml.Controls;

namespace UWP.SEMCNS.ViewModels
{
    public class ModelViewModel : Observable
    {
        private ICommand _itemClickCommand;
        private ICommand _addTargetCommand;
        private ICommand _removeTargetCommand;

        public ObservableCollection<SettingData> ParamSettings { get; set; } = new ObservableCollection<SettingData>();

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Target>(OnItemClick));
        public ICommand AddTargetCommand => _addTargetCommand ?? (_addTargetCommand = new RelayCommand(AddTarget));
        public ICommand RemoveTargetCommand => _removeTargetCommand ?? (_removeTargetCommand = new RelayCommand<Target>(RemoveTarget));

        Target _seleced;
        public Target Selected
        {
            get => _seleced;
            set
            {
                Set(ref _seleced, value);
                TargetDataService.Current = value;

                ParamSettings.Clear();

                if (value != null)
                    foreach (var setting in SettingDataAttribute.GetProperties(value.TargetParam))
                        ParamSettings.Add(setting);
            }
        }

        public ObservableCollection<Base.Models.Target> Source
        {
            get
            {
                return TargetDataService.TargetList;
            }
        }

        private async void OnItemClick(Target model)
        {
            if (Selected == model)
                return;

            if (Selected != null)
            {
                ContentDialog targetSelectDialog = new ContentDialog
                {
                    Title = "Do you want to select the model you clicked ?",
                    Content = "Unsaved results can not be recovered.",
                    PrimaryButtonText = "Accept",
                    CloseButtonText = "Cancle",
                };

                ContentDialogResult result = await targetSelectDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    model.LastModifiedDate = DateTime.Now;
                    TargetDataService.TargetList.Move(TargetDataService.TargetList.IndexOf(model), 0);
                    Selected = model;
                }
            }
            else
            {
                model.LastModifiedDate = DateTime.Now;
                TargetDataService.TargetList.Move(TargetDataService.TargetList.IndexOf(model), 0);
                Selected = model;
            }
        }

        private async void RemoveTarget(Target model)
        {
            ContentDialog targetSelectDialog = new ContentDialog
            {
                Title = "Do you want to remove the model you clicked ?",
                Content = "Removed models can not be recovered. But the result remains.",
                PrimaryButtonText = "Accept",
                CloseButtonText = "Cancle",
            };

            ContentDialogResult result = await targetSelectDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (Selected == model)
                    Selected = null;

                TargetDataService.TargetList.Remove(model);

            }
        }

        private async void AddTarget()
        {
            NewModelControl newModelControl = new NewModelControl(new Target());

            ContentDialogResult result = await newModelControl.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (string.IsNullOrEmpty(newModelControl.Target.Name))
                {
                    ContentDialog invalidDialog = new ContentDialog
                    {
                        Title = "This is a invalid name",
                        Content = "The name must not be blank.",
                        CloseButtonText = "Cancle",
                    };

                    await invalidDialog.ShowAsync();
                    return;
                }

                foreach (var target in TargetDataService.TargetList)
                {
                    if (newModelControl.Target.Name == target.Name)
                    {
                        ContentDialog existDialog = new ContentDialog
                        {
                            Title = "The same name is registered",
                            Content = "The name of the new model should not be the same as the name of the existing model.",
                            CloseButtonText = "Cancle",
                        };

                        await existDialog.ShowAsync();
                        return;
                    }
                }

                TargetDataService.TargetList.Insert(0, newModelControl.Target);
                await TargetDataService.SaveSettingsAsync();
            }
        }

        public ModelViewModel()
        {
            
        }
    }
}
