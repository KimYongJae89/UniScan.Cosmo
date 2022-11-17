using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber.UI;

using Matrox.MatroxImagingLibrary;

namespace DynMvp.Devices.FrameGrabber
{
    public enum MilSystemType
    {
        Solios
    }

    public class CameraInfoMil : CameraInfo
    {
        MilSystemType systemType;
        public MilSystemType SystemType
        {
            get { return systemType; }
            set { systemType = value; }
        }

        uint systemNum;
        public uint SystemNum
        {
            get { return systemNum; }
            set { systemNum = value; }
        }

        // InitializeDevice 후, boardType, boardId로 할당된 SystemId를 저장하는 변수
        MilSystem milSystem;
        public MilSystem MilSystem
        {
            get { return milSystem; }
            set { milSystem = value; }
        }

        uint digitizerNum;
        public uint DigitizerNum
        {
            get { return digitizerNum; }
            set { digitizerNum = value; }
        }

        CameraType cameraType;
        public CameraType CameraType
        {
            get { return cameraType; }
        }

        public CameraInfoMil()
        {
            GrabberType = GrabberType.MIL;
        }

        public CameraInfoMil(MilSystemType systemType, uint systemNum, uint digitizerNum, CameraType cameraType)
        {
            this.GrabberType = GrabberType.MIL;

            this.systemType = systemType;
            this.systemNum = systemNum;
            this.digitizerNum = digitizerNum;
            this.cameraType = cameraType;
        }

        public override void LoadXml(XmlElement cameraElement)
        {
            base.LoadXml(cameraElement);

            systemType = (MilSystemType)Enum.Parse(typeof(MilSystemType), XmlHelper.GetValue(cameraElement, "SystemType", "Solios"));
            systemNum = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "SystemNum", "0"));
            digitizerNum = Convert.ToUInt32(XmlHelper.GetValue(cameraElement, "DigitizerNum", "0"));
            cameraType = (CameraType)Enum.Parse(typeof(CameraType), XmlHelper.GetValue(cameraElement, "CameraType", "PrimeTech_PXCB120VTH"));
        }

        public override void SaveXml(XmlElement cameraElement)
        {
            base.SaveXml(cameraElement);

            XmlHelper.SetValue(cameraElement, "SystemType", systemType.ToString());
            XmlHelper.SetValue(cameraElement, "SystemNum", systemNum.ToString());
            XmlHelper.SetValue(cameraElement, "DigitizerNum", digitizerNum.ToString());
            XmlHelper.SetValue(cameraElement, "CameraType", cameraType.ToString());
        }
    }

    public class MilSystem
    {
        string systemDescriptor;
        public string SystemDescriptor
        {
            get { return systemDescriptor; }
            set { systemDescriptor = value; }
        }

        uint systemNum;
        public uint SystemNum
        {
            get { return systemNum; }
            set { systemNum = value; }
        }

        MIL_ID systemId;
        public MIL_ID SystemId
        {
            get { return systemId; }
            set { systemId = value; }
        }
    }

    public class GrabberMil : Grabber
    {
        static List<MilSystem> milSystemList = new List<MilSystem>();

        public GrabberMil(string name) : base(GrabberType.MIL, name)
        {
            LogHelper.Debug(LoggerType.StartUp, "MIL Grabber is Created");
        }

        public override Camera CreateCamera()
        {
            return new CameraMil();
        }

        public override bool SetupCameraConfiguration(int numCamera, CameraConfiguration cameraConfiguration)
        {
            MatroxBoardListForm form = new MatroxBoardListForm();
            form.CameraConfiguration = cameraConfiguration;
            return form.ShowDialog() == System.Windows.Forms.DialogResult.OK;
        }

        static string GetSystemDescriptor(MilSystemType systemType)
        {
            if (systemType == MilSystemType.Solios)
                return MIL.M_SYSTEM_SOLIOS;

            return MIL.M_SYSTEM_SOLIOS;
        }

        public static MilSystem GetMilSystem(MilSystemType systemType, uint systemNum)
        {
            string systemDescriptor = GetSystemDescriptor(systemType);

            return GetMilSystem(systemDescriptor, systemNum);

        }

        static MilSystem GetMilSystem(string systemDescriptor, uint systemNum)
        {
            MilSystem milSystem = milSystemList.Find(x => x.SystemDescriptor == systemDescriptor && x.SystemNum == systemNum);
            if (milSystem == null)
                milSystem = CreateMilSystem(systemDescriptor, systemNum);

            return milSystem;
        }

        static MilSystem CreateMilSystem(string systemDescriptor, uint systemNum)
        {
            MilSystem milSystem = null;

            MIL_ID systemId = MIL.M_NULL;
            if (MIL.MsysAlloc(systemDescriptor, systemNum, MIL.M_DEFAULT, ref systemId) == MIL.M_NULL)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Can't Allocate MIL System. {0}, {1}", systemDescriptor, systemNum));
            }
            else
            {
                milSystem = new MilSystem();
                milSystem.SystemDescriptor = systemDescriptor;
                milSystem.SystemNum = systemNum;
                milSystem.SystemId = systemId; 
            }

            return milSystem;
        }

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            LogHelper.Debug(LoggerType.StartUp, "Initialize MultiCam Camera Manager");

            // MatroxHelper.InitApplication();

            return true;
        }

        public override void Release()
        {
            base.Release();

            LogHelper.Debug(LoggerType.Shutdown, "Release MilSystem");

            foreach(MilSystem milSystem in milSystemList)
            {
                MIL.MsysFree(milSystem.SystemId);
            }
        }

        public override void UpdateCameraInfo(CameraInfo cameraInfo)
        {

        }
    }
}
