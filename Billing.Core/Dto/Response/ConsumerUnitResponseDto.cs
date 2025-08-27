namespace MoveEnergia.Billing.Core.Dto.Response
{
public class ConsumerUnitResponseDto
    {
        public int id { get; set; }
        public string name {  get; set; }
        public decimal consumption { get; set; }
        public decimal totalInvoice { get; set; }
        public decimal savings { get; set; }
    }
}
