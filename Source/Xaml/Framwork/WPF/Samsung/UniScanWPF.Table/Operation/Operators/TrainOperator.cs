using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation.Operators
{
    public class TeachOperator : Operator
    {
        TeachOperatorSettings settings;

        public TeachOperatorSettings Settings { get => settings; }

        public TeachOperator()
        {
            settings = new TeachOperatorSettings();
        }

        public void Train(List<ExtractOperatorResult> extractOperatorResultList)
        {
            lock (this)
            {
                OperatorState = OperatorState.Run;
                PatternGroup sumPatternGroup = new PatternGroup();

                foreach (ExtractOperatorResult extractOperatorResult in extractOperatorResultList)
                {
                    if (extractOperatorResult.BlobRectList != null)
                        sumPatternGroup.AddPattern(extractOperatorResult.BlobRectList);
                }


                List<PatternGroup> patternGroupList = sumPatternGroup.DivideSubGroup(settings.DiffGroupThreshold / DeveloperSettings.Instance.Resolution);
                patternGroupList = patternGroupList.OrderByDescending(patternGroup => patternGroup.Count).ToList();

                PatternGroup refPatternGroup = new PatternGroup();
                if (patternGroupList.Count > 0)
                {
                    float maxArea = patternGroupList.Max(patternGroup => patternGroup.SumArea);
                    refPatternGroup = patternGroupList.Find(patternGroup => patternGroup.SumArea == maxArea);

                    float refAngle = refPatternGroup.PatternList.Average(blob => blob.RotateAngle);

                    int inflateLength = 50;

                    foreach (PatternGroup patternGroup in patternGroupList)
                    {
                        BlobRect blobRect = patternGroup.GetAverageBlobRect();
                        AlgoImage patternImage = null;
                        foreach (ExtractOperatorResult extractOperatorResult in extractOperatorResultList)
                        {
                            if (extractOperatorResult?.BlobRectList == null)
                                continue;

                            if (extractOperatorResult.BlobRectList.Contains(blobRect))
                            {
                                Rectangle sourceRect = new Rectangle(0, 0, extractOperatorResult.ScanOperatorResult.TopLightImage.Width, extractOperatorResult.ScanOperatorResult.TopLightImage.Height);

                                Rectangle rect = Rectangle.Truncate(blobRect.BoundingRect);
                                rect.Inflate(inflateLength, inflateLength);
                                rect.Intersect(sourceRect);

                                patternImage = extractOperatorResult.ScanOperatorResult.TopLightImage.GetSubImage(rect);
                                break;
                            }
                        }

                        if (patternImage != null)
                        {
                            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(patternImage);
                            int width = blobRect.RotateAngle <= 45 && blobRect.RotateAngle > -45 ? (int)Math.Ceiling(blobRect.RotateWidth + inflateLength) : (int)Math.Ceiling(blobRect.RotateHeight + inflateLength); ;
                            int height = blobRect.RotateAngle <= 45 && blobRect.RotateAngle > -45 ? (int)Math.Ceiling(blobRect.RotateHeight + inflateLength) : (int)Math.Ceiling(blobRect.RotateWidth + inflateLength);
                            AlgoImage rotateImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(width, height));
                            imageProcessing.Rotate(patternImage, rotateImage, blobRect.RotateAngle <= 45 && blobRect.RotateAngle > -45 ? -blobRect.RotateAngle : 90 - blobRect.RotateAngle);

                            patternGroup.RefImage = rotateImage.ToBitmapSource();
                            //Helper.WPFImageHelper.SaveBitmapSource("d:\\asdad.bmp", patternGroup.RefImage);
                            //rotateImage.Save("rotateImage.bmp", new DebugContext(true, "d:\\"));
                            rotateImage.Dispose();
                            patternImage.Dispose();
                        }
                    }

                    patternGroupList.Remove(refPatternGroup);
                }

                TeachOperatorResult teachOperatorResult = new TeachOperatorResult(resultKey, new List<PatternGroup>() { refPatternGroup }, patternGroupList);
                SystemManager.Instance().OperatorCompleted(teachOperatorResult);

                OperatorState = OperatorState.Idle;
            }
        }
    }

    public class TeachOperatorResult : OperatorResult
    {
        List<PatternGroup> inspectPatternList = new List<PatternGroup>();
        List<PatternGroup> candidatePatternList = new List<PatternGroup>();

        public List<PatternGroup> InspectPatternList { get => inspectPatternList; }
        public List<PatternGroup> CandidatePatternList { get => candidatePatternList; }

        public TeachOperatorResult(ResultKey resultKey, List<PatternGroup> inspectPatternList, List<PatternGroup> candidatePatternList) : base(ResultType.Train, resultKey, null)
        {
            this.inspectPatternList = inspectPatternList;
            this.candidatePatternList = candidatePatternList;
            this.exceptionMessage = "Completed";
        }

        public TeachOperatorResult(ResultKey resultKey, string exeptionMessage) : base(ResultType.Train, resultKey, exeptionMessage)
        {

        }
    }

    public class TeachOperatorSettings : OperatorSettings
    {
        [CatecoryAttribute("Teach"), NameAttribute("Grouping Threshold")]
        public float DiffGroupThreshold
        {
            get => diffGroupThreshold;
            set => diffGroupThreshold = Math.Max(5, value);
        }
        float diffGroupThreshold = 100;

        protected override void Initialize()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Trainner");
        }

        public override void Load(XmlElement xmlElement)
        {
            diffGroupThreshold = XmlHelper.GetValue(xmlElement, "DiffGroupThreshold", diffGroupThreshold);
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "DiffGroupThreshold", diffGroupThreshold.ToString());
        }
    }
}
