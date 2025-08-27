using System.Text.Json.Serialization;

namespace MoveEnergia.Billing.Core.Entity
{
    public class FederativeUnit
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public int CountryId { get; set; }

        [JsonIgnore]
        public virtual City? City { get; set; }
        public virtual Country? Country { get; set; } 

    }
}
