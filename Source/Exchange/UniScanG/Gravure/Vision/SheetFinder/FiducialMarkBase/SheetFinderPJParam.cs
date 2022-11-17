using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System.IO;
using UniEye.Base.Settings;
using UniScanG.Inspect;
using UniScanG.Gravure.Inspect;

namespace UniScanG.Gravure.Vision.SheetFinder.FiducialMarkBase
{
    public class SheetFinderPJParam : SheetFinderBaseParam
    {
        float fidSearchLBound;
        public float FidSearchLBound
        {
            get { return fidSearchLBound; }
            set { fidSearchLBound = value; }
        }

        float fidSearchRBound;
        public float FidSearchRBound
        {
            get { return fidSearchRBound; }
            set { fidSearchRBound = value; }
        }
        
        public SheetFinderPJParam()
        {
            this.fidSize = new System.Drawing.Size(500, 150);

            this.fidSearchLBound = 0.00f;
            this.fidSearchRBound = 0.92f;
        }

        ~SheetFinderPJParam()
        {
            Clear();
        }

        #region Abstract
        #endregion

        #region Override
        #endregion

        public override AlgorithmParam Clone()
        {
            SheetFinderPJParam param = new SheetFinderPJParam();
            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            SheetFinderPJParam srcParam = srcAlgorithmParam as SheetFinderPJParam;

            base.CopyFrom(srcParam);

            fidSize = srcParam.fidSize;

            fidSearchLBound = srcParam.fidSearchLBound;
            fidSearchRBound = srcParam.fidSearchRBound;
        }

        public override void SyncParam(AlgorithmParam srcAlgorithmParam)
        {
            CopyFrom(srcAlgorithmParam);
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);
            
            this.fidSearchLBound = Convert.ToSingle(XmlHelper.GetValue(paramElement, "FidSearchLBound", fidSearchLBound.ToString()));
            this.fidSearchRBound = Convert.ToSingle(XmlHelper.GetValue(paramElement, "FidSearchRBound", fidSearchRBound.ToString()));
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "FidSearchLBound", fidSearchLBound.ToString());
            XmlHelper.SetValue(paramElement, "FidSearchRBound", fidSearchRBound.ToString());
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
    }
}
