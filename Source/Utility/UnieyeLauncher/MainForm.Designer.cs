namespace UnieyeLauncher
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.autoPatchCheckBox = new System.Windows.Forms.CheckBox();
            this.textBoxPatchPath = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.patchFilePathLabel = new Infragistics.Win.Misc.UltraLabel();
            this.listBoxPatchFiles = new System.Windows.Forms.ListBox();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.textBoxPatchFile = new System.Windows.Forms.TextBox();
            this.buttonPatchFileAdd = new System.Windows.Forms.Button();
            this.buttonPatchFileDel = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.watchdogTimer = new System.Windows.Forms.Timer(this.components);
            this.initTimer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoPatchCheckBox
            // 
            this.autoPatchCheckBox.AutoSize = true;
            this.autoPatchCheckBox.Location = new System.Drawing.Point(12, 6);
            this.autoPatchCheckBox.Name = "autoPatchCheckBox";
            this.autoPatchCheckBox.Size = new System.Drawing.Size(111, 16);
            this.autoPatchCheckBox.TabIndex = 0;
            this.autoPatchCheckBox.Text = "Use Auto Patch";
            this.autoPatchCheckBox.UseVisualStyleBackColor = true;
            this.autoPatchCheckBox.CheckedChanged += new System.EventHandler(this.autoPatchCheckBox_CheckedChanged);
            // 
            // textBoxPatchPath
            // 
            this.textBoxPatchPath.Enabled = false;
            this.textBoxPatchPath.Location = new System.Drawing.Point(48, 28);
            this.textBoxPatchPath.Name = "textBoxPatchPath";
            this.textBoxPatchPath.Size = new System.Drawing.Size(403, 21);
            this.textBoxPatchPath.TabIndex = 2;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(407, 184);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(83, 31);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Save";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // patchFilePathLabel
            // 
            appearance1.BorderColor2 = System.Drawing.Color.Black;
            this.patchFilePathLabel.Appearance = appearance1;
            this.patchFilePathLabel.AutoSize = true;
            this.patchFilePathLabel.Location = new System.Drawing.Point(12, 33);
            this.patchFilePathLabel.Name = "patchFilePathLabel";
            this.patchFilePathLabel.Size = new System.Drawing.Size(30, 14);
            this.patchFilePathLabel.TabIndex = 6;
            this.patchFilePathLabel.Text = "Path";
            // 
            // listBoxPatchFiles
            // 
            this.listBoxPatchFiles.AllowDrop = true;
            this.listBoxPatchFiles.FormattingEnabled = true;
            this.listBoxPatchFiles.ItemHeight = 12;
            this.listBoxPatchFiles.Location = new System.Drawing.Point(48, 63);
            this.listBoxPatchFiles.Name = "listBoxPatchFiles";
            this.listBoxPatchFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxPatchFiles.Size = new System.Drawing.Size(403, 88);
            this.listBoxPatchFiles.TabIndex = 9;
            this.listBoxPatchFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.listBoxPatchFiles_DragDrop);
            this.listBoxPatchFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.listBoxPatchFiles_DragEnter);
            this.listBoxPatchFiles.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxPatchFiles_KeyDown);
            // 
            // ultraLabel1
            // 
            appearance2.BorderColor2 = System.Drawing.Color.Black;
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(11, 63);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(31, 14);
            this.ultraLabel1.TabIndex = 10;
            this.ultraLabel1.Text = "Files";
            // 
            // textBoxPatchFile
            // 
            this.textBoxPatchFile.Location = new System.Drawing.Point(48, 157);
            this.textBoxPatchFile.Name = "textBoxPatchFile";
            this.textBoxPatchFile.Size = new System.Drawing.Size(403, 21);
            this.textBoxPatchFile.TabIndex = 11;
            // 
            // buttonPatchFileAdd
            // 
            this.buttonPatchFileAdd.Location = new System.Drawing.Point(457, 155);
            this.buttonPatchFileAdd.Name = "buttonPatchFileAdd";
            this.buttonPatchFileAdd.Size = new System.Drawing.Size(33, 23);
            this.buttonPatchFileAdd.TabIndex = 12;
            this.buttonPatchFileAdd.Text = "+";
            this.buttonPatchFileAdd.UseVisualStyleBackColor = true;
            this.buttonPatchFileAdd.Click += new System.EventHandler(this.buttonPatchFileAdd_Click);
            // 
            // buttonPatchFileDel
            // 
            this.buttonPatchFileDel.Location = new System.Drawing.Point(457, 128);
            this.buttonPatchFileDel.Name = "buttonPatchFileDel";
            this.buttonPatchFileDel.Size = new System.Drawing.Size(33, 23);
            this.buttonPatchFileDel.TabIndex = 13;
            this.buttonPatchFileDel.Text = "-";
            this.buttonPatchFileDel.UseVisualStyleBackColor = true;
            this.buttonPatchFileDel.Click += new System.EventHandler(this.buttonPatchFileDel_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "UniEye Launcher";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(113, 48);
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.settingToolStripMenuItem.Text = "Setting";
            this.settingToolStripMenuItem.Click += new System.EventHandler(this.settingToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // watchdogTimer
            // 
            this.watchdogTimer.Interval = 1000;
            this.watchdogTimer.Tick += new System.EventHandler(this.WatchdogTimer_Tick);
            // 
            // initTimer
            // 
            this.initTimer.Interval = 3000;
            this.initTimer.Tick += new System.EventHandler(this.initTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 224);
            this.Controls.Add(this.buttonPatchFileDel);
            this.Controls.Add(this.buttonPatchFileAdd);
            this.Controls.Add(this.textBoxPatchFile);
            this.Controls.Add(this.ultraLabel1);
            this.Controls.Add(this.listBoxPatchFiles);
            this.Controls.Add(this.patchFilePathLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.textBoxPatchPath);
            this.Controls.Add(this.autoPatchCheckBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "UniEye Launcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.VisibleChanged += new System.EventHandler(this.MainForm_VisibleChanged);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox autoPatchCheckBox;
        private System.Windows.Forms.TextBox textBoxPatchPath;
        private System.Windows.Forms.Button cancelButton;
        private Infragistics.Win.Misc.UltraLabel patchFilePathLabel;
        private System.Windows.Forms.ListBox listBoxPatchFiles;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private System.Windows.Forms.TextBox textBoxPatchFile;
        private System.Windows.Forms.Button buttonPatchFileAdd;
        private System.Windows.Forms.Button buttonPatchFileDel;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Timer watchdogTimer;
        private System.Windows.Forms.Timer initTimer;
    }
}