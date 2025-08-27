using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.Billing.Core.Entity;
using MoveEnergia.Billing.Core.Interface.Adapter;
using System.Text;

namespace MoveEnergia.Billing.Adapter
{
    public class AuthenticationAdapter : IAuthenticationAdapter
    {

        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationAdapter> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationAdapter(ILogger<AuthenticationAdapter> logger,
                                     IMapper mapper,
                                     UserManager<User> userManager,
                                     SignInManager<User> signInManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ReturnResponseDto> SetAuthenticateUser(AuthenticateUserRequestDto authenticateUser)
        {
            ReturnResponseDto returnResponseDto = new ReturnResponseDto();
            returnResponseDto.Erros = new List<ReturnResponseErrorDto>();

            try
            {

                byte[] dataUserName = Convert.FromBase64String(authenticateUser.UserName);
                string userName = Encoding.UTF8.GetString(dataUserName);

                byte[] dataPassWord = Convert.FromBase64String(authenticateUser.PassWord);
                string userPassWord = Encoding.UTF8.GetString(dataPassWord);

                var userAuthenticate = await _userManager.FindByNameAsync(userName);

                if (userAuthenticate == null || !userAuthenticate.IsActive)
                {
                    returnResponseDto.Error = true;
                    returnResponseDto.StatusCode = 400;
                    returnResponseDto.Data = null;
                    returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                    {
                        ErrorCode = 400,
                        ErrorMessage = "Usuário inválido ou inativo."
                    });
                }
                else
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(userAuthenticate, userPassWord, false);

                    if (result.Succeeded)
                    {
                        returnResponseDto.Error = false;
                        returnResponseDto.StatusCode = 200;
                        returnResponseDto.Data = userAuthenticate; 
                    }
                    else
                    {
                        returnResponseDto.Error = true;
                        returnResponseDto.StatusCode = 400;
                        returnResponseDto.Data = null;
                        returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                        {
                            ErrorCode = 400,
                            ErrorMessage = "Senha inválida."
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                returnResponseDto.Error = true;
                returnResponseDto.StatusCode = 500;
                returnResponseDto.Data = null;
                returnResponseDto.Erros?.Add(new ReturnResponseErrorDto()
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
            }

            return returnResponseDto;

        }
    }
}
