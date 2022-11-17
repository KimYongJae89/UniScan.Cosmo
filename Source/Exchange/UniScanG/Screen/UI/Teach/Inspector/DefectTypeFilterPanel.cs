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

namespace UniScanG.Screen.UI.Teach.Inspector
{
    public partial class DefectTypeFilterPanel : UserControl, IDefectTypeFilter
    {
        private OnDefectTpyeSelectChangedDelegate OnDefectTpyeSelectChanged;
        public DefectTypeFilterPanel()
        {
            InitializeComponent();
        }

        public void SetDefectTpyeSelectChanged(OnDefectTpyeSelectChangedDelegate onDefectTpyeSelectChanged)
        {
            OnDefectTpyeSelectChanged = onDefectTpyeSelectChanged;
        }

        private void type_CheckedChanged(object sender, EventArgs e)
        {
            DefectType defectType = DefectType.Total;
            RadioButton radioButton = sender as RadioButton;
            if(radioButton!=null)
            {
                if (radioButton.Name == this.typeTotal.Name)
                    defectType = DefectType.Total;
                else if (radioButton.Name == this.typeDielectric.Name)
                    defectType = DefectType.Dielectric;
                else if (radioButton.Name == this.typePinHole.Name)
                    defectType = DefectType.PinHole;
                else if (radioButton.Name == this.typePoleCircle.Name)
                    defectType = DefectType.PoleCircle;
                else if (radioButton.Name == this.typePoleLine.Name)
                    defectType = DefectType.PoleLine;
                else if (radioButton.Name == this.typeShape.Name)
                    defectType = DefectType.Shape;
                else if (radioButton.Name == this.typeSheetAttack.Name)
                    defectType = DefectType.SheetAttack;
            }

            if (OnDefectTpyeSelectChanged != null)
                OnDefectTpyeSelectChanged(defectType);
        }
    }
}
