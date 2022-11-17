using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniEye.Base.Data
{
    public class ModelManager : DynMvp.Data.ModelManager
    {
        public ModelManager():base()
        {
            this.Init(PathSettings.Instance().Model);
        }

        public virtual void Init(string modelPath)
        {
            try
            {
                this.modelPath = modelPath;
                this.Refresh();
            }catch(IOException ex)
            { }
        }

        protected override void LoadModel(Model model, IReportProgress reportProgress)
        {
            string modelPath = GetModelPath(model.ModelDescription);

            string algorithmPoolFilePath = String.Format("{0}\\AlgorithmPool.xml", modelPath);
            if (File.Exists(algorithmPoolFilePath))
                AlgorithmPool.Instance().Load(algorithmPoolFilePath);
            else
                AlgorithmPool.Instance().BuildAlgorithmPool();

            string modelFilePath = String.Format("{0}\\Model.xml", modelPath);
            model.ModelPath = modelPath;
            if (File.Exists(modelFilePath))
            {
                ModelReader modelReader = ModelReaderBuilder.Create(modelFilePath);

                MachineSettings machineSettings = MachineSettings.Instance();
                modelReader.Initialize(SystemManager.Instance().AlgorithmArchiver, 1, machineSettings.NumCamera, machineSettings.NumLight, machineSettings.NumLightType);
                modelReader.Load(model, modelFilePath, reportProgress);
            }
        }

        public override bool SaveModel(Model model)
        {
            try
            {
                LogHelper.Info(LoggerType.Operation, string.Format("Model {0} Save", model.Name));

                SaveModelDescription(model.ModelDescription);

                string modelPath = model.ModelPath;
                Directory.CreateDirectory(modelPath);

                string tempAlgorithmPoolFilePath = String.Format("{0}\\~AlgorithmPool.xml", modelPath);
                AlgorithmPool.Instance().Save(tempAlgorithmPoolFilePath);

                string tempModelFilePath = String.Format("{0}\\~Model.xml", modelPath);

                ModelWriter modelWriter = ModelWriterBuilder.Create((float)2.0);
                modelWriter.Write(model, tempModelFilePath, null);

                string algorithmPoolFilePath = String.Format("{0}\\AlgorithmPool.xml", modelPath);
                string modelFilePath = String.Format("{0}\\Model.xml", modelPath);

                string bakAlgorithmPoolFilePath = String.Format("{0}\\AlgorithmPool.xml.bak", modelPath);
                string bakModelFilePath = String.Format("{0}\\Model.xml.bak", modelPath);

                FileHelper.SafeSave(tempModelFilePath, bakModelFilePath, modelFilePath);
                FileHelper.SafeSave(tempAlgorithmPoolFilePath, bakAlgorithmPoolFilePath, algorithmPoolFilePath);

                bool ok = model.SaveModelSchema();
                return ok;
            }
            catch (Exception ex)
            {
                string message = string.Format("Model Save Error : {0}", ex.Message);
                LogHelper.Error(LoggerType.Error, message);
                //MessageForm.Show(null, message);
                return false;
            }
        }

        public void CloseModel(Model model = null)
        {
            if (model == null)
                model = SystemManager.Instance().CurrentModel;

            model?.CloseModel();

            SystemManager.Instance().CurrentModel = null;
        }
    }
}
