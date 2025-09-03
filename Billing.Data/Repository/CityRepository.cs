using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        public CityRepository(ApplicationDbContext context) : base(context) { }

        public async Task<City> GetByNameAsync(string name)
        {

            var result = _context.Set<City>().AsNoTracking()
                                  .Where(x => x.Nome == name);

            return result.FirstOrDefault();
        }

    }
}
