using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace UWP.Base.Services.Device
{
    public class SerialDeviceEventHandler
    {
        private static SerialDeviceEventHandler eventHandlerForDevice;
        private static Object singletonCreationLock = new Object();

        private String deviceSelector;
        private DeviceWatcher deviceWatcher;

        private DeviceInformation deviceInformation;
        private DeviceAccessInformation deviceAccessInformation;
        private SerialDevice device;

        private SuspendingEventHandler appSuspendEventHandler;
        private EventHandler<Object> appResumeEventHandler;

        private TypedEventHandler<SerialDeviceEventHandler, DeviceInformation> deviceCloseCallback;
        private TypedEventHandler<SerialDeviceEventHandler, DeviceInformation> deviceConnectedCallback;

        private TypedEventHandler<DeviceWatcher, DeviceInformation> deviceAddedEventHandler;
        private TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> deviceRemovedEventHandler;
        private TypedEventHandler<DeviceAccessInformation, DeviceAccessChangedEventArgs> deviceAccessEventHandler;

        private Boolean watcherSuspended;
        private Boolean watcherStarted;
        private Boolean isEnabledAutoReconnect;
        
        public static SerialDeviceEventHandler Current
        {
            get
            {
                if (eventHandlerForDevice == null)
                {
                    lock (singletonCreationLock)
                    {
                        if (eventHandlerForDevice == null)
                        {
                            CreateNewEventHandlerForDevice();
                        }
                    }
                }

                return eventHandlerForDevice;
            }
        }
        
        public static void CreateNewEventHandlerForDevice()
        {
            eventHandlerForDevice = new SerialDeviceEventHandler();
        }

        public TypedEventHandler<SerialDeviceEventHandler, DeviceInformation> OnDeviceClose
        {
            get
            {
                return deviceCloseCallback;
            }

            set
            {
                deviceCloseCallback = value;
            }
        }

        public TypedEventHandler<SerialDeviceEventHandler, DeviceInformation> OnDeviceConnected
        {
            get
            {
                return deviceConnectedCallback;
            }

            set
            {
                deviceConnectedCallback = value;
            }
        }

        public Boolean IsDeviceConnected
        {
            get
            {
                return (device != null);
            }
        }

        public SerialDevice Device
        {
            get
            {
                return device;
            }
        }
        
        public DeviceInformation DeviceInformation
        {
            get
            {
                return deviceInformation;
            }
        }
        
        public DeviceAccessInformation DeviceAccessInformation
        {
            get
            {
                return deviceAccessInformation;
            }
        }
        
        public String DeviceSelector
        {
            get
            {
                return deviceSelector;
            }
        }
        
        public Boolean IsEnabledAutoReconnect
        {
            get
            {
                return isEnabledAutoReconnect;
            }
            set
            {
                isEnabledAutoReconnect = value;
            }
        }
        
        public async Task<Boolean> OpenDeviceAsync(DeviceInformation deviceInfo, String deviceSelector)
        {
            device = await SerialDevice.FromIdAsync(deviceInfo.Id);

            Boolean successfullyOpenedDevice = false;
            //NotifyType notificationStatus;
            String notificationMessage = null;

            // Device could have been blocked by user or the device has already been opened by another app.
            if (device != null)
            {
                successfullyOpenedDevice = true;

                deviceInformation = deviceInfo;
                this.deviceSelector = deviceSelector;

                //notificationStatus = NotifyType.StatusMessage;
                //notificationMessage = "Device " + deviceInformation.Id + " opened";

                // Notify registered callback handle that the device has been opened
                if (deviceConnectedCallback != null)
                {
                    deviceConnectedCallback(this, deviceInformation);
                }

                if (appSuspendEventHandler == null || appResumeEventHandler == null)
                {
                    RegisterForAppEvents();
                }

                // Register for DeviceAccessInformation.AccessChanged event and react to any changes to the
                // user access after the device handle was opened.
                if (deviceAccessEventHandler == null)
                {
                    RegisterForDeviceAccessStatusChange();
                }

                // Create and register device watcher events for the device to be opened unless we're reopening the device
                if (deviceWatcher == null)
                {
                    deviceWatcher = DeviceInformation.CreateWatcher(deviceSelector);

                    RegisterForDeviceWatcherEvents();
                }

                if (!watcherStarted)
                {
                    // Start the device watcher after we made sure that the device is opened.
                    StartDeviceWatcher();
                }
            }
            else
            {
                successfullyOpenedDevice = false;

                //notificationStatus = NotifyType.ErrorMessage;

                var deviceAccessStatus = DeviceAccessInformation.CreateFromId(deviceInfo.Id).CurrentStatus;

                if (deviceAccessStatus == DeviceAccessStatus.DeniedByUser)
                {
                    notificationMessage = "Access to the device was blocked by the user : " + deviceInfo.Id;
                }
                else if (deviceAccessStatus == DeviceAccessStatus.DeniedBySystem)
                {
                    // This status is most likely caused by app permissions (did not declare the device in the app's package.appxmanifest)
                    // This status does not cover the case where the device is already opened by another app.
                    notificationMessage = "Access to the device was blocked by the system : " + deviceInfo.Id;
                }
                else
                {
                    // Most likely the device is opened by another app, but cannot be sure
                    notificationMessage = "Unknown error, possibly opened by another app : " + deviceInfo.Id;
                }
            }

            //MainPage.Current.NotifyUser(notificationMessage, notificationStatus);

            return successfullyOpenedDevice;
        }
        
        public void CloseDevice()
        {
            if (IsDeviceConnected)
            {
                CloseCurrentlyConnectedDevice();
            }

            if (deviceWatcher != null)
            {
                if (watcherStarted)
                {
                    StopDeviceWatcher();

                    UnregisterFromDeviceWatcherEvents();
                }

                deviceWatcher = null;
            }

            if (deviceAccessInformation != null)
            {
                UnregisterFromDeviceAccessStatusChange();

                deviceAccessInformation = null;
            }

            if (appSuspendEventHandler != null || appResumeEventHandler != null)
            {
                UnregisterFromAppEvents();
            }

            deviceInformation = null;
            deviceSelector = null;

            deviceConnectedCallback = null;
            deviceCloseCallback = null;

            isEnabledAutoReconnect = true;
        }

        private SerialDeviceEventHandler()
        {
            watcherStarted = false;
            watcherSuspended = false;
            isEnabledAutoReconnect = true;
        }
        
        private async void CloseCurrentlyConnectedDevice()
        {
            if (device != null)
            {
                if (deviceCloseCallback != null)
                {
                    deviceCloseCallback(this, deviceInformation);
                }
                
                device.Dispose();

                device = null;
                
                String deviceId = deviceInformation.Id;

                //await rootPage.Dispatcher.RunAsync(
                //    CoreDispatcherPriority.Normal,
                //    new DispatchedHandler(() =>
                //    {
                //        MainPage.Current.NotifyUser(deviceId + " is closed", NotifyType.StatusMessage);
                //    }));
            }
        }
        
        private void RegisterForAppEvents()
        {
            //appSuspendEventHandler = new SuspendingEventHandler(SerialDeviceEventHandler.Current.OnAppSuspension);
            //appResumeEventHandler = new EventHandler<Object>(SerialDeviceEventHandler.Current.OnAppResume);

            // This event is raised when the app is exited and when the app is suspended
            //App.Current.Suspending += appSuspendEventHandler;
            //App.Current.Resuming += appResumeEventHandler;
        }

        private void UnregisterFromAppEvents()
        {
            //App.Current.Suspending -= appSuspendEventHandler;
            //appSuspendEventHandler = null;

            //App.Current.Resuming -= appResumeEventHandler;
            //appResumeEventHandler = null;
        }
        
        private void RegisterForDeviceWatcherEvents()
        {
            deviceAddedEventHandler = new TypedEventHandler<DeviceWatcher, DeviceInformation>(this.OnDeviceAdded);

            deviceRemovedEventHandler = new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(this.OnDeviceRemoved);

            deviceWatcher.Added += deviceAddedEventHandler;

            deviceWatcher.Removed += deviceRemovedEventHandler;
        }

        private void UnregisterFromDeviceWatcherEvents()
        {
            deviceWatcher.Added -= deviceAddedEventHandler;
            deviceAddedEventHandler = null;

            deviceWatcher.Removed -= deviceRemovedEventHandler;
            deviceRemovedEventHandler = null;
        }
        
        private void RegisterForDeviceAccessStatusChange()
        {
            // Enable the following registration ONLY if the Serial device under test is non-internal.
            //

            //deviceAccessInformation = DeviceAccessInformation.CreateFromId(deviceInformation.Id);
            //deviceAccessEventHandler = new TypedEventHandler<DeviceAccessInformation, DeviceAccessChangedEventArgs>(this.OnDeviceAccessChanged);
            //deviceAccessInformation.AccessChanged += deviceAccessEventHandler;
        }

        private void UnregisterFromDeviceAccessStatusChange()
        {
            deviceAccessInformation.AccessChanged -= deviceAccessEventHandler;

            deviceAccessEventHandler = null;
        }

        private void StartDeviceWatcher()
        {
            watcherStarted = true;

            if ((deviceWatcher.Status != DeviceWatcherStatus.Started)
                && (deviceWatcher.Status != DeviceWatcherStatus.EnumerationCompleted))
            {
                deviceWatcher.Start();
            }
        }

        private void StopDeviceWatcher()
        {
            if ((deviceWatcher.Status == DeviceWatcherStatus.Started)
                || (deviceWatcher.Status == DeviceWatcherStatus.EnumerationCompleted))
            {
                deviceWatcher.Stop();
            }

            watcherStarted = false;
        }
        
        private void OnAppSuspension(Object sender, SuspendingEventArgs args)
        {
            if (watcherStarted)
            {
                watcherSuspended = true;
                StopDeviceWatcher();
            }
            else
            {
                watcherSuspended = false;
            }

            CloseCurrentlyConnectedDevice();
        }
        
        private void OnAppResume(Object sender, Object args)
        {
            if (watcherSuspended)
            {
                watcherSuspended = false;
                StartDeviceWatcher();
            }
        }
        
        private void OnDeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInformationUpdate)
        {
            if (IsDeviceConnected && (deviceInformationUpdate.Id == deviceInformation.Id))
            {
                // The main reasons to close the device explicitly is to clean up resources, to properly handle errors,
                // and stop talking to the disconnected device.
                CloseCurrentlyConnectedDevice();
            }
        }
        
        private async void OnDeviceAdded(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            if ((deviceInformation != null) && (deviceInfo.Id == deviceInformation.Id) && !IsDeviceConnected && isEnabledAutoReconnect)
            {
                await OpenDeviceAsync(deviceInformation, deviceSelector);
            }
        }

        /// <summary>
        /// Close the device if the device access was denied by anyone (system or the user) and reopen it if permissions are allowed again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private async void OnDeviceAccessChanged(DeviceAccessInformation sender, DeviceAccessChangedEventArgs eventArgs)
        {
            if ((eventArgs.Status == DeviceAccessStatus.DeniedBySystem)
                || (eventArgs.Status == DeviceAccessStatus.DeniedByUser))
            {
                CloseCurrentlyConnectedDevice();
            }
            else if ((eventArgs.Status == DeviceAccessStatus.Allowed) && (deviceInformation != null) && isEnabledAutoReconnect)
            {
                await OpenDeviceAsync(deviceInformation, deviceSelector);
            }
        }
    }
}
