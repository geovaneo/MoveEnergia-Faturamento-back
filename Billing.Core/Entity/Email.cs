namespace MoveEnergia.Billing.Core.Entity
{
    public class Email
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int ContactId { get; set; }
        public virtual Contact Contact { get; set; }    
    }
}
