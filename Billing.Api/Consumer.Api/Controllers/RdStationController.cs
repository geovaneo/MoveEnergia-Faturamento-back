using Microsoft.AspNetCore.Mvc;
using MoveEnergia.Billing.Core.Dto;
using MoveEnergia.Billing.Core.Dto.Request;
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
        [Route("Contacts/{dealId}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCellphoneNumbersAsync(string dealId)
        {
            var retornoDto = await _iRdstationIntegrationAdapter.GetCellphoneNumbersAsync(dealId);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpPost]
        [Route("Fetch/Unity")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FetchUnidadesPageAsync([FromBody] FetchUnidadesPageRequestDto request)
        {
            var retornoDto = await _iRdstationIntegrationAdapter.FetchUnidadesPageAsync(request.page, request.limit, request.next_page);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }

        [HttpPost]
        [Route("Fetch/Unity/{dealId}")]
        [ProducesResponseType(typeof(ReturnResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FetchUnidadesFromRdStationAsync(string dealId, [FromBody]FetchUnidadesPageRequestDto request)
        {
            var retornoDto = await _iRdstationIntegrationAdapter.FetchUnidadesFromRdStationAsync(dealId, request.isStage, request.page, request.limit);
            return StatusCode(retornoDto.StatusCode, retornoDto);
        }
    }
}
