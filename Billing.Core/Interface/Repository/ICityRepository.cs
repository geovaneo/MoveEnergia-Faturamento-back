using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Repository
{
    public interface ICityRepository : IBaseRepository<City>
    {
        Task<City> GetByNameAsync(string Name);
    }
}
