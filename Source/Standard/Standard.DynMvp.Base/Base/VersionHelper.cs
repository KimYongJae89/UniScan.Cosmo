using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Standard.DynMvp.Base
{
    public class VersionHelper
    {
        static VersionHelper instance = null;

        public int MajorVersion { get => majorVersion;}
        int majorVersion;

        public int MinorVersion { get => minorVersion;}
        int minorVersion;

        public int DebugVersion { get => debugVersion;}
        int debugVersion;

        public DateTime BuildDateTime { get => buildDateTime;}
        DateTime buildDateTime;

        public string VersionString { get => this.versionString; }
        string versionString;

        public string BuildString { get => this.buildString; }
        string buildString;

        public static VersionHelper Instance()
        {
            if (instance == null)
                instance = new VersionHelper();
            return instance;
        }

        private VersionHelper()
        {
            this.majorVersion = GetMajorVersion();
            this.minorVersion= GetMinorVersion();
            this.debugVersion= GetDebugVersion();
            this.buildDateTime = GetBuildDateTime();
            this.versionString = GetVersionString();
            this.buildString = GetBuildString();
        }

        private string GetVersionString()
        {
            int[] version = new int[3] { this.GetMajorVersion(), this.GetMinorVersion(), this.GetDebugVersion() };

            if (version[2] == 0)
                return string.Format("{0}.{1}", version[0], version[1]);
            else
                return string.Format("{0}.{1}.{2}", version[0], version[1], version[2]);
        }

        private int GetMajorVersion()
        {
            // 최종 Project의 Version 가져옴
            int version = 0;
            string strVersionText = Assembly.GetEntryAssembly().FullName.Split(',')[1].Trim().Split('=')[1];
            int.TryParse(strVersionText.Split('.')[0], out version);
            return version;

        }

        private int GetMinorVersion()
        {
            // 최종 Project의 Version 가져옴
            int version = 0;
            string strVersionText = Assembly.GetEntryAssembly().FullName.Split(',')[1].Trim().Split('=')[1];
            int.TryParse(strVersionText.Split('.')[1], out version);
            return version;
        }

        private int GetDebugVersion()
        {
            // 최종 Project의 Version 가져옴
            int version = 0;
            string strVersionText = Assembly.GetEntryAssembly().FullName.Split(',')[1].Trim().Split('=')[1];
            int.TryParse(strVersionText.Split('.')[2], out version);
            return version;
        }

        private DateTime GetBuildDateTime()
        {
            //1. Assembly.GetExecutingAssembly().FullName의 값은  
            //'ApplicationName, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null' 
            //와 같다.  
            // DynMVP의 Build 날짜 가져옴
            string strVersionText = Assembly.GetExecutingAssembly().FullName.Split(',')[1].Trim().Split('=')[1];

            //2. Version Text의 세번째 값(Build Number)은 2000년 1월 1일부터  
            //Build된 날짜까지의 총 일(Days) 수 이다. 
            int intDays = Convert.ToInt32(strVersionText.Split('.')[2]);
            DateTime refDate = new DateTime(2000, 1, 1);
            DateTime dtBuildDate = refDate.AddDays(intDays);

            //3. Verion Text의 네번째 값(Revision NUmber)은 자정으로부터 Build된 
            //시간까지의 지나간 초(Second) 값 이다. 
            int intSeconds = Convert.ToInt32(strVersionText.Split('.')[3]);
            intSeconds = intSeconds * 2;
            dtBuildDate = dtBuildDate.AddSeconds(intSeconds);

            //4. 시차조정 
            DaylightTime daylingTime = TimeZone.CurrentTimeZone.GetDaylightChanges(dtBuildDate.Year);
            if (TimeZone.IsDaylightSavingTime(dtBuildDate, daylingTime))
                dtBuildDate = dtBuildDate.Add(daylingTime.Delta);

            return dtBuildDate;
        }

        private string GetBuildString()
        {
            return this.buildDateTime.ToString("yyMMdd.HHmm");
        }
    }
}
