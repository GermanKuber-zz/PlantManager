using System.Collections.Generic;
using Core.Business;

namespace Core.Expose
{
    public class CentralExpose
    {
        public delegate void UpdateConfiguracionDelegate(IotCentral iotCentral);
        public delegate void ReconectarDelegate();
        public delegate void EnviarComandoDelegate(Comando comanndo);
        public delegate Data EnviarComandoInmediatoDelegate(Comando comando);
        public delegate void EnviarColaDeComandosDelegate(List<Comando> listCommandos);
    }
}
