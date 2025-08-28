using System.Text.Json.Serialization;

namespace MoveEnergia.Rdstation.Adapter.Entity
{
   public class Contact
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("phones")]
        public List<Phone> phones { get; set; }
    }
}
