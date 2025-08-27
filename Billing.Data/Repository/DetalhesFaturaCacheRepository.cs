using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class DetalhesFaturaCacheRepository : BaseRepository<DetalhesFaturaCache>, IDetalhesFaturaCacheRepository
    {
        public DetalhesFaturaCacheRepository(ApplicationDbContext context) : base(context) { }

        public async Task<DetalhesFaturaCache> GetDetalhesFaturaCacheByReferenceMonth(string Uc, int referenceMonth)
        {

            var result = await _context.Set<DetalhesFaturaCache>().AsNoTracking()
                           .OrderByDescending(c => c.MesReferencia)
                           .Where(c => c.Uc == Uc && c.MesReferencia == referenceMonth)
                           .FirstOrDefaultAsync();
            return result;

        }
        public async Task<List<DetalhesFaturaCache>> GetListDetalhesFaturaCacheByReferenceMonth(string Uc, int referenceMonth)
        {

            var result = await _context.Set<DetalhesFaturaCache>().AsNoTracking()
                           .OrderByDescending(c => c.MesReferencia)
                           .Where(c => c.Uc == Uc && c.MesReferencia == referenceMonth)
                           .ToListAsync();
            return result;

        }
    }
}
