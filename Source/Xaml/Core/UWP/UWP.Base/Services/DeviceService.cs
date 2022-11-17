using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.IO;
using Standard.DynMvp.Devices.Dio;
using Standard.DynMvp.Devices.MotionController;
using Standard.DynMvp.Devices;
using Standard.DynMvp.Base.Helpers;
using Standard.DynMvp.Devices.ImageDevices;
using System.Threading.Tasks;
using UWP.Base.Settings;
using System.Collections.ObjectModel;
using Standard.DynMvp.Devices.LightController;
using Windows.Devices.SerialCommunication;
using Standard.DynMvp.Devices.Serial;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;

namespace UWP.Base.Services
{
    public class SerialLightContolExcuter : LightControlExcuter
    {
        SerialDevice _serialDevice;
        object _writeLock;
        DataWriter _dataWriter;

        public SerialLightContolExcuter(SerialDevice serialDevice)
        {
            _serialDevice = serialDevice;
        }

        public override void Dispose()
        {
            _serialDevice.Dispose();
        }

        public override Task Excute(string message)
        {
            return Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(message))
                {
                    try
                    {
                        _dataWriter = new DataWriter(_serialDevice.OutputStream);
                        _dataWriter.WriteString(message);
                        Task<UInt32> storeAsyncTask = _dataWriter.StoreAsync().AsTask();

                        storeAsyncTask.Wait();
                        //결과 받아오기
                        UInt32 bytesWritten = storeAsyncTask.Result;
                    }
                    finally
                    {
                        _dataWriter.DetachStream();
                        _dataWriter = null;
                    }
                }
            });
        }
    }

    public static class DeviceService
    {
        static ObservableCollection<LightController> _lightControllers = new ObservableCollection<LightController>();
        public static IEnumerable<LightController> LightControllers { get => _lightControllers; }

        static ObservableCollection<Grabber> _grabbers = new ObservableCollection<Grabber>();
        public static IEnumerable<Grabber> Grabbers { get => _grabbers; }

        static private async Task InitializeGrabber(IEnumerable<GrabberInfo> grabberInfos)
        {
            await Task.Run(() =>
                {
                    foreach (var info in grabberInfos)
                        _grabbers.Add(Grabber.Create(info));
                });
        }

        static private async Task InitializeLightController(IEnumerable<LightControllerInfo> lightControllerInfos)
        {
            foreach (var info in lightControllerInfos)
            {
                LightControlExcuter lightControlExcuter = null;

                if (info.Protocol == LightControllerProtocol.Serial)
                {
                    var serialInfo = info.ProtocolInfo as SerialInfo;
                    var selector = SerialDevice.GetDeviceSelector(serialInfo.Comport.ToString());
                    var serialDevices = await DeviceInformation.FindAllAsync(selector);
                    if (serialDevices.Any())
                    {
                        var serialDevice = await SerialDevice.FromIdAsync(serialDevices.First().Id);
                        if (serialDevice != null)
                        {
                            OpenSerialDevice(serialDevice, serialInfo);
                            lightControlExcuter = new SerialLightContolExcuter(serialDevice);
                        }
                    }
                }

                _lightControllers.Add(LightController.Create(info, lightControlExcuter));
            }
        }

        static void OpenSerialDevice(SerialDevice serialDevice, SerialInfo serialInfo)
        {
            //serialDevice.WriteTimeout = TimeSpan.FromMilliseconds(1000);
            //serialDevice.ReadTimeout = TimeSpan.FromMilliseconds(1000);
            serialDevice.BaudRate = serialInfo.BaudRate;
            serialDevice.DataBits = serialInfo.DataBits;
            switch (serialInfo.Parity)
            {
                case Standard.DynMvp.Devices.Serial.SerialParity.None:
                    serialDevice.Parity = Windows.Devices.SerialCommunication.SerialParity.None;
                    break;
                case Standard.DynMvp.Devices.Serial.SerialParity.Odd:
                    serialDevice.Parity = Windows.Devices.SerialCommunication.SerialParity.Odd;
                    break;
                case Standard.DynMvp.Devices.Serial.SerialParity.Even:
                    serialDevice.Parity = Windows.Devices.SerialCommunication.SerialParity.Even;
                    break;
                case Standard.DynMvp.Devices.Serial.SerialParity.Mark:
                    serialDevice.Parity = Windows.Devices.SerialCommunication.SerialParity.Mark;
                    break;
                case Standard.DynMvp.Devices.Serial.SerialParity.Space:
                    serialDevice.Parity = Windows.Devices.SerialCommunication.SerialParity.Space;
                    break;
            }

            switch (serialInfo.StopBits)
            {
                case Standard.DynMvp.Devices.Serial.SerialStopBitCount.One:
                    serialDevice.StopBits = Windows.Devices.SerialCommunication.SerialStopBitCount.One;
                    break;
                case Standard.DynMvp.Devices.Serial.SerialStopBitCount.OnePointFive:
                    serialDevice.StopBits = Windows.Devices.SerialCommunication.SerialStopBitCount.OnePointFive;
                    break;
                case Standard.DynMvp.Devices.Serial.SerialStopBitCount.Two:
                    serialDevice.StopBits = Windows.Devices.SerialCommunication.SerialStopBitCount.Two;
                    break;
            }

            switch (serialInfo.Handshake)
            {
                case Standard.DynMvp.Devices.Serial.SerialHandshake.None:
                    serialDevice.Handshake = Windows.Devices.SerialCommunication.SerialHandshake.None;
                    break;
                case Standard.DynMvp.Devices.Serial.SerialHandshake.RequestToSend:
                    serialDevice.Handshake = Windows.Devices.SerialCommunication.SerialHandshake.None;
                    break;
                case Standard.DynMvp.Devices.Serial.SerialHandshake.XOnXOff:
                    serialDevice.Handshake = Windows.Devices.SerialCommunication.SerialHandshake.None;
                    break;
                case Standard.DynMvp.Devices.Serial.SerialHandshake.RequestToSendXOnXOff:
                    serialDevice.Handshake = Windows.Devices.SerialCommunication.SerialHandshake.None;
                    break;
            }
        }

        static public async Task Initialize()
        {
            var setting = Singleton<DeviceSettings>.Instance;

            if (setting.DeviceInfoList.Count == 0)
                return;

            await InitializeGrabber(setting.DeviceInfoList.Where(i => i.DeviceType == DeviceType.FrameGrabber).ToList().ConvertAll<GrabberInfo>(i => (GrabberInfo)i));
            await InitializeLightController(setting.DeviceInfoList.Where(i => i.DeviceType == DeviceType.LightController).ToList().ConvertAll<LightControllerInfo>(i => (LightControllerInfo)i));


            //setting.GrabberInfoList.ForEach(grabberInfo => _grabbers.Concat(new[] { Grabber.CreateGrabber(grabberInfo) }));
            //InitializeLightCtrl(settings.LightCtrlInfoList);

            //InitializeMotion(settings.MotionInfoList, isVirtual);
            //InitializeDigitalIo(settings.DigitalIoInfoList, isVirtual);
            //InitializeLightCtrl(settings.LightCtrlInfoList, isVirtual);
            //InitializeDaqDevice(settings.DaqChannelPropertyList, isVirtual);
            //InitializeSerialDevice(settings.SerialDeviceInfoList, isVirtual);
            //InitializeMachineIF(settings.MachineIfSetting, isVirtual);
        }
    }
}