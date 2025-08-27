using AutoMapper;
using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Adapter;
using MoveEnergia.Billing.Core.Interface.Service;
using MoveEnergia.Billing.Helper;
using System.ComponentModel.Design;
using System.Globalization;

namespace MoveEnergia.Billing.Adapter
{
    public class HomeInfoAdapter : AdapterBase, IHomeInfoAdapter
    {

        private readonly IMapper _mapper;
        private readonly ILogger<HomeInfoAdapter> _logger;
        private readonly IHomeInfoService _homeInfoService;
        private readonly ICustomerService _customerService;
        public HomeInfoAdapter(ILogger<HomeInfoAdapter> logger,
                               IMapper mapper,
                               IHomeInfoService homeInfoService,
                               ICustomerService customerService)
        {
            _logger = logger;
            _mapper = mapper;
            _homeInfoService = homeInfoService;
            _customerService = customerService;
        }
        public async Task<ReturnResponseDto> GetHomeInfoByIdUCAsync(int idUC)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var result = await _homeInfoService.GetCurrentInvoiceByIdUcAsync(idUC);

                if (result != null)
                {
                    var customer = await _customerService.GetByIdCustomerAsync(result.CustomerId.Value);

                    decimal totalSavings = 0;
                    foreach (var itemValue in result.TotalSavings)
                    {
                        var value = itemValue == null ? "0" : itemValue.Replace('.', ',');
                        totalSavings += decimal.Parse(value);
                    }

                    decimal compensatedEnergyDecimal = decimal.Parse(result.CompensatedEnergy, CultureInfo.InvariantCulture);
                    long compensatedEnergy = (long)compensatedEnergyDecimal;

                    var addresse = customer?.Addresses?.FirstOrDefault();

                    HomeInfoResponseDto homeInfoResponseDto = new HomeInfoResponseDto()
                    {

                        InvoicesStatus = result.invoicesStatus,
                        CurrentInvoice = new CurrentInvoiceInfoResponseDto()
                        {
                            Id = result.Id,
                            BillingMonth = result.BillingMonth,
                            BillingNumber = result.BillingNumber,
                            ClientNumber = result.ClientNumber,
                            DueDate = result.DueDate,
                            InstallationNumber = result.InstallationNumber,
                            IssuedDate = result.IssuedDate,
                            TotalValue = Math.Round(result.TotalValue, 2),
                            ChargedCustomer = new ChargedCustomerInfoResponseDto()
                            {
                                Name = customer.Name,
                                City = addresse.City.Nome,
                                Address = $"{addresse.Logradouro}-{addresse.Numero}, {addresse.Bairro}, {addresse.CEP}, {addresse.City.Nome} - {addresse.City?.UF.Sigla}.",
                                Phone = customer.User.PhoneNumber == null ? "" : customer.User.PhoneNumber
                            }
                        },
                        GenaralInfo = new GeneralInfoResponseDto()
                        {
                            CompensatedEnergy = compensatedEnergy,
                            MonthSavings = Math.Round(decimal.Parse(result.MonthSavings.Replace('.', ',')), 2),
                            TotalSavings = Math.Round(totalSavings, 2)
                        }
                    };

                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 200;
                    returnResponseDto.Data = homeInfoResponseDto;
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
        public async Task<ReturnResponseDto> GetHomeInfoByUCAsync(string UC)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var result = await _homeInfoService.GetCurrentInvoiceByUcAsync(UC);

                if (result != null)
                {
                    var customer = await _customerService.GetByIdCustomerAsync(result.CustomerId.Value);

                    decimal totalSavings = 0;
                    foreach (var itemValue in result.TotalSavings)
                    {
                        var value = itemValue == null ? "0" : itemValue.Replace('.', ',');
                        totalSavings += decimal.Parse(value);
                    }

                    decimal compensatedEnergyDecimal = decimal.Parse(result.CompensatedEnergy, CultureInfo.InvariantCulture);
                    long compensatedEnergy = (long)compensatedEnergyDecimal;

                    var addresse = customer?.Addresses?.FirstOrDefault();

                    HomeInfoResponseDto homeInfoResponseDto = new HomeInfoResponseDto()
                    {

                        InvoicesStatus = result.invoicesStatus,
                        CurrentInvoice = new CurrentInvoiceInfoResponseDto()
                        {
                            Id = result.Id,
                            BillingMonth = result.BillingMonth,
                            BillingNumber = result.BillingNumber,
                            ClientNumber = result.ClientNumber,
                            DueDate = result.DueDate,
                            InstallationNumber = result.InstallationNumber,
                            IssuedDate = result.IssuedDate,
                            TotalValue = Math.Round(result.TotalValue, 2),
                            ChargedCustomer = new ChargedCustomerInfoResponseDto()
                            {
                                Name = customer.Name,
                                City = addresse.City.Nome,
                                Address = $"{addresse.Logradouro}-{addresse.Numero}, {addresse.Bairro}, {addresse.CEP}, {addresse.City.Nome} - {addresse.City?.UF.Sigla}.",
                                Phone = customer.User.PhoneNumber == null ? "" : customer.User.PhoneNumber
                            }
                        },
                        GenaralInfo = new GeneralInfoResponseDto()
                        {
                            CompensatedEnergy = compensatedEnergy,
                            MonthSavings = Math.Round(decimal.Parse(result.MonthSavings.Replace('.', ',')), 2),
                            TotalSavings = Math.Round(totalSavings, 2)
                        }
                    };

                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 200;
                    returnResponseDto.Data = homeInfoResponseDto;
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
        public async Task<ReturnResponseDto> GetLabelGraphicByIdUCAsync(int idUC)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                var result = await _homeInfoService.GetLabelGraphicByIdUcAsync(idUC);

                if (result != null)
                {
                    if (result.ConsumerUnitMeasurements != null && result.ConsumerUnitMeasurements.Count > 0)
                    {
                        List<LabelGraphicResponseDto> labels = new List<LabelGraphicResponseDto>();

                        var measurements = result.ConsumerUnitMeasurements.OrderBy(x => x.Date).ToList();

                        foreach (var itemConsumo in measurements)
                        {
                            labels.Add(new LabelGraphicResponseDto()
                            {
                                id = result.Id,
                                month = itemConsumo.Date.ToString("MMM/yyyy").ToUpper(),
                                consumption = Math.Round(itemConsumo.Value.Value, 2)
                            });
                        }

                        returnResponseDto.Error = false;
                        returnResponseDto.StatusCode = 200;
                        returnResponseDto.Data = labels;
                    }
                    else
                    {
                        returnResponseDto.Error = true;
                        returnResponseDto.StatusCode = 404;
                        returnResponseDto.Data = null;
                        returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                        {
                            ErrorCode = 404,
                            ErrorMessage = "Não existe medições para a UC informada."
                        });
                    }
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
        public async Task<ReturnResponseDto> GetLabelGraphicDetailByIdUCAsync(int idUc, int month, int year)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {
                DateTime dateRef = DateTime.Parse($"{year}/{month}/01");

                var consumerUnitMeasurementDetail = await _homeInfoService.GetConsumerUnitMeasurementByIdUcReferenMonthAsync(idUc, dateRef);

                if (consumerUnitMeasurementDetail != null)
                {
                    var measurementDetail = consumerUnitMeasurementDetail.ConsumerUnitMeasurements.FirstOrDefault();

                    if (measurementDetail == null)
                    {
                        returnResponseDto.Error = false;
                        returnResponseDto.StatusCode = 404;
                        returnResponseDto.Data = null;
                        returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                        {
                            ErrorCode = 404,
                            ErrorMessage = "",
                            ErrorMessageDetail = "Medições não localizada para o UC informada."
                        });

                        return returnResponseDto;
                    }

                    var firstDate = DateTimeExtensions.FirstDayOfMonth(measurementDetail.Date).ToString("dd/MM/yyyy");
                    var lastDate = DateTimeExtensions.LastDayOfMonth(measurementDetail.Date).ToString("dd/MM/yyyy");

                    LabelGraphicDetailResponseDto labelGraphicDetailResponseDto = new LabelGraphicDetailResponseDto();
                    labelGraphicDetailResponseDto.month = measurementDetail.Date.ToString("MMM/yyyy").ToUpper();
                    labelGraphicDetailResponseDto.consumption = Math.Round(measurementDetail.Value.Value, 2);
                    labelGraphicDetailResponseDto.period = $"{firstDate} - {lastDate}";
                    labelGraphicDetailResponseDto.totalInvoice = 0;
                    labelGraphicDetailResponseDto.monthSavings = 0;

                    var consumerUnitMeasurementRefDate = await _homeInfoService.GetConsumerUnitMeasurementReferenceDateAsync(dateRef);

                    if (consumerUnitMeasurementRefDate != null && consumerUnitMeasurementRefDate.Count > 0)
                    {
                        List<ConsumerUnitResponseDto>? consumerUnits = new List<ConsumerUnitResponseDto>();

                        foreach (var itemConsumerUnit in consumerUnitMeasurementRefDate)
                        {
                            var refMonthConvert = int.Parse(dateRef.ToString("yyyyMM"));
                            var detailFatureinvoicedetails = await _homeInfoService.GetListDetalhesFaturaCacheByReferenceMonth(itemConsumerUnit.ConsumerUnit.UC, refMonthConvert);

                            decimal invoiceCount = 0;
                            decimal savingsCount = 0;

                            if (detailFatureinvoicedetails != null && detailFatureinvoicedetails.Count > 0)
                            {
                                foreach (var item in detailFatureinvoicedetails)
                                {
                                    var value = item.VlEconomia == null ? "0" : item.VlEconomia.Replace('.', ',');
                                    savingsCount += decimal.Parse(value);

                                    invoiceCount += item.Total == null ? 0 : item.Total.Value;
                                }
                            }

                            var addresseDefault = itemConsumerUnit?.ConsumerUnit?.Customer?.Addresses?.FirstOrDefault();

                            string addresse = "";

                            if (addresseDefault != null)
                            {
                                addresse = $"{addresseDefault?.Logradouro}-{addresseDefault?.Numero}, {addresseDefault?.Bairro}, {addresseDefault?.CEP}, {addresseDefault?.City.Nome} - {addresseDefault?.City?.UF?.Sigla}.";
                            }

                            consumerUnits.Add(new ConsumerUnitResponseDto(){
                                id = itemConsumerUnit.ConsumerUnitId,
                                name = addresse,
                                consumption = itemConsumerUnit?.Value == null ? 0 : itemConsumerUnit.Value.Value,
                                totalInvoice = Math.Round(invoiceCount,2),
                                savings = Math.Round(savingsCount,2)
                            });
                        }

                        consumerUnits.Add(new ConsumerUnitResponseDto()
                        {
                            id = 0,
                            name = "Todas",
                            consumption = Math.Round(consumerUnits.Sum(x => x.consumption),2),
                            totalInvoice = Math.Round(consumerUnits.Sum(x => x.totalInvoice), 2),
                            savings = Math.Round(consumerUnits.Sum(x => x.savings), 2)
                        });

                        labelGraphicDetailResponseDto.totalInvoice = consumerUnits.Where(x => x.id == 0).FirstOrDefault().totalInvoice;
                        labelGraphicDetailResponseDto.monthSavings = consumerUnits.Where(x => x.id == 0).FirstOrDefault().savings;
                        labelGraphicDetailResponseDto.consumerUnit = consumerUnits.OrderBy(x => x.id).ToList();
                    }

                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 200;
                    returnResponseDto.Data = labelGraphicDetailResponseDto;
                }
                else
                {
                    returnResponseDto.Error = false;
                    returnResponseDto.StatusCode = 404;
                    returnResponseDto.Data = null;
                    returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                    {
                        ErrorCode = 404,
                        ErrorMessage = "",
                        ErrorMessageDetail = "Uc não localizada com os parâmetros informado."
                    });
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
