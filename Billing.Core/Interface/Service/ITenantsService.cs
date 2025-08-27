using MoveEnergia.Billing.Core.Dto;

namespace MoveEnergia.Billing.Core.Interface.Service
{
    public interface ITenantsService
    {
        Task<ReturnResponseDto> GetByIdAsync(int id);
    }
}
