﻿using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using DynMvp.Devices.Light;
using DynMvp.Devices;
using DynMvp.UI.Touch;
using DynMvp.Base;
using UniScanG.Gravure.Data;

namespace UniScanG.UI.Teach.Monitor
{
    public partial class LightSettingButton : UserControl,IMultiLanguageSupport
    {
        Control control = null;
        public LightSettingButton()
        {
            InitializeComponent();

            this.TabIndex = 0;
            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void buttonLight_Click(object sender, EventArgs e)
        {
            if (this.control == null)
            {
                Form form = new Form();
                form.Size = new System.Drawing.Size(579, 162);
                form.TopMost = true;
                form.FormBorderStyle = FormBorderStyle.FixedSingle;
                form.MinimizeBox = form.MaximizeBox = false;
                form.FormClosed += Form_FormClosed;
                form.AutoSize = true;

                LightCtrl lightCtrl = SystemManager.Instance().DeviceBox.LightCtrlHandler.GetLightCtrl(0);
                if (lightCtrl == null)
                {
                    MessageForm.Show(null, StringManager.GetString("There is no light controller installed."));
                    return;
                }

                UniEye.Base.MachineInterface.MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;

                LightParamSet lightParamSet = SystemManager.Instance().CurrentModel.LightParamSet;
                LightParam userSetLightParam = SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0];
                LightParam autoSetLightParam = (LightParam)userSetLightParam.Clone();
                //if (Array.TrueForAll(userSetLightParam.LightValue.Value, f => f == 0))
                {
                    float lineSpeed = 0;
                    ProductionG production = SystemManager.Instance().ProductionManager.CreateProduction(SystemManager.Instance().CurrentModel, "") as ProductionG;
                    if (production != null)
                    {
                        production.UpdateLineSpeedMpm(-1);
                        lineSpeed = production.LineSpeedMpm;
                    }

                    if (lineSpeed > 0)
                    {
                        int tempValue = (int)Math.Round(3 * lineSpeed - 40);
                        byte lightValue = (byte)Math.Max(tempValue, 10);
                        lightValue = (byte)Math.Min(lightValue, byte.MaxValue);

                        autoSetLightParam = (LightParam)userSetLightParam.Clone();
                        for (int j = 0; j < autoSetLightParam.LightValue.NumLight - 1; j++)
                            autoSetLightParam.LightValue.Value[j] = lightValue;
                    }
                }
                LightSettingPanel lightSettingPanel = new LightSettingPanel(lightCtrl, userSetLightParam, autoSetLightParam);
                //lightSettingPanel.Dock = DockStyle.Fill;
                form.Controls.Add(lightSettingPanel);
                form.AutoSize = true;
                //form.Size = lightSettingPanel.Size;
                //lightCtrl.TurnOn(lightParam.LightValue);
                form.Show();
                this.control = form;

                //lightCtrl.TurnOff();

            }
            else
            {
                this.control.Focus();
            }
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.control = null;
            SystemManager.Instance().CurrentModel.Modified = true;
            SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
        }
    }
}
