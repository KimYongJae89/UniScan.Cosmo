﻿using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using UniScanM.Settings;

namespace UniScanM.StillImage.Settings
{
    public enum EAutoStartMethod { Encoder, PLC }
    public enum EInspectionMode { Inspect, Monitor }
    public enum EOperationMode { Sequencial, Random }
    public class StillImageSettings : UniScanM.Settings.UniScanMSettings
    {
        float encoderResolution = 7.0f;
        EAutoStartMethod autoStartMethod = EAutoStartMethod.Encoder;
        float minimumLineSpeed = 10.0f;
        float speedStableVariation = 0.1f;
        EInspectionMode inspectionMode = EInspectionMode.Inspect;
        EOperationMode operationMode = EOperationMode.Sequencial;
        float fovMultipication = 1.0f;
        bool asyncMode = false;
        float asyncGrabHz = 1000.0f;
        bool autoResetAlarm = false;
        bool modelAutoChange = true;
        int targetIntensity = 230;
        int targetIntensityVal = 20;
        int initialTopLightValue = 32;
        float backLightMultiplier = 2.0f;
        int invalidateBound = 5;

        [Category("Encoder"), DisplayName("Encoder Resolution"), Description("Encoder Resolution [um/pls]")]
        public float EncoderResolution
        {
            get { return encoderResolution; }
            set { encoderResolution = value; }
        }

        [Category("LightTune"), DisplayName("Target Intensity"), Description("Target Intensity")]
        public int TargetIntensity
        {
            get { return targetIntensity; }
            set { targetIntensity = value; }
        }

        [Category("LightTune"), DisplayName("Target Intensity Variate"), Description("Target Intensity Variate")]
        public int TargetIntensityVal
        {
            get { return targetIntensityVal; }
            set { targetIntensityVal = value; }
        }

        [Category("LightTune"), DisplayName("Initial TopLight Value"), Description("Initial Top Light Value")]
        public int InitialTopLightValue
        {
            get { return initialTopLightValue; }
            set { initialTopLightValue = value; }
        }

        [Category("LightTune"), DisplayName("BackLight Multiplier"), Description("Back Light Multiplier")]
        public float BackLightMultiplier
        {
            get { return backLightMultiplier; }
            set { backLightMultiplier = value; }
        }

        [Category("Operation"), DisplayName("Start / Stop Method"), Description("Start / Stop Method")]
        public EAutoStartMethod AutoStartMethod
        {
            get { return autoStartMethod; }
            set { autoStartMethod = value; }
        }

        [Category("Operation"), DisplayName("Inspection Mode"), Description("Inspection Mode")]
        public EInspectionMode InspectionMode
        {
            get { return inspectionMode; }
            set { inspectionMode = value; }
        }

        [Category("Operation"), DisplayName("Operation Mode"), Description("Operation Mode")]
        public EOperationMode OperationMode
        {
            get { return operationMode; }
            set { operationMode = value; }
        }

        [Category("Operation"), DisplayName("Minimum Line Speed"), Description("Minimum Line Speed")]
        public float MinimumLineSpeed
        {
            get { return minimumLineSpeed; }
            set { minimumLineSpeed = value; }
        }

        [Category("Operation"), DisplayName("Speed Stable Variation"), Description("Speed Stable Variation")]
        public float SpeedStableVariation
        {
            get { return speedStableVariation; }
            set { speedStableVariation = value; }
        }

        [Category("Operation"), DisplayName("Fov Multipication"), Description("Multifacation Factor of FOV Size as Move")]
        public float FovMultipication
        {
            get { return fovMultipication; }
            set { fovMultipication = value; }
        }

        [Category("Operation"), DisplayName("Camera Async Mode"), Description("Camera Async Mode")]
        public bool AsyncMode
        {
            get { return asyncMode; }
            set { asyncMode = value; }
        }

        [Category("Operation"), DisplayName("Camera Async Grab Speed"), Description("Camera Async Grab Speed")]
        public float AsyncGrabHz
        {
            get { return asyncGrabHz; }
            set { asyncGrabHz = value; }
        }

        [Category("Operation"), DisplayName("Invalidate Boundary"), Description("Invalidate Boundary Size")]
        public int InvalidateBound
        {
            get { return invalidateBound; }
            set { invalidateBound = value; }
        }

        [Category("Error"), DisplayName("Auto Reset Alarm"), Description("Auto Reset Alarm")]
        public bool AutoResetAlarm
        {
            get { return autoResetAlarm; }
            set { autoResetAlarm = value; }
        }

        //[Category("Model"), DisplayName("Model Auto Change"), Description("Model Auto Change")]
        //public bool ModelAutoChange
        //{
        //    get { return modelAutoChange; }
        //    set { modelAutoChange = value; }
        //}

        protected StillImageSettings() { }

        public static StillImageSettings Instance()
        {
            return instance as StillImageSettings;
        }

        public static new void CreateInstance()
        {
            if (instance == null)
                instance = new StillImageSettings();
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
                return;

            XmlHelper.SetValue(xmlElement, "EncoderUmPerPulse", encoderResolution.ToString());
            XmlHelper.SetValue(xmlElement, "AutoStartMethod", autoStartMethod.ToString());

            XmlHelper.SetValue(xmlElement, "TargetIntensity", targetIntensity.ToString());
            XmlHelper.SetValue(xmlElement, "TargetIntensityVal", targetIntensityVal.ToString());
            XmlHelper.SetValue(xmlElement, "InitialTopLightValue", initialTopLightValue.ToString());
            XmlHelper.SetValue(xmlElement, "BackLightMultiplier", backLightMultiplier.ToString());

            XmlHelper.SetValue(xmlElement, "MinimumLineSpeed", minimumLineSpeed.ToString());
            XmlHelper.SetValue(xmlElement, "SpeedStableVariation", speedStableVariation.ToString());
            XmlHelper.SetValue(xmlElement, "InspectionMode", inspectionMode.ToString());
            XmlHelper.SetValue(xmlElement, "OperationMode", operationMode.ToString());
            XmlHelper.SetValue(xmlElement, "FovMultipication", fovMultipication.ToString());
            XmlHelper.SetValue(xmlElement, "InvalidateBound", invalidateBound.ToString());

            XmlHelper.SetValue(xmlElement, "AsyncMode", asyncMode.ToString());
            XmlHelper.SetValue(xmlElement, "AsyncGrabHz", asyncGrabHz.ToString());

            XmlHelper.SetValue(xmlElement, "AutoResetAlarm", autoResetAlarm.ToString());
            XmlHelper.SetValue(xmlElement, "ModelAutoChange", modelAutoChange.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            if (xmlElement == null)
                return;

            encoderResolution = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "EncoderUmPerPulse", encoderResolution.ToString()));
            autoStartMethod = (EAutoStartMethod)Enum.Parse(typeof(EAutoStartMethod), XmlHelper.GetValue(xmlElement, "AutoStartMethod", autoStartMethod.ToString()));

            targetIntensity = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "TargetIntensity", targetIntensity.ToString()));
            targetIntensityVal = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "TargetIntensityVal", targetIntensityVal.ToString()));
            initialTopLightValue = XmlHelper.GetValue(xmlElement, "InitialTopLightValue", initialTopLightValue);
            backLightMultiplier = XmlHelper.GetValue(xmlElement, "BackLightMultiplier", backLightMultiplier);

            minimumLineSpeed = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MinimumLineSpeed", minimumLineSpeed.ToString()));
            speedStableVariation = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "SpeedStableVariation", speedStableVariation.ToString()));
            inspectionMode = (EInspectionMode)Enum.Parse(typeof(EInspectionMode), XmlHelper.GetValue(xmlElement, "InspectionMode", inspectionMode.ToString()));
            operationMode = (EOperationMode)Enum.Parse(typeof(EOperationMode), XmlHelper.GetValue(xmlElement, "OperationMode", operationMode.ToString()));
            fovMultipication = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "FovMultipication", fovMultipication.ToString()));
            invalidateBound = XmlHelper.GetValue(xmlElement, "InvalidateBound", invalidateBound);

            asyncMode = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "AsyncMode", asyncMode.ToString()));
            asyncGrabHz = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AsyncGrabHz", asyncGrabHz.ToString()));

            this.autoResetAlarm = XmlHelper.GetValue(xmlElement, "AutoResetAlarm", this.autoResetAlarm);
            this.modelAutoChange = XmlHelper.GetValue(xmlElement, "ModelAutoChange", this.modelAutoChange);
        }
    }
}
