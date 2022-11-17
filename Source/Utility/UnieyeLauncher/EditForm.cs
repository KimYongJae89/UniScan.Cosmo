using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnieyeLauncher
{
    public partial class EditForm : Form
    {
        public string patchFilePath = "";
        public bool autoPatchMode = true;

        public EditForm(string patchPath = "", bool autoPatch = true)
        {
            InitializeComponent();
            this.patchFilePath = patchPath;
            this.autoPatchMode = autoPatch;

            this.patchFilePathTextBox.Text = this.patchFilePath;

            this.autoPatchCheckBox.Checked = this.autoPatchMode;
        }

        private void patchFilePathDirButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                patchFilePath = dlg.SelectedPath;
                patchFilePathTextBox.Text = patchFilePath;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            patchFilePath = this.patchFilePathTextBox.Text;

            autoPatchMode = this.autoPatchCheckBox.Checked;

            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
