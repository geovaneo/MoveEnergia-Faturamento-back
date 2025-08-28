using MoveEnergia.RdStation.Adapter.Dto;

namespace MoveEnergia.RdStation.Adapter.Interface.Service
{
    public interface IRdstationIntegrationService
    {
        Task<ReturnResponseDto> GetCellphoneNumbersAsync(string dealId);
    }
}
