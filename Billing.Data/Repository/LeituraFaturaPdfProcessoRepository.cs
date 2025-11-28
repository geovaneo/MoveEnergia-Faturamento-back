using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class LeituraFaturaPdfProcessoRepository : BaseRepository<LeituraFaturaPdfProcesso>, ILeituraFaturaPdfProcessoRepository
    {
        public LeituraFaturaPdfProcessoRepository(ApplicationDbContext context) : base(context) { }
        

    }
}
