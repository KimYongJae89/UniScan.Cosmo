using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Controls.ViewModel
{
    public class CompareChartViewModel
    {
        public Func<ChartPoint, string> PointLabel => GetLabel;

        public ObservableValue _leftNum = new ObservableValue();
        public ObservableValue _rightNum = new ObservableValue();

        public ObservableValue _sideNum = new ObservableValue();
        public ObservableValue _innerNum = new ObservableValue();

        public ChartValues<ObservableValue> LeftValues => new ChartValues<ObservableValue>() { _leftNum };
        public ChartValues<ObservableValue> RightValues => new ChartValues<ObservableValue>() { _rightNum };
        public ChartValues<ObservableValue> SideValues => new ChartValues<ObservableValue>() { _sideNum };
        public ChartValues<ObservableValue> InnerValues => new ChartValues<ObservableValue>() { _innerNum };

        ICommand _dataClickCommand;
        public ICommand DataClickCommand => _dataClickCommand ?? (_dataClickCommand = new RelayCommand<ChartPoint>(DataClick));

        DefectStorage _defectStorage;

        public CompareChartViewModel()
        {

        }

        void DataClick(ChartPoint chartPoint)
        {
            if (chartPoint.Y == _leftNum.Value)
            {
                _defectStorage.DefectViewMode = DefectViewMode.Left;
            }
            else if (chartPoint.Y == _rightNum.Value)
            {
                _defectStorage.DefectViewMode = DefectViewMode.Right;
            }
            else if (chartPoint.Y == _sideNum.Value)
            {
                _defectStorage.DefectViewMode = DefectViewMode.Edge;
            }
            else if (chartPoint.Y == _innerNum.Value)
            {
                _defectStorage.DefectViewMode = DefectViewMode.Inner;
            }
        }

        string GetLabel(ChartPoint chartPoint)
        {
            return string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
        }

        void ClearValue()
        {
            _leftNum.Value = 0;
            _rightNum.Value = 0;
            _sideNum.Value = 0;
            _innerNum.Value = 0;
        }

        public void Initialize(DefectStorage defectStorage)
        {
            _defectStorage = defectStorage;

            AxisGrabService.Initialized += Initialized;
            InspectService.InspectDone += UpdateData;
            ResultService.Initialized += Initialized;
            ResultService.LoadDone += UpdateData;
        }

        void Initialized()
        {
            ClearValue();
        }
        
        void UpdateData()
        {
            IEnumerable<Defect> sideDefects = _defectStorage.Defects.Where(defect => defect is EdgeDefect).Concat(_defectStorage.Defects.Where(defect => defect is SectionDefect));
            IEnumerable<Defect> innerDefects = _defectStorage.Defects.Where(defect => defect is InnerDefect);
            IEnumerable<Defect> leftDefects = _defectStorage.Defects?.Where(defect => (defect as IDirectionDefect)?.ScanDirection == ScanDirection.LeftToRight);
            IEnumerable<Defect> rightDefects = _defectStorage.Defects?.Where(defect => (defect as IDirectionDefect)?.ScanDirection == ScanDirection.RightToLeft);

            _sideNum.Value += sideDefects.Count();
            _innerNum.Value += innerDefects.Count();

            _leftNum.Value += leftDefects.Count();
            _rightNum.Value += rightDefects.Count();
        }
    }
}
