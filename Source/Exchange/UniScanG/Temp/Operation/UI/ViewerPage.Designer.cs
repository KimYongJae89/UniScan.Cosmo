//using System;

//namespace UniScanG.Temp
//{
//    partial class ViewerPage
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
//            this.splitContainer = new System.Windows.Forms.SplitContainer();
//            this.panelImageTail = new System.Windows.Forms.Panel();
//            this.panelImageBody = new System.Windows.Forms.Panel();
//            this.panelImageHeader = new System.Windows.Forms.Panel();
//            this.tabControlCommand = new System.Windows.Forms.TabControl();
//            this.tabPageMeasure = new System.Windows.Forms.TabPage();
//            this.tabPageProperty = new System.Windows.Forms.TabPage();
//            this.groupBox2 = new System.Windows.Forms.GroupBox();
//            this.propertyGridMachine = new System.Windows.Forms.PropertyGrid();
//            this.groupBox1 = new System.Windows.Forms.GroupBox();
//            this.propertyGridVision = new System.Windows.Forms.PropertyGrid();
//            this.labelVisionCamera = new System.Windows.Forms.Label();
//            this.label4 = new System.Windows.Forms.Label();
//            this.labelVisionState = new System.Windows.Forms.Label();
//            this.label1 = new System.Windows.Forms.Label();
//            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
//            this.splitContainer.Panel1.SuspendLayout();
//            this.splitContainer.Panel2.SuspendLayout();
//            this.splitContainer.SuspendLayout();
//            this.tabControlCommand.SuspendLayout();
//            this.tabPageProperty.SuspendLayout();
//            this.groupBox2.SuspendLayout();
//            this.groupBox1.SuspendLayout();
//            this.SuspendLayout();
//            // 
//            // splitContainer
//            // 
//            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.splitContainer.Location = new System.Drawing.Point(3, 3);
//            this.splitContainer.Name = "splitContainer";
//            // 
//            // splitContainer.Panel1
//            // 
//            this.splitContainer.Panel1.Controls.Add(this.panelImageTail);
//            this.splitContainer.Panel1.Controls.Add(this.panelImageBody);
//            this.splitContainer.Panel1.Controls.Add(this.panelImageHeader);
//            // 
//            // splitContainer.Panel2
//            // 
//            this.splitContainer.Panel2.Controls.Add(this.tabControlCommand);
//            this.splitContainer.Size = new System.Drawing.Size(1427, 648);
//            this.splitContainer.SplitterDistance = 904;
//            this.splitContainer.TabIndex = 64;
//            // 
//            // panelImageTail
//            // 
//            this.panelImageTail.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.panelImageTail.Location = new System.Drawing.Point(0, 581);
//            this.panelImageTail.Name = "panelImageTail";
//            this.panelImageTail.Size = new System.Drawing.Size(904, 67);
//            this.panelImageTail.TabIndex = 1;
//            // 
//            // panelImageBody
//            // 
//            this.panelImageBody.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.panelImageBody.Location = new System.Drawing.Point(0, 69);
//            this.panelImageBody.Name = "panelImageBody";
//            this.panelImageBody.Size = new System.Drawing.Size(904, 579);
//            this.panelImageBody.TabIndex = 1;
//            // 
//            // panelImageHeader
//            // 
//            this.panelImageHeader.Dock = System.Windows.Forms.DockStyle.Top;
//            this.panelImageHeader.Location = new System.Drawing.Point(0, 0);
//            this.panelImageHeader.Name = "panelImageHeader";
//            this.panelImageHeader.Size = new System.Drawing.Size(904, 69);
//            this.panelImageHeader.TabIndex = 0;
//            // 
//            // tabControlCommand
//            // 
//            this.tabControlCommand.Alignment = System.Windows.Forms.TabAlignment.Bottom;
//            this.tabControlCommand.Controls.Add(this.tabPageMeasure);
//            this.tabControlCommand.Controls.Add(this.tabPageProperty);
//            this.tabControlCommand.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tabControlCommand.Location = new System.Drawing.Point(0, 0);
//            this.tabControlCommand.Name = "tabControlCommand";
//            this.tabControlCommand.SelectedIndex = 0;
//            this.tabControlCommand.Size = new System.Drawing.Size(519, 648);
//            this.tabControlCommand.TabIndex = 0;
//            // 
//            // tabPageMeasure
//            // 
//            this.tabPageMeasure.Location = new System.Drawing.Point(4, 4);
//            this.tabPageMeasure.Name = "tabPageMeasure";
//            this.tabPageMeasure.Size = new System.Drawing.Size(511, 615);
//            this.tabPageMeasure.TabIndex = 2;
//            this.tabPageMeasure.Text = "Measure";
//            this.tabPageMeasure.UseVisualStyleBackColor = true;
//            // 
//            // tabPageProperty
//            // 
//            this.tabPageProperty.Controls.Add(this.groupBox2);
//            this.tabPageProperty.Controls.Add(this.groupBox1);
//            this.tabPageProperty.Location = new System.Drawing.Point(4, 4);
//            this.tabPageProperty.Name = "tabPageProperty";
//            this.tabPageProperty.Padding = new System.Windows.Forms.Padding(3);
//            this.tabPageProperty.Size = new System.Drawing.Size(511, 615);
//            this.tabPageProperty.TabIndex = 0;
//            this.tabPageProperty.Text = "Property";
//            this.tabPageProperty.UseVisualStyleBackColor = true;
//            // 
//            // groupBox2
//            // 
//            this.groupBox2.Controls.Add(this.propertyGridMachine);
//            this.groupBox2.Location = new System.Drawing.Point(6, 357);
//            this.groupBox2.Name = "groupBox2";
//            this.groupBox2.Size = new System.Drawing.Size(499, 252);
//            this.groupBox2.TabIndex = 1;
//            this.groupBox2.TabStop = false;
//            this.groupBox2.Text = "Machine";
//            // 
//            // propertyGridMachine
//            // 
//            this.propertyGridMachine.CommandsVisibleIfAvailable = false;
//            this.propertyGridMachine.HelpVisible = false;
//            this.propertyGridMachine.LineColor = System.Drawing.SystemColors.ControlDark;
//            this.propertyGridMachine.Location = new System.Drawing.Point(15, 26);
//            this.propertyGridMachine.Name = "propertyGridMachine";
//            this.propertyGridMachine.PropertySort = System.Windows.Forms.PropertySort.NoSort;
//            this.propertyGridMachine.Size = new System.Drawing.Size(467, 213);
//            this.propertyGridMachine.TabIndex = 9;
//            this.propertyGridMachine.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridMachine_PropertyValueChanged);
//            // 
//            // groupBox1
//            // 
//            this.groupBox1.Controls.Add(this.propertyGridVision);
//            this.groupBox1.Controls.Add(this.labelVisionCamera);
//            this.groupBox1.Controls.Add(this.label4);
//            this.groupBox1.Controls.Add(this.labelVisionState);
//            this.groupBox1.Controls.Add(this.label1);
//            this.groupBox1.Location = new System.Drawing.Point(6, 6);
//            this.groupBox1.Name = "groupBox1";
//            this.groupBox1.Size = new System.Drawing.Size(499, 334);
//            this.groupBox1.TabIndex = 0;
//            this.groupBox1.TabStop = false;
//            this.groupBox1.Text = "Vision";
//            // 
//            // propertyGridVision
//            // 
//            this.propertyGridVision.CommandsVisibleIfAvailable = false;
//            this.propertyGridVision.HelpVisible = false;
//            this.propertyGridVision.LineColor = System.Drawing.SystemColors.ControlDark;
//            this.propertyGridVision.Location = new System.Drawing.Point(15, 48);
//            this.propertyGridVision.Name = "propertyGridVision";
//            this.propertyGridVision.PropertySort = System.Windows.Forms.PropertySort.NoSort;
//            this.propertyGridVision.Size = new System.Drawing.Size(467, 276);
//            this.propertyGridVision.TabIndex = 9;
//            this.propertyGridVision.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridVision_PropertyValueChanged);
//            // 
//            // labelVisionCamera
//            // 
//            this.labelVisionCamera.AutoSize = true;
//            this.labelVisionCamera.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.labelVisionCamera.Location = new System.Drawing.Point(90, 23);
//            this.labelVisionCamera.Name = "labelVisionCamera";
//            this.labelVisionCamera.Size = new System.Drawing.Size(131, 21);
//            this.labelVisionCamera.TabIndex = 8;
//            this.labelVisionCamera.Text = "MODELNAME___";
//            // 
//            // label4
//            // 
//            this.label4.AutoSize = true;
//            this.label4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.label4.Location = new System.Drawing.Point(11, 23);
//            this.label4.Name = "label4";
//            this.label4.Size = new System.Drawing.Size(68, 21);
//            this.label4.TabIndex = 7;
//            this.label4.Text = "Camera";
//            // 
//            // labelVisionState
//            // 
//            this.labelVisionState.AutoSize = true;
//            this.labelVisionState.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.labelVisionState.Location = new System.Drawing.Point(385, 24);
//            this.labelVisionState.Name = "labelVisionState";
//            this.labelVisionState.Size = new System.Drawing.Size(84, 21);
//            this.labelVisionState.TabIndex = 6;
//            this.labelVisionState.Text = "GOOD/NC";
//            // 
//            // label1
//            // 
//            this.label1.AutoSize = true;
//            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.label1.Location = new System.Drawing.Point(335, 24);
//            this.label1.Name = "label1";
//            this.label1.Size = new System.Drawing.Size(49, 21);
//            this.label1.TabIndex = 5;
//            this.label1.Text = "State";
//            // 
//            // ViewerPage
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.splitContainer);
//            this.DoubleBuffered = true;
//            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
//            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
//            this.Name = "ViewerPage";
//            this.Padding = new System.Windows.Forms.Padding(3);
//            this.Size = new System.Drawing.Size(1433, 654);
//            this.VisibleChanged += new System.EventHandler(this.ViewerPage_VisibleChanged);
//            this.splitContainer.Panel1.ResumeLayout(false);
//            this.splitContainer.Panel2.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
//            this.splitContainer.ResumeLayout(false);
//            this.tabControlCommand.ResumeLayout(false);
//            this.tabPageProperty.ResumeLayout(false);
//            this.groupBox2.ResumeLayout(false);
//            this.groupBox1.ResumeLayout(false);
//            this.groupBox1.PerformLayout();
//            this.ResumeLayout(false);

//        }

//        #endregion
//        private System.Windows.Forms.SplitContainer splitContainer;
//        private System.Windows.Forms.Panel panelImageTail;
//        private System.Windows.Forms.Panel panelImageBody;
//        private System.Windows.Forms.Panel panelImageHeader;
//        private System.Windows.Forms.TabControl tabControlCommand;
//        private System.Windows.Forms.TabPage tabPageMeasure;
//        private System.Windows.Forms.TabPage tabPageProperty;
//        private System.Windows.Forms.GroupBox groupBox2;
//        private System.Windows.Forms.PropertyGrid propertyGridMachine;
//        private System.Windows.Forms.GroupBox groupBox1;
//        private System.Windows.Forms.PropertyGrid propertyGridVision;
//        private System.Windows.Forms.Label labelVisionCamera;
//        private System.Windows.Forms.Label label4;
//        private System.Windows.Forms.Label labelVisionState;
//        private System.Windows.Forms.Label label1;
//    }
//}
