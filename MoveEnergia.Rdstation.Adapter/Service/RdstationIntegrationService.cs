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
using System;
using System.Net.Mail;

namespace MoveEnergia.Rdstation.Adapter.Service
{
    public class RdStationIntegrationService : IRdStationIntegrationService
    {
        private readonly ILogger<RdStationIntegrationService> _logger;
        private readonly IHttpService _httpService;
        private readonly RdStationConfiguration _rdStationConfiguration;
        private readonly RdStationIntegrationCustomer _rdStationIntegrationCustomer;
        private readonly IRdFieldsIntegrationRepository _iRdFieldsIntegrationRepository;    

        public RdStationIntegrationService(ILogger<RdStationIntegrationService> logger,
                                           IHttpService httpService,
                                           IOptions<RdStationConfiguration> rdStationConfiguration,
                                           IOptions<RdStationIntegrationCustomer> rdStationIntegrationCustomer,
                                           IRdFieldsIntegrationRepository rdFieldsIntegrationRepository                                           
                                          )
        {
            _logger = logger;
            _httpService = httpService;
            _rdStationConfiguration = rdStationConfiguration.Value;
            _rdStationIntegrationCustomer = rdStationIntegrationCustomer.Value;
            _iRdFieldsIntegrationRepository = rdFieldsIntegrationRepository;
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
        public async Task<ReturnResponseDto> MappingDealToCustomer(Dictionary<string, string> fieldsDeal, DealsResponseDto dealsResponseDto)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            var fieldsIntegration = await _iRdFieldsIntegrationRepository.GetAll();

            if (fieldsIntegration != null )
            {
                var fields = fieldsIntegration.ToList();

                RdCustomerResponseDto customer = new RdCustomerResponseDto();

                if (fields != null && fields.Count > 0)
                {
                    
                    RdFieldsIntegration rdField = new RdFieldsIntegration();

                    rdField = fields.Where(x => x.Label == "Nome Consumidor").FirstOrDefault();
                    customer.Name = DictionaryString.GetValueByFieldId(fieldsDeal, rdField.IdRd);
                    var isNameRequired = rdField.Required;

                    rdField = fields.Where(x => x.Label == "CPF/CNPJ").FirstOrDefault();
                    customer.Code = DictionaryString.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    if (rdField.Required && customer.Code == "")
                    {
                        returnResponseDto.Error = true;
                        returnResponseDto.StatusCode = 400;
                        return returnResponseDto;
                    }
                    else
                    {
                        var doc = Strings.FormatToValueCPFCNPJ(customer.Code);
                        customer.TipoCustomer = doc.Length > 11 ? (byte)TipoCustomer.Juridica : (byte)TipoCustomer.Fisica;
                        customer.TenantId = 0;
                    }

                    string NameUser = "";

                    RdCustomerUserResponseDto user = new RdCustomerUserResponseDto();                   

                    if (dealsResponseDto.contacts != null && dealsResponseDto.contacts.Count > 0)
                    {
                        var emailReg = dealsResponseDto.contacts.FirstOrDefault().emails.FirstOrDefault();

                        if (emailReg != null)
                        {
                            user.Email = emailReg.email;
                        }

                        if (customer.Name == "")
                        {
                            NameUser = dealsResponseDto.contacts.FirstOrDefault().name;
                        }
                        else 
                        {
                            NameUser = customer.Name;
                        }

                    }
                    else
                    {
                        if (dealsResponseDto.user != null )
                        {
                            user.Email = dealsResponseDto.user.email;
                            NameUser = dealsResponseDto.name;
                        }
                        else
                        {
                            rdField = fields.Where(x => x.Label == "Coop - E-mail para envio fatura").FirstOrDefault();
                            user.Email = DictionaryString.GetValueByFieldId(fieldsDeal, rdField.IdRd);
                        }
                    }

                    if (isNameRequired && customer.Name == "")
                    {
                        returnResponseDto.Error = true;
                        returnResponseDto.StatusCode = 400;
                        return returnResponseDto;
                    }

                    var nameToMail = user.Email.Substring(0, user.Email.IndexOf('@')).Replace(" ","").Trim();

                    user.TenantId = customer.TenantId;
                    user.UserName = nameToMail;
                    user.Name = Strings.FormatToName(NameUser);
                    user.Surname = Strings.FormatToName(NameUser);
                    user.PasswordHash = _rdStationIntegrationCustomer.HashPassWordDefaultUser;
                    user.PhoneNumberConfirmed = false;
                    user.EmailConfirmed = true;
                    user.IsActive = true;
                    user.AccessFailedCount = 0;
                    user.NormalizedEmail = user.Email.ToUpperInvariant();
                    user.NormalizedUserName = user.UserName.ToUpperInvariant();

                    customer.User = user;

                    RdCustomerAdressResponseDto adress = new RdCustomerAdressResponseDto();

                    rdField = fields.Where(x => x.Label == "Endereço - CEP").FirstOrDefault();
                    adress.CEP = DictionaryString.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    rdField = fields.Where(x => x.Label == "Endereço - Logradouro").FirstOrDefault();
                    adress.Logradouro = DictionaryString.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    rdField = fields.Where(x => x.Label == "Endereço - Número").FirstOrDefault();
                    adress.Numero = DictionaryString.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    rdField = fields.Where(x => x.Label == "Endereço - Bairro").FirstOrDefault();
                    adress.Bairro = DictionaryString.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    rdField = fields.Where(x => x.Label == "Endereço - Cidade").FirstOrDefault();
                    var cidade = DictionaryString.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    adress.CityId = 1;

                    customer.Adress = adress;


                }

                returnResponseDto.Error = false;
                returnResponseDto.StatusCode = 200;
                returnResponseDto.Data = customer;
                return returnResponseDto;

            }

            returnResponseDto.Error = false;
            returnResponseDto.StatusCode = 404;
            return returnResponseDto;
        }
    }
}
