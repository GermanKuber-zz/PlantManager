using System;
using Core.Business;

namespace RaspberryCom.Process.ProcesatorData
{
    public class ProcessDataSiTieneUnaDiferenciaMenor : IProcessData
    {
        public bool CompararConOtroPuerto(Reaction reAction, Port puertoAComparar, Port puertoContraElQueSerComparado)
        {
            var portInt = int.Parse(puertoAComparar.Value);
            var portComparadoInt = int.Parse(puertoContraElQueSerComparado.Value);

            var diff = Math.Abs(portInt - portComparadoInt);


            if (diff < reAction.ValorExactoComparacion)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CompararConSigoMismo(Reaction reAction, Port puerto)
        {
            return false;
        }
    }
}