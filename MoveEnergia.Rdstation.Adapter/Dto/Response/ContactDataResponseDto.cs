using System.Text.Json.Serialization;

namespace MoveEnergia.RdStation.Adapter.Dto.Response
{
  public class ContactDataResponseDto
    {
        [JsonPropertyName("contacts")]
        public List<ContactResponseDto> contacts { get; set; }
    }
}
