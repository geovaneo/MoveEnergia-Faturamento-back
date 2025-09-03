using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Repository
{
    public interface ICustomerRepository : IBaseRepository<Customer>
    {
        Task<Customer> GetByIdCustomerAsync(long idCustomer);
        Task<Customer> GetByCodeAsync(string code);
    }
}
