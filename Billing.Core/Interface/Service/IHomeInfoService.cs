using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Service
{
    public interface IHomeInfoService
    {
        Task<CurrentInvoiceResponseDto> GetCurrentInvoiceByIdUcAsync(int idUC);
        Task<CurrentInvoiceResponseDto> GetCurrentInvoiceByUcAsync(string UC);
        Task<ConsumerUnit> GetLabelGraphicByIdUcAsync(int idUC);
        Task<ConsumerUnit> GetConsumerUnitMeasurementByIdUcReferenMonthAsync(int idUc, DateTime referenceDate);
        Task<List<ConsumerUnitMeasurement>> GetConsumerUnitMeasurementReferenceDateAsync(DateTime referenceDate);
        Task<DetalhesFaturaCache> GetDetalhesFaturaCacheByReferenceMonth(string Uc, int referenceMonth);
        Task<List<DetalhesFaturaCache>> GetListDetalhesFaturaCacheByReferenceMonth(string Uc, int referenceMonth);
    }
}
