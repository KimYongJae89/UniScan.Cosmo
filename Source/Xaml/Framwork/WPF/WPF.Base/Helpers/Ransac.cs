using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Base.Models;

namespace WPF.Base.Helpers
{
    public static class Ransac
    {
        const int numSample = 2;
        
        private static int Compute(IEnumerable<Point> samples, RansacModel model)
        {
            double sx = 0;
            double sy = 0;
            double sxx = 0;
            double syy = 0;
            double sxy = 0;
            double sw = 0;

            foreach (var sample in samples)
            {
                sx += sample.X;
                sy += sample.Y;
                sxx += sample.X * sample.X;
                sxy += sample.X * sample.Y;
                syy += sample.Y * sample.Y;
                sw += 1;
            }

            //variance;
            double vxx = (sxx - sx * sx / sw) / sw;
            double vxy = (sxy - sx * sy / sw) / sw;
            double vyy = (syy - sy * sy / sw) / sw;

            //principal axis
            double theta = Math.Atan2(2 * vxy, vxx - vyy) / 2;

            model.Radian = theta;

            model.Cos = Math.Cos(theta);
            model.Sin = Math.Sin(theta);
            model.Tan = Math.Tan(theta);

            //center of mass(xc, yc)
            model.CenterX = sx / sw;
            model.CenterY = sy / sw;

            //직선의 방정식: sin(theta)*(x - sx) = cos(theta)*(y - sy);
            return 1;
        }

        private static double Verification(List<Point> inliers, RansacModel model, IEnumerable<Point> data, double distThreshold)
        {
            double cost = 0;

            foreach (var point in data)
            {
                // 직선에 내린 수선의 길이를 계산한다.
                double distance = Math.Abs((point.X - model.CenterX) * model.Sin - (point.Y - model.CenterY) * model.Cos) 
                    / Math.Sqrt(model.Cos * model.Cos + model.Sin * model.Sin);

                // 예측된 모델에서 유효한 데이터인 경우, 유효한 데이터 집합에 더한다.
                if (distance < distThreshold)
                {
                    cost ++;
                    inliers.Add(point);
                }
            }

            return cost;
        }

        public static RansacModel GetModel(IEnumerable<Point> points, double distThreshold)
        {
            RansacModel model = new RansacModel();
            Random random = new Random();

            int numInteration = (int)(Math.Log(1 - 0.99) / Math.Log(1 - Math.Pow(0.5, 3)));

            var samples = new Point[numSample];
            var inliers = new List<Point>();
            
            for (int iter = 0; iter < numInteration; iter++)
            {
                for (int i = 0; i < numSample; i++)
                {
                    var sample = points.ElementAt(random.Next(0, points.Count()));
                    if (!samples.Contains(sample))
                        samples[i] = sample;
                }

                // 이 데이터를 정상적인 데이터로 보고 모델 파라메터를 예측한다.
                Compute(samples, model);

                // 2. Verification

                // 원본 데이터가 예측된 모델에 잘 맞는지 검사한다.
                double cost = Verification(inliers, model, points, distThreshold);

                // 만일 예측된 모델이 잘 맞는다면, 이 모델에 대한 유효한 데이터로 새로운 모델을 구한다.

                if (model.Cost < cost)
                {
                    model.Cost = cost;
                    Compute(inliers, model);
                }
            }

            return model;
        }
    }
}
