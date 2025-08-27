using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Request;

namespace MoveEnergia.Billing.Core.Interface.Adapter
{
    public interface IAuthenticationAdapter
    {
        Task<ReturnResponseDto> SetAuthenticateUser(AuthenticateUserRequestDto authenticateUser);
    }
}
