using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace UniEye.Base.MachineInterface
{
    public abstract class MachineIfProtocol
    {
        protected Enum command;
        protected bool use = false;
        protected int waitResponceMs = 400;

        public Enum Command
        {
            get { return command; }
            set { command = value; }
        }

        public bool Use
        {
            get { return use; }
            set { use = value; }
        }

        public int WaitResponceMs
        {
            get { return waitResponceMs; }
            set { waitResponceMs = value; }
        }

        public MachineIfProtocol(Enum command, bool use, int waitResponceMs)
        {
            this.command = command;
            this.use = use;
            this.waitResponceMs = waitResponceMs;
        }

        protected virtual void SaveXml(XmlElement element)
        {
            XmlHelper.SetValue(element, "Use", use.ToString());
            XmlHelper.SetValue(element, "WaitResponceMs", waitResponceMs.ToString());
        }

        public void Save(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Save(subElement);
                return;
            }

            SaveXml(element);
        }

        protected virtual void LoadXml(XmlElement element)
        {
            use = Convert.ToBoolean(XmlHelper.GetValue(element, "Use", use.ToString()));
            waitResponceMs = Convert.ToInt32(XmlHelper.GetValue(element, "WaitResponceMs", waitResponceMs.ToString()));
            if (waitResponceMs < 0)
                waitResponceMs = 400;
        }

        public void Load(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Load(subElement);
                return;
            }

            LoadXml(element);
        }

        public override string ToString()
        {
            return command?.ToString();
        }

        public abstract MachineIfProtocol Clone();

        public virtual void SetArgument(params string[] args)
        {
            throw new NotImplementedException();
        }
    }
    
    public class MachineIfProtocolResponce
    {
        MachineIfProtocol sentProtocol;
        string reciveData;
        bool isGood = false;
        bool isResponced = false;
        ReceivedPacket receivedPacket = null;
        ManualResetEvent onResponce = new ManualResetEvent(false);
        
        public bool IsResponced
        {
            get { return isResponced; }
        }

        public MachineIfProtocol SentProtocol
        {
            get { return sentProtocol; }
        }

        public string ReciveData
        {
            get { return reciveData; }
        }

        public bool IsGood
        {
            get { return isResponced && isGood && !string.IsNullOrEmpty(reciveData); }
        }

        public ReceivedPacket ReceivedPacket { get => receivedPacket; }

        public MachineIfProtocolResponce(MachineIfProtocol sentProtocol)
        {
            this.sentProtocol = sentProtocol;
        }

        public void SetRecivedData(string reciveData, bool isGood, ReceivedPacket receivedPacket)
        {
            //if (string.IsNullOrEmpty(reciveData))
            //    return;

            this.reciveData = reciveData;
            this.isGood = isGood;
            this.isResponced = true;
            this.receivedPacket = receivedPacket;
            this.onResponce.Set();
        }

        public bool WaitResponce(int timeoutMs=-1)
        {
            if (timeoutMs < 0)
                timeoutMs = this.SentProtocol.WaitResponceMs;

            return onResponce.WaitOne(timeoutMs);
        }

        /// <summary>
        /// Ascii Byte -> Unicode String
        /// </summary>
        /// <returns></returns>
        public string Convert2String()
        {
            if (this.isResponced == false)
                return null;

            char[] chars = new char[reciveData.Length / 2];
            for (int i = 0; i < chars.Length; i++)
                chars[i] = (char)Convert.ToByte(reciveData.Substring(i * 2, 2), 16);
            return new string(chars).Trim('\0');
        }

        public string Convert2StringLE()
        {
            if (this.isResponced == false)
                return null;

            char[] chars = new char[reciveData.Length/2];
            for (int i = 0; i < chars.Length; i+=2)
            {
                int idx1 = (i + 1) * 2;
                if (idx1 + 1 < chars.Length)
                    chars[i] = (char)Convert.ToByte(reciveData.Substring(idx1, 2),  16);

                int idx2 = (i) * 2;
                if (idx2 + 1 < chars.Length)
                    chars[i + 1] = (char)Convert.ToByte(reciveData.Substring(idx2, 2), 16);
            }
            string converted = new string(chars).Trim('\0');
            return converted;
        }
    }
}
