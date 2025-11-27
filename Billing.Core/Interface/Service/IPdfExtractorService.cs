using MoveEnergia.Billing.Core.Dto.General;

namespace MoveEnergia.Billing.Core.Interface.Service
{
    public interface IPdfExtractorService
    {
        Task<FaturaPdfData> StartProcess(int Id);
    }
}
