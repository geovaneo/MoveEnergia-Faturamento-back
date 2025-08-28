namespace MoveEnergia.RdStation.Adapter.Dto
{
    public class ReturnResponseErrorDto
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorMessageDetail { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }

}
