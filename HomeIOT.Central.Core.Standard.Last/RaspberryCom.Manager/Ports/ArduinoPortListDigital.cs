using RaspberryCom.Enum;
using RaspberryCom.Interfaces;

namespace RaspberryCom.Ports
{
    public class ArduinoMega2560PortListDigital : IArduinoPortListDigital
    {
        private PortsMega2560Container.WriteFunction _writeFunction;
        public ArduinoDigitalPort P2 { get; set; }
        public ArduinoDigitalPort P3 { get; set; }
        public ArduinoDigitalPort P4 { get; set; }
        public ArduinoDigitalPort P5 { get; set; }
        public ArduinoDigitalPort P6 { get; set; }
        public ArduinoDigitalPort P7 { get; set; }
        public ArduinoDigitalPort P8 { get; set; }
        public ArduinoDigitalPort P16 { get; set; }
        public ArduinoDigitalPort P21 { get; set; }
        public ArduinoMega2560PortListDigital(PortsMega2560Container.WriteFunction writeFunction)
        {
            this.P2 = new ArduinoDigitalPort(DigitalPortEnum.P2, writeFunction);
            this.P3 = new ArduinoDigitalPort(DigitalPortEnum.P3, writeFunction);
            this.P4 = new ArduinoDigitalPort(DigitalPortEnum.P4, writeFunction);
            this.P5 = new ArduinoDigitalPort(DigitalPortEnum.P5, writeFunction);
            this.P6 = new ArduinoDigitalPort(DigitalPortEnum.P6, writeFunction);
            this.P7 = new ArduinoDigitalPort(DigitalPortEnum.P7, writeFunction);
            this.P8 = new ArduinoDigitalPort(DigitalPortEnum.P8, writeFunction);
            this.P16 = new ArduinoDigitalPort(DigitalPortEnum.P16, writeFunction);
            this.P21 = new ArduinoDigitalPort(DigitalPortEnum.P21, writeFunction);
            this._writeFunction = writeFunction;
        }
    }
}