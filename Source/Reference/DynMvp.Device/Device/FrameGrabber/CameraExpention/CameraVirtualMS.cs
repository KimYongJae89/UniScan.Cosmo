using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using System.Runtime.InteropServices;

using DynMvp.Base;

using System.Diagnostics;
using DynMvp.UI.Touch;
using System.Windows.Forms;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices;
using System.Threading;
using DynMvp.Device.Device.FrameGrabber;

namespace DynMvp.Devices.FrameGrabber
{
    public class DialogState
    {
        public DialogResult result;
        public FileDialog dialog;

        public void ThreadProcShowDialog()
        {
            result = dialog.ShowDialog();
        }
    }

    public class CameraVirtualMS : CameraVirtual
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        protected object onCreating = new object();

        List<ImageD> virtualImageList = null;

        int copySrcImageStartIdx = -1;
        int copySrcImageStartLineNo = 0;

        protected int lineModePitch;
        public int LineModePitch
        { get { return lineModePitch; } }

        protected Size lineModeSize;
        public Size LineModeSize
        { get { return lineModeSize; } }

        protected Size areaModeSize;
        public Size AreaModeSize
        { get { return areaModeSize; } }

        int bufferOutputImageIndex = -1;
        public int BufferOutputImageIndex
        { get { return bufferOutputImageIndex; }

        }

        public CameraVirtualMS() : base()
        {
            this.virtualImageList = new List<ImageD>();
        }

        public override void SetScanMode(ScanMode scanMode)
        {
            switch (scanMode)
            {
                case ScanMode.Area:
                    ImageSize = areaModeSize;
                    exposureTimeUs *= areaModeSize.Height * 1.0f / lineModeSize.Height;
                    break;
                case ScanMode.Line:
                    ImageSize = lineModeSize;
                    exposureTimeUs *= lineModeSize.Height * 1.0f / areaModeSize.Height;
                    break;
            }
        }

        [STAThreadAttribute]
        public override void Initialize(CameraInfo cameraInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize Virtual CameraMS");

            base.Initialize(cameraInfo);

            int frameBufferSize = 10;
            
            if (cameraInfo is CameraInfoGenTL)
                frameBufferSize = (int)((CameraInfoGenTL)cameraInfo).FrameNum * 5;

            for (int i = 0; i < frameBufferSize; i++)
                this.virtualImageList.Add(null);

            lineModeSize = ImageSize;
            lineModePitch = lineModeSize.Width * NumOfBand;

            areaModeSize = new Size(ImageSize.Width, 128);

            SetScanMode(ScanMode.Line);
        }

        //private DialogResult STAShowDialog(OpenFileDialog dialog)
        //{
        //    DialogState state = new DialogState();
        //    state.dialog = dialog;
        //    System.Threading.Thread t = new System.Threading.Thread(state.ThreadProcShowDialog);
        //    t.SetApartmentState(System.Threading.ApartmentState.STA);
        //    t.Start();
        //    t.Join();
        //    return state.result;
        //}

        public override void Release()
        {
            callbackTimer.Stop();
            base.Release();
        }

        protected override int UpdateVirtualImage()
        {
            LogHelper.Debug(LoggerType.Function, "CameraVirtualMS::UpdateVirtualImage");
            virtualOutputImageIndex = (virtualOutputImageIndex + 1) % this.virtualSoruceImageDic.Count;
            bufferOutputImageIndex = (bufferOutputImageIndex + 1) % this.virtualImageList.Count;

            ImageD imageD = this.virtualImageList[bufferOutputImageIndex];
            MakeNextVirtualImage(ref imageD);
            imageD.Tag = new CameraBufferTag(bufferOutputImageIndex, this.grabbedCount - 1);
            this.virtualImageList[bufferOutputImageIndex] = imageD;

            return bufferOutputImageIndex;
        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            if (this.bufferOutputImageIndex < 0)
                return null;

            int imageIdx = this.bufferOutputImageIndex;
            if (ptr != IntPtr.Zero)
                imageIdx = (int)ptr - 1;

            ImageD imageD = this.virtualImageList[imageIdx];
            Debug.Assert(imageD != null);
            return imageD;
        }

        /// <summary>
        /// 높이가 다른이미지를 src-> dst로 카피, src가 더 작으면 다시 처음 영상으로 이어 붙여줌
        /// 다음 호출시 이전 offset 영상 위치를 기억하여 거기부터 만들어줌.
        /// </summary>
        /// <param name="dstImage"></param>
        private bool MakeNextVirtualImage(ref ImageD imageD)     //184~186, 372,
        {
            int LastYOffset = 0;
            if (virtualOutputImageIndex <0) virtualOutputImageIndex=0;
            if (copySrcImageStartIdx < 0)
            {
                copySrcImageStartIdx = 0;
                copySrcImageStartLineNo = 0;
            }

            if (Monitor.TryEnter(onCreating) == false)
                return false;

            if (imageD == null)
                imageD = (ImageD)this.CreateCompatibleImage();

            int srcImageIdx = copySrcImageStartIdx;
            int YoffsetSrc = copySrcImageStartLineNo;
            int YoffsetDst = 0; //0 base
            while (YoffsetDst < imageD.Height)
            {
                ImageD srcImage = base.GetVirtualSourceImage(srcImageIdx);

                int rectWidth = Math.Min(srcImage.Width, imageD.Width);

                int srcHeight = srcImage.Height - YoffsetSrc;
                int dstHeight = imageD.Height - YoffsetDst;
                int rectHeight = Math.Min(srcHeight, dstHeight);

                Rectangle srcRect = new Rectangle(0, YoffsetSrc, rectWidth, rectHeight);
                Rectangle dstRect = new Rectangle(0, YoffsetDst, rectWidth, rectHeight);

                imageD.CopyFrom(srcImage, srcRect, srcImage.Pitch, dstRect.Location);

                int befr = YoffsetSrc;
                YoffsetSrc = YoffsetSrc + rectHeight;
                YoffsetDst = YoffsetDst + rectHeight;

                if (YoffsetSrc == srcImage.Height)  //다음 이미지 파일로 작업
                {
                    srcImageIdx = (++srcImageIdx % this.virtualSoruceImageDic.Count);
                    YoffsetSrc = 0;
                }
                LastYOffset = YoffsetSrc;
            }
            copySrcImageStartIdx = srcImageIdx;
            copySrcImageStartLineNo = LastYOffset;

            Monitor.Exit(onCreating);
            return true;
        }

        public override List<ImageD> GetImageBufferList()
        {
            List<ImageD> imageList = new List<ImageD>();
            imageList.AddRange(this.virtualImageList);
            return imageList;
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            return true;
            //float exp = exposureTimeMs * this.ImageSize.Height;
            //return base.SetDeviceExposure(exp);
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            if (hz <= 0)
                return false;

            base.SetDeviceExposure(1E3f / hz * this.ImageSize.Height);
            return true;
        }
    }
}
