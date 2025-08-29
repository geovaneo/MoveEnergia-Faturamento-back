namespace MoveEnergia.Billing.Core.Dto.Request
{
    public class FetchUnidadesPageRequestDto
    {
        public int page { get; set; } =  0;
        public int limit { get; set; } = 200;
        public string next_page { get; set; } = string.Empty;
        public bool isStage { get; set; } = true;
    }
}
