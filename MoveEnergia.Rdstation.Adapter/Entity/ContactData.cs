using System.Text.Json.Serialization;

namespace MoveEnergia.Rdstation.Adapter.Entity
{
  public class ContactData
    {
        [JsonPropertyName("contacts")]
        public List<Contact> contacts { get; set; }
    }
}
