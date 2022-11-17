using DynMvp.Devices;
using MahApps.Metro.SimpleChildWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UniEye.Base;
using UniEye.Base.Device;
using WPF.Base.Helpers;
using WPF.Base.Views;

namespace WPF.Base.ViewModels
{
    class TowerLampSettingViewModel : Observable
    {
        #region 변수

        private List<TowerLampState> towerLampStateList;
        public List<TowerLampState> TowerLampStateList
        {
            get => towerLampStateList;
            set => Set(ref towerLampStateList, value);
        }

        private TowerLampState selectedTowerLamp;
        public TowerLampState SelectedTowerLamp
        {
            get => selectedTowerLamp;
            set => Set(ref selectedTowerLamp, value);
        }

        #endregion

        #region Command 

        private ICommand towerLampStateCommand;
        public ICommand TowerLampStateCommand { get => towerLampStateCommand ?? (towerLampStateCommand = new RelayCommand<Lamp>(LampState)); }

        private void LampState(Lamp lamp)
        {
            TowerLampOptionWindow settingWindow = new TowerLampOptionWindow(lamp as Lamp);
            if (settingWindow.ShowDialog() == true)
                UpdateTowerLamp();
        }

        private ICommand closeCommand;
        public ICommand CloseCommand { get => closeCommand ?? (closeCommand = new RelayCommand<ChildWindow>(Close)); }

        private void Close(ChildWindow window)
        {
            window.Close();
        }

        #endregion

        public TowerLampSettingViewModel()
        {
            UpdateTowerLamp();
        }

        public void UpdateTowerLamp()
        {
            TowerLampStateList = null;

            var towerLamp = SystemManager.Instance().DeviceController.TowerLamp;
            if (towerLamp != null)
                TowerLampStateList = towerLamp.TowerLampStateList;
        }
    }
}
