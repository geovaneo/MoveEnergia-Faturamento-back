using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class DistributorRepository : BaseRepository<Distributor>, IDistributorRepository
    {
        public DistributorRepository(ApplicationDbContext context) : base(context) { }

        public async Task DeleteForPk(int Id) 
        { 
            await _context.Set<Distributor>().Where(d  => d.Id == Id).ExecuteDeleteAsync();
        }            
    }
}
