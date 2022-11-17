using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using System.IO;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Vision;
using System.Drawing.Imaging;
using DynMvp.Devices.Dio;
using DynMvp.InspData;
using DynMvp.Devices.MotionController;
using System.Threading.Tasks;
using UniEye.Base.Data;
using DynMvp.UI.Touch;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;

namespace UniEye.Base.Inspect
{
    public class InspectionOption
    {
        ImageDevice imageDevice = null;
        public ImageDevice ImageDevice
        {
            get { return imageDevice; }
        }

        CancellationTokenSource cancellationTokenSource = null;
        public CancellationTokenSource CancellationTokenSource
        {
            get { return cancellationTokenSource; }
        }

        DebugContext debugContext = null;
        public DebugContext DebugContext
        {
            get { return debugContext; }
        }

        public InspectionOption(ImageDevice imageDevice)
        {
            this.imageDevice = ImageDevice;
            cancellationTokenSource = new CancellationTokenSource();
            debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, imageDevice == null ? "" : imageDevice.Name));
        }
    }

    public class ProcessTask
    {
        public CancellationTokenSource CancellationTokenSource
        {
            get { return cancellationTokenSource; }
            set { cancellationTokenSource = value; }
        }

        CancellationTokenSource cancellationTokenSource;
    }
    
    public delegate void InspectionEventHandler(object hintObj);
    public abstract class InspectRunner : IDisposable
    {
        protected InspectRunnerExtender inspectRunnerExtender = null;
        public InspectRunnerExtender InspectRunnerExtender
        {
            get { return inspectRunnerExtender; }
            set { inspectRunnerExtender = value; }
        }

        public InspectRunner()
        {
            ErrorManager.Instance().OnStartAlarmState += ErrorManager_OnStartAlarm;
        }

        protected virtual void ErrorManager_OnStartAlarm()
        {
            ExitWaitInspection();
        }

        /// <summary>
        /// 검사 대기 상태로 들어갈 때
        /// </summary>
        public virtual bool EnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::EnterWaitInspection");
            if (SystemManager.Instance().CurrentModel == null)
                return false;

            Model model = SystemManager.Instance().CurrentModel;
            if (SystemManager.Instance().CurrentModel.IsTaught() == false)
            {
                MessageForm.Show(null, "There is no data or teach state is invalid.");
                return false;
            }

            if (SystemManager.Instance().DeviceController.OnEnterWaitInspection() == false)
                return false;


            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed += ImageGrabbed;

            imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);

            SystemState.Instance().SetWait();

            return PostEnterWaitInspection();
        }

        public virtual bool PostEnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::PostEnterWaitInspection");
            SystemState.Instance().SetInspect();
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.GrabMulti();
            return true;
        }

        public virtual void PreExitWaitInspection()
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::PreExitWaitInspection");

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.Stop();
        }

        /// <summary>
        /// 검사 대기 상태를 해제할 때 호출
        /// </summary>
        public virtual void ExitWaitInspection()
        {
            PreExitWaitInspection();
            LogHelper.Debug(LoggerType.Function, "InspectRunner::ExitWaitInspection");

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);
            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed -= ImageGrabbed;

            SystemManager.Instance().ProductionManager?.Save();

            SystemState.Instance().SetIdle();
            SystemManager.Instance().MainForm?.InspectPage?.InspectionPanelList.ForEach(panel => panel.ExitWaitInspection());
        }

        //public virtual void PreExitWaitInspection() { }

        public virtual void EnterPauseInspection()
        {
            throw new NotImplementedException();
        }

        protected virtual InspectionResult BuildInspectionResult(string inspectionNo = null)
        {
            InspectionResult inspectionResult = inspectRunnerExtender.BuildInspectionResult();
            if (inspectionResult == null)
            {
                LogHelper.Error(LoggerType.Inspection, "Fail to BuildInspectionResult");
                return null;
            }

            return inspectionResult;
        }

        protected virtual void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::ImageGrabbed");
            InspectionResult inspectionResult = BuildInspectionResult();
            if (inspectionResult == null)
                return;

            InspectionOption inspectionOption = new InspectionOption(imageDevice);

            Inspect(imageDevice, ptr, inspectionResult, inspectionOption);
        }
        
        /// <summary>
        /// 검사 시퀀스
        /// </summary>
        public abstract void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectionOption inspectionOption = null);

        protected virtual void PreInspect()
        {
            SystemState.Instance().SetInspectState(InspectState.Run);
        }

        protected virtual void PostInspect()
        {
            SystemState.Instance().SetWait();
        }

        public virtual void ProductInspected(InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::ProductInspected");
            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = (inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime);

            SystemManager.Instance().ExportData(inspectionResult);
        }

        public virtual bool IsOnInspect()
        {
            return SystemState.Instance().GetOpState() == OpState.Inspect;
            //foreach (InspectionTargetGroupTask inspectionTargetGroupTask in inspectionTargetGroupTaskList)
            //{
            //    if (inspectionTargetGroupTask.IsAlive == true)
            //        return true;
            //}

            return false;
        }

        public virtual bool IsWaitIntpect()
        {
           return SystemState.Instance().GetOpState() == OpState.Wait;
        }

        public virtual bool IsIdleIntpect()
        {
            return SystemState.Instance().GetOpState() == OpState.Idle;
        }

        public virtual void ResetState()
        {

        }

        public virtual void Dispose()
        {
            inspectRunnerExtender?.Dispose();
        }
    }
}
