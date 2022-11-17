using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Data;

namespace UniEye.Base.Data
{
    public class Production : DynMvp.Data.ProductionBase
    {
        public Production(string name, string lotNo) : base(name, lotNo) { }
        public Production(XmlElement xmlElement) : base(xmlElement) { }

        public override string GetResultPath()
        {
            return PathManager.GetResultPath(this.Name, this.StartTime, this.LotNo);
        }
    }

    public class ProductionManager : DynMvp.Data.ProductionManagerBase
    {
        public ProductionManager(string defaultPath) : base(defaultPath) { }

        public override ProductionBase CreateProduction(Model model, string lotNo)
        {
            return new Production(model == null ? "" : model.Name, lotNo);
        }

        public override ProductionBase CreateProduction(XmlElement productionElement)
        {
            return new Production(productionElement);
        }
    }

}
