using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPF.Base.Helpers
{
    public class TranslationHelper : Observable
    {
        public static CultureInfo[] CultureInfos { get; set; }
            = new CultureInfo[] { CultureInfo.CreateSpecificCulture("en-us"), CultureInfo.CreateSpecificCulture("ko-kr") };

        static readonly Lazy<ResourceManager> resourceManager = new Lazy<ResourceManager>(
            () => new ResourceManager("WPF.Base.Strings.Resources", Assembly.GetExecutingAssembly()));

        private static TranslationHelper _instance;
        
        public static TranslationHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TranslationHelper();

                return _instance;
            }
        }
        
        CultureInfo _currentCultureInfo = Thread.CurrentThread.CurrentCulture;
        public CultureInfo CurrentCultureInfo
        {
            get
            {
                return _currentCultureInfo;
            }
            set
            {
                Set(ref _currentCultureInfo, value);
            }
        }

        public string Translate(string key)
        {
            if (string.IsNullOrEmpty(key))
                return key;

            try
            {
                return resourceManager.Value.GetString(key, CurrentCultureInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine("언어 키가 없습니다 : {0}", key);
                //System.Diagnostics.Debug.Assert(false, e.Message);
            }

            return string.Format("!{0}!", key);
        }
    }
}
