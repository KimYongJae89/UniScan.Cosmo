using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using UniScan.Common.Util;
using System.Drawing;
using DynMvp.Authentication;
using DynMvp.UI;
using UniEye.Base.Data;
using DynMvp.Base;
using System.IO;
using UniScan.Common.Settings;
using DynMvp.Data;

namespace UniScanG.UI.Etc
{
    public partial class VolumePanel : UserControl, IStatusStripPanel
    {
        DriveInfo driveInfo;
        Color baseColor;

        int count;

        public VolumePanel(DriveInfo driveInfo)
        {
            InitializeComponent();

            this.baseColor = this.BackColor;
            
            this.driveInfo = driveInfo;
            this.textLabel.Text = driveInfo.Name;
        }

        public void StateUpdate()
        {
            float ratio = (1.0f - ((float)driveInfo.TotalFreeSpace / (float)driveInfo.TotalSize)) * 100.0f;

            volumeBar.ToolTipText = string.Format("{0:0.00} %", ratio);
            volumeBar.Value = (int)ratio;

            DataCopier dataCopier = SystemManager.Instance().DataManagerList.Find(f => f is DataCopier) as DataCopier;
            if (dataCopier?.CurBackupDrive?.Name == this.driveInfo.Name)
            {
                textLabel.ToolTipText = string.Format("Copy to {0}..", this.driveInfo.Name);

                if (count % 2 == 0)
                {
                    this.BackColor = Color.FromArgb(127, baseColor);
                }
                else
                {
                    textLabel.BackColor = Color.FromArgb(0, baseColor);
                    this.BackColor = Color.FromArgb(0, baseColor);
                }

                count++;
            }
            else
            {
                textLabel.ToolTipText = null;
                this.BackColor = baseColor;
                count = 0;
            }
        }
    }
}