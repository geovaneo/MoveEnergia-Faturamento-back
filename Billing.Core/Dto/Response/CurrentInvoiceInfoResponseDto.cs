namespace MoveEnergia.Billing.Core.Dto.Response
{
    public class CurrentInvoiceInfoResponseDto
    {
        public int Id { get; set; }
        public string BillingNumber { get; set; }
        public string IssuedDate { get; set; }
        public string InstallationNumber { get; set; }
        public string ClientNumber { get; set; }
        public string BillingMonth { get; set; }
        public string DueDate { get; set; }
        public decimal TotalValue { get; set; }
        public ChargedCustomerInfoResponseDto? ChargedCustomer {  get; set; }
    }

}
