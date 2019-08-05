using System.Collections.Generic;

namespace Core.Business
{
    public class Port
    {
        public int Id { get; set; }

        public int DeviceID { get; set; }
        public virtual Device Device { get; set; }

        public Component Componente { get; set; }
        public int? ReactionId { get; set; }
        public virtual Reaction Reaction { get; set; }
        public string Pin { get; set; }
        public string Nombre { get; set; }
        public string Value { get; set; }
        public ComponentTypeEnum Type { get; set; }
        public virtual ICollection<Reaction> ReActions { get; set; }
        public PortAccionEnum Accion { get; set; }
    }
}