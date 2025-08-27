using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Core.Interface.Service;

namespace MoveEnergia.Billing.Service
{
    public class DistributorService : IDistributorService
    {
        private readonly ILogger<DistributorService> _logger;
        private readonly IDistributorRepository _distributorRepository;
        public DistributorService(ILogger<DistributorService> logger,
                                  IDistributorRepository distributorRepository)
        {
            _logger = logger;
            _distributorRepository = distributorRepository;
        }
        public async Task<Distributor> GetByIdAsync(int Id)
        {
            var registro = await _distributorRepository.Get(Id);
            return registro;
        }
        public async Task<List<Distributor>> GetAllAsync()
        {
            var registro = await _distributorRepository.GetAll();
            return registro.ToList();
        }
        public async Task<Distributor> CreateAsync(Distributor distributor) 
        {
            var registro = await _distributorRepository.CreateAsync(distributor);
            await _distributorRepository.SaveAsync();

            return registro;
        }
        public async Task<Distributor> UpdateAsync(Distributor distributor)
        {
            var registro = await _distributorRepository.UpdateAsync(distributor);
            await _distributorRepository.SaveAsync();

            return registro;
        }

        public async Task DeleteAsync(int Id)
        {
            await _distributorRepository.DeleteForPk(Id);
            await _distributorRepository.SaveAsync();

        }
        
            
    }
}
