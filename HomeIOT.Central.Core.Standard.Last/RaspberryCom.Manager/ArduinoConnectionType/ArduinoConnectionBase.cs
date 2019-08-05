using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.Business;
using RaspberryCom.Helpers;
using RaspberryCom.Interfaces;
using RaspberryCom.Ports;
using RaspberryCom.Sync;

namespace RaspberryCom.ArduinoConnectionType
{
    public abstract class ArduinoConnectionBase<TPortContainer, TPortAnalog, TPortDigital> : IArduinoConnection<TPortContainer, TPortAnalog, TPortDigital>
                                                                                            where TPortAnalog : IArduinoPortListAnalog
                                                                                            where TPortDigital : IArduinoPortListDigital
                                                                                            where TPortContainer : IPortsContainer<TPortAnalog, TPortDigital>
    {

        private const int WAIT_SECOND_COMMAND = 5;
        protected bool CammandEnable = true;

        protected abstract string DeviceId { get; }
        public Task WriteCommand(Comando commando)
        {
            var concatCommand = "";
            switch (commando.Accion)
            {
                case PortAccionEnum.READ:
                    concatCommand = WriteCommandClass.Generate("R", commando.Pin, commando.Valor);
                    break;
                case PortAccionEnum.WRITE:
                    concatCommand = WriteCommandClass.Generate("W", commando.Pin, commando.Valor);
                    break;

            }
            if (!string.IsNullOrWhiteSpace(concatCommand))
            {
                return this.Write(concatCommand);
            }
            return null;
        }

        public abstract TPortContainer Ports { get; }
        public virtual async Task Connect(Device device) { }
        public abstract Task<List<Device>> GetDevices();

        protected void ProcessArduinoResults(string data)
        {

            data = data.Replace("\n", "").Replace("\r", "");
            //P=A12,V=708
            if (data.StartsWith("="))
            {
                //Se introduce este fix, dado que la lectura elmina el primer caracter
                data = "P" + data;
            }
            var multipleCommand = data.Split('|');

            foreach (var command in multipleCommand)
            {
                try
                {
                    if (command.Contains("P=") && command.Contains("V="))
                    {
                        int indexP = command.IndexOf("P=", StringComparison.Ordinal);
                        int indexV = command.IndexOf("V=", StringComparison.Ordinal) + 2;
                        int indexComa = command.IndexOf(",", StringComparison.Ordinal) - 2;
                        string pin = command.Substring(2, indexComa);
                        var countFin = command.Length - 5 - indexComa;
                        string value = command.Substring(indexV, countFin);
                        ArduinoDataRead dataSendArduino = new ArduinoDataRead(pin, value);
                        this.Ports.ReadDataPortArduino(dataSendArduino);
                    }
                }
                catch (Exception ex)
                {
                    //throw new Exception("Error en el procesamiento de comando. Commando mal Formateado : " + data);
                }
            }
        }

        private bool Reconnect()
        {
            bool succeeded = false;

            //this.Connect();

            return succeeded;
        }

        public void Disconected()
        {
            if (!this.Disconnected)
            {
                this.Disconnected = true;
                this.Reconnect();
            }

        }

        private bool Disconnected
        {
            get { return this.Disconnected; }
            set
            {
                this.Disconnected = value;

            }
        }

        protected async Task<bool> ReconnectWaitUseCommand()
        {
            CammandEnable = false;
            bool succeeded = false;
            var count = 0;
            var exit = false;
            var cts = new CancellationToken();
            await Task.Delay(new TimeSpan(6000), cts).ContinueWith(_ =>
             {
                 //TODO: Verificar porque despues de reconectarse no lee datos
                 CammandEnable = true;
             }, cts);

            return succeeded;
        }
        /// <summary>
        /// Permite escribir un comando directamente.
        /// </summary>
        /// <param name="command"></param>
        public Task WriteCommand(string command)
        {
            return this.Write(command);
        }
        /// <summary>
        /// Permite escribir un comando directamente.
        /// </summary>
        /// <param name="command"></param>
        public Task WriteCommand(string command, string pin, string value)
        {
            var concatCommand = WriteCommandClass.Generate(command, pin, value);
            return this.Write(concatCommand);
        }

        protected abstract Task Write(string writeCommand);


    }
}