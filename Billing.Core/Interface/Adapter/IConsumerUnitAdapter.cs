using MoveEnergia.Billing.Core.Dto;

namespace MoveEnergia.Billing.Core.Interface.Adapter
{
    public interface IConsumerUnitAdapter
    {
        Task<ReturnResponseDto> GetConsumerUnitByIdUser(int idUser);
        Task<ReturnResponseDto> GetConsumerUnitByAdressIdUserAsync(long idUser);
    }
}
