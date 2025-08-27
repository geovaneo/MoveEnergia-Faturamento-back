namespace MoveEnergia.Billing.Core.Dto.Response
{
    public class CustomerUnitAdressReponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UC { get; set; }

        public List<AdressCustomerUnitResponseDto> Address { get; set; }

    }
}
