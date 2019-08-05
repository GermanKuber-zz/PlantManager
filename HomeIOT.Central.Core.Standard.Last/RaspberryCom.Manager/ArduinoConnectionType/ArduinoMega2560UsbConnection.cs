using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using Core.Business;
using RaspberryCom.Ports;
using System.Diagnostics;

namespace RaspberryCom.ArduinoConnectionType
{
    class DeviceAndReaderContainer
    {
        public int Id { get; set; }
        public DataReader Reader { get; set; }
        public DataWriter DataWriter { get; set; }
        public DeviceInformation DeviceInformation { get; set; }
        public SerialDevice SerialDevice { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
    public class ArduinoMega2560UsbConnection : ArduinoConnectionBase<PortsMega2560Container, ArduinoMega2560PortListAnalog, ArduinoMega2560PortListDigital>
    {
        #region Privados
        //private SerialDevice _serialPort = null;
        //private DataWriter _dataWriteObject = null;
        //private DataReader _dataReaderObject = null;
        private CancellationTokenSource _readCancellationTokenSource;
        private List<DeviceAndReaderContainer> containerList = new List<DeviceAndReaderContainer>();
        protected override string DeviceId { get; }
        #endregion

        #region Constructor
        public ArduinoMega2560UsbConnection()
        {
            this.Ports = new PortsMega2560Container(this.Write);
            var watcher = DeviceInformation.CreateWatcher();
            watcher.EnumerationCompleted += Watcher_EnumerationCompleted;
            watcher.Added += WatcherOnAdded;
            watcher.Updated += WatcherOnUpdated;
            watcher.Removed += WatcherOnRemoved;
            watcher.Stopped += WatcherOnStopped;
        }

        private void Watcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
        }

        private void WatcherOnUpdated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
        }

        private void WatcherOnStopped(DeviceWatcher sender, object args)
        {
        }

        private void WatcherOnRemoved(DeviceWatcher sender, DeviceInformationUpdate args)
        {
        }

        private void WatcherOnAdded(DeviceWatcher sender, DeviceInformation args)
        {
        }

        #endregion

        #region Metodos Publicos
        public override PortsMega2560Container Ports { get; }

        public override async Task Connect(Device device)
        {
            await ListAvailablePorts(device);
        }

        public override async Task<List<Device>> GetDevices()
        {
            var devices = new List<Device>();
            try
            {
                string aqs = SerialDevice.GetDeviceSelector();
                var devicesList = await DeviceInformation.FindAllAsync(aqs);

                if (devicesList != null && devicesList.Count > 0)
                {
                    devices.AddRange(devicesList.Select(device => new Device()
                    {
                        Identifier = device.Id
                    }));
                }
            }
            catch (Exception ex)
            {
            }
            return devices;
        }
        #endregion

        #region Metodos Privados

        private async Task ListAvailablePorts(Device deviceConnect)
        {
            try
            {
                string aqs = SerialDevice.GetDeviceSelector();

                var devicesList = await DeviceInformation.FindAllAsync(aqs);

                if (devicesList != null && devicesList.Count > 0)
                {
                    foreach (var device in devicesList)
                    {
                        if (device.Id.Contains(deviceConnect.Identifier))
                        {
                            await Connect(device);
                            deviceConnect.Estado = true;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }
        private async Task Connect(DeviceInformation entry)
        {
            try
            {
                var container = CancelAsyncTask(entry);
                if (container != null)
                {
                    container.CancellationTokenSource = new CancellationTokenSource();
                    //await Task.Delay(3000);
                }

                SerialDevice serialPort = await SerialDevice.FromIdAsync(entry.Id);
                container.SerialDevice = serialPort;
                if (serialPort != null)
                {
                    serialPort.WriteTimeout = TimeSpan.FromMilliseconds(1000);
                    serialPort.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                    serialPort.BaudRate = 115200;

                    serialPort.Parity = SerialParity.None;
                    serialPort.StopBits = SerialStopBitCount.One;
                    serialPort.DataBits = 8;

                    serialPort.Handshake = SerialHandshake.None;
                    if (container != null)
                        await Listen(container);

                }
            }
            catch (Exception ex)
            {

            }

        }

        private DeviceAndReaderContainer CancelAsyncTask(DeviceInformation device)
        {


            var exist = false;
            foreach (var deviceAndReaderContainer in this.containerList)
            {
                if (deviceAndReaderContainer.DeviceInformation != null &&
                    deviceAndReaderContainer.DeviceInformation.Id == device.Id)
                {

                    if (deviceAndReaderContainer.CancellationTokenSource != null)
                    {
                        if (!deviceAndReaderContainer.CancellationTokenSource.IsCancellationRequested)
                        {
                            deviceAndReaderContainer.CancellationTokenSource.Cancel();
                            deviceAndReaderContainer.SerialDevice?.Dispose();
                            deviceAndReaderContainer.SerialDevice = null;
                            exist = true;

                            return deviceAndReaderContainer;
                        }
                    }
                }
            }
            if (!exist)
            {
                if (this.containerList == null)
                    this.containerList = new List<DeviceAndReaderContainer>();
                var deviceRead = new DeviceAndReaderContainer();
                deviceRead.DeviceInformation = device;
                this.containerList.Add(deviceRead);

                return deviceRead;
            }

            return null;

        }

        protected override Task Write(string writeCommand)
        {
            Task returnTask = null;
            try
            {

                foreach (var deviceAndReaderContainer in this.containerList)
                {
                    if (deviceAndReaderContainer.SerialDevice != null)
                    {
                        if (deviceAndReaderContainer.DataWriter == null)
                        {
                            deviceAndReaderContainer.DataWriter = new DataWriter(deviceAndReaderContainer.SerialDevice.OutputStream);

                        }
                        returnTask = WriteAsync(writeCommand);
                        try
                        {
                            //TODO: Verificar porque se rompe
                            deviceAndReaderContainer.DataWriter.DetachStream();
                            deviceAndReaderContainer.DataWriter.DetachBuffer();
                            deviceAndReaderContainer.DataWriter = null;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The device does not recognize the command"))
                {

                }

            }
            finally
            {
                foreach (var deviceAndReaderContainer in this.containerList)
                {
                    // Cleanup once complete
                    if (deviceAndReaderContainer.DataWriter != null)
                    {
                        try
                        {
                            //TODO: Verificar porque se rompe
                            //_dataWriteObject.DetachStream();
                            //_dataWriteObject = null;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            return returnTask;
        }
        private async Task WriteAsync(string writeCommand)
        {

            Task<UInt32> storeAsyncTask;

            foreach (var deviceAndReaderContainer in this.containerList)
            {
                try
                {
                    if (writeCommand.Length != 0)
                    {

                        // Load the text from the sendText input text box to the dataWriter object
                        deviceAndReaderContainer.DataWriter.WriteString(writeCommand);

                        // Launch an async task to complete the write operation
                        storeAsyncTask = deviceAndReaderContainer.DataWriter.StoreAsync().AsTask();

                        UInt32 bytesWritten = await storeAsyncTask;
                        //Se envio el comando
                    }
                }
                catch (Exception ex)
                {

                }
            }


        }

        private void CloseDevice(SerialDevice serialPort)
        {

            serialPort?.Dispose();
            serialPort = null;
        }

        private async Task Listen(DeviceAndReaderContainer container)
        {


            try
            {
                if (container.SerialDevice != null)
                {
                    container.Reader = new DataReader(container.SerialDevice.InputStream);

                    // keep reading the serial input
                    while (true)
                    {
                        await ReadAsync(container);
                    }
                }
            }
            catch (Exception ex)
            {

                if (ex.GetType().Name == "TaskCanceledException")
                {

                    CloseDevice(container.SerialDevice);
                }
                else
                {

                }
            }
            finally
            {
                // Cleanup once complete
                if (container.Reader != null)
                {
                    container.Reader.DetachStream();
                    container.Reader = null;
                }

            }
        }
        private async Task ReadAsync(DeviceAndReaderContainer container)
        {
            try
            {


                //Realiza la lectura de puerto serie

                uint ReadBufferLength = 1;

                // If task cancellation was requested, comply
                container.CancellationTokenSource.Token.ThrowIfCancellationRequested();

                // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
                container.Reader.InputStreamOptions = InputStreamOptions.Partial;

                // Create a task object to wait for data on the serialPort.InputStream
                var loadAsyncTask = container.Reader.LoadAsync(ReadBufferLength).AsTask(container.CancellationTokenSource.Token);

                // Launch the task and wait
                UInt32 bytesRead = await loadAsyncTask;
                if (bytesRead > 0)
                {

                    var data = container.Reader.ReadString(bytesRead);


                    concatCommand = concatCommand + data;
                    if (data == "\n")
                    {
                        ProcessArduinoResults(concatCommand);
                        Debug.WriteLine("L - " + contador + " : " + concatCommand);
                        ++contador;
                        concatCommand = string.Empty;
                    }



                }
            }
            catch (Exception ex)
            {

                //throw ex;
            }
        }
        private string concatCommand = "";
        private int contador = 0;
        private void CancelReadTask()
        {
            if (_readCancellationTokenSource != null)
            {
                if (!_readCancellationTokenSource.IsCancellationRequested)
                {
                    _readCancellationTokenSource.Cancel();
                }
            }
        }
        #endregion
    }

}
