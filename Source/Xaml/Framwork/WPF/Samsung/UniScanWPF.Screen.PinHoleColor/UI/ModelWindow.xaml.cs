using DynMvp.Devices;
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
using System.Windows.Shapes;
using UniScanWPF.Screen.PinHoleColor.Color.Inspect;
using UniScanWPF.Screen.PinHoleColor.Data;
using UniScanWPF.Screen.PinHoleColor.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Inspect;

namespace UniScanWPF.Screen.PinHoleColor.UI
{
    /// <summary>
    /// ModelWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelWindow : Window
    {
        public ModelWindow(ModelManager modelManager)
        {
            InitializeComponent();
            
            ModelList.ItemsSource = SystemManager.Instance().ModelManager.PreSetList;
        }

        private void ModelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = (ListBox)sender;
            Model model = (Model)listBox.SelectedItem;

            PinHoleParam.DataContext = model.DeviceDictionary.Where(pair => pair.Value is PinHoleDetectorParam);
            ColorParam.DataContext = model.DeviceDictionary.Where(pair => pair.Value is ColorDetectorParam);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SystemManager.Instance().ModelManager.SaveAllPreset();

            e.Cancel = true;
            this.Hide();
        }
    }
}
