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

namespace UniScanG.Gravure.Vision.SheetFinder.SheetBase
{
    public class SheetFinderV2Param : SheetFinderBaseParam
    {
        public float BlankLengthMul
        {
            get { return blankLengthMul; }
            set { blankLengthMul = value; }
        }

        public float ProjectionBinalizeMul
        {
            get { return projectionBinalizeMul; }
            set { projectionBinalizeMul = value; }
        }

        float blankLengthMul = 6.0f;
        float projectionBinalizeMul = 1.0f;

        public SheetFinderV2Param():base()
        {
        }

        ~SheetFinderV2Param()
        {
            Clear();
        }

        #region Abstract
        #endregion

        #region Override
        #endregion

        public override AlgorithmParam Clone()
        {
            SheetFinderV2Param param = new SheetFinderV2Param();
            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            SheetFinderV2Param srcParam = srcAlgorithmParam as SheetFinderV2Param;

            base.CopyFrom(srcParam);

            blankLengthMul = srcParam.blankLengthMul;
            projectionBinalizeMul = srcParam.projectionBinalizeMul;
        }

        public override void SyncParam(AlgorithmParam srcAlgorithmParam)
        {
            CopyFrom(srcAlgorithmParam);
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            this.blankLengthMul = Convert.ToSingle(XmlHelper.GetValue(paramElement, "BlankLengthMul", blankLengthMul.ToString()));
            this.projectionBinalizeMul = Convert.ToSingle(XmlHelper.GetValue(paramElement, "ProjectionBinalizeMul", projectionBinalizeMul.ToString()));
            this.projectionBinalizeMul = 1.0f;
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "BlankLengthMul", blankLengthMul.ToString());
            XmlHelper.SetValue(paramElement, "ProjectionBinalizeMul", projectionBinalizeMul.ToString());
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
    }
}
