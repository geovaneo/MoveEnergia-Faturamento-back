namespace MoveEnergia.RdStation.Adapter.Dto.Response
{
    public class DealsResponseDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public DealsUserResponseDto user { get; set; }
        public List<CustomFieldsResponseDto> deal_custom_fields { get; set; }
        public List<Contact> contacts { get; set; }
    }
}
