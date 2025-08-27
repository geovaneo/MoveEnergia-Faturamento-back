using AutoMapper;
using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Billing.Core.Interface.Adapter;
using MoveEnergia.Billing.Core.Interface.Service;

namespace MoveEnergia.Billing.Adapter
{
    public class ConsumerUnitAdapter : IConsumerUnitAdapter
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ConsumerUnitAdapter> _logger;
        private readonly IConsumerUnitService _consumerUnitService;
        public ConsumerUnitAdapter(ILogger<ConsumerUnitAdapter> logger,
                                   IMapper mapper,
                                   IConsumerUnitService consumerUnitService)
        {
            _logger = logger;
            _mapper = mapper;
            _consumerUnitService = consumerUnitService;
        }

        public async Task<ReturnResponseDto> GetConsumerUnitByIdUser(int idUser)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var result = await _consumerUnitService.GetByIdUserAsync(idUser);

                if (result != null)
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 200;
                    returnResponseDto.Data = result; //JsonConvert.SerializeObject(result);
                }
                else
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 404;
                    returnResponseDto.Data = null;
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

        public async Task<ReturnResponseDto> GetConsumerUnitByAdressIdUserAsync(long idUser)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var result = await _consumerUnitService.GetByAdressIdUserAsync(idUser);

                List<CustomerUnitAdressReponseDto> customerUnitList = new List<CustomerUnitAdressReponseDto>();

                if (result != null && result.Count > 0)
                {
                    foreach (var itemUnit in result)
                    {

                        List<AdressCustomerUnitResponseDto> Enderecos = new List<AdressCustomerUnitResponseDto>();

                        var customerUnit = new CustomerUnitAdressReponseDto()
                        {
                            Id = itemUnit.Id,
                            Name = itemUnit.Nome,
                            UC = itemUnit.UC
                        };

                        if (itemUnit.Customer?.Addresses != null && itemUnit.Customer?.Addresses.Count > 0 )
                        {
                            var customerUnitAdress = itemUnit.Customer.Addresses.Select( a => new AdressCustomerUnitResponseDto()
                            {
                                Id = a.Id,
                                Street = a.Logradouro,
                                Number = a.Numero,
                                Complement = a.Complemento,
                                Neighborhood = a.Bairro,
                                ZipCode = a.CEP,
                                City = a.City?.Nome == null ? "" : a.City?.Nome,
                                FederativeUnit = a.City?.UF.Sigla == null ? "" : a.City?.UF.Sigla,
                                Country = a.City?.UF?.Country.Nome == null ? "" : a.City?.UF?.Country.Nome,
                                AddressStreet = $"{a.Logradouro}-{a.Numero}, {a.Bairro}, {a.CEP}, {a.City.Nome} - {a.City?.UF.Sigla}."
                            }).ToList();

                            customerUnit.Address = customerUnitAdress;
                        }

                        customerUnitList.Add(customerUnit);

                    }

                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 200;
                    returnResponseDto.Data = customerUnitList; //JsonConvert.SerializeObject(result);
                }
                else
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 404;
                    returnResponseDto.Data = null;
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
