using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Enum;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Helper;
using MoveEnergia.Rdstation.Adapter.Configuration;
using MoveEnergia.Rdstation.Adapter.Interface.Service;
using MoveEnergia.RdStation.Adapter.Configuration;
using MoveEnergia.RdStation.Adapter.Dto.Response;
using MoveEnergia.RdStation.Adapter.Interface.Service;
using Serilog;

namespace MoveEnergia.Rdstation.Adapter.Service
{
    public class RdCargaService : IRdCargaService
    {
        private readonly ILogger<RdStationIntegrationService> _logger;
        private readonly IHttpService _httpService;
        private readonly RdStationConfiguration _rdStationConfiguration;
        private readonly RdStationIntegrationCustomer _rdStationIntegrationCustomer;
        private readonly IRdFieldsIntegrationRepository _iRdFieldsIntegrationRepository;
        private readonly ICityRepository _iCityRepository;
        private readonly IAddressRepository _iAddressRepository;
        private readonly ICustomerRepository _iCustomerRepository;
        private readonly IUserRepository _iUserRepository;
        private readonly IConsumerUnitCustumerRepository _iConsumerUnitCostumerRepository;
        private readonly IDealRepository _iDealRepository;
        private readonly IConsumerUnitRepository _iconsumerUnitRepository;

        public RdCargaService(ILogger<RdStationIntegrationService> logger,
                                           IHttpService httpService,
                                           IOptions<RdStationConfiguration> rdStationConfiguration,
                                           IOptions<RdStationIntegrationCustomer> rdStationIntegrationCustomer,
                                           IRdFieldsIntegrationRepository rdFieldsIntegrationRepository,
                                           ICityRepository iCityRepository,
                                           IAddressRepository iAddressRepository,
                                           ICustomerRepository iCustomerRepository,
                                           IUserRepository iUserRepository,
                                           IConsumerUnitCustumerRepository iConsumerUnitCostumerRepository,
                                           IDealRepository iDealRepository,
                                           IConsumerUnitRepository consumerUnitRepository
                                          )
        {
            _logger = logger;
            _httpService = httpService;
            _rdStationConfiguration = rdStationConfiguration.Value;
            _rdStationIntegrationCustomer = rdStationIntegrationCustomer.Value;
            _iRdFieldsIntegrationRepository = rdFieldsIntegrationRepository;
            _iCityRepository = iCityRepository;
            _iAddressRepository = iAddressRepository;
            _iCustomerRepository = iCustomerRepository;
            _iUserRepository = iUserRepository;
            _iConsumerUnitCostumerRepository = iConsumerUnitCostumerRepository;
            _iDealRepository = iDealRepository;
            _iconsumerUnitRepository = consumerUnitRepository;
        }

        public async Task<ReturnResponseDto> GetPipelines()
        {

            var roteApi = $"{_rdStationConfiguration.UrlBase}/deal_pipelines?token={_rdStationConfiguration.Token}";

            var returnHttp = await _httpService.GetAsyncToJsonArray(roteApi);

            Log.Debug(returnHttp.ToString());


            var returnDto = new ReturnResponseDto()
            {
                Error = false,
                StatusCode = 200,
                Data = returnHttp,
            };

            return returnDto;
        }

        public async Task<ReturnResponseDto> CargaEnderecosAsync(int page = 0, int limit = 200, string next_page = "")
        {

            var roteApi = $"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&page={page}&limit={limit}&deal_pipeline_id=6568d8ab81277a0020e5a736";

            if (!string.IsNullOrEmpty(next_page))
            {
                roteApi = $"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&next_page={next_page}&limit={limit}";
            }

            var returnHttp = await _httpService.GetAsync<RdStationUnidadeConsumidoraResponseDto>(roteApi);

            while (returnHttp.has_more)
            {

                foreach(var deal in returnHttp.deals)
                {

                    Log.Debug(deal.name);

                    string end = "";
                    foreach(var cf in deal.deal_custom_fields)
                    {
                        if ("6176e7009ed1b10013bcebba".Equals(cf.custom_field_id))
                        {
                            //UF
                            end += "UF: " + cf.value;
                        } else if ("6176e7009ed1b10013bcebba".Equals(cf.custom_field_id))
                        {
                            //Cidade
                            end += "Cidade: " + cf.value;
                        }
                    }
                    break;
                }

                break;
            }

            Log.Debug(returnHttp.ToString());


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
