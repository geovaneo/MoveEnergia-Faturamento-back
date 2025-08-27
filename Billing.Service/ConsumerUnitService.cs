using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Core.Interface.Service;

namespace MoveEnergia.Billing.Service
{
    public class ConsumerUnitService : IConsumerUnitService
    {
        private readonly ILogger<ConsumerUnitService> _logger;
        private readonly IConsumerUnitRepository _consumerUnitRepository;
        public ConsumerUnitService(ILogger<ConsumerUnitService> logger,
                                   IConsumerUnitRepository consumerUnitRepository)
        {
            _logger = logger;
            _consumerUnitRepository = consumerUnitRepository;
        }

        public async Task<List<ConsumerUnit>> GetByIdUserAsync(long idUser)
        {
           var result = await _consumerUnitRepository.GetByIdUserAsync(idUser); 

           return result;
        }

        public async Task<List<ConsumerUnit>> GetByAdressIdUserAsync(long idUser)
        {
           var result = await _consumerUnitRepository.GetByIdUserAsync(idUser);

          return result;
        }
    }
}
