using System;
using Core.Business;

namespace RaspberryCom.Process.ProcesatorData
{
    public class ProcessDataSiEstaApagado : IProcessData
    {
        public bool CompararConOtroPuerto(Reaction reAction, Port puertoAComparar, Port puertoContraElQueSerComparado)
        {
            try
            {
                var intValue = int.Parse(puertoAComparar.Value);
                if (intValue == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
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