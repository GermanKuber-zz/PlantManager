using System;
using Core.Business;
using RaspberryCom.Enum;
using RaspberryCom.Helpers;
using RaspberryCom.Ports;
using RaspberryCom.Process.ProcesatorData;

namespace RaspberryCom.Process
{
    public class ProcessReadData
    {
        #region Privados
        private IotCentral _iotConfigurations;
        private readonly PortsMega2560Container.WriteFunction _writeFunction;
        private readonly NotificarPuertoDelegate _urgencia;
        private readonly NotificarPuertoDelegate _notificar;

        #endregion

        #region Metodos Publicos
        public ProcessReadData(IotCentral iotConfigurations, 
            PortsMega2560Container.WriteFunction writeFunction, 
            NotificarPuertoDelegate urgencia = null, 
            NotificarPuertoDelegate notificar = null)
        {
            _iotConfigurations = iotConfigurations;
            _writeFunction = writeFunction;
            _urgencia = urgencia;
            _notificar = notificar;
        }

        public void ReadData(string pin, string value)
        {
            var puerto = this._iotConfigurations.GetPortByPin(pin);
            if (puerto != null)
            {
                puerto.Value = value;
                this.ProcessReactions(puerto);
            }

        }

        #endregion



        #region Metodos Privados

        private void ProcessReactions(Port puerto)
        {
            int valuePort = 0;
            int.TryParse(puerto.Value, out valuePort);
            if (puerto.ReActions != null)
            {
                foreach (var reAction in puerto.ReActions)
                {
                    ActuarSegunReactionType(reAction, puerto);
                }
            }
        }

        private void ActuarSegunReactionType(Reaction reAction, Port puerto)
        {
            switch (reAction.ReactionType)
            {
                case ReactionType.SiEsMayor:
                    VerificacionesAntesDeAplicarReaccion(reAction, puerto, new ProcessDataSiEsMayor());
                    break;
                case ReactionType.SiEsMenor:
                   VerificacionesAntesDeAplicarReaccion(reAction, puerto, new ProcessDataSiEsMenor());
                    break;
                case ReactionType.SiEsIgual:
                    VerificacionesAntesDeAplicarReaccion(reAction, puerto, new ProcessDataSiEsIgual());
                    break;
                case ReactionType.SiTieneUnaDiferenciaMenor:
                    VerificacionesAntesDeAplicarReaccion(reAction, puerto, new ProcessDataSiTieneUnaDiferenciaMenor());
                    break;
                case ReactionType.SiTieneUnaDiferenciaMayor:
                    VerificacionesAntesDeAplicarReaccion(reAction, puerto, new ProcessDataSiTieneUnaDiferenciaMayor());
                    break;
                case ReactionType.SiEstaEntre:
                    VerificacionesAntesDeAplicarReaccion(reAction, puerto, new ProcessDataSiEstaEntre());
                    break;
                case ReactionType.SiEstaApagado:
                    VerificacionesAntesDeAplicarReaccion(reAction, puerto, new ProcessDataSiEstaApagado());
                    break;
                case ReactionType.SiEstaPrendido:
                    VerificacionesAntesDeAplicarReaccion(reAction, puerto, new ProcessDataSiEstaPrendido());
                    break;
                default:
                    break;
            }
        }

        private void EjecutarAccion(Reaction reAction, Port puerto)
        {
            if (reAction.ActionExecute?.ActionExecuteType != null)
            {
                switch (reAction.ActionExecute.ActionExecuteType)
                {
                    case ActionExecuteTypeEnum.Apager:
                        if (reAction.ActionExecute != null)
                            ApagarPuerto(reAction.ActionExecute.ActionPort);
                        break;
                    case ActionExecuteTypeEnum.Notificar:
                        if (reAction.ActionExecute != null)
                            Notificar(puerto);
                        break;
                    case ActionExecuteTypeEnum.Prender:
                        if (reAction.ActionExecute != null)
                            PrenderPuerto(reAction.ActionExecute.ActionPort);
                        break;
                    case ActionExecuteTypeEnum.Urgencia:
                        if (reAction.ActionExecute != null)
                            Urgencia(puerto);
                        break;
                }
            }
        }
        #endregion

        #region Reacciones


        private void VerificacionesAntesDeAplicarReaccion(Reaction reAction, Port puerto, IProcessData processData)
        {
            if (reAction.CompararConPuerto)
            {
                if (reAction.Port != null)
                {
                    var port = this._iotConfigurations.GetPortByPin(reAction.Port.Pin);
                    try
                    {
                        if (processData.CompararConOtroPuerto(reAction, puerto, port))
                        {
                            if (reAction.ActionExecute != null)
                            {
                                EjecutarAccion(reAction, puerto);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //this._logger.Log(LogLevel.ERROR, "Error al parsear el valor de los puertos : " + ex.Message);
                    }

                }
            }
            else
            {
                if (processData.CompararConSigoMismo(reAction, puerto))
                {
                    if (reAction.ActionExecute != null)
                    {
                        EjecutarAccion(reAction, puerto);
                    }
                }
            }
        }

        #endregion

        #region Acciones
        private void Urgencia(Port puerto)
        {
            this._urgencia?.Invoke(puerto);
        }

        private void PrenderPuerto(Port port)
        {
            var command = WriteCommandClass.Generate(CommandTypeEnum.WRITE, port.Pin, "1");
            this._writeFunction(command);
        }

        private void Notificar(Port puerto)
        {
                this._notificar?.Invoke(puerto);
        }

        private void ApagarPuerto(Port port)
        {
            var command = WriteCommandClass.Generate(CommandTypeEnum.WRITE, port.Pin, "0");
            this._writeFunction(command);
        }
        #endregion

        public delegate void NotificarPuertoDelegate(Port port);
    }
}
