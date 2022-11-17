using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Vision;
using UniEye.Base.Settings;

namespace UniScanM.Data
{
    public class ModelManager : UniEye.Base.Data.ModelManager
    {
        public override DynMvp.Data.Model CreateModel()
        {
            return new UniScanM.Data.Model();
        }

        public bool IsModelExist(string name, string paste)
        {
            return modelDescriptionList.Exists(md => md.Name == name && ((ModelDescription)md).Paste == paste);
        }

        public ModelDescription GetModelDescription(string name, string paste)
        {
            return (ModelDescription)modelDescriptionList.Find(md => md.Name == name && ((ModelDescription)md).Paste == paste);
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new ModelDescription();
        }

        public string GetModelPath(ModelDescription modelDescription)
        {
            ModelDescription modelDescriptionM = (ModelDescription)modelDescription;

            return Path.Combine(modelPath, modelDescription.Name, modelDescriptionM.Paste.ToString());
        }

        public override void Refresh(string modelPath = null)
        {
            if (modelPath == null)
                modelPath = this.modelPath;

            bool exist = Directory.Exists(modelPath);
            DirectoryInfo modelRootDir = new DirectoryInfo(modelPath);
            if (modelRootDir.Exists == false)
            {
                Directory.CreateDirectory(modelPath);
                return;
            }

            modelDescriptionList.Clear();

            foreach (DirectoryInfo nameDirectory in modelRootDir.GetDirectories())
            {
                foreach (DirectoryInfo indexDirectory in nameDirectory.GetDirectories())
                {
                    ModelDescription modelDescription = (ModelDescription)LoadModelDescription(indexDirectory.FullName);
                    if (modelDescription == null)
                        continue;

                    modelDescriptionList.Add(modelDescription);

                    if (String.IsNullOrEmpty(modelDescription.Category) == false)
                        CategoryList.Add(modelDescription.Category);
                }
            }
        }

        
        public override void DeleteModel(DynMvp.Data.ModelDescription modelDescription)
        {
            ModelDescription modelDescriptionM = (ModelDescription)modelDescription;
            
            modelDescriptionList.Remove(modelDescriptionM);

            string namePath = String.Format("{0}\\{1}", modelPath, modelDescriptionM.Name);
            string pastePath = String.Format("{0}\\{1}", namePath, modelDescriptionM.Paste);

            if (Directory.Exists(pastePath) == true)
            {
                Directory.Delete(pastePath, true);

                DirectoryInfo namePathInfo = new DirectoryInfo(namePath);
                if (namePathInfo.GetFiles().Length + namePathInfo.GetDirectories().Length == 0)
                    Directory.Delete(namePath, true);
            }

            Refresh();
        }
    }
}
