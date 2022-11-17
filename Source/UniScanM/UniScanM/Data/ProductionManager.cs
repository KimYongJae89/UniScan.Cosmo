using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Data;
using UniEye.Base.Settings;

namespace UniScanM.Data
{
    public class ProductionManager : UniEye.Base.Data.ProductionManager
    {
        public ProductionManager(string defaultPath) : base(defaultPath) { }

        public new UniScanM.Data.Production CurProduction
        {
            get { return (UniScanM.Data.Production)curProduction; }
        }

        public override DynMvp.Data.ProductionBase GetProduction(DynMvp.Data.Model model, string lotNo)
        {
            Model modelM = (Model)model;
            return list.Find(p => p.Name == model.Name && ((Production)p).Paste == modelM.ModelDescription.Paste && p.LotNo == lotNo);
        }

        public override DynMvp.Data.ProductionBase GetProduction(DynMvp.Data.Model model, DateTime date, string lotNo)
        {
            Model modelM = (Model)model;
            return list.Find(p => p.Name == model.Name && ((Production)p).Paste == modelM.ModelDescription.Paste && p.StartTime.Date == date.Date && p.LotNo == lotNo);
        }

        public Production GetProduction(UniScanM.Data.InspectionResult inspectionResult)
        {
            return (Production)list.LastOrDefault(f =>
            (f is Production) &&
            (f.Name == inspectionResult.ModelName) &&
            (f.LotNo == inspectionResult.LotNo) &&
            (((Production)f).Worker == inspectionResult.Wroker));
        }

        public override DynMvp.Data.ProductionBase GetLastProduction(DynMvp.Data.Model model)
        {
            Model modelM = (Model)model;
            return list.FindLast(p => p.Name == model.Name && ((Production)p).Paste == modelM.ModelDescription.Paste);
        }

        public int LotExistCount(DateTime dateTime, string name, string worker, string lotNo, string paste, string mode, int rewinderSite)
        {
            Production production = (Production)list.FindLast(prod =>
            {
                Production p = (Production)prod;
                bool same = (p.StartTime.Date == dateTime.Date && p.Name == name && ((Production)p).Worker == worker && ((Production)p).Paste == paste && ((Production)p).Mode == mode);
                if (same == false)
                    return false;

                string[] tokens = p.LotNo.Split('_');
                return tokens[0] == lotNo;
            });

            if (production == null)
                return 0;

            string[] tokens2 = production.LotNo.Split('_');
            int idx = (tokens2.Length == 1) ? 0 : Convert.ToInt32(tokens2[1]);
            if (production.RewinderSite != rewinderSite)
                idx++;

            return idx;
        }

        public int LotExistCount(string name, string lotNo, string paste, string mode)
        {
            return list.Count(p => 
            {
                Production production = (Production)p;
                bool same = p.Name == name && ((Production)p).Paste == paste && ((Production)p).Mode == mode;
                if (same == false)
                    return false;

                string[] tokens = production.LotNo.Split('_');
                return tokens[0] == lotNo;
            });
        }

        public Production LotChange(string name, string worker, string lotNo, string paste, string mode, int rewinderSite)
        {
            Production production = GetProduction(name, worker,lotNo, paste, rewinderSite, mode);
            if (production == null)
            {
                production = CreateProduction(name, worker, lotNo, paste, mode, rewinderSite);
                list.Add(production);
                this.Save(PathSettings.Instance().Result);
            }

            curProduction = production;
            return production;
        }

        private Production GetProduction(string name, string worker, string lotNo, string paste, int rewinderSite, string mode)
        {
            return (Production)list.Find(p => p.Name == name && ((Production)p).Worker == worker && ((Production)p).Paste == paste && p.LotNo == lotNo && ((Production)p).Mode == mode && ((Production)p).RewinderSite == rewinderSite);
        }

        public virtual Production CreateProduction(string name,string worker, string lotNo, string paste, string mode,int rewinderSite)
        {
            Production newProduction = new Production(name, worker, lotNo, paste, mode, rewinderSite);
            return newProduction;
        }

        public override DynMvp.Data.ProductionBase CreateProduction(XmlElement productionElement)
        {
            Production production = new Production(productionElement);
            return production;
        }
    }
}