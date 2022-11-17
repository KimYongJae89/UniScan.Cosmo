using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using WPF.Base.Extensions;
using WPF.Base.Helpers;

namespace WPF.COSMO.Offline.Services
{
    public delegate void LotNoInfoChangedDelegate();

    public class LotNoCollections : Observable
    {
        [JsonIgnore]
        public LotNoInfoChangedDelegate LotNoInfoChanged;

        public ObservableCollection<KeyValuePair<string, string>> CoatingDeviceList { get; set; } = new ObservableCollection<KeyValuePair<string, string>>();
        public ObservableCollection<KeyValuePair<string, string>> SlitterDeviceList { get; set; } = new ObservableCollection<KeyValuePair<string, string>>();
        
        public uint slitterCut;
        public uint SlitterCut
        {
            get => slitterCut;
            set
            {
                Set(ref slitterCut, value);
                if (LotNoInfoChanged != null)
                    LotNoInfoChanged();
            }
        }

        public uint slitterLane;
        public uint SlitterLane
        {
            get => slitterLane;
            set
            {
                Set(ref slitterLane, value);
                if (LotNoInfoChanged != null)
                    LotNoInfoChanged();
            }
        }

        public LotNoCollections()
        {
            CoatingDeviceList.CollectionChanged += CollectionChanged;
            SlitterDeviceList.CollectionChanged += CollectionChanged;
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (LotNoInfoChanged != null)
                LotNoInfoChanged();
        }
    }

    static class LotNoService
    {
        const string key = "LotNo";

        public static LotNoCollections Collections { get; set; }

        public static async Task SaveAxisGrabInfos()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);
            await directoryInfo.SaveAsync(key, Collections);
        }

        public static async Task LoadAxisGrabInfos()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);
            Collections = await directoryInfo.ReadAsync<LotNoCollections>(key) ?? new LotNoCollections();
        }
    }
}
