namespace MoveEnergia.RdStation.Adapter.Dto.Response
{
    public class RdCustomerResponseDto
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
        public string RazoSocial { get; set; }
        public string Code { get; set; }
        public byte TipoCustomer { get; set; }
        public int TenantId { get; set; }
        public byte Mercado { get; set; } = 0;
        public RdCustomerUserResponseDto? User { get; set; }
        public RdCustomerAdressResponseDto? Adress { get; set; }
        public string UC { get; set; }
    }
}
