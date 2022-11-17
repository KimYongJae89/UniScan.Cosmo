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
using UniEye.Base.Settings;
using DynMvp.Devices.MotionController;
using UniEye.Base.UI;
using System.Threading;
using DynMvp.Device.Serial;
using DynMvp.Devices.Light;
using DynMvp.Devices;
using DynMvp.Data;
using UniScanG.Gravure.Data;
using DynMvp.InspData;
using UniScanG.Gravure.Settings;

namespace UniScan.Inspector.Device.Gravure
{
    public class DeviceController: UniEye.Base.Device.DeviceController
    {
        ThreadHandler ioThreadHandler = null;
        TimeOutTimer ioOutputHolder = null;
        List<Tuple<DateTime, bool>> tupleList = new List<Tuple<DateTime, bool>>();

        IoPort ioPort = null;

        public override void Initialize(UniEye.Base.Device.DeviceBox deviceBox)
        {
            base.Initialize(deviceBox);

            this.ioPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(PortMap.IoPortName.OutVisionNG);
            if (this.ioPort != null)
                SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputDeactive(this.ioPort);

            this.ioOutputHolder = new TimeOutTimer() { OnTimeout = IoOutputHolder_OnTimeout };
        }

        public override void InitializeMotionEventHandler(UniEye.Base.Device.DeviceBox deviceBox)
        {
            base.InitializeMotionEventHandler(deviceBox);
        }

        public override void InitializeIoEventHandler(UniEye.Base.Device.DeviceBox deviceBox)
        {
            base.InitializeIoEventHandler(deviceBox);

            PortMap portMap = (PortMap)deviceBox.PortMap;

            //IoEventHandler airFan = new IoEventHandler("AirFan", deviceBox.DigitalIoHandler, portMap.GetOutPort(UniScanG.Gravure.Device.PortMap.IoPortName.OutAirFan), IoEventHandlerDirection.Out);
            //airFan.OnInputOn += airFan_OnInputOn;
            //airFan.OnInputOff += airFan_OnInputOff;
            //airFan.Update();
            //ioEventHandlerList.Add(airFan);
        }

        public override bool OnEnterWaitInspection()
        {
            tupleList.Clear();

            this.ioThreadHandler = new ThreadHandler("GravureInspectorIoThreadHandler", new Thread(IoThreadProc), false);
            this.ioThreadHandler.Start();

            return true;
        }

        public override bool OnExitWaitInspection()
        {
            this.ioThreadHandler?.Stop();
            this.ioThreadHandler = null;

            tupleList.Clear();
            return true;
        }

        public override void OnProductInspected(InspectionResult inspectionResult)
        {
            base.OnProductInspected(inspectionResult);

            if (ioPort == null)
                return;

            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = UniScanG.Gravure.Settings.AdditionalSettings.Instance();
            if (additionalSettings.LaserSettingElement.CheckCondition(inspectionResult))
            {
                lock (this.tupleList)
                    this.tupleList.Add(new Tuple<DateTime, bool>(inspectionResult.InspectionStartTime, inspectionResult.IsGood()));
            }
        }

        private void IoThreadProc()
        {

            while (!this.ioThreadHandler.RequestStop)
            {
                double laserIoOutputDelayMs = CalculateOutputDelayMs();
                if (double.IsNaN(laserIoOutputDelayMs))
                {
                    Thread.Sleep(100);
                    continue;
                }

                DateTime now = DateTime.Now;
                DateTime grabTime = now.AddMilliseconds(-laserIoOutputDelayMs);

                Tuple<DateTime, bool> found = null;
                lock (this.tupleList)
                    found = this.tupleList.Find(f => f.Item1.CompareTo(grabTime) < 0);

                if (found == null)
                    continue;

                if (found.Item2 == false)
                {
                    SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputActive(this.ioPort);
                    this.ioOutputHolder.Restart(100);
                }
                this.tupleList.Remove(found);
            }
        }

        private void IoOutputHolder_OnTimeout(object sender, EventArgs e)
        {
            SystemManager.Instance().DeviceBox.DigitalIoHandler.SetOutputDeactive(this.ioPort);
        }

        private double CalculateOutputDelayMs()
        {
            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = UniScanG.Gravure.Settings.AdditionalSettings.Instance();
            double laserDistanceM = additionalSettings.LaserSettingElement.DistanceM;
            double safeDistanceM = additionalSettings.LaserSettingElement.SafeDistanceM;
            double distanceM = Math.Max(laserDistanceM - safeDistanceM, 0);

            ProductionG productionG = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
            if (productionG.LineSpeedMpm <= 0)
            {
#if DEBUG
                productionG.UpdateLineSpeedMpm(4 * 60);
#else
                return -1;
#endif
            }
            double lineSpdMps = Math.Max(0, productionG.LineSpeedMpm / 60.0);

            if (lineSpdMps == 0)
                return double.NaN;
            return distanceM / lineSpdMps * 1000;
        }
    }
}
