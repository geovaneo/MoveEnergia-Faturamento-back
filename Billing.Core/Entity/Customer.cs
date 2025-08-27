using System.Text.Json.Serialization;

namespace MoveEnergia.Billing.Core.Entity
{
    public class Customer
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string RazoSocial { get; set; }
        public string Code { get; set; }
        public DateTime CreationTime { get; set; }
        public byte TipoCustomer { get; set; }
        public int TenantId { get; set; }
        public byte Mercado { get; set; }
        public long? CreatorUserId { get; set; }
        public long? DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
        public long? LastModifierUserId { get; set; }
        /// <summary>
        /// Gets or sets the path key.
        /// </summary>
        /// <value>
        /// The path key.
        /// </value>
        public Guid? PathKey { get; set; }

        public virtual User? User { get; set; }
        public virtual List<Contact>? Contacts { get; set; }
        public virtual List<Address>? Addresses { get; set; }
        public virtual List<ConsumerTipo>? ConsumerTipos { get; set; }

        [JsonIgnore]
        public virtual List<ConsumerUnit>? ConsumerUnit { get; set; }
    }
}
