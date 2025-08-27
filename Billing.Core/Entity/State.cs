using System.Text.Json.Serialization;

namespace MoveEnergia.Billing.Core.Entity
{
    public class State
    {
        public byte Id { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public virtual List<ConsumerUnit>? ConsumerUnit { get; set; }
    }
}
