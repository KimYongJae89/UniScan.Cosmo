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
using WPF.COSMO.Offline.Controls.ViewModel;
using WPF.COSMO.Offline.Models;

namespace WPF.COSMO.Offline.Controls.View
{
    /// <summary>
    /// LotNoChartControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LotNoChartControl : UserControl
    {
        public static readonly DependencyProperty LotNoTypeProperty =
            DependencyProperty.Register("LotNoType", typeof(CosmoLotNoType), typeof(LotNoChartControl),
                new PropertyMetadata(LotNoTypeChanged));

        private static void LotNoTypeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            LotNoChartViewModel viewModel = ((UserControl)dependencyObject).DataContext as LotNoChartViewModel;
            viewModel.Initialize((CosmoLotNoType)e.NewValue);
        }

        public CosmoLotNoType LotNoType
        {
            get => (CosmoLotNoType)GetValue(LotNoTypeProperty);
            set => SetValue(LotNoTypeProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(LotNoChartControl),
                new PropertyMetadata(ItemsSourceChanged));

        private static void ItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LotNoChartViewModel vm = ((UserControl)d).DataContext as LotNoChartViewModel;
            vm.SetResultList(e.NewValue);
        }

        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty DefectViewModeProperty =
            DependencyProperty.Register("DefectViewMode", typeof(DefectViewMode), typeof(LotNoChartControl),
                new PropertyMetadata(DefectViewModeChanged));

        private static void DefectViewModeChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            LotNoChartViewModel viewModel = ((UserControl)dependencyObject).DataContext as LotNoChartViewModel;
            viewModel.DefectViewMode = (DefectViewMode)e.NewValue;
        }

        public DefectViewMode DefectViewMode
        {
            get => (DefectViewMode)GetValue(DefectViewModeProperty);
            set => SetValue(DefectViewModeProperty, value);
        }

        public LotNoChartControl()
        {
            InitializeComponent();
            DataContext = new LotNoChartViewModel();
        }
    }
}
