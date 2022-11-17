using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using WPF.Base.Extensions;
using WPF.Base.Helpers;
using WPF.Base.Models;

namespace WPF.Base.Services
{
    public abstract class ModelService : Observable
    {
        private const string _key = "Model";

        protected static ModelService _instance;

        protected static List<JsonConverter> _converterList = new List<JsonConverter>();

        [JsonIgnore]
        public static ModelService Instance { get => _instance;}

        protected Model _current;
        [JsonIgnore]
        public Model Current
        {
            get
            {
                return _current;
            }
            set
            {
                Set(ref _current, value);
            }
        }

        public abstract Model CreateModel();
        public abstract void Filtering();
        public abstract bool IsContains(Model model);
            
        protected static ObservableCollection<Model> _modelList = new ObservableCollection<Model>();
        
        public static ObservableCollection<Model> ModelList { get => _modelList; }

        public virtual async Task LoadFromSettingsAsync()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Model);
            var modelFiles = directoryInfo.GetFiles("*.json");

            foreach (var file in modelFiles)
            {
                var name = file.Name.Replace(".json", "");
                _modelList.Add(await directoryInfo.ReadAsync<Model>(name, _converterList.ToArray()));
            }

            _instance.Filtering();
        }
        
        public virtual async Task AddModel(Model model)
        {
            _modelList.Add(model);

            await SaveModel(model);
        }

        public virtual async Task SaveModel(Model model)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Model);
            await directoryInfo.SaveAsync<Model>(model.Name, model, _converterList.ToArray());
        }

        public virtual async Task RemoveModel(Model model)
        {
            _modelList.Remove(model);

            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Model);
            await directoryInfo.RemoveAsync(model.Name);
        }
    }
}
;