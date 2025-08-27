namespace MoveEnergia.Billing.Core.Entity
{
    public class ConsumerUnitMeasurement
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal? Value { get; set; }
        public decimal? Ponta { get; set; }        
        public decimal? ForaPonta { get; set; }
        public int ConsumerUnitId { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }

        public virtual ConsumerUnit ConsumerUnit { get; set; }
    }
}
