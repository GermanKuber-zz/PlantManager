using Core.Business;

namespace RaspberryCom.Process.ProcesatorData
{
    public interface IProcessData
    {
        bool CompararConOtroPuerto(Reaction reAction, Port puertoAComparar, Port puertoContraElQueSerComparado);
        bool CompararConSigoMismo(Reaction reAction,Port puerto);
    }
}