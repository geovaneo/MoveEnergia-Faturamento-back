using MoveEnergia.Billing.Core.Dto;

namespace MoveEnergia.Billing.Core.Interface.Adapter
{
    public interface IAdapterBase
    {
        Task<ReturnResponseDto> SetReturnResponse<T>(object resultData);
        Task<ReturnResponseDto> SetReturnResponseException(Exception exception);
        Task<ReturnResponseDto> SetReturnResponseErros(List<string> erros);
    }
}
