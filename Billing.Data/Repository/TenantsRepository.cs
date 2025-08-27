using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Data.Context;
using MoveEnergia.Billing.Core.Interface.Repository;

namespace MoveEnergia.Billing.Data.Repository
{
    public class TenantsRepository : BaseRepository<Tenants>, ITenantsRepository
    {
        public TenantsRepository(ApplicationDbContext context) : base(context) { }
    }
}
