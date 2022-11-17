using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Windows.Forms;

namespace DynMvp.Devices.FrameGrabber.UI
{
    public partial class EuresysBoardListForm : Form
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

        public EuresysBoardListForm()
        {
            InitializeComponent();

            buttonMoveUp.Text = StringManager.GetString(this.GetType().FullName,buttonMoveUp.Text);
            buttonMoveDown.Text = StringManager.GetString(this.GetType().FullName,buttonMoveDown.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);
        }

        private void EuresysBoardListForm_Load(object sender, EventArgs e)
        {
            foreach (CameraInfo cameraInfo in cameraConfiguration)
            {
                if (cameraInfo is CameraInfoMultiCam)
                {
                    CameraInfoMultiCam cameraInfoMultiCam = (CameraInfoMultiCam)cameraInfo;

                    cameraInfoGrid.Rows.Add(cameraInfoMultiCam.BoardType.ToString(), cameraInfoMultiCam.BoardId, cameraInfoMultiCam.ConnectorId, cameraInfoMultiCam.CameraType.ToString(), cameraInfoMultiCam.SurfaceNum.ToString(), cameraInfoMultiCam.PageLength.ToString(), cameraInfoMultiCam.UseNativeBuffering.ToString());
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
                if (row.Cells[0].Value != null && row.Cells[1].Value != null && row.Cells[2].Value != null && row.Cells[3].Value != null && row.Cells[4].Value != null && row.Cells[5].Value != null)
                {
                    CameraInfoMultiCam cameraInfoMultiCam = new CameraInfoMultiCam(
                                    (EuresysBoardType)Enum.Parse(typeof(EuresysBoardType), row.Cells[0].Value.ToString()), uint.Parse(row.Cells[1].Value.ToString()),
                                    uint.Parse(row.Cells[2].Value.ToString()), (CameraType)Enum.Parse(typeof(CameraType), row.Cells[3].Value.ToString()), uint.Parse(row.Cells[4].Value.ToString()),
                                    uint.Parse(row.Cells[5].Value.ToString()));

                    cameraInfoMultiCam.UseNativeBuffering = bool.Parse(row.Cells[6].Value.ToString()); 
                    cameraConfiguration.AddCameraInfo(cameraInfoMultiCam);
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
