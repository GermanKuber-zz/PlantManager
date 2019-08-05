using Core.Business;
using RaspberryCom.Interfaces;
using RaspberryCom.Process;
using System.Threading.Tasks;

namespace RaspberryCom.Ports
{
    public class PortsMega2560Container : IPortsContainer<ArduinoMega2560PortListAnalog, ArduinoMega2560PortListDigital>
    {

        #region Delegados
        public delegate  Task WriteFunction(string commando);
        public delegate void NotifyReadPort(int value);
        public delegate void NotifyReadGeneralPort(ArduinoDataRead data);
        #endregion

        #region Constructor

        public PortsMega2560Container(WriteFunction writeFunction)
        {
            this._writeFunction = writeFunction;
            this.Analog = new ArduinoMega2560PortListAnalog(writeFunction);
            this.Digital = new ArduinoMega2560PortListDigital(writeFunction);
            this._writeFunction = writeFunction;
        }

        #endregion

        #region Privados

        private NotifyReadGeneralPort _notifiFunctionGeneralPort;
        private bool _notifyReadGeneral = false;
        private WriteFunction _writeFunction;
        private bool _activeProcessRead = false;
        private ProcessReadData _processReadData;

        #endregion

        #region Metodos Publicos

    
        public ArduinoMega2560PortListAnalog Analog { get; }
        public ArduinoMega2560PortListDigital Digital { get; }
        public void ReadDataPortArduino(ArduinoDataRead data)
        {
            this.VerificarAnalogico(data);
            this.VerificarDigital(data);
            this.NotificarAllPortRead(data);
        }
        public void NotificateAllPortRead(NotifyReadGeneralPort notifiFunctionGeneralPort)
        {
            this._notifiFunctionGeneralPort = notifiFunctionGeneralPort;
            if (this._notifiFunctionGeneralPort != null)
            {
                this._notifyReadGeneral = true;
            }
            
        }
        public void ActiveProcessRead(IotCentral iotConfigurations, 
            ProcessReadData.NotificarPuertoDelegate urgencia = null,
            ProcessReadData.NotificarPuertoDelegate notificar = null)
        {
            this._activeProcessRead = true;
            this._processReadData = new ProcessReadData(iotConfigurations, this._writeFunction,urgencia,notificar);
        }
        public void ChangeConfigurationRead(IotCentral iotConfigurations)
        {
            this._processReadData = new ProcessReadData(iotConfigurations, this._writeFunction);
        }
        public void CancelNotificateAllPortRead(NotifyReadGeneralPort notifiFunctionGeneralPort)
        {
            this._notifiFunctionGeneralPort = null;
            this._notifyReadGeneral = false;

        }

        #endregion

        #region Metodos Privados
        private void VerificarAnalogico(ArduinoDataRead data)
        {
            if (data.Pin == "A10")
            {
                this.Analog.A10.NotifyNewReadValue(int.Parse(data.Value));
            }
            if (data.Pin == "A11")
            {
                this.Analog.A11.NotifyNewReadValue(int.Parse(data.Value));
            }
            if (data.Pin == "A12")
            {
                this.Analog.A12.NotifyNewReadValue(int.Parse(data.Value));
            }
            if (data.Pin == "A13")
            {
                this.Analog.A13.NotifyNewReadValue(int.Parse(data.Value));
            }
        }
        private void VerificarDigital(ArduinoDataRead data)
        {
            if (data.Pin == "2")
            {
                this.Digital.P16.NotifyNewReadValue(int.Parse(data.Value));
            }
            if (data.Pin == "3")
            {
                this.Digital.P16.NotifyNewReadValue(int.Parse(data.Value));
            }
            if (data.Pin == "4")
            {
                this.Digital.P4.NotifyNewReadValue(int.Parse(data.Value));
            }
            if (data.Pin == "5")
            {
                this.Digital.P16.NotifyNewReadValue(int.Parse(data.Value));
            }
            if (data.Pin == "6")
            {
                this.Digital.P16.NotifyNewReadValue(int.Parse(data.Value));
            }
            if (data.Pin == "7")
            {
                this.Digital.P16.NotifyNewReadValue(int.Parse(data.Value));
            }
            if (data.Pin == "8")
            {
                this.Digital.P16.NotifyNewReadValue(int.Parse(data.Value));
            }
            if (data.Pin == "21")
            {
                this.Digital.P21.NotifyNewReadValue(int.Parse(data.Value));
            }
        }

        private void NotificarAllPortRead(ArduinoDataRead data)
        {
            if (this._notifyReadGeneral)
            {
                this._notifiFunctionGeneralPort?.Invoke(data);
            }
            if (this._activeProcessRead)
            {
                this._processReadData.ReadData(data.Pin,data.Value);
            }
        }


        #endregion
    }
}