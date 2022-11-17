using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UniEye.Base.Settings;
using WPF.Base.Extensions;
using WPF.Base.Helpers;
using WPF.Base.Models;
using WPF.Base.Services;
using WPF.COSMO.Offline.Converters;
using WPF.COSMO.Offline.Models;

namespace WPF.COSMO.Offline.Services
{
    public class ModelCOSMOConverter : JsonCreationConverter<Model>
    {
        private class AxisGrabInfo
        {
            public string Name { get; set; }
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }
         
        protected override Model Create(Type objectType, JObject jObject)
        {
            return new Model_COSMO();
        }
    }

    public class ModelService_COSMO : ModelService
    {
        [JsonIgnore]
        public static new ModelService_COSMO Instance { get => _instance as ModelService_COSMO; }

        [JsonIgnore]
        public new Model_COSMO Current => _current as Model_COSMO;

        public static void Initialize()
        {
            _instance = new ModelService_COSMO();
            _converterList.Add(new ModelCOSMOConverter());
        }

        public override void Filtering()
        {
            var infoList = AxisGrabService.Settings.AxisGrabInfoList.ToList();
            foreach (var model in ModelList)
            {
                var cosmoModel = model as Model_COSMO;
                List<string> removeKeys = new List<string>();
                foreach (var key in cosmoModel.CalibrationDataMap.Keys)
                {
                    if (infoList.Find(info => info.Name == key) == null || cosmoModel.CalibrationDataMap[key] == null)
                        removeKeys.Add(key);
                }

                foreach (var key in removeKeys)
                    cosmoModel.CalibrationDataMap.Remove(key);
            }
        }

        public override Model CreateModel()
        {
            return new Model_COSMO();
        }
        
        private ICommand saveModelCommand;
        public ICommand SaveModelCommand
        {
            get => saveModelCommand ?? (saveModelCommand = new RelayCommand(async () =>
            {
                await SaveModel(Current);
            }));
        }

        public override async Task AddModel(Model model)
        {
            _modelList.Add(model);

            await SaveModel(model);
        }

        public override async Task SaveModel(Model model)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Model);
            Model_COSMO cosmoModel = model as Model_COSMO;
            await directoryInfo.SaveAsync<Model>(string.Format("{0} {1} {2}", cosmoModel.Name, cosmoModel.Thickness, cosmoModel.Width), model, _converterList.ToArray());
        }

        public override async Task RemoveModel(Model model)
        {
            _modelList.Remove(model);

            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Model);
            Model_COSMO cosmoModel = model as Model_COSMO;
            await directoryInfo.RemoveAsync(string.Format("{0} {1} {2}", cosmoModel.Name, cosmoModel.Thickness, cosmoModel.Width));
        }

        public override async Task LoadFromSettingsAsync()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Model);
            var modelFiles = directoryInfo.GetFiles("*.json");

            foreach (var file in modelFiles)
            {
                var fullName = file.Name.Replace(".json", "");
                string[] strs =fullName.Split(' ');

                if (strs.Count() < 3)
                    continue;

                if (UInt32.TryParse(strs.Last(), out uint width) == false)
                    continue;

                if (UInt32.TryParse(strs[strs.Count() - 2], out uint thickness) == false)
                    continue;

                string name = string.Empty;
                for (int i = 0; i < strs.Count() - 2; i++)
                    name += strs[i] + " ";

                name.Trim();

                if (string.IsNullOrEmpty(name))
                    continue;
                
                _modelList.Add(await directoryInfo.ReadAsync<Model>(fullName, _converterList.ToArray()));
            }

            _instance.Filtering();
        }

        public override bool IsContains(Model model)
        {
            var cosmoModel = model as Model_COSMO;

            return Array.ConvertAll(_modelList.ToArray(), m => (Model_COSMO)m).Any(m => m.Name == cosmoModel.Name && m.Width == cosmoModel.Width && m.Thickness == cosmoModel.Thickness);
        }
    }
}
