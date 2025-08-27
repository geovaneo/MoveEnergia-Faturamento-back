namespace MoveEnergia.Billing.Core.Entity
{
    public class Country
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public virtual FederativeUnit? FederativeUnit { get; set; }  
    }
}
