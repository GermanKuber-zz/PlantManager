using System.Threading.Tasks;
using Core.Business;

namespace Core.Expose
{

    public interface IServerToCentralActions
    {
        Task<IotCentral> ConectarCentralIot(string identifier);
        Task<IotCentral> GetConfiguracion(string identifier);
        void ConectarNuevoDispositivo(Device device, string idIotCentralIdentifier);
        void EnviarData(Data data);
    }
}
