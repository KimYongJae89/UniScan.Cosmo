using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using DynMvp.Base;
using UniEye.Base.UI;
using UniScanG.Data;
using UniScanG.Data.Model;
using System.Reflection;
using DynMvp.Authentication;

namespace UniScanG.UI.Report
{
    public partial class ReportPage : UserControl, IMainTabPage, IMultiLanguageSupport
    {
        bool onUpdateData = false;
        IReportPanel reportPanel;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public ReportPage()
        {
            onUpdateData = true;

            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            this.TabIndex = 0;
            this.Dock = DockStyle.Fill;

            reportPanel = SystemManager.Instance().UiChanger.CreateReportPanel();
            reportContainer.Controls.Add((Control)reportPanel);

            startDate.CustomFormat = "yyyy-MM-dd";
            endDate.CustomFormat = "yyyy-MM-dd";

            onUpdateData = false;
        }

        private List<ModelDescription> FindModel(DateTime start, DateTime end, string search)
        {
            List<ModelDescription> modelDescriptionList = new List<ModelDescription>();

            foreach (DynMvp.Data.ProductionBase production in SystemManager.Instance().ProductionManager.List)
            {
                Production productionG = (Production)production;

                if (productionG.StartTime < startDate.Value.Date || productionG.StartTime > endDate.Value.Date.AddDays(1))
                    continue;

                if (productionG.Name.Contains(findModelName.Text) == false)
                    continue;

                UniScanG.Data.Model.ModelDescription modelDescription = new UniScanG.Data.Model.ModelDescription();
                modelDescription.Name = productionG.Name;
                modelDescription.Paste = productionG.Paste;
                modelDescription.Thickness = Convert.ToSingle(productionG.Thickness);

                if (modelDescriptionList.Exists(f => f.Name == modelDescription.Name))
                    continue;

                modelDescriptionList.Add(modelDescription);
            }
            return modelDescriptionList;
        }

        private void UpdateModelList()
        {
            if (onUpdateData == true)
                return;

            onUpdateData = true;

            ModelDescription selectedMd = null;
            if (this.modelList.SelectedRows.Count > 0)
                selectedMd = this.modelList.SelectedRows[0].Tag as ModelDescription;

            modelList.Rows.Clear();
            modelList.Rows.Add(0, "__ALL__", 0, "");

            int selectIndex = 0;
            List<ModelDescription> modelDescriptionList = FindModel(startDate.Value.Date, endDate.Value.Date.AddDays(1), findModelName.Text);
            for (int i=0; i< modelDescriptionList.Count; i++)
            {
                UniScanG.Data.Model.ModelDescription md = modelDescriptionList[i];
                int rowIndex = modelList.Rows.Add(i + 1, md.Name, md.Thickness, md.Paste.ToString());
                modelList.Rows[rowIndex].Tag = md;

                if (md.Equals(selectedMd))
                    selectIndex = i + 1;
            }

            modelList.Sort(modelList.Columns[0], System.ComponentModel.ListSortDirection.Ascending);

            totalModel.Text = modelDescriptionList.Count.ToString();
            modelList.ClearSelection();

            onUpdateData = false;

            modelList.Rows[selectIndex].Selected = true;
        }

        private List<Production> FindLot(ModelDescription modelDescription, DateTime start, DateTime end, string search)
        {
            List<DynMvp.Data.ProductionBase> modelProductionList;
            if (modelDescription == null)
                modelProductionList = SystemManager.Instance().ProductionManager.List;
            else
                modelProductionList = SystemManager.Instance().ProductionManager.List.FindAll(f => modelDescription.Equals(f.GetModelDescription()));

            List<Production> productionList = new List<Production>();
            foreach (Production production in modelProductionList)
            {
                if (production.StartTime < startDate.Value.Date || production.StartTime > endDate.Value.Date.AddDays(1))
                    continue;

                if (production.LotNo.Contains(search) == false)
                    continue;
                
                productionList.Add(production);
            }
            return productionList;
        }

        public void UpdateLotList()
        {
            if (onUpdateData == true)
                return;

            onUpdateData = true;

            lotList.Rows.Clear();

            ModelDescription selectedMd = null;
            if (this.modelList.SelectedRows.Count > 0)
            {
                selectedMd = this.modelList.SelectedRows[0].Tag as ModelDescription;
            }

            List<Production> productionList = FindLot(selectedMd, startDate.Value.Date, endDate.Value.Date.AddDays(1), findLotName.Text);
            foreach (Production p in productionList)
            {
                if (p.Total > 0)
                {
                    int rowIndex = lotList.Rows.Add(p.StartTime, p.Name, p.LotNo, p.Total);
                    lotList.Rows[rowIndex].Tag = p;
                }
            }

            lotList.Sort(lotList.Columns[0], System.ComponentModel.ListSortDirection.Descending);

            totalLot.Text = productionList.Count.ToString();

            lotList.ClearSelection();
            onUpdateData = false;
        }
        
        private void startDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateModelList();
        }

        private void endDate_ValueChanged(object sender, EventArgs e)
        {
            UpdateModelList();
        }
        private void findModelName_TextChanged(object sender, EventArgs e)
        {
            UpdateModelList();
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            if (lotList.SelectedRows.Count < 1)
                return;

            Production production = (Production)lotList.SelectedRows[0].Tag;
            reportPanel.Search(production);
        }

        private void findLotName_TextChanged(object sender, EventArgs e)
        {
            UpdateLotList();
        }

        public void UpdateControl(string item, object value) { }
        public void PageVisibleChanged(bool visibleFlag) { }
        public void EnableControls(UserType userType) { }

        public void UpdateLanguage()
        {
            onUpdateData = true;

            StringManager.UpdateString(this);

            onUpdateData= false;
        }

        private void SelectModelList()
        {
            if (onUpdateData == true)
                return;

            if (modelList.SelectedRows.Count == 0)
                return;

            Data.Model.ModelDescription md = (Data.Model.ModelDescription)modelList.SelectedRows[0].Tag;
            UpdateLotList();
            //findModelName.Text = md.Name;
        }

        private void modelList_SelectionChanged(object sender, EventArgs e)
        {
            UpdateLotList();
        }

        private void modelList_Click(object sender, EventArgs e)
        {
            UpdateLotList();
        }

        private void ReportPage_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
                UpdateModelList();
        }
    }
}
