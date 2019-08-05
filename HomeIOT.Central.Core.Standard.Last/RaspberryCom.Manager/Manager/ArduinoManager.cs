using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business;
using RaspberryCom.ArduinoConnectionType;
using RaspberryCom.Interfaces;
using RaspberryCom.Ports;

namespace RaspberryCom.Manager
{
    public class ArduinosManager
    {
    
        public IArduinoConnection<PortsMega2560Container, ArduinoMega2560PortListAnalog, ArduinoMega2560PortListDigital> ArduinoMega2560 { get; set; }

        public ArduinosManager()
        {
            this.ArduinoMega2560 = new ArduinoMega2560UsbConnection();
            
        }
  
        public async Task ConnectAll(Device deviceConnect)
        {
            await this.ArduinoMega2560.Connect(deviceConnect);
        }
        public async Task<List<Device>> GetDevices()
        {
            return await this.ArduinoMega2560.GetDevices();
        }
   
    }
}
