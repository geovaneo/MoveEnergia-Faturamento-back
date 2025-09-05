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

namespace MoveEnergia.Rdstation.Adapter.Service
{
    public class RdStationIntegrationService : IRdStationIntegrationService
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
        public RdStationIntegrationService(ILogger<RdStationIntegrationService> logger,
                                           IHttpService httpService,
                                           IOptions<RdStationConfiguration> rdStationConfiguration,
                                           IOptions<RdStationIntegrationCustomer> rdStationIntegrationCustomer,
                                           IRdFieldsIntegrationRepository rdFieldsIntegrationRepository,
                                           ICityRepository iCityRepository,
                                           IAddressRepository iAddressRepository,
                                           ICustomerRepository iCustomerRepository,
                                           IUserRepository iUserRepository,
                                           IConsumerUnitCustumerRepository iConsumerUnitCostumerRepository,
                                           IDealRepository iDealRepository
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
        }
        public async Task<ReturnResponseDto> GetContactsAsync(string dealId)
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
                roteApi = $"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&page={page}&deal_stage_id={dealId}&limit={limit}";
            }
            else
            {
                roteApi = $"{_rdStationConfiguration.UrlBase}/deals?token={_rdStationConfiguration.Token}&page={page}&deal_id={dealId}&limit={limit}";
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
        public async Task<ReturnResponseDto> MappingDealToCustomerApi(Dictionary<string, string> fieldsDeal, DealsResponseDto dealsResponseDto)
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
                    customer.Name = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);
                    var isNameRequired = rdField.Required;

                    rdField = fields.Where(x => x.Label == "CPF/CNPJ").FirstOrDefault();
                    customer.Code = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    if (rdField.Required && customer.Code == "")
                    {
                        returnResponseDto.Error = true;
                        returnResponseDto.StatusCode = 400;
                        return returnResponseDto;
                    }
                    else
                    {
                        var doc = StringsExtensions.FormatToValueCPFCNPJ(customer.Code);
                        customer.TipoCustomer = doc.Length > 11 ? (byte)TipoCustomer.Juridica : (byte)TipoCustomer.Fisica;
                        customer.TenantId = 0;
                    }

                    rdField = fields.Where(x => x.Label == "Unidade consumidora").FirstOrDefault();
                    customer.UC = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    string NameUser = "";

                    RdCustomerUserResponseDto user = new RdCustomerUserResponseDto();                   

                    if (dealsResponseDto.contacts != null && dealsResponseDto.contacts.Count > 0)
                    {
                        var emailReg = dealsResponseDto.contacts.FirstOrDefault().emails.FirstOrDefault();

                        if (emailReg != null)
                        {
                            user.Email = emailReg.email;
                        }

                        var nomeContact = dealsResponseDto.contacts.FirstOrDefault().name;

                        if (nomeContact != "")
                        {
                            NameUser = nomeContact;
                            customer.Name = nomeContact;
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
                            user.Email = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);
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
                    user.Name = StringsExtensions.FormatToName(NameUser);
                    user.Surname = StringsExtensions.FormatToName(NameUser);
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
                    adress.CEP = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    rdField = fields.Where(x => x.Label == "Endereço - Logradouro").FirstOrDefault();
                    adress.Logradouro = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    rdField = fields.Where(x => x.Label == "Endereço - Número").FirstOrDefault();
                    adress.Numero = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    rdField = fields.Where(x => x.Label == "Endereço - Bairro").FirstOrDefault();
                    adress.Bairro = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    rdField = fields.Where(x => x.Label == "Endereço - Cidade").FirstOrDefault();
                    var cidade = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                    adress.CityId = 0;
                    
                    var city = await _iCityRepository.GetByNameAsync(cidade);

                    if (city != null)
                    {
                        adress.CityId = city.Id;
                    }

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
        public async Task<ReturnResponseDto> SetCustomerSync(Customer customer, Address address, User user, ConsumerUnitCustumer consumerUnitCostumer)
        {

            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            var customerExist = await _iCustomerRepository.GetByCodeAsync(customer.Code);
            var userExist = new User();

            if (customerExist == null) 
            {
                userExist = await _iUserRepository.GetByUserNameAsync(user.UserName);

                if (userExist == null)
                {
                    var userAdd = await _iUserRepository.CreateAsync(user);
                    await _iUserRepository.SaveAsync();

                    if (userAdd.Id != 0)
                    {
                        customer.UserId = userAdd.Id;

                        var customerAdd = await _iCustomerRepository.CreateAsync(customer);
                        await _iCustomerRepository.SaveAsync();

                        if (customerAdd.Id != 0)
                        {
                            if (consumerUnitCostumer.UC != null && consumerUnitCostumer.UC != "")
                            {
                                var consumerUnitCostumerExist = await _iConsumerUnitCostumerRepository.GetByCustomerIdUCAsync(consumerUnitCostumer.UC, customerAdd.Id);

                                if (consumerUnitCostumerExist == null)
                                {
                                    consumerUnitCostumer.CustomerId = customerAdd.Id;

                                    var consumerUnitCostumerAdd = await _iConsumerUnitCostumerRepository.CreateAsync(consumerUnitCostumer);
                                    await _iConsumerUnitCostumerRepository.SaveAsync();
                                }
                            }

                            var addresExist = await _iAddressRepository.GetByCepNumeroCustomerAsync(address.CEP, address.Numero, customerAdd.Id);

                            if (addresExist == null)
                            {
                                address.CustomerId = customerAdd.Id;

                                var addresAdd = await _iAddressRepository.CreateAsync(address);
                                await _iAddressRepository.SaveAsync();
                            }
                        }
                    }
                }
            }
            else
            {
                var addresExist = await _iAddressRepository.GetByCepNumeroCustomerAsync(address.CEP, address.Numero, customerExist.Id);

                if (addresExist == null)
                {
                    address.CustomerId = customerExist.Id;
                    var addresAdd = await _iAddressRepository.CreateAsync(address);
                    await _iAddressRepository.SaveAsync();
                }

                if (consumerUnitCostumer.UC != null && consumerUnitCostumer.UC != "")
                {
                    var consumerUnitCostumerExist = await _iConsumerUnitCostumerRepository.GetByCustomerIdUCAsync(consumerUnitCostumer.UC, customerExist.Id);

                    if (consumerUnitCostumerExist == null)
                    {
                        consumerUnitCostumer.CustomerId = customerExist.Id;

                        var consumerUnitCostumerAdd = await _iConsumerUnitCostumerRepository.CreateAsync(consumerUnitCostumer);
                        await _iConsumerUnitCostumerRepository.SaveAsync();
                    }
                }

            }
            returnResponseDto.Error = false;
            returnResponseDto.StatusCode = 200;
            returnResponseDto.Data = customer;


            return returnResponseDto;
        }
        public async Task<List<Deals>> GetByTitularidadeAsync(string titularidade)
        {
            var registro = await _iDealRepository.GetByTitularidadeAsync(titularidade);
            return registro.ToList();
        }
        public async Task<ReturnResponseDto> MappingDealToCustomer(Deals deal)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            RdCustomerUserResponseDto user = new RdCustomerUserResponseDto();   
            RdCustomerResponseDto customer = new RdCustomerResponseDto();
            RdCustomerAdressResponseDto adress = new RdCustomerAdressResponseDto();

            var dealResponse = await GetContactsAsync(deal.DealId);

            if (dealResponse != null && dealResponse.Data != null)
            {
                ContactEmailResponseDto contactMail = new ContactEmailResponseDto();

                var contactData = (ContactDataResponseDto)dealResponse.Data;

                if (contactData.contacts != null)
                {
                    var contactDeal = contactData.contacts.OrderByDescending(c => c.id).FirstOrDefault();

                    if (contactDeal != null)
                    {
                        contactMail = contactDeal.emails.OrderByDescending(c => c.id).FirstOrDefault();
                    }
                }

                if (contactMail != null && contactMail.email != null)
                {
                    var nameToMail = contactMail.email.Substring(0, contactMail.email.IndexOf('@')).Replace(" ", "").Trim();

                    var doc = StringsExtensions.FormatToValueCPFCNPJ(deal.CNPJCPF);

                    customer.Name = deal.Name;
                    customer.Code = deal.CNPJCPF;
                    customer.UC = deal.UC;
                    customer.TipoCustomer = doc.Length > 11 ? (byte)TipoCustomer.Juridica : (byte)TipoCustomer.Fisica;
                    customer.TenantId = 0;

                    user = new RdCustomerUserResponseDto()
                    {
                        Email = contactMail.email,
                        TenantId = customer.TenantId,
                        UserName = nameToMail,
                        Name = StringsExtensions.FormatToName(deal.Name),
                        Surname = StringsExtensions.FormatToName(deal.Name),
                        PasswordHash = _rdStationIntegrationCustomer.HashPassWordDefaultUser,
                        PhoneNumberConfirmed = false,
                        EmailConfirmed = true,
                        IsActive = true,
                        AccessFailedCount = 0,
                        NormalizedEmail = contactMail.email.ToUpperInvariant(),
                        NormalizedUserName = nameToMail.ToUpperInvariant()
                    };

                    customer.User = user;

                    var dealDataCustomer = await GetDealToIdAsync(deal.DealId);

                    if (dealDataCustomer != null && dealDataCustomer.Data != null)
                    {
                        var dealRegAdress = (DealsResponseDto)dealDataCustomer.Data;

                        if (dealRegAdress.deal_custom_fields != null && dealRegAdress.deal_custom_fields.Count > 0)
                        {
                            var dictyDeal = dealRegAdress.deal_custom_fields
                                            .GroupBy(x => x.custom_field_id)
                                            .ToDictionary(
                                                g => g.Key,
                                                g => g.Last().value?.ToString() ?? string.Empty
                                            ).ToList();

                            var fieldsDeal = dictyDeal.ToDictionary(kv => kv.Key, kv => kv.Value);

                            var fieldsIntegration = await _iRdFieldsIntegrationRepository.GetAll();
                            var fields = fieldsIntegration.ToList();
                            RdFieldsIntegration rdField = new RdFieldsIntegration();

                            rdField = fields.Where(x => x.Label == "Endereço - CEP").FirstOrDefault();
                            var CEP = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                            rdField = fields.Where(x => x.Label == "Endereço - Logradouro").FirstOrDefault();
                            var Logradouro = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                            rdField = fields.Where(x => x.Label == "Endereço - Número").FirstOrDefault();
                            var Numero = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                            rdField = fields.Where(x => x.Label == "Endereço - Bairro").FirstOrDefault();
                            var Bairro = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                            rdField = fields.Where(x => x.Label == "Endereço - Cidade").FirstOrDefault();
                            var cidade = DictionaryExtensions.GetValueByFieldId(fieldsDeal, rdField.IdRd);

                            var CityId = 0;

                            var city = await _iCityRepository.GetByNameAsync(cidade);

                            if (city != null && CEP != null && Logradouro != null && Numero != null && Bairro != null)
                            {
                                CityId = city.Id;

                                adress = new RdCustomerAdressResponseDto()
                                {
                                    CEP = CEP,
                                    CityId = CityId,
                                    Bairro = Bairro,
                                    Logradouro = Logradouro,
                                    Numero = Numero
                                };

                                customer.Adress = adress;
                            }
                        }
                    }
                }
            }

            if (customer != null && customer.User != null && customer.Adress != null)
            {
                returnResponseDto.Error = false;
                returnResponseDto.StatusCode = 200;
                returnResponseDto.Data = customer;

            }
            else
            {
                returnResponseDto.Error = true;
                returnResponseDto.StatusCode = 404;
            }

            return returnResponseDto;
        }
        public async Task<ReturnResponseDto> GetDealToIdAsync(string dealId)
        {

            var roteApi = $"{_rdStationConfiguration.UrlBase}/deals/{dealId}?token={_rdStationConfiguration.Token}";

            var returnHttp = await _httpService.GetAsync<DealsResponseDto>(roteApi);

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
