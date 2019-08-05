using System;
using System.Collections.Generic;

namespace Core.Business
{
    public class Device
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Identifier { get; set; }
        public string Model { get; set; }
        public string Detalle { get; set; }
        public bool Verificado { get; set; }
        public bool Activo { get; set; }
        public bool Ignorado { get; set; }
        public bool Estado { get; set; }
        public Guid VersionConfiguration { get; set; }
        public virtual ICollection<Port> Puertos { get; set; } = new List<Port>();
        public Port GetPortByPin(string pin)
        {
            if (this.Puertos != null)
            {
                foreach (var currentPort in this.Puertos)
                {
                    if (currentPort.Pin == pin)
                    {
                        return currentPort;
                    }
                }
            }
            return null;
        }
    }
}