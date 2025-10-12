namespace MoveEnergia.RdStation.Adapter.Dto.Response
{
    public class RdReturnResponseDto
    {
        public bool Error { get; set; }
        public int StatusCode { get; set; }
        public object? Data { get; set; }
        public List<ReturnResponseErrorDto>? Erros { get; set; } = new List<ReturnResponseErrorDto>();
    }
}
