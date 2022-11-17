using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UniEye.Base.Data;
using UniScan.Common.Data;
using UniScan.Common.Exchange;

namespace UniScan.Common.Data
{
    public enum CommState
    {
        CONNECTED, DISCONNECTED
    }
    
    public class InspectorObj
    {
        private ModelManager modelManager;
        public ModelManager ModelManager
        {
            get { return modelManager; }
        }

        private InspectorInfo info;
        public InspectorInfo Info
        {
            get { return info; }
        }

        CommState commState = CommState.DISCONNECTED;
        public CommState CommState
        {
            get { return commState; }
            set { commState = value; }
        }

        InspectState inspectState = InspectState.Done;
        public InspectState InspectState
        {
            get { return inspectState; }
            set { inspectState = value; }
        }

        OpState opState = OpState.Idle;
        public OpState OpState
        {
            get { return opState; }
            set { opState = value; }
        }

        public InspectorObj(InspectorInfo inspectorInfo)
        {
            this.info = inspectorInfo;

            commState = CommState.DISCONNECTED;
            
            modelManager = SystemManager.Instance().CreateModelManager();
            Initialize();
        }

        public void Initialize()
        {
            modelManager.Init(Path.Combine(this.info.Path, "Model"));
        }

        public bool Exist(ModelDescription modelDescription)
        {
            return modelManager.IsModelExist(modelDescription);
        }

        public void DeleteModel(ModelDescription modelDescription)
        {
            modelManager.DeleteModel(modelDescription);
        }

        public void NewModel(ModelDescription modelDescription)
        {
            if (modelManager.IsModelExist(modelDescription) == true)
                return;

            modelManager.AddModel(modelDescription);

            Model model = (Model)modelManager.CreateModel();
            model.Modified = true;
            //AlgorithmPool.Instance().BuildAlgorithmPool();

            model.ModelDescription = modelDescription;
            modelManager.SaveModel(model);
        }

        public void Refesh()
        {
            modelManager.Refresh();
        }

        public bool IsTrained(ModelDescription modelDescription)
        {
            try
            {
                modelManager.Refresh();
            }
            catch (IOException e)
            {
                return true;
            }

            ModelDescription getModelDescription = modelManager.GetModelDescription(modelDescription);

            if (getModelDescription == null)
            {
                ModelDescription clone = (ModelDescription)modelDescription.Clone();

                NewModel(clone);

                getModelDescription = modelManager.GetModelDescription(clone);
            }
            return getModelDescription.IsTrained;
        }

        public bool IsInspectable(int sheetNo)
        {
            if (this.info.ClientIndex < 0 || sheetNo < 0)
                return true;

            return (sheetNo % 2) == this.info.ClientIndex;
        }

        public Bitmap GetPreviewImage(ModelDescription modelDescription)
        {
            string imagePath = modelManager.GetPreviewImagePath(modelDescription);

            Bitmap image = null;

            if (File.Exists(imagePath) == true)
                image = (Bitmap)ImageHelper.LoadImage(imagePath);

            return image;
        }
    }
}
