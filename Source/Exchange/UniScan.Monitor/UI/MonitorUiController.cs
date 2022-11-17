using UniScan.Common;
using UniScan.Common.Exchange;
using UniScan.Common.UI;

namespace UniScan.Monitor.UI
{
    public class MonitorUiController : UniScan.Common.UI.UiController
    {
        public override void UpdateTab(bool trained)
        {
            //SystemManager.Instance().ExchangeOperator.SendCommand(ETab.UPDATE, trained.ToString());

            base.UpdateTab(trained);
        }
        
        public override void TabChanged(string key)
        {
            //SystemManager.Instance().ExchangeOperator.SendCommand(ETab.CHANGE, key.ToString());
        }
    }
}
