using System;

namespace Core.Business
{
    public class Comando
    {
        public PortAccionEnum Accion { get; set; }
        public string Pin { get; set; }
        public string Valor { get; set; }
        public DateTime HoraEnvioCliente { get; set; }
        public DateTime HoraEnvioServidor { get; set; }

        public Comando()
        {

        }
        public Comando(PortAccionEnum accion, string pin, string valor)
        {
            Accion = accion;
            Pin = pin ?? throw new ArgumentNullException(nameof(pin));
            Valor = valor ?? throw new ArgumentNullException(nameof(valor));
            HoraEnvioCliente = DateTime.Now;
            HoraEnvioServidor = DateTime.Now;
        }
        public static Comando Read(string pin) => new Comando
        {
            Accion = PortAccionEnum.READ,
            HoraEnvioCliente = DateTime.Now,
            HoraEnvioServidor = DateTime.Now,
            Pin = pin
        };
        public static Comando Write(string pin, string valor) => new Comando
        {
            Accion = PortAccionEnum.WRITE,
            HoraEnvioCliente = DateTime.Now,
            HoraEnvioServidor = DateTime.Now,
            Pin = pin,
            Valor = valor
        };
    }
}