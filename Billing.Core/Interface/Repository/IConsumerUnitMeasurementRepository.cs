using MoveEnergia.Billing.Core.Entity;

namespace MoveEnergia.Billing.Core.Interface.Repository
{
    public interface IConsumerUnitMeasurementRepository : IBaseRepository<ConsumerUnitMeasurement>
    {
        Task<List<ConsumerUnitMeasurement>> GetConsumerUnitMeasurementReferenceDateAsync(DateTime referenceDate);
    }
}
