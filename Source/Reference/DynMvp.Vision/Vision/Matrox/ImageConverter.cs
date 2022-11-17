using DynMvp.Base;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Matrox
{
    public class ImageConverter : DynMvp.Vision.ImageConverter
    {
        public override ConvertPack Pack(AlgoImage algoImage, bool usePtr)
        {
            ConvertPack pack = new ConvertPack();
            pack.ImageType = algoImage.ImageType;
            pack.Size = algoImage.Size;
            pack.Pitch = algoImage.Pitch;
            if (usePtr)
                pack.Ptr = algoImage.GetImagePtr();
            else
                pack.Bytes = algoImage.GetByte();

            return pack;
        }

        public override AlgoImage Unpack(ConvertPack convertPack, ImageType imageType)
        {
            if (convertPack.ImageType != imageType)
            // 포멧 변환 필요함
            {
                MIL_ID tempMilObject = MIL.M_NULL;
                MIL_INT type = 0;
                MIL_INT attribute = MIL.M_IMAGE + MIL.M_PROC;
                switch (convertPack.ImageType)
                {
                    case ImageType.Binary: type = 1 + MIL.M_UNSIGNED; break;
                    case ImageType.Grey: type = 8 + MIL.M_UNSIGNED; break;
                    case ImageType.Color: type = 24 + MIL.M_UNSIGNED; break;
                    case ImageType.Depth: type = 24 + MIL.M_FLOAT; break;
                    case ImageType.Gpu: type = 8 + MIL.M_UNSIGNED; break;
                }
                if (convertPack.Ptr == IntPtr.Zero)
                {
                    tempMilObject = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, convertPack.Width, convertPack.Height, type, attribute, MIL.M_NULL);
                    //int tempPitch = (int)MIL.MbufInquire(tempMilObject, MIL.M_PITCH_BYTE);
                    //if (convertPack.Pitch == tempPitch)
                    //{
                    //    MIL.MbufPut(tempMilObject, convertPack.Bytes);
                    //}
                    //else
                    //{
                    //    int blockSize = Math.Min(convertPack.Pitch, tempPitch);
                    //    for (int y = 0; y < convertPack.Height; y++)
                    //    {
                    //        int destOffset = y * tempPitch;
                    //        convertPack.Bytes.
                    //        MIL.MbufPut1d(tempMilObject, destOffset, blockSize, convertPack.Bytes.);
                    //    }
                    //}
                }
                else
                {
                    tempMilObject = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, convertPack.Width, convertPack.Height, type, attribute, MIL.M_HOST_ADDRESS + MIL.M_PITCH_BYTE, convertPack.Pitch, (ulong)convertPack.Ptr, MIL.M_NULL);
                }

                MilImage convertImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, imageType, convertPack.Size) as MilImage;
                MIL.MbufCopy(tempMilObject, convertImage.Image);

                MIL.MbufFree(tempMilObject);

                return convertImage;
            }
            else
            // 포멧 변환 필요 없음
            {
                Image2D image2D = null;
                if (convertPack.Ptr == null)
                    image2D = new Image2D(convertPack.Width, convertPack.Height, convertPack.NumBand, convertPack.Pitch, convertPack.Bytes);
                else
                    image2D = new Image2D(convertPack.Width, convertPack.Height, convertPack.NumBand, convertPack.Pitch, convertPack.Ptr);
                MilImage convertImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, image2D, imageType) as MilImage;
                return convertImage;
            }
        }

        public override void Unpack(ConvertPack convertPack, AlgoImage algoImage)
        {
            if (convertPack.Ptr == IntPtr.Zero)
                algoImage.PutByte(convertPack.Bytes);
            else
                algoImage.PutByte(convertPack.Ptr, convertPack.Pitch);
        }
    }
}
