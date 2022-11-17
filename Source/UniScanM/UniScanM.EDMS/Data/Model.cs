using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.UI;
using System.Drawing;
using DynMvp.Devices.MotionController;
using DynMvp.Devices;
using System.ComponentModel;
using UniScanM.Data;

namespace UniScanM.EDMS.Data
{
    public class EDMSParam : InspectParam
    {
        double filmThreshold = 10;
        [LocalizedCategoryAttribute("EDMSParam", "Inspection Setting"),
        LocalizedDisplayNameAttribute("EDMSParam", "Film Threshold [D.N.]"),
        LocalizedDescriptionAttribute("EDMSParam", "Film Threshold [D.N.]")]
        public double FilmThreshold
        {
            get { return filmThreshold; }
            set { filmThreshold = value; }
        }

        double coatingThreshold = 50;
        [LocalizedCategoryAttribute("EDMSParam", "Inspection Setting"),
        LocalizedDisplayNameAttribute("EDMSParam", "Coating Threshold [D.N.]"),
        LocalizedDescriptionAttribute("EDMSParam", "Coating Threshold [D.N.]")]
        public double CoatingThreshold
        {
            get { return coatingThreshold; }
            set { coatingThreshold = value; }
        }

        double printingThreshold = 10;
        [LocalizedCategoryAttribute("EDMSParam", "Inspection Setting"),
        LocalizedDisplayNameAttribute("EDMSParam", "Printing Threshold [D.N.]"),
        LocalizedDescriptionAttribute("EDMSParam", "Printing Threshold [D.N.]")]
        public double PrintingThreshold
        {
            get { return printingThreshold; }
            set { printingThreshold = value; }
        }
        
        public override void Export(XmlElement element, string subKey = null)
        {
            XmlHelper.SetValue(element, "FilmThreshold", filmThreshold.ToString());
            XmlHelper.SetValue(element, "CoatingThreshold", coatingThreshold.ToString());
            XmlHelper.SetValue(element, "PrintingThreshold", printingThreshold.ToString());
        }

        public override void Import(XmlElement element, string subKey = null)
        {
            filmThreshold = Convert.ToSingle(XmlHelper.GetValue(element, "FilmThreshold", "0"));
            coatingThreshold = Convert.ToSingle(XmlHelper.GetValue(element, "CoatingThreshold", "0"));
            printingThreshold = Convert.ToSingle(XmlHelper.GetValue(element, "PrintingThreshold", "0"));
        }
    }

    public class Model : UniScanM.Data.Model
    {
        public Model() : base()
        {
            inspectParam = new EDMSParam();
        }

        public new EDMSParam InspectParam
        {
            get { return (EDMSParam)inspectParam; }
        }
    }
}
