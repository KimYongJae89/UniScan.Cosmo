using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Inspect;
using UniScanG.Vision;

namespace UniScanG.Screen.Vision.FiducialFinder
{
    public class FiducialFinderParam : DynMvp.Vision.AlgorithmParam
    {
        protected int minScore;
        public int MinScore
        {
            get { return minScore; }
            set { minScore = value; }
        }

        protected int searchRangeHalfWidth;
        public int SearchRangeHalfWidth
        {
            get { return searchRangeHalfWidth; }
            set { searchRangeHalfWidth = value; }
        }

        protected int searchRangeHalfHeight;
        public int SearchRangeHalfHeight
        {
            get { return searchRangeHalfHeight; }
            set { searchRangeHalfHeight = value; }
        }

        public FiducialFinderParam()
        {
            searchRangeHalfWidth = 100;
            searchRangeHalfHeight = 100;
            minScore = 80;
        }

        public override AlgorithmParam Clone()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
