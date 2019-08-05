using System.Collections.Generic;
using System.Linq;

namespace Core.Business
{
    public class IotCentral
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Identifier { get; set; }
        public string Model { get; set; }
        public string Detalle { get; set; }
        public bool Verificado { get; set; }
        public bool Activo { get; set; }
        public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

        public IotCentral()
        {

        }

        public Port GetPortByPin(string pin)
        {
            if (Devices != null)
            {
                return this.Devices.Select(currentDevice => currentDevice.GetPortByPin(pin)).FirstOrDefault();
            }
            return null;
        }
    }
}