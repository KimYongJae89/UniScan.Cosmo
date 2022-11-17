using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanWPF.Screen.PinHoleColor.Device;

namespace UniScanWPF.Screen.PinHoleColor.Data
{
    public class ModelManager : UniEye.Base.Data.ModelManager
    {
        List<Model> preSetList = new List<Model>();
        public List<Model> PreSetList { get => preSetList; }

        public override void Init(string modelPath)
        {
            base.Init(modelPath);

            int length = Enum.GetNames(typeof(PortMap.ModelPortName)).Length;

            for (int i = 0; i < length; i++)
            {
                string name = "1".PadRight(i + 1, '0').PadLeft(length, '0');
                if (modelDescriptionList.Exists(m => m.Name == name) == true)
                    continue;

                ModelDescription md = (ModelDescription)CreateModelDescription();
                md.Name = name;
                AddModel(md);
            }
        }

        public void InitPreset()
        {
            modelDescriptionList.ForEach(md => preSetList.Add((Model)LoadModel(md, null)));
        }

        public override DynMvp.Data.Model CreateModel()
        {
            return new Model();
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new ModelDescription();
        }

        protected override void LoadModel(DynMvp.Data.Model model, IReportProgress reportProgress)
        {
            string modelPath = GetModelPath(model.ModelDescription);
            model.ModelPath = modelPath;

            string modelFilePath = String.Format("{0}\\Model.xml", modelPath);
            if (File.Exists(modelFilePath))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(modelFilePath);
                model.LoadModel(xmlDocument["Model"]);
            }
        }

        public void SaveAllPreset()
        {
            preSetList.ForEach(preset => SaveModel(preset));
        }

        public override bool SaveModel(DynMvp.Data.Model model)
        {
            try
            {
                SaveModelDescription(model.ModelDescription);

                string modelPath = model.ModelPath;

                if (Directory.Exists(modelPath) == false)
                    Directory.CreateDirectory(modelPath);

                string tempModelFilePath = String.Format("{0}\\~Model.xml", modelPath);
                string modelFilePath = String.Format("{0}\\Model.xml", modelPath);
                string bakModelFilePath = String.Format("{0}\\Model.xml.bak", modelPath);

                XmlDocument xmlDocument = new XmlDocument();
                XmlElement element = xmlDocument.CreateElement("", "Model", "");
                xmlDocument.AppendChild(element);
                model.SaveModel(xmlDocument.DocumentElement);
                xmlDocument.Save(modelFilePath);
                
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
