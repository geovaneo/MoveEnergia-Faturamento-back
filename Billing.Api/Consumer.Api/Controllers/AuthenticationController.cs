using Microsoft.AspNetCore.Mvc;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Request;
using MoveEnergia.Billing.Core.Interface.Adapter;
using System.Text;

namespace MoveEnergia.Billing.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IAuthenticationAdapter _authenticationAdapter;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationAdapter authenticationAdapter)
        {
            _logger = logger;
            _authenticationAdapter = authenticationAdapter;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]

        public IActionResult HealthCheck()
        {
            StringBuilder informacoes = new StringBuilder();
            informacoes.AppendLine($"API MoveEnergia Consumer = MoveEnergia.Billing.Api");
            informacoes.AppendLine($"Situação = Saudável");

            return Ok(informacoes.ToString());
        }

        [HttpPost]
        [Route("AuthenticateUser")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticateUserRequestDto authenticateUser)
        {
            var retornoDto = await _authenticationAdapter.SetAuthenticateUser(authenticateUser);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }


    }
}
