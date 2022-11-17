﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Data.UI;
using DynMvp.InspData;

namespace DynMvp.Data.UI
{
    public partial class TargetGridView : UserControl, InspectionResultVisualizer
    {
        List<TargetView> targetViews = new List<TargetView>();

        public TargetGridView()
        {
            InitializeComponent();
        }

        public void Update(Model model)
        {
            LogHelper.Debug(LoggerType.Operation, "UpdateTargetGrid");

            targetViewPanel.Controls.Clear();
            targetViews = new List<TargetView>();

            if (model == null)
                return;

            List<Target> targetList = new List<Target>();
            model.GetTargets(targetList);
            if (model.MasterModel != null)
                model.MasterModel.GetTargets(targetList);

            int numTarget = targetList.Count;

            int numCount = (int)Math.Ceiling(Math.Sqrt(numTarget));

            targetViewPanel.RowCount = targetViewPanel.ColumnCount = numCount;
            targetViewPanel.ColumnStyles.Clear();
            targetViewPanel.RowStyles.Clear();
            for (int i = 0; i < numCount; i++)
            {
                targetViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 / numCount));
                targetViewPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100 / numCount));
            }

            foreach (Target target in targetList)
            {
                int targetIndex = targetViews.Count;
                int rowIndex = targetIndex / numCount;
                int colIndex = targetIndex % numCount;

                TargetView targetView = CreateTargetView(targetList[targetIndex]);
                targetView.TargetIndex = targetIndex;

                targetViews.Add(targetView);

                targetViewPanel.Controls.Add(targetViews[targetIndex], colIndex, rowIndex);
            }
        }

        private TargetView CreateTargetView(Target target)
        {
            TargetView targetView = new TargetView();

            targetView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            targetView.Dock = System.Windows.Forms.DockStyle.Fill;
            targetView.Location = new System.Drawing.Point(3, 3);
            targetView.Name = "targetView";
            targetView.Size = new System.Drawing.Size(409, 523);
            targetView.TabStop = false;
            targetView.Enable = false;
            targetView.SetTarget(target);
            targetView.DoubleClick += new System.EventHandler(this.targetView_DoubleClick);

            return targetView;
        }

        private void targetView_DoubleClick(object sender, EventArgs e)
        {
            int numTarget = targetViews.Count;
            int numCount = (int)Math.Ceiling(Math.Sqrt(numTarget));

            TargetView targetView = (TargetView)sender;

            if (targetView.Parent == this)
            {
                int targetIndex = (int)targetView.TargetIndex;

                int rowIndex = targetIndex / numCount;
                int colIndex = targetIndex % numCount;

                targetViewPanel.Controls.Add(targetView, colIndex, rowIndex);
                targetViewPanel.Show();
            }
            else
            {
                targetView.Parent = this;
                targetViewPanel.Hide();
            }
        }

        public void ResetResult()
        {
            foreach (TargetView targetView in targetViews)
            {
                targetView.Invalidate();
            }
        }

        public void UpdateResult(Target target, InspectionResult targetInspectionResult)
        {
            foreach (TargetView targetView in targetViews)
            {
                if (targetView.LinkedTarget.FullId == target.FullId)
                {
                    targetView.UpdteTargetView(targetInspectionResult);
                }
            }
        }

        public void UpdateResult(TargetGroup targetGroup, InspectionResult targetInspectionResult)
        {

        }
    }
}
