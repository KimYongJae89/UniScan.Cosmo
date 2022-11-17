using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using WPF.Base.Helpers;
using WPF.SEMCNS.Offline.Models;

namespace WPF.SEMCNS.Offline.Services
{
    public class TargetService : ModelManager
    {
        private const string _key = "Target";

        [JsonIgnore]
        public static TargetParam Current { get; set; }

        static ObservableCollection<TargetParam> _targetList = new ObservableCollection<TargetParam>();
        public static ObservableCollection<TargetParam> TargetList { get => _targetList; }

        public static async Task LoadFromSettingsAsync()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Model);
            _targetList = await directoryInfo.ReadAsync<ObservableCollection<TargetParam>>(_key, null) ?? _targetList;
        }

        public static async Task SaveSettingsAsync()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Model);
            await directoryInfo.SaveAsync<ObservableCollection<TargetParam>>(_key, _targetList);
        }
    }
}
