﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.InspData;
using UniEye.Base.Data;
using DynMvp.Data.UI;
using DynMvp.Base;
using UniEye.Base.UI;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using UniEye.Base;
using DynMvp.Device.Serial;
using DynMvp.Devices.Comm;
using UniScanM.RVMS.Settings;
using UniScanM.RVMS.UI.Chart;
using DynMvp.UI.Touch;
using UniScanM.RVMS.Data;
using UniEye.Base.MachineInterface;
using System.Threading;
using static UniScanM.RVMS.MachineIF.MachineIfProtocolList;
using UniScanM.RVMS.MachineIF;
using UniScanM.MachineIF;
using UniScanM.RVMS.Operation;
using DynMvp.Authentication;

namespace UniScanM.RVMS.UI
{
    public partial class SettingPage : UserControl, ISettingPage, IMultiLanguageSupport
    {
        RVMSSettings setting;
        
        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public SettingPage(RVMSSettings setting)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
               
            this.setting = setting;

            this.Text = StringManager.GetString(this.GetType().FullName, this.Text);

            propertyGrid.SelectedObject = setting;
            StringManager.AddListener(this);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            RVMSSettings.Instance().Save();
        }
    
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize() { }
        public void ClearPanel() { }
        public void EnterWaitInspection() { }
        public void ExitWaitInspection() { }
        public void OnPreInspection() { }
        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, DynMvp.InspData.InspectionResult inspectionResult) { }
        public void TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult) { }
        public void TargetInspected(Target target, DynMvp.InspData.InspectionResult targetInspectionResult) { }
        public void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult) { }
        public void OnPostInspection() { }
        public void ModelChanged(Model model = null) { }
        public void InfomationChanged(object obj = null) { }
        public void SaveSettings() { }
        public void EnableControls(UserType user) { }
        public void UpdateControl(string item, object value) { }
        public void PageVisibleChanged(bool visibleFlag) { }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            RVMSSettings.Instance().AdditionalSettingChangedDelegate?.Invoke();
        }
    }
}
