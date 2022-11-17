using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;
using Euresys.MultiCam;
using DynMvp.Devices.FrameGrabber.UI;

namespace DynMvp.Devices.FrameGrabber
{
    public class CameraInfoMultiCam : CameraInfo
    {
        EuresysBoardType boardType;
        public EuresysBoardType BoardType
        {
            get { return boardType; }
        }

        uint boardId;
        public uint BoardId
        {
            get { return boardId; }
        }

        uint connectorId;
        public uint ConnectorId
        {
            get { return connectorId; }
        }

        CameraType cameraType;
        public CameraType CameraType
        {
            get { return cameraType; }
        }

        uint surfaceNum;
        public uint SurfaceNum
        {
            get { return surfaceNum; }
        }

        uint pageLength;
        public uint PageLength
        {
            get { return pageLength; }
            set { pageLength = value; }
        }

        public CameraInfoMultiCam()
        {
            GrabberType = GrabberType.MultiCam;
        }

        public CameraInfoMultiCam(EuresysBoardType boardType, uint boardId, uint connectorId, CameraType cameraType, uint surfaceNum, uint pageLength)
        {
            this.GrabberType = GrabberType.MultiCam;

            this.boardType = boardType;
            this.boardId = boardId;
            this.connectorId = connectorId;
            this.cameraType = cameraType;
            this.surfaceNum = surfaceNum;
            this.pageLength = pageLength;
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            boardType = (EuresysBoardType)Enum.Parse(typeof(EuresysBoardType), XmlHelper.GetValue(cameraElement, "BoardType", "GrabLink_Base"));
            boardId = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "BoardId", "0"));
            connectorId = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "ConnectorId", "0"));
            cameraType = (CameraType)Enum.Parse(typeof(CameraType), XmlHelper.GetValue(cameraElement, "CameraType", "Jai_GO_5000"));
            surfaceNum = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "SurfaceNum", "1"));
            pageLength = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "PageLength", "0"));
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "BoardType", boardType.ToString());
            XmlHelper.SetValue(cameraElement, "BoardId", boardId.ToString());
            XmlHelper.SetValue(cameraElement, "ConnectorId", connectorId.ToString());
            XmlHelper.SetValue(cameraElement, "CameraType", cameraType.ToString());
            XmlHelper.SetValue(cameraElement, "SurfaceNum", surfaceNum.ToString());
            XmlHelper.SetValue(cameraElement, "PageLength", pageLength.ToString());
        }
    }

    public class GrabberMultiCam : Grabber
    {
        static int cntOpenDriver = 0;

        public GrabberMultiCam(string name) : base(GrabberType.MultiCam, name)
        {
        }

        public override Camera CreateCamera()
        {
            return new CameraMultiCam();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            EuresysBoardListForm form = new EuresysBoardListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize MultiCam Camera Manager");

            // Open MultiCam driver
            if (cntOpenDriver == 0)
            {
                LogHelper.Debug(LoggerType.StartUp, "Open MultiCam Driver");
                MC.OpenDriver();
            }

            cntOpenDriver++;

            // Enable error logging
            MC.SetParam(MC.CONFIGURATION, "ErrorLog", "error.log");

            return true;
        }

        public override void Release()
        {
            base.Release();

            cntOpenDriver--;

            if (cntOpenDriver == 0)
            {
                LogHelper.Debug(LoggerType.Shutdown, "Release MultiCam Driver");
                MC.CloseDriver();
            }
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {

        }
    }
}
