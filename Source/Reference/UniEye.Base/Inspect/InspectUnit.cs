using DynMvp.Base;
using DynMvp.Data;
using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniEye.Base.Inspect
{
    public class UnitInspectItem
    {
        AlgorithmInspectParam algorithmInspectParam;
        public AlgorithmInspectParam AlgorithmInspectParam
        {
            get { return algorithmInspectParam; }
            set { algorithmInspectParam = value; }
        }

        InspectionOption inspectionOption;
        public InspectionOption InspectionOption
        {
            get { return inspectionOption; }
            set { inspectionOption = value; }
        }

        InspectionResult inspectionResult;
        public InspectionResult InspectionResult
        {
            get { return inspectionResult; }
            set { inspectionResult = value; }
        }

        public UnitInspectItem(AlgorithmInspectParam algorithmInspectParam, InspectionResult inspectionResult, InspectionOption inspectionOption = null)
        {
            this.algorithmInspectParam = algorithmInspectParam;
            this.inspectionOption = inspectionOption;
            this.inspectionResult = inspectionResult;
        }

        public void Dispose()
        {
            algorithmInspectParam.Dispose();
            inspectionResult.Dispose();
        }
    }

    public delegate void UnitInspectedDelegate(UnitInspectItem unitInspectItem);
    public class InspectUnitManager
    {
        List<InspectUnit> inspectUnitList = new List<InspectUnit>();

        public UnitInspectedDelegate AllUnitInspected;

        public InspectUnit this[int i]
        {
            get { return inspectUnitList[i]; }
        }
        
        public virtual void Dispose()
        {
            Stop();

            foreach (InspectUnit unit in inspectUnitList)
                unit.Dispose();

            inspectUnitList.Clear();
        }

        public InspectUnitManager()
        {
            
        }

        //~InspectUnitManager()
        //{
        //    Dispose();
        //}
        
        public int Count
        {
            get { return inspectUnitList.Count; }
        }
        
        public void Add(InspectUnit inspectUnit)
        {
            inspectUnit.Link(this);

            lock (inspectUnitList)
                inspectUnitList.Add(inspectUnit);
        }

        public void AddRange(InspectUnit[] inspectUnits)
        {
            foreach(InspectUnit inspectUnit in inspectUnits)
                Add(inspectUnit);
        }

        public void Remove(InspectUnit inspectUnit)
        {
            inspectUnit.Stop();
            inspectUnit.Link(null);

            lock (inspectUnitList)
                inspectUnitList.Remove(inspectUnit);
        }
        
        public int GetBufferCount(int i)
        {
            return this.inspectUnitList[i].GetQueueCount();
        }

        public void Run()
        {
            foreach (InspectUnit inspectUnit in inspectUnitList)
                inspectUnit.Run();
        }

        public void Stop()
        {
            foreach (InspectUnit inspectUnit in this.inspectUnitList)
                inspectUnit.Stop();
        }
        
        public bool IsRunning()
        {
            lock (inspectUnitList)
            {
                if (inspectUnitList.Count == 0)
                    return false;

                if (inspectUnitList.Exists(f => f.IsRunning))
                    return true;
            }

            return false;
        }

        public bool IsBusy()
        {
            if (IsRunning() == false)
                return false;

            if (inspectUnitList.Exists(f => f.IsBusy || f.GetQueueCount() > 0))
                return true;

            return false;
        }

        public void OnUnitInspected(string unitName, UnitInspectItem unitInspectItem)
        {
            if (unitInspectItem.InspectionOption.CancellationTokenSource.Token.IsCancellationRequested)
            {
                // 검사 취소시 종료
                AllUnitInspected(unitInspectItem);
            }
            else
            {
                int curUnitIndex = this.inspectUnitList.FindIndex(f => f.Name == unitName);
                if (curUnitIndex == this.inspectUnitList.Count - 1)
                // 마지막 검사면 종료
                {
                    AllUnitInspected(unitInspectItem);
                }
                else
                // 다음 검사 계속
                {
                    StartInspect(curUnitIndex + 1, unitInspectItem);
                }
            }
        }

        public bool StartInspect(UnitInspectItem unitInspectItem)
        {
            LogHelper.Debug(LoggerType.Function, "InspectUnitManager::StartInspect");
            return StartInspect(0, unitInspectItem);
        }

        public bool StartInspect(string unitName, UnitInspectItem unitInspectItem)
        {
            int unitIndex = this.inspectUnitList.FindIndex(f => f.Name == unitName);
            return StartInspect(unitIndex, unitInspectItem);
        }

        protected bool StartInspect(int unitIndex, UnitInspectItem unitInspectItem)
        {
            if (IsRunning())
                return this.inspectUnitList[unitIndex].AddInspectionQueue(unitInspectItem);

            return false;
        }
    }
    
    public class InspectUnit : ThreadHandler
    {
        [DllImport("kernel32")]
        static extern int GetCurrentThreadId();

        // Unit 동작 중.
        ManualResetEvent isRunning = new ManualResetEvent(false);

        // Unit 검사 중.
        ManualResetEvent isBusy = new ManualResetEvent(false);

        // 알고리즘 수행 중.
        ManualResetEvent algorithmStarted = new ManualResetEvent(false);

        // 검사 준비됨. (이전 검사가 끝나고 결과를 가져갔음)
        ManualResetEvent isReady = new ManualResetEvent(false);

        CancellationTokenSource cancellationTokenSource;

        int maxParamCount = 0;
        public int MaxParamCount
        {
            get { return maxParamCount; }
            set { maxParamCount = value; }
        }

        public DynMvp.Vision.Algorithm inspAlgorithm = null;
        List<UnitInspectItem> inspParamList = new List<UnitInspectItem>();
        InspectUnitManager inspectUnitManager = null;
        public UnitInspectedDelegate UnitInspected = null;
        
        string name;
        public string Name
        {
            get { return name; }
        }

        public bool IsRunning
        {
            get { return isRunning.WaitOne(0); }
        }

        public bool IsBusy
        {
            get { return isBusy.WaitOne(0); }
        }

        public bool IsAlgorithmStarted
        {
            get { return algorithmStarted.WaitOne(0); }
        }

        public InspectUnit(string name, Algorithm inspAlgorithm):base(string.Format("InspectUnit.{0}",name))
        {
            this.name = name;
            this.inspAlgorithm = inspAlgorithm;

            Initialize();
        }
        
        public void Link(InspectUnitManager inspectUnitManager = null)
        {
            this.inspectUnitManager = inspectUnitManager;
        }

        public void Initialize()
        {
            isRunning.Reset();
            isBusy.Reset();
            isReady.Reset();
        }

        public void Run()
        {
            this.WorkingThread = new Thread(ProcessProc);
            base.Start();
        }

        public void Stop()
        {
            while (IsRunning)
                base.Stop(10);
        }
        
        private void ProcessProc()
        {
            isRunning.Set();

            //this.SetAffinity(this.id + 1);
            //LogHelper.Debug(LoggerType.Inspection, string.Format("InspectUnit::ThreadProc \"{0}\" Run.", this.name));

            while (this.RequestStop == false)
            {
                if (inspParamList.Count == 0)
                {
                    Thread.Sleep(0);
                    continue;
                }

                isBusy.Set();

                UnitInspectItem unitInspectItem = null;
                lock (inspParamList)
                {
                    unitInspectItem = inspParamList[0];
                    inspParamList.RemoveAt(0);
                }
                this.cancellationTokenSource = new CancellationTokenSource();
                unitInspectItem.AlgorithmInspectParam.CancellationToken = this.cancellationTokenSource.Token;

                if (unitInspectItem.InspectionResult.Judgment != Judgment.Skip)
                {
                    algorithmStarted.Set();
                    AlgorithmResult algorithmResult = null;
                    Stopwatch sw = new Stopwatch();
                    LogHelper.Debug(LoggerType.Function, string.Format("InspectUnit::ThreadProc \"{0}\" Inspect Start.", this.name));
                    try
                    {
                        sw.Start();
                        algorithmResult = inspAlgorithm.Inspect(unitInspectItem.AlgorithmInspectParam);
                        sw.Stop();
                    }
#if DEBUG == false
                    catch (Exception ex)
                    {
                        LogHelper.Error(LoggerType.Inspection, string.Format("InspectUnit::ThreadProc \"{0}\" Inspect Exception. {1}", this.name, ex.Message));
                        LogHelper.Error(LoggerType.Inspection, string.Format("{0}", ex.StackTrace.ToString()));
                    }
#endif
                    finally
                    {
                        if (algorithmResult == null)
                            unitInspectItem.InspectionResult.SetSkip();
                        else
                        {
                            algorithmResult.SpandTime = sw.Elapsed;
                            if (algorithmResult.Good == false)
                                unitInspectItem.InspectionResult.SetDefect();
                        }

                        unitInspectItem.InspectionResult.AlgorithmResultLDic.Add(inspAlgorithm.GetAlgorithmType(), algorithmResult);
                    }
                    algorithmStarted.Reset();
                    Debug.WriteLine(string.Format("InspectUnit Process Time [{0}]: {1}[ms]", this.name, sw.ElapsedMilliseconds));
                }

                //LogHelper.Debug(LoggerType.Inspection, string.Format("InspectUnit::ThreadProc \"{0}\" Inspect End.", this.name));

                isBusy.Reset();

                if (this.UnitInspected != null)
                    UnitInspected(unitInspectItem);

                if (inspectUnitManager != null)
                    inspectUnitManager.OnUnitInspected(this.name, unitInspectItem);
            }
            isRunning.Reset();
            Thread.Sleep(100);

            inspParamList.ForEach(f => f.Dispose());
            inspParamList.Clear();
            //LogHelper.Debug(LoggerType.Inspection, string.Format("InspectUnit::ThreadProc \"{0}\" Stop.", this.name));
        }

        public void Dispose()
        {
            this.Stop();

            foreach (UnitInspectItem item in inspParamList)
                item.Dispose();

            inspParamList.Clear();
            inspAlgorithm = null;
        }

        public bool AddInspectionQueue(UnitInspectItem unitInspectItem)
        {
            //LogHelper.Debug(LoggerType.Inspection, string.Format("InspectUnit::AddInspectionQueue \"{0}\"", this.name));
            if (this.IsRunning == false)
                return false;

            lock (this.inspParamList)
            {
                this.inspParamList.Add(unitInspectItem);
                return true;
            }
        }

        public int GetQueueCount()
        {
            return this.inspParamList.Count + (isBusy.WaitOne(0) ? 1 : 0);
        }
    }
}

