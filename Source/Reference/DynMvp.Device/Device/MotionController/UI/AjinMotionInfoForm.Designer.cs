﻿namespace DynMvp.Device.MotionController.UI
{
    partial class AjinMotionInfoForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.name = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.PciMotionInfoForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.browse = new System.Windows.Forms.Button();
            this.paramFile = new System.Windows.Forms.TextBox();
            this.labelParamFile = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.PciMotionInfoForm_Fill_Panel.ClientArea.SuspendLayout();
            this.PciMotionInfoForm_Fill_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(133, 131);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(83, 32);
            this.buttonCancel.TabIndex = 163;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(47, 131);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(83, 32);
            this.buttonOK.TabIndex = 164;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(110, 8);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(142, 27);
            this.name.TabIndex = 174;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(8, 11);
            this.labelName.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(49, 20);
            this.labelName.TabIndex = 173;
            this.labelName.Text = "Name";
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
            // _ConfigPage_UltraFormManager_Dock_Area_Top
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(262, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 204);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(262, 1);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 173);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(261, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 173);
            // 
            // PciMotionInfoForm_Fill_Panel
            // 
            // 
            // PciMotionInfoForm_Fill_Panel.ClientArea
            // 
            this.PciMotionInfoForm_Fill_Panel.ClientArea.Controls.Add(this.browse);
            this.PciMotionInfoForm_Fill_Panel.ClientArea.Controls.Add(this.paramFile);
            this.PciMotionInfoForm_Fill_Panel.ClientArea.Controls.Add(this.labelParamFile);
            this.PciMotionInfoForm_Fill_Panel.ClientArea.Controls.Add(this.name);
            this.PciMotionInfoForm_Fill_Panel.ClientArea.Controls.Add(this.labelName);
            this.PciMotionInfoForm_Fill_Panel.ClientArea.Controls.Add(this.buttonCancel);
            this.PciMotionInfoForm_Fill_Panel.ClientArea.Controls.Add(this.buttonOK);
            this.PciMotionInfoForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.PciMotionInfoForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PciMotionInfoForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.PciMotionInfoForm_Fill_Panel.Name = "PciMotionInfoForm_Fill_Panel";
            this.PciMotionInfoForm_Fill_Panel.Size = new System.Drawing.Size(260, 173);
            this.PciMotionInfoForm_Fill_Panel.TabIndex = 185;
            // 
            // browse
            // 
            this.browse.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.browse.Location = new System.Drawing.Point(169, 75);
            this.browse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.browse.Name = "browse";
            this.browse.Size = new System.Drawing.Size(83, 32);
            this.browse.TabIndex = 177;
            this.browse.Text = "Browse";
            this.browse.UseVisualStyleBackColor = true;
            this.browse.Click += new System.EventHandler(this.browse_Click);
            // 
            // paramFile
            // 
            this.paramFile.Enabled = false;
            this.paramFile.Location = new System.Drawing.Point(110, 41);
            this.paramFile.Name = "paramFile";
            this.paramFile.Size = new System.Drawing.Size(142, 27);
            this.paramFile.TabIndex = 176;
            // 
            // labelParamFile
            // 
            this.labelParamFile.AutoSize = true;
            this.labelParamFile.Location = new System.Drawing.Point(8, 44);
            this.labelParamFile.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelParamFile.Name = "labelParamFile";
            this.labelParamFile.Size = new System.Drawing.Size(80, 20);
            this.labelParamFile.TabIndex = 175;
            this.labelParamFile.Text = "Param File";
            // 
            // AjinMotionInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(262, 205);
            this.Controls.Add(this.PciMotionInfoForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AjinMotionInfoForm";
            this.Text = "PCI Motion Info";
            this.Load += new System.EventHandler(this.PciMotionInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.PciMotionInfoForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.PciMotionInfoForm_Fill_Panel.ClientArea.PerformLayout();
            this.PciMotionInfoForm_Fill_Panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label labelName;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel PciMotionInfoForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.Button browse;
        private System.Windows.Forms.TextBox paramFile;
        private System.Windows.Forms.Label labelParamFile;
    }
}