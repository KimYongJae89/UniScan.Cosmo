using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Data
{
    public abstract class Production : DynMvp.Data.ProductionBase
    {
        protected float thickness;
        public float Thickness
        {
            get { return thickness; }
        }

        protected string paste;
        public string Paste
        {
            get { return paste; }
        }
        
        public Production(string name, string lotNo, float thickness, string paste) : base(name, lotNo)
        {
            this.thickness = thickness;
            this.paste = paste;
        }

        public Production(XmlElement productionElement) : base (productionElement)
        {

        }

        public override ModelDescription GetModelDescription()
        {
            return new UniScanG.Data.Model.ModelDescription { Name = this.Name, Thickness = this.thickness, Paste = this.paste };
        }

        public abstract void Update(SheetResult sheetResult);
        public abstract string GetResultPath(string root);
    }
}
