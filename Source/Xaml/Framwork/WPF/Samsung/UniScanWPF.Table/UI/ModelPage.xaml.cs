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
using UniScanWPF.Table.Data;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// ModelPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ModelPage : Page
    {
        bool removeMode = false;

        public ModelPage()
        {
            InitializeComponent();
        }

        private bool Filter(object item)
        {
            if (string.IsNullOrEmpty(FilterTextBox.Text))
                return true;
            
            return ((ModelDescription)item).Name.IndexOf(FilterTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible == true)
            {
                ModelList.ItemsSource = null;
                ModelList.ItemsSource = SystemManager.Instance().ModelManager.ModelDescriptionList.OrderByDescending(md => md.LastModifiedDate);

                CollectionViewSource.GetDefaultView(ModelList.ItemsSource).Filter = Filter;
            }
        }

        private void NewModel_Click(object sender, RoutedEventArgs e)
        {
            ModelDescription newMd = (ModelDescription)SystemManager.Instance().ModelManager.CreateModelDescription();
            ModelDescriptionWindow modelDescriptionWindow = new ModelDescriptionWindow(newMd);
            modelDescriptionWindow.ShowDialog();
            if (modelDescriptionWindow.DialogResult == true)
            {
                SystemManager.Instance().ModelManager.AddModel(newMd);
                ModelList.ItemsSource = SystemManager.Instance().ModelManager.ModelDescriptionList.OrderByDescending(md => md.LastModifiedDate);
            }
        }

        private void ModelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (removeMode)
                return;

            SimpleProgressWindow loadingWindow = new SimpleProgressWindow("Load");

            ModelDescription md = (ModelDescription)((ListBox)sender).SelectedItem;
            Model model = null;

            if (md != null)
            {
                loadingWindow.Show(() =>
                {
                    model = (Model)SystemManager.Instance().ModelManager.LoadModel(md, null);

                    if (model != null)
                        SystemManager.Instance().CurrentModel = model;
                });
            }

            if (model != null)
            {
                if (model.IsTaught())
                    SystemManager.Instance().MainPage.Navigate(PageType.Inspect);
                else
                    SystemManager.Instance().MainPage.Navigate(PageType.Teach);
            }
        }
        
        private void RemoveCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            removeMode = true;

            ModelList.SelectedIndex = -1;
        }

        private void RemoveCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            removeMode = false;

            if (ModelList.SelectedItems.Count > 0)
            {
                if (WpfControlLibrary.UI.CustomMessageBox.Show(string.Format("Are you really going to delete the models [Count : {0}] ?", ModelList.SelectedItems.Count), "Delete", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    foreach (var md in ModelList.SelectedItems)
                    {
                        if (md != null)
                        {
                            if (SystemManager.Instance().CurrentModel != null)
                                if (SystemManager.Instance().CurrentModel.ModelDescription == md)
                                    SystemManager.Instance().CurrentModel = null;

                            SystemManager.Instance().ModelManager.DeleteModel((ModelDescription)md);
                        }
                    }
                }
            }
            ModelList.SelectedIndex = -1;
            ModelList.ItemsSource = SystemManager.Instance().ModelManager.ModelDescriptionList.OrderByDescending(md => md.LastModifiedDate);

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ModelList.ItemsSource != null)
                CollectionViewSource.GetDefaultView(ModelList.ItemsSource).Refresh();
        }
    }
}
