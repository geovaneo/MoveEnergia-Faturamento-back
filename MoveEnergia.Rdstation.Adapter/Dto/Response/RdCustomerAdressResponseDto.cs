namespace MoveEnergia.RdStation.Adapter.Dto.Response
{
    public class RdCustomerAdressResponseDto
    {
        public int Id { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public int CityId { get; set; }
        public int CustomerId { get; set; }
    }
}
