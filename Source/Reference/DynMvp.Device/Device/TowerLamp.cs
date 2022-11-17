using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Devices
{
    public enum TowerLampStateType
    {
        Unknown = -1, Idle, Wait, Working, Defect, Alarm
    }

    public enum TowerLampValue
    {
        Off, On, Blink
    }

    public class Lamp
    {
        TowerLampValue value;
        public TowerLampValue Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public Lamp(TowerLampValue value = TowerLampValue.Off)
        {
            this.value = value;
        }
    }

    public class TowerLampState
    {
        TowerLampStateType type;
        public TowerLampStateType Type
        {
            get { return type; }
        }

        Lamp redLamp;
        public Lamp RedLamp
        {
            get { return redLamp; }
        }

        Lamp yellowLamp;
        public Lamp YellowLamp
        {
            get { return yellowLamp; }
        }

        Lamp greenLamp;
        public Lamp GreenLamp
        {
            get { return greenLamp; }
        }

        Lamp buzzer;
        public Lamp Buzzer
        {
            get { return buzzer; }
        }

        public TowerLampState()
        {
            this.type = TowerLampStateType.Unknown;
            this.redLamp = new Lamp(TowerLampValue.Off);
            this.yellowLamp = new Lamp(TowerLampValue.Off);
            this.greenLamp = new Lamp(TowerLampValue.Off);
            this.buzzer = new Lamp(TowerLampValue.Off);
        }

        public TowerLampState(TowerLampStateType type, Lamp redLamp, Lamp yellowLamp, Lamp greenLamp, Lamp buzzer)
        {
            this.type = type;
            this.redLamp = redLamp;
            this.yellowLamp = yellowLamp;
            this.greenLamp = greenLamp;
            this.buzzer = buzzer;
        }

        public void LoadXml(XmlElement xmlElement)
        {
            string[] towerLampValues = Enum.GetNames(typeof(TowerLampValue));

            type = (TowerLampStateType)Enum.Parse(typeof(TowerLampStateType), XmlHelper.GetValue(xmlElement, "Type", "Idle"));

            redLamp.Value = (TowerLampValue)Array.FindIndex(towerLampValues,
                element => element == XmlHelper.GetValue(xmlElement, "RedLampValue", "Off"));
            yellowLamp.Value = (TowerLampValue)Array.FindIndex(towerLampValues,
                element => element == XmlHelper.GetValue(xmlElement, "YellowLampValue", "Off"));
            greenLamp.Value = (TowerLampValue)Array.FindIndex(towerLampValues,
                element => element == XmlHelper.GetValue(xmlElement, "GreenLampValue", "Off"));
            buzzer.Value = (TowerLampValue)Array.FindIndex(towerLampValues,
                element => element == XmlHelper.GetValue(xmlElement, "BuzzerValue", "Off"));
        }

        public void SaveXml(XmlElement xmlElement)
        {
            //ResetValue();

            XmlHelper.SetValue(xmlElement, "Type", type.ToString());
            XmlHelper.SetValue(xmlElement, "RedLampValue", redLamp.Value.ToString());
            XmlHelper.SetValue(xmlElement, "YellowLampValue", yellowLamp.Value.ToString());
            XmlHelper.SetValue(xmlElement, "GreenLampValue", greenLamp.Value.ToString());
            XmlHelper.SetValue(xmlElement, "BuzzerValue", buzzer.Value.ToString());
        }
    }

    public delegate TowerLampState GetDynamicStateDelegate();

    public class TowerLamp
    {
        public GetDynamicStateDelegate GetDynamicState;

        DigitalIoHandler digitalIoHandler;

        IoPort towerLampRed;
        IoPort towerLampYellow;
        IoPort towerLampGreen;
        IoPort towerBuzzer;

        Task workingTask;

        bool useBuzzerPlayer;
        public bool UseBuzzerPlayer
        {
            get { return useBuzzerPlayer; }
            set { useBuzzerPlayer = value; }
        }

        bool buzzerPlayerOn = false;

        private SoundPlayer buzzerPlayer = new SoundPlayer(DynMvp.Properties.Resources.BUZZER_1);

        List<TowerLampState> towerLampStateList = new List<TowerLampState>();
        public List<TowerLampState> TowerLampStateList
        {
            get { return towerLampStateList; }
        }

        public void Stop()
        {
            stopThreadFlag = true;

            if (workingTask != null)
                workingTask.Wait();

            TurnOffTowerLamp();
        }

        bool stopThreadFlag = false;
        TowerLampStateType towerLampStateType;
        public TowerLampStateType TowerLampStateType
        {
            set { towerLampStateType = value; }
        }

        int updateIntervalMs;

        public void Setup(DigitalIoHandler digitalIoHandler, int updateIntervalMs)
        {
            this.digitalIoHandler = digitalIoHandler;
            this.updateIntervalMs = updateIntervalMs;

            InitTowerLampSateList();
        }

        public void SetupPort(IoPort[] towerLampIoPort)
        {
            if(towerLampIoPort != null && towerLampIoPort.Length == 4)
            {
                this.towerLampRed = towerLampIoPort[0];
                this.towerLampYellow = towerLampIoPort[1];
                this.towerLampGreen = towerLampIoPort[2];
                this.towerBuzzer = towerLampIoPort[3];
            }
        }

        public void SetupPort(IoPort towerLampRed, IoPort towerLampYellow, IoPort towerLampGreen, IoPort towerBuzzer)
        {
            this.towerLampRed = towerLampRed;
            this.towerLampYellow = towerLampYellow;
            this.towerLampGreen = towerLampGreen;
            this.towerBuzzer = towerBuzzer;
        }

        private void InitTowerLampSateList()
        {
            towerLampStateList.Add(new TowerLampState(TowerLampStateType.Idle, new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.On), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.Off)));
            towerLampStateList.Add(new TowerLampState(TowerLampStateType.Wait, new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.On), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.Off)));
            towerLampStateList.Add(new TowerLampState(TowerLampStateType.Working, new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.On), new Lamp(TowerLampValue.Off)));
            towerLampStateList.Add(new TowerLampState(TowerLampStateType.Defect, new Lamp(TowerLampValue.On), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.On)));
            towerLampStateList.Add(new TowerLampState(TowerLampStateType.Alarm, new Lamp(TowerLampValue.On), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.Off), new Lamp(TowerLampValue.On)));
        }

        public void Save(string configPath)
        {
            string filePath = String.Format(@"{0}\TowerLamp.xml", configPath);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement element = xmlDocument.CreateElement("TowerLamp");
            xmlDocument.AppendChild(element);
            foreach (TowerLampState state in towerLampStateList)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(state.Type.ToString());
                element.AppendChild(subElement);
                state.SaveXml(subElement);
            }

            XmlHelper.Save(xmlDocument, filePath);
        }

        public void Load(string configPath)
        {
            string filePath = String.Format(@"{0}\TowerLamp.xml", configPath);
            XmlDocument xmlDocument = XmlHelper.Load(filePath);
            if (xmlDocument == null)
                return;

            XmlElement element = xmlDocument.DocumentElement;
            string[] types = Enum.GetNames(typeof(TowerLampStateType));
            foreach (string type in types)
            {
                XmlElement subElement = element[type];
                if (subElement == null)
                {
                    continue;
                }

                // 중복된 상태 제거
                List<TowerLampState> findList = towerLampStateList.FindAll(f => f.Type.ToString() == type);
                foreach (TowerLampState find in findList)
                {
                    towerLampStateList.Remove(find);
                }

                TowerLampState state = new TowerLampState();
                state.LoadXml(subElement);
                towerLampStateList.Add(state);
            }
        }

        public TowerLampState GetState()
        {
            return towerLampStateList.Find(x => x.Type == towerLampStateType);
        }

        private TowerLampState GetState(TowerLampStateType type)
        {
            return towerLampStateList.Find(x => x.Type == type);
        }

        public void SetState(TowerLampStateType type)
        {
            towerLampStateType = type;
        }

        private void TurnOnTowerLamp(TowerLampState towerLampState, bool isblinkOn = false)
        {
            TurnOnTowerLamp(towerLampRed, towerLampState.RedLamp.Value, isblinkOn);
            TurnOnTowerLamp(towerLampYellow, towerLampState.YellowLamp.Value, isblinkOn);
            TurnOnTowerLamp(towerLampGreen, towerLampState.GreenLamp.Value, isblinkOn);
            TurnOnTowerLamp(towerBuzzer, towerLampState.Buzzer.Value, isblinkOn);
        }

        public void TurnOnTowerLamp(IoPort towerLampPort, TowerLampValue value, bool isblinkOn)
        {
            bool isOnLamp = value == TowerLampValue.On || (isblinkOn && value == TowerLampValue.Blink);
            digitalIoHandler.WriteOutput(towerLampPort, isOnLamp);
        }

        public void TurnOffTowerLamp()
        {
            digitalIoHandler.WriteOutput(towerLampRed, false);
            digitalIoHandler.WriteOutput(towerLampYellow, false);
            digitalIoHandler.WriteOutput(towerLampGreen, false);
            digitalIoHandler.WriteOutput(towerBuzzer, false);
        }

        public void Start()
        {
            towerLampStateType = TowerLampStateType.Idle;
            workingTask = new Task(new Action(WorkingProc));
            workingTask.Start();
        }

        public void WorkingProc()
        {
            bool isblinkOn = false;

            while (stopThreadFlag == false)
            {
                // Blink 시키기 위해 On/Off 플래그 생성
                TowerLampState state;

                if (ErrorManager.Instance().IsAlarmed())
                {
                    state = GetState(TowerLampStateType.Alarm);

                    if (ErrorManager.Instance().BuzzerOn)
                    {
                        state.Buzzer.Value = TowerLampValue.On;
                        if (buzzerPlayerOn == false)
                        {
                            buzzerPlayerOn = true;
                            buzzerPlayer.Play();
                        }
                    }
                    else
                    {
                        state.Buzzer.Value = TowerLampValue.Off;
                        if (buzzerPlayerOn == true)
                        {
                            buzzerPlayerOn = false;
                            buzzerPlayer.Stop();
                        }
                    }
                }
                else
                {
                    if (buzzerPlayerOn == true)
                    {
                        buzzerPlayerOn = false;
                        buzzerPlayer.Stop();
                    }

                    if (GetDynamicState != null)
                        state = GetDynamicState();
                    else
                        state = GetState();
                }

                if (state != null)
                {
                    TurnOnTowerLamp(state, isblinkOn);
                    Thread.Sleep(500);
                }

                //Thread.Sleep(updateIntervalMs);
                //TurnOffTowerLamp();
                Thread.Sleep(updateIntervalMs);

                isblinkOn = !isblinkOn;
            }
        }
    }
}
