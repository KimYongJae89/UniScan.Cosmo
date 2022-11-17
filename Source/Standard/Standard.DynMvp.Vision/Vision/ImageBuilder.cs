using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using Standard.DynMvp.Base;
using Standard.DynMvp.Vision.Euresys;
using Standard.DynMvp.Vision.Matrox;

namespace Standard.DynMvp.Vision
{
    public enum ImageType
    {
        Grey, Color, Depth, Gpu
    }

    public abstract class ImageBuilder
    {
        static OpenEVisionImageBuilder openEVisionImageBuilder = new OpenEVisionImageBuilder();
        public static OpenEVisionImageBuilder OpenEVisionImageBuilder
        {
            get { return ImageBuilder.openEVisionImageBuilder; }
        }

        static MilImageBuilder milImageBuilder = new MilImageBuilder();
        public static MilImageBuilder MilImageBuilder
        {
            get { return ImageBuilder.milImageBuilder; }
        }

        abstract public bool IsCudaAvailable();

        public static ImageBuilder GetInstance(ImagingLibrary libraryType)
        {
            switch (libraryType)
            {
                case ImagingLibrary.EuresysOpenEVision:
                    return OpenEVisionImageBuilder;
                case ImagingLibrary.MatroxMIL:
                    return milImageBuilder;
                default:
                    throw new InvalidTypeException();
            }
        }

        public static AlgoImage Build(AlgoImage algoImage)
        {
            return Build(algoImage.LibraryType, algoImage.ImageType, algoImage.Width, algoImage.Height);
        }

        public static bool CheckImageType(AlgorithmStrategy strategy, ref ImagingLibrary libraryType, ref ImageType imageType)
        {
            if (strategy != null)
            {
                libraryType = strategy.LibraryType;
                //if (strategy.ImageType != ImageType.Gpu)
                    imageType = strategy.ImageType;
            }

            return true;
        }

        public static AlgoImage Build(ImagingLibrary libraryType, ImageType imageType, Size size, ImageBandType imageBand = ImageBandType.Luminance)
        {
            //CheckImageType(null, ref libraryType, ref imageType);
            return GetInstance(libraryType).Build(imageType, size.Width, size.Height);
        }

        public static AlgoImage Build(ImagingLibrary libraryType, ImageType imageType, int width, int height, ImageBandType imageBand = ImageBandType.Luminance)
        {
            //CheckImageType(null, ref libraryType, ref imageType);
            return GetInstance(libraryType).Build(imageType, width, height);
        }

        public static AlgoImage Build(ImagingLibrary libraryType, ImageD image, ImageType imageType, ImageBandType imageBand = ImageBandType.Luminance)
        {
            CheckImageType(null, ref libraryType, ref imageType);
            AlgoImage algoImage = GetInstance(libraryType).Build(image, imageType, imageBand);
            algoImage.Tag = image.Tag;
            return algoImage;
        }

        public static AlgoImage Build(string algorithmType, ImageType imageType, int width, int height)
        {
            AlgorithmStrategy strategy = AlgorithmBuilder.GetStrategy(algorithmType);
            //CheckImageType(strategy, ref libraryType, ref imageType);

            return GetInstance(strategy.LibraryType).Build(imageType, width, height);
        }

        public static AlgoImage Build(string algorithmType, int width, int height)
        {
            AlgorithmStrategy strategy = AlgorithmBuilder.GetStrategy(algorithmType);
            //CheckImageType(strategy, ref libraryType, ref imageType);

            return GetInstance(strategy.LibraryType).Build(strategy.ImageType, width, height);

        }
        public static AlgoImage Build(string algorithmType, ImageD image, ImageType imageType, ImageBandType imageBand = ImageBandType.Luminance)
        {
            AlgorithmStrategy strategy = AlgorithmBuilder.GetStrategy(algorithmType);

            //ImagingLibrary libraryType = ImagingLibrary.OpenCv;
            //CheckImageType(strategy, ref libraryType, ref imageType);

            return GetInstance(strategy.LibraryType).Build(image, imageType, imageBand);
        }

        public static AlgoImage Build(string algorithmType, ImageD image, ImageBandType imageBand = ImageBandType.Luminance)
        {
            AlgorithmStrategy strategy = AlgorithmBuilder.GetStrategy(algorithmType);

            //ImagingLibrary libraryType = ImagingLibrary.OpenCv;
            //CheckImageType(strategy, ref libraryType, ref imageType);

            return GetInstance(strategy.LibraryType).Build(image, strategy.ImageType, imageBand);
        }

        public abstract AlgoImage Build(ImageType imageType, int width, int height);
        public abstract AlgoImage Build(ImageD image, ImageType imageType,ImageBandType imageBand = ImageBandType.Luminance);
    }
}
