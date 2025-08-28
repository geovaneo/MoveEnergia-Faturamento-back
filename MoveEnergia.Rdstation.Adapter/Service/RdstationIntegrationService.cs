using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoveEnergia.Rdstation.Adapter.Configuration;
using MoveEnergia.Rdstation.Adapter.Interface.Service;
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
        public async Task GetCellphoneNumbersAsync(string dealId, string token)
        {
            string url = $"{_rdStationConfiguration.UrlBase}/deals/{dealId}/contacts?token={token}&limit=200";
        }


    }
}
