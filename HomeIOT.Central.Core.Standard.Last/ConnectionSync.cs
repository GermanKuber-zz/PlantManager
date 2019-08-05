using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Business;
using Core.Expose;
using Microsoft.AspNet.SignalR.Client;

namespace RaspberryCom.Sync
{
    public class ConnectionSync : IServerToCentralActions
    {
        #region Configuraciones
        private string urlHub = "http://192.168.0.97:45455/signalr";
        private string hub = "ServerToCentralHub";
        
        #endregion

        #region Privadas
        private HubConnection _connection;
        private IHubProxy _hubProxy;
        #endregion

        #region Publicas
        public Suscribe Suscribe;
        #endregion

        #region Constructor
        public ConnectionSync(string hub = "")
        {
            this._connection = new HubConnection(urlHub);
            var hubLocal = this.hub;
            if (!string.IsNullOrWhiteSpace(hub))
            {
                hubLocal = hub;
            }
            _hubProxy = _connection.CreateHubProxy(hubLocal);
            this.Suscribe = new Suscribe(_hubProxy);
        }
        #endregion

        #region Medodos Necesarios
        public Task Start()
        {
          

            return _connection.Start();
        }
        #endregion


        #region Metodos expuestos por el servidor
        public Task<IotCentral> ConectarCentralIot(string identifier)
        {
            return this._hubProxy.Invoke("ConectarCentralIot", identifier) as Task<IotCentral>;
        }

        public Task<IotCentral> GetConfiguracion(string identifier)
        {
            return this._hubProxy.Invoke("GetConfiguracion", identifier) as Task<IotCentral>;
        }

        public void ConectarNuevoDispositivo(Device device,string idIotCentralIdentifier)
        {
            this._hubProxy.Invoke("ConectarNuevoDispositivo", device, idIotCentralIdentifier);
        }

        public void EnviarData(Data data)
        {
            this._hubProxy.Invoke("EnviarData", data);
        }

        #endregion
    }

    public class Suscribe
    {
        private readonly IHubProxy _hubProxy;

        public Suscribe(IHubProxy hubProxy)
        {
            _hubProxy = hubProxy;
        }

        #region Metodos de suscripcion de eventos de servidor
        public void UpdateConfiguracion(CentralExpose.UpdateConfiguracionDelegate updateConfiguracion)
        {
            this._hubProxy.On<IotCentral>("UpdateConfiguracion", iotCentral => updateConfiguracion(iotCentral));
        }
        public void Reconectar(CentralExpose.ReconectarDelegate reconectar)
        {
            this._hubProxy.On<IotCentral>("Reconectar", iotCentral => reconectar());
        }

   

        public void EnviarComando(CentralExpose.EnviarComandoDelegate enviarComando)
        {
            this._hubProxy.On<Comando>("EnviarComando", comando => enviarComando(comando));
        }
        public Task<Data> EnviarComandoInmediato(CentralExpose.EnviarComandoInmediatoDelegate enviarComandoInmediato)
        {
            return this._hubProxy.On<Comando>("EnviarComandoInmediato", comando => enviarComandoInmediato(comando)) as Task<Data>;
        }
        public void EnviarColaDeComandos(CentralExpose.EnviarColaDeComandosDelegate enviarColaDeComandos)
        {
            this._hubProxy.On<List<Comando>>("EnviarColaDeComandos", comandos => enviarColaDeComandos(comandos));
        }
        #endregion
    }
}
