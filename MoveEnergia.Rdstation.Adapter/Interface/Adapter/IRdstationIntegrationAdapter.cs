namespace MoveEnergia.RdStation.Adapter.Interface.Adapter
{
    public interface IRdstationIntegrationAdapter
    {
        Task GetCellphoneNumbersAsync(string dealId, string token);
    }
}
