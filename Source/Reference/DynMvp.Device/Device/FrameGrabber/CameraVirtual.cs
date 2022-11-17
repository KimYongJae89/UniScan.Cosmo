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
using DynMvp.UI;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace DynMvp.Devices.FrameGrabber
{
    public delegate bool OnGrabTimerDelegate();
    public class CameraVirtual : Camera
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public ulong GrabbedCount
        {
            get { return grabbedCount; }
        }

        public int SkipFrame
        {
            get { return skipFrame; }
            set { skipFrame = value; }
        }

        public string VirtualSourceImageNameFilter
        {
            get { return virtualSourceImageNameFilter; }
            set { virtualSourceImageNameFilter = value; }
        }

        public override bool IsVirtual => true;
        /// <summary>
        /// Grab Timer 경과 후 가상 이미지가 준비될대까지 Grab 차단
        /// timer가 빠른 경우 이후 이미지가 이전 이미지를 앞서 ImageGrabbed 처리되는 경우 있음 (timer callback은 한번에 하나씩만 처리)
        /// </summary>
        protected bool virtualCameraReady = false;
        protected Timer callbackTimer = new Timer();
        protected bool requestStopGrab = false;
        protected ulong grabbedCount = 0;
        protected int skipFrame = 0;

        protected string virtualSourceImageNameFilter = "*.bmp";
        protected Dictionary<string, ImageD> virtualSoruceImageDic = null;
        protected int virtualOutputImageIndex = -1;

        public override void Initialize(CameraInfo cameraInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize Virtual Camera");

            base.Initialize(cameraInfo);
            
            ImageSize = new Size(cameraInfo.Width, cameraInfo.Height);           
            NumOfBand = cameraInfo.PixelFormat == PixelFormat.Format8bppIndexed ? 1 : 3;
            ImagePitch = cameraInfo.Width * NumOfBand;

            virtualSoruceImageDic = new Dictionary<string, ImageD>();

            string[] vImagePaths = Directory.GetFiles(cameraInfo.VirtualImagePath, virtualSourceImageNameFilter, SearchOption.TopDirectoryOnly);
            if (vImagePaths.Length == 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("There is no Exist Image Files");
                sb.AppendLine(cameraInfo.VirtualImagePath);
                MessageForm.Show(null, sb.ToString());
            }
            else
            {
                // loadNow: 가상 이미지를 바로 로드한다. NO이면 Grab 할 때 로드한다.
                bool loadNow = MessageForm.Show(null, StringManager.GetString("Load VirtualImage NOW?"), "UniScan", MessageFormType.YesNo, 3000, System.Windows.Forms.DialogResult.No) == System.Windows.Forms.DialogResult.Yes;

                bool loadDone = false;
                ProgressForm progressForm = new ProgressForm();
                progressForm.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                progressForm.TitleText = StringManager.GetString("Load Image");
                progressForm.MessageText = string.Format("0 / {0}", vImagePaths.Length);

                progressForm.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler((sender, arg) =>
                {
                    BackgroundWorker worker = (BackgroundWorker)sender;
                    for (int i = 0; i < vImagePaths.Length; i++)
                        //foreach (string imageFilePath in vImagePaths)
                        {
                        if (worker.CancellationPending)
                        {
                            arg.Cancel = true;
                            break;
                        }

                        worker.ReportProgress((i + 1) * 100 / vImagePaths.Length);
                        progressForm.MessageText = string.Format("{0} / {1}", i + 1, vImagePaths.Length);
                        string imageFilePath = vImagePaths[i];
                        LogHelper.Debug(LoggerType.StartUp, string.Format("CameraVirtual::Initialize - {0}", imageFilePath));
                        Image2D image2D = null;
                        if (loadNow)
                        {
                            image2D = new Image2D();
                            image2D.LoadImage(imageFilePath);
                        }
                        virtualSoruceImageDic.Add(imageFilePath, image2D);
                    }
                });

                progressForm.ShowDialog();
            }

            virtualOutputImageIndex = -1;

            callbackTimer.Interval = 1000;
            callbackTimer.Elapsed += new ElapsedEventHandler(callbackTimer_Elapsed);
        }

        public void FilterVirtualSource(string searchPattern)
        {
            List<string> pathList = virtualSoruceImageDic.Keys.ToList();
            if (pathList.Count == 0)
                return;
            pathList.RemoveAll(f => !Regex.IsMatch(f, searchPattern));
            pathList.ForEach(f => virtualSoruceImageDic.Remove(f));
        }

        public override bool SetStepLight(int stepNo, int lightNo)
        {
            string searchPattern = String.Format("Image_C{0:00}_S{1:000}_L{2:00}", Index, stepNo, lightNo);

            int idx = this.virtualSoruceImageDic.ToList().FindIndex(f => Path.GetFileNameWithoutExtension(f.Key) == searchPattern);
            if (idx == -1)
            {
                Debug.WriteLine(string.Format("CameraVirtual::SetSetpLight({0}, {1}) Fail",stepNo,lightNo));
                virtualOutputImageIndex = -1;
                return false;
            }

            virtualOutputImageIndex = idx - 1;
            return true;
        }

        public override void Release()
        {
            base.Release();

            callbackTimer.Stop();

            foreach (KeyValuePair<string, ImageD> pair in virtualSoruceImageDic)
            {
                pair.Value?.Dispose();
            }
            virtualSoruceImageDic.Clear();
        }

        public override void SetTriggerMode(TriggerMode triggerMode, TriggerType triggerType)
        {
            base.SetTriggerMode(triggerMode, triggerType);
        }

        public override void SetTriggerDelay(int triggerDelay)
        {

        }

        public override ImageD GetGrabbedImage(IntPtr ptr)
        {
            if (virtualOutputImageIndex < 0)
                return null;

            int imageIdx = virtualOutputImageIndex;
            if (ptr != IntPtr.Zero)
                imageIdx = (int)ptr - 1;

            KeyValuePair<string, ImageD> pair = this.virtualSoruceImageDic.ElementAt(imageIdx);
            ImageD image = this.virtualSoruceImageDic[pair.Key];
            Debug.Assert(image != null);
            return image;
        }

        public override void GrabOnce()
        {
            if (this.SetupGrab())
            {
                this.grabbedCount = 0;
                this.grabCount = 1;
                //this.SetDeviceExposure(this.exposureTimeUs / 1000);
                requestStopGrab = false;
                virtualCameraReady = true;
                callbackTimer.Start();
            }
        }

        public override void GrabMulti(int grabCount = CONTINUOUS)
        {
            if (this.SetupGrab())
            {
                this.grabbedCount = 0;
                this.grabCount = grabCount;
                //this.SetDeviceExposure(this.exposureTimeUs / 1000);

                requestStopGrab = false;
                virtualCameraReady = true;
                callbackTimer.Start();
            }
        }

        public override void SetStopFlag()
        {
            LogHelper.Debug(LoggerType.Grab, String.Format("Stop Continuous {0}", Index));
            requestStopGrab = true;
            Stop();
        }

        public override void Stop()
        {
            base.Stop();
            callbackTimer.Stop();
            grabCount = 0;
        }

        //DateTime dt = DateTime.MinValue;
        void callbackTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            //DateTime now = DateTime.Now;
            //Debug.WriteLine(string.Format("callbackTimer_Elapsed - {0}", (now - dt).ToString(@"ss\.fff")));
            //dt = now;

            LogHelper.Debug(LoggerType.Function, "CameraVirtual::callbackTimer_Elapsed");
            if (this.exposureTimeUs > 0)
            {
                //callbackTimer.Stop();
                //this.callbackTimer.Interval = this.exposureTimeUs / 1000;
                //callbackTimer.Start();
            }

            if (virtualCameraReady == false)
                return;

            virtualCameraReady = false;

            int bufferId;
            lock (this)
            {
                grabbedCount++;
                bufferId = UpdateVirtualImage();

                if (grabbedCount == (ulong)grabCount) //grabonce(...) , grabmulti(...)
                    callbackTimer.Stop();
            }
            this.isGrabbed.Set();

            if (skipFrame > 0)
            {
                skipFrame--;
                LogHelper.Debug(LoggerType.Function, string.Format("CameraVirtual::callbackTimer_Elapsed - Skip Frame (remain: {0})", skipFrame));
            }
            else
                ImageGrabbedCallback((IntPtr)bufferId + 1);  // ptr == grabbed image index. 1-base

            virtualCameraReady = true;
            this.isGrabDone.Set();
        }

        protected virtual int UpdateVirtualImage()
        {
            LogHelper.Debug(LoggerType.Function, "CameraVirtual::UpdateVirtualImage");
            virtualOutputImageIndex = (virtualOutputImageIndex + 1) % this.virtualSoruceImageDic.Count;
            GetVirtualSourceImage(virtualOutputImageIndex);
            return virtualOutputImageIndex;
        }

        protected ImageD GetVirtualSourceImage(int index)
        {
            KeyValuePair<string, ImageD> pair = this.virtualSoruceImageDic.ElementAt(index);
            if (pair.Value == null)
            {
                this.virtualSoruceImageDic[pair.Key] = new Image2D(pair.Key);
            }
            return this.virtualSoruceImageDic[pair.Key];
        }

        public override bool SetDeviceExposure(float exposureTimeMs)
        {
            if (exposureTimeMs < 0)
                return false;

            this.exposureTimeUs = exposureTimeMs * 1000;
            callbackTimer.Interval = Math.Max(1, (int)exposureTimeMs);

            return true;
        }

        public override List<ImageD> GetImageBufferList()
        {
            throw new NotImplementedException();
        }

        public override float GetDeviceExposureMs()
        {
            return this.exposureTimeUs/1000;
        }

        public override bool SetAcquisitionLineRate(float hz)
        {
            if (hz <= 0)
                return false;

            this.SetExposureTime(1E6f / hz * this.ImageSize.Height);
            return true;
        }

        public override float GetAcquisitionLineRate()
        {
            return (this.ImageSize.Height * 1E6f) / this.exposureTimeUs;
        }

        public override void SetScanMode(ScanMode scanMode)
        {
        }
    }
}
