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
using WPF.SEMCNS.Offline.Models;

namespace WPF.SEMCNS.Offline.Services
{
    public class ResultService
    {
        private const string _key = "Result";

        static ObservableCollection<Result> _resultList = new ObservableCollection<Result>();
        public static ObservableCollection<Result> ResultList
        {
            get => _resultList;
        }

        public static async Task LoadListAsync()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Result);
            _resultList = await directoryInfo.ReadAsync<ObservableCollection<Result>>(_key, null) ?? _resultList;
        }

        public static async Task SaveListAsync()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Result);
            await directoryInfo.SaveAsync<ObservableCollection<Result>>(_key, _resultList);
        }

        public static async Task<Result> LoadResultAsync(Result result)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Result);
            var name = result.InspectTime.ToString("yyyy_MM_dd_HHmmss");
            result.ImageSource = await result.ImageSource.ReadAsync(directoryInfo, name);
            foreach (var defect in result.Defects)
                defect.SetImage(result.ImageSource);

            return result;
        }

        public static async Task SaveResultAsync(Result result)
        {
            _resultList.Add(result);
            await SaveListAsync();

            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Result);
            var name = result.InspectTime.ToString("yyyy_MM_dd_HHmmss");
            await directoryInfo.SaveAsync<Result>(name, result);
            await result.ImageSource.SaveAsync(directoryInfo, name);
        }
    }
}
