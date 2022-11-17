using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class MatroxBoardListForm : Form
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

        public MatroxBoardListForm()
        {
            InitializeComponent();

            buttonMoveUp.Text = StringManager.GetString(this.GetType().FullName,buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(this.GetType().FullName,buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);
        }

        private void MatroxBoardListForm_Load(object sender, EventArgs e)
        {
            foreach (CameraInfo cameraInfo in cameraConfiguration)
            {
                if (cameraInfo is CameraInfoMil)
                {
                    CameraInfoMil cameraInfoMil = (CameraInfoMil)cameraInfo;

                    cameraInfoGrid.Rows.Add(cameraInfoMil.SystemType.ToString(), cameraInfoMil.SystemNum, cameraInfoMil.DigitizerNum, cameraInfoMil.CameraType.ToString());
                }
            }

            if (cameraInfoGrid.Rows.Count < requiredNumCamera)
            {
                cameraInfoGrid.Rows.Add();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            cameraConfiguration.Clear();

            foreach (DataGridViewRow row in cameraInfoGrid.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null)
                {
                    CameraInfoMil cameraInfoMil = new CameraInfoMil(
                                    (MilSystemType)Enum.Parse(typeof(MilSystemType), row.Cells[0].Value.ToString()), uint.Parse(row.Cells[1].Value.ToString()),
                                    uint.Parse(row.Cells[2].Value.ToString()), (CameraType)Enum.Parse(typeof(CameraType), row.Cells[3].Value.ToString()));
                    cameraConfiguration.AddCameraInfo(cameraInfoMil);
                }
            }

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
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
