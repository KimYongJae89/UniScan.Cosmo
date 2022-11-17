using Standard.DynMvp.Base.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UWP.Base.Helpers;
using UWP.Base.Services.UwpTemplate.Core.Services;
using UWP.SEMCNS.Models;
using UWP.SEMCNS.Services;

namespace UWP.SEMCNS.ViewModels
{
    public class DefectViewModel : Observable
    {
        private ICommand _itemClickCommand;
        private ICommand _selectTargetCommand;

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Target>(OnItemClick));
        public ICommand SelectTargetCommand => _selectTargetCommand ?? (_selectTargetCommand = new RelayCommand(SelectTarget));


        public Base.Models.Target Selected { get; set; }

        public ObservableCollection<Defect> Source
        {
            get
            {
                return DefectDataService.DefectList;
            }
        }

        private void OnItemClick(Target model)
        {
            Selected = model;
        }
        
        private void SelectTarget()
        {
            TargetDataService.Current = Selected;
        }

        public DefectViewModel()
        {
            
        }
    }
}
