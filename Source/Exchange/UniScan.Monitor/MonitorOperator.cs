using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using UniEye.Base.MachineInterface;
using UniScan.Common;
using UniScan.Common.Data;
using UniScan.Common.Exchange;
using UniScan.Common.UI;
using UniScan.Common.Util;
using UniScan.Monitor.Exchange;
using UniScan.Monitor.Settings.Monitor;
using UniScanG.Data;

namespace UniScan.Monitor
{
    internal class MonitorOperator : ExchangeOperator, IServerExchangeOperator, IUserHandlerListener
    {
        List<IVisitListener> visitListenerList = new List<IVisitListener>();

        Server server;
        public Server Server
        {
            get { return server; }
        }

        public MonitorOperator()
        {
            this.server = new Server(MonitorSystemSettings.Instance().ServerSetting);
        }

        public override void Initialize()
        {
            this.server.Initialize();
            UserHandler.Instance().AddListener(this);
        }

        public override bool ModelTrained(ModelDescription modelDescription)
        {
            modelDescription.IsTrained = server.ModelTrained(modelDescription);

            return modelDescription.IsTrained;
        }
        
        public override void ModelTeachDone(int camId)
        {
            base.ModelTeachDone(camId);
        }

        //delegate void SyncModelDelegate(int camId);
        public void SyncModel(int camId)
        {
            //if (MonitorConfigHelper.Instance().MainForm.InvokeRequired)
            //{
            //    MonitorConfigHelper.Instance().MainForm.Invoke(new SyncModelDelegate(SyncModel), camId);
            //    return;
            //}

            UniScanG.UI.Etc.ProgressForm progressForm = new UniScanG.UI.Etc.ProgressForm();
            
            progressForm.TitleText = StringManager.GetString(this.GetType().FullName, "Sync") + string.Format(" (Cam - {0})", camId + 1);
            progressForm.MessageText = StringManager.GetString(this.GetType().FullName, "Model Sync");
            progressForm.BackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(SyncModel);
            progressForm.BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SyncModelComplete);
            progressForm.Argument = camId;
            
            //progressForm.TopLevel = true;
            progressForm.TopMost = true;

            progressForm.ShowDialog();
            
            if (syncDone == false)
            {
                MessageForm.Show(MonitorConfigHelper.Instance().MainForm, StringManager.GetString("Model Sync Fail..."));
                return;
            }

            string[] modelDiscArgs = ((ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).GetArgs();

            List<string> argList = new List<string>();
            argList.Add(camId.ToString());
            argList.Add("-1");
            //argList.AddRange(modelDiscArgs);
            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.M_RESELECT, argList.ToArray());
        }

        bool syncDone = false;
        private void SyncModel(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int camIndex = (int)e.Argument;

            UniScanG.Data.Model.Model curModel = SystemManager.Instance().CurrentModel as UniScanG.Data.Model.Model;
            List<InspectorObj> targetInspectorObjList = GetInspectorList().FindAll(obj => obj.Info.CamIndex == camIndex && obj.Info.ClientIndex != 0);
            InspectorObj baseInspectorObj = GetInspectorList().Find(f => f.Info.CamIndex == camIndex && f.Info.ClientIndex <= 0);

            if (baseInspectorObj == null)
            {
                e.Result = string.Format(StringManager.GetString("Can not found master device of {0}"), baseInspectorObj.Info.GetName());
                return;
            }

            worker?.ReportProgress(0, string.Format("0 / {0}", targetInspectorObjList.Count));
            string srcModelPath = baseInspectorObj.ModelManager.GetModelPath(curModel.ModelDescription);
            string srcCommonFile = Path.GetFullPath(Path.Combine(srcModelPath, "..", "..", "..", "..", "Config", "AlgorithmSetting.xml"));
            for (int i = 0; i < targetInspectorObjList.Count; i++)
            {
                InspectorObj targetInspectorObj = targetInspectorObjList[i];
                bool exist = curModel != null && targetInspectorObj.ModelManager.IsModelExist(curModel.ModelDescription);
                if (exist == false)
                {
                    e.Result = string.Format(StringManager.GetString("Model is NOT exist in {0}"), targetInspectorObj.Info.GetName());
                    return;
                }

                //bool isTrained = baseInspectorObj.IsTrained(curModel.ModelDescription);
                //if (isTrained == true)
                {
                    string dstModelPath = targetInspectorObj.ModelManager.GetModelPath(curModel.ModelDescription);
                    string dstCommonFile = Path.GetFullPath(Path.Combine(dstModelPath, "..", "..", "..", "..", "Config", "AlgorithmSetting.xml"));
                    if (srcModelPath != dstModelPath)
                        FileHelper.CopyDirectory(srcModelPath, dstModelPath, false, true);

                    if (srcCommonFile != dstCommonFile)
                        FileHelper.CopyFile(srcCommonFile, dstCommonFile, true);
                }

                worker?.ReportProgress((i + 1) * 100 / targetInspectorObjList.Count, string.Format("{0} / {1}", i+1, targetInspectorObjList.Count));
            }
        }

        private void SyncModelComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            Exception ex = e.Error;
            string message = (string)e.Result;
            if (string.IsNullOrEmpty(message) && ex == null)
                syncDone = true;
            else
                syncDone = false;
        }

        public override bool ModelExist(ModelDescription modelDescription)
        {
            if (server == null)
                return false;

            return server.ModelExist(modelDescription);
        }

        public override bool SelectModel(ModelDescription modelDescription)
        {
            server.SelectModel(modelDescription);

            return base.SelectModel(modelDescription);
        }

        public override void DeleteModel(ModelDescription modelDescription)
        {
            server.DeleteModel(modelDescription);

            base.DeleteModel(modelDescription);
        }

        public override bool NewModel(ModelDescription modelDescription)
        {
            server.NewModel(modelDescription);

            return base.NewModel(modelDescription);
        }

        public List<InspectorObj> GetInspectorList(int sheetNo = -1)
        {
            return server.InspectorList.FindAll(f => f.IsInspectable(sheetNo));
        }

        public void CloseVnc()
        {
            server.SendVisit(ExchangeCommand.V_DONE);
        }

        public Process OpenVnc(ExchangeCommand eVisit, Process process, string ipAddress, IntPtr handle)
        {
            server.SendVisit(eVisit);

            return VncHelper.OpenVnc(process, ipAddress, handle, MonitorSystemSettings.Instance().VncPath);
        }

        public override void SendCommand(ExchangeCommand exchangeCommand, params string[] args)
        {
            server.SendCommand(exchangeCommand, args);
        }

        public bool ModelTrained(int camIndex, int clientIndex, ModelDescription modelDescription)
        {
            return server.ModelTrained(camIndex, clientIndex, modelDescription);
        }

        public void UserChanged()
        {
            server.SendCommand(ExchangeCommand.U_CHANGE, UserHandler.Instance().CurrentUser.Id);
        }

        public override int GetCamIndex()
        {
            return -1;
        }

        public override int GetClientIndex()
        {
            return -1;
        }

        public override bool SaveModel()
        {
            Model model = SystemManager.Instance().CurrentModel;
            if (model == null)
                return false;

            Bitmap image = SheetCombiner.CreateModelImage(SystemManager.Instance().CurrentModel.ModelDescription);
            if (image != null)
            {
                string imagePath = SystemManager.Instance().ModelManager.GetPreviewImagePath(SystemManager.Instance().CurrentModel.ModelDescription);
                ImageHelper.SaveImage(image, imagePath);
            }

            model.ModelDescription.IsTrained = server.ModelTrained(SystemManager.Instance().CurrentModel.ModelDescription);
            model.Modified = true;
            return SystemManager.Instance().ModelManager.SaveModel(model);
        }
    }
}
