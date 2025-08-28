using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Entity;
using System.Threading.Tasks;

namespace MoveEnergia.Billing.Core.Interface.Service
{
    public interface IDistributorService
    {
        Task<Distributor> GetByIdAsync(int Id);
        Task<List<Distributor>> GetAllAsync();
        Task<ReturnResponseDto> CreateAsync(Distributor distributor);
        Task<ReturnResponseDto> UpdateAsync(Distributor distributor);
        Task DeleteAsync(int Id);      
    }
}
