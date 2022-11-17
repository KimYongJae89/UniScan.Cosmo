using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DynMvp.Data;
using UniScanG.Data.Model;

namespace UniScanG.Data
{
    public abstract class ProductionManager : DynMvp.Data.ProductionManagerBase
    {
        public ProductionManager(string defaultPath) : base(defaultPath)
        {
        }

        public abstract Production LotChange(DynMvp.Data.Model model, string lotNo, float lineSpeed);

        public override DynMvp.Data.ProductionBase GetProduction(DynMvp.Data.Model model, string lotNo)
        {
            ModelDescription modelDescription = (ModelDescription)model.ModelDescription;

            return list.Find(p => ((Production)p).Name == modelDescription.Name && ((Production)p).Paste == modelDescription.Paste && ((Production)p).Thickness == modelDescription.Thickness && p.LotNo == lotNo);
        }

        public override DynMvp.Data.ProductionBase GetProduction(DynMvp.Data.Model model,DateTime date, string lotNo)
        {
            ModelDescription modelDescription = (ModelDescription)model.ModelDescription;

            return list.Find(p =>
            {
                Production production = p as Production;
                return production.Name == modelDescription.Name &&
                production.Paste == modelDescription.Paste &&
                production.Thickness == modelDescription.Thickness &&
                production.StartTime.Date == date.Date &&
                production.LotNo == lotNo;
            });
        }

        public override DynMvp.Data.ProductionBase GetLastProduction(DynMvp.Data.Model model)
        {
            ModelDescription modelDescription = (ModelDescription)model.ModelDescription;

            List<DynMvp.Data.ProductionBase> lastList = list.FindAll(p => ((Production)p).Name == modelDescription.Name && ((Production)p).Paste == modelDescription.Paste && ((Production)p).Thickness == modelDescription.Thickness);

            if (lastList.Count == 0)
                return null;

            return lastList.Last();
        }
    }
}
