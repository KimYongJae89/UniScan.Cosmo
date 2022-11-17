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
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Controls
{
    /// <summary>
    /// ModelControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelControl : UserControl
    {
        public ModelService_COSMO ModelService => ModelService_COSMO.Instance;

        public ModelControl()
        {
            InitializeComponent();

            this.DataContext = ModelService;
        }
    }
}
