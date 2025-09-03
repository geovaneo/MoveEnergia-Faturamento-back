using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Enum;
using MoveEnergia.Rdstation.Adapter.Configuration;
using MoveEnergia.RdStation.Adapter.Configuration;
using MoveEnergia.RdStation.Adapter.Dto.Response;
using MoveEnergia.RdStation.Adapter.Interface.Adapter;
using MoveEnergia.RdStation.Adapter.Interface.Service;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<ReturnResponseDto> GetCellphoneNumbersAsync(string dealId)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var returnDto = await _iRdStationIntegrationService.GetCellphoneNumbersAsync(dealId);

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

                while (true)
                {
                    var registros = await _iRdStationIntegrationService.FetchUnidadesPageAsync(0, requestDto.Records, nextPage);

                    if (registros != null && registros.Data == null)
                        break;

                    if (registros != null &&  registros.Data != null)
                    {
                        var listDeals = (RdStationUnidadeConsumidoraResponseDto)registros.Data;

                        if (listDeals.deals != null && listDeals.deals.Count > 0)
                        {

                            foreach (var itemDeal in listDeals.deals)
                            {
                                var dictyDeal = itemDeal.deal_custom_fields.ToDictionary(x => x.custom_field_id, y => y.value?.ToString() ?? string.Empty).ToList();

                                if (dictyDeal != null && dictyDeal.Count > 0)
                                {
                                    var isCustomer = dictyDeal.Exists(kv =>
                                                                       kv.Key == _rdStationIntegrationCustomer.KeyCustomer &&
                                                                       kv.Value == _rdStationIntegrationCustomer.ValueCustomer);

                                    if (isCustomer)
                                    {

                                        var listCustomer = dictyDeal.ToDictionary(kv => kv.Key, kv => kv.Value);

                                        var customerMap = await _iRdStationIntegrationService.MappingDealToCustomer(listCustomer, itemDeal);

                                        if(customerMap.Data != null)
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
                                                    Surname = customerData.User.UserName,
                                                    PasswordHash = customerData.User.PasswordHash,
                                                    PhoneNumberConfirmed = customerData.User.PhoneNumberConfirmed,
                                                    EmailConfirmed = customerData.User.EmailConfirmed,
                                                    IsActive = customerData.User.IsActive,
                                                    AccessFailedCount = customerData.User.AccessFailedCount,
                                                    NormalizedEmail = customerData.User.NormalizedEmail,
                                                    NormalizedUserName = customerData.User.NormalizedUserName
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
                                                    Mercado = customerData.Mercado
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
                                                    Id = customerData.Adress.Id,
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

                                            var customerAdd = await _iRdStationIntegrationService.SetCustomerSync(customer, address, user);

                                        }
                                    }
                                }
                            }
                        }
                        nextPage = listDeals.next_page;
                    }
                    else
                    {
                        break;
                    }                   
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
