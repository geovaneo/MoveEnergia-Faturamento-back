using Microsoft.AspNetCore.Mvc;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.RdStation.Adapter.Interface.Adapter;
using System.Text;

namespace MoveEnergia.Billing.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RdStationController : ControllerBase
    {
        private readonly ILogger<RdStationController> _logger;
        private readonly IRdstationIntegrationAdapter _iRdstationIntegrationAdapter;

        public RdStationController(ILogger<RdStationController> logger, IRdstationIntegrationAdapter iRdstationIntegrationAdapter)
        {
            _logger = logger;
            _iRdstationIntegrationAdapter = iRdstationIntegrationAdapter;
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
        [Route("{id}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCellphoneNumbersAsync(int id)
        {
            await _iRdstationIntegrationAdapter.GetCellphoneNumbersAsync("", "");
            //return StatusCode(retornoDto.StatusCode, retornoDto);
            return Ok();
        }
    }
}
