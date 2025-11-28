using MoveEnergia.Billing.Core.Dto.General;
using UglyToad.PdfPig;

namespace MoveEnergia.Billing.Core.Interface.Service
{
    public interface IPdfExtractorByDistr
    {
        Task<FaturaPdfData> ExtractInfo(PdfDocument document);
    }
}
