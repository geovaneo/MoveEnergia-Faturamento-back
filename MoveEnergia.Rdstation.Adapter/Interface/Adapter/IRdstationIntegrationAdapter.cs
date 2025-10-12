using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.RdStation.Adapter.Dto.Response;

namespace MoveEnergia.RdStation.Adapter.Interface.Adapter
{
    public interface IRdstationIntegrationAdapter
    {
        Task<RdReturnResponseDto> GetContactsAsync(string dealId);
        Task<RdReturnResponseDto> FetchUnidadesPageAsync(int page = 0, int limit = 200, string next_page = "");
        Task<RdReturnResponseDto> FetchUnidadesFromRdStationAsync(string dealId, bool isStage, int page = 0, int limit = 1);
        Task<RdReturnResponseDto> ProcessIntegrationCustomerAsync(ProcessIntegrationCustomerRequestDto requestDto);
        Task<RdReturnResponseDto> SyncCustomerAsync(SyncCustomerRequestDto requestDto);
        Task<RdReturnResponseDto> SyncCustomerListUCAsync(string listUCs);
    }
}
