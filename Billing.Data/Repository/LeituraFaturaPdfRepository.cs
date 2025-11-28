using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class LeituraFaturaPdfRepository : BaseRepository<LeituraFaturaPdf>, ILeituraFaturaPdfRepository
    {
        public LeituraFaturaPdfRepository(ApplicationDbContext context) : base(context) { }
        

    }
}
