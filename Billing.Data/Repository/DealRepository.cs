using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class DealRepository : BaseRepository<Deals>, IDealRepository
    {
        public DealRepository(ApplicationDbContext context) : base(context) { }

    }
}
