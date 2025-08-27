using System.Text.Json.Serialization;

namespace MoveEnergia.Billing.Core.Entity
{
    public class Address
    {
        public int Id { get; set; } 
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public int CityId { get; set; }
        public int CustomerId { get; set; }
        public virtual City City { get; set; }

        [JsonIgnore]
        public virtual Customer Customer { get; set; }
      
    }
}
