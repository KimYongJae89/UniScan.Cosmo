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
    public partial class DefectTypeFilterPanel : UserControl, IDefectTypeFilter, IMultiLanguageSupport
    {
        private OnDefectTpyeSelectChangedDelegate OnDefectTpyeSelectChanged;
        public DefectTypeFilterPanel()
        {
            InitializeComponent();

            StringManager.AddListener(this);
        }

        public void SetDefectTpyeSelectChanged(OnDefectTpyeSelectChangedDelegate onDefectTpyeSelectChanged)
        {
            OnDefectTpyeSelectChanged = onDefectTpyeSelectChanged;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void type_CheckedChanged(object sender, EventArgs e)
        {
            DefectType defectType = DefectType.Total;
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null)
            {
                if (radioButton.Name == this.typeTotal.Name)
                    defectType = DefectType.Total;
                else if (radioButton.Name == this.typeDielectric.Name)
                    defectType = DefectType.Dielectric;
                else if (radioButton.Name == this.typePinHole.Name)
                    defectType = DefectType.PinHole;
                else if (radioButton.Name == this.typeNotprint.Name)
                    defectType = DefectType.Noprint;
                else if (radioButton.Name == this.typeSheetAttack.Name)
                    defectType = DefectType.SheetAttack;
            }

            if (OnDefectTpyeSelectChanged != null)
                OnDefectTpyeSelectChanged(defectType);
        }
    }
}
