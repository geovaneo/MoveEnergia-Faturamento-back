using MoveEnergia.RdStation.Adapter.Dto;

namespace MoveEnergia.RdStation.Adapter.Interface.Adapter
{
    public interface IRdstationIntegrationAdapter
    {
        Task<ReturnResponseDto> GetCellphoneNumbersAsync(string dealId);
    }
}
