using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using WPF.Base.Helpers;

namespace WPF.Base.Extensions
{
    public static class DirectoryInfoExtensions
    {
        private const string FileExtension = ".json";

        public static async Task SaveAsync<T>(this DirectoryInfo directoryInfo, string name, T content, params JsonConverter[] converters)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            var fileContent = await Json.StringifyAsync(content, converters);

            await Task.Run(() => File.WriteAllText(path, fileContent));
        }

        public static async Task<T> ReadAsync<T>(this DirectoryInfo directoryInfo, string name, params JsonConverter[] converters)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            if (!File.Exists(path))
                return default(T);

            var fileContent = await Task<string>.Run(() => File.ReadAllText(path));
            return await Json.ToObjectAsync<T>(fileContent, converters);
        }

        public static void Save<T>(this DirectoryInfo directoryInfo, string name, T content, params JsonConverter[] converters)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            var fileContent = Json.Stringify(content, converters);
            File.WriteAllText(path, fileContent);
        }

        public static T Read<T>(this DirectoryInfo directoryInfo, string name, params JsonConverter[] converters)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            if (!File.Exists(path))
                return default(T);

            var fileContent = File.ReadAllText(path);
            return Json.ToObject<T>(fileContent, converters);
        }

        public static async Task RemoveAsync(this DirectoryInfo directoryInfo, string name)
        {
            string path = Path.Combine(directoryInfo.FullName, name + FileExtension);

            if (File.Exists(path))
            {
                await Task.Run(() =>
                {
                    using (new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose)) { };
                });
            }
        }
    }
}
