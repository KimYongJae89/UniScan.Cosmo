using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Data;

namespace UniScanG.UI.Etc
{
    public partial class ContextInfoForm : Form, IMultiLanguageSupport
    {
        SheetSubResult sheetSubResult = null;

        public ContextInfoForm()
        {
            InitializeComponent();

            StringManager.AddListener(this);
        }

        private void ContextInfoForm_Load(object sender, EventArgs e)
        {
        }

        public void CanvasPanel_MouseClicked(PointF point, ref bool processingCancelled)
        {
            Hide();
        }

        public void CanvasPanel_MouseLeaved()
        {
            Hide();
            //System.Diagnostics.Debug.WriteLine("MouseLeaved-Hide");
        }

        public void CanvasPanel_FigureFocused(Figure figure)
        {
            if (figure == null)
            {
                this.sheetSubResult = null;
                Hide();
            }
            else if (figure.Tag is SheetSubResult)
            {
                this.sheetSubResult = (SheetSubResult)figure.Tag;
                this.Location = Point.Subtract(MousePosition, new Size(-5, -10));

                defectType.Text = StringManager.GetString(this.GetType().FullName, sheetSubResult.GetDefectType().ToString());
                infoText.Text = sheetSubResult.ToString();
                image.Image = sheetSubResult.Image;
                Show();
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void image_Click(object sender, EventArgs e)
        {
            if (image.Image == sheetSubResult.BufImage)
                image.Image = sheetSubResult.Image;
            else
                image.Image = sheetSubResult.BufImage;

        }
    }
}
