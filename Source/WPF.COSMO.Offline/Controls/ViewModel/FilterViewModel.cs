using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Controls.ViewModel
{
    public class FilterViewModel : Observable
    {
        bool coatingEnable;
        public bool CoatingEnable
        {
            get => coatingEnable;
            set => Set(ref coatingEnable, value);
        }

        bool sliterEnable;
        public bool SliterEnable
        {
            get => sliterEnable;
            set => Set(ref sliterEnable, value);
        }

        DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set => Set(ref startDate, value);
        }

        DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set => Set(ref endDate, value);
        }

        bool enableSlitterDeivce;
        public bool EnableSlitterDeivce
        {
            get => enableSlitterDeivce;
            set => Set(ref enableSlitterDeivce, value);
        }

        bool enableCoatingDeivce;
        public bool EnableCoatingDeivce
        {
            get => enableCoatingDeivce;
            set => Set(ref enableCoatingDeivce, value);
        }

        bool enableCoatingNo;
        public bool EnableCoatingNo
        {
            get => enableCoatingNo;
            set => Set(ref enableCoatingNo, value);
        }

        bool enableSlitterCut;
        public bool EnableSlitterCut
        {
            get => enableSlitterCut;
            set => Set(ref enableSlitterCut, value);
        }

        bool enableSlitterLane;
        public bool EnableSlitterLane
        {
            get => enableSlitterLane;
            set => Set(ref enableSlitterLane, value);
        }

        public ObservableCollection<ObservableTuple<string, string, bool>> SlitterDeviceList { get; set; } = new ObservableCollection<ObservableTuple<string, string, bool>>();
        public ObservableCollection<ObservableTuple<int, bool>> SlitterCutList { get; set; } = new ObservableCollection<ObservableTuple<int, bool>>();
        public ObservableCollection<ObservableTuple<int, bool>> SlitterLaneList { get; set; } = new ObservableCollection<ObservableTuple<int, bool>>();

        public ObservableCollection<ObservableTuple<string, string, bool>> CoatingDeviceList { get; set; } = new ObservableCollection<ObservableTuple<string, string, bool>>();

        public uint coatingStartNo;
        public uint CoatingStartNo
        {
            get => coatingStartNo;
            set => Set(ref coatingStartNo, value);
        }

        public uint coatingEndNo;
        public uint CoatingEndNo
        {
            get => coatingEndNo;
            set => Set(ref coatingEndNo, value);
        }

        private KeyValuePair<CosmoLotNoInfo, DirectoryInfo> selectedItem;
        public KeyValuePair<CosmoLotNoInfo, DirectoryInfo> SelectedItem
        {
            get => selectedItem;
            set => Set(ref selectedItem, value);
        }

        Dictionary<CosmoLotNoInfo, DirectoryInfo> results;
        public Dictionary<CosmoLotNoInfo, DirectoryInfo> Results
        {
            get => results;
            set => Set(ref results, value);
        }


        public FilterViewModel()
        {
            
        }

        public void Initialize()
        {
            LotNoService.Collections.LotNoInfoChanged += LotNoInfoChanged;

            Clear();
            Initilize();
        }

        void Initilize()
        {
            CoatingDeviceList.Clear();
            SlitterDeviceList.Clear();
            SlitterCutList.Clear();
            SlitterLaneList.Clear();

            foreach (var device in LotNoService.Collections.CoatingDeviceList)
                CoatingDeviceList.Add(new ObservableTuple<string, string, bool>(device.Key, string.Format("{0} ({1})", device.Key, device.Value), false));

            foreach (var device in LotNoService.Collections.SlitterDeviceList)
                SlitterDeviceList.Add(new ObservableTuple<string, string, bool>(device.Key, string.Format("{0} ({1})", device.Key, device.Value), false));

            for (int i = 1; i < LotNoService.Collections.SlitterCut; i++)
                SlitterCutList.Add(new ObservableTuple<int, bool>(i, false));

            for (int i = 1; i < LotNoService.Collections.SlitterLane; i++)
                SlitterLaneList.Add(new ObservableTuple<int, bool>(i, false));

            CoatingStartNo = 1;
            CoatingEndNo = 99;

            EnableCoatingDeivce = false;
            EnableCoatingNo = false;
            EnableSlitterDeivce = false;
            enableSlitterCut = false;
            EnableSlitterLane = false;
        }

        void LotNoInfoChanged()
        {
            Clear();
            Initilize();
        }

        public void Clear()
        {
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date;

            EnableCoatingDeivce = false;
            EnableCoatingNo = false;
            EnableSlitterDeivce = false;
            EnableSlitterCut = false;
            EnableSlitterLane = false;

            foreach (var tuple in SlitterLaneList)
                tuple.Item2 = true;

            foreach (var tuple in SlitterLaneList)
                tuple.Item2 = true;
        }

        public Dictionary<CosmoLotNoInfo, DirectoryInfo> Search()
        {
            List<KeyValuePair<string, string>?> coatingDevices = null;
            List<KeyValuePair<string, string>?> slitterDevices = null;
            List<int?> coatingNoList = null;
            List<int?> slitterCutList = null;
            List<int?> slitterLaneList = null;
            
            if (enableCoatingDeivce)
            {
                coatingDevices = new List<KeyValuePair<string, string>?>();
                
                foreach (var device in CoatingDeviceList)
                {
                    if (device.Item3)
                        coatingDevices.Add(LotNoService.Collections.CoatingDeviceList.First(d => d.Key == device.Item1));
                }
            }

            if (enableSlitterDeivce)
            {
                slitterDevices = new List<KeyValuePair<string, string>?>();
                foreach (var device in SlitterDeviceList)
                {
                    if (device.Item3)
                        slitterDevices.Add(LotNoService.Collections.SlitterDeviceList.First(d => d.Key == device.Item1));
                }
            }
            
            if (enableCoatingNo)
            {
                coatingNoList = new List<int?>();
                for (uint i =  coatingStartNo; i <= coatingEndNo; i++)
                    coatingNoList.Add((int)i);
            }

            if (enableSlitterCut)
            {
                slitterCutList = new List<int?>();
                foreach (var cut in SlitterCutList)
                {
                    if (cut.Item2)
                        slitterCutList.Add(cut.Item1);
                }
            }

            if (enableSlitterLane)
            {
                slitterLaneList = new List<int?>();
                foreach (var lane in SlitterLaneList)
                {
                    if (lane.Item2)
                        slitterLaneList.Add((int)lane.Item1);
                }
            }
            
            return ResultService.SearchInfos(startDate, endDate, coatingDevices, coatingNoList, slitterDevices, slitterCutList, slitterLaneList);
        }
    }
}
