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
using System.Diagnostics;

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

        public async Task<RdReturnResponseDto> GetContactsAsync(string dealId)
        {
            RdReturnResponseDto returnResponseDto = new RdReturnResponseDto();
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
        public async Task<RdReturnResponseDto> FetchUnidadesPageAsync(int page = 0, int limit = 200, string next_page = "")
        {
            RdReturnResponseDto returnResponseDto = new RdReturnResponseDto();
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
        public async Task<RdReturnResponseDto> FetchUnidadesFromRdStationAsync(string dealId, bool isStage, int page = 0, int limit = 1)
        {
            RdReturnResponseDto returnResponseDto = new RdReturnResponseDto();
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
        public async Task<RdReturnResponseDto> ProcessIntegrationCustomerAsync(ProcessIntegrationCustomerRequestDto requestDto)
        {
            RdReturnResponseDto returnResponseDto = new RdReturnResponseDto();
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
        public async Task<RdReturnResponseDto> SyncCustomerAsync(SyncCustomerRequestDto requestDto)
        {
            RdReturnResponseDto returnResponseDto = new RdReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var deals = await _iRdStationIntegrationService.GetByTitularidadeAsync(Titularidade.Consumidor.GetDescription());
                var registros = deals.ToList();

                int records = 0;
                int processSucess = 0;
                int processError = 0;

                if (registros != null && registros.Count > 0)
                {
                    records = registros.Count;

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

                                if (customerAdd.Error == false)
                                {
                                    processSucess++;

                                    _logger.LogInformation($"Processando com sucesso: {processSucess} de {records} - Error {processError}");
                                }
                            }

                            var returnProcess = new
                            {
                               registros = records,
                               sincronizado = processSucess,
                               Error = processError
                            };

                            returnResponseDto.Error = false;
                            returnResponseDto.StatusCode = 200;
                            returnResponseDto.Data = returnProcess;
                        }
                        else
                        {
                             processError++; 

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
        public async Task<RdReturnResponseDto> SyncCustomerListUCAsync(string listUCs)
        {
            RdReturnResponseDto returnResponseDto = new RdReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {

                var UCs = listUCs.Split(";").ToList();

                if (UCs == null && UCs.Count() == 0)
                {
                    returnResponseDto.Error = true;
                    returnResponseDto.StatusCode = 400;
                    returnResponseDto.Data = null;
                    returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Lista de UCs não pode ser vazia"
                    });

                    return returnResponseDto;
                }

                var deals = await _iRdStationIntegrationService.GetByUCValidateAsync(UCs);
                var registros = deals.ToList();

                int records = 0;
                int processSucess = 0;
                int processError = 0;

                if (registros != null && registros.Count > 0)
                {
                    records = registros.Count;

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
                                        CustomerId = customerData.Id
                                    };
                                }

                                ConsumerUnitCustumer consumerUnitCostumer = new ConsumerUnitCustumer()
                                {
                                    UC = customerData.UC,
                                    CreateDate = DateTime.Now
                                };

                                var customerAdd = await _iRdStationIntegrationService.SetCustomerSync(customer, address, user, consumerUnitCostumer);

                                if (customerAdd.Error == false)
                                {
                                    processSucess++;

                                    _logger.LogInformation($"Processando com sucesso: {processSucess} de {records} - Error {processError}");
                                }
                            }

                            var returnProcess = new
                            {
                                registros = records,
                                sincronizado = processSucess,
                                Error = processError
                            };

                            returnResponseDto.Error = false;
                            returnResponseDto.StatusCode = 200;
                            returnResponseDto.Data = returnProcess;
                        }
                        else
                        {
                            processError++;

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
        private async Task<List<string>> GetUCSync()
        {
            var listUC = new List<string>
                {
                    "49326637",
                    "56096108",
                    "52044650",
                    "3192229",
                    "28840756",
                    "1943",
                    "46822579",
                    "219045",
                    "18139960",
                    "96146443",
                    "43225553",
                    "51643666",
                    "32294022",
                    "43534343",
                    "6173160",
                    "5409195",
                    "29923230",
                    "49325983",
                    "95790799",
                    "3083285874",
                    "5421799",
                    "40474420",
                    "34371",
                    "50902668",
                    "4607953",
                    "6191878",
                    "54200536",
                    "101180683",
                    "48192777",
                    "99864",
                    "49403348",
                    "6181210",
                    "64166821",
                    "27148948",
                    "45548803",
                    "30160967",
                    "56144943"
            };

            return listUC;
        }
    }
}
