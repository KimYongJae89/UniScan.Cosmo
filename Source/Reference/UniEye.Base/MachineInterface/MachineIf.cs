using DynMvp.Base;
using DynMvp.Device;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using UniEye.Base.Data;
using UniEye.Base.Inspect;

namespace UniEye.Base.MachineInterface
{
    public enum MachineIfType
    {
        None, /*UniEyeExchange, IO, Fins,*/ TcpServer, TcpClient, Melsec
    }
    
    public abstract class MachineIfProtocolList
    {
        public Type[] ProtocolListType
        {
            get { return protocolListType; }
        }

        public Dictionary<Enum, MachineIfProtocol> Dic
        {
            get { return dic; }
        }

        Type[] protocolListType;
        protected Dictionary<Enum, MachineIfProtocol> dic = new Dictionary<Enum, MachineIfProtocol>();

        public MachineIfProtocolList(params Type[] protocolListType)
        {
            Debug.Assert(protocolListType != null && protocolListType.Length > 0);

            this.protocolListType = protocolListType;
            for (int i = 0; i < protocolListType.Length; i++)
                AddProtocolList(protocolListType[i]);
        }

        private void AddProtocolList(Type protocolListType)
        {
            //this.dic.Clear();
            Array array = Enum.GetValues(protocolListType);
            foreach (Enum e in array)
            {
                this.dic.Add(e, null);
            }
        }

        public Enum GetEnum(string command)
        {
            foreach (Type type in this.ProtocolListType)
            {
                if (type.IsEnumDefined(command))
                {
                    Enum e = (Enum)Enum.Parse(type, command);
                    return e;
                }
            }
            return null;
        }

        public virtual void Initialize(MachineIfType machineIfType)
        {
            Func<KeyValuePair<Enum, MachineIfProtocol>, MachineIfProtocol> func = new Func<KeyValuePair<Enum, MachineIfProtocol>, MachineIfProtocol>(f =>
            {
                switch (machineIfType)
                {
                    case MachineIfType.TcpClient:
                    case MachineIfType.TcpServer:
                        return new TcpIpMachineIfProtocol(f.Key);
                    case MachineIfType.Melsec:
                        return new MelsecMachineIfProtocol(f.Key);
                    default:
                        return null;
                }
            });

            this.dic = this.dic.ToDictionary(f => f.Key, f => func(f));
        }

        protected abstract void LoadXml(XmlElement xmlElement);
        public void Load(XmlElement xmlElement, string subKey = null)
        {
            if (xmlElement == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = xmlElement[subKey];
                Load(subElement);
                return;
            }

            LoadXml(xmlElement);
        }

        protected abstract void SaveXml(XmlElement xmlElement);
        public void Save(XmlElement xmlElement, string subKey = null)
        {
            if (xmlElement == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(subKey);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            SaveXml(xmlElement);
        }

        public virtual MachineIfProtocol GetProtocol(string e)
        {
            foreach(KeyValuePair<Enum, MachineIfProtocol> pair in this.dic)
            {
                if (pair.Key.ToString() == e)
                    return pair.Value.Clone();
            }
            return null;
        }

        public virtual MachineIfProtocol GetProtocol(Enum e)
        {
            return this.dic[e]?.Clone();
        }
    }

    public abstract class MachineIfSetting : ICloneable
    {
        string name= "";
        public string Name { get => name; set => name = value; }

        bool isVirtualMode = false;
        public bool IsVirtualMode { get => isVirtualMode; set => isVirtualMode = value; }
        

        protected MachineIfType machineIfType = MachineIfType.None;
        protected MachineIfProtocolList machineIfProtocolList = null;

        public MachineIfType MachineIfType
        {
            get { return machineIfType; }
            set { machineIfType = value; }
        }

        public MachineIfProtocolList MachineIfProtocolList
        {
            get { return machineIfProtocolList; }
            set { machineIfProtocolList = value; }
        }

        protected MachineIfSetting(MachineIfType machineIfType)
        {
            this.machineIfType = machineIfType;
        }

        public static MachineIfSetting LoadSettings(XmlElement xmlElement, string key = null)
        {
            if (xmlElement == null)
                return null;

            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement[key];
                return LoadSettings(subElement);
            }

            MachineIfType machineIfType = (MachineIfType)Enum.Parse(typeof(MachineIfType), XmlHelper.GetValue(xmlElement, "MachineIfType", MachineIfType.None.ToString()));
            MachineIfSetting machineIfSetting = Create(machineIfType);
            if (machineIfSetting != null)
                machineIfSetting.Load(xmlElement);

            return machineIfSetting;
        }

        public abstract object Clone();

        protected abstract void SaveXml(XmlElement xmlElement);
        public void Save(XmlElement xmlElement, string key = null)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement xmlSubElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(xmlSubElement);
                Save(xmlSubElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "MachineIfType", machineIfType.ToString());
            XmlHelper.SetValue(xmlElement, "IsVirtualMode", isVirtualMode.ToString());
            
            if (machineIfProtocolList != null)
                machineIfProtocolList.Save(xmlElement, "ProtocolList");
            XmlHelper.SetValue(xmlElement, "Name", name);

            SaveXml(xmlElement);
        }

        protected abstract void LoadXml(XmlElement xmlElement);
        public void Load(XmlElement xmlElement, string key=null)
        {
            if (xmlElement == null)
                return;

            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement xmlSubElement = xmlElement[key];
                Load(xmlSubElement);
                return;
            }

            machineIfType = (MachineIfType)Enum.Parse(typeof(MachineIfType), XmlHelper.GetValue(xmlElement, "MachineIfType", machineIfType.ToString()));
            isVirtualMode = XmlHelper.GetValue(xmlElement, "IsVirtualMode", isVirtualMode);

            if (machineIfProtocolList != null)
                machineIfProtocolList.Load(xmlElement, "ProtocolList");

            name = XmlHelper.GetValue(xmlElement, "Name", "");

            LoadXml(xmlElement);
        }

        internal static MachineIfSetting Create(MachineIfType machineIfType)
        {
            MachineIfSetting machineIfSetting = null;
            switch (machineIfType)
            {
                case MachineIfType.None:
                    machineIfSetting = null;
                    break;
                case MachineIfType.TcpClient:
                case MachineIfType.TcpServer:
                    machineIfSetting = new TcpIpMachineIfSettings(machineIfType);
                    break;
                case MachineIfType.Melsec:
                    machineIfSetting = new MelsecMachineIfSetting();
                    break;
            }

            if (machineIfSetting != null)
            {
                SystemManager systemManager = SystemManager.Instance();
                if (systemManager != null && systemManager.MachineIfProtocolList != null)
                {
                    machineIfSetting.machineIfProtocolList = SystemManager.Instance().MachineIfProtocolList;
                    machineIfSetting.machineIfProtocolList.Initialize(machineIfType);
                }
            }

            return machineIfSetting;
        }
    }

    public interface IVirtualMachineIf
    {
        void SetStateConnect(bool connect);
    }

    public abstract class MachineIf 
    {
        protected MachineIfSetting machineIfSetting = null;
        protected MachineIfProtocolResponce protocolResponce = null;

        bool isIdle = true;
        public bool IsIdle { get => isIdle; set => isIdle = value; }

        public abstract bool IsConnected { get; }
        

        List<MachineIfExecuter> executerList = new List<MachineIfExecuter>();
        
        public virtual void Initialize() { }

        public abstract void Release();
        public abstract void Start();
        public abstract void Stop();
        
        public static MachineIf Create(MachineIfSetting machineIfSetting, bool isVirtual)
        {
            if (machineIfSetting == null)
                return null;

            if (isVirtual)
                machineIfSetting.IsVirtualMode = true;

            switch (machineIfSetting.MachineIfType)
            {
                case MachineIfType.None:
                    return null;
                case MachineIfType.TcpClient:
                    return new TcpIpMachineIfClient(machineIfSetting);
                case MachineIfType.TcpServer:
                    return new TcpIpMachineIfServer(machineIfSetting);
                case MachineIfType.Melsec:
                    return MelsecMachineIf.BuildMelsecMachineIf(machineIfSetting);
                    //return new MelsecMachineIf(machineIfSetting, isVirtual);
            }
            return null;
        }

        public MachineIf(MachineIfSetting machineIfSetting)
        {
            this.machineIfSetting = machineIfSetting;
        }
        
        public void AddExecuter(MachineIfExecuter executer)
        {
            executerList.Add(executer);
        }

        public void AddExecuters(MachineIfExecuter[] executers)
        {
            executerList.AddRange(executers);
        }

        public ExecuteResult ExecuteCommand(string commandString)
        {
            ExecuteResult executeResult = new ExecuteResult();
            if (String.IsNullOrEmpty(commandString) == true)
                return executeResult;
            
            foreach (MachineIfExecuter executer in executerList)
            {
                executeResult = executer.ExecuteCommand(commandString);

                if (executeResult.Success == true)
                    break;
            }

            return executeResult;
        }

        protected abstract bool Send(MachineIfProtocol protocol);

        public abstract void SendCommand(byte[] bytes);

        public MachineIfProtocolResponce SendCommand(MachineIfProtocol protocol)
        {
            int timeoutMs = protocol.WaitResponceMs;

            //Monitor.TryEnter(this, 500);
            isIdle = false;
            lock (this)
            {
                protocolResponce = new MachineIfProtocolResponce(protocol);
                {
                    bool ok = this.Send(protocol);
                    if (ok)
                    {
                        if (timeoutMs != 0 && protocolResponce.WaitResponce(timeoutMs) == false)
                        {
                            LogHelper.Error(LoggerType.Error, string.Format("MachineIf::Send Timeout. {0}", protocol.Command.ToString()));
                            //Debug.WriteLine("MachineIf::Send Timeout");
                        }
                    }
                }
                isIdle = true;
                return protocolResponce;
            }
            isIdle = true;

        }

        public MachineIfProtocolResponce SendCommand(Enum command, params string[] args)
        {
            MachineIfProtocol machineIfProtocol = SystemManager.Instance().MachineIfProtocolList.GetProtocol(command);
            //if (timeout >= 0)
            //    machineIfProtocol.WaitResponceMs = timeout;

            if (args != null)
                machineIfProtocol.SetArgument(args);

            return SendCommand(machineIfProtocol);
        }

        public MachineIfProtocolResponce SendCommand(MachineIfProtocolList machineIfProtocolList, Enum command, params string[] args)
        {
            MachineIfProtocol machineIfProtocol = machineIfProtocolList.GetProtocol(command);

            if (args != null)
                machineIfProtocol.SetArgument(args);

            return SendCommand(machineIfProtocol);
        }

        public MachineIfType GetMachineIfType()
        {
            return machineIfSetting.MachineIfType;
        }
    }
}
