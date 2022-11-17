//using System;
//using System.Windows.Forms;
//using DynMvp.Data;
//using DynMvp.Base;
//using System.Text.RegularExpressions;

//namespace UniScanG.Temp
//{
//    public enum ModelFormType
//    {
//        New, Edit, Copy
//    }

//    public partial class ModelForm : Form
//    {
//        string initModelName;
//        public string InitModelName
//        {
//            get { return initModelName; }
//            set { initModelName = value; }
//        }

//        ModelManager modelManager;
//        public ModelManager ModelManager
//        {
//            get { return modelManager; }
//            set { modelManager = value; }
//        }

//        ModelDescription modelDescription = null;
//        public ModelDescription ModelDescription
//        {
//            get { return modelDescription; }
//            set { modelDescription = value; }
//        }

//        ModelFormType modelFormType;
//        public ModelFormType ModelFormType
//        {
//            get { return modelFormType; }
//            set { modelFormType = value; }
//        }
        
//        public ModelForm()
//        {
//            InitializeComponent();
            
//            labelRegistrant.Text = StringManager.GetString(this.GetType().FullName,labelRegistrant.Text);

//            // language change
//            labelScreenModel.Text = StringManager.GetString(this.GetType().FullName,labelScreenModel.Text);
//            labelDescription.Text = StringManager.GetString(this.GetType().FullName,labelDescription.Text);
//            labelRegistrant.Text = StringManager.GetString(this.GetType().FullName,labelRegistrant.Text);
//            labelRegistrationDate.Text = StringManager.GetString(this.GetType().FullName,labelRegistrationDate.Text);
//            btnOK.Text = StringManager.GetString(this.GetType().FullName,btnOK.Text);
//            btnCancel.Text = StringManager.GetString(this.GetType().FullName,btnCancel.Text);
//        }

//        private void ModelForm_Load(object sender, EventArgs e)
//        {
//            modelManager = UniEye.Base.SystemManager.Instance().ModelManager;
            
//            if (modelDescription == null)
//            {
//                Text = StringManager.GetString(this.GetType().FullName, "New Model");

//                screenModel.Text = initModelName;
////                int count = modelManager.NewModelExistCount(name);
//            }
//            else
//            {
//                if (modelFormType == ModelFormType.Edit)
//                {
//                    Text = StringManager.GetString(this.GetType().FullName, "Edit Model");
                    
//                    screenModel.Enabled = false;
//                    registrant.Enabled = false;
//                    registrationDate.Enabled = false;
//                }
//                else
//                {
//                    Text = StringManager.GetString(this.GetType().FullName, "Copy Model");
//                }

//                SetModelData();
//            }
//        }

//        private void SetModelData()
//        {
//            screenModel.Text = modelDescription.Name;
//            description.Text = modelDescription.Description;
//            registrant.Text = modelDescription.Registrant;
//        }

//        private void GetModelData(string modelName)
//        {
//            modelDescription.Name = modelName;
//            modelDescription.Description = description.Text;
//            modelDescription.Registrant = registrant.Text;
////            modelDescription.RegistrationDate = registrationDate.Value.Date;
            
//            ModelManager.SaveModelDescription(modelDescription);
//        }

//        private void btnOK_Click(object sender, EventArgs e)
//        {
//            if (String.IsNullOrEmpty(screenModel.Text))
//            {
//                errorProvider.SetError(screenModel, "Invalid Model Name");
//                return;
//            }

//            if (String.IsNullOrEmpty(thickness.Text))
//            {
//                errorProvider.SetError(thickness, "Invalid Thickness");
//                return;
//            }

//            if (float.Parse(thickness.Text) <= 0)
//            {
//                errorProvider.SetError(thickness, "Invalid Thickness");
//                return;
//            }

//            if (String.IsNullOrEmpty(paste.Text))
//            {
//                errorProvider.SetError(paste, "Invalid Paste");
//                return;
//            }

//            string modelName = string.Format("{0}-T{1}({2})", screenModel.Text, thickness.Text, paste.Text);

//            if (modelDescription == null || modelFormType == ModelFormType.Copy)
//            {
//                if (modelManager.IsModelExist(modelName))
//                {
//                    MessageBox.Show(this, StringManager.GetString(this.GetType().FullName, "The model name is exist\n Please, use other model name or thickness."), "Error", MessageBoxButtons.OK);
//                    return;
//                }

//                modelDescription = modelManager.CreateModelDescription();
//            }

//            DialogResult = DialogResult.OK;

//            GetModelData(modelName);
            
//            Close();
//        }
//        private int GetModelCount()
//        {
//            return 0;
//        }

//        private void screenModel_Validating(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            Regex rgx = new Regex(@"[A-Z0-9-]");
//            MatchCollection matches = rgx.Matches(screenModel.Text);
//            //e.Cancel = ();
//            e.Cancel = matches.Count != screenModel.Text.Length; 
//            if (e.Cancel == true)
//            {
//                screenModel.Select(0, screenModel.Text.Length);

//                errorProvider.SetError(screenModel, "Invalid Model Name");
//            }
//            else
//            {
//                errorProvider.Clear();
//            }
//        }

//        private void thickness_Validating(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            Regex rgx = new Regex(@"[0-9.]");
//            MatchCollection matches = rgx.Matches(thickness.Text);
//            e.Cancel = matches.Count != thickness.Text.Length;
//            if (e.Cancel == true)
//            {
//                thickness.Select(0, thickness.Text.Length);
//                errorProvider.SetError(thickness, "Invalid Thickness");
//            }
//            else
//            {
//                errorProvider.Clear();
//            }
//        }
        
//        private void paste_Validating(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            Regex rgx = new Regex(@"[A-Z0-9]");
//            MatchCollection matches = rgx.Matches(paste.Text);
//            e.Cancel = matches.Count != paste.Text.Length;
//            if (e.Cancel == true)
//            {
//                paste.Select(0, paste.Text.Length);
//                errorProvider.SetError(paste, "Invalid Paste");
//            }
//            else
//            {
//                errorProvider.Clear();
//            }
//        }
//    }
//}
