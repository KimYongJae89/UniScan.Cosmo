using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Globalization;

using DynMvp.Base;

namespace DynMvp.Data
{
    public delegate void DataManagerLockItemWork(string message);
    public static class DataManagerLockItem
    {
        static DataManagerLockItemWork onWork = null;
        static public DataManagerLockItemWork OnWork { get { return onWork; } set { onWork = value; } }
        
        static List<string> onWorkingPath = new List<string>();

        public static bool StartWork(string path, string message = "")
        {
            lock (onWorkingPath)
            {
                if (onWorkingPath.Contains(path))
                    return false;

                onWorkingPath.Add(path);

                string logMessage = string.Format("[{0}] {1} - {2}",DateTime.Now.ToString("yyyyMMdd.HHmmss"), message, path);
                if (onWork != null)
                    onWork(logMessage);

                return true;
            }
        }

        public static void EndWork(string path, string message = "")
        {
            lock (onWorkingPath)
            {
                if (onWorkingPath.Contains(path))
                {
                    onWorkingPath.Remove(path);

                    string logMessage = string.Format("[{0}] {1} - {2}", DateTime.Now.ToString("yyyyMMdd.HHmmss"), message, path);
                    if (onWork != null)
                        onWork(logMessage);
                }
            }
        }
    }

    public class DataRemover : ThreadHandler
    {
        int resultStoringDays;
        int minimumFreeSpaceP;
        ProductionManagerBase productionManager;
        DirectoryInfo logDataFolder;

        public DataRemover(int resultStoringDays, int minimumFreeSpaceP, ProductionManagerBase productionManager, DirectoryInfo logDataFolder) : base("DataRemover")
        {
            this.resultStoringDays = resultStoringDays;
            this.minimumFreeSpaceP = minimumFreeSpaceP;
            this.productionManager = productionManager;
            this.logDataFolder = logDataFolder;

            this.workingThread = new Thread(new ThreadStart(DataRemoveProc));
            this.workingThread.IsBackground = true;
            this.requestStop = false;

            this.Start();
        }

        private void DataRemoveProc()
        {
            while (RequestStop == false)
            {
                if (this.productionManager != null)
                    RemoveResult();

                RemoveErrorReportData();

                if (this.logDataFolder != null)
                    RemoveLogData();
#if DEBUG
                Thread.Sleep(new TimeSpan(0, 0, 10));
#else
                Thread.Sleep(new TimeSpan(1, 0, 0));
#endif
            }
        }

        private List<string> GetSubDirectorys(DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] subDirectoryInfos = directoryInfo.GetDirectories();
            List<string> subDirectoryList = new List<string>();
            Array.ForEach(subDirectoryInfos, f =>
            {
                subDirectoryList.Add(f.FullName);
                subDirectoryList.AddRange(GetSubDirectorys(f));
            });
            return subDirectoryList;
        }

        private void RemoveErrorReportData()
        {
            DateTime limit = DateTime.Now - new TimeSpan(resultStoringDays, 0, 0, 0);
            Predicate<ErrorItem> predicate = new Predicate<ErrorItem>(f =>
            {
                return f.ErrorTime < limit && f.Alarmed == false;
            });

            List<ErrorItem> errorItemList = ErrorManager.Instance().ErrorItemList;
            lock (errorItemList)
            {
                ErrorManager.Instance().ErrorItemList.RemoveAll(predicate);
                ErrorManager.Instance().SaveErrorList();
            }
        }

        private void RemoveLogData()
        {
            DirectoryInfo[] subDirectoryInfo = logDataFolder.GetDirectories();
            foreach (DirectoryInfo directortInfo in subDirectoryInfo)
            {
                DateTime dateTime;
                bool ok = DateTime.TryParseExact(directortInfo.Name, LogHelper.BackupPathForamt, null, DateTimeStyles.None, out dateTime);
                if (ok == false)
                    continue;

                TimeSpan timeSpan = DateTime.Now - dateTime;
                if (timeSpan.TotalDays > this.resultStoringDays)
                {
                    FileHelper.ClearFolder(directortInfo.FullName);
                    Directory.Delete(directortInfo.FullName);
                }
            }
        }

        private void RemoveResult()
        {
            if (this.productionManager.List.Count == 0)
                return;

            bool isFull;
            while (isFull = IsDriveFull())
            {
                ProductionBase production = this.productionManager.List.FirstOrDefault();
                if (production == null)
                    break;
                if (RemoveAllData(production))
                    this.productionManager.RemoveProduction(production);
            }

            List<ProductionBase> productionList = new List<ProductionBase>(this.productionManager.List);
            foreach (ProductionBase production in productionList)
            {
                if (production.Equals(this.productionManager.CurProduction))
                    continue;

                bool isCopied = IsCopied(production);
                if (isCopied)
                {   // Copier에 의해 복사되었으면 바로 제거.
                    RemoveLocalData(production);
                }

                bool isOld = IsOldData(production);
                if (isOld)
                {   // 오래된 파일 제거.
                    if (RemoveAllData(production))
                        this.productionManager.RemoveProduction(production);
                }
            }
        }

        private bool IsDriveFull()
        {
            if (this.minimumFreeSpaceP < 0)
                return false;

            DirectoryInfo directoryInfo = new DirectoryInfo(productionManager.DefaultPath);
            if (directoryInfo.Exists == false)
                return false;

            DriveInfo driveInfo = new DriveInfo(directoryInfo.Root.Name);
            float freeRate = driveInfo.AvailableFreeSpace * 100.0f / driveInfo.TotalSize;
            return freeRate < this.minimumFreeSpaceP;
        }

        private bool RemoveLocalData(ProductionBase production)
        {
            string targetPath = production.GetResultPath();

            try
            {
                if (DataManagerLockItem.StartWork(targetPath, "Local Remover Start") == false) // 동작중이면 작업 안 함
                    return false;

                LogHelper.Debug(LoggerType.DataRemover, string.Format("Start Delete / SrcPath: {0}", targetPath));
                if (Directory.Exists(targetPath))
                    FileHelper.ClearFolder(targetPath, DataCopier.FlagFileName);
                LogHelper.Debug(LoggerType.DataRemover, string.Format("End Delete / SrcPath: {0}", targetPath));

                DataManagerLockItem.EndWork(targetPath, "Local Remover End");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.DataRemover, string.Format("Delete Fail: {0} / SrcPath : {1}", ex.Message, targetPath));
                return false;
            }
        }

        protected bool RemoveAllData(ProductionBase production)
        {
            string targetPath = production.GetResultPath();
            return RemoveAllData(targetPath);
        }

        protected bool RemoveAllData(string targetPath)
        {
            try
            {
                if (DataManagerLockItem.StartWork(targetPath, "Global Remover Start") == false) // 동작중이면 작업 안 함
                    return false;

                LogHelper.Debug(LoggerType.DataRemover, string.Format("Start Delete / SrcPath: {0}", targetPath));
                string copiedFlag = Path.Combine(targetPath, DataCopier.FlagFileName);
                if (File.Exists(copiedFlag))
                {
                    string copiedFile = File.ReadAllText(copiedFlag);
                    if (Directory.Exists(copiedFile))
                    {
                        FileHelper.ClearFolder(copiedFile);
                        RemoveUpperFolder(copiedFile);
                    }
                }

                RemoveCopiedDataForOldVersion(targetPath);

                if (Directory.Exists(targetPath))
                {
                    FileHelper.ClearFolder(targetPath);
                    RemoveUpperFolder(targetPath);
                }

                RemoveAllDataExtend(targetPath);

                LogHelper.Debug(LoggerType.DataRemover, string.Format("End Delete / SrcPath: {0}", targetPath));

                DataManagerLockItem.EndWork(targetPath, "Global Remover End");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.DataRemover, string.Format("Delete Fail: {0} / SrcPath : {1}", ex.Message, targetPath));
                return false;
            }
        }

        protected virtual void RemoveAllDataExtend(string reportPath) { }

        /// <summary>
        /// 알고리즘 개선 전 장비와 호환을 위함. 추후 제거 예정
        /// </summary>
        /// <param name="targetPath"></param>
        private void RemoveCopiedDataForOldVersion(string targetPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(targetPath);
            List<DriveInfo> driveInfoList = DataCopier.GetTargetDriveInfoList();
            foreach (DriveInfo driveInfo in driveInfoList)
            {
                string copiedName = targetPath.Replace(directoryInfo.Root.FullName, driveInfo.RootDirectory.FullName);
                copiedName = copiedName.Replace(@"\\", @"\");

                if (Directory.Exists(copiedName))
                {
                    FileHelper.ClearFolder(copiedName);
                    RemoveUpperFolder(copiedName);
                }
            }
        }

        protected void RemoveUpperFolder(string copiedFile)
        {
            DirectoryInfo di = new DirectoryInfo(copiedFile);
            while (di != null && di.Exists == false)
                di = di.Parent;

            if (di == null)
                return;

            int lowerItemCount = di.GetDirectories().Length + di.GetFiles().Length;
            while (lowerItemCount == 0)
            {
                Directory.Delete(di.FullName, false);
                Thread.Sleep(10);
                di = di.Parent;
                lowerItemCount = di.GetDirectories().Length + di.GetFiles().Length;
            }
        }

        private bool IsOldData(ProductionBase production)
        {
            if (resultStoringDays < 0)
                return false;

            DateTime currentDate = DateTime.Now;
            DateTime productionDate = production.StartTime;

            double days = (currentDate.Date - productionDate.Date).TotalDays;

            return (days >= resultStoringDays);
        }

        private bool IsCopied(ProductionBase production)
        {
            string resultPath = production.GetResultPath();
            if (Directory.Exists(resultPath) == false)
                return false;

            string file = Path.Combine(production.GetResultPath(), DataCopier.FlagFileName);

            bool exist = File.Exists(file);
            int files = Directory.GetFiles(resultPath).Length;
            int directories = Directory.GetDirectories(resultPath).Length;
            return exist && (files > 1 || directories > 0);
        }
    }

    public class DataCopier : ThreadHandler
    {
        public static string FlagFileName = "Copied";

        DriveInfo curTargetDrive = null;
        public DriveInfo CurBackupDrive { get => curTargetDrive; }

        ProductionManagerBase productionManager;
        int srcStoringDays;
        float minFreeSpace;

        public static List<DriveInfo> GetTargetDriveInfoList()
        {
            return DriveInfo.GetDrives().ToList().FindAll(f => f.IsReady && f.VolumeLabel.ToLower() == "backup" && f.DriveFormat == "NTFS");
        }

        public DataCopier(ProductionManagerBase productionManager, int srcStoringDays, float minFreeSpace) : base("ResultCopier")
        {
            this.productionManager = productionManager;
            this.srcStoringDays = srcStoringDays;
            this.minFreeSpace = minFreeSpace;

            this.workingThread = new Thread(new ThreadStart(DataCopyProc));
            this.workingThread.IsBackground = true;
            this.requestStop = false;

            this.Start();
        }

        //public string GetActualPath(string resultPath)
        //{
        //    // 로컬 드라이브에 존재함. 백업중일 수 있음.
        //    if (Directory.Exists(resultPath))
        //    {
        //        bool copied = File.Exists(Path.Combine(resultPath, "Copied"));
        //        if (copied == false)    // 파일이 백업 드라이브로 복사되지 않음.
        //            return resultPath;
        //    }

        //    // 로컬 드라이브에 존재하지 않음. 백업 후 삭제되었음.
        //    DirectoryInfo virtualDirInfo = new DirectoryInfo(resultPath);
        //    foreach (DriveInfo driveInfo in this.VolumeList)
        //    {
        //        string tempPath = resultPath.Replace(virtualDirInfo.Root.FullName, driveInfo.RootDirectory.FullName);
        //        if (Directory.Exists(tempPath))
        //            return tempPath;
        //    }
        //    return "";
        //}

        private DriveInfo SelectTargetVolume()
        {
            List<DriveInfo> driveInfoList = GetTargetDriveInfoList();
            driveInfoList.ForEach(driveInfo =>
            {
                float rate = driveInfo.AvailableFreeSpace * 100.0f / driveInfo.TotalSize;
                string message = string.Format("Drive: {0}, Total: {1}, Free: {2}, Rate: {3:0.00}%", driveInfo.Name, driveInfo.TotalSize, driveInfo.AvailableFreeSpace, rate);
                LogHelper.Debug(LoggerType.DataRemover, message);
            });

            return driveInfoList.Find(f => (f.AvailableFreeSpace * 100.0f / f.TotalSize) > minFreeSpace);
        }

        private void DataCopyProc()
        {
            while (RequestStop == false)
            {
                List<ProductionBase> productionList = new List<ProductionBase>(this.productionManager.List);
                foreach (ProductionBase production in productionList)
                {
                    if (production == this.productionManager.CurProduction)
                        continue;

                    string targetPath = production.GetResultPath();

                    bool copiable = IsCopiable(production);
                    if (copiable)
                    {
                        if (DataManagerLockItem.StartWork(targetPath, "Copier Start") == false) // 동작중이면 작업 안 함
                            continue;
                        CopyData(production);
                        DataManagerLockItem.EndWork(targetPath, "Copier End");
                    }

                }
#if DEBUG
                Thread.Sleep(new TimeSpan(0, 1, 0));
#else
            Thread.Sleep(new TimeSpan(1, 0, 0));
#endif
            }
        }

        private bool IsCopiable(ProductionBase production)
        {
            if (srcStoringDays < 0)
                return false;

            string path = production.GetResultPath();
            if (Directory.Exists(path) == false)
                return false;

            bool copied = File.Exists(Path.Combine(path, DataCopier.FlagFileName));
            if (copied) // 이미 복사되었음.
                return false;

            DateTime curTime = DateTime.Now;
            DateTime procTime = production.StartTime;
            double totalDays = (curTime - procTime).TotalDays;
            return (totalDays >= this.srcStoringDays);
        }

        private void CopyData(ProductionBase production)
        {
            string srcPath = production.GetResultPath();

            DirectoryInfo srcInfo = new DirectoryInfo(srcPath);
            if (srcInfo.Exists == false)
                return;

            DriveInfo dstInfo = SelectTargetVolume();
            if (dstInfo == null)
            {
                LogHelper.Error(LoggerType.DataRemover, string.Format("Copy Fail / Cannot find target drive or Target drive has no free space / SrcPath: {0}", srcInfo.FullName));
                return;
            }

            string destPath = srcInfo.FullName.Replace(srcInfo.Root.FullName, dstInfo.RootDirectory.Name);
            destPath = destPath.Replace(@"\\", @"\");
            lock (this)
                curTargetDrive = dstInfo;

            try
            {
                LogHelper.Debug(LoggerType.DataRemover, string.Format("Start Copy / SrcPath: {0} / DestPath: {1}", srcInfo.FullName, destPath));
                FileHelper.CopyDirectory(srcInfo.FullName, destPath, true, false);
                File.WriteAllText(Path.Combine(srcInfo.FullName, DataCopier.FlagFileName), destPath);
                LogHelper.Debug(LoggerType.DataRemover, string.Format("End Copy / SrcPath: {0} / DestPath: {1}", srcInfo.FullName, destPath));
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.DataRemover, string.Format("Copy Fail: {0} / SrcPath : {1} / DestPath : {2}", ex.Message, srcInfo.FullName, destPath));
            }

            lock (this)
                curTargetDrive = null;
        }
    }
}
