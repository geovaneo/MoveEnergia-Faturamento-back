using FluentValidation;
using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Core.Interface.Service;

namespace MoveEnergia.Billing.Service
{
    public class DistributorService : IDistributorService
    {
        private readonly ILogger<DistributorService> _logger;
        private readonly IValidator<Distributor> _validator;
        private readonly IDistributorRepository _distributorRepository;
        public DistributorService(ILogger<DistributorService> logger,
                                  IValidator<Distributor> validator,
                                  IDistributorRepository distributorRepository)
        {
            _logger = logger;
            _validator = validator;
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
        public async Task<ReturnResponseDto> CreateAsync(Distributor distributor) 
        {
            var validation = await ValidationExtensions.ProcessValidateAsync(distributor, _validator);

            if (validation != null && validation.Count > 0)
            {
                return new ReturnResponseDto()
                {
                    Error = true,
                    StatusCode = 400,
                    Data = null,
                    Erros = validation
                };
            }

            var registro = await _distributorRepository.CreateAsync(distributor);
            await _distributorRepository.SaveAsync();

            return new ReturnResponseDto()
            {
                Error = false,
                StatusCode = 200,
                Data = registro,
                Erros = null
            };
        }
        public async Task<ReturnResponseDto> UpdateAsync(Distributor distributor)
        {
            var validation = await ValidationExtensions.ProcessValidateAsync(distributor, _validator);

            if (validation != null && validation.Count > 0)
            {
                return new ReturnResponseDto()
                {
                    Error = true,
                    StatusCode = 400,
                    Data = null,
                    Erros = validation
                };
            }

            var registro = await _distributorRepository.UpdateAsync(distributor);
            await _distributorRepository.SaveAsync();

            return new ReturnResponseDto()
            {
                Error = false,
                StatusCode = 200,
                Data = registro,
                Erros = null
            };
        }
        public async Task DeleteAsync(int Id)
        {
            await _distributorRepository.DeleteForPk(Id);
            await _distributorRepository.SaveAsync();

        }
   
            
    }
}
