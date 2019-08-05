using RaspberryCom.Enum;

namespace RaspberryCom.Ports
{
    public class ArduinoDigitalPort : ArduinoDigitalPortBase
    {
        private readonly PortsMega2560Container.WriteFunction _writeFunction;
      
        private readonly DigitalPortEnum _port;
        public void Write(int value)
        {
            string command = this.GenerateCommand(CommandTypeEnum.WRITE, _port, value);
            this._writeFunction(command);
        }
        public void Read()
        {
            string command = this.GenerateCommand(CommandTypeEnum.READ, _port, -1);
            this._writeFunction(command);
        }
     

        public ArduinoDigitalPort(DigitalPortEnum port, PortsMega2560Container.WriteFunction writeFunction)
        {
            this._writeFunction = writeFunction;
            this._port = port;
        }
    }
}