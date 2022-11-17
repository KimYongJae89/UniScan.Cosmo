using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Standard.DynMvp.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.SEMCNS.Models
{
    public class TargetConverter : JsonCreationConverter<UWP.Base.Models.Target>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        protected override UWP.Base.Models.Target Create(Type objectType, JObject jObject)
        {
            return new Target();
        }
    }

    public class TargetParam
    {
        [SettingData(SettingDataType.Numeric)]
        public uint Lower { get; set; }
        [SettingData(SettingDataType.Numeric)]
        public uint Upper { get; set; }

        [SettingData(SettingDataType.Numeric)]
        public uint MinArea { get; set; }
        [SettingData(SettingDataType.Numeric)]
        public uint LightValue { get; set; }
    }

    public class Target : UWP.Base.Models.Target
    {
        TargetParam _targetParam = new TargetParam();
        public TargetParam TargetParam { get => _targetParam; }
    }
}
