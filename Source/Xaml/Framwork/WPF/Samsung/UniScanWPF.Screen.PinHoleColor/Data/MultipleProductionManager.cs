using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Data;
//using UniEye.Base.Data;

namespace UniScanWPF.Screen.PinHoleColor.Data
{
    public class MultipleProductionManager : UniEye.Base.Data.ProductionManager
    {
        public new MultipleProduction CurProduction
        {
            get { return (MultipleProduction)curProduction; }
        }

        public MultipleProductionManager(string defaultPath) : base(defaultPath)
        {
        }

        public override DynMvp.Data.ProductionBase CreateProduction(DynMvp.Data.Model model, string lotNo = "")
        {
            MultipleProduction newProduction;

            if (model == null)
                newProduction = new MultipleProduction("", lotNo);
            else
                newProduction = new MultipleProduction(model.Name, lotNo);

            return newProduction;
        }

        public override DynMvp.Data.ProductionBase CreateProduction(XmlElement productionElement)
        {
            return new MultipleProduction(productionElement);
        }
    }
}
