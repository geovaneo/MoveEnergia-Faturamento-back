namespace MoveEnergia.Billing.Core.Dto.Response
{
    public class CurrentInvoiceResponseDto
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string BillingNumber { get; set; }
        public string IssuedDate { get; set; }
        public string InstallationNumber { get; set; }
        public string ClientNumber { get; set; }
        public string BillingMonth { get; set; }
        public string DueDate { get; set; }
        public decimal TotalValue { get; set; }
        public string invoicesStatus { get; set; }
        public string CompensatedEnergy { get; set; }  
        public string MonthSavings { get; set; }
        public List<string> TotalSavings { get; set; }
        public string UC { get; set; }
    }
}
