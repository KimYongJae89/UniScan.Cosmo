namespace UniScanG.Temp
{
    partial class ViewerPanel
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewerPanel));
            this.panelCamera = new System.Windows.Forms.Panel();
            this.panelControl = new System.Windows.Forms.Panel();
            this.toolStripGrab = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripDraw = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelDraw = new System.Windows.Forms.ToolStripLabel();
            this.toolStripMeasure = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonGrabOnce = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGrabContinuous = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDrawLine = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDrawRect = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDrawCircle = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMeasureDist = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMeasureAngle = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMeasureArea = new System.Windows.Forms.ToolStripButton();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelHeaderTitle = new System.Windows.Forms.Label();
            this.panelControl.SuspendLayout();
            this.toolStripGrab.SuspendLayout();
            this.toolStripDraw.SuspendLayout();
            this.toolStripMeasure.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCamera
            // 
            this.panelCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCamera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCamera.Location = new System.Drawing.Point(0, 39);
            this.panelCamera.Name = "panelCamera";
            this.panelCamera.Size = new System.Drawing.Size(384, 390);
            this.panelCamera.TabIndex = 0;
            // 
            // panelControl
            // 
            this.panelControl.AutoSize = true;
            this.panelControl.Controls.Add(this.toolStripMeasure);
            this.panelControl.Controls.Add(this.toolStripDraw);
            this.panelControl.Controls.Add(this.toolStripGrab);
            this.panelControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl.Location = new System.Drawing.Point(0, 429);
            this.panelControl.Name = "panelControl";
            this.panelControl.Size = new System.Drawing.Size(384, 114);
            this.panelControl.TabIndex = 1;
            // 
            // toolStripGrab
            // 
            this.toolStripGrab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripButtonGrabOnce,
            this.toolStripButtonGrabContinuous});
            this.toolStripGrab.Location = new System.Drawing.Point(0, 0);
            this.toolStripGrab.Name = "toolStripGrab";
            this.toolStripGrab.Size = new System.Drawing.Size(384, 38);
            this.toolStripGrab.TabIndex = 0;
            this.toolStripGrab.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(32, 35);
            this.toolStripLabel1.Text = "Grab";
            // 
            // toolStripDraw
            // 
            this.toolStripDraw.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelDraw,
            this.toolStripButtonDrawLine,
            this.toolStripButtonDrawRect,
            this.toolStripButtonDrawCircle});
            this.toolStripDraw.Location = new System.Drawing.Point(0, 38);
            this.toolStripDraw.Name = "toolStripDraw";
            this.toolStripDraw.Size = new System.Drawing.Size(384, 38);
            this.toolStripDraw.TabIndex = 1;
            this.toolStripDraw.Text = "toolStrip1";
            // 
            // toolStripLabelDraw
            // 
            this.toolStripLabelDraw.Name = "toolStripLabelDraw";
            this.toolStripLabelDraw.Size = new System.Drawing.Size(35, 35);
            this.toolStripLabelDraw.Text = "Draw";
            // 
            // toolStripMeasure
            // 
            this.toolStripMeasure.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.toolStripButtonMeasureDist,
            this.toolStripButtonMeasureAngle,
            this.toolStripButtonMeasureArea});
            this.toolStripMeasure.Location = new System.Drawing.Point(0, 76);
            this.toolStripMeasure.Name = "toolStripMeasure";
            this.toolStripMeasure.Size = new System.Drawing.Size(384, 38);
            this.toolStripMeasure.TabIndex = 2;
            this.toolStripMeasure.Text = "toolStrip2";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(52, 35);
            this.toolStripLabel2.Text = "Measure";
            // 
            // toolStripButtonGrabOnce
            // 
            this.toolStripButtonGrabOnce.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGrabOnce.Image")));
            this.toolStripButtonGrabOnce.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonGrabOnce.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGrabOnce.Name = "toolStripButtonGrabOnce";
            this.toolStripButtonGrabOnce.Size = new System.Drawing.Size(39, 35);
            this.toolStripButtonGrabOnce.Text = "Once";
            this.toolStripButtonGrabOnce.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButtonGrabContinuous
            // 
            this.toolStripButtonGrabContinuous.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGrabContinuous.Image")));
            this.toolStripButtonGrabContinuous.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonGrabContinuous.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGrabContinuous.Name = "toolStripButtonGrabContinuous";
            this.toolStripButtonGrabContinuous.Size = new System.Drawing.Size(73, 35);
            this.toolStripButtonGrabContinuous.Text = "Continuous";
            this.toolStripButtonGrabContinuous.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButtonDrawLine
            // 
            this.toolStripButtonDrawLine.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawLine.Image")));
            this.toolStripButtonDrawLine.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonDrawLine.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDrawLine.Name = "toolStripButtonDrawLine";
            this.toolStripButtonDrawLine.Size = new System.Drawing.Size(33, 35);
            this.toolStripButtonDrawLine.Text = "Line";
            this.toolStripButtonDrawLine.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButtonDrawRect
            // 
            this.toolStripButtonDrawRect.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawRect.Image")));
            this.toolStripButtonDrawRect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonDrawRect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDrawRect.Name = "toolStripButtonDrawRect";
            this.toolStripButtonDrawRect.Size = new System.Drawing.Size(34, 35);
            this.toolStripButtonDrawRect.Text = "Rect";
            this.toolStripButtonDrawRect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButtonDrawCircle
            // 
            this.toolStripButtonDrawCircle.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawCircle.Image")));
            this.toolStripButtonDrawCircle.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonDrawCircle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDrawCircle.Name = "toolStripButtonDrawCircle";
            this.toolStripButtonDrawCircle.Size = new System.Drawing.Size(41, 35);
            this.toolStripButtonDrawCircle.Text = "Circle";
            this.toolStripButtonDrawCircle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButtonMeasureDist
            // 
            this.toolStripButtonMeasureDist.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMeasureDist.Image")));
            this.toolStripButtonMeasureDist.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonMeasureDist.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMeasureDist.Name = "toolStripButtonMeasureDist";
            this.toolStripButtonMeasureDist.Size = new System.Drawing.Size(57, 35);
            this.toolStripButtonMeasureDist.Text = "Distance";
            this.toolStripButtonMeasureDist.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButtonMeasureAngle
            // 
            this.toolStripButtonMeasureAngle.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMeasureAngle.Image")));
            this.toolStripButtonMeasureAngle.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonMeasureAngle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMeasureAngle.Name = "toolStripButtonMeasureAngle";
            this.toolStripButtonMeasureAngle.Size = new System.Drawing.Size(42, 35);
            this.toolStripButtonMeasureAngle.Text = "Angle";
            this.toolStripButtonMeasureAngle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButtonMeasureArea
            // 
            this.toolStripButtonMeasureArea.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonMeasureArea.Image")));
            this.toolStripButtonMeasureArea.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonMeasureArea.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMeasureArea.Name = "toolStripButtonMeasureArea";
            this.toolStripButtonMeasureArea.Size = new System.Drawing.Size(35, 35);
            this.toolStripButtonMeasureArea.Text = "Area";
            this.toolStripButtonMeasureArea.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // panelHeader
            // 
            this.panelHeader.AutoSize = true;
            this.panelHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelHeader.Controls.Add(this.labelHeaderTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(384, 39);
            this.panelHeader.TabIndex = 2;
            // 
            // labelHeaderTitle
            // 
            this.labelHeaderTitle.AutoSize = true;
            this.labelHeaderTitle.Font = new System.Drawing.Font("맑은 고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelHeaderTitle.Location = new System.Drawing.Point(0, 0);
            this.labelHeaderTitle.Name = "labelHeaderTitle";
            this.labelHeaderTitle.Size = new System.Drawing.Size(173, 37);
            this.labelHeaderTitle.TabIndex = 0;
            this.labelHeaderTitle.Text = "CAMERA ##";
            // 
            // ViewerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelCamera);
            this.Controls.Add(this.panelControl);
            this.Controls.Add(this.panelHeader);
            this.Name = "ViewerPanel";
            this.Size = new System.Drawing.Size(384, 543);
            this.panelControl.ResumeLayout(false);
            this.panelControl.PerformLayout();
            this.toolStripGrab.ResumeLayout(false);
            this.toolStripGrab.PerformLayout();
            this.toolStripDraw.ResumeLayout(false);
            this.toolStripDraw.PerformLayout();
            this.toolStripMeasure.ResumeLayout(false);
            this.toolStripMeasure.PerformLayout();
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelCamera;
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.ToolStrip toolStripMeasure;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton toolStripButtonMeasureDist;
        private System.Windows.Forms.ToolStripButton toolStripButtonMeasureAngle;
        private System.Windows.Forms.ToolStripButton toolStripButtonMeasureArea;
        private System.Windows.Forms.ToolStrip toolStripDraw;
        private System.Windows.Forms.ToolStripLabel toolStripLabelDraw;
        private System.Windows.Forms.ToolStripButton toolStripButtonDrawLine;
        private System.Windows.Forms.ToolStripButton toolStripButtonDrawRect;
        private System.Windows.Forms.ToolStripButton toolStripButtonDrawCircle;
        private System.Windows.Forms.ToolStrip toolStripGrab;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrabContinuous;
        private System.Windows.Forms.ToolStripButton toolStripButtonGrabOnce;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelHeaderTitle;
    }
}
