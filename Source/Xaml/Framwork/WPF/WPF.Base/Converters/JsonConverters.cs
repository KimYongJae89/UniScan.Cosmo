using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using WPF.Base.Helpers;
using WPF.Base.Services;

namespace WPF.Base.Converters
{
    public class ImageDeviceJsonConverter : JsonCreationConverter<DynMvp.Devices.ImageDevice>
    {
        //fake
        private class ImageDevice
        {
            public string Name { get; set; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var imageDevice = value as DynMvp.Devices.ImageDevice;
            var fakeImageDevice = new ImageDevice() { Name = imageDevice.Name };
            serializer.Serialize(writer, fakeImageDevice);
        }

        protected override DynMvp.Devices.ImageDevice Create(Type objectType, JObject jObject)
        {
            return SystemManager.Instance().DeviceBox.ImageDeviceHandler.ImageDeviceList.Find(device => device.Name == jObject["Name"].ToString());
        }
    }

    public class AxisHandlerJsonConverter : JsonCreationConverter<DynMvp.Devices.MotionController.AxisHandler>
    {
        //fake
        private class AxisHandler
        {
            public string Name { get; set; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var axisHandler = value as DynMvp.Devices.MotionController.AxisHandler;
            var fake = new AxisHandler() { Name = axisHandler.Name };

            serializer.Serialize(writer, fake);
        }

        protected override DynMvp.Devices.MotionController.AxisHandler Create(Type objectType, JObject jObject)
        {
            return MotionService.Handlers.First(handler => handler.Name == jObject["Name"].ToString());
        }
    }

    public class IoPortJsonConverter : JsonCreationConverter<DynMvp.Devices.Dio.IoPort>
    {
        //fake
        private class IoPort
        {
            public string Name { get; set; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var port = value as DynMvp.Devices.Dio.IoPort;
            var fake = new IoPort() { Name = port.Name };
            serializer.Serialize(writer, fake);
        }

        protected override DynMvp.Devices.Dio.IoPort Create(Type objectType, JObject jObject)
        {
            return SystemManager.Instance().DeviceBox.PortMap.GetOutPort(jObject["Name"].ToString());
        }
    }
}
