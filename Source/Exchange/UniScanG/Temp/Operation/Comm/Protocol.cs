using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Temp
{
    public enum ECommand
    {
        NULL,

        // 운영
        START,
        STOP,
        CHANGE_MODE,
        TAB_DISABLE,
        USERCHANGE,
        STATE,

        // 모델 관련
        QUERY_MODEL_INFO,
        NEW_MODEL,
        CHANGE_MODEL,
        DELETE_MODEL,
        SAVE_MODEL,

        // 티칭
        TEACH,
        EXIT_TEACH_JOB,
        TEACH_INSPECT,

        // 파라메터
        SYNC,
        SETTING_SYNC,

        // 그랩
        GRAB,
        START_TEST_GRAB,

        // 검사
        LOT_CHANGE,
        SET_INSPECTION_NO,
        ENTER_WAIT,
        EXIT_WAIT,
        START_INSP,
        INSP_DONE,
        RESET,

        // 통신
        TIME,
        ECHO,
        CHK
    };

    public class SamsungProtocolFactory
    {
        static SamsungProtocolFactory instance = new SamsungProtocolFactory();

        Dictionary<ECommand, Protocol> protocolDic= null;
        
       public static SamsungProtocolFactory Instance()
        {
            //if (instance == null)
            //    instance = new SamsungProtocolFactory();
            return instance;
        }

        private SamsungProtocolFactory()
        {
            protocolDic = new Dictionary<ECommand, Protocol>();

            foreach (ECommand command in Enum.GetValues(typeof(ECommand)))
            {
                switch (command)
                {
                    case ECommand.GRAB:
                    case ECommand.TEACH:
                        protocolDic.Add(command, new Protocol(command, 30000, 0));
                        break;

                    case ECommand.CHANGE_MODE:
                    case ECommand.CHANGE_MODEL:
                    case ECommand.SAVE_MODEL:
                        protocolDic.Add(command, new Protocol(command, 5000, 0));
                        break;

                    case ECommand.SYNC:
                    case ECommand.ENTER_WAIT:
                        protocolDic.Add(command, new Protocol(command, 10000, 0));
                        break;

                    default:
                        protocolDic.Add(command, new Protocol(command, 1000, 0));
                        break;
                }
            }
        }

        public Protocol Create(ECommand command, params string[] args)
        {
            Protocol protocol = this.protocolDic[command].Clone(args);
            return protocol;
        }

        public Protocol Create(params string[] args)
        {
            ECommand command = (ECommand)Enum.Parse(typeof(ECommand), args[0]);
            //int camIndex = int.Parse(args[1]);
            //int clientIndex = int.Parse(args[2]);
            string[] newArgs = new string[args.Length - 1];
            Array.Copy(args, 1, newArgs, 0, newArgs.Length);

            Protocol protocol = this.protocolDic[command].Clone(newArgs);
            return protocol;
        }
    }

    public class Protocol
    {
        ECommand command;
        public ECommand Command
        {
            get { return command; }
        }

        string[] args;
        public string[] Args
        {
            get { return args; }
        }

        int timeoutMS = 1000;
        public int TimeoutMS
        {
            get { return timeoutMS; }
        }

        int retry = 0;
        public int Retry
        {
            get { return retry; }
        }

        public Protocol(ECommand command, int timeoutMS, int retry)
        {
            this.command = command;
            this.timeoutMS = timeoutMS;
            this.retry = retry;
        }

        public Protocol Clone(params string[] args)
        {
            Protocol protocol = new Protocol(this.command, this.timeoutMS, this.retry);
            protocol.args = args;
            return protocol;
        }

        //public Protocol Clone(string[] args)
        //{
        //    this.command = (ECommand)Enum.Parse(typeof(ECommand), args[0]);
        //    this.args = new string[args.Length - 1];
        //    for (int i = 1; i < args.Length; i++)
        //        this.args[i - 1] = args[i];
        //}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.command.ToString());
            foreach (string arg in this.args)
                sb.Append(string.Format(";{0}", arg));

            return sb.ToString();
        }
    }
}
