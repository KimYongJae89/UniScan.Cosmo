using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using UniEye.Base;
using UniEye.Base.UI;
using UniScanM.Data;

namespace UniScanM.UI
{
    public abstract class UiChanger : UniEye.Base.UI.UiChanger
    {
        public override IMainForm CreateMainForm()
        {
            return (IMainForm)(new MainForm());
        }

        public abstract ITeachPage CreateTeachPage();
        public abstract ReportPageController CreateReportPageController();
    }

    public delegate void UpdateDataDelegate(DataImporter dataImporter,bool showNgOnly);
    public interface IUniScanMReportPanel : IReportPanel
    {
        string AxisYFormat { get; }
        bool ShowNgOnlyButton();

        void UpdateData(DataImporter dataImporter, bool showNgOnly);

        Dictionary<string, List<DataPoint>> GetChartData(int startPos, int endPos, DataImporter dataImporter);
        
        DataImporter CreateDataImprter();

        void InitializeChart(Chart chart);
    }
}
