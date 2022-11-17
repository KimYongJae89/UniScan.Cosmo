using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScan.Common;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.Data.Vision;
using UniScanG.Screen.Data;
using UniScanG.Screen.Vision;
using UniScanG.Screen.Vision.Detector;
using UniScanG.Screen.Vision.Trainer;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach;
using UniScanG.Vision;

namespace UniScanG.UI.Teach
{
    public delegate void ImageUpdatedDelegate(Image image);
    public delegate void UpdateZoomDelegate(Rectangle viewPort);
    public delegate void UpdateFigureDelegate(Figure figure);
    public delegate void UpdateSheetResultDelegate(SheetResult sheetResult);
    public delegate void ExportDataDelegate();

    public abstract class ModellerPageExtender : UniEye.Base.UI.HwTriggerModellerPageExtender, IModelListener
    {
        protected Image2D currentImage;

        public ImageUpdatedDelegate ImageUpdated;
        public UpdateZoomDelegate UpdateZoom;
        public UpdateFigureDelegate UpdateFigure;
        public UpdateSheetResultDelegate UpdateSheetResult;
        public ExportDataDelegate ExportData;

        public Image2D CurrentImage
        {
            get { return this.currentImage; }
        }

        public ModellerPageExtender()
        {
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        public virtual void DataExport()
        {
            if (ExportData != null)
                ExportData();
        }

        public void LoadImage()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (currentImage != null)
                    currentImage.Dispose();

                SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
                simpleProgressForm.Show(() =>
                {
                    currentImage = new Image2D(dlg.FileName);
                    currentImage.ConvertFromData();
                });

                if (currentImage != null && UpdateImage != null)
                    UpdateImage(currentImage, false);
            }
        }

        //protected override void SetupGrab(LightParam lightParam)
        //{
        //    LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;

        //    foreach (LightCtrl light in lightCtrlHandler)
        //        light.TurnOn(lightParam.LightValue);

        //    ImageDevice imageDevice = GetImageDevice(deviceIndex);
        //    imageDevice.ImageGrabbed += ImageGrabbed;    
        //}

        public abstract bool GrabSheet(int count);
        public abstract void GrabFrame();
        
        public void Teach(object arg = null)
        {
            Etc.ProgressForm progressForm = new Etc.ProgressForm();
            progressForm.StartPosition = FormStartPosition.CenterScreen;

            progressForm.TitleText = StringManager.GetString(this.GetType().FullName, "Auto Teach");
            progressForm.MessageText = StringManager.GetString(this.GetType().FullName, "Start");
            
            progressForm.BackgroundWorker.DoWork += TeachBackgroundWorker_DoWork;
            progressForm.RunWorkerCompleted = TeachRunWorkerCompleted;
            progressForm.Argument = arg;

            progressForm.TopMost = true;
            
            progressForm.ShowDialog();
        }
        
        protected abstract void TeachBackgroundWorker_DoWork(object sender, DoWorkEventArgs e);
        protected abstract void TeachRunWorkerCompleted(bool result);
        
        public virtual void SaveModel()
        {
            UniScanG.Data.Model.Model currentModel = SystemManager.Instance().CurrentModel;

            SimpleProgressForm loadingForm = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Save Model"));
            loadingForm.Show(new Action(() =>
            {
                if (currentModel.Modified)
                {
                    SystemManager.Instance().ModelManager.SaveModel(currentModel);
                    SystemManager.Instance().ModelManager.SaveModelDescription(currentModel.ModelDescription);
                    
                    Gravure.Vision.AlgorithmSetting.Instance().Save(); // 여기. 일반화 수정 필요!!
                    UniEye.Base.Settings.AdditionalSettings.Instance().Save();
                    UniEye.Base.Settings.OperationSettings.Instance().Save();

                    string imagePath = currentModel.GetImagePath();
                    if (Directory.Exists(imagePath) == false)
                        Directory.CreateDirectory(imagePath);

                    if (currentImage != null)
                    {
                        string previewFileName = string.Format("prev.{0}", ImageFormat.Bmp.ToString());
                        Bitmap prevImage = SheetCombiner.CreatePrevImage(currentImage.ToBitmap());
                        ImageHelper.SaveImage(prevImage, Path.Combine(imagePath, previewFileName), ImageFormat.Bmp);
                        prevImage.Dispose();

                        int camIdx = SystemManager.Instance().ExchangeOperator.GetCamIndex();
                        string fullFileName = GetModelImageName();
                        //string fullFileName = SystemManager.Instance().CurrentModel.GetImageName(camIdx, 0, 0);
                        Bitmap fullImage = currentImage.ToBitmap();
                        ImageHelper.SaveImage(fullImage, Path.Combine(imagePath, fullFileName), ImageFormat.Bmp);
                        fullImage.Dispose();
                        SystemManager.Instance().ExchangeOperator.ModelTeachDone(camIdx);
                    }
                }
            }));

        }

        public abstract void Inspect();
        public abstract void Inspect(RegionInfo regionInfo);

        public abstract string GetModelImageName();

        public void ModelChanged()
        {
            string imagePath = SystemManager.Instance().CurrentModel.GetImagePath();

            string fileName = GetModelImageName();
            string filePath = Path.Combine(imagePath, fileName);

            if (currentImage != null)
                currentImage.Dispose();

            if (File.Exists(filePath))
            {
                currentImage = new Image2D(filePath);

                if (currentImage != null && UpdateImage != null)
                    UpdateImage(currentImage, true);
            }
            else
            {
                currentImage = null;
                UpdateImage?.Invoke(currentImage, true);
            }
        }


        public void ModelTeachDone(int camId) { }

        public void ModelRefreshed() { }
    }
}