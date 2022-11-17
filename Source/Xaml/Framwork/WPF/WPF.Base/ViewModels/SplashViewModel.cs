using System;
using WPF.Base.Extensions;
using WPF.Base.Helpers;

namespace WPF.Base.ViewModels
{
    public class SplashViewModel : Observable
    {
        string _message;
        string _version;

        RelayCommand _keyCommand;
        public RelayCommand KeyCommand { get => _keyCommand; }
        
        public String Message
        {
            get { return _message; }
            set { Set(ref _message, value); }
        }

        public String Version
        {
            get { return _version; }
            set { Set(ref _version, value); }
        }

        public void Initialize(RelayCommand keyCommand)
        {
            Version = GetVersionDescription();
            _keyCommand = keyCommand;
        }
        
        string GetVersionDescription()
        {
            var appName = "AppDisplayName";
            //var package = Package.Current;
            //var packageId = package.Id;
            //var version = packageId.Version;

            return appName;//$"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
