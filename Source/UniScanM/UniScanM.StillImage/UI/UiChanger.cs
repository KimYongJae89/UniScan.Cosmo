using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.UI;
using UniScanM.StillImage.UI.MenuPage;

namespace UniScanM.StillImage.UI
{
    internal class UiChanger : UniScanM.UI.UiChanger
    {
        public override IInspectionPanel CreateInspectionPanel(int index)
        {
            switch (index)
            {
                case 0:
                    return new InspectionPanelLeft();
                case 1:
                    return new InspectionPanelRight();
            }

            return null;
        }

        public override UniScanM.UI.ReportPageController CreateReportPageController()
        {
            return new ReportPageController();
        }

        public override IReportPanel CreateReportPanel()
        {
            return new UI.ReportPanel();
        }

        public override ISettingPage CreateSettingPage()
        {
            return new UniScanM.StillImage.UI.MenuPage.SettingPage();
        }

        public override ITeachPage CreateTeachPage()
        {
            return null;
        }
    }
}
