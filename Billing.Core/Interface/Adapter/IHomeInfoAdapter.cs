using MoveEnergia.Billing.Core.Dto;

namespace MoveEnergia.Billing.Core.Interface.Adapter
{
    public interface IHomeInfoAdapter
    {
        Task<ReturnResponseDto> GetHomeInfoByIdUCAsync(int idUC);
        Task<ReturnResponseDto> GetHomeInfoByUCAsync(string UC);
        Task<ReturnResponseDto> GetLabelGraphicByIdUCAsync(int idUC);
        Task<ReturnResponseDto> GetLabelGraphicDetailByIdUCAsync(int idUc, int month, int year);
    }
}
