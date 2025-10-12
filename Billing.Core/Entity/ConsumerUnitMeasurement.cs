using System.ComponentModel.DataAnnotations.Schema;

namespace MoveEnergia.Billing.Core.Entity
{
    public class ConsumerUnitMeasurement
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal? Value { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
        public decimal? Ponta { get; set; }
        [Column(TypeName = "DECIMAL(18,3)")]
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
