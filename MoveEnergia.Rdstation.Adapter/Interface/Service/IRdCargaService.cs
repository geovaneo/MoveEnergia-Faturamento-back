using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.RdStation.Adapter.Dto.Response;

namespace MoveEnergia.RdStation.Adapter.Interface.Service
{
    public interface IRdCargaService
    {

        Task<ReturnResponseDto> GetPipelines();
        Task<ReturnResponseDto> CargaEnderecosAsync(int page = 0, int limit = 200, string next_page = "");

    }
}
