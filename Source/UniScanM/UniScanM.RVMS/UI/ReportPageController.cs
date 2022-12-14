using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanM.Data;

namespace UniScanM.RVMS.UI.Chart
{
    public class ReportPageController : UniScanM.UI.ReportPageController
    {
        public override void Initialize(DataGridView dataGridView)
        {
            base.Initialize(dataGridView);

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = StringManager.GetString(this.GetType().FullName, "Diff"), AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells });
        }

        public override void BuildRowData(DataGridViewRow dataGridViewRow, Production production)
        {
            base.BuildRowData(dataGridViewRow, production);

            dataGridViewRow.Cells[4].Value = production.Value.ToString("F3");
        }
    }
}
