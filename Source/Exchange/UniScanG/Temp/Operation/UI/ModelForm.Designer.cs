//namespace UniScanG.Temp
//{
//    partial class ModelForm
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

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
//            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelForm));
//            this.btnCancel = new System.Windows.Forms.Button();
//            this.btnOK = new System.Windows.Forms.Button();
//            this.labelScreenModel = new System.Windows.Forms.Label();
//            this.screenModel = new System.Windows.Forms.TextBox();
//            this.labelDescription = new System.Windows.Forms.Label();
//            this.description = new System.Windows.Forms.TextBox();
//            this.mainPanel = new System.Windows.Forms.Panel();
//            this.panelDefault = new System.Windows.Forms.Panel();
//            this.groupBox2 = new System.Windows.Forms.GroupBox();
//            this.registrationDate = new System.Windows.Forms.DateTimePicker();
//            this.labelRegistrationDate = new System.Windows.Forms.Label();
//            this.registrant = new System.Windows.Forms.TextBox();
//            this.labelRegistrant = new System.Windows.Forms.Label();
//            this.groupBox1 = new System.Windows.Forms.GroupBox();
//            this.paste = new System.Windows.Forms.TextBox();
//            this.labelPaste = new System.Windows.Forms.Label();
//            this.thickness = new System.Windows.Forms.TextBox();
//            this.labelThickness = new System.Windows.Forms.Label();
//            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
//            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
//            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
//            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
//            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
//            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
//            this.mainPanel.SuspendLayout();
//            this.panelDefault.SuspendLayout();
//            this.groupBox2.SuspendLayout();
//            this.groupBox1.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // btnCancel
//            // 
//            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
//            this.btnCancel.BackColor = System.Drawing.SystemColors.ButtonFace;
//            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
//            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.btnCancel.Location = new System.Drawing.Point(194, 401);
//            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Size = new System.Drawing.Size(107, 38);
//            this.btnCancel.TabIndex = 13;
//            this.btnCancel.Text = "Cancel";
//            this.btnCancel.UseVisualStyleBackColor = false;
//            // 
//            // btnOK
//            // 
//            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
//            this.btnOK.BackColor = System.Drawing.SystemColors.ButtonFace;
//            this.btnOK.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.btnOK.Location = new System.Drawing.Point(79, 402);
//            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
//            this.btnOK.Name = "btnOK";
//            this.btnOK.Size = new System.Drawing.Size(107, 37);
//            this.btnOK.TabIndex = 12;
//            this.btnOK.Text = "OK";
//            this.btnOK.UseVisualStyleBackColor = false;
//            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
//            // 
//            // labelScreenModel
//            // 
//            this.labelScreenModel.BackColor = System.Drawing.Color.Navy;
//            this.labelScreenModel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.labelScreenModel.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.labelScreenModel.ForeColor = System.Drawing.Color.White;
//            this.labelScreenModel.Location = new System.Drawing.Point(10, 27);
//            this.labelScreenModel.Margin = new System.Windows.Forms.Padding(0);
//            this.labelScreenModel.Name = "labelScreenModel";
//            this.labelScreenModel.Size = new System.Drawing.Size(164, 27);
//            this.labelScreenModel.TabIndex = 0;
//            this.labelScreenModel.Text = "Screen Model";
//            this.labelScreenModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // screenModel
//            // 
//            this.screenModel.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
//            this.screenModel.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.screenModel.Location = new System.Drawing.Point(178, 27);
//            this.screenModel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
//            this.screenModel.Name = "screenModel";
//            this.screenModel.Size = new System.Drawing.Size(161, 27);
//            this.screenModel.TabIndex = 1;
//            this.screenModel.Validating += new System.ComponentModel.CancelEventHandler(this.screenModel_Validating);
//            // 
//            // labelDescription
//            // 
//            this.labelDescription.BackColor = System.Drawing.Color.Navy;
//            this.labelDescription.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.labelDescription.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.labelDescription.ForeColor = System.Drawing.Color.White;
//            this.labelDescription.Location = new System.Drawing.Point(8, 65);
//            this.labelDescription.Margin = new System.Windows.Forms.Padding(0);
//            this.labelDescription.Name = "labelDescription";
//            this.labelDescription.Size = new System.Drawing.Size(164, 27);
//            this.labelDescription.TabIndex = 6;
//            this.labelDescription.Text = "Description";
//            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // description
//            // 
//            this.description.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.description.Location = new System.Drawing.Point(176, 65);
//            this.description.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
//            this.description.Multiline = true;
//            this.description.Name = "description";
//            this.description.Size = new System.Drawing.Size(161, 150);
//            this.description.TabIndex = 7;
//            // 
//            // mainPanel
//            // 
//            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
//            this.mainPanel.Controls.Add(this.panelDefault);
//            this.mainPanel.Controls.Add(this.btnOK);
//            this.mainPanel.Controls.Add(this.btnCancel);
//            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.mainPanel.Location = new System.Drawing.Point(1, 31);
//            this.mainPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
//            this.mainPanel.Name = "mainPanel";
//            this.mainPanel.Size = new System.Drawing.Size(391, 447);
//            this.mainPanel.TabIndex = 1;
//            // 
//            // panelDefault
//            // 
//            this.panelDefault.Controls.Add(this.groupBox2);
//            this.panelDefault.Controls.Add(this.groupBox1);
//            this.panelDefault.Dock = System.Windows.Forms.DockStyle.Top;
//            this.panelDefault.Location = new System.Drawing.Point(0, 0);
//            this.panelDefault.Name = "panelDefault";
//            this.panelDefault.Size = new System.Drawing.Size(389, 394);
//            this.panelDefault.TabIndex = 17;
//            // 
//            // groupBox2
//            // 
//            this.groupBox2.Controls.Add(this.registrationDate);
//            this.groupBox2.Controls.Add(this.labelRegistrationDate);
//            this.groupBox2.Controls.Add(this.labelDescription);
//            this.groupBox2.Controls.Add(this.registrant);
//            this.groupBox2.Controls.Add(this.description);
//            this.groupBox2.Controls.Add(this.labelRegistrant);
//            this.groupBox2.Location = new System.Drawing.Point(6, 156);
//            this.groupBox2.Name = "groupBox2";
//            this.groupBox2.Size = new System.Drawing.Size(376, 223);
//            this.groupBox2.TabIndex = 25;
//            this.groupBox2.TabStop = false;
//            this.groupBox2.Text = "선택 입력 항목";
//            // 
//            // registrationDate
//            // 
//            this.registrationDate.CalendarFont = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.registrationDate.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.registrationDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
//            this.registrationDate.Location = new System.Drawing.Point(8, 163);
//            this.registrationDate.Name = "registrationDate";
//            this.registrationDate.Size = new System.Drawing.Size(164, 27);
//            this.registrationDate.TabIndex = 19;
//            this.registrationDate.Visible = false;
//            // 
//            // labelRegistrationDate
//            // 
//            this.labelRegistrationDate.BackColor = System.Drawing.Color.Navy;
//            this.labelRegistrationDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.labelRegistrationDate.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.labelRegistrationDate.ForeColor = System.Drawing.Color.White;
//            this.labelRegistrationDate.Location = new System.Drawing.Point(8, 133);
//            this.labelRegistrationDate.Margin = new System.Windows.Forms.Padding(0);
//            this.labelRegistrationDate.Name = "labelRegistrationDate";
//            this.labelRegistrationDate.Size = new System.Drawing.Size(164, 27);
//            this.labelRegistrationDate.TabIndex = 18;
//            this.labelRegistrationDate.Text = "Registration Date";
//            this.labelRegistrationDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            this.labelRegistrationDate.Visible = false;
//            // 
//            // registrant
//            // 
//            this.registrant.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
//            this.registrant.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.registrant.Location = new System.Drawing.Point(176, 28);
//            this.registrant.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
//            this.registrant.Name = "registrant";
//            this.registrant.Size = new System.Drawing.Size(161, 27);
//            this.registrant.TabIndex = 17;
//            // 
//            // labelRegistrant
//            // 
//            this.labelRegistrant.BackColor = System.Drawing.Color.Navy;
//            this.labelRegistrant.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.labelRegistrant.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.labelRegistrant.ForeColor = System.Drawing.Color.White;
//            this.labelRegistrant.Location = new System.Drawing.Point(8, 28);
//            this.labelRegistrant.Margin = new System.Windows.Forms.Padding(0);
//            this.labelRegistrant.Name = "labelRegistrant";
//            this.labelRegistrant.Size = new System.Drawing.Size(164, 27);
//            this.labelRegistrant.TabIndex = 0;
//            this.labelRegistrant.Text = "Registrant";
//            this.labelRegistrant.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // groupBox1
//            // 
//            this.groupBox1.Controls.Add(this.paste);
//            this.groupBox1.Controls.Add(this.labelPaste);
//            this.groupBox1.Controls.Add(this.thickness);
//            this.groupBox1.Controls.Add(this.labelThickness);
//            this.groupBox1.Controls.Add(this.labelScreenModel);
//            this.groupBox1.Controls.Add(this.screenModel);
//            this.groupBox1.Location = new System.Drawing.Point(6, 9);
//            this.groupBox1.Name = "groupBox1";
//            this.groupBox1.Size = new System.Drawing.Size(377, 139);
//            this.groupBox1.TabIndex = 24;
//            this.groupBox1.TabStop = false;
//            this.groupBox1.Text = "필수 입력 항목";
//            // 
//            // paste
//            // 
//            this.paste.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
//            this.paste.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
//            this.paste.Location = new System.Drawing.Point(178, 102);
//            this.paste.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
//            this.paste.Name = "paste";
//            this.paste.Size = new System.Drawing.Size(161, 27);
//            this.paste.TabIndex = 23;
//            this.paste.Validating += new System.ComponentModel.CancelEventHandler(this.paste_Validating);
//            // 
//            // labelPaste
//            // 
//            this.labelPaste.BackColor = System.Drawing.Color.Navy;
//            this.labelPaste.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.labelPaste.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.labelPaste.ForeColor = System.Drawing.Color.White;
//            this.labelPaste.Location = new System.Drawing.Point(10, 102);
//            this.labelPaste.Margin = new System.Windows.Forms.Padding(0);
//            this.labelPaste.Name = "labelPaste";
//            this.labelPaste.Size = new System.Drawing.Size(164, 27);
//            this.labelPaste.TabIndex = 22;
//            this.labelPaste.Text = "Paste";
//            this.labelPaste.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // thickness
//            // 
//            this.thickness.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
//            this.thickness.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.thickness.Location = new System.Drawing.Point(178, 63);
//            this.thickness.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
//            this.thickness.Name = "thickness";
//            this.thickness.Size = new System.Drawing.Size(161, 27);
//            this.thickness.TabIndex = 21;
//            this.thickness.Validating += new System.ComponentModel.CancelEventHandler(this.thickness_Validating);
//            // 
//            // labelThickness
//            // 
//            this.labelThickness.BackColor = System.Drawing.Color.Navy;
//            this.labelThickness.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
//            this.labelThickness.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.labelThickness.ForeColor = System.Drawing.Color.White;
//            this.labelThickness.Location = new System.Drawing.Point(10, 63);
//            this.labelThickness.Margin = new System.Windows.Forms.Padding(0);
//            this.labelThickness.Name = "labelThickness";
//            this.labelThickness.Size = new System.Drawing.Size(164, 27);
//            this.labelThickness.TabIndex = 20;
//            this.labelThickness.Text = "Thickness (um)";
//            this.labelThickness.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
//            // 
//            // ultraFormManager
//            // 
//            this.ultraFormManager.Form = this;
//            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
//            appearance1.TextHAlignAsString = "Left";
//            this.ultraFormManager.FormStyleSettings.CaptionAreaAppearance = appearance1;
//            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
//            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
//            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.Appearance = appearance2;
//            appearance3.BackColor = System.Drawing.Color.Transparent;
//            appearance3.ForeColor = System.Drawing.Color.White;
//            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.HotTrackAppearance = appearance3;
//            appearance4.BackColor = System.Drawing.Color.Transparent;
//            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(168)))), ((int)(((byte)(12)))));
//            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.PressedAppearance = appearance4;
//            this.ultraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2013;
//            // 
//            // _ConfigPage_UltraFormManager_Dock_Area_Right
//            // 
//            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
//            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
//            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
//            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
//            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(392, 31);
//            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
//            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 447);
//            // 
//            // _ConfigPage_UltraFormManager_Dock_Area_Left
//            // 
//            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
//            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
//            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
//            this._ConfigPage_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
//            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
//            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
//            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 447);
//            // 
//            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
//            // 
//            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
//            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
//            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
//            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
//            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 478);
//            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
//            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(393, 1);
//            // 
//            // _ConfigPage_UltraFormManager_Dock_Area_Top
//            // 
//            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
//            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
//            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
//            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
//            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
//            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
//            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
//            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(393, 31);
//            // 
//            // errorProvider
//            // 
//            this.errorProvider.ContainerControl = this;
//            // 
//            // ModelForm
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(393, 479);
//            this.Controls.Add(this.mainPanel);
//            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
//            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
//            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
//            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
//            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
//            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
//            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
//            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
//            this.Name = "ModelForm";
//            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
//            this.Text = "Model";
//            this.Load += new System.EventHandler(this.ModelForm_Load);
//            this.mainPanel.ResumeLayout(false);
//            this.panelDefault.ResumeLayout(false);
//            this.groupBox2.ResumeLayout(false);
//            this.groupBox2.PerformLayout();
//            this.groupBox1.ResumeLayout(false);
//            this.groupBox1.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private System.Windows.Forms.Button btnCancel;
//        private System.Windows.Forms.Button btnOK;
//        private System.Windows.Forms.Label labelScreenModel;
//        private System.Windows.Forms.TextBox screenModel;
//        private System.Windows.Forms.Label labelDescription;
//        private System.Windows.Forms.TextBox description;
//        private System.Windows.Forms.Panel mainPanel;
//        private System.Windows.Forms.Label labelRegistrant;
//        private System.Windows.Forms.Panel panelDefault;
//        private System.Windows.Forms.TextBox registrant;
//        private System.Windows.Forms.DateTimePicker registrationDate;
//        private System.Windows.Forms.Label labelRegistrationDate;
//        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
//        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
//        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
//        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
//        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
//        private System.Windows.Forms.ErrorProvider errorProvider;
//        private System.Windows.Forms.Label labelThickness;
//        private System.Windows.Forms.TextBox thickness;
//        private System.Windows.Forms.GroupBox groupBox2;
//        private System.Windows.Forms.GroupBox groupBox1;
//        private System.Windows.Forms.TextBox paste;
//        private System.Windows.Forms.Label labelPaste;
//    }
//}