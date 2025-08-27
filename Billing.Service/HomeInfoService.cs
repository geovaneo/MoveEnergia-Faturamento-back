using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Repository;
using MoveEnergia.Billing.Core.Interface.Service;

namespace MoveEnergia.Billing.Service
{
    public class HomeInfoService : IHomeInfoService
    {
        private readonly ILogger<HomeInfoService> _logger;
        private readonly IConsumerUnitRepository _consumerUnitRepository;
        private readonly IConsumerUnitMeasurementRepository _consumerUnitMeasurementRepository;
        private readonly IDetalhesFaturaCacheRepository _dedhesFaturaCacheRepository;

        public HomeInfoService(ILogger<HomeInfoService> logger,
                               IConsumerUnitRepository consumerUnitRepository,
                               IConsumerUnitMeasurementRepository consumerUnitMeasurementRepository,
                               IDetalhesFaturaCacheRepository dedhesFaturaCacheRepository)
        {
            _logger = logger;
            _consumerUnitRepository = consumerUnitRepository;
            _consumerUnitMeasurementRepository = consumerUnitMeasurementRepository;
            _dedhesFaturaCacheRepository = dedhesFaturaCacheRepository;
        }
        public async Task<CurrentInvoiceResponseDto> GetCurrentInvoiceByIdUcAsync(int idUC)
        {
            var result = await _consumerUnitRepository.GetCurrentInvoiceByIdUcAsync(idUC);

            return result;
        }
        public async Task<CurrentInvoiceResponseDto> GetCurrentInvoiceByUcAsync(string UC)
        {
            var result = await _consumerUnitRepository.GetCurrentInvoiceByUcAsync(UC);

            return result;
        }
        public async Task<ConsumerUnit> GetLabelGraphicByIdUcAsync(int idUC)
        {
            var result = await _consumerUnitRepository.GetConsumerUnitMeasurementByIdUcAsync(idUC);

            return result;
        }
        public async Task<ConsumerUnit> GetConsumerUnitMeasurementByIdUcReferenMonthAsync(int idUc, DateTime referenceDate)
        {
            var result = await _consumerUnitRepository.GetConsumerUnitMeasurementByIdUcReferenMonthAsync(idUc, referenceDate);

            return result;
        }
        public async Task<List<ConsumerUnitMeasurement>> GetConsumerUnitMeasurementReferenceDateAsync(DateTime referenceDate)
        {
            var result = await _consumerUnitMeasurementRepository.GetConsumerUnitMeasurementReferenceDateAsync(referenceDate);

            return result;
        }
        public async Task<DetalhesFaturaCache> GetDetalhesFaturaCacheByReferenceMonth(string Uc, int referenceMonth)
        {
            var result = await _dedhesFaturaCacheRepository.GetDetalhesFaturaCacheByReferenceMonth(Uc, referenceMonth);

            return result;
        }

        public async Task<List<DetalhesFaturaCache>> GetListDetalhesFaturaCacheByReferenceMonth(string Uc, int referenceMonth)
        {
            var result = await _dedhesFaturaCacheRepository.GetListDetalhesFaturaCacheByReferenceMonth(Uc, referenceMonth);

            return result;
        }
    }
}


