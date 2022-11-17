using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data;

namespace UniScanG.Gravure.Data
{
    public class ProductionManagerG : UniScanG.Data.ProductionManager
    {
        public ProductionManagerG(string defaultPath) : base(defaultPath)
        {
        }

        public override ProductionBase CreateProduction(Model model, string lotNo)
        {
            UniScanG.Data.Model.Model modelG = (UniScanG.Data.Model.Model)model;
            string modelName = "";
            if (modelG != null)
                modelName = modelG.Name;
            return new ProductionG(modelName, lotNo, modelG.ModelDescription.Thickness, modelG.ModelDescription.Paste, -1);
        }

        public override ProductionBase CreateProduction(XmlElement productionElement)
        {
            return new ProductionG(productionElement);
        }

        public override Production LotChange(Model model, string lotNo, float lineSpeed)
        {
            ProductionG productionG = base.LotChange(model, lotNo) as ProductionG;
            productionG.UpdateLineSpeedMpm(lineSpeed);
            return productionG;
        }
    }
}
