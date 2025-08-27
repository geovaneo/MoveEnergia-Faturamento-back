using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Service
{
    public interface IConsumerUnitService
    {
        Task<List<ConsumerUnit>> GetByIdUserAsync(long idUser);
        Task<List<ConsumerUnit>> GetByAdressIdUserAsync(long idUser);
    }
}
