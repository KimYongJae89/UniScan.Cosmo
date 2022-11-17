using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Standard.DynMvp.Vision
{
    public static class ImageProcessingFactory
    {
        public static ImageProcessing CreateImageProcessing(ImagingLibrary libraryType)
        {
            switch (libraryType)
            {
                case ImagingLibrary.EuresysOpenEVision:
                    return new Euresys.OpenEVisionImageProcessing();
                case ImagingLibrary.MatroxMIL:
                    return new Matrox.MilImageProcessing();
                default:
                    return null;
            }
        }
    }
}
