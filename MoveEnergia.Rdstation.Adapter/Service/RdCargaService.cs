using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Enum;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Data.Context;
using MoveEnergia.Billing.Helper;
using MoveEnergia.Rdstation.Adapter.Configuration;
using MoveEnergia.Rdstation.Adapter.Interface.Service;
using MoveEnergia.RdStation.Adapter.Configuration;
using MoveEnergia.RdStation.Adapter.Dto.Response;
using MoveEnergia.RdStation.Adapter.Interface.Service;
using Serilog;
using System.Text;

namespace MoveEnergia.Rdstation.Adapter.Service
{
    public class RdCargaService : IRdCargaService
    {
        private readonly ILogger<RdStationIntegrationService> _logger;
        private readonly IHttpService _httpService;
        private readonly RdStationConfiguration _rdStationConfiguration;
        private readonly RdStationIntegrationCustomer _rdStationIntegrationCustomer;
        private readonly IRdFieldsIntegrationRepository _iRdFieldsIntegrationRepository;
        private readonly IDealRepository _iDealRepository;
        private readonly ApplicationDbContext _dbContext;


        public RdCargaService(ILogger<RdStationIntegrationService> logger,
                                           IHttpService httpService,
                                           IOptions<RdStationConfiguration> rdStationConfiguration,
                                           IOptions<RdStationIntegrationCustomer> rdStationIntegrationCustomer,
                                           IRdFieldsIntegrationRepository rdFieldsIntegrationRepository,
                                           IDealRepository iDealRepository,
                                           ApplicationDbContext dbContext
                                          )
        {
            _logger = logger;
            _httpService = httpService;
            _rdStationConfiguration = rdStationConfiguration.Value;
            _rdStationIntegrationCustomer = rdStationIntegrationCustomer.Value;
            _iRdFieldsIntegrationRepository = rdFieldsIntegrationRepository;
            _iDealRepository = iDealRepository;
            _dbContext = dbContext;
        }

        public async Task<RdReturnResponseDto> GetPipelines()
        {

            var roteApi = $"{_rdStationConfiguration.UrlBase}/deal_pipelines?token={_rdStationConfiguration.Token}";

            var returnHttp = await _httpService.GetAsyncToJsonArray(roteApi);

            Log.Debug(returnHttp.ToString());


            var returnDto = new RdReturnResponseDto()
            {
                Error = false,
                StatusCode = 200,
                Data = returnHttp,
            };

            return returnDto;
        }

        public async Task<RdReturnResponseDto> CargaEnderecosAsync(int page = 0, int limit = 200, string next_page = "")
        {

            var roteApi = $"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&page={page}&limit={limit}&deal_pipeline_id=6568d8ab81277a0020e5a736";

            if (!string.IsNullOrEmpty(next_page))
            {
                roteApi = $"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&next_page={next_page}&limit={limit}";
            }

            var returnHttp = await _httpService.GetAsync<RdStationUnidadeConsumidoraResponseDto>(roteApi);

            while (true)
            {
                Log.Debug(">>>>>>>>>>>>>>>>>>>>>> INICIANDO NOVO LOOP");
                foreach(var deal in returnHttp.deals)
                {

                    string? UC = "";
                    string? rua = "";
                    string? numero = "";
                    string? complemento = "";
                    string? bairro = "";
                    string? cidade = "";
                    string? uf = "";
                    string? cep = "";
                    foreach (var cf in deal.deal_custom_fields)
                    {
                        
                        if ("6176e55ab06e22000c09340a".Equals(cf.custom_field_id)) UC = Convert.ToString(cf.value);
                        else if ("6176e69eea04fc000b923707".Equals(cf.custom_field_id)) rua = Convert.ToString(cf.value);
                        else if ("6176e6ab575f85000c36b177".Equals(cf.custom_field_id)) numero = Convert.ToString(cf.value);
                        else if ("6176e6e69bc2880012ef32a4".Equals(cf.custom_field_id)) complemento = Convert.ToString(cf.value);
                        else if ("6176e6ee4646dc0010331027".Equals(cf.custom_field_id)) bairro = Convert.ToString(cf.value);
                        else if ("6176e6f7fce5e60016590117".Equals(cf.custom_field_id)) cidade = Convert.ToString(cf.value);
                        else if ("6176e7009ed1b10013bcebba".Equals(cf.custom_field_id)) uf = Convert.ToString(cf.value);
                        else if ("6176e70c081cb3001dac55a6".Equals(cf.custom_field_id)) cep = Convert.ToString(cf.value);

                    }
                    Log.Debug(UC+"//"+deal.name + "//" + rua+"//"+cidade);

                    if (!String.IsNullOrEmpty(UC))
                    {
                        Deals? dealDomain = await _dbContext.deals
                            .Where(u => u.UC == UC)
                            .FirstOrDefaultAsync();

                        if (dealDomain != null)
                        {
                            Log.Debug("Atualizando UC:"+UC);
                            dealDomain.EndRua = String.IsNullOrEmpty(rua) ? null : rua;
                            dealDomain.EndNumero = String.IsNullOrEmpty(numero) ? null : numero;
                            dealDomain.EndComplemento = String.IsNullOrEmpty(complemento) ? null : complemento;
                            dealDomain.EndBairro = String.IsNullOrEmpty(bairro) ? null : bairro;
                            dealDomain.EndCidade = String.IsNullOrEmpty(cidade) ? null : cidade;
                            dealDomain.EndUF = String.IsNullOrEmpty(uf) ? null : uf;
                            dealDomain.CEP = String.IsNullOrEmpty(cep) ? null : cep;
                            await _iDealRepository.UpdateAsync(dealDomain);
                            await _iDealRepository.SaveAsync();

                        }

                    }
                }

                Log.Debug("HAS MORE:" + returnHttp.has_more);
                if (returnHttp.has_more)
                {
                    Log.Debug($"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&next_page={returnHttp.next_page}&limit={limit}&deal_pipeline_id=6568d8ab81277a0020e5a736");
                    roteApi = $"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&next_page={returnHttp.next_page}&limit={limit}&deal_pipeline_id=6568d8ab81277a0020e5a736";
                    returnHttp = await _httpService.GetAsync<RdStationUnidadeConsumidoraResponseDto>(roteApi);
                } else break;
            }

            var returnDto = new RdReturnResponseDto()
            {
                Error = false,
                StatusCode = 200,
                Data = returnHttp,
            };

            return returnDto;
        }
        
    }
}
