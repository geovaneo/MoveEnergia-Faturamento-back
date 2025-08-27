using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Customer> GetByIdCustomerAsync(long idCustomer)
        {

            var result = _context.Set<Customer>().AsNoTracking()
                                  .Include(c => c.Addresses)
                                     .ThenInclude(a => a.City)
                                       .ThenInclude(c => c.UF)
                                         .ThenInclude(u => u.Country)
                                .Include(c => c.User);
            return result.FirstOrDefault();
        }
    }
}
