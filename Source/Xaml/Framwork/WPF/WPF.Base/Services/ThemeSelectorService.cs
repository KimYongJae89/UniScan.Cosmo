using MahApps.Metro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using UniEye.Base.Settings;
using WPF.Base.Extensions;
using WPF.Base.Helpers;

namespace WPF.Base.Services
{
    public static class ThemeSelectorService
    {
        //static Dictionary<string, ResourceDictionary> _themeDictionary = new Dictionary<string, ResourceDictionary>();
        //static Dictionary<string, ResourceDictionary> _accentDictionary = new Dictionary<string, ResourceDictionary>();

        //static ResourceDictionary _colorDictionary = new ResourceDictionary() { Source = new Uri("pack://application:,,,/WPF.Base;component/Styles/_Colors.xaml") };

        private const string _key = "Theme";

        public static IEnumerable<Accent> Accents { get => ThemeManager.Accents; }
        public static IEnumerable<AppTheme> Themes { get => ThemeManager.AppThemes; }

        static Accent _accent;
        public static Accent Accent
        {
            get { return _accent ?? ThemeManager.GetAccent("Blue"); }
            set
            {
                _accent = value;
                ThemeManager.ChangeAppStyle(Application.Current, _accent, Theme);
            }
        }

        static AppTheme _theme;
        public static AppTheme Theme
        {
            get
            {
                return _theme ?? ThemeManager.GetAppTheme("BaseDark");
            }
            set
            {
                _theme = value;
                ThemeManager.ChangeAppStyle(Application.Current, Accent, _theme);
                var count = Application.Current.Resources.Keys.Count;

                foreach (var obj in Application.Current.Resources.Keys)
                {
                    var str = obj as string;

                    if (str != null)
                    {
                        if (str.Contains("White"))
                        {

                        }
                    }
                }
            }
        }

        public static async Task InitializeAsync()
        {
            ThemeManager.AddAppTheme("DarkBlue", new Uri("pack://application:,,,/WPF.Base;component/Styles/Themes/BaseDarkBlue.xaml"));
            ThemeManager.AddAppTheme("LightBlue", new Uri("pack://application:,,,/WPF.Base;component/Styles/Themes/BaseLightBlue.xaml"));

            await LoadThemeFromSettingsAsync();
        }

        private static async Task LoadThemeFromSettingsAsync()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);
            var temp = await directoryInfo.ReadAsync<Tuple<string, string>>(_key);

            var themeName  = temp != null ? temp.Item1 : "BaseDark";
            var accentName = temp != null ? temp.Item2 : "Blue";

            _theme = ThemeManager.GetAppTheme(themeName);
            _accent = ThemeManager.GetAccent(accentName);

            ThemeManager.ChangeAppStyle(Application.Current, _accent, _theme);
        }
        
        public static async void SaveThemeInSettingsAsync()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);
            await directoryInfo.SaveAsync<Tuple<string, string>>(_key, new Tuple<string, string>(Theme.Name, Accent.Name));
        }
    }
}
