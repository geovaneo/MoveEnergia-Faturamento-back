namespace MoveEnergia.RdStation.Adapter.Dto.Response
{
   public class Contact
    {
        public string name { get; set; }
        public string? title { get; set; }
        public List<EmailDataResponseDto> emails { get; set; }
        public List<TelefoneDataResponseDto> phones { get; set; }
    }
}
