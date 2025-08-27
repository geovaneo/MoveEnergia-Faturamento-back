using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Request;

namespace MoveEnergia.Billing.Core.Interface.Adapter
{
    public interface IDistributorAdapter
    {
        Task<ReturnResponseDto> GetByIdAsync(int Id);
        Task<ReturnResponseDto> GetAllAsync();
        Task<ReturnResponseDto> CreateAsync(DistributorRequestDto request);
        Task<ReturnResponseDto> UpDateAsync(DistributorRequestDto request);
        Task<ReturnResponseDto> DeleteAsync(int id);
    }
}
