using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.RdStation.Adapter.Dto.Response;

namespace MoveEnergia.RdStation.Adapter.Interface.Service
{
    public interface IRdStationIntegrationService
    {
        Task<RdReturnResponseDto> GetContactsAsync(string dealId);
        Task<RdReturnResponseDto> FetchUnidadesPageAsync(int page = 0, int limit = 200, string next_page = "");
        Task<RdReturnResponseDto> FetchUnidadesFromRdStationAsync(string dealId, bool isStage, int page = 0, int limit = 1);
        Task<RdReturnResponseDto> MappingDealToCustomer(Deals deals);
        Task<RdReturnResponseDto> SetCustomerSync(Customer customer, Address address, User user, ConsumerUnitCustumer consumerUnitCostumer);
        Task<List<Deals>> GetByTitularidadeAsync(string titularidade);
        Task<RdReturnResponseDto> MappingDealToCustomerApi(Dictionary<string, string> fieldsDeal, DealsResponseDto dealsResponseDto);
        Task<RdReturnResponseDto> GetDealToIdAsync(string dealId);
        Task<List<Deals>> GetByUCValidateAsync(List<string> listUC);
    }
}
