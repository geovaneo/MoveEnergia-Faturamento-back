

using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class ConsumerUnitCustumerRepository : BaseRepository<ConsumerUnitCustumer>, IConsumerUnitCustumerRepository
    {
        public ConsumerUnitCustumerRepository(ApplicationDbContext context) : base(context) { }

        public async Task<ConsumerUnitCustumer> GetByCustomerIdUCAsync(string UC, int customerId)
        {

            var result = _context.Set<ConsumerUnitCustumer>().AsNoTracking()
                                  .Where(x => x.UC == UC &&
                                              x.CustomerId == customerId);

            return result.FirstOrDefault();
        }
    }
}
