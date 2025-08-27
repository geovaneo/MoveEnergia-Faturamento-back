namespace MoveEnergia.Billing.Core.Entity
{
    public class ConsumerTipo
    {
        public int Id { get; set; }
        //public Tipo Tipo { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
