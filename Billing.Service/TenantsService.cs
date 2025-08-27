using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Core.Interface.Service;

namespace Service
{
    public class TenantsService : ITenantsService
    {
        private readonly ILogger<TenantsService> _logger;
        private readonly ITenantsRepository _tenantesRepository;
        public TenantsService(ILogger<TenantsService> logger,
                              ITenantsRepository tenantsRepository)
        {
            _logger = logger;
            _tenantesRepository = tenantsRepository;
        }

        public async Task<ReturnResponseDto> GetByIdAsync(int id)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();

            try
            {
                var result = await _tenantesRepository.Get(id);
                if (result != null)
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 200;
                    returnResponseDto.Data = result; //JsonConvert.SerializeObject(result);
                }
                else 
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 400;
                    returnResponseDto.Data = null;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return returnResponseDto;
        }
    }
}
