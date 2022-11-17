﻿namespace UniScanG.UI.Etc
{
    partial class ProgressForm
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.startTimer = new System.Windows.Forms.Timer(this.components);
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.layoutButton = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.labelMessage = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.layoutMain.SuspendLayout();
            this.layoutButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // startTimer
            // 
            this.startTimer.Interval = 1000;
            this.startTimer.Tick += new System.EventHandler(this.startTimer_Tick);
            // 
            // ultraFormManager
            // 
            this.ultraFormManager.Form = this;
            appearance2.TextHAlignAsString = "Left";
            this.ultraFormManager.FormStyleSettings.CaptionAreaAppearance = appearance2;
            appearance3.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.Appearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.White;
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.HotTrackAppearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(168)))), ((int)(((byte)(12)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.PressedAppearance = appearance5;
            this.ultraFormManager.FormStyleSettings.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.Standard;
            this.ultraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2013;
            this.ultraFormManager.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Top
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(397, 0);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 221);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(397, 0);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(0, 221);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(397, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(0, 221);
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.labelTitle, 0, 0);
            this.layoutMain.Controls.Add(this.layoutButton, 0, 3);
            this.layoutMain.Controls.Add(this.labelMessage, 0, 1);
            this.layoutMain.Controls.Add(this.progressBar, 0, 2);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 4;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutMain.Size = new System.Drawing.Size(397, 221);
            this.layoutMain.TabIndex = 191;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("Malgun Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(397, 75);
            this.labelTitle.TabIndex = 181;
            this.labelTitle.Text = "Title";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutButton
            // 
            this.layoutButton.ColumnCount = 3;
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutButton.Controls.Add(this.btnCancel, 1, 0);
            this.layoutButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutButton.Location = new System.Drawing.Point(0, 171);
            this.layoutButton.Margin = new System.Windows.Forms.Padding(0);
            this.layoutButton.Name = "layoutButton";
            this.layoutButton.RowCount = 1;
            this.layoutButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutButton.Size = new System.Drawing.Size(397, 50);
            this.layoutButton.TabIndex = 178;
            // 
            // btnCancel
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            this.btnCancel.Appearance = appearance1;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Location = new System.Drawing.Point(128, 5);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(140, 40);
            this.btnCancel.TabIndex = 179;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMessage.Location = new System.Drawing.Point(0, 75);
            this.labelMessage.Margin = new System.Windows.Forms.Padding(0);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(397, 50);
            this.labelMessage.TabIndex = 177;
            this.labelMessage.Text = "Message";
            this.labelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar.Location = new System.Drawing.Point(10, 125);
            this.progressBar.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(377, 46);
            this.progressBar.TabIndex = 0;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(397, 221);
            this.ControlBox = false;
            this.Controls.Add(this.layoutMain);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "ProgressForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ProgressForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.layoutButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer startTimer;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TableLayoutPanel layoutButton;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private System.Windows.Forms.Label labelMessage;
        private System.Windows.Forms.ProgressBar progressBar;
    }
}