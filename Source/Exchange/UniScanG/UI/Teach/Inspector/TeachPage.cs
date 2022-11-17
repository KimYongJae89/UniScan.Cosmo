﻿using DynMvp.Authentication;
using DynMvp.Base;
using System.Collections.Generic;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Util;

namespace UniScanG.UI.Teach.Inspector
{
    public interface IModellerControl
    {
        void SetModellerExtender(ModellerPageExtender modellerPageExtender);
    }

    public partial class TeachPage : UserControl, IMainTabPage, IMultiLanguageSupport
    {
        IModellerControl teachToolbar;
        IModellerControl imageController;
        IModellerControl paramController;

        ModellerPageExtender modellerPageExtender;

        public TeachPage()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            InspectorUiChanger inspectorUiChanger = (InspectorUiChanger)SystemManager.Instance().UiChanger;

            modellerPageExtender = SystemManager.Instance().ModellerPageExtender as ModellerPageExtender;

            imageController = inspectorUiChanger.CreateImageController();
            paramController = inspectorUiChanger.CreateParamController();
            teachToolbar = inspectorUiChanger.CreateTeachToolBar();

            teachToolbar.SetModellerExtender(modellerPageExtender);
            imageController.SetModellerExtender(modellerPageExtender);
            paramController.SetModellerExtender(modellerPageExtender);

            imagePanel.Controls.Add((Control)imageController);
            paramPanel.Controls.Add((Control)paramController);
            toolbarPanel.Controls.Add((Control)teachToolbar);
        }

        public void EnableControls(UserType user)
        {
            throw new System.NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
        }

        public void UpdateControl(string item, object value)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
