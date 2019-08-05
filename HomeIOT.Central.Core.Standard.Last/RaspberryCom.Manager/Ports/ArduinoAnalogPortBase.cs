using System.Text;
using RaspberryCom.Components;
using RaspberryCom.Enum;
using RaspberryCom.Ports;

namespace RaspberryCom
{
    public abstract class ArduinoAnalogPortBase : ArduinoPortBase
    {

        protected string GenerateCommand(CommandTypeEnum commandType, AnalogPortEnum port, int value)
        {
            StringBuilder generateCommand = new StringBuilder();
            generateCommand.Append("C=");
            switch (commandType)
            {
                case CommandTypeEnum.READ:
                    generateCommand.Append("R");
                    break;
                case CommandTypeEnum.WRITE:
                    generateCommand.Append("W");
                    break;
            }
            generateCommand.Append(",");
            generateCommand.Append("P=");
            generateCommand.Append(port.ToString());
            generateCommand.Append(",");
            generateCommand.Append("V=");
            generateCommand.Append(value.ToString());
            generateCommand.Append("|");

            return generateCommand.ToString();
        }
    }

    public abstract class ArduinoPortBase
    {
        private PortsMega2560Container.NotifyReadPort _notifyFunction;
        public void NotifyNewReadValue(int value)
        {
            if (this._notifyFunction != null)
            {
                value = ProcessValue(value);
                this._notifyFunction(value);
            }
        }
        public void SetNotifyReadNewDataInPort(PortsMega2560Container.NotifyReadPort notifyFunction)
        {
            this._notifyFunction = notifyFunction;
        }
        private IComponents component;
        public void SetComponent(IComponents component)
        {
            this.component = component;
        }

        protected int ProcessValue(int value)
        {
            int returnValue = value;
            if (this.component != null)
            {
                returnValue = this.component.Read(value);
            }
            return returnValue;
        }
    }
}