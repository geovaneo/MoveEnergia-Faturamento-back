namespace MoveEnergia.Billing.Core.Entity
{
    public class Telephone
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public int ContactId { get; set; }

        public virtual Contact Contact { get; set; }    
    }
}
