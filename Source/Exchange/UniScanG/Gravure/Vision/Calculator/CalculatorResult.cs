using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.UI;
using System.Drawing;
using System.Diagnostics;
using UniEye.Base.Settings;
using DynMvp.Data;
using System.Threading;
using System.Runtime.InteropServices;
using UniEye.Base;
using System.IO;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Data;
using UniScanG.Vision;
using UniScanG.Data;
using UniScan.Common.Settings;
using UniScan.Common.Data;

namespace UniScanG.Gravure.Vision.Calculator
{
    public class CalculatorResult : SheetResult, IExportable
    {
        public ImageD PreviewImageD { get => previewImageD; set => previewImageD = value; }
        ImageD previewImageD = null;

        private Point sheetPosOffset;
        public Point SheetPosOffset
        {
            get { return sheetPosOffset; }
            set { sheetPosOffset = value; }
        }

        public CalculatorResult()
        {
            resultCollector = new UniScanG.Gravure.Data.ResultCollector();
        }

        public override void Export(string path, CancellationToken cancellationToken)
        {
            CalculatorParam calculatorParam = AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName).Param as CalculatorParam;

            SizeF pelSize = new SizeF(14, 14);
            if(SystemManager.Instance()!=null)
                pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault().PelSize;

            // 시트 1장에 대한 검사 결과 저장.
            string fileName = Path.Combine(path, string.Format("{0}.csv", "Result"));
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(string.Format("CALCULATOR TEACH"));
            stringBuilder.AppendLine(string.Format("BinValue,{0}", calculatorParam.BinValue));//티칭값
            stringBuilder.AppendLine(string.Format("BasePosition,{0},{1}", calculatorParam.BasePosition.X, calculatorParam.BasePosition.Y));//티칭값
            stringBuilder.AppendLine(string.Format("SheetSize,{0},{1}", calculatorParam.SheetSize.Width, calculatorParam.SheetSize.Height));//티칭값

            stringBuilder.AppendLine(string.Format("CALCULATOR RESULT"));
            stringBuilder.AppendLine(string.Format("Spand Time,{0}", this.spandTime));//검사값
            stringBuilder.AppendLine(string.Format("SheetPositionOffset,{0},{1}", this.sheetPosOffset.X, this.sheetPosOffset.Y));//검사값
            stringBuilder.AppendLine(string.Format("SheetSize,{0},{1}", this.sheetSize.Width, sheetSize.Height));//검사값

            stringBuilder.AppendLine(string.Format("REGION INFO LIST"));
            stringBuilder.AppendLine(string.Format("X,RealX,Y,RealY,Width,RealWidth,Height,RealHeight,PoleAvg,DielectricAvg"));
            SizeF scale = new SizeF(pelSize.Width / 1000.0f, pelSize.Height / 1000.0f);
            foreach (RegionInfoG regionInfo in calculatorParam.RegionInfoList)
            {
                Rectangle regionRect = regionInfo.Region;
                regionRect.Offset(this.sheetPosOffset);
                stringBuilder.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                    regionRect.X, regionRect.X * scale.Width,
                    regionRect.Y, regionRect.Y * scale.Height,
                    regionRect.Width, regionRect.Width * scale.Width,
                    regionRect.Height, regionRect.Height * scale.Height,
                    regionInfo.PoleAvg, regionInfo.DielectricAvg
                    ));
            }

            File.AppendAllText(fileName, stringBuilder.ToString());
            //base.Export(path);

            if(previewImageD!=null)
            {
                previewImageD.SaveImage(Path.Combine(path, string.Format("Prev.Jpg")));
            }
            else if (prevImage != null)
            {
                Bitmap bitmap = null;
                // 저장이 오래걸려서 화면 업데이트중 [개채를 다른곳에서 사용중] 예외 발생.
                lock (prevImage)
                    bitmap = (Bitmap)prevImage.Clone();
                ImageHelper.SaveImage(bitmap, Path.Combine(path, string.Format("Prev.Jpg")));
            }
        }

        public override bool Import(string path)
        {
            string fileName = Path.Combine(path, string.Format("{0}.csv", "Result"));

            if (File.Exists(fileName) == false)
                return false;

            try
            {
                List<string> lines = File.ReadAllLines(fileName).ToList();

                int startIdxResult = lines.FindIndex(f => f == "CALCULATOR RESULT");
                if (startIdxResult >= 0)
                {
                    this.spandTime = TimeSpan.Parse(lines[startIdxResult + 1].Split(',')[1]);
                }
                return true;
            }
            catch { return false; }
        }
    }
}
