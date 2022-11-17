using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using UniScan.Common.Util;
using System.Drawing;
using DynMvp.Authentication;
using DynMvp.UI;
using UniEye.Base.Data;
using DynMvp.Base;

namespace UniScanG.UI.Etc
{
    public partial class InspectorStatusStripPanel : UserControl, IStatusStripPanel, IMultiLanguageSupport
    {
        InspectorObj inspector;

        public InspectorStatusStripPanel(InspectorObj inspector)
        {
            InitializeComponent();
            
            this.TabIndex = 0;

            this.inspector = inspector;
            
            StringManager.AddListener(this);

            StateUpdate();
        }
        
        public void StateUpdate()
        {
            labelOpStatus.Text = StringManager.GetString(this.GetType().FullName, inspector.OpState.ToString());
            labelInspectStatus.Text = StringManager.GetString(this.GetType().FullName, inspector.InspectState.ToString());

            switch (inspector.CommState)
            {
                case CommState.CONNECTED:
                    labelConnect.BackColor = Colors.Connected;
                    break;
                case CommState.DISCONNECTED:
                    labelConnect.BackColor = Colors.Disconnected;
                    break;
            }

            switch (inspector.InspectState)
            {
                case InspectState.Run:
                    labelInspectStatus.BackColor = Colors.Run;
                    break;
                case InspectState.Done:
                    labelInspectStatus.BackColor = Colors.Idle;
                    break;
                case InspectState.Wait:
                    labelInspectStatus.BackColor = Colors.Wait;
                    break;
            }

            switch (inspector.OpState)
            {
                case OpState.Idle:
                    labelOpStatus.BackColor = Colors.Idle;
                    break;
                case OpState.Wait:
                    labelOpStatus.BackColor = Colors.Wait;
                    break;
                case OpState.Inspect:
                    labelOpStatus.BackColor = Colors.Run;
                    break;
                case OpState.Teach:
                    labelOpStatus.BackColor = Colors.Teach;
                    break;
                case OpState.Alarm:
                    labelOpStatus.BackColor = Colors.Alarm;
                    break;
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            labelConnect.Text = StringManager.GetString(this.GetType().FullName, inspector.Info.GetName());
        }
    }
}