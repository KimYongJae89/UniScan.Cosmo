namespace CameraCalibration
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._MainForm_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MainForm_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MainForm_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._MainForm_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.buttonCalibrate = new System.Windows.Forms.Button();
            this.buttonSaveCalibration = new System.Windows.Forms.Button();
            this.buttonLoadCalibration = new System.Windows.Forms.Button();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.label14 = new System.Windows.Forms.Label();
            this.labelTargetDevice = new System.Windows.Forms.Label();
            this.layoutTargetDevice = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGrabMulti = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonLightOff = new System.Windows.Forms.Button();
            this.buttonGrab = new System.Windows.Forms.Button();
            this.labelCamera = new System.Windows.Forms.Label();
            this.buttonLightOn = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.numericUpDownExternalExpose = new System.Windows.Forms.NumericUpDown();
            this.label49 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.comboBoxCamera = new System.Windows.Forms.ComboBox();
            this.radioDeviceLightType = new System.Windows.Forms.RadioButton();
            this.radioDeviceLightValue = new System.Windows.Forms.RadioButton();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.comboLightType = new System.Windows.Forms.ComboBox();
            this.layoutMiddle = new System.Windows.Forms.TableLayoutPanel();
            this.Value = new System.Windows.Forms.Label();
            this.labelImage = new System.Windows.Forms.Label();
            this.layoutResult = new System.Windows.Forms.TableLayoutPanel();
            this.label52 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.imageContainer = new System.Windows.Forms.SplitContainer();
            this.panelHistogram = new System.Windows.Forms.Panel();
            this.panelDrawBox = new System.Windows.Forms.Panel();
            this.layoutBottom = new System.Windows.Forms.TableLayoutPanel();
            this.tabControlCalibrationType = new System.Windows.Forms.TabControl();
            this.tabPageContant = new System.Windows.Forms.TabPage();
            this.pelHeight = new System.Windows.Forms.TextBox();
            this.pelWidth = new System.Windows.Forms.TextBox();
            this.labelScaleY = new System.Windows.Forms.Label();
            this.labelScaleX = new System.Windows.Forms.Label();
            this.tabPageJig = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelNumRow = new System.Windows.Forms.Label();
            this.labelNumCol = new System.Windows.Forms.Label();
            this.colSpace = new System.Windows.Forms.TextBox();
            this.labelRowSpace = new System.Windows.Forms.Label();
            this.numCol = new System.Windows.Forms.NumericUpDown();
            this.labelColSpace = new System.Windows.Forms.Label();
            this.rowSpace = new System.Windows.Forms.TextBox();
            this.numRow = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.calibrationTypeGrid = new System.Windows.Forms.RadioButton();
            this.calibrationTypeChessboard = new System.Windows.Forms.RadioButton();
            this.tabPageRuler = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label38 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label42 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label53 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.regionNum = new System.Windows.Forms.NumericUpDown();
            this.checkThreshold = new System.Windows.Forms.CheckBox();
            this.label50 = new System.Windows.Forms.Label();
            this.propertyPartial = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.propertyScale = new System.Windows.Forms.NumericUpDown();
            this.propertyThreshold = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.propertyWidth = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.propertyHeight = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.layoutMain.SuspendLayout();
            this.layoutTargetDevice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExternalExpose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.layoutMiddle.SuspendLayout();
            this.layoutResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageContainer)).BeginInit();
            this.imageContainer.Panel1.SuspendLayout();
            this.imageContainer.Panel2.SuspendLayout();
            this.imageContainer.SuspendLayout();
            this.layoutBottom.SuspendLayout();
            this.tabControlCalibrationType.SuspendLayout();
            this.tabPageContant.SuspendLayout();
            this.tabPageJig.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRow)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tabPageRuler.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.regionNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyPartial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Top
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(6);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1274, 0);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 940);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(6);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1274, 0);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(6);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(0, 908);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1274, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(6);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(0, 908);
            // 
            // ultraFormManager
            // 
            this.ultraFormManager.Form = this;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            appearance1.TextHAlignAsString = "Left";
            this.ultraFormManager.FormStyleSettings.CaptionAreaAppearance = appearance1;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.HotTrackAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(168)))), ((int)(((byte)(12)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.PressedAppearance = appearance4;
            this.ultraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2013;
            // 
            // _MainForm_UltraFormManager_Dock_Area_Top
            // 
            this._MainForm_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MainForm_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._MainForm_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._MainForm_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MainForm_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._MainForm_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._MainForm_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(6);
            this._MainForm_UltraFormManager_Dock_Area_Top.Name = "_MainForm_UltraFormManager_Dock_Area_Top";
            this._MainForm_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1274, 31);
            // 
            // _MainForm_UltraFormManager_Dock_Area_Bottom
            // 
            this._MainForm_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MainForm_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._MainForm_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._MainForm_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MainForm_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._MainForm_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._MainForm_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 939);
            this._MainForm_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(6);
            this._MainForm_UltraFormManager_Dock_Area_Bottom.Name = "_MainForm_UltraFormManager_Dock_Area_Bottom";
            this._MainForm_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1274, 1);
            // 
            // _MainForm_UltraFormManager_Dock_Area_Left
            // 
            this._MainForm_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MainForm_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._MainForm_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._MainForm_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MainForm_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._MainForm_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._MainForm_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._MainForm_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(6);
            this._MainForm_UltraFormManager_Dock_Area_Left.Name = "_MainForm_UltraFormManager_Dock_Area_Left";
            this._MainForm_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 908);
            // 
            // _MainForm_UltraFormManager_Dock_Area_Right
            // 
            this._MainForm_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._MainForm_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._MainForm_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._MainForm_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._MainForm_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._MainForm_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._MainForm_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1273, 31);
            this._MainForm_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(6);
            this._MainForm_UltraFormManager_Dock_Area_Right.Name = "_MainForm_UltraFormManager_Dock_Area_Right";
            this._MainForm_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 908);
            // 
            // buttonCalibrate
            // 
            this.buttonCalibrate.Location = new System.Drawing.Point(863, 3);
            this.buttonCalibrate.Name = "buttonCalibrate";
            this.buttonCalibrate.Size = new System.Drawing.Size(108, 79);
            this.buttonCalibrate.TabIndex = 19;
            this.buttonCalibrate.Text = "Calibrate";
            this.buttonCalibrate.UseVisualStyleBackColor = true;
            // 
            // buttonSaveCalibration
            // 
            this.buttonSaveCalibration.Location = new System.Drawing.Point(863, 203);
            this.buttonSaveCalibration.Name = "buttonSaveCalibration";
            this.buttonSaveCalibration.Size = new System.Drawing.Size(108, 14);
            this.buttonSaveCalibration.TabIndex = 21;
            this.buttonSaveCalibration.Text = "Save";
            this.buttonSaveCalibration.UseVisualStyleBackColor = true;
            // 
            // buttonLoadCalibration
            // 
            this.buttonLoadCalibration.Location = new System.Drawing.Point(863, 103);
            this.buttonLoadCalibration.Name = "buttonLoadCalibration";
            this.buttonLoadCalibration.Size = new System.Drawing.Size(108, 27);
            this.buttonLoadCalibration.TabIndex = 20;
            this.buttonLoadCalibration.Text = "Load";
            this.buttonLoadCalibration.UseVisualStyleBackColor = true;
            // 
            // layoutMain
            // 
            this.layoutMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.label14, 0, 3);
            this.layoutMain.Controls.Add(this.labelTargetDevice, 0, 0);
            this.layoutMain.Controls.Add(this.layoutTargetDevice, 0, 1);
            this.layoutMain.Controls.Add(this.layoutMiddle, 0, 2);
            this.layoutMain.Controls.Add(this.layoutBottom, 0, 4);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(1, 31);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 5;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 224F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutMain.Size = new System.Drawing.Size(1272, 908);
            this.layoutMain.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Gray;
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(1, 636);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(219, 39);
            this.label14.TabIndex = 9;
            this.label14.Text = "TargetDevice";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTargetDevice
            // 
            this.labelTargetDevice.AutoSize = true;
            this.labelTargetDevice.BackColor = System.Drawing.Color.Gray;
            this.labelTargetDevice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelTargetDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.labelTargetDevice.Location = new System.Drawing.Point(1, 1);
            this.labelTargetDevice.Margin = new System.Windows.Forms.Padding(0);
            this.labelTargetDevice.Name = "labelTargetDevice";
            this.labelTargetDevice.Size = new System.Drawing.Size(219, 39);
            this.labelTargetDevice.TabIndex = 6;
            this.labelTargetDevice.Text = "TargetDevice";
            this.labelTargetDevice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutTargetDevice
            // 
            this.layoutTargetDevice.ColumnCount = 12;
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutTargetDevice.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 151F));
            this.layoutTargetDevice.Controls.Add(this.buttonGrabMulti, 11, 0);
            this.layoutTargetDevice.Controls.Add(this.buttonSave, 11, 1);
            this.layoutTargetDevice.Controls.Add(this.buttonLoad, 10, 1);
            this.layoutTargetDevice.Controls.Add(this.buttonLightOff, 9, 1);
            this.layoutTargetDevice.Controls.Add(this.buttonGrab, 10, 0);
            this.layoutTargetDevice.Controls.Add(this.labelCamera, 0, 0);
            this.layoutTargetDevice.Controls.Add(this.buttonLightOn, 9, 0);
            this.layoutTargetDevice.Controls.Add(this.label18, 2, 0);
            this.layoutTargetDevice.Controls.Add(this.numericUpDownExternalExpose, 1, 1);
            this.layoutTargetDevice.Controls.Add(this.label49, 0, 1);
            this.layoutTargetDevice.Controls.Add(this.label13, 7, 1);
            this.layoutTargetDevice.Controls.Add(this.comboBoxCamera, 1, 0);
            this.layoutTargetDevice.Controls.Add(this.radioDeviceLightType, 4, 0);
            this.layoutTargetDevice.Controls.Add(this.radioDeviceLightValue, 4, 1);
            this.layoutTargetDevice.Controls.Add(this.trackBar1, 5, 1);
            this.layoutTargetDevice.Controls.Add(this.comboLightType, 5, 0);
            this.layoutTargetDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTargetDevice.Location = new System.Drawing.Point(1, 52);
            this.layoutTargetDevice.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTargetDevice.Name = "layoutTargetDevice";
            this.layoutTargetDevice.RowCount = 2;
            this.layoutTargetDevice.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutTargetDevice.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.layoutTargetDevice.Size = new System.Drawing.Size(1270, 70);
            this.layoutTargetDevice.TabIndex = 1;
            // 
            // buttonGrabMulti
            // 
            this.buttonGrabMulti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGrabMulti.Location = new System.Drawing.Point(1119, 0);
            this.buttonGrabMulti.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGrabMulti.Name = "buttonGrabMulti";
            this.buttonGrabMulti.Size = new System.Drawing.Size(151, 35);
            this.buttonGrabMulti.TabIndex = 11;
            this.buttonGrabMulti.Text = "Grab Multi";
            this.buttonGrabMulti.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Location = new System.Drawing.Point(1119, 35);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(151, 35);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Save Image";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoad.Location = new System.Drawing.Point(969, 35);
            this.buttonLoad.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(150, 35);
            this.buttonLoad.TabIndex = 16;
            this.buttonLoad.Text = "Load Image";
            this.buttonLoad.UseVisualStyleBackColor = true;
            // 
            // buttonLightOff
            // 
            this.buttonLightOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLightOff.Location = new System.Drawing.Point(819, 35);
            this.buttonLightOff.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLightOff.Name = "buttonLightOff";
            this.buttonLightOff.Size = new System.Drawing.Size(150, 35);
            this.buttonLightOff.TabIndex = 19;
            this.buttonLightOff.Text = "Light Off";
            this.buttonLightOff.UseVisualStyleBackColor = true;
            // 
            // buttonGrab
            // 
            this.buttonGrab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGrab.Location = new System.Drawing.Point(969, 0);
            this.buttonGrab.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGrab.Name = "buttonGrab";
            this.buttonGrab.Size = new System.Drawing.Size(150, 35);
            this.buttonGrab.TabIndex = 10;
            this.buttonGrab.Text = "Grab Once";
            this.buttonGrab.UseVisualStyleBackColor = true;
            // 
            // labelCamera
            // 
            this.labelCamera.AutoSize = true;
            this.labelCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCamera.Location = new System.Drawing.Point(0, 0);
            this.labelCamera.Margin = new System.Windows.Forms.Padding(0);
            this.labelCamera.Name = "labelCamera";
            this.labelCamera.Size = new System.Drawing.Size(150, 35);
            this.labelCamera.TabIndex = 5;
            this.labelCamera.Text = "Camera";
            this.labelCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonLightOn
            // 
            this.buttonLightOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLightOn.Location = new System.Drawing.Point(819, 0);
            this.buttonLightOn.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLightOn.Name = "buttonLightOn";
            this.buttonLightOn.Size = new System.Drawing.Size(150, 35);
            this.buttonLightOn.TabIndex = 18;
            this.buttonLightOn.Text = "Light On";
            this.buttonLightOn.UseVisualStyleBackColor = true;
            this.buttonLightOn.Click += new System.EventHandler(this.buttonLightOn_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Location = new System.Drawing.Point(300, 0);
            this.label18.Margin = new System.Windows.Forms.Padding(0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 35);
            this.label18.TabIndex = 17;
            this.label18.Text = "ms";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDownExternalExpose
            // 
            this.numericUpDownExternalExpose.DecimalPlaces = 1;
            this.numericUpDownExternalExpose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownExternalExpose.Location = new System.Drawing.Point(150, 38);
            this.numericUpDownExternalExpose.Margin = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.numericUpDownExternalExpose.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownExternalExpose.Name = "numericUpDownExternalExpose";
            this.numericUpDownExternalExpose.Size = new System.Drawing.Size(150, 29);
            this.numericUpDownExternalExpose.TabIndex = 13;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label49.Location = new System.Drawing.Point(0, 35);
            this.label49.Margin = new System.Windows.Forms.Padding(0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(150, 35);
            this.label49.TabIndex = 12;
            this.label49.Text = "Exposure";
            this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(752, 35);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 35);
            this.label13.TabIndex = 14;
            this.label13.Text = "000";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBoxCamera
            // 
            this.comboBoxCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxCamera.FormattingEnabled = true;
            this.comboBoxCamera.Location = new System.Drawing.Point(150, 1);
            this.comboBoxCamera.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.comboBoxCamera.Name = "comboBoxCamera";
            this.comboBoxCamera.Size = new System.Drawing.Size(150, 32);
            this.comboBoxCamera.TabIndex = 3;
            // 
            // radioDeviceLightType
            // 
            this.radioDeviceLightType.AutoSize = true;
            this.radioDeviceLightType.Checked = true;
            this.radioDeviceLightType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioDeviceLightType.Location = new System.Drawing.Point(352, 0);
            this.radioDeviceLightType.Margin = new System.Windows.Forms.Padding(0);
            this.radioDeviceLightType.Name = "radioDeviceLightType";
            this.radioDeviceLightType.Size = new System.Drawing.Size(150, 35);
            this.radioDeviceLightType.TabIndex = 7;
            this.radioDeviceLightType.TabStop = true;
            this.radioDeviceLightType.Text = "Light Type";
            this.radioDeviceLightType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioDeviceLightType.UseVisualStyleBackColor = true;
            // 
            // radioDeviceLightValue
            // 
            this.radioDeviceLightValue.AutoSize = true;
            this.radioDeviceLightValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioDeviceLightValue.Location = new System.Drawing.Point(352, 35);
            this.radioDeviceLightValue.Margin = new System.Windows.Forms.Padding(0);
            this.radioDeviceLightValue.Name = "radioDeviceLightValue";
            this.radioDeviceLightValue.Size = new System.Drawing.Size(150, 35);
            this.radioDeviceLightValue.TabIndex = 8;
            this.radioDeviceLightValue.Text = "Light Value";
            this.radioDeviceLightValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioDeviceLightValue.UseVisualStyleBackColor = true;
            // 
            // trackBar1
            // 
            this.layoutTargetDevice.SetColumnSpan(this.trackBar1, 2);
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar1.LargeChange = 10;
            this.trackBar1.Location = new System.Drawing.Point(502, 35);
            this.trackBar1.Margin = new System.Windows.Forms.Padding(0);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(250, 35);
            this.trackBar1.TabIndex = 9;
            // 
            // comboLightType
            // 
            this.comboLightType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboLightType.FormattingEnabled = true;
            this.comboLightType.Location = new System.Drawing.Point(502, 0);
            this.comboLightType.Margin = new System.Windows.Forms.Padding(0);
            this.comboLightType.Name = "comboLightType";
            this.comboLightType.Size = new System.Drawing.Size(150, 32);
            this.comboLightType.TabIndex = 2;
            // 
            // layoutMiddle
            // 
            this.layoutMiddle.ColumnCount = 2;
            this.layoutMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.64567F));
            this.layoutMiddle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.35433F));
            this.layoutMiddle.Controls.Add(this.Value, 1, 0);
            this.layoutMiddle.Controls.Add(this.labelImage, 0, 0);
            this.layoutMiddle.Controls.Add(this.layoutResult, 1, 1);
            this.layoutMiddle.Controls.Add(this.imageContainer, 0, 1);
            this.layoutMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMiddle.Location = new System.Drawing.Point(1, 123);
            this.layoutMiddle.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMiddle.Name = "layoutMiddle";
            this.layoutMiddle.RowCount = 2;
            this.layoutMiddle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutMiddle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMiddle.Size = new System.Drawing.Size(1270, 512);
            this.layoutMiddle.TabIndex = 2;
            // 
            // Value
            // 
            this.Value.AutoSize = true;
            this.Value.BackColor = System.Drawing.Color.Gray;
            this.Value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Value.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.Value.Location = new System.Drawing.Point(947, 0);
            this.Value.Margin = new System.Windows.Forms.Padding(0);
            this.Value.Name = "Value";
            this.Value.Size = new System.Drawing.Size(219, 39);
            this.Value.TabIndex = 8;
            this.Value.Text = "TargetDevice";
            this.Value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelImage
            // 
            this.labelImage.AutoSize = true;
            this.labelImage.BackColor = System.Drawing.Color.Gray;
            this.labelImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.labelImage.Location = new System.Drawing.Point(0, 0);
            this.labelImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelImage.Name = "labelImage";
            this.labelImage.Size = new System.Drawing.Size(112, 39);
            this.labelImage.TabIndex = 7;
            this.labelImage.Text = "Image";
            this.labelImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutResult
            // 
            this.layoutResult.ColumnCount = 3;
            this.layoutResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutResult.Controls.Add(this.label52, 2, 4);
            this.layoutResult.Controls.Add(this.label51, 0, 4);
            this.layoutResult.Controls.Add(this.label10, 2, 3);
            this.layoutResult.Controls.Add(this.label8, 2, 1);
            this.layoutResult.Controls.Add(this.label17, 0, 3);
            this.layoutResult.Controls.Add(this.label12, 1, 2);
            this.layoutResult.Controls.Add(this.label11, 2, 2);
            this.layoutResult.Controls.Add(this.label9, 1, 1);
            this.layoutResult.Controls.Add(this.label7, 2, 0);
            this.layoutResult.Controls.Add(this.label6, 1, 0);
            this.layoutResult.Controls.Add(this.label5, 0, 0);
            this.layoutResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutResult.Location = new System.Drawing.Point(947, 50);
            this.layoutResult.Margin = new System.Windows.Forms.Padding(0);
            this.layoutResult.Name = "layoutResult";
            this.layoutResult.RowCount = 5;
            this.layoutResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.layoutResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutResult.Size = new System.Drawing.Size(323, 462);
            this.layoutResult.TabIndex = 0;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label52.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label52.Location = new System.Drawing.Point(262, 368);
            this.label52.Margin = new System.Windows.Forms.Padding(0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(61, 94);
            this.label52.TabIndex = 19;
            this.label52.Text = "Value";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layoutResult.SetColumnSpan(this.label51, 2);
            this.label51.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label51.Location = new System.Drawing.Point(0, 368);
            this.label51.Margin = new System.Windows.Forms.Padding(0);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(262, 94);
            this.label51.TabIndex = 18;
            this.label51.Text = "Focus Value";
            this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(262, 276);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(61, 92);
            this.label10.TabIndex = 16;
            this.label10.Text = "Value";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(262, 92);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(61, 92);
            this.label8.TabIndex = 15;
            this.label8.Text = "Value";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layoutResult.SetColumnSpan(this.label17, 2);
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Location = new System.Drawing.Point(0, 276);
            this.label17.Margin = new System.Windows.Forms.Padding(0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(262, 92);
            this.label17.TabIndex = 12;
            this.label17.Text = "Resolution [um/px]";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(131, 184);
            this.label12.Margin = new System.Windows.Forms.Padding(0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(131, 92);
            this.label12.TabIndex = 7;
            this.label12.Text = "Maximum";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(262, 184);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 92);
            this.label11.TabIndex = 6;
            this.label11.Text = "Value";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(131, 92);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(131, 92);
            this.label9.TabIndex = 4;
            this.label9.Text = "Minimum";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(262, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 92);
            this.label7.TabIndex = 2;
            this.label7.Text = "Value";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(131, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 92);
            this.label6.TabIndex = 1;
            this.label6.Text = "Average";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.layoutResult.SetRowSpan(this.label5, 3);
            this.label5.Size = new System.Drawing.Size(131, 276);
            this.label5.TabIndex = 0;
            this.label5.Text = "Brightness";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imageContainer
            // 
            this.imageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageContainer.Location = new System.Drawing.Point(0, 50);
            this.imageContainer.Margin = new System.Windows.Forms.Padding(0);
            this.imageContainer.Name = "imageContainer";
            // 
            // imageContainer.Panel1
            // 
            this.imageContainer.Panel1.Controls.Add(this.panelHistogram);
            // 
            // imageContainer.Panel2
            // 
            this.imageContainer.Panel2.Controls.Add(this.panelDrawBox);
            this.imageContainer.Size = new System.Drawing.Size(947, 462);
            this.imageContainer.SplitterDistance = 310;
            this.imageContainer.TabIndex = 1;
            // 
            // panelHistogram
            // 
            this.panelHistogram.AutoSize = true;
            this.panelHistogram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHistogram.Location = new System.Drawing.Point(0, 0);
            this.panelHistogram.Name = "panelHistogram";
            this.panelHistogram.Size = new System.Drawing.Size(310, 462);
            this.panelHistogram.TabIndex = 1;
            // 
            // panelDrawBox
            // 
            this.panelDrawBox.AutoSize = true;
            this.panelDrawBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDrawBox.Location = new System.Drawing.Point(0, 0);
            this.panelDrawBox.Name = "panelDrawBox";
            this.panelDrawBox.Size = new System.Drawing.Size(633, 462);
            this.panelDrawBox.TabIndex = 0;
            // 
            // layoutBottom
            // 
            this.layoutBottom.ColumnCount = 2;
            this.layoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.78656F));
            this.layoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.21344F));
            this.layoutBottom.Controls.Add(this.buttonSaveCalibration, 1, 2);
            this.layoutBottom.Controls.Add(this.buttonLoadCalibration, 1, 1);
            this.layoutBottom.Controls.Add(this.tabControlCalibrationType, 0, 0);
            this.layoutBottom.Controls.Add(this.buttonCalibrate, 1, 0);
            this.layoutBottom.Location = new System.Drawing.Point(1, 683);
            this.layoutBottom.Margin = new System.Windows.Forms.Padding(0);
            this.layoutBottom.Name = "layoutBottom";
            this.layoutBottom.RowCount = 3;
            this.layoutBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutBottom.Size = new System.Drawing.Size(1270, 220);
            this.layoutBottom.TabIndex = 3;
            // 
            // tabControlCalibrationType
            // 
            this.tabControlCalibrationType.Controls.Add(this.tabPageContant);
            this.tabControlCalibrationType.Controls.Add(this.tabPageJig);
            this.tabControlCalibrationType.Controls.Add(this.tabPageRuler);
            this.tabControlCalibrationType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCalibrationType.Location = new System.Drawing.Point(0, 0);
            this.tabControlCalibrationType.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlCalibrationType.Name = "tabControlCalibrationType";
            this.layoutBottom.SetRowSpan(this.tabControlCalibrationType, 3);
            this.tabControlCalibrationType.SelectedIndex = 0;
            this.tabControlCalibrationType.Size = new System.Drawing.Size(860, 220);
            this.tabControlCalibrationType.TabIndex = 17;
            // 
            // tabPageContant
            // 
            this.tabPageContant.Controls.Add(this.pelHeight);
            this.tabPageContant.Controls.Add(this.pelWidth);
            this.tabPageContant.Controls.Add(this.labelScaleY);
            this.tabPageContant.Controls.Add(this.labelScaleX);
            this.tabPageContant.Location = new System.Drawing.Point(4, 33);
            this.tabPageContant.Name = "tabPageContant";
            this.tabPageContant.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageContant.Size = new System.Drawing.Size(852, 183);
            this.tabPageContant.TabIndex = 0;
            this.tabPageContant.Text = "Constant";
            this.tabPageContant.UseVisualStyleBackColor = true;
            // 
            // pelHeight
            // 
            this.pelHeight.Location = new System.Drawing.Point(215, 17);
            this.pelHeight.Name = "pelHeight";
            this.pelHeight.Size = new System.Drawing.Size(75, 29);
            this.pelHeight.TabIndex = 10;
            // 
            // pelWidth
            // 
            this.pelWidth.Location = new System.Drawing.Point(62, 17);
            this.pelWidth.Name = "pelWidth";
            this.pelWidth.Size = new System.Drawing.Size(75, 29);
            this.pelWidth.TabIndex = 11;
            // 
            // labelScaleY
            // 
            this.labelScaleY.AutoSize = true;
            this.labelScaleY.Location = new System.Drawing.Point(160, 21);
            this.labelScaleY.Name = "labelScaleY";
            this.labelScaleY.Size = new System.Drawing.Size(74, 24);
            this.labelScaleY.TabIndex = 8;
            this.labelScaleY.Text = "Scale Y";
            // 
            // labelScaleX
            // 
            this.labelScaleX.AutoSize = true;
            this.labelScaleX.Location = new System.Drawing.Point(7, 21);
            this.labelScaleX.Name = "labelScaleX";
            this.labelScaleX.Size = new System.Drawing.Size(76, 24);
            this.labelScaleX.TabIndex = 9;
            this.labelScaleX.Text = "Scale X";
            // 
            // tabPageJig
            // 
            this.tabPageJig.Controls.Add(this.groupBox4);
            this.tabPageJig.Controls.Add(this.groupBox3);
            this.tabPageJig.Location = new System.Drawing.Point(4, 33);
            this.tabPageJig.Name = "tabPageJig";
            this.tabPageJig.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageJig.Size = new System.Drawing.Size(852, 183);
            this.tabPageJig.TabIndex = 1;
            this.tabPageJig.Text = "Jig";
            this.tabPageJig.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.labelNumRow);
            this.groupBox4.Controls.Add(this.labelNumCol);
            this.groupBox4.Controls.Add(this.colSpace);
            this.groupBox4.Controls.Add(this.labelRowSpace);
            this.groupBox4.Controls.Add(this.numCol);
            this.groupBox4.Controls.Add(this.labelColSpace);
            this.groupBox4.Controls.Add(this.rowSpace);
            this.groupBox4.Controls.Add(this.numRow);
            this.groupBox4.Location = new System.Drawing.Point(146, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(340, 82);
            this.groupBox4.TabIndex = 20;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Property";
            // 
            // labelNumRow
            // 
            this.labelNumRow.AutoSize = true;
            this.labelNumRow.Location = new System.Drawing.Point(15, 28);
            this.labelNumRow.Name = "labelNumRow";
            this.labelNumRow.Size = new System.Drawing.Size(94, 24);
            this.labelNumRow.TabIndex = 12;
            this.labelNumRow.Text = "Num Row";
            // 
            // labelNumCol
            // 
            this.labelNumCol.AutoSize = true;
            this.labelNumCol.Location = new System.Drawing.Point(176, 28);
            this.labelNumCol.Name = "labelNumCol";
            this.labelNumCol.Size = new System.Drawing.Size(84, 24);
            this.labelNumCol.TabIndex = 11;
            this.labelNumCol.Text = "Num Col";
            // 
            // colSpace
            // 
            this.colSpace.Location = new System.Drawing.Point(237, 51);
            this.colSpace.Name = "colSpace";
            this.colSpace.Size = new System.Drawing.Size(75, 29);
            this.colSpace.TabIndex = 15;
            this.colSpace.Text = "5";
            // 
            // labelRowSpace
            // 
            this.labelRowSpace.AutoSize = true;
            this.labelRowSpace.Location = new System.Drawing.Point(6, 55);
            this.labelRowSpace.Name = "labelRowSpace";
            this.labelRowSpace.Size = new System.Drawing.Size(107, 24);
            this.labelRowSpace.TabIndex = 10;
            this.labelRowSpace.Text = "Row Space";
            // 
            // numCol
            // 
            this.numCol.Location = new System.Drawing.Point(237, 24);
            this.numCol.Name = "numCol";
            this.numCol.Size = new System.Drawing.Size(75, 29);
            this.numCol.TabIndex = 13;
            // 
            // labelColSpace
            // 
            this.labelColSpace.AutoSize = true;
            this.labelColSpace.Location = new System.Drawing.Point(167, 55);
            this.labelColSpace.Name = "labelColSpace";
            this.labelColSpace.Size = new System.Drawing.Size(97, 24);
            this.labelColSpace.TabIndex = 9;
            this.labelColSpace.Text = "Col Space";
            // 
            // rowSpace
            // 
            this.rowSpace.Location = new System.Drawing.Point(82, 51);
            this.rowSpace.Name = "rowSpace";
            this.rowSpace.Size = new System.Drawing.Size(75, 29);
            this.rowSpace.TabIndex = 16;
            this.rowSpace.Text = "5";
            // 
            // numRow
            // 
            this.numRow.Location = new System.Drawing.Point(82, 24);
            this.numRow.Name = "numRow";
            this.numRow.Size = new System.Drawing.Size(75, 29);
            this.numRow.TabIndex = 14;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.calibrationTypeGrid);
            this.groupBox3.Controls.Add(this.calibrationTypeChessboard);
            this.groupBox3.Location = new System.Drawing.Point(7, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(133, 82);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Type";
            // 
            // calibrationTypeGrid
            // 
            this.calibrationTypeGrid.AutoSize = true;
            this.calibrationTypeGrid.Checked = true;
            this.calibrationTypeGrid.Location = new System.Drawing.Point(16, 21);
            this.calibrationTypeGrid.Name = "calibrationTypeGrid";
            this.calibrationTypeGrid.Size = new System.Drawing.Size(63, 28);
            this.calibrationTypeGrid.TabIndex = 17;
            this.calibrationTypeGrid.TabStop = true;
            this.calibrationTypeGrid.Text = "Grid";
            this.calibrationTypeGrid.UseVisualStyleBackColor = true;
            // 
            // calibrationTypeChessboard
            // 
            this.calibrationTypeChessboard.AutoSize = true;
            this.calibrationTypeChessboard.Location = new System.Drawing.Point(16, 51);
            this.calibrationTypeChessboard.Name = "calibrationTypeChessboard";
            this.calibrationTypeChessboard.Size = new System.Drawing.Size(131, 28);
            this.calibrationTypeChessboard.TabIndex = 18;
            this.calibrationTypeChessboard.Text = "ChessBoard";
            this.calibrationTypeChessboard.UseVisualStyleBackColor = true;
            // 
            // tabPageRuler
            // 
            this.tabPageRuler.Controls.Add(this.tableLayoutPanel3);
            this.tabPageRuler.Controls.Add(this.groupBox5);
            this.tabPageRuler.Location = new System.Drawing.Point(4, 33);
            this.tabPageRuler.Name = "tabPageRuler";
            this.tabPageRuler.Size = new System.Drawing.Size(852, 183);
            this.tabPageRuler.TabIndex = 2;
            this.tabPageRuler.Text = "Ruler";
            this.tabPageRuler.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox10, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.groupBox9, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.groupBox6, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.groupBox8, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(180, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(672, 183);
            this.tableLayoutPanel3.TabIndex = 21;
            // 
            // groupBox10
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.groupBox10, 3);
            this.groupBox10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox10.Location = new System.Drawing.Point(3, 3);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(666, 123);
            this.groupBox10.TabIndex = 25;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Line-Profile";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.tableLayoutPanel5);
            this.groupBox9.Controls.Add(this.pictureBox3);
            this.groupBox9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox9.Location = new System.Drawing.Point(450, 132);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(219, 48);
            this.groupBox9.TabIndex = 23;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Right-Side Result";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.label38, 2, 4);
            this.tableLayoutPanel5.Controls.Add(this.label39, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.label41, 2, 3);
            this.tableLayoutPanel5.Controls.Add(this.label42, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.label44, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.label45, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.label46, 2, 2);
            this.tableLayoutPanel5.Controls.Add(this.label47, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.label48, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.label57, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.label58, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 110);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 5;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(213, 0);
            this.tableLayoutPanel5.TabIndex = 7;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label38.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label38.Location = new System.Drawing.Point(152, 0);
            this.label38.Margin = new System.Windows.Forms.Padding(0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(61, 1);
            this.label38.TabIndex = 19;
            this.label38.Text = "Value";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel5.SetColumnSpan(this.label39, 2);
            this.label39.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label39.Location = new System.Drawing.Point(0, 0);
            this.label39.Margin = new System.Windows.Forms.Padding(0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(152, 1);
            this.label39.TabIndex = 18;
            this.label39.Text = "Focus Value";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label41.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label41.Location = new System.Drawing.Point(152, 0);
            this.label41.Margin = new System.Windows.Forms.Padding(0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(61, 1);
            this.label41.TabIndex = 16;
            this.label41.Text = "Value";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label42.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label42.Location = new System.Drawing.Point(152, 0);
            this.label42.Margin = new System.Windows.Forms.Padding(0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(61, 1);
            this.label42.TabIndex = 15;
            this.label42.Text = "Value";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel5.SetColumnSpan(this.label44, 2);
            this.label44.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label44.Location = new System.Drawing.Point(0, 0);
            this.label44.Margin = new System.Windows.Forms.Padding(0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(152, 1);
            this.label44.TabIndex = 12;
            this.label44.Text = "Resolution [um/px]";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label45.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label45.Location = new System.Drawing.Point(76, 0);
            this.label45.Margin = new System.Windows.Forms.Padding(0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(76, 1);
            this.label45.TabIndex = 7;
            this.label45.Text = "Maximum";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label46.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label46.Location = new System.Drawing.Point(152, 0);
            this.label46.Margin = new System.Windows.Forms.Padding(0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(61, 1);
            this.label46.TabIndex = 6;
            this.label46.Text = "Value";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label47.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label47.Location = new System.Drawing.Point(76, 0);
            this.label47.Margin = new System.Windows.Forms.Padding(0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(76, 1);
            this.label47.TabIndex = 4;
            this.label47.Text = "Minimum";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label48.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label48.Location = new System.Drawing.Point(152, 0);
            this.label48.Margin = new System.Windows.Forms.Padding(0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(61, 1);
            this.label48.TabIndex = 2;
            this.label48.Text = "Value";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label57.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label57.Location = new System.Drawing.Point(76, 0);
            this.label57.Margin = new System.Windows.Forms.Padding(0);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(76, 1);
            this.label57.TabIndex = 1;
            this.label57.Text = "Average";
            this.label57.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label58.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label58.Location = new System.Drawing.Point(0, 0);
            this.label58.Margin = new System.Windows.Forms.Padding(0);
            this.label58.Name = "label58";
            this.tableLayoutPanel5.SetRowSpan(this.label58, 3);
            this.label58.Size = new System.Drawing.Size(76, 1);
            this.label58.TabIndex = 0;
            this.label58.Text = "Brightness";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox3.Location = new System.Drawing.Point(3, 25);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(213, 85);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.tableLayoutPanel4);
            this.groupBox6.Controls.Add(this.pictureBox2);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(226, 132);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(218, 48);
            this.groupBox6.TabIndex = 22;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Center Result";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.label27, 2, 4);
            this.tableLayoutPanel4.Controls.Add(this.label28, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.label30, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.label31, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.label33, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.label34, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.label35, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.label36, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label37, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label55, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label56, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 110);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 5;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(212, 0);
            this.tableLayoutPanel4.TabIndex = 7;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label27.Location = new System.Drawing.Point(150, 0);
            this.label27.Margin = new System.Windows.Forms.Padding(0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(62, 1);
            this.label27.TabIndex = 19;
            this.label27.Text = "Value";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel4.SetColumnSpan(this.label28, 2);
            this.label28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label28.Location = new System.Drawing.Point(0, 0);
            this.label28.Margin = new System.Windows.Forms.Padding(0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(150, 1);
            this.label28.TabIndex = 18;
            this.label28.Text = "Focus Value";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label30.Location = new System.Drawing.Point(150, 0);
            this.label30.Margin = new System.Windows.Forms.Padding(0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(62, 1);
            this.label30.TabIndex = 16;
            this.label30.Text = "Value";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label31.Location = new System.Drawing.Point(150, 0);
            this.label31.Margin = new System.Windows.Forms.Padding(0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(62, 1);
            this.label31.TabIndex = 15;
            this.label31.Text = "Value";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel4.SetColumnSpan(this.label33, 2);
            this.label33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label33.Location = new System.Drawing.Point(0, 0);
            this.label33.Margin = new System.Windows.Forms.Padding(0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(150, 1);
            this.label33.TabIndex = 12;
            this.label33.Text = "Resolution [um/px]";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label34.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label34.Location = new System.Drawing.Point(75, 0);
            this.label34.Margin = new System.Windows.Forms.Padding(0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(75, 1);
            this.label34.TabIndex = 7;
            this.label34.Text = "Maximum";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label35.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label35.Location = new System.Drawing.Point(150, 0);
            this.label35.Margin = new System.Windows.Forms.Padding(0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(62, 1);
            this.label35.TabIndex = 6;
            this.label35.Text = "Value";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label36.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label36.Location = new System.Drawing.Point(75, 0);
            this.label36.Margin = new System.Windows.Forms.Padding(0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(75, 1);
            this.label36.TabIndex = 4;
            this.label36.Text = "Minimum";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label37.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label37.Location = new System.Drawing.Point(150, 0);
            this.label37.Margin = new System.Windows.Forms.Padding(0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(62, 1);
            this.label37.TabIndex = 2;
            this.label37.Text = "Value";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label55.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label55.Location = new System.Drawing.Point(75, 0);
            this.label55.Margin = new System.Windows.Forms.Padding(0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(75, 1);
            this.label55.TabIndex = 1;
            this.label55.Text = "Average";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label56.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label56.Location = new System.Drawing.Point(0, 0);
            this.label56.Margin = new System.Windows.Forms.Padding(0);
            this.label56.Name = "label56";
            this.tableLayoutPanel4.SetRowSpan(this.label56, 3);
            this.label56.Size = new System.Drawing.Size(75, 1);
            this.label56.TabIndex = 0;
            this.label56.Text = "Brightness";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox2.Location = new System.Drawing.Point(3, 25);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(212, 85);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.tableLayoutPanel2);
            this.groupBox8.Controls.Add(this.pictureBox1);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(3, 132);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(217, 48);
            this.groupBox8.TabIndex = 21;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Left-Side Result";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label15, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.label16, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label19, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.label20, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label22, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label23, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label24, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.label25, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label26, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label53, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label54, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 110);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(211, 0);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(150, 0);
            this.label15.Margin = new System.Windows.Forms.Padding(0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(61, 1);
            this.label15.TabIndex = 19;
            this.label15.Text = "Value";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel2.SetColumnSpan(this.label16, 2);
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Location = new System.Drawing.Point(0, 0);
            this.label16.Margin = new System.Windows.Forms.Padding(0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(150, 1);
            this.label16.TabIndex = 18;
            this.label16.Text = "Focus Value";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Location = new System.Drawing.Point(150, 0);
            this.label19.Margin = new System.Windows.Forms.Padding(0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(61, 1);
            this.label19.TabIndex = 16;
            this.label19.Text = "Value";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Location = new System.Drawing.Point(150, 0);
            this.label20.Margin = new System.Windows.Forms.Padding(0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(61, 1);
            this.label20.TabIndex = 15;
            this.label20.Text = "Value";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel2.SetColumnSpan(this.label22, 2);
            this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label22.Location = new System.Drawing.Point(0, 0);
            this.label22.Margin = new System.Windows.Forms.Padding(0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(150, 1);
            this.label22.TabIndex = 12;
            this.label22.Text = "Resolution [um/px]";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label23.Location = new System.Drawing.Point(75, 0);
            this.label23.Margin = new System.Windows.Forms.Padding(0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(75, 1);
            this.label23.TabIndex = 7;
            this.label23.Text = "Maximum";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label24.Location = new System.Drawing.Point(150, 0);
            this.label24.Margin = new System.Windows.Forms.Padding(0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(61, 1);
            this.label24.TabIndex = 6;
            this.label24.Text = "Value";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label25.Location = new System.Drawing.Point(75, 0);
            this.label25.Margin = new System.Windows.Forms.Padding(0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(75, 1);
            this.label25.TabIndex = 4;
            this.label25.Text = "Minimum";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label26.Location = new System.Drawing.Point(150, 0);
            this.label26.Margin = new System.Windows.Forms.Padding(0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(61, 1);
            this.label26.TabIndex = 2;
            this.label26.Text = "Value";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label53.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label53.Location = new System.Drawing.Point(75, 0);
            this.label53.Margin = new System.Windows.Forms.Padding(0);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(75, 1);
            this.label53.TabIndex = 1;
            this.label53.Text = "Average";
            this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label54.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label54.Location = new System.Drawing.Point(0, 0);
            this.label54.Margin = new System.Windows.Forms.Padding(0);
            this.label54.Name = "label54";
            this.tableLayoutPanel2.SetRowSpan(this.label54, 3);
            this.label54.Size = new System.Drawing.Size(75, 1);
            this.label54.TabIndex = 0;
            this.label54.Text = "Brightness";
            this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(3, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(211, 85);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.regionNum);
            this.groupBox5.Controls.Add(this.checkThreshold);
            this.groupBox5.Controls.Add(this.label50);
            this.groupBox5.Controls.Add(this.propertyPartial);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.propertyScale);
            this.groupBox5.Controls.Add(this.propertyThreshold);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.propertyWidth);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.propertyHeight);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(180, 183);
            this.groupBox5.TabIndex = 19;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Property";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 174);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 24);
            this.label3.TabIndex = 26;
            this.label3.Text = "Region [N]";
            // 
            // regionNum
            // 
            this.regionNum.Location = new System.Drawing.Point(88, 170);
            this.regionNum.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.regionNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.regionNum.Name = "regionNum";
            this.regionNum.Size = new System.Drawing.Size(75, 29);
            this.regionNum.TabIndex = 27;
            this.regionNum.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // checkThreshold
            // 
            this.checkThreshold.AutoSize = true;
            this.checkThreshold.Location = new System.Drawing.Point(7, 135);
            this.checkThreshold.Name = "checkThreshold";
            this.checkThreshold.Size = new System.Drawing.Size(115, 28);
            this.checkThreshold.TabIndex = 25;
            this.checkThreshold.Text = "Threshold";
            this.checkThreshold.UseVisualStyleBackColor = true;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(13, 205);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(156, 24);
            this.label50.TabIndex = 23;
            this.label50.Text = "Partial Region [%]";
            // 
            // propertyPartial
            // 
            this.propertyPartial.Location = new System.Drawing.Point(87, 220);
            this.propertyPartial.Name = "propertyPartial";
            this.propertyPartial.Size = new System.Drawing.Size(75, 29);
            this.propertyPartial.TabIndex = 24;
            this.propertyPartial.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 24);
            this.label4.TabIndex = 21;
            this.label4.Text = "Scale [mm]";
            // 
            // propertyScale
            // 
            this.propertyScale.DecimalPlaces = 4;
            this.propertyScale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.propertyScale.Location = new System.Drawing.Point(89, 100);
            this.propertyScale.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.propertyScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.propertyScale.Name = "propertyScale";
            this.propertyScale.Size = new System.Drawing.Size(75, 29);
            this.propertyScale.TabIndex = 22;
            this.propertyScale.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // propertyThreshold
            // 
            this.propertyThreshold.Location = new System.Drawing.Point(89, 133);
            this.propertyThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.propertyThreshold.Name = "propertyThreshold";
            this.propertyThreshold.Size = new System.Drawing.Size(75, 29);
            this.propertyThreshold.TabIndex = 20;
            this.propertyThreshold.Value = new decimal(new int[] {
            110,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 24);
            this.label1.TabIndex = 16;
            this.label1.Text = "Width [%]";
            // 
            // propertyWidth
            // 
            this.propertyWidth.Location = new System.Drawing.Point(89, 35);
            this.propertyWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.propertyWidth.Name = "propertyWidth";
            this.propertyWidth.Size = new System.Drawing.Size(75, 29);
            this.propertyWidth.TabIndex = 18;
            this.propertyWidth.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 24);
            this.label2.TabIndex = 15;
            this.label2.Text = "Height [%]";
            // 
            // propertyHeight
            // 
            this.propertyHeight.Location = new System.Drawing.Point(89, 67);
            this.propertyHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.propertyHeight.Name = "propertyHeight";
            this.propertyHeight.Size = new System.Drawing.Size(75, 29);
            this.propertyHeight.TabIndex = 17;
            this.propertyHeight.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1274, 940);
            this.Controls.Add(this.layoutMain);
            this.Controls.Add(this._MainForm_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._MainForm_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._MainForm_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._MainForm_UltraFormManager_Dock_Area_Bottom);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainForm";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.layoutTargetDevice.ResumeLayout(false);
            this.layoutTargetDevice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownExternalExpose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.layoutMiddle.ResumeLayout(false);
            this.layoutMiddle.PerformLayout();
            this.layoutResult.ResumeLayout(false);
            this.layoutResult.PerformLayout();
            this.imageContainer.Panel1.ResumeLayout(false);
            this.imageContainer.Panel1.PerformLayout();
            this.imageContainer.Panel2.ResumeLayout(false);
            this.imageContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageContainer)).EndInit();
            this.imageContainer.ResumeLayout(false);
            this.layoutBottom.ResumeLayout(false);
            this.tabControlCalibrationType.ResumeLayout(false);
            this.tabPageContant.ResumeLayout(false);
            this.tabPageContant.PerformLayout();
            this.tabPageJig.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRow)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPageRuler.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.regionNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyPartial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertyHeight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MainForm_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MainForm_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MainForm_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _MainForm_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Label labelTargetDevice;
        private System.Windows.Forms.TableLayoutPanel layoutTargetDevice;
        private System.Windows.Forms.Button buttonGrabMulti;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonLightOff;
        private System.Windows.Forms.Button buttonGrab;
        private System.Windows.Forms.Label labelCamera;
        private System.Windows.Forms.Button buttonLightOn;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.NumericUpDown numericUpDownExternalExpose;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBoxCamera;
        private System.Windows.Forms.RadioButton radioDeviceLightType;
        private System.Windows.Forms.RadioButton radioDeviceLightValue;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.ComboBox comboLightType;
        private System.Windows.Forms.TableLayoutPanel layoutBottom;
        private System.Windows.Forms.Button buttonSaveCalibration;
        private System.Windows.Forms.Button buttonLoadCalibration;
        private System.Windows.Forms.TabControl tabControlCalibrationType;
        private System.Windows.Forms.TabPage tabPageContant;
        private System.Windows.Forms.TextBox pelHeight;
        private System.Windows.Forms.TextBox pelWidth;
        private System.Windows.Forms.Label labelScaleY;
        private System.Windows.Forms.Label labelScaleX;
        private System.Windows.Forms.TabPage tabPageJig;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label labelNumRow;
        private System.Windows.Forms.Label labelNumCol;
        private System.Windows.Forms.TextBox colSpace;
        private System.Windows.Forms.Label labelRowSpace;
        private System.Windows.Forms.NumericUpDown numCol;
        private System.Windows.Forms.Label labelColSpace;
        private System.Windows.Forms.TextBox rowSpace;
        private System.Windows.Forms.NumericUpDown numRow;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton calibrationTypeGrid;
        private System.Windows.Forms.RadioButton calibrationTypeChessboard;
        private System.Windows.Forms.TabPage tabPageRuler;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown regionNum;
        private System.Windows.Forms.CheckBox checkThreshold;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.NumericUpDown propertyPartial;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown propertyScale;
        private System.Windows.Forms.NumericUpDown propertyThreshold;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown propertyWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown propertyHeight;
        private System.Windows.Forms.Button buttonCalibrate;
        private System.Windows.Forms.TableLayoutPanel layoutMiddle;
        private System.Windows.Forms.Label Value;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.TableLayoutPanel layoutResult;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SplitContainer imageContainer;
        private System.Windows.Forms.Panel panelHistogram;
        private System.Windows.Forms.Panel panelDrawBox;
        private System.Windows.Forms.Label label14;
    }
}

