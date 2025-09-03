using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class RdFieldsIntegrationRepository : BaseRepository<RdFieldsIntegration>, IRdFieldsIntegrationRepository
    {
        public RdFieldsIntegrationRepository(ApplicationDbContext context) : base(context) { }
    }
}
