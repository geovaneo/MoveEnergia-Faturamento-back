using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class ConsumerUnitMeasurementRepository : BaseRepository<ConsumerUnitMeasurement>, IConsumerUnitMeasurementRepository
    {
        public ConsumerUnitMeasurementRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<ConsumerUnitMeasurement>> GetConsumerUnitMeasurementReferenceDateAsync(DateTime referenceDate)
        {

            var result = await _context.Set<ConsumerUnitMeasurement>().AsNoTracking()
                                       .Include(c => c.ConsumerUnit)
                                          .ThenInclude(c => c.Customer)
                                            .ThenInclude(c => c.Addresses)
                                                .ThenInclude(a => a.City)
                                                    .ThenInclude(c => c.UF)
                                                        .ThenInclude(u => u.Country)
                                       .Include(c => c.ConsumerUnit)
                                            .ThenInclude(c => c.State)
                                       .Where(c => c.Date.Month == referenceDate.Month && 
                                                   c.Date.Year == referenceDate.Year).ToListAsync();
            return result;
        }
    }
}
