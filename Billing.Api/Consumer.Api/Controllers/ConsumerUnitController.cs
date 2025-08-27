using Microsoft.AspNetCore.Mvc;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Interface.Adapter;
using System.Text;

namespace MoveEnergia.Billing.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ConsumerUnitController : ControllerBase
    {     
        private readonly ILogger<ConsumerUnitController> _logger;
        private readonly IConsumerUnitAdapter _consumerUnitAdapter;

        public ConsumerUnitController(ILogger<ConsumerUnitController> logger,
                                      IConsumerUnitAdapter consumerUnitAdapter)
        {
            _logger = logger;
            _consumerUnitAdapter = consumerUnitAdapter;
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

        [HttpGet]
        [Route("User/{idUser}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(int idUser)
        {
            var retornoDto = await _consumerUnitAdapter.GetConsumerUnitByIdUser(idUser);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpGet]
        [Route("Adress/{idUser}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAdressByIdAsync(int idUser)
        {
            var retornoDto = await _consumerUnitAdapter.GetConsumerUnitByAdressIdUserAsync(idUser);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }


    }
}
