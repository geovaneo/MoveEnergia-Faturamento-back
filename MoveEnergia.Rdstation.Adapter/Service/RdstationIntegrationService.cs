using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoveEnergia.Rdstation.Adapter.Configuration;
using MoveEnergia.Rdstation.Adapter.Interface.Service;
using MoveEnergia.RdStation.Adapter.Dto;
using MoveEnergia.RdStation.Adapter.Interface.Service;

namespace MoveEnergia.Rdstation.Adapter.Service
{
    public class RdstationIntegrationService : IRdstationIntegrationService
    {
        private readonly ILogger<RdstationIntegrationService> _logger;
        private readonly IHttpService _httpService;
        private readonly RdStationConfiguration _rdStationConfiguration;

        public RdstationIntegrationService(ILogger<RdstationIntegrationService> logger,
                                           IHttpService httpService,
                                           IOptions<RdStationConfiguration> rdStationConfiguration
                              )
        {
            _logger = logger;
            _httpService = httpService;
            _rdStationConfiguration = rdStationConfiguration.Value;
        }
        public async Task<ReturnResponseDto> GetCellphoneNumbersAsync(string dealId)
        {
            string roteApi = $"{_rdStationConfiguration.UrlBase}/deals/{dealId}/contacts?token={_rdStationConfiguration.Token}&limit=200";

            var returnHttp = await _httpService.GetAsync<ContactDataResponseDto>(roteApi);

            var returnDto = new ReturnResponseDto()
            {
                Error = false,
                StatusCode = 200,
                Data = returnHttp,
            };

            return returnDto;
        }
    }
}
