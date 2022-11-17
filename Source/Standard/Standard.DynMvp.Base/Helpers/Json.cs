using System;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Standard.DynMvp.Base.Helpers
{
    public static class Json
    {
        public static async Task<T> ToObjectAsync<T>(string value, params JsonConverter[] converters)
        {
            return await Task.Run<T>(() =>
            {
                //return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
                //{
                //    Error = HandleDeserializationError
                //});
                return JsonConvert.DeserializeObject<T>(value, converters);
            });
        }

        public static void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }

        public static async Task<string> StringifyAsync(object value)
        {
            return await Task.Run<string>(() =>
            {
                return JsonConvert.SerializeObject(value);
            });
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
