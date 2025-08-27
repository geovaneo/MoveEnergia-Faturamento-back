namespace MoveEnergia.Billing.Core.Entity
{
    public class Contact
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string Nome { get; set; }
        public int PositionId { get; set; }
        public int CustomerId { get; set; }

        public virtual Position Position { get; set; }
        public virtual List<Email> Emails { get; set; }
        public virtual List<Telephone> Telephones { get; set; }
    }
}
