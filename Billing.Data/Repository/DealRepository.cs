using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class DealRepository : BaseRepository<Deals>, IDealRepository
    {
        public DealRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Deals>> GetByTitularidadeAsync(string titularidade)
        {

            var result = _context.Set<Deals>().AsNoTracking()
                                  .Where(x => x.Titularidade == titularidade);

            return result.ToList();
        }
    }
}
