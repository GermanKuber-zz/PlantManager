using RaspberryCom.Enum;
using RaspberryCom.Ports;

namespace RaspberryCom
{
    public class ArduinoAnalogAnalogPort : ArduinoAnalogPortBase
    {
        private AnalogPortEnum analogPort;
         private readonly PortsMega2560Container.WriteFunction _writeFunction;
        public void Write(int value)
        {
            string command = this.GenerateCommand(CommandTypeEnum.WRITE, analogPort, value);
            this._writeFunction(command);
        }
        public void Read()
        {
            string command = this.GenerateCommand(CommandTypeEnum.READ, analogPort, -1);
            this._writeFunction(command);
        }

 
        public ArduinoAnalogAnalogPort(PortsMega2560Container.WriteFunction writeFunction, AnalogPortEnum analogPort)
        {
            this._writeFunction = writeFunction;
            this.analogPort = analogPort;
        }
    }
}