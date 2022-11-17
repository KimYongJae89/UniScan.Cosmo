using System;
using System.Windows.Forms;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.UI.Touch;
using UniEye.Base.Device;
using UniEye.Base.Data;
using System.Drawing;
using System.Threading;
using DynMvp.Authentication;
using UniEye.Base.UI;
using UniScanG;
using UniScan.Common;
using UniScan.Common.Exchange;
using UniEye.Base.Settings;
using System.Linq;

namespace UniScanG.UI
{
    public partial class InspectorMainForm : Form, IVisitListener, IMultiLanguageSupport
    {
        LogInForm loginForm = new LogInForm();

        ContextMenu opContextMenu = new ContextMenu();
        ContextMenu masterContextMenu = new ContextMenu();

        IMainTabPage curSelectedPage = null;

        IMainTabPage inspectPanel;
        IMainTabPage modelPanel;
        IMainTabPage teachPanel;
        IMainTabPage reportPanel;
        ISettingPage settingPanel;
        
        public InspectorMainForm()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens.LastOrDefault(f => !f.Primary);
            if (screen == null)
                screen = System.Windows.Forms.Screen.AllScreens.First();

            this.MaximizedBounds = screen.WorkingArea;
            this.MinimumSize = screen.WorkingArea.Size;
            this.WindowState = FormWindowState.Maximized;

            //loginForm.TopMost = true;

            InitPanels();

            InitContextMenu();

            IClientExchangeOperator clientExchangeOperator = (IClientExchangeOperator)SystemManager.Instance().ExchangeOperator;
            clientExchangeOperator.AddVisitListener(this);
        }

        private void InitPanels()
        {
            InspectorUiChanger inspectorUiChanger = (InspectorUiChanger)SystemManager.Instance().UiChanger;

            inspectPanel = inspectorUiChanger.CreateInspectPage();
            modelPanel = inspectorUiChanger.CreateModelPage();
            teachPanel = inspectorUiChanger.CreateTeachPage();
            reportPanel = inspectorUiChanger.CreateReportPage();
            settingPanel = inspectorUiChanger.CreateSettingPage();
        }

        private void InitContextMenu()
        {
            MenuItem versiontMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Version"), Version_Clicked);
            MenuItem inspectMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Inspect"), Inspect_Clicked);
            MenuItem modelMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Model"), Model_Clicked);
            MenuItem teachMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Teach"), Teach_Clicked);
            MenuItem reportMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Report"), Report_Clicked);
            MenuItem settingMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Setting"), Setting_Clicked);
            MenuItem closeMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Close"), Close_Clicked);
            MenuItem exitMenuItem1 = new MenuItem(StringManager.GetString(this.GetType().FullName, "Exit"), Exit_Clicked);
            MenuItem exitMenuItem2 = new MenuItem(StringManager.GetString(this.GetType().FullName, "Exit"), Exit_Clicked);

            masterContextMenu.MenuItems.Add(versiontMenuItem);
            masterContextMenu.MenuItems.Add(new MenuItem("-"));
            masterContextMenu.MenuItems.Add(inspectMenuItem);
            masterContextMenu.MenuItems.Add(modelMenuItem);
            masterContextMenu.MenuItems.Add(teachMenuItem);
            masterContextMenu.MenuItems.Add(reportMenuItem);
            masterContextMenu.MenuItems.Add(settingMenuItem);
            masterContextMenu.MenuItems.Add(new MenuItem("-"));
            masterContextMenu.MenuItems.Add(closeMenuItem);
            masterContextMenu.MenuItems.Add(exitMenuItem1);

            opContextMenu.MenuItems.Add(exitMenuItem2);
            notifyIcon.ContextMenu = masterContextMenu;

        }

        private void Version_Clicked(object sender, EventArgs e)
        {
            MessageForm.Show(this, string.Format("Version {0}, Build {1}", VersionHelper.Instance().VersionString, VersionHelper.Instance().BuildString));
        }

        private void Inspect_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_INSPECT);
            this.WindowState = FormWindowState.Maximized;
        }

        private void Model_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_MODEL);
            this.WindowState = FormWindowState.Maximized;
        }

        private void Teach_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_TEACH);
            this.WindowState = FormWindowState.Maximized;
        }

        private void Report_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_REPORT);
            this.WindowState = FormWindowState.Maximized;
        }

        private void Setting_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_SETTING);
            this.WindowState = FormWindowState.Maximized;
        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (loginForm.Visible)
                return;

            if (loginForm.ShowDialog() == DialogResult.OK)
                UserHandler.Instance().CurrentUser = loginForm.LogInUser;

            if (UserHandler.Instance().CurrentUser.SuperAccount == true)
                notifyIcon.ContextMenu = masterContextMenu;
            else
                notifyIcon.ContextMenu = opContextMenu;
        }

        private void TrayIconForm_Load(object sender, EventArgs e)
        {
#if DEBUG
            this.FormBorderStyle = FormBorderStyle.Sizable;
#endif
            System.Windows.Forms.Screen[] sc = System.Windows.Forms.Screen.AllScreens;
            int screenIndex = sc.Length > 2 ? 2 : 0;
            this.MaximizedBounds = sc[screenIndex].WorkingArea;

            this.WindowState = FormWindowState.Maximized;

            panelContol.Controls.Add((Control)inspectPanel);
            panelContol.Controls.Add((Control)modelPanel);
            panelContol.Controls.Add((Control)teachPanel);
            panelContol.Controls.Add((Control)reportPanel);


            panelContol.Controls.Clear();
            panelContol.Controls.Add((Control)modelPanel);

            string title = CustomizeSettings.Instance().ProgramTitle;
            string copyright = CustomizeSettings.Instance().Copyright;
            this.Text = string.Format("{0} @ {1}, Version {2} Build {3}", title, copyright, VersionHelper.Instance().VersionString, VersionHelper.Instance().BuildString);

            //this.Show();
            //this.Hide();
        }

        private void TrayIconForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        
        private void TrayIconForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon.Dispose();
        }

        delegate void PreparePanelDelegate(ExchangeCommand eVisit);
        public void PreparePanel(ExchangeCommand eVisit)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new PreparePanelDelegate(PreparePanel), eVisit);
                return;
            }

            IMainTabPage selectedControl = null;
            switch (eVisit)
            {
                case ExchangeCommand.V_INSPECT:
                    selectedControl = inspectPanel;
                    break;
                case ExchangeCommand.V_MODEL:
                    selectedControl = modelPanel;
                    break;
                case ExchangeCommand.V_TEACH:
                    //if(SystemManager.Instance().CurrentModel==null)
                    //{
                    //    MessageForm.Show(this, "Model is NOT Selected");
                    //    break;
                    //}
                    selectedControl = teachPanel;
                    break;
                case ExchangeCommand.V_REPORT:
                    selectedControl = reportPanel;
                    break;
                case ExchangeCommand.V_SETTING:
                    selectedControl = settingPanel;
                    break;
                case ExchangeCommand.V_DONE:
                    this.Hide();
                    break;
            }

            if (curSelectedPage != selectedControl)
            {
                if (curSelectedPage != null)
                    curSelectedPage.PageVisibleChanged(false);

                if (selectedControl != null)
                {
                    panelContol.Controls.Clear();
                    panelContol.Controls.Add((Control)selectedControl);
                    selectedControl.PageVisibleChanged(true);
                }
                curSelectedPage = selectedControl;
            }
            this.WindowState = FormWindowState.Maximized;
            this.Show();
        }

        delegate void ClearDelegate();
        public void Clear()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ClearDelegate(Clear));
                return;
            }

            //panelContol.Controls.Clear();
            this.Hide();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            // 뭐 없네
        }

        bool isConnected = false;
        private void timer_Tick(object sender, EventArgs e)
        {
            //((IClientExchangeOperator)SystemManager.Instance().ExchangeOperator).SendAlive();
            bool isConnected = ((IClientExchangeOperator)SystemManager.Instance().ExchangeOperator).IsConnected;
            if (this.isConnected != isConnected)
            {
                this.isConnected = isConnected;
                if (isConnected)
                    this.Show();
                else
                {
                    if(SystemState.Instance().GetOpState() != OpState.Idle)
                        SystemManager.Instance().InspectRunner.ExitWaitInspection();
                    this.Hide();
                }
            }
        }
    }
}
