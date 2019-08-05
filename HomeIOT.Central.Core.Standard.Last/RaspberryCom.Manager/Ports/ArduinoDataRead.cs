using System.Reflection.Metadata;
using System.Threading;

namespace RaspberryCom.Ports
{
    public class ArduinoDataRead
    {
        public string Pin { get; set; }
        public string Value { get; set; }

        public ArduinoDataRead(string pin, string value)
        {
            this.Pin = pin;
            this.Value = value;
        }

        public ArduinoDataRead()
        {
            
        }
    }
}