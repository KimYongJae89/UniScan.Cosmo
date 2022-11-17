using Newtonsoft.Json;
using Standard.DynMvp.Base.Helpers;
using Standard.DynMvp.Devices;
using Standard.DynMvp.Devices.Helpers;
using Standard.DynMvp.Devices.ImageDevices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UWP.Base.Helpers;
using Windows.Storage;

namespace UWP.Base.Settings
{
    public abstract class Setting<T> where T : class, new()
    {
        private string _key;
        public string Key { get => _key; }
        
        protected Setting(string key)
        {
            _key = key;
        }
        
        public async Task LoadFromSettingsAsync()
        {
            var temp = await ApplicationData.Current.LocalSettings.ReadAsync<T>(_key, new DeivceInfoConverter(), new ProtocolInfoConverter());
            if (temp != null)
                Singleton<T>.SetInstance(temp);
        }

        public async Task SaveSettingsAsync()
        {
            await ApplicationData.Current.LocalSettings.SaveAsync<T>(_key, this as T);
        }
    }
}
