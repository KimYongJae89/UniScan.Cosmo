﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DynMvp.Vision;

namespace DynMvp.Data.Forms
{
    public partial class GridParamPanel : UserControl
    {
        GridParam gridParam;
        public GridParam GridParam
        {
            set { gridParam = value; }
        }

        public GridParamPanel()
        {
            InitializeComponent();
        }

        private void useGrid_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rowCount_ValueChanged(object sender, EventArgs e)
        {

        }

        private void columnCount_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
