using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Repository
{
    public interface IDistributorRepository : IBaseRepository<Distributor>
    {
        Task DeleteForPk(int Id);
    }
}
