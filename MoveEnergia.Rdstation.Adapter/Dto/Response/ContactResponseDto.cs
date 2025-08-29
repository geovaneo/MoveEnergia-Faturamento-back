using System.Text.Json.Serialization;

namespace MoveEnergia.RdStation.Adapter.Dto.Response
{
   public class ContactResponseDto
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("phones")]
        public List<ContactPhoneResponseDto> phones { get; set; }
    }
}
