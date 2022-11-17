using DynMvp.Base;
using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UniEye.Base.Device;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniEye.Base.Settings.UI;
using UniEye.Base.UI;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using UniScan.Common.Settings;
using UniScan.Common.Settings.UI;
using UniScan.Common.UI;

namespace UniScan.Common
{
    public abstract class SystemManager : UniEye.Base.SystemManager
    {
        public ModellerPageExtender ModellerPageExtender { get => this.modellerPageExtender; }
        protected ModellerPageExtender modellerPageExtender;

        ExchangeOperator exchangeOperator;
        public ExchangeOperator ExchangeOperator
        {
            get { return exchangeOperator; }
            set { exchangeOperator = value; }
        }

        UiController uiController;
        public UiController UiController
        {
            get { return uiController; }
            set { uiController = value; }
        }

        public new static SystemManager Instance()
        {
            return (SystemManager)_instance;
        }

        public new ModelManager ModelManager
        {
            get { return (ModelManager)modelManager; }
        }

        public virtual ModelManager CreateModelManager()
        {
            return new ModelManager();
        }

        public new Model CurrentModel
        {
            get { return (Model)currentModel; }
        }

        public void InitalizeModellerPageExtender()
        {
            this.modellerPageExtender = this.uiChanger.CreateModellerPageExtender();
        }

        public bool LoadModel(string[] args)
        {
            try
            {
                currentModel = ModelManager.LoadModel(args, progressForm);
                if (currentModel == null)
                    return false;

                if (deviceController != null)
                    deviceController.OnModelLoaded(currentModel);
            }
            catch (InvalidModelNameException)
            {
                currentModel = null;
                return false;
            }

            return true;
        }

        public bool LoadModel(ModelDescription modelDescription)
        {
            try
            {
                currentModel = modelManager.LoadModel(modelDescription, progressForm);
                if (currentModel == null)
                    return false;

                if (deviceController != null)
                    deviceController.OnModelLoaded(currentModel);
            }
            catch (InvalidModelNameException)
            {
                currentModel = null;
                return false;
            }

            return true;
        }

        public override void InitializeResultManager()
        {
            if (OperationSettings.Instance().ResultCopyDays >= 0)
                //dataManagerList.Add(new DynMvp.Data.DataCopier(DynMvp.Data.DataStoringType.Seq, PathSettings.Instance().Result, OperationSettings.Instance().ResultCopyDays, "yy-MM-dd", 10f));
                dataManagerList.Add(new DynMvp.Data.DataCopier(SystemManager.Instance().productionManager, OperationSettings.Instance().ResultCopyDays, 10f));

            if (OperationSettings.Instance().ResultStoringDays >= 0)
            {
                //dataManagerList.Add(new DynMvp.Data.DataRemover(DynMvp.Data.DataStoringType.Seq, PathSettings.Instance().Result, OperationSettings.Instance().ResultStoringDays, "yy-MM-dd", true));
                dataManagerList.Add(new DynMvp.Data.DataRemover(OperationSettings.Instance().ResultStoringDays, OperationSettings.Instance().MinimumFreeSpace, SystemManager.Instance().productionManager, new DirectoryInfo(PathSettings.Instance().Log)));

                //if (OperationSettings.Instance().ResultCopyDays >= 0)
                //{
                //    // 복사 안정화 이전 데이터 호환 위함 - 한달 후 삭제해되 됨.
                //    List<DriveInfo> driveInfoList = DynMvp.Data.DataCopier.GetTargetDriveInfoList();
                //    DirectoryInfo root = new DirectoryInfo(PathSettings.Instance().Result).Root;
                //    foreach (DriveInfo driveInfo in driveInfoList)
                //    {
                //        string newPath = PathSettings.Instance().Result.Replace(root.Name, driveInfo.Name);
                //        dataManagerList.Add(new DynMvp.Data.DataRemover(DynMvp.Data.DataStoringType.Seq, newPath, OperationSettings.Instance().ResultStoringDays, "yy-MM-dd", true));
                //    }
                //}
            }
        }
    }
}
