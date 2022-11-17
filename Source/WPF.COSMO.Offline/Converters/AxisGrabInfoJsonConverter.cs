using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Models;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Converters
{
    public class DefectJsonConverter : JsonCreationConverter<Defect>
    {
        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        protected override Defect Create(Type objectType, JObject jObject)
        {
            if (jObject["Distance"] == null)
                return new EdgeDefect();
            else
            {
                if (jObject["ScanDirection"] == null)
                    return new InnerDefect();
                else
                    return new SectionDefect();
            }
        }
    }

    public class AxisGrabInfoJsonConverter : JsonCreationConverter<Models.AxisGrabInfo>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }

        protected override Models.AxisGrabInfo Create(Type objectType, JObject jObject)
        {
            return AxisGrabService.Settings.AxisGrabInfoList.First(info => info.Name == jObject["Name"].ToString());
        }
    }
}
