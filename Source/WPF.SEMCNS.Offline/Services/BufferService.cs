using DynMvp.Devices.FrameGrabber;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace WPF.SEMCNS.Offline.Services
{
    public class BufferService : IDisposable
    {
        Camera _camera;
        
        AlgoImage _transposeImage;
        AlgoImage _transposeBuffer;
        AlgoImage _defectBuffer;
        byte[] _lowerArray;
        byte[] _upperArray;


        public Camera Camera { get => _camera; }
        public AlgoImage TransposeImage { get => _transposeImage; }
        public AlgoImage TransposeBuffer { get => _transposeBuffer; }
        public AlgoImage DefectBuffer { get => _defectBuffer; }

        public byte[] LowerArray { get => _lowerArray; }
        public byte[] UpperArray { get => _upperArray; }

        public BufferService(Camera camera)
        {
            _camera = camera;
            Initialize();
        }
        
        private void Initialize()
        {
            _transposeImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, _camera.CameraInfo.Height, _camera.CameraInfo.Width);
            _transposeBuffer = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, _camera.CameraInfo.Height, _camera.CameraInfo.Width);
            _defectBuffer = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, _camera.CameraInfo.Width, _camera.CameraInfo.Height);
            _lowerArray = new byte[_camera.CameraInfo.Width * _camera.CameraInfo.Height];
            _upperArray = new byte[_camera.CameraInfo.Width * _camera.CameraInfo.Height];
        }

        public void Dispose()
        {
            _transposeImage.Dispose();
            _transposeBuffer.Dispose();
            _defectBuffer.Dispose();
        }
    }
}
