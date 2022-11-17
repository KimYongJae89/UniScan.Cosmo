using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data;

namespace UniScanG.Screen.Data
{
    public class ProductionManagerS : UniScanG.Data.ProductionManager
    {
        public ProductionManagerS(string defaultPath) : base(defaultPath)
        {
        }

        public override ProductionBase CreateProduction(DynMvp.Data.Model model, string lotNo = "")
        {
            UniScanG.Data.Model.Model modelG = (UniScanG.Data.Model.Model)model;

            ProductionBase newProduction;

            if (model == null)
                newProduction = new ProductionS("", 0, "", lotNo);
            else
                newProduction = new ProductionS(modelG.Name, modelG.ModelDescription.Thickness, modelG.ModelDescription.Paste, lotNo);
            
            newProduction.StartTime = DateTime.Now;

            //list.Add(newProduction);

            return newProduction;
        }

        public override ProductionBase CreateProduction(XmlElement productionElement)
        {
            return new ProductionS(productionElement);
        }

        public override Production LotChange(Model model, string lotNo, float lineSpeed)
        {
            return (base.LotChange(model, lotNo) as ProductionS);
        }
    }
}
