using AutoMapper;
using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Adapter;
using MoveEnergia.Billing.Core.Interface.Service;

namespace MoveEnergia.Billing.Adapter
{
    public class DistributorAdapter : IDistributorAdapter
    {
        private readonly IMapper _mapper;
        private readonly ILogger<DistributorAdapter> _logger;
        private readonly IDistributorService _distributorService;
        public DistributorAdapter(ILogger<DistributorAdapter> logger,
                                   IMapper mapper,
                                   IDistributorService distributorService)
        {
            _logger = logger;
            _mapper = mapper;
            _distributorService = distributorService;
        }

        public async Task<ReturnResponseDto> GetByIdAsync(int Id)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var result = await _distributorService.GetByIdAsync(Id);

                if (result != null) 
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 200;
                    returnResponseDto.Data = result; 
                }
                else
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 404;
                    returnResponseDto.Data = null;
                }
            }
            catch (Exception ex)
            {
                returnResponseDto.Error = true;
                returnResponseDto.StatusCode = 500;
                returnResponseDto.Data = null;
                returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
            }

            return returnResponseDto;
        }
        public async Task<ReturnResponseDto> GetAllAsync()
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var result = await _distributorService.GetAllAsync();

                if (result != null)
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 200;
                    returnResponseDto.Data = result;
                }
                else
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 404;
                    returnResponseDto.Data = null;
                }
            }
            catch (Exception ex)
            {
                returnResponseDto.Error = true;
                returnResponseDto.StatusCode = 500;
                returnResponseDto.Data = null;
                returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
            }

            return returnResponseDto;
        }
        public async Task<ReturnResponseDto> CreateAsync(DistributorRequestDto request)
        {

            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var distributor = _mapper.Map<Distributor>(request);

                var result = await _distributorService.CreateAsync(distributor);

                if (result != null)
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 200;
                    returnResponseDto.Data = result;
                }
                else
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 404;
                    returnResponseDto.Data = null;
                }
            }
            catch (Exception ex)
            {
                returnResponseDto.Error = true;
                returnResponseDto.StatusCode = 500;
                returnResponseDto.Data = null;
                returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
            }

            return returnResponseDto;
        }
        public async Task<ReturnResponseDto> UpDateAsync(DistributorRequestDto request)
        {

            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var distributor = _mapper.Map<Distributor>(request);

                var result = await _distributorService.UpdateAsync(distributor);

                if (result != null)
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 200;
                    returnResponseDto.Data = result;
                }
                else
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 404;
                    returnResponseDto.Data = null;
                }
            }
            catch (Exception ex)
            {
                returnResponseDto.Error = true;
                returnResponseDto.StatusCode = 500;
                returnResponseDto.Data = null;
                returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
            }

            return returnResponseDto;
        }
        public async Task<ReturnResponseDto> DeleteAsync(int id)
        {

            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                //var distributor = _mapper.Map<Distributor>(request);

                await _distributorService.DeleteAsync(id);

                //if (result != null)
                //{
                //    returnResponseDto.Error = false;
                //    returnResponseDto.StatusCode = 200;
                //    returnResponseDto.Data = result;
                //}
                //else
                //{
                //    returnResponseDto.Error = false;
                //    returnResponseDto.StatusCode = 404;
                //    returnResponseDto.Data = null;
                //}
            }
            catch (Exception ex)
            {
                returnResponseDto.Error = true;
                returnResponseDto.StatusCode = 500;
                returnResponseDto.Data = null;
                returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
            }

            return returnResponseDto;
        }
    }
}
