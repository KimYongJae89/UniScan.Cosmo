using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.UI.Teach.Inspector;
using UniScanG.Data;
using DynMvp.Base;

namespace UniScanG.Gravure.UI.Teach.Inspector
{
    public partial class DefectLegendPanel : UserControl, IDefectLegend, IMultiLanguageSupport
    {
        public DefectLegendPanel()
        {
            InitializeComponent();

            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
