using AutoMapper;
using Microsoft.Extensions.Logging;
using MoveEnergia.RdStation.Adapter.Dto.Response;
using MoveEnergia.RdStation.Adapter.Interface.Adapter;
using MoveEnergia.RdStation.Adapter.Interface.Service;

namespace MoveEnergia.RdStation.Adapter
{
    public class RdstationIntegrationAdapter : IRdstationIntegrationAdapter
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RdstationIntegrationAdapter> _logger;
        private readonly IRdstationIntegrationService _iRdstationIntegrationService;
        public RdstationIntegrationAdapter(ILogger<RdstationIntegrationAdapter> logger,
                                           IMapper mapper,
                                           IRdstationIntegrationService iRdstationIntegrationService)
        {
            _logger = logger;
            _mapper = mapper;
            _iRdstationIntegrationService = iRdstationIntegrationService;
        }

        public async Task<ReturnResponseDto> GetCellphoneNumbersAsync(string dealId)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var returnDto = await _iRdstationIntegrationService.GetCellphoneNumbersAsync(dealId);

                if (returnDto.Data != null)
                {
                    returnResponseDto = returnDto;
                }
                else
                {
                    returnResponseDto.Error = true;
                    returnResponseDto.StatusCode = 404;
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
        public async Task<ReturnResponseDto> FetchUnidadesPageAsync(int page = 0, int limit = 200, string next_page = "")
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();
            
            try
            {
                var returnDto = await _iRdstationIntegrationService.FetchUnidadesPageAsync(page = 0, limit = 200,next_page = "");

                if (returnDto.Data != null)
                {
                    returnResponseDto = returnDto;
                }
                else
                {
                    returnResponseDto.Error = true;
                    returnResponseDto.StatusCode = 404;
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

        public async Task<ReturnResponseDto> FetchUnidadesFromRdStationAsync(string dealId, bool isStage, int page = 0, int limit = 1)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var returnDto = await _iRdstationIntegrationService.FetchUnidadesFromRdStationAsync(dealId, isStage, page, limit);

                if (returnDto.Data != null)
                {
                    returnResponseDto = returnDto;
                }
                else
                {
                    returnResponseDto.Error = true;
                    returnResponseDto.StatusCode = 404;
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
    }
}
