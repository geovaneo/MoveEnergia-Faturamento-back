namespace MoveEnergia.Billing.Core.Entity
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
