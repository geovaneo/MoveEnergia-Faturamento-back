using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Repository
{
    public interface IDetalhesFaturaCacheRepository : IBaseRepository<DetalhesFaturaCache>
    {
        Task<DetalhesFaturaCache> GetDetalhesFaturaCacheByReferenceMonth(string Uc, int referenceMonth);
        Task<List<DetalhesFaturaCache>> GetListDetalhesFaturaCacheByReferenceMonth(string Uc, int referenceMonth);
    }
}
