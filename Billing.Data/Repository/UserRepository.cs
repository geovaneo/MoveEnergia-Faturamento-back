using Microsoft.EntityFrameworkCore;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;

namespace MoveEnergia.Billing.Data.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User> GetByUserNameAsync(string userName)
        {

            var result = _context.Set<User>().AsNoTracking()
                                  .Where(x => x.UserName == userName);

            return result.FirstOrDefault();
        }

    }
}
