using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Controls.ViewModel
{
    public class ObservableTuple<T1, T2> : Observable
    {
        T1 item1;
        public T1 Item1
        {
            get => item1;
            set => Set(ref item1, value);
        }

        T2 item2;
        public T2 Item2
        {
            get => item2;
            set => Set(ref item2, value);
        }

        public ObservableTuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    public class ObservableTuple<T1, T2, T3> : Observable
    {
        T1 item1;
        public T1 Item1
        {
            get => item1;
            set => Set(ref item1, value);
        }

        T2 item2;
        public T2 Item2
        {
            get => item2;
            set => Set(ref item2, value);
        }

        T3 item3;
        public T3 Item3
        {
            get => item3;
            set => Set(ref item3, value);
        }

        public ObservableTuple(T1 item1, T2 item2, T3 item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }
    }

    public class ObservableTuple<T1, T2, T3, T4> : Observable
    {
        T1 item1;
        public T1 Item1
        {
            get => item1;
            set => Set(ref item1, value);
        }

        T2 item2;
        public T2 Item2
        {
            get => item2;
            set => Set(ref item2, value);
        }

        T3 item3;
        public T3 Item3
        {
            get => item3;
            set => Set(ref item3, value);
        }

        T4 item4;
        public T4 Item4
        {
            get => item4;
            set => Set(ref item4, value);
        }

        public ObservableTuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
            Item4 = item4;
        }
    }

    public class ResultSearchViewModel : Observable
    {
        ICommand clearCommand;
        public ICommand ClearCommand => clearCommand ?? (clearCommand = new RelayCommand(Clear));

        ICommand searchCommand;
        public ICommand SearchCommand => searchCommand ?? (searchCommand = new RelayCommand(Serach));

        ICommand loadCommand;
        public ICommand LoadCommand => loadCommand ?? (loadCommand = new RelayCommand<object>(Load));
        
        Dictionary<CosmoLotNoInfo, DirectoryInfo> results;
        public Dictionary<CosmoLotNoInfo, DirectoryInfo> Results
        {
            get => results;
            set => Set(ref results, value);
        }
        private IDialogCoordinator _dialogCoordinator;
        FilterViewModel _filterViewModel;
        public ResultSearchViewModel()
        {
            
        }

        public void Initialize(IDialogCoordinator dialogCoordinator, FilterViewModel filterViewModel)
        {
            _dialogCoordinator = dialogCoordinator;
            _filterViewModel = filterViewModel;
        }
        
        void Clear()
        {
            Results = new Dictionary<CosmoLotNoInfo, DirectoryInfo>();
            _filterViewModel.Clear();
        }

        async void Load(object obj)
        {
            if (obj == null)
                return;

            var info = ((KeyValuePair<CosmoLotNoInfo, DirectoryInfo>)obj).Value;
            ResultLoadWindow resultLoadWindow = new ResultLoadWindow(info);
            await Application.Current.MainWindow.ShowChildWindowAsync<bool>(resultLoadWindow, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        void Serach()
        {
            Results = _filterViewModel.Search();
        }
    }
}
