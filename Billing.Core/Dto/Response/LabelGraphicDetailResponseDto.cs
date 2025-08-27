namespace MoveEnergia.Billing.Core.Dto.Response
{
    public class LabelGraphicDetailResponseDto
    {
        public string month { get; set; }
        public decimal consumption {  get; set; }
        public decimal increasedConsumption { get; set; }
        public string period { get; set; }
        public decimal totalInvoice { get; set; }
        public decimal monthSavings { get; set; }
        public List<ConsumerUnitResponseDto>? consumerUnit { get; set; }
    }
}
