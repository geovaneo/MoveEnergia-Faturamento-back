using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class LeituraFaturaPdfLogRepository : BaseRepository<LeituraFaturaPdfLog>, ILeituraFaturaPdfLogRepository
    {
        public LeituraFaturaPdfLogRepository(ApplicationDbContext context) : base(context) { }
        

    }
}
