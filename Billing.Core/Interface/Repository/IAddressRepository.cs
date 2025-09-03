using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Repository
{
    public interface IAddressRepository : IBaseRepository<Address>
    {
        Task<Address> GetByCepNumeroCustomerAsync(string cep, string numero, int customerId);
    }
}
