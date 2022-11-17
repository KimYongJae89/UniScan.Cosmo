using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml;

using Standard.DynMvp.Base;
 
using Standard.DynMvp.Vision.Euresys;
using Standard.DynMvp.Vision.Matrox;

using System.IO;

namespace Standard.DynMvp.Vision
{
    public class AlgorithmStrategy
    {
        string algorithmType;
        public string AlgorithmType
        {
            get { return algorithmType; }
            set { algorithmType = value; }
        }

        ImagingLibrary libraryType;
        public ImagingLibrary LibraryType
        {
            get { return libraryType; }
            set { libraryType = value; }
        }

        ImageType imageType;
        public ImageType ImageType
        {
            get { return imageType; }
            set { imageType = value; }
        }

        string subLibraryType;
        public string SubLibraryType
        {
            get { return subLibraryType; }
            set { subLibraryType = value; }
        }

        bool enabled = false;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public AlgorithmStrategy(string algorithmType, ImagingLibrary libraryType, string subLibraryType, ImageType imageType = ImageType.Grey)
        {
            this.algorithmType = algorithmType;
            this.libraryType = libraryType;
            this.imageType = imageType;
            this.subLibraryType = subLibraryType;
        }
    }

    public class AlgorithmBuilder
    {
        static int licenseErrorCount = 0;
        public static int LicenseErrorCount
        {
            get { return licenseErrorCount; }
        }

        static List<AlgorithmStrategy> algorithmStrategyList = new List<AlgorithmStrategy>();
        static OpenEVisionImageProcessing openEVisionImageProcessing = new OpenEVisionImageProcessing();
        static MilImageProcessing milImageProcessing = new MilImageProcessing();

        public static void ClearStrategyList()
        {
            algorithmStrategyList.Clear();
        }

        public static void AddStrategy(AlgorithmStrategy strategy)
        {
            algorithmStrategyList.Add(strategy);
        }

        public static bool IsUseMatroxMil()
        {
            foreach (AlgorithmStrategy algorithmStrategy in algorithmStrategyList)
            {
                if (algorithmStrategy.LibraryType == ImagingLibrary.MatroxMIL)
                    return true;
            }

            return false;
        }

        public static bool IsUseCognexVisionPro()
        {
            foreach (AlgorithmStrategy algorithmStrategy in algorithmStrategyList)
            {
                if (algorithmStrategy.LibraryType == ImagingLibrary.CognexVisionPro)
                    return true;
            }

            return false;
        }

        public static void InitStrategy(string strategyFileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Init Algorithm Strategy");

            if (File.Exists(strategyFileName) == true)
            {
                LoadStrategy(strategyFileName);
            }
            else
            {
                SaveStrategy(strategyFileName);
            }
        }

        private static void LoadStrategy(string strategyFileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Load Algorithm Strategy");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(strategyFileName);

            XmlElement strategyListElement = xmlDocument.DocumentElement;
            foreach (XmlElement strategyElement in strategyListElement)
            {
                if (strategyElement.Name == "AlgorithmStrategy")
                {
                    string algorithmType = XmlHelper.GetValue(strategyElement, "AlgorithmType", "PatternMatching");
                    AlgorithmStrategy algorithmStrategy = GetStrategy(algorithmType);
                    if (algorithmStrategy != null)
                    {
                        algorithmStrategy.LibraryType = (ImagingLibrary)Enum.Parse(typeof(ImagingLibrary), XmlHelper.GetValue(strategyElement, "LibraryType", "OpenCv"));
                        algorithmStrategy.SubLibraryType = XmlHelper.GetValue(strategyElement, "SubLibraryType", "");
                    }
                }
            }
        }

        public static void SaveStrategy(string strategyFileName)
        {
            LogHelper.Debug(LoggerType.StartUp, "Save Algorithm Strategy");

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement strategyListElement = xmlDocument.CreateElement("", "AlgorithmStrategyList", "");
            xmlDocument.AppendChild(strategyListElement);

            foreach (AlgorithmStrategy algorithmStrategy in algorithmStrategyList)
            {
                XmlElement strategyElement = xmlDocument.CreateElement("", "AlgorithmStrategy", "");
                strategyListElement.AppendChild(strategyElement);

                XmlHelper.SetValue(strategyElement, "AlgorithmType", algorithmStrategy.AlgorithmType);
                XmlHelper.SetValue(strategyElement, "LibraryType", algorithmStrategy.LibraryType.ToString());
                XmlHelper.SetValue(strategyElement, "SubLibraryType", algorithmStrategy.SubLibraryType);
            }

            xmlDocument.Save(strategyFileName);
        }

        public static bool IsAlgorithmEnabled(string algorithmType)
        {
            AlgorithmStrategy findedStrategy = GetStrategy(algorithmType);
            if (findedStrategy != null)
            {
                return findedStrategy.Enabled;
            }

            return false;
        }

        public static void SetAlgorithmEnabled(string algorithmType, bool enabled)
        {
            AlgorithmStrategy findedStrategy = GetStrategy(algorithmType);
            if (findedStrategy != null)
            {
                findedStrategy.Enabled = enabled;
            }
            else
            {
                LogHelper.Error(LoggerType.Error, String.Format("Algorithm License Error : {0}", algorithmType));
                licenseErrorCount++;
            }
        }

        public static AlgorithmStrategy GetStrategy(string algorithmType)
        {
            AlgorithmStrategy findedStrategy = algorithmStrategyList.Find(delegate (AlgorithmStrategy strategy) { return strategy.AlgorithmType == algorithmType; });
            if (findedStrategy != null)
            {
                if (LicenseManager.LicenseExist(findedStrategy.LibraryType, findedStrategy.SubLibraryType) == true)
                    return findedStrategy;
            }

            return null;
        }
        
        public static ImageProcessing GetImageProcessing(ImagingLibrary imagingLibrary)
        {
            switch (imagingLibrary)
            {
                case ImagingLibrary.EuresysOpenEVision:
                    return openEVisionImageProcessing;
                case ImagingLibrary.MatroxMIL:
                    return milImageProcessing;
            }

            return null;
        }

        public static ImageProcessing GetImageProcessing(AlgoImage algoImage)
        {
             return GetImageProcessing(algoImage.LibraryType);
        }
    }
}
