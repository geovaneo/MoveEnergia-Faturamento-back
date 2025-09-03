using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class AddressRepository : BaseRepository<Address>, IAddressRepository
    {
        public AddressRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Address> GetByCepNumeroCustomerAsync(string cep, string numero, int customerId)
        {

            var result = _context.Set<Address>().AsNoTracking()
                                  .Where(x => x.CEP == cep && 
                                              x.Numero == numero && 
                                              x.CustomerId == customerId);

            return result.FirstOrDefault();
        }
    }
}
