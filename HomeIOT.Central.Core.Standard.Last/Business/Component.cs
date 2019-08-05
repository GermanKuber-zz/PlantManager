namespace Core.Business
{
    public class Component
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public ComponentTypeEnum Type { get; set; }
        public string ImagenOn { get; set; }
        public string ImagenOff { get; set; }
        public string IdentificadorComponente { get; set; }
    }
}