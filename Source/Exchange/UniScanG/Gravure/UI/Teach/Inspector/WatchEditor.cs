using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.Data;
using DynMvp.UI;
using DynMvp.Data.UI;
using DynMvp.Vision;
using DynMvp.UI.Touch;
using DynMvp.Base;

namespace UniScanG.Gravure.UI.Teach.Inspector
{
    public partial class WatchEditor : UserControl
    {
        CanvasPanel canvasPanel = null;
        List<WatchItem> watchItemList = null;
        List<WatchItem> watchItemListWork = null;
        List<WatchItem> watchItemListDone = null;

        bool onUpdate = false;
        List<WatchItem> selectedWatchItemList = new List<WatchItem>();

        public List<WatchItem> WatchItemListDone { get { return watchItemListDone; } }

        public WatchEditor()
        {
            InitializeComponent();

            this.canvasPanel = new CanvasPanel();
            this.canvasPanel.Dock = DockStyle.Fill;
            this.canvasPanel.DragMode = DragMode.Pan;
            this.canvasPanel.ShowCenterGuide = false;

            this.canvasPanel.Editable = true;
            this.canvasPanel.FigureCreated = canvasPanel_FigureCreated;
            this.canvasPanel.FigureClicked= canvasPanel_FigureClicked;
            this.canvasPanel.KeyDown += canvasPanel_KeyDown;

            this.panelImage.Controls.Add(this.canvasPanel);

            this.toolStripType.ComboBox.DataSource = Enum.GetValues(typeof(WatchType));

        }

        private void canvasPanel_KeyDown(object sender, KeyEventArgs e)
        {
            Point pt = Point.Empty;

            Keys ketData = e.KeyData & ~(Keys.Control | Keys.Shift);
            bool control = (e.KeyData & Keys.Control) > 0;
            bool shift = (e.KeyData & Keys.Shift) > 0;

            if (control)
            {
                int diff = shift ? 1 : 50;
                switch (ketData)
                {
                    case Keys.Left:
                        pt.X = -diff;
                        break;
                    case Keys.Up:
                        pt.Y = -diff;
                        break;
                    case Keys.Right:
                        pt.X = +diff;
                        break;
                    case Keys.Down:
                        pt.Y = +diff;
                        break;
                }
            }

            if (pt.IsEmpty == false)
            {
                selectedWatchItemList.ForEach(f => f.Offset(pt));
                UpdateFigure();
            }
        }

        private void canvasPanel_FigureClicked(Figure figure)
        {
            selectedWatchItemList.Clear();
            WatchItem selWatchItem = figure.Tag as WatchItem;
            if (selWatchItem != null)
                selectedWatchItemList.Add(selWatchItem);

            UpdateData();
        }

        private void UpdateData()
        {
            WatchItem watchItem = selectedWatchItemList.FirstOrDefault();
            if (watchItem == null || selectedWatchItemList.Count != 1)
            {
                this.toolStripId.Text = "";
                this.toolStripName.Text = "";
                this.toolStripType.SelectedItem = null;
            }
            else
            {
                this.toolStripId.Text = watchItem.Index.ToString();
                this.toolStripName.Text = watchItem.Name.ToString();
                this.toolStripType.SelectedItem = watchItem.WatchType;
            }

            this.toolStripLabelSelCount.Text = string.Format(StringManager.GetString(this.GetType().FullName, "{0} Item Selected"), selectedWatchItemList.Count);
        }
        
        private void canvasPanel_FigureCreated(Figure figure, CoordMapper coordMapper, FigureGroup workingFigures, FigureGroup backgroundFigures)
        {
            workingFigures.AddFigure(figure);

            WatchItem watchItem = new WatchItem();
            watchItem.Index = -1;
            watchItem.Name = "";
            watchItem.WatchType = defaultWatchType;
            watchItem.Rectangle = coordMapper.InverseTransform(figure.GetRectangle().ToRectangle());
            watchItem.Rectangle = figure.GetRectangle().ToRectangle();
            figure.Tag = watchItem;

            this.watchItemListWork.Add(watchItem);

            this.canvasPanel.FigureClicked(figure);
            this.canvasPanel.DragMode = DragMode.Pan;

            defaultWatchType = WatchType.NONE;
        }

        public void Initialize(Bitmap bgImage, List<WatchItem> watchItemList)
        {
            this.canvasPanel.UpdateImage(bgImage);

            this.watchItemList = watchItemList;

            this.watchItemListWork = new List<WatchItem>();
            watchItemList.ForEach(f => this.watchItemListWork.Add(f.Clone()));

            UpdateFigure();
        }
        
        private void UpdateFigure()
        {
            this.canvasPanel.WorkingFigures.Clear();

            foreach(WatchItem watchItem in this.watchItemListWork)
            {
                Figure figure = watchItem.GetFigure();
                figure.Tag = watchItem;
                this.canvasPanel.WorkingFigures.AddFigure(figure);
            }
            this.canvasPanel.Invalidate();
        }

        private void RegionEditor_SizeChanged(object sender, EventArgs e)
        {
            this.canvasPanel.ZoomFit();
        }

        private void Apply()
        {
            UpdateIndex();
            UpdateFigure();
            watchItemListDone = new List<WatchItem>();
            watchItemListWork.ForEach(f => watchItemListDone.Add(f.Clone()));
        }

        private void UpdateIndex()
        {
            List<WatchItem> founded = this.watchItemListWork.FindAll(f => f.WatchType == WatchType.NONE);
            founded.ForEach(f => f.Index = -1);

            founded = this.watchItemListWork.FindAll(f => f.WatchType == WatchType.CHIP);
            for (int i = 0; i < founded.Count; i++)
                founded[i].Index = i;

            founded = this.watchItemListWork.FindAll(f => f.WatchType == WatchType.FP);
            for (int i = 0; i < founded.Count; i++)
                founded[i].Index = i;

            founded = this.watchItemListWork.FindAll(f => f.WatchType == WatchType.INDEX);
            for (int i = 0; i < founded.Count; i++)
                founded[i].Index = i;

        }

        private void Reset()
        {
            this.watchItemListWork = new List<WatchItem>();
            watchItemList.ForEach(f => this.watchItemListWork.Add(f.Clone()));
        }

        WatchType defaultWatchType = WatchType.NONE;
        private void toolStripSplitButtonAdd_ButtonClick(object sender, EventArgs e)
        {
            this.canvasPanel.DragMode = DragMode.Add;
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                if (item.Name == toolStripMenuAddChip.Name)
                    defaultWatchType = WatchType.CHIP;
                else if (item.Name == toolStripMenuAddFP.Name)
                    defaultWatchType = WatchType.FP;
                else if (item.Name == toolStripMenuAddIndex.Name)
                    defaultWatchType = WatchType.INDEX;
            }
        }

        private void toolStripButtonRemove_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageForm.Show(null, string.Format(StringManager.GetString("{0} Item(s) will be Remove."), this.selectedWatchItemList.Count), MessageFormType.YesNo);
            if (dialogResult == DialogResult.Cancel)
                return;

            this.selectedWatchItemList.ForEach(f =>
            {
                WatchItem watchItem = f as WatchItem;
                this.watchItemListWork.Remove(watchItem);
            });
            selectedWatchItemList.Clear();

            UpdateFigure();
        }

        private void toolStripButtonAll_Click(object sender, EventArgs e)
        {
            selectedWatchItemList.Clear();
            selectedWatchItemList.AddRange(watchItemListWork);
            UpdateData();
        }

        private void toolStripName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (onUpdate)
                return;

            if (this.selectedWatchItemList.Count != 1)
                return;

            WatchItem watchItem = selectedWatchItemList.FirstOrDefault();
            if (watchItem == null)
                return;

            watchItem.Name = toolStripName.Text;
        }

        private void toolStripType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (onUpdate)
                return;

            if (this.selectedWatchItemList.Count != 1)
                return;

            WatchItem watchItem = selectedWatchItemList.FirstOrDefault();
            if (watchItem == null)
                return;

            WatchType watchType = (WatchType)toolStripType.SelectedItem;
            watchItem.WatchType = watchType;
        }

        private void toolStripButtonReset_Click(object sender, EventArgs e)
        {
            Reset();
            UpdateFigure();
            MessageForm.Show(null, "Reset Success");
        }

        private void toolStripButtonApply_Click(object sender, EventArgs e)
        {
            Apply();
            MessageForm.Show(null, "Apply Success");
        }
    }
}
