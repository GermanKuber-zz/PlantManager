namespace Core.Business
{
    public class Reaction
    {
     public int Id { get; set; }
        //[ForeignKey("Port")]
        //public int PortId { get; set; }
        public virtual Port Port { get; set; }

        public int? PortParaCompararId { get; set; }
        public virtual Port PortParaComparar { get; set; }
        public bool CompararConPuerto { get; set; }
        public int MargenInferiorComparacion { get; set; }
        public int MargenSuperiorComparacion { get; set; }
        public int ValorExactoComparacion { get; set; }
        public ReactionType ReactionType { get; set; }
        public int ActionExecuteID { get; set; }
        public virtual ActionExecute ActionExecute { get; set; }
    }

    public class ActionExecute
    {
        public int Id { get; set; }

        //public int ReactionId { get; set; }
        //[ForeignKey("ReactionId")]
        //public virtual Reaction Reaction { get; set; }

        public int ActionPortId { get; set; }
        public virtual Port ActionPort { get; set; }
        public ActionExecuteTypeEnum ActionExecuteType { get; set; }
    }

    public enum ActionExecuteTypeEnum
    {
        Prender,
        Apager,
        Notificar,
        Urgencia

    }
    public enum ReactionType
    {
        SiEsMayor,
        SiEsMenor,
        SiEsIgual,
        SiTieneUnaDiferenciaMenor,
        SiTieneUnaDiferenciaMayor,
        SiEstaEntre,
        SiEstaApagado,
        SiEstaPrendido
    }
}