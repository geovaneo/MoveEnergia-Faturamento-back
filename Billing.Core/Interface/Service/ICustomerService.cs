using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Service
{
    public interface ICustomerService
    {
        Task<Customer> GetByIdCustomerAsync(int idCustomer);
    }
}
