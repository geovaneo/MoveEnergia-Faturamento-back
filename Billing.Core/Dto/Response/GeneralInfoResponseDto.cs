namespace MoveEnergia.Billing.Core.Dto.Response
{
   public  class GeneralInfoResponseDto
    {
        public long CompensatedEnergy { get; set; }
        public decimal MonthSavings {  get; set; }
        public decimal TotalSavings { get; set; }

    }
}
