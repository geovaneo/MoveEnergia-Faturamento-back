using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Repository
{
    public interface IDealRepository : IBaseRepository<Deals>
    {
        Task<List<Deals>> GetByTitularidadeAsync(string titularidade);
        Task<List<Deals>> GetByUCValidateAsync(List<string> listUC);
    }
}
