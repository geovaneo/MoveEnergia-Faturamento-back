using MoveEnergia.Billing.Core.Dto.General;

namespace MoveEnergia.Billing.Core.Interface.Service
{
    public interface IPdfExtractorByDistr
    {
        Task<FaturaPdfData> ExtractInfo(int Id);
    }
}
