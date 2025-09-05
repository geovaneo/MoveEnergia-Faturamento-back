namespace MoveEnergia.Billing.Core.Dto.Request
{
     public class SyncCustomerRequestDto
    {
        public bool isFullLoad { get; set; } = false;
        public int RetroactiveDays { get; set; } = 0;
        public string Funil { get; set; }
        public string DealId { get; set; }
    }
}
