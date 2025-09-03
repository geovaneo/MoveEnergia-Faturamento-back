using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.RdStation.Adapter.Dto.Response;

namespace MoveEnergia.RdStation.Adapter.Interface.Service
{
    public interface IRdStationIntegrationService
    {
        Task<ReturnResponseDto> GetCellphoneNumbersAsync(string dealId);
        Task<ReturnResponseDto> FetchUnidadesPageAsync(int page = 0, int limit = 200, string next_page = "");
        Task<ReturnResponseDto> FetchUnidadesFromRdStationAsync(string dealId, bool isStage, int page = 0, int limit = 1);
        Task<ReturnResponseDto> MappingDealToCustomer(Dictionary<string, string> fieldsDeal, DealsResponseDto dealsResponseDto);
        Task<ReturnResponseDto> SetCustomerSync(Customer customer, Address address, User user);
    }
}
