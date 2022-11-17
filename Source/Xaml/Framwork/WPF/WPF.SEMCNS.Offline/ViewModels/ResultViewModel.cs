using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Base.Helpers;
using WPF.SEMCNS.Offline.Models;
using WPF.SEMCNS.Offline.Services;

namespace WPF.SEMCNS.Offline.ViewModels
{
    public class ResultViewModel : Observable
    {
        public Result Selected { get; set; }

        public IEnumerable<Result> ItemSource
        {
            get
            {
                return ResultService.ResultList.Where(result => result.InspectTime >= StartTime && result.InspectTime <= EndTime);
            }
        }

        DateTime _startTime = DateTime.Now;
        public DateTime StartTime
        {
            get
            {
                return _startTime;
            }
            set
            {
                Set(ref _startTime, value);
                OnPropertyChanged("ItemSource");
            }
        }


        DateTime _endTime = DateTime.Now;
        public DateTime EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                Set(ref _endTime, value);
                OnPropertyChanged("ItemSource");
            }
        }

        ICommand _loadCommand;
        public ICommand LoadCommand => _loadCommand ?? (_loadCommand = new RelayCommand(LoadResult));

        ReportViewModel _reportViewModel;

        IDialogCoordinator _dialogCoordinator;
        public IDialogCoordinator DialogCoordinator { get => _dialogCoordinator; set => _dialogCoordinator = value; }

        private async void LoadResult()
        {
            if (Selected == null)
                return;

            string defectHedear = "Report";

            ProgressDialogController controller = await _dialogCoordinator.ShowProgressAsync(this, defectHedear, "Load..");
            controller.SetIndeterminate();

            await ResultService.LoadResultAsync(Selected);
            _reportViewModel.Update(Selected);

            await controller.CloseAsync();
        }

        public void Initialize(ReportViewModel reportViewModel)
        {
            _reportViewModel = reportViewModel;
        }
    }
}
