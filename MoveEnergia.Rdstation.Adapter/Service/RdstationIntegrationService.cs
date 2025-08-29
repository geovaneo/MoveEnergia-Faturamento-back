using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Rdstation.Adapter.Configuration;
using MoveEnergia.Rdstation.Adapter.Interface.Service;
using MoveEnergia.RdStation.Adapter.Dto.Response;
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

        public async Task<ReturnResponseDto> FetchUnidadesPageAsync(int page = 0, int limit = 200, string next_page = "")
        {

            var roteApi = $"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&page={page}&limit={limit}";

            if (!string.IsNullOrEmpty(next_page))
            {
                roteApi = $"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&next_page={next_page}&limit={limit}";
            }

            var returnHttp = await _httpService.GetAsync<RdStationUnidadeConsumidoraResponseDto>(roteApi);

            var returnDto = new ReturnResponseDto()
            {
                Error = false,
                StatusCode = 200,
                Data = returnHttp,
            };

            return returnDto;
        }

        public async Task<ReturnResponseDto> FetchUnidadesFromRdStationAsync(string dealId, bool isStage, int page = 0, int limit = 1)
        {
            string roteApi = "";

            if (isStage)
            {
                roteApi = $"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&&page={page}&deal_stage_id={dealId}&limit={limit}";
            }
            else
            {
                roteApi = $"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&&page={page}&deal_id={dealId}&limit={limit}";
            }

            var returnHttp = await _httpService.GetAsync<RdStationUnidadeConsumidoraResponseDto>(roteApi);

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
