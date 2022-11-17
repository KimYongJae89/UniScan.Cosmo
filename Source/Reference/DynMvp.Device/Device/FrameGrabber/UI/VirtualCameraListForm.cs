using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class VirtualCameraListForm : Form
    {
        int requiredNumCamera;
        public int RequiredNumCamera
        {
            set { requiredNumCamera = value; }
        }

        CameraConfiguration cameraConfiguration;
        public CameraConfiguration CameraConfiguration
        {
            get { return cameraConfiguration; }
            set { cameraConfiguration = value; }
        }

        public VirtualCameraListForm()
        {
            InitializeComponent();

            detectAllButton.Text = StringManager.GetString(this.GetType().FullName,detectAllButton.Text);
            buttonMoveUp.Text = StringManager.GetString(this.GetType().FullName,buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(this.GetType().FullName,buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);
        }

        private void VirtualCameraListForm_Load(object sender, EventArgs e)
        {
            for (int i=0; i< requiredNumCamera; i++)
            {
                if (cameraConfiguration.CameraInfoList.Count > i)
                {
                    CameraInfo cameraInfo = cameraConfiguration.CameraInfoList[i];
                    cameraInfoGrid.Rows.Add(cameraInfo.Index, cameraInfo.Width, cameraInfo.Height, cameraInfo.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                }
                else
                {
                    cameraInfoGrid.Rows.Add();
                    cameraInfoGrid.Rows[i].Cells[0].Value = i;
                    cameraInfoGrid.Rows[i].Cells[1].Value = 0;
                    cameraInfoGrid.Rows[i].Cells[2].Value = 0;
                    cameraInfoGrid.Rows[i].Cells[3].Value = false;
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            cameraConfiguration.Clear();

            foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null)
                {
                    CameraInfo cameraInfo = new CameraInfo();
                    cameraInfo.Index = int.Parse(row.Cells[0].Value.ToString());
                    cameraInfo.Width = int.Parse(row.Cells[1].Value.ToString());
                    cameraInfo.Height = int.Parse(row.Cells[2].Value.ToString());
                    cameraInfo.PixelFormat = (bool.Parse(row.Cells[3].Value.ToString()) == true ? System.Drawing.Imaging.PixelFormat.Format24bppRgb : System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                    cameraConfiguration.AddCameraInfo(cameraInfo);
                }
            }

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void autoDetectButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            
            if (UiHelper.ShowSTADialog(dialog) == DialogResult.OK)
            {
                if (Directory.Exists(dialog.SelectedPath) == false)
                    return;
                uint cameraIndex = 0;
                
                foreach (DataGridViewRow row in cameraInfoGrid.Rows)
                {
                    //uint cameraIndex = uint.Parse(row.Cells[0].Value.ToString());
                    //string searchPattern = string.Format("Image_C{0:00}_*.*", cameraIndex);
                   // string searchPattern = string.Format("Image_C{0:00}*.*", cameraIndex);
                    string searchPattern = string.Format("*.bmp");

                    String[] filePaths = Directory.GetFiles(dialog.SelectedPath, searchPattern);
                    if (filePaths.Count() == 0)
                        continue;

                    Bitmap defaultImage = new Bitmap(filePaths[0]);

                    row.Cells[1].Value = defaultImage.Width;
                    row.Cells[2].Value = defaultImage.Height;
                    row.Cells[3].Value = defaultImage.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
                    cameraIndex++;
                }
            }
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            UiHelper.MoveUp(cameraInfoGrid);
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            UiHelper.MoveDown(cameraInfoGrid);
        }
    }
}
