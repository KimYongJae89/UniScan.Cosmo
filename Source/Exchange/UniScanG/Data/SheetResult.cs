using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Settings;

namespace UniScanG.Data
{
    public enum DefectType { Unknown = -1, Total, SheetAttack, Noprint, PoleLine, PoleCircle, Dielectric, PinHole, Shape }
    public abstract class SheetResult : AlgorithmResult, IExportable
    {
        public static ResultCollector resultCollector = null;

        protected string prevImagePath;

        public Bitmap PrevImage
        {
            get
            {
                if (prevImage == null && File.Exists(this.prevImagePath))
                    prevImage = ImageHelper.LoadImage(prevImagePath) as Bitmap;
                return prevImage;
            }
            set { prevImage = value; }
        }
        protected Bitmap prevImage = null;

        public List<SheetSubResult> SheetSubResultList { get => sheetSubResultList; set => this.sheetSubResultList = value; }
        protected List<SheetSubResult> sheetSubResultList = new List<SheetSubResult>();

        public SizeF SheetSize { get => this.sheetSize; set => this.sheetSize = value; }
        protected SizeF sheetSize = SizeF.Empty;

        public void Copy(SheetResult sheetResult)
        {
            this.SpandTime = sheetResult.SpandTime;
            this.prevImage = sheetResult.prevImage;
            this.good = sheetResult.good;
            this.sheetSize = sheetResult.sheetSize;

            this.SheetSubResultList.AddRange(sheetResult.sheetSubResultList.ToArray());
        }
        
        public static SheetResult Union(SheetResult[] sheetResults)
        {
            SheetResult sheetResult = resultCollector.CreateSheetResult();

            Array.ForEach (sheetResults,f =>
            {
                sheetResult.sheetSubResultList.AddRange(f.sheetSubResultList.ToArray());
            });

            sheetResult.sheetSize.Width = sheetResults.Sum(f=>f.sheetSize.Width);
            sheetResult.sheetSize.Height = sheetResults.Average(f => f.sheetSize.Height);
            sheetResult.SpandTime = sheetResults.Max(f => f.spandTime);

            return sheetResult;
        }

        public void Offset(int x, int y)
        {
            if (x == 0 && y == 0)
                return;

            float resizeReatio = SystemTypeSettings.Instance().ResizeRatio;
            resizeReatio = 1;
            sheetSubResultList.ForEach(f =>
            {
                SheetSubResult ssr = f as SheetSubResult;
                if (ssr != null)
                    ssr.Offset(x, y, resizeReatio);
                else
                    f.Offset(x * resizeReatio, y * resizeReatio);
            });

        }

        internal void UpdateSubResultImage()
        {
            this.sheetSubResultList.ForEach(f => f.ImportImage());
        }

        public abstract void Export(string path, CancellationToken cancellationToken);
        public abstract bool Import(string path);
    }
}
