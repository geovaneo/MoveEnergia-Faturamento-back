using MoveEnergia.Billing.Core.Dto.Response;
using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Repository
{
    public interface IConsumerUnitRepository : IBaseRepository<ConsumerUnit>
    {
        Task<List<ConsumerUnit>> GetByIdUserAsync(long idUser);
        Task<CurrentInvoiceResponseDto> GetCurrentInvoiceByUcAsync(string UC);
        Task<CurrentInvoiceResponseDto> GetCurrentInvoiceByIdUcAsync(int idUC);
        Task<ConsumerUnit> GetConsumerUnitMeasurementByIdUcAsync(int idUc);
        Task<ConsumerUnit> GetConsumerUnitMeasurementByIdUcReferenMonthAsync(int idUc, DateTime referenceDate);
        Task<ConsumerUnit> GetByUCAsync(string UC);
    }
}
