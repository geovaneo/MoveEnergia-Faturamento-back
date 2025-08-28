using System.Text.Json.Serialization;

namespace MoveEnergia.RdStation.Adapter.Dto
{
    public class ContactPhoneResponseDto
    {
        public string id { get; set; }

        [JsonPropertyName("type")]
        public string? type { get; set; }

        [JsonPropertyName("phone")]
        public string phone { get; set; }
    }
}
