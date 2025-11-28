namespace MoveEnergia.Billing.Core.Entity
{
    public class City
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int UFId { get; set; }
        public virtual FederativeUnit? UF { get; set; }
        public virtual List<Address> Address { get; set; }
    }
}
