using DynMvp.Base;
using DynMvp.Device.Device;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Device;
using DynMvp.Device.Device.MotionController;
//using UniEye.Base.Settings;
using DynMvp.Devices.MotionController;
using UniEye.Base.UI;
using System.Threading;
using DynMvp.Device.Serial;
using DynMvp.Devices.Light;
using DynMvp.Devices;
using DynMvp.Data;
using UniScanG.Gravure.Data;
using UniScan.Common.Settings;
using UniScan.Monitor.Settings.Monitor;
using UniScan.Monitor.Device.Gravure.Laser;
using UniScanG.Gravure.Settings;
using UniEye.Base.MachineInterface;
using UniScanG.Gravure.MachineIF;

namespace UniScan.Monitor.Device.Gravure
{
    public class DeviceController: UniEye.Base.Device.DeviceController
    {
        ThreadHandler machineIfCommThreadHandler = null;

        public HanbitLaser HanbitLaser { get => this.hanbitLaser; }
        HanbitLaser hanbitLaser = null;

        public TestbedStage TestbedStage { get => this.testbedStage; }
        TestbedStage testbedStage = null;

        public override void Initialize(DeviceBox deviceBox)
        {
            if (MonitorSystemSettings.Instance().UseLaserBurner)
            {
                //if (UniEye.Base.Settings.MachineSettings.Instance().VirtualMode)
                //    this.hanbitLaser = new HanbitLaserVirtual(this);
                //else
                //    this.hanbitLaser = new HanbitLaser(this);
                this.hanbitLaser = HanbitLaser.Create(this);
                this.hanbitLaser.Initialize(deviceBox);
            }

            if (MonitorSystemSettings.Instance().UseTestbedStage)
            {
                this.testbedStage = new TestbedStage(this);
                this.testbedStage.Initialize(deviceBox);
            }

            ThreadHandler machineIfCommThreadHandler = new ThreadHandler("MachineIfCommThreadHandler", new Thread(MachineIfCommThreadProc));
            machineIfCommThreadHandler.Start();

            base.Initialize(deviceBox);
        }

        private void MachineIfCommThreadProc()
        {
            while (!machineIfCommThreadHandler.RequestStop)
            {
                // to PLC
                SetState();

                // from PLC
                GetState();

                Thread.Sleep(500);
            }
        }

        private void SetState()
        {
            string visionReady = ErrorManager.Instance().IsAlarmed() ? "0000" : "0001";
            string leasrReady = (this.hanbitLaser.IsAlive && this.hanbitLaser.IsReady && !this.hanbitLaser.IsError) ? "0001" : "0000";
            string laserCount = this.hanbitLaser.DoneCount.ToString("0000");

            MachineIfProtocol protocol = SystemManager.Instance().MachineIfProtocolList?.GetProtocol(UniScanGMachineIfCommon.SET_VISION_STATE_GRAVURE_INSP2);
            protocol.SetArgument(visionReady, leasrReady, laserCount);
            MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(protocol);
            responce?.WaitResponce();
        }

        private void GetState()
        {
            MachineIfProtocol protocol = SystemManager.Instance().MachineIfProtocolList?.GetProtocol(UniScanGMachineIfCommon.GET_MACHINE_STATE2);
            MachineIfProtocolResponce responce = SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(protocol);
            responce?.WaitResponce();

            if (responce != null && responce.IsGood)
            {
                bool useEraser = responce.ReciveData.Substring(0, 4) != "0000";
                bool forceEraser = responce.ReciveData.Substring(4, 4) != "0000";

                if (this.hanbitLaser.UseFromRemote != useEraser)
                    this.hanbitLaser.UseFromRemote = useEraser;

                if (this.hanbitLaser.IsSetNG != forceEraser)
                    this.hanbitLaser.SetNG(forceEraser);
            }
        }

        public override TowerLampState towerLamp_GetDynamicState()
        {
            if (this.testbedStage != null)
                return this.testbedStage.GetTowerlampState();

            return base.towerLamp_GetDynamicState();
        }

        public override void InitializeMotionEventHandler(UniEye.Base.Device.DeviceBox deviceBox)
        {
            this.testbedStage?.InitializeMotionEventHandler(deviceBox);
            this.hanbitLaser?.InitializeMotionEventHandler(deviceBox);

            base.InitializeMotionEventHandler(deviceBox);
        }

        public override void InitializeIoEventHandler(UniEye.Base.Device.DeviceBox deviceBox)
        {
            this.testbedStage?.InitializeIoEventHandler(deviceBox);
            this.hanbitLaser?.InitializeIoEventHandler(deviceBox);

            base.InitializeIoEventHandler(deviceBox);
        }


        public override bool OnEnterWaitInspection()
        {
            if (this.testbedStage != null)
            {
                bool ok = this.testbedStage.UpdateAxisHandler(true);
                if (ok == false)
                    throw new AlarmException(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Error,
                        StringManager.GetString("Testbed Stage Running Failure."));
            }

            if (this.hanbitLaser != null)
            {
                this.hanbitLaser.ResetDoneCount();
                bool ok = this.hanbitLaser.Start(AdditionalSettings.Instance().LaserSettingElement.Use);
                if (ok == false)
                    throw new AlarmException(ErrorSection.Machine, ErrorSubSection.CommonReason, ErrorLevel.Error, StringManager.GetString("Laser Device is not prepared."));
            }

            UpdateSerialEncoder(true);
            UpdateLightCtrl(true);

            return true;
        }

        private void AdditionalSettingChanged()
        {
            this.hanbitLaser.UseFromLocal = AdditionalSettings.Instance().LaserSettingElement.Use;
        }

        public override bool OnExitWaitInspection()
        {
            this.testbedStage?.UpdateAxisHandler(false);
            this.HanbitLaser?.Stop();

            UpdateSerialEncoder(false);
            UpdateLightCtrl(false);

            return true;
        }

        private void UpdateSerialEncoder(bool enable)
        {
            SerialDeviceHandler sdh = SystemManager.Instance().DeviceBox.SerialDeviceHandler;
            SerialEncoder se = SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == ESerialDeviceType.SerialEncoder) as SerialEncoder;
            if (se != null)
            {
                string[] token = se.ExcuteCommand(SerialEncoderV105.ECommand.EN, enable ? "1" : "0");
            }
        }

        int lightOnCount = 0;
        private void UpdateLightCtrl(bool enable)
        {
            LightCtrlHandler lch = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            for (int i = 0; i < lch.Count; i++)
            {
                SerialLightCtrl serialLightCtrl = lch.GetLightCtrl(i) as SerialLightCtrl;
                if (serialLightCtrl != null)
                {
                    //serialLightCtrl.LightSerialPort.WritePacket(enable ? "UDIO1\r\n" : "UDIO0\r\n");
                    if (enable)
                    {
                        LightParam lightParam = SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0];

                        // Auto-Adjust
                        Model curModel = SystemManager.Instance().CurrentModel;
                        ProductionG p = SystemManager.Instance().ProductionManager.CreateProduction(curModel, "") as ProductionG;
                        p?.UpdateLineSpeedMpm(-1);
                        if (p != null && p.LineSpeedMpm > 0 && Array.TrueForAll(lightParam.LightValue.Value, f => f == 0))
                        {
                            //byte lightValue = (byte)Math.Min(p.LineSpeedMpm * 2, byte.MaxValue);
                            int tempValue = (int)Math.Round(3 * p.LineSpeedMpm - 40);
                            byte lightValue = (byte)Math.Max(tempValue, 10);
                            lightValue = (byte)Math.Min(lightValue, byte.MaxValue);

                            lightParam = new LightParam(serialLightCtrl.NumChannel, 0);
                            for (int j = 0; j < serialLightCtrl.NumChannel - 1; j++)
                                lightParam.LightValue.Value[j] = lightValue;
                        }
                        serialLightCtrl.TurnOn(lightParam.LightValue);
                        lightOnCount++;
                    }
                    else
                    {
                        lightOnCount--;
                        if (lightOnCount <= 0)
                        {
                            serialLightCtrl.TurnOff();
                            lightOnCount = 0;
                        }
                    }
                }
            }
        }

        public override void Release()
        {
            this.machineIfCommThreadHandler.Stop();

            this.testbedStage?.UpdateAxisHandler(false);
            this.hanbitLaser?.Stop();
            this.UpdateLightCtrl(false);

            base.Release();
        }
    }
}
