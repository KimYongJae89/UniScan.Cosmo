﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;

namespace DynMvp.Barcode
{
    public abstract class BarcodePrinter
    {
        protected BarcodeSettings barcodeSettings = null;
        protected BarcodeRenderer barcodeRenderer = null;
        protected Image barcodeImage = null;
        protected PrintDocument printDocument = null;

        public BarcodePrinter()
        {
        }

        public virtual void Initialzie(BarcodeSettings barcodeSettings, string printerName = null)
        {
            this.barcodeSettings = barcodeSettings;
            barcodeRenderer = BarcodeRenderer.GetBarcodeRenderer(this.barcodeSettings.BarcodeRendererType);

            printDocument = new PrintDocument();
            if (String.IsNullOrEmpty(printerName) == false)
                printDocument.PrinterSettings.PrinterName = printerName;

            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintPage);
        }

        private void PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            using (Graphics g = e.Graphics)
            {
                foreach (BarcodeTextItem barcodeTextItem in barcodeSettings)
                    DrawBarcodeText(g, barcodeTextItem);

                g.DrawImage(barcodeImage, barcodeSettings.BarcodePos.X, barcodeSettings.BarcodePos.Y, barcodeSettings.BarcodeSize.Width, barcodeSettings.BarcodeSize.Height);
            }
        }

        private void DrawBarcodeText(Graphics g, BarcodeTextItem barcodeTextItem)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = barcodeTextItem.Alignment;
            stringFormat.LineAlignment = StringAlignment.Near;

            Brush fontBrush = new SolidBrush(Color.Black);

            string text = GetTextValue(barcodeTextItem.Name);

            g.DrawString(text, barcodeTextItem.Font, fontBrush, (float)barcodeTextItem.Position.X, (float)barcodeTextItem.Position.Y, stringFormat);
        }

        public abstract string GetTextValue(string name);
    }
}
