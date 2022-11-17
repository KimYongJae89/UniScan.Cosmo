namespace UniScanG.Gravure.UI.Setting
{
    partial class SettingAlarmPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.useContinuedDefectAlarm = new System.Windows.Forms.CheckBox();
            this.useRepeatedDefectAlarm = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelReapetedNc = new System.Windows.Forms.Label();
            this.labelReapetedRc = new System.Windows.Forms.Label();
            this.continuedN = new System.Windows.Forms.NumericUpDown();
            this.continuedR = new System.Windows.Forms.NumericUpDown();
            this.labelReapetedNcUnit = new System.Windows.Forms.Label();
            this.labelReapetedRcUnit = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelReapetedRUnit = new System.Windows.Forms.Label();
            this.labelReapetedNUnit = new System.Windows.Forms.Label();
            this.labelReapetedN = new System.Windows.Forms.Label();
            this.labelReapetedR = new System.Windows.Forms.Label();
            this.repeatedN = new System.Windows.Forms.NumericUpDown();
            this.repeatedR = new System.Windows.Forms.NumericUpDown();
            this.useNormalDefectAlarm = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.labelNormalRUnit = new System.Windows.Forms.Label();
            this.labelNormalNUnit = new System.Windows.Forms.Label();
            this.labelNormalN = new System.Windows.Forms.Label();
            this.labelNormalR = new System.Windows.Forms.Label();
            this.normalN = new System.Windows.Forms.NumericUpDown();
            this.normalR = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.labelNormalDefects = new System.Windows.Forms.Label();
            this.labelRepeatedDefects = new System.Windows.Forms.Label();
            this.labelSheetLength = new System.Windows.Forms.Label();
            this.useSheetLengthAlarm = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.labelSheetLengthN = new System.Windows.Forms.Label();
            this.labelSheetLengthD = new System.Windows.Forms.Label();
            this.sheetLengthN = new System.Windows.Forms.NumericUpDown();
            this.sheetLengthD = new System.Windows.Forms.NumericUpDown();
            this.labelSheetLengthNUnit = new System.Windows.Forms.Label();
            this.labelSheetLengthDUnit = new System.Windows.Forms.Label();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.continuedN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.continuedR)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repeatedN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repeatedR)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.normalR)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetLengthN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetLengthD)).BeginInit();
            this.SuspendLayout();
            // 
            // useContinuedDefectAlarm
            // 
            this.useContinuedDefectAlarm.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.useContinuedDefectAlarm, 2);
            this.useContinuedDefectAlarm.Location = new System.Drawing.Point(44, 282);
            this.useContinuedDefectAlarm.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.useContinuedDefectAlarm.Name = "useContinuedDefectAlarm";
            this.useContinuedDefectAlarm.Size = new System.Drawing.Size(368, 25);
            this.useContinuedDefectAlarm.TabIndex = 3;
            this.useContinuedDefectAlarm.Text = "Over than [Rc]% in continued last [Nc] sheets";
            this.useContinuedDefectAlarm.UseVisualStyleBackColor = true;
            this.useContinuedDefectAlarm.CheckedChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // useRepeatedDefectAlarm
            // 
            this.useRepeatedDefectAlarm.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.useRepeatedDefectAlarm, 2);
            this.useRepeatedDefectAlarm.Location = new System.Drawing.Point(44, 175);
            this.useRepeatedDefectAlarm.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.useRepeatedDefectAlarm.Name = "useRepeatedDefectAlarm";
            this.useRepeatedDefectAlarm.Size = new System.Drawing.Size(272, 25);
            this.useRepeatedDefectAlarm.TabIndex = 3;
            this.useRepeatedDefectAlarm.Text = "Over than [R]% in last [N] sheets";
            this.useRepeatedDefectAlarm.UseVisualStyleBackColor = true;
            this.useRepeatedDefectAlarm.CheckedChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.Controls.Add(this.labelReapetedNc, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelReapetedRc, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.continuedN, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.continuedR, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelReapetedNcUnit, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelReapetedRcUnit, 2, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(174, 319);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(140, 58);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // labelReapetedNc
            // 
            this.labelReapetedNc.AutoSize = true;
            this.labelReapetedNc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReapetedNc.Location = new System.Drawing.Point(0, 0);
            this.labelReapetedNc.Margin = new System.Windows.Forms.Padding(0);
            this.labelReapetedNc.Name = "labelReapetedNc";
            this.labelReapetedNc.Size = new System.Drawing.Size(40, 29);
            this.labelReapetedNc.TabIndex = 2;
            this.labelReapetedNc.Text = "Nc";
            this.labelReapetedNc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReapetedRc
            // 
            this.labelReapetedRc.AutoSize = true;
            this.labelReapetedRc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReapetedRc.Location = new System.Drawing.Point(0, 29);
            this.labelReapetedRc.Margin = new System.Windows.Forms.Padding(0);
            this.labelReapetedRc.Name = "labelReapetedRc";
            this.labelReapetedRc.Size = new System.Drawing.Size(40, 29);
            this.labelReapetedRc.TabIndex = 2;
            this.labelReapetedRc.Text = "Rc";
            this.labelReapetedRc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // continuedN
            // 
            this.continuedN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.continuedN.Location = new System.Drawing.Point(40, 0);
            this.continuedN.Margin = new System.Windows.Forms.Padding(0);
            this.continuedN.Name = "continuedN";
            this.continuedN.Size = new System.Drawing.Size(60, 29);
            this.continuedN.TabIndex = 3;
            this.continuedN.ValueChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // continuedR
            // 
            this.continuedR.DecimalPlaces = 1;
            this.continuedR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.continuedR.Location = new System.Drawing.Point(40, 29);
            this.continuedR.Margin = new System.Windows.Forms.Padding(0);
            this.continuedR.Name = "continuedR";
            this.continuedR.Size = new System.Drawing.Size(60, 29);
            this.continuedR.TabIndex = 3;
            this.continuedR.ValueChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // labelReapetedNcUnit
            // 
            this.labelReapetedNcUnit.AutoSize = true;
            this.labelReapetedNcUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReapetedNcUnit.Location = new System.Drawing.Point(100, 0);
            this.labelReapetedNcUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelReapetedNcUnit.Name = "labelReapetedNcUnit";
            this.labelReapetedNcUnit.Size = new System.Drawing.Size(40, 29);
            this.labelReapetedNcUnit.TabIndex = 2;
            this.labelReapetedNcUnit.Text = "EA";
            this.labelReapetedNcUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReapetedRcUnit
            // 
            this.labelReapetedRcUnit.AutoSize = true;
            this.labelReapetedRcUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReapetedRcUnit.Location = new System.Drawing.Point(100, 29);
            this.labelReapetedRcUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelReapetedRcUnit.Name = "labelReapetedRcUnit";
            this.labelReapetedRcUnit.Size = new System.Drawing.Size(40, 29);
            this.labelReapetedRcUnit.TabIndex = 2;
            this.labelReapetedRcUnit.Text = "%";
            this.labelReapetedRcUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Controls.Add(this.labelReapetedRUnit, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelReapetedNUnit, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelReapetedN, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelReapetedR, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.repeatedN, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.repeatedR, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(174, 212);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(140, 58);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelReapetedRUnit
            // 
            this.labelReapetedRUnit.AutoSize = true;
            this.labelReapetedRUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReapetedRUnit.Location = new System.Drawing.Point(100, 29);
            this.labelReapetedRUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelReapetedRUnit.Name = "labelReapetedRUnit";
            this.labelReapetedRUnit.Size = new System.Drawing.Size(40, 29);
            this.labelReapetedRUnit.TabIndex = 5;
            this.labelReapetedRUnit.Text = "%";
            this.labelReapetedRUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReapetedNUnit
            // 
            this.labelReapetedNUnit.AutoSize = true;
            this.labelReapetedNUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReapetedNUnit.Location = new System.Drawing.Point(100, 0);
            this.labelReapetedNUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelReapetedNUnit.Name = "labelReapetedNUnit";
            this.labelReapetedNUnit.Size = new System.Drawing.Size(40, 29);
            this.labelReapetedNUnit.TabIndex = 4;
            this.labelReapetedNUnit.Text = "EA";
            this.labelReapetedNUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReapetedN
            // 
            this.labelReapetedN.AutoSize = true;
            this.labelReapetedN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReapetedN.Location = new System.Drawing.Point(0, 0);
            this.labelReapetedN.Margin = new System.Windows.Forms.Padding(0);
            this.labelReapetedN.Name = "labelReapetedN";
            this.labelReapetedN.Size = new System.Drawing.Size(40, 29);
            this.labelReapetedN.TabIndex = 2;
            this.labelReapetedN.Text = "N";
            this.labelReapetedN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReapetedR
            // 
            this.labelReapetedR.AutoSize = true;
            this.labelReapetedR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelReapetedR.Location = new System.Drawing.Point(0, 29);
            this.labelReapetedR.Margin = new System.Windows.Forms.Padding(0);
            this.labelReapetedR.Name = "labelReapetedR";
            this.labelReapetedR.Size = new System.Drawing.Size(40, 29);
            this.labelReapetedR.TabIndex = 2;
            this.labelReapetedR.Text = "R";
            this.labelReapetedR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // repeatedN
            // 
            this.repeatedN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.repeatedN.Location = new System.Drawing.Point(40, 0);
            this.repeatedN.Margin = new System.Windows.Forms.Padding(0);
            this.repeatedN.Name = "repeatedN";
            this.repeatedN.Size = new System.Drawing.Size(60, 29);
            this.repeatedN.TabIndex = 3;
            this.repeatedN.ValueChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // repeatedR
            // 
            this.repeatedR.DecimalPlaces = 1;
            this.repeatedR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.repeatedR.Location = new System.Drawing.Point(40, 29);
            this.repeatedR.Margin = new System.Windows.Forms.Padding(0);
            this.repeatedR.Name = "repeatedR";
            this.repeatedR.Size = new System.Drawing.Size(60, 29);
            this.repeatedR.TabIndex = 3;
            this.repeatedR.ValueChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // useNormalDefectAlarm
            // 
            this.useNormalDefectAlarm.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.useNormalDefectAlarm, 2);
            this.useNormalDefectAlarm.Location = new System.Drawing.Point(44, 27);
            this.useNormalDefectAlarm.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.useNormalDefectAlarm.Name = "useNormalDefectAlarm";
            this.useNormalDefectAlarm.Size = new System.Drawing.Size(282, 25);
            this.useNormalDefectAlarm.TabIndex = 3;
            this.useNormalDefectAlarm.Text = "Over than [R]% at least [N] sheets";
            this.useNormalDefectAlarm.UseVisualStyleBackColor = true;
            this.useNormalDefectAlarm.CheckedChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.Controls.Add(this.labelNormalRUnit, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.labelNormalNUnit, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelNormalN, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelNormalR, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.normalN, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.normalR, 1, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(174, 64);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(140, 58);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // labelNormalRUnit
            // 
            this.labelNormalRUnit.AutoSize = true;
            this.labelNormalRUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNormalRUnit.Location = new System.Drawing.Point(100, 29);
            this.labelNormalRUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelNormalRUnit.Name = "labelNormalRUnit";
            this.labelNormalRUnit.Size = new System.Drawing.Size(40, 29);
            this.labelNormalRUnit.TabIndex = 5;
            this.labelNormalRUnit.Text = "%";
            this.labelNormalRUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelNormalNUnit
            // 
            this.labelNormalNUnit.AutoSize = true;
            this.labelNormalNUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNormalNUnit.Location = new System.Drawing.Point(100, 0);
            this.labelNormalNUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelNormalNUnit.Name = "labelNormalNUnit";
            this.labelNormalNUnit.Size = new System.Drawing.Size(40, 29);
            this.labelNormalNUnit.TabIndex = 4;
            this.labelNormalNUnit.Text = "EA";
            this.labelNormalNUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelNormalN
            // 
            this.labelNormalN.AutoSize = true;
            this.labelNormalN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNormalN.Location = new System.Drawing.Point(0, 0);
            this.labelNormalN.Margin = new System.Windows.Forms.Padding(0);
            this.labelNormalN.Name = "labelNormalN";
            this.labelNormalN.Size = new System.Drawing.Size(40, 29);
            this.labelNormalN.TabIndex = 2;
            this.labelNormalN.Text = "N";
            this.labelNormalN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelNormalR
            // 
            this.labelNormalR.AutoSize = true;
            this.labelNormalR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNormalR.Location = new System.Drawing.Point(0, 29);
            this.labelNormalR.Margin = new System.Windows.Forms.Padding(0);
            this.labelNormalR.Name = "labelNormalR";
            this.labelNormalR.Size = new System.Drawing.Size(40, 29);
            this.labelNormalR.TabIndex = 2;
            this.labelNormalR.Text = "R";
            this.labelNormalR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // normalN
            // 
            this.normalN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.normalN.Location = new System.Drawing.Point(40, 0);
            this.normalN.Margin = new System.Windows.Forms.Padding(0);
            this.normalN.Name = "normalN";
            this.normalN.Size = new System.Drawing.Size(60, 29);
            this.normalN.TabIndex = 3;
            this.normalN.ValueChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // normalR
            // 
            this.normalR.DecimalPlaces = 1;
            this.normalR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.normalR.Location = new System.Drawing.Point(40, 29);
            this.normalR.Margin = new System.Windows.Forms.Padding(0);
            this.normalR.Name = "normalR";
            this.normalR.Size = new System.Drawing.Size(60, 29);
            this.normalR.TabIndex = 3;
            this.normalR.ValueChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel2, 2, 8);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 2, 6);
            this.tableLayoutPanel4.Controls.Add(this.useNormalDefectAlarm, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.labelNormalDefects, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.labelRepeatedDefects, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.labelSheetLength, 0, 10);
            this.tableLayoutPanel4.Controls.Add(this.useSheetLengthAlarm, 1, 12);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 2, 13);
            this.tableLayoutPanel4.Controls.Add(this.useContinuedDefectAlarm, 1, 7);
            this.tableLayoutPanel4.Controls.Add(this.useRepeatedDefectAlarm, 1, 5);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 15;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1003, 685);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // labelNormalDefects
            // 
            this.labelNormalDefects.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.labelNormalDefects, 2);
            this.labelNormalDefects.Location = new System.Drawing.Point(3, 0);
            this.labelNormalDefects.Name = "labelNormalDefects";
            this.labelNormalDefects.Size = new System.Drawing.Size(125, 21);
            this.labelNormalDefects.TabIndex = 4;
            this.labelNormalDefects.Text = "Normal Defects";
            // 
            // labelRepeatedDefects
            // 
            this.labelRepeatedDefects.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.labelRepeatedDefects, 2);
            this.labelRepeatedDefects.Location = new System.Drawing.Point(3, 148);
            this.labelRepeatedDefects.Name = "labelRepeatedDefects";
            this.labelRepeatedDefects.Size = new System.Drawing.Size(142, 21);
            this.labelRepeatedDefects.TabIndex = 4;
            this.labelRepeatedDefects.Text = "Repeated Defects";
            // 
            // labelSheetLength
            // 
            this.labelSheetLength.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.labelSheetLength, 2);
            this.labelSheetLength.Location = new System.Drawing.Point(3, 403);
            this.labelSheetLength.Name = "labelSheetLength";
            this.labelSheetLength.Size = new System.Drawing.Size(109, 21);
            this.labelSheetLength.TabIndex = 4;
            this.labelSheetLength.Text = "Sheet Length";
            // 
            // useSheetLengthAlarm
            // 
            this.useSheetLengthAlarm.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.useSheetLengthAlarm, 2);
            this.useSheetLengthAlarm.Location = new System.Drawing.Point(44, 430);
            this.useSheetLengthAlarm.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.useSheetLengthAlarm.Name = "useSheetLengthAlarm";
            this.useSheetLengthAlarm.Size = new System.Drawing.Size(298, 25);
            this.useSheetLengthAlarm.TabIndex = 3;
            this.useSheetLengthAlarm.Text = "Over than [D]mm at least [N] sheets";
            this.useSheetLengthAlarm.UseVisualStyleBackColor = true;
            this.useSheetLengthAlarm.CheckedChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel5.Controls.Add(this.labelSheetLengthN, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.labelSheetLengthD, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.sheetLengthN, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.sheetLengthD, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.labelSheetLengthNUnit, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.labelSheetLengthDUnit, 2, 1);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(174, 467);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(140, 58);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // labelSheetLengthN
            // 
            this.labelSheetLengthN.AutoSize = true;
            this.labelSheetLengthN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthN.Location = new System.Drawing.Point(0, 0);
            this.labelSheetLengthN.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthN.Name = "labelSheetLengthN";
            this.labelSheetLengthN.Size = new System.Drawing.Size(40, 29);
            this.labelSheetLengthN.TabIndex = 2;
            this.labelSheetLengthN.Text = "N";
            this.labelSheetLengthN.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetLengthD
            // 
            this.labelSheetLengthD.AutoSize = true;
            this.labelSheetLengthD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthD.Location = new System.Drawing.Point(0, 29);
            this.labelSheetLengthD.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthD.Name = "labelSheetLengthD";
            this.labelSheetLengthD.Size = new System.Drawing.Size(40, 29);
            this.labelSheetLengthD.TabIndex = 2;
            this.labelSheetLengthD.Text = "D";
            this.labelSheetLengthD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetLengthN
            // 
            this.sheetLengthN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetLengthN.Location = new System.Drawing.Point(40, 0);
            this.sheetLengthN.Margin = new System.Windows.Forms.Padding(0);
            this.sheetLengthN.Name = "sheetLengthN";
            this.sheetLengthN.Size = new System.Drawing.Size(60, 29);
            this.sheetLengthN.TabIndex = 3;
            this.sheetLengthN.ValueChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // sheetLengthD
            // 
            this.sheetLengthD.DecimalPlaces = 3;
            this.sheetLengthD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetLengthD.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.sheetLengthD.Location = new System.Drawing.Point(40, 29);
            this.sheetLengthD.Margin = new System.Windows.Forms.Padding(0);
            this.sheetLengthD.Name = "sheetLengthD";
            this.sheetLengthD.Size = new System.Drawing.Size(60, 29);
            this.sheetLengthD.TabIndex = 3;
            this.sheetLengthD.ValueChanged += new System.EventHandler(this.OnValueChanged);
            // 
            // labelSheetLengthNUnit
            // 
            this.labelSheetLengthNUnit.AutoSize = true;
            this.labelSheetLengthNUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthNUnit.Location = new System.Drawing.Point(100, 0);
            this.labelSheetLengthNUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthNUnit.Name = "labelSheetLengthNUnit";
            this.labelSheetLengthNUnit.Size = new System.Drawing.Size(40, 29);
            this.labelSheetLengthNUnit.TabIndex = 2;
            this.labelSheetLengthNUnit.Text = "EA";
            this.labelSheetLengthNUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetLengthDUnit
            // 
            this.labelSheetLengthDUnit.AutoSize = true;
            this.labelSheetLengthDUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthDUnit.Location = new System.Drawing.Point(100, 29);
            this.labelSheetLengthDUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthDUnit.Name = "labelSheetLengthDUnit";
            this.labelSheetLengthDUnit.Size = new System.Drawing.Size(40, 29);
            this.labelSheetLengthDUnit.TabIndex = 2;
            this.labelSheetLengthDUnit.Text = "mm";
            this.labelSheetLengthDUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SettingAlarmPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel4);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "SettingAlarmPage";
            this.Size = new System.Drawing.Size(1003, 685);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.continuedN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.continuedR)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repeatedN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repeatedR)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.normalN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.normalR)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetLengthN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetLengthD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox useContinuedDefectAlarm;
        private System.Windows.Forms.CheckBox useRepeatedDefectAlarm;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelReapetedNc;
        private System.Windows.Forms.Label labelReapetedRc;
        private System.Windows.Forms.NumericUpDown continuedN;
        private System.Windows.Forms.NumericUpDown continuedR;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelReapetedN;
        private System.Windows.Forms.Label labelReapetedR;
        private System.Windows.Forms.NumericUpDown repeatedN;
        private System.Windows.Forms.NumericUpDown repeatedR;
        private System.Windows.Forms.Label labelReapetedNcUnit;
        private System.Windows.Forms.Label labelReapetedRcUnit;
        private System.Windows.Forms.Label labelReapetedRUnit;
        private System.Windows.Forms.Label labelReapetedNUnit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label labelNormalRUnit;
        private System.Windows.Forms.Label labelNormalNUnit;
        private System.Windows.Forms.Label labelNormalN;
        private System.Windows.Forms.Label labelNormalR;
        private System.Windows.Forms.NumericUpDown normalN;
        private System.Windows.Forms.NumericUpDown normalR;
        private System.Windows.Forms.CheckBox useNormalDefectAlarm;
        private System.Windows.Forms.Label labelNormalDefects;
        private System.Windows.Forms.Label labelRepeatedDefects;
        private System.Windows.Forms.Label labelSheetLength;
        private System.Windows.Forms.CheckBox useSheetLengthAlarm;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label labelSheetLengthN;
        private System.Windows.Forms.Label labelSheetLengthD;
        private System.Windows.Forms.NumericUpDown sheetLengthN;
        private System.Windows.Forms.NumericUpDown sheetLengthD;
        private System.Windows.Forms.Label labelSheetLengthNUnit;
        private System.Windows.Forms.Label labelSheetLengthDUnit;
    }
}
