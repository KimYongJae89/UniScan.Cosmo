using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
//using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using UniScanM.StillImage.Algorithm;
using UniScanM.StillImage.Data;
using UniScanM.StillImage.Settings;
using UniScanM.StillImage.Settings;

namespace UniScanM.StillImage.State
{
    public delegate void OnAsyncProcessDoneDelegate(DynMvp.InspData.InspectionResult inspectionResult);
    public abstract class UniscanState : IInspectProcesser
    {
        protected OnAsyncProcessDoneDelegate OnAsyncProcessDone;

        protected bool initialized = false;
        public bool Initialized
        {
            get { return initialized; }
        }

        public abstract bool IsTeachState { get; }

        protected int imageSequnece = -1;
        public int ImageSequnece
        {
            get { return imageSequnece; }
        }
        
        protected InspectState inspectState;
        public InspectState InspectState
        {
            get { return inspectState; }
        }
        /// <summary>
        /// 동기 상태: Process함수 종료 후 ProductInspected 호출.
        /// 비동기 상태: Process함수가 스레드로 동작. 함수 종료 후 ProductInspected 델리게이트 호출.
        /// </summary>
        public bool IsSyncState
        {
            get { return OnAsyncProcessDone == null; }
        }

        public UniscanState()
        {

        }

        public void Initialize()
        {
            //this.OnAsyncProcessDone = SystemManager.Instance().InspectRunner.ProductInspected;
            Init();
            initialized = true;
        }

        public ProcessTask Process(AlgoImage algoImage, DynMvp.InspData.InspectionResult inspectionResult, InspectionOption inspectionOption = null)
        {
            PreProcess();
            OnProcess(algoImage, inspectionResult, inspectionOption);
            PostProcess(inspectionResult);
            return null;
        }

        /// <summary>
        /// 상태 변경시
        /// </summary>
        protected abstract void Init();

        /// <summary>
        ///  알고리즘 시작 전
        /// </summary>
        public abstract void PreProcess();

        /// <summary>
        /// 알고리즘
        /// </summary>
        /// <param name="algoImage"></param>
        /// <param name="inspectionResult"></param>
        public abstract void OnProcess(AlgoImage algoImage, DynMvp.InspData.InspectionResult inspectionResult, InspectionOption inspectionOption = null);

        /// <summary>
        /// 알고리즘 종료 후
        /// </summary>
        /// <param name="inspectionResult"></param>
        public abstract void PostProcess(DynMvp.InspData.InspectionResult inspectionResult);

        /// <summary>
        /// 다음 스탭 가져오기
        /// </summary>
        /// <param name="inspectionResult">현재 검사 결과</param>
        /// <returns></returns>
        public abstract UniscanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult);

        public bool WaitProcessDone(ProcessTask inspectionTask, int timeoutMs = -1)
        {
            throw new NotImplementedException();
        }

        public void CancelProcess(ProcessTask inspectionTask = null)
        {
            throw new NotImplementedException();
        }

        public void StartGrab()
        {

        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

    public class FindingSheet
    {
        SheetFinder sheetFinder = null;
        Random random = null;

        public FindingSheet()
        {
            this.sheetFinder = SheetFinder.Create(0);
            this.random = new Random();
        }

        public AlgoImage GetSheetImage(AlgoImage algoImage, DynMvp.InspData.InspectionResult inspectionResult)
        {
            InspectionResult inspectionResult2 = (InspectionResult)inspectionResult;

            // Get Sheet
            Rectangle sheetRect = GetSheetRect(algoImage);
            //algoImage.Save(@"d:\temp\tt.bmp");
            if (sheetRect.Width == 0 || sheetRect.Height == 0)
            {
                LogHelper.Debug(LoggerType.Inspection, "Can not found Sheet in frame");
                inspectionResult?.SetDefect();
                //adjustImage.Save(Path.Combine(imageSavePath, "Grab", iamgeSaveName));
                return null;
            }

            if (inspectionResult2 != null)
            {
                inspectionResult2.SheetRectInFrame = sheetRect;
                //inspectionResult2.FovRectInSheet = GetFovRect(sheetRect.Size);
            }

            AlgoImage sheetImage = algoImage.GetSubImage(sheetRect);
            //sheetImage.Save(Path.Combine(imageSavePath, "Sheet", iamgeSaveName));
            
            return sheetImage;
        }

        public AlgoImage GetInspImage(AlgoImage frameImage, Size inspSize, DynMvp.InspData.InspectionResult inspectionResult)
        {
            InspectionResult inspectionResult2 = (InspectionResult)inspectionResult;

            Rectangle sheetRect = GetSheetRect(frameImage);
            if (sheetRect.Width == 0 || sheetRect.Height == 0)
            {
                LogHelper.Debug(LoggerType.Inspection, "Can not found Sheet in frame");
                inspectionResult.SetDefect();
                //adjustImage.Save(Path.Combine(imageSavePath, "Grab", iamgeSaveName));
                return null;
            }
            inspectionResult2.SheetRectInFrame = sheetRect;

            // Get FOV
            Rectangle inspRect = SheetFinder.GetInspRect(sheetRect.Size, inspSize);
            if (inspRect.Width == 0 || inspRect.Height == 0)
            {
                LogHelper.Debug(LoggerType.Inspection, "Can not found Sheet in frame");
                inspectionResult.SetDefect();
                return null;
            }
            inspectionResult2.InspRectInSheet = inspRect;

            Settings.StillImageSettings additionalSettings = Settings.StillImageSettings.Instance() as Settings.StillImageSettings;
            if(additionalSettings.OperationMode == EOperationMode.Random)
            {
                int availableY1 = sheetRect.Top;
                int availableY2 = sheetRect.Bottom - inspRect.Height;
                int newY = random.Next(availableY2 - availableY1) + availableY1;
                inspRect.Y = newY;
            }
            else
            {
                inspRect.Offset(sheetRect.Location);
            }

            AlgoImage inspImage = frameImage.GetSubImage(inspRect);
            return inspImage;
        }

        private Rectangle GetSheetRect(AlgoImage frameImage)
        {
            // Extract Sheet in Image
            //frameImage.Save("d:\\tt\\temp.bmp");
            Rectangle sheetRect = this.sheetFinder.FindSheet(frameImage);
            return sheetRect;
        }
    }
}
