using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScan.Common.Data;
using UniScanG.Screen.Vision.Detector;

namespace UniScanG.Screen.Vision.FiducialFinder
{
    public class FiducialFinderAlgorithmResult : AlgorithmResult, IExportable
    {
        public FiducialFinderAlgorithmResult()
        {
            this.algorithmName = FiducialFinderS.TypeName;
        }
        public void Export(string path, CancellationToken cancellationToken)
        {
            FiducialFinderS fiducialFinder = (FiducialFinderS)AlgorithmPool.Instance().GetAlgorithm(FiducialFinderS.TypeName);

            if (fiducialFinder == null)
                return;

            FiducialFinderParam param = (FiducialFinderParam)fiducialFinder.Param;

            string fileName = Path.Combine(path, string.Format("{0}.csv", FiducialFinderS.TypeName));
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}\t{1}\t{2}\t", param.MinScore, param.SearchRangeHalfWidth, param.SearchRangeHalfHeight);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();

            stringBuilder.AppendFormat("{0}\t{1}\t", OffsetFound.Width, RealOffsetFound.Height);
            stringBuilder.AppendFormat("{0}\t{1}\t", RealOffsetFound.Width, RealOffsetFound.Height);
            
            File.WriteAllText(fileName, stringBuilder.ToString());
        }

        public static Tuple<AlgorithmResult, AlgorithmParam> Import(string path)
        {
            string fileName = Path.Combine(path, string.Format("{0}.csv", FiducialFinderS.TypeName));
            string[] lines = File.ReadAllLines(path);
            
            FiducialFinderAlgorithmResult result = new FiducialFinderAlgorithmResult();
            FiducialFinderParam param = new FiducialFinderParam();

            List<string> resultList = new List<string>();
            foreach (string line in lines)
                resultList.AddRange(line.Split('\t'));

            if (resultList.Count < 6)
                return null;

            param.MinScore = Convert.ToInt32(resultList[0]);
            param.SearchRangeHalfWidth = Convert.ToInt32(resultList[1]);
            param.SearchRangeHalfHeight = Convert.ToInt32(resultList[2]);

            result.OffsetFound = new System.Drawing.SizeF(Convert.ToSingle(resultList[3]), Convert.ToSingle(resultList[4]));
            result.RealOffsetFound = new System.Drawing.SizeF(Convert.ToSingle(resultList[5]), Convert.ToSingle(resultList[6]));

            Tuple<AlgorithmResult, AlgorithmParam> set = new Tuple<AlgorithmResult, AlgorithmParam>(result, param);

            return set;
        }
    }
}
