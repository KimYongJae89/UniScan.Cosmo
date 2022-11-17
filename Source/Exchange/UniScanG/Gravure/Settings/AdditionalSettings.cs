using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using UniScan.Common;
using UniScan.Common.Data;

namespace UniScanG.Gravure.Settings
{

    public class AdditionalSettings : UniEye.Base.Settings.AdditionalSettings
    {
        // 운영 관련
        [LocalizedCategoryAttributeUniScanG("Operation"), LocalizedDisplayNameAttributeUniScanG("Encoder Resolution"), LocalizedDescriptionAttributeUniScanG("Encoder Resolution")]
        public float EncoderResolution { get => encoderResolution;  }
        [LocalizedCategoryAttributeUniScanG("Operation"), LocalizedDisplayNameAttributeUniScanG("Async Grab Use"), LocalizedDescriptionAttributeUniScanG("Async Grab Use")]
        public bool UseAsyncMode { get => useAsyncMode; set => useAsyncMode = value; }
        [LocalizedCategoryAttributeUniScanG("Operation"), LocalizedDisplayNameAttributeUniScanG("Async Grab Speed [Hz]"), LocalizedDescriptionAttributeUniScanG("Async Grab Speed [Hz]")]
        public float AsyncGrabHz { get => asyncGrabHz; set => asyncGrabHz = value; }
        [LocalizedCategoryAttributeUniScanG("Operation"), LocalizedDisplayNameAttributeUniScanG("Auto Teach"), LocalizedDescriptionAttributeUniScanG("Auto Teach")]
        public bool AutoTeach { get => autoTeach; set => autoTeach = value; }
        [LocalizedCategoryAttributeUniScanG("Operation"), LocalizedDisplayNameAttributeUniScanG("Auto Teach Timeout"), LocalizedDescriptionAttributeUniScanG("Auto Teach Timeout")]
        public int AutoTeachTimeout { get => autoTeachTimeout; set => autoTeachTimeout = value; }
        [LocalizedCategoryAttributeUniScanG("Operation"), LocalizedDisplayNameAttributeUniScanG("Buffer Allocation Timeout"), LocalizedDescriptionAttributeUniScanG("Buffer Allocation Timeout")]
        public int BufferAllocTimeout { get => bufferAllocTimeout; set => bufferAllocTimeout = value; }

        float encoderResolution = 14.0f;
        bool useAsyncMode = false;
        float asyncGrabHz = 1500;
        bool autoTeach = false;
        int autoTeachTimeout = 60000;
        int bufferAllocTimeout = 30000;

        // 반복불량 알람 관련
        //[LocalizedCategoryAttributeUniScanG("Normal Defect Alarm"), LocalizedDisplayNameAttributeUniScanG("NormalDefectAlaramUse"), LocalizedDescriptionAttributeUniScanG("NormalDefectAlaramUse")]
        //public bool UseNormalDefectAlaram { get => useNormalDefectAlaram; set => useNormalDefectAlaram = value; }
        //[LocalizedCategoryAttributeUniScanG("Normal Defect Alarm"), LocalizedDisplayNameAttributeUniScanG("Normal N [EA]"), LocalizedDescriptionAttributeUniScanG("Normal N [EA]")]
        //public int NormalN { get => normalN; set => normalN = value; }
        //[LocalizedCategoryAttributeUniScanG("Normal Defect Alarm"), LocalizedDisplayNameAttributeUniScanG("Normal R [%]"), LocalizedDescriptionAttributeUniScanG("Normal R [%]")]
        //public float NormalR { get => normalR; set => normalR = value; }

        //[LocalizedCategoryAttributeUniScanG("Repeated Defect Alarm"), LocalizedDisplayNameAttributeUniScanG("RepeatDefectAlaramUse"), LocalizedDescriptionAttributeUniScanG("RepeatDefectAlaramUse")]
        //public bool UseRepeatDefectAlaram { get => useRepeatDefectAlaram; set => useRepeatDefectAlaram = value; }
        //[LocalizedCategoryAttributeUniScanG("Repeated Defect Alarm"), LocalizedDisplayNameAttributeUniScanG("Repeat N [EA]"), LocalizedDescriptionAttributeUniScanG("Repeat N [EA]")]
        //public int RepeatN { get => repeatN; set => repeatN = value; }
        //[LocalizedCategoryAttributeUniScanG("Repeated Defect Alarm"), LocalizedDisplayNameAttributeUniScanG("Repeat R [%]"), LocalizedDescriptionAttributeUniScanG("Repeat R [%]")]
        //public float RepeatR { get => repeatR; set => repeatR = value; }

        //[LocalizedCategoryAttributeUniScanG("Continued Defect Alarm"), LocalizedDisplayNameAttributeUniScanG("ContinueDefectAlarmUse"), LocalizedDescriptionAttributeUniScanG("ContinueDefectAlarmUse")]
        //public bool UseContinueDefectAlarm { get => useContinueDefectAlarm; set => useContinueDefectAlarm = value; }
        //[LocalizedCategoryAttributeUniScanG("Continued Defect Alarm"), LocalizedDisplayNameAttributeUniScanG("Continue N [EA]"), LocalizedDescriptionAttributeUniScanG("Continue N [EA]")]
        //public int ContinueN { get => continueN; set => continueN = value; }
        //[LocalizedCategoryAttributeUniScanG("Continued Defect Alarm"), LocalizedDisplayNameAttributeUniScanG("Continue R [%]"), LocalizedDescriptionAttributeUniScanG("Continue R [%]")]
        //public float ContinueR { get => continueR; set => continueR = value; }

        //[LocalizedCategoryAttributeUniScanG("Sheet Length Alarm"), LocalizedDisplayNameAttributeUniScanG("SheeetLengthAlarmUse"), LocalizedDescriptionAttributeUniScanG("SheeetLengthAlarmUse")]
        //public bool UseSheeetLengthAlarm { get => useSheeetLengthAlarm; set => useSheeetLengthAlarm = value; }
        //[LocalizedCategoryAttributeUniScanG("Sheet Length Alarm"), LocalizedDisplayNameAttributeUniScanG("SheeetLength N [EA]"), LocalizedDescriptionAttributeUniScanG("SheeetLength N [EA]")]
        //public int SheeetLengthN { get => sheeetLengthN; set => sheeetLengthN = value; }
        //[LocalizedCategoryAttributeUniScanG("Sheet Length Alarm"), LocalizedDisplayNameAttributeUniScanG("SheeetLength D [mm]"), LocalizedDescriptionAttributeUniScanG("SheeetLength D [mm]")]
        //public float SheeetLengthD { get => sheeetLengthD; set => sheeetLengthD = value; }

        //bool useNormalDefectAlaram;
        //int normalN;
        //float normalR;

        //bool useRepeatDefectAlaram;
        //int repeatN;
        //float repeatR;

        //bool useContinueDefectAlarm;
        //int continueN;
        //float continueR;

        //bool useSheeetLengthAlarm;
        //int sheeetLengthN;
        //float sheeetLengthD;

        [LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Normal Defect Alarm")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public RatioAlarmSettingElement NormalDefectAlarm { get => normalDefectAlarm; set => normalDefectAlarm = value; }
        RatioAlarmSettingElement normalDefectAlarm;

        [LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Repeated Defect Alarm")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public RatioAlarmSettingElement RepeatedDefectAlarm { get => repeatedDefectAlarm; set => repeatedDefectAlarm = value; }
        RatioAlarmSettingElement repeatedDefectAlarm;

        [LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Continuous Defect Alarm")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public RatioAlarmSettingElement ContinuousDefectAlarm { get => continuousDefectAlarm; set => continuousDefectAlarm = value; }
        RatioAlarmSettingElement continuousDefectAlarm;

        [LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Sheet Length Alarm")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ValueAlarmSettingElement SheetLengthAlarm { get => sheetLengthAlarm; set => sheetLengthAlarm = value; }
        ValueAlarmSettingElement sheetLengthAlarm;
                
        // 통신 관련
        [LocalizedCategoryAttributeUniScanG("Communication"), LocalizedDisplayNameAttributeUniScanG("Auto Operation"), LocalizedDescriptionAttributeUniScanG("Auto Operation")]
        public bool AutoOperation { get=> autoOperation; set=> autoOperation=value; }

        bool autoOperation;

        // 검사 관련
        [LocalizedCategoryAttributeUniScanG("Inspection"), LocalizedDisplayNameAttributeUniScanG("Use MultiLayer Buffer"), LocalizedDescriptionAttributeUniScanG("Use MultiLayer Buffer")]
        public bool IsMultiLayerBuffer { get => isMultiLayerBuffer; set => isMultiLayerBuffer = value; }

        bool isMultiLayerBuffer;

        // 레이저 관련
        [LocalizedCategoryAttributeUniScanG("Laser"), LocalizedDisplayNameAttributeUniScanG("Laser")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public LaserSettingElement LaserSettingElement { get => laserSettingElement; set => laserSettingElement = value; }
        LaserSettingElement laserSettingElement;

        protected AdditionalSettings()
        {
            encoderResolution = 14.0f;
            useAsyncMode = false;
            asyncGrabHz = 1500;

            this.normalDefectAlarm = new RatioAlarmSettingElement(true,100,80.0);
            //useNormalDefectAlaram = true;
            //normalN = 100;
            //normalR = 80;

            this.repeatedDefectAlarm = new RatioAlarmSettingElement(true,200,50.0);
            //useRepeatDefectAlaram = true;
            //repeatN = 200;
            //repeatR = 50.0f;

            this.continuousDefectAlarm = new RatioAlarmSettingElement(true, 50, 100);
            //useContinueDefectAlarm = true;
            //continueN = 50;
            //continueR = 100;

            this.sheetLengthAlarm = new ValueAlarmSettingElement(true, 100, 10, "um");
            //useSheeetLengthAlarm = true;
            //sheeetLengthN = 100;
            //sheeetLengthD = 10;

            autoOperation = false;

            isMultiLayerBuffer = false;

            this.laserSettingElement = new LaserSettingElement(false, 0, 0, 0, 0, 0, 0);
        }


        public static new void CreateInstance()
        {
            if (instance == null)
                instance = new AdditionalSettings();
        }
        
        public static new AdditionalSettings Instance()
        {
            return instance as AdditionalSettings;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "EncoderResolution", encoderResolution.ToString());
            XmlHelper.SetValue(xmlElement, "UseAsyncMode", useAsyncMode.ToString());
            XmlHelper.SetValue(xmlElement, "AsyncGrabHz", asyncGrabHz.ToString());
            XmlHelper.SetValue(xmlElement, "AutoTeach", autoTeach.ToString());
            XmlHelper.SetValue(xmlElement, "AutoTeachTimeout", autoTeachTimeout.ToString());
            XmlHelper.SetValue(xmlElement, "BufferAllocTimeout", bufferAllocTimeout.ToString());

            this.normalDefectAlarm.Save(xmlElement, "NormalDefectAlarm");
            //XmlHelper.SetValue(xmlElement, "UseNormalDefectAlaram", useNormalDefectAlaram.ToString());
            //XmlHelper.SetValue(xmlElement, "NormalN", normalN.ToString());
            //XmlHelper.SetValue(xmlElement, "NormalR", normalR.ToString());

            this.repeatedDefectAlarm.Save(xmlElement, "RepeatedDefectAlarm");
            //XmlHelper.SetValue(xmlElement, "UseRepeatDefectAlaram", useRepeatDefectAlaram.ToString());
            //XmlHelper.SetValue(xmlElement, "RepeatN", repeatN.ToString());
            //XmlHelper.SetValue(xmlElement, "RepeatR", repeatR.ToString());

            this.continuousDefectAlarm.Save(xmlElement, "ContinuousDefectAlarm");
            //XmlHelper.SetValue(xmlElement, "UseContinueDefectAlarm", useContinueDefectAlarm.ToString());
            //XmlHelper.SetValue(xmlElement, "ContinueN", continueN.ToString());
            //XmlHelper.SetValue(xmlElement, "ContinueR", continueR.ToString());

            this.sheetLengthAlarm.Save(xmlElement, "SheetLengthAlarm");
            //XmlHelper.SetValue(xmlElement, "UseSheeetLengthAlarm", useSheeetLengthAlarm.ToString());
            //XmlHelper.SetValue(xmlElement, "SheeetLengthN", sheeetLengthN.ToString());
            //XmlHelper.SetValue(xmlElement, "SheeetLengthD", sheeetLengthD.ToString());

            XmlHelper.SetValue(xmlElement, "AutoOperation", autoOperation.ToString());

            XmlHelper.SetValue(xmlElement, "IsMultiLayerBuffer", isMultiLayerBuffer.ToString());

            this.laserSettingElement.Save(xmlElement, "LaserSettingElement");
            //XmlHelper.SetValue(xmlElement, "UseLaser", useLaser.ToString());
            //XmlHelper.SetValue(xmlElement, "LaserDistanceM", laserDistanceM.ToString());
            //XmlHelper.SetValue(xmlElement, "SafeDistanceM", safeDistanceM.ToString());
        }

        protected override void PostSave()
        {
            base.PostSave();

            IServerExchangeOperator server = SystemManager.Instance()?.ExchangeOperator as IServerExchangeOperator;
            if (server == null)
                return;

            List<InspectorObj> inspectorObjList = server.GetInspectorList();

            string src = Path.Combine(PathSettings.Instance().Config, fileName);
            foreach (InspectorObj inspectorObj in inspectorObjList)
            {
                string dst = Path.Combine(inspectorObj.Info.Path, "Config", this.fileName);
                FileHelper.CopyFile(src, dst, true);
            }

            SystemManager.Instance()?.ExchangeOperator?.SendCommand(UniScan.Common.Exchange.ExchangeCommand.C_SYNC);
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            encoderResolution = float.Parse(XmlHelper.GetValue(xmlElement, "EncoderResolution", encoderResolution.ToString()));
            useAsyncMode = bool.Parse(XmlHelper.GetValue(xmlElement, "UseAsyncMode", useAsyncMode.ToString()));
            asyncGrabHz = float.Parse(XmlHelper.GetValue(xmlElement, "AsyncGrabHz", asyncGrabHz.ToString()));
            autoTeach = XmlHelper.GetValue(xmlElement, "AutoTeach", autoTeach);
            autoTeachTimeout = XmlHelper.GetValue(xmlElement, "AutoTeachTimeout", autoTeachTimeout);
            bufferAllocTimeout = XmlHelper.GetValue(xmlElement, "BufferAllocTimeout", bufferAllocTimeout);

            bool load;

            load = this.normalDefectAlarm.Load(xmlElement, "NormalDefectAlarm");
            if (load == false)
            {
                this.normalDefectAlarm.Use = XmlHelper.GetValue(xmlElement, "UseNormalDefectAlaram", this.normalDefectAlarm.Use);
                this.normalDefectAlarm.Count = XmlHelper.GetValue(xmlElement, "NormalN", this.normalDefectAlarm.Count);
                this.normalDefectAlarm.Ratio = XmlHelper.GetValue(xmlElement, "NormalR", this.normalDefectAlarm.Ratio);
            }

            load = this.repeatedDefectAlarm.Load(xmlElement, "RepeatedDefectAlarm");
            if (load == false)
            {
                this.repeatedDefectAlarm.Use = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "UseRepeatDefectAlaram", this.repeatedDefectAlarm.Use));
                this.repeatedDefectAlarm.Count = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "RepeatN", this.repeatedDefectAlarm.Count));
                this.repeatedDefectAlarm.Ratio = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "RepeatR", this.repeatedDefectAlarm.Ratio));
            }

            load = this.continuousDefectAlarm.Load(xmlElement, "ContinuousDefectAlarm");
            if (load == false)
            {
                this.continuousDefectAlarm.Use = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "UseContinueDefectAlarm", this.continuousDefectAlarm.Use));
                this.continuousDefectAlarm.Count = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "ContinueN", this.continuousDefectAlarm.Count));
                this.continuousDefectAlarm.Ratio = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "ContinueR", this.continuousDefectAlarm.Ratio));
            }

            load = this.sheetLengthAlarm.Load(xmlElement, "SheetLengthAlarm");
            if (load == false)
            {
                this.sheetLengthAlarm.Use = XmlHelper.GetValue(xmlElement, "UseSheeetLengthAlarm", this.sheetLengthAlarm.Use);
                this.sheetLengthAlarm.Count = XmlHelper.GetValue(xmlElement, "SheeetLengthN", this.sheetLengthAlarm.Count);
                this.sheetLengthAlarm.Value= XmlHelper.GetValue(xmlElement, "SheeetLengthD", this.sheetLengthAlarm.Value);
            }

            autoOperation = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "AutoOperation", autoOperation.ToString()));

            isMultiLayerBuffer = XmlHelper.GetValue(xmlElement, "IsMultiLayerBuffer", isMultiLayerBuffer);

            load = this.laserSettingElement.Load(xmlElement, "LaserSettingElement");
            if (load == false)
            {
                this.laserSettingElement.Use = XmlHelper.GetValue(xmlElement, "UseLaser", this.laserSettingElement.Use);
                this.laserSettingElement.DistanceM = XmlHelper.GetValue(xmlElement, "LaserDistanceM", this.laserSettingElement.DistanceM);
                this.laserSettingElement.SafeDistanceM = XmlHelper.GetValue(xmlElement, "SafeDistanceM", this.laserSettingElement.SafeDistanceM);
            }
        }
    }
}
