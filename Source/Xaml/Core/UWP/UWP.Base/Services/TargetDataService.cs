using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.Base.Services
{
    using Newtonsoft.Json;
    using Standard.DynMvp.Base.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using UWP.Base.Helpers;
    using UWP.Base.Models;
    using Windows.Storage;

    namespace UwpTemplate.Core.Services
    {
        // This class holds sample data used by some generated pages to show how they can be used.
        // TODO WTS: Delete this file once your app is using real data.
        public static class TargetDataService
        {
            static string _key = "Target";

            [JsonIgnore]
            public static Target Current { get; set; }

            [JsonIgnore]
            public static JsonCreationConverter<Target> TargetCreationConverter { get; set; }

            static ObservableCollection<Target> _targetList = new ObservableCollection<Target>();
            public static ObservableCollection<Target> TargetList { get => _targetList; }

            public static async Task LoadFromSettingsAsync()
            {
                _targetList = await ApplicationData.Current.LocalFolder.ReadAsync<ObservableCollection<Target>>(_key, TargetCreationConverter) ?? _targetList;
            }

            public static async Task SaveSettingsAsync()
            {
                await ApplicationData.Current.LocalFolder.SaveAsync<ObservableCollection<Target>>(_key, _targetList);
            }
        }
    }

}
