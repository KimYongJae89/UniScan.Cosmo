using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UinScanGTest
{
    public partial class Form1 : Form
    {
        Image2D image2D = new Image2D();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MatroxHelper.InitApplication();
            if (MatroxHelper.LicenseExist() == false)
            {
                MessageBox.Show("Can not found MIL Licenses");
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string file = @"D:\UniScan\Gravure_Inspector\VirtualImage\440-31BMJE503-GL03SC\Image_C00_S000_L00.bmp";
            if(!File.Exists(file))
            {
                file = "";
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "BMP files(*.bmp)|*.bmp";
                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                    file = openFileDialog.FileName;
            }

            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                if (!string.IsNullOrEmpty(file))
                    this.image2D.LoadImage(file);
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.image2D == null)
            {
                MessageBox.Show("There is no Image loaded");
                return;
            }

            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() =>
            {
                Operation.Processer processer = new Operation.Processer();
                processer.Start(this.image2D);
            });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MatroxHelper.FreeApplication();
        }

    }
}

