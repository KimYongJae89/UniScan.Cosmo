//using System;

//namespace UniScanG.Temp
//{
//    partial class MonitoringPageV2
//    {
//        /// <summary> 
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Component Designer generated code

//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
//            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
//            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
//            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
//            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
//            this.StateUpdateTimer = new System.Windows.Forms.Timer(this.components);
//            this.controlPanel = new Infragistics.Win.Misc.UltraPanel();
//            this.panelStatusPanel = new System.Windows.Forms.Panel();
//            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
//            this.tableLayoutPanelProductCount = new System.Windows.Forms.TableLayoutPanel();
//            this.label3 = new System.Windows.Forms.Label();
//            this.label2 = new System.Windows.Forms.Label();
//            this.label1 = new System.Windows.Forms.Label();
//            this.labelProductGood = new System.Windows.Forms.Label();
//            this.labelProductNG = new System.Windows.Forms.Label();
//            this.labelProductRate = new System.Windows.Forms.Label();
//            this.labelLotNo = new System.Windows.Forms.Label();
//            this.labelModelName = new System.Windows.Forms.Label();
//            this.labelStatus = new System.Windows.Forms.Label();
//            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
//            this.buttonStart = new Infragistics.Win.Misc.UltraButton();
//            this.buttonStop = new Infragistics.Win.Misc.UltraButton();
//            this.buttonResetCount = new Infragistics.Win.Misc.UltraButton();
//            this.buttonLotChange = new Infragistics.Win.Misc.UltraButton();
//            this.appStylistRuntime1 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
//            this.appStylistRuntime2 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
//            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
//            this.cameraPanel = new System.Windows.Forms.Panel();
//            this.controlPanel.ClientArea.SuspendLayout();
//            this.controlPanel.SuspendLayout();
//            this.panelStatusPanel.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
//            this.tableLayoutPanelProductCount.SuspendLayout();
//            this.buttonPanel.SuspendLayout();
//            this.tableLayoutPanel1.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // controlPanel
//            // 
//            // 
//            // controlPanel.ClientArea
//            // 
//            this.controlPanel.ClientArea.Controls.Add(this.panelStatusPanel);
//            this.controlPanel.ClientArea.Controls.Add(this.buttonPanel);
//            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.controlPanel.Location = new System.Drawing.Point(794, 3);
//            this.controlPanel.Name = "controlPanel";
//            this.controlPanel.Size = new System.Drawing.Size(645, 822);
//            this.controlPanel.TabIndex = 55;
//            // 
//            // panelStatusPanel
//            // 
//            this.panelStatusPanel.Controls.Add(this.chart1);
//            this.panelStatusPanel.Controls.Add(this.tableLayoutPanelProductCount);
//            this.panelStatusPanel.Controls.Add(this.labelLotNo);
//            this.panelStatusPanel.Controls.Add(this.labelModelName);
//            this.panelStatusPanel.Controls.Add(this.labelStatus);
//            this.panelStatusPanel.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.panelStatusPanel.Location = new System.Drawing.Point(0, 0);
//            this.panelStatusPanel.Name = "panelStatusPanel";
//            this.panelStatusPanel.Size = new System.Drawing.Size(537, 822);
//            this.panelStatusPanel.TabIndex = 30;
//            // 
//            // chart1
//            // 
//            this.chart1.BackColor = System.Drawing.SystemColors.Control;
//            chartArea1.BackColor = System.Drawing.SystemColors.Control;
//            chartArea1.Name = "ChartArea1";
//            this.chart1.ChartAreas.Add(chartArea1);
//            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
//            legend1.BackColor = System.Drawing.SystemColors.Control;
//            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
//            legend1.IsTextAutoFit = false;
//            legend1.Name = "Legend1";
//            legend1.TableStyle = System.Windows.Forms.DataVisualization.Charting.LegendTableStyle.Wide;
//            this.chart1.Legends.Add(legend1);
//            this.chart1.Location = new System.Drawing.Point(0, 411);
//            this.chart1.Name = "chart1";
//            series1.BorderWidth = 5;
//            series1.ChartArea = "ChartArea1";
//            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
//            series1.Color = System.Drawing.Color.Red;
//            series1.Legend = "Legend1";
//            series1.LegendText = "Total";
//            series1.MarkerBorderWidth = 3;
//            series1.Name = "totalDefect";
//            series2.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dot;
//            series2.BorderWidth = 3;
//            series2.ChartArea = "ChartArea1";
//            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
//            series2.Color = System.Drawing.Color.Black;
//            series2.Legend = "Legend1";
//            series2.LegendText = "White";
//            series2.Name = "whiteDefect";
//            series3.BorderWidth = 3;
//            series3.ChartArea = "ChartArea1";
//            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
//            series3.Color = System.Drawing.Color.Black;
//            series3.Legend = "Legend1";
//            series3.LegendText = "Black";
//            series3.Name = "blackDefect";
//            this.chart1.Series.Add(series1);
//            this.chart1.Series.Add(series2);
//            this.chart1.Series.Add(series3);
//            this.chart1.Size = new System.Drawing.Size(537, 411);
//            this.chart1.TabIndex = 24;
//            this.chart1.Text = "chart1";
//            // 
//            // tableLayoutPanelProductCount
//            // 
//            this.tableLayoutPanelProductCount.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
//            this.tableLayoutPanelProductCount.ColumnCount = 3;
//            this.tableLayoutPanelProductCount.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
//            this.tableLayoutPanelProductCount.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
//            this.tableLayoutPanelProductCount.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
//            this.tableLayoutPanelProductCount.Controls.Add(this.label3, 2, 0);
//            this.tableLayoutPanelProductCount.Controls.Add(this.label2, 1, 0);
//            this.tableLayoutPanelProductCount.Controls.Add(this.label1, 0, 0);
//            this.tableLayoutPanelProductCount.Controls.Add(this.labelProductGood, 0, 1);
//            this.tableLayoutPanelProductCount.Controls.Add(this.labelProductNG, 1, 1);
//            this.tableLayoutPanelProductCount.Controls.Add(this.labelProductRate, 2, 1);
//            this.tableLayoutPanelProductCount.Dock = System.Windows.Forms.DockStyle.Top;
//            this.tableLayoutPanelProductCount.Location = new System.Drawing.Point(0, 290);
//            this.tableLayoutPanelProductCount.Name = "tableLayoutPanelProductCount";
//            this.tableLayoutPanelProductCount.RowCount = 2;
//            this.tableLayoutPanelProductCount.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
//            this.tableLayoutPanelProductCount.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 72.72727F));
//            this.tableLayoutPanelProductCount.Size = new System.Drawing.Size(537, 121);
//            this.tableLayoutPanelProductCount.TabIndex = 19;
//            // 
//            // label3
//            // 
//            this.label3.AutoSize = true;
//            this.label3.BackColor = System.Drawing.Color.Pink;
//            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.label3.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.label3.Location = new System.Drawing.Point(360, 1);
//            this.label3.Name = "label3";
//            this.label3.Size = new System.Drawing.Size(173, 32);
//            this.label3.TabIndex = 5;
//            this.label3.Text = "Rate";
//            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // label2
//            // 
//            this.label2.AutoSize = true;
//            this.label2.BackColor = System.Drawing.Color.Crimson;
//            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.label2.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.label2.Location = new System.Drawing.Point(182, 1);
//            this.label2.Name = "label2";
//            this.label2.Size = new System.Drawing.Size(171, 32);
//            this.label2.TabIndex = 4;
//            this.label2.Text = "NG";
//            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // label1
//            // 
//            this.label1.AutoSize = true;
//            this.label1.BackColor = System.Drawing.Color.LawnGreen;
//            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.label1.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.label1.Location = new System.Drawing.Point(4, 1);
//            this.label1.Name = "label1";
//            this.label1.Size = new System.Drawing.Size(171, 32);
//            this.label1.TabIndex = 3;
//            this.label1.Text = "Good";
//            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // labelProductGood
//            // 
//            this.labelProductGood.AutoSize = true;
//            this.labelProductGood.BackColor = System.Drawing.Color.LawnGreen;
//            this.labelProductGood.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.labelProductGood.Font = new System.Drawing.Font("맑은 고딕", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.labelProductGood.Location = new System.Drawing.Point(4, 34);
//            this.labelProductGood.Name = "labelProductGood";
//            this.labelProductGood.Size = new System.Drawing.Size(171, 86);
//            this.labelProductGood.TabIndex = 0;
//            this.labelProductGood.Text = "00000";
//            this.labelProductGood.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // labelProductNG
//            // 
//            this.labelProductNG.AutoSize = true;
//            this.labelProductNG.BackColor = System.Drawing.Color.Crimson;
//            this.labelProductNG.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.labelProductNG.Font = new System.Drawing.Font("맑은 고딕", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.labelProductNG.Location = new System.Drawing.Point(182, 34);
//            this.labelProductNG.Name = "labelProductNG";
//            this.labelProductNG.Size = new System.Drawing.Size(171, 86);
//            this.labelProductNG.TabIndex = 1;
//            this.labelProductNG.Text = "00000";
//            this.labelProductNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // labelProductRate
//            // 
//            this.labelProductRate.AutoSize = true;
//            this.labelProductRate.BackColor = System.Drawing.Color.Pink;
//            this.labelProductRate.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.labelProductRate.Font = new System.Drawing.Font("맑은 고딕", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.labelProductRate.Location = new System.Drawing.Point(360, 34);
//            this.labelProductRate.Name = "labelProductRate";
//            this.labelProductRate.Size = new System.Drawing.Size(173, 86);
//            this.labelProductRate.TabIndex = 2;
//            this.labelProductRate.Text = "000%";
//            this.labelProductRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // labelLotNo
//            // 
//            this.labelLotNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.labelLotNo.Dock = System.Windows.Forms.DockStyle.Top;
//            this.labelLotNo.Font = new System.Drawing.Font("맑은 고딕", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.labelLotNo.Location = new System.Drawing.Point(0, 210);
//            this.labelLotNo.Name = "labelLotNo";
//            this.labelLotNo.Size = new System.Drawing.Size(537, 80);
//            this.labelLotNo.TabIndex = 22;
//            this.labelLotNo.Text = "Lot";
//            this.labelLotNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // labelModelName
//            // 
//            this.labelModelName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.labelModelName.Dock = System.Windows.Forms.DockStyle.Top;
//            this.labelModelName.Font = new System.Drawing.Font("맑은 고딕", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.labelModelName.Location = new System.Drawing.Point(0, 120);
//            this.labelModelName.Name = "labelModelName";
//            this.labelModelName.Size = new System.Drawing.Size(537, 90);
//            this.labelModelName.TabIndex = 21;
//            this.labelModelName.Text = "Model Name";
//            this.labelModelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // labelStatus
//            // 
//            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Top;
//            this.labelStatus.Font = new System.Drawing.Font("맑은 고딕", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.labelStatus.Location = new System.Drawing.Point(0, 0);
//            this.labelStatus.Name = "labelStatus";
//            this.labelStatus.Size = new System.Drawing.Size(537, 120);
//            this.labelStatus.TabIndex = 23;
//            this.labelStatus.Text = "Idle";
//            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // buttonPanel
//            // 
//            this.buttonPanel.AutoSize = true;
//            this.buttonPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.buttonPanel.Controls.Add(this.buttonStart);
//            this.buttonPanel.Controls.Add(this.buttonStop);
//            this.buttonPanel.Controls.Add(this.buttonResetCount);
//            this.buttonPanel.Controls.Add(this.buttonLotChange);
//            this.buttonPanel.Cursor = System.Windows.Forms.Cursors.Default;
//            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Right;
//            this.buttonPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
//            this.buttonPanel.Location = new System.Drawing.Point(537, 0);
//            this.buttonPanel.Name = "buttonPanel";
//            this.buttonPanel.Size = new System.Drawing.Size(108, 822);
//            this.buttonPanel.TabIndex = 29;
//            // 
//            // buttonStart
//            // 
//            appearance1.BackColor = System.Drawing.Color.Black;
//            appearance1.FontData.BoldAsString = "True";
//            appearance1.FontData.Name = "Malgun Gothic";
//            appearance1.FontData.SizeInPoints = 16F;
//            appearance1.Image = global::UniScanG.Properties.Resources.Start;
//            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
//            appearance1.ImageVAlign = Infragistics.Win.VAlign.Top;
//            appearance1.TextVAlignAsString = "Bottom";
//            this.buttonStart.Appearance = appearance1;
//            this.buttonStart.Font = new System.Drawing.Font("맑은 고딕", 12.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.buttonStart.ImageSize = new System.Drawing.Size(60, 60);
//            this.buttonStart.Location = new System.Drawing.Point(3, 3);
//            this.buttonStart.Name = "buttonStart";
//            this.buttonStart.Size = new System.Drawing.Size(100, 100);
//            this.buttonStart.TabIndex = 27;
//            this.buttonStart.Text = "Start";
//            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
//            // 
//            // buttonStop
//            // 
//            appearance2.BackColor = System.Drawing.Color.Transparent;
//            appearance2.FontData.BoldAsString = "True";
//            appearance2.FontData.Name = "Malgun Gothic";
//            appearance2.FontData.SizeInPoints = 16F;
//            appearance2.Image = global::UniScanG.Properties.Resources.Stop;
//            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
//            appearance2.ImageVAlign = Infragistics.Win.VAlign.Top;
//            appearance2.TextVAlignAsString = "Bottom";
//            this.buttonStop.Appearance = appearance2;
//            this.buttonStop.ImageSize = new System.Drawing.Size(60, 60);
//            this.buttonStop.Location = new System.Drawing.Point(3, 109);
//            this.buttonStop.Name = "buttonStop";
//            this.buttonStop.Size = new System.Drawing.Size(100, 100);
//            this.buttonStop.TabIndex = 26;
//            this.buttonStop.Text = "Stop";
//            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
//            // 
//            // buttonResetCount
//            // 
//            appearance3.BackColor = System.Drawing.Color.Transparent;
//            appearance3.FontData.BoldAsString = "True";
//            appearance3.FontData.Name = "Malgun Gothic";
//            appearance3.FontData.SizeInPoints = 16F;
//            appearance3.Image = global::UniScanG.Properties.Resources.Reset;
//            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
//            appearance3.ImageVAlign = Infragistics.Win.VAlign.Top;
//            appearance3.TextVAlignAsString = "Bottom";
//            this.buttonResetCount.Appearance = appearance3;
//            this.buttonResetCount.ImageSize = new System.Drawing.Size(50, 50);
//            this.buttonResetCount.Location = new System.Drawing.Point(3, 215);
//            this.buttonResetCount.Name = "buttonResetCount";
//            this.buttonResetCount.Size = new System.Drawing.Size(100, 100);
//            this.buttonResetCount.TabIndex = 28;
//            this.buttonResetCount.Text = "Reset";
//            this.buttonResetCount.Click += new System.EventHandler(this.buttonResetCount_Click);
//            // 
//            // buttonLotChange
//            // 
//            appearance4.BackColor = System.Drawing.Color.Transparent;
//            appearance4.FontData.BoldAsString = "True";
//            appearance4.FontData.Name = "Malgun Gothic";
//            appearance4.FontData.SizeInPoints = 16F;
//            appearance4.Image = global::UniScanG.Properties.Resources.LotOver641;
//            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
//            appearance4.ImageVAlign = Infragistics.Win.VAlign.Top;
//            appearance4.TextVAlignAsString = "Bottom";
//            this.buttonLotChange.Appearance = appearance4;
//            this.buttonLotChange.ImageSize = new System.Drawing.Size(60, 60);
//            this.buttonLotChange.Location = new System.Drawing.Point(3, 321);
//            this.buttonLotChange.Name = "buttonLotChange";
//            this.buttonLotChange.Size = new System.Drawing.Size(100, 123);
//            this.buttonLotChange.TabIndex = 26;
//            this.buttonLotChange.Text = "Lot Change";
//            // 
//            // tableLayoutPanel1
//            // 
//            this.tableLayoutPanel1.ColumnCount = 2;
//            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.85437F));
//            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.14563F));
//            this.tableLayoutPanel1.Controls.Add(this.controlPanel, 1, 0);
//            this.tableLayoutPanel1.Controls.Add(this.cameraPanel, 0, 0);
//            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
//            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
//            this.tableLayoutPanel1.RowCount = 1;
//            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
//            this.tableLayoutPanel1.Size = new System.Drawing.Size(1442, 828);
//            this.tableLayoutPanel1.TabIndex = 56;
//            // 
//            // cameraPanel
//            // 
//            this.cameraPanel.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.cameraPanel.Location = new System.Drawing.Point(3, 3);
//            this.cameraPanel.Name = "cameraPanel";
//            this.cameraPanel.Size = new System.Drawing.Size(785, 822);
//            this.cameraPanel.TabIndex = 56;
//            // 
//            // MonitoringPageV2
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.tableLayoutPanel1);
//            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
//            this.Name = "MonitoringPageV2";
//            this.Padding = new System.Windows.Forms.Padding(3);
//            this.Size = new System.Drawing.Size(1448, 834);
//            this.controlPanel.ClientArea.ResumeLayout(false);
//            this.controlPanel.ClientArea.PerformLayout();
//            this.controlPanel.ResumeLayout(false);
//            this.panelStatusPanel.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
//            this.tableLayoutPanelProductCount.ResumeLayout(false);
//            this.tableLayoutPanelProductCount.PerformLayout();
//            this.buttonPanel.ResumeLayout(false);
//            this.tableLayoutPanel1.ResumeLayout(false);
//            this.ResumeLayout(false);

//        }

//        #endregion
//        private System.Windows.Forms.Timer StateUpdateTimer;
//        private Infragistics.Win.Misc.UltraPanel controlPanel;
//        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime1;
//        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime2;
//        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
//        private Infragistics.Win.Misc.UltraButton buttonStart;
//        private Infragistics.Win.Misc.UltraButton buttonStop;
//        private Infragistics.Win.Misc.UltraButton buttonLotChange;
//        private Infragistics.Win.Misc.UltraButton buttonResetCount;
//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProductCount;
//        private System.Windows.Forms.Label labelProductGood;
//        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
//        private System.Windows.Forms.Panel panelStatusPanel;
//        private System.Windows.Forms.Label label3;
//        private System.Windows.Forms.Label label2;
//        private System.Windows.Forms.Label label1;
//        private System.Windows.Forms.Label labelProductNG;
//        private System.Windows.Forms.Label labelProductRate;
//        private System.Windows.Forms.Label labelLotNo;
//        private System.Windows.Forms.Label labelModelName;
//        private System.Windows.Forms.Label labelStatus;
//        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
//        private System.Windows.Forms.Panel cameraPanel;
//    }
//}
