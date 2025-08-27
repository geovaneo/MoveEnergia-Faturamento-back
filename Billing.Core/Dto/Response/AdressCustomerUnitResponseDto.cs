namespace MoveEnergia.Billing.Core.Dto.Response
{
    public class AdressCustomerUnitResponseDto
    {
        public int Id { get; set; } 
        public string ZipCode { get; set; }
        public string Street   { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string FederativeUnit { get; set; }
        public string Country { get; set; }
        public string AddressStreet { get; set; }
    }
}
