using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniScan.Common.Data;
using UniScan.Common.Exchange;

namespace UniScan.Common
{
    public interface IModelListener
    {
        void ModelRefreshed();
        void ModelChanged();
        void ModelTeachDone(int camId);
    }

    public interface IVisitListener
    {
        void PreparePanel(ExchangeCommand trayPanel);
        void Clear();
    }

    public interface IServerExchangeOperator
    {
        List<InspectorObj> GetInspectorList(int sheetNo=-1);
        Process OpenVnc(ExchangeCommand trayPanel, Process process, string ipAddress, IntPtr handle);
        void CloseVnc();
        bool ModelTrained(int camIndex, int clientIndex,ModelDescription modelDescription);
        void SyncModel(int camId);
    }

    public interface IClientExchangeOperator
    {
        void AddVisitListener(IVisitListener visitListener);
        void SendInspectDone(string inspectionNo, string time);
        void SendAlive();
        int GetCamIndex();
        int GetClientIndex();
        bool IsConnected { get; }
    }

    public abstract class ExchangeOperator
    {
        List<IModelListener> modelListenerList = new List<IModelListener>();

        public abstract void Initialize();
        public abstract int GetCamIndex();
        public abstract int GetClientIndex();

        public virtual bool SaveModel() { return true; }

        public virtual bool ModelExist(ModelDescription modelDescription)
        {
            return SystemManager.Instance().ModelManager.IsModelExist(modelDescription);
        }

        public virtual void ModelTeachDone(int camId)
        {
            foreach (IModelListener listener in modelListenerList)
                listener.ModelTeachDone(camId);
        }
        
        public void AddModelListener(IModelListener modelListener)
        {
            modelListenerList.Add(modelListener);
        }

        public virtual bool SelectModel(string[] args)
        {
            bool loadOk = false;
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm("Model Open");
            simpleProgressForm.Show(() =>
            {
                loadOk = SystemManager.Instance().LoadModel(args);
            });

            if (loadOk)
            {
                foreach (IModelListener listener in modelListenerList)
                    listener.ModelChanged();
            }

            return loadOk;
        }

        public virtual void UpdateModelList()
        {
            //SystemManager.Instance().ExchangeOperator.UpdateModelList();
            SystemManager.Instance().ModelManager.Refresh();
            foreach (IModelListener listener in modelListenerList)
                listener.ModelRefreshed();
        }

        public virtual bool SelectModel(ModelDescription modelDescription)
        {
            bool loadOk = false;
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                loadOk = SystemManager.Instance().LoadModel(modelDescription);
            });

            if (loadOk)
            {
                foreach (IModelListener listener in modelListenerList)
                    listener.ModelChanged();
            }
            return loadOk;
        }

        public virtual void DeleteModel(ModelDescription modelDescription)
        {
            Model model = (Model)SystemManager.Instance().CurrentModel;
            if (model != null && model.ModelDescription == modelDescription)
            {
                model.Release();
                model = null;
            }

            ModelManager modelManager = (ModelManager)SystemManager.Instance().ModelManager;
            modelManager.DeleteModel(modelDescription);
        }

        public virtual bool NewModel(ModelDescription modelDescription)
        {
            ModelManager modelManager = (ModelManager)SystemManager.Instance().ModelManager;

            if (modelManager.IsModelExist(modelDescription) == true)
                return false;

            modelManager.AddModel(modelDescription);

            Model model = (Model)modelManager.CreateModel();
            model.Modified = true;

            AlgorithmPool.Instance().BuildAlgorithmPool();

            model.ModelDescription = modelDescription;
            modelManager.SaveModel(model);

            return true;
        }

        public virtual bool ModelTrained(ModelDescription modelDescription)
        {
            ModelDescription md = SystemManager.Instance().ModelManager.GetModelDescription(modelDescription);
            
            return md.IsTrained;
        }

        public virtual void Release()
        {

        }

        public abstract void SendCommand(ExchangeCommand exchangeCommand, params string[] args);
    }
}
