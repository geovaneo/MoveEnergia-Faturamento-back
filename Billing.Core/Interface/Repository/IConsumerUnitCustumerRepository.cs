using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Repository
{
    public interface IConsumerUnitCustumerRepository : IBaseRepository<ConsumerUnitCustumer>
    {
        Task<ConsumerUnitCustumer> GetByCustomerIdUCAsync(string UC, int customerId);
    }
}
