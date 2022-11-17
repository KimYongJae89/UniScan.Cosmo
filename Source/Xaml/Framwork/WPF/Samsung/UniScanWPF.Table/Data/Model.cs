using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Operation.Operators;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Data
{
    public class Model : DynMvp.Data.Model
    {
        int lightValueTop;
        int binarizeValueTop;
        int binarizeValueBack;
        List<MarginMeasurePoint> marginMeasurePointList;
        ObservableCollection<PatternGroup> inspectPatternList;
        ObservableCollection<PatternGroup> candidatePatternList;
        
        public int LightValueTop { get => lightValueTop; set => lightValueTop = value; }
        public ObservableCollection<PatternGroup> InspectPatternList { get => inspectPatternList; }
        public ObservableCollection<PatternGroup> CandidatePatternList { get => candidatePatternList; }
        public int BinarizeValueTop { get => binarizeValueTop; set => binarizeValueTop = value; }
        public int BinarizeValueBack { get => binarizeValueBack; set => binarizeValueBack = value; }
        public List<MarginMeasurePoint> MarginMeasurePointList { get => marginMeasurePointList; }

        public new ModelDescription ModelDescription
        {
            get
            {
                return (ModelDescription)modelDescription;
            }
        }

        public Model()
        {
            inspectPatternList = new ObservableCollection<PatternGroup>();
            candidatePatternList = new ObservableCollection<PatternGroup>();
            marginMeasurePointList = new List<MarginMeasurePoint>();
        }

        public override bool IsTaught()
        {
            return inspectPatternList.Count + candidatePatternList.Count == 0 ? false : true;
        }

        public void ModelTraind(OperatorResult operatorResult)
        {
            if (operatorResult.Type != ResultType.Train)
                return;

            TeachOperatorResult teachOperatorResult = (TeachOperatorResult)operatorResult;
            
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.inspectPatternList.Clear();
                this.candidatePatternList.Clear();

                foreach (PatternGroup pg in teachOperatorResult.InspectPatternList)
                    this.inspectPatternList.Add(pg);

                foreach(PatternGroup pg in teachOperatorResult.CandidatePatternList)
                    this.candidatePatternList.Add(pg);
                
                ModelDescription.IsTeached = true;

                SimpleProgressWindow teachWindow = new SimpleProgressWindow("Save");
                teachWindow.Show(() =>
                {
                    SystemManager.Instance().ModelManager.SaveModel(this);
                });
            }));
        }

        public override void LoadModel(XmlElement xmlElement)
        {
            base.LoadModel(xmlElement);

            lightValueTop = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "LightValueTop", "0"));
            binarizeValueTop = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "BinarizeValueTop", "100"));
            binarizeValueBack = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "BinarizeValueBack", "100"));
            if (xmlElement.GetElementsByTagName("LightValue").Count > 0)
                lightValueTop = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "LightValue", "0"));

            if (binarizeValueTop == 0) binarizeValueTop = 100;
            if (binarizeValueBack == 0) binarizeValueBack = 100;

            // Load Margin Measure Point
            XmlElement marginMeasureListElement = xmlElement["MarginMeasureList"];
            LoadMarginMeasureList(marginMeasureListElement);

            int index = 0;
            string inspectPath = Path.Combine(ModelPath, "Inspect");
            if (Directory.Exists(inspectPath))
            {
                foreach (XmlElement childElement in xmlElement)
                {
                    if (childElement.Name.Contains("Inspect"))
                    {
                        PatternGroup patternGroup = new PatternGroup();
                        patternGroup.Load(inspectPath, index++, childElement);

                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            inspectPatternList.Add(patternGroup);
                        }));
                    }
                }
            }

            index = 0;
            string candidatePath = Path.Combine(ModelPath, "Candidate");
            if (Directory.Exists(candidatePath))
            {
                foreach (XmlElement childElement in xmlElement)
                {
                    if (childElement.Name.Contains("Candidate"))
                    {
                        PatternGroup patternGroup = new PatternGroup();
                        patternGroup.Load(candidatePath, index++, childElement);

                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            candidatePatternList.Add(patternGroup);
                        }));
                    }
                }
            }
        }

        private void LoadMarginMeasureList(XmlElement xmlElement)
        {
            this.marginMeasurePointList.Clear();
            if (xmlElement == null)
            {
                this.marginMeasurePointList.Add(new MarginMeasurePoint(ReferencePos.LT, new Point(30, 30)));
                this.marginMeasurePointList.Add(new MarginMeasurePoint(ReferencePos.RT, new Point(30, 30)));
                this.marginMeasurePointList.Add(new MarginMeasurePoint(ReferencePos.RB, new Point(30, 30)));
                this.marginMeasurePointList.Add(new MarginMeasurePoint(ReferencePos.LB, new Point(30, 30)));
                //this.marginMeasurePointList.Add(new MarginMeasurePoint(ReferencePos.CT, new Point(-30, -30)));
                return;
            }

            XmlNodeList subElementList = xmlElement.GetElementsByTagName("MarginMeasurePoint");
            foreach (XmlElement subElement in subElementList)
            {
                this.marginMeasurePointList.Add(MarginMeasurePoint.Load(subElement));
            }
        }

        public override void SaveModel(XmlElement xmlElement)
        {
            base.SaveModel(xmlElement);

            XmlHelper.SetValue(xmlElement, "LightValueTop", lightValueTop.ToString());
            XmlHelper.SetValue(xmlElement, "BinarizeValueTop", binarizeValueTop.ToString());
            XmlHelper.SetValue(xmlElement, "BinarizeValueBack", binarizeValueBack.ToString());
            SaveMarginMeasureList(xmlElement);

            string inspectPath = Path.Combine(ModelPath, "Inspect");

            if (Directory.Exists(inspectPath) == false)
                Directory.CreateDirectory(inspectPath);

            for (int i = 0; i < inspectPatternList.Count; i++)
            {
                XmlElement patternGroupElement = xmlElement.OwnerDocument.CreateElement(string.Format("Inspect{0}", i));
                inspectPatternList[i].Save(inspectPath, i, patternGroupElement);
                xmlElement.AppendChild(patternGroupElement);
            }

            string candidatePath = Path.Combine(ModelPath, "Candidate");

            if (Directory.Exists(candidatePath) == false)
                Directory.CreateDirectory(candidatePath);
            
            for (int i = 0; i < candidatePatternList.Count; i++)
            {
                XmlElement patternGroupElement = xmlElement.OwnerDocument.CreateElement(string.Format("Candidate{0}", i));
                candidatePatternList[i].Save(candidatePath, i, patternGroupElement);
                xmlElement.AppendChild(patternGroupElement);
            }
        }

        private void SaveMarginMeasureList(XmlElement xmlElement)
        {
            XmlElement subElement =xmlElement.OwnerDocument.CreateElement("MarginMeasurePointList");
            xmlElement.AppendChild(subElement);

            this.marginMeasurePointList.ForEach(f => f.Save(subElement, "MarginMeasurePoint"));
        }
    }

    public class ModelManager : UniEye.Base.Data.ModelManager
    {
        public override DynMvp.Data.Model CreateModel()
        {
            return new Model();
        }

        public ModelManager() : base()
        {
            Init(modelPath);
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
        
        public bool IsModelExist(DynMvp.Data.ModelDescription modelDescription)
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

        public override void DeleteModel(DynMvp.Data.ModelDescription modelDescription)
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

        public override bool SaveModel(DynMvp.Data.Model model)
        {
            if (model == null)
                return false;

            model.ModelPath = GetModelPath((ModelDescription)model.ModelDescription);
            return base.SaveModel(model);
        }
    }

    public class ModelDescription : DynMvp.Data.ModelDescription
    {
        float thickness;
        public float Thickness
        {
            get { return thickness; }
            set { thickness = value; }
        }

        string paste;
        public string Paste
        {
            get { return paste; }
            set { paste = value; }
        }

        bool isTeached;
        public bool IsTeached
        {
            get { return isTeached; }
            set { isTeached = value; }
        }

        public override bool Equals(object obj)
        {
            bool same = base.Equals(obj);
            if (same == false)
                return false;

            ModelDescription md = obj as ModelDescription;
            return thickness == md.thickness && paste == md.paste;
        }

        public override void Load(XmlElement modelDescElement)
        {
            base.Load(modelDescElement);

            Name = XmlHelper.GetValue(modelDescElement, "Name", "");
            paste = XmlHelper.GetValue(modelDescElement, "Paste", "");
            thickness = Convert.ToSingle(XmlHelper.GetValue(modelDescElement, "Thickness", "0"));
            isTeached = XmlHelper.GetValue(modelDescElement, "IsTeached", false);
        }

        public override void Save(XmlElement modelDescElement)
        {
            base.Save(modelDescElement);

            XmlHelper.SetValue(modelDescElement, "Name", Name.ToString());
            XmlHelper.SetValue(modelDescElement, "Thickness", thickness.ToString());
            XmlHelper.SetValue(modelDescElement, "Paste", paste);
            XmlHelper.SetValue(modelDescElement, "IsTeached", isTeached);
        }

        public override DynMvp.Data.ModelDescription Clone()
        {
            ModelDescription discription = new ModelDescription();

            discription.Copy(this);

            return discription;
        }

        public override void Copy(DynMvp.Data.ModelDescription srcDesc)
        {
            base.Copy(srcDesc);
            ModelDescription md = (ModelDescription)srcDesc;
            thickness = md.thickness;
            paste = md.paste;
        }
    }
}
