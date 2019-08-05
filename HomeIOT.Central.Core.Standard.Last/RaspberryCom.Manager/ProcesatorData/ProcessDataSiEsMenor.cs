using Core.Business;

namespace RaspberryCom.Process.ProcesatorData
{
    public class ProcessDataSiEsMenor : IProcessData
    {
        public bool CompararConOtroPuerto(Reaction reAction, Port puertoAComparar, Port puertoContraElQueSerComparado)
        {
            if (int.Parse(puertoAComparar.Value) <int.Parse(puertoContraElQueSerComparado.Value))
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
            var puertoValue = int.Parse(puerto.Value);
            if (puertoValue < reAction.ValorExactoComparacion)
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