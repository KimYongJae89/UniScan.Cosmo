using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Data;

namespace UniScanWPF.Table.Data
{
    public class ProductionManager : DynMvp.Data.ProductionManagerBase
    {
        public new Production CurProduction
        {
            get => (Production)curProduction;
        }

        public ProductionManager(string defaultPath) : base(defaultPath)
        {
            Load();
        }

        public override DynMvp.Data.ProductionBase CreateProduction(DynMvp.Data.Model model, string lotNo)
        {
            return new Production(model.Name, lotNo);
        }

        public override DynMvp.Data.ProductionBase CreateProduction(XmlElement productionElement)
        {
            return new Production(productionElement);
        }

        public override ProductionBase LotChange(DynMvp.Data.Model model, string lotNo)
        {
            lock (this)
            {
                if (model == null || lotNo == null)
                {
                    curProduction = null;
                    InfoBox.Instance.ProductionChanged();
                    return null;
                }

                DynMvp.Data.ProductionBase production = GetProduction(model, lotNo);
                if (production == null)
                {
                    production = CreateProduction(model, lotNo);
                    this.list.Add(production);
                    this.Save();
                }

                curProduction = production;

                InfoBox.Instance.ProductionChanged();
                return curProduction;
            }
        }
    }
}
