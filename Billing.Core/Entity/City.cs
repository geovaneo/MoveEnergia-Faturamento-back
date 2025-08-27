using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoveEnergia.Billing.Core.Entity
{
    public class City
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int UFId { get; set; }
        public virtual FederativeUnit? UF { get; set; }

        [JsonIgnore]
        public virtual Address? Address { get; set; }    
    }
}
