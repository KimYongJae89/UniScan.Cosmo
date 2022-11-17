using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.IO;
using UniEye.Base.Settings;

namespace UniScanG.Data.Model
{
    public class ModelManager : UniScan.Common.Data.ModelManager
    {
        public ModelManager() : base()
        {
            //Init(modelPath);
        }

        public override void Init(string modelPath)
        {
            base.Init(modelPath);

            this.modelPath = modelPath;
            try
            {
                this.Refresh();
            }
            catch (IOException ex)
            { }
        }

        public override DynMvp.Data.ModelDescription CreateModelDescription()
        {
            return new ModelDescription();
        }

        public override DynMvp.Data.Model CreateModel()
        {
            Model model = new Model();
            model.Modified = true;
            return model;
        }

        public override bool IsModelExist(UniScan.Common.Data.ModelDescription modelDescription)
        {
            ModelDescription modelDescriptionG = (ModelDescription)modelDescription;

            foreach (ModelDescription m in modelDescriptionList)
            {
                if (m.Name == modelDescriptionG.Name && m.Thickness == modelDescriptionG.Thickness && m.Paste == modelDescriptionG.Paste)
                    return true;
            }

            return false;
        }

        public override string GetModelPath(DynMvp.Data.ModelDescription modelDescription)
        {
            ModelDescription modelDescriptionG = (ModelDescription)modelDescription;

            return Path.Combine(modelPath, modelDescription.Name, modelDescriptionG.Thickness.ToString(), modelDescriptionG.Paste);
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
                foreach (DirectoryInfo thicknessDir in nameDirectory.GetDirectories())
                {
                    foreach (DirectoryInfo pasteDir in thicknessDir.GetDirectories())
                    {
                        ModelDescription modelDescription = (ModelDescription)LoadModelDescription(pasteDir.FullName);
                        if (modelDescription == null)
                            continue;
                        
                        modelDescriptionList.Add(modelDescription);

                        if (String.IsNullOrEmpty(modelDescription.Category) == false)
                            CategoryList.Add(modelDescription.Category);
                    }
                }
            }
        }

        public override void DeleteModel(UniScan.Common.Data.ModelDescription modelDescription)
        {
            ModelDescription modelDescriptionG = (ModelDescription)modelDescription;

            ModelDescription realMD = null;
            foreach (ModelDescription md in modelDescriptionList)
            {
                if (md.Name == modelDescriptionG.Name && md.Thickness == modelDescriptionG.Thickness && md.Paste == modelDescriptionG.Paste)
                    realMD = md;
            }

            if (realMD == null)
                return;

            modelDescriptionList.Remove(realMD);

            string firstPath = String.Format("{0}\\{1}", modelPath, realMD.Name);
            string middlePath = String.Format("{0}\\{1}", firstPath, realMD.Thickness);
            string lastPath = String.Format("{0}\\{1}", middlePath, realMD.Paste);

            if (Directory.Exists(lastPath) == true)
            {
                Directory.Delete(lastPath, true);

                DirectoryInfo middleInfo = new DirectoryInfo(middlePath);
                if (middleInfo.GetFiles().Length + middleInfo.GetDirectories().Length == 0)
                    Directory.Delete(middlePath, true);

                DirectoryInfo firstInfo = new DirectoryInfo(firstPath);
                if (firstInfo.GetFiles().Length + firstInfo.GetDirectories().Length == 0)
                    Directory.Delete(firstPath, true);
            }
            
            Refresh();
        }

        public override DynMvp.Data.Model LoadModel(string[] args, IReportProgress reportProgress)
        {
            Refresh();

            ModelDescription md = (ModelDescription)CreateModelDescription();
            md.Name = args[0];
            md.Thickness = Convert.ToSingle(args[1]);
            md.Paste = args[2];

            ModelDescription getMd = (ModelDescription)GetModelDescription(md);
            if (getMd == null)
                return null;

            return LoadModel(getMd, reportProgress);
        }

        public override bool SaveModel(DynMvp.Data.Model model)
        {
            if (model.Modified == false)
                return true;

            model.Modified = false;
            model.ModelPath = GetModelPath((ModelDescription)model.ModelDescription);
            return base.SaveModel(model);
        }
    }
}
