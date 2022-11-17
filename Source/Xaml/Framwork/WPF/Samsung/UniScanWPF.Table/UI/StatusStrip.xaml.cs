using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using UniScanWPF.Table.Data;
using WpfControlLibrary.Helper;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// StatusStrip.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class StatusStrip : UserControl, IMultiLanguageSupport
    {
        public StatusStrip()
        {
            InitializeComponent();

            LocalizeHelper.AddListener(this);

            this.IOStatus.DataContext = SystemManager.Instance().MachineObserver.IoBox;
            this.ModelStatus.DataContext = InfoBox.Instance;
            this.VersionBuildStatus.DataContext = VersionHelper.Instance();
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
