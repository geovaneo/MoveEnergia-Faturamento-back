using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Core.Interface.Service;

namespace MoveEnergia.Billing.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ILogger<CustomerService> logger,
                               ICustomerRepository customerRepository)
        {
            _logger = logger;
            _customerRepository = customerRepository;
        }
        public async Task<Customer> GetByIdCustomerAsync(int idCustomer)
        {
            var result = await _customerRepository.GetByIdCustomerAsync(idCustomer);

            return result;
        }
    }
}
