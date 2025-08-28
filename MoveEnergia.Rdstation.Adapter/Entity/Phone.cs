using System.Text.Json.Serialization;

namespace MoveEnergia.Rdstation.Adapter.Entity
{
    public class Phone
    {
        public string id { get; set; }

        [JsonPropertyName("type")]
        public string? type { get; set; }

        [JsonPropertyName("phone")]
        public string phone { get; set; }
    }
}
