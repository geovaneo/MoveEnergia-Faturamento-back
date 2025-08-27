namespace MoveEnergia.Billing.Core.Dto.Response
{
    public class HomeInfoResponseDto
    {
        public string InvoicesStatus { get; set; }
        public CurrentInvoiceInfoResponseDto? CurrentInvoice { get; set; }
        public GeneralInfoResponseDto? GenaralInfo { get; set; }
    }
}
