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
using UniScanWPF.Table.Data;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// ModelDescriptionWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelDescriptionWindow : Window, IMultiLanguageSupport
    {
        ModelDescription modelDescription;

        public ModelDescriptionWindow(ModelDescription modelDescription)
        {
            InitializeComponent();
            
            modelDescription.Name = string.Empty;
            modelDescription.Paste = string.Empty;

            this.DataContext = modelDescription;
            this.modelDescription = modelDescription;

            LocalizeHelper.AddListener(this);
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(modelDescription.Name) || string.IsNullOrEmpty(modelDescription.Paste)
                || modelDescription.Name.Any(ch => char.GetUnicodeCategory(ch) == System.Globalization.UnicodeCategory.OtherLetter)
                || modelDescription.Paste.Any(ch => char.GetUnicodeCategory(ch) == System.Globalization.UnicodeCategory.OtherLetter)
                || modelDescription.Thickness <= 0)
            {
                CustomMessageBox.Show("Invalid model info.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            if (SystemManager.Instance().ModelManager.IsModelExist(modelDescription))
            {
                CustomMessageBox.Show("This model is exist.", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
            this.Close();
        }

        private void Cancle_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LocalizeHelper.RemoveListener(this);
        }
    }
}
