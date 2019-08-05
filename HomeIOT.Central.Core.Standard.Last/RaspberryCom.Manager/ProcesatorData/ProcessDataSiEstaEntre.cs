using System;
using Core.Business;

namespace RaspberryCom.Process.ProcesatorData
{
    public class ProcessDataSiEstaEntre : IProcessData
    {
        public bool CompararConOtroPuerto(Reaction reAction, Port puertoAComparar, Port puertoContraElQueSerComparado)
        {
            //Se tiene que completar para que compare con 2 valores.
            return false;
        }

        public bool CompararConSigoMismo(Reaction reAction, Port puerto)
        {
            var portInt = int.Parse(puerto.Value);
            if (portInt < reAction.MargenSuperiorComparacion && portInt > reAction.MargenInferiorComparacion)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}