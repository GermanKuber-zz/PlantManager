using RaspberryCom.Enum;
using RaspberryCom.Interfaces;

namespace RaspberryCom.Ports
{
    public class ArduinoMega2560PortListAnalog : IArduinoPortListAnalog
    {
        private PortsMega2560Container.WriteFunction _writeFunction;
        public ArduinoAnalogAnalogPort A10 { get; set; }
        public ArduinoAnalogAnalogPort A11 { get; set; }
        public ArduinoAnalogAnalogPort A12 { get; set; }
        public ArduinoAnalogAnalogPort A13 { get; set; }
        public ArduinoMega2560PortListAnalog(PortsMega2560Container.WriteFunction writeFunction)
        {
            this.A10 = new ArduinoAnalogAnalogPort(writeFunction, AnalogPortEnum.A10);
            this.A11 = new ArduinoAnalogAnalogPort(writeFunction, AnalogPortEnum.A11);
            this.A12 = new ArduinoAnalogAnalogPort(writeFunction, AnalogPortEnum.A12);
            this.A13 = new ArduinoAnalogAnalogPort(writeFunction, AnalogPortEnum.A13);
            this._writeFunction = writeFunction;
        }
    }
}