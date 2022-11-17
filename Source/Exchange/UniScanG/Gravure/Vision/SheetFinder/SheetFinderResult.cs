using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Vision.SheetFinder
{
    public class SheetFinderResult:AlgorithmResult
    {
        List<Rectangle> foundedFiducialRectList = null;

        public List<Rectangle> FoundedFiducialRectList { get => foundedFiducialRectList; }

        public SheetFinderResult()
        {
            this.foundedFiducialRectList = new List<Rectangle>();
        }

    }
}
