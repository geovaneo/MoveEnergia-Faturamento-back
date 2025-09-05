using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Enum;
using MoveEnergia.Billing.Helper;
using MoveEnergia.RdStation.Adapter.Configuration;
using MoveEnergia.RdStation.Adapter.Dto.Response;
using MoveEnergia.RdStation.Adapter.Interface.Adapter;
using MoveEnergia.RdStation.Adapter.Interface.Service;

namespace MoveEnergia.RdStation.Adapter
{
    public class RdstationIntegrationAdapter : IRdstationIntegrationAdapter
    {
        private readonly IMapper _mapper;
        private readonly ILogger<RdstationIntegrationAdapter> _logger;
        private readonly IRdStationIntegrationService _iRdStationIntegrationService;
        private readonly RdStationIntegrationCustomer _rdStationIntegrationCustomer;
        public RdstationIntegrationAdapter(ILogger<RdstationIntegrationAdapter> logger,
                                           IMapper mapper,
                                           IRdStationIntegrationService iRdStationIntegrationService,
                                           IOptions<RdStationIntegrationCustomer> rdStationIntegrationCustomer)
        {
            _logger = logger;
            _mapper = mapper;
            _iRdStationIntegrationService = iRdStationIntegrationService;
            _rdStationIntegrationCustomer = rdStationIntegrationCustomer.Value;
        }

        public async Task<ReturnResponseDto> GetContactsAsync(string dealId)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var returnDto = await _iRdStationIntegrationService.GetContactsAsync(dealId);

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
                var returnDto = await _iRdStationIntegrationService.FetchUnidadesPageAsync(page, limit, next_page);

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
                var returnDto = await _iRdStationIntegrationService.FetchUnidadesFromRdStationAsync(dealId, isStage, page, limit);

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
        public async Task<ReturnResponseDto> ProcessIntegrationCustomerAsync(ProcessIntegrationCustomerRequestDto requestDto)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                string nextPage = "";
                int limit = requestDto.Records;
                int totalRecords =  0;
                int totalPages = 0;
                int page = 1;

                var registros = await _iRdStationIntegrationService.FetchUnidadesPageAsync(page, limit, nextPage);

                if (registros != null && registros.Data != null)
                {
                    var dealData = (RdStationUnidadeConsumidoraResponseDto)registros.Data;
                    totalRecords = dealData.total;
                    totalPages = (int)Math.Ceiling((double)totalRecords / limit);

                    do
                    {

                        if (registros != null && registros.Data != null)
                        {
                            var listDeals = (RdStationUnidadeConsumidoraResponseDto)registros.Data;

                            if (listDeals.deals != null && listDeals.deals.Count > 0)
                            {

                                foreach (var itemDeal in listDeals.deals)
                                {

                                    var dictyDeal = itemDeal.deal_custom_fields
                                        .GroupBy(x => x.custom_field_id)
                                        .ToDictionary(
                                            g => g.Key,
                                            g => g.Last().value?.ToString() ?? string.Empty
                                        ).ToList();

                                    if (dictyDeal != null && dictyDeal.Count > 0)
                                    {
                                        var isCustomer = dictyDeal.Exists(kv =>
                                                                            kv.Key == _rdStationIntegrationCustomer.KeyCustomer &&
                                                                            kv.Value == _rdStationIntegrationCustomer.ValueCustomer);
                                        var isRS = dictyDeal.Exists(kv =>
                                                                    kv.Key == "6176e7009ed1b10013bcebba" &&
                                                                    kv.Value == "RS");

                                        var isCidade = dictyDeal.Exists(kv =>
                                                                    kv.Key == "6176e6f7fce5e60016590117" &&
                                                                    kv.Value == "PORTO ALEGRE");


                                        if (isCustomer && isRS && isCidade)
                                        {

                                            var listCustomer = dictyDeal.ToDictionary(kv => kv.Key, kv => kv.Value);

                                            var customerMap = await _iRdStationIntegrationService.MappingDealToCustomerApi(listCustomer, itemDeal);

                                            if (customerMap.Data != null)
                                            {
                                                var customerData = (RdCustomerResponseDto)customerMap.Data;

                                                User user = new User();
                                                Customer customer = new Customer();
                                                Address address = new Address();

                                                if (customerData.User != null)
                                                {
                                                    user = new User()
                                                    {
                                                        TenantId = customerData.User.TenantId,
                                                        UserName = customerData.User.UserName,
                                                        Name = customerData.User.Name,
                                                        Surname = customerData.User.Surname,
                                                        PasswordHash = customerData.User.PasswordHash,
                                                        PhoneNumberConfirmed = customerData.User.PhoneNumberConfirmed,
                                                        EmailConfirmed = customerData.User.EmailConfirmed,
                                                        IsActive = customerData.User.IsActive,
                                                        AccessFailedCount = customerData.User.AccessFailedCount,
                                                        NormalizedEmail = customerData.User.NormalizedEmail,
                                                        NormalizedUserName = customerData.User.NormalizedUserName,
                                                        Email = customerData.User.Email,
                                                        CreationTime = DateTime.Now,
                                                        IsDelete = false,

                                                    };
                                                }
                                                else
                                                {
                                                    //error
                                                }

                                                if (customerData != null)
                                                {
                                                    customer = new Customer()
                                                    {
                                                        Id = customerData.Id,
                                                        UserId = customerData.UserId,
                                                        Name = customerData.Name,
                                                        RazoSocial = customerData.RazoSocial,
                                                        Code = customerData.Code,
                                                        TipoCustomer = customerData.TipoCustomer,
                                                        TenantId = customerData.TenantId,
                                                        Mercado = customerData.Mercado,
                                                        CreationTime = DateTime.Now
                                                    };
                                                }
                                                else
                                                {
                                                    //erro
                                                }

                                                if (customerData.Adress != null)
                                                {
                                                    address = new Address()
                                                    {
                                                        //Id = customerData.Adress.Id,
                                                        CEP = customerData.Adress.CEP,
                                                        Logradouro = customerData.Adress.Logradouro,
                                                        Numero = customerData.Adress.Numero,
                                                        Complemento = customerData.Adress.Complemento,
                                                        Bairro = customerData.Adress.Bairro,
                                                        CityId = customerData.Adress.CityId,
                                                        CustomerId = customerData.Adress.CustomerId
                                                    };
                                                }
                                                else
                                                {
                                                    //erro
                                                }

                                                ConsumerUnitCustumer consumerUnitCostumer = new ConsumerUnitCustumer()
                                                {
                                                    UC = customerData.UC,
                                                    CreateDate = DateTime.Now
                                                };

                                                var customerAdd = await _iRdStationIntegrationService.SetCustomerSync(customer, address, user, consumerUnitCostumer);

                                            }
                                        }
                                    }
                                }
                            }

                            if (page == totalPages)
                            {
                                page = -1;
                            }
                            else
                            {
                                page++;

                                registros = await _iRdStationIntegrationService.FetchUnidadesPageAsync(page, limit, nextPage);

                                _logger.LogInformation($"Pagina: {page} de {totalPages}");
                            }
                            
                        }
                        else
                        {
                            _logger.LogCritical("Objeto contendo dados veio vazio ou null");
                            break;
                        }
                    }
                    while (page > -1);

                }

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Erro: {ex.Message}");

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
        public async Task<ReturnResponseDto> SyncCustomerAsync(SyncCustomerRequestDto requestDto)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var deals = await _iRdStationIntegrationService.GetByTitularidadeAsync(Titularidade.Consumidor.GetDescription());
                var registros = deals.ToList();

                if (registros != null && registros.Count > 0)
                {
                    foreach (var itemDeal in registros) 
                    {

                        _logger.LogInformation($"Processando Mapeamento Deal : {itemDeal.DealId} - {itemDeal.Name} ");

                        var dealMap = await _iRdStationIntegrationService.MappingDealToCustomer(itemDeal);

                        if (dealMap.Error == false && dealMap.StatusCode == 200)
                        {
                            if (dealMap.Data != null)
                            {
                                _logger.LogInformation($"Gravando Deal : {itemDeal.DealId} - {itemDeal.Name} ");

                                var customerData = (RdCustomerResponseDto)dealMap.Data;

                                User user = new User();
                                Customer customer = new Customer();
                                Address address = new Address();

                                if (customerData.User != null)
                                {
                                    user = new User()
                                    {
                                        TenantId = customerData.User.TenantId,
                                        UserName = customerData.User.UserName,
                                        Name = customerData.User.Name,
                                        Surname = customerData.User.Surname,
                                        PasswordHash = customerData.User.PasswordHash,
                                        PhoneNumberConfirmed = customerData.User.PhoneNumberConfirmed,
                                        EmailConfirmed = customerData.User.EmailConfirmed,
                                        IsActive = customerData.User.IsActive,
                                        AccessFailedCount = customerData.User.AccessFailedCount,
                                        NormalizedEmail = customerData.User.NormalizedEmail,
                                        NormalizedUserName = customerData.User.NormalizedUserName,
                                        Email = customerData.User.Email,
                                        CreationTime = DateTime.Now,
                                        IsDelete = false,

                                    };
                                }

                                if (customerData != null)
                                {
                                    customer = new Customer()
                                    {
                                        Id = customerData.Id,
                                        UserId = customerData.UserId,
                                        Name = customerData.Name,
                                        RazoSocial = customerData.RazoSocial,
                                        Code = customerData.Code,
                                        TipoCustomer = customerData.TipoCustomer,
                                        TenantId = customerData.TenantId,
                                        Mercado = customerData.Mercado,
                                        CreationTime = DateTime.Now
                                    };
                                }

                                if (customerData.Adress != null)
                                {
                                    address = new Address()
                                    {
                                        //Id = customerData.Adress.Id,
                                        CEP = customerData.Adress.CEP,
                                        Logradouro = customerData.Adress.Logradouro,
                                        Numero = customerData.Adress.Numero,
                                        Complemento = customerData.Adress.Complemento,
                                        Bairro = customerData.Adress.Bairro,
                                        CityId = customerData.Adress.CityId,
                                        CustomerId = customerData.Adress.CustomerId
                                    };
                                }

                                ConsumerUnitCustumer consumerUnitCostumer = new ConsumerUnitCustumer()
                                {
                                    UC = customerData.UC,
                                    CreateDate = DateTime.Now
                                };

                                var customerAdd = await _iRdStationIntegrationService.SetCustomerSync(customer, address, user, consumerUnitCostumer);

                            }
                        }
                        else
                        {
                            _logger.LogCritical($"Erro no mapeamento da deal: {itemDeal.DealId} - {itemDeal.Name}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Erro: {ex.Message}");

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
