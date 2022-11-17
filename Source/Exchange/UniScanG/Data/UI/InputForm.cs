using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.UI.Touch;
using DynMvp.Base;
using System.Text.RegularExpressions;

namespace UniScanG.Data.UI
{
    public delegate bool ValidCheckFunc(string inputValue);
    public partial class InputForm : Form
    {
        string inputText;
        public string InputText
        {
            get { return inputText; }
        }

        string nowString;
        public string NowString
        {
            get { return nowString; }
            set { nowString = value; }
        }

        ValidCheckFunc validCheckFunc;
        public ValidCheckFunc ValidCheckFunc
        {
            get { return validCheckFunc; }
            set { validCheckFunc = value; }
        }
        
        public InputForm(string lableString, string nowString="")
        {
            InitializeComponent();

            InitializeText(lableString, nowString);

            //btnOk.DialogResult = DialogResult.OK;
            //btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void InitializeText(string labelString, string nowString = "")
        {
            btnOk.Text = StringManager.GetString(this.GetType().FullName, btnOk.Text);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName, btnCancel.Text);

            if (string.IsNullOrEmpty(labelString))
                labelTitle.Text = StringManager.GetString(this.GetType().FullName, labelTitle.Text);
            else
                labelTitle.Text = labelString;

            inputTextBox.Text = nowString;
            //if (InputForm.ActiveForm != null)
            //    InputForm.ActiveForm.Text = StringManager.GetString(this.GetType().FullName, InputForm.ActiveForm.Text);
        }

        private void inputTextBox_Enter(object sender, EventArgs e)
        {
            UpDownControl.ShowControl("Text", (Control)inputTextBox);
        }

        private void inputTextBox_Leave(object sender, EventArgs e)
        {
            UpDownControl.HideAllControls();
        }

        public void ChangeLocation(Point point)
        {
            this.Location = point;
        }

        private void InputForm_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;
        }        

        private void inputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
            if (e.KeyCode == Keys.Enter)
                btnOk_Click(sender, null);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (inputTextBox.Text.Length == 0)
            {
                inputTextBox.Select(0, inputTextBox.Text.Length);
                errorProvider.SetError(inputTextBox, "Invalid Input");
            }

            bool ok = CheckValid();
            if (ok == false)
            {
                inputTextBox.Select(0, inputTextBox.Text.Length);
                errorProvider.SetError(inputTextBox, "Invalid Input");
                return;
            }
            else
            {
                errorProvider.Clear();
            }

            try
            {
                inputText = inputTextBox.Text;
                this.DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void inputTextBox_Validating(object sender, CancelEventArgs e)
        {
            Regex rgx = new Regex(@"[A-Z0-9_]");
            MatchCollection matches = rgx.Matches(inputTextBox.Text);
            e.Cancel = matches.Count != inputTextBox.Text.Length;
            if (e.Cancel == true)
            {
                inputTextBox.Select(0, inputTextBox.Text.Length);
                errorProvider.SetError(inputTextBox, "Invalid Input");
            }
            else
            {
                errorProvider.Clear();
            }
        }

        private bool CheckValid()
        {
            if (validCheckFunc != null)
                return validCheckFunc(inputTextBox.Text);

            Regex rgx = new Regex(@"[A-Z0-9_]");
            MatchCollection matches = rgx.Matches(inputTextBox.Text);
            return matches.Count == inputTextBox.Text.Length;
        }
    }
}
