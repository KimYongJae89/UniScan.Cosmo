using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Data
{
    public abstract class ProductionManagerBase
    {
        string defaultPath = "";
        protected ProductionBase curProduction = null;
        public ProductionBase CurProduction
        {
            get { return curProduction; }
        }

        protected List<ProductionBase> list = new List<ProductionBase>();
        public List<ProductionBase> List
        {
            get { return list; }
        }

        public string DefaultPath { get => defaultPath; }

        public ProductionManagerBase(string defaultPath)
        {
            this.defaultPath = defaultPath;
            //this.Load(defaultPath);
        }
        public List<ProductionBase> FindAll(DateTime startTime, DateTime endTime)
        {
            return list.FindAll(f => startTime < f.StartTime && f.StartTime <= endTime);
        }
        
        public void Clear()
        {
            this.curProduction = null;
            list.Clear();
        }

        public bool LotExist(Model model, string lotNo)
        {
            ProductionBase production = GetProduction(model, lotNo);
            return production != null;
        }

        public bool LotExist(Model model, DateTime date, string lotNo)
        {
            ProductionBase production = GetProduction(model, date, lotNo);
            return production != null;
        }

        public virtual ProductionBase LotChange(Model model, string lotNo)
        {
            LogHelper.Debug(LoggerType.Operation, string.Format("ProductionManager::LotChange. {0}", lotNo));

            ProductionBase production = GetProduction(model, lotNo);
            if (production == null)
            {
                production = CreateProduction(model, lotNo);
                this.list.Add(production);
                this.Save();
            }

            curProduction = production;
            return curProduction;
        }

        public bool LotChange(Model model, DateTime date, string lotNo)
        {
            LogHelper.Debug(LoggerType.Operation, string.Format("ProductionManager::LotChange. {0}", lotNo));

            ProductionBase production = GetProduction(model, date, lotNo);
            if (production == null)
            {
                production = CreateProduction(model, lotNo);
                this.list.Add(production);
                this.Save();
            }

            curProduction = production;
            return true;
        }

        public abstract ProductionBase CreateProduction(Model model, string lotNo);
        //{
        //    return BuildProduction(model, lotNo);
        //}

        public abstract ProductionBase CreateProduction(XmlElement productionElement);
        //{
        //    return BuildProduction(productionElement);
        //}

        public bool RemoveProduction(ProductionBase production)
        {
            lock (this.list)
            {
                if (this.list.Remove(production))
                {
                    this.Save();
                    return true;
                }
                return false;
            }
        }

        public virtual ProductionBase GetProduction(Model model, string lotNo)
        {
            return list.Find(p => p.Name == model.Name && p.LotNo == lotNo);
        }

        public virtual ProductionBase GetProduction(Model model, DateTime date, string lotNo)
        {
            return list.Find(p => p.Name == model.Name && p.StartTime.Date == date.Date && p.LotNo == lotNo);
        }

        public virtual ProductionBase GetLastProduction(Model model)
        {
            List<ProductionBase> lastList = list.FindAll(p => p.Name == model.Name);

            if (lastList.Count == 0)
                return null;
            
            return lastList.Last();
        }

        public int GetTodayCount()
        {
            int count = 0;
            foreach (ProductionBase production in list)
            {
                if (production.StartTime.DayOfYear == DateTime.Today.DayOfYear)
                    count++;
            }

            return count;
        }

        public List<ProductionBase> Load(string filePath = "", string fileName = "ProductionList.xml")
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = this.defaultPath;

            list.Clear();

            string oldFileName = Path.Combine(filePath, "ProductionList.csv");
            string fullFileName = Path.Combine(filePath, fileName);

            if (File.Exists(fullFileName) == false)
            {
                if (File.Exists(oldFileName))
                    File.Move(oldFileName, fullFileName);
                else
                    return list;
            }

            try
            {
                if (File.Exists(fullFileName) == false)
                    return list;

                XmlDocument xmlDocument = XmlHelper.Load(fullFileName);
                if (xmlDocument == null)
                    return list;

                XmlElement productionListElement = xmlDocument.DocumentElement;
                XmlNodeList xmlNodeList = productionListElement.GetElementsByTagName("Production");
                foreach (XmlElement productionElement in xmlNodeList)
                {
                    ProductionBase production = CreateProduction(productionElement);
                    //production.Load(productionElement);

                    if (list.Exists(f => f.Equals(production)) == false)
                        list.Add(production);
                }

                if (list.Count > 0)
                    list = list.OrderBy(f => f.StartTime).ToList();
            }
            catch (Exception ex)
            {
                // 로드 실패시 프로덕션 메니지 백업
                LogHelper.Error(LoggerType.Error, string.Format("ProductionManager::Load - {0}", ex.Message));
                if (File.Exists(fullFileName))
                    Archive(fullFileName);
            }
            return list;
        }

        public void Save(string filePath = "", string fileName = "ProductionList.xml")
        {
            if (string.IsNullOrEmpty(filePath))
                filePath = this.defaultPath;

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement productionListElement = xmlDocument.CreateElement("", "ProductionList", "");
            xmlDocument.AppendChild(productionListElement);

            lock (list)
            {
                list.ForEach(f =>
                {
                    if (f != null)
                    {
                        lock (f)
                        {
                            XmlElement productionElement = xmlDocument.CreateElement("", "Production", "");
                            f.Save(productionElement);
                            productionListElement.AppendChild(productionElement);
                        }
                    }
                });
            }
            string fullFileName = Path.Combine(filePath, fileName);
            string tempFileName = Path.Combine(filePath, "ProductionList.xm_");
            string backupFileName = Path.Combine(filePath, "ProductionList.xml.bak");

            try
            {
                xmlDocument.Save(tempFileName);
                //if (File.Exists(fullFileName))
                //{
                //    //FileInfo fileInfo = new FileInfo(fullFileName);
                //    //FileInfo tempFileInfo = new FileInfo(tempFileName);
                //    //if (fileInfo.Length > tempFileInfo.Length)
                //    //    Archive(fullFileName);

                //    File.Delete(fullFileName);
                //    File.Move(tempFileName, fullFileName);
                //}
                FileHelper.SafeSave(tempFileName, backupFileName, fullFileName);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ProductionManager::Save - {0}", ex.Message));
            }
        }

        private void Archive(string fileName)
        {
            if (File.Exists(fileName))
            {
                string path = Path.GetDirectoryName(fileName);
                string name = Path.GetFileNameWithoutExtension(fileName);
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                string ext = Path.GetExtension(fileName);
                string copyFileName = Path.Combine(path, string.Format("{0}.{1}.{2}", name, date, ext));
                if (File.Exists(copyFileName))
                    File.Delete(copyFileName);
                File.Copy(fileName, copyFileName);
            }
        }
    }
    
    public abstract class ProductionBase
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string lotNo;
        public string LotNo
        {
            get { return lotNo; }
            set { lotNo = value; }
        }

        private int lastSequenceNo;
        public int LastSequenceNo
        {
            get { return lastSequenceNo; }
            set { lastSequenceNo = value; }
        }

        public double NgRatio
        {
            get { return (ng * 100.0f / Done); }
        }

        public double GoodRatio
        {
            get { return (good * 100.0f / Done); }
        }

        public double PassRatio
        {
            get { return (pass * 100.0f / Done); }
        }

        private int total;
        public int Total
        {
            get { return total; }
            //set { ng = value; }
        }

        public int Done { get => (good + ng + pass); }
        private int ng;
        public int Ng
        {
            get { return ng; }
            //set { ng = value; }
        }

        private int pass;
        public int Pass
        {
            get { return pass; }
            //set { pass = value; }
        }

        private int good;
        public int Good
        {
            get { return good; }
            //set { good = value; }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = lastUpdateTime = value; }
        }

        protected DateTime lastUpdateTime;
        public DateTime LastUpdateTime
        {
            get { return lastUpdateTime; }
            set { lastUpdateTime = value; }
        }
        
        public ProductionBase(string name , string lotNo)
        {
            this.name = name;
            this.lotNo = lotNo;

            StartTime = DateTime.Now;
            //Reset();
        }

        public ProductionBase(XmlElement productionElement)
        {
            Load(productionElement);
        }

        public virtual ModelDescription GetModelDescription()
        {
            return new ModelDescription { Name = this.name };
        }

        public override bool Equals(object obj)
        {
            ProductionBase production = obj as ProductionBase;
            if (production == null)
                return false;

            return this.name == production.name && this.lotNo == production.lotNo && this.startTime == production.startTime;
        }

        public virtual void Reset()
        {
            total = good = ng = pass = 0;
            lastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// 비동기 검사에서, 검사 진행 중 다음 검사 요청시 InspectionNo가 중복될 수 있음 -> Total 카운트 따로 관리해야 함.
        /// </summary>
        public void AddTotal()
        {
            if (IsDateChanged() && Configuration.AutoResetProductionCount)
            {
                Reset();
            }

            total++;

            lastUpdateTime = DateTime.Now;
        }

        public void AddNG()
        {
            if (IsDateChanged() && Configuration.AutoResetProductionCount)
            {
                Reset();
            }

            ng++;

            lastUpdateTime = DateTime.Now;
        }

        public void AddPass()
        {
            if (IsDateChanged() && Configuration.AutoResetProductionCount)
            {
                Reset();
            }

            pass++;

            lastUpdateTime = DateTime.Now;
        }

        public void AddGood()
        {
            if (IsDateChanged() && Configuration.AutoResetProductionCount)
            {
                Reset();
            }

            good++;

            lastUpdateTime = DateTime.Now;
        }

        public bool IsDateChanged()
        {
            return (lastUpdateTime.Day != DateTime.Now.Day);
        }

        public virtual void Load(XmlElement productionElement)
        {
            name = XmlHelper.GetValue(productionElement, "Name", "");
            lotNo = XmlHelper.GetValue(productionElement, "LotNo", "");
            startTime = Convert.ToDateTime(XmlHelper.GetValue(productionElement, "StartTime", DateTime.Now.ToString()));
            total = Convert.ToInt32(XmlHelper.GetValue(productionElement, "Total", "0"));
            good = Convert.ToInt32(XmlHelper.GetValue(productionElement, "Good", "0"));
            ng = Convert.ToInt32(XmlHelper.GetValue(productionElement, "Ng", "0"));
            pass = Convert.ToInt32(XmlHelper.GetValue(productionElement, "Pass", "0"));
            lastUpdateTime = Convert.ToDateTime(XmlHelper.GetValue(productionElement, "LastUpdateTime", DateTime.Now.ToString()));
            lastSequenceNo = Convert.ToInt32(XmlHelper.GetValue(productionElement, "LastSequenceNo", "0"));
        }

        public virtual void Save(XmlElement productionElement)
        {
            XmlHelper.SetValue(productionElement, "Name", name);
            XmlHelper.SetValue(productionElement, "LotNo", lotNo);
            XmlHelper.SetValue(productionElement, "StartTime", startTime.ToString());
            XmlHelper.SetValue(productionElement, "Total", total.ToString());
            XmlHelper.SetValue(productionElement, "Good", good.ToString());
            XmlHelper.SetValue(productionElement, "Ng", ng.ToString());
            XmlHelper.SetValue(productionElement, "Pass", pass.ToString());
            XmlHelper.SetValue(productionElement, "LastUpdateTime", lastUpdateTime.ToString());
            XmlHelper.SetValue(productionElement, "LastSequenceNo", lastSequenceNo.ToString());
        }

        public abstract string GetResultPath();
    }
}

