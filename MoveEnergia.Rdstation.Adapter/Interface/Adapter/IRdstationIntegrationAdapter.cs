using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.RdStation.Adapter.Dto.Response;

namespace MoveEnergia.RdStation.Adapter.Interface.Adapter
{
    public interface IRdstationIntegrationAdapter
    {
        Task<ReturnResponseDto> GetContactsAsync(string dealId);
        Task<ReturnResponseDto> FetchUnidadesPageAsync(int page = 0, int limit = 200, string next_page = "");
        Task<ReturnResponseDto> FetchUnidadesFromRdStationAsync(string dealId, bool isStage, int page = 0, int limit = 1);
        Task<ReturnResponseDto> ProcessIntegrationCustomerAsync(ProcessIntegrationCustomerRequestDto requestDto);
        Task<ReturnResponseDto> SyncCustomerAsync(SyncCustomerRequestDto requestDto);
        Task<ReturnResponseDto> SyncCustomerListUCAsync(string listUCs);
    }
}
