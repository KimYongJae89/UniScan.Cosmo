using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Standard.DynMvp.Base.Helpers;
using Standard.DynMvp.Devices.ImageDevices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.DynMvp.Devices
{
    public enum ProtocolType
    {
        IO, Serial
    }

    public class ProtocolInfoConverter : JsonCreationConverter<ProtocolInfo>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        protected override ProtocolInfo Create(Type objectType, JObject jObject)
        {
            //ProtocolType protocolType = (ProtocolType)Enum.Parse(typeof(ProtocolInfo), jObject["Type"].ToString());
            //var name = jObject["Name"].ToString();

            //switch (protocolType)
            //{
            //    case ProtocolType.IO:
            //        break;
            //    case ProtocolType.Serial:
            //        return new Serial.SerialInfo();
            //}

            return new Serial.SerialInfo(); ;
        }
    }

    public abstract class ProtocolInfo
    {
        ProtocolType _type;

        public ProtocolInfo(ProtocolType type)
        {
            _type = type;
        }
    }
}
