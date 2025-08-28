namespace MoveEnergia.RdStation.Adapter.Interface.Service
{
    public interface IRdstationIntegrationService
    {
        Task GetCellphoneNumbersAsync(string dealId, string token);
    }
}
