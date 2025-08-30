using MoveEnergia.RdStation.Adapter.Dto.Response;

namespace MoveEnergia.Billing.Core.Dto.Response
{
    public class RdStationUnidadeConsumidoraResponseDto
    {
        public int total { get; set; }
        public string next_page { get; set; }
        public bool has_more { get; set; }
        public List<DealsResponseDto> deals { get; set; }
    }
}
